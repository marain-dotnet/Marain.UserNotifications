// <copyright file="WebLinkBuilder.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Menes.Hal
{
    using System;
    using System.IO;
    using System.Text.Json;

    /// <summary>
    /// Builder for a hypermedia link to a related resource. Follows the general pattern described
    /// in RFC 8288 (https://tools.ietf.org/html/rfc8288).
    /// </summary>
    public struct WebLinkBuilder
    {
        /// <summary>
        /// Creates a weblink from the component values.
        /// </summary>
        /// <param name="href">The URI of the target resource.</param>
        /// <param name="name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        /// <param name="isTemplated">An optional value that indicates whether the link is templated.</param>
        /// <param name="title">The (optional) human-readable title of the link.</param>
        /// <param name="profile">Additional (optional) semantics of the target resource.</param>
        /// <param name="type">The (optional) media type indication of the target resource.</param>
        /// <param name="hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        /// <returns>A <see cref="WebLink"/> initialized with the specified values.</returns>
        public WebLink CreateWebLink(string href, string? name = null, bool? isTemplated = null, string? title = null, string? profile = null, string? type = null, string? hreflang = null)
        {
            if (href == null)
            {
                throw new ArgumentNullException(nameof(href));
            }

            using var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream);
            WebLinkWriter.WriteWebLink(writer, href, name, isTemplated, title, profile, type, hreflang);

            stream.Flush();
            stream.Position = 0;

            return new WebLink(JsonDocument.Parse(stream).RootElement.Clone());
        }

        /// <summary>
        /// Write a WebLink to a Utf8JsonWriter.
        /// </summary>
        /// <param name="href">The URI of the target resource.</param>
        /// <param name="name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        /// <param name="isTemplated">An optional value that indicates whether the link is templated.</param>
        /// <param name="title">The (optional) human-readable title of the link.</param>
        /// <param name="profile">Additional (optional) semantics of the target resource.</param>
        /// <param name="type">The (optional) media type indication of the target resource.</param>
        /// <param name="hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        /// <returns>A <see cref="WebLink"/> initialized with the specified values.</returns>
        public WebLink CreateWebLink(ReadOnlySpan<char> href, ReadOnlySpan<char> name = default, bool isTemplated = default, ReadOnlySpan<char> title = default, ReadOnlySpan<char> profile = default, ReadOnlySpan<char> type = default, ReadOnlySpan<char> hreflang = default)
        {
            using var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream);
            WebLinkWriter.WriteWebLink(writer, href, name, isTemplated, title, profile, type, hreflang);

            stream.Flush();
            stream.Position = 0;

            return new WebLink(JsonDocument.Parse(stream).RootElement.Clone());
        }

        /// <summary>
        /// Create a WebLink from utf8 text.
        /// </summary>
        /// <param name="utf8Href">The URI of the target resource.</param>
        /// <param name="utf8Name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        /// <param name="isTemplated">An optional value that indicates whether the link is templated.</param>
        /// <param name="utf8Title">The (optional) human-readable title of the link.</param>
        /// <param name="utf8Profile">Additional (optional) semantics of the target resource.</param>
        /// <param name="utf8Type">The (optional) media type indication of the target resource.</param>
        /// <param name="utf8Hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        /// <returns>A <see cref="WebLink"/> initialized with the specified values.</returns>
        public WebLink CreateWebLink(ReadOnlySpan<byte> utf8Href, ReadOnlySpan<byte> utf8Name = default, bool isTemplated = default, ReadOnlySpan<byte> utf8Title = default, ReadOnlySpan<byte> utf8Profile = default, ReadOnlySpan<byte> utf8Type = default, ReadOnlySpan<byte> utf8Hreflang = default)
        {
            using var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream);
            WebLinkWriter.WriteWebLink(writer, utf8Href, utf8Name, isTemplated, utf8Title, utf8Profile, utf8Type, utf8Hreflang);

            stream.Flush();
            stream.Position = 0;

            return new WebLink(JsonDocument.Parse(stream).RootElement.Clone());
        }

        /// <summary>
        /// Create a weblink from existing <see cref="JsonElement"/>s.
        /// </summary>
        /// <param name="href">The URI of the target resource.</param>
        /// <param name="name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        /// <param name="isTemplated">An optional value that indicates whether the link is templated.</param>
        /// <param name="title">The (optional) human-readable title of the link.</param>
        /// <param name="profile">Additional (optional) semantics of the target resource.</param>
        /// <param name="type">The (optional) media type indication of the target resource.</param>
        /// <param name="hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        /// <returns>A <see cref="WebLink"/> initialized with the specified values.</returns>
        public WebLink CreateWebLink(in JsonElement href, in JsonElement name = default, in JsonElement isTemplated = default, in JsonElement title = default, in JsonElement profile = default, in JsonElement type = default, in JsonElement hreflang = default)
        {
            using var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream);
            WebLinkWriter.WriteWebLink(writer, href, name, isTemplated, title, profile, type, hreflang);

            stream.Flush();
            stream.Position = 0;

            return new WebLink(JsonDocument.Parse(stream).RootElement.Clone());
        }
    }
}
