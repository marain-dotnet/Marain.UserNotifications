// <copyright file="GetNotificationsResult.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    /// <summary>
    /// Results from a call to <see cref="IUserNotificationStore.GetAsync(string, string?, int)"/>.
    /// </summary>
    public class GetNotificationsResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetNotificationsResult"/> class.
        /// </summary>
        /// <param name="results">The <see cref="Results"/>.</param>
        /// <param name="continuationToken">The <see cref="ContinuationToken"/>.</param>
        public GetNotificationsResult(
            UserNotification[] results,
            string? continuationToken)
        {
            this.Results = results;
            this.ContinuationToken = continuationToken;
        }

        /// <summary>
        /// Gets the result notifications.
        /// </summary>
        public UserNotification[] Results { get; }

        /// <summary>
        /// Gets a continuation token that can be used to retrieve further notifications from the result set.
        /// </summary>
        public string? ContinuationToken { get; }
    }
}