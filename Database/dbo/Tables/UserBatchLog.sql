CREATE TABLE [dbo].[UserBatchLog] (
    [BatchId]   INT          IDENTITY (1, 1) NOT NULL,
    [LogTo]     VARCHAR (16) NOT NULL,
    [LangWork]  VARCHAR (12) NOT NULL,
    [LangHelp]  VARCHAR (12) NOT NULL,
    [CreatedAt] DATETIME     CONSTRAINT [DF_UserBatch_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [ProjectId] INT          NOT NULL,
    [CreatedBy] INT          DEFAULT (user_id()) NULL,
    CONSTRAINT [PK_UserBatch] PRIMARY KEY CLUSTERED ([BatchId] ASC),
    CONSTRAINT [FK_UserBatch_LangHelp] FOREIGN KEY ([LangHelp]) REFERENCES [dbo].[TextLanguage] ([LangKey]),
    CONSTRAINT [FK_UserBatch_LangWork] FOREIGN KEY ([LangWork]) REFERENCES [dbo].[TextLanguage] ([LangKey]) ON UPDATE CASCADE,
    CONSTRAINT [FK_UserBatch_LogTo] FOREIGN KEY ([LogTo]) REFERENCES [dbo].[UserList] ([LogTo]) ON UPDATE CASCADE,
    CONSTRAINT [FK_UserBatch_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId])
);

