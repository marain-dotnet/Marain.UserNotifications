// <copyright file="NotificationMetadata.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    /// <summary>
    /// Class representing metadata for a <see cref="Notification"/>.
    /// </summary>
    public class NotificationMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationMetadata"/> class.
        /// </summary>
        /// <param name="correlationIds">The <see cref="CorrelationIds"/>.</param>
        /// <param name="etag">The <see cref="ETag"/>.</param>
        public NotificationMetadata(string[] correlationIds, string? etag)
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
