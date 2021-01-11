// <copyright file="IDeliveryChannel.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Interfaces
{
    /// <summary>
    /// Interface for the delivery channels to be implemented from.
    /// </summary>
    public interface IDeliveryChannel
    {
        /// <summary>
        /// Gets the content type that will determine the version and type of the delivery channel
        /// </summary>
        string ContentType { get; }
    }
}
