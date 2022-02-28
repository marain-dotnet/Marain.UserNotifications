// <copyright file="TenantedFunctionData.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Helpers
{
    using System;

    /// <summary>
    /// Provides a wrapper class for passing data between functions where it is also necessary to know the requesting
    /// tenant Id.
    /// </summary>
    /// <typeparam name="T">The type of data being passed.</typeparam>
    public class TenantedFunctionData<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantedFunctionData{T}"/> class.
        /// </summary>
        /// <param name="tenantId">The <see cref="TenantId"/>.</param>
        /// <param name="payload">The <see cref="Payload"/>.</param>
        /// <param name="longRunningOperationId">The <see cref="LongRunningOperationId"/>.</param>
        public TenantedFunctionData(string tenantId, T payload, Guid? longRunningOperationId = null)
        {
            this.TenantId = tenantId;
            this.Payload = payload;
            this.LongRunningOperationId = longRunningOperationId;
        }

        /// <summary>
        /// Gets the Id of the tenant to which the data belongs.
        /// </summary>
        public string TenantId { get; }

        /// <summary>
        /// Gets the payload data.
        /// </summary>
        public T Payload { get; }

        /// <summary>
        /// Gets the Id of the long running operation associated with this data item.
        /// </summary>
        public Guid? LongRunningOperationId { get; }
    }
}