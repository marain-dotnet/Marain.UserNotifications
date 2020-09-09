// <copyright file="UpdateNotificationReadStatusRequest.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Requests
{
    using System;

    /// <summary>
    /// A request to update the "read" status of a notification.
    /// </summary>
    public class UpdateNotificationReadStatusRequest
    {
        /// <summary>
        /// Gets or sets the new status for the notification.
        /// </summary>
        public UpdateNotificationReadStatusRequestNewStatus NewStatus { get; set; }

        /// <summary>
        /// Gets or sets the time at which the status change occurred.
        /// </summary>
        public DateTime UpdateTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the list of correlation Ids for the request.
        /// </summary>
        public string[] CorrelationIds { get; set; }
    }
}