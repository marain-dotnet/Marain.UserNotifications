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
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task StoreAsync(Notification notification);
    }
}
