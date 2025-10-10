 CREATE PROCEDURE [Web].[GetProjects]( @LogTo VARCHAR(16) ) AS
 BEGIN
  SELECT up.*, p.ProjectName, p.ShortName, ul.FirstName
  FROM dbo.UserProject up
  JOIN dbo.Project p ON p.ProjectId = up.ProjectId AND p.IsActive = 1
  JOIN dbo.UserList ul ON ul.LogTo = up.LogTo
  WHERE @LogTo = up.LogTo AND up.IsActive = 1;
 END