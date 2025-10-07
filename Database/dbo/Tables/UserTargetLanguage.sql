CREATE TABLE [dbo].[UserTargetLanguage] (
    [TargId]    INT          IDENTITY (1, 1) NOT NULL,
    [LogTo]     VARCHAR (16) NOT NULL,
    [LangKey]   VARCHAR (12) NOT NULL,
    [CreatedAt] DATETIME     DEFAULT (getdate()) NOT NULL,
    FOREIGN KEY ([LangKey]) REFERENCES [dbo].[TextLanguage] ([LangKey]) ON UPDATE CASCADE,
    FOREIGN KEY ([LogTo]) REFERENCES [dbo].[UserList] ([LogTo]) ON UPDATE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_UserTargetLanguage_LogTo_LangKey]
    ON [dbo].[UserTargetLanguage]([LogTo] ASC, [LangKey] ASC);

