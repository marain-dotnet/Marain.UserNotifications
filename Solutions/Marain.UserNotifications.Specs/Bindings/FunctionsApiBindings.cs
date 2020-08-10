// <copyright file="FunctionsApiBindings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Bindings
{
    using System.Threading.Tasks;
    using Corvus.Testing.AzureFunctions;
    using Corvus.Testing.AzureFunctions.SpecFlow;
    using Corvus.Testing.SpecFlow;
    using TechTalk.SpecFlow;

    [Binding]
    public static class FunctionsApiBindings
    {
        [BeforeFeature("useManagementApi", Order = BindingSequence.FunctionStartup)]
        public static Task StartManagementApi(FeatureContext featureContext)
        {
            FunctionsController functionsController = FunctionsBindings.GetFunctionsController(featureContext);
            FunctionConfiguration functionConfiguration = FunctionsBindings.GetFunctionConfiguration(featureContext);

            return functionsController.StartFunctionsInstance(
                "Marain.UserNotifications.Management.Host",
                7080,
                "netcoreapp3.1",
                configuration: functionConfiguration);
        }

        [AfterScenario("useManagementApi")]
        public static void WriteOutput(FeatureContext featureContext)
        {
            FunctionsController functionsController = FunctionsBindings.GetFunctionsController(featureContext);
            functionsController.GetFunctionsOutput().WriteAllToConsoleAndClear();
        }

        [AfterFeature("useManagementApi")]
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
