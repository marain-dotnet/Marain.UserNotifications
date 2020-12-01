// <copyright file="Email.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplate.NotificationTemplate.CommunicationObjects
{
    using System.Collections.Generic;

    /// <summary>
    /// Email Object.
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Constructor for the Email object.
        /// </summary>
        /// <param name="body">The inner templated text.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="important">Set if the email is important.</param>
        /// <param name="fromName">The from name of the email.</param>
        /// <param name="fromEmail">The from email of the email.</param>
        /// <param name="toEmails">The email addresses this email will be sent to.</param>
        /// <param name="cCEmails">The carbon copy email addresses this email will be sent to.</param>
        /// <param name="bCCEmails">The blind carbon copy email addresses this email will be sent to.</param>
        public Email(
            string body,
            string subject,
            bool? important,
            string fromName,
            string fromEmail,
            IEnumerable<string> toEmails,
            IEnumerable<string> cCEmails,
            IEnumerable<string> bCCEmails)
        {
            this.Body = body;
            this.Subject = subject;
            this.Important = important;
            this.FromName = fromName;
            this.FromEmail = fromEmail;
            this.ToEmails = toEmails;
            this.CCEmails = cCEmails;
            this.BCCEmails = bCCEmails;
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

        /// <summary>
        /// Gets the name of who the email is from.
        /// </summary>
        public string FromName { get; }

        /// <summary>
        /// Gets the email address of who the email is from.
        /// </summary>
        public string FromEmail { get; }

        /// <summary>
        ///  Gets a list of email addresses who will receive this email.
        /// </summary>
        public IEnumerable<string> ToEmails { get; }

        /// <summary>
        /// Gets a list of email addresses who will receive carbon copies of this email.
        /// </summary>
        public IEnumerable<string> CCEmails { get; }

        /// <summary>
        /// Gets a list of email addresses who will receive blind carbon copies of this email.
        /// </summary>
        public IEnumerable<string> BCCEmails { get; }
    }
}