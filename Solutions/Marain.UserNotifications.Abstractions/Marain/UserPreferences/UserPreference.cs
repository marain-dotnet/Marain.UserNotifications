// <copyright file="UserPreference.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserPreferences
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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
        public UserPreference(
            string userId,
            string? email,
            string? phoneNumber,
            Dictionary<string, List<string>> communicationChannelsPerNotificationConfiguration,
            DateTimeOffset timestamp)
        {
            this.UserId = userId;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.CommunicationChannelsPerNotificationConfiguration = communicationChannelsPerNotificationConfiguration;
            this.Timestamp = timestamp != default ? timestamp.ToUniversalTime() : DateTimeOffset.UtcNow;
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
        public Dictionary<string, List<string>> CommunicationChannelsPerNotificationConfiguration { get; }

        /// <summary>
        /// Gets the date and time at which the user preferences were last updated.
        /// </summary>
        public DateTimeOffset Timestamp { get; }
    }
}
