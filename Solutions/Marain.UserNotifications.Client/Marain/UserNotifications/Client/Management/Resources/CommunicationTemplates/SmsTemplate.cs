// <copyright file="SmsTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources.CommunicationTemplates
{
    /// <summary>
    /// Sms Object.
    /// </summary>
    public class SmsTemplate : ICommunicationTemplate
    {
        /// <summary>
        /// The content type that will be used when serializing/deserializing.
        /// </summary>
        public const string RegisteredContentType = "application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1";

        /// <inheritdoc/>
        public string NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the body of the Sms object.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets the registered content type used when this object is serialized/deserialized.
        /// </summary>
        public string ContentType => RegisteredContentType;
    }
}