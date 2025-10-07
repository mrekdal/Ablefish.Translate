
 CREATE PROCEDURE Web.GetTargets( @LogTo VARCHAR(16) ) AS
 BEGIN
   SELECT utl.LogTo, utl.LangKey, ul.FirstName, tl.EnglishName, tl.ShortName 
   FROM dbo.UserTargetLanguage utl 
   JOIN dbo.UserList ul ON ul.LogTo = utl.LogTo 
   JOIN dbo.TextLanguage tl ON tl.LangKey = utl.LangKey
  WHERE utl.LogTo = @LogTo
  ORDER BY FirstName, EnglishName;
 END