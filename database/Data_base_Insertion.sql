
select * from Users

select * from Recruiters

select * from Students

select * from Applications

select * from JobPostings

select * from Companies

select * from Students

select * from Interviews

select * from Reviews

-- insertion 
select * from Companies
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

SET IDENTITY_INSERT Students ON;
select * from Users
select * from Students
-- Insert 80 student records
INSERT INTO Students (StudentID, GPA, DegreeProgram, CurrentSemester)
VALUES
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
(80, 3.40, 'Business Administration', 1),
(81, 3.75, 'Computer Science', 5),
(82, 3.20, 'Electrical Engineering', 3),
(83, 3.90, 'Business Administration', 7),
(84, 2.95, 'Mechanical Engineering', 4),
(85, 3.60, 'Computer Science', 6),
(86, 3.45, 'Civil Engineering', 2),
(87, 3.80, 'Software Engineering', 8),
(88, 3.10, 'Business Administration', 1);



INSERT INTO Recruiters (RecruiterID, CompanyID)
VALUES
(89, 3), -- Recruiter 1 with Google
(90, 4), -- Recruiter 2 with Microsoft
(91, 5), -- Recruiter 3 with Amazon
(92, 6), -- Recruiter 4 with Apple
(93, 7); -- Recruiter 5 with Facebook
select * from Users
-- 
SELECT r.RecruiterID, u.Name, u.Email, c.Name AS CompanyName
FROM Recruiters r
JOIN Users u ON r.RecruiterID = u.UserID
JOIN Companies c ON r.CompanyID = c.CompanyID;

select * from Skills
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
-- Student 9: Technical + Soft skills
(9, 21),  -- Python
(9, 27),  -- Database Management
(9, 22),  -- Web Development
(9, 19),  -- Game Development
-- Student 10: Mixed skills
(10, 17),  -- Java
(10, 24),  -- Machine Learning
(10, 18),  -- Problem Solving
(10, 23),  -- Artificial Intelligence
-- Student 11: Technical focus
(11, 16),  -- C++
(11, 26),  -- Cloud Computing
(11, 25),  -- Data Analysis
(11, 29),  -- UI/UX Design
-- Student 12: Soft + Technical
(12, 27),  -- Database Management
(12, 22),  -- Web Development
(12, 20),  -- Mobile Development (N8N)
(12, 23),  -- Artificial Intelligence
-- Student 13: Broad skills
(13, 21),  -- Python
(13, 17),  -- Java
(13, 18),  -- Problem Solving
(13, 26),  -- Cloud Computing
-- Student 14
(14, 16),  -- C++
(14, 24),  -- Machine Learning
(14, 25),  -- Data Analysis
(14, 30),  -- Software Testing
-- Student 15
(15, 26),  -- Cloud Computing
(15, 20),  -- Mobile Development (N8N)
(15, 19),  -- Game Development
(15, 21),  -- Python
-- Student 16
(16, 21),  -- Python
(16, 27),  -- Database Management
(16, 18),  -- Problem Solving
(16, 24),  -- Machine Learning
-- Student 17
(17, 17),  -- Java
(17, 22),  -- Web Development
(17, 25),  -- Data Analysis
(17, 16),  -- C++
-- Student 18
(18, 16),  -- C++
(18, 24),  -- Machine Learning
(18, 27),  -- Database Management
(18, 29);  -- UI/UX Design


--SELECT StudentID, COUNT(SkillID) AS SkillCount
--FROM StudentSkills
--GROUP BY StudentID
--ORDER BY StudentID;

--SELECT ss.StudentID, s.SkillName
--FROM StudentSkills ss
--JOIN Skills s ON ss.SkillID = s.SkillID
--WHERE ss.StudentID <= 10
--ORDER BY ss.StudentID, s.SkillName;

select * from Companies
INSERT INTO JobPostings (CompanyID, Title, Description, JobType, SalaryRange, Location)
VALUES
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
(24, 'CRM Developer', 'Build customer relationship tools.', 'Full-time', '85,000-105,000', 'San Francisco, CA'),
(25, 'Software Engineer', 'Develop web applications using Python and JavaScript.', 'Full-time', '80,000-100,000', 'Mountain View, CA'),
(25, 'Data Science Intern', 'Analyze datasets to support AI projects.', 'Internship', '30/hr', 'Remote'),
(26, 'Cloud Engineer', 'Design and manage Azure cloud infrastructure.', 'Full-time', '90,000-110,000', 'Redmond, WA'),
(27, 'Software Developer Intern', 'Assist in building Windows applications.', 'Internship', '25/hr', 'Redmond, WA');

select * from JobPostings

