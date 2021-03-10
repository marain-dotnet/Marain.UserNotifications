// <copyright file="Startup.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

[assembly: Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup(typeof(Marain.UserNotifications.ApiDeliveryChannel.Host.Startup))]

namespace Marain.UserNotifications.ApiDeliveryChannel.Host
{
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Startup code for the management host.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Configures the function host.
        /// </summary>
        /// <param name="builder">The functions host builder.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IServiceCollection services = builder.Services;

            services.AddLogging();

            services.AddCommonUserNotificationsApiServices();

            services.AddTenantedUserNotificationsApiDeliveryChannel();

            services.EnsureDateTimeOffsetConverterNotPresent();
        }
    }
}
