// <copyright file="ManagementApiBenchmarks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Benchmarks
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;

    /// <summary>
    /// Benchmarks for the management API.
    /// </summary>
    [JsonExporterAttribute.Full]
    [MarkdownExporter]
    [HtmlExporter]
    public class ManagementApiBenchmarks : BenchmarksBase
    {
        private readonly Uri managementApiBaseUrl;
        private readonly string managementApiResourceId;
        private readonly string benchmarkClientTenantId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementApiBenchmarks"/> class.
        /// </summary>
        public ManagementApiBenchmarks()
        {
            this.managementApiBaseUrl = new Uri(this.Configuration["ManagementApi:BaseUri"]);
            this.managementApiResourceId = this.Configuration["ManagementApi:ResourceIdForMsiAuthentication"];
            this.benchmarkClientTenantId = this.Configuration["BenchmarkClientTenantId"];
        }

        /// <summary>
        /// Benchmarks the /swagger endpoint.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Benchmark]
        public async Task GetSwagger()
        {
            HttpRequestMessage request = await this.GetHttpRequestMessageWithAuthorizationHeaderAsync(this.managementApiResourceId).ConfigureAwait(false);
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(this.managementApiBaseUrl, "/swagger");

            HttpResponseMessage response = await this.HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Benchmarks calls to the notification creation endpoint without waiting for the resulting long running
        /// operation to complete.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Benchmark]
        public async Task CreateSingleNotificationInitialResponse()
        {
            HttpRequestMessage request = await this.GetHttpRequestMessageWithAuthorizationHeaderAsync(this.managementApiResourceId).ConfigureAwait(false);
            request.Method = HttpMethod.Put;
            request.RequestUri = new Uri(this.managementApiBaseUrl, $"/{this.benchmarkClientTenantId}/marain/usernotifications");

            string requestJson = "{" +
                "'notificationType': 'marain.notifications.test.v1'," +
                "'timestamp': '2020-07-21T17:32:28Z'," +
                "'userIds': [" +
                    "'" + Guid.NewGuid() + "'" +
                "]," +
                "'correlationIds': ['cid1', 'cid2']," +
                "'properties': {" +
                    "'thing1': 'value1'," +
                    "'thing2': 'value2'" +
                "}" +
            "}";

            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await this.HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
    }
}
