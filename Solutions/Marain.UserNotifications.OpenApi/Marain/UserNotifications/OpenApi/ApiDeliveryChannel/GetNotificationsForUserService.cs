// <copyright file="GetNotificationsForUserService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.ApiDeliveryChannel
{
    using System.Threading.Tasks;
    using Menes;

    /// <summary>
    /// Implements the user notifications retrieval endpoint for the API delivery channel.
    /// </summary>
    public class GetNotificationsForUserService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string GetNotificationsForUserOperationId = "getNotificationsForUser";

        /// <summary>
        /// Retrieves notifications for a user.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="userId">The Id of the user to retrieve notifications for.</param>
        /// <param name="sinceSequenceId">The earliest sequence Id to return.</param>
        /// <param name="maxItems">The maximum number of items to return.</param>
        /// <param name="continuationToken">A continuation token from a previous request.</param>
        /// <returns>The notifications, as an OpenApiResult.</returns>
        public Task<OpenApiResult> GetNotificationsForUserAsync(
            IOpenApiContext context,
            string userId,
            long? sinceSequenceId,
            int? maxItems,
            string? continuationToken)
        {
            return Task.FromResult(this.NotImplementedResult());
        }
    }
}
