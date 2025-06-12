// <copyright file="NotificationStoreSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Corvus.Extensions;
    using Corvus.Extensions.Json;
    using Corvus.Json;
    using Corvus.Testing.SpecFlow;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Reqnroll;

    [Binding]
    public class NotificationStoreSteps
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ScenarioContext scenarioContext;
        private readonly IJsonSerializerSettingsProvider serializationSettingsProvider;

        public NotificationStoreSteps(ScenarioContext scenarioContext)
        {
            this.serviceProvider = ContainerBindings.GetServiceProvider(scenarioContext);
            this.scenarioContext = scenarioContext;
            this.serializationSettingsProvider = this.serviceProvider.GetRequiredService<IJsonSerializerSettingsProvider>();
        }

        [Given("I have a user notification called '(.*)'")]
        public void GivenIHaveAUserNotificationCalled(string notificationName, Table table)
        {
            UserNotification notification = DataSetupSteps.BuildNotificationFrom(table.Rows[0], this.serializationSettingsProvider.Instance);
            this.scenarioContext.Set(notification, notificationName);
        }

        [Given("I have user notifications")]
        public void GivenIHaveUserNotifications(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                UserNotification notification = DataSetupSteps.BuildNotificationFrom(row, this.serializationSettingsProvider.Instance);
                this.scenarioContext.Set(notification, row["Name"]);
            }
        }

        [Given("the user notification called '(.*)' has the etag (.*)")]
        public void GivenTheUserNotificationCalledHasTheEtag_Z(string name, string etag)
        {
            UserNotification notification = this.scenarioContext.Get<UserNotification>(name);

            // Changing the etag of a notification is something we'd only want to do inside a test, so there isn't
            // a "WithEtag" method on the notification. As such, we need to construct the updated notification manually.
            var updatedNotification = new UserNotification(
                notification.Id,
                notification.NotificationType,
                notification.UserId,
                notification.Timestamp,
                notification.Properties,
                new UserNotificationMetadata(notification.Metadata.CorrelationIds, etag),
                notification.ChannelStatuses);

            this.scenarioContext.Set(updatedNotification, name);
        }

        [Given("I update the delivery status of the notification called '(.*)' to '(.*)' for the delivery channel '(.*)' and call it '(.*)'")]
        public void GivenIUpdateTheDeliveryStatusOfTheNotificationCalledToForTheDeliveryChannelAndCallIt(
            string originalName,
            UserNotificationDeliveryStatus deliveryStatus,
            string deliveryChannelId,
            string newName)
        {
            UserNotification notification = this.scenarioContext.Get<UserNotification>(originalName);
            UserNotification updatedNotification = notification.WithChannelDeliveryStatus(deliveryChannelId, deliveryStatus, DateTimeOffset.UtcNow);
            this.scenarioContext.Set(updatedNotification, newName);
        }

        [When("I tell the user notification store to store the user notification called '(.*)' and call the result '(.*)'")]
        public async Task WhenITellTheUserNotificationStoreToStoreTheUserNotificationCalledAndCallTheResult(string notificationName, string resultName)
        {
            IUserNotificationStore store = this.serviceProvider.GetRequiredService<IUserNotificationStore>();
            UserNotification notification = this.scenarioContext.Get<UserNotification>(notificationName);

            try
            {
                UserNotification result = await store.StoreAsync(notification).ConfigureAwait(false);

                this.scenarioContext.Set(result, resultName);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [Given("I have created and stored a notification for the user with Id '(.*)'")]
        public Task GivenIHaveCreatedAndStoredANotificationForTheUserWithId(string userId)
        {
            return this.GivenIHaveCreatedAndStoredNotificationsWithTimestampsAtSecondIntervalsForTheUserWithId(1, 0, userId);
        }

        [Given("I have created and stored (.*) notifications with timestamps at (.*) second intervals for the user with Id '(.*)'")]
        public Task GivenIHaveCreatedAndStoredNotificationsWithTimestampsAtSecondIntervalsForTheUserWithId(int notificationCount, int interval, string userId)
        {
            IUserNotificationStore store = this.serviceProvider.GetRequiredService<IUserNotificationStore>();
            IPropertyBagFactory propertyBagFactory = this.serviceProvider.GetRequiredService<IPropertyBagFactory>();

            DateTimeOffset timestamp = DateTimeOffset.UtcNow;
            var offset = TimeSpan.FromSeconds(interval);

            var tasks = new List<Task>();
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

            return Task.WhenAll(tasks);
        }

        [Given("I have told the user notification store to store the user notification called '(.*)' and call the result '(.*)'")]
        public async Task WhenITellTheUserNotificationStoreToStoreTheUserNotificationCalled(string notificationName, string resultName)
        {
            IUserNotificationStore store = this.serviceProvider.GetRequiredService<IUserNotificationStore>();
            UserNotification notification = this.scenarioContext.Get<UserNotification>(notificationName);
            UserNotification result = await store.StoreAsync(notification).ConfigureAwait(false);
            this.scenarioContext.Set(result, resultName);
        }

        [When("I ask the user notification store for the user notification with the same Id as the user notification called '(.*)' and call it '(.*)'")]
        public async Task WhenIAskTheUserNotificationStoreForTheUserNotificationWithTheSameIdAsTheUserNotificationCalledAndCallIt(string notificationName, string resultName)
        {
            IUserNotificationStore store = this.serviceProvider.GetRequiredService<IUserNotificationStore>();
            UserNotification notification = this.scenarioContext.Get<UserNotification>(notificationName);
            UserNotification result = await store.GetByIdAsync(notification.Id!).ConfigureAwait(false);
            this.scenarioContext.Set(result, resultName);
        }

        [Given("I have asked the user notification store for (.*) notifications for the user with Id '(.*)' and called the result '(.*)'")]
        [When("I ask the user notification store for (.*) notifications for the user with Id '(.*)' and call the result '(.*)'")]
        public async Task WhenIAskTheUserNotificationStoreForNotificationsForTheUserWithId(int itemCount, string userId, string resultName)
        {
            IUserNotificationStore store = this.serviceProvider.GetRequiredService<IUserNotificationStore>();
            GetNotificationsResult result = await store.GetAsync(userId, null, itemCount).ConfigureAwait(false);
            this.scenarioContext.Set(result, resultName);
        }

        [When("I ask the user notification store for (.*) notifications since the first notification in the results called '(.*)' for the user with Id '(.*)' and call the result '(.*)'")]
        public async Task WhenIAskTheUserNotificationStoreForNotificationsSinceTheFirstNotificationInTheResultsCalledForTheUserWithIdAndCallTheResult(int itemCount, string previousResultName, string userId, string newResultName)
        {
            IUserNotificationStore store = this.serviceProvider.GetRequiredService<IUserNotificationStore>();
            GetNotificationsResult previousResult = this.scenarioContext.Get<GetNotificationsResult>(previousResultName);
            GetNotificationsResult result = await store.GetAsync(userId, previousResult.Results[0].Id, itemCount).ConfigureAwait(false);
            this.scenarioContext.Set(result, newResultName);
        }

        [Given("I have asked the user notification store for notifications for the user with Id '(.*)' using the continuation token from the result called '(.*)' and call the result '(.*)'")]
        [When("I ask the user notification store for notifications for the user with Id '(.*)' using the continuation token from the result called '(.*)' and call the result '(.*)'")]
        public async Task WhenIAskTheUserNotificationStoreForNotificationsUsingTheContinuationTokenFromTheResultCalledAndCallTheResult(string userId, string previousResultName, string newResultName)
        {
            IUserNotificationStore store = this.serviceProvider.GetRequiredService<IUserNotificationStore>();
            GetNotificationsResult previousResult = this.scenarioContext.Get<GetNotificationsResult>(previousResultName);
            GetNotificationsResult result = await store.GetAsync(userId, previousResult.ContinuationToken!).ConfigureAwait(false);
            this.scenarioContext.Set(result, newResultName);
        }

        [Then("the get notifications result called '(.*)' should contain (.*) notifications")]
        public void ThenTheGetNotificationsResultShouldContainNotifications(string resultName, int expectedNotificationCount)
        {
            GetNotificationsResult result = this.scenarioContext.Get<GetNotificationsResult>(resultName);
            Assert.AreEqual(expectedNotificationCount, result.Results.Length);
        }

        [Then("the get notifications results called '(.*)' and '(.*)' should not contain any of the same notifications")]
        public void ThenTheGetNotificationsResultsCalledAndShouldNotContainAnyOfTheSameNotifications(string resultsName1, string resultsName2)
        {
            GetNotificationsResult results1 = this.scenarioContext.Get<GetNotificationsResult>(resultsName1);
            GetNotificationsResult results2 = this.scenarioContext.Get<GetNotificationsResult>(resultsName2);

            int overlapCount = results1.Results.Count(r1 => results2.Results.Any(r2 => r2.Id == r1.Id));
            Assert.AreEqual(0, overlapCount);
        }

        [Then("the get notifications result called '(.*)' should only contain notifications with an earlier timestamp than those in the get notifications result '(.*)'")]
        public void ThenTheGetNotificationsResultCalledShouldOnlyContainNotificationsWithAnEarlierTimestampThanThoseInTheGetNotificationsResult(string earlierResultsName, string laterResultsName)
        {
            GetNotificationsResult earlierResults = this.scenarioContext.Get<GetNotificationsResult>(earlierResultsName);
            GetNotificationsResult laterResults = this.scenarioContext.Get<GetNotificationsResult>(laterResultsName);

            int overlapCount = earlierResults.Results.Count(e => laterResults.Results.Any(l => l.Timestamp < e.Timestamp));
            Assert.AreEqual(0, overlapCount);
        }

        [Then("the get notifications result called '(.*)' should contain notifications in descending order of timestamp")]
        public void ThenTheGetNotificationsResultCalledShouldContainNotificationsInDescendingOrderOfTimestamp(string resultName)
        {
            GetNotificationsResult result = this.scenarioContext.Get<GetNotificationsResult>(resultName);
            result.Results.ForEachAtIndex((n, i) =>
            {
                if (i != 0)
                {
                    Assert.GreaterOrEqual(result.Results[i - 1].Timestamp, n.Timestamp);
                }
            });
        }

        [Then("the get notifications result called '(.*)' should contain a continuation token")]
        public void ThenTheGetNotificationsResultShouldContainAContinuationToken(string resultName)
        {
            GetNotificationsResult result = this.scenarioContext.Get<GetNotificationsResult>(resultName);
            Assert.IsNotNull(result.ContinuationToken);
            Assert.IsNotEmpty(result.ContinuationToken);
        }

        [Then("the get notifications result called '(.*)' should not contain a continuation token")]
        public void ThenTheGetNotificationsResultCalledShouldNotContainAContinuationToken(string resultName)
        {
            GetNotificationsResult result = this.scenarioContext.Get<GetNotificationsResult>(resultName);
            Assert.IsNull(result.ContinuationToken);
        }

        [Then("the notification called '(.*)' should be the same as the notification called '(.*)'")]
        public void ThenTheNotificationCalledShouldBeTheSameAsTheNotificationCalled(string expectedName, string actualName)
        {
            UserNotification expected = this.scenarioContext.Get<UserNotification>(expectedName);
            UserNotification actual = this.scenarioContext.Get<UserNotification>(actualName);

            NotificationSteps.AssertUserNotificationsMatch(expected, actual, this.serializationSettingsProvider);
        }
    }
}