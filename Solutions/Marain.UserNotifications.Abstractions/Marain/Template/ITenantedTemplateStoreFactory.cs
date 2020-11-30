// <copyright file="ITenantedTemplateStoreFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System.Threading.Tasks;
    using Corvus.Tenancy;

    /// <summary>
    /// Interface for a factory that can create a tenant-specific <see cref="ITemplateStore"/>.
    /// </summary>
    public interface ITenantedTemplateStoreFactory
    {
        /// <summary>
        /// Retrieves a <see cref="ITemplateStore"/> for the specified <see cref="ITenant"/>.
        /// </summary>
        /// <param name="tenant">The tenant to retrieve the store for.</param>
        /// <returns>The template store for the tenant.</returns>
        Task<ITemplateStore> GetTemplateStoreForTenantAsync(ITenant tenant);
    }
}
