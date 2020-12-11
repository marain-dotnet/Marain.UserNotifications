// <copyright file="Startup.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

[assembly: Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup(typeof(Marain.UserNotifications.Management.Host.Startup))]

namespace Marain.UserNotifications.Management.Host
{
    using Marain.UserNotifications.Management.Host.Helpers;
    using Marain.UserNotifications.Management.Host.Mappers;
    using Marain.UserNotifications.Management.Host.OpenApi;
    using Marain.UserPreferences;
    using Menes;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Startup code for the management host.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Configures the function host.
        /// </summary>
        /// <param name="builder">The functions host builder.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IServiceCollection services = builder.Services;

            services.AddLogging();

            services.AddSingleton<IMessageSerializerSettingsFactory, SerializationSettingsFactoryAdapter>();

            services.AddCommonUserNotificationsApiServices();

            AddTenantedUserNotificationsManagementApi(services);
        }

        /// <summary>
        /// Configures services neededed to provide the user notifications management API.
        /// </summary>
        /// <param name="services">The service collection.</param>
        private static void AddTenantedUserNotificationsManagementApi(IServiceCollection services)
        {
            services.AddHalDocumentMapper<UserPreference, IOpenApiContext, UserPreferenceMapper>();

            services.AddSingleton<IOpenApiService, CreateNotificationsService>();
            services.AddSingleton<IOpenApiService, GetNotificationsService>();
            services.AddSingleton<IOpenApiService, GetNotificationStatusService>();
            services.AddSingleton<IOpenApiService, BatchDeliveryStatusUpdateService>();
            services.AddSingleton<IOpenApiService, BatchReadStatusUpdateService>();
            services.AddSingleton<IOpenApiService, CreateUserPreferenceService>();
            services.AddSingleton<IOpenApiService, GetUserPreferenceService>();
            services.AddSingleton<IOpenApiService, GetTemplateService>();
            services.AddSingleton<IOpenApiService, CreateTemplateService>();
            services.AddSingleton<IOpenApiService, GenerateTemplateService>();

            services.AddOpenApiHttpRequestHosting<DurableFunctionsOpenApiContext>(
                hostConfig =>
                {
                    System.Type serviceType = typeof(CreateNotificationsService);
                    hostConfig.Documents.RegisterOpenApiServiceWithEmbeddedDefinition(
                        serviceType.Assembly,
                        $"{serviceType.Namespace}.ManagementService.yaml");

                    hostConfig.Documents.AddSwaggerEndpoint();
                });
        }
    }
}
