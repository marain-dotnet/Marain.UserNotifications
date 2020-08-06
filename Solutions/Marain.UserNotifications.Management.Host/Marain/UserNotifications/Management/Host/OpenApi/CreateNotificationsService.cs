// <copyright file="CreateNotificationsService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.Services.Tenancy;
    using Marain.UserNotifications.Management.Host.Helpers;
    using Marain.UserNotifications.Management.Host.Orchestrations;
    using Menes;
    using Menes.Exceptions;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// Implements the create notifications endpoint for the management API.
    /// </summary>
    public class CreateNotificationsService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string CreateNotificationsOperationId = "createNotifications";

        private readonly IMarainServicesTenancy marainServicesTenancy;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNotificationsService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        public CreateNotificationsService(
            IMarainServicesTenancy marainServicesTenancy)
        {
            this.marainServicesTenancy = marainServicesTenancy;
        }

        /// <summary>
        /// Creates new notification(s) for one or more users.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="body">The request body.</param>
        /// <returns>Confirmation that the update request has been accepted.</returns>
        [OperationId(CreateNotificationsOperationId)]
        public async Task<OpenApiResult> CreateNotificationsAsync(
            IOpenApiContext context,
            CreateNotificationsRequest body)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            // TODO: Start long running operation
            string delegatedTenantId = await this.marainServicesTenancy.GetDelegatedTenantIdForRequestingTenantAsync(tenant.Id).ConfigureAwait(false);

            // TODO: Add the operation Id to the correlation Ids
            // TODO: Start new orchestration
            IDurableOrchestrationClient orchestrationClient = context.AsDurableFunctionsOpenApiContext().OrchestrationClient
                ?? throw new OpenApiServiceMismatchException($"Operation {CreateNotificationsOperationId} has been invoked, but no Durable Orchestration Client is available on the OpenApi context.");

            await orchestrationClient.StartNewAsync(
                nameof(CreateAndDispatchNotificationsOrchestration),
                new TenantedFunctionData<CreateNotificationsRequest>(context.CurrentTenantId!, body)).ConfigureAwait(false);

            // TODO: Return long running op Id.
            return this.AcceptedResult("http://mylocation.com/myoperationid");
        }
    }
}
