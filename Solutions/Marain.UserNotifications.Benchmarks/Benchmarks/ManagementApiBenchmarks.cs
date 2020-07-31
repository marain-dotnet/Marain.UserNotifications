// <copyright file="ManagementApiBenchmarks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Benchmarks
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
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
        private string managementApiResourceId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementApiBenchmarks"/> class.
        /// </summary>
        public ManagementApiBenchmarks()
        {
            this.managementApiBaseUrl = new Uri(this.Configuration["ManagementApi:BaseUri"]);
            this.managementApiResourceId = this.Configuration["ManagementApi:ResourceIdForMsiAuthentication"];
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
    }
}
