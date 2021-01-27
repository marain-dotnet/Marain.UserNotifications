// <copyright file="Constants.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Models
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text;

    /// <summary>
    /// Constants for tenant property names, field names, etc.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Property names for Marain tenants.
        /// </summary>
        public static class TenantPropertyNames
        {
            /// <summary>
            /// The Airship key vault url.
            /// </summary>
            public const string AirshipKeyVaultUrl = "AirshipKeyVaultUrl";
        }
    }
}
