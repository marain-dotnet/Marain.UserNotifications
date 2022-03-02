// <copyright file="NotificationTemplateStoreConcurrencyException.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.NotificationTemplates
{
    using System;

    /// <summary>
    /// Exception raised by a notification template store when it is prevented from reading or writing a template due to
    /// a conflict.
    /// </summary>
    public class NotificationTemplateStoreConcurrencyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTemplateStoreConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">The underlying exception thrown by the store.</param>
        public NotificationTemplateStoreConcurrencyException(string message, Exception? inner)
            : base(message, inner)
        {
        }
    }
}