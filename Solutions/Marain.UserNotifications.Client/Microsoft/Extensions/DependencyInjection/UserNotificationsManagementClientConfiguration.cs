// <copyright file="UserNotificationsManagementClientConfiguration.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using Marain.UserNotifications.Client.Management;

    /// <summary>
    /// Configuration for the <see cref="UserNotificationsManagementClient"/>.
    /// </summary>
    public class UserNotificationsManagementClientConfiguration
    {
        /// <summary>
        /// Gets or sets the base Url of the API.
        /// </summary>
        public string BaseUri { get; set; }
    }
}
