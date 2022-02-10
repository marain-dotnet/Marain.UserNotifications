// <copyright file="UserNotificationStoreConcurrencyException.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#pragma warning disable RCS1194 // Roslynator's 'all the constructors' fixation

namespace Marain.UserNotifications
{
    using System;

    /// <summary>
    /// Exception raised by a notification store when it is prevented from reading or writing a notification due to
    /// a conflict.
    /// </summary>
    public class UserNotificationStoreConcurrencyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationStoreConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">The underlying exception thrown by the store.</param>
        public UserNotificationStoreConcurrencyException(string message, Exception? inner)
            : base(message, inner)
        {
        }
    }
}