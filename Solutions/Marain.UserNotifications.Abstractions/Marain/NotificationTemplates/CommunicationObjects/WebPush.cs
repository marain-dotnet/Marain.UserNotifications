// <copyright file="WebPush.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplates.CommunicationObjects
{
    /// <summary>
    /// Webpush Object.
    /// </summary>
    public class WebPush
    {
        /// <summary>
        /// Constructor for WebPush object.
        /// </summary>
        /// <param name="body">The inner templated text.</param>
        /// <param name="title">The title of the notification.</param>
        /// <param name="image">The image of the webpush notification.</param>
        /// <param name="userIdentifier">The id of the webpush notification.</param>
        public WebPush(
            string body,
            string title,
            string image,
            string userIdentifier)
        {
            this.Body = body;
            this.Title = title;
            this.Image = image;
            this.UserIdentifier = userIdentifier;
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

        /// <summary>
        /// Gets the unique user identifier for a WebPush notification.
        /// </summary>
        public string UserIdentifier { get; }
    }
}