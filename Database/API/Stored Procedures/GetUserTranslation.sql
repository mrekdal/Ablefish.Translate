CREATE PROCEDURE API.GetUserTranslation( @ProjectId INT, @LangKey VARCHAR(12), @LogTo VARCHAR(16) ) AS
BEGIN
  SELECT wi.RowKey, wi.RawText AS TextOriginal, tbdl.RawText AS TextResX, tbni.RawText AS TextTranslated, IIF(tbdl.RawText <> tbni.RawText, 1, 0) AS Changed,  tbni.LogTo
  FROM dbo.WorkItem  wi
  LEFT JOIN dbo.TextBlock tbni ON tbni.WorkId = wi.WorkId AND tbni.LangKey = @LangKey AND tbni.LogTo = @LogTo
  LEFT JOIN dbo.TextBlock tbdl ON tbdl.WorkId = wi.WorkId AND tbdl.LangKey = @LangKey AND tbdl.LogTo = 'ResX'
  WHERE wi.ProjectId = @ProjectId AND wi.InUse = 1
  ORDER BY wi.RowKey;
END