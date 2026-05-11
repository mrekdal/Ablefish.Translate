CREATE TABLE [dbo].[TextBlock] (
    [BlockId]     INT                                         IDENTITY (1, 1) NOT NULL,
    [LangKey]     VARCHAR (12)                                NULL,
    [RawText]     NVARCHAR (MAX)                              NULL,
    [WorkId]      INT                                         NULL,
    [LogTo]       VARCHAR (16)                                DEFAULT ('public') NOT NULL,
    [RowVer]      ROWVERSION                                  NOT NULL,
    [CreatedAt]   DATETIME                                    DEFAULT (getdate()) NOT NULL,
    [CheckRaw]    AS                                          (checksum([RawText])) PERSISTED,
    [IsDiscarded] BIT                                         DEFAULT ((0)) NOT NULL,
    [DiscardedBy] VARCHAR (16)                                NULL,
    [WithDoubt]   BIT                                         DEFAULT ((0)) NOT NULL,
    [CheckSrc]    INT                                         NOT NULL,
    [ValidFrom]   DATETIME2 (7) GENERATED ALWAYS AS ROW START CONSTRAINT [DF_TextBlock_ValidFrom] DEFAULT (sysutcdatetime()) NOT NULL,
    [ValidTo]     DATETIME2 (7) GENERATED ALWAYS AS ROW END   CONSTRAINT [DF_TextBlock_ValidTo] DEFAULT (CONVERT([datetime2](7),'9999-12-31 23:59:59.9999999')) NOT NULL,
    CONSTRAINT [PK_BlockId] PRIMARY KEY CLUSTERED ([BlockId] ASC),
    CONSTRAINT [FK_TextBlock_DiscardedBy] FOREIGN KEY ([DiscardedBy]) REFERENCES [dbo].[UserList] ([LogTo]),
    CONSTRAINT [FK_TextBlock_LangKey] FOREIGN KEY ([LangKey]) REFERENCES [dbo].[TextLanguage] ([LangKey]),
    CONSTRAINT [FK_TextBlock_LogTo] FOREIGN KEY ([LogTo]) REFERENCES [dbo].[UserList] ([LogTo]) ON UPDATE CASCADE,
    CONSTRAINT [FK_TextBlock_WorkId] FOREIGN KEY ([WorkId]) REFERENCES [dbo].[WorkItem] ([WorkId]),
    PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[TextBlockHistory], DATA_CONSISTENCY_CHECK=ON));
















GO



GO



GO
CREATE NONCLUSTERED INDEX [IDX_TextBlock_LogTo]
    ON [dbo].[TextBlock]([LogTo] ASC)
    INCLUDE([LangKey], [WorkId], [CheckSrc]);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_TextBlock_WorkItem_WorkId_CheckSrc_LangKey_LogTo]
    ON [dbo].[TextBlock]([WorkId] ASC, [CheckSrc] ASC, [LangKey] ASC, [LogTo] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_TextBlock_LangKey_IsDiscarded]
    ON [dbo].[TextBlock]([LangKey] ASC, [IsDiscarded] ASC)
    INCLUDE([WorkId], [LogTo], [CheckRaw]);

