CREATE PROCEDURE [dbo].[ApproveFinalText]( @WorkId INT, @LogTo VARCHAR(16), @TargetLanguage VARCHAR(12), @CheckSrc INT, @FinalText NVARCHAR(MAX), @WithDoubt BIT = 0 ) AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @BlockId INT;
  DECLARE @ApprId INT;

  -- Verify that the row hasn't changed by using a checksum.

  IF NOT EXISTS ( SELECT 1 FROM dbo.WorkItem WHERE WorkId = @WorkId AND CheckSrc = @CheckSrc )
  BEGIN
    RAISERROR( 'The source text has changed. Checksum changed.', 16, 1 );
	RETURN;
  END;

  -- Update the TextBlock or create it if it doesn't exist.

  SELECT @BlockId = BlockId FROM dbo.TextBlock WHERE WorkId = @WorkId AND LangKey = @TargetLanguage AND LogTo = @LogTo;
  IF @BlockId IS NULL
    INSERT INTO dbo.TextBlock ( WorkId, LangKey, RawText, LogTo, WithDoubt ) 
	VALUES ( @WorkId, @TargetLanguage, @FinalText, @LogTo, @WithDoubt );
  ELSE
    UPDATE dbo.TextBlock SET RawText = @FinalText, WithDoubt = @WithDoubt 
	WHERE BlockId = @BlockId AND ( ISNULL(RawText,'') <> ISNULL(@FinalText,'') OR WithDoubt <> @WithDoubt );

  -- Update the approval status or create it if it doesn't exist.

  SELECT @ApprId = ApprId FROM dbo.TextApproved WHERE WorkId = @WorkId AND LangTrg = @TargetLanguage AND CheckSrc = @CheckSrc;

  IF @ApprId IS NULL
    INSERT INTO dbo.TextApproved( WorkId, LangTrg, CheckSrc, CheckTrg, WithDoubt ) 
	VALUES ( @WorkId, @TargetLanguage, @CheckSrc, CHECKSUM(@FinalText), @WithDoubt )
  ELSE
    UPDATE dbo.TextApproved 
	SET CheckTrg = CHECKSUM( @FinalText ), UpdatedAt = GETDATE(), WithDoubt = @WithDoubt 
	WHERE ApprId = @ApprId AND ( CheckTrg <> CHECKSUM( @FinalText ) OR WithDoubt <> @WithDoubt );

END