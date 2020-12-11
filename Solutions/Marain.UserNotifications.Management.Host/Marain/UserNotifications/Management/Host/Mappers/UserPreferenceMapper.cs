// <copyright file="UserPreferenceMapper.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Mappers
{
    using System;
    using System.Threading.Tasks;
    using Marain.UserNotifications.Management.Host.OpenApi;
    using Marain.UserPreferences;
    using Menes;
    using Menes.Hal;
    using Menes.Links;

    /// <summary>
    /// Maps a single <see cref="UserNotification"/> to a Hal response document.
    /// </summary>
    public class UserPreferenceMapper : IHalDocumentMapper<UserPreference, IOpenApiContext>
    {
        private readonly IHalDocumentFactory halDocumentFactory;
        private readonly IOpenApiWebLinkResolver openApiWebLinkResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPreferenceMapper"/> class.
        /// </summary>
        /// <param name="halDocumentFactory">The service provider to construct <see cref="HalDocument"/> instances.</param>
        /// <param name="openApiWebLinkResolver">The link resolver.</param>
        public UserPreferenceMapper(
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
            links.MapByContentTypeAndRelationTypeAndOperationId<UserPreference>(
                "self",
                GetUserPreferenceService.GetUserPreferenceOperationId);
        }

        /// <inheritdoc/>
        public ValueTask<HalDocument> MapAsync(UserPreference resource, IOpenApiContext context)
        {
            // Note that we're hard coding "delivered" to true. This is because we're mapping the notification so we
            // can return it from a request to the API - which means that even if it hasn't been delivered on any other
            // channel, it's being delivered now on this one.
            HalDocument response = this.halDocumentFactory.CreateHalDocumentFrom(
                new
                {
                    ContentType = "application/vnd.marain.usernotifications.management.userpreference.v1",
                    resource.UserId,
                    resource.Email,
                    resource.PhoneNumber,
                    resource.CommunicationChannelsPerNotificationConfiguration,
                    resource.Timestamp,
                    resource.ETag,
                });

            response.ResolveAndAddByOwnerAndRelationType(
                this.openApiWebLinkResolver,
                resource,
                "self",
                ("tenantId", context.CurrentTenantId),
                ("userId", resource.UserId));

            return new ValueTask<HalDocument>(response);
        }
    }
}
