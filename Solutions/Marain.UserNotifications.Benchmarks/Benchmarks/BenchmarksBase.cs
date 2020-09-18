// <copyright file="BenchmarksBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Benchmarks
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Corvus.Identity.ManagedServiceIdentity.ClientAuthentication;
    using Marain.UserNotifications.Client.ApiDeliveryChannel;
    using Marain.UserNotifications.Client.Management;
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

            services.AddUserNotificationsManagementClient(_ => this.Configuration.GetSection("ManagementApi").Get<UserNotificationsManagementClientConfiguration>());
            services.AddUserNotificationsApiDeliveryChannelClient(_ => this.Configuration.GetSection("ApiDeliveryChannel").Get<UserNotificationsApiDeliveryChannelClientConfiguration>());

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            this.ApiDeliveryChannelClient = serviceProvider.GetRequiredService<IUserNotificationsApiDeliveryChannelClient>();
            this.ManagementClient = serviceProvider.GetRequiredService<IUserNotificationsManagementClient>();

            this.BenchmarkClientTenantId = this.Configuration["BenchmarkClientTenantId"];
        }

        /// <summary>
        /// Gets the API delivery channel client.
        /// </summary>
        public IUserNotificationsApiDeliveryChannelClient ApiDeliveryChannelClient { get; }

        /// <summary>
        /// Gets the management client.
        /// </summary>
        public IUserNotificationsManagementClient ManagementClient { get; }

        /// <summary>
        /// Gets the tenant Id for benchmarking.
        /// </summary>
        public string BenchmarkClientTenantId { get; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        protected IConfiguration Configuration { get; }
    }
}
