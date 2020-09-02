// <copyright file="UserNotificationReadStatus.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    /// <summary>
    /// Potential values for <see cref="UserNotificationStatus.ReadStatus" />.
    /// </summary>
    public enum UserNotificationReadStatus
    {
        /// <summary>
        /// The status has not been set.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The delivery channel is not able to track notification read statuses.
        /// </summary>
        NotTracked = 1,

        /// <summary>
        /// The notification has not been read.
        /// </summary>
        Unread = 2,

        /// <summary>
        /// The notification has been read.
        /// </summary>
        Read = 3,
    }
}