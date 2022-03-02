// <copyright file="NotificationTemplateNotFoundException.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplates
{
    using System;

    /// <summary>
    /// Exception thrown when a requested notification does not exist.
    /// </summary>
    [Serializable]
    public class NotificationTemplateNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTemplateNotFoundException"/> class.
        /// </summary>
        /// <param name="blockBlobName">The block blob Id for the Template.</param>
        public NotificationTemplateNotFoundException(string blockBlobName)
            : base($"The template for {blockBlobName} could not be found")
        {
        }
    }
}