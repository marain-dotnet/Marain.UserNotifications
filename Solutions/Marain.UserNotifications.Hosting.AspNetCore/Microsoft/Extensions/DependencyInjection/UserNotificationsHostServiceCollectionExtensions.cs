// <copyright file="UserNotificationsHostServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System.Linq;
    using Corvus.Azure.Storage.Tenancy;
    using Corvus.Extensions.Json.Internal;
    using Marain.Operations.Client.OperationsControl;
    using Marain.Tenancy.Client;
    using Marain.UserNotifications;
    using Marain.UserNotifications.OpenApi.ApiDeliveryChannel;
    using Menes;
    using Menes.Exceptions;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Configures the user notifications API hosts.
    /// </summary>
    public static class UserNotificationsHostServiceCollectionExtensions
    {
        /// <summary>
        /// Configures common services required by both management and delivery channel APIs.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <returns>The service collection, to enable chaining.</returns>
        public static IServiceCollection AddCommonUserNotificationsApiServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Monitoring - for better AppInsights integration
            services.AddApplicationInsightsInstrumentationTelemetry();

            services.AddOpenApiJsonSerializerSettings();

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

            services.AddTenantProviderServiceClient(true);

            // Operations control client
            services.AddOperationsControlClient(
                sp =>
                {
                    MarainOperationsControlClientOptions operationsConfiguration =
                        sp.GetRequiredService<IConfiguration>().GetSection("Operations").Get<MarainOperationsControlClientOptions>();

                    if (operationsConfiguration?.OperationsControlServiceBaseUri == default)
                    {
                        throw new OpenApiServiceMismatchException("Could not find a configuration value for Operations:OperationsControlServiceBaseUri");
                    }

                    return operationsConfiguration;
                });

            // Token source, to provide authentication when accessing external services.
            string azureServicesAuthConnectionString = configuration["AzureServicesAuthConnectionString"];
            services.AddServiceIdentityAzureTokenCredentialSourceFromLegacyConnectionString(azureServicesAuthConnectionString);
            services.AddMicrosoftRestAdapterForServiceIdentityAccessTokenSource();

            // Notification storage
            services.AddTenantedAzureTableUserNotificationStore();

            // Template Store
            services.AddTenantedAzureBlobTemplateStore();

            services.EnsureDateTimeOffsetConverterNotPresent();

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

            services.AddOpenApiActionResultHosting<SimpleOpenApiContext>(
                hostConfig =>
                {
                    hostConfig.Documents.RegisterOpenApiServiceWithEmbeddedDefinition(
                        typeof(GetNotificationsForUserService).Assembly,
                        "Marain.UserNotifications.OpenApi.ApiDeliveryChannelService.yaml");

                    hostConfig.Documents.AddSwaggerEndpoint();

                    hostConfig.Exceptions.Map<UserNotificationNotFoundException>(404);
                });

            services.AddUserNotificationsManagementClient(
                sp =>
                {
                    UserNotificationsManagementClientConfiguration managementClientConfiguration =
                        sp.GetRequiredService<IConfiguration>().GetSection("UserNotificationsManagementClient").Get<UserNotificationsManagementClientConfiguration>();

                    if (string.IsNullOrEmpty(managementClientConfiguration?.BaseUri))
                    {
                        throw new OpenApiServiceMismatchException("Could not find a configuration value for UserNotificationsManagementClient:BaseUrl");
                    }

                    return managementClientConfiguration;
                });

            return services;
        }

        /// <summary>
        /// Add the JSON serialization settings we need for the service.
        /// </summary>
        /// <param name="services">The target service collection.</param>
        /// <returns>The service collection.</returns>
        /// <remarks>
        /// This is a modified version of the
        /// <see cref="JsonSerializerSettingsProviderServiceCollectionExtensions.AddJsonSerializerSettings(IServiceCollection)"/>
        /// method that doesn't register the <see cref="DateTimeOffsetConverter"/>. We don't want that particuar
        /// converter as we want the DateTimeOffset to be serialized in the standard way, as defined in RFC3339 section 5.6.
        /// </remarks>
        public static IServiceCollection AddOpenApiJsonSerializerSettings(
            this IServiceCollection services)
        {
            services.AddJsonNetSerializerSettingsProvider();
            services.AddJsonNetPropertyBag();
            services.AddJsonNetCultureInfoConverter();
            services.AddSingleton<JsonConverter>(new StringEnumConverter(new CamelCaseNamingStrategy()));

            return services;
        }

        /// <summary>
        /// Ensures that the DateTimeOffsetConverter from Corvus.Extensions.Newtonsoft.Json is not present in the
        /// service collection.
        /// </summary>
        /// <param name="services">The target service collection.</param>
        /// <returns>The service collection.</returns>
        /// <remarks>
        /// Various of the Marain components add this by default, primarily now for backwards compatibility. However,
        /// this service has never used the DateTimeOffsetConverter, so we need to make sure it's not in the service
        /// collection. This method should be called at the end of your service initialisation.
        /// </remarks>
        public static IServiceCollection EnsureDateTimeOffsetConverterNotPresent(this IServiceCollection services)
        {
            ServiceDescriptor? registration = services.FirstOrDefault(x => x.ImplementationType == typeof(DateTimeOffsetConverter));

            if (registration is not null)
            {
                services.Remove(registration);
            }

            return services;
        }
    }
}