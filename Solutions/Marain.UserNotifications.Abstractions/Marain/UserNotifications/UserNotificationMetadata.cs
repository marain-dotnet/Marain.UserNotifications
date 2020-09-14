// <copyright file="UserNotificationMetadata.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;

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
        public UserNotificationMetadata(IEnumerable<string> correlationIds, string? etag)
        {
            // Note: although correlation Ids should not be null, some combinations of serializer settings can result
            // in an empty array not being serialized, meaning that when this constructor is called during
            // deserialization we will get a null value. As such, we'll just treat null the same as an empty array.
            this.CorrelationIds = correlationIds?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(correlationIds));
            this.ETag = etag;
        }

        /// <summary>
        /// Gets a list of correlation Ids associated with the notification.
        /// </summary>
        public ImmutableArray<string> CorrelationIds { get; }

        /// <summary>
        /// Gets the notification's etag.
        /// </summary>
        public string? ETag { get; }
    }
}
