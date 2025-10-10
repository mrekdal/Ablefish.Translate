CREATE TABLE [dbo].[TextBlock] (
    [BlockId]   INT            IDENTITY (1, 1) NOT NULL,
    [LangKey]   VARCHAR (12)   NULL,
    [RawText]   NVARCHAR (MAX) NULL,
    [WorkId]    INT            NULL,
    [LogTo]     VARCHAR (16)   DEFAULT ('public') NOT NULL,
    [RowVer]    ROWVERSION     NOT NULL,
    [CreatedAt] DATETIME       DEFAULT (getdate()) NOT NULL,
    [CheckRaw]  AS             (checksum([RawText])) PERSISTED,
    CONSTRAINT [PK_BlockId] PRIMARY KEY CLUSTERED ([BlockId] ASC),
    CONSTRAINT [FK_TextBlock_LangKey] FOREIGN KEY ([LangKey]) REFERENCES [dbo].[TextLanguage] ([LangKey]),
    CONSTRAINT [FK_TextBlock_LogTo] FOREIGN KEY ([LogTo]) REFERENCES [dbo].[UserList] ([LogTo]) ON UPDATE CASCADE,
    CONSTRAINT [FK_TextBlock_WorkId] FOREIGN KEY ([WorkId]) REFERENCES [dbo].[WorkItem] ([WorkId])
);












GO



GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_TextBlock_WorkItemRowId_LangKey_LogTo]
    ON [dbo].[TextBlock]([WorkId] ASC, [LangKey] ASC, [LogTo] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_TextBlock_LogTo]
    ON [dbo].[TextBlock]([LogTo] ASC)
    INCLUDE([LangKey], [WorkId], [CheckRaw]);

