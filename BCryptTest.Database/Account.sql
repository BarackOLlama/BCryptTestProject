CREATE TABLE [dbo].[Account]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [username] VARCHAR(50) NOT NULL, 
    [password] VARCHAR(64) NOT NULL, 
    [salt] VARCHAR(32) NOT NULL
)
