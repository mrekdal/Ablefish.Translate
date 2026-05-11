CREATE TABLE [dbo].[TextBlockHistory] (
    [BlockId]     INT            NOT NULL,
    [LangKey]     VARCHAR (12)   NULL,
    [RawText]     NVARCHAR (MAX) NULL,
    [WorkId]      INT            NULL,
    [LogTo]       VARCHAR (16)   NOT NULL,
    [RowVer]      ROWVERSION     NOT NULL,
    [CreatedAt]   DATETIME       NOT NULL,
    [CheckRaw]    INT            NULL,
    [IsDiscarded] BIT            NOT NULL,
    [DiscardedBy] VARCHAR (16)   NULL,
    [WithDoubt]   BIT            NOT NULL,
    [CheckSrc]    INT            NOT NULL,
    [ValidFrom]   DATETIME2 (7)  NOT NULL,
    [ValidTo]     DATETIME2 (7)  NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_TextBlockHistory]
    ON [dbo].[TextBlockHistory]([ValidTo] ASC, [ValidFrom] ASC) WITH (DATA_COMPRESSION = PAGE);

