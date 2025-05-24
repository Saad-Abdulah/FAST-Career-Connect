
-- Functions

CREATE PROCEDURE sp_GetStudentProfile
    @StudentID INT
AS
BEGIN
    SELECT 
        U.Name,
        S.GPA,
        S.DegreeProgram,
        S.CurrentSemester
    FROM 
        Students S
    INNER JOIN 
        Users U ON S.StudentID = U.UserID
    WHERE 
        S.StudentID = @StudentID;
END

CREATE PROCEDURE sp_GetStudentSkills
    @StudentID INT
AS
BEGIN
    SELECT 
        SkillName
    FROM 
        StudentSkills ss
    JOIN 
        Skills s ON ss.SkillID = s.SkillID
    WHERE 
        ss.StudentID = @StudentID;
END

-- =============================================
-- Procedure: sp_CheckExistingStudentProfile
-- Description: Checks if a student profile exists by email
-- =============================================
CREATE PROCEDURE sp_CheckExistingStudentProfile
    @Email VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- First check if the user exists
    DECLARE @UserID INT = NULL;
    SELECT @UserID = UserID FROM Users WHERE Email = @Email;

    IF @UserID IS NULL
    BEGIN
        -- User doesn't exist
        SELECT 0 AS UserExists, 0 AS ProfileExists, NULL AS StudentID, NULL AS Name;
        RETURN;
    END

    -- Check if student profile exists
    DECLARE @ProfileExists BIT = 0;
    SET @ProfileExists = CASE WHEN EXISTS (SELECT 1 FROM Students WHERE StudentID = @UserID) THEN 1 ELSE 0 END;

    -- Get user name
    DECLARE @Name VARCHAR(255);
    SELECT @Name = Name FROM Users WHERE UserID = @UserID;

    -- Return results
    SELECT 1 AS UserExists, @ProfileExists AS ProfileExists, @UserID AS StudentID, @Name AS Name;
END

-- =============================================
-- Procedure: sp_CreateStudentProfile
-- Description: Creates a new student profile, handling both new and existing users
-- =============================================
CREATE PROCEDURE sp_CreateStudentProfile
    @Name VARCHAR(255),
    @Email VARCHAR(255),
    @Password VARCHAR(255) = 'password123',
    @GPA DECIMAL(3,2),
    @DegreeProgram VARCHAR(50),
    @CurrentSemester INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @StudentID INT;
        
        -- Check if user already exists with this email
        SELECT @StudentID = UserID FROM Users WHERE Email = @Email;
        
        IF @StudentID IS NULL
        BEGIN
            -- User doesn't exist, create a new user
            INSERT INTO Users (Name, Email, Password, Role, IsApproved)
            VALUES (@Name, @Email, @Password, 'Student', 1);
            
            SET @StudentID = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            -- User exists, check if they already have a student profile
            IF EXISTS (SELECT 1 FROM Students WHERE StudentID = @StudentID)
            BEGIN
                -- Student profile already exists, update the profile
                UPDATE Students
                SET GPA = @GPA,
                    DegreeProgram = @DegreeProgram,
                    CurrentSemester = @CurrentSemester
                WHERE StudentID = @StudentID;
                
                COMMIT TRANSACTION;
                SELECT @StudentID AS StudentID, 1 AS ProfileAlreadyExists;
                RETURN;
            END
        END
        
        -- Create or update student profile
        INSERT INTO Students (StudentID, GPA, DegreeProgram, CurrentSemester)
        VALUES (@StudentID, @GPA, @DegreeProgram, @CurrentSemester);
        
        COMMIT TRANSACTION;
        SELECT @StudentID AS StudentID, 0 AS ProfileAlreadyExists;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END

-- =============================================
-- Procedure: sp_ManageStudentSkills
-- Description: Manages skills for a student profile 
-- =============================================
CREATE PROCEDURE sp_ManageStudentSkills
    @StudentID INT,
    @SkillsList VARCHAR(MAX) -- Comma separated list of skills
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Delete existing skills for this student to avoid duplicates
        DELETE FROM StudentSkills WHERE StudentID = @StudentID;
        
        -- Process each skill in the comma-separated list
        IF @SkillsList IS NOT NULL AND LEN(@SkillsList) > 0
        BEGIN
            DECLARE @Skill VARCHAR(50);
            DECLARE @Position INT;
            DECLARE @SkillID INT;
            
            WHILE LEN(@SkillsList) > 0
            BEGIN
                -- Extract skill from list
                SET @Position = CHARINDEX(',', @SkillsList);
                
                IF @Position > 0
                BEGIN
                    SET @Skill = LTRIM(RTRIM(LEFT(@SkillsList, @Position - 1)));
                    SET @SkillsList = SUBSTRING(@SkillsList, @Position + 1, LEN(@SkillsList));
                END
                ELSE
                BEGIN
                    SET @Skill = LTRIM(RTRIM(@SkillsList));
                    SET @SkillsList = '';
                END
                
                IF LEN(@Skill) > 0
                BEGIN
                    -- Check if skill exists
                    SELECT @SkillID = SkillID FROM Skills WHERE SkillName = @Skill;
                    
                    -- If not, create it
                    IF @SkillID IS NULL
                    BEGIN
                        INSERT INTO Skills (SkillName) VALUES (@Skill);
                        SET @SkillID = SCOPE_IDENTITY();
                    END
                    
                    -- Add to student's skills
                    INSERT INTO StudentSkills (StudentID, SkillID)
                    VALUES (@StudentID, @SkillID);
                END
            END
        END
        
        COMMIT TRANSACTION;
        SELECT 1 AS Success;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END

