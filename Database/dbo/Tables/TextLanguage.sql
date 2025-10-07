CREATE TABLE [dbo].[TextLanguage] (
    [LangKey]     VARCHAR (12) NOT NULL,
    [EnglishName] VARCHAR (32) NULL,
    [ManEdit]     BIT          NULL,
    [DeepL]       BIT          DEFAULT ((0)) NOT NULL,
    [RowVer]      ROWVERSION   NOT NULL,
    [CreatedAt]   DATETIME     DEFAULT (getdate()) NOT NULL,
    [ShortName]   VARCHAR (3)  NOT NULL,
    PRIMARY KEY CLUSTERED ([LangKey] ASC)
);

