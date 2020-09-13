// <copyright file="RelationCollectionWriter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Menes.Hal
{
    using System;
    using System.Text.Json;

    /// <summary>
    /// Writer for the any array of links for a relation in a _links collection of a <see cref="HalDocumentResource"/>.
    /// </summary>
    public struct RelationCollectionWriter
    {
        private State state;

        private enum State
        {
            NotStarted,
            Started,
            WritingLink,
            Completed,
        }

        /// <summary>
        /// Begin writing a relation collection for a particular relation.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write the relation collection.</param>
        /// <param name="rel">The relation type to add.</param>
        /// <returns>The <see cref="RelationCollectionWriter"/> with which to write the relations.</returns>
        public static RelationCollectionWriter BeginWriteRelationCollection(Utf8JsonWriter writer, string rel)
        {
            RelationCollectionWriter result = default;
            writer.WriteStartArray(rel);
            result.state = State.Started;
            return result;
        }

        /// <summary>
        /// Begin writing a relation collection for a particular relation.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write the relation collection.</param>
        /// <param name="rel">The relation type to add.</param>
        /// <returns>The <see cref="RelationCollectionWriter"/> with which to write the relations.</returns>
        public static RelationCollectionWriter BeginWriteRelationCollection(Utf8JsonWriter writer, JsonEncodedText rel)
        {
            RelationCollectionWriter result = default;
            writer.WriteStartArray(rel);
            result.state = State.Started;
            return result;
        }

        /// <summary>
        /// Begin writing a relation collection for a particular relation.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write the relation collection.</param>
        /// <param name="rel">The relation type to add.</param>
        /// <returns>The <see cref="RelationCollectionWriter"/> with which to write the relations.</returns>
        public static RelationCollectionWriter BeginWriteRelationCollection(Utf8JsonWriter writer, ReadOnlySpan<byte> rel)
        {
            RelationCollectionWriter result = default;
            writer.WriteStartArray(rel);
            result.state = State.Started;
            return result;
        }

        /// <summary>
        /// Begin writing a link collection for a particular relation.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write the relation collection.</param>
        /// <param name="rel">The relation type to add.</param>
        /// <returns>The <see cref="RelationCollectionWriter"/> with which to write the relations.</returns>
        public static RelationCollectionWriter BeginWriteRelationCollection(Utf8JsonWriter writer, ReadOnlySpan<char> rel)
        {
            RelationCollectionWriter result = default;
            writer.WriteStartArray(rel);
            result.state = State.Started;
            return result;
        }

        /// <summary>
        /// Writes a link to the collection.
        /// </summary>
        /// <param name="writer">The writer to write.</param>
        /// <param name="link">The link to write.</param>
        public void WriteWebLink(Utf8JsonWriter writer, in WebLink link)
        {
            this.ValidateWriteWebLinkState();

            this.state = State.WritingLink;
            WebLinkWriter.WriteWebLink(writer, link);
        }

        /// <summary>
        /// Writes a link to the collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="link">The link to write.</param>
        public void WriteWebLink(Utf8JsonWriter writer, JsonElement link)
        {
            this.ValidateWriteWebLinkState();

            this.state = State.WritingLink;

            WebLinkWriter.WriteWebLink(writer, link);
        }

        /// <summary>
        /// Begin writing a link to the collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <returns>The <see cref="WebLinkWriter"/> to use to write the web link.</returns>
        public WebLinkWriter BeginWriteWebLink(Utf8JsonWriter writer)
        {
            this.ValidateWriteWebLinkState();

            this.state = State.WritingLink;

            return WebLinkWriter.BeginWriteWebLink(writer);
        }

        /// <summary>
        /// Finish writing a web link using a <see cref="WebLinkWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        /// <param name="webLinkWriter">The <see cref="WebLinkWriter"/> to complete.</param>
        public void EndWriteWebLink(Utf8JsonWriter writer, WebLinkWriter webLinkWriter)
        {
            this.ValidateEndWriteWebLinkState();

            webLinkWriter.EndWriteWebLink(writer);
            this.state = State.Started;
        }

        /// <summary>
        /// Complete writing the link collection for a particular relation.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to write.</param>
        public void EndWriteRelationCollection(Utf8JsonWriter writer)
        {
            this.ValidateEndWriteRelationCollectionState();

            writer.WriteEndArray();
            this.state = State.Completed;
        }

        private readonly void ValidateEndWriteRelationCollectionState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(RelationCollectionWriter)} has not been started. You must call {nameof(BeginWriteRelationCollection)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The {nameof(RelationCollectionWriter)} has already been completed.");
            }

            if (this.state == State.WritingLink)
            {
                throw new InvalidOperationException($"The {nameof(RelationCollectionWriter)} is writing a link. The operation must be completed first.");
            }
        }

        private readonly void ValidateEndWriteWebLinkState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(RelationCollectionWriter)} has not been started. You must call {nameof(BeginWriteRelationCollection)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The {nameof(RelationCollectionWriter)} has already been completed.");
            }

            if (this.state == State.Started)
            {
                throw new InvalidOperationException($"The {nameof(RelationCollectionWriter)} is not writing a link. You must call {nameof(this.BeginWriteWebLink)} first.");
            }
        }

        private readonly void ValidateWriteWebLinkState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(RelationCollectionWriter)} has not been started. You must call {nameof(BeginWriteRelationCollection)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The {nameof(RelationCollectionWriter)} has already been completed.");
            }

            if (this.state == State.WritingLink)
            {
                throw new InvalidOperationException($"The {nameof(RelationCollectionWriter)} is writing a link. The operation must be completed first.");
            }
        }
    }
}