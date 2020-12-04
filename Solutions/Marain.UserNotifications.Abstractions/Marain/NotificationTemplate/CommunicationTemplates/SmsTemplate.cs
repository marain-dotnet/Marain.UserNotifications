// <copyright file="SmsTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplate.NotificationTemplate.CommunicationTemplates
{
    /// <summary>
    /// Sms Object.
    /// </summary>
    public class SmsTemplate
    {
        /// <summary>
        /// Constructor for the Sms object.
        /// </summary>
        /// <param name="body">The inner templated text.</param>
        public SmsTemplate(
            string body)
        {
            this.Body = body;
        }

        /// <summary>
        /// Gets the body of the Sms object.
        /// </summary>
        public string Body { get; }
    }
}