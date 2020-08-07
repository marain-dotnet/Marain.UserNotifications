// <copyright file="CreateAndDispatchNotificationsOrchestration.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#pragma warning disable RCS1090 // Call 'ConfigureAwait(false)'
namespace Marain.UserNotifications.Management.Host.Orchestrations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Marain.UserNotifications.Management.Host.Activities;
    using Marain.UserNotifications.Management.Host.Helpers;
    using Marain.UserNotifications.Management.Host.OpenApi;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Durable orchestration to create new notifications.
    /// </summary>
    public class CreateAndDispatchNotificationsOrchestration
    {
        /// <summary>
        /// Executes the orchestration.
        /// </summary>
        /// <param name="orchestrationContext">The <see cref="IDurableOrchestrationContext"/>.</param>
        /// <param name="log">The current logger.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName(nameof(CreateAndDispatchNotificationsOrchestration))]
        public async Task RunAsync(
            [OrchestrationTrigger] IDurableOrchestrationContext orchestrationContext,
            ILogger log)
        {
            ILogger replaySafeLogger = orchestrationContext.CreateReplaySafeLogger(log);

            TenantedFunctionData<CreateNotificationsRequest> request = orchestrationContext.GetInput<TenantedFunctionData<CreateNotificationsRequest>>();

            try
            {
                // Fan out to create each notification
                string[] correlationIds = new string[request.Payload.CorrelationIds.Length + 1];
                request.Payload.CorrelationIds.CopyTo(correlationIds, 0);
                correlationIds[^1] = orchestrationContext.InstanceId;

                IEnumerable<Task> createNotificationTasks = request.Payload.UserIds
                    .Select(userId => new UserNotification(
                        null,
                        request.Payload.NotificationType,
                        userId,
                        request.Payload.Timestamp,
                        request.Payload.Properties,
                        new UserNotificationMetadata(correlationIds, null)))
                    .Select(notification => orchestrationContext.CallSubOrchestratorAsync(
                        nameof(CreateAndDispatchNotificationOrchestration),
                        new TenantedFunctionData<UserNotification>(request.TenantId, notification)));

                await Task.WhenAll(createNotificationTasks);

                await orchestrationContext.CallActivityAsync(
                    nameof(CompleteLongRunningOperationActivity),
                    (request.LongRunningOperationId.Value, request.TenantId));
            }
            catch (FunctionFailedException)
            {
                await orchestrationContext.CallActivityAsync(
                    nameof(FailLongRunningOperationActivity),
                    (request.LongRunningOperationId.Value, request.TenantId));
            }
        }
    }
}
