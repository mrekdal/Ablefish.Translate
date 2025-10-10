CREATE TABLE [dbo].[ProjectTranslation] (
    [RowId]     INT          IDENTITY (1, 1) NOT NULL,
    [ProjectId] INT          NOT NULL,
    [LangKey]   VARCHAR (12) NOT NULL,
    [GoldLogTo] VARCHAR (16) NOT NULL,
    [RowVer]    ROWVERSION   NOT NULL,
    [CreatedAt] DATETIME     DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ProjectTranslation] PRIMARY KEY CLUSTERED ([RowId] ASC),
    CONSTRAINT [FK_ProjectTranslation_LangKey] FOREIGN KEY ([LangKey]) REFERENCES [dbo].[TextLanguage] ([LangKey]),
    CONSTRAINT [FK_ProjectTranslation_LogTo] FOREIGN KEY ([GoldLogTo]) REFERENCES [dbo].[UserList] ([LogTo]) ON UPDATE CASCADE,
    CONSTRAINT [FK_ProjectTranslation_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId])
);

