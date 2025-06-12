// <copyright file="ContinuationToken.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureStorage.Internal
{
    using System;
    using System.Text;
    using System.Text.Json;

    /// <summary>
    /// A continuation token for a request to the user notifications table storage call.
    /// </summary>
    public class ContinuationToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuationToken"/> class.
        /// </summary>
        /// <param name="userId">The <see cref="UserId"/>.</param>
        /// <param name="maxItems">The <see cref="MaxItems"/>.</param>
        /// <param name="beforeRowKey">The <see cref="BeforeRowKey"/>.</param>
        /// <param name="afterRowKey">The <see cref="AfterRowKey"/>.</param>
        public ContinuationToken(string userId, int maxItems, string? beforeRowKey, string? afterRowKey)
        {
            this.UserId = userId;
            this.MaxItems = maxItems;
            this.BeforeRowKey = beforeRowKey;
            this.AfterRowKey = afterRowKey;
        }

        /// <summary>
        /// Gets the Id of the user for whom notifications were requested.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets the maximum items that were requested.
        /// </summary>
        public int MaxItems { get; }

        /// <summary>
        /// Gets the row key of the notification that all returned notifications must be older than.
        /// </summary>
        public string? BeforeRowKey { get; }

        /// <summary>
        /// Gets the row key of the notification that all returned notifications must be more recent than.
        /// </summary>
        public string? AfterRowKey { get; }

        /// <summary>
        /// Recreates a continuation token from its string representation.
        /// </summary>
        /// <param name="input">The string representation of the token.</param>
        /// <param name="serializerOptions">The <see cref="JsonSerializerOptions"/> to use.</param>
        /// <returns>The continuation token.</returns>
        public static ContinuationToken FromString(string input, JsonSerializerOptions serializerOptions)
        {
            string serializedToken = Encoding.UTF8.GetString(Convert.FromBase64String(input));
            return JsonSerializer.Deserialize<ContinuationToken>(serializedToken, serializerOptions)!;
        }

        /// <summary>
        /// Returns a string representation of the token.
        /// </summary>
        /// <param name="serializerOptions">The <see cref="JsonSerializerOptions"/> to use.</param>
        /// <returns>A string representation of the token.</returns>
        public string AsString(JsonSerializerOptions serializerOptions)
        {
            string serializedToken = JsonSerializer.Serialize(this, serializerOptions);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serializedToken));
        }
    }
}