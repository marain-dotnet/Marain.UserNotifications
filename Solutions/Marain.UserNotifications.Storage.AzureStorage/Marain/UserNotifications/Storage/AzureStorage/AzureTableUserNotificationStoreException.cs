// <copyright file="AzureTableUserNotificationStoreException.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#pragma warning disable RCS1194 // Roslynator's 'all the constructors' fixation

namespace Marain.UserNotifications.Storage.AzureStorage
{
    using System;

    /// <summary>
    /// Exception thrown when a request to the user notification store fails for some unexpected reason.
    /// </summary>
    public class AzureTableUserNotificationStoreException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableUserNotificationStoreException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The original exception, if any.</param>
        public AzureTableUserNotificationStoreException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}
