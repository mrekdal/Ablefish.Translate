CREATE TABLE [dbo].[ProjectTranslation] (
    [RowId]     INT          IDENTITY (1, 1) NOT NULL,
    [ProjectId] INT          NOT NULL,
    [LangKey]   VARCHAR (12) NOT NULL,
    [GoldLogTo] VARCHAR (16) NOT NULL,
    [RowVer]    ROWVERSION   NOT NULL,
    [CreatedAt] DATETIME     DEFAULT (getdate()) NOT NULL,
    FOREIGN KEY ([LangKey]) REFERENCES [dbo].[TextLanguage] ([LangKey]),
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_ProjectTranslation_LogTo] FOREIGN KEY ([GoldLogTo]) REFERENCES [dbo].[UserList] ([LogTo]) ON UPDATE CASCADE
);

