create database FASTCareerConnect;

-- Create Tables


-- table 1
USE FASTCareerConnect;

DROP DATABASE FASTCareerConnect;


-- Create database FASTCareerConnect;
USE FASTCareerConnect;

-- Create Tables

-- Users Table
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    IsApproved BIT NOT NULL DEFAULT 0,
    CONSTRAINT CHK_Role CHECK (Role IN ('Student', 'Recruiter', 'TPO', 'BoothCoordinator'))
);
-- Students Table
select * from Users
select * from Students

CREATE TABLE Students (
    StudentID INT PRIMARY KEY IDENTITY(1,1), -- Automatically incrementing ID
    GPA DECIMAL(3,2),
    DegreeProgram VARCHAR(50),
    CurrentSemester INT,
    CONSTRAINT FK_Students_Users FOREIGN KEY (StudentID) REFERENCES Users(UserID)
);

-- Companies Table
CREATE TABLE Companies (
    CompanyID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Sector VARCHAR(50),
    Description NVARCHAR(MAX),
    CONSTRAINT UQ_Companies_Name UNIQUE (Name)
);
select * from Recruiters
-- Recruiters Table
CREATE TABLE Recruiters (
    RecruiterID INT PRIMARY KEY,
    CompanyID INT NOT NULL,
    CONSTRAINT FK_Recruiters_Users FOREIGN KEY (RecruiterID) REFERENCES Users(UserID),
    CONSTRAINT FK_Recruiters_Companies FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID)
);

-- Skills Table
CREATE TABLE Skills (
    SkillID INT PRIMARY KEY IDENTITY(1,1),
    SkillName VARCHAR(50) NOT NULL UNIQUE
);

-- StudentSkills Table
CREATE TABLE StudentSkills (
    StudentID INT NOT NULL,
    SkillID INT NOT NULL,
    CONSTRAINT PK_StudentSkills PRIMARY KEY (StudentID, SkillID),
    CONSTRAINT FK_StudentSkills_Students FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    CONSTRAINT FK_StudentSkills_Skills FOREIGN KEY (SkillID) REFERENCES Skills(SkillID)
);

-- JobPostings Table
CREATE TABLE JobPostings (
    JobPostingID INT PRIMARY KEY IDENTITY(1,1),
    CompanyID INT NOT NULL,
    Title VARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    JobType VARCHAR(20) NOT NULL,
    SalaryRange VARCHAR(50),
    Location VARCHAR(100),
    CONSTRAINT FK_JobPostings_Companies FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID),
    CONSTRAINT CHK_JobType CHECK (JobType IN ('Internship', 'Full-time'))
);

-- JobPostingSkills Table
CREATE TABLE JobPostingSkills (
    JobPostingID INT NOT NULL,
    SkillID INT NOT NULL,
    CONSTRAINT PK_JobPostingSkills PRIMARY KEY (JobPostingID, SkillID),
    CONSTRAINT FK_JobPostingSkills_JobPostings FOREIGN KEY (JobPostingID) REFERENCES JobPostings(JobPostingID),
    CONSTRAINT FK_JobPostingSkills_Skills FOREIGN KEY (SkillID) REFERENCES Skills(SkillID)
);

-- JobFairEvents Table
CREATE TABLE JobFairEvents (
    JobFairID INT PRIMARY KEY IDENTITY(1,1),
    EventDate DATE NOT NULL,
    EventTime TIME(0) NOT NULL,
    Venue VARCHAR(100) NOT NULL
);

-- Booths Table
CREATE TABLE Booths (
    BoothID INT PRIMARY KEY IDENTITY(1,1),
    JobFairID INT NOT NULL,
    CompanyID INT NOT NULL,
    Location VARCHAR(50),
    CoordinatorID INT NOT NULL,
    CONSTRAINT UQ_Booths_JobFair_Company UNIQUE (JobFairID, CompanyID),
    CONSTRAINT FK_Booths_JobFairEvents FOREIGN KEY (JobFairID) REFERENCES JobFairEvents(JobFairID),
    CONSTRAINT FK_Booths_Companies FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID),
    CONSTRAINT FK_Booths_Users FOREIGN KEY (CoordinatorID) REFERENCES Users(UserID)
);

-- Applications Table
CREATE TABLE Applications (
    ApplicationID INT PRIMARY KEY IDENTITY(1,1),
    StudentID INT NOT NULL,
    JobPostingID INT NOT NULL,
    ApplicationDate DATE NOT NULL,
    Status VARCHAR(20) NOT NULL,
    CONSTRAINT FK_Applications_Students FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    CONSTRAINT FK_Applications_JobPostings FOREIGN KEY (JobPostingID) REFERENCES JobPostings(JobPostingID),
    CONSTRAINT CHK_ApplicationStatus CHECK (Status IN ('Applied', 'Shortlisted', 'Interviewed', 'Accepted'))
);

-- Interviews Table
CREATE TABLE Interviews (
    InterviewID INT PRIMARY KEY IDENTITY(1,1),
    ApplicationID INT NOT NULL,
    RecruiterID INT NOT NULL,
    DateTime DATETIME NOT NULL,
    Status VARCHAR(20) NOT NULL,
    CONSTRAINT FK_Interviews_Applications FOREIGN KEY (ApplicationID) REFERENCES Applications(ApplicationID),
    CONSTRAINT FK_Interviews_Recruiters FOREIGN KEY (RecruiterID) REFERENCES Recruiters(RecruiterID),
    CONSTRAINT CHK_InterviewStatus CHECK (Status IN ('Scheduled', 'Completed', 'Cancelled'))
);

-- Reviews Table
CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    StudentID INT NOT NULL,
    RecruiterID INT NOT NULL,
    Rating INT NOT NULL,
    Comment NVARCHAR(MAX),
    CONSTRAINT FK_Reviews_Students FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    CONSTRAINT FK_Reviews_Recruiters FOREIGN KEY (RecruiterID) REFERENCES Recruiters(RecruiterID),
    CONSTRAINT CHK_Rating CHECK (Rating >= 1 AND Rating <= 5)
);

-- Visits Table
CREATE TABLE Visits (
    VisitID INT PRIMARY KEY IDENTITY(1,1),
    StudentID INT NOT NULL,
    BoothID INT NOT NULL,
    VisitTime DATETIME NOT NULL,
    CONSTRAINT FK_Visits_Students FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    CONSTRAINT FK_Visits_Booths FOREIGN KEY (BoothID) REFERENCES Booths(BoothID)
);