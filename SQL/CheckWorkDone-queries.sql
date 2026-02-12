SELECT YMDH, LogTo, COUNT(*) AS n
FROM
(
  SELECT CONVERT(char(13), CreatedAt, 120) AS YMDH, LogTo
  FROM dbo.TextBlock
  -- WHERE LogTo = 'lr2a649vjvg1'
) agg
GROUP BY YMDH, LogTo
ORDER BY YMDH DESC, LogTo;

exec web.GetWorkDone 'Martí';
GO
exec web.GetWorkDone 'Huda';
GO

exec web.GetWorkDone 'zxs29rog9jp4';
GO

exec Web.GetWorkBatch 4, 'zxs29rog9jp4', 'ca', 'nb';
exec Web.GetWorkBatch 4, 'zxs29rog9jp4', 'da', 'nb';
GO

SELECT * FROM dbo.UserList;
SELECT * FROM dbo.WorkItem WHERE ProjectId = 4;
GO

