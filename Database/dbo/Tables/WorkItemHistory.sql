CREATE TABLE [dbo].[WorkItemHistory] (
    [WorkId]    INT            NOT NULL,
    [ProjectId] INT            NOT NULL,
    [RowKey]    VARCHAR (64)   NOT NULL,
    [RawText]   NVARCHAR (MAX) NOT NULL,
    [CheckSrc]  INT            NULL,
    [RowVer]    ROWVERSION     NOT NULL,
    [CreatedAt] DATETIME       NOT NULL,
    [UpdatedAt] DATETIME       NOT NULL,
    [Flagged]   BIT            NOT NULL,
    [FlaggedBy] VARCHAR (16)   NULL,
    [ValidFrom] DATETIME2 (7)  NOT NULL,
    [ValidTo]   DATETIME2 (7)  NOT NULL,
    [InUse]     BIT            NOT NULL
);




GO
CREATE CLUSTERED INDEX [ix_MSSQL_TemporalHistoryFor_338100245]
    ON [dbo].[WorkItemHistory]([ValidTo] ASC, [ValidFrom] ASC) WITH (DATA_COMPRESSION = PAGE);

