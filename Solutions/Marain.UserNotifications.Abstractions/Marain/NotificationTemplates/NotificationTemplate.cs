// <copyright file="NotificationTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplates
{
    using System;
    using Marain.NotificationTemplates.CommunicationTemplates;

    /// <summary>
    /// The Notification Template structure.
    /// </summary>
    public class NotificationTemplate
    {
        /// <summary>
        /// Constructor for the notification template object.
        /// </summary>
        /// <param name="notificationType">The notification type.</param>
        /// <param name="timestamp">The last updated time.</param>
        /// <param name="smsTemplate">The SMS object for the template.</param>
        /// <param name="emailTemplate">The email object for the notification.</param>
        /// <param name="webPushTemplate">The web push object for the notification.</param>
        public NotificationTemplate(
            string notificationType,
            DateTimeOffset? timestamp = null,
            SmsTemplate? smsTemplate = null,
            EmailTemplate? emailTemplate = null,
            WebPushTemplate? webPushTemplate = null)
        {
            this.NotificationType = notificationType;
            this.Timestamp = timestamp != null && timestamp != default ? timestamp.Value.ToUniversalTime() : DateTimeOffset.UtcNow;
            this.SmsTemplate = smsTemplate;
            this.EmailTemplate = emailTemplate;
            this.WebPushTemplate = webPushTemplate;
        }

        /// <summary>
        /// Gets the notification type.
        /// </summary>
        public string NotificationType { get; }

        /// <summary>
        /// Gets the date and time at which the templates were last updated.
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Gets the Sms object.
        /// The object should be defined if this notification type uses Sms templates.
        /// </summary>
        public SmsTemplate? SmsTemplate { get; }

        /// <summary>
        /// Gets the email object.
        /// The object should be defined if this notification type uses Email templates.
        /// </summary>
        public EmailTemplate? EmailTemplate { get; }

        /// <summary>
        /// Gets the web push object.
        /// The object should be defined if this notification type uses web push templates.
        /// </summary>
        public WebPushTemplate? WebPushTemplate { get; }
    }
}
