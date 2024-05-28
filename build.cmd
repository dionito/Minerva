@echo off

set target=%~1
if "%target%" equ "" set "target=restore;compile;test"

if "%target:restore=%" neq "%target%" (
    echo Restoring nuget packages
    dotnet restore
    if %errorlevel% neq 0 exit /B 1
    echo.
)

if "%target:compile=%" neq "%target%" (
    echo Building Minerva
    dotnet build --no-restore
    if %errorlevel% neq 0 exit /B 1
    echo.
)

if "%target:test=%" neq "%target%" (
    echo Running Tests
    dotnet test --no-build --verbosity normal
    if %errorlevel% neq 0 exit /B 1
)

exit 0
