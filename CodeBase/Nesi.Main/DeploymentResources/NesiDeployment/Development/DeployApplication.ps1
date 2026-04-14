Import-Module -Name $PSScriptRoot\..\..\Common\DeploymentModule -Force
$InformationPreference = "Continue"
try {
    Deploy-Application -ResourceFolder  $PSScriptRoot -ConfigurationJson $PSScriptRoot\Settings.json
}
catch {
    Write-Error $_.Exception.Message
}