// <copyright file="SerializationSettingsFactoryAdapter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Helpers
{
    using Corvus.Extensions.Json;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Newtonsoft.Json;

    /// <summary>
    /// Adapts the <see cref="IJsonSerializerSettingsProvider"/> to work with the
    /// <see cref="IMessageSerializerSettingsFactory"/> interface used by durable functions.
    /// </summary>
    public class SerializationSettingsFactoryAdapter : IMessageSerializerSettingsFactory
    {
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationSettingsFactoryAdapter"/> class.
        /// </summary>
        /// <param name="serializerSettingsProvider">The <see cref="IJsonSerializerSettingsProvider"/>.</param>
        public SerializationSettingsFactoryAdapter(IJsonSerializerSettingsProvider serializerSettingsProvider)
        {
            this.serializerSettingsProvider = serializerSettingsProvider;
        }

        /// <inheritdoc/>
        public JsonSerializerSettings CreateJsonSerializerSettings()
        {
            return this.serializerSettingsProvider.Instance;
        }
    }
}