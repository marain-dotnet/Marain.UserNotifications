<#
This is called during Marain.Instance infrastructure deployment after the Marain-PreDeploy.ps
script. It is our opportunity to create Azure resources.
#>

# Marain.Instance expects us to define just this one function.
Function MarainDeployment([MarainServiceDeploymentContext] $ServiceDeploymentContext) {

    # TODO: make this discoverable
    $serviceTenantId = '3633754ac4c9be44b55bfe791b1780f17ffa2f897c1169458ecb7240edb9f0c3'
    $serviceTenantDisplayName = 'User Notifications v1'

    [MarainAppService]$TenancyService = $ServiceDeploymentContext.InstanceContext.GetCommonAppService("Marain.Tenancy")
    [MarainAppService]$OperationsService = $ServiceDeploymentContext.InstanceContext.GetCommonAppService("Marain.Tenancy.Operations.Control")

    $ApiDeliveryChannelAppId = $ServiceDeploymentContext.GetAppId("del")
    $ManagementAppId = $ServiceDeploymentContext.GetAppId("mng")

    $TemplateParameters = @{
        appName="notifications"
        apiDeliveryChannelFunctionAuthAadClientId=$ApiDeliveryChannelAppId
        managementFunctionAuthAadClientId=$ManagementAppId
        operationsControlServiceBaseUrl=$OperationsService.BaseUrl
        operationsControlResourceIdForMsiAuthentication=$OperationsService.AuthAppId
        tenancyServiceResourceIdForMsiAuthentication=$TenancyService.AuthAppId
        tenancyServiceBaseUri=$TenancyService.BaseUrl
        appInsightsInstrumentationKey=$ServiceDeploymentContext.InstanceContext.ApplicationInsightsInstrumentationKey
        marainServiceTenantId=$serviceTenantId
        marainServiceTenantDisplayName=$serviceTenantDisplayName
    }
    $InstanceResourceGroupName = $InstanceDeploymentContext.MakeResourceGroupName("notifications")
    $DeploymentResult = $ServiceDeploymentContext.InstanceContext.DeployArmTemplate(
        $PSScriptRoot,
        "deploy.json",
        $TemplateParameters,
        $InstanceResourceGroupName)

    $ServiceDeploymentContext.SetAppServiceDetails($DeploymentResult.Outputs.apiDeliveryChannelFunctionServicePrincipalId.Value, "del", $null)
    $ServiceDeploymentContext.SetAppServiceDetails($DeploymentResult.Outputs.managementFunctionServicePrincipalId.Value, "mng", $null)


    # ensure the service tenancy exists
    Write-Host "Ensuring User Notifications service tenancy..."
    $serviceManifest = Join-Path $PSScriptRoot "ServiceManifests\UserNotificationsServiceManifest.jsonc" -Resolve
    try {
        $cliOutput = & $ServiceDeploymentContext.InstanceContext.MarainCliPath create-service $serviceManifest
        if ( $LASTEXITCODE -ne 0 -and -not ($cliOutput -imatch 'service tenant.*already exists') ) {
            Write-Error "Error whilst trying to register the User Notifications service tenant: ExitCode=$LASTEXITCODE`n$cliOutput"
        }
    }
    catch {
        throw $_
    }
}