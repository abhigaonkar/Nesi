del ..\nesi.web\*.js /Q  > nul
del ..\nesi.web\roboto-*.* /Q  > nul
del ..\nesi.web\*.png /Q  > nul
del ..\nesi.web\index.html  /Q  > nul
del ..\nesi.web\line.*.gif  /Q  > nul
del ..\nesi.web\styles.*.css  /Q  > nul
del ..\nesi.web\assets\*.* /S/Q  > nul
xcopy  dist\*.* /s/e ..\nesi.web /Y/Q/R/D
