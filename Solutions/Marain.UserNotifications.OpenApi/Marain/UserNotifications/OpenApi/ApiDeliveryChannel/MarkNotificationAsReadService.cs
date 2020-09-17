// <copyright file="MarkNotificationAsReadService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.ApiDeliveryChannel
{
    using System;
    using System.Threading.Tasks;
    using Marain.Services.Tenancy;
    using Marain.UserNotifications.Client;
    using Marain.UserNotifications.Client.Management;
    using Marain.UserNotifications.Client.Management.Requests;
    using Menes;

    /// <summary>
    /// Implements the mark notification as read for the API delivery channel.
    /// </summary>
    public class MarkNotificationAsReadService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string MarkNotificationReadOperationId = "markNotificationRead";

        private readonly IUserNotificationsManagementClient managementApiClient;
        private readonly IMarainServicesTenancy marainServicesTenancy;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkNotificationAsReadService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="managementApiClient">The client for the management API.</param>
        public MarkNotificationAsReadService(
            IUserNotificationsManagementClient managementApiClient,
            IMarainServicesTenancy marainServicesTenancy)
        {
            this.marainServicesTenancy = marainServicesTenancy
                ?? throw new ArgumentNullException(nameof(marainServicesTenancy));
            this.managementApiClient = managementApiClient
                ?? throw new ArgumentNullException(nameof(managementApiClient));
        }

        /// <summary>
        /// Marks the specified notification as read for the API delivery channel.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="notificationId">The Id of the notification to mark as read.</param>
        /// <returns>Confirmation that the update request has been accepted.</returns>
        [OperationId(MarkNotificationReadOperationId)]
        public async Task<OpenApiResult> MarkNotificationReadAsync(
            IOpenApiContext context,
            string notificationId)
        {
            // We're going to forward this on to the management API, which already implements all of the long running
            // operation semantics we would like here. At present, this is via the "batch update" endpoint - if at
            // some point in the future we add the ability to update a single notification status, we should switch
            // over to that in this code.

            // We can guarantee tenant Id is available because it's part of the Uri.
            // We don't actually need the tenant, but this method has the benefit of validating that the requesting
            // tenant is valid and is enrolled for this service.
            _ = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            BatchReadStatusUpdateRequestItem[] body = new[]
            {
                    new BatchReadStatusUpdateRequestItem
                    {
                        DeliveryChannelId = Constants.ApiDeliveryChannelId,
                        NewStatus = UpdateNotificationReadStatusRequestNewStatus.Read,
                        NotificationId = notificationId,
                        UpdateTimestamp = DateTimeOffset.UtcNow,
                    },
            };

            ApiResponse response = await this.managementApiClient.BatchReadStatusUpdateAsync(
                context.CurrentTenantId,
                body).ConfigureAwait(false);

            return this.AcceptedResult(response.Headers["Location"]);
        }
    }
}
