// <copyright file="ExceptionSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ExceptionSteps
    {
        public const string LastExceptionContextKey = "lastException";

        private readonly ScenarioContext scenarioContext;

        public ExceptionSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        public static void StoreLastExceptionInScenarioContext(Exception exception, ScenarioContext scenarioContext)
        {
            scenarioContext.Set(exception, LastExceptionContextKey);
        }

        [Then("no exception should be thrown")]
        public void ThenTheNoExceptionShouldBeThrown()
        {
            if (this.scenarioContext.TryGetValue(LastExceptionContextKey, out Exception val))
            {
                Assert.Fail($"Expected no exception, but the following exception was thrown:\n{val}");
            }
        }

        [Then("a '(.*)' should be thrown")]
        public void ThenAShouldBeThrown(string expectedExceptionTypeName)
        {
            if (!this.scenarioContext.TryGetValue(LastExceptionContextKey, out Exception val))
            {
                Assert.Fail($"Expected an exception of type '{expectedExceptionTypeName}', but no exception was thrown.");
            }

            string actualExceptionTypeName = val.GetType().Name;

            if (actualExceptionTypeName != expectedExceptionTypeName)
            {
                Assert.Fail($"Expected an exception of type '{expectedExceptionTypeName}', but the thrown exception was of type '{actualExceptionTypeName}'.");
            }
        }
    }
}
