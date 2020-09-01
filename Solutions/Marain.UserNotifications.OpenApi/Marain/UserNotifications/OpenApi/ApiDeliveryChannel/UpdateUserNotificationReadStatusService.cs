// <copyright file="UpdateUserNotificationReadStatusService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.OpenApi.ApiDeliveryChannel
{
    using System.Threading.Tasks;
    using Menes;

    /// <summary>
    /// Implements the mark notification as read for the API delivery channel.
    /// </summary>
    public class UpdateUserNotificationReadStatusService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string MarkNotificationReadOperationId = "markNotificationRead";

        /// <summary>
        /// Marks the specified notification as read for the API delivery channel.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="notificationId">The Id of the notification to mark as read.</param>
        /// <returns>Confirmation that the update request has been accepted.</returns>
        [OperationId(MarkNotificationReadOperationId)]
        public Task<OpenApiResult> MarkNotificationReadAsync(
            IOpenApiContext context,
            string notificationId)
        {
            return Task.FromResult(this.NotImplementedResult());
        }
    }
}
