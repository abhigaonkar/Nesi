###########################################################################
# Executes the given command text using cmd.exe
# Allows for execution of command in either the current window or an external one
###########################################################################

$InformationPreference = "Continue"
function Execute-Command() {
[CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, HelpMessage = "Command text to execute")]
        [ValidateNotNullOrEmpty()]
        [string] $Command,

        [Parameter(Mandatory = $false, HelpMessage = "Launch in external command window")]
        [bool] $ExternalWindow = $true,

        [Parameter(Mandatory = $false, HelpMessage = "Require user to close external window")]
        [bool] $ManualClose = $false,

        [Parameter(Mandatory = $false, HelpMessage = "Pass in information to be appended to result")]
        [HashTable] $Result = $null
    )
    $executionResult = $Result

    if($null -eq $executionResult){
        $executionResult = @{}
    }

    $executionResult.ExitCode = $null
    $executionResult.Success = $true
    $executionResult.Message = $null
    $executionResult.Command = $Command
    $executionResult.Duration = [TimeSpan]::Zero

    Write-Information "Executing command $Command"
    #Execute in an external window
    if($ExternalWindow){
        $autoCloseSwitch = "/c"
        if ($ManualClose) {
            $autoCloseSwitch = "/k"
        }
        $executionProcess = $null
        $performBuildScriptBlock =
        {
            $executionProcess = Start-Process cmd.exe -ArgumentList $autoCloseSwitch,"`"$Command`"" -Wait -PassThru -WindowStyle "normal"
            if(!$ManualClose){
                $executionResult.ExitCode =$executionProcess.ExitCode
                $executionResult.Success = $executionProcess.ExitCode -eq 0
            }
        }

        # Perform the build and record how long it takes.
        $executionResult.Duration = (Measure-Command -Expression $performBuildScriptBlock)
    }
    else {
        cmd /c "$Command" | Write-Host

        if (! $?) {
            $executionResult.Success = $false
            $executionResult.Message = "Failed to execute command"
        }
        else{
            $executionResult.ExitCode = 0
        }
    }
    return $executionResult
}
Export-ModuleMember -Function Execute-Command