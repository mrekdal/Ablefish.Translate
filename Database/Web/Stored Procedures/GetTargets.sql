 CREATE PROCEDURE [Web].[GetTargets]( @LogTo VARCHAR(16) ) AS
 BEGIN
   SELECT utl.*, tl.EnglishName, tl.ShortName, ul.FirstName 
   FROM dbo.UserTargetLanguage utl 
   JOIN dbo.UserList ul ON ul.LogTo = utl.LogTo 
   JOIN dbo.TextLanguage tl ON tl.LangKey = utl.LangKey
  WHERE utl.LogTo = @LogTo AND utl.IsActive = 1
  ORDER BY FirstName, EnglishName;
 END