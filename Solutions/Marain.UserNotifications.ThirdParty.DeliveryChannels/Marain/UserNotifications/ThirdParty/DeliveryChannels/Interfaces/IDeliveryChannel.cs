// <copyright file="IDeliveryChannel.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Interfaces
{
    using System.Collections.Generic;
    using Marain.Models;

    /// <summary>
    /// Interface for the delivery channels to be implemented from.
    /// </summary>
    public interface IDeliveryChannel
    {
        /// <summary>
        /// Gets the content type that will determine the version and type of the delivery channel.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets list of supported communication types for implemented delivery channel.
        /// </summary>
        IList<CommunicationType> SupportedCommunicationTypes { get; }
    }
}