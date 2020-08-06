// <copyright file="UserNotificationsContainerBindings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Bindings
{
    using System;
    using Corvus.Configuration;
    using Corvus.Extensions.Json;
    using Corvus.Testing.SpecFlow;
    using Dynamitey;
    using Marain.UserNotifications.Storage.AzureTable;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TechTalk.SpecFlow;

    [Binding]
    public static class UserNotificationsContainerBindings
    {
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
                    sp.AddJsonSerializerSettings();
                });
        }

        [BeforeScenario("withUserNotificationTableStorage", Order = ContainerBeforeScenarioOrder.PopulateServiceCollection)]
        public static void AddTableStorage(ScenarioContext scenarioContext)
        {
            ContainerBindings.ConfigureServices(
                scenarioContext,
                services =>
                {
                    services.AddSingleton<INotificationStore>(
                        sp =>
                        {
                            IConfiguration config = sp.GetRequiredService<IConfiguration>();
                            string connectionString = config["TestStorage:ConnectionString"];

                            CloudStorageAccount storageAccount = string.IsNullOrEmpty(connectionString)
                                ? CloudStorageAccount.DevelopmentStorageAccount
                                : CloudStorageAccount.Parse(connectionString);

                            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                            CloudTable table = tableClient.GetTableReference($"testrun{Guid.NewGuid():N}");

                            // Add the table to the scenario context so it can be deleted later.
                            scenarioContext.Set(table);

                            return new AzureTableNotificationStore(
                                table,
                                sp.GetRequiredService<IJsonSerializerSettingsProvider>(),
                                sp.GetRequiredService<ILogger<AzureTableNotificationStore>>());
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
