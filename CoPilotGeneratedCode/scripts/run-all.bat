@echo off
REM NESI Complete Application Startup Script
REM This script starts both backend and frontend in separate windows

echo ======================================
echo  Starting NESI Complete Application
echo ======================================

echo.
echo This will start:
echo   1. Backend API (http://localhost:5000)
echo   2. Frontend App (http://localhost:4200)
echo.
echo Two new command windows will open.
echo.

REM Get the directory where this script is located
set SCRIPT_DIR=%~dp0

REM Start backend in new window
echo Starting Backend...
start "NESI Backend API" cmd /k "cd /d %SCRIPT_DIR% && run-backend.bat"

REM Wait a bit for backend to start
timeout /t 3 /nobreak >nul

REM Start frontend in new window
echo Starting Frontend...
start "NESI Frontend" cmd /k "cd /d %SCRIPT_DIR% && run-frontend.bat"

echo.
echo ✓ Backend and Frontend starting in separate windows
echo.
echo Once both servers are running:
echo   - Backend API: http://localhost:5000
echo   - Swagger UI: http://localhost:5000/swagger
echo   - Frontend: http://localhost:4200
echo.
echo Login with demo account:
echo   Username: sarah.tech
echo   Password: Demo123!
echo.
echo Press any key to close this window...
pause >nul
