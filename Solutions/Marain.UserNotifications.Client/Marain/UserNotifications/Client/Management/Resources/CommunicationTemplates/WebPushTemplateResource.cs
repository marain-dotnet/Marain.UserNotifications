// <copyright file="WebPushTemplateResource.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources.CommunicationTemplates
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using Menes.Hal;

    /// <summary>
    /// The web push template resource structure.
    /// </summary>
    [JsonConverter(typeof(WebPushTemplateResourceConverter))]
    public class WebPushTemplateResource : IHalDocumentResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebPushTemplateResource"/> class.
        /// </summary>
        /// <param name="source">The underlying JSON document for the object.</param>
        public WebPushTemplateResource(in HalDocumentResource source)
        {
            if (!source.IsWebPushTemplateResource())
            {
                throw new ArgumentException("The supplied document is not a WebPushTemplateResource", nameof(source));
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
        /// Gets the notification type of a WebPush notification.
        /// </summary>
        public string NotificationType
        {
            get
            {
                if (this.HalDocument.TryGetProperty("notificationType", out JsonElement notificationType) &&
                    notificationType.ValueKind == JsonValueKind.String)
                {
                    return notificationType.GetString();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the body of a WebPush notification.
        /// </summary>
        public string Body
        {
            get
            {
                if (this.HalDocument.TryGetProperty("body", out JsonElement body) &&
                    body.ValueKind == JsonValueKind.String)
                {
                    return body.GetString();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the title of the WebPush notification.
        /// </summary>
        public string Title
        {
            get
            {
                if (this.HalDocument.TryGetProperty("title", out JsonElement title) &&
                    title.ValueKind == JsonValueKind.String)
                {
                    return title.GetString();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets the Base64 image of a WebPush notification.
        /// </summary>
        public string Image
        {
            get
            {
                if (this.HalDocument.TryGetProperty("image", out JsonElement image) &&
                    image.ValueKind == JsonValueKind.String)
                {
                    return image.GetString();
                }

                throw new Exception("Schema violation - this value is required.");
            }
        }

        /// <summary>
        /// Gets navigation url on click of the notification.
        /// </summary>
        public string ActionUrl
        {
            get
            {
                if (this.HalDocument.TryGetProperty("actionUrl", out JsonElement actionUrl) &&
                    actionUrl.ValueKind == JsonValueKind.String)
                {
                    return actionUrl.GetString();
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

        private class WebPushTemplateResourceConverter : JsonConverter<WebPushTemplateResource>
        {
            public override WebPushTemplateResource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var document = JsonDocument.ParseValue(ref reader);

                // If we are participating in serialization, rather than operating over the object model,
                // then we clone the Json element to detatch it from the underlying document stream.
                // Note that this will create a single copy of the elements within the Hal document, and that
                // they will be shared by any child documents.
                return document.RootElement.Clone().AsWebPushTemplateResource();
            }

            public override void Write(Utf8JsonWriter writer, WebPushTemplateResource value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value.HalDocument, options);
            }
        }
    }
}