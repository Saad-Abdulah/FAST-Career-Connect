using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fast_Connect_DB_Final_project
{
    public partial class _2 : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private int recruiterID;

        public _2(int recruiterID = 0)
        {
            InitializeComponent();
            this.recruiterID = recruiterID;
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            // No action needed
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // No action needed
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        private void _2_Load(object sender, EventArgs e)
        {
            // Hide dataGridView1 since we don't need it
            dataGridView1.Visible = false;
            
            // Remove dateTimePicker and lblSelect_date from panel1
            panel1.Controls.Remove(dateTimePicker);
            panel1.Controls.Remove(lblSelect_date);
            
            // Move date and time controls to main form
            lblSelect_date.Parent = this;
            dateTimePicker.Parent = this;
            timePicker.Parent = this;
            lblSelectTime.Parent = this;
            
            // Position the controls outside panel1
            lblSelect_date.Location = new Point(410, 115);
            dateTimePicker.Location = new Point(510, 115);
            lblSelectTime.Location = new Point(410, 170);
            timePicker.Location = new Point(510, 170);
            
            // Set default date to today
            dateTimePicker.Value = DateTime.Today;
            
            // Set default time to current time
            timePicker.Value = DateTime.Now;
            
            // Position the allocate button
            btn_Allocate_Slot.Location = new Point(510, 220);
            
            // Load applications with status 'Shortlisted'
            LoadShortlistedApplications();
        }

        private void LoadShortlistedApplications()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if we have any applied applications
                    string checkQuery = "SELECT COUNT(*) FROM Applications WHERE Status = 'Applied'";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    int count = (int)checkCmd.ExecuteScalar();

                    // We don't need to update applications since we're looking for Applied status now
                    // Use a query to get applied applications
                    string query = @"
                        SELECT 
                            ApplicationID,
                            StudentID,
                            JobPostingID,
                            ApplicationDate,
                            Status
                        FROM 
                            Applications
                        WHERE 
                            Status = 'Shortlisted'";

                    // Filter by recruiter's company if recruiter is logged in
                    if (recruiterID > 0)
                    {
                        query += @" AND JobPostingID IN (
                                    SELECT JobPostingID 
                                    FROM JobPostings 
                                    WHERE CompanyID = (
                                        SELECT CompanyID 
                                        FROM Recruiters 
                                        WHERE RecruiterID = @RecruiterID))";
                    }

                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (recruiterID > 0)
                    {
                        cmd.Parameters.AddWithValue("@RecruiterID", recruiterID);
                    }

                    // Execute a direct count query to verify
                    string directCountQuery = "SELECT COUNT(*) FROM Applications WHERE Status = 'Applied'";
                    SqlCommand directCountCmd = new SqlCommand(directCountQuery, conn);
                    int directCount = (int)directCountCmd.ExecuteScalar();

                    if (recruiterID > 0)
                    {
                        // Check if there are matching job postings for this recruiter
                        string jobCheckQuery = @"
                            SELECT COUNT(*) 
                            FROM JobPostings 
                            WHERE CompanyID = (
                                SELECT CompanyID 
                                FROM Recruiters 
                                WHERE RecruiterID = @RecruiterID)";
                        SqlCommand jobCheckCmd = new SqlCommand(jobCheckQuery, conn);
                        jobCheckCmd.Parameters.AddWithValue("@RecruiterID", recruiterID);
                        int jobCount = (int)jobCheckCmd.ExecuteScalar();
                    }

                    // Check detailed information about applied applications
                    string appDetailsQuery = @"
                        SELECT a.ApplicationID, a.JobPostingID, j.CompanyID 
                        FROM Applications a
                        JOIN JobPostings j ON a.JobPostingID = j.JobPostingID
                        WHERE a.Status = 'Applied'";
                    SqlCommand appDetailsCmd = new SqlCommand(appDetailsQuery, conn);
                    SqlDataAdapter appDetailsAdapter = new SqlDataAdapter(appDetailsCmd);
                    DataTable appDetailsTable = new DataTable();
                    appDetailsAdapter.Fill(appDetailsTable);
                    
                    // If recruiter is logged in, check their company ID
                    if (recruiterID > 0)
                    {
                        string recruiterCompanyQuery = "SELECT CompanyID FROM Recruiters WHERE RecruiterID = @RecruiterID";
                        SqlCommand recruiterCompanyCmd = new SqlCommand(recruiterCompanyQuery, conn);
                        recruiterCompanyCmd.Parameters.AddWithValue("@RecruiterID", recruiterID);
                        object companyResult = recruiterCompanyCmd.ExecuteScalar();
                        
                        if (companyResult != null)
                        {
                            int companyID = Convert.ToInt32(companyResult);
                            
                            // Now check which applications match this company
                            string matchingAppsQuery = @"
                                SELECT COUNT(*) 
                                FROM Applications a
                                JOIN JobPostings j ON a.JobPostingID = j.JobPostingID
                                WHERE a.Status = 'Applied' AND j.CompanyID = @CompanyID";
                            SqlCommand matchingAppsCmd = new SqlCommand(matchingAppsQuery, conn);
                            matchingAppsCmd.Parameters.AddWithValue("@CompanyID", companyID);
                            int matchingCount = (int)matchingAppsCmd.ExecuteScalar();
                        }
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Clear panel contents
                    panel1.Controls.Clear();

                    // Add "Applied Applications" label
                    Label lblTitle = new Label();
                    lblTitle.Text = "Applied Applications:";
                    lblTitle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
                    lblTitle.AutoSize = true;
                    lblTitle.Location = new Point(10, 10);
                    panel1.Controls.Add(lblTitle);

                    if (dt.Rows.Count > 0)
                    {
                        // Create a DataGridView to display applications
                        DataGridView dgvApplications = new DataGridView();
                        dgvApplications.Location = new Point(10, 40);
                        dgvApplications.Size = new Size(350, 180);
                        dgvApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgvApplications.ReadOnly = true;
                        dgvApplications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dgvApplications.RowHeadersVisible = false;
                        dgvApplications.DataSource = dt;
                        
                        // Add to panel
                        panel1.Controls.Add(dgvApplications);
                        
                        // Enable the allocate button
                        btn_Allocate_Slot.Enabled = true;
                    }
                    else
                    {
                        // No applications found
                        Label lblNoApps = new Label();
                        lblNoApps.Text = "No applied applications found.";
                        lblNoApps.ForeColor = Color.Red;
                        lblNoApps.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                        lblNoApps.Location = new Point(10, 40);
                        lblNoApps.AutoSize = true;
                        panel1.Controls.Add(lblNoApps);

                        // Disable allocate button
                        btn_Allocate_Slot.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Removed MessageBox.Show instruction
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btn_Allocate_Slot_Click(object sender, EventArgs e)
        {
            // Find the DataGridView in the panel
            DataGridView dgvApplications = null;
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is DataGridView)
                {
                    dgvApplications = (DataGridView)ctrl;
                    break;
                }
            }

            // Check if we have a selected application
            if (dgvApplications != null && dgvApplications.SelectedRows.Count > 0)
            {
                // Get the selected application details
                DataGridViewRow selectedRow = dgvApplications.SelectedRows[0];
                int applicationID = Convert.ToInt32(selectedRow.Cells["ApplicationID"].Value);
                int studentID = Convert.ToInt32(selectedRow.Cells["StudentID"].Value);
                int jobPostingID = Convert.ToInt32(selectedRow.Cells["JobPostingID"].Value);
                
                // Get the selected date and time
                DateTime dateTime = dateTimePicker.Value.Date.Add(timePicker.Value.TimeOfDay);

                // Check if there's already an interview scheduled at this time
                bool isTimeSlotAvailable = CheckTimeSlotAvailability(dateTime);
                
                if (!isTimeSlotAvailable)
                {
                    MessageBox.Show("There is already an interview scheduled at this date and time. Please select a different time.", 
                        "Scheduling Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get student name and job title for display purposes
                string studentName = "";
                string jobTitle = "";
                
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        
                        // Get student name
                        string studentQuery = @"
                            SELECT u.Name 
                            FROM Users u
                            WHERE u.UserID = @StudentID";
                        SqlCommand studentCmd = new SqlCommand(studentQuery, conn);
                        studentCmd.Parameters.AddWithValue("@StudentID", studentID);
                        object studentResult = studentCmd.ExecuteScalar();
                        if (studentResult != null)
                            studentName = studentResult.ToString();
                        
                        // Get job title
                        string jobQuery = @"
                            SELECT Title 
                            FROM JobPostings
                            WHERE JobPostingID = @JobPostingID";
                        SqlCommand jobCmd = new SqlCommand(jobQuery, conn);
                        jobCmd.Parameters.AddWithValue("@JobPostingID", jobPostingID);
                        object jobResult = jobCmd.ExecuteScalar();
                        if (jobResult != null)
                            jobTitle = jobResult.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error getting application details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    studentName = "Student ID: " + studentID;
                    jobTitle = "Job ID: " + jobPostingID;
                }

                // Insert the interview record
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlTransaction transaction = conn.BeginTransaction();

                        try
                        {
                            // Insert into Interviews table
                            string insertQuery = @"
                                INSERT INTO Interviews (ApplicationID, RecruiterID, DateTime, Status)
                                VALUES (@ApplicationID, @RecruiterID, @DateTime, 'Scheduled')";

                            SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction);
                            insertCmd.Parameters.AddWithValue("@ApplicationID", applicationID);
                            insertCmd.Parameters.AddWithValue("@RecruiterID", recruiterID);
                            insertCmd.Parameters.AddWithValue("@DateTime", dateTime);

                            int rowsAffected = insertCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Update application status to 'Interviewed'
                                string updateQuery = @"
                                    UPDATE Applications
                                    SET Status = 'Interviewed'
                                    WHERE ApplicationID = @ApplicationID";

                                SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction);
                                updateCmd.Parameters.AddWithValue("@ApplicationID", applicationID);
                                updateCmd.ExecuteNonQuery();

                                // Commit the transaction
                                transaction.Commit();

                                MessageBox.Show($"Interview scheduled successfully for {studentName} for position: {jobTitle}", 
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Reload applications
                                LoadShortlistedApplications();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction on error
                            transaction.Rollback();
                            MessageBox.Show("Error scheduling interview: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an application to schedule an interview.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // New method to check if the time slot is available
        private bool CheckTimeSlotAvailability(DateTime dateTime)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Query to check for interviews at the exact same date and time (hour and minute)
                    string query = @"
                        SELECT COUNT(*) 
                        FROM Interviews 
                        WHERE RecruiterID = @RecruiterID 
                        AND CONVERT(date, DateTime) = CONVERT(date, @DateTime)
                        AND DATEPART(HOUR, DateTime) = DATEPART(HOUR, @DateTime)
                        AND DATEPART(MINUTE, DateTime) = DATEPART(MINUTE, @DateTime)";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@RecruiterID", recruiterID);
                    cmd.Parameters.AddWithValue("@DateTime", dateTime);
                    
                    int count = (int)cmd.ExecuteScalar();
                    
                    // If count is 0, time slot is available
                    return count == 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking time slot availability: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Assume not available on error to be safe
            }
        }
    }
}
