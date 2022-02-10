// <copyright file="NotificationStatusReadStatus.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    /// <summary>
    /// The read status of a notification.
    /// </summary>
    public enum NotificationStatusReadStatus
    {
        /// <summary>
        /// The delivery channel has not updated the read status.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The delivery channel is not able to track the read status.
        /// </summary>
        NotTracked = 1,

        /// <summary>
        /// The notification is unread.
        /// </summary>
        Unread = 2,

        /// <summary>
        /// The notification is read.
        /// </summary>
        Read = 3,
    }
}