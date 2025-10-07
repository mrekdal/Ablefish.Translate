 CREATE PROCEDURE [Web].[GetProjects]( @LogTo VARCHAR(16) ) AS
 BEGIN
  SELECT up.*, p.ProjectName, ul.FirstName
  FROM dbo.UserProject up
  JOIN dbo.Project p ON p.ProjectId = up.ProjectId
  JOIN dbo.UserList ul ON ul.LogTo = up.LogTo
  WHERE @LogTo = up.LogTo;
 END