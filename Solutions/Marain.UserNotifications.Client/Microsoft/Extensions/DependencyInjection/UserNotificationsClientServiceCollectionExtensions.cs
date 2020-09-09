// <copyright file="UserNotificationsClientServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Text.Json;
    using Marain.UserNotifications.Client.ApiDeliveryChannel;
    using Marain.UserNotifications.Client.Management;

    /// <summary>
    /// Extension methods to add the service client to the DI container.
    /// </summary>
    public static class UserNotificationsClientServiceCollectionExtensions
    {
        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

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
            services.AddSingleton<IUserNotificationsManagementClient>(sp =>
            {
                UserNotificationsManagementClientConfiguration config = configurationCallback(sp);
                return new UserNotificationsManagementClient(config.BaseUrl, serializerOptions);
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
            services.AddSingleton<IUserNotificationsApiDeliveryChannelClient>(sp =>
            {
                UserNotificationsApiDeliveryChannelClientConfiguration config = configurationCallback(sp);
                return new UserNotificationsApiDeliveryChannelClient(config.BaseUrl, serializerOptions);
            });

            return services;
        }
    }
}