INSERT INTO JobPostingSkills (JobPostingID, SkillID)
VALUES
-- Job 3: Software Engineer (Google, Full-time)
(3, 21),  -- Python
(3, 22),  -- Web Development
-- Job 4: Data Science Intern (Google, Internship)
(4, 21),  -- Python
(4, 24),  -- Machine Learning
(4, 25),  -- Data Analysis
-- Job 5: Cloud Engineer (Microsoft, Full-time)
(5, 26),  -- Cloud Computing
(5, 27),  -- SQL (Database Management)
(5, 18),  -- Problem Solving
-- Job 6: Software Developer Intern (Microsoft, Internship)
(6, 17),  -- Java
(6, 22),  -- Web Development
-- Job 7: Backend Developer (Amazon, Full-time)
(7, 21),  -- Python
(7, 27),  -- SQL (Database Management)
(7, 22),  -- Web Development
(7, 18),  -- Problem Solving
-- Job 8: Machine Learning Intern (Amazon, Internship)
(8, 24),  -- Machine Learning
(8, 21),  -- Python
(8, 25),  -- Data Analysis
-- Job 9: iOS Developer (Apple, Full-time)
(9, 20),  -- Mobile Development (N8N)
(9, 16),  -- C++
(9, 18),  -- Problem Solving
-- Job 10: UX Design Intern (Apple, Internship)
(10, 22),  -- Web Development
-- Job 11: Frontend Developer (Facebook, Full-time)
(11, 22),  -- Web Development
(11, 18),  -- Problem Solving
-- Job 12: Social Media Intern (Facebook, Internship)
-- (No valid skills remain after removing Communication, Teamwork, Project Management)
-- Job 13: Firmware Engineer (Tesla, Full-time)
(13, 16),  -- C++
(13, 18),  -- Problem Solving
-- Job 14: Robotics Intern (Tesla, Internship)
(14, 24),  -- Machine Learning
(14, 16),  -- C++
-- Job 15: Database Administrator (IBM, Full-time)
(15, 27),  -- SQL (Database Management)
(15, 26),  -- Cloud Computing
(15, 18),  -- Problem Solving
-- Job 16: Java Developer (Oracle, Full-time)
(16, 17),  -- Java
(16, 22),  -- Web Development
(16, 18),  -- Problem Solving
-- Job 17: Hardware Engineer (Intel, Full-time)
(17, 16),  -- C++
-- Job 18: Network Engineer (Cisco, Full-time)
(18, 26),  -- Cloud Computing
(18, 18),  -- Problem Solving
-- Job 19: Mobile Developer (Samsung, Full-time)
(19, 20),  -- Mobile Development (N8N)
(19, 17),  -- Java
-- Job 20: Game Developer (Sony, Full-time)
(20, 16),  -- C++
(20, 22),  -- Web Development
(20, 18),  -- Problem Solving
-- Job 21: Quality Engineer (Toyota, Full-time)
(21, 18),  -- Problem Solving
-- Job 22: Automotive Software Engineer (BMW, Full-time)
(22, 16),  -- C++
(22, 18),  -- Problem Solving
-- Job 23: Data Analyst (Mercedes-Benz, Full-time)
(23, 25),  -- Data Analysis
(23, 27),  -- SQL (Database Management)
-- Job 24: Marketing Specialist (Coca-Cola, Full-time)
-- (No valid skills remain after removing Communication, Teamwork, Project Management)
-- Job 25: Supply Chain Analyst (PepsiCo, Full-time)
(25, 25),  -- Data Analysis
(25, 18),  -- Problem Solving
-- Job 26: IT Support Specialist (McDonald's, Full-time)
(26, 18),  -- Problem Solving
-- Job 27: Store Manager (Walmart, Full-time)
-- (No valid skills remain after removing Project Management, Communication, Teamwork)
-- Job 28: Retail Analyst Intern (Target, Internship)
(28, 25),  -- Data Analysis
-- Job 29: Aerospace Engineer (SpaceX, Full-time)
(29, 16),  -- C++
(29, 18),  -- Problem Solving
-- Job 30: Content Developer (Netflix, Full-time)
(30, 22),  -- Web Development
-- Job 31: UI/UX Designer (Adobe, Full-time)
(31, 22),  -- Web Development
(31, 18),  -- Problem Solving
-- Job 32: CRM Developer (Salesforce, Full-time)
(32, 17),  -- Java
(32, 26);  -- Cloud Computing

INSERT INTO Applications (StudentID, JobPostingID, ApplicationDate, Status)
VALUES
-- Student 9 applies to 3 jobs (was Student 1)
(9, 3, '2023-10-16', 'Applied'),  -- Software Engineer, Google
(9, 4, '2023-10-17', 'Applied'),  -- Data Science Intern, Google
(9, 7, '2023-10-18', 'Applied'),  -- Backend Developer, Amazon
-- Student 10 applies to 3 jobs (was Student 2)
(10, 5, '2023-10-16', 'Applied'), -- Cloud Engineer, Microsoft
(10, 6, '2023-10-17', 'Applied'), -- Software Developer Intern, Microsoft
(10, 9, '2023-10-18', 'Applied'), -- iOS Developer, Apple
-- Student 11 applies to 2 jobs (was Student 3)
(11, 11, '2023-10-16', 'Applied'), -- Frontend Developer, Facebook
(11, 12, '2023-10-17', 'Applied'), -- Social Media Intern, Facebook
-- Student 12 applies to 3 jobs (was Student 4)
(12, 13, '2023-10-16', 'Applied'), -- Firmware Engineer, Tesla
(12, 14, '2023-10-17', 'Applied'), -- Robotics Intern, Tesla
(12, 15, '2023-10-18', 'Applied'), -- Database Administrator, IBM
-- Student 13 applies to 2 jobs (was Student 5)
(13, 16, '2023-10-16', 'Applied'), -- Java Developer, Oracle
(13, 17, '2023-10-17', 'Applied'), -- Hardware Engineer, Intel
-- Student 14 applies to 3 jobs (was Student 6)
(14, 18, '2023-11-21', 'Applied'), -- Network Engineer, Cisco
(14, 19, '2023-11-22', 'Applied'), -- Mobile Developer, Samsung
(14, 20, '2023-11-23', 'Applied'), -- Game Developer, Sony
-- Student 15 applies to 2 jobs (was Student 7)
(15, 21, '2023-11-21', 'Applied'), -- Quality Engineer, Toyota
(15, 22, '2023-11-22', 'Applied'), -- Automotive Software Engineer, BMW
-- Student 16 applies to 3 jobs (was Student 8)
(16, 23, '2023-11-21', 'Applied'), -- Data Analyst, Mercedes-Benz
(16, 24, '2023-11-22', 'Applied'), -- Marketing Specialist, Coca-Cola
(16, 25, '2023-11-23', 'Applied'), -- Supply Chain Analyst, PepsiCo
-- Student 17 applies to 2 jobs (was Student 9)
(17, 26, '2023-12-06', 'Applied'), -- IT Support Specialist, McDonald's
(17, 27, '2023-12-07', 'Applied'), -- Store Manager, Walmart
-- Student 18 applies to 3 jobs (was Student 10)
(18, 28, '2023-12-06', 'Applied'), -- Retail Analyst Intern, Target
(18, 29, '2023-12-07', 'Applied'), -- Aerospace Engineer, SpaceX
(18, 30, '2023-12-08', 'Applied'), -- Content Developer, Netflix
-- Student 19 applies to 2 jobs (was Student 11)
(19, 31, '2023-12-06', 'Applied'), -- UI/UX Designer, Adobe
(19, 32, '2023-12-07', 'Applied'), -- CRM Developer, Salesforce
-- Student 20 applies to 3 jobs (was Student 12)
(20, 3, '2024-01-11', 'Applied'),  -- Software Engineer, Google
(20, 5, '2024-01-12', 'Applied'),  -- Cloud Engineer, Microsoft
(20, 7, '2024-01-13', 'Applied'),  -- Backend Developer, Amazon
-- Student 21 applies to 2 jobs (was Student 13)
(21, 9, '2024-01-11', 'Applied'),  -- iOS Developer, Apple
(21, 11, '2024-01-12', 'Applied'), -- Frontend Developer, Facebook
-- Student 22 applies to 3 jobs (was Student 14)
(22, 13, '2024-02-26', 'Applied'), -- Firmware Engineer, Tesla
(22, 15, '2024-02-27', 'Applied'), -- Database Administrator, IBM
(22, 17, '2024-02-28', 'Applied'), -- Hardware Engineer, Intel
-- Student 23 applies to 2 jobs (was Student 15)
(23, 19, '2024-02-26', 'Applied'), -- Mobile Developer, Samsung
(23, 21, '2024-02-27', 'Applied'), -- Quality Engineer, Toyota
-- Student 24 applies to 3 jobs (was Student 16)
(24, 3, '2023-10-16', 'Applied'),  -- Software Engineer, Google
(24, 4, '2023-10-17', 'Applied'),  -- Data Science Intern, Google
-- Student 25 applies to 2 jobs (was Student 17)
(25, 5, '2023-11-21', 'Applied'),  -- Cloud Engineer, Microsoft
(25, 6, '2023-11-22', 'Applied'),  -- Software Developer Intern, Microsoft
-- Student 26 applies to 3 jobs (was Student 18)
(26, 7, '2023-12-06', 'Applied'),  -- Backend Developer, Amazon
(26, 8, '2023-12-07', 'Applied'),  -- Machine Learning Intern, Amazon
-- Student 27 applies to 2 jobs (was Student 19)
(27, 9, '2024-01-11', 'Applied'),  -- iOS Developer, Apple
(27, 10, '2024-01-12', 'Applied'), -- UX Design Intern, Apple
-- Student 28 applies to 2 jobs (was Student 20)
(28, 11, '2024-02-26', 'Applied'), -- Frontend Developer, Facebook
(28, 12, '2024-02-27', 'Applied'); -- Social Media Intern, Facebook

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


SELECT ApplicationID
FROM Applications
WHERE ApplicationID IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                        21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 35, 36, 37, 38,
                        39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50);

