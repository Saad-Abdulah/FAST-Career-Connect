using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Fast_Connect_DB_Final_project
{
    public partial class Job_Fairs_Explorer : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private int studentID;
        private int selectedJobFairID = 0;
        private int selectedJobID = 0;
        private List<int> studentSkillIDs = new List<int>();
        private List<string> studentSkills = new List<string>();
        
        public Job_Fairs_Explorer(int studentID = 0)
        {
            InitializeComponent();
            this.studentID = studentID;
            
            // Load job types for dropdown
            cmbJobType.Items.Add("All");
            cmbJobType.Items.Add("Internship");
            cmbJobType.Items.Add("Full-time");
            cmbJobType.Items.Add("Part-time");
            cmbJobType.Items.Add("Contract");
            cmbJobType.SelectedIndex = 0;
            
            // Load salary expectations for dropdown
            cmbSalaryExpectation.Items.Add("All");
            cmbSalaryExpectation.Items.Add("40000");
            cmbSalaryExpectation.Items.Add("60000");
            cmbSalaryExpectation.Items.Add("80000");
            cmbSalaryExpectation.Items.Add("100000");
            cmbSalaryExpectation.Items.Add("120000");
            cmbSalaryExpectation.Items.Add("140000");
            cmbSalaryExpectation.Items.Add("160000");
            cmbSalaryExpectation.SelectedIndex = 0;
            
            // Load skills for dropdown
            LoadSkills();
            
            // Load locations for dropdown
            LoadLocations();
            
            // Load student skills and show checkbox if student is logged in
            if (studentID > 0)
            {
                LoadStudentSkills();
                lblStudentSkills.Visible = true;
                panelStudentSkills.Visible = true;
                chkOnlyMatchingSkills.Visible = true;
            }
            else
            {
                lblStudentSkills.Visible = false;
                panelStudentSkills.Visible = false;
                chkOnlyMatchingSkills.Visible = false;
            }
            
            // Load job fairs
            LoadJobFairs();
            
            // Add checkbox event handler
            chkOnlyMatchingSkills.CheckedChanged += new EventHandler(chkOnlyMatchingSkills_CheckedChanged);
        }
        
        private void LoadStudentSkills()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT s.SkillID, s.SkillName
                        FROM StudentSkills ss
                        JOIN Skills s ON ss.SkillID = s.SkillID
                        WHERE ss.StudentID = @StudentID
                        ORDER BY s.SkillName";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int skillID = reader.GetInt32(0);
                                string skillName = reader.GetString(1);
                                studentSkillIDs.Add(skillID);
                                studentSkills.Add(skillName);
                            }
                        }
                    }
                    
                    // Display student skills in the FlowLayoutPanel
                    DisplayStudentSkills();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student skills: " + ex.Message);
            }
        }
        
        private void DisplayStudentSkills()
        {
            panelStudentSkills.Controls.Clear();
            
            if (studentSkills.Count == 0)
            {
                Label lblNoSkills = new Label
                {
                    Text = "No skills found.",
                    AutoSize = true,
                    Margin = new Padding(3),
                    Font = new Font("Microsoft Sans Serif", 9)
                };
                panelStudentSkills.Controls.Add(lblNoSkills);
                return;
            }
            
            foreach (string skill in studentSkills)
            {
                Label lblSkill = new Label
                {
                    Text = skill,
                    AutoSize = false,
                    Size = new Size(120, 25),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(3),
                    BackColor = Color.LightBlue,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Microsoft Sans Serif", 9)
                };
                panelStudentSkills.Controls.Add(lblSkill);
            }
        }
        
        private void LoadSkills()
        {
            try
            {
                cmbSkills.Items.Add("All");
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT DISTINCT SkillName FROM Skills ORDER BY SkillName";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbSkills.Items.Add(reader["SkillName"].ToString());
                            }
                        }
                    }
                }
                
                cmbSkills.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading skills: " + ex.Message);
            }
        }
        
        private void LoadLocations()
        {
            try
            {
                cmbLocation.Items.Add("All");
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT DISTINCT Location FROM JobPostings ORDER BY Location";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbLocation.Items.Add(reader["Location"].ToString());
                            }
                        }
                    }
                }
                
                cmbLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading locations: " + ex.Message);
            }
        }
        
        private void LoadJobFairs()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT JobFairID, EventDate, Venue, EventTime FROM JobFairEvents ORDER BY EventDate DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        DataTable dt = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                        
                        // Format the date and time columns for display
                        dt.Columns.Add("Event_date", typeof(string));
                        dt.Columns.Add("Time", typeof(string));
                        dt.Columns.Add("Event_Venue", typeof(string));
                        
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["EventDate"] != DBNull.Value)
                            {
                                DateTime date = (DateTime)row["EventDate"];
                                row["Event_date"] = date.ToString("dd/MM/yyyy");
                            }
                            
                            if (row["EventTime"] != DBNull.Value)
                            {
                                TimeSpan time = (TimeSpan)row["EventTime"];
                                row["Time"] = time.ToString(@"hh\:mm");
                            }
                            
                            row["Event_Venue"] = row["Venue"];
                        }
                        
                        // Set data source for the grid
                        DataView dv = new DataView(dt);
                        dgvJobFairs.DataSource = dv;
                        
                        // Hide the original columns
                        dgvJobFairs.Columns["JobFairID"].Visible = false;
                        dgvJobFairs.Columns["EventDate"].Visible = false;
                        dgvJobFairs.Columns["Venue"].Visible = false;
                        dgvJobFairs.Columns["EventTime"].Visible = false;
                        
                        // Set column order and properties
                        dgvJobFairs.Columns["Event_date"].DisplayIndex = 0;
                        dgvJobFairs.Columns["Event_Venue"].DisplayIndex = 1;
                        dgvJobFairs.Columns["Time"].DisplayIndex = 2;
                        
                        dgvJobFairs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading job fairs: " + ex.Message);
            }
        }
        
        private void dgvJobFairs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvJobFairs.SelectedRows.Count > 0)
            {
                DataRowView selectedRow = (DataRowView)dgvJobFairs.SelectedRows[0].DataBoundItem;
                selectedJobFairID = Convert.ToInt32(selectedRow["JobFairID"]);
                LoadJobsForFair();
            }
        }
        
        private void LoadJobsForFair(string jobType = null, string salaryExpectation = null, string skillName = null, string location = null, bool onlyMatchingSkills = false)
        {
            if (selectedJobFairID == 0)
            {
                MessageBox.Show("Please select a job fair first.");
                return;
            }
            
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("JobPostingID", typeof(int));
                dt.Columns.Add("Title", typeof(string));
                dt.Columns.Add("CompanyName", typeof(string));
                dt.Columns.Add("JobType", typeof(string));
                dt.Columns.Add("SalaryRange", typeof(string));
                dt.Columns.Add("Location", typeof(string));
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Get companies in this job fair
                    string companyQuery = @"
                        SELECT DISTINCT c.CompanyID
                        FROM Booths b
                        JOIN Companies c ON b.CompanyID = c.CompanyID
                        WHERE b.JobFairID = @JobFairID";
                        
                    List<int> companies = new List<int>();
                    using (SqlCommand cmd = new SqlCommand(companyQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobFairID", selectedJobFairID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                companies.Add(reader.GetInt32(0));
                            }
                        }
                    }
                    
                    if (companies.Count == 0)
                    {
                        MessageBox.Show("No companies found for this job fair.");
                        dgvJobs.DataSource = dt;
                        return;
                    }
                    
                    // Get job postings for these companies
                    foreach (int companyID in companies)
                    {
                        string jobQuery = @"
                            SELECT jp.JobPostingID, jp.Title, c.Name AS CompanyName, jp.JobType, jp.SalaryRange, jp.Location
                            FROM JobPostings jp
                            JOIN Companies c ON jp.CompanyID = c.CompanyID
                            WHERE jp.CompanyID = @CompanyID";
                            
                        using (SqlCommand cmd = new SqlCommand(jobQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@CompanyID", companyID);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    DataRow row = dt.NewRow();
                                    row["JobPostingID"] = reader.GetInt32(0);
                                    row["Title"] = reader.GetString(1);
                                    row["CompanyName"] = reader.GetString(2);
                                    row["JobType"] = reader.GetString(3);
                                    row["SalaryRange"] = reader.IsDBNull(4) ? "" : reader.GetString(4);
                                    row["Location"] = reader.IsDBNull(5) ? "" : reader.GetString(5);
                                    dt.Rows.Add(row);
                                }
                            }
                        }
                    }
                    
                    // Filter results manually 
                    if (!string.IsNullOrEmpty(jobType) && jobType != "All")
                    {
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dt.Rows[i]["JobType"].ToString() != jobType)
                            {
                                dt.Rows.RemoveAt(i);
                            }
                        }
                    }
                    
                    // Filter by salary expectation
                    if (!string.IsNullOrEmpty(salaryExpectation) && salaryExpectation != "All")
                    {
                        int expectedSalary = Convert.ToInt32(salaryExpectation);
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            string salaryRange = dt.Rows[i]["SalaryRange"].ToString();
                            
                            // Parse salary range, e.g. "80,000-100,000" or "30/hr"
                            bool inRange = false;
                            
                            if (salaryRange.Contains("-"))
                            {
                                string[] parts = salaryRange.Split('-');
                                if (parts.Length == 2)
                                {
                                    string minStr = parts[0].Trim().Replace(",", "").Replace(".", "");
                                    string maxStr = parts[1].Trim().Replace(",", "").Replace(".", "");
                                    
                                    int min = 0;
                                    int max = 999999;
                                    
                                    // Extract numbers only
                                    minStr = new string(minStr.Where(char.IsDigit).ToArray());
                                    maxStr = new string(maxStr.Where(char.IsDigit).ToArray());
                                    
                                    if (int.TryParse(minStr, out min) && int.TryParse(maxStr, out max))
                                    {
                                        inRange = (expectedSalary >= min && expectedSalary <= max);
                                    }
                                }
                            }
                            else if (salaryRange.Contains("/"))
                            {
                                // Hourly rate (e.g., "30/hr")
                                string rateStr = salaryRange.Split('/')[0].Trim().Replace(",", "").Replace(".", "");
                                rateStr = new string(rateStr.Where(char.IsDigit).ToArray());
                                
                                if (int.TryParse(rateStr, out int hourlyRate))
                                {
                                    // Convert to approximate annual (hourlyRate * 2080)
                                    int annualEquivalent = hourlyRate * 2080; 
                                    inRange = (expectedSalary <= annualEquivalent);
                                }
                            }
                            
                            if (!inRange)
                            {
                                dt.Rows.RemoveAt(i);
                            }
                        }
                    }
                    
                    // Filter by location
                    if (!string.IsNullOrEmpty(location) && location != "All")
                    {
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dt.Rows[i]["Location"].ToString() != location)
                            {
                                dt.Rows.RemoveAt(i);
                            }
                        }
                    }
                    
                    // Filter by skill
                    if (!string.IsNullOrEmpty(skillName) && skillName != "All")
                    {
                        // Get skill ID
                        int skillID = -1;
                        string skillQuery = "SELECT SkillID FROM Skills WHERE SkillName = @SkillName";
                        using (SqlCommand cmd = new SqlCommand(skillQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@SkillName", skillName);
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                skillID = Convert.ToInt32(result);
                            }
                        }
                        
                        if (skillID != -1)
                        {
                            for (int i = dt.Rows.Count - 1; i >= 0; i--)
                            {
                                int jobID = Convert.ToInt32(dt.Rows[i]["JobPostingID"]);
                                
                                // Check if job requires this skill
                                string jobSkillQuery = "SELECT COUNT(*) FROM JobPostingSkills WHERE JobPostingID = @JobID AND SkillID = @SkillID";
                                using (SqlCommand cmd = new SqlCommand(jobSkillQuery, conn))
                                {
                                    cmd.Parameters.AddWithValue("@JobID", jobID);
                                    cmd.Parameters.AddWithValue("@SkillID", skillID);
                                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                                    
                                    if (count == 0)
                                    {
                                        dt.Rows.RemoveAt(i);
                                    }
                                }
                            }
                        }
                    }
                    
                    // Filter by student skills
                    if (onlyMatchingSkills && studentID > 0 && studentSkillIDs.Count > 0)
                    {
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            int jobID = Convert.ToInt32(dt.Rows[i]["JobPostingID"]);
                            
                            // Get required skills for this job
                            List<int> jobSkills = new List<int>();
                            string jobSkillQuery = "SELECT SkillID FROM JobPostingSkills WHERE JobPostingID = @JobID";
                            using (SqlCommand cmd = new SqlCommand(jobSkillQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@JobID", jobID);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        jobSkills.Add(reader.GetInt32(0));
                                    }
                                }
                            }
                            
                            // Check if student has all required skills
                            bool hasAllSkills = true;
                            foreach (int skillID in jobSkills)
                            {
                                if (!studentSkillIDs.Contains(skillID))
                                {
                                    hasAllSkills = false;
                                    break;
                                }
                            }
                            
                            if (!hasAllSkills)
                            {
                                dt.Rows.RemoveAt(i);
                            }
                        }
                    }
                }
                
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No jobs found matching your criteria.");
                }
                
                dgvJobs.DataSource = dt;
                
                // Set column headers
                if (dgvJobs.Columns.Contains("JobPostingID"))
                    dgvJobs.Columns["JobPostingID"].Visible = false;
                if (dgvJobs.Columns.Contains("Title"))
                    dgvJobs.Columns["Title"].HeaderText = "Title_Job";
                if (dgvJobs.Columns.Contains("CompanyName"))
                    dgvJobs.Columns["CompanyName"].HeaderText = "Company";
                if (dgvJobs.Columns.Contains("JobType"))
                    dgvJobs.Columns["JobType"].HeaderText = "Job_type";
                if (dgvJobs.Columns.Contains("SalaryRange"))
                    dgvJobs.Columns["SalaryRange"].HeaderText = "Salary_job";
                if (dgvJobs.Columns.Contains("Location"))
                    dgvJobs.Columns["Location"].HeaderText = "Location_job";
                
                dgvJobs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading jobs: " + ex.Message);
            }
        }
        
        private void btnApplyFilters_Click(object sender, EventArgs e)
        {
            if (selectedJobFairID == 0)
            {
                MessageBox.Show("Please select a job fair first.");
                return;
            }
            
            string jobType = cmbJobType.SelectedItem.ToString();
            string salaryExpectation = "All";
            
            // Get salary expectation from combo box or text box
            if (cmbSalaryExpectation.Text != "All")
            {
                // Try to parse the input as an integer
                if (int.TryParse(cmbSalaryExpectation.Text, out int salary))
                {
                    salaryExpectation = salary.ToString();
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for salary expectation.");
                    return;
                }
            }
            
            string skill = cmbSkills.SelectedItem.ToString();
            string location = cmbLocation.SelectedItem.ToString();
            bool onlyMatchingSkills = chkOnlyMatchingSkills.Checked;
            
            LoadJobsForFair(jobType, salaryExpectation, skill, location, onlyMatchingSkills);
        }
        
        private void chkOnlyMatchingSkills_CheckedChanged(object sender, EventArgs e)
        {
            // If checked and student is logged in, enable filtering by student skills
            if (chkOnlyMatchingSkills.Checked && studentID == 0)
            {
                MessageBox.Show("You must be logged in as a student to use this filter.");
                chkOnlyMatchingSkills.Checked = false;
            }
        }
        
        private void dgvJobs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvJobs.SelectedRows.Count > 0)
            {
                DataRowView selectedRow = (DataRowView)dgvJobs.SelectedRows[0].DataBoundItem;
                selectedJobID = Convert.ToInt32(selectedRow["JobPostingID"]);
            }
        }
        
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (studentID == 0)
            {
                MessageBox.Show("You must be logged in as a student to apply for jobs.");
                return;
            }
            
            if (dgvJobs.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a job to apply for.");
                return;
            }
            
            try
            {
                // Get selected job ID
                DataRowView selectedRow = (DataRowView)dgvJobs.SelectedRows[0].DataBoundItem;
                int jobPostingID = Convert.ToInt32(selectedRow["JobPostingID"]);
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // First check if already applied
                    string checkQuery = "SELECT COUNT(*) FROM Applications WHERE StudentID = @StudentID AND JobPostingID = @JobPostingID";
                    
                    using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentID);
                        cmd.Parameters.AddWithValue("@JobPostingID", jobPostingID);
                        
                        int count = (int)cmd.ExecuteScalar();
                        
                        if (count > 0)
                        {
                            MessageBox.Show("You have already applied for this job.");
                            return;
                        }
                    }
                    
                    // Insert new application
                    string insertQuery = @"
                        INSERT INTO Applications (StudentID, JobPostingID, ApplicationDate, Status)
                        VALUES (@StudentID, @JobPostingID, @ApplicationDate, 'Applied')";
                        
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentID);
                        cmd.Parameters.AddWithValue("@JobPostingID", jobPostingID);
                        cmd.Parameters.AddWithValue("@ApplicationDate", DateTime.Now.Date);
                        
                        int result = cmd.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            MessageBox.Show("Application submitted successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to submit application. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying for job: " + ex.Message);
            }
        }
        
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Job_Fairs_Explorer_Load(object sender, EventArgs e)
        {
            
        }
        
        private void cmbSalaryExpectation_SelectedIndexChanged(object sender, EventArgs e)
        {
            // This event handler is not needed but included for completeness
        }
    }
}

       