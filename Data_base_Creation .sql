-- create database FASTCareerConnect;

-- Create Tables
-- table 1
USE FASTCareerConnect;

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1), -- AUTO_INCREMENT equivalent
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    IsApproved BIT NOT NULL DEFAULT 0, -- BOOLEAN replaced with BIT, FALSE = 0
    CONSTRAINT CHK_Role CHECK (Role IN ('Student', 'Recruiter', 'TPO', 'BoothCoordinator'))
);


-- Create Roles table to manage user roles (alternative to ENUM)
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName VARCHAR(50) NOT NULL UNIQUE,
    CONSTRAINT CHK_RoleName CHECK (RoleName IN ('Student', 'Recruiter', 'TPO', 'BoothCoordinator'))
);


-- Students Table
CREATE TABLE Students (
    StudentID INT PRIMARY KEY,
    GPA DECIMAL(3,2),
    DegreeProgram VARCHAR(50),
    CurrentSemester INT,
    CONSTRAINT FK_Students_Users FOREIGN KEY (StudentID) REFERENCES Users(UserID),
    CONSTRAINT CHK_DegreeProgram CHECK (DegreeProgram IN ('BS(SE)', 'BS(CS)', 'BS(DS)', 'BS(AI)', 'MS(AI)', 'MS(CS)', 'MS(SE)')),
    CONSTRAINT CHK_Semester CHECK (CurrentSemester BETWEEN 1 AND 8)
);

-- Companies Table
CREATE TABLE Companies (
    CompanyID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Sector VARCHAR(50),
    Description NVARCHAR(MAX),
    CONSTRAINT UQ_Companies_Name UNIQUE (Name)
);

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
    EventDate DATE NOT NULL, -- Renamed 'Date' to avoid reserved word issues
    EventTime TIME(0) NOT NULL, -- TIME(0) for no fractional seconds
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
    CONSTRAINT CHK_ApplicationStatus CHECK (Status IN ('Applied', 'Interviewed', 'Offered', 'Accepted'))
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


-- insertion 

INSERT INTO Companies (Name, Sector, Description) VALUES
('Google', 'Technology', 'Description of Google'),
('Microsoft', 'Technology', 'Description of Microsoft'),
('Amazon', 'Technology', 'Description of Amazon'),
('Apple', 'Technology', 'Description of Apple'),
('Facebook', 'Technology', 'Description of Facebook'),
('Tesla', 'Technology', 'Description of Tesla'),
('IBM', 'Technology', 'Description of IBM'),
('Oracle', 'Technology', 'Description of Oracle'),
('Intel', 'Technology', 'Description of Intel'),
('Cisco', 'Technology', 'Description of Cisco'),
('Samsung', 'Technology', 'Description of Samsung'),
('Sony', 'Technology', 'Description of Sony'),
('Toyota', 'Automotive', 'Description of Toyota'),
('BMW', 'Automotive', 'Description of BMW'),
('Mercedes-Benz', 'Automotive', 'Description of Mercedes-Benz'),
('Coca-Cola', 'Consumer Goods', 'Description of Coca-Cola'),
('PepsiCo', 'Consumer Goods', 'Description of PepsiCo'),
('McDonald''s', 'Consumer Goods', 'Description of McDonald''s'),
('Walmart', 'Retail', 'Description of Walmart'),
('Target', 'Retail', 'Description of Target'),
('SpaceX', 'Technology', 'Description of SpaceX'),
('Netflix', 'Technology', 'Description of Netflix'),
('Adobe', 'Technology', 'Description of Adobe'),
('Salesforce', 'Technology', 'Description of Salesforce'),
('Uber', 'Technology', 'Description of Uber');

INSERT INTO JobFairEvents (EventDate, EventTime, Venue) VALUES
('2023-10-15', '09:00:00', 'FAST Campus Hall'),
('2023-11-20', '10:00:00', 'Convention Center'),
('2023-12-05', '11:00:00', 'University Auditorium'),
('2024-01-10', '08:30:00', 'City Expo Center'),
('2024-02-25', '12:00:00', 'Tech Park Pavilion');


-- Insert 80 student users
DECLARE @i INT = 1;
WHILE @i <= 80
BEGIN
    INSERT INTO Users (Name, Email, Password, Role, IsApproved)
    VALUES ('Student ' + CAST(@i AS VARCHAR), 'student' + CAST(@i AS VARCHAR) + '@example.com', 'password', 'Student', 1);
    SET @i = @i + 1;
END

-- Insert 5 recruiter users
INSERT INTO Users (Name, Email, Password, Role, IsApproved)
VALUES
('Recruiter 1', 'recruiter1@example.com', 'password', 'Recruiter', 1),
('Recruiter 2', 'recruiter2@example.com', 'password', 'Recruiter', 1),
('Recruiter 3', 'recruiter3@example.com', 'password', 'Recruiter', 1),
('Recruiter 4', 'recruiter4@example.com', 'password', 'Recruiter', 1),
('Recruiter 5', 'recruiter5@example.com', 'password', 'Recruiter', 1);

-- Insert 2 TPO users
INSERT INTO Users (Name, Email, Password, Role, IsApproved)
VALUES
('TPO 1', 'tpo1@example.com', 'password', 'TPO', 1),
('TPO 2', 'tpo2@example.com', 'password', 'TPO', 1);

-- Insert 3 booth coordinator users
INSERT INTO Users (Name, Email, Password, Role, IsApproved)
VALUES
('BoothCoordinator 1', 'boothcoordinator1@example.com', 'password', 'BoothCoordinator', 1),
('BoothCoordinator 2', 'boothcoordinator2@example.com', 'password', 'BoothCoordinator', 1),
('BoothCoordinator 3', 'boothcoordinator3@example.com', 'password', 'BoothCoordinator', 1);



