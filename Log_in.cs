using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // For database connection

namespace Fast_Connect_DB_Final_project
{
    public partial class Log_in : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;

        public Log_in()
        {
            InitializeComponent();
        }

        private void Log_in_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Register form2 = new Register();
            form2.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Reading values from TextBoxes
            string email = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            // Validate inputs
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Check if user exists, get their role, and verify approval
                    string query = "SELECT UserID, Role FROM Users WHERE Email = @Email AND Password = @Password AND IsApproved = 1";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password); // Note: Use hashed passwords in production
                        int userId = 0;           // UserID
                        string role = "";         // Role
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0);           // UserID
                                role = reader.GetString(1);         // Role
                            }
                            else
                            {
                                // Invalid login or not approved
                                MessageBox.Show("User not found, please register.");
                                // Redirect to Register form
                                this.Hide();
                                Register registerForm = new Register();
                                registerForm.Show();
                            }
                        }
                       
                            
                            MessageBox.Show("You are logged in!");
                            this.Hide();

                            // Redirect based on role
                            switch (role)
                            {
                                case "TPO":
                                    // Redirect to TPO Dashboard
                                    TPODashboard tpoDashboard = new TPODashboard(email);
                                    tpoDashboard.Show();

                                    break;
                                case "Recruiter":
                                    // Redirect to Recruiter Dashboard
                                    Recruiter recruiterDashboard = new Recruiter(userId);
                                    recruiterDashboard.Show();

                                    break;
                                case "Student":
                                    // Redirect to Student Dashboard
                                    Form1 from1 = new Form1(email);
                                    from1.Show();

                                    break;
                                case "BoothCoordinator":
                                    // Redirect to Booth Coordinator Dashboard
                                    Booth boothCoordinatorDashboard = new Booth(email);
                                    boothCoordinatorDashboard.Show();


                                    break;
                                default:
                                    MessageBox.Show("Unknown role. Please contact the administrator.");
                                    this.Hide();
                                    Register registerForm = new Register();
                                    registerForm.Show();
                                    break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}