// <copyright file="UserNotificationsApiDeliveryChannelClientConfiguration.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using Marain.UserNotifications.Client.ApiDeliveryChannel;

    /// <summary>
    /// Configuration for the <see cref="UserNotificationsApiDeliveryChannelClient"/>.
    /// </summary>
    public class UserNotificationsApiDeliveryChannelClientConfiguration
    {
        /// <summary>
        /// Gets or sets the base Url of the API.
        /// </summary>
        public string BaseUri { get; set; }
    }
}
