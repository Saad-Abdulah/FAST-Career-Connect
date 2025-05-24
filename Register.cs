using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;  // For database connection

namespace Fast_Connect_DB_Final_project
{
    public partial class Register : Form
    {
        // Connection string
        private string connectionString = DatabaseConfig.ConnectionString;

        public Register()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Reading values from TextBoxes
            string name = textBox1.Text;           // TextBox for Name
            string email = textBox2.Text;         // TextBox for Email
            string password = textBox3.Text;       // TextBox for Password
            string role = comboBox1.SelectedItem?.ToString().Trim();  // ComboBox for Role

            // Validate inputs
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if email already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int userCount = (int)checkCmd.ExecuteScalar();

                        if (userCount > 0)
                        {
                            MessageBox.Show("You are already registered, please log in.");
                            // Redirect to login form
                            this.Hide();
                            Log_in loginForm = new Log_in();
                            loginForm.Show();
                            return;
                        }
                    }

                    // Insert new user if email doesn't exist
                    string insertQuery = "INSERT INTO Users (Name, Email, Password, Role, IsApproved) VALUES (@Name, @Email, @Password, @Role, @IsApproved)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password); // Note: Consider hashing passwords in production
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@IsApproved", 1);  // Directly approve for now

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registration Successful!");
                            // Redirect to login form
                            this.Hide();
                            Log_in loginForm = new Log_in();
                            loginForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Registration Failed.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Log_in form1 = new Log_in();
            form1.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }
    }
}