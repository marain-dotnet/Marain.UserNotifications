﻿// <copyright file="DataSetupSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
    using Corvus.Json;
    using Corvus.Testing.SpecFlow;
    using Marain.Models;
    using Marain.NotificationTemplates;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Marain.UserNotifications.Specs.Bindings;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Reqnroll;

    [Binding]
    public class DataSetupSteps
    {
        public const string CreatedNotificationsKey = "CreatedNotificationsKey";

        private readonly IServiceProvider serviceProvider;
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;

        public DataSetupSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
            this.serviceProvider = ContainerBindings.GetServiceProvider(featureContext);
        }

        public static UserNotification BuildNotificationFrom(DataTableRow tableRow, JsonSerializerSettings serializerSettings)
        {
            string[] correlationIds = JArray.Parse(tableRow["CorrelationIds"]).Select(token => token.Value<string>()).ToArray()!;
            IPropertyBag properties = JsonConvert.DeserializeObject<IPropertyBag>(tableRow["PropertiesJson"], serializerSettings)!;

            string? notificationId = tableRow.ContainsKey("Id") ? tableRow["Id"] : null;

            return new UserNotification(
                notificationId,
                tableRow["NotificationType"],
                tableRow["UserId"],
                DateTime.Parse(tableRow["Timestamp"]),
                properties,
                new UserNotificationMetadata(correlationIds, null));
        }

        public static WebPushTemplate BuildWebPushNotificationTemplateFrom(DataTableRow tableRow, string? eTag = null)
        {
            return new WebPushTemplate(
                tableRow["notificationType"],
                tableRow["title"],
                tableRow["body"],
                tableRow["image"],
                tableRow["actionUrl"],
                eTag);
        }

        public static EmailTemplate BuildEmailNotificationTemplateFrom(DataTableRow tableRow, string? eTag = null)
        {
            return new EmailTemplate(
                tableRow["notificationType"],
                tableRow["subject"],
                tableRow["body"],
                eTag);
        }

        public static SmsTemplate BuildSmsNotificationTemplateFrom(DataTableRow tableRow, string? eTag = null)
        {
            return new SmsTemplate(
                tableRow["notificationType"],
                tableRow["body"],
                tableRow.ContainsKey("eTag") ? tableRow["eTag"] : eTag);
        }

        public static NotificationTemplate BuildNotificationTemplateFrom(DataTableRow tableRow, JsonSerializerSettings serializerSettings)
        {
            SmsTemplate? deserialisedSms = tableRow.ContainsKey("smsTemplate") ? JsonConvert.DeserializeObject<SmsTemplate>(tableRow["smsTemplate"], serializerSettings) : null;
            EmailTemplate? deserialisedEmail = tableRow.ContainsKey("emailTemplate") ? JsonConvert.DeserializeObject<EmailTemplate>(tableRow["emailTemplate"], serializerSettings) : null;
            WebPushTemplate? deserialisedWebPush = tableRow.ContainsKey("webPushTemplate") ? JsonConvert.DeserializeObject<WebPushTemplate>(tableRow["webPushTemplate"], serializerSettings) : null;

            return new NotificationTemplate(
                tableRow["notificationType"],
                deserialisedSms,
                deserialisedEmail,
                deserialisedWebPush);
        }

        [Given("I have created and stored a notification in the current transient tenant for the user with Id '(.*)'")]
        public Task GivenIHaveCreatedAndStoredANotificationInTheCurrentTransientTenantForTheUserWithId(string userId)
        {
            return this.GivenIHaveCreatedAndStoredNotificationsInTheCurrentTransientTenantWithTimestampsAtSecondIntervalsForTheUserWithId(1, 0, userId);
        }

        [Given("I have created and stored (.*) notifications in the current transient tenant with timestamps at (.*) second intervals for the user with Id '(.*)'")]
        public async Task GivenIHaveCreatedAndStoredNotificationsInTheCurrentTransientTenantWithTimestampsAtSecondIntervalsForTheUserWithId(int notificationCount, int interval, string userId)
        {
            ITenantedUserNotificationStoreFactory storeFactory = this.serviceProvider.GetRequiredService<ITenantedUserNotificationStoreFactory>();
            IUserNotificationStore store = await storeFactory.GetUserNotificationStoreForTenantAsync(this.featureContext.GetTransientTenant()).ConfigureAwait(false);
            IPropertyBagFactory propertyBagFactory = this.serviceProvider.GetRequiredService<IPropertyBagFactory>();

            var offset = TimeSpan.FromSeconds(interval);
            DateTimeOffset timestamp = DateTimeOffset.UtcNow - offset;

            var tasks = new List<Task<UserNotification>>();
            var propertiesDictionary = new Dictionary<string, object>
            {
                { "prop1", "val1" },
                { "prop2", 2 },
                { "prop3", DateTime.Now },
            };

            IPropertyBag properties = propertyBagFactory.Create(propertiesDictionary);

            for (int i = 0; i < notificationCount; i++)
            {
                string[] correlationIds = Enumerable.Range(0, 3).Select(_ => Guid.NewGuid().ToString()).ToArray();
                var metadata = new UserNotificationMetadata(correlationIds, null);
                tasks.Add(store.StoreAsync(new UserNotification(null, "marain.usernotifications.test", userId, timestamp, properties, metadata)));
                timestamp -= offset;
            }

            UserNotification[] newlyCreatedNotifications = await Task.WhenAll(tasks).ConfigureAwait(false);

            // Store the notifications in session state
            if (!this.scenarioContext.TryGetValue(CreatedNotificationsKey, out List<UserNotification> createdNotifications))
            {
                createdNotifications = new List<UserNotification>();
                this.scenarioContext.Set(createdNotifications, CreatedNotificationsKey);
            }

            createdNotifications.AddRange(newlyCreatedNotifications);
        }

        [Given("I have created and stored a notification in the current transient tenant and called the result '(.*)'")]
        public async Task GivenIHaveCreatedAndStoredANotificationInTheCurrentTransientTenantAndCalledTheResult(string resultName, Table table)
        {
            ITenantedUserNotificationStoreFactory storeFactory = this.serviceProvider.GetRequiredService<ITenantedUserNotificationStoreFactory>();
            IJsonSerializerSettingsProvider serializerSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();
            UserNotification notification = BuildNotificationFrom(table.Rows[0], serializerSettingsProvider.Instance);
            IUserNotificationStore store = await storeFactory.GetUserNotificationStoreForTenantAsync(this.featureContext.GetTransientTenant()).ConfigureAwait(false);

            UserNotification result = await store.StoreAsync(notification).ConfigureAwait(false);

            this.scenarioContext.Set(result, resultName);
        }

        [Given("I have created and stored a notification template")]
        public async Task GivenIHaveCreatedAndStoredANotificationTemplate(Table table)
        {
            ITenantedNotificationTemplateStoreFactory storeFactory = this.serviceProvider.GetRequiredService<ITenantedNotificationTemplateStoreFactory>();
            IJsonSerializerSettingsProvider serializerSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();

            NotificationTemplate notificationTemplate = BuildNotificationTemplateFrom(table.Rows[0], serializerSettingsProvider.Instance);

            INotificationTemplateStore store = await storeFactory.GetTemplateStoreForTenantAsync(this.featureContext.GetTransientTenant()).ConfigureAwait(false);
            EmailTemplate? result = await store.CreateOrUpdate("testshouldbreak", CommunicationType.Email, null, notificationTemplate.EmailTemplate!).ConfigureAwait(false);

            this.scenarioContext.Set(result);
        }

        [Given("I have created and stored a web push notification template")]
        public async Task GivenIHaveCreatedAndStoredAWebPushNotificationTemplate(Table table)
        {
            ITenantedNotificationTemplateStoreFactory storeFactory = this.serviceProvider.GetRequiredService<ITenantedNotificationTemplateStoreFactory>();
            WebPushTemplate notificationTemplate = BuildWebPushNotificationTemplateFrom(table.Rows[0]);

            INotificationTemplateStore? store = await storeFactory.GetTemplateStoreForTenantAsync(this.featureContext.GetTransientTenant()).ConfigureAwait(false);
            await store.CreateOrUpdate(notificationTemplate.NotificationType!, CommunicationType.WebPush, notificationTemplate.ETag, notificationTemplate).ConfigureAwait(false);
            (WebPushTemplate, string?) webPushTemplate = await store.GetAsync<WebPushTemplate>(notificationTemplate.NotificationType!, CommunicationType.WebPush).ConfigureAwait(false);
            webPushTemplate.Item1.ETag = webPushTemplate.Item2;
            this.scenarioContext.Set(webPushTemplate.Item1);
        }

        [Given("I have created and stored an email notification template")]
        public async Task GivenIHaveCreatedAndStoredAnEmailNotificationTemplate(Table table)
        {
            ITenantedNotificationTemplateStoreFactory storeFactory = this.serviceProvider.GetRequiredService<ITenantedNotificationTemplateStoreFactory>();
            EmailTemplate notificationTemplate = BuildEmailNotificationTemplateFrom(table.Rows[0]);

            INotificationTemplateStore? store = await storeFactory.GetTemplateStoreForTenantAsync(this.featureContext.GetTransientTenant()).ConfigureAwait(false);
            await store.CreateOrUpdate(notificationTemplate.NotificationType!, CommunicationType.Email, notificationTemplate.ETag, notificationTemplate).ConfigureAwait(false);
            (EmailTemplate, string?) emailTemplate = await store.GetAsync<EmailTemplate>(notificationTemplate.NotificationType!, CommunicationType.Email).ConfigureAwait(false);
            emailTemplate.Item1.ETag = emailTemplate.Item2;
            this.scenarioContext.Set(emailTemplate.Item1);
        }

        [Given("I have created and stored an sms notification template")]
        public async Task GivenIHaveCreatedAndStoredAnSmsNotificationTemplate(Table table)
        {
            ITenantedNotificationTemplateStoreFactory storeFactory = this.serviceProvider.GetRequiredService<ITenantedNotificationTemplateStoreFactory>();
            SmsTemplate notificationTemplate = BuildSmsNotificationTemplateFrom(table.Rows[0]);

            INotificationTemplateStore? store = await storeFactory.GetTemplateStoreForTenantAsync(this.featureContext.GetTransientTenant()).ConfigureAwait(false);
            await store.CreateOrUpdate(notificationTemplate.NotificationType!, CommunicationType.Sms, notificationTemplate.ETag, notificationTemplate).ConfigureAwait(false);
            (SmsTemplate, string?) smsTemplate = await store.GetAsync<SmsTemplate>(notificationTemplate.NotificationType!, CommunicationType.Sms).ConfigureAwait(false);
            smsTemplate.Item1.ETag = smsTemplate.Item2;
            this.scenarioContext.Set(smsTemplate.Item1);
        }

        [Given("I have created and stored a sms notification template")]
        public async Task GivenIHaveCreatedAndStoredASmsNotificationTemplate(Table table)
        {
            ITenantedNotificationTemplateStoreFactory storeFactory = this.serviceProvider.GetRequiredService<ITenantedNotificationTemplateStoreFactory>();
            SmsTemplate notificationTemplate = BuildSmsNotificationTemplateFrom(table.Rows[0]);

            INotificationTemplateStore? store = await storeFactory.GetTemplateStoreForTenantAsync(this.featureContext.GetTransientTenant()).ConfigureAwait(false);
            await store.CreateOrUpdate(notificationTemplate.NotificationType!, CommunicationType.Sms, notificationTemplate.ETag, notificationTemplate).ConfigureAwait(false);
            (SmsTemplate, string?) smsTemplate = await store.GetAsync<SmsTemplate>(notificationTemplate.NotificationType!, CommunicationType.Sms).ConfigureAwait(false);
            smsTemplate.Item1.ETag = smsTemplate.Item2;
            this.scenarioContext.Set(smsTemplate.Item1);
        }
    }
}