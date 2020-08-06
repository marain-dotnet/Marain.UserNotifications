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

        [Then("the properties of the notification called '(.*)' should match the notification called '(.*)'")]
        public void ThenThePropertiesOfTheNotificationCalledShouldMatchTheNotificationCalled(string actualNotificationName, string expectedNotificationName)
        {
            Notification actual = this.scenarioContext.Get<Notification>(actualNotificationName);
            Notification expected = this.scenarioContext.Get<Notification>(expectedNotificationName);

            Assert.AreEqual(expected.CorrelationIds, actual.CorrelationIds);
            Assert.AreEqual(expected.NotificationType, actual.NotificationType);
            Assert.AreEqual(expected.Timestamp, actual.Timestamp);
            Assert.AreEqual(expected.UserId, actual.UserId);

            // Easiest way to compare property bags is to serialize them both.
            string serializedActualProperties = JsonConvert.SerializeObject(actual.Properties, this.serializationSettingsProvider.Instance);
            string serializedExpectedProperties = JsonConvert.SerializeObject(expected.Properties, this.serializationSettingsProvider.Instance);

            Assert.AreEqual(serializedExpectedProperties, serializedActualProperties);
        }

        [Then("the Id of the notification called '(.*)' should be set")]
        public void ThenIdOfTheNotificationCalledShouldBeSet(string notificationName)
        {
            Notification actual = this.scenarioContext.Get<Notification>(notificationName);
            Assert.IsNotNull(actual.NotificationId);
        }
    }
}
