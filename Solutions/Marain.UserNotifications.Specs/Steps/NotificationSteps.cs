// <copyright file="NotificationSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
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

        [Then("the properties of the user notification called '(.*)' should match the user notification called '(.*)'")]
        public void ThenThePropertiesOfTheUserNotificationCalledShouldMatchTheUserNotificationCalled(string actualNotificationName, string expectedNotificationName)
        {
            UserNotification actual = this.scenarioContext.Get<UserNotification>(actualNotificationName);
            UserNotification expected = this.scenarioContext.Get<UserNotification>(expectedNotificationName);

            Assert.AreEqual(expected.Metadata.CorrelationIds, actual.Metadata.CorrelationIds);
            Assert.AreEqual(expected.NotificationType, actual.NotificationType);
            Assert.AreEqual(expected.Timestamp, actual.Timestamp);
            Assert.AreEqual(expected.UserId, actual.UserId);

            // Easiest way to compare property bags is to serialize them both.
            string serializedActualProperties = JsonConvert.SerializeObject(actual.Properties, this.serializationSettingsProvider.Instance);
            string serializedExpectedProperties = JsonConvert.SerializeObject(expected.Properties, this.serializationSettingsProvider.Instance);

            Assert.AreEqual(serializedExpectedProperties, serializedActualProperties);
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
