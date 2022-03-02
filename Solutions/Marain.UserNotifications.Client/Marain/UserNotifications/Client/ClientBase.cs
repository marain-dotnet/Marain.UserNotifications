// <copyright file="ClientBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#pragma warning disable CA1822 // Mark members as static - protected members don't use 'this' today but might in the future

namespace Marain.UserNotifications.Client
{
    using System;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Base class for the clients.
    /// </summary>
    public abstract class ClientBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ClientBase"/> class.
        /// </summary>
        /// <param name="httpClient">The client to use for API requests.</param>
        protected ClientBase(HttpClient httpClient)
        {
            this.SerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            this.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            this.Client = httpClient;
        }

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// Gets the serialization options that will be used to serialize and deserialize data.
        /// </summary>
        protected JsonSerializerOptions SerializerOptions { get; }

        /// <summary>
        /// Gets data from the API using a link returned from a previous request.
        /// </summary>
        /// <typeparam name="T">The expected type of the response body.</typeparam>
        /// <param name="relativePath">The Url to request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response.</returns>
        protected async Task<ApiResponse<T>> GetPathAsync<T>(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException(nameof(relativePath));
            }

            var requestUri = new Uri(relativePath, UriKind.Relative);

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Get, requestUri);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            using Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            T result = await JsonSerializer.DeserializeAsync<T>(contentStream, this.SerializerOptions, cancellationToken).ConfigureAwait(false);

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

            string json = JsonSerializer.Serialize(body, body.GetType(), this.SerializerOptions);
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
        protected Uri ConstructUri(string path, params (string Key, string Value)[] queryParameters)
        {
            string query = string.Join("&", queryParameters.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));

            if (!string.IsNullOrEmpty(query))
            {
                path += "?" + query;
            }

            return new Uri(path, UriKind.Relative);
        }

        /// <summary>
        /// Shortcut method for invoking an endpoint that is expected to return a 202 status code and implement the
        /// long running operation pattern.
        /// </summary>
        /// <typeparam name="T">The type of the request body.</typeparam>
        /// <param name="requestUri">The Uri to request.</param>
        /// <param name="method">The method use to request the Uri.</param>
        /// <param name="body">Data to be sent in the request body.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An API containing the Location header from the response.</returns>
        protected Task<ApiResponse> CallLongRunningOperationEndpointAsync<T>(
            Uri requestUri,
            HttpMethod method,
            T body,
            CancellationToken cancellationToken = default)
        {
            if (body is null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            HttpRequestMessage request = this.BuildRequest(method, requestUri, body);
            return this.CallLongRunningOperationEndpointInternalAsync(request, cancellationToken);
        }

        /// <summary>
        /// Shortcut method for invoking an endpoint that is expected to return a 202 status code and implement the
        /// long running operation pattern.
        /// </summary>
        /// <param name="requestUri">The Uri to request.</param>
        /// <param name="method">The method use to request the Uri.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An API containing the Location header from the response.</returns>
        protected Task<ApiResponse> CallLongRunningOperationEndpointAsync(
            Uri requestUri,
            HttpMethod method,
            CancellationToken cancellationToken = default)
        {
            HttpRequestMessage request = this.BuildRequest(method, requestUri);
            return this.CallLongRunningOperationEndpointInternalAsync(request, cancellationToken);
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
                response = await this.Client.SendAsync(request, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException ex)
            {
                string responseContent = (response is null || response.Content is null)
                    ? string.Empty
                    : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                throw new UserNotificationsApiException("Unexpected error when calling service; see InnerException for details.", ex)
                {
                    StatusCode = response?.StatusCode,
                    ResponseMessage = responseContent,
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
            using Stream content = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return JsonDocument.Parse(content);
        }

        private async Task<ApiResponse> CallLongRunningOperationEndpointInternalAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            ImmutableDictionary<string, string>.Builder builder = ImmutableDictionary.CreateBuilder<string, string>();
            builder.Add("Location", response.Headers.Location.ToString());

            return new ApiResponse(response.StatusCode, builder.ToImmutable());
        }
    }
}