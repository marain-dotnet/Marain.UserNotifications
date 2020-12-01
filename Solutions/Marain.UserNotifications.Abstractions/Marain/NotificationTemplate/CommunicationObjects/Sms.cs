// <copyright file="Sms.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplate.NotificationTemplate.CommunicationObjects
{
    /// <summary>
    /// Sms Object.
    /// </summary>
    public class Sms
    {
        /// <summary>
        /// Constructor for the Sms object.
        /// </summary>
        /// <param name="body">The inner templated text.</param>
        /// <param name="toPhoneNumber">The number the sms object will be sent to. </param>
        public Sms(
            string body,
            string toPhoneNumber)
        {
            this.Body = body;
            this.ToPhoneNumber = toPhoneNumber;
        }

        /// <summary>
        /// Gets the body of the Sms object.
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// Gets the to phone number of the Sms object.
        /// </summary>
        public string ToPhoneNumber { get; }
    }
}