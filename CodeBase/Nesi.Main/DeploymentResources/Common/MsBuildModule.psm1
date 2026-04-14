###########################################################################
# Various msbuild related functions
###########################################################################
Import-Module -Name $PSScriptRoot\CommandModule -Force
$InformationPreference = "Continue"
###########################################################################
# Cleans the given solution
###########################################################################
function Clean-Solution {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, HelpMessage = "Name of the solution configuration to clean")]
        [string] $Configuration,

        [Parameter(Mandatory = $true, HelpMessage = "Solution containing the projects you want cleaned")]
        [ValidateNotNullOrEmpty()]
        [ValidateScript( {Test-Path $_ })]
        [string] $Solution,

        [parameter(Mandatory = $false, HelpMessage = "Display output in a separate window")]
        [switch] $ExternalWindow,

        [parameter(Mandatory = $false, HelpMessage = "Manually close the external window")]
        [switch] $ManualClose
    )

    Write-Information "Cleaning ($Solution) configuration: $Configuration"
    $MsBuildPath = Get-MsBuildLocation
    Write-Information "Using MSBUILD located at $MsBuildPath"
    $command = """$MSBuildPath"" /p:Configuration=$Configuration /t:Clean $Solution"
    $executionResult = Execute-Command -Command $command -ExternalWindow $ExternalWindow -ManualClose $ManualClose
    return $executionResult
}

###########################################################################
# Restore nuget packages for solution
###########################################################################
function Restore-Nuget {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, HelpMessage = "Solution containing the projects you want restored")]
        [ValidateNotNullOrEmpty()]
        [ValidateScript( {Test-Path $_ })]
        [string] $Solution,

        [Parameter(Mandatory = $false, HelpMessage = "Folder where the nuget.exe is located, or to be downloaded")]
        [ValidateNotNullOrEmpty()]
        [ValidateScript( {Test-Path $_ })]
        [string] $NugetExeFolderPath = ".",

        [parameter(Mandatory = $false, HelpMessage = "Allow script to download nuget.exe automatically")]
        [switch] $AllowNugetDownload,

        [parameter(Mandatory = $false, HelpMessage = "Display output in a separate window")]
        [switch] $ExternalWindow,

        [parameter(Mandatory = $false, HelpMessage = "Manually close the external window")]
        [switch] $ManualClose
    )
    $targetNugetExeFilePath = Join-Path $NugetExeFolderPath "nuget.exe"
    $restoreCommand = """$targetNugetExeFilePath""" + " restore " + $Solution

    #
    #   Download the nuget exe if so required and allowed
    #
    if(!(Test-Path $targetNugetExeFilePath) -and $AllowNugetDownload) {
        $sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
        Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExeFilePath
    }

    if (!(Test-Path $targetNugetExeFilePath)) {
        throw "Unable to locate nuget.exe"
    }
    else {
        $executionResult = Execute-Command -Command $restoreCommand -ExternalWindow $ExternalWindow -ManualClose $ManualClose
        return $executionResult
    }
}

###########################################################################
# Publish solution using msbuild/msdeploy
###########################################################################
function Publish-Solution {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, HelpMessage = "Name of the profile to build and deploy")]
        [string] $Configuration,

        [Parameter(Mandatory = $true, HelpMessage = "Solution containing the projects you want published")]
        [ValidateNotNullOrEmpty()]
        [ValidateScript( {Test-Path $_ })]
        [string] $Solution,

        [Parameter(Mandatory = $true, HelpMessage = "Credentials used to authenticate with IIS")]
        [ValidateNotNullOrEmpty()]
        [string] $UserName,

        [Parameter(Mandatory = $true, HelpMessage = "Credentials used to authenticate with IIS")]
        [ValidateNotNullOrEmpty()]
        [string] $Password,
    
        [parameter(Mandatory = $false)]
        [ValidateSet('q', 'quiet', 'm', 'minimal', 'n', 'normal', 'd', 'detailed', 'diag', 'diagnostic')]
        [string] $LogVerbosityLevel = 'normal',

        [parameter(Mandatory = $false, HelpMessage = "Display output in a separate window")]
        [switch] $ExternalWindow,

        [parameter(Mandatory = $false, HelpMessage = "Manually close the external window")]
        [switch] $ManualClose
    )

    #Setup the result structure
    $result = @{}
    $result.MsBuildArgs = $null
    $result.LogFile = $null

    try {
        Write-Information "Deploying ($Solution) to environment: $Configuration"
        $MsBuildPath = Get-MsBuildLocation
        Write-Information "Using MSBUILD located at $MsBuildPath"
        $PublishLogFile = Get-LogFile
        $result.LogFile = $PublishLogFile
        Write-Information "Log file located at $PublishLogFile"

        #Assemble the MsBuild full arguments
        $MsBuildParameters = "/p:DeployOnBuild=true /p:AllowUntrustedCertificate=true /p:Configuration=$Configuration /p:PublishProfile=$Configuration /p:UserName=$UserName /p:Password=$Password /verbosity:$LogVerbosityLevel"

        $MsBuildArguments = """$Solution"" $MsBuildParameters /fileLoggerParameters:LogFile=""$PublishLogFile"""
        #Save in result for display purposes
        $result.MsBuildArgs = $MsBuildArguments
        $cmdArgumentsToRunMsBuild = """$MsBuildPath"" " + "$MsBuildArguments "
        $result = Execute-Command -Command $cmdArgumentsToRunMsBuild -ExternalWindow $ExternalWindow -ManualClose $ManualClose -Result $result
    }
    catch {
        $errorMessage = $_
        $result.Message = "Unexpected error occurred while building ""$Solution"": $errorMessage"
        $result.Success = $false
        Write-Error ($result.Message)
        return $result
    }

    # If we can't find the build's log file in order to inspect it, write a warning and return null.
    if (!(Test-Path -LiteralPath $PublishLogFile -PathType Leaf)) {
        $result.Success = $null
        $result.Message = "Cannot find the build log file at '$PublishLogFile', so unable to determine if build succeeded or not."

        Write-Warning ($result.Message)
        return $result
    }

    # Get if the build succeeded or not.
    [bool] $buildOutputDoesNotContainFailureMessage =$null -eq (Select-String -Path $PublishLogFile -Pattern "Build FAILED." -SimpleMatch)
    #If we are in manual close mode, the return code won't be 0
    [bool] $buildReturnedSuccessfulExitCode = ($ManualClose) -or ( $result.ExitCode -eq 0)
    $buildSucceeded = $buildOutputDoesNotContainFailureMessage -and $buildReturnedSuccessfulExitCode
    $result.Success = $buildSucceeded

    # If the build succeeded.
    if ($buildSucceeded) {
        Write-Information "Successfully deployed ""$Solution - $Configuration""."
    }
    # Else at least one of the projects failed to build.
    else {
        $result.Message = "FAILED to deploy ""$Solution - $Configuration"". Please check the log ""$PublishLogFile"" for details."

        # Write the error message as a warning.
        Write-Warning ($result.Message)
    }
    return $result
}

