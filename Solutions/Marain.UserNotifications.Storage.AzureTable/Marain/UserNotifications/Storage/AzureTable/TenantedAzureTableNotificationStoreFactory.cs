// <copyright file="TenantedAzureTableNotificationStoreFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureTable
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of <see cref="ITenantedNotificationStoreFactory"/> for <see cref="AzureTableNotificationStore"/>.
    /// </summary>
    public class TenantedAzureTableNotificationStoreFactory : ITenantedNotificationStoreFactory
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantedAzureTableNotificationStoreFactory"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TenantedAzureTableNotificationStoreFactory(
            ILogger<TenantedAzureTableNotificationStoreFactory> logger)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public Task<INotificationStore> GetNotificationStoreForTenantAsync(ITenant tenant)
        {
            return Task.FromResult<INotificationStore>(new AzureTableNotificationStore(this.logger));
        }
    }
}
