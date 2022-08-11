CREATE TABLE [dbo].[Score] (
    [Date]   DATETIME     NOT NULL,
    [Name]   VARCHAR (50) NULL,
    [Scores] INT          NULL,
    PRIMARY KEY CLUSTERED ([Date] ASC)
);

