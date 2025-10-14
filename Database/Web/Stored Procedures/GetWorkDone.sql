CREATE PROCEDURE [Web].[GetWorkDone]( @LogTo VARCHAR(16) = NULL ) AS 
BEGIN
  SELECT tb.LangKey, ul.FirstName, ul.LogTo, COUNT(*) AS n
  FROM dbo.TextBlock tb
  JOIN dbo.UserList ul ON ul.LogTo = tb.LogTo AND ul.MachineUser = 0
  WHERE ISNULL(@LogTo,ul.LogTo) = ul.LogTo
  GROUP BY ul.LogTo, ul.FirstName, tb.LangKey
  ORDER by tb.LangKey, ul.FirstName
END