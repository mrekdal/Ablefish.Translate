CREATE FUNCTION Web.GetApprovedTranslations( @ProjectId INT, @LangTrg VARCHAR(12) ) RETURNS TABLE AS RETURN
(
  SELECT wi.WorkId, wi.RowKey, tb.RawText, wi.CheckSrc, tb.CheckRaw,
         COUNT(*) AS Cnt, MIN(tb.LogTo) AS FirstLogTo, MAX(tb.LogTo) AS LastLogTo
  FROM dbo.WorkItem wi
  JOIN dbo.TextApproved ta
    ON ta.WorkId = wi.WorkId AND ta.CheckSrc = wi.CheckSrc
  JOIN dbo.TextBlock tb
    ON tb.WorkId = wi.WorkId AND tb.CheckRaw = ta.CheckTrg
  WHERE wi.ProjectId = @ProjectId AND ta.LangTrg = @LangTrg
  GROUP BY wi.WorkId, wi.RowKey, tb.RawText, wi.CheckSrc, tb.CheckRaw
);