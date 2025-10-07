CREATE PROCEDURE [dbo].[AddTextBlock]( @WorkId INT, @LangKey VARCHAR(12), @RawText NVARCHAR(MAX), @LogTo VARCHAR(16) ) AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @RowId INT;
  DECLARE @CheckSum INT;
  SELECT @RowId = RowId FROM dbo.TextBlock WHERE WorkId = @WorkId AND LangKey = @LangKey AND LogTo = @LogTo;
  IF @RowId IS NULL
    INSERT INTO dbo.TextBlock( WorkId, LangKey, RawText, LogTo ) VALUES ( @WorkId, @LangKey, @RawText, @LogTo );
  ELSE 
    UPDATE dbo.TextBlock SET RawText = @RawText WHERE RowId = @RowId AND ISNULL(RawText,'') <> ISNULL(@RawText,'');
END