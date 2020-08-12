// <copyright file="DataSetupSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Corvus.Json;
    using Corvus.Testing.SpecFlow;
    using Marain.UserNotifications.Specs.Bindings;
    using Microsoft.Extensions.DependencyInjection;
    using TechTalk.SpecFlow;

    [Binding]
    public class DataSetupSteps
    {
        private readonly IServiceProvider serviceProvider;
        private readonly FeatureContext featureContext;

        public DataSetupSteps(FeatureContext featureContext)
        {
            this.featureContext = featureContext;
            this.serviceProvider = ContainerBindings.GetServiceProvider(featureContext);
        }

        [Given("I have created and stored (.*) notifications in the current transient tenant with timestamps at (.*) second intervals for the user with Id '(.*)'")]
        public async Task GivenIHaveCreatedAndStoredNotificationsInTheCurrentTransientTenantWithTimestampsAtSecondIntervalsForTheUserWithId(int notificationCount, int interval, string userId)
        {
            ITenantedUserNotificationStoreFactory storeFactory = this.serviceProvider.GetRequiredService<ITenantedUserNotificationStoreFactory>();
            IUserNotificationStore store = await storeFactory.GetUserNotificationStoreForTenantAsync(this.featureContext.GetTransientTenant()).ConfigureAwait(false);
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

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}
