@echo off
cd ark
echo Writing file list...
del ..\tmp.lst
setlocal EnableDelayedExpansion
for /L %%n in (1 1 500) do if "!__cd__:~%%n,1!" neq "" set /a "len=%%n+1"
setlocal DisableDelayedExpansion
for /r . %%g in (*.*) do (
  set "absPath=%%g"
  setlocal EnableDelayedExpansion
  set "relPath=!absPath:~%len%!"
  echo(!relPath! >> ..\tmp.lst
  endlocal
)
..\..\bin\sed -i s/\\/\//g ..\tmp.lst
pause
del sed*
copy ..\gen\blank.hdr ..\gen\main.hdr /y
copy ..\gen\blank_0.ark ..\gen\main_0.ark /y
echo Rebuilding the ARK file...
:#echo Writing file list... (slow, thanks Windows) ^>:(
:#FOR /F "tokens=*" %%A IN ('..\list') DO (echo Adding %%A... && ..\..\bin\ArkTool -a ..\gen "%%A" "%%A")
FOR /F "tokens=*" %%A IN (..\tmp.lst) DO (echo Adding %%A... && ..\..\bin\ArkTool -a ..\gen "%%A" "%%A")
