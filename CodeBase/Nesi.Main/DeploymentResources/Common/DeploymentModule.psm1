###########################################################################
# Module that builds and deploys applications from a configuration file
###########################################################################
Import-Module -Name $PSScriptRoot\MsBuildModule -Force
Import-Module -Name $PSScriptRoot\MsDeployModule -Force
Import-Module -Name $PSScriptRoot\CommandModule -Force

$InformationPreference = "Continue"
$ResourceFolder
function Deploy-Application {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, HelpMessage = "Folder where relative paths will be resolved from in modules")]
        [ValidateNotNullOrEmpty()]
        [ValidateScript( {Test-Path $_ })]
        [string] $ResourceFolder,

        [Parameter(Mandatory = $true, HelpMessage = "Json file containing all the deployment options")]
        [ValidateNotNullOrEmpty()]
        [ValidateScript( {Test-Path $_ })]
        [string] $ConfigurationJson
    )
    $Script:ResourceFolder = $ResourceFolder
    $jsonFileContent = Get-Content -Raw -Path $ConfigurationJson | ConvertFrom-Json
    Write-Information "Deploying application '$($jsonFileContent.Deployment.Name)'"

    # Display a confirmation to the user if so required
    if ($null -eq $jsonFileContent.Deployment.Confirm -or $true -eq $jsonFileContent.Deployment.Confirm ) {
        $confirmation = Read-Host "Proceed with deployment of '$($jsonFileContent.Deployment.Name)'? (Y/N)"
        if ($confirmation -ne 'y') {
            Write-Information "Deployment cancelled by user"
            return
        }
    }

    foreach ($project in $jsonFileContent.Deployment.Projects) {
        Deploy-Project $project
    }
}

function Deploy-Project($project) {
    if ($null -eq $project) {
        throw "Undefined project"
    }

    if ($project.Type -eq "dotnet") {
        Deploy-Dotnet $project
    }
    elseif ($project.Type -eq "angular" ) {
        Deploy-Angular $project
    }
    else {
        throw "Undefined or unknown project type"
    }
}

function Deploy-Dotnet($project) {

    if ($null -eq $project.Name -or `
            $null -eq $project.Solution -or `
            $null -eq $project.Configuration -or `
            $null -eq $project.Username -or `
            $null -eq $project.Password) {
        throw "Invalid .NET project configuration"
    }

    if (!(Test-Path $project.Solution) ) {
        throw "Solution file  $($project.Solution) does not exist"
    }
    $useExternalFlag = $true -eq $project.UseExternalWindow
    $manualCloseFlag = $false -eq $project.AutoCloseExternalWindow

    Write-Information "Deploying .NET project '$($project.Name)'"

    if ($true -eq $project.Clean) {
        Write-Information "Cleaning project $($project.Solution)..."
        $cleanResult = Clean-Solution `
            -ExternalWindow:$useExternalFlag `
            -ManualClose:$manualCloseFlag `
            -Configuration $project.Configuration `
            -Solution $project.Solution

        Write-Information ($cleanResult | Format-List | Out-String)
        if (!$cleanResult.Success) {
            throw "Failed to clean solution"
        }
    }

    if ($true -eq $project.RestorePackages) {
        Write-Information "Restoring packages for project..."
        $restoreResult = Restore-Nuget `
            -ExternalWindow:$useExternalFlag `
            -ManualClose:$manualCloseFlag `
            -Solution $project.Solution `
            -NugetExeFolderPath $env:temp `
            -AllowNugetDownload
        Write-Information ($restoreResult | Format-List | Out-String)
        if (!$restoreResult.Success) {
            throw $restoreResult.Message
        }
    }
    if ($true -eq $project.Deploy) {
        Write-Information "Building and deploying project..."
        $publishResult = Publish-Solution -ExternalWindow:$useExternalFlag `
            -ManualClose:$manualCloseFlag `
            -Configuration $project.Configuration `
            -Solution $project.Solution `
            -User $project.Username `
            -Password $project.Password
        Write-Information ($publishResult|  Format-List | Out-String)

        if (!$publishResult.Success) {
            throw $publishResult.Message
        }
    }
    else {
        Write-Information "Deploying .NET project '$($project.Name)' skipped due to configuration"
    }
}

function Deploy-Angular($project) {

    $useExternalFlag = $true -eq $project.UseExternalWindow
    $manualCloseFlag = $false -eq $project.AutoCloseExternalWindow
    Write-Information "Deploying Angular project '$($project.Name)'"

    if (!($true -eq $project.Deploy)) {
        Write-Information "Deploying Angular project '$($project.Name)' skipped due to configuration"
    }

    if ($null -eq $project.Name -or `
            $null -eq $project.ServerAddress -or `
            $null -eq $project.SiteName -or `
            $null -eq $project.ProjectFolder -or `
            $null -eq $project.Username -or `
            $null -eq $project.Password) {
        throw "Invalid Angular project configuration"
    }

    if (!(Test-Path $project.ProjectFolder) ) {
        throw "Solution file  $($project.ProjectFolder) does not exist"
    }

    Push-Location $project.ProjectFolder
    try {

        if ($true -eq $project.Build) {
            Write-Information "Building Angular project '$($project.Name)'"
            $buildResult = Execute-Command `
                -Command $project.BuildCommand `
                -ExternalWindow:$useExternalFlag `
                -ManualClose:$manualCloseFlag

            Write-Information ($buildResult |Format-List | Out-String)
            if (!$buildResult.Success) {
                throw $buildResult.Message
            }
        }
        else {
            Write-Information "Angular build skipped due to configuration"
        }

        if ($true -eq $project.Deploy) {
            if (!(Test-Path $project.BuildOutputFolder) ) {
                throw "Build output folder $($project.BuildOutputFolder) does not exist"
            }
            $buildOutputPath = Resolve-Path $project.BuildOutputFolder
            $preScriptPath = Process-Path $project.PreDeployScript
            $postScriptPath = Process-Path $project.PostDeployScript

            Write-Information "Deploying Angular build output '$($project.Name)' to remote site '$($project.SiteName)' from output path $buildOutputPath"
            $deployResult = Deploy-Folder -ServerAddress $project.ServerAddress `
                -ServerPort $project.Port `
                -IISSiteName $project.SiteName `
                -SourceFolder $buildOutputPath `
                -UserName $project.Username `
                -Password $project.Password `
                -PreDeployScript $preScriptPath `
                -PostDeployScript $postScriptPath `
                -ExternalWindow:$useExternalFlag `
                -ManualClose:$manualCloseFlag

            Write-Information ($deployResult | Format-List | Out-String)

            if (!$deployResult.Success) {
                throw $deployResult.Message
            }
        }
        else {
            Write-Information "Angular deployment skipped due to configuration"
        }
    }
    finally {
        Pop-Location
    }
    Write-Information ($deployResult| Format-List | Out-String)
}

#
#   Process a path provided at script level
#   If the path is absolute, ensure the path exists and return it as is
#   If the path is relative, return it resolved from the resource root that was passed in
#
function Process-Path($filePath) {
    if ([string]::IsNullOrEmpty($filePath)) {
        return $null
    }

    if([System.IO.Path]::IsPathRooted($filePath)){
        return Resolve-Path $filePath
    }
    return Resolve-Path (Join-Path $Script:ResourceFolder $filePath)
}

Export-ModuleMember -Function Deploy-Application
