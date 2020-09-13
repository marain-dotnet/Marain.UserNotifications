// <copyright file="NotificationStatusResource.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Menes.Hal;

    /// <summary>
    /// The delivery/read status for a notification on a delivery channel.
    /// </summary>
    [JsonConverter(typeof(NotificationStatusResourceConverter))]
    public readonly struct NotificationStatusResource : IHalDocumentResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationStatusResource"/> class.
        /// </summary>
        /// <param name="source">The underlying JSON document for the object.</param>
        public NotificationStatusResource(in HalDocumentResource source)
        {
            if (!source.IsNotificationStatusResource())
            {
                throw new ArgumentException("The supplied document is not a NotificationStatusResource", nameof(source));
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
        /// Gets the unique identifier for the channel on which the notification was dispatched.
        /// </summary>
        public string ChannelId
        {
            get
            {
                if (this.HalDocument.TryGetProperty("channelId", out JsonElement channelId) &&
                    channelId.ValueKind == JsonValueKind.String)
                {
                    return channelId.GetString();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the delivery status for the channel. Not all channels will be able to track this, hence the need for more than just a true/false status.
        /// </summary>
        public NotificationStatusDeliveryStatus DeliveryStatus
        {
            get
            {
                if (this.HalDocument.TryGetProperty("deliveryStatus", out JsonElement deliveryStatus) &&
                    deliveryStatus.ValueKind == JsonValueKind.String)
                {
                    return Enum.Parse<NotificationStatusDeliveryStatus>(deliveryStatus.GetString());
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the date/time of the last update to the delivery status.
        /// </summary>
        public DateTimeOffset DeliveryStatusLastUpdated
        {
            get
            {
                if (this.HalDocument.TryGetProperty("deliveryStatusLastUpdated", out JsonElement deliveryStatusLastUpdated) &&
                    deliveryStatusLastUpdated.ValueKind == JsonValueKind.Object)
                {
                    return DateTimeOffset.Parse(deliveryStatusLastUpdated.GetString());
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the read status for the channel. Not all channels will be able to track this, hence the need for more than just a true/false status.
        /// </summary>
        public NotificationStatusReadStatus ReadStatus
        {
            get
            {
                if (this.HalDocument.TryGetProperty("readStatus", out JsonElement readStatus) &&
                    readStatus.ValueKind == JsonValueKind.String)
                {
                    return Enum.Parse<NotificationStatusReadStatus>(readStatus.GetString());
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the date/time of the last update to the delivery status.
        /// </summary>
        public DateTimeOffset ReadStatusLastUpdated
        {
            get
            {
                if (this.HalDocument.TryGetProperty("readStatusLastUpdated", out JsonElement readStatusLastUpdated) &&
                    readStatusLastUpdated.ValueKind == JsonValueKind.Object)
                {
                    return DateTimeOffset.Parse(readStatusLastUpdated.GetString());
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

        private class NotificationStatusResourceConverter : JsonConverter<NotificationStatusResource>
        {
            public override NotificationStatusResource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var document = JsonDocument.ParseValue(ref reader);

                // If we are participating in serialization, rather than operating over the object model,
                // then we clone the Json element to detatch it from the underlying document stream.
                // Note that this will create a single copy of the elements within the Hal document, and that
                // they will be shared by any child documents.
                return document.RootElement.Clone().AsNotificationStatusResource();
            }

            public override void Write(Utf8JsonWriter writer, NotificationStatusResource value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value.HalDocument, options);
            }
        }
    }
}
