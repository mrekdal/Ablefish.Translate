CREATE FUNCTION [Web].[GetTextBlockDisagreements]( @ProjectId INT, @LangKey VARCHAR(12) ) RETURNS TABLE AS
RETURN
(
    SELECT tb.WorkId, @LangKey AS LangKey, 
	  MIN(tb.CheckRaw) AS MinCheck, MAX(tb.CheckRaw) AS MaxCheck, MIN(tb.LogTo) AS MinLogTo, MAX(tb.LogTo) AS MaxLogTo, COUNT(*) AS n
    FROM dbo.TextBlock tb
	JOIN dbo.WorkItem wi ON wi.WorkId = tb.WorkId AND wi.CheckSrc = tb.CheckSrc -- Limit to active originals
    JOIN dbo.UserList ul ON ul.LogTo = tb.LogTo AND ul.MachineUser = 0 -- Limit to non-machine users.
    WHERE wi.ProjectId = @ProjectId AND tb.LangKey = @LangKey AND tb.IsDiscarded = 0 -- Exclude discarded text blocks
    GROUP BY tb.WorkId
    HAVING COUNT(*) > 1 AND MIN(tb.CheckRaw) <> MAX(tb.CheckRaw)
);