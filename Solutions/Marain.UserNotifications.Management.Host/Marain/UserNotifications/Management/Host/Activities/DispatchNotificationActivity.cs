// <copyright file="DispatchNotificationActivity.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.DeliveryChannelConfiguration;
    using Marain.Models;
    using Marain.NotificationTemplates;
    using Marain.UserNotifications.Management.Host.Composer;
    using Marain.UserNotifications.Management.Host.Helpers;
    using Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship;
    using Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models;
    using Marain.UserNotifications.ThirdParty.DeliveryChannels.KeyVaultSecretModels;
    using Marain.UserPreferences;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Dispatches a notification with all the configured delivery channels for the incoming notification type.
    /// </summary>
    public class DispatchNotificationActivity
    {
        private readonly ITenantProvider tenantProvider;
        private readonly ITenantedUserNotificationStoreFactory notificationStoreFactory;
        private readonly ITenantedUserPreferencesStoreFactory tenantedUserPreferencesStoreFactory;
        private readonly ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory;
        private readonly IGenerateTemplateComposer generateTemplateComposer;
        private readonly IAirshipClientFactory airshipClientFactory;
        private readonly ITenantedDeliveryChannelConfigurationStoreFactory tenantedDeliveryChannelConfigurationStoreFactory;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchNotificationActivity"/> class.
        /// </summary>
        /// <param name="tenantProvider">The tenant provider.</param>
        /// <param name="notificationStoreFactory">The factory for the notification store.</param>
        /// <param name="tenantedUserPreferencesStoreFactory">The factory for the user preference store.</param>
        /// <param name="tenantedTemplateStoreFactory">The factory for the templated store.</param>
        /// <param name="tenantedDeliveryChannelConfigurationStoreFactory">The factory for delivery channel configuration store.</param>
        /// <param name="generateTemplateComposer">The composer to generate the templated notification per communication channel.</param>
        /// <param name="airshipClientFactory">The Airship Factory.</param>
        /// <param name="configuration">IConfiguration.</param>
        public DispatchNotificationActivity(
            ITenantProvider tenantProvider,
            ITenantedUserNotificationStoreFactory notificationStoreFactory,
            ITenantedUserPreferencesStoreFactory tenantedUserPreferencesStoreFactory,
            ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory,
            ITenantedDeliveryChannelConfigurationStoreFactory tenantedDeliveryChannelConfigurationStoreFactory,
            IGenerateTemplateComposer generateTemplateComposer,
            IAirshipClientFactory airshipClientFactory,
            IConfiguration configuration)
        {
            this.tenantProvider = tenantProvider
                ?? throw new ArgumentNullException(nameof(tenantProvider));
            this.notificationStoreFactory = notificationStoreFactory
                ?? throw new ArgumentNullException(nameof(notificationStoreFactory));
            this.tenantedUserPreferencesStoreFactory = tenantedUserPreferencesStoreFactory
                ?? throw new ArgumentNullException(nameof(tenantedUserPreferencesStoreFactory));
            this.tenantedTemplateStoreFactory = tenantedTemplateStoreFactory
                ?? throw new ArgumentNullException(nameof(tenantedTemplateStoreFactory));
            this.generateTemplateComposer = generateTemplateComposer
                ?? throw new ArgumentNullException(nameof(generateTemplateComposer));
            this.airshipClientFactory = airshipClientFactory
                ?? throw new ArgumentNullException(nameof(airshipClientFactory));
            this.tenantedDeliveryChannelConfigurationStoreFactory = tenantedDeliveryChannelConfigurationStoreFactory
                ?? throw new ArgumentNullException(nameof(tenantedDeliveryChannelConfigurationStoreFactory));
            this.configuration = configuration
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Executes the activity.
        /// </summary>
        /// <param name="context">The activity context.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName(nameof(DispatchNotificationActivity))]
        public async Task ExecuteAsync(
            [ActivityTrigger] IDurableActivityContext context,
            ILogger logger)
        {
            TenantedFunctionData<UserNotification> request = context.GetInput<TenantedFunctionData<UserNotification>>();

            ITenant tenant = await this.tenantProvider.GetTenantAsync(request.TenantId).ConfigureAwait(false);

            logger.LogInformation(
                "Executing DispatchNotificationActivity for notification of type {notificationType} for user {userId}",
                request.Payload.NotificationType,
                request.Payload.UserId);

            // Get the UserPreferencesStore for the tenant
            IUserPreferencesStore userPreferencesStore = await this.tenantedUserPreferencesStoreFactory.GetUserPreferencesStoreForTenantAsync(tenant).ConfigureAwait(false);

            // Get the user preference for the userId
            UserPreference? userPreference = await userPreferencesStore.GetAsync(request.Payload.UserId).ConfigureAwait(false);

            // Check if the user has set the communication channels for the incoming notification type
            if (userPreference is null)
            {
                logger.LogError($"There is no user preference set up for this user {request.Payload.UserId} for tenant {tenant.Id}");
                return;
            }

            if (userPreference.CommunicationChannelsPerNotificationConfiguration is null)
            {
                logger.LogError($"There are no communication channel set up for the user {request.Payload.UserId} for tenant {tenant.Id}");
                return;
            }

            if (!userPreference.CommunicationChannelsPerNotificationConfiguration.ContainsKey(request.Payload.NotificationType))
            {
                logger.LogError($"There is no communication channel set up for the user {request.Payload.UserId} for notification type {request.Payload.NotificationType} for tenant {tenant.Id}");
            }

            List<CommunicationType>? registeredCommunicationChannels = userPreference.CommunicationChannelsPerNotificationConfiguration[request.Payload.NotificationType];

            if (registeredCommunicationChannels is null || registeredCommunicationChannels.Count == 0)
            {
                throw new Exception($"There are no communication channel set up for the user {request.Payload.UserId} for notification type {request.Payload.NotificationType} for tenant {tenant.Id}");
            }

            // Gets the AzureBlobTemplateStore
            INotificationTemplateStore templateStore = await this.tenantedTemplateStoreFactory.GetTemplateStoreForTenantAsync(tenant).ConfigureAwait(false);
            NotificationTemplate? notificationTemplate = await this.generateTemplateComposer.GenerateTemplateAsync(templateStore, request.Payload.Properties, registeredCommunicationChannels, request.Payload.NotificationType).ConfigureAwait(false);

            // UserId will be a combination of tenantId and userId of that business.
            // TODO: We still need to think about this.
            string airshipUserId = $"{tenant.Id}:{request.Payload.UserId}";

            IDeliveryChannelConfigurationStore? deliveryChannelConfigurationStore = await this.tenantedDeliveryChannelConfigurationStoreFactory.GetDeliveryChannelConfigurationStoreForTenantAsync(tenant).ConfigureAwait(false);
            DeliveryChannelConfiguration? deliveryChannelConfiguration = await deliveryChannelConfigurationStore.GetAsync(tenant.Id).ConfigureAwait(false);

            if (deliveryChannelConfiguration is null)
            {
                throw new Exception($"There is no delivery channel configuration for tenant {tenant.Id}");
            }

            if (deliveryChannelConfiguration.DeliveryChannelConfiguredPerCommunicationType is null)
            {
                throw new Exception($"There is no delivery channel configuration per communication type setup for tenant {tenant.Id}");
            }

            // TODO: THINK ABOUT THIS. SHOLD BE GENERIC AND HANDLE ALL THE MUMBO JUMBO CONFIG THAT THE USER MIGHT HAVE CONFIGURED.
            string? airshipSecretsString = await KeyVaultHelper.GetDeliveryChannelSecretAsync(this.configuration, string.Empty).ConfigureAwait(false);

            if (airshipSecretsString is null)
            {
                throw new Exception($"There is no airship delivery channel configuration setup in the keyvault");
            }

            // Convert secret
            Airship airshipSecrets = JsonConvert.DeserializeObject<Airship>(airshipSecretsString);

            // TODO: get the delivery channel configuration for that tenant
            // Need to also set the delivery channel configuration before we try to get it for that tenant
            AirshipClient? airshipClient = this.airshipClientFactory.GetAirshipClient(airshipSecrets.ApplicationKey!, airshipSecrets.MasterSecret!);
            var newNotification = new Notification()
            {
                Alert = "Is this needed",
                Web = new WebAlert()
                {
                    Alert = "Client wrapper body",
                    Title = "Client wrapper title",
                    Image = new Image()
                    {
                        Url = "https://upload.wikimedia.org/wikipedia/commons/6/6e/Golde33443.jpg",
                    },
                    Buttons = new List<Button>()
                                {
                                    new Button()
                                    {
                                        Label = "Button One",
                                        Id = "button-one",
                                        Actions = new Actions()
                                        {
                                            Open = new OpenUrlAction()
                                            {
                                                Content = "https://www.google.com",
                                                Type = "url",
                                            },
                                        },
                                    },
                                    new Button()
                                    {
                                        Label = "Button Two",
                                        Id = "button-two",
                                        Actions = new Actions()
                                        {
                                            Open = new OpenUrlAction()
                                            {
                                                Content = "https://www.yahoo.com",
                                                Type = "url",
                                            },
                                        },
                                    },
                                },
                },
                Actions = new Actions()
                {
                    Open = new OpenUrlAction { Type = "url", Content = "https://www.google.com" },
                },
            };

            string? test = await airshipClient.PushNotification(airshipUserId, newNotification).ConfigureAwait(false);
        }
    }
}