###########################################################################
# Attempts to locate the path to msbuild.exe on the local machine
###########################################################################
Function Get-MsBuildLocation([int] $MaxVersion = 2017) {
    $agentPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\msbuild.exe"
    $devPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe"
    $proPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild.exe"
    $communityPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe"
    $fallback2015Path = "${Env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe"
    $fallback2013Path = "${Env:ProgramFiles(x86)}\MSBuild\12.0\Bin\MSBuild.exe"
    $fallbackPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319"

    If ((2017 -le $MaxVersion) -And (Test-Path $devPath)) { return $devPath } 
    If ((2017 -le $MaxVersion) -And (Test-Path $proPath)) { return $proPath } 

    If ((2017 -le $MaxVersion) -And (Test-Path $communityPath)) { 
        Write-Debug "Using the VS community for MSBUILD" 
        return $communityPath 
    } 

    If ((2017 -le $MaxVersion) -And (Test-Path $agentPath)) { 
        Write-Debug "Using the BUILDTOOLS install location for MSBUILD" 
        return $agentPath 
    } 
    If ((2015 -le $MaxVersion) -And (Test-Path $fallback2015Path)) { return $fallback2015Path } 
    If ((2013 -le $MaxVersion) -And (Test-Path $fallback2013Path)) { return $fallback2013Path } 
    If (Test-Path $fallbackPath) { return $fallbackPath } 
    throw "Unable to find msbuild.exe"
}

###########################################################################
# Creates the log folder if needed
###########################################################################
Function Get-LogFile() {
    $logFileName = "Deploy.$Configuration.$(get-date -f yyyy-MM-dd-hh-ss).log"
    $buildLogFilePath = [io.path]::combine($Env:APPDATA, "Nesi", $logFileName)
    $buildLogFilePath
}

Export-ModuleMember -Function Publish-Solution, Get-MsBuildLocation, Clean-Solution,Restore-Nuget,Get-LogFile





