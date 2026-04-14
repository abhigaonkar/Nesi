-- ============================================================
-- NESI Database Creation Script for MySQL
-- ============================================================
-- This script creates the NesiDb database and a user for the application
-- Run this as MySQL root user before running the application
-- ============================================================

-- Create database
DROP DATABASE IF EXISTS NesiDb;
CREATE DATABASE NesiDb
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

-- Create application user (optional - adjust password as needed)
-- CREATE USER IF NOT EXISTS 'nesiapp'@'localhost' IDENTIFIED BY 'NesiApp@2024!';
-- GRANT ALL PRIVILEGES ON NesiDb.* TO 'nesiapp'@'localhost';
-- FLUSH PRIVILEGES;

USE NesiDb;

-- Note: The actual table schema will be created by EF Core migrations
-- This script only creates the database itself
-- To create tables, run: dotnet ef database update

SELECT 'Database NesiDb created successfully!' as Status;
