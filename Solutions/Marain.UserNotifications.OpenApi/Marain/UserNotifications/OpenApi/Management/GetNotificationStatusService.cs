// <copyright file="GetNotificationStatusService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.Management
{
    using System.Threading.Tasks;
    using Menes;

    /// <summary>
    /// Implements the get notification status endpoint for the management API.
    /// </summary>
    public class GetNotificationStatusService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string GetNotificationStatusOperationId = "getNotificationStatus";

        /// <summary>
        /// Gets the delivery/read status of the specified notification for each channel it's been dispatched to.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="notificationId">The Id of the notification to retrieve statuses for.</param>
        /// <param name="maxItems">The maximum number of items to return.</param>
        /// <param name="continuationToken">A continuation token from a previous request.</param>
        /// <returns>The notifications, as an OpenApiResult.</returns>
        public Task<OpenApiResult> GetNotificationsAsync(
            IOpenApiContext context,
            string notificationId,
            int? maxItems,
            string? continuationToken)
        {
            return Task.FromResult(this.NotImplementedResult());
        }
    }
}
