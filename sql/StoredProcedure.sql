GO
CREATE PROCEDURE GetEndedMatchInDay
	@dtStamp DATETIME = NULL
AS
	SET @dtStamp = ISNULL(@dtStamp, GETDATE())

	SELECT * FROM S_Match 
	WHERE idEvent IN (SELECT idEvent FROM G_Quiz WHERE idStatus <> -1)
	AND startDate IS NOT NULL AND endDate IS NOT NULL
	AND DATEDIFF(HOUR, endDate, @dtStamp) BETWEEN 0 AND 24
GO

GO
CREATE PROCEDURE GetGamerTrendingByDay
	@idEvent INT = 0,
	@idGamer INT = 0
AS
	SELECT CAST(t.dtResult AS date) dtResult, ISNULL(point, 0) point,
	SUM(ISNULL(point, 0)) OVER(ORDER BY CAST(t.dtResult AS date) ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS cumulPoint
	FROM (
		SELECT distinct CAST(dtResult as date) dtResult from G_Predict
		WHERE idEvent = @idEvent AND dtResult is not null
	) t
	LEFT JOIN (
		SELECT CAST(dtResult as date) dtResult, idEvent, idGamer, SUM(point) point
		FROM G_Predict
		WHERE idEvent = @idEvent AND dtResult is not null
		GROUP BY CAST(dtResult as date), idEvent, idGamer
	) p ON CAST(p.dtResult as date) = t.dtResult AND idEvent = @idEvent AND idGamer = @idGamer
	ORDER BY CAST(t.dtResult AS date)
GO