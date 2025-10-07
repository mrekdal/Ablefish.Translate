CREATE PROCEDURE [dbo].[GetWorkList]( @ProjectId INT, @LangKey VARCHAR(12), @LogTo VARCHAR(16) = 'DeepL' ) AS
BEGIN
  SELECT wi.RowKey, wi.RawText AS SrcText, tb.RawText, p.LangKey AS LangCode, tb.LogTo
  FROM dbo.WorkItem wi
    JOIN dbo.Project p ON p.ProjectId = wi.ProjectId
    LEFT JOIN dbo.TextBlock tb ON tb.WorkId = wi.WorkId AND tb.LangKey = @LangKey AND tb.LogTo = @LogTo
  WHERE p.ProjectId = @ProjectId
    AND tb.RawText IS NULL
  ORDER BY wi.RowKey;
END