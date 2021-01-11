// <copyright file="UserPreference.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserPreferences
{
    using System;
    using System.Collections.Generic;
    using Marain.Models;

    /// <summary>
    /// The user preference structure.
    /// </summary>
    public class UserPreference
    {
        /// <summary>
        /// Create UserPreference Object.
        /// </summary>
        /// <param name="userId">The UserId of the owner.</param>
        /// <param name="email">The Email of the owner.</param>
        /// <param name="phoneNumber">The PhoneNumber of the owner.</param>
        /// <param name="communicationChannelsPerNotificationConfiguration">The communication channels configured for a notification type.</param>
        /// <param name="timestamp"><see cref="Timestamp"/>.</param>
        /// <param name="etag">The <see cref="ETag"/>.</param>
        public UserPreference(
            string userId,
            string? email,
            string? phoneNumber,
            Dictionary<string, List<CommunicationType>> communicationChannelsPerNotificationConfiguration,
            DateTimeOffset timestamp,
            string? etag)
        {
            this.UserId = userId;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.CommunicationChannelsPerNotificationConfiguration = communicationChannelsPerNotificationConfiguration;
            this.Timestamp = timestamp != default ? timestamp.ToUniversalTime() : DateTimeOffset.UtcNow;
            this.ETag = etag;
        }

        /// <summary>
        /// Gets the UserId of the owner.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets the Email of the owner.
        /// </summary>
        public string? Email { get; }

        /// <summary>
        /// Gets the PhoneNumber of the owner.
        /// </summary>
        public string? PhoneNumber { get; }

        /// <summary>
        /// Gets the CommunicationChannelsPerNotificationConfiguration.
        /// This will be notification: list of communication channels configured.
        /// </summary>
        public Dictionary<string, List<CommunicationType>> CommunicationChannelsPerNotificationConfiguration { get; }

        /// <summary>
        /// Gets the date and time at which the user preferences were last updated.
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Gets the notification's etag.
        /// </summary>
        public string? ETag { get; }

        /// <summary>
        /// Add Etag to <see cref="UserPreference"/> and keep the object immutable.
        /// </summary>
        /// <param name="userPreference">The <see cref="UserPreference"/> object.</param>
        /// <param name="eTag">Etag from the CloudBlockBlob Properties.</param>
        /// <returns><see cref="UserPreference"/> object.</returns>
        public UserPreference AddETag(in UserPreference userPreference, string? eTag)
        {
            return new UserPreference(
                userPreference.UserId,
                userPreference.Email,
                userPreference.PhoneNumber,
                userPreference.CommunicationChannelsPerNotificationConfiguration,
                userPreference.Timestamp,
                eTag);
        }
    }
}
