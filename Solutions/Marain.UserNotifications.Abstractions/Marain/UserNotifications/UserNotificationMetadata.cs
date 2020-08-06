// <copyright file="UserNotificationMetadata.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    /// <summary>
    /// Class representing metadata for a <see cref="UserNotification"/>.
    /// </summary>
    public class UserNotificationMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationMetadata"/> class.
        /// </summary>
        /// <param name="correlationIds">The <see cref="CorrelationIds"/>.</param>
        /// <param name="etag">The <see cref="ETag"/>.</param>
        public UserNotificationMetadata(string[] correlationIds, string? etag)
        {
            this.CorrelationIds = correlationIds;
            this.ETag = etag;
        }

        /// <summary>
        /// Gets a list of correlation Ids associated with the notification.
        /// </summary>
        public string[] CorrelationIds { get; }

        /// <summary>
        /// Gets the notification's etag.
        /// </summary>
        public string? ETag { get; }
    }
}
