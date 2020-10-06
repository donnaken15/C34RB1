@echo off
:#HATE FREAKING BATCH
IF [%1]==[] goto :noparam
IF NOT %1=="" goto :param

:noparam
start "" bin\hijack setup
exit /b

:noexist
echo FILE DOES NOT EXIST! Exiting...
exit /b

:param
IF NOT EXIST game goto :noexist
	choice /m "The game folder already exists. Overwrite?"
	if %ERRORLEVEL%==1 rmdir /q /s game
	if %ERRORLEVEL%==2 goto :eof
:noexist
echo Extracting disc...
bin\wit X %1 tmp --name "Rock Band" --id C34RB1 --disc-id C34RB1 --boot-id C34RB1
echo Shifting folders...
move tmp\DATA .
rename DATA game
echo Cleaning up...
rmdir /q /s tmp
del game\align-files.txt
del game\setup.*
rmdir /q /s game\disc
del game\cert.bin
del game\h3.bin
del game\ticket.bin
pause

:#"C:\C34RB1\final\setup.bat" "C:\Program Files (x86)\Wiimm\WIT\R37E69.wbfs"
