CREATE PROCEDURE [Tools].[GetChangedWorkItems]( @ProjectId INT, @StartAt DATETIME ) AS
BEGIN
  WITH Versions AS
  (
    SELECT
        WorkId,
        RawText,
        ValidFrom,
        ValidTo,
        LAG(RawText) OVER (
            PARTITION BY WorkId
            ORDER BY ValidFrom
        ) AS OldRawText
    FROM dbo.WorkItem
	FOR SYSTEM_TIME ALL
	WHERE ProjectId = @ProjectId
  )
  SELECT
    WorkId,
    OldRawText,
    RawText AS NewRawText,
    ValidFrom AS ChangedAt
  FROM Versions
  WHERE OldRawText IS NOT NULL
    AND OldRawText <> RawText
    AND ValidFrom >= @StartAt
  ORDER BY ChangedAt DESC;
END