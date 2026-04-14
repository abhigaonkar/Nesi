#!/bin/bash

# NESI Backend Startup Script
# This script starts the ASP.NET Core backend API

echo "======================================"
echo " Starting NESI Backend API"
echo "======================================"

# Navigate to API project
cd "$(dirname "$0")/../backend/src/Nesi.Api" || exit

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "Error: .NET SDK not found. Please install .NET 8 SDK."
    exit 1
fi

# Check .NET version
dotnet --version

echo ""
echo "Restoring packages..."
dotnet restore

echo ""
echo "Building project..."
dotnet build

echo ""
echo "Starting API server..."
echo "API will be available at: http://localhost:5000"
echo "Swagger UI: http://localhost:5000/swagger"
echo ""
echo "Press Ctrl+C to stop the server"
echo ""

# Run the application
dotnet run
