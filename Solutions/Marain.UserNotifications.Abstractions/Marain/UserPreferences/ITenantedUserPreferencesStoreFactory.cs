// <copyright file="ITenantedUserPreferencesStoreFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System.Threading.Tasks;
    using Corvus.Tenancy;

    /// <summary>
    /// Interface for a factory that can create a tenant-specific <see cref="IUserNotificationStore"/>.
    /// </summary>
    public interface ITenantedUserPreferencesStoreFactory
    {
        /// <summary>
        /// Retrieves a <see cref="IUserPreferencesStore"/> for the specified <see cref="ITenant"/>.
        /// </summary>
        /// <param name="tenant">The tenant to retrieve the store for.</param>
        /// <returns>The preferences store for the tenant.</returns>
        Task<IUserPreferencesStore> GetUserPreferencesStoreForTenantAsync(ITenant tenant);
    }
}
