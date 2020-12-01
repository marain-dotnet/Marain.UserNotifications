// <copyright file="INotificationTemplateStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplate.NotificationTemplate
{
    using System.Threading.Tasks;
    using Marain.UserPreferences;

    /// <summary>
    /// Interface for a service that can store and retrieve templates.
    /// </summary>
    public interface INotificationTemplateStore
    {
        /// <summary>
        /// Stores the given template.
        /// </summary>
        /// <param name="template">The template to save.</param>
        /// <returns>The stored notification.</returns>
        Task<NotificationTemplate> StoreAsync(NotificationTemplate template);

        /// <summary>
        /// Retrieves template for the specified notification type.
        /// </summary>
        /// <param name="notificationType">The notification type.</param>
        /// <returns>The template.</returns>
        Task<NotificationTemplate?> GetAsync(string notificationType);
    }
}
