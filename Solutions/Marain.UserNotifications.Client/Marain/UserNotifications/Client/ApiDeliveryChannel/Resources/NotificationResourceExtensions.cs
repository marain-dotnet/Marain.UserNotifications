// <copyright file="NotificationResourceExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.ApiDeliveryChannel.Resources
{
    using System.Text.Json;
    using Menes.Hal;

    /// <summary>
    /// Extension methods for creating and working with <see cref="NotificationResource"/>.
    /// </summary>
    public static class NotificationResourceExtensions
    {
        /// <summary>
        /// The content type for the <see cref="NotificationResource"/>.
        /// </summary>
        public const string ContentType = "application/vnd.marain.usernotifications.apidelivery.notification";

        /// <summary>
        /// Gets a value that indicates whether the hal document is a person name resource.
        /// </summary>
        /// <param name="source">The <see cref="IHalDocumentResource"/> to check.</param>
        /// <returns>True if the <see cref="IHalDocumentResource"/> is a <see cref="NotificationResource"/>.</returns>
        public static bool IsNotificationResource(this IHalDocumentResource source)
        {
            return source.ContentTypeEquals(ContentType);
        }

        /// <summary>
        /// Gets a <see cref="HalDocumentResource"/> as a <see cref="NotificationResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="HalDocumentResource"/>.</param>
        /// <returns>A <see cref="NotificationResource"/>.</returns>
        public static NotificationResource AsNotificationResource(in this HalDocumentResource source)
        {
            return new NotificationResource(source);
        }

        /// <summary>
        /// Gets a <see cref="IHalDocumentResource"/> as a <see cref="NotificationResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IHalDocumentResource"/>.</param>
        /// <returns>A <see cref="NotificationResource"/>.</returns>
        public static NotificationResource AsNotificationResource(this IHalDocumentResource source)
        {
            if (source is NotificationResource doc)
            {
                return doc;
            }

            // Note that this throws if the source is not a PersonResource.
            return new NotificationResource(source.HalDocument);
        }

        /// <summary>
        /// Gets a <see cref="JsonElement"/> as a <see cref="NotificationResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonElement"/>.</param>
        /// <returns>A <see cref="NotificationResource"/>.</returns>
        public static NotificationResource AsNotificationResource(in this JsonElement source)
        {
            return new HalDocumentResource(source).AsNotificationResource();
        }

        /// <summary>
        /// Gets a <see cref="JsonDocument"/> as a <see cref="NotificationResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonDocument"/>.</param>
        /// <returns>A <see cref="NotificationResource"/>.</returns>
        public static NotificationResource AsNotificationResource(this JsonDocument source)
        {
            return new HalDocumentResource(source).AsNotificationResource();
        }
    }
}