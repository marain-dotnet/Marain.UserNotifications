// <copyright file="ApiSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
    using Corvus.Retry;
    using Corvus.Retry.Policies;
    using Corvus.Retry.Strategies;
    using Corvus.Testing.SpecFlow;
    using Marain.UserNotifications.Management.Host.OpenApi;
    using Marain.UserNotifications.Specs.Bindings;
    using Marain.UserPreferences;
    using Microsoft.Extensions.DependencyInjection;
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

            // If present, we'll use the response content as a message in case of failure
            this.scenarioContext.TryGetValue(ResponseContent, out string responseContent);

            Assert.AreEqual(expectedStatusCode, response.StatusCode, responseContent);
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

        [Given("I have sent a management API request to batch update the delivery status of the first (.*) stored notifications for user '(.*)' to '(.*)' for the delivery channel with Id '(.*)'")]
        [When("I send a management API request to batch update the delivery status of the first (.*) stored notifications for user '(.*)' to '(.*)' for the delivery channel with Id '(.*)'")]
        public Task WhenISendAManagementAPIRequestToBatchUpdateTheDeliveryStatusOfTheFirstStoredNotificationsToForTheDeliveryChannelWithId(int countToUpdate, string userId, UserNotificationDeliveryStatus targetStatus, string deliveryChannelId)
        {
            // Get the notifications from session state
            List<UserNotification> notifications = this.scenarioContext.Get<List<UserNotification>>(DataSetupSteps.CreatedNotificationsKey);
            BatchDeliveryStatusUpdateRequestItem[] requestBatch = notifications
                .Where(n => n.UserId == userId)
                .Take(countToUpdate)
                .Select(
                    n =>
                    new BatchDeliveryStatusUpdateRequestItem(
                        n.Id!,
                        targetStatus,
                        DateTimeOffset.UtcNow,
                        deliveryChannelId)).ToArray();

            string transientTenantId = this.featureContext.GetTransientTenantId();

            return this.SendPostRequest(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/batchdeliverystatusupdate", requestBatch);
        }

        [Given("I have sent a management API request to batch update the read status of the first (.*) stored notifications for user '(.*)' to '(.*)' for the delivery channel with Id '(.*)'")]
        [When("I send a management API request to batch update the read status of the first (.*) stored notifications for user '(.*)' to '(.*)' for the delivery channel with Id '(.*)'")]
        public Task WhenISendAManagementAPIRequestToBatchUpdateTheReadStatusOfTheFirstStoredNotificationsForUserToForTheDeliveryChannelWithId(int countToUpdate, string userId, UserNotificationReadStatus targetStatus, string deliveryChannelId)
        {
            // Get the notifications from session state
            List<UserNotification> notifications = this.scenarioContext.Get<List<UserNotification>>(DataSetupSteps.CreatedNotificationsKey);
            BatchReadStatusUpdateRequestItem[] requestBatch = notifications
                .Where(n => n.UserId == userId)
                .Take(countToUpdate)
                .Select(
                    n =>
                    new BatchReadStatusUpdateRequestItem(
                        n.Id!,
                        targetStatus,
                        DateTimeOffset.UtcNow,
                        deliveryChannelId)).ToArray();

            string transientTenantId = this.featureContext.GetTransientTenantId();

            return this.SendPostRequest(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/batchreadstatusupdate", requestBatch);
        }

        [Given("I have sent an API delivery request for the user notification with the same Id as the user notification called '(.*)'")]
        [When("I send an API delivery request for the user notification with the same Id as the user notification called '(.*)'")]
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

        [Given("I have waited for up to (.*) seconds for the long running operation whose Url is in the response Location header to have a '(.*)' of '(.*)'")]
        public Task GivenIHaveWaitedForUpToSecondsForTheLongRunningOperationWhoseUrlIsInTheResponseLocationHeaderToHaveAOf(int timeout, string operationPropertyPath, string expectedOperationPropertyValue)
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

        [Given("I have sent an API delivery request for notifications for the user with Id '(.*)'")]
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

        [When("I send a request to mark a notification as read using the Url from the response property '(.*)'")]
        public Task WhenISendARequestToMarkANotificationAsReadUsingTheUrlFromTheResponseProperty(string path)
        {
            // Get the previous response
            JObject previousResponse = this.scenarioContext.Get<JObject>();
            JToken url = JsonSteps.GetRequiredToken(previousResponse, path);
            return this.SendPostRequest(FunctionsApiBindings.ApiDeliveryChannelBaseUri, url.Value<string>(), null);
        }

        [When(@"I send the user preference API a request to create a new user preference")]
        public async Task WhenISendTheUserPreferenceAPIARequestToCreateANewUserPreference(string requestJson)
        {
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            string transientTenantId = this.featureContext.GetTransientTenantId();

            HttpResponseMessage response = await HttpClient.PutAsync(
                new Uri(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/userpreference"),
                requestContent).ConfigureAwait(false);

            this.scenarioContext.Set(response);

            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(responseContent))
            {
                this.scenarioContext.Set(responseContent, ResponseContent);
            }
        }

        [When(@"I send a user preference API request to retreive a user preference")]
        public Task WhenISendAUserPreferenceAPIRequestToRetreiveAUserPreference()
        {
            UserPreference userPreference = this.scenarioContext.Get<UserPreference>();

            string userId = userPreference.UserId;
            string transientTenantId = this.featureContext.GetTransientTenantId();

            return this.SendGetRequest(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/userpreference?userId={userId}");
        }

        [When(@"I send the user notification template API a request to create a new user notification template")]
        [When(@"I send the user notification template API a request to update a new user notification template")]
        public async Task WhenISendTheUserNotificationTemplateAPIARequestToCreateANewUserNotificationTemplate(string requestJson)
        {
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            string transientTenantId = this.featureContext.GetTransientTenantId();

            HttpResponseMessage response = await HttpClient.PutAsync(
                new Uri(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/templates"),
                requestContent).ConfigureAwait(false);

            this.scenarioContext.Set(response);

            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(responseContent))
            {
                this.scenarioContext.Set(responseContent, ResponseContent);
            }
        }

        [When(@"I send the notification template API a request to retreive a notification template with notificationType '(.*)'")]
        public Task WhenISendTheNotificationTemplateAPIARequestToRetreiveANotificationTemplateWithNotificationType(string notificationType)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();

            return this.SendGetRequest(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/templates?notificationType={notificationType}");
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

        private async Task SendPostRequest(Uri baseUri, string path, object? data)
        {
            HttpContent? content = null;

            if (!(data is null))
            {
                IJsonSerializerSettingsProvider serializerSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();
                string requestJson = JsonConvert.SerializeObject(data, serializerSettingsProvider.Instance);
                content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = await HttpClient.PostAsync(new Uri(baseUri, path), content).ConfigureAwait(false);

            this.scenarioContext.Set(response);

            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(ResponseContent))
            {
                this.scenarioContext.Set(responseContent, ResponseContent);
            }
        }
    }
}
