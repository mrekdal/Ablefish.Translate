select * from dbo.UserList;
select *  from dbo.Project;

select wi.WorkId, wi.RowKey, wi.RawText, tbh.RawText as HudaText, tbm.RawText as MartiText
from dbo.WorkItem wi 
join dbo.TextBlock tbh ON tbh.WorkId = wi.WorkId
join dbo.TextBlock tbm ON tbm.WorkId = wi.WorkId
JOIN dbo.UserList uld ON uld.LogTo = tbd.LogTo AND ulh.FirstName = 'DeepL'
JOIN dbo.UserList ulh ON ulh.LogTo = tbh.LogTo AND ulh.FirstName = 'Huda'
JOIN dbo.UserList ulm ON ulm.LogTo = tbm.LogTo AND ulm.FirstName = 'Martí'
WHERE wi.ProjectId = 3 
  AND tbh.RawText <> tbm.RawText
ORDER BY tbh.RawText;




UPDATE dbo.TextBlock SET RawText = REPLACE( RawText, 'Uso no satisfactorio notificado', 'Uso sin complicaciones notificado' )
WHERE CHARINDEX ( 'Uso no satisfactorio notificado', RawText ) > 0
AND LogTo in (  'lr2a649vjvg1', '23kxcn6h3ard' );

UPDATE dbo.TextBlock SET RawText = REPLACE( RawText, 'Uso sin incindencias notificado', 'Uso sin complicaciones notificado' )
WHERE CHARINDEX ( 'Uso sin incindencias notificado', RawText ) > 0
AND LogTo in (  'lr2a649vjvg1', '23kxcn6h3ard' );

UPDATE dbo.TextBlock SET RawText = REPLACE( RawText, 'Uso sin incidencias notificado', 'Uso sin complicaciones notificado' )
WHERE CHARINDEX ( 'Uso sin incidencias notificado', RawText ) > 0
AND LogTo in (  'lr2a649vjvg1', '23kxcn6h3ard' );

UPDATE dbo.TextBlock SET RawText = REPLACE( RawText, 'Uso sin incidentes notificado', 'Uso sin complicaciones notificado' )
WHERE CHARINDEX ( 'Uso sin incidentes notificado', RawText ) > 0
AND LogTo in (  'lr2a649vjvg1', '23kxcn6h3ard' );

UPDATE dbo.TextBlock SET RawText = REPLACE( RawText, 'Uso sin incidentes reportado', 'Uso sin complicaciones notificado' )
WHERE CHARINDEX ( 'Uso sin incidentes reportado', RawText ) > 0
AND LogTo in (  'lr2a649vjvg1', '23kxcn6h3ard' );

UPDATE dbo.TextBlock SET RawText = 'Ninguno'
 WHERE RawText IN ( 'ninguna', 'ninguno', 'ninguno.', 'ninguna.' )
AND LogTo in (  'lr2a649vjvg1', '23kxcn6h3ard' )
AND WorkId IN ( SELECT WorkId FROM dbo.WorkItem WHERE RowKey LIKE 'Mono%Preclinical' );

UPDATE dbo.TextBlock SET RawText = 'Ninguno'
 WHERE RawText IN ( 'ninguna', 'ninguno', 'ninguno.', 'ninguna.', 'NOne' )
AND LogTo in (  'lr2a649vjvg1', '23kxcn6h3ard' )
AND WorkId IN ( SELECT WorkId FROM dbo.WorkItem WHERE RowKey LIKE 'Mono%Published' );
