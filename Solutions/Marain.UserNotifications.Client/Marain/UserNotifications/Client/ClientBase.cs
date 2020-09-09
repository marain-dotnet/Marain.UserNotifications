// <copyright file="ClientBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Base class for the clients.
    /// </summary>
    public abstract class ClientBase : IApiClient
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ClientBase"/> class.
        /// </summary>
        /// <param name="baseUrl">The base Url for the service.</param>
        /// <param name="serializerOptions">The Json serializer options.</param>
        protected ClientBase(string baseUrl, JsonSerializerOptions serializerOptions)
        {
            this.BaseUrl = new Uri(baseUrl, UriKind.Absolute);
            this.SerializerOptions = serializerOptions;
        }

        /// <inheritdoc/>
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// Gets the HTTP client that will be used for underlying requests.
        /// </summary>
        protected HttpClient HttpClient { get; } = new HttpClient();

        /// <summary>
        /// Gets the serialization options that will be used to serialize and deserialize data.
        /// </summary>
        protected JsonSerializerOptions SerializerOptions { get; }

        /// <inheritdoc/>
        public async Task<ApiResponse<T>> GetPathAsync<T>(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException(nameof(relativePath));
            }

            var requestUri = new Uri(this.BaseUrl, relativePath);

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Get, requestUri);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            T result = await JsonSerializer.DeserializeAsync<T>(contentStream, this.SerializerOptions).ConfigureAwait(false);

            return new ApiResponse<T>(
                response.StatusCode,
                result);
        }

        /// <summary>
        /// Builds an HTTP request with the supplied data.
        /// </summary>
        /// <typeparam name="T">The object type to send as the request content.</typeparam>
        /// <param name="method">The HTTP method to use.</param>
        /// <param name="requestUri">The URI of the request.</param>
        /// <param name="body">The data to send as the request content.</param>
        /// <returns>The constructed message.</returns>
        protected HttpRequestMessage BuildRequest<T>(HttpMethod method, Uri requestUri, T body)
        {
            var request = new HttpRequestMessage(method, requestUri);

            string json = JsonSerializer.Serialize(body, this.SerializerOptions);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return request;
        }

        /// <summary>
        /// Builds an HTTP request with the supplied data.
        /// </summary>
        /// <param name="method">The HTTP method to use.</param>
        /// <param name="requestUri">The URI of the request.</param>
        /// <returns>The constructed message.</returns>
        protected HttpRequestMessage BuildRequest(HttpMethod method, Uri requestUri)
        {
            return new HttpRequestMessage(method, requestUri);
        }

        /// <summary>
        /// Builds a URL from the supplied path and query params.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <returns>The Uri.</returns>
        protected Uri ConstructUri(string path, params (string, string)[] queryParameters)
        {
            var builder = new UriBuilder(this.BaseUrl);
            builder.Path = path;
            foreach ((string key, string value) in queryParameters)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    builder.Query += $"{key}={Uri.EscapeUriString(value)}";
                }
            }

            return builder.Uri;
        }

        /// <summary>
        /// Sends the supplied request and throws a <see cref="UserNotificationsApiException"/> if either the request
        /// fails or the response status code does not indicate success.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response.</returns>
        protected async Task<HttpResponseMessage> SendRequestAndThrowOnFailure(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await this.HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new UserNotificationsApiException("Unexpected error when calling service; see InnerException for details.", ex)
                {
                    StatusCode = response?.StatusCode,
                };
            }
        }

        /// <summary>
        /// Gets the response body as a JsonDocument.
        /// </summary>
        /// <param name="responseMessage">The response.</param>
        /// <returns>The resulting JsonDocument.</returns>
        protected async Task<JsonDocument> GetResponseJsonDocumentAsync(HttpResponseMessage responseMessage)
        {
            // TODO: Make this right
            string json = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonDocument.Parse(json);
        }
    }
}
