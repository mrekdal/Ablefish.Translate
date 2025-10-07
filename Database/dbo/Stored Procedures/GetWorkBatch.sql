CREATE PROCEDURE [dbo].[GetWorkBatch]( @ProjectId INT, @LogTo VARCHAR(16), @LangWork VARCHAR(12), @LangHelp VARCHAR(12), @LogToAi VARCHAR(12) = 'DeepL' ) AS
BEGIN
  SELECT TOP 10 wi.WorkId, wi.RowKey, 
    p.LangKey AS Src1Key, wi.RawText AS Src1Text, CHECKSUM(wi.RawText) AS Src1Check, -- Source text 
    tsh.LangKey AS Src2Key, tsh.RawText AS Src2Text, CHECKSUM(tsh.RawText) AS Src2Check, ul.MachineUser AS Src2Machine, -- Translated helper text
    td.LangKey AS LangAiKey, td.RawText AS WorkAi, CHECKSUM(td.RawText) AS WorkAiCheck, -- AI Translated text
    @LangWork AS LangWorkKey, tw.RawText AS WorkFinal, CHECKSUM(tw.RawText) AS WorkFinalCheck,
	tlw.EnglishName AS WorkLanguage, tlh.EnglishName AS HelpLanguage,
	tw.LogTo,
	ta.ApprId
  FROM dbo.WorkItem wi
  JOIN dbo.Project p ON p.ProjectId = wi.ProjectId
  JOIN dbo.TextLanguage tlw ON tlw.LangKey = @LangWork
  LEFT JOIN dbo.ProjectTranslation pt ON pt.ProjectId = wi.ProjectId AND pt.LangKey = @LangWork
  LEFT JOIN dbo.ProjectTranslation pth ON pth.ProjectId = wi.ProjectId AND pth.LangKey = @LangHelp
  LEFT JOIN dbo.TextLanguage tlh ON tlh.LangKey = @LangHelp
  LEFT JOIN dbo.TextBlock td ON td.WorkId = wi.WorkId AND td.LangKey = @LangWork AND td.LogTo = @LogToAi
  LEFT JOIN dbo.TextBlock tsh ON tsh.WorkId = wi.WorkId AND tsh.LangKey = @LangHelp AND tsh.LogTo = pth.GoldLogTo
  LEFT JOIN dbo.TextBlock tw ON tw.WorkId = wi.WorkId AND tw.LangKey = @LangWork AND tw.LogTo = @LogTo
  LEFT JOIN dbo.UserList ul ON ul.LogTo = pth.GoldLogTo
  LEFT JOIN dbo.TextApproved ta ON ta.WorkId = wi.WorkId AND ta.LangTrg = @LangWork AND ta.CheckSrc = wi.CheckSrc AND ta.CheckTrg = CHECKSUM(tw.RawText)
  WHERE wi.ProjectId = @ProjectId AND ta.ApprId IS NULL;
END