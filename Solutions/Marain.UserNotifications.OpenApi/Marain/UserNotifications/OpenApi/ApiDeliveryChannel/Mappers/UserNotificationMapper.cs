// <copyright file="UserNotificationMapper.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.ApiDeliveryChannel.Mappers
{
    using System;
    using System.Threading.Tasks;
    using Menes;
    using Menes.Hal;
    using Menes.Links;

    /// <summary>
    /// Maps a single <see cref="UserNotification"/> to a Hal response document.
    /// </summary>
    public class UserNotificationMapper : IHalDocumentMapper<UserNotification, IOpenApiContext>
    {
        private readonly IHalDocumentFactory halDocumentFactory;
        private readonly IOpenApiWebLinkResolver openApiWebLinkResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationMapper"/> class.
        /// </summary>
        /// <param name="halDocumentFactory">The service provider to construct <see cref="HalDocument"/> instances.</param>
        /// <param name="openApiWebLinkResolver">The link resolver.</param>
        public UserNotificationMapper(
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
            links.MapByContentTypeAndRelationTypeAndOperationId<UserNotification>(
                "self",
                GetNotificationService.GetNotificationOperationId);

            links.MapByContentTypeAndRelationTypeAndOperationId<UserNotification>(
                "mark-read",
                MarkNotificationAsReadService.MarkNotificationReadOperationId);
        }

        /// <inheritdoc/>
        public ValueTask<HalDocument> MapAsync(UserNotification resource, IOpenApiContext context)
        {
            // Note that we're hard coding "delivered" to true. This is because we're mapping the notification so we
            // can return it from a request to the API - which means that even if it hasn't been delivered on any other
            // channel, it's being delivered now on this one.
            HalDocument response = this.halDocumentFactory.CreateHalDocumentFrom(new
            {
                ContentType = "application/vnd.marain.usernotifications.apidelivery.notification",
                resource.UserId,
                resource.NotificationType,
                resource.Properties,
                resource.Timestamp,
                Delivered = true,
                Read = resource.HasBeenReadOnAtLeastOneChannel(),
            });

            response.ResolveAndAddByOwnerAndRelationType(
                this.openApiWebLinkResolver,
                resource,
                "self",
                ("tenantId", context.CurrentTenantId),
                ("notificationId", resource.Id));

            if (resource.GetReadStatusForChannel(Constants.ApiDeliveryChannelId) != UserNotificationReadStatus.Read)
            {
                response.ResolveAndAddByOwnerAndRelationType(
                    this.openApiWebLinkResolver,
                    resource,
                    "mark-read",
                    ("tenantId", context.CurrentTenantId),
                    ("notificationId", resource.Id));
            }

            return new ValueTask<HalDocument>(response);
        }
    }
}
