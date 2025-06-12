<#
This example demonstrates a software build process using the 'ZeroFailed.Build.DotNet' extension
to provide the features needed when building a .NET solutions.
#>

$zerofailedExtensions = @(
    @{
        # References the extension from its GitHub repository. If not already installed, use latest version from 'main' will be downloaded.
        Name = "ZeroFailed.Build.DotNet"
        GitRepository = "https://github.com/zerofailed/ZeroFailed.Build.DotNet"
        GitRef = "main"
    }
)

# Load the tasks and process
. ZeroFailed.tasks -ZfPath $here/.zf


#
# Build process control options
#
$SkipInit = $false
$SkipVersion = $false
$SkipBuild = $false
$CleanBuild = $Clean
$SkipTest = $false
$SkipTestReport = $false
$SkipAnalysis = $false
$SkipPackage = $false

#
# Build process configuration
#
$SolutionToBuild = (Resolve-Path (Join-Path $here "./Solutions/Marain.UserNotifications.sln")).Path
$ProjectsToPublish = @(
    "Solutions/Marain.UserNotifications.Hosting.AspNetCore/Marain.UserNotifications.Hosting.AspNetCore.csproj"
    "Solutions/Marain.UserNotifications.Management.Host/Marain.UserNotifications.Management.Host.csproj"
)
$NuSpecFilesToPackage = @()
$NugetPublishSource = property ZF_NUGET_PUBLISH_SOURCE "$here/_local-nuget-feed"
$IncludeAssembliesInCodeCoverage = "Marain.UserNotifications*"
$ExcludeAssembliesInCodeCoverage = ""


# Customise the build process
task installAzureFunctionsSDK {
    
    $existingVersion = ""
    if ((Get-Command func -ErrorAction Ignore)) {
        $existingVersion = exec { & func --version }
    }

    if (!$existingVersion -or $existingVersion -notlike "4.*") {
        Write-Build White "Installing/updating Azure Functions Core Tools..."
        if ($IsWindows) {
            exec { & npm install -g azure-functions-core-tools@ --unsafe-perm true }
        }
        else {
            Write-Build Yellow "NOTE: May require 'sudo' on Linux/MacOS"
            exec { & sudo npm install -g azure-functions-core-tools@ --unsafe-perm true }
        }
    } 
}

task . FullBuild


#
# Build Process Extensibility Points - uncomment and implement as required
#

# task RunFirst {}
# task PreInit {}
# task PostInit {}
# task PreVersion {}
# task PostVersion {}
# task PreBuild {}
# task PostBuild {}
task PreTest Init,installAzureFunctionsSDK
# task PostTest {}
# task PreTestReport {}
# task PostTestReport {}
# task PreAnalysis {}
# task PostAnalysis {}
# task PrePackage {}
# task PostPackage {}
# task PrePublish {}
# task PostPublish {}
# task RunLast {}
