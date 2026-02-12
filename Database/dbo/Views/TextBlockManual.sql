CREATE VIEW [dbo].[TextBlockManual] AS
  SELECT wi.ProjectId, tb.*
  FROM dbo.TextBlock tb
  JOIN dbo.WorkItem wi ON wi.WorkId = tb.WorkId AND wi.CheckSrc = tb.CheckSrc
  JOIN dbo.UserList ul ON ul.LogTo = tb.LogTo AND ul.MachineUser = 0;