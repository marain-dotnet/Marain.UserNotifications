// <copyright file="SelfHostedApiBindings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Bindings
{
    using System.Threading.Tasks;
    using Corvus.Testing.SpecFlow;
    using Menes.Testing.AspNetCoreSelfHosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TechTalk.SpecFlow;

    [Binding]
    public static class SelfHostedApiBindings
    {
        [BeforeScenario("useApiDeliveryChannelApi", Order = ContainerBeforeScenarioOrder.ServiceProviderAvailable)]
        public static Task StartSelfHostedApiDeliveryChannelApi(ScenarioContext scenarioContext)
        {
            var hostManager = new OpenApiWebHostManager();
            scenarioContext.Set(hostManager);

            return hostManager.StartHostAsync<ApiDeliveryChannel.Host.Startup>(
                "http://localhost:7080",
                services =>
                {
                    // Ensure log level for the service is set to debug.
                    services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Debug));
                });
        }

        [AfterScenario("useApiDeliveryChannelApi")]
        public static Task StopSelfHostedDeliveryChannelApi(ScenarioContext scenarioContext)
        {
            OpenApiWebHostManager hostManager = scenarioContext.Get<OpenApiWebHostManager>();
            return hostManager.StopAllHostsAsync();
        }
    }
}
