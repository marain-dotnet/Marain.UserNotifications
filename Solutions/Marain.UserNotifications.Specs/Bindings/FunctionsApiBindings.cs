// <copyright file="FunctionsApiBindings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Bindings
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Testing.AzureFunctions;
    using Corvus.Testing.AzureFunctions.SpecFlow;
    using Corvus.Testing.SpecFlow;
    using TechTalk.SpecFlow;

    [Binding]
    public static class FunctionsApiBindings
    {
        public const int ManagementApiPort = 7000;

        public const int ApiDeliveryChannelPort = 7001;

        public static readonly Uri ManagementApiBaseUri = new Uri($"http://localhost:{ManagementApiPort}");

        public static readonly Uri ApiDeliveryChannelBaseUri = new Uri($"http://localhost:{ApiDeliveryChannelPort}");

        [BeforeFeature("useApis", Order = BindingSequence.FunctionStartup)]
        public static Task StartManagementApi(FeatureContext featureContext)
        {
            FunctionsController functionsController = FunctionsBindings.GetFunctionsController(featureContext);
            FunctionConfiguration functionConfiguration = FunctionsBindings.GetFunctionConfiguration(featureContext);

            return Task.WhenAll(
                functionsController.StartFunctionsInstance(
                    "Marain.UserNotifications.Management.Host",
                    ManagementApiPort,
                    "netcoreapp3.1",
                    configuration: functionConfiguration),
                functionsController.StartFunctionsInstance(
                    "Marain.UserNotifications.ApiDeliveryChannel.Host",
                    ApiDeliveryChannelPort,
                    "netcoreapp3.1",
                    configuration: functionConfiguration));
        }

        [AfterScenario("useApis")]
        public static void WriteOutput(FeatureContext featureContext)
        {
            FunctionsController functionsController = FunctionsBindings.GetFunctionsController(featureContext);
            functionsController.GetFunctionsOutput().WriteAllToConsoleAndClear();
        }

        [AfterFeature("useApis")]
        public static void StopManagementApi(FeatureContext featureContext)
        {
            featureContext.RunAndStoreExceptions(
                () =>
                {
                    FunctionsController functionsController = FunctionsBindings.GetFunctionsController(featureContext);
                    functionsController.TeardownFunctions();
                });
        }
    }
}
