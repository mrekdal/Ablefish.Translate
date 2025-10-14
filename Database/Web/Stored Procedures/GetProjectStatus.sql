CREATE PROCEDURE Web.GetProjectStatus( @LogTo VARCHAR(16) ) AS
BEGIN
  SELECT p.ProjectId, p.ProjectName, p.ShortName, utl.LangKey,
    COUNT(*) AS WorkTotal, 
    COUNT (tb.BlockId) AS WorkDone
  FROM dbo.Project p
  JOIN dbo.WorkItem wi ON wi.ProjectId = p.ProjectId
  JOIN dbo.UserProject up ON up.ProjectId = p.ProjectId AND up.LogTo = @LogTo
  JOIN dbo.UserTargetLanguage utl ON utl.LogTo = up.LogTo AND utl.IsActive = 1
  LEFT JOIN dbo.TextBlock tb ON tb.WorkId = wi.WorkId AND tb.LogTo = up.LogTo AND tb.LangKey = utl.LangKey
  GROUP BY p.ProjectId, p.ProjectName, p.ShortName, utl.LangKey;
END