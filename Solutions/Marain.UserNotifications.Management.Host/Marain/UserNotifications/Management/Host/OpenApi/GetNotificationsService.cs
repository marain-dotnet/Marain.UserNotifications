// <copyright file="GetNotificationsService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
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
        [OperationId(GetNotificationsOperationId)]
#pragma warning disable RCS1163 // Unused parameter - not implemented yet.
#pragma warning disable IDE0060 // Unused parameter - not implemented yet.
#pragma warning disable IDE0079 // Remove unnecessary suppression - IDE0060 keeps changing its mind about whether it applies here
        public Task<OpenApiResult> GetNotificationsAsync(
            IOpenApiContext context,
            string? userId,
            string? notificationType,
            int? maxItems,
            string? continuationToken)
#pragma warning restore IDE0079, IDE0060, RCS1163
        {
            return Task.FromResult(this.NotImplementedResult());
        }
    }
}