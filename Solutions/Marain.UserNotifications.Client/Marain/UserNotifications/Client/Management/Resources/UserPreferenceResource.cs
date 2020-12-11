// <copyright file="UserPreferenceResource.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Menes.Hal;

    /// <summary>
    /// The user preference structure.
    /// </summary>
    [JsonConverter(typeof(UserPreferenceResourceConverter))]
    public class UserPreferenceResource : IHalDocumentResource
    {
        /// <summary>
        /// Constructor for <see cref="UserPreference"/>.
        /// </summary>
        /// <param name="source">The underlying JSON document for the object.</param>
        public UserPreferenceResource(in HalDocumentResource source)
        {
            if (!source.IsUserPreferenceResource())
            {
                throw new ArgumentException("The supplied document is not a UserPreferenceResource", nameof(source));
            }

            this.HalDocument = source;
        }

        /// <inheritdoc/>
        public HalDocumentResource HalDocument { get; }

        /// <inheritdoc/>
        public string ContentType => this.HalDocument.ContentType;

        /// <inheritdoc/>
        public WebLink SelfLink => this.HalDocument.SelfLink;

        /// <summary>
        /// Gets the Id of the user the user preference is for.
        /// </summary>
        public string UserId
        {
            get
            {
                if (this.HalDocument.TryGetProperty("userId", out JsonElement userId) &&
                    userId.ValueKind == JsonValueKind.String)
                {
                    return userId.GetString();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the email Id of the user the user preference is for.
        /// </summary>
        public string Email
        {
            get
            {
                if (this.HalDocument.TryGetProperty("email", out JsonElement email) &&
                    email.ValueKind == JsonValueKind.String)
                {
                    return email.GetString();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the phone number of the user the user preference is for.
        /// </summary>
        public string PhoneNumber
        {
            get
            {
                if (this.HalDocument.TryGetProperty("phoneNumber", out JsonElement phoneNumber) &&
                    phoneNumber.ValueKind == JsonValueKind.String)
                {
                    return phoneNumber.GetString();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the CommunicationChannelsPerNotificationConfiguration.
        /// This will be notification: list of communication channels configured.
        /// </summary>
        public string CommunicationChannelsPerNotificationConfiguration
        {
            get
            {
                if (this.HalDocument.TryGetProperty("communicationChannelsPerNotificationConfiguration", out JsonElement communicationChannel) &&
                    communicationChannel.ValueKind == JsonValueKind.String)
                {
                    return communicationChannel.GetString();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the date and time at which the event being notified took place.
        /// </summary>
        public DateTimeOffset Timestamp
        {
            get
            {
                if (this.HalDocument.TryGetProperty("timestamp", out JsonElement timestamp) &&
                    timestamp.ValueKind == JsonValueKind.String)
                {
                    return timestamp.GetDateTimeOffset();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the etag at which the event being notified took place.
        /// </summary>
        public string ETag
        {
            get
            {
                if (this.HalDocument.TryGetProperty("eTag", out JsonElement etag) &&
                    etag.ValueKind == JsonValueKind.String)
                {
                    return etag.GetString();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <inheritdoc/>
        public bool ContentTypeEquals(ReadOnlySpan<byte> utf8Text)
        {
            return this.HalDocument.ContentTypeEquals(utf8Text);
        }

        /// <inheritdoc/>
        public bool ContentTypeEquals(ReadOnlySpan<char> text)
        {
            return this.HalDocument.ContentTypeEquals(text);
        }

        /// <inheritdoc/>
        public bool ContentTypeEquals(string text)
        {
            return this.HalDocument.ContentTypeEquals(text);
        }

        /// <inheritdoc/>
        public HalDocumentResource.EmbeddedEnumerator EnumerateEmbedded()
        {
            return this.HalDocument.EnumerateEmbedded();
        }

        /// <inheritdoc/>
        public HalDocumentResource.EmbeddedForRelationEnumerator EnumerateEmbedded(string relation)
        {
            return this.HalDocument.EnumerateEmbedded(relation);
        }

        /// <inheritdoc/>
        public HalDocumentResource.LinksEnumerator EnumerateLinks()
        {
            return this.HalDocument.EnumerateLinks();
        }

        /// <inheritdoc/>
        public HalDocumentResource.LinksForRelationEnumerator EnumerateLinks(string relation)
        {
            return this.HalDocument.EnumerateLinks(relation);
        }

        /// <inheritdoc/>
        public HalDocumentResource.PropertiesEnumerator EnumerateProperties()
        {
            return this.HalDocument.EnumerateProperties();
        }

        /// <inheritdoc/>
        public bool TryGetProperty(string propertyName, out JsonElement element)
        {
            return this.HalDocument.TryGetProperty(propertyName, out element);
        }

        /// <inheritdoc/>
        public bool TryGetProperty(ReadOnlySpan<char> propertyName, out JsonElement element)
        {
            return this.HalDocument.TryGetProperty(propertyName, out element);
        }

        /// <inheritdoc/>
        public bool TryGetProperty(ReadOnlySpan<byte> utf8PropertyName, out JsonElement element)
        {
            return this.HalDocument.TryGetProperty(utf8PropertyName, out element);
        }

        /// <inheritdoc/>
        IJsonPropertyBag.IJsonPropertyBagEnumerator IJsonPropertyBag.EnumerateProperties()
        {
            return ((IJsonPropertyBag)this.HalDocument).EnumerateProperties();
        }

        private class UserPreferenceResourceConverter : JsonConverter<UserPreferenceResource>
        {
            public override UserPreferenceResource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var document = JsonDocument.ParseValue(ref reader);

                // If we are participating in serialization, rather than operating over the object model,
                // then we clone the Json element to detatch it from the underlying document stream.
                // Note that this will create a single copy of the elements within the Hal document, and that
                // they will be shared by any child documents.
                return document.RootElement.Clone().AsUserPreferenceResource();
            }

            public override void Write(Utf8JsonWriter writer, UserPreferenceResource value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value.HalDocument, options);
            }
        }
    }
}
