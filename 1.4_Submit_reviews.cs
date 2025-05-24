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
    public partial class Review : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private int studentID;
        private Dictionary<string, int> recruiterDict = new Dictionary<string, int>();
        
        public Review(int studentID = 0)
        {
            InitializeComponent();
            this.studentID = studentID;
        }

        private void Review_Load(object sender, EventArgs e)
        {
            LoadRecruiters();
            
            // Set default rating
            comboBox2.SelectedIndex = 4; // Default to 5 stars
        }
        
        private void LoadRecruiters()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Get all recruiters with company names
                    string query = @"
                        SELECT r.RecruiterID, u.Name + ' (' + c.Name + ')' AS RecruiterInfo
                        FROM Recruiters r
                        JOIN Users u ON r.RecruiterID = u.UserID
                        JOIN Companies c ON r.CompanyID = c.CompanyID
                        ORDER BY u.Name";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            comboBox1.Items.Clear();
                            recruiterDict.Clear();
                            
                            while (reader.Read())
                            {
                                int recruiterID = reader.GetInt32(0);
                                string recruiterInfo = reader.GetString(1);
                                
                                comboBox1.Items.Add(recruiterInfo);
                                recruiterDict.Add(recruiterInfo, recruiterID);
                            }
                            
                            if (comboBox1.Items.Count > 0)
                            {
                                comboBox1.SelectedIndex = 0;
                            }
                            else
                            {
                                MessageBox.Show("No recruiters found in the database.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading recruiters: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (studentID == 0)
            {
                MessageBox.Show("You must be logged in as a student to submit a review.");
                return;
            }
            
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a recruiter.");
                return;
            }
            
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a rating.");
                return;
            }
            
            string selectedRecruiter = comboBox1.SelectedItem.ToString();
            int recruiterID = recruiterDict[selectedRecruiter];
            int rating = int.Parse(comboBox2.SelectedItem.ToString());
            string comment = textBox1.Text.Trim();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Check if student has already reviewed this recruiter
                    string checkQuery = "SELECT COUNT(*) FROM Reviews WHERE StudentID = @StudentID AND RecruiterID = @RecruiterID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@StudentID", studentID);
                        checkCmd.Parameters.AddWithValue("@RecruiterID", recruiterID);
                        
                        int existingReviews = (int)checkCmd.ExecuteScalar();
                        
                        if (existingReviews > 0)
                        {
                            // Update existing review
                            string updateQuery = @"
                                UPDATE Reviews 
                                SET Rating = @Rating, Comment = @Comment 
                                WHERE StudentID = @StudentID AND RecruiterID = @RecruiterID";
                                
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@StudentID", studentID);
                                updateCmd.Parameters.AddWithValue("@RecruiterID", recruiterID);
                                updateCmd.Parameters.AddWithValue("@Rating", rating);
                                updateCmd.Parameters.AddWithValue("@Comment", string.IsNullOrEmpty(comment) ? DBNull.Value : (object)comment);
                                
                                int rowsAffected = updateCmd.ExecuteNonQuery();
                                
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Review updated successfully.");
                                    textBox1.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update review.");
                                }
                            }
                        }
                        else
                        {
                            // Insert new review
                            string insertQuery = @"
                                INSERT INTO Reviews (StudentID, RecruiterID, Rating, Comment)
                                VALUES (@StudentID, @RecruiterID, @Rating, @Comment)";
                                
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@StudentID", studentID);
                                insertCmd.Parameters.AddWithValue("@RecruiterID", recruiterID);
                                insertCmd.Parameters.AddWithValue("@Rating", rating);
                                insertCmd.Parameters.AddWithValue("@Comment", string.IsNullOrEmpty(comment) ? DBNull.Value : (object)comment);
                                
                                int rowsAffected = insertCmd.ExecuteNonQuery();
                                
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Review submitted successfully.");
                                    textBox1.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to submit review.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting review: " + ex.Message);
            }
        }
    }
}
