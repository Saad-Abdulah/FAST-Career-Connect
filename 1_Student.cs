using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Fast_Connect_DB_Final_project
{
    public partial class Form1 : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private string userEmail;
        private int studentId;

        // Constructor to accept email
        public Form1(string email)
        {
            InitializeComponent();
            userEmail = email;
            LoadUserWelcomeMessage();
        }

        // Default constructor for designer
        public Form1() : this(null)
        {
        }

        private void LoadUserWelcomeMessage()
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT u.Name, u.Role, s.StudentID FROM Users u LEFT JOIN Students s ON u.UserID = s.StudentID WHERE u.Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", userEmail);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader["Name"].ToString();
                                string role = reader["Role"].ToString();
                                label1.Text = $"Welcome {role} {name}";
                                if (role == "Student" && !reader.IsDBNull(reader.GetOrdinal("StudentID")))
                                {
                                    studentId = reader.GetInt32(reader.GetOrdinal("StudentID"));
                                    // Update button text if profile exists
                                    button1.Text = "Update Profile";
                                }
                                else
                                {
                                    // Keep default text for new profile
                                    button1.Text = "Create Profile";
                                }
                            }
                            else
                            {
                                MessageBox.Show("User not found in database.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading user data: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if student profile exists
            if (studentId > 0)
            {
                // Edit existing profile
                Profile_Student profile_Student = new Profile_Student(studentId, userEmail);
                profile_Student.Show();
            }
            else
            {
                // Create new profile and pass the email
                Profile_Student profile_Student = new Profile_Student(0, userEmail);
                profile_Student.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Job_Fairs_Explorer job_Fairs_Explorer = new Job_Fairs_Explorer(studentId);
            job_Fairs_Explorer.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            My_Application my_Application = new My_Application();
            my_Application.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Interview interview = new Interview(studentId);
            interview.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Review review = new Review(studentId);
            review.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Log_in log_in = new Log_in();
            log_in.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}