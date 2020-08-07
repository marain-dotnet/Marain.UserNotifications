// <copyright file="ApiSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.Retry;
    using Corvus.Retry.Policies;
    using Corvus.Retry.Strategies;
    using Marain.UserNotifications.Specs.Bindings;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ApiSteps
    {
        private const string ResponseContent = "ApiSteps:ResponseContent";

        private static readonly HttpClient HttpClient = HttpClientFactory.Create();

        private static readonly Uri BaseUri = new Uri("http://localhost:7080");
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;

        public ApiSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
        }

        [When("I request the Swagger definition for the API")]
        public Task WhenIRequestTheSwaggerDefinitionForTheAPI()
        {
            return this.SendGetRequest("/swagger");
        }

        [Then("the response status code should be '(.*)'")]
        public void ThenTheResultStatusCodeShouldBe(HttpStatusCode expectedStatusCode)
        {
            HttpResponseMessage response = this.scenarioContext.Get<HttpResponseMessage>();
            Assert.AreEqual(expectedStatusCode, response.StatusCode);
        }

        [When("I send a request to create a new notification:")]
        public async Task WhenISendARequestToCreateANewNotification(string requestJson)
        {
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            string transientTenantId = this.featureContext.GetTransientTenantId();

            HttpResponseMessage response = await HttpClient.PutAsync(
                new Uri(BaseUri, $"/{transientTenantId}/marain/usernotifications"),
                requestContent).ConfigureAwait(false);

            this.scenarioContext.Set(response);

            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(responseContent))
            {
                this.scenarioContext.Set(responseContent, ResponseContent);
            }
        }

        [Then("the response should contain a '(.*)' header")]
        public void ThenTheResponseShouldContainAHeader(string headerName)
        {
            HttpResponseMessage response = this.scenarioContext.Get<HttpResponseMessage>();
            Assert.IsTrue(response.Headers.Contains(headerName));
        }

        [Then("the long running operation whose Url is in the response Location header should have a '(.*)' of '(.*)' within (.*) seconds")]
        public Task ThenTheLongRunningOperationWhoseUrlIsInTheResponseLocationHeaderShouldHaveAOfWithinSeconds(string operationPropertyPath, string expectedOperationPropertyValue, int timeout)
        {
            HttpResponseMessage response = this.scenarioContext.Get<HttpResponseMessage>();
            Uri operationLocation = response.Headers.Location;

            return this.LongRunningOperationPropertyCheck(
                operationLocation,
                operationPropertyPath,
                timeout,
                val =>
                {
                    if (val != expectedOperationPropertyValue)
                    {
                        throw new Exception($"Property '{operationPropertyPath}' has the value '{val}', not the required value '{expectedOperationPropertyValue}'");
                    }
                });
        }

        [Then("the long running operation whose Url is in the response Location header should not have a '(.*)' of '(.*)' within (.*) seconds")]
        public Task ThenTheLongRunningOperationWhoseUrlIsInTheResponseLocationHeaderShouldNotHaveAOfWithinSeconds(string operationPropertyPath, string expectedOperationPropertyValue, int timeout)
        {
            HttpResponseMessage response = this.scenarioContext.Get<HttpResponseMessage>();
            Uri operationLocation = response.Headers.Location;

            return this.LongRunningOperationPropertyCheck(
                operationLocation,
                operationPropertyPath,
                timeout,
                val =>
                {
                    if (val == expectedOperationPropertyValue)
                    {
                        throw new Exception($"Property '{operationPropertyPath}' still has the value '{val}'.");
                    }
                });
        }

        private Task LongRunningOperationPropertyCheck(Uri location, string operationPropertyPath, int timeoutSeconds, Action<string> testValue)
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

            return Retriable.RetryAsync(
                async () =>
                {
                    HttpResponseMessage response = await HttpClient.GetAsync(location).ConfigureAwait(false);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var operation = JObject.Parse(responseBody);
                    JToken targetToken = operation.SelectToken(operationPropertyPath);
                    string currentValue = targetToken.Value<string>();
                    testValue(currentValue);
                },
                tokenSource.Token,
                new Linear(TimeSpan.FromSeconds(3), int.MaxValue),
                new AnyExceptionPolicy(),
                false);
        }

        private async Task SendGetRequest(string path)
        {
            HttpResponseMessage response = await HttpClient.GetAsync(new Uri(BaseUri, path)).ConfigureAwait(false);

            this.scenarioContext.Set(response);

            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(content))
            {
                this.scenarioContext.Set(content, ResponseContent);
            }
        }
    }
}
