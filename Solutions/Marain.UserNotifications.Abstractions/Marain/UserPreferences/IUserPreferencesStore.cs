// <copyright file="IUserPreferencesStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System.Threading.Tasks;
    using Marain.UserPreferences;

    /// <summary>
    /// Interface for a service that can store and retrieve user preference.
    /// </summary>
    public interface IUserPreferencesStore
    {
        /// <summary>
        /// Stores the given user preference.
        /// </summary>
        /// <param name="userPreference">The user preference to store.</param>
        /// <returns>The stored notification.</returns>
        Task<UserPreference> StoreAsync(UserPreference userPreference);

        /// <summary>
        /// Retrieves user preference for the specified user.
        /// </summary>
        /// <param name="userId">The user to retrieve notifications for.</param>
        /// <returns>The user notifications.</returns>
        Task<UserPreference?> GetAsync(string userId);
    }
}
