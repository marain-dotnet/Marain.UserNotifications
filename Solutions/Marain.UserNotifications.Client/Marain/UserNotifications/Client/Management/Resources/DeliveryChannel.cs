// <copyright file="DeliveryChannel.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    /// <summary>
    /// Values of the different Delivery Channels which are integrated.
    /// </summary>
    public static class DeliveryChannel
    {
        /// <summary>
        /// Airship delivery channel.
        /// </summary>
        public const string Airship = "application/vnd.marain.usernotifications.deliverychannel.airship.v1";

        /// <summary>
        /// SendGrid delivery channel.
        /// </summary>
        public const string SendGrid = "application/vnd.marain.usernotifications.deliverychannel.sendgrid.v1";
    }
}
