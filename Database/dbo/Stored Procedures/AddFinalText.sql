CREATE PROCEDURE [dbo].[AddFinalText]( @ProjectId INT, @RowKey VARCHAR(64), @LangKey VARCHAR(12), @FinalText NVARCHAR(MAX) ) AS
BEGIN
  DECLARE @WorkId INT;
  DECLARE @RowId INT;
  SET NOCOUNT ON;
  SELECT @WorkId = WorkId FROM dbo.WorkItem WHERE ProjectId = @ProjectId AND RowKey = @RowKey;
  SELECT @RowId = RowId FROM dbo.TextBlock WHERE WorkId = @WorkId AND LangKey = @LangKey AND LogTo = 'final';
  IF @RowId IS NULL
    INSERT INTO dbo.TextBlock( WorkId, LangKey, LogTo, RawText ) VALUES ( @WorkId, @LangKey, 'final', @FinalText );
  ELSE
    UPDATE dbo.TextBlock SET RawText = @FinalText WHERE RowId = @RowId AND ISNULL(RawText, '' ) <> ISNULL(@FinalText,'');
END