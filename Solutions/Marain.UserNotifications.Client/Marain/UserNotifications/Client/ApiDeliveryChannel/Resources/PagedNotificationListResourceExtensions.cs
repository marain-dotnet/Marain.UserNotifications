// <copyright file="PagedNotificationListResourceExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.ApiDeliveryChannel.Resources
{
    using System.Text.Json;
    using Menes.Hal;

    /// <summary>
    /// Extension methods for creating and working with <see cref="PagedNotificationListResource"/>.
    /// </summary>
    public static class PagedNotificationListResourceExtensions
    {
        /// <summary>
        /// The content type for the <see cref="PagedNotificationListResource"/>.
        /// </summary>
        public const string ContentType = "application/vnd.marain.usernotifications.apidelivery.notificationslist";

        /// <summary>
        /// Gets a value that indicates whether the hal document is a person name resource.
        /// </summary>
        /// <param name="source">The <see cref="IHalDocumentResource"/> to check.</param>
        /// <returns>True if the <see cref="IHalDocumentResource"/> is a <see cref="PagedNotificationListResource"/>.</returns>
        public static bool IsPagedNotificationListResource(this IHalDocumentResource source)
        {
            return source.ContentTypeEquals(ContentType);
        }

        /// <summary>
        /// Gets a <see cref="HalDocumentResource"/> as a <see cref="PagedNotificationListResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="HalDocumentResource"/>.</param>
        /// <returns>A <see cref="PagedNotificationListResource"/>.</returns>
        public static PagedNotificationListResource AsPagedNotificationListResource(in this HalDocumentResource source)
        {
            return new PagedNotificationListResource(source);
        }

        /// <summary>
        /// Gets a <see cref="IHalDocumentResource"/> as a <see cref="PagedNotificationListResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IHalDocumentResource"/>.</param>
        /// <returns>A <see cref="PagedNotificationListResource"/>.</returns>
        public static PagedNotificationListResource AsPagedNotificationListResource(this IHalDocumentResource source)
        {
            if (source is PagedNotificationListResource doc)
            {
                return doc;
            }

            // Note that this throws if the source is not a PersonResource.
            return new PagedNotificationListResource(source.HalDocument);
        }

        /// <summary>
        /// Gets a <see cref="JsonElement"/> as a <see cref="PagedNotificationListResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonElement"/>.</param>
        /// <returns>A <see cref="PagedNotificationListResource"/>.</returns>
        public static PagedNotificationListResource AsPagedNotificationListResource(in this JsonElement source)
        {
            return new HalDocumentResource(source).AsPagedNotificationListResource();
        }

        /// <summary>
        /// Gets a <see cref="JsonDocument"/> as a <see cref="PagedNotificationListResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonDocument"/>.</param>
        /// <returns>A <see cref="PagedNotificationListResource"/>.</returns>
        public static PagedNotificationListResource AsPagedNotificationListResource(this JsonDocument source)
        {
            return new HalDocumentResource(source).AsPagedNotificationListResource();
        }
    }
}