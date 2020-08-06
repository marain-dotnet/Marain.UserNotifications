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

        [When("I tell the notification store to store the notification called '(.*)' and call the result '(.*)'")]
        public async Task WhenITellTheNotificationStoreToStoreTheNotificationCalledAndCallTheResult(string notificationName, string resultName)
        {
            INotificationStore store = this.serviceProvider.GetRequiredService<INotificationStore>();
            Notification notification = this.scenarioContext.Get<Notification>(notificationName);

            try
            {
                Notification result = await store.StoreAsync(notification).ConfigureAwait(false);

                this.scenarioContext.Set(result, resultName);
            }
            catch (Exception ex)
            {
                ExceptionSteps.StoreLastExceptionInScenarioContext(ex, this.scenarioContext);
            }
        }

        [Given("I have told the notification store to store the notification called '(.*)'")]
        public Task WhenITellTheNotificationStoreToStoreTheNotificationCalled(string notificationName)
        {
            INotificationStore store = this.serviceProvider.GetRequiredService<INotificationStore>();
            Notification notification = this.scenarioContext.Get<Notification>(notificationName);
            return store.StoreAsync(notification);
        }
    }
}
