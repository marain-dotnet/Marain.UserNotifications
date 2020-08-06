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
            Assert.IsFalse(this.scenarioContext.ContainsKey(LastExceptionContextKey));
        }
    }
}
