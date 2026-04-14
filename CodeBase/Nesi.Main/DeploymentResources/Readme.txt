Included files and structure
-----------------------------

Under .\Common you will find reusable powershell modules. These should not need modification unless there are issues
- CommandModule.psm1 allows for commands to be executed either in the current window or in an external window. Output is retrieved and returned to the caller.
- MsDeployModule.psm1 allows the secure deployment of a folder to a remote location. Also allows for execution of local scripts remotely.
- SvnModule.psm1 allows caller to checkout/switch branches
- MsBuildModule.psm1 allows caller to clean, restore package, build and deploy a solution using webdeploy.
- DeploymentModule.psm1 ties everything together, and it allows for json file driven deployment (declarative).

Under the .\NesiDeployment folder you'll find per-environment Nesi release scripts. 
Each folder includes :
- A launcher script (DeployApplication.ps1)
- A Settings.json file containing all the paths, credentials and switches used for that environment
- A batch file used for remote execution (Angular copy).

Most options should be configured through the Settings.json file, and there are a couple of folder paths that need to be configured in the batch file.
One-time setup
----------------------------------
For each account used to deploy the application, you will need to intall all npm packages that are required to build the Angular component of the application.
- Run the firstInstall.ps1 script that can be found in Nesi\Angular and monitor the output; there will be some warnings, this is ok.
- Test that the angular-cli has been installed correctly; in a NEW command window run "ng --version" command. If the command is not found, add the %APPDATA%/npm directory to the uaser PATH environment variable.

Deploying
----------------------------------
- On the target machine, switch the source code to the desired branches (this can be done with svn switch or git checkout)
- Open a powershell ADMIN window and navigate to the .\NesiDeployment\YOUR_ENVIRONMENT
- Run the .\DeployApplication.ps1

Remarks
----------------------------------
- All powershell Output is written to the Information stream.
- The build and deploy Output is also logged to file, and the file path is logged to the console.
- If the compile or website deployment fails, you will clearly see the errors indicated in the output window, and the process will stop.

Angular
-----------------------------------
- The Angular code is built on the build machine. It is then transferred (synced) using WebDeploy to the target server to a temporary site.
- The output is then transferred remotely by invoking the copy angular script.
- To enable this, create the IIS site (it doesn't need a valid binding, or even to run). Configure deployment security as usual.
- To allow this user to remote execute the batch file, see:
    https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/ee619740(v=ws.10)#to-add-the-replace-a-process-level-token-privilege
- For more details on how this works and potential problems, see:
	https://www.jamescrowley.net/2011/09/05/deploying-windows-services-using-msdeploy/
- Please note that the output from the remote batch execution is ECHOED to the output window, but any errors occuring on the remote machine will not break the build.
	PLEASE LOOK AT THE LOG OUTPUT TO ENSURE NO ERRORS OCCURRED DURING THIS LAST STEP.

TODO
-------------------------------------
Decouple the deployment machine code path from the settings file; maybe create and use an ENV VAR.
Review and update the Production scripts
Use secure strings for passwords
Make sure that everything is logged, not just build
Improve the command module by better dealing with the process launched. Why it sometimes take a long time to return.
    -- Add pause at the end of it all even on auto close
    -- Better see what all the params are for pass through, etc
