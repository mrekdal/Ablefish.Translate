

UPDATE dbo.TextBlock SET RawText = REPLACE( RawText, '®', '�' ) WHERE CHARINDEX( '®', RawText) > 0; 
UPDATE dbo.TextBlock SET RawText = REPLACE( RawText, 'ó', '�' ) WHERE CHARINDEX( 'ó', RawText) > 0; 
UPDATE dbo.TextBlock SET RawText = REPLACE( RawText, 'á', '�' ) WHERE CHARINDEX( 'á', RawText) > 0; 



UPDATE dbo.WorkItem SET RawText = REPLACE( RawText, '®', '�' ) WHERE CHARINDEX( '®', RawText) > 0; 