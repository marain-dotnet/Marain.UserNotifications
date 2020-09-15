// <copyright file="NotificationSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
    using Corvus.Retry;
    using Corvus.Retry.Policies;
    using Corvus.Retry.Strategies;
    using Corvus.Testing.SpecFlow;
    using Marain.UserNotifications.Specs.Bindings;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class NotificationSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly IServiceProvider serviceProvider;
        private readonly IJsonSerializerSettingsProvider serializationSettingsProvider;
        private readonly FeatureContext featureContext;

        public NotificationSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;

            this.serviceProvider = featureContext.FeatureInfo.Tags.Contains("perFeatureContainer")
                ? ContainerBindings.GetServiceProvider(featureContext)
                : ContainerBindings.GetServiceProvider(scenarioContext);

            this.serializationSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();
        }

        public static void AssertUserNotificationsMatch(
            UserNotification expected,
            UserNotification actual,
            IJsonSerializerSettingsProvider serializerSettingsProvider)
        {
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.NotificationType, actual.NotificationType);
            Assert.AreEqual(expected.Timestamp, actual.Timestamp);

            Assert.AreEqual(expected.Metadata.CorrelationIds.Length, actual.Metadata.CorrelationIds.Length);
            for (int idx = 0; idx < expected.Metadata.CorrelationIds.Length; idx++)
            {
                Assert.AreEqual(expected.Metadata.CorrelationIds[idx], actual.Metadata.CorrelationIds[idx], $"Correlation Ids at index {idx} do not match.");
            }

            Assert.AreEqual(expected.ChannelStatuses.Count(), actual.ChannelStatuses.Count());
            foreach (UserNotificationStatus expectedStatus in expected.ChannelStatuses)
            {
                UserNotificationStatus? actualStatus = actual.ChannelStatuses.FirstOrDefault(s => s.DeliveryChannelId == expectedStatus.DeliveryChannelId);

                Assert.IsNotNull(actualStatus, $"Could not find channel delivery status for channel Id '{expectedStatus.DeliveryChannelId}'");

                Assert.AreEqual(expectedStatus.DeliveryStatus, actualStatus!.DeliveryStatus, $"Delivery status mismatch for channel Id '{expectedStatus.DeliveryChannelId}'");
                Assert.AreEqual(expectedStatus.DeliveryStatusLastUpdated, actualStatus!.DeliveryStatusLastUpdated, $"Delivery status last updated mismatch for channel Id '{expectedStatus.DeliveryChannelId}'");

                Assert.AreEqual(expectedStatus.ReadStatus, actualStatus!.ReadStatus, $"Read status mismatch for channel Id '{expectedStatus.DeliveryChannelId}'");
                Assert.AreEqual(expectedStatus.ReadStatusLastUpdated, actualStatus!.ReadStatusLastUpdated, $"Read status last updated mismatch for channel Id '{expectedStatus.DeliveryChannelId}'");
            }

            // As always, the easiest way to verify two property bags match is to serialize them.
            string serializedActualProperties = JsonConvert.SerializeObject(actual.Properties, serializerSettingsProvider.Instance);
            string serializedExpectedProperties = JsonConvert.SerializeObject(expected.Properties, serializerSettingsProvider.Instance);

            Assert.AreEqual(serializedExpectedProperties, serializedActualProperties);
        }

        [Then("the properties of the user notification called '(.*)' should match the user notification called '(.*)'")]
        public void ThenThePropertiesOfTheUserNotificationCalledShouldMatchTheUserNotificationCalled(string actualNotificationName, string expectedNotificationName)
        {
            UserNotification actual = this.scenarioContext.Get<UserNotification>(actualNotificationName);
            UserNotification expected = this.scenarioContext.Get<UserNotification>(expectedNotificationName);

            AssertUserNotificationsMatch(expected, actual, this.serializationSettingsProvider);
        }

        [Then("the Id of the user notification called '(.*)' should be set")]
        public void ThenIdOfTheUserNotificationCalledShouldBeSet(string notificationName)
        {
            UserNotification actual = this.scenarioContext.Get<UserNotification>(notificationName);
            Assert.IsNotNull(actual.Id);
        }

        [Then("the ETag of the user notification called '(.*)' should be set")]
        public void ThenETagOfTheUserNotificationCalledShouldBeSet(string notificationName)
        {
            UserNotification actual = this.scenarioContext.Get<UserNotification>(notificationName);
            Assert.IsNotNull(actual.Metadata.ETag);
        }

        [Then("the first (.*) notifications stored in the transient tenant for the user with Id '(.*)' have the delivery status '(.*)' for the delivery channel with Id '(.*)'")]
        public async Task ThenTheFirstNotificationsStoredInTheTransientTenantForTheUserWithIdHaveTheDeliveryStatusForTheDeliveryChannelWithId(
            int count,
            string userId,
            UserNotificationDeliveryStatus expectedDeliveryStatus,
            string deliveryChannelId)
        {
            GetNotificationsResult userNotifications = await this.GetNotificationsForUserAsync(userId, count).ConfigureAwait(false);

            foreach (UserNotification current in userNotifications.Results)
            {
                Assert.AreEqual(expectedDeliveryStatus, current.GetDeliveryStatusForChannel(deliveryChannelId));
            }
        }

        [Then("within (.*) seconds, the first (.*) notifications stored in the transient tenant for the user with Id '(.*)' have the delivery status '(.*)' for the delivery channel with Id '(.*)'")]
        public Task ThenWithinSecondsTheFirstNotificationsStoredInTheTransientTenantForTheUserWithIdHaveTheDeliveryStatusForTheDeliveryChannelWithId(
            int timeoutSeconds,
            int count,
            string userId,
            UserNotificationDeliveryStatus expectedDeliveryStatus,
            string deliveryChannelId)
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

            return Retriable.RetryAsync(
                async () =>
                {
                    GetNotificationsResult userNotifications = await this.GetNotificationsForUserAsync(userId, count).ConfigureAwait(false);

                    foreach (UserNotification current in userNotifications.Results)
                    {
                        if (current.GetDeliveryStatusForChannel(deliveryChannelId) != expectedDeliveryStatus)
                        {
                            throw new Exception($"Notification with Id '{current.Id}' has delivery status '{current.GetDeliveryStatusForChannel(deliveryChannelId)}' instead of expected value '{expectedDeliveryStatus}'");
                        }
                    }
                },
                tokenSource.Token,
                new Linear(TimeSpan.FromSeconds(5), int.MaxValue),
                new AnyExceptionPolicy(),
                false);
        }

        [Then("the first (.*) notifications stored in the transient tenant for the user with Id '(.*)' have the delivery status last updated set to within (.*) seconds of now for the delivery channel with Id '(.*)'")]
        public async Task ThenTheFirstNotificationsStoredInTheTransientTenantForTheUserWithIdHaveTheDeliveryStatusLastUpdatedSetToWithinSecondsOfNow(
            int count,
            string userId,
            int allowableTimeRangeFromNow,
            string deliveryChannelId)
        {
            GetNotificationsResult userNotifications = await this.GetNotificationsForUserAsync(userId, count).ConfigureAwait(false);

            var timeRange = TimeSpan.FromSeconds(allowableTimeRangeFromNow);

            foreach (UserNotification current in userNotifications.Results)
            {
                UserNotificationStatus status = current.ChannelStatuses.Single(x => x.DeliveryChannelId == deliveryChannelId);
                Assert.LessOrEqual(DateTimeOffset.UtcNow - status.DeliveryStatusLastUpdated, timeRange);
            }
        }

        [Then("the first (.*) notifications stored in the transient tenant for the user with Id '(.*)' have the read status '(.*)' for the delivery channel with Id '(.*)'")]
        public async Task ThenTheFirstNotificationsStoredInTheTransientTenantForTheUserWithIdHaveTheDeliveryStatusForTheDeliveryChannelWithId(
            int count,
            string userId,
            UserNotificationReadStatus expectedReadStatus,
            string deliveryChannelId)
        {
            GetNotificationsResult userNotifications = await this.GetNotificationsForUserAsync(userId, count).ConfigureAwait(false);

            foreach (UserNotification current in userNotifications.Results)
            {
                Assert.AreEqual(expectedReadStatus, current.GetReadStatusForChannel(deliveryChannelId));
            }
        }

        [Then("the first (.*) notifications stored in the transient tenant for the user with Id '(.*)' have the read status last updated set to within (.*) seconds of now for the delivery channel with Id '(.*)'")]
        public async Task ThenTheFirstNotificationsStoredInTheTransientTenantForTheUserWithIdHaveTheReadStatusLastUpdatedSetToWithinSecondsOfNow(
            int count,
            string userId,
            int allowableTimeRangeFromNow,
            string deliveryChannelId)
        {
            GetNotificationsResult userNotifications = await this.GetNotificationsForUserAsync(userId, count).ConfigureAwait(false);

            var timeRange = TimeSpan.FromSeconds(allowableTimeRangeFromNow);

            foreach (UserNotification current in userNotifications.Results)
            {
                UserNotificationStatus status = current.ChannelStatuses.Single(x => x.DeliveryChannelId == deliveryChannelId);
                Assert.LessOrEqual(DateTimeOffset.UtcNow - status.ReadStatusLastUpdated, timeRange);
            }
        }

        private async Task<GetNotificationsResult> GetNotificationsForUserAsync(string userId, int count)
        {
            ITenantedUserNotificationStoreFactory storeFactory = this.serviceProvider.GetRequiredService<ITenantedUserNotificationStoreFactory>();
            IUserNotificationStore store = await storeFactory.GetUserNotificationStoreForTenantAsync(this.featureContext.GetTransientTenant()).ConfigureAwait(false);
            return await store.GetAsync(userId, null, count).ConfigureAwait(false);
        }
    }
}
