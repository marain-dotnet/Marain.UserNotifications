// <copyright file="UserNotificationsOpenApiServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using Marain.UserNotifications;
    using Marain.UserNotifications.OpenApi.ApiDeliveryChannel;
    using Marain.UserNotifications.OpenApi.ApiDeliveryChannel.Mappers;
    using Menes;

    /// <summary>
    /// Extension methods for configuring DI for the OpenApi service implementations.
    /// </summary>
    public static class UserNotificationsOpenApiServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the OpenApiServices for the API delivery channel.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection, to enable chaining.</returns>
        public static IServiceCollection AddApiDeliveryChannelOpenApiServices(
            this IServiceCollection services)
        {
            services.AddHalDocumentMapper<GetNotificationsResult, UserNotificationsMappingContext, UserNotificationsMapper>();
            services.AddHalDocumentMapper<UserNotification, IOpenApiContext, UserNotificationMapper>();

            services.AddSingleton<IOpenApiService, GetNotificationService>();
            services.AddSingleton<IOpenApiService, GetNotificationsForUserService>();
            services.AddSingleton<IOpenApiService, MarkNotificationAsReadService>();

            return services;
        }
    }
}
