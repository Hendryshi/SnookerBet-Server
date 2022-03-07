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