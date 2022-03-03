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
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Marain.UserNotifications;
    using Marain.UserNotifications.Management.Host.OpenApi;
    using Marain.UserNotifications.Specs.Bindings;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ApiSteps
    {
        private const string ResponseContent = "ApiSteps:ResponseContent";
        private const string ResponseETag = "ApiSteps:ResponseETag";

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
            await this.CreateNotifications(requestJson, string.Empty).ConfigureAwait(false);
        }

        [When("I send a management API request to create a new notification via third party delivery channels")]
        public async Task WhenISendAManagementAPIRequestToCreateANewNotificationForDeliveryChannels(string requestJson)
        {
            await this.CreateNotifications(requestJson, "/v2").ConfigureAwait(false);
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
            Uri operationLocation = response.Headers.Location!;

            return LongRunningOperationPropertyCheck(
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
            Uri operationLocation = response.Headers.Location!;

            return LongRunningOperationPropertyCheck(
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
            Uri operationLocation = response.Headers.Location!;

            return LongRunningOperationPropertyCheck(
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
            return this.SendPostRequest(FunctionsApiBindings.ApiDeliveryChannelBaseUri, url.Value<string>()!, null);
        }

        [When("I send the user notification template API a request to create a new user notification template")]
        [When("I send the user notification template API a request to update an existing notification template")]
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

        [When("I send the user notification template API a request to update an existing web push notification template")]
        public async Task WhenISendTheUserNotificationTemplateAPIARequestToUpdateAnExistingWebPushNotificationTemplate(Table table)
        {
            IJsonSerializerSettingsProvider serializerSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();
            WebPushTemplate? savedWebPushTemplate = this.scenarioContext.Get<WebPushTemplate>();

            WebPushTemplate notificationTemplate = DataSetupSteps.BuildWebPushNotificationTemplateFrom(table.Rows[0], savedWebPushTemplate.ETag);

            string? requestJson = JsonConvert.SerializeObject(notificationTemplate, serializerSettingsProvider.Instance);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            string transientTenantId = this.featureContext.GetTransientTenantId();

            var uri = new Uri(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/templates");
            await this.SendRequest(uri, HttpMethod.Put, requestContent, notificationTemplate.ETag).ConfigureAwait(false);
        }

        [When("I send the user notification template API a request to update an existing email notification template")]
        public async Task WhenISendTheUserNotificationTemplateAPIARequestToUpdateAnExistingEmailNotificationTemplate(Table table)
        {
            IJsonSerializerSettingsProvider serializerSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();
            EmailTemplate? savedEmailTemplate = this.scenarioContext.Get<EmailTemplate>();

            EmailTemplate notificationTemplate = DataSetupSteps.BuildEmailNotificationTemplateFrom(table.Rows[0], savedEmailTemplate.ETag);

            string? requestJson = JsonConvert.SerializeObject(notificationTemplate, serializerSettingsProvider.Instance);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            string transientTenantId = this.featureContext.GetTransientTenantId();

            var uri = new Uri(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/templates");
            await this.SendRequest(uri, HttpMethod.Put, requestContent, notificationTemplate.ETag).ConfigureAwait(false);
        }

        [When("I send the user notification template API a request to update an existing sms notification template with an invalid eTag")]
        [When("I send the user notification template API a request to update an existing sms notification template without an eTag")]
        public async Task WhenISendTheUserNotificationTemplateAPIARequestToUpdateAnExistingSmsNotificationTemplateWithoutAnETag(Table table)
        {
            IJsonSerializerSettingsProvider serializerSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();

            SmsTemplate notificationTemplate = DataSetupSteps.BuildSmsNotificationTemplateFrom(table.Rows[0]);
            string? requestJson = JsonConvert.SerializeObject(notificationTemplate, serializerSettingsProvider.Instance);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            string transientTenantId = this.featureContext.GetTransientTenantId();

            var uri = new Uri(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/templates");
            await this.SendRequest(uri, HttpMethod.Put, requestContent, notificationTemplate.ETag).ConfigureAwait(false);
        }

        [When("I send the user notification template API a request to update an existing sms notification template")]
        public async Task WhenISendTheUserNotificationTemplateAPIARequestToUpdateAnExistingSmsNotificationTemplate(Table table)
        {
            IJsonSerializerSettingsProvider serializerSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();
            SmsTemplate? savedSmsTemplate = this.scenarioContext.Get<SmsTemplate>();

            SmsTemplate notificationTemplate = DataSetupSteps.BuildSmsNotificationTemplateFrom(table.Rows[0], savedSmsTemplate.ETag);
            string? requestJson = JsonConvert.SerializeObject(notificationTemplate, serializerSettingsProvider.Instance);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            string transientTenantId = this.featureContext.GetTransientTenantId();

            var uri = new Uri(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/templates");
            await this.SendRequest(uri, HttpMethod.Put, requestContent, notificationTemplate.ETag).ConfigureAwait(false);
        }

        [When("I send the notification template API a request to retreive a notification template with notificationType '(.*)' and communicationType '(.*)'")]
        public Task WhenISendTheNotificationTemplateAPIARequestToRetreiveANotificationTemplateWithNotificationTypeAndCommunicationType(string notificationType, string communicationType)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();

            return this.SendGetRequest(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/templates?notificationType={notificationType}&communicationType={communicationType}");
        }

        [When("I send the generate template API request")]
        public async Task WhenISendTheGenerateTemplateAPIRequest(string requestJson)
        {
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            string transientTenantId = this.featureContext.GetTransientTenantId();

            HttpResponseMessage response = await HttpClient.PutAsync(
                new Uri(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications/templates/generate"),
                requestContent).ConfigureAwait(false);

            this.scenarioContext.Set(response);

            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            this.scenarioContext.Set(content, ResponseContent);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(content))
            {
                var parsedResponse = JObject.Parse(content);
                this.scenarioContext.Set(parsedResponse);
            }
        }

        [Then("the response header should have a property called eTag that should not be null")]
        public void ThenTheResponseHeaderShouldHaveAPropertyCalledETagThatShouldNotBeNull()
        {
            string? eTag = this.scenarioContext.Get<string?>(ResponseETag);
            Assert.IsFalse(string.IsNullOrWhiteSpace(eTag));
        }

        private static Task LongRunningOperationPropertyCheck(Uri location, string operationPropertyPath, int timeoutSeconds, Action<string> testValue)
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

            return Retriable.RetryAsync(
                async () =>
                {
                    HttpResponseMessage response = await HttpClient.GetAsync(location).ConfigureAwait(false);
                    string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var operation = JObject.Parse(responseBody);
                    JToken targetToken = operation.SelectToken(operationPropertyPath)!;
                    string currentValue = targetToken.Value<string>()!;
                    testValue(currentValue);
                },
                tokenSource.Token,
                new Linear(TimeSpan.FromSeconds(5), int.MaxValue),
                new AnyExceptionPolicy(),
                false);
        }

        private async Task CreateNotifications(string requestJson, string version)
        {
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            string transientTenantId = this.featureContext.GetTransientTenantId();

            HttpResponseMessage response = await HttpClient.PutAsync(
                new Uri(FunctionsApiBindings.ManagementApiBaseUri, $"/{transientTenantId}/marain/usernotifications{version}"),
                requestContent).ConfigureAwait(false);

            this.scenarioContext.Set(response);

            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(responseContent))
            {
                this.scenarioContext.Set(responseContent, ResponseContent);
            }
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

            if (!string.IsNullOrWhiteSpace(response.Headers?.ETag?.ToString()))
            {
                this.scenarioContext.Set(response.Headers?.ETag?.ToString(), ResponseETag);
            }
        }

        private async Task SendPostRequest(Uri baseUri, string path, object? data)
        {
            HttpContent? content = null;

            if (data is not null)
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

        private async Task SendRequest(Uri uri, HttpMethod httpMethod, StringContent requestContent, string? etag = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = httpMethod,
                Content = requestContent,
            };

            if (!string.IsNullOrWhiteSpace(etag))
            {
                request.Headers.Add("If-None-Match", etag);
            }

            HttpResponseMessage response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            this.scenarioContext.Set(response);

            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(ResponseContent))
            {
                this.scenarioContext.Set(responseContent, ResponseContent);
            }

            if (!string.IsNullOrWhiteSpace(response.Headers?.ETag?.ToString()))
            {
                this.scenarioContext.Set(response.Headers?.ETag?.ToString(), ResponseETag);
            }
        }
    }
}