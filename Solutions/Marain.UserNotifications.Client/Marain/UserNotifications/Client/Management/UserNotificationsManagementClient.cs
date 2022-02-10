// <copyright file="UserNotificationsManagementClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Marain.UserNotifications.Client.Management.Requests;
    using Marain.UserNotifications.Client.Management.Resources;
    using Marain.UserNotifications.Client.Management.Resources.CommunicationTemplates;

    /// <summary>
    /// Client for the user notifications management service.
    /// </summary>
    public class UserNotificationsManagementClient : ClientBase, IUserNotificationsManagementClient
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserNotificationsManagementClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests. Should be initialised with the service base Url.</param>
        public UserNotificationsManagementClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <inheritdoc />
        public Task<ApiResponse> BatchDeliveryStatusUpdateAsync(
            string tenantId,
            IEnumerable<BatchDeliveryStatusUpdateRequestItem> body,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            var requestUri = new Uri($"/{tenantId}/marain/usernotifications/batchdeliverystatusupdate", UriKind.Relative);

            return this.CallLongRunningOperationEndpointAsync(requestUri, HttpMethod.Post, body, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ApiResponse> BatchReadStatusUpdateAsync(
            string tenantId,
            IEnumerable<BatchReadStatusUpdateRequestItem> body,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            var requestUri = new Uri($"/{tenantId}/marain/usernotifications/batchreadstatusupdate", UriKind.Relative);

            return this.CallLongRunningOperationEndpointAsync(requestUri, HttpMethod.Post, body, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ApiResponse> CreateNotificationsAsync(
            string tenantId,
            CreateNotificationsRequest body,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            var requestUri = new Uri($"/{tenantId}/marain/usernotifications", UriKind.Relative);

            return this.CallLongRunningOperationEndpointAsync(requestUri, HttpMethod.Put, body, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ApiResponse> CreateNotificationForDeliveryChannelsAsync(
            string tenantId,
            CreateNotificationForDeliveryChannelsRequest body,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            var requestUri = new Uri($"/{tenantId}/marain/usernotifications/v2", UriKind.Relative);

            return this.CallLongRunningOperationEndpointAsync(requestUri, HttpMethod.Put, body, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<WebPushTemplateResource>> GetWebPushNotificationTemplate(
            string tenantId,
            string notificationType,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (string.IsNullOrEmpty(notificationType))
            {
                throw new ArgumentNullException(nameof(notificationType));
            }

            string communicationType = "webPush";

            Uri requestUri = this.ConstructUri($"/{tenantId}/marain/usernotifications/templates?notificationType={notificationType}&communicationType={communicationType}");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Get, requestUri);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            WebPushTemplateResource result = await JsonSerializer.DeserializeAsync<WebPushTemplateResource>(contentStream, this.SerializerOptions).ConfigureAwait(false);

            return new ApiResponse<WebPushTemplateResource>(
               response.StatusCode,
               result,
               WrapETagInImmutableDictionary(response));
        }

        /// <inheritdoc />
        public Task<ApiResponse<WebPushTemplateResource>> GetWebPushNotificationTemplateByLinkAsync(
            string link,
            CancellationToken cancellationToken = default)
        {
            return this.GetPathAsync<WebPushTemplateResource>(link, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<EmailTemplateResource>> GetEmailNotificationTemplate(
            string tenantId,
            string notificationType,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (string.IsNullOrEmpty(notificationType))
            {
                throw new ArgumentNullException(nameof(notificationType));
            }

            string communicationType = "email";

            Uri requestUri = this.ConstructUri($"/{tenantId}/marain/usernotifications/templates?notificationType={notificationType}&communicationType={communicationType}");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Get, requestUri);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            EmailTemplateResource result = await JsonSerializer.DeserializeAsync<EmailTemplateResource>(contentStream, this.SerializerOptions).ConfigureAwait(false);

            return new ApiResponse<EmailTemplateResource>(
               response.StatusCode,
               result,
               WrapETagInImmutableDictionary(response));
        }

        /// <inheritdoc />
        public Task<ApiResponse<EmailTemplateResource>> GetEmailNotificationTemplateByLinkAsync(
            string link,
            CancellationToken cancellationToken = default)
        {
            return this.GetPathAsync<EmailTemplateResource>(link, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<SmsTemplateResource>> GetSmsNotificationTemplate(
            string tenantId,
            string notificationType,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (string.IsNullOrEmpty(notificationType))
            {
                throw new ArgumentNullException(nameof(notificationType));
            }

            string communicationType = "sms";

            Uri requestUri = this.ConstructUri($"/{tenantId}/marain/usernotifications/templates?notificationType={notificationType}&communicationType={communicationType}");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Get, requestUri);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            SmsTemplateResource result = await JsonSerializer.DeserializeAsync<SmsTemplateResource>(contentStream, this.SerializerOptions).ConfigureAwait(false);

            return new ApiResponse<SmsTemplateResource>(
               response.StatusCode,
               result,
               WrapETagInImmutableDictionary(response));
        }

        /// <inheritdoc />
        public Task<ApiResponse<SmsTemplateResource>> GetSmsNotificationTemplateByLinkAsync(
            string link,
            CancellationToken cancellationToken = default)
        {
            return this.GetPathAsync<SmsTemplateResource>(link, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<ApiResponse> SetNotificationTemplate(
            string tenantId,
            ICommunicationTemplate communicationTemplate,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (communicationTemplate == null)
            {
                throw new ArgumentNullException(nameof(communicationTemplate));
            }

            Uri requestUri = this.ConstructUri($"/{tenantId}/marain/usernotifications/templates");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Put, requestUri, communicationTemplate);
            request = this.AddETagToNotificationTemplateHeaders(communicationTemplate, request);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            return new ApiResponse(response.StatusCode);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<NotificationTemplate>> GenerateNotificationTemplate(
            string tenantId,
            CreateNotificationsRequest createNotificationsRequest,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (createNotificationsRequest == null)
            {
                throw new ArgumentNullException(nameof(createNotificationsRequest));
            }

            Uri requestUri = this.ConstructUri($"{tenantId}/marain/usernotifications/templates/generate");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Put, requestUri, createNotificationsRequest);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            NotificationTemplate result = await JsonSerializer.DeserializeAsync<NotificationTemplate>(contentStream, this.SerializerOptions).ConfigureAwait(false);

            return new ApiResponse<NotificationTemplate>(
                response.StatusCode,
                result);
        }

        private static ImmutableDictionary<string, string> WrapETagInImmutableDictionary(HttpResponseMessage response)
        {
            ImmutableDictionary<string, string>.Builder builder = ImmutableDictionary.CreateBuilder<string, string>();
            if (!string.IsNullOrEmpty(response.Headers.ETag?.Tag))
            {
                builder.Add("ETag", response.Headers.ETag.Tag);
            }

            return builder.ToImmutable();
        }

        private HttpRequestMessage AddETagToHeader(HttpRequestMessage httpRequestMessage, string eTag)
        {
            if (!string.IsNullOrWhiteSpace(eTag))
            {
                httpRequestMessage.Headers.Add("If-None-Match", eTag);
            }

            return httpRequestMessage;
        }

        private HttpRequestMessage AddETagToNotificationTemplateHeaders(ICommunicationTemplate communicationTemplate, HttpRequestMessage httpRequestMessage)
        {
            if (communicationTemplate is EmailTemplate emailTemplate)
            {
                return this.AddETagToHeader(httpRequestMessage, emailTemplate.ETag);
            }
            else if (communicationTemplate is SmsTemplate smsTemplate)
            {
                return this.AddETagToHeader(httpRequestMessage, smsTemplate.ETag);
            }
            else if (communicationTemplate is WebPushTemplate webPushTemplate)
            {
                return this.AddETagToHeader(httpRequestMessage, webPushTemplate.ETag);
            }

            return null;
        }
    }
}