// <copyright file="ServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Linq;
    using Corvus.Azure.Storage.Tenancy;
    using Marain.UserNotifications;
    using Marain.UserNotifications.Storage.AzureTable;

    /// <summary>
    /// Service collection extensions to add the Azure implementation of user notification store.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Azure table-based implementation of <see cref="ITenantedUserNotificationStoreFactory"/> to the service container.
        /// </summary>
        /// <param name="services">The collection.</param>
        /// <param name="getCloudTableFactoryOptions">A callback function that returns the <see cref="TenantCloudTableFactoryOptions"/>.</param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddTenantedAzureTableUserNotificationStore(
            this IServiceCollection services,
            Func<IServiceProvider, TenantCloudTableFactoryOptions> getCloudTableFactoryOptions)
        {
            if (services.Any(s => s.ServiceType is ITenantedUserNotificationStoreFactory))
            {
                return services;
            }

            services.AddTenantCloudTableFactory(getCloudTableFactoryOptions);
            services.AddSingleton<ITenantedUserNotificationStoreFactory, TenantedAzureTableUserNotificationStoreFactory>();

            return services;
        }
    }
}