select * from Recruiters;

INSERT INTO Interviews (ApplicationID, RecruiterID, DateTime, Status)
VALUES
-- Google (CompanyID 1, RecruiterID 89, JobPostingID 3, 4)
(1, 89, '2023-10-18 10:00:00', 'Scheduled'),  -- Student 9, Software Engineer
(2, 89, '2023-10-18 11:00:00', 'Scheduled'),  -- Student 9, Data Science Intern
(19, 89, '2024-01-11 12:00:00', 'Scheduled'), -- Student 20, Software Engineer
-- Microsoft (CompanyID 2, RecruiterID 90, JobPostingID 5, 6)
(4, 90, '2023-10-18 14:00:00', 'Scheduled'),  -- Student 10, Cloud Engineer
(5, 90, '2023-10-18 15:00:00', 'Scheduled'),  -- Student 10, Software Developer Intern
(20, 90, '2024-01-12 10:00:00', 'Scheduled'), -- Student 20, Cloud Engineer
-- Amazon (CompanyID 3, RecruiterID 91, JobPostingID 7, 8)
(3, 91, '2023-10-20 09:00:00', 'Scheduled'),  -- Student 9, Backend Developer
(6, 91, '2023-10-20 10:00:00', 'Cancelled'),  -- Student 10, Machine Learning Intern
-- Apple (CompanyID 4, RecruiterID 92, JobPostingID 9, 10)
(6, 92, '2023-10-20 11:00:00', 'Scheduled'),  -- Student 10, iOS Developer
(8, 92, '2023-10-20 12:00:00', 'Completed'),  -- Student 11, UX Design Intern
-- Facebook (CompanyID 5, RecruiterID 93, JobPostingID 11, 12)
(7, 93, '2023-10-20 13:00:00', 'Scheduled'),  -- Student 11, Frontend Developer
(8, 93, '2023-10-20 14:00:00', 'Scheduled'),  -- Student 11, Social Media Intern
-- Tesla (CompanyID 6, RecruiterID 89, JobPostingID 13, 14)
(9, 89, '2023-10-21 10:00:00', 'Scheduled'),  -- Student 12, Firmware Engineer
(10, 89, '2023-10-21 11:00:00', 'Scheduled'), -- Student 12, Robotics Intern
-- IBM (CompanyID 7, RecruiterID 90, JobPostingID 15)
(11, 90, '2023-10-21 12:00:00', 'Scheduled'), -- Student 12, Database Administrator
-- Oracle (CompanyID 8, RecruiterID 91, JobPostingID 16)
(12, 91, '2023-10-21 13:00:00', 'Scheduled'), -- Student 13, Java Developer
-- Cisco (CompanyID 10, RecruiterID 92, JobPostingID 18)
(14, 92, '2023-11-24 11:00:00', 'Scheduled'), -- Student 14, Network Engineer
-- Samsung (CompanyID 11, RecruiterID 93, JobPostingID 19)
(15, 93, '2023-11-24 13:00:00', 'Scheduled'), -- Student 14, Mobile Developer
-- Toyota (CompanyID 13, RecruiterID 89, JobPostingID 21)
(17, 89, '2023-11-25 10:00:00', 'Scheduled'), -- Student 15, Quality Engineer
-- BMW (CompanyID 14, RecruiterID 90, JobPostingID 22)
(18, 90, '2023-11-25 11:00:00', 'Scheduled'), -- Student 15, Automotive Software Engineer
-- Mercedes-Benz (CompanyID 15, RecruiterID 91, JobPostingID 23)
(19, 91, '2023-11-25 12:00:00', 'Scheduled'), -- Student 16, Data Analyst
-- Coca-Cola (CompanyID 16, RecruiterID 92, JobPostingID 24)
(20, 92, '2023-11-25 13:00:00', 'Scheduled'), -- Student 16, Marketing Specialist
-- PepsiCo (CompanyID 17, RecruiterID 93, JobPostingID 25)
(21, 93, '2023-12-09 10:00:00', 'Scheduled'), -- Student 16, Supply Chain Analyst
-- McDonald's (CompanyID 18, RecruiterID 89, JobPostingID 26)
(22, 89, '2023-12-09 11:00:00', 'Scheduled'), -- Student 17, IT Support Specialist
-- Walmart (CompanyID 19, RecruiterID 90, JobPostingID 27)
(23, 90, '2023-12-09 12:00:00', 'Scheduled'), -- Student 17, Store Manager
-- Target (CompanyID 20, RecruiterID 91, JobPostingID 28)
(24, 91, '2023-12-09 13:00:00', 'Scheduled'), -- Student 18, Retail Analyst Intern
-- SpaceX (CompanyID 21, RecruiterID 92, JobPostingID 29)
(25, 92, '2024-01-14 10:00:00', 'Scheduled'), -- Student 18, Aerospace Engineer
-- Netflix (CompanyID 22, RecruiterID 93, JobPostingID 30)
(26, 93, '2024-01-14 11:00:00', 'Scheduled'), -- Student 18, Content Developer
-- Adobe (CompanyID 23, RecruiterID 89, JobPostingID 31)
(27, 89, '2024-01-14 12:00:00', 'Completed'), -- Student 19, UI/UX Designer
-- Salesforce (CompanyID 24, RecruiterID 90, JobPostingID 32)
(28, 90, '2024-01-14 13:00:00', 'Scheduled'); -- Student 19, CRM Developer


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

