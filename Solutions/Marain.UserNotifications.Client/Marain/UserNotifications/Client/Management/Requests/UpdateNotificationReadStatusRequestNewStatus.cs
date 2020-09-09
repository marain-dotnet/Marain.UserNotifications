// <copyright file="UpdateNotificationReadStatusRequestNewStatus.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Requests
{
    /// <summary>
    /// Possible values for the new read status of a notificatino.
    /// </summary>
    public enum UpdateNotificationReadStatusRequestNewStatus
    {
        /// <summary>
        /// Read status is not tracked by the channel.
        /// </summary>
        NotTracked = 0,

        /// <summary>
        /// The notification has not been read.
        /// </summary>
        Unread = 1,

        /// <summary>
        /// The notification has been read.
        /// </summary>
        Read = 2,
    }
}