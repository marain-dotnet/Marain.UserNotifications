// <copyright file="WebPushTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplate.NotificationTemplate.CommunicationTemplates
{
    /// <summary>
    /// Webpush Object.
    /// </summary>
    public class WebPushTemplate
    {
        /// <summary>
        /// Constructor for WebPush object.
        /// </summary>
        /// <param name="body">The inner templated text.</param>
        /// <param name="title">The title of the notification.</param>
        /// <param name="image">The image of the webpush notification.</param>
        public WebPushTemplate(
            string body,
            string title,
            string image)
        {
            this.Body = body;
            this.Title = title;
            this.Image = image;
        }

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
        public string Image { get; }
    }
}