-- Insert 80 student records
INSERT INTO Students (StudentID, GPA, DegreeProgram, CurrentSemester)
VALUES
(1, 3.75, 'Computer Science', 5),
(2, 3.20, 'Electrical Engineering', 3),
(3, 3.90, 'Business Administration', 7),
(4, 2.95, 'Mechanical Engineering', 4),
(5, 3.60, 'Computer Science', 6),
(6, 3.45, 'Civil Engineering', 2),
(7, 3.80, 'Software Engineering', 8),
(8, 3.10, 'Business Administration', 1),
(9, 3.55, 'Computer Science', 5),
(10, 3.25, 'Electrical Engineering', 3),
(11, 3.85, 'Mechanical Engineering', 7),
(12, 3.70, 'Software Engineering', 4),
(13, 3.30, 'Civil Engineering', 6),
(14, 3.95, 'Computer Science', 2),
(15, 3.40, 'Business Administration', 8),
(16, 3.65, 'Electrical Engineering', 1),
(17, 3.50, 'Mechanical Engineering', 5),
(18, 3.15, 'Software Engineering', 3),
(19, 3.90, 'Computer Science', 7),
(20, 3.35, 'Civil Engineering', 4),
(21, 3.60, 'Business Administration', 6),
(22, 3.80, 'Electrical Engineering', 2),
(23, 3.25, 'Software Engineering', 8),
(24, 3.70, 'Computer Science', 1),
(25, 3.45, 'Mechanical Engineering', 5),
(26, 3.90, 'Business Administration', 3),
(27, 3.20, 'Civil Engineering', 7),
(28, 3.55, 'Software Engineering', 4),
(29, 3.75, 'Computer Science', 6),
(30, 3.30, 'Electrical Engineering', 2),
(31, 3.85, 'Mechanical Engineering', 8),
(32, 3.40, 'Business Administration', 1),
(33, 3.65, 'Software Engineering', 5),
(34, 3.50, 'Computer Science', 3),
(35, 3.15, 'Civil Engineering', 7),
(36, 3.80, 'Electrical Engineering', 4),
(37, 3.25, 'Mechanical Engineering', 6),
(38, 3.70, 'Business Administration', 2),
(39, 3.95, 'Software Engineering', 8),
(40, 3.30, 'Computer Science', 1),
(41, 3.60, 'Civil Engineering', 5),
(42, 3.45, 'Electrical Engineering', 3),
(43, 3.90, 'Mechanical Engineering', 7),
(44, 3.35, 'Business Administration', 4),
(45, 3.80, 'Software Engineering', 6),
(46, 3.20, 'Computer Science', 2),
(47, 3.55, 'Civil Engineering', 8),
(48, 3.75, 'Electrical Engineering', 1),
(49, 3.40, 'Mechanical Engineering', 5),
(50, 3.65, 'Business Administration', 3),
(51, 3.50, 'Software Engineering', 7),
(52, 3.15, 'Computer Science', 4),
(53, 3.90, 'Civil Engineering', 6),
(54, 3.25, 'Electrical Engineering', 2),
(55, 3.70, 'Mechanical Engineering', 8),
(56, 3.45, 'Business Administration', 1),
(57, 3.80, 'Software Engineering', 5),
(58, 3.30, 'Computer Science', 3),
(59, 3.95, 'Civil Engineering', 7),
(60, 3.40, 'Electrical Engineering', 4),
(61, 3.65, 'Mechanical Engineering', 6),
(62, 3.50, 'Business Administration', 2),
(63, 3.20, 'Software Engineering', 8),
(64, 3.75, 'Computer Science', 1),
(65, 3.35, 'Civil Engineering', 5),
(66, 3.90, 'Electrical Engineering', 3),
(67, 3.25, 'Mechanical Engineering', 7),
(68, 3.70, 'Business Administration', 4),
(69, 3.55, 'Software Engineering', 6),
(70, 3.80, 'Computer Science', 2),
(71, 3.45, 'Civil Engineering', 8),
(72, 3.60, 'Electrical Engineering', 1),
(73, 3.15, 'Mechanical Engineering', 5),
(74, 3.90, 'Business Administration', 3),
(75, 3.35, 'Software Engineering', 7),
(76, 3.70, 'Computer Science', 4),
(77, 3.25, 'Civil Engineering', 6),
(78, 3.80, 'Electrical Engineering', 2),
(79, 3.55, 'Mechanical Engineering', 8),
(80, 3.40, 'Business Administration', 1);



INSERT INTO Recruiters (RecruiterID, CompanyID)
VALUES
(81, 1), -- Recruiter 1 with Google
(82, 2), -- Recruiter 2 with Microsoft
(83, 3), -- Recruiter 3 with Amazon
(84, 4), -- Recruiter 4 with Apple
(85, 5); -- Recruiter 5 with Facebook
-- 
SELECT r.RecruiterID, u.Name, u.Email, c.Name AS CompanyName
FROM Recruiters r
JOIN Users u ON r.RecruiterID = u.UserID
JOIN Companies c ON r.CompanyID = c.CompanyID;

INSERT INTO Skills (SkillName)
VALUES
('C++'),
('Java'),
('Problem Solving'),
('Game Developer'),
('N8N'),
('Python'),
('Web Developer'),
('Artificial Intelligence'),
('Machine Learning'),
('Data Analysis'),
('Cloud Computing'),
('Mobile Development'),
('Database Management'),
('UI/UX Design'),
('Software Testing');


INSERT INTO StudentSkills (StudentID, SkillID)
VALUES
-- Student 1: Technical + Soft skills
(1, 1),  -- Python
(1, 5),  -- SQL
(1, 7),  -- Web Development
(1, 9),  -- Communication
-- Student 2: Mixed skills
(2, 2),  -- Java
(2, 6),  -- Machine Learning
(2, 10), -- Teamwork
(2, 11), -- Problem Solving
-- Student 3: Technical focus
(3, 3),  -- C++
(3, 4),  -- JavaScript
(3, 8),  -- Cloud Computing
(3, 13), -- Data Analysis
(3, 9),  -- Communication
-- Student 4: Soft + Technical
(4, 5),  -- SQL
(4, 7),  -- Web Development
(4, 12), -- Project Management
(4, 10), -- Teamwork
-- Student 5: Broad skills
(5, 1),  -- Python
(5, 2),  -- Java
(5, 9),  -- Communication
(5, 11), -- Problem Solving
(5, 14), -- Cybersecurity
-- Student 6
(6, 3),  -- C++
(6, 6),  -- Machine Learning
(6, 10), -- Teamwork
(6, 13), -- Data Analysis
-- Student 7
(7, 4),  -- JavaScript
(7, 8),  -- Cloud Computing
(7, 9),  -- Communication
(7, 12), -- Project Management
(7, 15), -- Mobile Development
-- Student 8
(8, 1),  -- Python
(8, 5),  -- SQL
(8, 11), -- Problem Solving
(8, 14), -- Cybersecurity
-- Student 9
(9, 2),  -- Java
(9, 7),  -- Web Development
(9, 10), -- Teamwork
(9, 13), -- Data Analysis
-- Student 10
(10, 3),  -- C++
(10, 6),  -- Machine Learning
(10, 9),  -- Communication
(10, 12); -- Project Management


--SELECT StudentID, COUNT(SkillID) AS SkillCount
--FROM StudentSkills
--GROUP BY StudentID
--ORDER BY StudentID;

--SELECT ss.StudentID, s.SkillName
--FROM StudentSkills ss
--JOIN Skills s ON ss.SkillID = s.SkillID
--WHERE ss.StudentID <= 10
--ORDER BY ss.StudentID, s.SkillName;


