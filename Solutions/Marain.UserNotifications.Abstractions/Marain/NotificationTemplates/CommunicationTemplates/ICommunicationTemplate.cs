// <copyright file="ICommunicationTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplates.CommunicationTemplates
{
    /// <summary>
    /// The interface for classes that use content type to
    /// serilize/deserialise an object into a certain
    /// communication template.
    /// </summary>
    public interface ICommunicationTemplate
    {
        /// <summary>
        /// Gets the content type that will be used when serializing/deserializing.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets or sets the notification type of the communication template.
        /// </summary>
        string? NotificationType { get; set; }

        /// <summary>
        /// Gets or Sets the notification's etag.
        /// </summary>
        public string? ETag { get; set; }
    }
}
