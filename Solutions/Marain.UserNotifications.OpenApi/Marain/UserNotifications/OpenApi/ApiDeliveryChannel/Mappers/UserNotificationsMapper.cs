// <copyright file="UserNotificationsMapper.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.ApiDeliveryChannel.Mappers
{
    using System;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Threading.Tasks;
    using Menes;
    using Menes.Hal;
    using Menes.Links;

    /// <summary>
    /// Maps an array of <see cref="UserNotification"/> to a Hal response document.
    /// </summary>
    public class UserNotificationsMapper : IHalDocumentMapper<GetNotificationsResult, UserNotificationsMappingContext>
    {
        private readonly IHalDocumentFactory halDocumentFactory;
        private readonly IOpenApiWebLinkResolver openApiWebLinkResolver;
        private readonly UserNotificationMapper userNotificationMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationsMapper"/> class.
        /// </summary>
        /// <param name="halDocumentFactory">The service provider to construct <see cref="HalDocument"/> instances.</param>
        /// <param name="openApiWebLinkResolver">The link resolver.</param>
        /// <param name="userNotificationMapper">The user notification mapper.</param>
        public UserNotificationsMapper(
            IHalDocumentFactory halDocumentFactory,
            IOpenApiWebLinkResolver openApiWebLinkResolver,
            UserNotificationMapper userNotificationMapper)
        {
            this.halDocumentFactory = halDocumentFactory
                ?? throw new ArgumentNullException(nameof(halDocumentFactory));

            this.openApiWebLinkResolver = openApiWebLinkResolver
                ?? throw new ArgumentNullException(nameof(openApiWebLinkResolver));

            this.userNotificationMapper = userNotificationMapper
                ?? throw new ArgumentNullException(nameof(userNotificationMapper));
        }

        /// <inheritdoc/>
        public void ConfigureLinkMap(IOpenApiLinkOperationMap links)
        {
            links.MapByContentTypeAndRelationTypeAndOperationId<GetNotificationsResult>(
                "self",
                GetNotificationsForUserService.GetNotificationsForUserOperationId);

            links.MapByContentTypeAndRelationTypeAndOperationId<GetNotificationsResult>(
                "next",
                GetNotificationsForUserService.GetNotificationsForUserOperationId);

            links.MapByContentTypeAndRelationTypeAndOperationId<GetNotificationsResult>(
                "newer",
                GetNotificationsForUserService.GetNotificationsForUserOperationId);
        }

        /// <inheritdoc/>
        public async ValueTask<HalDocument> MapAsync(GetNotificationsResult resource, UserNotificationsMappingContext context)
        {
            HalDocument response = this.halDocumentFactory.CreateHalDocument();

            HalDocument[] mappedItems =
                await Task.WhenAll(
                    resource.Results.Select(n => this.userNotificationMapper.MapAsync(n, context.OpenApiContext).AsTask())).ConfigureAwait(false);

            response.AddEmbeddedResources("items", mappedItems);

            foreach (HalDocument current in mappedItems)
            {
                response.AddLink("items", current.GetLinksForRelation("self").First());
            }

            response.ResolveAndAddByOwnerAndRelationType(
                this.openApiWebLinkResolver,
                resource,
                "self",
                ("tenantId", context.OpenApiContext.CurrentTenantId),
                ("userId", context.UserId),
                ("sinceNotificationId", context.SinceNotificationId),
                ("maxItems", context.MaxItems),
                ("continuationToken", context.ContinuationToken));

            if (!string.IsNullOrEmpty(resource.ContinuationToken))
            {
                response.ResolveAndAddByOwnerAndRelationType(
                    this.openApiWebLinkResolver,
                    resource,
                    "next",
                    ("tenantId", context.OpenApiContext.CurrentTenantId),
                    ("userId", context.UserId),
                    ("continuationToken", resource.ContinuationToken));
            }

            // If there are any results, we can also return a "newer" link, which can be used to request notifications
            // newer than those in this result set. If there aren't any, the user can just make the same request again
            // to get any newly created notifications.
            if (resource.Results.Length > 0)
            {
                response.ResolveAndAddByOwnerAndRelationType(
                    this.openApiWebLinkResolver,
                    resource,
                    "newer",
                    ("tenantId", context.OpenApiContext.CurrentTenantId),
                    ("userId", context.UserId),
                    ("sinceNotificationId", resource.Results[0].Id),
                    ("maxItems", context.MaxItems));
            }

            return response;
        }
    }
}
