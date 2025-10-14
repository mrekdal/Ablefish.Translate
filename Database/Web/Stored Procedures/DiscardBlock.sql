CREATE PROCEDURE [Web].[DiscardBlock]( @BlockId INT, @DiscardedBy VARCHAR(16) ) AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @CanDiscard BIT;
  SELECT @CanDiscard = ISNULL(utl.CanApprove,0)
  FROM dbo.UserTargetLanguage utl
  JOIN dbo.TextBlock tb ON tb.BlockId = @BlockId AND utl.LangKey = tb.LangKey
  WHERE utl.LogTo = @DiscardedBy;
  IF @CanDiscard = 0  
    RAISERROR( 'You are not authorized to approve or discard this text block in this language.', 16, 1 )
  ELSE 
    UPDATE dbo.TextBlock 
    SET IsDiscarded = 1, DiscardedBy = @DiscardedBy 
    WHERE BlockId = @BlockId AND IsDiscarded = 0;
END