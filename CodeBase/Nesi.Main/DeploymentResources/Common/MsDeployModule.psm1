###########################################################################
# Author: Razvan Ghimici
# Date: June 14, 2018
# This uses a raw msdeploy.exe functionality to deploy a non-project folder to a specified website
#
# Configuration:  Which configuration to deploy can be one of Development, QA or Production
# UserName:       User name that will be used for IIS authentication
# Password:       Password used for authentication
#
# Example:
# > .\DeployAll.ps1 -Configuration Development -UserName USER -Password PASSWORD
###########################################################################
Import-Module -Name $PSScriptRoot\CommandModule -Force
$InformationPreference = "Continue"
function Deploy-Folder() {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, HelpMessage = "Destination server address (IIS host)")]
        [ValidateNotNullOrEmpty()]
        [string] $ServerAddress,

        [Parameter(Mandatory = $false, HelpMessage = "Destination server port (IIS host)")]
        [Int] $ServerPort = 8172,

        [Parameter(Mandatory = $true, HelpMessage = "Name of the site node in IIS")]
        [ValidateNotNullOrEmpty()]
        [string] $IISSiteName,

        [Parameter(Mandatory = $true, HelpMessage = "Folder containing the files to be published")]
        [ValidateNotNullOrEmpty()]
        [ValidateScript( {Test-Path $_ })]
        [string] $SourceFolder,

        [Parameter(Mandatory = $true, HelpMessage = "Credentials used to authenticate with IIS")]
        [ValidateNotNullOrEmpty()]
        [string] $UserName,

        [Parameter(Mandatory = $true, HelpMessage = "Credentials used to authenticate with IIS")]
        [ValidateNotNullOrEmpty()]
        [string] $Password,
    
        [Parameter(Mandatory = $false, HelpMessage = "Script to run before deploying files")]
        [string] $PreDeployScript,

        [Parameter(Mandatory = $false, HelpMessage = "Script to run after deploying files")]
        [string] $PostDeployScript,

        [parameter(Mandatory = $false, HelpMessage = "Display output in a separate window")]
        [switch] $ExternalWindow,

        [parameter(Mandatory = $false, HelpMessage = "Manually close the external window")]
        [switch] $ManualClose
    )
    $fullDeployUrl = "https://$ServerAddress`:$ServerPort/msdeploy.axd?site=$IISSiteName"
    $msDeployPath = Get-MsDeploy-Path
    Write-Information "Deploying to $fullDeployUrl"
    Write-Information "Using MsDeploy.exe at $msDeployPath"

    $msDeployParameters = "-verb:sync " `
        +"-source:contentPath=$SourceFolder " `
        +"-allowUntrusted " `
        +"-dest:contentPath=$IISSiteName,ComputerName=$fullDeployUrl,UserName=$UserName,Password=$Password,AuthType=Basic"
    
    if(![string]::IsNullOrEmpty($PreDeployScript)){
        if (!(Test-Path $PreDeployScript)) { throw "Pre-Deploy script $PreDeployScript does not exist" }
        $msDeployParameters += " -preSync:runCommand=""$PreDeployScript"",waitAttempts=10,waitInterval=3000 "
    }
    if(![string]::IsNullOrEmpty($PostDeployScript)){
        if (!(Test-Path $PostDeployScript)) { throw "Post-Deploy script $PostDeployScript does not exist" }
        $msDeployParameters+=" -postSync:runCommand=""$PostDeployScript"",waitAttempts=10,waitInterval=3000 "
    }
    $command = """$msDeployPath"" " + "$msDeployParameters "
    $result = Execute-Command -Command $command -ExternalWindow $ExternalWindow -ManualClose $ManualClose
    return $result
}

###########################################################################
# Discovers the local installation of msdeploy.exe
###########################################################################
function Get-MsDeploy-Path() {
    $installPath = (get-childitem "HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy" | Select -last 1).GetValue("InstallPath")
    if (!(Test-Path $installPath)) { throw "MsDeploy install path $installPath does not exist" }
    return $installPath + "msdeploy.exe"
}

Export-ModuleMember -Function Deploy-Folder