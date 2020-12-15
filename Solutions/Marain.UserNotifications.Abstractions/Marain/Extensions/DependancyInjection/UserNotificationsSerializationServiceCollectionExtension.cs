// <copyright file="UserNotificationsSerializationServiceCollectionExtension.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.Extensions.DependancyInjection
{
    using Corvus.ContentHandling;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Class that provides a method for adding user notification objects
    /// to the service collection so they can be serialized and deserialised by
    /// their content type.
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
                factory.RegisterTransientContent<EmailTemplate>();
                factory.RegisterTransientContent<SmsTemplate>();
                factory.RegisterTransientContent<WebPushTemplate>();
            });

            return services;
        }
    }
}
