USE master

IF (EXISTS (SELECT 1 FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = '[SampleCreateDB]' OR name = 'SampleCreateDB')))
	BEGIN

		ALTER DATABASE [SampleCreateDB]

		SET SINGLE_USER

		WITH ROLLBACK IMMEDIATE

		DROP DATABASE [SampleCreateDB];
		
	END