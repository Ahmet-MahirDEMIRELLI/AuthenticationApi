CREATE DATABASE AuthApp;

USE AuthApp;

CREATE TABLE Users
(
    UserId INT IDENTITY(1,1) NOT NULL,
    UserName NVARCHAR(100) NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    Email NVARCHAR(50) NOT NULL,
    PRIMARY KEY (UserId)
);

CREATE TABLE LoggedUsers
(
	LogId INT IDENTITY(1,1) NOT NULL,
	UserId INT NOT NULL,
	LogInDateTime DATETIME,
	SpecialCode nvarchar(50),
	PRIMARY KEY(LogId),
	FOREIGN KEY(UserId) REFERENCES Users(UserId)
);