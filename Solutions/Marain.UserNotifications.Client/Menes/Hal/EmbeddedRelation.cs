// <copyright file="EmbeddedRelation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#nullable enable
namespace Menes.Hal
{
    using System;
    using System.Text.Json;

    /// <summary>
    /// A link and a relation.
    /// </summary>
    public readonly struct EmbeddedRelation
    {
        private readonly JsonProperty relSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedRelation"/> struct.
        /// </summary>
        /// <param name="relSource">The json property that is the source of the relation.</param>
        /// <param name="embedded">The embedded resource in the relation.</param>
        public EmbeddedRelation(JsonProperty relSource, HalDocumentResource embedded)
        {
            this.relSource = relSource;
            this.Embedded = embedded;
        }

        /// <summary>
        /// Gets the <see cref="HalDocumentResource"/> for the relation.
        /// </summary>
        public HalDocumentResource Embedded { get; }

        /// <summary>
        /// Gets the relationship type of this link.
        /// </summary>
        /// <remarks>Note that this allocates a string. See <see cref="RelationEquals(string)"/> for an allocation-free comparison.</remarks>
        public string Relation => this.relSource.Name;

        /// <summary>
        /// Compares the specified string to the name of the relation.
        /// </summary>
        /// <param name="relation">The string to compare.</param>
        /// <returns>True if the relation matches the string.</returns>
        public bool RelationEquals(string relation)
        {
            return this.relSource.NameEquals(relation);
        }

        /// <summary>
        /// Compares the specified string to the name of the relation.
        /// </summary>
        /// <param name="relation">The string to compare.</param>
        /// <returns>True if the relation matches the string.</returns>
        public bool RelationEquals(ReadOnlySpan<byte> relation)
        {
            return this.relSource.NameEquals(relation);
        }

        /// <summary>
        /// Compares the specified string to the name of the relation.
        /// </summary>
        /// <param name="relation">The string to compare.</param>
        /// <returns>True if the relation matches the string.</returns>
        public bool RelationEquals(ReadOnlySpan<char> relation)
        {
            return this.relSource.NameEquals(relation);
        }
    }
}