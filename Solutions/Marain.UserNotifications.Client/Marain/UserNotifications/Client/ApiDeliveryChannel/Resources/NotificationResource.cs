// <copyright file="NotificationResource.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.ApiDeliveryChannel.Resources
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Menes.Hal;

    /// <summary>
    /// A notification.
    /// </summary>
    [JsonConverter(typeof(NotificationResourceConverter))]
    public readonly struct NotificationResource : IHalDocumentResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationResource"/> class.
        /// </summary>
        /// <param name="source">The underlying JSON document for the object.</param>
        public NotificationResource(in HalDocumentResource source)
        {
            if (!source.IsNotificationResource())
            {
                throw new ArgumentException("The supplied document is not a NotificationResource", nameof(source));
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
        /// Gets the type of the notification.
        /// </summary>
        public string NotificationType
        {
            get
            {
                if (this.HalDocument.TryGetProperty("notificationType", out JsonElement title) &&
                    title.ValueKind == JsonValueKind.String)
                {
                    return title.GetString();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the notification properties.
        /// </summary>
        public JsonElement Properties
        {
            get
            {
                if (this.HalDocument.TryGetProperty("properties", out JsonElement properties) &&
                    properties.ValueKind == JsonValueKind.Object)
                {
                    return properties;
                }

                return default;
            }
        }

        /// <summary>
        /// Gets the Id of the user the notification is for.
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
        /// Gets a value indicating whether a boolean indicating whether or not the notification has been
        /// delivered via at least one channel.
        /// </summary>
        public bool Delivered
        {
            get
            {
                if (this.HalDocument.TryGetProperty("delivered", out JsonElement delivered) &&
                    (delivered.ValueKind == JsonValueKind.True || delivered.ValueKind == JsonValueKind.False))
                {
                    return delivered.GetBoolean();
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a boolean indicating whether or not the notification has been
        /// read/acknowledged via at least one channel.
        /// </summary>
        public bool Read
        {
            get
            {
                if (this.HalDocument.TryGetProperty("read", out JsonElement read) &&
                    (read.ValueKind == JsonValueKind.True || read.ValueKind == JsonValueKind.False))
                {
                    return read.GetBoolean();
                }

                return false;
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
        IJsonPropertyBag.IJsonPropertyBagEnumerator IJsonPropertyBag.EnumerateProperties()
        {
            return this.HalDocument.EnumerateProperties();
        }

        /// <inheritdoc/>
        public bool TryGetProperty(string propertyName, out JsonElement element)
        {
            return this.HalDocument.TryGetProperty(propertyName, out element);
        }

        /// <inheritdoc/>
        public bool TryGetProperty(ReadOnlySpan<byte> utf8PropertyName, out JsonElement element)
        {
            return this.HalDocument.TryGetProperty(utf8PropertyName, out element);
        }

        /// <inheritdoc/>
        public bool TryGetProperty(ReadOnlySpan<char> propertyName, out JsonElement element)
        {
            return this.HalDocument.TryGetProperty(propertyName, out element);
        }

        private class NotificationResourceConverter : JsonConverter<NotificationResource>
        {
            public override NotificationResource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var document = JsonDocument.ParseValue(ref reader);

                // If we are participating in serialization, rather than operating over the object model,
                // then we clone the Json element to detatch it from the underlying document stream.
                // Note that this will create a single copy of the elements within the Hal document, and that
                // they will be shared by any child documents.
                return document.RootElement.Clone().AsNotificationResource();
            }

            public override void Write(Utf8JsonWriter writer, NotificationResource value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value.HalDocument, options);
            }
        }
    }
}
