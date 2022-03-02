// <copyright file="UserNotificationNotFoundException.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System;

    /// <summary>
    /// Exception thrown when a requested notification does not exist.
    /// </summary>
    [Serializable]
    public class UserNotificationNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationNotFoundException"/> class.
        /// </summary>
        /// <param name="notificationId">The Id of the requested notification.</param>
        public UserNotificationNotFoundException(string notificationId)
            : base($"The notification with Id '{notificationId}' could not be found")
        {
        }
    }
}