// <copyright file="EmailTemplateResourceExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources.CommunicationTemplates
{
    using System.Text.Json;
    using Menes.Hal;

    /// <summary>
    /// Extension methods for creating and working with <see cref="EmailTemplateResource"/>.
    /// </summary>
    public static class EmailTemplateResourceExtensions
    {
        /// <summary>
        /// The content type for the <see cref="EmailTemplateResource"/>.
        /// </summary>
        public const string ContentType = "application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1";

        /// <summary>
        /// Gets a value that indicates whether the hal document is a person name resource.
        /// </summary>
        /// <param name="source">The <see cref="IHalDocumentResource"/> to check.</param>
        /// <returns>True if the <see cref="IHalDocumentResource"/> is a <see cref="EmailTemplateResource"/>.</returns>
        public static bool IsEmailTemplateResource(this IHalDocumentResource source)
        {
            return source.ContentTypeEquals(ContentType);
        }

        /// <summary>
        /// Gets a <see cref="HalDocumentResource"/> as a <see cref="EmailTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="HalDocumentResource"/>.</param>
        /// <returns>A <see cref="EmailTemplateResource"/>.</returns>
        public static EmailTemplateResource AsEmailTemplateResource(in this HalDocumentResource source)
        {
            return new EmailTemplateResource(source);
        }

        /// <summary>
        /// Gets a <see cref="IHalDocumentResource"/> as a <see cref="EmailTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IHalDocumentResource"/>.</param>
        /// <returns>A <see cref="NotificationStatusResource"/>.</returns>
        public static EmailTemplateResource AsEmailTemplateResource(this IHalDocumentResource source)
        {
            if (source is EmailTemplateResource doc)
            {
                return doc;
            }

            // Note that this throws if the source is not a PersonResource.
            return new EmailTemplateResource(source.HalDocument);
        }

        /// <summary>
        /// Gets a <see cref="JsonElement"/> as a <see cref="EmailTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonElement"/>.</param>
        /// <returns>A <see cref="EmailTemplateResource"/>.</returns>
        public static EmailTemplateResource AsEmailTemplateResource(in this JsonElement source)
        {
            return new HalDocumentResource(source).AsEmailTemplateResource();
        }

        /// <summary>
        /// Gets a <see cref="JsonDocument"/> as a <see cref="EmailTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonDocument"/>.</param>
        /// <returns>A <see cref="EmailTemplateResource"/>.</returns>
        public static EmailTemplateResource AsEmailTemplateResource(this JsonDocument source)
        {
            return new HalDocumentResource(source).AsEmailTemplateResource();
        }
    }
}