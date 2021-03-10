// <copyright file="GenerateTemplateComposer.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Composer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Corvus.Json;
    using DotLiquid;
    using Marain.Models;
    using Marain.NotificationTemplates;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Microsoft.Extensions.Logging;
    using NotificationTemplate = Marain.NotificationTemplates.NotificationTemplate;

    /// <inheritdoc/>
    public class GenerateTemplateComposer : IGenerateTemplateComposer
    {
        private readonly ILogger<GenerateTemplateComposer> logger;

        /// <summary>
        /// Initializes a new instance of <see cref="GenerateTemplateComposer"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public GenerateTemplateComposer(ILogger<GenerateTemplateComposer> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<NotificationTemplate> GenerateTemplateAsync(
            INotificationTemplateStore templateStore,
            IPropertyBag body,
            List<CommunicationType> registeredCommunicationChannels,
            string notificationType)
        {
            EmailTemplate? emailTemplate = null;
            SmsTemplate? smsTemplate = null;
            WebPushTemplate? webPushTemplate = null;
            var propertiesDictionary = body.AsDictionaryRecursive().ToDictionary(x => x.Key, x => x.Value);
            var propertiesHash = Hash.FromDictionary(propertiesDictionary);

            foreach (CommunicationType channel in registeredCommunicationChannels)
            {
                switch (channel)
                {
                    case CommunicationType.Email:
                        try
                        {
                            (EmailTemplate, string?) emailRawTemplate = await templateStore.GetAsync<EmailTemplate>(notificationType, CommunicationType.Email).ConfigureAwait(false);
                            string? emailBody = await this.GenerateTemplateForFieldAsync(emailRawTemplate.Item1.Body, propertiesHash).ConfigureAwait(false);
                            string? emailSubject = await this.GenerateTemplateForFieldAsync(emailRawTemplate.Item1.Subject, propertiesHash).ConfigureAwait(false);

                            if (string.IsNullOrEmpty(emailSubject))
                            {
                                this.logger.LogError($"The template for the communication type Email doesn't have subject which is necessary to trigger a {notificationType} notification.");
                                break;
                            }

                            if (string.IsNullOrEmpty(emailBody))
                            {
                                this.logger.LogError($"The template for the communication type Email doesn't have body which is necessary to trigger a {notificationType} notification.");
                                break;
                            }

                            emailTemplate = new EmailTemplate(
                                notificationType,
                                emailSubject,
                                emailBody);
                        }
                        catch (Exception)
                        {
                            this.logger.LogError("The template for the communication type Email doesn't exist");
                        }

                        break;

                    case CommunicationType.Sms:
                        try
                        {
                            (SmsTemplate, string?) smsRawTemplate = await templateStore.GetAsync<SmsTemplate>(notificationType, CommunicationType.Sms).ConfigureAwait(false);
                            string? smsBody = await this.GenerateTemplateForFieldAsync(smsRawTemplate.Item1.Body!, propertiesHash).ConfigureAwait(false);

                            if (string.IsNullOrEmpty(smsBody))
                            {
                                this.logger.LogError($"The template for the communication type Sms doesn't have body which is necessary to trigger a {notificationType} notification.");
                                break;
                            }

                            smsTemplate = new SmsTemplate(notificationType, smsBody);
                        }
                        catch (Exception)
                        {
                            this.logger.LogError("The template for the communication type Sms doesn't exist");
                        }

                        break;

                    case CommunicationType.WebPush:
                        try
                        {
                            (WebPushTemplate, string?) webPushRawTemplate = await templateStore.GetAsync<WebPushTemplate>(notificationType, CommunicationType.WebPush).ConfigureAwait(false);
                            string? webPushTitle = await this.GenerateTemplateForFieldAsync(webPushRawTemplate.Item1.Title, propertiesHash).ConfigureAwait(false);
                            string? webPushBody = await this.GenerateTemplateForFieldAsync(webPushRawTemplate.Item1.Body, propertiesHash).ConfigureAwait(false);
                            string? webPushImage = await this.GenerateTemplateForFieldAsync(webPushRawTemplate.Item1.Image, propertiesHash).ConfigureAwait(false);
                            string? actionUrl = await this.GenerateTemplateForFieldAsync(webPushRawTemplate.Item1.ActionUrl, propertiesHash).ConfigureAwait(false);

                            if (string.IsNullOrEmpty(webPushTitle))
                            {
                                this.logger.LogError($"The template for the communication type WebPush doesn't have title which is necessary to trigger a {notificationType} notification.");
                                break;
                            }

                            if (string.IsNullOrEmpty(webPushBody))
                            {
                                this.logger.LogError($"The template for the communication type WebPush doesn't have body which is necessary to trigger a {notificationType} notification.");
                                break;
                            }

                            webPushTemplate = new WebPushTemplate(
                                notificationType,
                                webPushTitle,
                                webPushBody,
                                webPushImage,
                                actionUrl);
                        }
                        catch (Exception)
                        {
                            this.logger.LogError("The template for the communication type WebPush doesn't exist");
                        }

                        break;
                }
            }

            return new NotificationTemplate(
                notificationType,
                smsTemplate,
                emailTemplate,
                webPushTemplate);
        }

        /// <summary>
        /// Generate a template for a single field.
        /// </summary>
        /// <param name="templateBody">A string with handlebars. </param>
        /// <param name="properties">A dictionary of all properties that can be used to render the templateBody string. </param>
        /// <returns>A rendered string. </returns>
        private async Task<string?> GenerateTemplateForFieldAsync(string? templateBody, Hash properties)
        {
            if (string.IsNullOrEmpty(templateBody))
            {
                return null;
            }

            var template = Template.Parse(templateBody);
            return await template.RenderAsync().ConfigureAwait(false);
        }
    }
}
