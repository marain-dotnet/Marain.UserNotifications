namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using System.Collections.Generic;
    using Marain.Models;
    using Marain.UserNotifications.ThirdParty.DeliveryChannels.Interfaces;

    public class AirshipDeliveryChannel : IDeliveryChannel
    {
        /// <summary>
        /// The content type that will be used when serializing/deserializing.
        /// </summary>
        private const string RegisteredContentType = "application/vnd.marain.usernotifications.thirdparty.deliverychannels.airship.v1";

        public AirshipDeliveryChannel()
        {
            this.SupportedCommunicationTypes = new List<CommunicationType>() { CommunicationType.WebPush };
        }

        public string ContentType => RegisteredContentType;

        public IList<CommunicationType> SupportedCommunicationTypes { get; }
    }
}
