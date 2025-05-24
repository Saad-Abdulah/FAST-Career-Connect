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
    public partial class Interview : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private int studentId;
        private int selectedInterviewId = 0;

        public Interview(int studentId = 0)
        {
            InitializeComponent();
            this.studentId = studentId;
            this.Text = "Manage Interviews";
            LoadInterviews();
        }

        private void LoadInterviews()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            i.InterviewID,
                            c.Name AS CompanyName, 
                            jp.Title AS JobTitle, 
                            jp.SalaryRange,
                            a.ApplicationID, 
                            i.DateTime,
                            u.Name AS StudentName,
                            i.Status,
                            r.RecruiterID,
                            ru.Name AS RecruiterName
                        FROM 
                            Interviews i
                        INNER JOIN 
                            Applications a ON i.ApplicationID = a.ApplicationID
                        INNER JOIN 
                            Students s ON a.StudentID = s.StudentID
                        INNER JOIN 
                            Users u ON s.StudentID = u.UserID
                        INNER JOIN 
                            JobPostings jp ON a.JobPostingID = jp.JobPostingID
                        INNER JOIN 
                            Companies c ON jp.CompanyID = c.CompanyID
                        INNER JOIN
                            Recruiters r ON i.RecruiterID = r.RecruiterID
                        INNER JOIN
                            Users ru ON r.RecruiterID = ru.UserID
                        WHERE 
                            a.StudentID = @StudentID 
                        AND 
                            i.Status = 'Scheduled'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Clear existing columns and set up new ones
                        dataGridView1.Columns.Clear();
                        dataGridView1.AutoGenerateColumns = false;
                        
                        // Increase the grid width to use more screen space
                        dataGridView1.Size = new System.Drawing.Size(630, 284);
                        dataGridView1.Location = new System.Drawing.Point(150, 54);

                        // Add columns with appropriate headers
                        dataGridView1.Columns.Add("CompanyName", "Company");
                        dataGridView1.Columns.Add("JobTitle", "Job Title");
                        dataGridView1.Columns.Add("SalaryRange", "Salary");
                        dataGridView1.Columns.Add("DateTime", "Interview Date & Time");
                        dataGridView1.Columns.Add("RecruiterName", "Recruiter");
                        dataGridView1.Columns.Add("Status", "Status");
                        
                        // Hidden columns for IDs
                        DataGridViewTextBoxColumn interviewIdColumn = new DataGridViewTextBoxColumn();
                        interviewIdColumn.Name = "InterviewID";
                        interviewIdColumn.DataPropertyName = "InterviewID";
                        interviewIdColumn.Visible = false;
                        dataGridView1.Columns.Add(interviewIdColumn);

                        DataGridViewTextBoxColumn applicationIdColumn = new DataGridViewTextBoxColumn();
                        applicationIdColumn.Name = "ApplicationID";
                        applicationIdColumn.DataPropertyName = "ApplicationID";
                        applicationIdColumn.Visible = false;
                        dataGridView1.Columns.Add(applicationIdColumn);

                        DataGridViewTextBoxColumn recruiterIdColumn = new DataGridViewTextBoxColumn();
                        recruiterIdColumn.Name = "RecruiterID";
                        recruiterIdColumn.DataPropertyName = "RecruiterID";
                        recruiterIdColumn.Visible = false;
                        dataGridView1.Columns.Add(recruiterIdColumn);

                        // Clear rows before adding new ones
                        dataGridView1.Rows.Clear();

                        // Add data to grid
                        foreach (DataRow row in dt.Rows)
                        {
                            // Format DateTime
                            string formattedDateTime = "N/A";
                            if (row["DateTime"] != DBNull.Value)
                            {
                                DateTime interviewDateTime = Convert.ToDateTime(row["DateTime"]);
                                formattedDateTime = interviewDateTime.ToString("MMM dd, yyyy hh:mm tt");
                            }

                            dataGridView1.Rows.Add(
                                row["CompanyName"],
                                row["JobTitle"],
                                row["SalaryRange"],
                                formattedDateTime,
                                row["RecruiterName"],
                                row["Status"],
                                row["InterviewID"],
                                row["ApplicationID"],
                                row["RecruiterID"]
                            );
                        }

                        // Remove the existing second data grid and buttons for slot scheduling
                        this.Controls.Remove(dataGridView2);
                        this.Controls.Remove(button2);
                        this.Controls.Remove(button3);
                        this.Controls.Remove(label3);

                        // Rename buttons with new colors and locations
                        button2 = new System.Windows.Forms.Button();
                        button2.Location = new System.Drawing.Point(790, 100);
                        button2.Name = "button2";
                        button2.Size = new System.Drawing.Size(150, 35);
                        button2.TabIndex = 6;
                        button2.Text = "Follow Schedule";
                        button2.BackColor = System.Drawing.Color.ForestGreen;
                        button2.ForeColor = System.Drawing.Color.White;
                        button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
                        button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        button2.UseVisualStyleBackColor = false;
                        button2.Click += new System.EventHandler(this.btnFollowSchedule_Click);
                        this.Controls.Add(button2);

                        button3 = new System.Windows.Forms.Button();
                        button3.Location = new System.Drawing.Point(790, 150);
                        button3.Name = "button3";
                        button3.Size = new System.Drawing.Size(150, 35);
                        button3.TabIndex = 7;
                        button3.Text = "Cancel Interview";
                        button3.BackColor = System.Drawing.Color.Firebrick;
                        button3.ForeColor = System.Drawing.Color.White;
                        button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
                        button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        button3.UseVisualStyleBackColor = false;
                        button3.Click += new System.EventHandler(this.btnCancelInterview_Click);
                        this.Controls.Add(button3);

                        // Update label
                        this.Controls.Remove(label2); // Remove the "Your Scheduled Interviews" label
                        label1.Text = "Manage Your Interviews";
                        
                        // Center the title over the grid
                        int titleWidth = label1.Width;
                        int gridWidth = dataGridView1.Width;
                        int gridStartX = dataGridView1.Location.X;
                        int titleX = gridStartX + (gridWidth - titleWidth) / 2;
                        label1.Location = new System.Drawing.Point(titleX, label1.Location.Y);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading interviews: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedInterviewId = Convert.ToInt32(row.Cells["InterviewID"].Value);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedInterviewId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["InterviewID"].Value);
            }
        }

        private void btnFollowSchedule_Click(object sender, EventArgs e)
        {
            if (selectedInterviewId <= 0)
            {
                MessageBox.Show("Please select an interview to mark as completed.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Interviews SET Status = 'Completed' WHERE InterviewID = @InterviewID";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@InterviewID", selectedInterviewId);
                        int result = cmd.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            MessageBox.Show("Interview marked as completed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadInterviews(); // Refresh the list
                        }
                        else
                        {
                            MessageBox.Show("Failed to update interview status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating interview: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelInterview_Click(object sender, EventArgs e)
        {
            if (selectedInterviewId <= 0)
            {
                MessageBox.Show("Please select an interview to cancel.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to cancel this interview?", 
                "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "UPDATE Interviews SET Status = 'Cancelled' WHERE InterviewID = @InterviewID";
                        
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@InterviewID", selectedInterviewId);
                            int updateResult = cmd.ExecuteNonQuery();
                            
                            if (updateResult > 0)
                            {
                                MessageBox.Show("Interview cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadInterviews(); // Refresh the list
                            }
                            else
                            {
                                MessageBox.Show("Failed to cancel interview.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error cancelling interview: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Interview_Load(object sender, EventArgs e)
        {
            // Set the selection mode to full row selection for better UX
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // Set focus to the first row if there are any rows
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
                selectedInterviewId = Convert.ToInt32(dataGridView1.Rows[0].Cells["InterviewID"].Value);
            }
        }
    }
}
