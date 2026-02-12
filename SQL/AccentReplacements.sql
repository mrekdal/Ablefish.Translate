SELECT tb.*, wi.RowKey, wi.RawText 
FROM dbo.TextBlock tb
JOIN dbo.WorkItem wi ON wi.WorkId = tb.WorkId
WHERE ( CHARINDEX('Â', tb.RawText) > 0  ) AND tb.LangKey = 'es' AND NOT tb.LogTo IN ( 'DeepL', 'Azure' );
GO
CREATE OR ALTER PROCEDURE dbo.Utf8Fixes AS
BEGIN
    SET NOCOUNT ON;
    -- Table of corrupted text → correct replacement
    DECLARE @Fixes TABLE ( Corrupted NVARCHAR(50), Correct NVARCHAR(50) );
    -- Insert all replacements
    INSERT INTO @Fixes (Corrupted, Correct)
    VALUES
        (' Â®', '®'),
        ('âˆ´', '′'),
        ('Ã¡', 'á'),
        ('Â´', '´'),
        ('Ã¸', 'ö'),
        ('â¤"', '—'),
        ('Ã¤', 'ä'),
        ('â€"', '–'),
        ('â€¦', '…'),
        ('â€™', '’'),
        ('Â±', '±'),
        ('Ã±', 'ñ'),
        ('Ã©', 'é'),
        ('Â®', '®'),
        ('Âµ', 'µ'),
        ('Ã¶', 'ö'),
        ('â†´', '’'),
        ('â‰', '≈'),
        ('Ã¼', 'ü'),
        ('Ã³', 'ó'),
        ('Ãº', 'ú'),
        ('Î´', 'δ'),
        ('Î±', 'α'),
        ('Î¼', 'µ'),
        ('Î²', 'β'),
        ('Î³', 'γ'),
        ('Îº', 'κ'),
        ('proteÃnas', 'proteínas'),
        ('porfÃricos', 'porfíricos'),
        ('podrÃa', 'podría'),
        ('disminuÃa', 'disminuía'),
        ('producÃa', 'producía'),
        ('lÃquido', 'líquido'),
        ('deberÃa', 'debería'),
        ('VelÃk', 'Velík'),
        ('hÃgado', 'hígado'),
        ('lidocaÃna', 'lidocaína'),
        ('estadÃsticamente', 'estadísticamente'),
        ('tenÃa', 'tenía'),
        ('clÃnico', 'clínico'),
        ('clÃnica', 'clínica');

    DECLARE @corrupted NVARCHAR(50), @correct NVARCHAR(50);

    DECLARE FixCursor CURSOR LOCAL FAST_FORWARD FOR SELECT Corrupted, Correct FROM @Fixes;

    OPEN FixCursor;
    FETCH NEXT FROM FixCursor INTO @corrupted, @correct;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        UPDATE dbo.TextBlock
        SET RawText = REPLACE(RawText, @corrupted, @correct)
        WHERE CHARINDEX(@corrupted, RawText) > 0;

        FETCH NEXT FROM FixCursor INTO @corrupted, @correct;
    END
    CLOSE FixCursor;
    DEALLOCATE FixCursor;

END
GO


-- Phrase replacement
UPDATE dbo.TextBlock SET RawText = REPLACE( RawText, 'referencia: usar.', 'referencia: seguro al uso.' ) WHERE CHARINDEX( 'referencia: usar.', RawText ) > 0 AND Logto <> 'DeepL';
UPDATE dbo.TextBlock SET RawText = REPLACE( RawText, 'utilizar con precaución', 'usar con precaución' ) WHERE CHARINDEX( 'utilizar con precaución', RawText ) > 0 AND Logto <> 'DeepL';
GO

