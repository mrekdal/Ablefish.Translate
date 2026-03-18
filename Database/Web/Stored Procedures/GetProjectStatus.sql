CREATE PROCEDURE [Web].[GetProjectStatus]( @LogTo VARCHAR(16) ) AS
BEGIN
  DECLARE @CoLogTo VARCHAR(16);
  SELECT @CoLogTo = CoLogTo FROM dbo.UserList WHERE LogTo = @LogTo;
  SELECT p.ProjectId, p.ProjectName, p.ShortName, utl.LangKey,
    COUNT(*) AS WorkTotal, 
    COUNT (tb.BlockId) AS WorkDone
  FROM dbo.Project p
  JOIN dbo.WorkItem wi ON wi.ProjectId = p.ProjectId
  JOIN dbo.UserProject up ON up.ProjectId = p.ProjectId AND up.LogTo = @LogTo AND up.IsActive = 1
  JOIN dbo.UserTargetLanguage utl ON utl.LogTo = @LogTo AND utl.IsActive = 1
  LEFT JOIN dbo.TextBlock tb ON tb.WorkId = wi.WorkId AND tb.CheckSrc = wi.CheckSrc AND tb.LangKey = utl.LangKey 
    AND ( tb.LogTo = @LogTo OR tb.LogTo = @CoLogTo )
  GROUP BY p.ProjectId, p.ProjectName, p.ShortName, utl.LangKey;
END