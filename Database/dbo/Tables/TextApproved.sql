CREATE TABLE [dbo].[TextApproved] (
    [ApprId]    INT          IDENTITY (1, 1) NOT NULL,
    [LangTrg]   VARCHAR (12) NOT NULL,
    [CheckSrc]  INT          NOT NULL,
    [CheckTrg]  INT          NOT NULL,
    [CreatedAt] DATETIME     DEFAULT (getdate()) NOT NULL,
    [RowVer]    ROWVERSION   NOT NULL,
    [WorkId]    INT          NULL,
    [UpdatedAt] DATETIME     DEFAULT (getdate()) NOT NULL,
    [WithDoubt] BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TextApproved] PRIMARY KEY CLUSTERED ([ApprId] ASC),
    CONSTRAINT [FK_TextApproved_LangTrg] FOREIGN KEY ([LangTrg]) REFERENCES [dbo].[TextLanguage] ([LangKey]),
    CONSTRAINT [FK_TextApproved_WorkId] FOREIGN KEY ([WorkId]) REFERENCES [dbo].[WorkItem] ([WorkId])
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_TextApproved_WorkId_CheckSrc_LangTrg]
    ON [dbo].[TextApproved]([WorkId] ASC, [CheckSrc] ASC, [LangTrg] ASC);

