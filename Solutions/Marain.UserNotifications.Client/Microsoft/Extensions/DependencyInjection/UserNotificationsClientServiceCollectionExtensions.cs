﻿// <copyright file="UserNotificationsClientServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.Identity.ManagedServiceIdentity.ClientAuthentication;
    using Marain.UserNotifications.Client.ApiDeliveryChannel;
    using Marain.UserNotifications.Client.Management;
    using Microsoft.Rest;

    /// <summary>
    /// Extension methods to add the service client to the DI container.
    /// </summary>
    public static class UserNotificationsClientServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the management client to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configurationCallback">A callback to retrieve configuration for the client.</param>
        /// <returns>The service collection, for chaining.</returns>
        public static IServiceCollection AddUserNotificationsManagementClient(
            this IServiceCollection services,
            Func<IServiceProvider, UserNotificationsManagementClientConfiguration> configurationCallback)
        {
            services.AddHttpClient<IUserNotificationsManagementClient, UserNotificationsManagementClient>((sp, client) =>
            {
                UserNotificationsManagementClientConfiguration config = configurationCallback(sp);
                client.BaseAddress = new Uri(config.BaseUri);
            }).AddHttpMessageHandler(sp =>
            {
                IServiceIdentityTokenSource tokenSource = sp.GetRequiredService<IServiceIdentityTokenSource>();
                UserNotificationsManagementClientConfiguration config = configurationCallback(sp);
                var provider = new ServiceIdentityTokenProvider(tokenSource, config.ResourceIdForMsiAuthentication);
                var credentials = new TokenCredentials(provider);

                return new AddAuthenticationHeaderHandler(tokenSource, config.ResourceIdForMsiAuthentication);
            });

            return services;
        }

        /// <summary>
        /// Adds the management client to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configurationCallback">A callback to retrieve configuration for the client.</param>
        /// <returns>The service collection, for chaining.</returns>
        public static IServiceCollection AddUserNotificationsApiDeliveryChannelClient(
            this IServiceCollection services,
            Func<IServiceProvider, UserNotificationsApiDeliveryChannelClientConfiguration> configurationCallback)
        {
            services.AddHttpClient<IUserNotificationsApiDeliveryChannelClient, UserNotificationsApiDeliveryChannelClient>((sp, client) =>
            {
                UserNotificationsApiDeliveryChannelClientConfiguration config = configurationCallback(sp);
                client.BaseAddress = new Uri(config.BaseUri);
            }).AddHttpMessageHandler(sp =>
            {
                IServiceIdentityTokenSource tokenSource = sp.GetRequiredService<IServiceIdentityTokenSource>();
                UserNotificationsApiDeliveryChannelClientConfiguration config = configurationCallback(sp);
                var provider = new ServiceIdentityTokenProvider(tokenSource, config.ResourceIdForMsiAuthentication);
                var credentials = new TokenCredentials(provider);

                return new AddAuthenticationHeaderHandler(tokenSource, config.ResourceIdForMsiAuthentication);
            });

            return services;
        }

        private class AddAuthenticationHeaderHandler : DelegatingHandler
        {
            private readonly TokenCredentials credentials;

            public AddAuthenticationHeaderHandler(IServiceIdentityTokenSource tokenSource, string resourceIdForMsiAuthentication)
            {
                if (!string.IsNullOrEmpty(resourceIdForMsiAuthentication))
                {
                    var provider = new ServiceIdentityTokenProvider(tokenSource, resourceIdForMsiAuthentication);
                    this.credentials = new TokenCredentials(provider);
                }
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (this.credentials != null)
                {
                    await this.credentials.ProcessHttpRequestAsync(request, cancellationToken).ConfigureAwait(false);
                }

                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
