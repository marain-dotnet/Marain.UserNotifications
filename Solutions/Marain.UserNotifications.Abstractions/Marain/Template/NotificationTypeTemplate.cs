// <copyright file="NotificationTypeTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserPreferences
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The template structure.
    /// </summary>
    public class NotificationTypeTemplate
    {
        /// <summary>
        /// Create TemplateWrapper Object.
        /// </summary>
        /// <param name="notificationType">The notification type.</param>
        /// <param name="timestamp">The last updated time.</param>
        /// <param name="smsObject">The SMS object for the template.</param>
        public NotificationTypeTemplate(
            string notificationType,
            DateTimeOffset? timestamp = null,
            SMSObject? smsObject = null)
        {
            this.NotificationType = notificationType;
            this.Timestamp = timestamp != null && timestamp != default ? timestamp.Value.ToUniversalTime() : DateTimeOffset.UtcNow;
            this.SMSObject = smsObject;
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
        /// Gets the sms object.
        /// The object should be defined if this notification type uses SMS Templates.
        /// </summary>
        public SMSObject? SMSObject { get; }
    }
}
