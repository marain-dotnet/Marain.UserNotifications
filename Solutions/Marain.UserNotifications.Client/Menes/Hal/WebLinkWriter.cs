// <copyright file="WebLinkWriter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#nullable enable
namespace Menes.Hal
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    /// <summary>
    /// Writer for a hypermedia link to a related resource. Follows the general pattern described
    /// in RFC 8288 (https://tools.ietf.org/html/rfc8288).
    /// </summary>
    public struct WebLinkWriter
    {
        private State state;

        private RequiredProperties requiredProperties;

        private enum State
        {
            NotStarted,
            Started,
            Completed,
        }

        [Flags]
        private enum RequiredProperties
        {
            None = 0,
            Href = 1,
            All = Href,
        }

        /// <summary>
        /// Write a WebLink to a Utf8JsonWriter.
        /// </summary>
        /// <param name="writer">The writer to which to write the link.</param>
        /// <param name="webLink">The weblink to write.</param>
        public static void WriteWebLink(Utf8JsonWriter writer, in WebLink webLink)
        {
            webLink.Write(writer);
        }

        /// <summary>
        /// Begin writing a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the <see cref="WebLink"/>.</param>
        /// <returns>The <see cref="WebLinkWriter"/> to use for continuing to write the link.</returns>
        public static WebLinkWriter BeginWriteWebLink(Utf8JsonWriter writer)
        {
            WebLinkWriter webLinkWriter = default;

            writer.WriteStartObject();
            webLinkWriter.state = State.Started;
            return webLinkWriter;
        }

        /// <summary>
        /// Write a WebLink to a Utf8JsonWriter.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="href">The URI of the target resource.</param>
        /// <param name="name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        /// <param name="isTemplated">An optional value that indicates whether the link is templated.</param>
        /// <param name="title">The (optional) human-readable title of the link.</param>
        /// <param name="profile">Additional (optional) semantics of the target resource.</param>
        /// <param name="type">The (optional) media type indication of the target resource.</param>
        /// <param name="hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        public static void WriteWebLink(Utf8JsonWriter writer, string href, string? name = null, bool? isTemplated = null, string? title = null, string? profile = null, string? type = null, string? hreflang = null)
        {
            writer.WriteStartObject();
            writer.WriteString(WebLink.HrefProperty, href);

            if (name != null)
            {
                writer.WriteString(WebLink.NameProperty, name);
            }

            if (isTemplated != null)
            {
                writer.WriteBoolean(WebLink.IsTemplatedProperty, isTemplated.Value);
            }

            if (title != null)
            {
                writer.WriteString(WebLink.TitleProperty, title);
            }

            if (profile != null)
            {
                writer.WriteString(WebLink.ProfileProperty, profile);
            }

            if (type != null)
            {
                writer.WriteString(WebLink.TypeProperty, type);
            }

            if (hreflang != null)
            {
                writer.WriteString(WebLink.HreflangProperty, hreflang);
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Write a WebLink to a Utf8JsonWriter.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="href">The URI of the target resource.</param>
        /// <param name="name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        /// <param name="isTemplated">An optional value that indicates whether the link is templated.</param>
        /// <param name="title">The (optional) human-readable title of the link.</param>
        /// <param name="profile">Additional (optional) semantics of the target resource.</param>
        /// <param name="type">The (optional) media type indication of the target resource.</param>
        /// <param name="hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        public static void WriteWebLink(Utf8JsonWriter writer, ReadOnlySpan<char> href, ReadOnlySpan<char> name = default, bool isTemplated = default, ReadOnlySpan<char> title = default, ReadOnlySpan<char> profile = default, ReadOnlySpan<char> type = default, ReadOnlySpan<char> hreflang = default)
        {
            writer.WriteStartObject();

            writer.WriteString(WebLink.HrefProperty, href);

            if (name.Length > 0)
            {
                writer.WriteString(WebLink.NameProperty, name);
            }

            if (isTemplated)
            {
                writer.WriteBoolean(WebLink.IsTemplatedProperty, isTemplated);
            }

            if (title.Length > 0)
            {
                writer.WriteString(WebLink.TitleProperty, title);
            }

            if (profile.Length > 0)
            {
                writer.WriteString(WebLink.ProfileProperty, profile);
            }

            if (type.Length > 0)
            {
                writer.WriteString(WebLink.TypeProperty, type);
            }

            if (hreflang.Length > 0)
            {
                writer.WriteString(WebLink.HreflangProperty, hreflang);
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Write a WebLink to a Utf8JsonWriter.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="utf8Href">The URI of the target resource.</param>
        /// <param name="utf8Name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        /// <param name="isTemplated">An optional value that indicates whether the link is templated.</param>
        /// <param name="utf8Title">The (optional) human-readable title of the link.</param>
        /// <param name="utf8Profile">Additional (optional) semantics of the target resource.</param>
        /// <param name="utf8Type">The (optional) media type indication of the target resource.</param>
        /// <param name="utf8Hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        public static void WriteWebLink(Utf8JsonWriter writer, ReadOnlySpan<byte> utf8Href, ReadOnlySpan<byte> utf8Name = default, bool isTemplated = default, ReadOnlySpan<byte> utf8Title = default, ReadOnlySpan<byte> utf8Profile = default, ReadOnlySpan<byte> utf8Type = default, ReadOnlySpan<byte> utf8Hreflang = default)
        {
            writer.WriteStartObject();
            writer.WriteString(WebLink.HrefProperty, utf8Href);

            if (utf8Name.Length > 0)
            {
                writer.WriteString(WebLink.NameProperty, utf8Name);
            }

            if (isTemplated)
            {
                writer.WriteBoolean(WebLink.IsTemplatedProperty, isTemplated);
            }

            if (utf8Title.Length > 0)
            {
                writer.WriteString(WebLink.TitleProperty, utf8Title);
            }

            if (utf8Profile.Length > 0)
            {
                writer.WriteString(WebLink.ProfileProperty, utf8Profile);
            }

            if (utf8Type.Length > 0)
            {
                writer.WriteString(WebLink.TypeProperty, utf8Type);
            }

            if (utf8Hreflang.Length > 0)
            {
                writer.WriteString(WebLink.HreflangProperty, utf8Hreflang);
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Writes a weblink from existing <see cref="JsonElement"/>s.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="href">The URI of the target resource.</param>
        /// <param name="name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        /// <param name="isTemplated">An optional value that indicates whether the link is templated.</param>
        /// <param name="title">The (optional) human-readable title of the link.</param>
        /// <param name="profile">Additional (optional) semantics of the target resource.</param>
        /// <param name="type">The (optional) media type indication of the target resource.</param>
        /// <param name="hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        public static void WriteWebLink(Utf8JsonWriter writer, in JsonElement href, in JsonElement name = default, JsonElement isTemplated = default, in JsonElement title = default, in JsonElement profile = default, in JsonElement type = default, in JsonElement hreflang = default)
        {
            if (href.ValueKind != JsonValueKind.String)
            {
                throw new ArgumentException("The value must be a string", nameof(href));
            }

            writer.WriteStartObject();

            writer.WritePropertyName(WebLink.HrefProperty);
            href.WriteTo(writer);

            if (name.ValueKind != JsonValueKind.Undefined)
            {
                if (name.ValueKind != JsonValueKind.String)
                {
                    throw new ArgumentException("The value must be a string.", nameof(name));
                }

                writer.WritePropertyName(WebLink.NameProperty);
                name.WriteTo(writer);
            }

            if (isTemplated.ValueKind != JsonValueKind.Undefined)
            {
                if (isTemplated.ValueKind != JsonValueKind.True && isTemplated.ValueKind != JsonValueKind.False)
                {
                    throw new ArgumentException("The value must be a boolean.", nameof(name));
                }

                writer.WritePropertyName(WebLink.NameProperty);
                name.WriteTo(writer);
            }

            if (title.ValueKind != JsonValueKind.Undefined)
            {
                if (title.ValueKind != JsonValueKind.String)
                {
                    throw new ArgumentException("The value must be a string.", nameof(title));
                }

                writer.WritePropertyName(WebLink.TitleProperty);
                title.WriteTo(writer);
            }

            if (profile.ValueKind != JsonValueKind.Undefined)
            {
                if (profile.ValueKind != JsonValueKind.String)
                {
                    throw new ArgumentException("The value must be a string.", nameof(profile));
                }

                writer.WritePropertyName(WebLink.ProfileProperty);
                profile.WriteTo(writer);
            }

            if (type.ValueKind != JsonValueKind.Undefined)
            {
                if (type.ValueKind != JsonValueKind.String)
                {
                    throw new ArgumentException("The value must be a string.", nameof(type));
                }

                writer.WritePropertyName(WebLink.TypeProperty);
                type.WriteTo(writer);
            }

            if (hreflang.ValueKind != JsonValueKind.Undefined)
            {
                if (hreflang.ValueKind != JsonValueKind.String)
                {
                    throw new ArgumentException("The value must be a string.", nameof(hreflang));
                }

                writer.WritePropertyName(WebLink.HreflangProperty);
                hreflang.WriteTo(writer);
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Begin writing a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        public void EndWriteWebLink(Utf8JsonWriter writer)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException("The web link has not been started. You must call BeginWriteWebLink() first.");
            }

            if ((this.requiredProperties & RequiredProperties.All) != RequiredProperties.All)
            {
                throw new InvalidOperationException($"The following required properties have not been set: {this.BuildMissingRequiredProperties()}");
            }

            writer.WriteEndObject();
            this.state = State.Completed;
        }

        /// <summary>
        /// Write the href property in a web link.
        /// </summary>
        /// <param name="writer">The writer to which to begin writing the <see cref="WebLink"/>.</param>
        /// <param name="href">The URI of the target resource.</param>
        public void WriteHref(Utf8JsonWriter writer, string href)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The WebLink has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The WebLink has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            this.requiredProperties |= RequiredProperties.Href;

            writer.WriteString(WebLink.HrefProperty, href);
        }

        /// <summary>
        /// Write the name property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        public void WriteName(Utf8JsonWriter writer, string name)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.NameProperty, name);
        }

        /// <summary>
        /// Write the isTemplated property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="isTemplated">An optional value that indicates whether the link is templated.</param>
        public void WriteIsTemplated(Utf8JsonWriter writer, bool isTemplated)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteBoolean(WebLink.IsTemplatedProperty, isTemplated);
        }

        /// <summary>
        /// Write the title property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="title">The (optional) human-readable title of the link.</param>
        public void WriteTitle(Utf8JsonWriter writer, string title)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.TitleProperty, title);
        }

        /// <summary>
        /// Write the profile property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="profile">Additional (optional) semantics of the target resource.</param>
        public void WriteProfile(Utf8JsonWriter writer, string profile)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.ProfileProperty, profile);
        }

        /// <summary>
        /// Write the type property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="type">The (optional) media type indication of the target resource.</param>
        public void WriteType(Utf8JsonWriter writer, string type)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.TypeProperty, type);
        }

        /// <summary>
        /// Write the hreflang property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        public void WriteHreflang(Utf8JsonWriter writer, string hreflang)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.HreflangProperty, hreflang);
        }

        /// <summary>
        /// Write the href property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="href">The URI of the target resource.</param>
        public void WriteHref(Utf8JsonWriter writer, ReadOnlySpan<char> href)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            this.requiredProperties |= RequiredProperties.Href;

            writer.WriteString(WebLink.HrefProperty, href);
        }

        /// <summary>
        /// Write the name property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        public void WriteName(Utf8JsonWriter writer, ReadOnlySpan<char> name)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.NameProperty, name);
        }

        /// <summary>
        /// Write the title property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="title">The (optional) human-readable title of the link.</param>
        public void WriteTitle(Utf8JsonWriter writer, ReadOnlySpan<char> title)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.TitleProperty, title);
        }

        /// <summary>
        /// Write the profile property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="profile">Additional (optional) semantics of the target resource.</param>
        public void WriteProfile(Utf8JsonWriter writer, ReadOnlySpan<char> profile)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.ProfileProperty, profile);
        }

        /// <summary>
        /// Write the type property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="type">The (optional) media type indication of the target resource.</param>
        public void WriteType(Utf8JsonWriter writer, ReadOnlySpan<char> type)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.TypeProperty, type);
        }

        /// <summary>
        /// Write the hreflang property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        public void WriteHreflang(Utf8JsonWriter writer, ReadOnlySpan<char> hreflang)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.HreflangProperty, hreflang);
        }

        /// <summary>
        /// Write the href property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="utf8Href">The URI of the target resource.</param>
        public void WriteHref(Utf8JsonWriter writer, ReadOnlySpan<byte> utf8Href)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            this.requiredProperties |= RequiredProperties.Href;

            writer.WriteString(WebLink.HrefProperty, utf8Href);
        }

        /// <summary>
        /// Write the name property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="utf8Name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        public void WriteName(Utf8JsonWriter writer, ReadOnlySpan<byte> utf8Name)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.NameProperty, utf8Name);
        }

        /// <summary>
        /// Write the title property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="utf8Title">The (optional) human-readable title of the link.</param>
        public void WriteTitle(Utf8JsonWriter writer, ReadOnlySpan<byte> utf8Title)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.TitleProperty, utf8Title);
        }

        /// <summary>
        /// Write the profile property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="utf8Profile">Additional (optional) semantics of the target resource.</param>
        public void WriteProfile(Utf8JsonWriter writer, ReadOnlySpan<byte> utf8Profile)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.ProfileProperty, utf8Profile);
        }

        /// <summary>
        /// Write the type property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="utf8Type">The (optional) media type indication of the target resource.</param>
        public void WriteType(Utf8JsonWriter writer, ReadOnlySpan<byte> utf8Type)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.TypeProperty, utf8Type);
        }

        /// <summary>
        /// Write the hreflang property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="utf8Hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        public void WriteHreflang(Utf8JsonWriter writer, ReadOnlySpan<byte> utf8Hreflang)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            writer.WriteString(WebLink.HreflangProperty, utf8Hreflang);
        }

        /// <summary>
        /// Write the href property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="href">The URI of the target resource.</param>
        public void WriteHref(Utf8JsonWriter writer, JsonElement href)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            if (href.ValueKind != JsonValueKind.String)
            {
                throw new ArgumentException("The value must be a string.", nameof(href));
            }

            this.requiredProperties |= RequiredProperties.Href;

            writer.WritePropertyName(WebLink.HrefProperty);
            href.WriteTo(writer);
        }

        /// <summary>
        /// Write the name property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="name">The (optional) name of the link, used to discriminate between different links for the same relation.</param>
        public void WriteName(Utf8JsonWriter writer, JsonElement name)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            if (name.ValueKind != JsonValueKind.String)
            {
                throw new ArgumentException("The value must be a string.", nameof(name));
            }

            writer.WritePropertyName(WebLink.NameProperty);
            name.WriteTo(writer);
        }

        /// <summary>
        /// Write the isTemplated property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="isTemplated">An optional value that indicates whether the link is templated.</param>
        public void WriteIsTemplated(Utf8JsonWriter writer, JsonElement isTemplated)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            if (isTemplated.ValueKind != JsonValueKind.False && isTemplated.ValueKind != JsonValueKind.True)
            {
                throw new ArgumentException("The value must be a boolean.", nameof(isTemplated));
            }

            writer.WritePropertyName(WebLink.IsTemplatedProperty);
            isTemplated.WriteTo(writer);
        }

        /// <summary>
        /// Write the title property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="title">The (optional) human-readable title of the link.</param>
        public void WriteTitle(Utf8JsonWriter writer, JsonElement title)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            if (title.ValueKind != JsonValueKind.String)
            {
                throw new ArgumentException("The value must be a string.", nameof(title));
            }

            writer.WritePropertyName(WebLink.TitleProperty);
            title.WriteTo(writer);
        }

        /// <summary>
        /// Write the profile property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="profile">Additional (optional) semantics of the target resource.</param>
        public void WriteProfile(Utf8JsonWriter writer, JsonElement profile)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            if (profile.ValueKind != JsonValueKind.String)
            {
                throw new ArgumentException("The value must be a string.", nameof(profile));
            }

            writer.WritePropertyName(WebLink.ProfileProperty);
            profile.WriteTo(writer);
        }

        /// <summary>
        /// Write the type property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="type">The (optional) media type indication of the target resource.</param>
        public void WriteType(Utf8JsonWriter writer, JsonElement type)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            if (type.ValueKind != JsonValueKind.String)
            {
                throw new ArgumentException("The value must be a string.", nameof(type));
            }

            writer.WritePropertyName(WebLink.TitleProperty);
            type.WriteTo(writer);
        }

        /// <summary>
        /// Write the hreflang property in a web link.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="hreflang">The (optional) language indication of the target resource [RFC5988].</param>
        public void WriteHreflang(Utf8JsonWriter writer, JsonElement hreflang)
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The web link has not been started. You must call {nameof(BeginWriteWebLink)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The web link has not been completed. You must complete writing properties before calling {nameof(this.EndWriteWebLink)}.");
            }

            if (hreflang.ValueKind != JsonValueKind.String)
            {
                throw new ArgumentException("The value must be a string.", nameof(hreflang));
            }

            writer.WritePropertyName(WebLink.HreflangProperty);
            hreflang.WriteTo(writer);
        }

        private string BuildMissingRequiredProperties()
        {
            var list = new List<string>();

            if ((this.requiredProperties & RequiredProperties.Href) == 0)
            {
                list.Add(WebLink.HrefProperty);
            }

            return string.Join(',', list);
        }
    }
}