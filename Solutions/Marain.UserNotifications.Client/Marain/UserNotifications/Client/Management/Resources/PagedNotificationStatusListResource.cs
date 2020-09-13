// <copyright file="PagedNotificationStatusListResource.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Menes.Hal;

    /// <summary>
    /// A paged list of notification statuses.
    /// </summary>
    [JsonConverter(typeof(PagedNotificationStatusListResourceConverter))]
    public readonly struct PagedNotificationStatusListResource : IHalDocumentResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedNotificationStatusListResource"/> class.
        /// </summary>
        /// <param name="source">The underlying JSON document for the object.</param>
        public PagedNotificationStatusListResource(in HalDocumentResource source)
        {
            if (!source.IsPagedNotificationStatusListResource())
            {
                throw new ArgumentException("The supplied document is not a PagedNotificationStatusListResource", nameof(source));
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
        /// Gets the notification statuses.
        /// </summary>
        public IEnumerable<NotificationStatusResource> Items
        {
            get
            {
                foreach (HalDocumentResource current in this.HalDocument.EnumerateEmbedded("items"))
                {
                    yield return current.AsNotificationStatusResource();
                }
            }
        }

        /// <summary>
        /// Gets the notification status links.
        /// </summary>
        public IEnumerable<WebLink> ItemLinks
        {
            get
            {
                return this.HalDocument.EnumerateLinks("items");
            }
        }

        /// <summary>
        /// Gets the link to the next page of notification status.
        /// </summary>
        public WebLink NextLink
        {
            get
            {
                return this.HalDocument.EnumerateLinks("next").SingleOrDefault();
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

        private class PagedNotificationStatusListResourceConverter : JsonConverter<PagedNotificationStatusListResource>
        {
            public override PagedNotificationStatusListResource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var document = JsonDocument.ParseValue(ref reader);

                // If we are participating in serialization, rather than operating over the object model,
                // then we clone the Json element to detatch it from the underlying document stream.
                // Note that this will create a single copy of the elements within the Hal document, and that
                // they will be shared by any child documents.
                return document.RootElement.Clone().AsPagedNotificationStatusListResource();
            }

            public override void Write(Utf8JsonWriter writer, PagedNotificationStatusListResource value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value.HalDocument, options);
            }
        }
    }
}
