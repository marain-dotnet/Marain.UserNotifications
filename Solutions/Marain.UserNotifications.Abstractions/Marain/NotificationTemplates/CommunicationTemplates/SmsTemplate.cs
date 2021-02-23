// <copyright file="SmsTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplates.CommunicationTemplates
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SmsTemplate"/> class.
        /// </summary>
        /// <param name="notificationType">The <see cref="NotificationType"/>.</param>
        /// <param name="body">The <see cref="Body"/>.</param>
        /// <param name="eTag">The <see cref="ETag"/>.</param>
        public SmsTemplate(
            string notificationType,
            string body,
            string? eTag = null)
        {
            this.NotificationType = notificationType;
            this.Body = body;
            this.ETag = eTag;
        }

        /// <inheritdoc/>
        public string NotificationType { get; }

        /// <inheritdoc/>
        public string? ETag { get; set; }

        /// <summary>
        /// Gets the body of the Sms object.
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// Gets the registered content type used when this object is serialized/deserialized.
        /// </summary>
        public string ContentType => RegisteredContentType;
    }
}