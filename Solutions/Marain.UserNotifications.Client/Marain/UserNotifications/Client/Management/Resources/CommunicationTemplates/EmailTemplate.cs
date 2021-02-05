// <copyright file="EmailTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources.CommunicationTemplates
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
        /// Gets or Sets the notification's etag.
        /// </summary>
        public string ETag { get; set; }

        /// <inheritdoc/>
        public string NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the body of the email.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets the registered content type used when this object is serialized/deserialized.
        /// </summary>
        public string ContentType => RegisteredContentType;
    }
}