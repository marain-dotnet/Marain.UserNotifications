// <copyright file="ServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Linq;
    using Corvus.Storage.Azure.TableStorage.Tenancy;
    using Marain.NotificationTemplates;
    using Marain.UserNotifications;
    using Marain.UserNotifications.Storage.AzureStorage;

    /// <summary>
    /// Service collection extensions to add the Azure implementation of user notification store.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Azure table-based implementation of <see cref="ITenantedUserNotificationStoreFactory"/> to the service container.
        /// </summary>
        /// <param name="services">The collection.</param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddTenantedAzureTableUserNotificationStore(
            this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType is ITenantedUserNotificationStoreFactory))
            {
                return services;
            }

            services.AddAzureTableClientSourceFromDynamicConfiguration();
            services.AddAzureTableV2ToV3Transition();
            services.AddSingleton<ITenantedUserNotificationStoreFactory, TenantedAzureTableUserNotificationStoreFactory>();

            return services;
        }

        /// <summary>
        /// Adds Azure blob-based implementation of <see cref="ITenantedNotificationTemplateStoreFactory"/> to the service container.
        /// </summary>
        /// <param name="services">The collection.</param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddTenantedAzureBlobTemplateStore(
            this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType is ITenantedNotificationTemplateStoreFactory))
            {
                return services;
            }

            services.AddAzureBlobStorageClientSourceFromDynamicConfiguration();
            services.AddBlobContainerV2ToV3Transition();
            services.AddSingleton<ITenantedNotificationTemplateStoreFactory, TenantedAzureBlobTemplateStoreFactory>();

            return services;
        }
    }
}