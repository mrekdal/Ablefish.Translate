CREATE PROCEDURE [dbo].[AddTextBlock]( @WorkId INT, @CheckSrc INT, @LangKey VARCHAR(12), @RawText NVARCHAR(MAX), @LogTo VARCHAR(16) ) AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @BlockId INT;
  DECLARE @CheckSum INT;
  SELECT @BlockId = BlockId FROM dbo.TextBlock WHERE WorkId = @WorkId AND CheckSrc = @CheckSrc AND LangKey = @LangKey AND LogTo = @LogTo;
  IF @BlockId IS NULL
    INSERT INTO dbo.TextBlock( WorkId, CheckSrc, LangKey, RawText, LogTo ) VALUES ( @WorkId, @CheckSrc, @LangKey, @RawText, @LogTo );
  ELSE 
    UPDATE dbo.TextBlock SET RawText = @RawText WHERE BlockId = @BlockId AND ISNULL(RawText,'') <> ISNULL(@RawText,'');
END