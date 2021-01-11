// <copyright file="GenerateTemplateComposer.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Composer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Corvus.Json;
    using DotLiquid;
    using Marain.Helper;
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
            Dictionary<string, object> existingProperties = PropertyBagHelpers.GetDictionaryFromPropertyBag(body);

            foreach (CommunicationType channel in registeredCommunicationChannels)
            {
                switch (channel)
                {
                    case CommunicationType.Email:
                        try
                        {
                            (EmailTemplate, string?) emailRawTemplate = await templateStore.GetAsync<EmailTemplate>(notificationType, CommunicationType.Email).ConfigureAwait(false);
                            string? emailBody = await this.GenerateTemplateForFieldAsync(emailRawTemplate.Item1.Body, existingProperties).ConfigureAwait(false);
                            string? emailSubject = await this.GenerateTemplateForFieldAsync(emailRawTemplate.Item1.Subject, existingProperties).ConfigureAwait(false);

                            emailTemplate = new EmailTemplate()
                            {
                                NotificationType = notificationType,
                                Body = emailBody,
                                Subject = emailSubject,
                                Important = false,
                            };
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
                            string? smsBody = await this.GenerateTemplateForFieldAsync(smsRawTemplate.Item1.Body!, existingProperties).ConfigureAwait(false);

                            smsTemplate = new SmsTemplate() { NotificationType = notificationType, Body = smsBody };
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
                            string? webPushTitle = await this.GenerateTemplateForFieldAsync(webPushRawTemplate.Item1.Title, existingProperties).ConfigureAwait(false);
                            string? webPushBody = await this.GenerateTemplateForFieldAsync(webPushRawTemplate.Item1.Body, existingProperties).ConfigureAwait(false);
                            string? webPushImage = await this.GenerateTemplateForFieldAsync(webPushRawTemplate.Item1.Image, existingProperties).ConfigureAwait(false);

                            webPushTemplate = new WebPushTemplate()
                            {
                                NotificationType = notificationType,
                                Body = webPushBody,
                                Title = webPushTitle,
                                Image = webPushImage,
                            };
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
                smsTemplate: smsTemplate,
                emailTemplate: emailTemplate,
                webPushTemplate: webPushTemplate);
        }

        /// <summary>
        /// Generate a template for a single field.
        /// </summary>
        /// <param name="templateBody">A string with handlebars. </param>
        /// <param name="properties">A dictionary of all properties that can be used to render the templateBody string. </param>
        /// <returns>A rendered string. </returns>
        private async Task<string?> GenerateTemplateForFieldAsync(string? templateBody, Dictionary<string, object> properties)
        {
            if (string.IsNullOrEmpty(templateBody))
            {
                return null;
            }

            var template = Template.Parse(templateBody);
            return await template.RenderAsync(Hash.FromDictionary(properties)).ConfigureAwait(false);
        }
    }
}
