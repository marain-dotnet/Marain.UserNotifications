// <copyright file="WebLink.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Menes.Hal
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a hypermedia link to a related resource. Follows the general pattern described
    /// in RFC 8288 (https://tools.ietf.org/html/rfc8288).
    /// </summary>
    [JsonConverter(typeof(WebLinkConverter))]
    public readonly struct WebLink
    {
        /// <summary>
        /// The href property.
        /// </summary>
        public const string HrefProperty = "href";

        /// <summary>
        /// The name property.
        /// </summary>
        public const string NameProperty = "name";

        /// <summary>
        /// The isTemplated property.
        /// </summary>
        public const string IsTemplatedProperty = "isTemplated";

        /// <summary>
        /// The title property.
        /// </summary>
        public const string TitleProperty = "title";

        /// <summary>
        /// The profile property.
        /// </summary>
        public const string ProfileProperty = "profile";

        /// <summary>
        /// The type property.
        /// </summary>
        public const string TypeProperty = "type";

        /// <summary>
        /// The hreflang property.
        /// </summary>
        public const string HreflangProperty = "hreflang";

        private readonly JsonElement source;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebLink"/> struct.
        /// </summary>
        /// <param name="source">The json element representing the web link.</param>
        public WebLink(in JsonElement source)
        {
            this.source = source;
        }

        /// <summary>
        /// Gets the URI of the target resource.
        /// </summary>
        /// <remarks>
        /// Either a URI [RFC3986] or URI Template [RFC6570] of the target
        /// resource.
        /// </remarks>
        public string Href => this.source.GetProperty(HrefProperty).GetString();

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <remarks>
        /// When present, may be used as a secondary key for selecting link
        /// objects that contain the same relation type.
        /// </remarks>
        public string? Name => this.source.GetProperty(NameProperty).GetString();

        /// <summary>
        /// Gets a value that indicates whether the link is templated (or null if the property is not set).
        /// </summary>
        /// <remarks>
        /// Is true when the link object's href property is a URI Template.
        /// Defaults to false.
        /// </remarks>
        public bool? IsTemplated => this.source.GetProperty(IsTemplatedProperty).GetBoolean();

        /// <summary>
        /// Gets a value for the human-readable title of the link.
        /// </summary>
        /// <remarks>
        /// When present, is used to label the destination of a link such that
        /// it can be used as a human-readable identifier (e.g. a menu entry)
        /// in the language indicated by the Content-Language header (if
        /// present).
        /// </remarks>
        public string? Title => this.source.GetProperty(TitleProperty).GetString();

        /// <summary>
        /// Gets additional semantics of the target resource.
        /// </summary>
        /// <remarks>
        /// A URI that, when dereferenced, results in a profile to allow
        /// clients to learn about additional semantics (constraints,
        /// conventions, extensions) that are associated with the target
        /// resource representation, in addition to those defined by the HAL
        /// media type and relations.
        /// </remarks>
        public string? Profile => this.source.GetProperty(ProfileProperty).GetString();

        /// <summary>
        /// Gets media type indication of the target resource.
        /// </summary>
        /// <remarks>
        /// When present, used as a hint to indicate the media type expected
        /// when dereferencing the target resource.
        /// </remarks>
        public string? Type => this.source.GetProperty(TypeProperty).GetString();

        /// <summary>
        /// Gets the language indication of the target resource [RFC5988].
        /// </summary>
        /// <remarks>
        /// When present, is a hint in RFC5646 format indicating what the
        /// language of the result of dereferencing the link should be.  Note
        /// that this is only a hint; for example, it does not override the
        /// Content-Language header of a HTTP response obtained by actually
        /// following the link.
        /// </remarks>
        public string? Hreflang => this.source.GetProperty(HreflangProperty).GetString();

        /// <summary>
        /// Determine if the href matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the href matches the given text.</returns>
        public bool HrefEquals(string text)
        {
            return this.source.GetProperty(HrefProperty).ValueEquals(text);
        }

        /// <summary>
        /// Determine if the href matches a given value, without allocating a string.
        /// </summary>
        /// <param name="utf8Text">The text to compare.</param>
        /// <returns>True if the href matches the given text.</returns>
        public bool HrefEquals(ReadOnlySpan<byte> utf8Text)
        {
            return this.source.GetProperty(HrefProperty).ValueEquals(utf8Text);
        }

        /// <summary>
        /// Determine if the href matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the href matches the given text.</returns>
        public bool HrefEquals(ReadOnlySpan<char> text)
        {
            return this.source.GetProperty(HrefProperty).ValueEquals(text);
        }

        /// <summary>
        /// Determine if the name matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the name matches the given text.</returns>
        public bool NameEquals(string text)
        {
            return this.source.GetProperty(NameProperty).ValueEquals(text);
        }

        /// <summary>
        /// Determine if the name matches a given value, without allocating a string.
        /// </summary>
        /// <param name="utf8Text">The text to compare.</param>
        /// <returns>True if the name matches the given text.</returns>
        public bool NameEquals(ReadOnlySpan<byte> utf8Text)
        {
            return this.source.GetProperty(NameProperty).ValueEquals(utf8Text);
        }

        /// <summary>
        /// Determine if the name matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the name matches the given text.</returns>
        public bool NameEquals(ReadOnlySpan<char> text)
        {
            return this.source.GetProperty(NameProperty).ValueEquals(text);
        }

        /// <summary>
        /// Determine if the title matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the title matches the given text.</returns>
        public bool TitleEquals(string text)
        {
            return this.source.GetProperty(TitleProperty).ValueEquals(text);
        }

        /// <summary>
        /// Determine if the title matches a given value, without allocating a string.
        /// </summary>
        /// <param name="utf8Text">The text to compare.</param>
        /// <returns>True if the title matches the given text.</returns>
        public bool TitleEquals(ReadOnlySpan<byte> utf8Text)
        {
            return this.source.GetProperty(TitleProperty).ValueEquals(utf8Text);
        }

        /// <summary>
        /// Determine if the title matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the title matches the given text.</returns>
        public bool TitleEquals(ReadOnlySpan<char> text)
        {
            return this.source.GetProperty(TitleProperty).ValueEquals(text);
        }

        /// <summary>
        /// Determine if the profile matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the profile matches the given text.</returns>
        public bool ProfileEquals(string text)
        {
            return this.source.GetProperty(ProfileProperty).ValueEquals(text);
        }

        /// <summary>
        /// Determine if the profile matches a given value, without allocating a string.
        /// </summary>
        /// <param name="utf8Text">The text to compare.</param>
        /// <returns>True if the profile matches the given text.</returns>
        public bool ProfileEquals(ReadOnlySpan<byte> utf8Text)
        {
            return this.source.GetProperty(ProfileProperty).ValueEquals(utf8Text);
        }

        /// <summary>
        /// Determine if the profile matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the profile matches the given text.</returns>
        public bool ProfileEquals(ReadOnlySpan<char> text)
        {
            return this.source.GetProperty(ProfileProperty).ValueEquals(text);
        }

        /// <summary>
        /// Determine if the type matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the type matches the given text.</returns>
        public bool TypeEquals(string text)
        {
            return this.source.GetProperty(TypeProperty).ValueEquals(text);
        }

        /// <summary>
        /// Determine if the type matches a given value, without allocating a string.
        /// </summary>
        /// <param name="utf8Text">The text to compare.</param>
        /// <returns>True if the type matches the given text.</returns>
        public bool TypeEquals(ReadOnlySpan<byte> utf8Text)
        {
            return this.source.GetProperty(TypeProperty).ValueEquals(utf8Text);
        }

        /// <summary>
        /// Determine if the type matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the type matches the given text.</returns>
        public bool TypeEquals(ReadOnlySpan<char> text)
        {
            return this.source.GetProperty(TypeProperty).ValueEquals(text);
        }

        /// <summary>
        /// Determine if the hreflang matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the hreflang matches the given text.</returns>
        public bool HreflangEquals(string text)
        {
            return this.source.GetProperty(HreflangProperty).ValueEquals(text);
        }

        /// <summary>
        /// Determine if the hreflang matches a given value, without allocating a string.
        /// </summary>
        /// <param name="utf8Text">The text to compare.</param>
        /// <returns>True if the hreflang matches the given text.</returns>
        public bool HreflangEquals(ReadOnlySpan<byte> utf8Text)
        {
            return this.source.GetProperty(HreflangProperty).ValueEquals(utf8Text);
        }

        /// <summary>
        /// Determine if the hreflang matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the hreflang matches the given text.</returns>
        public bool HreflangEquals(ReadOnlySpan<char> text)
        {
            return this.source.GetProperty(HreflangProperty).ValueEquals(text);
        }

        /// <summary>
        /// Write the weblink to the <see cref="Utf8JsonWriter"/>.
        /// </summary>
        /// <param name="writer">The writer to which to write the weblink.</param>
        internal void Write(Utf8JsonWriter writer)
        {
            this.source.WriteTo(writer);
        }

        private class WebLinkConverter : JsonConverter<WebLink>
        {
            public override WebLink Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // In the serialization mode we have to clone the link out of the initial document. This is less
                // efficient than reading it from the underlying Json document.
                return new WebLink(JsonDocument.ParseValue(ref reader).RootElement.Clone());
            }

            public override void Write(Utf8JsonWriter writer, WebLink value, JsonSerializerOptions options)
            {
                value.Write(writer);
            }
        }
    }
}
