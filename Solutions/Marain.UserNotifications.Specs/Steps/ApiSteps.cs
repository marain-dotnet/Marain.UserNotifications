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
    using Corvus.Testing.SpecFlow;
    using Marain.UserNotifications.Specs.Bindings;
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

        [Given("I send an API delivery request for the user notification with the same Id as the user notification called '(.*)'")]
        public Task GivenISendAnAPIDeliveryRequestForTheUserNotificationWithTheSameIdAsTheUserNotificationCalled(string notificationName)
        {
            UserNotification notification = this.scenarioContext.Get<UserNotification>(notificationName);
            string transientTenantId = this.featureContext.GetTransientTenantId();
            return this.SendGetRequest(FunctionsApiBindings.ApiDeliveryChannelBaseUri, $"/{transientTenantId}/marain/usernotifications/{WebUtility.UrlEncode(notification.Id)}");
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

        [When("I send an API delivery request for the user notification with the Id '(.*)'")]
        public Task WhenISendAnAPIDeliveryRequestForTheUserNotificationWithTheId(string notificationId)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            return this.SendGetRequest(FunctionsApiBindings.ApiDeliveryChannelBaseUri, $"/{transientTenantId}/marain/usernotifications/{WebUtility.UrlEncode(notificationId)}");
        }

        private Task LongRunningOperationPropertyCheck(Uri location, string operationPropertyPath, int timeoutSeconds, Action<string> testValue)
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

            return Retriable.RetryAsync(
                async () =>
                {
                    HttpResponseMessage response = await HttpClient.GetAsync(location).ConfigureAwait(false);
                    string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(content))
            {
                var parsedResponse = JObject.Parse(content);
                this.scenarioContext.Set(parsedResponse);
            }
        }
    }
}
