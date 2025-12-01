CREATE PROCEDURE [dbo].[AddWorkItem]( @ProjectId INT, @RowKey VARCHAR(64), @RawText NVARCHAR(MAX) ) AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @WorkId INT;
  SELECT @WorkId = WorkId FROM dbo.WorkItem WHERE ProjectId = @ProjectId AND RowKey = @RowKey;
  IF @WorkId IS NULL
    INSERT INTO dbo.WorkItem( ProjectId, RowKey, RawText ) 
	VALUES ( @ProjectId, @RowKey, @RawText );
  ELSE
    UPDATE dbo.WorkItem SET RawText = @RawText, UpdatedAt = GETDATE() 
	WHERE WorkId = @WorkId AND ISNULL(RawText,'') COLLATE Latin1_General_CS_AS  <> ISNULL(@RawText,'') COLLATE Latin1_General_CS_AS;
END