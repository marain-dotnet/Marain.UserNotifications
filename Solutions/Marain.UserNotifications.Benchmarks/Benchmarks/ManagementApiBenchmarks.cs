// <copyright file="ManagementApiBenchmarks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Benchmarks
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using Marain.UserNotifications.Client.Management.Requests;

    /// <summary>
    /// Benchmarks for the management API.
    /// </summary>
    [JsonExporterAttribute.Full]
    [MarkdownExporter]
    [HtmlExporter]
    public class ManagementApiBenchmarks : BenchmarksBase
    {
        /// <summary>
        /// Benchmarks calls to the notification creation endpoint without waiting for the resulting long running
        /// operation to complete.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Benchmark]
        public Task CreateSingleNotificationInitialResponse()
        {
            var properties = new Dictionary<string, object>
            {
                {  "thing1", "value 1" },
                { "thing2", 2 },
            };

            var body = new CreateNotificationsRequest
            {
                NotificationType = "marain.notifications.test.v1",
                CorrelationIds = new[] { "cid1", "cid2" },
                Properties = properties,
                Timestamp = DateTime.UtcNow,
                UserIds = new[] { Guid.NewGuid().ToString() },
            };

            return this.ManagementClient.CreateNotificationsAsync(this.BenchmarkClientTenantId, body);
        }
    }
}