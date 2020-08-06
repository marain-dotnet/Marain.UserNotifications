// <copyright file="CreateNotificationActivity.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Activities
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.UserNotifications.Management.Host.Helpers;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Creates a new notification for the given user.
    /// </summary>
    public class CreateNotificationActivity
    {
        private readonly ITenantProvider tenantProvider;
        private readonly ITenantedNotificationStoreFactory notificationStoreFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNotificationActivity"/> class.
        /// </summary>
        /// <param name="tenantProvider">The tenant provider.</param>
        /// <param name="notificationStoreFactory">The factory for the notification store.</param>
        public CreateNotificationActivity(
            ITenantProvider tenantProvider,
            ITenantedNotificationStoreFactory notificationStoreFactory)
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
        [FunctionName(nameof(CreateNotificationActivity))]
        public async Task ExecuteAsync(
            [ActivityTrigger] IDurableActivityContext context,
            ILogger logger)
        {
            TenantedFunctionData<Notification> request = context.GetInput<TenantedFunctionData<Notification>>();

            ITenant tenant = await this.tenantProvider.GetTenantAsync(request.TenantId).ConfigureAwait(false);

            logger.LogInformation(
                "Executing CreateNotificationActivity for notification of type {notificationType} for user {userId}",
                request.Payload.NotificationType,
                request.Payload.UserId);

            INotificationStore store = await this.notificationStoreFactory.GetNotificationStoreForTenantAsync(tenant).ConfigureAwait(false);

            await store.StoreAsync(request.Payload).ConfigureAwait(false);
        }
    }
}
