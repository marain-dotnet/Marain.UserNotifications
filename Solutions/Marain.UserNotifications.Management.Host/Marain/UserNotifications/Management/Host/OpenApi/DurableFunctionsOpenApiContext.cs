// <copyright file="DurableFunctionsOpenApiContext.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;
    using System.Security.Claims;
    using Menes;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// OpenApi context that allows the orchestration client to be supplied to functions.
    /// </summary>
    public class DurableFunctionsOpenApiContext : IOpenApiContext
    {
        private DurableFunctionsOpenApiContextAdditionalProperties additionalProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableFunctionsOpenApiContext"/> class.
        /// </summary>
        public DurableFunctionsOpenApiContext()
        {
            this.additionalProperties = new DurableFunctionsOpenApiContextAdditionalProperties();
        }

        /// <inheritdoc/>
        public ClaimsPrincipal? CurrentPrincipal { get; set; }

        /// <inheritdoc/>
        public string? CurrentTenantId { get; set; }

        /// <summary>
        /// Gets or sets the current functions execution context.
        /// </summary>
        public ExecutionContext? ExecutionContext
        {
            get => this.additionalProperties.ExecutionContext;
            set => this.additionalProperties.ExecutionContext = value;
        }

        /// <summary>
        /// Gets or sets the current functions durable functions orchestration client.
        /// </summary>
        public IDurableOrchestrationClient? OrchestrationClient
        {
            get => this.additionalProperties.OrchestrationClient;
            set => this.additionalProperties.OrchestrationClient = value;
        }

        /// <inheritdoc/>
        public object AdditionalContext
        {
            get
            {
                return this.additionalProperties;
            }

#pragma warning disable CS8614 // Nullability of reference types in type of parameter doesn't match implicitly implemented member.
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member.
            set
            {
                this.additionalProperties = value as DurableFunctionsOpenApiContextAdditionalProperties
                    ?? throw new ArgumentException("AdditionalContext can only be set to a non-null value of type 'DurableFunctionsOpenApiContextAdditionalProperties'");
            }
#pragma warning restore CS8614 // Nullability of reference types in type of parameter doesn't match implicitly implemented member.
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member.
        }
    }
}