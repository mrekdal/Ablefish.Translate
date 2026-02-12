CREATE TABLE [dbo].[UserList] (
    [LogTo]          VARCHAR (16) NOT NULL,
    [UserName]       VARCHAR (32) NULL,
    [Email]          VARCHAR (64) NULL,
    [FirstName]      VARCHAR (64) NULL,
    [LastName]       VARCHAR (64) NULL,
    [MachineUser]    BIT          DEFAULT ((0)) NOT NULL,
    [UserId]         INT          IDENTITY (1, 1) NOT NULL,
    [ProjectId]      INT          DEFAULT ((4)) NOT NULL,
    [TargetLanguage] VARCHAR (12) DEFAULT ('es') NOT NULL,
    [HelperLanguage] VARCHAR (12) DEFAULT ('nb') NOT NULL,
    [RowVer]         ROWVERSION   NOT NULL,
    [CreatedAt]      DATETIME     DEFAULT (getdate()) NOT NULL,
    [IsActive]       BIT          DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_UserList] PRIMARY KEY CLUSTERED ([LogTo] ASC),
    CONSTRAINT [FK_UserList_Helper] FOREIGN KEY ([HelperLanguage]) REFERENCES [dbo].[TextLanguage] ([LangKey]),
    CONSTRAINT [FK_UserList_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_UserList_Target] FOREIGN KEY ([TargetLanguage]) REFERENCES [dbo].[TextLanguage] ([LangKey])
);

