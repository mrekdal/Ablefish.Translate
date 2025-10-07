CREATE PROCEDURE dbo.GetTranslations( @ProjectId INT, @LangKey VARCHAR(12) ) AS 
BEGIN
  SELECT wi.WorkId, wi.RawText AS SrcText, td.RawText AS FinalText, ul.LastName AS Editor
  FROM dbo.WorkItem wi
  JOIN dbo.ProjectTranslation tp ON tp.ProjectId = wi.ProjectId AND tp.LangKey = @LangKey
  JOIN dbo.UserList ul ON ul.LogTo = tp.GoldLogTo
  LEFT JOIN dbo.TextBlock td ON td.WorkId = wi.WorkId AND td.LangKey = @LangKey AND td.LogTo = tp.GoldLogTo
  WHERE wi.ProjectId = @ProjectId;
END