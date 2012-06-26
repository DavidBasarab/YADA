USE [master];

IF (EXISTS (SELECT 1 FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = '[YadaTesting]' OR name = 'YadaTesting')))
    BEGIN

        ALTER DATABASE [YadaTesting]

        SET SINGLE_USER

        WITH ROLLBACK IMMEDIATE

        DROP DATABASE [YadaTesting];
        
    END
