// <copyright file="GetNotificationService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.ApiDeliveryChannel
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.Services.Tenancy;
    using Marain.UserNotifications.OpenApi.ApiDeliveryChannel.Mappers;
    using Menes;
    using Menes.Hal;

    /// <summary>
    /// Implements the user notifications retrieval endpoint for the API delivery channel.
    /// </summary>
    public class GetNotificationService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string GetNotificationOperationId = "getNotification";

        private readonly ITenantedUserNotificationStoreFactory userNotificationStoreFactory;
        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly UserNotificationMapper userNotificationMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNotificationService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="userNotificationStoreFactory">The user notification store factory.</param>
        /// <param name="userNotificationMapper">The mapper for the result notification.</param>
        public GetNotificationService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedUserNotificationStoreFactory userNotificationStoreFactory,
            UserNotificationMapper userNotificationMapper)
        {
            this.marainServicesTenancy = marainServicesTenancy
                ?? throw new ArgumentNullException(nameof(marainServicesTenancy));
            this.userNotificationStoreFactory = userNotificationStoreFactory
                ?? throw new ArgumentNullException(nameof(userNotificationStoreFactory));
            this.userNotificationMapper = userNotificationMapper
                ?? throw new ArgumentNullException(nameof(userNotificationMapper));
        }

        /// <summary>
        /// Retrieves notifications for a user.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="notificationId">The Id of the notification to retrieve.</param>
        /// <returns>The notifications, as an OpenApiResult.</returns>
        [OperationId(GetNotificationOperationId)]
        public async Task<OpenApiResult> GetNotificationAsync(
            IOpenApiContext context,
            string notificationId)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            IUserNotificationStore userNotificationStore =
                await this.userNotificationStoreFactory.GetUserNotificationStoreForTenantAsync(tenant).ConfigureAwait(false);

            UserNotification notifications =
                await userNotificationStore.GetByIdAsync(notificationId).ConfigureAwait(false);

            HalDocument response = await this.userNotificationMapper.MapAsync(notifications, context).ConfigureAwait(false);

            return this.OkResult(response, "application/json");
        }
    }
}
