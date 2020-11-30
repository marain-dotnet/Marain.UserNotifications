// <copyright file="ITemplateStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System.Threading.Tasks;
    using Marain.UserPreferences;

    /// <summary>
    /// Interface for a service that can store and retrieve templates.
    /// </summary>
    public interface ITemplateStore
    {
        /// <summary>
        /// Stores the given template.
        /// </summary>
        /// <param name="template">The template to save.</param>
        /// <returns>The stored notification.</returns>
        Task<NotificationTypeTemplate> StoreAsync(NotificationTypeTemplate template);

        /// <summary>
        /// Retrieves template for the specified notification type.
        /// </summary>
        /// <param name="notificationType">The notification type.</param>
        /// <returns>The template.</returns>
        Task<NotificationTypeTemplate?> GetAsync(string notificationType);
    }
}
