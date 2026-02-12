CREATE PROCEDURE Web.GetWorkBatches( @LogTo VARCHAR(16) ) AS
BEGIN
  SELECT ubl.*, ul.FirstName 
  FROM dbo.UserBatchLog ubl
  JOIN dbo.UserList ul ON ul.LogTo = ubl.LogTo AND ( ul.LogTo = @LogTo OR ul.FirstName = @LogTo )
  ORDER BY BatchId DESC;
END