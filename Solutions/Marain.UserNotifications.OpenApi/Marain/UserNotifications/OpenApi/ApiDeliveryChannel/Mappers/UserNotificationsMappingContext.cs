// <copyright file="UserNotificationsMappingContext.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.ApiDeliveryChannel.Mappers
{
    using Menes;

    /// <summary>
    /// Mapping context for the <see cref="UserNotificationsMapper"/>.
    /// </summary>
    public class UserNotificationsMappingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationsMappingContext"/> class.
        /// </summary>
        /// <param name="openApiContext">The <see cref="OpenApiContext"/>.</param>
        /// <param name="sinceNotificationId">The <see cref="SinceNotificationId"/>.</param>
        /// <param name="maxItems">The <see cref="MaxItems"/>.</param>
        /// <param name="continuationToken">The <see cref="ContinuationToken"/>.</param>
        public UserNotificationsMappingContext(
            IOpenApiContext openApiContext,
            string? sinceNotificationId,
            int maxItems,
            string? continuationToken)
        {
            this.OpenApiContext = openApiContext;
            this.SinceNotificationId = sinceNotificationId;
            this.MaxItems = maxItems;
            this.ContinuationToken = continuationToken;
        }

        /// <summary>
        /// Gets the <see cref="IOpenApiContext"/> for the current request.
        /// </summary>
        public IOpenApiContext OpenApiContext { get; }

        /// <summary>
        /// Gets the Id of the notification that all returned notifications should be newer than.
        /// </summary>
        public string? SinceNotificationId { get; }

        /// <summary>
        /// Gets the maximum number of notifications that should be returned.
        /// </summary>
        public int MaxItems { get; }

        /// <summary>
        /// Gets the requested continuation token, returned from a previous request.
        /// </summary>
        public string? ContinuationToken { get; }
    }
}