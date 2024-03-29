﻿// <copyright file="UpdateNotificationReadStatusesOrchestration.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

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
    /// Durable orchestration to update the delivery status for notifications.
    /// </summary>
    public class UpdateNotificationReadStatusesOrchestration
    {
        /// <summary>
        /// Executes the orchestration.
        /// </summary>
        /// <param name="orchestrationContext">The <see cref="IDurableOrchestrationContext"/>.</param>
        /// <param name="log">The current logger.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName(nameof(UpdateNotificationReadStatusesOrchestration))]
        public async Task RunAsync(
            [OrchestrationTrigger] IDurableOrchestrationContext orchestrationContext,
            ILogger log)
        {
            ILogger replaySafeLogger = orchestrationContext.CreateReplaySafeLogger(log);

            TenantedFunctionData<BatchReadStatusUpdateRequestItem[]> request = orchestrationContext.GetInput<TenantedFunctionData<BatchReadStatusUpdateRequestItem[]>>();

            try
            {
                await orchestrationContext.CallActivityAsync(
                    nameof(StartLongRunningOperationActivity),
                    (request.LongRunningOperationId!.Value, request.TenantId));

                // Fan out to update each notification
                IEnumerable<Task> updateStatusTasks = request.Payload
                    .Select(x => orchestrationContext.CallActivityAsync(
                        nameof(UpdateNotificationReadStatusActivity),
                        new TenantedFunctionData<BatchReadStatusUpdateRequestItem>(request.TenantId, x)));

                await Task.WhenAll(updateStatusTasks);

                await orchestrationContext.CallActivityAsync(
                    nameof(CompleteLongRunningOperationActivity),
                    (request.LongRunningOperationId!.Value, request.TenantId));
            }
            catch (FunctionFailedException)
            {
                await orchestrationContext.CallActivityAsync(
                    nameof(FailLongRunningOperationActivity),
                    (request.LongRunningOperationId!.Value, request.TenantId));
            }
        }
    }
}