CREATE PROCEDURE [dbo].[AddTextBlockRowKey]( @ProjectId INT, @RowKey VARCHAR(64), @LangKey VARCHAR(12), @RawText NVARCHAR(MAX), @LogTo VARCHAR(16) ) AS
BEGIN

  SET NOCOUNT ON;

  -- Find all relevant WorkIds.

  SELECT w2.WorkId 
  INTO #t
  FROM dbo.WorkItem wi
  JOIN dbo.WorkItem w2 ON w2.ProjectId = wi.ProjectId AND w2.RawText = wi.RawText AND w2.CheckSrc = wi.CheckSrc
  WHERE wi.ProjectId = @ProjectId AND wi.RowKey = @RowKey;

  -- Merge into TextBlock

  MERGE INTO dbo.TextBlock AS trg
  USING ( SELECT WorkId FROM #t ) AS src
  ON src.WorkId = trg.WorkId AND trg.LangKey = @LangKey AND trg.LogTo = @LogTo
  WHEN MATCHED THEN
    UPDATE SET trg.RawText = @RawText
  WHEN NOT MATCHED THEN
    INSERT ( WorkId, LangKey, RawText, LogTo ) VALUES ( src.WorkId, @LangKey, @RawText, @LogTo );

END