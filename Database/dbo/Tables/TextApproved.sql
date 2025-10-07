CREATE TABLE [dbo].[TextApproved] (
    [ApprId]    INT          IDENTITY (1, 1) NOT NULL,
    [LangTrg]   VARCHAR (12) NOT NULL,
    [CheckSrc]  INT          NOT NULL,
    [CheckTrg]  INT          NOT NULL,
    [CreatedAt] DATETIME     DEFAULT (getdate()) NOT NULL,
    [RowVer]    ROWVERSION   NOT NULL,
    [WorkId]    INT          NULL,
    PRIMARY KEY CLUSTERED ([ApprId] ASC),
    FOREIGN KEY ([LangTrg]) REFERENCES [dbo].[TextLanguage] ([LangKey]),
    FOREIGN KEY ([WorkId]) REFERENCES [dbo].[WorkItem] ([WorkId])
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_TextApproved_WorkId_LangTrg]
    ON [dbo].[TextApproved]([WorkId] ASC, [LangTrg] ASC);

