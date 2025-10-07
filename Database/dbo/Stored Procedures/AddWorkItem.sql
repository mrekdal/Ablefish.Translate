CREATE PROCEDURE dbo.AddWorkItem( @ProjectId INT, @RowKey VARCHAR(64), @RawText NVARCHAR(MAX) ) AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.WorkItem( ProjectId, RowKey, RawText ) VALUES ( @ProjectId, @RowKey, @RawText );
END