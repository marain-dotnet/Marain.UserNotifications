// <copyright file="TransientTenantBindings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Specs.Bindings
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Azure.Data.Tables;

    using Corvus.Storage.Azure.BlobStorage.Tenancy;
    using Corvus.Storage.Azure.TableStorage.Tenancy;
    using Corvus.Tenancy;
    using Corvus.Testing.AzureFunctions;
    using Corvus.Testing.AzureFunctions.ReqnRoll;
    using Corvus.Testing.SpecFlow;

    using Dynamitey.DynamicObjects;

    using Marain.TenantManagement.Configuration;
    using Marain.TenantManagement.EnrollmentConfiguration;
    using Marain.TenantManagement.Testing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.EnvironmentAccess;

    /// <summary>
    /// Bindings to manage creation and deletion of tenants for test features.
    /// </summary>
    [Binding]
    public static class TransientTenantBindings
    {
        /// <summary>
        /// Creates a new <see cref="ITenant"/> for the current feature, adding test storage configurations
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
                ITableSourceWithTenantLegacyTransition cloudTableFactory = ContainerBindings.GetServiceProvider(featureContext).GetRequiredService<ITableSourceWithTenantLegacyTransition>();

                TableClient testTable = await cloudTableFactory.GetTableClientFromTenantAsync(
                    tenant: tenantManager.PrimaryTransientClient,
                    v2ConfigurationKey: "StorageConfiguration__Table__usernotifications",
                    v3ConfigurationKey: "Marain:UserNotifications:TableConfiguration:UserNotifications",
                    containerName: "usernotifications").ConfigureAwait(false);
                await testTable.DeleteAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            await featureContext.RunAndStoreExceptionsAsync(async () => await tenantManager.CleanupAsync()).ConfigureAwait(false);
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

        private static EnrollmentConfigurationEntry GetUserNotificationsConfig(FeatureContext featureContext)
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
            LegacyV2TableConfiguration tableStorageConfiguration =
                configuration.GetSection("TestTableStorageConfiguration").Get<LegacyV2TableConfiguration>()
                ?? new LegacyV2TableConfiguration();

            if (string.IsNullOrEmpty(tableStorageConfiguration.AccountName))
            {
                logger.LogDebug("No configuration value 'TestTableStorageConfiguration:AccountName' provided; using local storage emulator.");
            }

            LegacyV2BlobStorageConfiguration blobStorageConfiguration =
                configuration.GetSection("TestBlobStorageConfiguration").Get<LegacyV2BlobStorageConfiguration>()
                ?? new LegacyV2BlobStorageConfiguration();

            if (string.IsNullOrEmpty(blobStorageConfiguration.AccountName))
            {
                logger.LogDebug("No configuration value 'TestBlobStorageConfiguration:AccountName' provided; using local storage emulator.");
            }

            return new EnrollmentConfigurationEntry(
                new Dictionary<string, ConfigurationItem>
                {
                    {
                        "Marain:UserNotifications:TableConfiguration:UserNotificationsMarain:UserNotifications:TableConfiguration:UserNotifications",
                        new LegacyV2TableStorageConfigurationItem { Configuration = tableStorageConfiguration }
                    },
                    {
                        "Marain:UserNotifications:BlobContainerConfiguration:Templates",
                        new LegacyV2BlobStorageConfigurationItem { Configuration = blobStorageConfiguration }
                    },
                },
                new Dictionary<string, EnrollmentConfigurationEntry>
                {
                    {
                        // Operations
                        "3633754ac4c9be44b55bfe791b1780f12429524fe7b6cc48a265a307407ec858",
                        new EnrollmentConfigurationEntry(
                            new Dictionary<string, ConfigurationItem>
                            {
                                {
                                    "Marain:Operations:BlobContainerConfiguration:Operations",
                                    new LegacyV2BlobStorageConfigurationItem { Configuration = blobStorageConfiguration }
                                },
                            },
                            null)
                    },
                });
        }
    }
}