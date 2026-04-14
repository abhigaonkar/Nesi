#!/bin/bash

# NESI Complete Application Startup Script
# This script starts both backend and frontend in separate terminal windows

echo "======================================"
echo " Starting NESI Complete Application"
echo "======================================"

# Get the directory where this script is located
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

echo ""
echo "This will start:"
echo "  1. Backend API (http://localhost:5000)"
echo "  2. Frontend App (http://localhost:4200)"
echo ""
echo "Two new terminal windows will open."
echo ""

# Detect OS and open terminals accordingly
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    # Linux
    if command -v gnome-terminal &> /dev/null; then
        gnome-terminal -- bash -c "cd '$SCRIPT_DIR' && ./run-backend.sh; exec bash"
        sleep 2
        gnome-terminal -- bash -c "cd '$SCRIPT_DIR' && ./run-frontend.sh; exec bash"
    elif command -v xterm &> /dev/null; then
        xterm -e "cd '$SCRIPT_DIR' && ./run-backend.sh" &
        sleep 2
        xterm -e "cd '$SCRIPT_DIR' && ./run-frontend.sh" &
    else
        echo "Error: No supported terminal found. Please run scripts manually:"
        echo "  Terminal 1: ./run-backend.sh"
        echo "  Terminal 2: ./run-frontend.sh"
        exit 1
    fi
elif [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS
    osascript -e "tell application \"Terminal\" to do script \"cd '$SCRIPT_DIR' && ./run-backend.sh\""
    sleep 2
    osascript -e "tell application \"Terminal\" to do script \"cd '$SCRIPT_DIR' && ./run-frontend.sh\""
else
    echo "Unsupported OS: $OSTYPE"
    echo "Please run the following scripts manually in separate terminals:"
    echo "  Terminal 1: ./run-backend.sh"
    echo "  Terminal 2: ./run-frontend.sh"
    exit 1
fi

echo ""
echo "✓ Backend and Frontend starting in separate terminals"
echo ""
echo "Once both servers are running:"
echo "  - Backend API: http://localhost:5000"
echo "  - Swagger UI: http://localhost:5000/swagger"
echo "  - Frontend: http://localhost:4200"
echo ""
echo "Login with demo account:"
echo "  Username: sarah.tech"
echo "  Password: Demo123!"
echo ""
