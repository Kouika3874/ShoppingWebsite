
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/16/2025 11:52:39
-- Generated from EDMX file: D:\Project14\Models\PChouseModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PChouseDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Product_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[table_Product] DROP CONSTRAINT [FK_Product_Category];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[table_Admin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[table_Admin];
GO
IF OBJECT_ID(N'[dbo].[table_Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[table_Category];
GO
IF OBJECT_ID(N'[dbo].[table_Member]', 'U') IS NOT NULL
    DROP TABLE [dbo].[table_Member];
GO
IF OBJECT_ID(N'[dbo].[table_Order]', 'U') IS NOT NULL
    DROP TABLE [dbo].[table_Order];
GO
IF OBJECT_ID(N'[dbo].[table_OrderDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[table_OrderDetail];
GO
IF OBJECT_ID(N'[dbo].[table_PaymentLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[table_PaymentLog];
GO
IF OBJECT_ID(N'[dbo].[table_Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[table_Product];
GO
IF OBJECT_ID(N'[dbo].[table_ShipmentLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[table_ShipmentLog];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'table_Admin'
CREATE TABLE [dbo].[table_Admin] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AdminId] nvarchar(50)  NOT NULL,
    [Password] nvarchar(255)  NOT NULL,
    [Name] nvarchar(100)  NULL
);
GO

-- Creating table 'table_Category'
CREATE TABLE [dbo].[table_Category] (
    [Name] nvarchar(100)  NULL,
    [Description] nvarchar(255)  NULL,
    [CategoryId] nvarchar(10)  NOT NULL,
    [TempCategoryId] nvarchar(10)  NULL
);
GO

-- Creating table 'table_Member'
CREATE TABLE [dbo].[table_Member] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(50)  NOT NULL,
    [Password] nvarchar(255)  NOT NULL,
    [Name] nvarchar(50)  NULL,
    [Email] nvarchar(100)  NULL
);
GO

-- Creating table 'table_Order'
CREATE TABLE [dbo].[table_Order] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderGuid] uniqueidentifier  NOT NULL,
    [UserId] nvarchar(50)  NULL,
    [Receiver] nvarchar(50)  NULL,
    [Email] nvarchar(100)  NULL,
    [Address] nvarchar(255)  NULL,
    [Date] datetime  NULL
);
GO

-- Creating table 'table_OrderDetail'
CREATE TABLE [dbo].[table_OrderDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderGuid] uniqueidentifier  NULL,
    [UserId] nvarchar(50)  NULL,
    [ProductId] nvarchar(50)  NULL,
    [Name] nvarchar(100)  NULL,
    [Price] decimal(10,2)  NULL,
    [Quantity] int  NULL,
    [IsApproved] bit  NULL
);
GO

-- Creating table 'table_PaymentLog'
CREATE TABLE [dbo].[table_PaymentLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderGuid] uniqueidentifier  NULL,
    [Amount] decimal(10,2)  NULL,
    [Status] nvarchar(50)  NULL,
    [CreatedAt] datetime  NULL
);
GO

-- Creating table 'table_Product'
CREATE TABLE [dbo].[table_Product] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] nvarchar(50)  NOT NULL,
    [Name] nvarchar(100)  NULL,
    [Price] decimal(10,2)  NULL,
    [Image] nvarchar(255)  NULL,
    [CategoryId] nvarchar(10)  NULL
);
GO

-- Creating table 'table_ShipmentLog'
CREATE TABLE [dbo].[table_ShipmentLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderGuid] uniqueidentifier  NULL,
    [Status] nvarchar(50)  NULL,
    [TrackingNumber] nvarchar(100)  NULL,
    [ShipDate] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'table_Admin'
ALTER TABLE [dbo].[table_Admin]
ADD CONSTRAINT [PK_table_Admin]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [CategoryId] in table 'table_Category'
ALTER TABLE [dbo].[table_Category]
ADD CONSTRAINT [PK_table_Category]
    PRIMARY KEY CLUSTERED ([CategoryId] ASC);
GO

-- Creating primary key on [Id] in table 'table_Member'
ALTER TABLE [dbo].[table_Member]
ADD CONSTRAINT [PK_table_Member]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'table_Order'
ALTER TABLE [dbo].[table_Order]
ADD CONSTRAINT [PK_table_Order]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'table_OrderDetail'
ALTER TABLE [dbo].[table_OrderDetail]
ADD CONSTRAINT [PK_table_OrderDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'table_PaymentLog'
ALTER TABLE [dbo].[table_PaymentLog]
ADD CONSTRAINT [PK_table_PaymentLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'table_Product'
ALTER TABLE [dbo].[table_Product]
ADD CONSTRAINT [PK_table_Product]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'table_ShipmentLog'
ALTER TABLE [dbo].[table_ShipmentLog]
ADD CONSTRAINT [PK_table_ShipmentLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CategoryId] in table 'table_Product'
ALTER TABLE [dbo].[table_Product]
ADD CONSTRAINT [FK_Product_Category]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[table_Category]
        ([CategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_Category'
CREATE INDEX [IX_FK_Product_Category]
ON [dbo].[table_Product]
    ([CategoryId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------