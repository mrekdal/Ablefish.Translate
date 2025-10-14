CREATE FUNCTION [Web].[GetTextBlockDisagreements]( @LangKey VARCHAR(12) ) RETURNS TABLE AS
RETURN
(
    SELECT *
    FROM
    (
        SELECT 
            tb.WorkId,
            tb.LangKey,
            MIN(tb.CheckRaw) AS MinCheck,
            MAX(tb.CheckRaw) AS MaxCheck,
            COUNT(*) AS n,
            MIN(tb.LogTo) AS MinLogTo,
            MAX(tb.LogTo) AS MaxLogTo
        FROM dbo.TextBlock tb
        INNER JOIN dbo.UserList ul 
            ON ul.LogTo = tb.LogTo
            AND ul.MachineUser = 0
		WHERE tb.LangKey = @LangKey AND tb.IsDiscarded = 0
        GROUP BY tb.WorkId, tb.LangKey
        HAVING COUNT(*) > 1
    ) agg
    WHERE agg.MinCheck <> agg.MaxCheck
);