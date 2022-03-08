CREATE TABLE S_Event
(	
	idEvent int NOT NULL PRIMARY KEY,
	name NVARCHAR(500) NOT NULL,
	startDate DATETIME NULL,
	endDate DATETIME NULL,
	season int NULL,
	tyEvent NVARCHAR(100) NULL,
	city NVARCHAR(200) NULL,
	country NVARCHAR(200) NULL,
	allRoundsAdded BIT DEFAULT 0,
	numCompetitors INT NULL,
	numUpcoming INT NULL,
	numActive INT NULL,
	numResults INT NULL,
	note NVARCHAR(1000) NULL,
	commonNote NVARCHAR(1000) NULL,
	dtUpdate DATETIME NULL
)

CREATE TABLE S_Player
(	
	idPlayer INT NOT NULL PRIMARY KEY,
	[type] SMALLINT NOT NULL,
	firstName NVARCHAR(200) NULL,
	middleName NVARCHAR(200) NULL,
	lastName NVARCHAR(200) NULL,
	shortName NVARCHAR(200) NULL,
	chineseName NVARCHAR(200) NULL,
	nationality NVARCHAR(200) NULL,
	sex NVARCHAR(10) NULL,
	born DATETIME NULL,
	firstSeasonAsPro INT NULL,
	lastSeasonAsPro INT NULL,
	seasonRank INT NULL,
	photo NVARCHAR(1000) NULL,
	dtUpdate DATETIME NOT NULL
)


CREATE TABLE S_EventRound
(	
	idEvent INT NOT NULL,
	idRound INT NOT NULL,
	roundName NVARCHAR(100) NOT NULL,
	distance INT NOT NULL,
	numLeft INT NULL,
	numMatches INT NULL,
	note NVARCHAR(1000) NULL,
	[rank] INT NULL,
	[money] INT NULL,
	actualMoney INT NULL,
	currency NVARCHAR(20) NULL,
	dtUpdate DATETIME NOT NULL,
	CONSTRAINT PK_EventRound PRIMARY KEY (idEvent, idRound),
	CONSTRAINT FK_S_EventRound_IdEvent FOREIGN KEY (idEvent) REFERENCES S_Event(idEvent)
)

CREATE TABLE S_Match
(	
	idMatch int NOT NULL PRIMARY KEY,
	idEvent int NOT NULL,
	idRound int NOT NULL,
	number int NOT NULL,
	player1Id int NOT NULL,
	score1 int NULL,
	player2Id int NOT NULL,
	score2 int NULL,
	winnerId int NULL,
	unfinished BIT DEFAULT 1,
	onBreak BIT DEFAULT 0,
	initDate DATETIME NULL,
	modDate DATETIME NULL,
	startDate DATETIME NULL,
	endDate DATETIME NULL,
	scheduledDate DATETIME NULL,
	note NVARCHAR(MAX) NULL,
	extendedNote NVARCHAR(MAX) NULL,
	dtUpdate DATETIME NOT NULL
)


CREATE TABLE G_Quiz
(	
	idQuiz int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	idEvent INT NOT NULL,
	dtStart DATETIME NULL,
	dtEnd DATETIME NULL,
	idStatus SMALLINT NOT NULL DEFAULT 0,
	dtUpdate DATETIME NOT NULL,
	CONSTRAINT FK_G_Quiz_IdEvent FOREIGN KEY (idEvent) REFERENCES S_Event(idEvent)
)

CREATE TABLE G_Gamer
(	
	idGamer int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	idEvent INT NOT NULL,
	wechatName NVARCHAR(100) NULL,
	gamerName NVARCHAR(100) NULL,
	totalScore INT NOT NULL DEFAULT 0,
	changePredict BIT NOT NULL DEFAULT 0,
	dtUpdate DATETIME NOT NULL,
	CONSTRAINT FK_G_Gamer_IdEvent FOREIGN KEY (idEvent) REFERENCES S_Event(idEvent)
)

CREATE TABLE G_Predict
(	
	idPredict int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	idGamer int NOT NULL,
	idEvent int NOT NULL,
	idRound int NOT NULL,
	number int NOT NULL,
	player1Id int NOT NULL,
	score1 int NOT NULL,
	player2Id int NOT NULL,
	score2 int NOT NULL,
	winnerId int NULL,
	point int NULL,
	winnerCorrect BIT NOT NULL DEFAULT 0,
	scoreCorrect BIT NOT NULL DEFAULT 0,
	dtResult DATETIME NULL,
	idStatus smallint NOT NULL DEFAULT 0,
	dtUpdate DATETIME NOT NULL
	CONSTRAINT FK_G_Predict_idGamer FOREIGN KEY (idGamer) REFERENCES G_Gamer(idGamer)
)

GO
CREATE VIEW vPredictSummary
AS
SELECT p.idGamer, g.gamerName, p.idEvent, COUNT(*) totalPredict, SUM(CAST(winnerCorrect AS INT)) nbrWinnerCorrect,
SUM(CAST(scoreCorrect AS INT)) nbrScoreCorrect, SUM(point) totalPoint
FROM G_Predict p
JOIN G_Gamer g ON p.idGamer = g.idGamer
WHERE p.point IS NOT NULL AND p.idStatus = 1
GROUP BY p.idGamer, g.gamerName, p.idEvent
GO

update S_Player set lastName = 'TBD' where idPlayer = 376
update S_Player set chineseName = 'Èû¶û±È', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1597.png' where idPlayer = 17
update S_Player set chineseName = '°ÂÉ³ÀûÎÄ', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1395.png' where idPlayer = 5
update S_Player set chineseName = 'ÌØÂ³Ä·ÆÕ', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/24.png' where idPlayer = 12
update S_Player set chineseName = 'Äá¶û¡¤ÂÞ²®Ñ·', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1560.png' where idPlayer = 154
update S_Player set chineseName = '¿­Â×¡¤Íþ¶ûÑ·', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2729.png' where idPlayer = 39
update S_Player set chineseName = 'Ï£½ðË¹', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1347.png' where idPlayer = 237
update S_Player set chineseName = 'ÕÔÐÄÍ¯', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/39470.png' where idPlayer = 946
update S_Player set chineseName = 'Âí¿Ë¡¤ÍþÁ®Ä·Ë¹', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1335.png' where idPlayer = 1
update S_Player set chineseName = '»ô½ðË¹', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/835.png' where idPlayer = 16
update S_Player set chineseName = 'Ä«·Æ', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/511.png' where idPlayer = 97
update S_Player set chineseName = 'Âí¿Ë°¬Â×', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/799.png' where idPlayer = 202
update S_Player set chineseName = 'Âí¿ü¶û', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/517.png' where idPlayer = 2
update S_Player set chineseName = 'ÑÕ±þÌÎ', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/40273.png' where idPlayer = 1260
update S_Player set chineseName = '±öººÄ·', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/743.png' where idPlayer = 30
update S_Player set chineseName = 'Âó¿Ë¼ª¶û', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2715.png' where idPlayer = 22
update S_Player set chineseName = 'Â¬¿¨¡¤²¼À×ÇÐ¶û', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2709.png' where idPlayer = 101