INSERT INTO JobPostings (CompanyID, Title, Description, JobType, SalaryRange, Location)
VALUES
(1, 'Software Engineer', 'Develop web applications using Python and JavaScript.', 'Full-time', '80,000-100,000', 'Mountain View, CA'),
(1, 'Data Science Intern', 'Analyze datasets to support AI projects.', 'Internship', '30/hr', 'Remote'),
(2, 'Cloud Engineer', 'Design and manage Azure cloud infrastructure.', 'Full-time', '90,000-110,000', 'Redmond, WA'),
(2, 'Software Developer Intern', 'Assist in building Windows applications.', 'Internship', '25/hr', 'Redmond, WA'),
(3, 'Backend Developer', 'Build scalable APIs for e-commerce platforms.', 'Full-time', '85,000-105,000', 'Seattle, WA'),
(3, 'Machine Learning Intern', 'Support ML model development.', 'Internship', '28/hr', 'Remote'),
(4, 'iOS Developer', 'Create innovative mobile apps for iOS.', 'Full-time', '100,000-120,000', 'Cupertino, CA'),
(4, 'UX Design Intern', 'Assist in designing user interfaces.', 'Internship', '27/hr', 'Cupertino, CA'),
(5, 'Frontend Developer', 'Build responsive web interfaces.', 'Full-time', '75,000-95,000', 'Menlo Park, CA'),
(5, 'Social Media Intern', 'Support social media strategy.', 'Internship', '22/hr', 'Remote'),
(6, 'Firmware Engineer', 'Develop embedded systems for vehicles.', 'Full-time', '95,000-115,000', 'Palo Alto, CA'),
(6, 'Robotics Intern', 'Work on autonomous vehicle projects.', 'Internship', '30/hr', 'Palo Alto, CA'),
(7, 'Database Administrator', 'Manage enterprise databases.', 'Full-time', '80,000-100,000', 'Armonk, NY'),
(8, 'Java Developer', 'Build enterprise applications.', 'Full-time', '85,000-105,000', 'Austin, TX'),
(9, 'Hardware Engineer', 'Design CPU components.', 'Full-time', '90,000-110,000', 'Santa Clara, CA'),
(10, 'Network Engineer', 'Manage network infrastructure.', 'Full-time', '80,000-100,000', 'San Jose, CA'),
(11, 'Mobile Developer', 'Create Android apps.', 'Full-time', '75,000-95,000', 'Seoul, South Korea'),
(12, 'Game Developer', 'Develop interactive games.', 'Full-time', '85,000-105,000', 'Tokyo, Japan'),
(13, 'Quality Engineer', 'Ensure vehicle quality standards.', 'Full-time', '70,000-90,000', 'Toyota City, Japan'),
(14, 'Automotive Software Engineer', 'Develop car software systems.', 'Full-time', '90,000-110,000', 'Munich, Germany'),
(15, 'Data Analyst', 'Analyze sales data.', 'Full-time', '65,000-85,000', 'Stuttgart, Germany'),
(16, 'Marketing Specialist', 'Promote beverage products.', 'Full-time', '60,000-80,000', 'Atlanta, GA'),
(17, 'Supply Chain Analyst', 'Optimize logistics.', 'Full-time', '70,000-90,000', 'Purchase, NY'),
(18, 'IT Support Specialist', 'Provide tech support.', 'Full-time', '55,000-75,000', 'Chicago, IL'),
(19, 'Store Manager', 'Manage retail operations.', 'Full-time', '60,000-80,000', 'Bentonville, AR'),
(20, 'Retail Analyst Intern', 'Analyze retail trends.', 'Internship', '20/hr', 'Minneapolis, MN'),
(21, 'Aerospace Engineer', 'Design spacecraft systems.', 'Full-time', '100,000-120,000', 'Hawthorne, CA'),
(22, 'Content Developer', 'Create streaming platform content.', 'Full-time', '70,000-90,000', 'Los Gatos, CA'),
(23, 'UI/UX Designer', 'Design software interfaces.', 'Full-time', '80,000-100,000', 'San Jose, CA'),
(24, 'CRM Developer', 'Build customer relationship tools.', 'Full-time', '85,000-105,000', 'San Francisco, CA');

