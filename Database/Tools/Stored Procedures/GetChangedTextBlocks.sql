CREATE PROCEDURE [Tools].[GetChangedTextBlocks]( @ProjectId INT, @StartAt DATETIME ) AS
BEGIN
  WITH Versions AS
  (
    SELECT
        tb.BlockId,
        tb.RawText,
        tb.ValidFrom,
        tb.ValidTo,
        LAG(tb.RawText) OVER (
            PARTITION BY tb.BlockId
            ORDER BY tb.ValidFrom
        ) AS OldRawText
    FROM dbo.TextBlock FOR SYSTEM_TIME ALL AS tb
	JOIN dbo.WorkItem wi ON wi.WorkId = tb.WorkId
	WHERE wi.ProjectId = @ProjectId
  )
  SELECT
    BlockId,
    OldRawText,
    RawText AS NewRawText,
    ValidFrom AS ChangedAt
  FROM Versions
  WHERE OldRawText IS NOT NULL
    AND OldRawText <> RawText
    AND ValidFrom >= @StartAt
  ORDER BY ChangedAt DESC;
END