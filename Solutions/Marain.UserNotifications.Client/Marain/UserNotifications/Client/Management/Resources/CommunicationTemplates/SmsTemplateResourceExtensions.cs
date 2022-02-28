// <copyright file="SmsTemplateResourceExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources.CommunicationTemplates
{
    using System.Text.Json;
    using Menes.Hal;

    /// <summary>
    /// Extension methods for creating and working with <see cref="SmsTemplateResource"/>.
    /// </summary>
    public static class SmsTemplateResourceExtensions
    {
        /// <summary>
        /// The content type for the <see cref="SmsTemplateResource"/>.
        /// </summary>
        public const string ContentType = "application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1";

        /// <summary>
        /// Gets a value that indicates whether the hal document is a person name resource.
        /// </summary>
        /// <param name="source">The <see cref="IHalDocumentResource"/> to check.</param>
        /// <returns>True if the <see cref="IHalDocumentResource"/> is a <see cref="SmsTemplateResource"/>.</returns>
        public static bool IsSmsTemplateResource(this IHalDocumentResource source)
        {
            return source.ContentTypeEquals(ContentType);
        }

        /// <summary>
        /// Gets a <see cref="HalDocumentResource"/> as a <see cref="SmsTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="HalDocumentResource"/>.</param>
        /// <returns>A <see cref="SmsTemplateResource"/>.</returns>
        public static SmsTemplateResource AsSmsTemplateResource(in this HalDocumentResource source)
        {
            return new SmsTemplateResource(source);
        }

        /// <summary>
        /// Gets a <see cref="IHalDocumentResource"/> as a <see cref="SmsTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IHalDocumentResource"/>.</param>
        /// <returns>A <see cref="NotificationStatusResource"/>.</returns>
        public static SmsTemplateResource AsSmsTemplateResource(this IHalDocumentResource source)
        {
            if (source is SmsTemplateResource doc)
            {
                return doc;
            }

            // Note that this throws if the source is not a PersonResource.
            return new SmsTemplateResource(source.HalDocument);
        }

        /// <summary>
        /// Gets a <see cref="JsonElement"/> as a <see cref="SmsTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonElement"/>.</param>
        /// <returns>A <see cref="SmsTemplateResource"/>.</returns>
        public static SmsTemplateResource AsSmsTemplateResource(in this JsonElement source)
        {
            return new HalDocumentResource(source).AsSmsTemplateResource();
        }

        /// <summary>
        /// Gets a <see cref="JsonDocument"/> as a <see cref="SmsTemplateResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonDocument"/>.</param>
        /// <returns>A <see cref="SmsTemplateResource"/>.</returns>
        public static SmsTemplateResource AsSmsTemplateResource(this JsonDocument source)
        {
            return new HalDocumentResource(source).AsSmsTemplateResource();
        }
    }
}