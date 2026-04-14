CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Customers` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `ContactName` varchar(100) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(100) CHARACTER SET utf8mb4 NULL,
    `Phone` varchar(20) CHARACTER SET utf8mb4 NULL,
    `Address` varchar(500) CHARACTER SET utf8mb4 NULL,
    `IsActive` tinyint(1) NOT NULL DEFAULT TRUE,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UpdatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
    CONSTRAINT `PK_Customers` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `JobTypes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Code` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(200) CHARACTER SET utf8mb4 NULL,
    `IsActive` tinyint(1) NOT NULL DEFAULT TRUE,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UpdatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
    CONSTRAINT `PK_JobTypes` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `PayTypes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Code` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Multiplier` decimal(5,2) NOT NULL,
    `IsActive` tinyint(1) NOT NULL DEFAULT TRUE,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UpdatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
    CONSTRAINT `PK_PayTypes` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Users` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Username` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `FirstName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `LastName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `PasswordHash` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
    `Role` int NOT NULL,
    `IsActive` tinyint(1) NOT NULL DEFAULT TRUE,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UpdatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `WorkOrders` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `WorkOrderNumber` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `CustomerId` int NOT NULL,
    `Description` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
    `StartDate` date NOT NULL,
    `EndDate` date NULL,
    `IsActive` tinyint(1) NOT NULL DEFAULT TRUE,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UpdatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
    CONSTRAINT `PK_WorkOrders` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_WorkOrders_Customers_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `Customers` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `Timesheets` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `Date` date NOT NULL,
    `Hours` decimal(5,2) NOT NULL,
    `WorkOrderId` int NULL,
    `PayTypeId` int NOT NULL,
    `JobTypeId` int NULL,
    `Notes` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL DEFAULT 0,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UpdatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
    CONSTRAINT `PK_Timesheets` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Timesheets_JobTypes_JobTypeId` FOREIGN KEY (`JobTypeId`) REFERENCES `JobTypes` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Timesheets_PayTypes_PayTypeId` FOREIGN KEY (`PayTypeId`) REFERENCES `PayTypes` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Timesheets_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Timesheets_WorkOrders_WorkOrderId` FOREIGN KEY (`WorkOrderId`) REFERENCES `WorkOrders` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

INSERT INTO `JobTypes` (`Id`, `Code`, `CreatedAt`, `CreatedBy`, `Description`, `IsActive`, `Name`, `UpdatedAt`, `UpdatedBy`)
VALUES (1, 'DEV', TIMESTAMP '2024-01-01 00:00:00', 'system', 'Software development work', TRUE, 'Development', NULL, NULL),
(2, 'QA', TIMESTAMP '2024-01-01 00:00:00', 'system', 'Testing and quality assurance', TRUE, 'Quality Assurance', NULL, NULL),
(3, 'DESIGN', TIMESTAMP '2024-01-01 00:00:00', 'system', 'UI/UX design work', TRUE, 'Design', NULL, NULL),
(4, 'PM', TIMESTAMP '2024-01-01 00:00:00', 'system', 'Project management activities', TRUE, 'Project Management', NULL, NULL);

INSERT INTO `PayTypes` (`Id`, `Code`, `CreatedAt`, `CreatedBy`, `IsActive`, `Multiplier`, `Name`, `UpdatedAt`, `UpdatedBy`)
VALUES (1, 'REG', TIMESTAMP '2024-01-01 00:00:00', 'system', TRUE, 1.0, 'Regular', NULL, NULL),
(2, 'OT', TIMESTAMP '2024-01-01 00:00:00', 'system', TRUE, 1.5, 'Overtime', NULL, NULL),
(3, 'DT', TIMESTAMP '2024-01-01 00:00:00', 'system', TRUE, 2.0, 'Double Time', NULL, NULL);

CREATE INDEX `IX_Customers_Name` ON `Customers` (`Name`);

CREATE UNIQUE INDEX `IX_JobTypes_Code` ON `JobTypes` (`Code`);

CREATE UNIQUE INDEX `IX_PayTypes_Code` ON `PayTypes` (`Code`);

CREATE INDEX `IX_Timesheets_JobTypeId` ON `Timesheets` (`JobTypeId`);

CREATE INDEX `IX_Timesheets_PayTypeId` ON `Timesheets` (`PayTypeId`);

CREATE INDEX `IX_Timesheets_Status` ON `Timesheets` (`Status`);

CREATE INDEX `IX_Timesheets_UserId_Date` ON `Timesheets` (`UserId`, `Date`);

CREATE INDEX `IX_Timesheets_WorkOrderId` ON `Timesheets` (`WorkOrderId`);

CREATE UNIQUE INDEX `IX_Users_Email` ON `Users` (`Email`);

CREATE UNIQUE INDEX `IX_Users_Username` ON `Users` (`Username`);

CREATE INDEX `IX_WorkOrders_CustomerId` ON `WorkOrders` (`CustomerId`);

CREATE UNIQUE INDEX `IX_WorkOrders_WorkOrderNumber` ON `WorkOrders` (`WorkOrderNumber`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260414165653_InitialCreate', '8.0.11');

COMMIT;

