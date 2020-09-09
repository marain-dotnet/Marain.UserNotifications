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
                    count,
                    null).ConfigureAwait(false);

                this.StoreApiResponseDetails(result.StatusCode, result.Headers, result.Body);
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
    }
}
