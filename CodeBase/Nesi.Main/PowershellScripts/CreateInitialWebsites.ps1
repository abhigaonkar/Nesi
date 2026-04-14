Import-Module IISAdministration

#https://octopus.com/blog/iis-powershell#iis-10-and-the-iisadministration-module
#https://stackoverflow.com/questions/9794985/config-error-this-configuration-section-cannot-be-used-at-this-path

function Invoke-WithRetry([ScriptBlock] $command) {
    $attemptCount = 0
    $operationIncomplete = $true
    $maxFailures = 1
    $sleepBetweenFailures = 1

    while ($operationIncomplete -and $attemptCount -lt $maxFailures) {
        $attemptCount = ($attemptCount + 1)

        if ($attemptCount -ge 2) {
            Write-Host "Waiting for $sleepBetweenFailures seconds before retrying..."
            Start-Sleep -s $sleepBetweenFailures
            Write-Host "Retrying..."
        }

        try {
            # Call the script block
            & $command

            $operationIncomplete = $false
        }
        catch [System.Exception] {
            if ($attemptCount -lt ($maxFailures)) {
                Write-Host ("Attempt $attemptCount of $maxFailures failed: " + $_.Exception.Message)
            }
            else {
                throw
            }
        }
    }
}
#
#   Creates an app pool with given name and default settings
#
function Add-AppPool($mananger, $appPoolName) {
    $pool = $manager.ApplicationPools.Add($appPoolName)
    $pool.ManagedPipelineMode = "Integrated"
    $pool.ManagedRuntimeVersion = "v4.0"
    $pool.Enable32BitAppOnWin64 = $false
    $pool.AutoStart = $true
    $pool.StartMode = "OnDemand"
    $pool.ProcessModel.IdentityType = "ApplicationPoolIdentity"
    return $pool
}

#
#   Creates the website if it doesn't exist, binds to a new app pool. All bindings on port 80
#
function Add-Website($siteName, $path, $hostName) {
    $manager = Get-IISServerManager
    $appPoolName = $siteName

    Write-Host "Creating website $siteName on path $path and binding $hostName"
    
    try {
        #
        #   We use the existance of the app pool or website to determine if we need to continue
        #   This is not ideal, but it will do for now
        #
        if ($manager.ApplicationPools[$appPoolName] -ne $null -or 
            $manager.Sites[$siteName] -ne $null) {
            Write-Host "Application pool/website exists, SKIPPING"
            return $false
        }
        Write-Host "Creating app pool $appPoolName"
        $appPool = Add-AppPool $manager $appPoolName
        $appPool

        Write-Host "Creating WEBSITE $siteName"
        $site = $manager.Sites.Add($siteName, "http", "*:80:$hostName", "$Path")

        Write-Host "Binding website to app pool"
        $site.Applications["/"].ApplicationPoolName = $appPoolName

        $manager.CommitChanges()
        Write-Host "SUCCESS: Created website $siteName on path $path and binding $hostName"
        return $true
    }
    catch [Exception]{
        Write-Error -Message $_
    }
   
}

function Show-IisInfo($title) {
    $manager = Get-IISServerManager
    Write-Host
    Write-Host "-----------------------------------------------------"
    Write-Host "$title"
    Write-Host 
    Write-Host "---------------APPLICATION POOLS---------------------"
    $manager.ApplicationPools

    Write-Host "---------------WEBSITES------------------------------"
    $manager.Sites
    Write-Host "-----------------------------------------------------"
    Write-Host
}

Show-IisInfo "Startup IIS info"
$success = $true

Add-Website "Nesi.Hub" "C:\inetpub\wwwroot\nesi\nesi.hub" "nesi.hub.localhost"
Add-Website "Nesi.WebApi" "C:\inetpub\wwwroot\nesi\nesi.webapi" "nesi.api.localhost"
Add-Website "Nesi.Web" "C:\inetpub\wwwroot\nesi\nesi.web" "nesi.web.localhost"

write-host  "Operation Result $(If ($condition) {"true"} Else {"false"})" 
Show-IisInfo "Final IIS info"