// <copyright file="IHalDocumentResource.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#nullable enable
namespace Menes.Hal
{
    using System;

    /// <summary>
    /// The interface implemented by all hal documents.
    /// </summary>
    public interface IHalDocumentResource : IJsonPropertyBag
    {
        /// <summary>
        /// Gets the content type for the document.
        /// </summary>
        /// <remarks>
        /// This allocates a string. For comparison operations see <see cref="ContentTypeEquals(string)"/>.
        /// </remarks>
        string ContentType { get; }

        /// <summary>
        /// Gets the self link for the resource.
        /// </summary>
        WebLink SelfLink { get; }

        /// <summary>
        /// Gets the resource as a plain <see cref="HalDocumentResource"/>.
        /// </summary>
        HalDocumentResource HalDocument
        {
            get;
        }

        /// <summary>
        /// Determine if the content type matches a given value, without allocating a string.
        /// </summary>
        /// <param name="utf8Text">The text to compare.</param>
        /// <returns>True if the content type matches the given text.</returns>
        bool ContentTypeEquals(ReadOnlySpan<byte> utf8Text);

        /// <summary>
        /// Determine if the content type matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the content type matches the given text.</returns>
        bool ContentTypeEquals(ReadOnlySpan<char> text);

        /// <summary>
        /// Determine if the content type matches a given value, without allocating a string.
        /// </summary>
        /// <param name="text">The text to compare.</param>
        /// <returns>True if the content type matches the given text.</returns>
        bool ContentTypeEquals(string text);

        /// <summary>
        /// Enumerate the embedded object in the document.
        /// </summary>
        /// <returns>An <see cref="HalDocumentResource.EmbeddedEnumerator"/> for the <see cref="HalDocumentResource"/>s embedded in the document.</returns>
        HalDocumentResource.EmbeddedEnumerator EnumerateEmbedded();

        /// <summary>
        /// Enumerate the embedded object in the document.
        /// </summary>
        /// <param name="relation">The relation type to which to filter the embedded objects.</param>
        /// <returns>An <see cref="HalDocumentResource.EmbeddedForRelationEnumerator"/> for the <see cref="HalDocumentResource"/>s embedded in the document for the specified relation.</returns>
        HalDocumentResource.EmbeddedForRelationEnumerator EnumerateEmbedded(string relation);

        /// <summary>
        /// Enumerate the links in the document.
        /// </summary>
        /// <returns>A <see cref="HalDocumentResource.LinksEnumerator"/> for the <see cref="LinkRelation"/>s in the document.</returns>
        HalDocumentResource.LinksEnumerator EnumerateLinks();

        /// <summary>
        /// Enumerate the links in the document.
        /// </summary>
        /// <param name="relation">The relation type to which to filter the links.</param>
        /// <returns>A <see cref="HalDocumentResource.LinksForRelationEnumerator"/> for the <see cref="WebLink"/>s in the document for that relation.</returns>
        HalDocumentResource.LinksForRelationEnumerator EnumerateLinks(string relation);

        /// <summary>
        /// Enumerate the properties in the document.
        /// </summary>
        /// <returns>A <see cref="HalDocumentResource.PropertiesEnumerator"/> for the properties in the document.</returns>
        new HalDocumentResource.PropertiesEnumerator EnumerateProperties();
    }
}