// <copyright file="AzureTableNotificationStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureTable
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of <see cref="INotificationStore"/> over Azure Table storage.
    /// </summary>
    public class AzureTableNotificationStore : INotificationStore
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableNotificationStore"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public AzureTableNotificationStore(
            ILogger logger)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public Task<Notification> StoreAsync(Notification notification)
        {
            this.logger.LogDebug(
                "Storing notification for user ",
                notification.UserId);

            return Task.FromResult(notification);
        }
    }
}