INSERT INTO JobPostingSkills (JobPostingID, SkillID)
VALUES
-- Job 1: Software Engineer (Google, Full-time)
(1, 1),  -- Python
(1, 4),  -- JavaScript
(1, 7),  -- Web Development
(1, 9),  -- Communication
-- Job 2: Data Science Intern (Google, Internship)
(2, 1),  -- Python
(2, 6),  -- Machine Learning
(2, 13), -- Data Analysis
(2, 10), -- Teamwork
-- Job 3: Cloud Engineer (Microsoft, Full-time)
(3, 8),  -- Cloud Computing
(3, 5),  -- SQL
(3, 11), -- Problem Solving
(3, 9),  -- Communication
-- Job 4: Software Developer Intern (Microsoft, Internship)
(4, 2),  -- Java
(4, 7),  -- Web Development
(4, 10), -- Teamwork
-- Job 5: Backend Developer (Amazon, Full-time)
(5, 1),  -- Python
(5, 5),  -- SQL
(5, 7),  -- Web Development
(5, 11), -- Problem Solving
-- Job 6: Machine Learning Intern (Amazon, Internship)
(6, 6),  -- Machine Learning
(6, 1),  -- Python
(6, 13), -- Data Analysis
(6, 9),  -- Communication
-- Job 7: iOS Developer (Apple, Full-time)
(7, 15), -- Mobile Development
(7, 3),  -- C++
(7, 9),  -- Communication
(7, 11), -- Problem Solving
-- Job 8: UX Design Intern (Apple, Internship)
(8, 7),  -- Web Development
(8, 9),  -- Communication
(8, 10), -- Teamwork
-- Job 9: Frontend Developer (Facebook, Full-time)
(9, 4),  -- JavaScript
(9, 7),  -- Web Development
(9, 9),  -- Communication
(9, 11), -- Problem Solving
-- Job 10: Social Media Intern (Facebook, Internship)
(10, 9),  -- Communication
(10, 10), -- Teamwork
(10, 12), -- Project Management
-- Job 11: Firmware Engineer (Tesla, Full-time)
(11, 3),  -- C++
(11, 11), -- Problem Solving
(11, 9),  -- Communication
(11, 14), -- Cybersecurity
-- Job 12: Robotics Intern (Tesla, Internship)
(12, 6),  -- Machine Learning
(12, 3),  -- C++
(12, 10), -- Teamwork
-- Job 13: Database Administrator (IBM, Full-time)
(13, 5),  -- SQL
(13, 8),  -- Cloud Computing
(13, 11), -- Problem Solving
(13, 9),  -- Communication
-- Job 14: Java Developer (Oracle, Full-time)
(14, 2),  -- Java
(14, 7),  -- Web Development
(14, 11), -- Problem Solving
-- Job 15: Hardware Engineer (Intel, Full-time)
(15, 3),  -- C++
(15, 14), -- Cybersecurity
(15, 9),  -- Communication
-- Job 16: Network Engineer (Cisco, Full-time)
(16, 14), -- Cybersecurity
(16, 8),  -- Cloud Computing
(16, 11), -- Problem Solving
-- Job 17: Mobile Developer (Samsung, Full-time)
(17, 15), -- Mobile Development
(17, 2),  -- Java
(17, 9),  -- Communication
-- Job 18: Game Developer (Sony, Full-time)
(18, 3),  -- C++
(18, 7),  -- Web Development
(18, 11), -- Problem Solving
-- Job 19: Quality Engineer (Toyota, Full-time)
(19, 11), -- Problem Solving
(19, 12), -- Project Management
(19, 9),  -- Communication
-- Job 20: Automotive Software Engineer (BMW, Full-time)
(20, 3),  -- C++
(20, 14), -- Cybersecurity
(20, 11), -- Problem Solving
-- Job 21: Data Analyst (Mercedes-Benz, Full-time)
(21, 13), -- Data Analysis
(21, 5),  -- SQL
(21, 9),  -- Communication
-- Job 22: Marketing Specialist (Coca-Cola, Full-time)
(22, 9),  -- Communication
(22, 10), -- Teamwork
(22, 12), -- Project Management
-- Job 23: Supply Chain Analyst (PepsiCo, Full-time)
(23, 13), -- Data Analysis
(23, 11), -- Problem Solving
(23, 12), -- Project Management
-- Job 24: IT Support Specialist (McDonald's, Full-time)
(24, 14), -- Cybersecurity
(24, 11), -- Problem Solving
(24, 9),  -- Communication
-- Job 25: Store Manager (Walmart, Full-time)
(25, 12), -- Project Management
(25, 9),  -- Communication
(25, 10), -- Teamwork
-- Job 26: Retail Analyst Intern (Target, Internship)
(26, 13), -- Data Analysis
(26, 9),  -- Communication
(26, 10), -- Teamwork
-- Job 27: Aerospace Engineer (SpaceX, Full-time)
(27, 3),  -- C++
(27, 11), -- Problem Solving
(27, 9),  -- Communication
-- Job 28: Content Developer (Netflix, Full-time)
(28, 7),  -- Web Development
(28, 9),  -- Communication
(28, 12), -- Project Management
-- Job 29: UI/UX Designer (Adobe, Full-time)
(29, 7),  -- Web Development
(29, 9),  -- Communication
(29, 11), -- Problem Solving
-- Job 30: CRM Developer (Salesforce, Full-time)
(30, 2),  -- Java
(30, 8),  -- Cloud Computing
(30, 9);  -- Communication


INSERT INTO Applications (StudentID, JobPostingID, ApplicationDate, Status)
VALUES
-- Student 1 applies to 3 jobs
(1, 1, '2023-10-16', 'Applied'),  -- Software Engineer, Google
(1, 2, '2023-10-17', 'Applied'),  -- Data Science Intern, Google
(1, 5, '2023-10-18', 'Applied'),  -- Backend Developer, Amazon
-- Student 2 applies to 3 jobs
(2, 3, '2023-10-16', 'Applied'),  -- Cloud Engineer, Microsoft
(2, 4, '2023-10-17', 'Applied'),  -- Software Developer Intern, Microsoft
(2, 7, '2023-10-18', 'Applied'),  -- iOS Developer, Apple
-- Student 3 applies to 2 jobs
(3, 9, '2023-10-16', 'Applied'),  -- Frontend Developer, Facebook
(3, 10, '2023-10-17', 'Applied'), -- Social Media Intern, Facebook
-- Student 4 applies to 3 jobs
(4, 11, '2023-10-16', 'Applied'), -- Firmware Engineer, Tesla
(4, 12, '2023-10-17', 'Applied'), -- Robotics Intern, Tesla
(4, 13, '2023-10-18', 'Applied'), -- Database Administrator, IBM
-- Student 5 applies to 2 jobs
(5, 14, '2023-10-16', 'Applied'), -- Java Developer, Oracle
(5, 15, '2023-10-17', 'Applied'), -- Hardware Engineer, Intel
-- Student 6 applies to 3 jobs
(6, 16, '2023-11-21', 'Applied'), -- Network Engineer, Cisco
(6, 17, '2023-11-22', 'Applied'), -- Mobile Developer, Samsung
(6, 18, '2023-11-23', 'Applied'), -- Game Developer, Sony
-- Student 7 applies to 2 jobs
(7, 19, '2023-11-21', 'Applied'), -- Quality Engineer, Toyota
(7, 20, '2023-11-22', 'Applied'), -- Automotive Software Engineer, BMW
-- Student 8 applies to 3 jobs
(8, 21, '2023-11-21', 'Applied'), -- Data Analyst, Mercedes-Benz
(8, 22, '2023-11-22', 'Applied'), -- Marketing Specialist, Coca-Cola
(8, 23, '2023-11-23', 'Applied'), -- Supply Chain Analyst, PepsiCo
-- Student 9 applies to 2 jobs
(9, 24, '2023-12-06', 'Applied'), -- IT Support Specialist, McDonald's
(9, 25, '2023-12-07', 'Applied'), -- Store Manager, Walmart
-- Student 10 applies to 3 jobs
(10, 26, '2023-12-06', 'Applied'), -- Retail Analyst Intern, Target
(10, 27, '2023-12-07', 'Applied'), -- Aerospace Engineer, SpaceX
(10, 28, '2023-12-08', 'Applied'), -- Content Developer, Netflix
-- Student 11 applies to 2 jobs
(11, 29, '2023-12-06', 'Applied'), -- UI/UX Designer, Adobe
(11, 30, '2023-12-07', 'Applied'), -- CRM Developer, Salesforce
-- Student 12 applies to 3 jobs
(12, 1, '2024-01-11', 'Applied'),  -- Software Engineer, Google
(12, 3, '2024-01-12', 'Applied'), -- Cloud Engineer, Microsoft
(12, 5, '2024-01-13', 'Applied'), -- Backend Developer, Amazon
-- Student 13 applies to 2 jobs
(13, 7, '2024-01-11', 'Applied'),  -- iOS Developer, Apple
(13, 9, '2024-01-12', 'Applied'),  -- Frontend Developer, Facebook
-- Student 14 applies to 3 jobs
(14, 11, '2024-02-26', 'Applied'), -- Firmware Engineer, Tesla
(14, 13, '2024-02-27', 'Applied'), -- Database Administrator, IBM
(14, 15, '2024-02-28', 'Applied'), -- Hardware Engineer, Intel
-- Student 15 applies to 2 jobs
(15, 17, '2024-02-26', 'Applied'), -- Mobile Developer, Samsung
(15, 19, '2024-02-27', 'Applied'), -- Quality Engineer, Toyota
-- Students 16-20 apply to jobs (continuing pattern)
(16, 1, '2023-10-16', 'Applied'),  -- Software Engineer, Google
(16, 2, '2023-10-17', 'Applied'),  -- Data Science Intern, Google
(17, 3, '2023-11-21', 'Applied'),  -- Cloud Engineer, Microsoft
(17, 4, '2023-11-22', 'Applied'),  -- Software Developer Intern, Microsoft
(18, 5, '2023-12-06', 'Applied'),  -- Backend Developer, Amazon
(18, 6, '2023-12-07', 'Applied'),  -- Machine Learning Intern, Amazon
(19, 7, '2024-01-11', 'Applied'),  -- iOS Developer, Apple
(19, 8, '2024-01-12', 'Applied'),  -- UX Design Intern, Apple
(20, 9, '2024-02-26', 'Applied'),  -- Frontend Developer, Facebook
(20, 10, '2024-02-27', 'Applied'); -- Social Media Intern, Facebook


-- checking 


--SELECT a.JobPostingID, jp.Title, COUNT(*) AS Applications
--FROM Applications a
--JOIN JobPostings jp ON a.JobPostingID = jp.JobPostingID
--GROUP BY a.JobPostingID, jp.Title
--ORDER BY a.JobPostingID;


--SELECT StudentID, COUNT(*) AS Applications
--FROM Applications
--GROUP BY StudentID
--ORDER BY StudentID;


--SELECT COUNT(*) AS ApplicationCount FROM Applications;
--SELECT ApplicationID, StudentID, JobPostingID
--FROM Applications
--ORDER BY ApplicationID;


--SELECT ApplicationID
--FROM Applications
--WHERE ApplicationID IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
--                        21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 35, 36, 37, 38,
--                        39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50);



