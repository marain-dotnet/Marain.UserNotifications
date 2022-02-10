// <copyright file="PagedNotificationStatusListResourceExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    using System.Text.Json;
    using Menes.Hal;

    /// <summary>
    /// Extension methods for creating and working with <see cref="PagedNotificationStatusListResource"/>.
    /// </summary>
    public static class PagedNotificationStatusListResourceExtensions
    {
        /// <summary>
        /// The content type for the <see cref="PagedNotificationStatusListResource"/>.
        /// </summary>
        public const string ContentType = "application/vnd.marain.usernotifications.notificationstatuslist";

        /// <summary>
        /// Gets a value that indicates whether the hal document is a person name resource.
        /// </summary>
        /// <param name="source">The <see cref="IHalDocumentResource"/> to check.</param>
        /// <returns>True if the <see cref="IHalDocumentResource"/> is a <see cref="PagedNotificationStatusListResource"/>.</returns>
        public static bool IsPagedNotificationStatusListResource(this IHalDocumentResource source)
        {
            return source.ContentTypeEquals(ContentType);
        }

        /// <summary>
        /// Gets a <see cref="HalDocumentResource"/> as a <see cref="PagedNotificationStatusListResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="HalDocumentResource"/>.</param>
        /// <returns>A <see cref="PagedNotificationStatusListResource"/>.</returns>
        public static PagedNotificationStatusListResource AsPagedNotificationStatusListResource(in this HalDocumentResource source)
        {
            return new PagedNotificationStatusListResource(source);
        }

        /// <summary>
        /// Gets a <see cref="IHalDocumentResource"/> as a <see cref="PagedNotificationStatusListResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IHalDocumentResource"/>.</param>
        /// <returns>A <see cref="PagedNotificationStatusListResource"/>.</returns>
        public static PagedNotificationStatusListResource AsPagedNotificationStatusListResource(this IHalDocumentResource source)
        {
            if (source is PagedNotificationStatusListResource doc)
            {
                return doc;
            }

            // Note that this throws if the source is not a PersonResource.
            return new PagedNotificationStatusListResource(source.HalDocument);
        }

        /// <summary>
        /// Gets a <see cref="JsonElement"/> as a <see cref="PagedNotificationStatusListResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonElement"/>.</param>
        /// <returns>A <see cref="PagedNotificationStatusListResource"/>.</returns>
        public static PagedNotificationStatusListResource AsPagedNotificationStatusListResource(in this JsonElement source)
        {
            return new HalDocumentResource(source).AsPagedNotificationStatusListResource();
        }

        /// <summary>
        /// Gets a <see cref="JsonDocument"/> as a <see cref="PagedNotificationStatusListResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonDocument"/>.</param>
        /// <returns>A <see cref="PagedNotificationStatusListResource"/>.</returns>
        public static PagedNotificationStatusListResource AsPagedNotificationStatusListResource(this JsonDocument source)
        {
            return new HalDocumentResource(source).AsPagedNotificationStatusListResource();
        }
    }
}