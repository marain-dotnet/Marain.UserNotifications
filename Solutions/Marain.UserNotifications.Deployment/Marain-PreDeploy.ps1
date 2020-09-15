<#
This is called during Marain.Instance infrastructure deployment prior to the Marain-ArmDeploy.ps
script. It is our opportunity to perform initialization that needs to complete before any Azure
resources are created.

We create the Azure AD Applications that the Workflow functions will use to authenticate incoming
requests. (Currently, this application is used with Azure Easy Auth, but the service could also
use it directly.)

#>

# Marain.Instance expects us to define just this one function.
Function MarainDeployment([MarainServiceDeploymentContext] $ServiceDeploymentContext) {

    $ApiDeliveryChannelApp = $ServiceDeploymentContext.DefineAzureAdAppForAppService("del")
    $ManagementApp = $ServiceDeploymentContext.DefineAzureAdAppForAppService("mng")

    $NotificationsControllerAppRoleId = "4e4c5d1a-2ab6-42a5-a806-57bfc4bdfb60"
    $ManagementApp.EnsureAppRolesContain(
        $NotificationsControllerAppRoleId,
        "Notifications controller",
        "Able to create and modify notifications",
        "NotificationsController",
        ("User", "Application"))
}