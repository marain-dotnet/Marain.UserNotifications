<#
This is called during Marain.Instance infrastructure deployment after the Marain-ArmDeploy.ps
script. It is our opportunity to do any deployment work that needs to happen after Azure resources
have been deployed.
#>

# Marain.Instance expects us to define just this one function.
Function MarainDeployment([MarainServiceDeploymentContext] $ServiceDeploymentContext) {

    Write-Host "Assigning application service principals to the tenancy service app's reader role"

    $TenancyReaderAppRoleId = "60743a6a-63b6-42e5-a464-a08698a0e9ed"
    $NotificationsControllerAppRoleId = "4e4c5d1a-2ab6-42a5-a806-57bfc4bdfb60"
    $ControllerAppRoleId = "77d9c620-a258-4f0b-945c-a7128e82f3ec"
    $ServiceDeploymentContext.AssignServicePrincipalToCommonServiceAppRole(
        "Marain.Tenancy",
        $TenancyReaderAppRoleId,
        "del"
    )
    $ServiceDeploymentContext.AssignServicePrincipalToInternalServiceAppRole(
        "mng",
        $NotificationsControllerAppRoleId,
        "del"
    )
    $ServiceDeploymentContext.AssignServicePrincipalToCommonServiceAppRole(
        "Marain.Tenancy.Operations.Control",
        $ControllerAppRoleId,
        "mng"
    )

    $ServiceDeploymentContext.AssignServicePrincipalToCommonServiceAppRole(
        "Marain.Tenancy",
        $TenancyReaderAppRoleId,
        "mng"
    )

}