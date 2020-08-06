// <copyright file="ObjectCreationSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Linq;
    using Corvus.Extensions.Json;
    using Corvus.Json;
    using Corvus.Testing.SpecFlow;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using TechTalk.SpecFlow;

    [Binding]
    public class ObjectCreationSteps
    {
        private readonly ScenarioContext scenarioContext;

        private readonly IServiceProvider serviceProvider;

        private readonly IJsonSerializerSettingsProvider serializationSettingsProvider;

        public ObjectCreationSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
            this.serviceProvider = ContainerBindings.GetServiceProvider(scenarioContext);
            this.serializationSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();
        }

        [Given("I have a user notification called '(.*)'")]
        public void GivenIHaveAUserNotificationCalled(string notificationName, Table table)
        {
            UserNotification notification = this.BuildNotificationFrom(table.Rows[0]);
            this.scenarioContext.Set(notification, notificationName);
        }

        [Given("I have user notifications")]
        public void GivenIHaveUserNotifications(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                UserNotification notification = this.BuildNotificationFrom(row);
                this.scenarioContext.Set(notification, row["Name"]);
            }
        }

        private UserNotification BuildNotificationFrom(TableRow tableRow)
        {
            string[] correlationIds = JArray.Parse(tableRow["CorrelationIds"]).Select(token => token.Value<string>()).ToArray();
            IPropertyBag properties = JsonConvert.DeserializeObject<IPropertyBag>(tableRow["PropertiesJson"], this.serializationSettingsProvider.Instance);

            string? notificationId = tableRow.ContainsKey("Id") ? tableRow["Id"] : null;

            return new UserNotification(
                notificationId,
                tableRow["NotificationType"],
                tableRow["UserId"],
                DateTime.Parse(tableRow["Timestamp"]),
                properties,
                new UserNotificationMetadata(correlationIds, null));
        }
    }
}
