// <copyright file="HostFunction.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host
{
    using System.Threading.Tasks;
    using Marain.UserNotifications.Management.Host.OpenApi;
    using Menes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Azure.WebJobs.Extensions.Http;

    /// <summary>
    /// The host for the functions app.
    /// </summary>
    public class HostFunction
    {
        private readonly IOpenApiHost<HttpRequest, IActionResult> host;

        /// <summary>
        /// Initializes a new instance of the <see cref="Host"/> class.
        /// </summary>
        /// <param name="host">The OpenApi host.</param>
        public HostFunction(IOpenApiHost<HttpRequest, IActionResult> host)
        {
            this.host = host;
        }

        /// <summary>
        /// Azure Functions entry point.
        /// </summary>
        /// <param name="req">The <see cref="HttpRequest"/>.</param>
        /// <param name="orchestrationClient">The durable functions orchestration client.</param>
        /// <param name="executionContext">The context for the function execution.</param>
        /// <returns>An action result which comes from executing the function.</returns>
        [FunctionName("Marain-UserNotifications-Management-OpenApiHostRoot")]
        public Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "{*path}")]HttpRequest req,
            [DurableClient] IDurableOrchestrationClient orchestrationClient,
            ExecutionContext executionContext)
        {
            var additionalParameters = new DurableFunctionsOpenApiContextAdditionalProperties
            {
                ExecutionContext = executionContext,
                OrchestrationClient = orchestrationClient,
            };

            return this.host.HandleRequestAsync(req.Path, req.Method, req, additionalParameters);
        }
    }
}
