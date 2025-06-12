// <copyright file="KeyVaultHelper.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Helpers
{
    using System;
    using System.Threading.Tasks;

    using Azure;
    using Azure.Security.KeyVault.Secrets;

    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Helper methods to work with the <see cref="SecretClient"/>.
    /// </summary>
    public static class KeyVaultHelper
    {
        /// <summary>
        /// Gets the stored key vault configuration.
        /// </summary>
        /// <param name="configuration">IConfiguration.</param>
        /// <param name="deliveryChannelSecretUrl">Azure Key Vault URL where the delivery channel's configuration is stored.</param>
        /// <returns>Delivery channel secret.</returns>
        public static async Task<string> GetDeliveryChannelSecretAsync(IConfiguration configuration, string deliveryChannelSecretUrl)
        {
            string azureConnectionString = configuration.GetValue<string>("AzureServicesAuthConnectionString") ?? throw new InvalidOperationException("AzureServicesAuthConnectionString is missing");
            var azureCredentials = Corvus.Identity.ClientAuthentication.Azure.LegacyAzureServiceTokenProviderConnectionString.ToTokenCredential(azureConnectionString);

            Uri secretUri = new(deliveryChannelSecretUrl);
            Uri vaultUri = new(secretUri, "/");
            SecretClient keyVaultClient = new(vaultUri, azureCredentials);
            Response<KeyVaultSecret> accountKey = await keyVaultClient.GetSecretAsync(deliveryChannelSecretUrl).ConfigureAwait(false);
            return accountKey.Value.Value;
        }
    }
}