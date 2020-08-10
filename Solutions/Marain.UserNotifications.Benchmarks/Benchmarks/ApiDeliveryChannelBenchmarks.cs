// <copyright file="ApiDeliveryChannelBenchmarks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Benchmarks
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;

    /// <summary>
    /// Benchmarks for the API delivery channel.
    /// </summary>
    [JsonExporterAttribute.Full]
    [MarkdownExporter]
    [HtmlExporter]
    public class ApiDeliveryChannelBenchmarks : BenchmarksBase
    {
        private readonly Uri apiDeliveryChannelBaseUri;
        private string apiDeliveryChannelResourceId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDeliveryChannelBenchmarks"/> class.
        /// </summary>
        public ApiDeliveryChannelBenchmarks()
        {
            this.apiDeliveryChannelBaseUri = new Uri(this.Configuration["ApiDeliveryChannel:BaseUri"]);
            this.apiDeliveryChannelResourceId = this.Configuration["ApiDeliveryChannel:ResourceIdForMsiAuthentication"];
        }

        /// <summary>
        /// Benchmarks the /swagger endpoint.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Benchmark]
        public async Task GetSwagger()
        {
            HttpRequestMessage request = await this.GetHttpRequestMessageWithAuthorizationHeaderAsync(this.apiDeliveryChannelResourceId).ConfigureAwait(false);
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(this.apiDeliveryChannelBaseUri, "/swagger");

            HttpResponseMessage response = await this.HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
    }
}
