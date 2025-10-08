CREATE PROCEDURE WebAdmin.AddUser( @LogTo VARCHAR(16), @LangKey VARCHAR(12) ) AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.UserList( LogTo ) VALUES ( @LogTo );
  INSERT INTO dbo.UserTargetLanguage ( LogTo, LangKey ) VALUES ( @LogTo, @LangKey );
  INSERT INTO dbo.UserProject (LogTo,ProjectId) VALUES ( @LogTo, 4 );
END