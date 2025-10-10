CREATE PROCEDURE Web.GetAllProjects( @LogTo VARCHAR(16) = NULL ) AS
BEGIN
  SELECT ul.LogTo, ul.FirstName, p.ProjectId, tl.LangKey, p.ProjectName 
  FROM dbo.UserList ul
  JOIN dbo.UserProject up ON up.LogTo = ul.LogTo AND up.IsActive = 1
  JOIN dbo.Project p ON p.ProjectId = up.ProjectId AND p.IsActive = 1
  JOIN dbo.UserTargetLanguage tl ON tl.LogTo = ul.LogTo AND tl.IsActive = 1
  WHERE ul.LogTo = @LogTo OR @LogTo IS NULL
  ORDER BY p.ProjectId, ul.FirstName
END