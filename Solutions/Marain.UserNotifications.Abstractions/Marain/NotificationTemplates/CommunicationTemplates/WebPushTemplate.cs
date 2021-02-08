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

        /// <summary>
        /// Initializes a new instance of the <see cref="WebPushTemplate"/> class.
        /// </summary>
        /// <param name="notificationType">The <see cref="NotificationType"/>.</param>
        /// <param name="title">The <see cref="Title"/>.</param>
        /// <param name="body">The <see cref="Body"/>.</param>
        /// <param name="image">The <see cref="Image"/>.</param>
        /// <param name="actionUrl">The <see cref="ActionUrl"/>.</param>
        /// <param name="eTag">The <see cref="ETag"/>.</param>
        public WebPushTemplate(
            string notificationType,
            string title,
            string body,
            string? image,
            string? actionUrl,
            string? eTag = null)
        {
            this.NotificationType = notificationType;
            this.Title = title;
            this.Body = body;
            this.Image = image;
            this.ActionUrl = actionUrl;
            this.ETag = eTag;
        }

        /// <inheritdoc/>
        public string NotificationType { get; }

        /// <inheritdoc/>
        public string? ETag { get; set; }

        /// <summary>
        /// Gets the body of a WebPush notification.
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// Gets the title of the WebPush notification.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the Base64 image of a WebPush notification.
        /// </summary>
        public string? Image { get; }

        /// <summary>
        /// Gets navigation url on click of the notification.
        /// </summary>
        public string? ActionUrl { get; }

        /// <summary>
        /// Gets the registered content type used when this object is serialized/deserialized.
        /// </summary>
        public string ContentType => RegisteredContentType;
    }
}