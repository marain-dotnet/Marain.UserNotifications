// <copyright file="GetNotificationsForUserService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.ApiDeliveryChannel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.Monitoring.Instrumentation;
    using Corvus.Tenancy;
    using Marain.Services.Tenancy;
    using Marain.UserNotifications.Client.Management;
    using Marain.UserNotifications.Client.Management.Requests;
    using Marain.UserNotifications.OpenApi.ApiDeliveryChannel.Mappers;
    using Menes;
    using Menes.Exceptions;
    using Menes.Hal;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implements the user notifications retrieval endpoint for the API delivery channel.
    /// </summary>
    public class GetNotificationsForUserService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string GetNotificationsForUserOperationId = "getNotificationsForUser";

        private readonly ITenantedUserNotificationStoreFactory userNotificationStoreFactory;
        private readonly IUserNotificationsManagementClient managementApiClient;
        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly UserNotificationsMapper userNotificationsMapper;
        private readonly ILogger<GetNotificationsForUserService> logger;
        private readonly IExceptionsInstrumentation<GetNotificationsForUserService> exceptionsInstrumentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNotificationsForUserService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="userNotificationStoreFactory">The user notification store factory.</param>
        /// <param name="userNotificationsMapper">The user notifications mapper.</param>
        /// <param name="managementApiClient">The client for the management API.</param>
        /// <param name="exceptionsInstrumentation">The <see cref="IExceptionsInstrumentation{T}"/> for this class.</param>
        /// <param name="logger">The logger.</param>
        public GetNotificationsForUserService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedUserNotificationStoreFactory userNotificationStoreFactory,
            UserNotificationsMapper userNotificationsMapper,
            IUserNotificationsManagementClient managementApiClient,
            IExceptionsInstrumentation<GetNotificationsForUserService> exceptionsInstrumentation,
            ILogger<GetNotificationsForUserService> logger)
        {
            this.marainServicesTenancy = marainServicesTenancy
                ?? throw new ArgumentNullException(nameof(marainServicesTenancy));
            this.userNotificationStoreFactory = userNotificationStoreFactory
                ?? throw new ArgumentNullException(nameof(userNotificationStoreFactory));
            this.userNotificationsMapper = userNotificationsMapper
                ?? throw new ArgumentNullException(nameof(userNotificationsMapper));
            this.managementApiClient = managementApiClient
                ?? throw new ArgumentNullException(nameof(managementApiClient));
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.exceptionsInstrumentation = exceptionsInstrumentation
                ?? throw new ArgumentNullException(nameof(exceptionsInstrumentation));
        }

        /// <summary>
        /// Retrieves notifications for a user.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="userId">The Id of the user to retrieve notifications for.</param>
        /// <param name="sinceNotificationId">If this is supplied, only notifications newer than the specified Id will be returned.</param>
        /// <param name="maxItems">The maximum number of items to return.</param>
        /// <param name="continuationToken">A continuation token returned from a previous request.</param>
        /// <returns>The notifications, as an OpenApiResult.</returns>
        [OperationId(GetNotificationsForUserOperationId)]
        public async Task<OpenApiResult> GetNotificationsForUserAsync(
            IOpenApiContext context,
            string userId,
            string? sinceNotificationId,
            int? maxItems,
            string? continuationToken)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            IUserNotificationStore userNotificationStore =
                await this.userNotificationStoreFactory.GetUserNotificationStoreForTenantAsync(tenant).ConfigureAwait(false);

            maxItems ??= 50;

            GetNotificationsResult results = await this.GetNotificationsAsync(
                userId,
                sinceNotificationId,
                maxItems.Value,
                continuationToken,
                userNotificationStore).ConfigureAwait(false);

            await this.EnsureAllNotificationsMarkedAsDelivered(context, results).ConfigureAwait(false);

            HalDocument result = await this.userNotificationsMapper.MapAsync(
                results,
                new UserNotificationsMappingContext(context, userId, sinceNotificationId, maxItems.Value, continuationToken)).ConfigureAwait(false);

            return this.OkResult(result);
        }

        private async Task EnsureAllNotificationsMarkedAsDelivered(IOpenApiContext context, GetNotificationsResult results)
        {
            // See if there are any notifications we need to mark as delivered. Whilst we want this to happen, we're
            // not going to fail the operation if something goes wrong, hence the catch-all exception handler.
            try
            {
                UserNotification[] notificationsToMarkAsDelivered = results.Results.Where(
                    x => x.GetDeliveryStatusForChannel(Constants.ApiDeliveryChannelId) != UserNotificationDeliveryStatus.Delivered).ToArray();

                if (notificationsToMarkAsDelivered.Length > 0)
                {
                    this.logger.LogDebug($"Updating notification state to 'Delivered' for {notificationsToMarkAsDelivered.Length} notifications.");

                    DateTimeOffset timestamp = DateTimeOffset.UtcNow;
                    IEnumerable<BatchDeliveryStatusUpdateRequestItem> deliveryStatusUpdateBatch = notificationsToMarkAsDelivered.Select(
                        n =>
                        new BatchDeliveryStatusUpdateRequestItem
                        {
                            NotificationId = n.Id,
                            NewStatus = UpdateNotificationDeliveryStatusRequestNewStatus.Delivered,
                            UpdateTimestamp = timestamp,
                            DeliveryChannelId = Constants.ApiDeliveryChannelId,
                        });

                    // Although this call returns the location for the long running op it kicks off, we don't really
                    // care. We're going to return the notifications as "delivered" anyway (because the fact that
                    // we're returning them means they are delivered). So we'll send this request and hope that it
                    // succeeds (safe in the knowledge that if it doesn't, we're logging the failure.
                    await this.managementApiClient.BatchDeliveryStatusUpdateAsync(
                        context.CurrentTenantId,
                        deliveryStatusUpdateBatch,
                        CancellationToken.None).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "Unable to update notification delivery state");
                this.exceptionsInstrumentation.ReportException(ex);
            }
        }

        private async Task<GetNotificationsResult> GetNotificationsAsync(
            string userId,
            string? sinceNotificationId,
            int maxItems,
            string? continuationToken,
            IUserNotificationStore userNotificationStore)
        {
            GetNotificationsResult results;

            try
            {
                this.logger.LogDebug($"Retrieving notifications for user {userId}");
                results = string.IsNullOrEmpty(continuationToken)
                                    ? await userNotificationStore.GetAsync(userId, sinceNotificationId, maxItems).ConfigureAwait(false)
                                    : await userNotificationStore.GetAsync(userId, continuationToken).ConfigureAwait(false);
            }
            catch (ArgumentException) when (!string.IsNullOrEmpty(continuationToken))
            {
                // The most likely reason for this is that the user Id in the continuation token doesn't match that in
                // the path - which makes this a bad request.
                throw new OpenApiBadRequestException("The combination of arguments supplied is invalid.", "The arguments supplied to the method were inconsistent. This likely means that the continuation token supplied was either invalid, or contained a different user Id to that specified in the path.");
            }

            return results;
        }
    }
}
