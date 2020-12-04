// <copyright file="EmailTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplate.NotificationTemplate.CommunicationTemplates
{
    using System.Collections.Generic;

    /// <summary>
    /// Email Object.
    /// </summary>
    public class EmailTemplate
    {
        /// <summary>
        /// Constructor for the Email object.
        /// </summary>
        /// <param name="body">The inner templated text.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="important">Set if the email is important.</param>
        public EmailTemplate(
            string body,
            string subject,
            bool? important)
        {
            this.Body = body;
            this.Subject = subject;
            this.Important = important;
        }

        /// <summary>
        /// Gets the body of the email.
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// Gets the subject of the email.
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// Gets whether the email is important or not.
        /// </summary>
        public bool? Important { get; }
    }
}