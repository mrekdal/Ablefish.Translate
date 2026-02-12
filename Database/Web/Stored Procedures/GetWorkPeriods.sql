CREATE PROCEDURE [Web].[GetWorkPeriods]( @LogTo VARCHAR(16), @WinSize INT = 30 ) AS
BEGIN
    SET NOCOUNT ON;

    WITH OrderedLogs AS
    (
        SELECT
            ubl.CreatedAt,
            LAG(ubl.CreatedAt) OVER (ORDER BY ubl.CreatedAt) AS PrevCreatedAt
        FROM dbo.UserBatchLog ubl
		JOIN dbo.UserList ul ON ul.LogTo = ubl.LogTo
        WHERE ubl.LogTo = @LogTo OR ul.FirstName = @LogTo
    ),
    PeriodFlags AS
    (
        SELECT
            CreatedAt,
            CASE
                WHEN PrevCreatedAt IS NULL
                     OR DATEDIFF(MINUTE, PrevCreatedAt, CreatedAt) > @WinSize
                THEN 1
                ELSE 0
            END AS IsNewPeriod
        FROM OrderedLogs
    ),
    PeriodGroups AS
    (
        SELECT
            CreatedAt,
            SUM(IsNewPeriod) OVER (
                ORDER BY CreatedAt
                ROWS UNBOUNDED PRECEDING
            ) AS PeriodID
        FROM PeriodFlags
    )
    SELECT
        @LogTo AS UserName,
        PeriodID,
        MIN(CreatedAt) AS PeriodStart,
        MAX(CreatedAt) AS PeriodEnd,
        DATEDIFF(MINUTE, MIN(CreatedAt), MAX(CreatedAt)) + 2 AS DurationMinutes
    FROM PeriodGroups
    GROUP BY PeriodID
    ORDER BY PeriodStart;
END;