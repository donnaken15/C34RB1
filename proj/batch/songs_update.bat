@echo off
echo Resetting ARK songs folder...
for /L %%n in (0 1 3999) do (
	..\bin\arktool -d ..\game\files\gen songs/*/* > nul
	if errorlevel 3 goto :nomoresongs
)
:nomoresongs
rmdir /q /s ZZ___build
mkdir ZZ___build 2> nul
cd ZZ___build
mkdir gen 2> nul
cd gen
..\..\..\bin\dtc ..\..\songs.dta
cd ..\..
dir /b *. > tmp2.lst
:#..\bin\sed -i s/\s//g tmp2.lst
..\bin\sed -i s/ZZ___.*//g tmp2.lst
echo Copying files to build phase...
FOR /F "tokens=*" %%A IN (tmp2.lst) DO (xcopy %%A ZZ___build\%%A /I /E&&copy ZZ___copy\song.pan ZZ___build\%%A\%%A.pan&&mkdir ZZ___build\%%A\gen&&copy ZZ___copy\gen\song_weights.bin ZZ___build\%%A\gen\%%A_weights.bin)
cd ZZ___build
echo Writing file list...
del ..\tmp.lst 2> nul
setlocal EnableDelayedExpansion
for /L %%n in (1 1 500) do if "!__cd__:~%%n,2!" neq "" set /a "len=%%n+1"
setlocal DisableDelayedExpansion
for /r . %%g in (*) do (
	set "absPath=%%g"
	setlocal EnableDelayedExpansion
	set "relPath=!absPath:~%len%!"
	echo(!relPath!>>..\tmp.lst
	endlocal
)
cd ..
..\bin\sed -i s/\\/\//g tmp.lst
..\bin\sed -i s/.*(songs\.dta^|update\.bat^|ZZ___.*^|tmp\d?\.lst).*\n//g tmp.lst
del sed*
echo Updating the ARK file...
FOR /F "tokens=*" %%A IN (tmp.lst) DO (echo %%A&&..\bin\ArkTool -a ..\game\files\gen "songs/%%A" "ZZ___build/%%A")
del tmp*.lst
