CREATE PROCEDURE [dbo].[ApproveFinalText]( @WorkId INT, @LogTo VARCHAR(16), @TargetLanguage VARCHAR(12), @CheckSrc INT, @FinalText NVARCHAR(MAX) ) AS
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
    INSERT INTO dbo.TextBlock ( WorkId, LangKey, RawText, LogTo ) VALUES ( @WorkId, @TargetLanguage, @FinalText, @LogTo );
  ELSE
    UPDATE dbo.TextBlock SET RawText = @FinalText WHERE BlockId = @BlockId AND ISNULL(RawText,'') <> ISNULL(@FinalText,'');

  -- Update the approval status or create it if it doesn't exist.

  SELECT @ApprId = ApprId FROM dbo.TextApproved WHERE WorkId = @WorkId AND LangTrg = @TargetLanguage;
  IF @ApprId IS NULL
    INSERT INTO dbo.TextApproved( WorkId, LangTrg, CheckSrc, CheckTrg) 
	VALUES ( @WorkId, @TargetLanguage, @CheckSrc, CHECKSUM(@FinalText) )
  ELSE
    UPDATE dbo.TextApproved SET CheckSrc = @CheckSrc, CheckTrg = CHECKSUM(@FinalText), UpdatedAt = GETDATE()
	WHERE ApprId = @ApprId AND CheckTrg <> CHECKSUM(@FinalText);

END