// <copyright file="ITenantedNotificationTemplateStoreFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System.Threading.Tasks;
    using Corvus.Tenancy;

    /// <summary>
    /// Interface for a factory that can create a tenant-specific <see cref="INotificationTemplateStore"/>.
    /// </summary>
    public interface ITenantedNotificationTemplateStoreFactory
    {
        /// <summary>
        /// Retrieves a <see cref="INotificationTemplateStore"/> for the specified <see cref="ITenant"/>.
        /// </summary>
        /// <param name="tenant">The tenant to retrieve the store for.</param>
        /// <returns>The template store for the tenant.</returns>
        Task<INotificationTemplateStore> GetTemplateStoreForTenantAsync(ITenant tenant);
    }
}
