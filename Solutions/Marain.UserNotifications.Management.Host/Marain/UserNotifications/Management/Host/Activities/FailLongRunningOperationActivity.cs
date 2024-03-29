﻿// <copyright file="FailLongRunningOperationActivity.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Activities
{
    using System;
    using System.Threading.Tasks;
    using Marain.Operations.Client.OperationsControl;
    using Marain.Operations.Client.OperationsControl.Models;
    using Marain.Services.Tenancy;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// The durable activity for recording that work on a long-running operation is complete.
    /// </summary>
    public class FailLongRunningOperationActivity
    {
        private readonly IMarainOperationsControl operationsControl;
        private readonly IMarainServicesTenancy marainServicesTenancy;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteLongRunningOperationActivity"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="operationsControl">The operations control client.</param>
        public FailLongRunningOperationActivity(
            IMarainServicesTenancy marainServicesTenancy,
            IMarainOperationsControl operationsControl)
        {
            this.operationsControl = operationsControl;
            this.marainServicesTenancy = marainServicesTenancy;
        }

        /// <summary>
        /// The run action.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        [FunctionName(nameof(FailLongRunningOperationActivity))]
        public async Task RunActionAsync(
            [ActivityTrigger] IDurableActivityContext context)
        {
            (Guid operationId, string tenantId) = context.GetInput<(Guid, string)>();
            string delegatedTenantId =
                await this.marainServicesTenancy.GetDelegatedTenantIdForRequestingTenantAsync(tenantId);

            ProblemDetails operationResult = await this.operationsControl.SetOperationFailedAsync(delegatedTenantId, operationId);

            if (operationResult != null)
            {
                var exception = new Exception($"Unexpected arror when attempting to fail long running operation '{operationId}' for tenant '{delegatedTenantId}': {operationResult.Status} - {operationResult.Title}");
                exception.Data.Add("ProblemDetails", operationResult);
                throw exception;
            }
        }
    }
}