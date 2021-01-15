// <copyright file="WebPushTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplates.CommunicationTemplates
{
    /// <summary>
    /// Webpush Object.
    /// </summary>
    public class WebPushTemplate : ICommunicationTemplate
    {
        /// <summary>
        /// The content type that will be used when serializing/deserializing.
        /// </summary>
        public const string RegisteredContentType = "application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1";

        /// <inheritdoc/>
        public string? NotificationType { get; set; }

        /// <inheritdoc/>
        public string? ETag { get; set; }

        /// <summary>
        /// Gets or sets the body of a WebPush notification.
        /// </summary>
        public string? Body { get; set; }

        /// <summary>
        /// Gets or sets the title of the WebPush notification.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the Base64 image of a WebPush notification.
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets navigation url on click of the notification.
        /// </summary>
        public string? ActionUrl { get; set; }

        /// <summary>
        /// Gets the registered content type used when this object is serialized/deserialized.
        /// </summary>
        public string ContentType => RegisteredContentType;
    }
}