CREATE TABLE [dbo].[Project] (
    [ProjectId]   INT          IDENTITY (1, 1) NOT NULL,
    [ProjectName] VARCHAR (64) NOT NULL,
    [LangKey]     VARCHAR (12) DEFAULT ('en') NOT NULL,
    [RowVer]      ROWVERSION   NOT NULL,
    [CreatedAt]   DATETIME     DEFAULT (getdate()) NOT NULL,
    [ShortName]   VARCHAR (16) NULL,
    [IsActive]    BIT          DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED ([ProjectId] ASC),
    CONSTRAINT [FK_Project_LangKey] FOREIGN KEY ([LangKey]) REFERENCES [dbo].[TextLanguage] ([LangKey])
);

