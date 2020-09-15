// <copyright file="TransientTenantBindings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Bindings
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Azure.Cosmos.Tenancy;
    using Corvus.Azure.Storage.Tenancy;
    using Corvus.Tenancy;
    using Corvus.Testing.AzureFunctions;
    using Corvus.Testing.AzureFunctions.SpecFlow;
    using Corvus.Testing.SpecFlow;
    using Marain.TenantManagement.EnrollmentConfiguration;
    using Marain.TenantManagement.Testing;
    using Marain.UserNotifications.Storage.AzureTable;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Bindings to manage creation and deletion of tenants for test features.
    /// </summary>
    [Binding]
    public static class TransientTenantBindings
    {
        /// <summary>
        /// Creates a new <see cref="ITenant"/> for the current feature, adding a test <see cref="CosmosConfiguration"/>
        /// to the tenant data.
        /// </summary>
        /// <param name="featureContext">The current <see cref="FeatureContext"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// The newly created tenant is added to the <see cref="FeatureContext"/>. Access it via the helper methods
        /// <see cref="GetTransientTenant(FeatureContext)"/> or <see cref="GetTransientTenantId(FeatureContext)"/>.
        /// </remarks>
        [BeforeFeature("useTransientTenant", Order = BindingSequence.TransientTenantSetup)]
        public static async Task SetupTransientTenant(FeatureContext featureContext)
        {
            ITenantProvider tenantProvider = ContainerBindings.GetServiceProvider(featureContext).GetRequiredService<ITenantProvider>();
            var transientTenantManager = TransientTenantManager.GetInstance(featureContext);
            await transientTenantManager.EnsureInitialised().ConfigureAwait(false);

            // Create a transient service tenant for testing purposes.
            ITenant transientServiceTenant = await transientTenantManager.CreateTransientServiceTenantFromEmbeddedResourceAsync(
                typeof(TransientTenantBindings).Assembly,
                "Marain.UserNotifications.Specs.ServiceManifests.UserNotificationsServiceManifest.jsonc").ConfigureAwait(false);

            // Now update the service Id in our configuration and in the function configuration
            UpdateServiceConfigurationWithTransientTenantId(featureContext, transientServiceTenant);

            // Now we need to construct a transient client tenant for the test, and enroll it in the new
            // transient service.
            ITenant transientClientTenant = await transientTenantManager.CreateTransientClientTenantAsync().ConfigureAwait(false);

            await transientTenantManager.AddEnrollmentAsync(
                transientClientTenant.Id,
                transientServiceTenant.Id,
                GetUserNotificationsConfig(featureContext)).ConfigureAwait(false);

            // TODO: Temporary hack to work around the fact that the transient tenant manager no longer holds the latest
            // version of the tenants it's tracking; see https://github.com/marain-dotnet/Marain.TenantManagement/issues/28
            transientTenantManager.PrimaryTransientClient = await tenantProvider.GetTenantAsync(transientClientTenant.Id).ConfigureAwait(false);
        }

        [AfterFeature("useTransientTenant")]
        public static async Task TearDownTenants(FeatureContext featureContext)
        {
            var tenantManager = TransientTenantManager.GetInstance(featureContext);

            await featureContext.RunAndStoreExceptionsAsync(async () =>
            {
                ITenantCloudTableFactory cloudTableFactory = ContainerBindings.GetServiceProvider(featureContext).GetRequiredService<ITenantCloudTableFactory>();
                CloudTable testTable = await cloudTableFactory.GetTableForTenantAsync(tenantManager.PrimaryTransientClient, TenantedAzureTableUserNotificationStoreFactory.TableDefinition).ConfigureAwait(false);
                await testTable.DeleteIfExistsAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            await featureContext.RunAndStoreExceptionsAsync(() =>
            {
                return tenantManager.CleanupAsync();
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the transient tenant created for the current feature from the supplied <see cref="FeatureContext"/>,
        /// or null if there is none.
        /// </summary>
        /// <param name="context">The current <see cref="FeatureContext"/>.</param>
        /// <returns>The <see cref="ITenant"/>.</returns>
        public static ITenant GetTransientTenant(this FeatureContext context)
        {
            return TransientTenantManager.GetInstance(context).PrimaryTransientClient;
        }

        /// <summary>
        /// Retrieves the Id of the transient tenant created for the current feature from the supplied feature context.
        /// <see cref="FeatureContext"/>.
        /// </summary>
        /// <param name="context">The current <see cref="FeatureContext"/>.</param>
        /// <returns>The Id of the <see cref="ITenant"/>.</returns>
        /// <exception cref="ArgumentNullException">There is no current tenant.</exception>
        public static string GetTransientTenantId(this FeatureContext context)
        {
            return context.GetTransientTenant().Id;
        }

        private static void UpdateServiceConfigurationWithTransientTenantId(
            FeatureContext featureContext,
            ITenant transientServiceTenant)
        {
            FunctionConfiguration functionConfiguration = FunctionsBindings.GetFunctionConfiguration(featureContext);

            functionConfiguration.EnvironmentVariables.Add(
                "MarainServiceConfiguration:ServiceTenantId",
                transientServiceTenant.Id);

            functionConfiguration.EnvironmentVariables.Add(
                "MarainServiceConfiguration:ServiceDisplayName",
                transientServiceTenant.Name);
        }

        private static EnrollmentConfigurationItem[] GetUserNotificationsConfig(FeatureContext featureContext)
        {
            IConfiguration configuration = ContainerBindings
                .GetServiceProvider(featureContext)
                .GetRequiredService<IConfiguration>();

            // Can't create a logger using the generic type of this class because it's static, so we'll do it using
            // the feature context instead.
            ILogger<FeatureContext> logger = ContainerBindings
                .GetServiceProvider(featureContext)
                .GetRequiredService<ILogger<FeatureContext>>();

            // Load the config items we need:
            TableStorageConfiguration tableStorageConfiguration =
                configuration.GetSection("TestTableStorageConfiguration").Get<TableStorageConfiguration>()
                ?? new TableStorageConfiguration();

            if (string.IsNullOrEmpty(tableStorageConfiguration.AccountName))
            {
                logger.LogDebug("No configuration value 'TestTableStorageConfiguration:AccountName' provided; using local storage emulator.");
            }

            BlobStorageConfiguration blobStorageConfiguration =
                configuration.GetSection("TestBlobStorageConfiguration").Get<BlobStorageConfiguration>()
                ?? new BlobStorageConfiguration();

            if (string.IsNullOrEmpty(blobStorageConfiguration.AccountName))
            {
                logger.LogDebug("No configuration value 'TestBlobStorageConfiguration:AccountName' provided; using local storage emulator.");
            }

            return new EnrollmentConfigurationItem[]
            {
                new EnrollmentTableStorageConfigurationItem
                {
                    Key = "userNotificationStore",
                    Configuration = tableStorageConfiguration,
                },
                new EnrollmentBlobStorageConfigurationItem
                {
                    Key = "operationsStore",
                    Configuration = blobStorageConfiguration,
                },
            };
        }
    }
}
