// <copyright file="UserNotificationsContainerBindings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Bindings
{
    using Corvus.Testing.SpecFlow;
    using Marain.UserNotifications.Storage.AzureTable;
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
                        sp => new AzureTableNotificationStore(sp.GetRequiredService<ILogger<AzureTableNotificationStore>>()));
                });
        }
    }
}
