CREATE PROCEDURE [dbo].[AddResourceText]( @ProjectId INT, @RowKey VARCHAR(64), @LangKey VARCHAR(12), @RawText NVARCHAR(MAX) ) AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @WorkId INT;
  DECLARE @CheckSrc INT;
  IF @LangKey = 'en-GB' 
    EXEC dbo.AddWorkItem @ProjectId, @RowKey, @RawText;
  ELSE BEGIN
    SELECT @WorkId = WorkId, @CheckSrc = CheckSrc FROM dbo.WorkItem WHERE ProjectId = @ProjectId AND RowKey = @RowKey;
	EXEC dbo.AddTextBlockRowKey @ProjectId, @RowKey, @LangKey, @RawText, @CheckSrc, 'ResX';
  END;
END