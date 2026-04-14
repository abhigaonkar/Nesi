@echo off
REM NESI Frontend Startup Script
REM This script starts the Angular development server

echo ======================================
echo  Starting NESI Frontend
echo ======================================

REM Navigate to frontend project
cd /d "%~dp0\..\frontend"

REM Check if Node.js is installed
where node >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo Error: Node.js not found. Please install Node.js 20+.
    pause
    exit /b 1
)

REM Check if npm is installed
where npm >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo Error: npm not found. Please install npm.
    pause
    exit /b 1
)

REM Display versions
echo Node.js version:
node --version
echo npm version:
npm --version

REM Check if node_modules exists
if not exist "node_modules" (
    echo.
    echo Installing dependencies...
    npm install
)

echo.
echo Starting Angular development server...
echo Frontend will be available at: http://localhost:4200
echo.
echo Press Ctrl+C to stop the server
echo.

REM Start the Angular dev server
npm start

pause
