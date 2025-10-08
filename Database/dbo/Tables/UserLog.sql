CREATE TABLE [dbo].[UserLog] (
    [RowId]     INT          IDENTITY (1, 1) NOT NULL,
    [LogTo]     VARCHAR (16) NOT NULL,
    [CreatedAt] DATETIME     DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([RowId] ASC)
);

