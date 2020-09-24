// <copyright file="HalDocumentResourceExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#nullable enable
namespace Menes.Hal
{
    using System.Linq;

    /// <summary>
    /// Extensions to the <see cref="IHalDocumentResource"/>.
    /// </summary>
    public static class HalDocumentResourceExtensions
    {
        /// <summary>
        /// Gets the single resource with the given relationship type.
        /// </summary>
        /// <param name="doc">The <see cref="IHalDocumentResource"/> for which to get the embedded document.</param>
        /// <param name="rel">The relationship type.</param>
        /// <returns>The single <see cref="IHalDocumentResource"/> for that relationship type.</returns>
        public static IHalDocumentResource EmbeddedSingle(this IHalDocumentResource doc, string rel)
        {
            return doc.EnumerateEmbedded(rel).Single();
        }

        /// <summary>
        /// Gets the first embedded resource with the given relationship type.
        /// </summary>
        /// <param name="doc">The <see cref="IHalDocumentResource"/> for which to get the embedded document.</param>
        /// <param name="rel">The relationship type.</param>
        /// <returns>The single <see cref="IHalDocumentResource"/> for that relationship type.</returns>
        public static IHalDocumentResource EmbeddedFirst(this IHalDocumentResource doc, string rel)
        {
            return doc.EnumerateEmbedded(rel).First();
        }
    }
}
