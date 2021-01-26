// <copyright file="IGenerateTemplateComposer.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Composer
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Corvus.Json;
    using Marain.Models;
    using Marain.NotificationTemplates;

    /// <summary>
    /// This is responsible to generate a templated notification based on the notification type.
    /// </summary>
    public interface IGenerateTemplateComposer
    {
        /// <summary>
        /// Generate the template and populate the notification Template object.
        /// </summary>
        /// <param name="templateStore">The template store instance.</param>
        /// <param name="body">The property bag of the notification.</param>
        /// <param name="registeredCommunicationChannels">The configured communication channels for the user.</param>
        /// <param name="notificationType">The notification type.</param>
        /// <returns><see cref="NotificationTemplate"/>.</returns>
        Task<NotificationTemplate> GenerateTemplateAsync(INotificationTemplateStore templateStore, IPropertyBag body, List<CommunicationType> registeredCommunicationChannels, string notificationType);
    }
}