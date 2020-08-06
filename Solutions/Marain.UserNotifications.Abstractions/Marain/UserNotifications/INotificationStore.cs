// <copyright file="INotificationStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for a service that can store and retrieve notifications.
    /// </summary>
    public interface INotificationStore
    {
        /// <summary>
        /// Stores the given notification.
        /// </summary>
        /// <param name="notification">The notification to store.</param>
        /// <returns>The stored notification.</returns>
        Task<Notification> StoreAsync(Notification notification);
    }
}
