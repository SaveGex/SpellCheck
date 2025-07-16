USE [spell_test_db]

CREATE TABLE Users(
	Id int PRIMARY KEY IDENTITY(1,1),
	Username nvarchar(32) NOT NULL,
	Password nvarchar(256) NOT NULL,
	Number nvarchar(25) UNIQUE NULL,
	Email nvarchar(254) UNIQUE NULL,
	Created_At smalldatetime NOT NULL DEFAULT(GETDATE())
);

CREATE TABLE Difficulty_Level(
	Id int PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(100) NOT NULL,
	Difficulty int NULL,

	CONSTRAINT CHK_Difficulty_Range CHECK(Difficulty >= 0 AND Difficulty <=6)
);

CREATE TABLE Words_To_Learn(
	Id int PRIMARY KEY IDENTITY(1,1),
	Learning_Progress int NOT NULL DEFAULT(0),
	Expression nvarchar(256) NOT NULL,
	Meaning nvarchar(256) NOT NULL,
	Difficulty int NULL,
	User_Id int NOT NULL,

	CONSTRAINT FK_Words_To_Learn_User FOREIGN KEY (User_Id) REFERENCES Users(Id),
	CONSTRAINT FK_Difficulty_Difficulty_Level FOREIGN KEY (Difficulty) REFERENCES Difficulty_Level(Id)
);

CREATE TABLE Questions(
	Id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Expression nvarchar(1024) NOT NULL,
	Correct_Variant nvarchar(1024) NOT NULL
);

CREATE TABLE Friends(
	Id int PRIMARY KEY IDENTITY(1,1),
	from_individual_id int NOT NULL,
	to_individual_id int NOT NULL,

	CONSTRAINT FK_from_individual_Friend_id_Users FOREIGN KEY(from_individual_id) REFERENCES Users(id),
	CONSTRAINT FK_to_individual_Friend_id_Users FOREIGN KEY(to_individual_id) REFERENCES Users(id),

	CONSTRAINT UQ_from_individual_to_individual_ids UNIQUE(from_individual_id, to_individual_id),
	CONSTRAINT CHK_Unique_ids CHECK(from_individual_id <= to_individual_id)
);

--generic relation / polymorphic association
CREATE TABLE Files (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Size INT NULL,

    EntityType NVARCHAR(50) NOT NULL,   -- 'User', 'Word', 'Friend', ����
    EntityId INT NOT NULL,              -- Id ������ � �������, ����. Users.Id = 1

    UploadedAt SMALLDATETIME NOT NULL DEFAULT(GETDATE())
);
