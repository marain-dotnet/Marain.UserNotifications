// <copyright file="UserNotificationsContainerBindings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Bindings
{
    using System;
    using Corvus.Azure.Storage.Tenancy;
    using Corvus.Configuration;
    using Corvus.Extensions.Json;
    using Corvus.Identity.ManagedServiceIdentity.ClientAuthentication;
    using Corvus.Testing.SpecFlow;
    using Marain.Extensions.DependancyInjection;
    using Marain.Tenancy.Client;
    using Marain.UserNotifications.Client.ApiDeliveryChannel;
    using Marain.UserNotifications.Client.Management;
    using Marain.UserNotifications.Storage.AzureTable;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TechTalk.SpecFlow;

    [Binding]
    public static class UserNotificationsContainerBindings
    {
        /// <summary>
        /// Bindings for "integration test" style specs (normally those running functions) where we set up/tear down
        /// per feature due to the complexity and slowness of setup.
        /// </summary>
        /// <param name="featureContext">Current feature context.</param>
        [BeforeFeature("perFeatureContainer", Order = ContainerBeforeFeatureOrder.PopulateServiceCollection)]
        public static void PerFeatureContainerSetup(FeatureContext featureContext)
        {
            ContainerBindings.ConfigureServices(
                featureContext,
                services =>
                {
                    var configBuilder = new ConfigurationBuilder();
                    configBuilder.AddConfigurationForTest("appsettings.json");
                    IConfigurationRoot config = configBuilder.Build();
                    services.AddSingleton<IConfiguration>(config);

                    services.AddLogging();
                    services.AddOpenApiJsonSerializerSettings();

                    // Tenancy service client.
                    services.AddSingleton(sp =>
                    {
                        TenancyClientOptions tenancyConfiguration = sp.GetRequiredService<IConfiguration>().GetSection("TenancyClient").Get<TenancyClientOptions>();

                        if (tenancyConfiguration?.TenancyServiceBaseUri == default)
                        {
                            throw new InvalidOperationException("Could not find a configuration value for TenancyClient:TenancyServiceBaseUri");
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

                    // Marain tenancy management, required to create transient client/service tenants.
                    services.AddMarainTenantManagement();

                    // Add the tenanted table store for notifications so we can clear up our own mess after the test.
                    services.AddTenantedAzureTableUserNotificationStore(
                        sp => new TenantCloudTableFactoryOptions
                        {
                            AzureServicesAuthConnectionString = sp.GetRequiredService<IConfiguration>()["AzureServicesAuthConnectionString"],
                        });

                    // Add the tenanted blob store for user preferences so we can clear up our own mess after the test.
                    services.AddTenantedAzureBlobUserPreferencesStore(
                        sp => new TenantCloudBlobContainerFactoryOptions
                        {
                            AzureServicesAuthConnectionString = sp.GetRequiredService<IConfiguration>()["AzureServicesAuthConnectionString"],
                        });

                    // Add the tenanted blob store for the notification tempalte store so we can clear up our own mess after the test.
                    services.AddTenantedAzureBlobTemplateStore(
                        sp => new TenantCloudBlobContainerFactoryOptions
                        {
                            AzureServicesAuthConnectionString = sp.GetRequiredService<IConfiguration>()["AzureServicesAuthConnectionString"],
                        });

                    services.RegisterCoreUserNotificationsContentTypes();
                });
        }

        /// <summary>
        /// Bindings for "unit test" style specs, where we can set up/tear down the container for each scenario.
        /// </summary>
        /// <param name="scenarioContext">Current scenario context.</param>
        [BeforeScenario("perScenarioContainer", Order = ContainerBeforeScenarioOrder.PopulateServiceCollection)]
        public static void PerScenarioContainerSetup(ScenarioContext scenarioContext)
        {
            ContainerBindings.ConfigureServices(
                scenarioContext,
                sp =>
                {
                    var configBuilder = new ConfigurationBuilder();
                    configBuilder.AddConfigurationForTest("local.settings.json");
                    IConfigurationRoot config = configBuilder.Build();
                    sp.AddSingleton<IConfiguration>(config);

                    sp.AddLogging();
                    sp.AddOpenApiJsonSerializerSettings();
                });
        }

        [BeforeFeature("useApis", Order = ContainerBeforeFeatureOrder.PopulateServiceCollection)]
        public static void AddApiClients(FeatureContext featureContext)
        {
            ContainerBindings.ConfigureServices(
                featureContext,
                services =>
                {
                    services.AddHttpClient<IUserNotificationsManagementClient>();
                    services.AddHttpClient<IUserNotificationsApiDeliveryChannelClient>();

                    var managementConfig = new UserNotificationsManagementClientConfiguration { BaseUri = FunctionsApiBindings.ManagementApiBaseUri.ToString() };
                    services.AddUserNotificationsManagementClient(_ => managementConfig);

                    var apiDeliveryChannelConfig = new UserNotificationsApiDeliveryChannelClientConfiguration { BaseUri = FunctionsApiBindings.ApiDeliveryChannelBaseUri.ToString() };
                    services.AddUserNotificationsApiDeliveryChannelClient(_ => apiDeliveryChannelConfig);
                });
        }

        [BeforeScenario("withUserNotificationTableStorage", Order = ContainerBeforeScenarioOrder.PopulateServiceCollection)]
        public static void AddTableStorage(ScenarioContext scenarioContext)
        {
            ContainerBindings.ConfigureServices(
                scenarioContext,
                services =>
                {
                    services.AddSingleton<IUserNotificationStore>(
                        sp =>
                        {
                            IConfiguration config = sp.GetRequiredService<IConfiguration>();
                            string connectionString = config["TestTableStorageConfiguration:AccountName"];

                            CloudStorageAccount storageAccount = string.IsNullOrEmpty(connectionString)
                                ? CloudStorageAccount.DevelopmentStorageAccount
                                : CloudStorageAccount.Parse(connectionString);

                            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                            CloudTable table = tableClient.GetTableReference($"testrun{Guid.NewGuid():N}");

                            // Add the table to the scenario context so it can be deleted later.
                            scenarioContext.Set(table);

                            return new AzureTableUserNotificationStore(
                                table,
                                sp.GetRequiredService<IJsonSerializerSettingsProvider>(),
                                sp.GetRequiredService<ILogger<AzureTableUserNotificationStore>>());
                        });
                });
        }

        [AfterScenario("withUserNotificationTableStorage")]
        public static void ClearDownTableStorage(ScenarioContext scenarioContext)
        {
            scenarioContext.RunAndStoreExceptionsAsync(() => scenarioContext.Get<CloudTable>().DeleteIfExistsAsync());
        }
    }
}
