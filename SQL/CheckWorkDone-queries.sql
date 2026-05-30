EXEC Web.GetWorkPeriods '78120y9vpvpa', 15; -- Ikram
EXEC Web.GetWorkPeriods 'truo918l9xwc', 15; -- Huda
GO

SELECT * FROM dbo.UserList ORDER BY FirstName;
GO

UPDATE dbo.UserList SET FirstName = 'Magne', LastName = 'Rekdal' WHERE UserId = 29;
UPDATE dbo.UserList SET FirstName = 'Azure', LastName = 'Translated' WHERE UserId = 18;

SELECT CheckTrg, MIN(CheckSrc), MAX(CheckSrc), COUNT(*) 
FROM dbo.TextApproved
GROUP BY CheckTrg
HAVING COUNT(*) > 1;
GO

SELECT * FROM dbo.TextApproved ORDER BY ApprId DESC;
GO

EXEC Web.GetProjectStatus 'truo918l9xwc'; -- Huda
EXEC Web.GetProjectStatus 'zxs29rog9jp4'; -- Magne
EXEC Web.GetProjectStatus 'lr2a649vjvg1'; -- Huda
EXEC Web.GetProjectStatus '23kxcn6h3ard'; -- Marti
EXEC Web.GetProjectStatus '78120y9vpvpa'; -- Ikram
EXEC Web.GetProjectStatus '0jm0se48cq02'; -- Nils
GO


EXEC Tools.GetWorkDetails 3, '2026-02-23', '2026-02-25 03:00';
EXEC Tools.GetWorkDetails 3, '2026-02-23', '2026-02-24 03:00';
EXEC Tools.GetWorkDetails 11, '2026-02-24', '2026-02-25 03:00';
EXEC Tools.GetWorkDetails 11, '2026-03-17', '2026-03-19'
EXEC Tools.GetWorkDetails 11, '2026-03-20 03:00', '2026-03-21 03:00'
EXEC Tools.GetWorkDetails 11, '2026-03-20 23:00', '2026-03-21 23:00' -- Saturday
EXEC Tools.GetWorkDetails 11, '2026-03-22 03:00', '2026-03-23 03:00'
EXEC Tools.GetWorkDetails 11, '2026-03-21 03:00', '2026-03-22 03:00'
EXEC Tools.GetWorkDetails 11, '2026-03-30 03:00', '2026-04-11 03:00'
GO

EXEC Tools.GetWorkDetails 3, '2026-02-14', '2026-02-15 03:00';
EXEC Tools.GetWorkDetails 11, '2026-03-18 12:00', '2026-03-19 03:00';
EXEC Tools.GetWorkDetails 11, '2026-03-19 12:00', '2026-03-20 03:00';
EXEC Tools.GetWorkDetails 11, '2026-03-25 11:00', '2026-03-26 03:00';
EXEC Tools.GetWorkDetails 11, '2026-04-11 11:00', '2026-04-12 04:00';
GO
EXEC Tools.GetWorkDetails 11;
GO

SELECT YMDH, LogTo, COUNT(*) AS n
FROM
(
  SELECT CONVERT(char(13), tb.CreatedAt, 120) AS YMDH, tb.LogTo
  FROM dbo.TextBlock tb
  JOIN dbo.WorkItem wi ON wi.WorkId = tb.WorkId 
  JOIN dbo.UserList ul ON ul.LogTo = tb.LogTo AND ul.MachineUser = 0
  WHERE -- wi.ProjectId in ( 3, 11 ) AND
  tb.LogTo = '23kxcn6h3ard'
) agg
GROUP BY YMDH, LogTo
ORDER BY YMDH DESC;




exec web.GetWorkDone 'Martí';
GO
EXEC web.GetWorkDone 'Huda';
GO
exec web.GetWorkDone 'Ikram';
GO

SELECT * FROM Web.GetTextBlockDisagreements( 'es' );
EXEC WebJson.GetDisagreements 3, 'es';

exec web.GetWorkDone 'zxs29rog9jp4';
GO

exec Web.GetWorkBatch 3, '78120y9vpvpa', 'es', 'en'; -- Ikram spanish mono

exec Web.GetWorkBatch 3, '23kxcn6h3ard', 'es', 'en';  -- Marti
exec Web.GetWorkBatch 3, '78120y9vpvpa', 'es', 'en';  -- Ikram
exec Web.GetWorkBatch 3, 'lr2a649vjvg1', 'es', 'en';  -- Huda
GO


SELECT  * FROM dbo.TextBlock WHERE WorkId IN (8840, 13585);

SELECT * FROM dbo.TextApproved WHERE CheckSrc IN (1266600793,1019905265,73069054 ); -- 23 (Marti)

SELECT * FROM dbo.TextBlock WHERE CheckRaw IN ( -1631364822, 1073796622, -389274370 );

SELECT * FROM dbo.UserList;
SELECT * FROM dbo.WorkItem WHERE ProjectId = 4;
SELECT TOP 100 * FROM dbo.TextBlock ORDER BY BlockId DESC;
GO

EXEC Web.GetProjects 'lr2a649vjvg1';
EXEC Web.GetProjectStatus 'zxs29rog9jp4'; -- Magne
EXEC Web.GetProjectStatus 'lr2a649vjvg1'; -- Huda
EXEC Web.GetProjectStatus '23kxcn6h3ard'; -- Marti
EXEC Web.GetProjectStatus '78120y9vpvpa'; -- Ikram
GO

UPDATE dbo.UserList SET CoLogTo = NULL WHERE CoLogTo = 'ikram';
UPDATE dbo.UserList SET LogTo = '78120y9vpvpa' WHERE LogTo = 'ikram';

SELECT * FROM dbo.UserList;

UPDATE dbo.UserList SET CoLogTo = '78120y9vpvpa' WHERE UserId = 19;

SELECT TOP 1000 * FROM dbo.TextBlock ORDER BY BlockId DESC;

SELECT TOP 10 * FROM dbo.TextBlock ORDER BY BlockId DESC;
SELECT TOP 10 * FROM dbo.TextApproved ORDER BY ApprId DESC;

