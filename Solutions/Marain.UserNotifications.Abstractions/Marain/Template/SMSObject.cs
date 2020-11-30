// <copyright file="SMSObject.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserPreferences
{
    /// <summary>
    /// SMS Object.
    /// </summary>
    public class SMSObject
    {
        /// <summary>
        /// Ctor for the SMS object.
        /// </summary>
        /// <param name="body">The inner templated text.</param>
        public SMSObject(
            string body)
        {
            this.Body = body;
        }

        /// <summary>
        /// Gets the body of the sms object.
        /// </summary>
        public string Body { get; }
    }
}