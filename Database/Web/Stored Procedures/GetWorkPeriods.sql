CREATE PROCEDURE [Web].[GetWorkPeriods]( @LogTo VARCHAR(16), @WinSize INT = 30 ) AS
BEGIN
    SET NOCOUNT ON;

    WITH OrderedLogs AS
    (

        SELECT ubl.BoundAt AS CreatedAt, LAG(ubl.BoundAt) OVER (ORDER BY ubl.BoundAt) AS PrevCreatedAt
        FROM Tools.GetUserWorkPeriods( @LogTo ) ubl
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
        MIN(DATEADD(HH,1,CreatedAt)) AS PeriodStart,
        MAX(DATEADD(HH,1,CreatedAt)) AS PeriodEnd,
        DATEDIFF(MINUTE, MIN(CreatedAt), MAX(CreatedAt)) + 2 AS DurationMinutes
    FROM PeriodGroups
    GROUP BY PeriodID
    ORDER BY PeriodStart;
END;