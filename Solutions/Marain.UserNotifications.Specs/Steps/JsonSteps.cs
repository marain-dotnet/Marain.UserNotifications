// <copyright file="JsonSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs
{
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class JsonSteps
    {
        private readonly ScenarioContext scenarioContext;

        public JsonSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Then("the response content should have a property called '(.*)'")]
        public void ThenTheResponseObjectShouldHaveAPropertyCalled(string propertyPath)
        {
            this.GetRequiredTokenFromResponseObject(propertyPath);
        }

        [Then("the response content should have a string property called '(.*)' with value '(.*)'")]
        public void ThenTheResponseObjectShouldHaveAStringPropertyCalledWithValue(string propertyPath, string expectedValue)
        {
            JToken actualToken = this.GetRequiredTokenFromResponseObject(propertyPath);

            string actualValue = actualToken.Value<string>();
            Assert.AreEqual(expectedValue, actualValue, $"Expected value of property '{propertyPath}' was '{expectedValue}', but actual value was '{actualValue}'");
        }

        [Then("the response content should not have a property called '(.*)'")]
        public void ThenTheResponseObjectShouldNotHaveAPropertyCalled(string propertyPath)
        {
            JObject data = this.scenarioContext.Get<JObject>();
            JToken token = data.SelectToken(propertyPath);
            Assert.IsNull(token, $"Expected not to find a property with path '{propertyPath}', but one was present.");
        }

        [Then("the response content should have an array property called '(.*)' containing (.*) entries")]
        public void ThenTheResponseObjectShouldHaveAnArrayPropertyCalledContainingEntries(string propertyPath, int expectedEntryCount)
        {
            JToken actualToken = this.GetRequiredTokenFromResponseObject(propertyPath);
            JToken[] tokenArray = actualToken.ToArray();
            Assert.AreEqual(expectedEntryCount, tokenArray.Length, $"Expected array '{propertyPath}' to contain {expectedEntryCount} elements but found {tokenArray.Length}.");
        }

        [Given("I have stored the value of the response content property called '(.*)' as '(.*)'")]
        public void GivenIHaveStoredTheValueOfTheResponseObjectPropertyCalledAs(string propertyPath, string storeAsName)
        {
            JToken token = this.GetRequiredTokenFromResponseObject(propertyPath);
            string valueAsString = token.Value<string>();
            this.scenarioContext.Set(valueAsString, storeAsName);
        }

        private JToken GetRequiredTokenFromResponseObject(string propertyPath)
        {
            JObject data = this.scenarioContext.Get<JObject>();
            JToken token = data.SelectToken(propertyPath);
            Assert.IsNotNull(token, $"Could not locate a property with path '{propertyPath}'");
            return token;
        }
    }
}
