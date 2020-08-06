// <copyright file="CreateNotificationActivity.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Activities
{
    using System.Threading.Tasks;
    using Marain.UserNotifications.Management.Host.OpenApi;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Creates a new notification for the given user.
    /// </summary>
    public class CreateNotificationActivity
    {
        /// <summary>
        /// Executes the activity.
        /// </summary>
        /// <param name="context">The activity context.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName(nameof(CreateNotificationActivity))]
        public Task ExecuteAsync(
            [ActivityTrigger] IDurableActivityContext context,
            ILogger logger)
        {
            Notification request = context.GetInput<Notification>();

            logger.LogInformation(
                "Executing CreateNotificationActivity for notification of type {notificationType} for user {userId}",
                request.NotificationType,
                request.UserId);

            return Task.CompletedTask;
        }
    }
}
