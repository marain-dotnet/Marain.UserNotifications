// <copyright file="UserNotificationsClientServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using Marain.UserNotifications.Client.ApiDeliveryChannel;
    using Marain.UserNotifications.Client.Management;

    /// <summary>
    /// Extension methods to add the service client to the DI container.
    /// </summary>
    public static class UserNotificationsClientServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the management client to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configurationCallback">A callback to retrieve configuration for the client.</param>
        /// <returns>The service collection, for chaining.</returns>
        public static IServiceCollection AddUserNotificationsManagementClient(
            this IServiceCollection services,
            Func<IServiceProvider, UserNotificationsManagementClientConfiguration> configurationCallback)
        {
            services.AddHttpClient<IUserNotificationsManagementClient, UserNotificationsManagementClient>((sp, client) =>
            {
                UserNotificationsManagementClientConfiguration config = configurationCallback(sp);
                client.BaseAddress = new Uri(config.BaseUrl);
            });

            return services;
        }

        /// <summary>
        /// Adds the management client to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configurationCallback">A callback to retrieve configuration for the client.</param>
        /// <returns>The service collection, for chaining.</returns>
        public static IServiceCollection AddUserNotificationsApiDeliveryChannelClient(
            this IServiceCollection services,
            Func<IServiceProvider, UserNotificationsApiDeliveryChannelClientConfiguration> configurationCallback)
        {
            services.AddHttpClient<IUserNotificationsApiDeliveryChannelClient, UserNotificationsApiDeliveryChannelClient>((sp, client) =>
            {
                UserNotificationsApiDeliveryChannelClientConfiguration config = configurationCallback(sp);
                client.BaseAddress = new Uri(config.BaseUrl);
            });

            return services;
        }
    }
}
