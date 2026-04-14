SET temp_angular_folder=".\dist"
SET nesi_folder="C:\inetpub\wwwroot\nesi\nesi.web"
ECHO OFF
ECHO Transferring NESI angular files from "%temp_angular_folder%" to %nesi_folder%

ECHO Cleaning the nesi folder
del %nesi_folder%\*.js /Q  > nul
del %nesi_folder%\roboto-*.* /Q  > nul
del %nesi_folder%\*.png /Q  > nul
del %nesi_folder%\index.html  /Q  > nul
del %nesi_folder%\line.*.gif  /Q  > nul
del %nesi_folder%\styles.*.css  /Q  > nul
del %nesi_folder%\assets\*.* /S/Q  > nul

ECHO Copying new files...
xcopy  %temp_angular_folder%\*.* /s/e  %nesi_folder% /Y/Q/R/D
ECHO DONE
FOR /F "tokens=2" %%i in ('date /t') do set mydate=%%i
SET mytime=%time%
ECHO Last updated at %mydate% %mytime% (Local Time)
ECHO ON
