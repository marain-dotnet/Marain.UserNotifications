// <copyright file="DispatchNotificationActivity.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.Models;
    using Marain.NotificationTemplates;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Marain.UserNotifications.Management.Host.Composer;
    using Marain.UserNotifications.Management.Host.Helpers;
    using Marain.UserNotifications.Management.Host.Models;
    using Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship;
    using Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models;
    using Marain.UserNotifications.ThirdParty.DeliveryChannels.KeyVaultSecretModels;
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
        private readonly ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory;
        private readonly IGenerateTemplateComposer generateTemplateComposer;
        private readonly IAirshipClientFactory airshipClientFactory;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchNotificationActivity"/> class.
        /// </summary>
        /// <param name="tenantProvider">The tenant provider.</param>
        /// <param name="notificationStoreFactory">The factory for the notification store.</param>
        /// <param name="tenantedTemplateStoreFactory">The factory for the templated store.</param>
        /// <param name="generateTemplateComposer">The composer to generate the templated notification per communication channel.</param>
        /// <param name="airshipClientFactory">The Airship Factory.</param>
        /// <param name="configuration">IConfiguration.</param>
        public DispatchNotificationActivity(
            ITenantProvider tenantProvider,
            ITenantedUserNotificationStoreFactory notificationStoreFactory,
            ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory,
            IGenerateTemplateComposer generateTemplateComposer,
            IAirshipClientFactory airshipClientFactory,
            IConfiguration configuration)
        {
            this.tenantProvider = tenantProvider
                ?? throw new ArgumentNullException(nameof(tenantProvider));
            this.notificationStoreFactory = notificationStoreFactory
                ?? throw new ArgumentNullException(nameof(notificationStoreFactory));
            this.tenantedTemplateStoreFactory = tenantedTemplateStoreFactory
                ?? throw new ArgumentNullException(nameof(tenantedTemplateStoreFactory));
            this.generateTemplateComposer = generateTemplateComposer
                ?? throw new ArgumentNullException(nameof(generateTemplateComposer));
            this.airshipClientFactory = airshipClientFactory
                ?? throw new ArgumentNullException(nameof(airshipClientFactory));
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

            if (request.Payload.Id is null)
            {
                throw new Exception("Notification has to be created before being dispatched via Third Party Delivery Channels.");
            }

            ITenant tenant = await this.tenantProvider.GetTenantAsync(request.TenantId).ConfigureAwait(false);

            logger.LogInformation(
                "Executing DispatchNotificationActivity for notification of type {notificationType} for user {userId}",
                request.Payload.NotificationType,
                request.Payload.UserId);

            if (request.Payload.DeliveryChannelConfiguredPerCommunicationType is null)
            {
                throw new Exception($"There is no communication channels and delivery channel defined the notification type: {request.Payload.NotificationType}");
            }

            var registeredCommunicationChannels = request.Payload.DeliveryChannelConfiguredPerCommunicationType.Keys.ToList();
            if (registeredCommunicationChannels.Count == 0)
            {
                throw new Exception($"There is no communication channels and delivery channel defined the notification type: {request.Payload.NotificationType}");
            }

            // Gets the AzureBlobTemplateStore
            INotificationTemplateStore templateStore = await this.tenantedTemplateStoreFactory.GetTemplateStoreForTenantAsync(tenant).ConfigureAwait(false);
            NotificationTemplate notificationTemplate = await this.generateTemplateComposer.GenerateTemplateAsync(templateStore, request.Payload.Properties, registeredCommunicationChannels, request.Payload.NotificationType).ConfigureAwait(false);

            if (notificationTemplate is null)
            {
                throw new Exception("There was an issue generating notification templates.");
            }

            foreach (KeyValuePair<CommunicationType, DeliveryChannel> keyValuePair in request.Payload.DeliveryChannelConfiguredPerCommunicationType)
            {
                switch (keyValuePair.Value)
                {
                    case DeliveryChannel.Airship:
                        if (notificationTemplate.WebPushTemplate != null && keyValuePair.Key == CommunicationType.WebPush)
                        {
                            await this.SendWebPushNotificationAsync(request.Payload.UserId, request.Payload.NotificationType, request.Payload.Id, tenant, notificationTemplate.WebPushTemplate).ConfigureAwait(false);
                        }

                        break;

                    default:
                        throw new Exception(
                            $"Currently the Delivery Channel is limited to {DeliveryChannel.Airship}");
                }
            }
        }

        private async Task SendWebPushNotificationAsync(string userId, string notificationType, string notificationId, ITenant tenant, WebPushTemplate webPushTemplate)
        {
            if (webPushTemplate is null)
            {
                throw new Exception($"There is no WebPushTemplate defined for tenant: {tenant.Id} and notification type: {notificationType}");
            }

            // UserId will be a combination of tenantId and userId of that business.
            string airshipUserId = $"{tenant.Id}:{userId}";

            // Fetch the shared airship config url from the tenant
            tenant.Properties.TryGet(Constants.TenantPropertyNames.SharedAirshipConfig, out string sharedAirshipConfigUrl);
            if (string.IsNullOrEmpty(sharedAirshipConfigUrl))
            {
                throw new Exception($"There is no SharedAirshipConfig defined for tenant: {tenant.Id} and notification type: {notificationType}");
            }

            string? airshipSecretsString = await KeyVaultHelper.GetDeliveryChannelSecretAsync(this.configuration, sharedAirshipConfigUrl).ConfigureAwait(false);

            if (airshipSecretsString is null)
            {
                throw new Exception("There is no airship delivery channel configuration setup in the keyvault");
            }

            // Convert secret to airship secret model
            Airship airshipSecrets = JsonConvert.DeserializeObject<Airship>(airshipSecretsString);

            var airshipDeliveryChannelObject = new AirshipDeliveryChannel(
                webPushTemplate.Title!,
                webPushTemplate.Body!,
                webPushTemplate.ActionUrl!);
            try
            {
                AirshipWebPushResponse? airshipResponse = await this.SendAirshipNotificationAsync(
                      airshipUserId,
                      airshipDeliveryChannelObject,
                      airshipSecrets).ConfigureAwait(false);

                await this.UpdateNotificationDeliveryStatusAsync(
                    airshipDeliveryChannelObject.ContentType,
                    notificationId,
                    airshipResponse is null ? UserNotificationDeliveryStatus.Unknown : UserNotificationDeliveryStatus.NotTracked,
                    tenant).ConfigureAwait(false);
            }
            catch (Exception)
            {
                // TODO: Capture the failure reason and add to the delivery channel json in the future.
                await this.UpdateNotificationDeliveryStatusAsync(airshipDeliveryChannelObject.ContentType, notificationId, UserNotificationDeliveryStatus.Failed, tenant).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Send a web push airship notification.
        /// </summary>
        /// <param name="airshipUserId">Targetted Airship User.</param>
        /// <param name="airshipDeliveryChannel">The <see cref="AirshipDeliveryChannel"/>.</param>
        /// <param name="airshipSecrets">The <see cref="Airship"/>.</param>
        private async Task<AirshipWebPushResponse?> SendAirshipNotificationAsync(string airshipUserId, AirshipDeliveryChannel airshipDeliveryChannel, Airship airshipSecrets)
        {
            if (airshipDeliveryChannel is null)
            {
                throw new Exception();
            }

            AirshipClient? airshipClient = this.airshipClientFactory.GetAirshipClient(airshipSecrets.ApplicationKey!, airshipSecrets.MasterSecret!);

            var newNotification = new Notification()
            {
                Alert = airshipDeliveryChannel.Title,
                Web = new WebAlert()
                {
                    Alert = airshipDeliveryChannel.Body,
                    Title = airshipDeliveryChannel.Title,
                },
                Actions = new Actions()
                {
                    Open = new OpenUrlAction { Type = "url", Content = airshipDeliveryChannel.ActionUrl },
                },
            };

            return await airshipClient.SendWebPushNotification(airshipUserId, newNotification).ConfigureAwait(false);
        }

        private async Task UpdateNotificationDeliveryStatusAsync(string deliveryChannelId, string notificationId, UserNotificationDeliveryStatus deliveryStatus, ITenant tenant)
        {
            IUserNotificationStore store = await this.notificationStoreFactory.GetUserNotificationStoreForTenantAsync(tenant).ConfigureAwait(false);
            UserNotification originalNotification = await store.GetByIdAsync(notificationId).ConfigureAwait(false);

            UserNotification modifiedNotification = originalNotification.WithChannelDeliveryStatus(
                deliveryChannelId,
                deliveryStatus,
                DateTimeOffset.UtcNow);

            await store.StoreAsync(modifiedNotification).ConfigureAwait(false);
        }
    }
}
