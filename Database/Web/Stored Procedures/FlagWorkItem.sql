CREATE PROCEDURE Web.FlagWorkItem( @WorkId INT, @FlaggedBy VARCHAR(16) ) AS 
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.WorkItem SET Flagged = 1, FlaggedBy = @FlaggedBy 
  WHERE WorkId = @WorkId;
END