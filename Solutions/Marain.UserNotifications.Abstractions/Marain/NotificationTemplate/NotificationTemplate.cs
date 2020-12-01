// <copyright file="NotificationTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplate.NotificationTemplate
{
    using System;
    using Marain.NotificationTemplate.NotificationTemplate.CommunicationObjects;

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
        /// <param name="sms">The SMS object for the template.</param>
        /// <param name="email">The email object for the notification.</param>
        /// <param name="webPush">The web push object for the notification.</param>
        public NotificationTemplate(
            string notificationType,
            DateTimeOffset? timestamp = null,
            Sms? sms = null,
            Email? email = null,
            WebPush? webPush = null)
        {
            this.NotificationType = notificationType;
            this.Timestamp = timestamp != null && timestamp != default ? timestamp.Value.ToUniversalTime() : DateTimeOffset.UtcNow;
            this.Sms = sms;
            this.Email = email;
            this.WebPush = webPush;
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
        public Sms? Sms { get; }

        /// <summary>
        /// Gets the email object.
        /// The object should be defined if this notification type uses Email templates.
        /// </summary>
        public Email? Email { get; }

        /// <summary>
        /// Gets the web push object.
        /// The object should be defined if this notification type uses web push templates.
        /// </summary>
        public WebPush? WebPush { get; }
    }
}
