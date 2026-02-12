CREATE PROCEDURE [Web].[GetUserFromLogTo] ( @LogTo VARCHAR(16) ) AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @IsActive BIT;
  SELECT @IsActive = IsActive FROM dbo.UserList WHERE LogTo = @LogTo;
  IF @IsActive IS NULL
    RAISERROR ( 'There is no such user.', 16, 1 );
  ELSE IF @IsActive = 0
    RAISERROR ( 'This user has been deactivated.', 16, 1 );
  ELSE 
  BEGIN
    INSERT INTO dbo.UserLog( LogTo ) VALUES( @LogTo );
    SELECT * FROM dbo.UserList WHERE LogTo = @LogTo;
  END;
END