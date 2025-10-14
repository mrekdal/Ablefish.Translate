CREATE PROCEDURE [WebJson].[GetDisagreements]( @ProjectId INT, @LangKey VARCHAR(12) ) AS
BEGIN
  DECLARE @jsonResult NVARCHAR(MAX);
  SET @jsonResult = 
  ( 
    SELECT wi.WorkId, wi.RowKey, wi.RawText AS SrcText,
      (
        SELECT tb.RawText, ul.UserName, tb.BlockId, tb.CheckRaw
        FROM dbo.TextBlockManual tb
        JOIN dbo.UserList ul ON ul.LogTo = tb.LogTo
        WHERE tb.WorkId = wi.WorkId AND tb.LangKey = @LangKey
        FOR JSON PATH
      ) AS Candidate
    FROM Web.GetTextBlockDisagreements(@LangKey) AS tbd
    JOIN dbo.WorkItem wi ON wi.WorkId = tbd.WorkId AND wi.ProjectId = @ProjectId
    ORDER BY wi.WorkId
    FOR JSON PATH, ROOT('Items')
  );
  SELECT @jsonResult;
END