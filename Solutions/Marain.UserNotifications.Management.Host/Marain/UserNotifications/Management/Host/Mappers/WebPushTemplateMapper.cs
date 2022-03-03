// <copyright file="WebPushTemplateMapper.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Mappers
{
    using System;
    using System.Threading.Tasks;
    using Marain.Models;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Marain.UserNotifications.Management.Host.OpenApi;
    using Menes;
    using Menes.Hal;
    using Menes.Links;

    /// <summary>
    /// Maps a single <see cref="WebPushTemplateMapper"/> to a Hal response document.
    /// </summary>
    public class WebPushTemplateMapper : IHalDocumentMapper<WebPushTemplate, IOpenApiContext>
    {
        private readonly IHalDocumentFactory halDocumentFactory;
        private readonly IOpenApiWebLinkResolver openApiWebLinkResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebPushTemplateMapper"/> class.
        /// </summary>
        /// <param name="halDocumentFactory">The service provider to construct <see cref="HalDocument"/> instances.</param>
        /// <param name="openApiWebLinkResolver">The link resolver.</param>
        public WebPushTemplateMapper(
            IHalDocumentFactory halDocumentFactory,
            IOpenApiWebLinkResolver openApiWebLinkResolver)
        {
            this.halDocumentFactory = halDocumentFactory
                ?? throw new ArgumentNullException(nameof(halDocumentFactory));

            this.openApiWebLinkResolver = openApiWebLinkResolver
                ?? throw new ArgumentNullException(nameof(openApiWebLinkResolver));
        }

        /// <inheritdoc/>
        public void ConfigureLinkMap(IOpenApiLinkOperationMap links)
        {
            links.MapByContentTypeAndRelationTypeAndOperationId<WebPushTemplate>(
                "self",
                GetTemplateService.GetTemplateOperationId);
        }

        /// <inheritdoc/>
        public ValueTask<HalDocument> MapAsync(WebPushTemplate resource, IOpenApiContext context)
        {
            HalDocument response = this.halDocumentFactory.CreateHalDocumentFrom(
                new
                {
                    resource.ContentType,
                    resource.Body,
                    resource.Title,
                    resource.Image,
                    resource.NotificationType,
                    CommunicationType = CommunicationType.WebPush,
                    resource.ActionUrl,
                });

            response.ResolveAndAddByOwnerAndRelationType(
                this.openApiWebLinkResolver,
                resource,
                "self",
                ("tenantId", context.CurrentTenantId),
                ("notificationType", resource.NotificationType),
                ("communicationType", nameof(CommunicationType.WebPush)));

            return new ValueTask<HalDocument>(response);
        }
    }
}