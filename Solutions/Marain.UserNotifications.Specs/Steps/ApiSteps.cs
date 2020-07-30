// <copyright file="ApiSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ApiSteps
    {
        private const string ResponseContent = "ApiSteps:ResponseContent";

        private static readonly HttpClient HttpClient = HttpClientFactory.Create();

        private static readonly Uri BaseUri = new Uri("http://localhost:7080");

        private readonly ScenarioContext scenarioContext;

        public ApiSteps(ScenarioContext scenarioContext)
        {
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