INSERT INTO Interviews (ApplicationID, RecruiterID, DateTime, Status)
VALUES
-- Google (CompanyID 1, RecruiterID 81, JobPostingID 1, 2)
(1, 81, '2023-10-18 10:00:00', 'Scheduled'),  -- Student 1, Software Engineer
(2, 81, '2023-10-18 11:00:00', 'Scheduled'),  -- Student 1, Data Science Intern
(29, 81, '2024-01-11 12:00:00', 'Scheduled'), -- Student 12, Software Engineer
-- Microsoft (CompanyID 2, RecruiterID 82, JobPostingID 3, 4)
(3, 82, '2023-10-18 14:00:00', 'Scheduled'),  -- Student 2, Cloud Engineer
(4, 82, '2023-10-18 15:00:00', 'Scheduled'),  -- Student 2, Software Developer Intern
(30, 82, '2024-01-12 10:00:00', 'Scheduled'), -- Student 12, Cloud Engineer
-- Amazon (CompanyID 3, RecruiterID 83, JobPostingID 5, 6)
(5, 83, '2023-10-20 09:00:00', 'Scheduled'),  -- Student 1, Backend Developer
(6, 83, '2023-10-20 10:00:00', 'Cancelled'),  -- Student 1, Machine Learning Intern
-- Apple (CompanyID 4, RecruiterID 84, JobPostingID 7, 8)
(7, 84, '2023-10-20 11:00:00', 'Scheduled'),  -- Student 2, iOS Developer
(8, 84, '2023-10-20 12:00:00', 'Completed'),  -- Student 2, UX Design Intern
-- Facebook (CompanyID 5, RecruiterID 85, JobPostingID 9, 10)
(9, 85, '2023-10-20 13:00:00', 'Scheduled'),  -- Student 3, Frontend Developer
(10, 85, '2023-10-20 14:00:00', 'Scheduled'), -- Student 3, Social Media Intern
-- Tesla (CompanyID 6, RecruiterID 81, JobPostingID 11, 12)
(11, 81, '2023-10-21 10:00:00', 'Scheduled'), -- Student 4, Firmware Engineer
(12, 81, '2023-10-21 11:00:00', 'Scheduled'), -- Student 4, Robotics Intern
-- IBM (CompanyID 7, RecruiterID 82, JobPostingID 13)
(13, 82, '2023-10-21 12:00:00', 'Scheduled'), -- Student 4, Database Administrator
-- Oracle (CompanyID 8, RecruiterID 83, JobPostingID 14)
(14, 83, '2023-10-21 13:00:00', 'Scheduled'), -- Student 5, Java Developer
-- Cisco (CompanyID 10, RecruiterID 84, JobPostingID 16)
(15, 84, '2023-11-24 11:00:00', 'Scheduled'), -- Student 5, Network Engineer
-- Samsung (CompanyID 11, RecruiterID 85, JobPostingID 17)
(16, 85, '2023-11-24 13:00:00', 'Scheduled'), -- Student 6, Mobile Developer
-- Toyota (CompanyID 13, RecruiterID 81, JobPostingID 19)
(17, 81, '2023-11-25 10:00:00', 'Scheduled'), -- Student 6, Quality Engineer
-- BMW (CompanyID 14, RecruiterID 82, JobPostingID 20)
(18, 82, '2023-11-25 11:00:00', 'Scheduled'), -- Student 6, Automotive Software Engineer
-- Mercedes-Benz (CompanyID 15, RecruiterID 83, JobPostingID 21)
(19, 83, '2023-11-25 12:00:00', 'Scheduled'), -- Student 7, Data Analyst
-- Coca-Cola (CompanyID 16, RecruiterID 84, JobPostingID 22)
(20, 84, '2023-11-25 13:00:00', 'Scheduled'), -- Student 7, Marketing Specialist
-- PepsiCo (CompanyID 17, RecruiterID 85, JobPostingID 23)
(21, 85, '2023-12-09 10:00:00', 'Scheduled'), -- Student 8, Supply Chain Analyst
-- McDonald's (CompanyID 18, RecruiterID 81, JobPostingID 24)
(22, 81, '2023-12-09 11:00:00', 'Scheduled'), -- Student 8, IT Support Specialist
-- Walmart (CompanyID 19, RecruiterID 82, JobPostingID 25)
(23, 82, '2023-12-09 12:00:00', 'Scheduled'), -- Student 8, Store Manager
-- Target (CompanyID 20, RecruiterID 83, JobPostingID 26)
(24, 83, '2023-12-09 13:00:00', 'Scheduled'), -- Student 9, Retail Analyst Intern
-- SpaceX (CompanyID 21, RecruiterID 84, JobPostingID 27)
(25, 84, '2024-01-14 10:00:00', 'Scheduled'), -- Student 9, Aerospace Engineer
-- Netflix (CompanyID 22, RecruiterID 85, JobPostingID 28)
(26, 85, '2024-01-14 11:00:00', 'Scheduled'), -- Student 9, Content Developer
-- Adobe (CompanyID 23, RecruiterID 81, JobPostingID 29)
(27, 81, '2024-01-14 12:00:00', 'Completed'), -- Student 10, UI/UX Designer
-- Salesforce (CompanyID 24, RecruiterID 82, JobPostingID 30)
(28, 82, '2024-01-14 13:00:00', 'Scheduled'); -- Student 10, CRM Developer


UPDATE Applications
SET Status = 'Interviewed'
WHERE ApplicationID IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                        21, 22, 23, 24, 25, 26, 27, 28, 29, 30);


--						-- checking 
--SELECT i.InterviewID, i.ApplicationID, a.StudentID, jp.Title, c.Name AS CompanyName, u.Name AS RecruiterName
--FROM Interviews i
--JOIN Applications a ON i.ApplicationID = a.ApplicationID
--JOIN JobPostings jp ON a.JobPostingID = jp.JobPostingID
--JOIN Companies c ON jp.CompanyID = c.CompanyID
--JOIN Users u ON i.RecruiterID = u.UserID
--ORDER BY i.InterviewID;

