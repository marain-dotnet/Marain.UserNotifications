// <copyright file="PropertyBagHelpers.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.Helper
{
    using System;
    using System.Collections.Generic;
    using Corvus.Extensions.Json;
    using Corvus.Json;
    using Newtonsoft.Json;

    /// <summary>
    /// Helper methods.
    /// </summary>
    public static class PropertyBagHelpers
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
        };

        /// <summary>
        /// Get dictionary from the propertybag object.
        /// </summary>
        /// <param name="properties">PropertyBag.</param>
        /// <returns>Returns Dictionary.</returns>
        public static Dictionary<string, object> GetDictionaryFromPropertyBag(IPropertyBag properties)
        {
            if (!(properties is IJsonNetPropertyBag propertyBag))
            {
                throw new ArgumentException(
                    "The input property bag must be a IJsonNetPropertyBag",
                    nameof(properties));
            }

            return (Dictionary<string, object>)propertyBag.AsDictionaryRecursive();
        }
    }
}
