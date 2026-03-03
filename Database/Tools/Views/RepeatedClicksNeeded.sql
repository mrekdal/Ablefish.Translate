CREATE VIEW Tools.RepeatedClicksNeeded AS
SELECT wi.ProjectId, ac.WorkId, ac.LogTo, ac.CheckSrc, wi.RowKey, COUNT(*) AS n
FROM dbo.ApproveClick ac
JOIN dbo.WorkItem wi ON wi.WorkId = ac.WorkId
WHERE wi.ProjectId <> 12
GROUP BY wi.ProjectId, ac.WorkId, ac.LogTo, ac.CheckSrc, wi.RowKey
HAVING COUNT(*) > 1;