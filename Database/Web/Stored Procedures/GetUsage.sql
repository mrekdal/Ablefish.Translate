CREATE PROCEDURE Web.GetUsage AS 
BEGIN
  SELECT ul.UserId, ul.Email, ul.LogTo, ul.FirstName, 
    COUNT(*) AS n, MAX(lg.CreatedAt) AS LastLogin, MIN(lg.CreatedAt) AS FirstLogin
  FROM dbo.UserLog lg
  JOIN dbo.UserList ul ON ul.LogTo = lg.LogTo
  GROUP BY ul.UserId, ul.Email, ul.LogTo, ul.FirstName
  ORDER BY MAX(lg.CreatedAt) DESC;
END