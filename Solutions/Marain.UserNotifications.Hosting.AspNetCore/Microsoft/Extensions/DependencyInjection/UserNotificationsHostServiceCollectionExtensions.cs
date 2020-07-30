// <copyright file="UserNotificationsHostServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using Marain.UserNotifications.OpenApi.Management;
    using Menes;

    /// <summary>
    /// Configures the user notifications API hosts.
    /// </summary>
    public static class UserNotificationsHostServiceCollectionExtensions
    {
        /// <summary>
        /// Configures services neededed to provide the user notifications management API.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection, to enable chaining.</returns>
        public static IServiceCollection AddTenantedUserNotificationsManagementApi(
            this IServiceCollection services)
        {
            services.AddManagementOpenApiServices();

            services.AddOpenApiHttpRequestHosting<SimpleOpenApiContext>(
                hostConfig =>
                {
                    hostConfig.Documents.RegisterOpenApiServiceWithEmbeddedDefinition(
                        typeof(CreateNotificationsService).Assembly,
                        "Marain.UserNotifications.OpenApi.ManagementService.yaml");

                    hostConfig.Documents.AddSwaggerEndpoint();
                });

            return services;
        }

        /// <summary>
        /// Configures services neededed to provide the user notifications API delivery channel.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection, to enable chaining.</returns>
        public static IServiceCollection AddTenantedUserNotificationsApiDeliveryChannel(
            this IServiceCollection services)
        {
            services.AddApiDeliveryChannelOpenApiServices();

            services.AddOpenApiHttpRequestHosting<SimpleOpenApiContext>(
                hostConfig =>
                {
                    hostConfig.Documents.RegisterOpenApiServiceWithEmbeddedDefinition(
                        typeof(CreateNotificationsService).Assembly,
                        "Marain.UserNotifications.OpenApi.ApiDeliveryChannelService.yaml");

                    hostConfig.Documents.AddSwaggerEndpoint();
                });

            return services;
        }
    }
}
