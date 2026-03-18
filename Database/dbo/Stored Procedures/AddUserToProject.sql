CREATE PROCEDURE dbo.AddUserToProject( @ProjectId INT, @LogTo VARCHAR(16) ) AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.UserProject( ProjectId, LogTo ) VALUES ( @ProjectId, @LogTo );
END