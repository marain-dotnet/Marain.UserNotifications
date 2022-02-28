// <copyright file="ApiDeliveryChannelBenchmarks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Benchmarks
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Benchmarks for the API delivery channel.
    /// </summary>
    [JsonExporterAttribute.Full]
    [MarkdownExporter]
    [HtmlExporter]
    public class ApiDeliveryChannelBenchmarks : BenchmarksBase
    {
        /// <summary>
        /// Benchmarks the /swagger endpoint.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Benchmark]
        public Task RequestNotificationsWhereNoneExistAsync()
        {
            string madeUpUserId = Guid.NewGuid().ToString();
            return this.ApiDeliveryChannelClient.GetUserNotificationsAsync(this.BenchmarkClientTenantId, madeUpUserId, null, null);
        }
    }
}