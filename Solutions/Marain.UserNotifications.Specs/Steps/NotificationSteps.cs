// <copyright file="NotificationSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Linq;
    using Corvus.Extensions.Json;
    using Corvus.Testing.SpecFlow;
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

        public NotificationSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
            this.serviceProvider = ContainerBindings.GetServiceProvider(scenarioContext);
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

            Assert.AreEqual(expected.ChannelDeliveryStatuses.Count(), actual.ChannelDeliveryStatuses.Count());
            foreach (UserNotificationStatus expectedStatus in expected.ChannelDeliveryStatuses)
            {
                UserNotificationStatus? actualStatus = actual.ChannelDeliveryStatuses.FirstOrDefault(s => s.DeliveryChannelId == expectedStatus.DeliveryChannelId);

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
    }
}
