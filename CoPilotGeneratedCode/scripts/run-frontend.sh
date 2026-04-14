#!/bin/bash

# NESI Frontend Startup Script
# This script starts the Angular development server

echo "======================================"
echo " Starting NESI Frontend"
echo "======================================"

# Navigate to frontend project
cd "$(dirname "$0")/../frontend" || exit

# Check if Node.js is installed
if ! command -v node &> /dev/null; then
    echo "Error: Node.js not found. Please install Node.js 20+."
    exit 1
fi

# Check if npm is installed
if ! command -v npm &> /dev/null; then
    echo "Error: npm not found. Please install npm."
    exit 1
fi

# Display versions
echo "Node.js version:"
node --version
echo "npm version:"
npm --version

# Check if node_modules exists
if [ ! -d "node_modules" ]; then
    echo ""
    echo "Installing dependencies..."
    npm install
fi

echo ""
echo "Starting Angular development server..."
echo "Frontend will be available at: http://localhost:4200"
echo ""
echo "Press Ctrl+C to stop the server"
echo ""

# Start the Angular dev server
npm start
