// <copyright file="UserNotificationsOpenApiServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using Marain.UserNotifications.OpenApi.ApiDeliveryChannel;
    using Marain.UserNotifications.OpenApi.Management;
    using Menes;

    /// <summary>
    /// Extension methods for configuring DI for the OpenApi service implementations.
    /// </summary>
    public static class UserNotificationsOpenApiServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the OpenApiServices for the management API.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection, to enable chaining.</returns>
        public static IServiceCollection AddManagementOpenApiServices(
            this IServiceCollection services)
        {
            services.AddSingleton<IOpenApiService, CreateNotificationsService>();
            services.AddSingleton<IOpenApiService, GetNotificationsService>();
            services.AddSingleton<IOpenApiService, GetNotificationStatusService>();

            return services;
        }

        /// <summary>
        /// Adds the OpenApiServices for the API delivery channel.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection, to enable chaining.</returns>
        public static IServiceCollection AddApiDeliveryChannelOpenApiServices(
            this IServiceCollection services)
        {
            services.AddSingleton<IOpenApiService, GetNotificationsForUserService>();
            services.AddSingleton<IOpenApiService, UpdateUserNotificationReadStatusService>();

            return services;
        }
    }
}
