


CREATE TABLE Users(
	Id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Username nvarchar(32) NOT NULL,
	Password nvarchar(256) NOT NULL,
	Number nvarchar(25) UNIQUE NULL,
	Email nvarchar(254) UNIQUE NULL

	
);


CREATE TABLE Modules(
	Id				int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Name			nvarchar(512) NULL,
	Description		nvarchar(512) NULL,
	User_Id			int NOT NULL,
	--blocks of data Questions and Images to Questions may be(by id)

	CONSTRAINT FK_user_Modules FOREIGN KEY(User_Id) REFERENCES Users(Id)
);

CREATE TABLE Questions(
	Id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Module_Id int NOT NULL,
	Question nvarchar(512) NOT NULL,
	Answer nvarchar(512) NOT NULL,

	CONSTRAINT FK_Modul_Questions FOREIGN KEY(Module_Id) REFERENCES Modules(Id)
);


CREATE TABLE Question_Images(
	Id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Question_Image varchar(1024) NULL,
	Answer_Image varchar(1024) NULL,
	Question_Id int NOT NULL

	CONSTRAINT FK_Question_Images FOREIGN KEY(Question_Id) REFERENCES Questions(Id)
);

CREATE INDEX IX_Modules_User_Id ON Modules(User_Id);
CREATE INDEX IX_Questions_Module_Id ON Questions(Module_Id);
CREATE INDEX IX_QuestionImages_Question_Id ON Question_Images(Question_Id);
