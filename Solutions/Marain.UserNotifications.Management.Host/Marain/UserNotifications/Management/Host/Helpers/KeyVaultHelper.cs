// <copyright file="KeyVaultHelper.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Helpers
{
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Helper methods to work with the <see cref="Microsoft.Azure.KeyVault"/>.
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
            string? azureConnectionString = configuration.GetValue<string>("AzureServicesAuthConnectionString");
            var azureServiceTokenProvider = new Microsoft.Azure.Services.AppAuthentication.AzureServiceTokenProvider(azureConnectionString);
            using var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            Microsoft.Azure.KeyVault.Models.SecretBundle accountKey = await keyVaultClient.GetSecretAsync(deliveryChannelSecretUrl).ConfigureAwait(false);
            return accountKey.Value;
        }
    }
}
