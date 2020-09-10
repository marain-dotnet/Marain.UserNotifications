// <copyright file="GetNotificationsForUserService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.ApiDeliveryChannel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.Services.Tenancy;
    using Marain.UserNotifications.OpenApi.ApiDeliveryChannel.Mappers;
    using Menes;
    using Menes.Exceptions;
    using Menes.Hal;

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
        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly UserNotificationsMapper userNotificationsMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNotificationsForUserService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="userNotificationStoreFactory">The user notification store factory.</param>
        /// <param name="userNotificationsMapper">The user notifications mapper.</param>
        public GetNotificationsForUserService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedUserNotificationStoreFactory userNotificationStoreFactory,
            UserNotificationsMapper userNotificationsMapper)
        {
            this.marainServicesTenancy = marainServicesTenancy
                ?? throw new ArgumentNullException(nameof(marainServicesTenancy));
            this.userNotificationStoreFactory = userNotificationStoreFactory
                ?? throw new ArgumentNullException(nameof(userNotificationStoreFactory));
            this.userNotificationsMapper = userNotificationsMapper
                ?? throw new ArgumentNullException(nameof(userNotificationsMapper));
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

            GetNotificationsResult results;

            try
            {
                results = string.IsNullOrEmpty(continuationToken)
                                    ? await userNotificationStore.GetAsync(userId, sinceNotificationId, maxItems.Value).ConfigureAwait(false)
                                    : await userNotificationStore.GetAsync(userId, continuationToken).ConfigureAwait(false);
            }
            catch (ArgumentException) when (!string.IsNullOrEmpty(continuationToken))
            {
                // The most likely reason for this is that the user Id in the continuation token doesn't match that in
                // the path - which makes this a bad request.
                throw new OpenApiBadRequestException();
            }

            HalDocument result = await this.userNotificationsMapper.MapAsync(
                results,
                new UserNotificationsMappingContext(context, userId, sinceNotificationId, maxItems.Value, continuationToken)).ConfigureAwait(false);

            return this.OkResult(result);
        }
    }
}