CREATE PROCEDURE sp_UpdateStudentProfileWithSkills
    @StudentID INT,
    @Name VARCHAR(255),
    @GPA DECIMAL(3,2),
    @DegreeProgram VARCHAR(50),
    @CurrentSemester INT,
    @Skills NVARCHAR(MAX) -- Comma-separated list of skills
AS
BEGIN
    -- Update Users table for Name
    UPDATE Users
    SET Name = @Name
    WHERE UserID = @StudentID;

    -- Update Students table for academic info
    UPDATE Students
    SET 
        GPA = @GPA,
        DegreeProgram = @DegreeProgram,
        CurrentSemester = @CurrentSemester
    WHERE StudentID = @StudentID;

    -- Delete existing skills
    DELETE FROM StudentSkills WHERE StudentID = @StudentID;

    -- Insert new skills
    DECLARE @SkillName NVARCHAR(50);

    DECLARE SkillCursor CURSOR FOR
    SELECT TRIM(value) FROM STRING_SPLIT(@Skills, ',');

    OPEN SkillCursor;
    FETCH NEXT FROM SkillCursor INTO @SkillName;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO StudentSkills (StudentID, SkillID)
        SELECT @StudentID, SkillID
        FROM Skills
        WHERE SkillName = @SkillName;

        FETCH NEXT FROM SkillCursor INTO @SkillName;
    END;

    CLOSE SkillCursor;
    DEALLOCATE SkillCursor;
END;

-- =============================================
-- Procedure: sp_GetJobsForFair
-- Description: Gets job postings for a specific job fair with filtering options
-- =============================================
CREATE PROCEDURE sp_GetJobsForFair
    @JobFairID INT,
    @JobType VARCHAR(20) = NULL,
    @MinSalary INT = NULL,
    @SkillName VARCHAR(50) = NULL,
    @Location VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT DISTINCT 
        jp.JobPostingID, 
        jp.Title, 
        c.Name AS CompanyName, 
        jp.JobType, 
        jp.SalaryRange, 
        jp.Location
    FROM 
        JobPostings jp
    INNER JOIN 
        Companies c ON jp.CompanyID = c.CompanyID
    INNER JOIN 
        Booths b ON c.CompanyID = b.CompanyID
    LEFT JOIN 
        JobPostingSkills jps ON jp.JobPostingID = jps.JobPostingID
    LEFT JOIN 
        Skills s ON jps.SkillID = s.SkillID
    WHERE 
        b.JobFairID = @JobFairID
        AND (@JobType IS NULL OR jp.JobType = @JobType)
        AND (@MinSalary IS NULL OR CAST(
                ISNULL(
                    NULLIF(
                        SUBSTRING(jp.SalaryRange, 1, PATINDEX('%[^0-9]%', jp.SalaryRange + 'a') - 1), 
                    ''),
                '0') 
            AS INT) >= @MinSalary)
        AND (@SkillName IS NULL OR s.SkillName = @SkillName)
        AND (@Location IS NULL OR jp.Location = @Location)
    ORDER BY 
        jp.Title;
END



-- =============================================
-- Procedure: sp_ApplyForJob
-- Description: Applies for a job with duplicate checking
-- =============================================
CREATE PROCEDURE sp_ApplyForJob
    @StudentID INT,
    @JobPostingID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if student profile exists
    IF NOT EXISTS (SELECT 1 FROM Students WHERE StudentID = @StudentID)
    BEGIN
        -- Return error code 1 - Student not found
        SELECT 1 AS ErrorCode, 'Student profile does not exist' AS ErrorMessage;
        RETURN;
    END
    
    -- Check if job posting exists
    IF NOT EXISTS (SELECT 1 FROM JobPostings WHERE JobPostingID = @JobPostingID)
    BEGIN
        -- Return error code 2 - Job not found
        SELECT 2 AS ErrorCode, 'Job posting does not exist' AS ErrorMessage;
        RETURN;
    END
    
    -- Check if student has already applied for this job
    IF EXISTS (SELECT 1 FROM Applications WHERE StudentID = @StudentID AND JobPostingID = @JobPostingID)
    BEGIN
        -- Return error code 3 - Already applied
        SELECT 3 AS ErrorCode, 'You have already applied for this job' AS ErrorMessage;
        RETURN;
    END
    
    -- Apply for the job
    BEGIN TRY
        INSERT INTO Applications (StudentID, JobPostingID, ApplicationDate, Status)
        VALUES (@StudentID, @JobPostingID, GETDATE(), 'Applied');
        
        -- Success
        SELECT 0 AS ErrorCode, 'Application submitted successfully' AS Message;
    END TRY
    BEGIN CATCH
        -- Return error code 4 - Database error
        SELECT 4 AS ErrorCode, 'Database error: ' + ERROR_MESSAGE() AS ErrorMessage;
    END CATCH
END