select * from Students
select * from JobPostings

INSERT INTO Applications (StudentID, JobPostingID, ApplicationDate, Status)
VALUES
-- Students 21-56 applying to jobs
(21, 3, '2023-10-16', 'Applied'), (21, 4, '2023-10-17', 'Applied'),
(22, 5, '2023-10-16', 'Applied'), (22, 6, '2023-10-17', 'Applied'), (22, 7, '2023-10-18', 'Applied'),
(23, 8, '2023-10-16', 'Applied'), (23, 9, '2023-10-17', 'Applied'),
(24, 10, '2023-10-16', 'Applied'), (24, 11, '2023-10-17', 'Applied'), (24, 12, '2023-10-18', 'Applied'),
(25, 13, '2023-11-21', 'Applied'), (25, 14, '2023-11-22', 'Applied'),
(26, 15, '2023-11-21', 'Applied'), (26, 16, '2023-11-22', 'Applied'), (26, 17, '2023-11-23', 'Applied'),
(27, 18, '2023-11-21', 'Applied'), (27, 19, '2023-11-22', 'Applied'),
(28, 20, '2023-12-06', 'Applied'), (28, 21, '2023-12-07', 'Applied'), (28, 22, '2023-12-08', 'Applied'),
(29, 23, '2023-12-06', 'Applied'), (29, 24, '2023-12-07', 'Applied'),
(30, 25, '2023-12-06', 'Applied'), (30, 26, '2023-12-07', 'Applied'), (30, 27, '2023-12-08', 'Applied'),
(31, 28, '2024-01-11', 'Applied'), (31, 29, '2024-01-12', 'Applied'),
(32, 30, '2024-01-11', 'Applied'), (32, 31, '2024-01-12', 'Applied'), (32, 32, '2024-01-13', 'Applied'),
(33, 3, '2024-01-11', 'Applied'), (33, 4, '2024-01-12', 'Applied'),
(34, 5, '2024-02-26', 'Applied'), (34, 6, '2024-02-27', 'Applied'), (34, 7, '2024-02-28', 'Applied'),
(35, 8, '2024-02-26', 'Applied'), (35, 9, '2024-02-27', 'Applied'),
(36, 10, '2024-02-26', 'Applied'), (36, 11, '2024-02-27', 'Applied'), (36, 12, '2024-02-28', 'Applied'),
(37, 13, '2023-10-16', 'Applied'), (37, 14, '2023-10-17', 'Applied'),
(38, 15, '2023-10-16', 'Applied'), (38, 16, '2023-10-17', 'Applied'), (38, 17, '2023-10-18', 'Applied'),
(39, 18, '2023-11-21', 'Applied'), (39, 19, '2023-11-22', 'Applied'),
(40, 20, '2023-11-21', 'Applied'), (40, 21, '2023-11-22', 'Applied'), (40, 22, '2023-11-23', 'Applied'),
(41, 23, '2023-12-06', 'Applied'), (41, 24, '2023-12-07', 'Applied'),
(42, 25, '2023-12-06', 'Applied'), (42, 26, '2023-12-07', 'Applied'), (42, 27, '2023-12-08', 'Applied'),
(43, 28, '2024-01-11', 'Applied'), (43, 29, '2024-01-12', 'Applied'),
(44, 30, '2024-01-11', 'Applied'), (44, 31, '2024-01-12', 'Applied'), (44, 32, '2024-01-13', 'Applied'),
(45, 3, '2024-02-26', 'Applied'), (45, 4, '2024-02-27', 'Applied'),
(46, 5, '2024-02-26', 'Applied'), (46, 6, '2024-02-27', 'Applied'), (46, 7, '2024-02-28', 'Applied'),
(47, 8, '2023-10-16', 'Applied'), (47, 9, '2023-10-17', 'Applied'),
(48, 10, '2023-10-16', 'Applied'), (48, 11, '2023-10-17', 'Applied'), (48, 12, '2023-10-18', 'Applied'),
(49, 13, '2023-11-21', 'Applied'), (49, 14, '2023-11-22', 'Applied'),
(50, 15, '2023-11-21', 'Applied'), (50, 16, '2023-11-22', 'Applied'), (50, 17, '2023-11-23', 'Applied'),
(51, 18, '2023-12-06', 'Applied'), (51, 19, '2023-12-07', 'Applied'),
(52, 20, '2023-12-06', 'Applied'), (52, 21, '2023-12-07', 'Applied'), (52, 22, '2023-12-08', 'Applied'),
(53, 23, '2024-01-11', 'Applied'), (53, 24, '2024-01-12', 'Applied'),
(54, 25, '2024-01-11', 'Applied'), (54, 26, '2024-01-12', 'Applied'), (54, 27, '2024-01-13', 'Applied'),
(55, 28, '2024-02-26', 'Applied'), (55, 29, '2024-02-27', 'Applied'),
(56, 30, '2024-02-26', 'Applied'), (56, 31, '2024-02-27', 'Applied'), (56, 32, '2024-02-28', 'Applied');

