// <copyright file="BenchmarksBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Benchmarks
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Corvus.Identity.ManagedServiceIdentity.ClientAuthentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Base class for benchmarks.
    /// </summary>
    public abstract class BenchmarksBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BenchmarksBase"/> class.
        /// </summary>
        protected BenchmarksBase()
        {
            this.Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(this.Configuration);
            services.AddAzureManagedIdentityBasedTokenSource(
                sp => new AzureManagedIdentityTokenSourceOptions
                {
                    AzureServicesAuthConnectionString = this.Configuration["AzureServicesAuthConnectionString"],
                });

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            this.TokenSource = serviceProvider.GetRequiredService<IServiceIdentityTokenSource>();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the HttpClient.
        /// </summary>
        protected HttpClient HttpClient { get; } = new HttpClient();

        /// <summary>
        /// Gets the token source.
        /// </summary>
        protected IServiceIdentityTokenSource TokenSource { get; }

        /// <summary>
        /// Gets a new <see cref="HttpRequestMessage"/> configured with the correct Authorization header if required.
        /// </summary>
        /// <param name="authenticationResourceId">The resource Id for authentication. If null/empty, no authorization header will be added.</param>
        /// <returns>The new <see cref="HttpRequestMessage"/>.</returns>
        protected async Task<HttpRequestMessage> GetHttpRequestMessageWithAuthorizationHeaderAsync(string authenticationResourceId)
        {
            var request = new HttpRequestMessage();

            if (!string.IsNullOrEmpty(authenticationResourceId))
            {
                string? token = await this.TokenSource.GetAccessToken(authenticationResourceId).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return request;
        }
    }
}
