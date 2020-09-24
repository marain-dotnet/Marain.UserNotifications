// <copyright file="HalDocumentResource.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#nullable enable
namespace Menes.Hal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// A document conforming to the HAL document specification.
    /// </summary>
    [JsonConverter(typeof(HalDocumentResourceConverter))]
    public readonly struct HalDocumentResource : IHalDocumentResource
    {
        /// <summary>
        /// The self relation.
        /// </summary>
        public const string SelfRel = "self";

        /// <summary>
        /// The contentType property.
        /// </summary>
        public const string ContentTypeProperty = "contentType";

        /// <summary>
        /// The _embedded property.
        /// </summary>
        public const string EmbeddedProperty = "_embedded";

        /// <summary>
        /// The _links property.
        /// </summary>
        public const string LinksProperty = "_links";

        private readonly JsonElement source;

        /// <summary>
        /// Initializes a new instance of the <see cref="HalDocumentResource"/> class.
        /// </summary>
        /// <param name="source">The source of the document.</param>
        public HalDocumentResource(JsonDocument source)
            : this(source.RootElement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HalDocumentResource"/> class.
        /// </summary>
        /// <param name="source">The source of the document.</param>
        public HalDocumentResource(in JsonElement source)
        {
            this.source = source;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public WebLink SelfLink
        {
            get
            {
                return this.EnumerateLinks(SelfRel).Single();
            }
        }

        /// <inheritdoc />
        public string ContentType
        {
            get
            {
                if (!this.source.TryGetProperty(ContentTypeProperty, out JsonElement element))
                {
                    throw new Exception("Invalid schema for HAL document - no content type.");
                }

                return element.GetString();
            }
        }

        /// <inheritdoc/>
        public HalDocumentResource HalDocument => this;

        /// <inheritdoc />
        public LinksEnumerator EnumerateLinks()
        {
            return new LinksEnumerator(this);
        }

        /// <inheritdoc />
        public PropertiesEnumerator EnumerateProperties()
        {
            return new PropertiesEnumerator(this);
        }

        /// <inheritdoc />
        IJsonPropertyBag.IJsonPropertyBagEnumerator IJsonPropertyBag.EnumerateProperties()
        {
            return new PropertiesEnumerator(this);
        }

        /// <inheritdoc />
        public bool TryGetProperty(string propertyName, out JsonElement element)
        {
            return this.source.TryGetProperty(propertyName, out element);
        }

        /// <inheritdoc/>
        public bool TryGetProperty(ReadOnlySpan<byte> utf8PropertyName, out JsonElement element)
        {
            return this.source.TryGetProperty(utf8PropertyName, out element);
        }

        /// <inheritdoc/>
        public bool TryGetProperty(ReadOnlySpan<char> propertyName, out JsonElement element)
        {
            return this.source.TryGetProperty(propertyName, out element);
        }

        /// <inheritdoc />
        public bool ContentTypeEquals(string text)
        {
            if (!this.source.TryGetProperty(ContentTypeProperty, out JsonElement element))
            {
                throw new Exception("Invalid schema for HAL document - no content type.");
            }

            return element.ValueEquals(text);
        }

        /// <inheritdoc />
        public bool ContentTypeEquals(ReadOnlySpan<byte> utf8Text)
        {
            if (!this.source.TryGetProperty(ContentTypeProperty, out JsonElement element))
            {
                throw new Exception("Invalid schema for HAL document - no content type.");
            }

            return element.ValueEquals(utf8Text);
        }

        /// <inheritdoc />
        public bool ContentTypeEquals(ReadOnlySpan<char> text)
        {
            if (!this.source.TryGetProperty(ContentTypeProperty, out JsonElement element))
            {
                throw new Exception("Invalid schema for HAL document - no content type.");
            }

            return element.ValueEquals(text);
        }

        /// <inheritdoc />
        public EmbeddedEnumerator EnumerateEmbedded()
        {
            return new EmbeddedEnumerator(this);
        }

        /// <inheritdoc />
        public LinksForRelationEnumerator EnumerateLinks(string relation)
        {
            return new LinksForRelationEnumerator(this, relation);
        }

        /// <summary>
        /// Enumerate the embedded object in the document.
        /// </summary>
        /// <param name="relation">The relation type to which to filter the embedded objects.</param>
        /// <returns>An <see cref="EmbeddedForRelationEnumerator"/> for the <see cref="HalDocumentResource"/>s embedded in the document for the specified relation.</returns>
        public EmbeddedForRelationEnumerator EnumerateEmbedded(string relation)
        {
            return new EmbeddedForRelationEnumerator(this, relation);
        }

        /// <summary>
        /// Write the <see cref="HalDocumentResource"/> to the <see cref="Utf8JsonWriter"/>.
        /// </summary>
        /// <param name="writer">The writer to which to write the weblink.</param>
        internal void Write(Utf8JsonWriter writer)
        {
            this.source.WriteTo(writer);
        }

        /// <summary>
        /// Represents an enumerator for the contents of the a specific embedded relation collection.
        /// </summary>
        [DebuggerDisplay("{Current,nq}")]
        public struct EmbeddedForRelationEnumerator : IEnumerable<HalDocumentResource>, IEnumerable, IEnumerator<HalDocumentResource>, IEnumerator, IDisposable
        {
            private readonly HalDocumentResource target;
            private readonly JsonElement relationElement;
            private JsonElement.ArrayEnumerator relationEnumerator;
            private bool hasEnumeratedRelationElement;

            /// <summary>
            /// Initializes a new instance of the <see cref="EmbeddedForRelationEnumerator"/> struct.
            /// </summary>
            /// <param name="target">The target <see cref="HalDocumentResource"/>.</param>
            /// <param name="relation">The relation for which to get the links.</param>
            internal EmbeddedForRelationEnumerator(HalDocumentResource target, string relation)
            {
                this.target = target;
                this.hasEnumeratedRelationElement = false;

                if (this.target.source.TryGetProperty(EmbeddedProperty, out JsonElement links) && links.TryGetProperty(relation, out JsonElement element))
                {
                    this.relationElement = element;
                    if (this.relationElement.ValueKind == JsonValueKind.Array)
                    {
                        this.relationEnumerator = this.relationElement.EnumerateArray();
                    }
                    else
                    {
                        this.relationEnumerator = default;
                    }
                }
                else
                {
                    this.relationEnumerator = default;
                    this.relationElement = default;
                }
            }

            /// <inheritdoc/>
            public HalDocumentResource Current
            {
                get
                {
                    return this.GetCurrentHalDocument();
                }
            }

            /// <inheritdoc/>
            object IEnumerator.Current => this.Current;

            /// <summary>
            /// Returns an enumerator that iterates the links on the document for a particular relation.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the links on the document for the relation.</returns>
            public EmbeddedForRelationEnumerator GetEnumerator()
            {
                EmbeddedForRelationEnumerator result = this;
                result.Reset();
                return result;
            }

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            IEnumerator<HalDocumentResource> IEnumerable<HalDocumentResource>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                this.relationEnumerator.Dispose();
            }

            /// <inheritdoc/>
            public void Reset()
            {
                this.relationEnumerator.Reset();
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                if (this.relationElement.ValueKind == JsonValueKind.Array)
                {
                    return this.relationEnumerator.MoveNext();
                }

                if (this.relationElement.ValueKind == JsonValueKind.Undefined)
                {
                    return false;
                }

                if (this.hasEnumeratedRelationElement)
                {
                    return false;
                }

                this.hasEnumeratedRelationElement = true;
                return true;
            }

            private HalDocumentResource GetCurrentHalDocument()
            {
                return this.relationElement.ValueKind == JsonValueKind.Array ?
                                                new HalDocumentResource(this.relationEnumerator.Current) :
                                                new HalDocumentResource(this.relationElement);
            }
        }

        /// <summary>
        /// Represents an enumerator for the properties of the resource.
        /// </summary>
        [DebuggerDisplay("{Current,nq}")]
        public struct PropertiesEnumerator : IJsonPropertyBag.IJsonPropertyBagEnumerator
        {
            private readonly HalDocumentResource target;
            private JsonElement.ObjectEnumerator enumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="PropertiesEnumerator"/> struct.
            /// </summary>
            /// <param name="target">The target <see cref="HalDocumentResource"/>.</param>
            internal PropertiesEnumerator(HalDocumentResource target)
            {
                this.target = target;
                this.enumerator = this.target.source.EnumerateObject();
            }

            /// <inheritdoc/>
            public JsonProperty Current
            {
                get
                {
                    return this.enumerator.Current;
                }
            }

            /// <inheritdoc/>
            object IEnumerator.Current => this.Current;

            /// <summary>
            /// Returns an enumerator that iterates the links on the document for a particular relation.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the links on the document for the relation.</returns>
            public PropertiesEnumerator GetEnumerator()
            {
                PropertiesEnumerator result = this;
                result.Reset();
                return result;
            }

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            IEnumerator<JsonProperty> IEnumerable<JsonProperty>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                this.enumerator.Dispose();
            }

            /// <inheritdoc/>
            public void Reset()
            {
                this.enumerator.Reset();
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                if (this.enumerator.MoveNext())
                {
                    // Skip _links and _embedded
                    if (this.enumerator.Current.NameEquals(LinksProperty) || this.enumerator.Current.NameEquals(EmbeddedProperty))
                    {
                        return this.MoveNext();
                    }

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Represents an enumerator for the contents of the a specific link relation collection.
        /// </summary>
        [DebuggerDisplay("{Current,nq}")]
        public struct LinksForRelationEnumerator : IEnumerable<WebLink>, IEnumerable, IEnumerator<WebLink>, IEnumerator, IDisposable
        {
            private readonly HalDocumentResource target;
            private readonly JsonElement relationElement;
            private bool hasEnumeratedRelationElement;
            private JsonElement.ArrayEnumerator relationEnumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="LinksEnumerator"/> struct.
            /// </summary>
            /// <param name="target">The target <see cref="HalDocumentResource"/>.</param>
            /// <param name="relation">The relation for which to get the links.</param>
            internal LinksForRelationEnumerator(HalDocumentResource target, string relation)
            {
                this.target = target;
                this.hasEnumeratedRelationElement = false;

                if (this.target.source.TryGetProperty(LinksProperty, out JsonElement links) && links.TryGetProperty(relation, out JsonElement element))
                {
                    this.relationElement = element;
                    if (this.relationElement.ValueKind == JsonValueKind.Array)
                    {
                        this.relationEnumerator = this.relationElement.EnumerateArray();
                    }
                    else
                    {
                        this.relationEnumerator = default;
                    }
                }
                else
                {
                    this.relationEnumerator = default;
                    this.relationElement = default;
                }
            }

            /// <inheritdoc/>
            public WebLink Current
            {
                get
                {
                    return this.GetCurrentWebLink();
                }
            }

            /// <inheritdoc/>
            object IEnumerator.Current => this.Current;

            /// <summary>
            /// Returns an enumerator that iterates the links on the document for a particular relation.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the links on the document for the relation.</returns>
            public LinksForRelationEnumerator GetEnumerator()
            {
                LinksForRelationEnumerator result = this;
                result.Reset();
                return result;
            }

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            IEnumerator<WebLink> IEnumerable<WebLink>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                this.relationEnumerator.Dispose();
            }

            /// <inheritdoc/>
            public void Reset()
            {
                this.relationEnumerator.Reset();
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                if (this.relationElement.ValueKind == JsonValueKind.Array)
                {
                    return this.relationEnumerator.MoveNext();
                }

                if (this.relationElement.ValueKind == JsonValueKind.Undefined)
                {
                    return false;
                }

                if (this.hasEnumeratedRelationElement)
                {
                    return false;
                }

                this.hasEnumeratedRelationElement = true;
                return true;
            }

            private WebLink GetCurrentWebLink()
            {
                return this.relationElement.ValueKind == JsonValueKind.Array ?
                                                new WebLink(this.relationEnumerator.Current) :
                                                new WebLink(this.relationElement);
            }
        }

        /// <summary>
        /// Represents an enumerator for the contents of the link relation collection.
        /// </summary>
        [DebuggerDisplay("{Current,nq}")]
        public struct LinksEnumerator : IEnumerable<LinkRelation>, IEnumerable, IEnumerator<LinkRelation>, IEnumerator, IDisposable
        {
            private readonly HalDocumentResource target;
            private JsonElement.ObjectEnumerator linksEnumerator;
            private JsonElement.ArrayEnumerator arrayEnumerator;
            private JsonProperty? currentProperty;

            /// <summary>
            /// Initializes a new instance of the <see cref="LinksEnumerator"/> struct.
            /// </summary>
            /// <param name="target">The target <see cref="HalDocumentResource"/>.</param>
            internal LinksEnumerator(HalDocumentResource target)
            {
                this.target = target;
                if (this.target.source.TryGetProperty(LinksProperty, out JsonElement links))
                {
                    this.linksEnumerator = links.EnumerateObject();
                }
                else
                {
                    this.linksEnumerator = default;
                }

                this.arrayEnumerator = default;
                this.currentProperty = null;
                this.target = target;
            }

            /// <inheritdoc/>
            public LinkRelation Current
            {
                get
                {
                    if (this.currentProperty.HasValue)
                    {
                        return new LinkRelation(
                            this.currentProperty.Value,
                            this.GetCurrentWebLink());
                    }

                    return default;
                }
            }

            /// <inheritdoc/>
            object IEnumerator.Current => this.Current;

            /// <summary>
            /// Returns an enumerator that iterates the links on the document.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the links on the document.</returns>
            public LinksEnumerator GetEnumerator()
            {
                LinksEnumerator result = this;
                result.Reset();
                return result;
            }

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            IEnumerator<LinkRelation> IEnumerable<LinkRelation>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                this.linksEnumerator.Dispose();
                this.arrayEnumerator.Dispose();
            }

            /// <inheritdoc/>
            public void Reset()
            {
                this.currentProperty = null;
                this.arrayEnumerator = default;
                this.linksEnumerator.Reset();
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                if (this.linksEnumerator.Current.Value.ValueKind == JsonValueKind.Array)
                {
                    if (this.arrayEnumerator.MoveNext())
                    {
                        return true;
                    }
                }

                if (!this.linksEnumerator.MoveNext())
                {
                    return false;
                }

                if (this.linksEnumerator.Current.Value.ValueKind == JsonValueKind.Array)
                {
                    this.arrayEnumerator = this.linksEnumerator.Current.Value.EnumerateArray();
                    return this.MoveNext();
                }

                return true;
            }

            private WebLink GetCurrentWebLink()
            {
                return this.linksEnumerator.Current.Value.ValueKind == JsonValueKind.Array ?
                                                new WebLink(this.arrayEnumerator.Current) :
                                                new WebLink(this.linksEnumerator.Current.Value);
            }
        }

        /// <summary>
        /// Represents an enumerator for the contents of the embedded resource relation collection.
        /// </summary>
        [DebuggerDisplay("{Current,nq}")]
        public struct EmbeddedEnumerator : IEnumerable<EmbeddedRelation>, IEnumerable, IEnumerator<EmbeddedRelation>, IEnumerator, IDisposable
        {
            private readonly HalDocumentResource target;
            private JsonElement.ObjectEnumerator embeddedEnumerator;
            private JsonElement.ArrayEnumerator arrayEnumerator;
            private JsonProperty? currentProperty;

            /// <summary>
            /// Initializes a new instance of the <see cref="LinksEnumerator"/> struct.
            /// </summary>
            /// <param name="target">The target <see cref="HalDocumentResource"/>.</param>
            internal EmbeddedEnumerator(HalDocumentResource target)
            {
                this.target = target;
                if (this.target.source.TryGetProperty(EmbeddedProperty, out JsonElement links))
                {
                    this.embeddedEnumerator = links.EnumerateObject();
                }
                else
                {
                    this.embeddedEnumerator = default;
                }

                this.arrayEnumerator = default;
                this.currentProperty = null;
            }

            /// <inheritdoc/>
            public EmbeddedRelation Current
            {
                get
                {
                    if (this.currentProperty.HasValue)
                    {
                        return new EmbeddedRelation(
                            this.currentProperty.Value,
                            this.GetCurrentHalDocument());
                    }

                    return default;
                }
            }

            /// <inheritdoc/>
            object IEnumerator.Current => this.Current;

            /// <summary>
            /// Returns an enumerator that iterates the embedded resources on the document.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the embedded resources on the document.</returns>
            public EmbeddedEnumerator GetEnumerator()
            {
                EmbeddedEnumerator result = this;
                result.Reset();
                return result;
            }

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            IEnumerator<EmbeddedRelation> IEnumerable<EmbeddedRelation>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                this.embeddedEnumerator.Dispose();
                this.arrayEnumerator.Dispose();
            }

            /// <inheritdoc/>
            public void Reset()
            {
                this.currentProperty = null;
                this.arrayEnumerator = default;
                this.embeddedEnumerator.Reset();
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                if (this.embeddedEnumerator.Current.Value.ValueKind == JsonValueKind.Array)
                {
                    if (this.arrayEnumerator.MoveNext())
                    {
                        return true;
                    }
                }

                if (!this.embeddedEnumerator.MoveNext())
                {
                    return false;
                }

                if (this.embeddedEnumerator.Current.Value.ValueKind == JsonValueKind.Array)
                {
                    this.arrayEnumerator = this.embeddedEnumerator.Current.Value.EnumerateArray();
                    return this.MoveNext();
                }

                return true;
            }

            private HalDocumentResource GetCurrentHalDocument()
            {
                return this.embeddedEnumerator.Current.Value.ValueKind == JsonValueKind.Array ?
                                                new HalDocumentResource(this.arrayEnumerator.Current) :
                                                new HalDocumentResource(this.embeddedEnumerator.Current.Value);
            }
        }

        private class HalDocumentResourceConverter : JsonConverter<HalDocumentResource>
        {
            public override HalDocumentResource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var document = JsonDocument.ParseValue(ref reader);

                // If we are participating in serialization, rather than operating over the object model,
                // then we clone the Json element to detatch it from the underlying document stream.
                // Note that this will create a single copy of the elements within the Hal document, and that
                // they will be shared by any child documents.
                return new HalDocumentResource(document.RootElement.Clone());
            }

            public override void Write(Utf8JsonWriter writer, HalDocumentResource value, JsonSerializerOptions options)
            {
                value.Write(writer);
            }
        }
    }
}
