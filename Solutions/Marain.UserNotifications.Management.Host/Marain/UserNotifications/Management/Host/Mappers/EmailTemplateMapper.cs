// <copyright file="EmailTemplateMapper.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Mappers
{
    using System;
    using System.Threading.Tasks;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Marain.UserNotifications.Management.Host.OpenApi;
    using Marain.UserPreferences;
    using Menes;
    using Menes.Hal;
    using Menes.Links;

    /// <summary>
    /// Maps a single <see cref="EmailTemplateMapper"/> to a Hal response document.
    /// </summary>
    public class EmailTemplateMapper : IHalDocumentMapper<EmailTemplate, IOpenApiContext>
    {
        private readonly IHalDocumentFactory halDocumentFactory;
        private readonly IOpenApiWebLinkResolver openApiWebLinkResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebPushTemplateMapper"/> class.
        /// </summary>
        /// <param name="halDocumentFactory">The service provider to construct <see cref="HalDocument"/> instances.</param>
        /// <param name="openApiWebLinkResolver">The link resolver.</param>
        public EmailTemplateMapper(
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
            links.MapByContentTypeAndRelationTypeAndOperationId<EmailTemplate>(
                "self",
                GetTemplateService.GetTemplateOperationId);
        }

        /// <inheritdoc/>
        public ValueTask<HalDocument> MapAsync(EmailTemplate resource, IOpenApiContext context)
        {
            HalDocument response = this.halDocumentFactory.CreateHalDocumentFrom(
                new
                {
                    resource.ContentType,
                    resource.Body,
                    resource.Subject,
                    resource.Important,
                    resource.NotificationType,
                    CommunicationType = CommunicationType.Email,
                });

            response.ResolveAndAddByOwnerAndRelationType(
                this.openApiWebLinkResolver,
                resource,
                "self",
                ("tenantId", context.CurrentTenantId),
                ("notificationType", resource.NotificationType),
                ("communicationType", CommunicationType.Email.ToString()));

            return new ValueTask<HalDocument>(response);
        }
    }
}
