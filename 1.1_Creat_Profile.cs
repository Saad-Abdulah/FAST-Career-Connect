using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;

namespace Fast_Connect_DB_Final_project
{
    public partial class Profile_Student : Form
    {
        private readonly int studentId;
        private readonly string userEmail;
        private string connectionString = DatabaseConfig.ConnectionString;

        public Profile_Student(int studentId = 0, string email = null)
        {
            InitializeComponent();
            this.studentId = studentId;
            this.userEmail = email;
        }

        private void Profile_Student_Load(object sender, EventArgs e)
        {
            if (studentId == 0)
            {
                // Logic for creating a new student profile
                txtName.Clear();
                txtGPA.Clear();
                txtEmail.Text = userEmail ?? string.Empty; // Set email if provided
                txtEmail.Enabled = string.IsNullOrEmpty(userEmail); // Disable editing if email was provided
                comboBoxDegree.SelectedIndex = -1;
                comboBoxSemester.SelectedIndex = -1;
                listBoxSkills.Items.Clear();
                
                // If userEmail is provided, get the name from Users table
                if (!string.IsNullOrEmpty(userEmail))
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "SELECT Name FROM Users WHERE Email = @Email";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@Email", userEmail);
                            object result = cmd.ExecuteScalar();
                            
                            if (result != null)
                            {
                                txtName.Text = result.ToString();
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                // Load existing profile
                LoadProfile();
                LoadSkills();
            }
            PopulateComboBoxes();
            PopulateSkillsComboBox();
        }

        private void LoadProfile()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Updated query to join Users table to get all needed information
                    string query = @"
                        SELECT u.Name, u.Email, s.GPA, s.DegreeProgram, s.CurrentSemester
                        FROM Students s
                        JOIN Users u ON s.StudentID = u.UserID
                        WHERE s.StudentID = @StudentID";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        txtName.Text = reader["Name"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtGPA.Text = reader["GPA"].ToString();
                        comboBoxDegree.Text = reader["DegreeProgram"].ToString();
                        comboBoxSemester.Text = reader["CurrentSemester"].ToString();
                        
                        // Disable email editing for existing profile
                        txtEmail.Enabled = false;
                    }
                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSkills()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT s.SkillName
                        FROM StudentSkills ss
                        JOIN Skills s ON ss.SkillID = s.SkillID
                        WHERE ss.StudentID = @StudentID";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    listBoxSkills.Items.Clear();
                    while (reader.Read())
                    {
                        listBoxSkills.Items.Add(reader["SkillName"].ToString());
                    }
                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateComboBoxes()
        {
            // Populate Degree Programs
            comboBoxDegree.Items.AddRange(new string[] { "Computer Science", "Software Engineering", "Electrical Engineering", "Civil Engineering", "Mechanical Engineering", "Business Administration", "MS(SE)", "MS(CS)", "MS(AI)" });

            // Populate Semesters
            comboBoxSemester.Items.AddRange(new string[] { "1", "2", "3", "4", "5", "6", "7", "8" });
        }

        private void PopulateSkillsComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT SkillName FROM Skills", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    comboBoxSkills.Items.Clear();

