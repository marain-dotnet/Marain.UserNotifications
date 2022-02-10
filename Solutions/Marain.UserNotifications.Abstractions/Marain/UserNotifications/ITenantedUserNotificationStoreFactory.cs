// <copyright file="ITenantedUserNotificationStoreFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System.Threading.Tasks;
    using Corvus.Tenancy;

    /// <summary>
    /// Interface for a factory that can create a tenant-specific <see cref="IUserNotificationStore"/>.
    /// </summary>
    public interface ITenantedUserNotificationStoreFactory
    {
        /// <summary>
        /// Retrieves a <see cref="IUserNotificationStore"/> for the specified <see cref="ITenant"/>.
        /// </summary>
        /// <param name="tenant">The tenant to retrieve the store for.</param>
        /// <returns>The notification store for the tenant.</returns>
        Task<IUserNotificationStore> GetUserNotificationStoreForTenantAsync(ITenant tenant);
    }
}