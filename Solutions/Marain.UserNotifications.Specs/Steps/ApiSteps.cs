// <copyright file="ApiSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
    using Corvus.Json;
    using Corvus.Retry;
    using Corvus.Retry.Policies;
    using Corvus.Retry.Strategies;
    using Corvus.Testing.SpecFlow;
    using Marain.UserNotifications.Management.Host.OpenApi;
    using Marain.UserNotifications.Specs.Bindings;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ApiSteps
    {
        private const string ResponseContent = "ApiSteps:ResponseContent";

        private static readonly HttpClient HttpClient = HttpClientFactory.Create();

        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;
        private readonly IServiceProvider serviceProvider;

        public ApiSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;

            this.serviceProvider = ContainerBindings.GetServiceProvider(this.featureContext);
        }

        [When("I request the Swagger definition for the management API")]
        public Task WhenIRequestTheSwaggerDefinitionForTheManagementAPI()
        {
            return this.SendGetRequest(FunctionsApiBindings.ManagementApiBaseUri, "/swagger");
        }

        [When("I request the Swagger definition for the API delivery channel")]
        public Task WhenIRequestTheSwaggerDefinitionForTheAPIDeliveryChannel()
        {
            return this.SendGetRequest(FunctionsApiBindings.ApiDeliveryChannelBaseUri, "/swagger");
        }

        [Then("the response status code should be '(.*)'")]
        public void ThenTheResultStatusCodeShouldBe(HttpStatusCode expectedStatusCode)
        {
            HttpResponseMessage response = this.scenarioContext.Get<HttpResponseMessage>();
            Assert.AreEqual(expectedStatusCode, response.StatusCode);
        }

        [When("I send a management API request to create a new notification:")]
        public async Task WhenISendAManagementApiRequestToCreateANewNotification(string requestJson)
        {
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            string transientTenantId = this.featureContext.GetTransientTenantId();

            HttpResponseMessage response = await HttpClient.PutAsync(
                new Uri(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications"),
                requestContent).ConfigureAwait(false);

            this.scenarioContext.Set(response);

            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(responseContent))
            {
                this.scenarioContext.Set(responseContent, ResponseContent);
            }
        }

        [Given("I have used the management API to create (.*) notifications with timestamps at (.*) second intervals for the user with Id '(.*)'")]
        public async Task GivenIHaveUsedTheManagementAPIToCreateNotificationsWithTimestampsAtSecondIntervalsForTheUserWithId(int notificationCount, int interval, string userId)
        {
            IPropertyBagFactory propertyBagFactory = this.serviceProvider.GetRequiredService<IPropertyBagFactory>();
            IJsonSerializerSettingsProvider serializerSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();

            var offset = TimeSpan.FromSeconds(interval);

            var propertiesDictionary = new Dictionary<string, object>
            {
                { "prop1", "val1" },
                { "prop2", 2 },
                { "prop3", DateTime.Now },
            };

            var request = new CreateNotificationsRequest(
                "marain.usernotifications.apitest",
                new[] { userId },
                DateTime.UtcNow,
                propertyBagFactory.Create(propertiesDictionary),
                new[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() });

            var initialRequestTasks = new List<Task<HttpResponseMessage>>();

            var timer = new Stopwatch();
            timer.Start();

            for (int i = 0; i < notificationCount; i++)
            {
                string requestJson = JsonConvert.SerializeObject(request, serializerSettingsProvider.Instance);
                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
                string transientTenantId = this.featureContext.GetTransientTenantId();

                initialRequestTasks.Add(HttpClient.PutAsync(
                    new Uri(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications"),
                    requestContent));

                request = new CreateNotificationsRequest(
                    request.NotificationType,
                    request.UserIds,
                    request.Timestamp - offset,
                    request.Properties,
                    request.CorrelationIds);
            }

            // Wait for the initial requests to complete
            HttpResponseMessage[] responses = await Task.WhenAll(initialRequestTasks).ConfigureAwait(false);

            timer.Stop();

            Trace.TraceInformation($"Executed initial create requests in {timer.ElapsedMilliseconds}ms");

            timer.Reset();
            timer.Start();

            // Now we need to wait for all of the operations to complete.
            await Task.WhenAll(responses.Select(
                r => this.LongRunningOperationPropertyCheck(
                    r.Headers.Location,
                    "status",
                    120,
                    val =>
                    {
                        if (val != "Succeeded")
                        {
                            throw new Exception("Long running operation has not completed successfully.");
                        }

                        Trace.TraceInformation("Long running operation completed successfully.");
                    }))).ConfigureAwait(false);

            timer.Stop();

            Trace.TraceInformation($"Create requests completed in {timer.ElapsedMilliseconds}ms");
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

        [Given("I have sent an API delivery request for (.*) notifications for the user with Id '(.*)'")]
        [When("I send an API delivery request for (.*) notifications for the user with Id '(.*)'")]
        public Task WhenISendAnAPIDeliveryRequestForNotificationsForTheUserWithId(int count, string userId)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            return this.SendGetRequest(FunctionsApiBindings.ApiDeliveryChannelBaseUri, $"/{transientTenantId}/marain/users/{userId}/notifications?maxItems={count}");
        }

        [When("I send an API delivery request for notifications for the user with Id '(.*)'")]
        public Task WhenISendAnAPIDeliveryRequestForNotificationsForTheUserWithId(string userId)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            return this.SendGetRequest(FunctionsApiBindings.ApiDeliveryChannelBaseUri, $"/{transientTenantId}/marain/users/{userId}/notifications");
        }

        [Given("I have sent an API delivery request using the path called '(.*)'")]
        [When("I send an API delivery request using the path called '(.*)'")]
        public Task WhenISendAnAPIDeliveryRequestUsingThePathCalled(string pathName)
        {
            string path = this.scenarioContext.Get<string>(pathName);
            return this.SendGetRequest(FunctionsApiBindings.ApiDeliveryChannelBaseUri, path);
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
                new Linear(TimeSpan.FromSeconds(5), int.MaxValue),
                new AnyExceptionPolicy(),
                false);
        }

        private async Task SendGetRequest(Uri baseUri, string path)
        {
            HttpResponseMessage response = await HttpClient.GetAsync(new Uri(baseUri, path)).ConfigureAwait(false);

            this.scenarioContext.Set(response);

            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            this.scenarioContext.Set(content, ResponseContent);

            var parsedResponse = JObject.Parse(content);
            this.scenarioContext.Set(parsedResponse);
        }
    }
}
