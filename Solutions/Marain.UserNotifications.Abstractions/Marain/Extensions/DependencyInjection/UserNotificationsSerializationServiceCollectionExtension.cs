// <copyright file="UserNotificationsSerializationServiceCollectionExtension.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.Extensions.DependencyInjection
{
    using Corvus.ContentHandling;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Adds content types to a service collection enabling serialization and deserializion of email, SMS, and web push templates.
    /// </summary>
    public static class UserNotificationsSerializationServiceCollectionExtension
    {
        /// <summary>
        /// Adds user notification object types that include contentType
        /// to the service collection.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection" /> to add the types to.
        /// </param>
        /// <returns>
        /// The <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection RegisterCoreUserNotificationsContentTypes(this IServiceCollection services)
        {
            services.AddContent(factory =>
            {
                factory.RegisterContent<EmailTemplate>();
                factory.RegisterContent<SmsTemplate>();
                factory.RegisterContent<WebPushTemplate>();
            });

            return services;
        }
    }
}