// <copyright file="DurableFunctionsOpenApiContextExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using Menes;

    /// <summary>
    /// Helper methods for working with the durable functions specific implementation of <see cref="IOpenApiContext"/>.
    /// </summary>
    public static class DurableFunctionsOpenApiContextExtensions
    {
        /// <summary>
        /// Returns the specified <see cref="IOpenApiContext"/> as a <see cref="DurableFunctionsOpenApiContext"/>.
        /// </summary>
        /// <param name="context">The current <see cref="IOpenApiContext"/>.</param>
        /// <returns>The context as a <see cref="DurableFunctionsOpenApiContext"/>.</returns>
        public static DurableFunctionsOpenApiContext AsDurableFunctionsOpenApiContext(this IOpenApiContext context)
        {
            return (DurableFunctionsOpenApiContext)context;
        }
    }
}
