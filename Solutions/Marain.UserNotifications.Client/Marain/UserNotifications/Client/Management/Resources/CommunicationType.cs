// <copyright file="CommunicationType.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    /// <summary>
    /// Values for the communication types a delivery channel could use.
    /// </summary>
    public enum CommunicationType
    {
        /// <summary>
        /// The delivery channel supports Email.
        /// </summary>
        Email,

        /// <summary>
        /// The delivery channel supports Sms.
        /// </summary>
        Sms,

        /// <summary>
        /// The delivery channel supports web push
        /// </summary>
        WebPush,

        /// <summary>
        /// The delivery channel support MMS.
        /// </summary>
        MMS,
    }
}
