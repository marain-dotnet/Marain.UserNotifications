// <copyright file="NotificationStoreSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Steps
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Testing.SpecFlow;
    using Microsoft.Extensions.DependencyInjection;
    using TechTalk.SpecFlow;

    [Binding]
    public class NotificationStoreSteps
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ScenarioContext scenarioContext;

        public NotificationStoreSteps(ScenarioContext scenarioContext)
        {
            this.serviceProvider = ContainerBindings.GetServiceProvider(scenarioContext);
            this.scenarioContext = scenarioContext;
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

        [Given("I have told the user notification store to store the user notification called '(.*)'")]
        public Task WhenITellTheUserNotificationStoreToStoreTheUserNotificationCalled(string notificationName)
        {
            IUserNotificationStore store = this.serviceProvider.GetRequiredService<IUserNotificationStore>();
            UserNotification notification = this.scenarioContext.Get<UserNotification>(notificationName);
            return store.StoreAsync(notification);
        }
    }
}
