// <copyright file="NotificationTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    using System;

    /// <summary>
    /// The Notification Template structure.
    /// </summary>
    public class NotificationTemplate
    {
        /// <summary>
        /// Constructor for the notification template object.
        /// </summary>
        public NotificationTemplate()
        {
        }

        /// <summary>
        /// Gets or Sets the notification type.
        /// </summary>
        public string NotificationType { get; set; }

        /// <summary>
        /// Gets or Sets the date and time at which the templates were last updated.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or Sets the Sms object.
        /// The object should be defined if this notification type uses Sms templates.
        /// </summary>
        public Sms Sms { get; set; }

        /// <summary>
        /// Gets or Sets the email object.
        /// The object should be defined if this notification type uses Email templates.
        /// </summary>
        public Email Email { get; set; }

        /// <summary>
        /// Gets or Sets the web push object.
        /// The object should be defined if this notification type uses web push templates.
        /// </summary>
        public WebPush WebPush { get; set; }
    }
}
