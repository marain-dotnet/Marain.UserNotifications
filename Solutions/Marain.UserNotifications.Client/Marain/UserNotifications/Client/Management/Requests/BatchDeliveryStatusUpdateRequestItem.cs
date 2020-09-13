// <copyright file="BatchDeliveryStatusUpdateRequestItem.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Requests
{
    /// <summary>
    /// Request data item that forms part of a batch update of delivery statuses.
    /// </summary>
    public class BatchDeliveryStatusUpdateRequestItem : UpdateNotificationDeliveryStatusRequest
    {
        /// <summary>
        /// Gets or sets the Id of the notification to update.
        /// </summary>
        public string NotificationId { get; set; }
    }
}
