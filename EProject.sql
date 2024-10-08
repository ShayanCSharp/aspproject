-- Create database
CREATE DATABASE EProject;

-- Use the created database
USE EProject;

-- 1. Candidates Table
CREATE TABLE Candidates (
    CandidateId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,  -- Increased size to 100 for typical emails
    PhoneNumber VARCHAR(20) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    EducationDetails VARCHAR(255) NOT NULL,
    WorkExperience VARCHAR(255) NOT NULL,
    IsTestCompleted BIT DEFAULT 0,  -- Changed SMALLINT to BIT since it's binary data (yes/no)
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 2. Manager Table
CREATE TABLE Managers (
    ManagerId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,   -- Added FullName as the table did not have a name column
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NOT NULL,  -- Increased size for email
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 3. Tests Table
CREATE TABLE Tests (
    TestId INT IDENTITY(1,1) PRIMARY KEY,
    TestName NVARCHAR(50) NOT NULL,
    TotalQuestions INT NOT NULL,
    MaxMarks INT NOT NULL,
    TimeLimit INT NOT NULL  -- Time limit in minutes
);

-- 4. Questions Table
CREATE TABLE Questions (
    QuestionId INT IDENTITY(1,1) PRIMARY KEY,
    TestId INT NOT NULL,
    QuestionText NVARCHAR(MAX) NOT NULL,
    Option1 NVARCHAR(255) NOT NULL,
    Option2 NVARCHAR(255) NOT NULL,
    Option3 NVARCHAR(255) NOT NULL,
    Option4 NVARCHAR(255) NOT NULL,
    CorrectAnswer NVARCHAR(255) NOT NULL,
    Marks INT NOT NULL,
    CONSTRAINT FK_Questions_TestId FOREIGN KEY (TestId) REFERENCES Tests(TestId)
);

-- 5. CandidateAnswers Table
CREATE TABLE CandidateAnswers (
    AnswerId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateId INT NOT NULL,
    TestId INT NOT NULL,
    QuestionId INT NOT NULL,
    SelectedAnswer NVARCHAR(255) NOT NULL,  -- This should be large enough to hold any answer
    IsCorrect BIT NOT NULL,
    MarksAwarded INT NOT NULL,
    CONSTRAINT FK_CandidateAnswers_CandidateId FOREIGN KEY (CandidateId) REFERENCES Candidates(CandidateId),
    CONSTRAINT FK_CandidateAnswers_TestId FOREIGN KEY (TestId) REFERENCES Tests(TestId),
    CONSTRAINT FK_CandidateAnswers_QuestionId FOREIGN KEY (QuestionId) REFERENCES Questions(QuestionId)
);

-- 6. Results Table
CREATE TABLE Results (
    ResultId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateId INT NOT NULL,
    TestId INT NOT NULL,
    TotalMarks INT NOT NULL,
    IsPassed BIT NOT NULL,
    TestDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Results_CandidateId FOREIGN KEY (CandidateId) REFERENCES Candidates(CandidateId),
    CONSTRAINT FK_Results_TestId FOREIGN KEY (TestId) REFERENCES Tests(TestId)
);