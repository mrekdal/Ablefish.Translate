CREATE TABLE [dbo].[ApproveClick] (
    [RowId]     INT          IDENTITY (1, 1) NOT NULL,
    [WorkId]    INT          NOT NULL,
    [LogTo]     VARCHAR (16) NOT NULL,
    [CreatedAt] DATETIME     DEFAULT (getdate()) NOT NULL,
    [CheckSrc]  INT          NULL,
    CONSTRAINT [PK_ApproveClick] PRIMARY KEY CLUSTERED ([RowId] ASC),
    FOREIGN KEY ([LogTo]) REFERENCES [dbo].[UserList] ([LogTo]) ON UPDATE CASCADE,
    FOREIGN KEY ([WorkId]) REFERENCES [dbo].[WorkItem] ([WorkId])
);

