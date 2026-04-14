@echo off
REM ============================================================
REM NESI Database Setup Helper Script (Windows)
REM ============================================================
REM This script helps set up the NESI database on Windows
REM Usage: setup-database.bat [option]
REM Options:
REM   auto     - Automatic setup (create DB, run migrations)
REM   manual   - Manual DB creation only  
REM   migrate  - Run migrations only
REM   reset    - Drop and recreate database (WARNING: DATA LOSS!)
REM ============================================================

setlocal enabledelayedexpansion

set SCRIPT_DIR=%~dp0
set BACKEND_DIR=%SCRIPT_DIR%..\backend
set DB_NAME=NesiDb
set DB_USER=root
set DB_PASSWORD=root

echo ================================
echo NESI Database Setup
echo ================================
echo.

REM Get option (default to auto)
set OPTION=%1
if "%OPTION%"=="" set OPTION=auto

REM Check MySQL is available
where mysql >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo ERROR: MySQL is not installed or not in PATH
    exit /b 1
)

echo Checking MySQL connection...
mysql -u%DB_USER% -p%DB_PASSWORD% -e "SELECT 1" >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo ERROR: Cannot connect to MySQL with provided credentials
    echo Please ensure MySQL is running and credentials are correct
    exit /b 1
)
echo [OK] MySQL connection successful

if "%OPTION%"=="auto" goto auto
if "%OPTION%"=="manual" goto manual
if "%OPTION%"=="migrate" goto migrate
if "%OPTION%"=="reset" goto reset

echo Invalid option: %OPTION%
echo.
echo Usage: %0 [option]
echo Options:
echo   auto     - Automatic setup (create DB, run migrations)
echo   manual   - Manual DB creation only
echo   migrate  - Run migrations only
echo   reset    - Drop and recreate database (WARNING: DATA LOSS!)
exit /b 1

:auto
echo Running automatic setup...
call :create_database
if %ERRORLEVEL% neq 0 exit /b 1
call :run_migrations
if %ERRORLEVEL% neq 0 exit /b 1
echo.
echo ================================
echo Setup Complete!
echo ================================
echo.
echo To seed demo data, run the application:
echo   cd %BACKEND_DIR%\src\Nesi.Api
echo   dotnet run
echo.
echo Demo users:
echo   admin      / Admin@123      (Admin)
echo   manager1   / Manager@123    (Manager)
echo   employee1  / Employee@123   (Employee)
goto :eof

:manual
echo Running manual database creation...
call :create_database
if %ERRORLEVEL% neq 0 exit /b 1
echo.
echo Database created. Run migrations manually:
echo   cd %BACKEND_DIR%
echo   dotnet ef database update --project src\Nesi.Infrastructure --startup-project src\Nesi.Api
goto :eof

:migrate
echo Running migrations only...
call :run_migrations
goto :eof

:reset
echo WARNING: This will DELETE ALL DATA in '%DB_NAME%'!
set /p confirm="Are you sure? Type 'yes' to continue: "
if not "!confirm!"=="yes" (
    echo Cancelled.
    exit /b 0
)
echo Dropping database '%DB_NAME%'...
mysql -u%DB_USER% -p%DB_PASSWORD% -e "DROP DATABASE IF EXISTS %DB_NAME%;"
call :create_database
if %ERRORLEVEL% neq 0 exit /b 1
call :run_migrations
if %ERRORLEVEL% neq 0 exit /b 1
echo [OK] Database reset complete
goto :eof

:create_database
echo Creating database '%DB_NAME%'...
mysql -u%DB_USER% -p%DB_PASSWORD% < "%SCRIPT_DIR%create-database.sql"
if %ERRORLEVEL% neq 0 (
    echo ERROR: Failed to create database
    exit /b 1
)
echo [OK] Database created
exit /b 0

:run_migrations
echo Running EF Core migrations...
cd /d "%BACKEND_DIR%"
dotnet ef database update --project src\Nesi.Infrastructure --startup-project src\Nesi.Api
if %ERRORLEVEL% neq 0 (
    echo ERROR: Failed to run migrations
    exit /b 1
)
echo [OK] Migrations completed
exit /b 0
