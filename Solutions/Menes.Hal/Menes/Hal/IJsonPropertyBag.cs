// <copyright file="IJsonPropertyBag.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Menes.Hal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.Json;

    /// <summary>
    /// An enumerable, extensible json property bag.
    /// </summary>
    public interface IJsonPropertyBag
    {
        /// <summary>
        /// Represents an enumerator for the properties of the resource.
        /// </summary>
        public interface IJsonPropertyBagEnumerator : IEnumerable<JsonProperty>, IEnumerable, IEnumerator<JsonProperty>, IEnumerator, IDisposable
        {
        }

        /// <summary>
        /// Enumerate the properties in the document.
        /// </summary>
        /// <returns>A <see cref="HalDocumentResource.PropertiesEnumerator"/> for the properties in the document.</returns>
        IJsonPropertyBagEnumerator EnumerateProperties();

        /// <summary>
        /// Looks for a property named <paramref name="propertyName"/>. When it exists, it returns the value in the <paramref name="element"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property to look for.</param>
        /// <param name="element">The element in which to return the value.</param>
        /// <returns>True if the property exists, otherwise false.</returns>
        bool TryGetProperty(string propertyName, out JsonElement element);

        /// <summary>
        /// Looks for a property named <paramref name="propertyName"/>. When it exists, it returns the value in the <paramref name="element"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property to look for.</param>
        /// <param name="element">The element in which to return the value.</param>
        /// <returns>True if the property exists, otherwise false.</returns>
        bool TryGetProperty(ReadOnlySpan<char> propertyName, out JsonElement element);

        /// <summary>
        /// Looks for a property named <paramref name="utf8PropertyName"/>. When it exists, it returns the value in the <paramref name="element"/>.
        /// </summary>
        /// <param name="utf8PropertyName">The name of the property to look for.</param>
        /// <param name="element">The element in which to return the value.</param>
        /// <returns>True if the property exists, otherwise false.</returns>
        bool TryGetProperty(ReadOnlySpan<byte> utf8PropertyName, out JsonElement element);
    }
}
