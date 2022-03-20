CREATE PROCEDURE DeleteEventQuiz
	@idGamer INT = 0,
	@idEvent INT = 0,
	@delEvent BIT = 0
AS

	DELETE FROM G_Predict WHERE idEvent = @idEvent AND (idGamer = @idGamer OR @idGamer = 0)
	DELETE FROM G_Gamer WHERE idEvent = @idEvent AND (idGamer = @idGamer OR @idGamer = 0)
	DELETE FROM G_Quiz WHERE idEvent = @idEvent AND @idGamer = 0

	IF @delEvent = 1
	BEGIN
		DELETE FROM S_Match WHERE idEvent = @idEvent
		DELETE FROM S_EventRound WHERE idEvent = @idEvent
		DELETE FROM S_Event WHERE idEvent = @idEvent
	END
GO

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
CREATE PROCEDURE GetOnGoingMatch
	@dtStamp DATETIME = NULL,
	@idEvent INT = 0
AS
	SET @dtStamp = ISNULL(@dtStamp, GETDATE())

	SELECT * FROM S_Match m
	JOIN S_EventRound r ON m.idRound = r.idRound
	WHERE m.idEvent IN (SELECT idEvent FROM G_Quiz WHERE idStatus <> -1)
	AND (m.idEvent = @idEvent OR @idEvent = 0)
	AND m.startDate IS NOT NULL AND (m.endDate IS NULL OR DATEDIFF(HOUR, m.endDate, @dtStamp) BETWEEN 0 AND 12)
	ORDER BY startDate, endDate ASC
GO

GO
ALTER PROCEDURE GetGamerTrendingByDay
	@idEvent INT = 0,
	@idGamer INT = 0
AS
	SELECT CAST(t.dtResult AS date) dtResult, ISNULL(point, 0) point, p.NbWinnerCorrect, p.NbScoreCorrect,
	SUM(ISNULL(point, 0)) OVER(ORDER BY CAST(t.dtResult AS date) ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS cumulPoint
	FROM (
		SELECT distinct CAST(dtResult as date) dtResult from G_Predict
		WHERE idEvent = @idEvent AND dtResult is not null
	) t
	LEFT JOIN (
		SELECT CAST(dtResult as date) dtResult, idEvent, idGamer, SUM(point) point, SUM(CAST(winnerCorrect AS INT)) NbWinnerCorrect, SUM(CAST(scoreCorrect AS INT)) NbScoreCorrect
		FROM G_Predict
		WHERE idEvent = @idEvent AND dtResult is not null
		GROUP BY CAST(dtResult as date), idEvent, idGamer
	) p ON CAST(p.dtResult as date) = t.dtResult AND idEvent = @idEvent AND idGamer = @idGamer
	ORDER BY CAST(t.dtResult AS date)
GO

GO
CREATE PROCEDURE GetMatchPredict
	@idEvent INT = 0,
	@idGamer INT = 0
AS
	SELECT r.roundName, m.number, CAST(m.scheduledDate AS DATE) scheduledDate, ISNULL(mp1.chineseName, mp1.lastName) player1 , m.score1, m.score2, ISNULL(mp2.chineseName, mp2.lastName) player2
		, ISNULL(p1.chineseName, p1.lastName) gplayer1 , p.score1, p.score2, ISNULL(p2.chineseName, p2.lastName) gplayer2
		, CASE WHEN p.winnerCorrect = 1 THEN 'yes' ELSE 'no' END IsWinnerCorrect
		, CASE WHEN p.scoreCorrect = 1 THEN 'yes' ELSE 'no' END IsScoreCorrect , ISNULL(p.point, 0) point
	FROM S_Match m
	JOIN S_EventRound r ON m.idEvent = r.idEvent AND m.idRound = r.idRound
	JOIN S_Player mp1 ON m.player1Id = mp1.idPlayer
	JOIN S_Player mp2 ON m.player2Id = mp2.idPlayer
	LEFT JOIN G_Predict p ON m.idEvent = p.idEvent AND m.idRound = p.idRound AND m.number = p.number AND p.idGamer = @idGamer
	LEFT JOIN S_Player p1 ON p.player1Id = p1.idPlayer
	LEFT JOIN S_Player p2 ON p.player2Id = p2.idPlayer
	WHERE m.idEvent = @idEvent 
	ORDER BY m.idRound, m.number
GO