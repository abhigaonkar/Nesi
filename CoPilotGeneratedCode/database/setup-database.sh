#!/bin/bash

# ============================================================
# NESI Database Setup Helper Script
# ============================================================
# This script helps set up the NESI database
# Usage: ./setup-database.sh [option]
# Options:
#   auto     - Automatic setup (create DB, run migrations, seed)
#   manual   - Manual DB creation only
#   migrate  - Run migrations only
#   reset    - Drop and recreate database (WARNING: DATA LOSS!)
# ============================================================

set -e  # Exit on error

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
BACKEND_DIR="$SCRIPT_DIR/../backend"
DB_NAME="NesiDb"
DB_USER="root"
DB_PASSWORD="root"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}================================${NC}"
echo -e "${GREEN}NESI Database Setup${NC}"
echo -e "${GREEN}================================${NC}"
echo ""

# Function to check if MySQL is running
check_mysql() {
    if ! command -v mysql &> /dev/null; then
        echo -e "${RED}ERROR: MySQL is not installed or not in PATH${NC}"
        exit 1
    fi
    
    if ! mysql -u"$DB_USER" -p"$DB_PASSWORD" -e "SELECT 1" &> /dev/null; then
        echo -e "${RED}ERROR: Cannot connect to MySQL with provided credentials${NC}"
        echo -e "${YELLOW}Please ensure MySQL is running and credentials are correct${NC}"
        exit 1
    fi
    
    echo -e "${GREEN}✓ MySQL connection successful${NC}"
}

# Function to create database
create_database() {
    echo -e "${YELLOW}Creating database '$DB_NAME'...${NC}"
    mysql -u"$DB_USER" -p"$DB_PASSWORD" < "$SCRIPT_DIR/create-database.sql"
    echo -e "${GREEN}✓ Database created${NC}"
}

# Function to run migrations
run_migrations() {
    echo -e "${YELLOW}Running EF Core migrations...${NC}"
    cd "$BACKEND_DIR"
    dotnet ef database update --project src/Nesi.Infrastructure --startup-project src/Nesi.Api
    echo -e "${GREEN}✓ Migrations completed${NC}"
}

# Function to run application (which will seed data)
seed_data() {
    echo -e "${YELLOW}Starting application to seed data...${NC}"
    echo -e "${YELLOW}Press Ctrl+C after seeing 'Database seeded successfully'${NC}"
    cd "$BACKEND_DIR/src/Nesi.Api"
    dotnet run
}

# Function to reset database
reset_database() {
    echo -e "${RED}WARNING: This will DELETE ALL DATA in '$DB_NAME'!${NC}"
    read -p "Are you sure? Type 'yes' to continue: " confirm
    
    if [ "$confirm" != "yes" ]; then
        echo -e "${YELLOW}Cancelled.${NC}"
        exit 0
    fi
    
    echo -e "${YELLOW}Dropping database '$DB_NAME'...${NC}"
    mysql -u"$DB_USER" -p"$DB_PASSWORD" -e "DROP DATABASE IF EXISTS $DB_NAME;"
    create_database
    run_migrations
    echo -e "${GREEN}✓ Database reset complete${NC}"
}

# Main script logic
case "${1:-auto}" in
    auto)
        echo -e "${YELLOW}Running automatic setup...${NC}"
        check_mysql
        create_database
        run_migrations
        echo ""
        echo -e "${GREEN}================================${NC}"
        echo -e "${GREEN}Setup Complete!${NC}"
        echo -e "${GREEN}================================${NC}"
        echo ""
        echo -e "${YELLOW}To seed demo data, run the application:${NC}"
        echo -e "  cd $BACKEND_DIR/src/Nesi.Api"
        echo -e "  dotnet run"
        echo ""
        echo -e "${YELLOW}Demo users:${NC}"
        echo -e "  admin      / Admin@123      (Admin)"
        echo -e "  manager1   / Manager@123    (Manager)"
        echo -e "  employee1  / Employee@123   (Employee)"
        ;;
    
    manual)
        echo -e "${YELLOW}Running manual database creation...${NC}"
        check_mysql
        create_database
        echo ""
        echo -e "${GREEN}Database created. Run migrations manually:${NC}"
        echo -e "  cd $BACKEND_DIR"
        echo -e "  dotnet ef database update --project src/Nesi.Infrastructure --startup-project src/Nesi.Api"
        ;;
    
    migrate)
        echo -e "${YELLOW}Running migrations only...${NC}"
        check_mysql
        run_migrations
        ;;
    
    reset)
        check_mysql
        reset_database
        ;;
    
    *)
        echo -e "${RED}Invalid option: $1${NC}"
        echo ""
        echo "Usage: $0 [option]"
        echo "Options:"
        echo "  auto     - Automatic setup (create DB, run migrations)"
        echo "  manual   - Manual DB creation only"
        echo "  migrate  - Run migrations only"
        echo "  reset    - Drop and recreate database (WARNING: DATA LOSS!)"
        exit 1
        ;;
esac
