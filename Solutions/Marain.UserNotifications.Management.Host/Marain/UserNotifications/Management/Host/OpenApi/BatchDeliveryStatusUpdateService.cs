// <copyright file="BatchDeliveryStatusUpdateService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.Operations.Client.OperationsControl;
    using Marain.Operations.Client.OperationsControl.Models;
    using Marain.Services.Tenancy;
    using Marain.UserNotifications.Management.Host.Helpers;
    using Marain.UserNotifications.Management.Host.Orchestrations;
    using Menes;
    using Menes.Exceptions;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// Implements the batch delivery update endpoint for the management API.
    /// </summary>
    public class BatchDeliveryStatusUpdateService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string BatchDeliveryStatusUpdateOperationId = "batchDeliveryStatusUpdate";

        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly IMarainOperationsControl operationsControlClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNotificationsService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="operationsControlClient">The operations control client.</param>
        public BatchDeliveryStatusUpdateService(
            IMarainServicesTenancy marainServicesTenancy,
            IMarainOperationsControl operationsControlClient)
        {
            this.marainServicesTenancy = marainServicesTenancy;
            this.operationsControlClient = operationsControlClient;
        }

        /// <summary>
        /// Starts a batch update of the delivery status for one or more notifications.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="body">The request body.</param>
        /// <returns>Confirmation that the update request has been accepted.</returns>
        [OperationId(BatchDeliveryStatusUpdateOperationId)]
        public async Task<OpenApiResult> UpdateDeliveryStatusesAsync(
            IOpenApiContext context,
            BatchDeliveryStatusUpdateRequestItem[] body)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            string delegatedTenantId = await this.marainServicesTenancy.GetDelegatedTenantIdForRequestingTenantAsync(tenant.Id).ConfigureAwait(false);
            var operationId = Guid.NewGuid();
            CreateOperationHeaders response = await this.operationsControlClient.CreateOperationAsync(
                delegatedTenantId,
                operationId).ConfigureAwait(false);

            IDurableOrchestrationClient orchestrationClient = context.AsDurableFunctionsOpenApiContext().OrchestrationClient
                ?? throw new OpenApiServiceMismatchException($"Operation {BatchDeliveryStatusUpdateOperationId} has been invoked, but no Durable Orchestration Client is available on the OpenApi context.");

            await orchestrationClient.StartNewAsync(
                nameof(UpdateNotificationDeliveryStatusesOrchestration),
                new TenantedFunctionData<BatchDeliveryStatusUpdateRequestItem[]>(context.CurrentTenantId!, body, operationId)).ConfigureAwait(false);

            return this.AcceptedResult(response.Location);
        }
    }
}
