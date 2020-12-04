// <copyright file="EmailTemplate.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    /// <summary>
    /// Email Object.
    /// </summary>
    public class EmailTemplate
    {
        /// <summary>
        /// Gets or Sets the body of the email.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or Sets the subject of the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or Sets whether the email is important or not.
        /// </summary>
        public bool? Important { get; set; }
    }
}