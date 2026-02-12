SELECT *, ABS( LenMin - LenMax )  AS LenDiff FROM 
( SELECT wi.WorkId, wi.RowKey, COUNT(*) AS n, 
  MAX(LEN(wi.RawText)) AS LenOrig, 
  MIN(LEN(tb.RawText)) AS LenMin, MAX(LEN(tb.RawText)) AS LenMax
FROM dbo.TextBlock tb
JOIN dbo.WorkItem wi ON wi.WorkId = tb.WorkId AND wi.ProjectId = 3
JOIN dbo.UserList ul ON ul.LogTo = tb.LogTo AND ul.FirstName = 'Huda'
GROUP BY wi.WorkId, wi.RowKey
HAVING COUNT(*) > 1) agg
ORDER BY ABS( LenMin - LenMax ) DESC;

SELECT * FROM dbo.TextBlock WHERE WorkId = 8871; 

Compuesto de amonio cuaternario. Carga hepática probablemente insignificante. Una referencia: uso
Compuesto de amonio cuaternario. Probablemente carga hepática insignificante.


SELECT * FROM dbo.TextBlock WHERE WorkId = 8883 AND LogTo = 'lr2a649vjvg1';

Carga metabólica sistémica probablemente insignificante. Una referencia: uso seguro.
Carga metabólica sistémica probablemente insignificante.


SELECT * FROM dbo.TextBlock WHERE WorkId = 8881 AND LogTo = 'lr2a649vjvg1';

Sustrato para CYPs 3A4 y 3A5. No hay datos que apunten a una inducción clínica del CYP. Una referencia: autorizado.
Sustrato para CYPs 3A4 y 3A5. No hay datos que apunten a una inducción clínica del CYP.


SELECT * FROM dbo.TextBlock WHERE WorkId = 13623 AND LogTo = 'lr2a649vjvg1';

Pomada dermatológica utilizada en ulceras y vulnera infectadas. Oxitetraciclina: Antibiótico de amplio espectro con alquilaminogrupo. Polimixina B: Sin datos sobre absorción o metabolismo.
Pomada dermatológica utilizada en úlceras y vulceras infectadas. Oxitetraciclina : Antibiótico de amplio espectro con alquilaminogrupo. Polimixina B: Sin datos sobre absorción o metabolismo. Oxitetraciclina : Lista sudafricana: utilizar sólo con extrema precaución. Tschudy y Lamon J1980: inseguro.