INSERT INTO Applications (StudentID, JobPostingID, ApplicationDate, Status)
VALUES
-- Students 21-80 applying to jobs
(21, 1, '2023-10-16', 'Applied'), (21, 2, '2023-10-17', 'Applied'),
(22, 3, '2023-10-16', 'Applied'), (22, 4, '2023-10-17', 'Applied'), (22, 5, '2023-10-18', 'Applied'),
(23, 6, '2023-10-16', 'Applied'), (23, 7, '2023-10-17', 'Applied'),
(24, 8, '2023-10-16', 'Applied'), (24, 9, '2023-10-17', 'Applied'), (24, 10, '2023-10-18', 'Applied'),
(25, 11, '2023-11-21', 'Applied'), (25, 12, '2023-11-22', 'Applied'),
(26, 13, '2023-11-21', 'Applied'), (26, 14, '2023-11-22', 'Applied'), (26, 15, '2023-11-23', 'Applied'),
(27, 16, '2023-11-21', 'Applied'), (27, 17, '2023-11-22', 'Applied'),
(28, 18, '2023-12-06', 'Applied'), (28, 19, '2023-12-07', 'Applied'), (28, 20, '2023-12-08', 'Applied'),
(29, 21, '2023-12-06', 'Applied'), (29, 22, '2023-12-07', 'Applied'),
(30, 23, '2023-12-06', 'Applied'), (30, 24, '2023-12-07', 'Applied'), (30, 25, '2023-12-08', 'Applied'),
(31, 26, '2024-01-11', 'Applied'), (31, 27, '2024-01-12', 'Applied'),
(32, 28, '2024-01-11', 'Applied'), (32, 29, '2024-01-12', 'Applied'), (32, 30, '2024-01-13', 'Applied'),
(33, 1, '2024-01-11', 'Applied'), (33, 2, '2024-01-12', 'Applied'),
(34, 3, '2024-02-26', 'Applied'), (34, 4, '2024-02-27', 'Applied'), (34, 5, '2024-02-28', 'Applied'),
(35, 6, '2024-02-26', 'Applied'), (35, 7, '2024-02-27', 'Applied'),
(36, 8, '2024-02-26', 'Applied'), (36, 9, '2024-02-27', 'Applied'), (36, 10, '2024-02-28', 'Applied'),
(37, 11, '2023-10-16', 'Applied'), (37, 12, '2023-10-17', 'Applied'),
(38, 13, '2023-10-16', 'Applied'), (38, 14, '2023-10-17', 'Applied'), (38, 15, '2023-10-18', 'Applied'),
(39, 16, '2023-11-21', 'Applied'), (39, 17, '2023-11-22', 'Applied'),
(40, 18, '2023-11-21', 'Applied'), (40, 19, '2023-11-22', 'Applied'), (40, 20, '2023-11-23', 'Applied'),
(41, 21, '2023-12-06', 'Applied'), (41, 22, '2023-12-07', 'Applied'),
(42, 23, '2023-12-06', 'Applied'), (42, 24, '2023-12-07', 'Applied'), (42, 25, '2023-12-08', 'Applied'),
(43, 26, '2024-01-11', 'Applied'), (43, 27, '2024-01-12', 'Applied'),
(44, 28, '2024-01-11', 'Applied'), (44, 29, '2024-01-12', 'Applied'), (44, 30, '2024-01-13', 'Applied'),
(45, 1, '2024-02-26', 'Applied'), (45, 2, '2024-02-27', 'Applied'),
(46, 3, '2024-02-26', 'Applied'), (46, 4, '2024-02-27', 'Applied'), (46, 5, '2024-02-28', 'Applied'),
(47, 6, '2023-10-16', 'Applied'), (47, 7, '2023-10-17', 'Applied'),
(48, 8, '2023-10-16', 'Applied'), (48, 9, '2023-10-17', 'Applied'), (48, 10, '2023-10-18', 'Applied'),
(49, 11, '2023-11-21', 'Applied'), (49, 12, '2023-11-22', 'Applied'),
(50, 13, '2023-11-21', 'Applied'), (50, 14, '2023-11-22', 'Applied'), (50, 15, '2023-11-23', 'Applied'),
(51, 16, '2023-12-06', 'Applied'), (51, 17, '2023-12-07', 'Applied'),
(52, 18, '2023-12-06', 'Applied'), (52, 19, '2023-12-07', 'Applied'), (52, 20, '2023-12-08', 'Applied'),
(53, 21, '2024-01-11', 'Applied'), (53, 22, '2024-01-12', 'Applied'),
(54, 23, '2024-01-11', 'Applied'), (54, 24, '2024-01-12', 'Applied'), (54, 25, '2024-01-13', 'Applied'),
(55, 26, '2024-02-26', 'Applied'), (55, 27, '2024-02-27', 'Applied'),
(56, 28, '2024-02-26', 'Applied'), (56, 29, '2024-02-27', 'Applied'), (56, 30, '2024-02-28', 'Applied');


-- checking 
--SELECT * FROM JobFairEvents;

--SELECT COUNT(*) as number_of FROM Companies;

SELECT b.BoothID, jf.EventDate, c.Name AS CompanyName, b.Location, u.Name AS CoordinatorName
FROM Booths b
JOIN JobFairEvents jf ON b.JobFairID = jf.JobFairID
JOIN Companies c ON b.CompanyID = c.CompanyID
JOIN Users u ON b.CoordinatorID = u.UserID
ORDER BY b.BoothID;


INSERT INTO Booths (JobFairID, CompanyID, Location, CoordinatorID)
VALUES
-- Job Fair 1 (2023-10-15)
(1, 1, 'Booth A1', 88), -- Google
(1, 2, 'Booth A2', 89), -- Microsoft
(1, 3, 'Booth A3', 90), -- Amazon
(1, 4, 'Booth A4', 88), -- Apple
(1, 5, 'Booth A5', 89), -- Facebook
-- Job Fair 2 (2023-11-20)
(2, 6, 'Booth B1', 90), -- Tesla
(2, 7, 'Booth B2', 88), -- IBM
(2, 8, 'Booth B3', 89), -- Oracle
(2, 9, 'Booth B4', 90), -- Intel
(2, 10, 'Booth B5', 88), -- Cisco
-- Job Fair 3 (2023-12-05)
(3, 11, 'Booth C1', 89), -- Samsung
(3, 12, 'Booth C2', 90), -- Sony
(3, 13, 'Booth C3', 88), -- Toyota
(3, 14, 'Booth C4', 89), -- BMW
(3, 15, 'Booth C5', 90), -- Mercedes-Benz
-- Job Fair 4 (2024-01-10)
(4, 16, 'Booth D1', 88), -- Coca-Cola
(4, 17, 'Booth D2', 89), -- PepsiCo
(4, 18, 'Booth D3', 90), -- McDonald's
(4, 19, 'Booth D4', 88), -- Walmart
(4, 20, 'Booth D5', 89), -- Target
-- Job Fair 5 (2024-02-25)
(5, 21, 'Booth E1', 90), -- SpaceX
(5, 22, 'Booth E2', 88), -- Netflix
(5, 23, 'Booth E3', 89), -- Adobe
(5, 24, 'Booth E4', 90), -- Salesforce
(5, 25, 'Booth E5', 88); -- Uber

--- visit Populate 

