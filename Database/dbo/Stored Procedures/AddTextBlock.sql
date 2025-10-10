CREATE PROCEDURE [dbo].[AddTextBlock]( @WorkId INT, @LangKey VARCHAR(12), @RawText NVARCHAR(MAX), @LogTo VARCHAR(16) ) AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @BlockId INT;
  DECLARE @CheckSum INT;
  SELECT @BlockId = BlockId FROM dbo.TextBlock WHERE WorkId = @WorkId AND LangKey = @LangKey AND LogTo = @LogTo;
  IF @BlockId IS NULL
    INSERT INTO dbo.TextBlock( WorkId, LangKey, RawText, LogTo ) VALUES ( @WorkId, @LangKey, @RawText, @LogTo );
  ELSE 
    UPDATE dbo.TextBlock SET RawText = @RawText WHERE BlockId = @BlockId AND ISNULL(RawText,'') <> ISNULL(@RawText,'');
END