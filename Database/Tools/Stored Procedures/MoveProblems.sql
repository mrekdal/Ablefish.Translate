CREATE PROCEDURE Tools.MoveProblems AS
BEGIN
  UPDATE dbo.WorkItem SET ProjectId = 12
  WHERE WorkId IN 
  (
    SELECT ac.WorkId
    FROM dbo.ApproveClick ac
    JOIN dbo.WorkItem wi ON wi.WorkId = ac.WorkId
    WHERE wi.ProjectId = 3
    GROUP BY ac.WorkId, wi.RowKey, ac.CheckSrc, ac.LogTo
    HAVING COUNT(*) > 2
  )
END;