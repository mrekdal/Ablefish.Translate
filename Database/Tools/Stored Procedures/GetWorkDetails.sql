CREATE PROCEDURE [Tools].[GetWorkDetails]( @ProjectId INT,  @StartAt DATETIME = NULL, @StopAt DATETIME = NULL ) AS
BEGIN
  SET @StartAt = ISNULL( @StartAt, GETDATE()-2 );
  SET @StopAt = ISNULL( @StopAt, DATEADD( DD, 2, @StartAt ) );

  SELECT CONCAT(YMDH,'0') AS Period, LogTo, Act, n, BoundAt
  FROM
  (
    SELECT CONVERT(char(15), tb.CreatedAt, 120) AS YMDH, tb.LogTo, 'TXT' AS Act, COUNT(*) AS n, MAX(tb.CreatedAt) AS BoundAt
      FROM dbo.TextBlock tb
      JOIN dbo.WorkItem wi ON wi.WorkId = tb.WorkId 
      JOIN dbo.UserList ul ON ul.LogTo = tb.LogTo AND ul.MachineUser = 0
      WHERE wi.ProjectId = @ProjectId AND tb.CreatedAt BETWEEN @StartAt AND @StopAt
	  GROUP BY CONVERT(char(15), tb.CreatedAt, 120), tb.LogTo
    UNION
    SELECT CONVERT(char(15), CreatedAt, 120) AS YMDH, LogTo, 'GET' AS Act, COUNT(*) AS n, MIN(CreatedAt) AS BoundAt
      FROM dbo.UserBatchLog 
	  WHERE ProjectId = @ProjectId AND  CreatedAt BETWEEN @StartAt AND @StopAt
	 GROUP BY CONVERT(char(15), CreatedAt, 120), LogTo
	UNION
    SELECT CONVERT(char(15), ac.CreatedAt, 120) AS YMDH, ac.LogTo, 'APP' AS Act, COUNT(*) AS n, MAX(ac.CreatedAt) AS BoundAt
      FROM dbo.ApproveClick ac
	  JOIN dbo.WorkItem wi ON wi.WorkId = ac.WorkId
	  WHERE wi.ProjectId = @ProjectId AND ac.CreatedAt BETWEEN @StartAt AND @StopAt
	 GROUP BY CONVERT(char(15), ac.CreatedAt, 120), LogTo
    ) agg
  ORDER BY YMDH, BoundAt;
END