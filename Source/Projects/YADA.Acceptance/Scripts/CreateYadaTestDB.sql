USE [master];
GO

IF (EXISTS (SELECT 1 FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = '[YadaTesting]' OR name = 'YadaTesting')))
    BEGIN

        ALTER DATABASE [YadaTesting]

        SET SINGLE_USER

        WITH ROLLBACK IMMEDIATE

        DROP DATABASE [YadaTesting];
        
    END

GO

USE [master];
GO

CREATE DATABASE [YadaTesting]
GO

ALTER DATABASE [YadaTesting] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [YadaTesting].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [YadaTesting] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [YadaTesting] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [YadaTesting] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [YadaTesting] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [YadaTesting] SET ARITHABORT OFF 
GO

ALTER DATABASE [YadaTesting] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [YadaTesting] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [YadaTesting] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [YadaTesting] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [YadaTesting] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [YadaTesting] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [YadaTesting] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [YadaTesting] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [YadaTesting] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [YadaTesting] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [YadaTesting] SET  DISABLE_BROKER 
GO

ALTER DATABASE [YadaTesting] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [YadaTesting] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [YadaTesting] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [YadaTesting] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [YadaTesting] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [YadaTesting] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [YadaTesting] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [YadaTesting] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [YadaTesting] SET  MULTI_USER 
GO

ALTER DATABASE [YadaTesting] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [YadaTesting] SET DB_CHAINING OFF 
GO

ALTER DATABASE [YadaTesting] SET  READ_WRITE 
GO


USE [YadaTesting]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'	AND TABLE_NAME='[dbo].[NarrowSmallData]') 
    BEGIN
    
        ALTER TABLE [dbo].[NarrowSmallData] DROP CONSTRAINT [DF_NarrowNoKeysSmallData_DateAdded]
        

        DROP TABLE [dbo].[NarrowSmallData]
    
    END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[NarrowSmallData](
    [TableKey] [int] IDENTITY(1,1) NOT NULL,
    [TestValue1] [varchar](50) NOT NULL,
    [TestValue2] [varchar](255) NOT NULL,
    [DateAdded] [datetime] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[NarrowSmallData] ADD  CONSTRAINT [DF_NarrowNoKeysSmallData_DateAdded]  DEFAULT (getdate()) FOR [DateAdded]
GO

USE [YadaTesting]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNarrowSmallDataByID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNarrowSmallDataByID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetNarrowSmallDataByID]
    @SmallDataID		INT
AS 
BEGIN
    SELECT 
        TableKey,
        TestValue1,
        TestValue2,
        DateAdded	
    FROM [dbo].[NarrowSmallData]
    WHERE
            TableKey = @SmallDataID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateSmallDataRow]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateSmallDataRow]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateSmallDataRow]
    @TestValue1		VARCHAR(50),
    @TestValue2		VARCHAR(255)
AS 
BEGIN
    
    INSERT INTO NarrowSmallData
        (TestValue1, TestValue2)
    VALUES
        (@TestValue1, @TestValue2)

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRangeOfRecords]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRangeOfRecords]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetRangeOfRecords]
    @MinRecordID	INT		= 0,
    @MaxRecordID	INT		= 15
AS 
BEGIN
    
    SELECT 
        TableKey,
        TestValue1,
        TestValue2,
        DateAdded	
    FROM [dbo].[NarrowSmallData]
    WHERE
            TableKey >= @MinRecordID 
        AND TableKey <= @MaxRecordID

END
GO

