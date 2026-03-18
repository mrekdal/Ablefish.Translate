CREATE PROCEDURE [Web].[ApproveFinalText]( @WorkId INT, @LogTo VARCHAR(16), @TargetLanguage VARCHAR(12), @CheckSrc INT, @FinalText NVARCHAR(MAX), @WithDoubt BIT = 0 ) AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @BlockId INT;
  DECLARE @ApprId INT;

  INSERT INTO dbo.ApproveClick( WorkId, LogTo, CheckSrc ) VALUES ( @WorkId, @LogTo, @CheckSrc );

  -- Verify that the row hasn't changed by using a checksum.

  IF NOT EXISTS ( SELECT 1 FROM dbo.WorkItem WHERE WorkId = @WorkId AND CheckSrc = @CheckSrc )
  BEGIN
    RAISERROR( 'The source text has changed. Checksum changed.', 16, 1 );
	RETURN;
  END;

  -- Update the TextBlock or create it if it doesn't exist.

  SELECT @BlockId = BlockId FROM dbo.TextBlock WHERE WorkId = @WorkId AND LangKey = @TargetLanguage AND LogTo = @LogTo AND CheckSrc = @CheckSrc;
  PRINT CONCAT( 'BlockId #',@BlockId);
  IF @BlockId IS NULL
    INSERT INTO dbo.TextBlock ( WorkId, LangKey, RawText, CheckSrc, LogTo, WithDoubt ) 
	VALUES ( @WorkId, @TargetLanguage, @FinalText, @CheckSrc, @LogTo, @WithDoubt );
  ELSE
    UPDATE dbo.TextBlock SET RawText = @FinalText, WithDoubt = @WithDoubt 
	WHERE BlockId = @BlockId AND CheckSrc = @CheckSrc AND ( ISNULL(RawText,'') <> ISNULL(@FinalText,'') OR WithDoubt <> @WithDoubt );

  -- Update the approval status or create it if it doesn't exist.

  SELECT @ApprId = ApprId FROM dbo.TextApproved WHERE WorkId = @WorkId AND LangTrg = @TargetLanguage AND CheckSrc = @CheckSrc;
  PRINT CONCAT( 'ApprId #',@ApprId);

  IF @ApprId IS NULL
    INSERT INTO dbo.TextApproved( WorkId, LangTrg, CheckSrc, CheckTrg, WithDoubt, LogTo ) 
	VALUES ( @WorkId, @TargetLanguage, @CheckSrc, CHECKSUM(@FinalText), @WithDoubt, @LogTo )
  ELSE
    UPDATE dbo.TextApproved 
	SET CheckTrg = CHECKSUM( @FinalText ), UpdatedAt = GETDATE(), WithDoubt = @WithDoubt, LogTo = @LogTo 
	WHERE ApprId = @ApprId AND ( CheckTrg <> CHECKSUM( @FinalText ) OR WithDoubt <> @WithDoubt );

END