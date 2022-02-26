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








update S_Player set chineseName = '������' where idPlayer = 17
update S_Player set chineseName = '��ɳ����' where idPlayer = 5
update S_Player set chineseName = '��³ķ��' where idPlayer = 12
update S_Player set chineseName = '������޲�ѷ' where idPlayer = 154
update S_Player set chineseName = '���ס�����ѷ' where idPlayer = 39
update S_Player set chineseName = 'ϣ��˹' where idPlayer = 237
update S_Player set chineseName = '����ͯ' where idPlayer = 946
update S_Player set chineseName = '��ˡ�����ķ˹' where idPlayer = 1
update S_Player set chineseName = '����˹' where idPlayer = 16
update S_Player set chineseName = 'ī��' where idPlayer = 97
update S_Player set chineseName = '��˰���' where idPlayer = 202
update S_Player set chineseName = '�����' where idPlayer = 2
update S_Player set chineseName = '�ձ���' where idPlayer = 1260
update S_Player set chineseName = '����ķ' where idPlayer = 30
update S_Player set chineseName = '��˼���' where idPlayer = 22
update S_Player set chineseName = '¬���������ж�' where idPlayer = 101