CREATE TABLE [dbo].[UserProject] (
    [RowId]     INT          IDENTITY (1, 1) NOT NULL,
    [ProjectId] INT          NOT NULL,
    [LogTo]     VARCHAR (16) NOT NULL,
    [CreatedAt] DATETIME     DEFAULT (getdate()) NOT NULL,
    [RowVer]    ROWVERSION   NOT NULL,
    PRIMARY KEY CLUSTERED ([RowId] ASC),
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_UserProject_LogTo] FOREIGN KEY ([LogTo]) REFERENCES [dbo].[UserList] ([LogTo]) ON UPDATE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_UserProject_LogTo_ProjectId]
    ON [dbo].[UserProject]([LogTo] ASC, [ProjectId] ASC);

