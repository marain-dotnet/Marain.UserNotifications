// <copyright file="HalDocumentLinksCollectionWriter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Menes.Hal
{
    using System;
    using System.Text.Json;

    /// <summary>
    /// Writer for the _links collection of a <see cref="HalDocumentResource"/>.
    /// </summary>
    public struct HalDocumentLinksCollectionWriter
    {
        private State state;

        private enum State
        {
            NotStarted,
            Started,
            WritingRelations,
            WritingRelation,
            Completed,
        }

        /// <summary>
        /// Begin writing a collection of web links.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the <see cref="HalDocumentResource"/>.</param>
        /// <returns>The <see cref="HalDocumentResourceWriter"/> to use for continuing to write the resource.</returns>
        public static HalDocumentLinksCollectionWriter BeginWriteLinksCollection(Utf8JsonWriter writer)
        {
            HalDocumentLinksCollectionWriter webLinkCollectionWriter = default;

            writer.WriteStartObject(HalDocumentResource.LinksProperty);
            webLinkCollectionWriter.state = State.Started;
            return webLinkCollectionWriter;
        }

        /// <summary>
        /// End writing a collection of web links.
        /// </summary>
        /// <param name="writer">The writer in which to end writing the <see cref="HalDocumentResource"/>.</param>
        public void EndWriteLinksCollection(Utf8JsonWriter writer)
        {
            this.ValidateEndWriteLinksCollectionState();

            writer.WriteEndObject();
            this.state = State.Completed;
        }

        /// <summary>
        /// Begin writing the links collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the Links collection.</param>
        /// <param name="rel">The relation for the links collection.</param>
        /// <returns>The <see cref="HalDocumentLinksCollectionWriter"/> with which to write the web links.</returns>
        public RelationCollectionWriter BeginWriteRelations(Utf8JsonWriter writer, string rel)
        {
            this.ValidateBeginWriteRelationsState();

            this.state = State.WritingRelations;

            return RelationCollectionWriter.BeginWriteRelationCollection(writer, rel);
        }

        /// <summary>
        /// Begin writing the links collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the Links collection.</param>
        /// <param name="rel">The relation for the links collection.</param>
        /// <returns>The <see cref="HalDocumentLinksCollectionWriter"/> with which to write the web links.</returns>
        public RelationCollectionWriter BeginWriteRelations(Utf8JsonWriter writer, ReadOnlySpan<char> rel)
        {
            this.ValidateBeginWriteRelationsState();

            this.state = State.WritingRelations;

            return RelationCollectionWriter.BeginWriteRelationCollection(writer, rel);
        }

        /// <summary>
        /// Begin writing the links collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the Links collection.</param>
        /// <param name="rel">The relation for the links collection.</param>
        /// <returns>The <see cref="HalDocumentLinksCollectionWriter"/> with which to write the web links.</returns>
        public RelationCollectionWriter BeginWriteRelations(Utf8JsonWriter writer, ReadOnlySpan<byte> rel)
        {
            this.ValidateBeginWriteRelationsState();

            this.state = State.WritingRelations;

            return RelationCollectionWriter.BeginWriteRelationCollection(writer, rel);
        }

        /// <summary>
        /// Begin writing the relation Links collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the Links collection for the relation.</param>
        /// <param name="rel">The relation for the links collection.</param>
        /// <returns>The <see cref="RelationCollectionWriter"/> with which to write the web links.</returns>
        public RelationCollectionWriter BeginWriteRelations(Utf8JsonWriter writer, JsonEncodedText rel)
        {
            this.ValidateBeginWriteRelationsState();

            this.state = State.WritingRelations;

            return RelationCollectionWriter.BeginWriteRelationCollection(writer, rel);
        }

        /// <summary>
        /// End writing the relations.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the Links collection for the relation.</param>
        /// <param name="relationCollectionWriter">The <see cref="RelationCollectionWriter"/> with which to write the web links.</param>
        public void EndWriteRelations(Utf8JsonWriter writer, RelationCollectionWriter relationCollectionWriter)
        {
            this.ValidateEndWrteRelationsState();

            relationCollectionWriter.EndWriteRelationCollection(writer);
            this.state = State.Started;
        }

        /// <summary>
        /// Begin writing a single link relation in the links collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the Links collection.</param>
        /// <param name="rel">The relation for a single link relation in the links collection.</param>
        /// <returns>The <see cref="HalDocumentLinksCollectionWriter"/> with which to write the web links.</returns>
        public WebLinkWriter BeginWriteRelation(Utf8JsonWriter writer, string rel)
        {
            this.ValidateBeginWriteRelationState();

            writer.WritePropertyName(rel);

            this.state = State.WritingRelation;

            return WebLinkWriter.BeginWriteWebLink(writer);
        }

        /// <summary>
        /// Begin writing a single link relation in the links collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the Links collection.</param>
        /// <param name="rel">The relation for a single link relation in the links collection.</param>
        /// <returns>The <see cref="HalDocumentLinksCollectionWriter"/> with which to write the web links.</returns>
        public WebLinkWriter BeginWriteRelation(Utf8JsonWriter writer, ReadOnlySpan<char> rel)
        {
            this.ValidateBeginWriteRelationState();

            writer.WritePropertyName(rel);

            this.state = State.WritingRelation;

            return WebLinkWriter.BeginWriteWebLink(writer);
        }

        /// <summary>
        /// Begin writing a single link relation in the links collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the Links collection.</param>
        /// <param name="rel">The relation for a single link relation in the links collection.</param>
        /// <returns>The <see cref="HalDocumentLinksCollectionWriter"/> with which to write the web links.</returns>
        public WebLinkWriter BeginWriteRelation(Utf8JsonWriter writer, ReadOnlySpan<byte> rel)
        {
            this.ValidateBeginWriteRelationState();

            writer.WritePropertyName(rel);

            this.state = State.WritingRelation;

            return WebLinkWriter.BeginWriteWebLink(writer);
        }

        /// <summary>
        /// Begin writing a single link relation in the links collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the Links collection for the relation.</param>
        /// <param name="rel">The relation for a single link relation in the links collection.</param>
        /// <returns>The <see cref="RelationCollectionWriter"/> with which to write the web links.</returns>
        public WebLinkWriter BeginWriteRelation(Utf8JsonWriter writer, JsonEncodedText rel)
        {
            this.ValidateBeginWriteRelationState();

            writer.WritePropertyName(rel);

            this.state = State.WritingRelation;

            return WebLinkWriter.BeginWriteWebLink(writer);
        }

        /// <summary>
        /// Begin writing a single link relation in the links collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the web link for the relation.</param>
        /// <param name="webLinkWriter">The <see cref="WebLinkWriter"/> with which to write the web link.</param>
        public void EndWriteRelation(Utf8JsonWriter writer, WebLinkWriter webLinkWriter)
        {
            this.ValidateEndWriteRelationState();

            webLinkWriter.EndWriteWebLink(writer);
            this.state = State.Started;
        }

        private readonly void ValidateEndWriteLinksCollectionState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} has not been started. You must call {nameof(BeginWriteLinksCollection)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} has already been completed.");
            }

            if (this.state == State.WritingRelations)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} is writing relations. You must complete the child operation first.");
            }
        }

        private readonly void ValidateEndWrteRelationsState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} has not been started. You must call {nameof(BeginWriteLinksCollection)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"Unable to write. The {nameof(HalDocumentLinksCollectionWriter)} has been completed.");
            }

            if (this.state == State.Started)
            {
                throw new InvalidOperationException($"The {nameof(RelationCollectionWriter)} has not been started. You must call {nameof(BeginWriteLinksCollection)} first.");
            }

            if (this.state == State.WritingRelation)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} is already writing a relation. You must complete the child operation first.");
            }
        }

        private readonly void ValidateEndWriteRelationState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} has not been started. You must call {nameof(BeginWriteLinksCollection)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"Unable to write. The {nameof(HalDocumentLinksCollectionWriter)} has been completed.");
            }

            if (this.state == State.Started)
            {
                throw new InvalidOperationException($"The {nameof(WebLinkWriter)} has not been started. You must call {nameof(BeginWriteLinksCollection)} first.");
            }

            if (this.state == State.WritingRelations)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} is already writing relations. You must complete the child operation first.");
            }
        }

        private readonly void ValidateBeginWriteRelationState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} has not been started. You must call {nameof(BeginWriteLinksCollection)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"Unable to write. The {nameof(HalDocumentLinksCollectionWriter)} has been completed.");
            }

            if (this.state == State.WritingRelation)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} is already writing relations. You must complete the child operation first.");
            }

            if (this.state == State.WritingRelations)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} is already writing relations. You must complete the child operation first.");
            }
        }

        private readonly void ValidateBeginWriteRelationsState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} has not been started. You must call {nameof(BeginWriteLinksCollection)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"Unable to write. The {nameof(HalDocumentLinksCollectionWriter)} has been completed.");
            }

            if (this.state == State.WritingRelations)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} is already writing relations. You must complete the child operation first.");
            }

            if (this.state == State.WritingRelation)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentLinksCollectionWriter)} is already writing a relation. You must complete the child operation first.");
            }
        }
    }
}
