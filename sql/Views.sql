--GO
--CREATE VIEW vMatchOnGoing
--AS
--SELECT * FROM S_Match
--WHERE idEvent IN (SELECT idEvent FROM G_Quiz WHERE dtEnd IS NULL AND idStatus <> -1)
--AND startDate IS NOT NULL AND (endDate IS NULL OR DATEDIFF(HOUR, endDate, GETDATE()) BETWEEN 0 AND 12)
--GO

GO
ALTER VIEW vPredictSummary
AS
SELECT p.idGamer, g.gamerName, p.idEvent, COUNT(*) totalPredict, SUM(CAST(winnerCorrect AS INT)) nbrWinnerCorrect,
SUM(CAST(scoreCorrect AS INT)) nbrScoreCorrect, SUM(point) totalPoint
FROM G_Predict p
JOIN G_Gamer g ON p.idGamer = g.idGamer
WHERE p.point IS NOT NULL AND p.idStatus = 1
GROUP BY p.idGamer, g.gamerName, p.idEvent
GO