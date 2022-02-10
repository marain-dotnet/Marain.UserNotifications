// <copyright file="UpdateNotificationReadStatusActivity.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Activities
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.UserNotifications.Management.Host.Helpers;
    using Marain.UserNotifications.Management.Host.OpenApi;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Updates the delivery status of a notification.
    /// </summary>
    public class UpdateNotificationReadStatusActivity
    {
        private readonly ITenantProvider tenantProvider;
        private readonly ITenantedUserNotificationStoreFactory notificationStoreFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNotificationReadStatusActivity"/> class.
        /// </summary>
        /// <param name="tenantProvider">The tenant provider.</param>
        /// <param name="notificationStoreFactory">The factory for the notification store.</param>
        public UpdateNotificationReadStatusActivity(
            ITenantProvider tenantProvider,
            ITenantedUserNotificationStoreFactory notificationStoreFactory)
        {
            this.tenantProvider = tenantProvider
                ?? throw new ArgumentNullException(nameof(tenantProvider));
            this.notificationStoreFactory = notificationStoreFactory
                ?? throw new ArgumentNullException(nameof(notificationStoreFactory));
        }

        /// <summary>
        /// Executes the activity.
        /// </summary>
        /// <param name="context">The activity context.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName(nameof(UpdateNotificationReadStatusActivity))]
        public async Task ExecuteAsync(
            [ActivityTrigger] IDurableActivityContext context,
            ILogger logger)
        {
            TenantedFunctionData<BatchReadStatusUpdateRequestItem> request = context.GetInput<TenantedFunctionData<BatchReadStatusUpdateRequestItem>>();

            ITenant tenant = await this.tenantProvider.GetTenantAsync(request.TenantId).ConfigureAwait(false);

            logger.LogInformation(
                "Executing UpdateNotificationReadStatusActivity for notification with Id {notificationId}",
                request.Payload.NotificationId);

            IUserNotificationStore store = await this.notificationStoreFactory.GetUserNotificationStoreForTenantAsync(tenant).ConfigureAwait(false);

            UserNotification originalNotification = await store.GetByIdAsync(request.Payload.NotificationId).ConfigureAwait(false);

            UserNotification modifiedNotification = originalNotification.WithChannelReadStatus(
                request.Payload.DeliveryChannelId,
                request.Payload.NewStatus,
                request.Payload.UpdateTimestamp);

            await store.StoreAsync(modifiedNotification).ConfigureAwait(false);
        }
    }
}