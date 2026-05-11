
CREATE FUNCTION Tools.GetUserWorkPeriods( @LogTo  VARCHAR(16) )
RETURNS TABLE AS RETURN
(
    SELECT CONVERT(char(15), tb.CreatedAt, 120) AS YMDH, tb.LogTo, 'TXT' AS Act, COUNT(*) AS n, MAX(tb.CreatedAt) AS BoundAt
      FROM dbo.TextBlock tb
      JOIN dbo.WorkItem wi ON wi.WorkId = tb.WorkId
      WHERE tb.LogTo = @LogTo
      GROUP BY CONVERT(char(15), tb.CreatedAt, 120), tb.LogTo
    UNION
    SELECT CONVERT(char(15), CreatedAt, 120), LogTo, 'GET', COUNT(*), MIN(CreatedAt)
      FROM dbo.UserBatchLog
      WHERE LogTo = @LogTo
      GROUP BY CONVERT(char(15), CreatedAt, 120), LogTo
    UNION
    SELECT CONVERT(char(15), ac.CreatedAt, 120), ac.LogTo, 'APP', COUNT(*), MAX(ac.CreatedAt)
      FROM dbo.ApproveClick ac
      WHERE ac.LogTo = @LogTo
      GROUP BY CONVERT(char(15), ac.CreatedAt, 120), ac.LogTo
);