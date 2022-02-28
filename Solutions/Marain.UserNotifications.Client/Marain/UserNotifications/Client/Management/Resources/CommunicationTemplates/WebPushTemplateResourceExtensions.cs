// <copyright file="WebPushTemplateResourceExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources.CommunicationTemplates
{
    using System.Text.Json;
    using Menes.Hal;

    /// <summary>
    /// Extension methods for creating and working with <see cref="WebPushTemplateResource"/>.
    /// </summary>
    public static class WebPushTemplateResourceExtensions
    {
        /// <summary>
        /// The content type for the <see cref="WebPushTemplateResource"/>.
        /// </summary>
        public const string ContentType = "application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1";

        /// <summary>
        /// Gets a value that indicates whether the hal document is a person name resource.
        /// </summary>
        /// <param name="source">The <see cref="IHalDocumentResource"/> to check.</param>
        /// <returns>True if the <see cref="IHalDocumentResource"/> is a <see cref="WebPushTemplateResource"/>.</returns>
        public static bool IsWebPushTemplateResource(this IHalDocumentResource source)
        {
            return source.ContentTypeEquals(ContentType);
        }

        /// <summary>
        /// Gets a <see cref="HalDocumentResource"/> as a <see cref="WebPushTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="HalDocumentResource"/>.</param>
        /// <returns>A <see cref="WebPushTemplateResource"/>.</returns>
        public static WebPushTemplateResource AsWebPushTemplateResource(in this HalDocumentResource source)
        {
            return new WebPushTemplateResource(source);
        }

        /// <summary>
        /// Gets a <see cref="IHalDocumentResource"/> as a <see cref="WebPushTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IHalDocumentResource"/>.</param>
        /// <returns>A <see cref="NotificationStatusResource"/>.</returns>
        public static WebPushTemplateResource AsWebPushTemplateResource(this IHalDocumentResource source)
        {
            if (source is WebPushTemplateResource doc)
            {
                return doc;
            }

            // Note that this throws if the source is not a PersonResource.
            return new WebPushTemplateResource(source.HalDocument);
        }

        /// <summary>
        /// Gets a <see cref="JsonElement"/> as a <see cref="WebPushTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonElement"/>.</param>
        /// <returns>A <see cref="WebPushTemplateResource"/>.</returns>
        public static WebPushTemplateResource AsWebPushTemplateResource(in this JsonElement source)
        {
            return new HalDocumentResource(source).AsWebPushTemplateResource();
        }

        /// <summary>
        /// Gets a <see cref="JsonDocument"/> as a <see cref="WebPushTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonDocument"/>.</param>
        /// <returns>A <see cref="WebPushTemplateResource"/>.</returns>
        public static WebPushTemplateResource AsWebPushTemplateResource(this JsonDocument source)
        {
            return new HalDocumentResource(source).AsWebPushTemplateResource();
        }
    }
}