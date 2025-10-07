CREATE PROCEDURE [Web].[GetUserFromLogTo] ( @LogTo VARCHAR(16) ) AS
BEGIN
  SET NOCOUNT ON;
  SELECT* FROM dbo.UserList WHERE LogTo = @LogTo;
  INSERT INTO dbo.UserLog( LogTo ) VALUES( @LogTo );
END