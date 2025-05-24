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

namespace Fast_Connect_DB_Final_project
{
    public partial class Application_Review_Screen : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private int recruiterID;

        public Application_Review_Screen(int recruiterID = 0)
        {
            InitializeComponent();
            this.recruiterID = recruiterID;
        }

        private void Application_Review_Screen_Load(object sender, EventArgs e)
        {
            LoadApplications();
        }

        private void LoadApplications()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Get applications with student and job details
                    string query = @"
                        SELECT 
                            a.ApplicationID, 
                            a.StudentID, 
                            u.Name AS StudentName, 
                            a.JobPostingID, 
                            j.Title AS JobTitle,
                            c.Name AS CompanyName,
                            a.ApplicationDate, 
                            a.Status
                        FROM 
                            Applications a
                        INNER JOIN 
                            Students s ON a.StudentID = s.StudentID
                        INNER JOIN 
                            Users u ON s.StudentID = u.UserID
                        INNER JOIN 
                            JobPostings j ON a.JobPostingID = j.JobPostingID
                        INNER JOIN 
                            Companies c ON j.CompanyID = c.CompanyID
                        WHERE 
                            a.Status = 'Applied'";
                    
                    // If recruiter is logged in, only show applications for their company
                    if (recruiterID > 0)
                    {
                        query += @" AND j.CompanyID = (
                                    SELECT CompanyID 
                                    FROM Recruiters 
                                    WHERE RecruiterID = @RecruiterID)";
                    }
                            
                    query += " ORDER BY a.ApplicationDate DESC";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    
                    if (recruiterID > 0)
                    {
                        cmd.Parameters.AddWithValue("@RecruiterID", recruiterID);
                    }
                    
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    // Clear existing data
                    dataGridView1.Rows.Clear();
                    
                    // Configure the DataGridView's columns if they don't exist
                    if (dataGridView1.Columns.Count == 0 || 
                        !dataGridView1.Columns.Contains("ApplicationID"))
                    {
                        dataGridView1.Columns.Clear();
                        
                        // Add columns in the required order
                        dataGridView1.Columns.Add("ApplicationID", "Application ID");
                        dataGridView1.Columns.Add("StudentID", "Student ID");
                        dataGridView1.Columns.Add("StudentName", "Student Name");
                        dataGridView1.Columns.Add("JobPostingID", "Job ID");
                        dataGridView1.Columns.Add("JobTitle", "Job Title");
                        dataGridView1.Columns.Add("CompanyName", "Company");
                        dataGridView1.Columns.Add("ApplicationDate", "Application Date");
                        dataGridView1.Columns.Add("Status", "Status");
                    }
                    
                    // Populate the DataGridView with data
                    foreach (DataRow row in dt.Rows)
                    {
                        int applicationID = Convert.ToInt32(row["ApplicationID"]);
                        int studentID = Convert.ToInt32(row["StudentID"]);
                        string studentName = row["StudentName"].ToString();
                        int jobID = Convert.ToInt32(row["JobPostingID"]);
                        string jobTitle = row["JobTitle"].ToString();
                        string companyName = row["CompanyName"].ToString();
                        DateTime applicationDate = Convert.ToDateTime(row["ApplicationDate"]);
                        string status = row["Status"].ToString();
                        
                        dataGridView1.Rows.Add(applicationID, studentID, studentName, jobID, jobTitle, companyName, applicationDate.ToString("yyyy-MM-dd"), status);
                    }
                    
                    // Set column visibility and order
                    dataGridView1.Columns["ApplicationID"].Visible = false;
                    dataGridView1.Columns["StudentID"].Visible = false;
                    dataGridView1.Columns["JobPostingID"].Visible = false;
                    
                    // Set column widths for better visibility
                    if (dataGridView1.Columns.Contains("StudentName"))
                        dataGridView1.Columns["StudentName"].Width = 150;
                    if (dataGridView1.Columns.Contains("JobTitle"))
                        dataGridView1.Columns["JobTitle"].Width = 200;
                    if (dataGridView1.Columns.Contains("CompanyName"))
                        dataGridView1.Columns["CompanyName"].Width = 150;
                    if (dataGridView1.Columns.Contains("ApplicationDate"))
                        dataGridView1.Columns["ApplicationDate"].Width = 100;
                    if (dataGridView1.Columns.Contains("Status"))
                        dataGridView1.Columns["Status"].Width = 80;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading applications: " + ex.Message);
            }
        }

        private void btnShortlist_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int applicationId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ApplicationID"].Value);
                UpdateApplicationStatus(applicationId, "Shortlisted");
                LoadApplications(); // Refresh the DataGridView
                MessageBox.Show("Application successfully shortlisted!");
            }
            else
            {
                MessageBox.Show("Please select an application to shortlist.");
            }
        }

        private void btnreject_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int applicationId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ApplicationID"].Value);
                
                // Ask for confirmation
                DialogResult result = MessageBox.Show("Are you sure you want to reject this application?", 
                    "Confirm Rejection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    UpdateApplicationStatus(applicationId, "Rejected");
                    LoadApplications(); // Refresh the DataGridView
                    MessageBox.Show("Application rejected.");
                }
            }
            else
            {
                MessageBox.Show("Please select an application to reject.");
            }
        }

        private void UpdateApplicationStatus(int applicationId, string newStatus)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Applications SET Status = @Status WHERE ApplicationID = @ApplicationID", conn);
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@ApplicationID", applicationId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating application status: " + ex.Message);
            }
        }

        private void btnviewprofile_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int studentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["StudentID"].Value);
                
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"
                            SELECT 
                                u.Name, 
                                s.GPA, 
                                s.DegreeProgram, 
                                s.CurrentSemester,
                                STRING_AGG(sk.SkillName, ', ') AS Skills
                            FROM 
                                Students s
                            INNER JOIN 
                                Users u ON s.StudentID = u.UserID
                            LEFT JOIN 
                                StudentSkills ss ON s.StudentID = ss.StudentID
                            LEFT JOIN 
                                Skills sk ON ss.SkillID = sk.SkillID
                            WHERE 
                                s.StudentID = @StudentID
                            GROUP BY 
                                u.Name, s.GPA, s.DegreeProgram, s.CurrentSemester";
                        
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@StudentID", studentID);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader["Name"].ToString();
                                string gpa = reader["GPA"].ToString();
                                string program = reader["DegreeProgram"].ToString();
                                string semester = reader["CurrentSemester"].ToString();
                                string skills = reader.IsDBNull(reader.GetOrdinal("Skills")) ? "No skills listed" : reader["Skills"].ToString();
                                
                                string message = $"Student Profile:\n\nName: {name}\nGPA: {gpa}\nDegree Program: {program}\nCurrent Semester: {semester}\n\nSkills: {skills}";
                                
                                MessageBox.Show(message, "Student Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Student profile not found.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error viewing student profile: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select an application to view student profile.");
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Select the entire row when a cell is clicked
            if (e.RowIndex >= 0)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }
    }
}
