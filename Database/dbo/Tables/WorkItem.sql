CREATE TABLE [dbo].[WorkItem] (
    [WorkId]    INT                                         IDENTITY (1, 1) NOT NULL,
    [ProjectId] INT                                         NOT NULL,
    [RowKey]    VARCHAR (64)                                NOT NULL,
    [RawText]   NVARCHAR (MAX)                              NOT NULL,
    [CheckSrc]  AS                                          (checksum([RawText])),
    [RowVer]    ROWVERSION                                  NOT NULL,
    [CreatedAt] DATETIME                                    DEFAULT (getdate()) NOT NULL,
    [UpdatedAt] DATETIME                                    DEFAULT (getdate()) NOT NULL,
    [Flagged]   BIT                                         DEFAULT ((0)) NOT NULL,
    [FlaggedBy] VARCHAR (16)                                NULL,
    [ValidFrom] DATETIME2 (7) GENERATED ALWAYS AS ROW START DEFAULT (sysutcdatetime()) NOT NULL,
    [ValidTo]   DATETIME2 (7) GENERATED ALWAYS AS ROW END   DEFAULT (CONVERT([datetime2](7),'9999-12-31 23:59:59.9999999')) NOT NULL,
    CONSTRAINT [PK_WorkItem] PRIMARY KEY CLUSTERED ([WorkId] ASC),
    FOREIGN KEY ([FlaggedBy]) REFERENCES [dbo].[UserList] ([LogTo]),
    CONSTRAINT [FK_WorkItem_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[MSSQL_TemporalHistoryFor_338100245], DATA_CONSISTENCY_CHECK=ON));










GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_WorkItem_ProjectId_RowKey]
    ON [dbo].[WorkItem]([ProjectId] ASC, [RowKey] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkItem_CheckSrc]
    ON [dbo].[WorkItem]([CheckSrc] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkItem_ProjectId]
    ON [dbo].[WorkItem]([ProjectId] ASC)
    INCLUDE([CheckSrc]);

