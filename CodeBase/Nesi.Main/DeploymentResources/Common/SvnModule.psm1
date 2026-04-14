###########################################################################
# Gets the specified branch from source control (checkout or switch)
###########################################################################
Import-Module -Name $PSScriptRoot\CommandModule -Force

function Get-Source(){
[CmdletBinding()]
param (
    [Parameter(Mandatory = $true, HelpMessage = "Url for the SVN repo")]
    [ValidateNotNullOrEmpty()]
    [string] $SvnUrl,
    [Parameter(Mandatory = $true, HelpMessage = "Branch path")]
    [ValidateNotNullOrEmpty()]
    [string] $SvnBranch,

    [Parameter(Mandatory = $true, HelpMessage = "Checkout folder or Working copy folder")]
    [ValidateNotNullOrEmpty()]
    [ValidateScript({Test-Path $_})]
    [string] $Folder,
    [parameter(Mandatory = $false, HelpMessage = "Display output in a separate window")]
    [switch] $ExternalWindow,

    [parameter(Mandatory = $false, HelpMessage = "Manually close the external window")]
    [switch] $ManualClose,

    [parameter(Mandatory=$false, HelpMessage = "Do full checkout rather than switch")]
	[switch] $DoFullCheckout = $false
)
    $fullSvnPath ="$SvnUrl$SvnBranch"
    Write-Information "Working with SVN location: $fullSvnPath"
    svn info $fullSvnPath

    $command = ""

    if($DoFullCheckout){
        Write-Information "Checking out at : $Folder to $fullSvnPath"
        $command = "svn checkout $fullSvnPath $Folder"
    }
    else {
        Write-Information "Switching working copy at : $Folder to $fullSvnPath"
        $command = "svn switch $fullSvnPath $Folder"
    }

    $result = Execute-Command -Command $command -ExternalWindow $ExternalWindow -ManualClose $ManualClose
    return $result
}

Export-ModuleMember -Function Get-Source