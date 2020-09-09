// <copyright file="UserNotificationsApiException.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    /// <summary>
    /// An exception thrown by a UserNotifications API client.
    /// </summary>
    [Serializable]
    public class UserNotificationsApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationsApiException"/> class.
        /// </summary>
        public UserNotificationsApiException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationsApiException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UserNotificationsApiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationsApiException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">The underlying exception.</param>
        public UserNotificationsApiException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationsApiException"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        protected UserNotificationsApiException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets or sets the status code of the response.
        /// </summary>
        public HttpStatusCode? StatusCode { get; set; }
    }
}
