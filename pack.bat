@echo off
setlocal

pushd "%~dp0"

set "CONFIGURATION=Release"
set "ARTIFACTS_DIR=%CD%\artifacts"
set "PACKAGES_DIR=%ARTIFACTS_DIR%\packages"

if exist "%PACKAGES_DIR%" rmdir /s /q "%PACKAGES_DIR%"
mkdir "%PACKAGES_DIR%"

echo [1/3] Restoring solution...
dotnet restore CodeWF.AvaloniaControls.slnx
if errorlevel 1 goto :error

echo [2/3] Building solution...
dotnet build CodeWF.AvaloniaControls.slnx -c %CONFIGURATION% --no-restore
if errorlevel 1 goto :error

echo [3/3] Packing libraries...
for %%P in (
    "src\CodeWF.AvaloniaControls\CodeWF.AvaloniaControls.csproj"
    "src\CodeWF.AvaloniaControls.Themes\CodeWF.AvaloniaControls.Themes.csproj"
    "src\CodeWF.AvaloniaControls.DataGrid\CodeWF.AvaloniaControls.DataGrid.csproj"
    "src\CodeWF.AvaloniaControls.Dock\CodeWF.AvaloniaControls.Dock.csproj"
    "src\CodeWF.AvaloniaControls.ProDataGrid\CodeWF.AvaloniaControls.ProDataGrid.csproj"
) do (
    dotnet pack %%~P -c %CONFIGURATION% --no-build -o "%PACKAGES_DIR%"
    if errorlevel 1 goto :error
)

echo.
echo Packages are available in:
echo %PACKAGES_DIR%

popd
exit /b 0

:error
set "EXIT_CODE=%ERRORLEVEL%"
echo.
echo Pack failed with exit code %EXIT_CODE%.
popd
exit /b %EXIT_CODE%
