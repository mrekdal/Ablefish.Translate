CREATE TABLE [dbo].[WorkItem] (
    [WorkId]    INT            IDENTITY (1, 1) NOT NULL,
    [ProjectId] INT            NOT NULL,
    [RowKey]    VARCHAR (64)   NOT NULL,
    [RawText]   NVARCHAR (MAX) NOT NULL,
    [CheckSrc]  AS             (checksum([RawText])),
    [RowVer]    ROWVERSION     NOT NULL,
    [CreatedAt] DATETIME       DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_WorkItem] PRIMARY KEY CLUSTERED ([WorkId] ASC),
    CONSTRAINT [FK_WorkItem_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId])
);








GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_WorkItem_ProjectId_RowKey]
    ON [dbo].[WorkItem]([ProjectId] ASC, [RowKey] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkItem_CheckSrc]
    ON [dbo].[WorkItem]([CheckSrc] ASC);

