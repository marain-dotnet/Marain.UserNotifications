// <copyright file="ClientLibrarySteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
    using Corvus.Testing.SpecFlow;
    using Marain.UserNotifications.Client;
    using Marain.UserNotifications.Client.ApiDeliveryChannel;
    using Marain.UserNotifications.Client.ApiDeliveryChannel.Resources;
    using Marain.UserNotifications.Client.Management;
    using Marain.UserNotifications.Client.Management.Requests;
    using Marain.UserNotifications.Client.Management.Resources;
    using Marain.UserNotifications.Specs.Bindings;
    using Menes.Hal;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ClientLibrarySteps
    {
        private const string ApiResponseStatusCodeKey = "ApiResponseStatusCode";
        private const string ApiResponseHeadersKey = "ApiResponseHeaders";
        private const string ApiResponseBodyKey = "ApiResponseBody";

        private readonly ScenarioContext scenarioContext;
        private readonly IServiceProvider serviceProvider;
        private readonly FeatureContext featureContext;

        public ClientLibrarySteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
            this.featureContext = featureContext;

            this.serviceProvider = ContainerBindings.GetServiceProvider(featureContext);
        }

        [When("I use the client to send a management API request to create a new notification")]
        public async Task WhenIUseTheClientToSendAManagementAPIRequestToCreateANewNotification(Table table)
        {
            IJsonSerializerSettingsProvider jsonSerializerSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();
            CreateNotificationsRequest data = BuildCreateNotificationsRequestFrom(table.Rows[0], jsonSerializerSettingsProvider.Instance);

            string transientTenantId = this.featureContext.GetTransientTenantId();
            IUserNotificationsManagementClient client = this.serviceProvider.GetRequiredService<IUserNotificationsManagementClient>();

            try
            {
                ApiResponse result = await client.CreateNotificationsAsync(transientTenantId, data, CancellationToken.None).ConfigureAwait(false);
                this.StoreApiResponseDetails(result.StatusCode, result.Headers);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [When("I use the client to send an API delivery request for notifications for the user with Id '(.*)'")]
        public Task WhenIUseTheClientToSendAnAPIDeliveryRequestForNotificationsForTheUserWithId(string userId)
        {
            return this.WhenIUseTheClientToSendAnAPIDeliveryRequestForNotificationsForTheUserWithId(null, userId);
        }

        [When("I use the client to send an API delivery request with an non-existent tenant Id for notifications for the user with Id '(.*)'")]
        public async Task WhenIUseTheClientToSendAnAPIDeliveryRequestWithAnNon_ExistentTenantIdForNotificationsForTheUserWithId(string userId)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            IUserNotificationsApiDeliveryChannelClient client = this.serviceProvider.GetRequiredService<IUserNotificationsApiDeliveryChannelClient>();

            try
            {
                ApiResponse<PagedNotificationListResource> result = await client.GetUserNotificationsAsync(
                    "75b9261673c2714681f14c97bc0439fb0000a5fa332426462456245252435600",
                    userId,
                    null,
                    null).ConfigureAwait(false);

                this.StoreApiResponseDetails(result.StatusCode, result.Headers, result.Body);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [When("I use the client to send a management API request to batch update the delivery status of the first (.*) stored notifications for user '(.*)' to '(.*)' for the delivery channel with Id '(.*)'")]
        public async Task WhenIUseTheClientToSendAManagementAPIRequestToBatchUpdateTheDeliveryStatusOfTheFirstStoredNotificationsForUserToForTheDeliveryChannelWithId(
            int countToUpdate,
            string userId,
            UpdateNotificationDeliveryStatusRequestNewStatus targetStatus,
            string deliveryChannelId)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            IUserNotificationsManagementClient client = this.serviceProvider.GetRequiredService<IUserNotificationsManagementClient>();

            List<UserNotification> notifications = this.scenarioContext.Get<List<UserNotification>>(DataSetupSteps.CreatedNotificationsKey);
            BatchDeliveryStatusUpdateRequestItem[] requestBatch = notifications
                .Where(n => n.UserId == userId)
                .Take(countToUpdate)
                .Select(
                    n =>
                    new BatchDeliveryStatusUpdateRequestItem
                    {
                        DeliveryChannelId = deliveryChannelId,
                        NewStatus = targetStatus,
                        NotificationId = n.Id!,
                        UpdateTimestamp = DateTimeOffset.UtcNow,
                    }).ToArray();

            try
            {
                ApiResponse result = await client.BatchDeliveryStatusUpdateAsync(
                    transientTenantId,
                    requestBatch,
                    CancellationToken.None).ConfigureAwait(false);

                this.StoreApiResponseDetails(result.StatusCode, result.Headers);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [When("I use the client to send a management API request to batch update the read status of the first (.*) stored notifications for user '(.*)' to '(.*)' for the delivery channel with Id '(.*)'")]
        public async Task WhenIUseTheClientToSendAManagementAPIRequestToBatchUpdateTheReadStatusOfTheFirstStoredNotificationsForUserToForTheDeliveryChannelWithId(
            int countToUpdate,
            string userId,
            UpdateNotificationReadStatusRequestNewStatus targetStatus,
            string deliveryChannelId)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            IUserNotificationsManagementClient client = this.serviceProvider.GetRequiredService<IUserNotificationsManagementClient>();

            List<UserNotification> notifications = this.scenarioContext.Get<List<UserNotification>>(DataSetupSteps.CreatedNotificationsKey);
            BatchReadStatusUpdateRequestItem[] requestBatch = notifications
                .Where(n => n.UserId == userId)
                .Take(countToUpdate)
                .Select(
                    n =>
                    new BatchReadStatusUpdateRequestItem
                    {
                        DeliveryChannelId = deliveryChannelId,
                        NewStatus = targetStatus,
                        NotificationId = n.Id!,
                        UpdateTimestamp = DateTimeOffset.UtcNow,
                    }).ToArray();

            try
            {
                ApiResponse result = await client.BatchReadStatusUpdateAsync(
                    transientTenantId,
                    requestBatch,
                    CancellationToken.None).ConfigureAwait(false);

                this.StoreApiResponseDetails(result.StatusCode, result.Headers);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [Given("I have used the client to send an API delivery request for (.*) notification for the user with Id '(.*)'")]
        [Given("I have used the client to send an API delivery request for (.*) notifications for the user with Id '(.*)'")]
        [When("I use the client to send an API delivery request for (.*) notifications for the user with Id '(.*)'")]
        public async Task WhenIUseTheClientToSendAnAPIDeliveryRequestForNotificationsForTheUserWithId(int? count, string userId)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            IUserNotificationsApiDeliveryChannelClient client = this.serviceProvider.GetRequiredService<IUserNotificationsApiDeliveryChannelClient>();

            try
            {
                ApiResponse<PagedNotificationListResource> result = await client.GetUserNotificationsAsync(
                    transientTenantId,
                    userId,
                    null,
                    count).ConfigureAwait(false);

                this.StoreApiResponseDetails(result.StatusCode, result.Headers, result.Body);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [When(@"I use the client to send the notification template API a request to create a new notification template")]
        public async Task WhenIUseTheClientToSendTheNotificationTemplateAPIARequestToCreateANewNotificationTemplate(string request)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            IUserNotificationsManagementClient client = this.serviceProvider.GetRequiredService<IUserNotificationsManagementClient>();

            // notification template
            Client.Management.Resources.NotificationTemplate notificationTemplate = JsonConvert.DeserializeObject<Client.Management.Resources.NotificationTemplate>(request);

            try
            {
                ApiResponse result = await client.SetNotificationTemplate(
                    transientTenantId,
                    notificationTemplate).ConfigureAwait(false);

                this.StoreApiResponseDetails(result.StatusCode, result.Headers, notificationTemplate);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [When(@"I use the client to send the notification template API a request to get a notification template with notification type '(.*)'")]
        public async Task WhenIUseTheClientToSendTheNotificationTemplateAPIARequestToGetANotificationTemplateWithNotificationType(string notificationType)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            IUserNotificationsManagementClient client = this.serviceProvider.GetRequiredService<IUserNotificationsManagementClient>();

            try
            {
                ApiResponse<Client.Management.Resources.NotificationTemplate> result = await client.GetNotificationTemplate(transientTenantId, notificationType).ConfigureAwait(false);

                this.StoreApiResponseDetails(result.StatusCode, result.Headers, result);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [Given("I use the client to send an API delivery request for the user notification with the same Id as the user notification called '(.*)'")]
        public Task GivenIUseTheClientToSendAnAPIDeliveryRequestForTheUserNotificationWithTheSameIdAsTheUserNotificationCalled(string notificationName)
        {
            UserNotification notification = this.scenarioContext.Get<UserNotification>(notificationName);
            return this.WhenIUseTheClientToSendAnAPIDeliveryRequestForTheUserNotificationWithTheId(notification.Id!);
        }

        [When("I use the client to send an API delivery request for the user notification with the Id '(.*)'")]
        public async Task WhenIUseTheClientToSendAnAPIDeliveryRequestForTheUserNotificationWithTheId(string notificationId)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            IUserNotificationsApiDeliveryChannelClient client = this.serviceProvider.GetRequiredService<IUserNotificationsApiDeliveryChannelClient>();

            try
            {
                ApiResponse<NotificationResource> result = await client.GetNotificationAsync(
                    transientTenantId,
                    notificationId).ConfigureAwait(false);

                this.StoreApiResponseDetails(result.StatusCode, result.Headers, result.Body);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [When("I use the client to send an API delivery request for a paged list of notifications using the link called '(.*)' from the previous API delivery channel response")]
        public async Task WhenIUseTheClientToSendAnAPIDeliveryRequestForAPagedListOfNotificationsUsingTheLinkCalledFromThePreviousAPIDeliveryChannelResponse(string linkRelationName)
        {
            PagedNotificationListResource previousResponseBody = this.GetApiResponseBody<PagedNotificationListResource>();
            WebLink nextLink = previousResponseBody.EnumerateLinks("next").Single();

            IUserNotificationsApiDeliveryChannelClient client = this.serviceProvider.GetRequiredService<IUserNotificationsApiDeliveryChannelClient>();

            try
            {
                ApiResponse<PagedNotificationListResource> result = await client.GetUserNotificationsByLinkAsync(
                    nextLink.Href).ConfigureAwait(false);

                this.StoreApiResponseDetails(result.StatusCode, result.Headers, result.Body);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [When("I use the client to send a request to mark a notification as read using the Url from the notififcation in the client response")]
        public async Task WhenIUseTheClientToSendARequestToMarkANotificationAsReadUsingTheUrlFromTheNotififcationInTheClientResponse()
        {
            PagedNotificationListResource previousResponse = this.GetApiResponseBody<PagedNotificationListResource>();
            WebLink targetLink = previousResponse.Items.First().EnumerateLinks("mark-read").Single();

            IUserNotificationsApiDeliveryChannelClient client = this.serviceProvider.GetRequiredService<IUserNotificationsApiDeliveryChannelClient>();

            try
            {
                ApiResponse updateResponse = await client.MarkNotificationAsReadByLinkAsync(
                    targetLink.Href).ConfigureAwait(false);

                this.StoreApiResponseDetails(updateResponse.StatusCode, updateResponse.Headers);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [Then("the client response status code should be '(.*)'")]
        public void ThenTheClientResponseStatusCodeShouldBe(HttpStatusCode expectedResponseCode)
        {
            (HttpStatusCode actualStatus, IDictionary<string, string> _) = this.GetApiResponseDetails();
            Assert.AreEqual(expectedResponseCode, actualStatus);
        }

        [Then("the client response should contain a '(.*)' header")]
        public void ThenTheClientResponseShouldContainAHeader(string headerName)
        {
            (HttpStatusCode _, IDictionary<string, string> actualHeaders) = this.GetApiResponseDetails();
            Assert.IsTrue(actualHeaders.ContainsKey(headerName));
            Assert.IsNotNull(actualHeaders[headerName]);
        }

        [Then("the paged list of notifications in the API delivery channel response should contain (.*) item links")]
        public void ThenThePagedListOfNotificationsInTheAPIDeliveryChannelResponseShouldContainItemLinks(int expectedCount)
        {
            PagedNotificationListResource response = this.GetApiResponseBody<PagedNotificationListResource>();
            Assert.AreEqual(expectedCount, response.ItemLinks.Count());
        }

        [Then("the paged list of notifications in the API delivery channel response should contain (.*) embedded items")]
        public void ThenThePagedListOfNotificationsInTheAPIDeliveryChannelResponseShouldContainEmbeddedItems(int expectedCount)
        {
            PagedNotificationListResource response = this.GetApiResponseBody<PagedNotificationListResource>();
            Assert.AreEqual(expectedCount, response.Items.Count());
        }

        [Then("the paged list of notifications in the API delivery channel response should have a '(.*)' link")]
        public void ThenThePagedListOfNotificationsInTheAPIDeliveryChannelResponseShouldHaveALink(string linkRelationName)
        {
            PagedNotificationListResource response = this.GetApiResponseBody<PagedNotificationListResource>();
            Assert.AreNotEqual(default(WebLink), response.EnumerateLinks(linkRelationName).SingleOrDefault());
        }

        [Then("the notification in the API delivery channel response should have a '(.*)' link")]
        public void ThenTheNotificationInTheAPIDeliveryChannelResponseShouldHaveALink(string linkRelationName)
        {
            NotificationResource response = this.GetApiResponseBody<NotificationResource>();
            Assert.AreNotEqual(default(WebLink), response.EnumerateLinks(linkRelationName).SingleOrDefault());
        }

        [Then("the notification in the API delivery channel response should have a Notification Type of '(.*)'")]
        public void ThenTheNotificationInTheAPIDeliveryChannelResponseShouldHaveANotificationTypeOf(string expected)
        {
            NotificationResource response = this.GetApiResponseBody<NotificationResource>();
            Assert.AreEqual(expected, response.NotificationType);
        }

        [Then("the notification in the API delivery channel response should have a User Id of '(.*)'")]
        public void ThenTheNotificationInTheAPIDeliveryChannelResponseShouldHaveAUserIdOf(string expected)
        {
            NotificationResource response = this.GetApiResponseBody<NotificationResource>();
            Assert.AreEqual(expected, response.UserId);
        }

        [Then("the notification in the API delivery channel response should have a Timestamp of '(.*)'")]
        public void ThenTheNotificationInTheAPIDeliveryChannelResponseShouldHaveATimestampOf(DateTimeOffset expected)
        {
            NotificationResource response = this.GetApiResponseBody<NotificationResource>();
            Assert.AreEqual(expected, response.Timestamp);
        }

        [Then("the notification in the API delivery channel response should have a Delivery Status of '(.*)'")]
        public void ThenTheNotificationInTheAPIDeliveryChannelResponseShouldHaveADeliveryStatusOf(bool expected)
        {
            NotificationResource response = this.GetApiResponseBody<NotificationResource>();
            Assert.AreEqual(expected, response.Delivered);
        }

        [Then("the notification in the API delivery channel response should have a Read Status of '(.*)'")]
        public void ThenTheNotificationInTheAPIDeliveryChannelResponseShouldHaveAReadStatusOf(bool expected)
        {
            NotificationResource response = this.GetApiResponseBody<NotificationResource>();
            Assert.AreEqual(expected, response.Read);
        }

        [Then(@"the user preference in the UserManagement API response should have a '(.*)' with value '(.*)'")]
        public void ThenTheUserPreferenceInTheUserManagementAPIResponseShouldHaveAUserIdWithValue(string propertyName, string expectedValue)
        {
            UserPreference response = this.GetApiResponseBody<UserPreference>();
            object? actualValue = this.GetPropertyValue(response, propertyName);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [When(@"I use the client to send a management API request to create a User Preference")]
        [When(@"I use the client to send a management API request to update a User Preference")]
        public async Task WhenIUseTheClientToSendAManagementAPIRequestToCreateAUserPreference(string request)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            IUserNotificationsManagementClient client = this.serviceProvider.GetRequiredService<IUserNotificationsManagementClient>();

            Client.Management.Resources.UserPreference userPreference = JsonConvert.DeserializeObject<Client.Management.Resources.UserPreference>(request);

            try
            {
                ApiResponse result = await client.SetUserPreference(
                    transientTenantId,
                    userPreference).ConfigureAwait(false);

                this.StoreApiResponseDetails(result.StatusCode, result.Headers, userPreference);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [When(@"I use the client to send a management API request to get a User Preference for userId '(.*)'")]
        [Then(@"I use the client to send a management API request to get a User Preference for userId '(.*)'")]
        public async Task WhenIUseTheClientToSendAManagementAPIRequestToGetAUserPreferenceForUserId(string userId)
        {
            string transientTenantId = this.featureContext.GetTransientTenantId();
            IUserNotificationsManagementClient client = this.serviceProvider.GetRequiredService<IUserNotificationsManagementClient>();

            try
            {
                ApiResponse<Client.Management.Resources.UserPreference> result = await client.GetUserPreference(
                    transientTenantId,
                    userId).ConfigureAwait(false);

                this.StoreApiResponseDetails(result.StatusCode, result.Headers, result.Body);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        private static CreateNotificationsRequest BuildCreateNotificationsRequestFrom(TableRow tableRow, JsonSerializerSettings serializerSettings)
        {
            string[] correlationIds = JArray.Parse(tableRow["CorrelationIds"]).Select(token => token.Value<string>()).ToArray();
            string[] userIds = JArray.Parse(tableRow["UserIds"]).Select(token => token.Value<string>()).ToArray();
            var properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(tableRow["PropertiesJson"], serializerSettings).ToDictionary(x => x.Key, x => (object)x.Value);

            string? notificationId = tableRow.ContainsKey("Id") ? tableRow["Id"] : null;

            return new CreateNotificationsRequest
            {
                NotificationType = tableRow["NotificationType"],
                CorrelationIds = correlationIds,
                Timestamp = DateTime.Parse(tableRow["Timestamp"]),
                UserIds = userIds,
                Properties = properties,
            };
        }

        private void StoreApiResponseDetails(HttpStatusCode statusCode, IDictionary<string, string> headers)
        {
            this.scenarioContext.Set(statusCode, ApiResponseStatusCodeKey);
            this.scenarioContext.Set(headers, ApiResponseHeadersKey);
        }

        private void StoreApiResponseDetails<T>(HttpStatusCode statusCode, IDictionary<string, string> headers, T body)
        {
            this.scenarioContext.Set(statusCode, ApiResponseStatusCodeKey);
            this.scenarioContext.Set(headers, ApiResponseHeadersKey);
            this.scenarioContext.Set(body, ApiResponseBodyKey);
        }

        private (HttpStatusCode, IDictionary<string, string>) GetApiResponseDetails()
        {
            return (this.scenarioContext.Get<HttpStatusCode>(ApiResponseStatusCodeKey), this.scenarioContext.Get<IDictionary<string, string>>(ApiResponseHeadersKey));
        }

        private T GetApiResponseBody<T>()
        {
            return this.scenarioContext.Get<T>(ApiResponseBodyKey);
        }

        private object? GetPropertyValue(object src, string propName)
        {
            return src?.GetType()?.GetProperty(propName)?.GetValue(src, null);
        }
    }
}
