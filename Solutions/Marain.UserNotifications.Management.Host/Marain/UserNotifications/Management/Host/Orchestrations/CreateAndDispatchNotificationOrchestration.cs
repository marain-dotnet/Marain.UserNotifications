﻿// <copyright file="CreateAndDispatchNotificationOrchestration.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Orchestrations
{
    using System.Threading.Tasks;
    using Marain.UserNotifications.Management.Host.Activities;
    using Marain.UserNotifications.Management.Host.Helpers;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Durable orchestration to create and dispatche an individual notification.
    /// </summary>
    public class CreateAndDispatchNotificationOrchestration
    {
        /// <summary>
        /// Executes the orchestration.
        /// </summary>
        /// <param name="orchestrationContext">The <see cref="IDurableOrchestrationContext"/>.</param>
        /// <param name="log">The current logger.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName(nameof(CreateAndDispatchNotificationOrchestration))]
        public async Task RunAsync(
            [OrchestrationTrigger] IDurableOrchestrationContext orchestrationContext,
            ILogger log)
        {
            ILogger replaySafeLogger = orchestrationContext.CreateReplaySafeLogger(log);

            replaySafeLogger.LogInformation(
                "Executing orchestration {orchestrationName} with instance Id {orchestrationInstanceId} from parent instance Id {parentOrchestrationInstanceId}",
                nameof(CreateAndDispatchNotificationOrchestration),
                orchestrationContext.InstanceId,
                orchestrationContext.ParentInstanceId);

            TenantedFunctionData<UserNotification> request = orchestrationContext.GetInput<TenantedFunctionData<UserNotification>>();

            replaySafeLogger.LogDebug("Deserialized CreateNotificationsRequest for user Id '{userId}'", request.Payload.UserId);

            UserNotification? savedUserNotification = await orchestrationContext
                .CallActivityAsync<UserNotification>(nameof(CreateNotificationActivity), request)
                .ConfigureAwait(true);

            // Only trigger to send the notifications from the configured thirdparty delivery channels if they have been configured in the request.
            if (request.Payload.DeliveryChannelConfiguredPerCommunicationType != null && savedUserNotification != null)
            {
                // Add DeliveryChannelConfiguredPerCommunicationType
                UserNotification updatedNotification = savedUserNotification.AddDeliveryChannelConfiguredPerCommunicationType(request.Payload.DeliveryChannelConfiguredPerCommunicationType);

                await orchestrationContext.CallActivityAsync(
                    nameof(DispatchNotificationActivity),
                    new TenantedFunctionData<UserNotification>(request.TenantId, updatedNotification));
            }
        }
    }
}