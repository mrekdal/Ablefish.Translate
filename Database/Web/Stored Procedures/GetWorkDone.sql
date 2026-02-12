CREATE PROCEDURE [Web].[GetWorkDone]( @LogTo VARCHAR(16) = NULL ) AS 
BEGIN
  SELECT wi.ProjectId, tb.LangKey, ul.FirstName, ul.LogTo, COUNT(*) AS n
  FROM dbo.TextBlock tb
  JOIN dbo.UserList ul ON ul.LogTo = tb.LogTo AND ul.MachineUser = 0
  JOIN dbo.WorkItem wi ON wi.WorkId = tb.WorkId
  WHERE ( ISNULL(@LogTo,ul.LogTo) = ul.LogTo ) OR ( ul.FirstName = @LogTo )
  GROUP BY wi.ProjectId, ul.LogTo, ul.FirstName, tb.LangKey
  ORDER by ul.FirstName, wi.ProjectId, tb.LangKey;
END