                    while (reader.Read())
                    {
                        comboBoxSkills.Items.Add(reader["SkillName"].ToString());
                    }
                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddSkill_Click(object sender, EventArgs e)
        {
            if (comboBoxSkills.SelectedItem != null)
            {
                string selectedSkill = comboBoxSkills.SelectedItem.ToString();
                if (!listBoxSkills.Items.Contains(selectedSkill) && !string.IsNullOrWhiteSpace(selectedSkill))
                {
                    listBoxSkills.Items.Add(selectedSkill);
                }
                else
                {
                    MessageBox.Show("Skill already added or invalid!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select a skill first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRemoveSkill_Click(object sender, EventArgs e)
        {
            if (listBoxSkills.SelectedItem != null)
            {
                listBoxSkills.Items.Remove(listBoxSkills.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please select a skill to remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtName.Text) || 
                string.IsNullOrWhiteSpace(txtEmail.Text) || 
                string.IsNullOrWhiteSpace(txtGPA.Text) || 
                comboBoxDegree.SelectedItem == null || 
                comboBoxSemester.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate email format
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtGPA.Text, out decimal gpa) || gpa < 1.2m || gpa > 4.0m)
            {
                MessageBox.Show("Please enter a valid GPA between 1.2 and 4.0.", "Invalid GPA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create a comma-separated string of skills
            string skills = string.Join(",", listBoxSkills.Items.Cast<string>());

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (studentId > 0)
                    {
                        // Update existing profile
                        UpdateProfile(conn, gpa);
                        SaveSkills(conn, studentId);
                        
                        MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Redirect to student dashboard
                        this.Hide();
                        Form1 studentDash = new Form1(txtEmail.Text);
                        studentDash.Show();
                        return;
                    }
                    
                    // Check if this email already exists
                    SqlCommand checkCmd = new SqlCommand("SELECT UserID FROM Users WHERE Email = @Email", conn);
                    checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    object existingUserId = checkCmd.ExecuteScalar();
                    
                    if (existingUserId != null)
                    {
                        // User exists, check if they already have a student profile
                        int userId = Convert.ToInt32(existingUserId);
                        
                        SqlCommand checkStudentCmd = new SqlCommand("SELECT COUNT(*) FROM Students WHERE StudentID = @StudentID", conn);
                        checkStudentCmd.Parameters.AddWithValue("@StudentID", userId);
                        
                        int studentCount = (int)checkStudentCmd.ExecuteScalar();
                        
                        if (studentCount > 0)
                        {
                            MessageBox.Show("A student profile already exists for this user. You will be redirected to the student dashboard.", 
                                "Profile Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // Redirect to student dashboard
                            this.Hide();
                            Form1 student = new Form1(txtEmail.Text);
                            student.Show();
                            return;
                        }
                        
                        // User exists but doesn't have a student profile - create one for the existing user
                        SqlCommand updateUserCmd = new SqlCommand(
                            "UPDATE Users SET Name = @Name WHERE UserID = @UserID", conn);
                        updateUserCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        updateUserCmd.Parameters.AddWithValue("@UserID", userId);
                        updateUserCmd.ExecuteNonQuery();
                        
                        // Now create the student profile using the existing user ID
                        CreateStudentProfile(conn, userId, gpa);
                        SaveSkills(conn, userId);
                    }
                    else
                    {
                        // Create a new user first
                        SqlCommand createUserCmd = new SqlCommand(
                            "INSERT INTO Users (Name, Email, Password, Role, IsApproved) VALUES (@Name, @Email, @Password, 'Student', 1); SELECT SCOPE_IDENTITY();", conn);
                        createUserCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        createUserCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        createUserCmd.Parameters.AddWithValue("@Password", "password123"); // Default password
                        
                        // Get the new user ID
                        int userId = Convert.ToInt32(createUserCmd.ExecuteScalar());
                        
                        // Now create the student profile
                        CreateStudentProfile(conn, userId, gpa);
                        SaveSkills(conn, userId);
                    }
                    
                    MessageBox.Show("Profile created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Redirect to student dashboard
                    this.Hide();
                    Form1 studentDashboard = new Form1(txtEmail.Text);
                    studentDashboard.Show();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void CreateStudentProfile(SqlConnection conn, int userId, decimal gpa)
        {
            try
            {
                // Turn on IDENTITY_INSERT
                SqlCommand setIdentityInsert = new SqlCommand("SET IDENTITY_INSERT Students ON", conn);
                setIdentityInsert.ExecuteNonQuery();
                
                // Insert student with specific ID
                SqlCommand createStudentCmd = new SqlCommand(
                    "INSERT INTO Students (StudentID, GPA, DegreeProgram, CurrentSemester) VALUES (@StudentID, @GPA, @DegreeProgram, @CurrentSemester)", conn);
                createStudentCmd.Parameters.AddWithValue("@StudentID", userId);
                createStudentCmd.Parameters.AddWithValue("@GPA", gpa);
                createStudentCmd.Parameters.AddWithValue("@DegreeProgram", comboBoxDegree.SelectedItem.ToString());
                createStudentCmd.Parameters.AddWithValue("@CurrentSemester", int.Parse(comboBoxSemester.SelectedItem.ToString()));
                createStudentCmd.ExecuteNonQuery();
                
                // Turn off IDENTITY_INSERT
                setIdentityInsert = new SqlCommand("SET IDENTITY_INSERT Students OFF", conn);
                setIdentityInsert.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 544) // Cannot insert explicit value for identity column error
                {
                    // Try alternate approach without IDENTITY_INSERT
                    SqlCommand alterCmd = new SqlCommand(
                        "INSERT INTO Students (GPA, DegreeProgram, CurrentSemester) VALUES (@GPA, @DegreeProgram, @CurrentSemester); " +
                        "UPDATE Students SET StudentID = @StudentID WHERE GPA = @GPA AND DegreeProgram = @DegreeProgram AND CurrentSemester = @CurrentSemester",
                        conn);
                    alterCmd.Parameters.AddWithValue("@StudentID", userId);
                    alterCmd.Parameters.AddWithValue("@GPA", gpa);
                    alterCmd.Parameters.AddWithValue("@DegreeProgram", comboBoxDegree.SelectedItem.ToString());
                    alterCmd.Parameters.AddWithValue("@CurrentSemester", int.Parse(comboBoxSemester.SelectedItem.ToString()));
                    alterCmd.ExecuteNonQuery();
                }
                else
                {
                    throw; // Re-throw other SQL exceptions
                }
            }
        }
        
        private void UpdateProfile(SqlConnection conn, decimal gpa)
        {
            // Update user information
            SqlCommand updateUserCmd = new SqlCommand(
                "UPDATE Users SET Name = @Name WHERE UserID = @UserID", conn);
            updateUserCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            updateUserCmd.Parameters.AddWithValue("@UserID", studentId);
            updateUserCmd.ExecuteNonQuery();
            
            // Update student profile
            SqlCommand updateStudentCmd = new SqlCommand(
                "UPDATE Students SET GPA = @GPA, DegreeProgram = @DegreeProgram, CurrentSemester = @CurrentSemester WHERE StudentID = @StudentID", conn);
            updateStudentCmd.Parameters.AddWithValue("@StudentID", studentId);
            updateStudentCmd.Parameters.AddWithValue("@GPA", gpa);
            updateStudentCmd.Parameters.AddWithValue("@DegreeProgram", comboBoxDegree.SelectedItem.ToString());
            updateStudentCmd.Parameters.AddWithValue("@CurrentSemester", int.Parse(comboBoxSemester.SelectedItem.ToString()));
            updateStudentCmd.ExecuteNonQuery();
        }
        
        private void SaveSkills(SqlConnection conn, int studentId)
        {
            // First delete existing skills
            SqlCommand deleteSkillsCmd = new SqlCommand(
                "DELETE FROM StudentSkills WHERE StudentID = @StudentID", conn);
            deleteSkillsCmd.Parameters.AddWithValue("@StudentID", studentId);
            deleteSkillsCmd.ExecuteNonQuery();
            
            // Add new skills
            foreach (string skill in listBoxSkills.Items)
            {
                // Check if the skill exists
                SqlCommand findSkillCmd = new SqlCommand(
                    "SELECT SkillID FROM Skills WHERE SkillName = @SkillName", conn);
                findSkillCmd.Parameters.AddWithValue("@SkillName", skill);
                object skillIdObj = findSkillCmd.ExecuteScalar();
                
                int skillId;
                if (skillIdObj == null)
                {
                    // Create new skill
                    SqlCommand createSkillCmd = new SqlCommand(
                        "INSERT INTO Skills (SkillName) VALUES (@SkillName); SELECT SCOPE_IDENTITY();", conn);
                    createSkillCmd.Parameters.AddWithValue("@SkillName", skill);
                    skillId = Convert.ToInt32(createSkillCmd.ExecuteScalar());
                }
                else
                {
                    skillId = Convert.ToInt32(skillIdObj);
                }
                
                // Link skill to student
                SqlCommand addSkillCmd = new SqlCommand(
                    "INSERT INTO StudentSkills (StudentID, SkillID) VALUES (@StudentID, @SkillID)", conn);
                addSkillCmd.Parameters.AddWithValue("@StudentID", studentId);
                addSkillCmd.Parameters.AddWithValue("@SkillID", skillId);
                addSkillCmd.ExecuteNonQuery();
            }
        }

        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Log_in log_in = new Log_in();
            log_in.Show();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtGPA_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtGPA.Text))
            {
                if (!decimal.TryParse(txtGPA.Text, out decimal gpa) || gpa < 1.2m || gpa > 4.0m)
                {
                    MessageBox.Show("Please enter a valid GPA between 1.2 and 4.0.", "Invalid GPA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGPA.Text = "";
                    txtGPA.Focus();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        private void comboBoxDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBoxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDegree.SelectedItem != null && comboBoxSemester.SelectedItem != null)
            {
                string selectedDegree = comboBoxDegree.SelectedItem.ToString();
                int selectedSemester = int.Parse(comboBoxSemester.SelectedItem.ToString());

                if ((selectedDegree == "MS(SE)" || selectedDegree == "MS(CS)" || selectedDegree == "MS(AI)") && selectedSemester > 4)
                {
                    MessageBox.Show("MS Students cannot have more than 4 semesters!", "Invalid Semester", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxSemester.SelectedIndex = -1;
                }
            }
        }

        private void listBoxSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBoxSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBoxSkills_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBoxSkills.SelectedItem != null)
            {
                string selectedSkill = comboBoxSkills.SelectedItem.ToString();
                if (!listBoxSkills.Items.Contains(selectedSkill))
                {
                    listBoxSkills.Items.Add(selectedSkill);
                }
                else
                {
                    MessageBox.Show("Skill already added!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void listBoxSkills_SelectedIndexChanged_1(object sender, EventArgs e)
        {
        }
    }
}