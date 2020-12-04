// <copyright file="WebPushTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    /// <summary>
    /// Webpush Object.
    /// </summary>
    public class WebPushTemplate
    {
        /// <summary>
        /// Gets or Sets the body of a WebPush notification.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or Sets the title of the WebPush notification.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets the Base64 image of a WebPush notification.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or Sets the unique user identifier for a WebPush notification.
        /// </summary>
        public string UserIdentifier { get; set; }
    }
}