-- checking 
--SELECT * FROM JobFairEvents;

--SELECT COUNT(*) as number_of FROM Companies;

SELECT b.BoothID, jf.EventDate, c.Name AS CompanyName, b.Location, u.Name AS CoordinatorName
FROM Booths b
JOIN JobFairEvents jf ON b.JobFairID = jf.JobFairID
JOIN Companies c ON b.CompanyID = c.CompanyID
JOIN Users u ON b.CoordinatorID = u.UserID
ORDER BY b.BoothID;

select * from Booths
select * from JobFairEvents
INSERT INTO Booths (JobFairID, CompanyID, Location, CoordinatorID)
VALUES
-- Job Fair 1 (2023-10-15)
(6, 3, 'Booth A1', 88), -- Google
(6, 4, 'Booth A2', 89), -- Microsoft
(6, 5, 'Booth A3', 90), -- Amazon
(6, 6, 'Booth A4', 88), -- Apple
(6, 7, 'Booth A5', 89), -- Facebook
-- Job Fair 2 (2023-11-20)
(7, 8, 'Booth B1', 90), -- Tesla
(7, 9, 'Booth B2', 88), -- IBM
(7, 10, 'Booth B3', 89), -- Oracle
(7, 11, 'Booth B4', 90), -- Intel
(7, 12, 'Booth B5', 88), -- Cisco
-- Job Fair 3 (2023-12-05)
(8, 13, 'Booth C1', 89), -- Samsung
(8, 14, 'Booth C2', 90), -- Sony
(8, 15, 'Booth C3', 88), -- Toyota
(8, 16, 'Booth C4', 89), -- BMW
(8, 17, 'Booth C5', 90), -- Mercedes-Benz
-- Job Fair 4 (2024-01-10)
(9, 18, 'Booth D1', 88), -- Coca-Cola
(9, 19, 'Booth D2', 89), -- PepsiCo
(9, 20, 'Booth D3', 90), -- McDonald's
(9, 21, 'Booth D4', 88), -- Walmart
(9, 22, 'Booth D5', 89), -- Target
-- Job Fair 5 (2024-02-25)
(10, 23, 'Booth E1', 90), -- SpaceX
(10, 24, 'Booth E2', 88), -- Netflix
(10, 25, 'Booth E3', 89), -- Adobe
(10, 26, 'Booth E4', 90), -- Salesforce
(10, 27, 'Booth E5', 88); -- Uber

