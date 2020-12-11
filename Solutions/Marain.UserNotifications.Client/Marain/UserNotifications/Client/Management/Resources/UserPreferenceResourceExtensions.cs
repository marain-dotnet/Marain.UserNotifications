// <copyright file="UserPreferenceResourceExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    using System.Text.Json;
    using Menes.Hal;

    /// <summary>
    /// Extension methods for creating and working with <see cref="NotificationStatusResource"/>.
    /// </summary>
    public static class UserPreferenceResourceExtensions
    {
        /// <summary>
        /// The content type for the <see cref="UserPreferenceResource"/>.
        /// </summary>
        public const string ContentType = "application/vnd.marain.usernotifications.management.userpreference.v1";

        /// <summary>
        /// Gets a value that indicates whether the hal document is a person name resource.
        /// </summary>
        /// <param name="source">The <see cref="IHalDocumentResource"/> to check.</param>
        /// <returns>True if the <see cref="IHalDocumentResource"/> is a <see cref="UserPreferenceResource"/>.</returns>
        public static bool IsUserPreferenceResource(this IHalDocumentResource source)
        {
            return source.ContentTypeEquals(ContentType);
        }

        /// <summary>
        /// Gets a <see cref="HalDocumentResource"/> as a <see cref="UserPreferenceResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="HalDocumentResource"/>.</param>
        /// <returns>A <see cref="UserPreferenceResource"/>.</returns>
        public static UserPreferenceResource AsUserPreferenceResource(in this HalDocumentResource source)
        {
            return new UserPreferenceResource(source);
        }

        /// <summary>
        /// Gets a <see cref="IHalDocumentResource"/> as a <see cref="UserPreferenceResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IHalDocumentResource"/>.</param>
        /// <returns>A <see cref="NotificationStatusResource"/>.</returns>
        public static UserPreferenceResource AsUserPreferenceResource(this IHalDocumentResource source)
        {
            if (source is UserPreferenceResource doc)
            {
                return doc;
            }

            // Note that this throws if the source is not a PersonResource.
            return new UserPreferenceResource(source.HalDocument);
        }

        /// <summary>
        /// Gets a <see cref="JsonElement"/> as a <see cref="UserPreferenceResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonElement"/>.</param>
        /// <returns>A <see cref="UserPreferenceResource"/>.</returns>
        public static UserPreferenceResource AsUserPreferenceResource(in this JsonElement source)
        {
            return new HalDocumentResource(source).AsUserPreferenceResource();
        }

        /// <summary>
        /// Gets a <see cref="JsonDocument"/> as a <see cref="UserPreferenceResource"/>.
        /// </summary>
        /// <param name="source">The source <see cref="JsonDocument"/>.</param>
        /// <returns>A <see cref="UserPreferenceResource"/>.</returns>
        public static UserPreferenceResource AsUserPreferenceResource(this JsonDocument source)
        {
            return new HalDocumentResource(source).AsUserPreferenceResource();
        }
    }
}
