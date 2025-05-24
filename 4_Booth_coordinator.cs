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
    public partial class Booth : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private string boothEmail; // Store Booth Email globally in the form

        public Booth(string email)
        {
            InitializeComponent();
            boothEmail = email; // Save email
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Booth_Student_CheckIN booth_Student_CheckIN = new Booth_Student_CheckIN(boothEmail); // Pass email to the next form
            booth_Student_CheckIN.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Monitor_Booth_traffic monitor_Booth_Traffic = new Monitor_Booth_traffic();
            monitor_Booth_Traffic.Show();
        }

        private void Booth_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT Name FROM Users WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", boothEmail);

                        object result = cmd.ExecuteScalar(); // Get single value

                        if (result != null)
                        {
                            label1.Text = result.ToString(); // Set Booth Name on label1
                        }
                        else
                        {
                            label1.Text = "Booth Not Found!";
                            MessageBox.Show("Booth coordinator not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading booth coordinator information: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