--- visit Populate 

select * from Students
select * from Booths
INSERT INTO Visits (StudentID, BoothID, VisitTime)
VALUES
-- Job Fair 1 (2023-10-15, BoothID 10-14: Google, Microsoft, Amazon, Apple, Facebook)
(9, 10, '2023-10-15 09:30:00'),  -- Student 9 visits Google
(9, 11, '2023-10-15 10:00:00'),  -- Student 9 visits Microsoft
(10, 10, '2023-10-15 09:45:00'), -- Student 10 visits Google
(10, 12, '2023-10-15 10:15:00'), -- Student 10 visits Amazon
(11, 10, '2023-10-15 10:30:00'), -- Student 11 visits Google
(11, 13, '2023-10-15 11:00:00'), -- Student 11 visits Apple
(12, 11, '2023-10-15 11:15:00'), -- Student 12 visits Microsoft
(12, 14, '2023-10-15 11:45:00'), -- Student 12 visits Facebook
(13, 10, '2023-10-15 12:00:00'), -- Student 13 visits Google
(13, 12, '2023-10-15 12:30:00'), -- Student 13 visits Amazon
-- Job Fair 2 (2023-11-20, BoothID 15-19: Tesla, IBM, Oracle, Intel, Cisco)
(14, 15, '2023-11-20 10:00:00'),  -- Student 14 visits Tesla
(14, 16, '2023-11-20 10:30:00'),  -- Student 14 visits IBM
(15, 17, '2023-11-20 11:00:00'),  -- Student 15 visits Oracle
(15, 18, '2023-11-20 11:30:00'),  -- Student 15 visits Intel
(16, 15, '2023-11-20 12:00:00'),  -- Student 16 visits Tesla
(16, 19, '2023-11-20 12:30:00'),  -- Student 16 visits Cisco
(17, 16, '2023-11-20 13:00:00'),  -- Student 17 visits IBM
(17, 17, '2023-11-20 13:30:00'),  -- Student 17 visits Oracle
(18, 15, '2023-11-20 14:00:00'),  -- Student 18 visits Tesla
(18, 18, '2023-11-20 14:30:00'),  -- Student 18 visits Intel
-- Job Fair 3 (2023-12-05, BoothID 20-24: Samsung, Sony, Toyota, BMW, Mercedes-Benz)
(19, 20, '2023-12-05 09:30:00'), -- Student 19 visits Samsung
(19, 21, '2023-12-05 10:00:00'), -- Student 19 visits Sony
(20, 22, '2023-12-05 10:30:00'), -- Student 20 visits Toyota
(20, 23, '2023-12-05 11:00:00'), -- Student 20 visits BMW
(21, 20, '2023-12-05 11:30:00'), -- Student 21 visits Samsung
(21, 24, '2023-12-05 12:00:00'), -- Student 21 visits Mercedes-Benz
(22, 21, '2023-12-05 12:30:00'), -- Student 22 visits Sony
(22, 22, '2023-12-05 13:00:00'), -- Student 22 visits Toyota
(23, 23, '2023-12-05 13:30:00'), -- Student 23 visits BMW
(23, 24, '2023-12-05 14:00:00'), -- Student 23 visits Mercedes-Benz
-- Job Fair 4 (2024-01-10, BoothID 25-29: Coca-Cola, PepsiCo, McDonald's, Walmart, Target)
(24, 25, '2024-01-10 10:00:00'), -- Student 24 visits Coca-Cola
(24, 26, '2024-01-10 10:30:00'), -- Student 24 visits PepsiCo
(25, 27, '2024-01-10 11:00:00'), -- Student 25 visits McDonald's
(25, 28, '2024-01-10 11:30:00'), -- Student 25 visits Walmart
(26, 25, '2024-01-10 12:00:00'), -- Student 26 visits Coca-Cola
(26, 29, '2024-01-10 12:30:00'), -- Student 26 visits Target
(27, 26, '2024-01-10 13:00:00'), -- Student 27 visits PepsiCo
(27, 27, '2024-01-10 13:30:00'), -- Student 27 visits McDonald's
(28, 28, '2024-01-10 14:00:00'), -- Student 28 visits Walmart
(28, 29, '2024-01-10 14:30:00'), -- Student 28 visits Target
-- Job Fair 5 (2024-02-25, BoothID 30-34: SpaceX, Netflix, Adobe, Salesforce, Uber)
(29, 30, '2024-02-25 09:30:00'), -- Student 29 visits SpaceX
(29, 31, '2024-02-25 10:00:00'), -- Student 29 visits Netflix
(30, 32, '2024-02-25 10:30:00'), -- Student 30 visits Adobe
(30, 33, '2024-02-25 11:00:00'), -- Student 30 visits Salesforce
(31, 30, '2024-02-25 11:30:00'), -- Student 31 visits SpaceX
(31, 34, '2024-02-25 12:00:00'), -- Student 31 visits Uber
(32, 31, '2024-02-25 12:30:00'), -- Student 32 visits Netflix
(32, 32, '2024-02-25 13:00:00'), -- Student 32 visits Adobe
(33, 33, '2024-02-25 13:30:00'), -- Student 33 visits Salesforce
(33, 34, '2024-02-25 14:00:00'); -- Student 33 visits Uber


-- Select records within the specified ID ranges
SELECT * FROM Students WHERE StudentID BETWEEN 9 AND 88;
SELECT * FROM Booths WHERE BoothID BETWEEN 10 AND 34;
SELECT * FROM JobFairEvents WHERE JobFairID BETWEEN 6 AND 10;

-- Declare variables for the loop
DECLARE @VisitCount INT = 1;
DECLARE @StudentID INT;
DECLARE @BoothID INT;
DECLARE @VisitTime DATETIME;

-- Insert 100 unique visits
WHILE @VisitCount <= 100
BEGIN
    -- Random StudentID (9 to 88)
    SET @StudentID = 9 + FLOOR(RAND() * 80); -- Range: 9 to 88 (80 possible values)
    
    -- Random BoothID (10 to 34)
    SET @BoothID = 10 + FLOOR(RAND() * 25); -- Range: 10 to 34 (25 possible values)
    
    -- Set VisitTime based on Booth's JobFairID
    SET @VisitTime = DATEADD(MINUTE, FLOOR(RAND() * 480), -- Between 09:00 and 17:00 (480 minutes)
        CASE
            WHEN @BoothID BETWEEN 10 AND 14 THEN '2023-10-15 09:00:00' -- JobFairID 6
            WHEN @BoothID BETWEEN 15 AND 19 THEN '2023-11-20 09:00:00' -- JobFairID 7
            WHEN @BoothID BETWEEN 20 AND 24 THEN '2023-12-05 09:00:00' -- JobFairID 8
            WHEN @BoothID BETWEEN 25 AND 29 THEN '2024-01-10 09:00:00' -- JobFairID 9
            ELSE '2024-02-25 09:00:00' -- JobFairID 10 (BoothID 30-34)
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
-- Select records within the specified ID ranges
SELECT * FROM Students WHERE StudentID BETWEEN 9 AND 88;
SELECT * FROM Recruiters WHERE RecruiterID BETWEEN 89 AND 93;

-- Insert reviews with adjusted StudentID and RecruiterID
INSERT INTO Reviews (StudentID, RecruiterID, Rating, Comment)
VALUES
-- Students from Interviews (StudentID 9-18)
-- Recruiter 89 (Google, CompanyID 1)
(9, 89, 5, 'Very professional and insightful about the role.'),  -- Student 9, interviewed for Software Engineer
(10, 89, 4, 'Helpful, but could explain the process better.'),  -- Student 10
(11, 89, 5, 'Great communication and very encouraging.'),      -- Student 11
(12, 89, 3, 'Okay, but seemed rushed during our talk.'),       -- Student 12
(13, 89, 4, 'Good feedback on my resume.'),                   -- Student 13
-- Recruiter 90 (Microsoft, CompanyID 2)
(10, 90, 5, 'Really supportive and answered all my questions.'), -- Student 10, interviewed for Cloud Engineer
(11, 90, 4, 'Nice, but the interview was too short.'),           -- Student 11
(12, 90, 5, 'Excellent guidance on technical skills.'),         -- Student 12
(14, 90, 3, 'Average interaction, expected more details.'),     -- Student 14
(15, 90, 4, 'Friendly and approachable.'),                     -- Student 15
-- Recruiter 91 (Amazon, CompanyID 3)
(9, 91, 4, 'Good experience, very professional.'),             -- Student 9, interviewed for Backend Developer
(13, 91, 5, 'Amazing recruiter, very thorough.'),              -- Student 13
(14, 91, 2, 'Not very engaging, seemed distracted.'),          -- Student 14
(16, 91, 4, 'Helpful with career advice.'),                    -- Student 16
(17, 91, 5, 'Really made me feel valued.'),                    -- Student 17
-- Recruiter 92 (Apple, CompanyID 4)
(10, 92, 5, 'Fantastic recruiter, very knowledgeable.'),       -- Student 10, interviewed for iOS Developer
(13, 92, 4, 'Good, but could be more responsive.'),            -- Student 13
(15, 92, 5, 'Really helped me prepare for the role.'),         -- Student 15
(17, 92, 3, 'Okay, but interview felt rushed.'),               -- Student 17
(18, 92, 4, 'Supportive and clear communication.'),            -- Student 18
-- Recruiter 93 (Facebook, CompanyID 5)
(11, 93, 5, 'Very encouraging and professional.'),            -- Student 11, interviewed for Frontend Developer
(12, 93, 4, 'Good feedback, but a bit formal.'),               -- Student 12
(14, 93, 5, 'Amazing experience, very motivating.'),           -- Student 14
(16, 93, 3, 'Average, expected more engagement.'),             -- Student 16
(18, 93, 4, 'Helpful and friendly.'),                         -- Student 18
-- Additional Students (19-28) who might have interacted via booths
-- Recruiter 89
(19, 89, 4, 'Met at the booth, very approachable.'),           -- Student 19
(20, 89, 5, 'Great conversation at the job fair.'),            -- Student 20
(21, 89, 3, 'Okay, but didnt provide much info.'),             -- Student 21
(22, 89, 4, 'Friendly and informative.'),                     -- Student 22
(23, 89, 5, 'Really helped me understand the company.'),       -- Student 23
-- Recruiter 90
(19, 90, 4, 'Nice interaction at the Microsoft booth.'),       -- Student 19
(20, 90, 5, 'Very helpful with internship advice.'),           -- Student 20
(21, 90, 3, 'Average, not very engaging.'),                    -- Student 21
(22, 90, 4, 'Good discussion about roles.'),                   -- Student 22
(23, 90, 5, 'Really supportive at the booth.'),                -- Student 23
-- Recruiter 91
(24, 91, 4, 'Met at the Amazon booth, good experience.'),      -- Student 24
(25, 91, 5, 'Very professional and insightful.'),             -- Student 25
(26, 91, 3, 'Okay, but seemed busy.'),                        -- Student 26
(27, 91, 4, 'Helpful with career tips.'),                     -- Student 27
(28, 91, 5, 'Great interaction at the job fair.'),             -- Student 28
-- Recruiter 92
(24, 92, 4, 'Nice talk at the Apple booth.'),                  -- Student 24
(25, 92, 5, 'Very knowledgeable about the company.'),          -- Student 25
(26, 92, 3, 'Average, expected more details.'),                -- Student 26
(27, 92, 4, 'Good advice on applying.'),                       -- Student 27
(28, 92, 5, 'Really encouraging.'),                            -- Student 28
-- Recruiter 93
(19, 93, 4, 'Met at the Facebook booth, good chat.'),          -- Student 19
(20, 93, 5, 'Very helpful with social media roles.'),          -- Student 20
(21, 93, 3, 'Okay, but not very engaging.'),                   -- Student 21
(22, 93, 4, 'Friendly and informative.'),                     -- Student 22
(23, 93, 5, 'Great experience at the booth.');                 -- Student 23