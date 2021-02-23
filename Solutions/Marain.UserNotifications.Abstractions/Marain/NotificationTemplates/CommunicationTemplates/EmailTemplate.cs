// <copyright file="EmailTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplates.CommunicationTemplates
{
    /// <summary>
    /// Email Object.
    /// </summary>
    public class EmailTemplate : ICommunicationTemplate
    {
        /// <summary>
        /// The content type that will be used when serializing/deserializing.
        /// </summary>
        public const string RegisteredContentType = "application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1";

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplate"/> class.
        /// </summary>
        /// <param name="notificationType">The <see cref="NotificationType"/>.</param>
        /// <param name="subject">The <see cref="Subject"/>.</param>
        /// <param name="body">The <see cref="Body"/>.</param>
        /// <param name="eTag">The <see cref="ETag"/>.</param>
        public EmailTemplate(
            string notificationType,
            string subject,
            string body,
            string? eTag = null)
        {
            this.NotificationType = notificationType;
            this.Subject = subject;
            this.Body = body;
            this.ETag = eTag;
        }

        /// <inheritdoc/>
        public string NotificationType { get; }

        /// <inheritdoc/>
        public string? ETag { get; set; }

        /// <summary>
        /// Gets the body of the email.
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// Gets the subject of the email.
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// Gets the registered content type used when this object is serialized/deserialized.
        /// </summary>
        public string ContentType => RegisteredContentType;
    }
}