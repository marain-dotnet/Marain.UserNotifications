// <copyright file="HalDocumentResourceWriter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#nullable enable
namespace Menes.Hal
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    /// <summary>
    /// Writer for a <see cref="HalDocumentResource"/>.
    /// </summary>
    public struct HalDocumentResourceWriter
    {
        private State state;

        private RequiredProperties requiredProperties;

        private Children writtenChildren;

        private Children currentChild;

        private enum State
        {
            NotStarted,
            Started,
            WritingChild,
            Completed,
        }

        [Flags]
        private enum Children
        {
            None = 0,
            Properties,
            Links,
            Embedded,
            ContentType,
        }

        [Flags]
        private enum RequiredProperties
        {
            None = 0,
            ContentType = 1,
            All = ContentType,
        }

        /// <summary>
        /// Write a <see cref="HalDocumentResource"/> to a <see cref="Utf8JsonWriter"/>.
        /// </summary>
        /// <param name="writer">The writer to which to write the resource.</param>
        /// <param name="resource">The weblink to write.</param>
        public static void WriteHalDocumentResource(Utf8JsonWriter writer, in HalDocumentResource resource)
        {
            resource.Write(writer);
        }

        /// <summary>
        /// Begin writing a <see cref="HalDocumentResource"/>.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the <see cref="HalDocumentResource"/>.</param>
        /// <returns>The <see cref="HalDocumentResourceWriter"/> to use for continuing to write the resource.</returns>
        public static HalDocumentResourceWriter BeginWriteHalDocumentResource(Utf8JsonWriter writer)
        {
            HalDocumentResourceWriter resourceWriter = default;

            writer.WriteStartObject();
            resourceWriter.state = State.Started;
            return resourceWriter;
        }

        /// <summary>
        /// Begin writing a web link.
        /// </summary>
        /// <param name="writer">The writer in which to end writing the <see cref="HalDocumentResource"/>.</param>
        public void EndWriteHalDocumentResource(Utf8JsonWriter writer)
        {
            this.ValidateEndWriteHalDocumentResourceState();

            writer.WriteEndObject();
            this.state = State.Completed;
        }

        /// <summary>
        /// Begin write the Links collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which to begin writing the Links collection.</param>
        /// <returns>The <see cref="HalDocumentLinksCollectionWriter"/> with which to write the web links.</returns>
        public HalDocumentLinksCollectionWriter BeginWriteLinks(Utf8JsonWriter writer)
        {
            this.ValidateBeginWriteLinksState();

            this.currentChild = Children.Links;
            this.state = State.WritingChild;

            return HalDocumentLinksCollectionWriter.BeginWriteLinksCollection(writer);
        }

        /// <summary>
        /// End writing the Links collection.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> in which to end writing the Links collection.</param>
        /// <param name="linksCollectionWriter">The <see cref="HalDocumentLinksCollectionWriter"/> to end.</param>
        public void EndWriteLinks(Utf8JsonWriter writer, HalDocumentLinksCollectionWriter linksCollectionWriter)
        {
            this.ValidateEndWriteLinksState();

            linksCollectionWriter.EndWriteLinksCollection(writer);

            this.state = State.Started;
            this.writtenChildren |= Children.Links;
            this.currentChild = Children.None;
        }

        /// <summary>
        /// Write the contentType property in a HalDocumentResource.
        /// </summary>
        /// <param name="writer">The writer to which to  write the <see cref="HalDocumentResource"/>.</param>
        /// <param name="contentType">The content type of the resource.</param>
        public void WriteContentType(Utf8JsonWriter writer, string contentType)
        {
            this.ValidateWriteContentTypeState();

            this.requiredProperties |= RequiredProperties.ContentType;
            this.writtenChildren |= Children.ContentType;
            writer.WriteString(HalDocumentResource.ContentTypeProperty, contentType);
        }

        /// <summary>
        /// Write the contentType property in a HalDocumentResource.
        /// </summary>
        /// <param name="writer">The writer to which to  write the <see cref="HalDocumentResource"/>.</param>
        /// <param name="contentType">The content type of the resource.</param>
        public void WriteContentType(Utf8JsonWriter writer, ReadOnlySpan<char> contentType)
        {
            this.ValidateWriteContentTypeState();

            this.requiredProperties |= RequiredProperties.ContentType;

            writer.WriteString(HalDocumentResource.ContentTypeProperty, contentType);
        }

        /// <summary>
        /// Write the contentType property in a HalDocumentResource.
        /// </summary>
        /// <param name="writer">The writer to which to  write the <see cref="HalDocumentResource"/>.</param>
        /// <param name="utf8ContentType">The content type of the resource.</param>
        public void WriteContentType(Utf8JsonWriter writer, ReadOnlySpan<byte> utf8ContentType)
        {
            this.ValidateWriteContentTypeState();
            this.requiredProperties |= RequiredProperties.ContentType;

            writer.WriteString(HalDocumentResource.ContentTypeProperty, utf8ContentType);
        }

        /// <summary>
        /// Write the contentType property in a HalDocumentResource.
        /// </summary>
        /// <param name="writer">The writer to which to  write the <see cref="HalDocumentResource"/>.</param>
        /// <param name="contentType">The content type of the resource.</param>
        public void WriteContentType(Utf8JsonWriter writer, JsonElement contentType)
        {
            this.ValidateWriteContentTypeState();

            this.requiredProperties |= RequiredProperties.ContentType;

            writer.WritePropertyName(HalDocumentResource.ContentTypeProperty);
            contentType.WriteTo(writer);
        }

        private readonly void ValidateWriteContentTypeState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} has not been started. You must call {nameof(BeginWriteHalDocumentResource)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} has been completed. You must complete writing properties before calling {nameof(this.EndWriteHalDocumentResource)}.");
            }

            if (this.state == State.WritingChild)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} is writing a child entity. You must complete writing the child entity before writing additional items.");
            }

            if ((this.writtenChildren & Children.ContentType) != 0)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} has already written the ContentType.");
            }
        }

        private readonly void ValidateEndWriteLinksState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} has not been started. You must call {nameof(BeginWriteHalDocumentResource)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} has already been completed.");
            }

            if (this.state == State.WritingChild && this.currentChild != Children.Links)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} is not writing links. You must complete the child operation first.");
            }
        }

        private void ValidateEndWriteHalDocumentResourceState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} has not been started. You must call {nameof(BeginWriteHalDocumentResource)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} has already been completed.");
            }

            if (this.state == State.WritingChild)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} is writing a child. You must complete the child operation first.");
            }

            if ((this.requiredProperties & RequiredProperties.All) != RequiredProperties.All)
            {
                throw new InvalidOperationException($"The following required properties have not been set: {this.BuildMissingRequiredProperties()}");
            }
        }

        private readonly void ValidateBeginWriteLinksState()
        {
            if (this.state == State.NotStarted)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} has not been started. You must call {nameof(BeginWriteHalDocumentResource)} first.");
            }

            if (this.state == State.Completed)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} has already been completed.");
            }

            if (this.state == State.WritingChild)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} is already writing a child. You must complete the child operation first.");
            }

            if ((this.writtenChildren & Children.Links) != 0)
            {
                throw new InvalidOperationException($"The {nameof(HalDocumentResourceWriter)} has already written the Links.");
            }
        }

        private string BuildMissingRequiredProperties()
        {
            var list = new List<string>();

            if ((this.requiredProperties & RequiredProperties.ContentType) == 0)
            {
                list.Add(HalDocumentResource.ContentTypeProperty);
            }

            return string.Join(',', list);
        }
    }
}