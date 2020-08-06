// <copyright file="UserNotificationsHostServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using Corvus.Azure.Storage.Tenancy;
    using Corvus.Identity.ManagedServiceIdentity.ClientAuthentication;
    using Marain.Tenancy.Client;
    using Marain.UserNotifications.OpenApi.ApiDeliveryChannel;
    using Menes;
    using Menes.Exceptions;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Configures the user notifications API hosts.
    /// </summary>
    public static class UserNotificationsHostServiceCollectionExtensions
    {
        /// <summary>
        /// Configures common services required by both management and delivery channel APIs.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection, to enable chaining.</returns>
        public static IServiceCollection AddCommonUserNotificationsApiServices(
            this IServiceCollection services)
        {
            // Marain services integration, allowing shorthand calls to get and validate the current tenant in operation implementations.
            services.AddMarainServiceConfiguration();
            services.AddMarainServicesTenancy();

            // Tenancy service client.
            services.AddSingleton(sp =>
            {
                TenancyClientOptions tenancyConfiguration = sp.GetRequiredService<IConfiguration>().GetSection("TenancyClient").Get<TenancyClientOptions>();

                if (tenancyConfiguration?.TenancyServiceBaseUri == default)
                {
                    throw new OpenApiServiceMismatchException("Could not find a configuration value for TenancyClient:TenancyServiceBaseUri");
                }

                return tenancyConfiguration;
            });

            services.AddTenantProviderServiceClient();

            // Token source, to provide authentication when accessing external services.
            services.AddAzureManagedIdentityBasedTokenSource(
                sp => new AzureManagedIdentityTokenSourceOptions
                {
                    AzureServicesAuthConnectionString = sp.GetRequiredService<IConfiguration>()["AzureServicesAuthConnectionString"],
                });

            // Notification storage
            services.AddTenantedAzureTableUserNotificationStore(
                sp => new TenantCloudTableFactoryOptions
                {
                    AzureServicesAuthConnectionString = sp.GetRequiredService<IConfiguration>()["AzureServicesAuthConnectionString"],
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
                        typeof(GetNotificationsForUserService).Assembly,
                        "Marain.UserNotifications.OpenApi.ApiDeliveryChannelService.yaml");

                    hostConfig.Documents.AddSwaggerEndpoint();
                });

            return services;
        }
    }
}
