@echo off
REM NESI Backend Startup Script
REM This script starts the ASP.NET Core backend API

echo ======================================
echo  Starting NESI Backend API
echo ======================================

REM Navigate to API project
cd /d "%~dp0\..\backend\src\Nesi.Api"

REM Check if .NET is installed
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo Error: .NET SDK not found. Please install .NET 8 SDK.
    pause
    exit /b 1
)

REM Check .NET version
dotnet --version

echo.
echo Restoring packages...
dotnet restore

echo.
echo Building project...
dotnet build

echo.
echo Starting API server...
echo API will be available at: http://localhost:5000
echo Swagger UI: http://localhost:5000/swagger
echo.
echo Press Ctrl+C to stop the server
echo.

REM Run the application
dotnet run

pause
