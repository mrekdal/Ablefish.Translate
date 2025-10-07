CREATE TABLE [dbo].[Project] (
    [ProjectId]   INT          IDENTITY (1, 1) NOT NULL,
    [ProjectName] VARCHAR (64) NOT NULL,
    [LangKey]     VARCHAR (12) DEFAULT ('en') NOT NULL,
    [RowVer]      ROWVERSION   NOT NULL,
    [CreatedAt]   DATETIME     DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProjectId] ASC),
    FOREIGN KEY ([LangKey]) REFERENCES [dbo].[TextLanguage] ([LangKey])
);

