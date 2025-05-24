using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Fast_Connect_DB_Final_project
{
    public partial class Hire : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private int recruiterID;

        public Hire(int recruiterID = 0)
        {
            InitializeComponent();
            this.recruiterID = recruiterID;
        }

        private void Hire_Load(object sender, EventArgs e)
        {
            LoadInterviewedStudents();
        }

        private void LoadInterviewedStudents()
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
                            a.Status,
                            s.GPA,
                            (SELECT AVG(Rating) FROM Reviews WHERE StudentID = a.StudentID) AS AverageRating
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
                            a.Status = 'Interviewed'";
                    
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

                    // Clear existing rows
                    dataGridView1.Rows.Clear();

                    // Populate the DataGridView with data
                    foreach (DataRow row in dt.Rows)
                    {
                        string studentName = row["StudentName"].ToString();
                        string jobTitle = row["JobTitle"].ToString();
                        string companyName = row["CompanyName"].ToString();
                        string status = row["Status"].ToString();
                        string gpa = row["GPA"].ToString();
                        
                        // Handle possible NULL in AverageRating
                        string interviewScore = row.IsNull("AverageRating") ? "N/A" : Math.Round(Convert.ToDouble(row["AverageRating"]), 1).ToString();
                        
                        int studentID = Convert.ToInt32(row["StudentID"]);
                        int jobID = Convert.ToInt32(row["JobPostingID"]);
                        
                        dataGridView1.Rows.Add(studentName, studentID.ToString(), gpa, interviewScore, status, studentID, jobID, jobTitle, companyName);
                    }
                    
                    // Hide columns not needed for display
                    dataGridView1.Columns["StudentID"].Visible = false;
                    dataGridView1.Columns["JobPostingID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading interviewed students: " + ex.Message);
            }
        }

        private void btnHire_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int studentId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["StudentID"].Value);
                int jobId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["JobPostingID"].Value);
                string studentName = dataGridView1.SelectedRows[0].Cells["Student_Name"].Value.ToString();
                string jobTitle = dataGridView1.SelectedRows[0].Cells["JobTitle"].Value.ToString();
                
                // Ask for confirmation
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to hire {studentName} for the position of {jobTitle}?", 
                    "Confirm Hiring", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    HireStudent(studentId, jobId);
                    LoadInterviewedStudents(); // Refresh the DataGridView
                }
            }
            else
            {
                MessageBox.Show("Please select a student to hire.");
            }
        }

        private void HireStudent(int studentId, int jobId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Start a transaction to ensure data consistency
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Update the status of the specific application
                            SqlCommand cmd = new SqlCommand(
                                "UPDATE Applications SET Status = 'Accepted' WHERE StudentID = @StudentID AND JobPostingID = @JobPostingID", 
                                conn, 
                                transaction);
                                
                            cmd.Parameters.AddWithValue("@StudentID", studentId);
                            cmd.Parameters.AddWithValue("@JobPostingID", jobId);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            
                            if (rowsAffected > 0)
                            {
                                // Commit the transaction
                                transaction.Commit();
                                MessageBox.Show("Student has been hired successfully!");
                            }
                            else
                            {
                                // Rollback if no rows were affected
                                transaction.Rollback();
                                MessageBox.Show("Failed to hire student. Application record not found.");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Rollback on error
                            transaction.Rollback();
                            throw new Exception("Error during hiring process: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error hiring student: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