INSERT INTO Visits (StudentID, BoothID, VisitTime)
VALUES
-- Job Fair 1 (2023-10-15, BoothID 1-5: Google, Microsoft, Amazon, Apple, Facebook)
(1, 1, '2023-10-15 09:30:00'),  -- Student 1 visits Google
(1, 2, '2023-10-15 10:00:00'),  -- Student 1 visits Microsoft
(2, 1, '2023-10-15 09:45:00'),  -- Student 2 visits Google
(2, 3, '2023-10-15 10:15:00'),  -- Student 2 visits Amazon
(3, 1, '2023-10-15 10:30:00'),  -- Student 3 visits Google
(3, 4, '2023-10-15 11:00:00'),  -- Student 3 visits Apple
(4, 2, '2023-10-15 11:15:00'),  -- Student 4 visits Microsoft
(4, 5, '2023-10-15 11:45:00'),  -- Student 4 visits Facebook
(5, 1, '2023-10-15 12:00:00'),  -- Student 5 visits Google
(5, 3, '2023-10-15 12:30:00'),  -- Student 5 visits Amazon
-- Job Fair 2 (2023-11-20, BoothID 6-10: Tesla, IBM, Oracle, Intel, Cisco)
(6, 6, '2023-11-20 10:00:00'),  -- Student 6 visits Tesla
(6, 7, '2023-11-20 10:30:00'),  -- Student 6 visits IBM
(7, 8, '2023-11-20 11:00:00'),  -- Student 7 visits Oracle
(7, 9, '2023-11-20 11:30:00'),  -- Student 7 visits Intel
(8, 6, '2023-11-20 12:00:00'),  -- Student 8 visits Tesla
(8, 10, '2023-11-20 12:30:00'), -- Student 8 visits Cisco
(9, 7, '2023-11-20 13:00:00'),  -- Student 9 visits IBM
(9, 8, '2023-11-20 13:30:00'),  -- Student 9 visits Oracle
(10, 6, '2023-11-20 14:00:00'), -- Student 10 visits Tesla
(10, 9, '2023-11-20 14:30:00'), -- Student 10 visits Intel
-- Job Fair 3 (2023-12-05, BoothID 11-15: Samsung, Sony, Toyota, BMW, Mercedes-Benz)
(11, 11, '2023-12-05 09:30:00'), -- Student 11 visits Samsung
(11, 12, '2023-12-05 10:00:00'), -- Student 11 visits Sony
(12, 13, '2023-12-05 10:30:00'), -- Student 12 visits Toyota
(12, 14, '2023-12-05 11:00:00'), -- Student 12 visits BMW
(13, 11, '2023-12-05 11:30:00'), -- Student 13 visits Samsung
(13, 15, '2023-12-05 12:00:00'), -- Student 13 visits Mercedes-Benz
(14, 12, '2023-12-05 12:30:00'), -- Student 14 visits Sony
(14, 13, '2023-12-05 13:00:00'), -- Student 14 visits Toyota
(15, 14, '2023-12-05 13:30:00'), -- Student 15 visits BMW
(15, 15, '2023-12-05 14:00:00'), -- Student 15 visits Mercedes-Benz
-- Job Fair 4 (2024-01-10, BoothID 16-20: Coca-Cola, PepsiCo, McDonald's, Walmart, Target)
(16, 16, '2024-01-10 10:00:00'), -- Student 16 visits Coca-Cola
(16, 17, '2024-01-10 10:30:00'), -- Student 16 visits PepsiCo
(17, 18, '2024-01-10 11:00:00'), -- Student 17 visits McDonald's
(17, 19, '2024-01-10 11:30:00'), -- Student 17 visits Walmart
(18, 16, '2024-01-10 12:00:00'), -- Student 18 visits Coca-Cola
(18, 20, '2024-01-10 12:30:00'), -- Student 18 visits Target
(19, 17, '2024-01-10 13:00:00'), -- Student 19 visits PepsiCo
(19, 18, '2024-01-10 13:30:00'), -- Student 19 visits McDonald's
(20, 19, '2024-01-10 14:00:00'), -- Student 20 visits Walmart
(20, 20, '2024-01-10 14:30:00'), -- Student 20 visits Target
-- Job Fair 5 (2024-02-25, BoothID 21-25: SpaceX, Netflix, Adobe, Salesforce, Uber)
(21, 21, '2024-02-25 09:30:00'), -- Student 21 visits SpaceX
(21, 22, '2024-02-25 10:00:00'), -- Student 21 visits Netflix
(22, 23, '2024-02-25 10:30:00'), -- Student 22 visits Adobe
(22, 24, '2024-02-25 11:00:00'), -- Student 22 visits Salesforce
(23, 21, '2024-02-25 11:30:00'), -- Student 23 visits SpaceX
(23, 25, '2024-02-25 12:00:00'), -- Student 23 visits Uber
(24, 22, '2024-02-25 12:30:00'), -- Student 24 visits Netflix
(24, 23, '2024-02-25 13:00:00'), -- Student 24 visits Adobe
(25, 24, '2024-02-25 13:30:00'), -- Student 25 visits Salesforce
(25, 25, '2024-02-25 14:00:00'); -- Student 25 visits Uber

DECLARE @VisitCount INT = 1;
DECLARE @StudentID INT;
DECLARE @BoothID INT;
DECLARE @VisitTime DATETIME;

WHILE @VisitCount <= 100
BEGIN
    -- Random StudentID (26 to 80)
    SET @StudentID = 26 + FLOOR(RAND() * 55);
    -- Random BoothID (1 to 25)
    SET @BoothID = 1 + FLOOR(RAND() * 25);
    -- Set VisitTime based on Booth's JobFairID
    SET @VisitTime = DATEADD(MINUTE, FLOOR(RAND() * 480), -- Between 09:00 and 17:00 (480 minutes)
        CASE
            WHEN @BoothID BETWEEN 1 AND 5 THEN '2023-10-15 09:00:00'  -- JobFairID 1
            WHEN @BoothID BETWEEN 6 AND 10 THEN '2023-11-20 09:00:00' -- JobFairID 2
            WHEN @BoothID BETWEEN 11 AND 15 THEN '2023-12-05 09:00:00' -- JobFairID 3
            WHEN @BoothID BETWEEN 16 AND 20 THEN '2024-01-10 09:00:00' -- JobFairID 4
            ELSE '2024-02-25 09:00:00' -- JobFairID 5
        END);

    -- Avoid duplicates (StudentID, BoothID)
    IF NOT EXISTS (
        SELECT 1 FROM Visits 
        WHERE StudentID = @StudentID AND BoothID = @BoothID
    )
    BEGIN
        INSERT INTO Visits (StudentID, BoothID, VisitTime)
        VALUES (@StudentID, @BoothID, @VisitTime);
        SET @VisitCount = @VisitCount + 1;
    END
END


---- checking 
--SELECT COUNT(*) AS VisitCount FROM Visits;

--SELECT v.BoothID, c.Name AS CompanyName, COUNT(*) AS Visits
--FROM Visits v
--JOIN Booths b ON v.BoothID = b.BoothID
--JOIN Companies c ON b.CompanyID = c.CompanyID
--GROUP BY v.BoothID, c.Name
--ORDER BY v.BoothID;

--SELECT StudentID, COUNT(*) AS Visits
--FROM Visits
--GROUP BY StudentID
--ORDER BY StudentID;

--- Reviews 
INSERT INTO Reviews (StudentID, RecruiterID, Rating, Comment)
VALUES
-- Students from Interviews (StudentID 1-10)
-- Recruiter 81 (Google, CompanyID 1)
(1, 81, 5, 'Very professional and insightful about the role.'),  -- Student 1, interviewed for Software Engineer
(2, 81, 4, 'Helpful, but could explain the process better.'),   -- Student 2
(3, 81, 5, 'Great communication and very encouraging.'),       -- Student 3
(4, 81, 3, 'Okay, but seemed rushed during our talk.'),        -- Student 4
(5, 81, 4, 'Good feedback on my resume.'),                    -- Student 5
-- Recruiter 82 (Microsoft, CompanyID 2)
(2, 82, 5, 'Really supportive and answered all my questions.'), -- Student 2, interviewed for Cloud Engineer
(3, 82, 4, 'Nice, but the interview was too short.'),           -- Student 3
(4, 82, 5, 'Excellent guidance on technical skills.'),          -- Student 4
(6, 82, 3, 'Average interaction, expected more details.'),      -- Student 6
(7, 82, 4, 'Friendly and approachable.'),                      -- Student 7
-- Recruiter 83 (Amazon, CompanyID 3)
(1, 83, 4, 'Good experience, very professional.'),             -- Student 1, interviewed for Backend Developer
(5, 83, 5, 'Amazing recruiter, very thorough.'),               -- Student 5
(6, 83, 2, 'Not very engaging, seemed distracted.'),           -- Student 6
(8, 83, 4, 'Helpful with career advice.'),                     -- Student 8
(9, 83, 5, 'Really made me feel valued.'),                     -- Student 9
-- Recruiter 84 (Apple, CompanyID 4)
(2, 84, 5, 'Fantastic recruiter, very knowledgeable.'),        -- Student 2, interviewed for iOS Developer
(5, 84, 4, 'Good, but could be more responsive.'),             -- Student 5
(7, 84, 5, 'Really helped me prepare for the role.'),          -- Student 7
(9, 84, 3, 'Okay, but interview felt rushed.'),                -- Student 9
(10, 84, 4, 'Supportive and clear communication.'),            -- Student 10
-- Recruiter 85 (Facebook, CompanyID 5)
(3, 85, 5, 'Very encouraging and professional.'),             -- Student 3, interviewed for Frontend Developer
(4, 85, 4, 'Good feedback, but a bit formal.'),                -- Student 4
(6, 85, 5, 'Amazing experience, very motivating.'),            -- Student 6
(8, 85, 3, 'Average, expected more engagement.'),              -- Student 8
(10, 85, 4, 'Helpful and friendly.'),                         -- Student 10
-- Additional Students (11-20) who might have interacted via booths
-- Recruiter 81
(11, 81, 4, 'Met at the booth, very approachable.'),           -- Student 11
(12, 81, 5, 'Great conversation at the job fair.'),            -- Student 12
(13, 81, 3, 'Okay, but didn't provide much info.'),            -- Student 13
(14, 81, 4, 'Friendly and informative.'),                     -- Student 14
(15, 81, 5, 'Really helped me understand the company.'),       -- Student 15
-- Recruiter 82
(11, 82, 4, 'Nice interaction at the Microsoft booth.'),       -- Student 11
(12, 82, 5, 'Very helpful with internship advice.'),           -- Student 12
(13, 82, 3, 'Average, not very engaging.'),                    -- Student 13
(14, 82, 4, 'Good discussion about roles.'),                   -- Student 14
(15, 82, 5, 'Really supportive at the booth.'),                -- Student 15
-- Recruiter 83
(16, 83, 4, 'Met at the Amazon booth, good experience.'),      -- Student 16
(17, 83, 5, 'Very professional and insightful.'),             -- Student 17
(18, 83, 3, 'Okay, but seemed busy.'),                        -- Student 18
(19, 83, 4, 'Helpful with career tips.'),                     -- Student 19
(20, 83, 5, 'Great interaction at the job fair.'),             -- Student 20
-- Recruiter 84
(16, 84, 4, 'Nice talk at the Apple booth.'),                  -- Student 16
(17, 84, 5, 'Very knowledgeable about the company.'),          -- Student 17
(18, 84, 3, 'Average, expected more details.'),                -- Student 18
(19, 84, 4, 'Good advice on applying.'),                       -- Student 19
(20, 84, 5, 'Really encouraging.'),                            -- Student 20
-- Recruiter 85
(11, 85, 4, 'Met at the Facebook booth, good chat.'),          -- Student 11
(12, 85, 5, 'Very helpful with social media roles.'),          -- Student 12
(13, 85, 3, 'Okay, but not very engaging.'),                   -- Student 13
(14, 85, 4, 'Friendly and informative.'),                     -- Student 14
(15, 85, 5, 'Great experience at the booth.');                 -- Student 15


-- checking 

--SELECT COUNT(*) AS ReviewCount FROM Reviews;

--SELECT RecruiterID, u.Name AS RecruiterName, COUNT(*) AS Reviews, AVG(CAST(Rating AS FLOAT)) AS AvgRating
--FROM Reviews r
--JOIN Users u ON r.RecruiterID = u.UserID
--GROUP BY RecruiterID, u.Name
--ORDER BY RecruiterID;

--SELECT r.ReviewID, r.StudentID, u.Name AS RecruiterName, r.Rating, r.Comment
--FROM Reviews r
--JOIN Users u ON r.RecruiterID = u.UserID
--ORDER BY r.ReviewID;

-- test Query to test the Existence of the Data 
-- Students who visited a booth and applied to a job from that company
--SELECT s.StudentID, u.Name AS StudentName, c.Name AS CompanyName, jp.Title
--FROM Visits v
--JOIN Booths b ON v.BoothID = b.BoothID
--JOIN Companies c ON b.CompanyID = c.CompanyID
--JOIN Applications a ON a.StudentID = v.StudentID
--JOIN JobPostings jp ON a.JobPostingID = jp.JobPostingID AND jp.CompanyID = c.CompanyID
--JOIN Students s ON a.StudentID = s.StudentID
--JOIN Users u ON s.StudentID = u.UserID
--ORDER BY s.StudentID;


use FASTCareerConnect;
select * from Users;
select * from Applications;
select * from Booths;
select * from Companies
select * from Interviews;
select * from JobFairEvents;
select * from JobPostings;
select * from JobPostingSkills;
select * from Recruiters;
select * from Reviews;
select * from Roles;
select * from Students;
select * from StudentSkills;

INSERT INTO Roles (RoleName) VALUES ('Student');
INSERT INTO Roles (RoleName) VALUES ('Recruiter');
INSERT INTO Roles (RoleName) VALUES ('TPO');
INSERT INTO Roles (RoleName) VALUES ('BoothCoordinator');

