// <copyright file="AirshipDeliveryChannel.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using System.Collections.Generic;
    using Marain.Models;
    using Marain.UserNotifications.ThirdParty.DeliveryChannels.Interfaces;

    /// <summary>
    /// Airship Delivery Channel.
    /// </summary>
    public class AirshipDeliveryChannel : IDeliveryChannel
    {
        /// <summary>
        /// The content type that will be used when serializing/deserializing.
        /// </summary>
        private const string RegisteredContentType = "application/vnd.marain.usernotifications.thirdparty.deliverychannels.airship.v1";

        /// <summary>
        /// Initilises the <see cref="AirshipDeliveryChannel"/> with predefined settings.
        /// </summary>
        public AirshipDeliveryChannel()
        {
            this.SupportedCommunicationTypes = new List<CommunicationType>() { CommunicationType.WebPush };
        }

        /// <summary>
        /// Gets the registered content type used when this object is serialized/deserialized.
        /// </summary>
        public string ContentType => RegisteredContentType;

        /// <summary>
        /// Gets list of supported communication types.
        /// </summary>
        public IList<CommunicationType> SupportedCommunicationTypes { get; }
    }
}
