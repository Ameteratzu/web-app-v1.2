USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'desarrollo')
BEGIN
    CREATE DATABASE desarrollo;
END
GO

USE desarrollo;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'colaboradores')
BEGIN
    CREATE TABLE colaboradores (
        Id INT PRIMARY KEY IDENTITY,
        Nombre NVARCHAR(100)
    );

    INSERT INTO colaboradores (Nombre) VALUES
    ('Amet'),
    ('Clark Kent'),
    ('Diana Prince');
END
GO
