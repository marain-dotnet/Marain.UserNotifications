// <copyright file="ApiSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Marain.UserNotifications.Specs.Bindings;
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
