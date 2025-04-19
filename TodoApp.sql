CREATE DATABASE [TodoApp];
GO

USE [TodoApp];
GO

CREATE TABLE [dbo].[Users] (
    [Id]           INT           IDENTITY(1,1) NOT NULL,
    [Username]     NVARCHAR(50)  NOT NULL,
    [Email]        NVARCHAR(100) NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,
    [CreatedAt]    DATETIME2     NOT NULL,
    [LastLoginAt]  DATETIME2     NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE INDEX [IX_Users_Username]
    ON [dbo].[Users]([Username] ASC);
GO

CREATE UNIQUE INDEX [IX_Users_Email]
    ON [dbo].[Users]([Email] ASC);
GO

CREATE TABLE [dbo].[Todos] (
    [Id]          INT           IDENTITY(1,1) NOT NULL,
    [Title]       NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NOT NULL,
    [IsCompleted] BIT           NOT NULL,
    [DueDate]     DATETIME2     NULL,
    [Priority]    INT           NOT NULL,
    [UserId]      INT           NOT NULL,
    [CreatedAt]   DATETIME2     NOT NULL,
    [UpdatedAt]   DATETIME2     NULL,
    CONSTRAINT [PK_Todos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Todos_Users_UserId]
        FOREIGN KEY([UserId])
        REFERENCES [dbo].[Users]([Id])
        ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Todos_UserId]
    ON [dbo].[Todos]([UserId] ASC);
GO
