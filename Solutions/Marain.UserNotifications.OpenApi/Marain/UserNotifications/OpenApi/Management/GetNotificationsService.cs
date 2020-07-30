// <copyright file="GetNotificationsService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.Management
{
    using System.Threading.Tasks;
    using Menes;

    /// <summary>
    /// Implements the get notifications endpoint for the management API.
    /// </summary>
    public class GetNotificationsService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string GetNotificationsOperationId = "getNotifications";

        /// <summary>
        /// Retrieves notifications for a user.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="userId">Optional user Id to filter notifications by.</param>
        /// <param name="notificationType">Optional notification type to filter notifications by.</param>
        /// <param name="maxItems">The maximum number of items to return.</param>
        /// <param name="continuationToken">A continuation token from a previous request.</param>
        /// <returns>The notifications, as an OpenApiResult.</returns>
        public Task<OpenApiResult> GetNotificationsAsync(
            IOpenApiContext context,
            string? userId,
            string? notificationType,
            int? maxItems,
            string? continuationToken)
        {
            return Task.FromResult(this.NotImplementedResult());
        }
    }
}
