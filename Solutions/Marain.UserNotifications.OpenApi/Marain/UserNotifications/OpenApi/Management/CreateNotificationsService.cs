// <copyright file="CreateNotificationsService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.Management
{
    using System.Threading.Tasks;
    using Menes;

    /// <summary>
    /// Implements the create notifications endpoint for the management API.
    /// </summary>
    public class CreateNotificationsService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string CreateNotificationsOperationId = "createNotifications";

        /// <summary>
        /// Creates new notification(s) for one or more users.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="body">The request body.</param>
        /// <returns>Confirmation that the update request has been accepted.</returns>
        public Task<OpenApiResult> CreateNotificationsAsync(
            IOpenApiContext context,
            CreateNotificationsRequest body)
        {
            return Task.FromResult(this.NotImplementedResult());
        }
    }
}
