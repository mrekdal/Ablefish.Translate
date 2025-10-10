CREATE VIEW dbo.TextBlockManual AS
SELECT tb.*
FROM dbo.TextBlock tb
JOIN dbo.UserList ul ON ul.LogTo = tb.LogTo AND ul.MachineUser = 0;