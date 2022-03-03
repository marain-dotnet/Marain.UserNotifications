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
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TechTalk.SpecFlow;

    [Binding]
    public static class FunctionsApiBindings
    {
        public const int ManagementApiPort = 7000;

        public const int ApiDeliveryChannelPort = 7001;

        public static readonly Uri ManagementApiBaseUri = new($"http://localhost:{ManagementApiPort}");

        public static readonly Uri ApiDeliveryChannelBaseUri = new($"http://localhost:{ApiDeliveryChannelPort}");

        [BeforeFeature("useApis", Order = BindingSequence.FunctionStartup)]
        public static Task StartManagementApi(FeatureContext featureContext)
        {
            FunctionsController functionsController = FunctionsBindings.GetFunctionsController(featureContext);
            FunctionConfiguration functionConfiguration = FunctionsBindings.GetFunctionConfiguration(featureContext);

            functionConfiguration.EnvironmentVariables.Add("UserNotificationsManagementClient:BaseUri", ManagementApiBaseUri.ToString());
            functionConfiguration.EnvironmentVariables.Add("UserNotificationsApiDeliveryChannelClient:BaseUri", ApiDeliveryChannelBaseUri.ToString());

            return Task.WhenAll(
                functionsController.StartFunctionsInstance(
                    "Marain.UserNotifications.Management.Host",
                    ManagementApiPort,
                    "net6.0",
                    configuration: functionConfiguration),
                functionsController.StartFunctionsInstance(
                    "Marain.UserNotifications.ApiDeliveryChannel.Host",
                    ApiDeliveryChannelPort,
                    "net6.0",
                    configuration: functionConfiguration));
        }

        [AfterScenario("useApis")]
        public static void WriteOutput(FeatureContext featureContext)
        {
            ILogger<FunctionsController> logger = ContainerBindings.GetServiceProvider(featureContext).GetRequiredService<ILogger<FunctionsController>>();
            FunctionsController functionsController = FunctionsBindings.GetFunctionsController(featureContext);
            logger.LogAllAndClear(functionsController.GetFunctionsOutput());
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