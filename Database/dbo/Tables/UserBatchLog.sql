CREATE TABLE [dbo].[UserBatchLog] (
    [BatchId]   INT          IDENTITY (1, 1) NOT NULL,
    [LogTo]     VARCHAR (16) NOT NULL,
    [LangWork]  VARCHAR (12) NOT NULL,
    [LangHelp]  VARCHAR (12) NOT NULL,
    [CreatedAt] DATETIME     DEFAULT (getdate()) NOT NULL,
    [ProjectId] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([BatchId] ASC),
    FOREIGN KEY ([LangHelp]) REFERENCES [dbo].[TextLanguage] ([LangKey]),
    FOREIGN KEY ([LangWork]) REFERENCES [dbo].[TextLanguage] ([LangKey]) ON UPDATE CASCADE,
    FOREIGN KEY ([LogTo]) REFERENCES [dbo].[UserList] ([LogTo]) ON UPDATE CASCADE,
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId])
);

