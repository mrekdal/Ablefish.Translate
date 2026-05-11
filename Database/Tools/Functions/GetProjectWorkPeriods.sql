
CREATE FUNCTION Tools.GetProjectWorkPeriods(@ProjectId INT, @StartAt DATETIME, @StopAt DATETIME)
RETURNS TABLE AS RETURN
(
    SELECT CONVERT(char(15), tb.CreatedAt, 120) AS YMDH, tb.LogTo, 'TXT' AS Act, COUNT(*) AS n, MAX(tb.CreatedAt) AS BoundAt
      FROM dbo.TextBlock tb
      JOIN dbo.WorkItem wi ON wi.WorkId = tb.WorkId
      JOIN dbo.UserList ul ON ul.LogTo = tb.LogTo AND ul.MachineUser = 0
      WHERE wi.ProjectId = @ProjectId AND tb.CreatedAt BETWEEN @StartAt AND @StopAt
      GROUP BY CONVERT(char(15), tb.CreatedAt, 120), tb.LogTo
    UNION
    SELECT CONVERT(char(15), CreatedAt, 120), LogTo, 'GET', COUNT(*), MIN(CreatedAt)
      FROM dbo.UserBatchLog
      WHERE ProjectId = @ProjectId AND CreatedAt BETWEEN @StartAt AND @StopAt
      GROUP BY CONVERT(char(15), CreatedAt, 120), LogTo
    UNION
    SELECT CONVERT(char(15), ac.CreatedAt, 120), ac.LogTo, 'APP', COUNT(*), MAX(ac.CreatedAt)
      FROM dbo.ApproveClick ac
      JOIN dbo.WorkItem wi ON wi.WorkId = ac.WorkId
      WHERE wi.ProjectId = @ProjectId AND ac.CreatedAt BETWEEN @StartAt AND @StopAt
      GROUP BY CONVERT(char(15), ac.CreatedAt, 120), ac.LogTo
);