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
	idGamer INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	idEvent INT NOT NULL,
	wechatName NVARCHAR(100) NULL,
	gamerName NVARCHAR(100) NULL,
	totalScore INT NOT NULL DEFAULT 0,
	nbEditPredict SMALLINT NOT NULL DEFAULT 0,
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

CREATE TABLE G_QuizSummary
(	
	idSummary INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	idEvent int NOT NULL,
	dtResult DATETIME NOT NULL,
	descSummary NVARCHAR(MAX)
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
update S_Player set chineseName = 'ÀûË÷·òË¹»ù', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2702.png' where idPlayer = 85
update S_Player set chineseName = 'ÎÖ¶Ù', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1564.png' where idPlayer = 62
update S_Player set chineseName = 'Íß·Æ', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/3395.png' where idPlayer = 666
update S_Player set chineseName = '¼ª¶û²®ÌØ', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/320.png' where idPlayer = 118
update S_Player set chineseName = '¹Å¶ûµÂ', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1618.png' where idPlayer = 27
update S_Player set chineseName = 'Èð¶÷´÷', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/546.png' where idPlayer = 68
update S_Player set chineseName = 'ÇÇÅåÀï', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1400.png' where idPlayer = 168
update S_Player set chineseName = 'ÖÜÔ¾Áú', photo='https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/39096.png' where idPlayer = 906
update S_Player set chineseName = '°¢Àï¿¨ÌØ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/822.png' where idplayer = 158
update S_Player set chineseName = 'ÇÇµ¤²¼ÀÊ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2541.png' where idplayer = 58
update S_Player set chineseName = 'ÌÀÄ·¸£µÂ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1563.png' where idplayer = 8
update S_Player set chineseName = '¼ªÃ×¡¤ÂÞ²®Ñ·', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1965.png' where idplayer = 93
update S_Player set chineseName = '¼ÓÀïÍþ¶ûÑ·', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1649.png' where idplayer = 546
update S_Player set chineseName = 'ÁºÎÄ²©', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2457.png' where idplayer = 200
update S_Player set chineseName = 'Èû¶ûÌØ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1835.png' where idplayer = 47
update S_Player set chineseName = '¶¡¿¡êÍ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1734.png' where idplayer = 224
update S_Player set chineseName = 'Â³Äþ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/39094.png' where idplayer = 908
update S_Player set chineseName = 'Âí·òÁÖ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1640.png' where idplayer = 61
update S_Player set chineseName = '·¶ÕýÒ»', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/40614.png' where idplayer = 1417
update S_Player set chineseName = '¶àÌØ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/673.png' where idplayer = 52
update S_Player set chineseName = 'Ð¤¹ú¶°', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2528.png' where idplayer = 24
update S_Player set chineseName = 'ÅµÅîÉ£¿²', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2742.png' where idplayer = 208
update S_Player set chineseName = 'Âí¿Ë½ð', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/216.png' where idplayer = 28
update S_Player set chineseName = '½ÜÃ×ÇíË¹', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2455.png' where idplayer = 10
update S_Player set chineseName = 'º£·Æ¶ûµÂ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2727.png' where idplayer = 45
update S_Player set chineseName = '¿ËÀ×¼ª', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2728.png' where idplayer = 109
update S_Player set chineseName = 'ÀîÐÐ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2532.png' where idplayer = 295
update S_Player set chineseName = 'Ã×¶û½ðË¹', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/759.png' where idplayer = 92
update S_Player set chineseName = 'ÎéÀ­Ë¹¶Ù', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2248.png' where idplayer = 19
update S_Player set chineseName = 'ººÃÜ¶û¶Ù', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/167.png' where idplayer = 115
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2858.png' where idplayer = 239
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2696.png' where idplayer = 90
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/33144.png' where idplayer = 894
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/3117.png' where idplayer = 608
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/39670.png' where idplayer = 1038
update S_Player set chineseName = 'Âí¿Ë´÷Î¬Ë¹', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/106.png' where idplayer = 15
update S_Player set chineseName = 'ÇÇÒÁË¹', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1870.png' where idplayer = 48
update S_Player set chineseName = 'Ëþ²ÂÑÇ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2549.png' where idplayer = 217
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2700.png' where idplayer = 128
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2530.png' where idplayer = 67
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/64479.png' where idplayer = 1763
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/144.png' where idplayer = 120
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/40287.png' where idplayer = 1257
update S_Player set chineseName = 'ÂíÐÞË¹µÙÎÄË¹', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/706.png' where idplayer = 9
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/806.png' where idplayer = 25
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2529.png' where idplayer = 96
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/33154.png' where idplayer = 1044
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1092.png' where idplayer = 33
update S_Player set chineseName = '¿ËÀ­¿Ë', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/3406.png' where idplayer = 106
update S_Player set chineseName = 'ÂÀºÆÌì', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/39097.png' where idplayer = 905
update S_Player set chineseName = 'ÌïÅô·É', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2456.png' where idplayer = 218
update S_Player set chineseName = '»ô¶ûÌØ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/841.png' where idplayer = 125
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/3119.png' where idplayer = 592
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/3396.png' where idplayer = 724
update S_Player set chineseName = '²ÜÓêÅô', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2690.png' where idplayer = 507
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/960.png' where idplayer = 63
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/100020.png' where idplayer = 2607
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1794.png' where idplayer = 26
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/184.png' where idplayer = 51
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/3086.png' where idplayer = 605
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1101.png' where idplayer = 14
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1122.png' where idplayer = 170
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/99922.png' where idplayer = 2557
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/64121.png' where idplayer = 1567
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/40397.png' where idplayer = 1323
update S_Player set chineseName = 'ÎâÒ»Ôò', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/99706.png' where idplayer = 2469
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/933.png' where idplayer = 50
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/39709.png' where idplayer = 1082
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2704.png' where idplayer = 49
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1884.png' where idplayer = 2335
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/3119.png' where idplayer = 37
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/711.png' where idplayer = 520
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/3162.png' where idplayer = 593
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/39819.png' where idplayer = 1122
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/99775.png' where idplayer = 2470
update S_Player set chineseName = 'Ô¬Ë¼¿¡', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/39759.png' where idplayer = 1108
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/40553.png' where idplayer = 1407
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2703.png' where idplayer = 89
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/98485.png' where idplayer = 2166
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2924.png' where idplayer = 448
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2755.png' where idplayer = 163
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/98349.png' where idplayer = 2134
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/64772.png' where idplayer = 1981
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/100285.png' where idplayer = 2751
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1633.png' where idplayer = 131
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1971.png' where idplayer = 534
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1468.png' where idplayer = 38
update S_Player set chineseName = 'ÕÅ°²´ï', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2558.png' where idplayer = 81
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/39644.png' where idplayer = 1045
update S_Player set chineseName = 'ºàµÃÀû', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1157.png' where idplayer = 153
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/99112.png' where idplayer = 1889
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2485.png' where idplayer = 79
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1200.png' where idplayer = 6
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/2701.png' where idplayer = 87
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/693.png' where idplayer = 21
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/99750.png' where idplayer = 2498
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1824.png' where idplayer = 65
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/99269.png' where idplayer = 2340
update S_Player set chineseName = '¼ªÃ×»³ÌØ', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1557.png' where idplayer = 20
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1368.png' where idplayer = 204
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/39507.png' where idplayer = 933
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/99790.png' where idplayer = 2499
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/40832.png' where idplayer = 1485
update S_Player set chineseName = '', photo = 'https://4d9e5aaf64f4cd6ea36e-a4f331decd676ada08548b37a013de11.ssl.cf3.rackcdn.com/1048.png' where idplayer = 175
update S_Player set chineseName = '¸µ¼Ò¿¡', photo = '' where idplayer = 4
update S_Player set chineseName = '', photo = '' where idplayer = 18