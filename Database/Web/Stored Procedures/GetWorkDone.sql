CREATE PROCEDURE Web.GetWorkDone AS 
BEGIN
  SELECT ul.LogTo, ul.FirstName,ul.TargetLanguage, COUNT(*) AS n
  FROM dbo.TextBlock tb
  JOIN dbo.UserList ul ON ul.LogTo = tb.LogTo AND ul.MachineUser = 0
  GROUP BY ul.LogTo, ul.FirstName, ul.TargetLanguage
  ORDER by ul.FirstName;
END