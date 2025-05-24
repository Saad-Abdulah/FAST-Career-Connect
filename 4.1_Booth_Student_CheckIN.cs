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
    public partial class Booth_Student_CheckIN : Form
    {
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public override string ToString() => Text;
        }

        private string connectionString = DatabaseConfig.ConnectionString;

        public Booth_Student_CheckIN(string Email)
        {
            InitializeComponent();
            string booth_email = Email;
        }

        private void Booth_Student_CheckIN_Load(object sender, EventArgs e)
        {
            // Load job fairs
            LoadJobFairs();
            
            // Load student IDs
            LoadStudentIDs();
        }
        
        private void LoadJobFairs()
        {
            comboBox1.Items.Clear();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT JobFairID, EventDate, Venue FROM JobFairEvents ORDER BY EventDate DESC", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(new ComboBoxItem
                    {
                        Text = $"{((DateTime)reader[1]):yyyy-MM-dd} - {reader[2]}",
                        Value = reader[0]
                    });
                }
                reader.Close();
            }
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
        }
        
        private void LoadStudentIDs()
        {
            comboBoxStudentID.Items.Clear();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    @"SELECT s.StudentID, u.Name 
                    FROM Students s 
                    JOIN Users u ON s.StudentID = u.UserID 
                    ORDER BY u.Name", conn);
                
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBoxStudentID.Items.Add(new ComboBoxItem
                    {
                        Text = $"{reader[0]} - {reader[1]}",
                        Value = reader[0]
                    });
                }
                reader.Close();
            }
            if (comboBoxStudentID.Items.Count > 0) comboBoxStudentID.SelectedIndex = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxStudentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Event handler for student ID combobox
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            if (comboBox1.SelectedItem is ComboBoxItem selectedFair)
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand(
                        @"SELECT b.BoothID, c.Name, b.Location 
                          FROM Booths b 
                          JOIN Companies c ON b.CompanyID = c.CompanyID 
                          WHERE b.JobFairID = @JobFairID", conn);
                    cmd.Parameters.AddWithValue("@JobFairID", selectedFair.Value);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox2.Items.Add(new ComboBoxItem
                        {
                            Text = $"{reader[1]} - {reader[2]}",
                            Value = reader[0]
                        });
                    }
                    reader.Close();
                }
            }
            if (comboBox2.Items.Count > 0) comboBox2.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(comboBox1.SelectedItem is ComboBoxItem selectedFair) || !(comboBox2.SelectedItem is ComboBoxItem selectedBooth))
            {
                MessageBox.Show("Please select a job fair and booth.", "Missing Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (!(comboBoxStudentID.SelectedItem is ComboBoxItem selectedStudent))
            {
                MessageBox.Show("Please select a Student ID.", "Missing Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            int studentId = Convert.ToInt32(selectedStudent.Value);
            
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Check for duplicate check-in
                var checkVisit = new SqlCommand("SELECT COUNT(*) FROM Visits WHERE StudentID = @StudentID AND BoothID = @BoothID", conn);
                checkVisit.Parameters.AddWithValue("@BoothID", selectedBooth.Value);
                checkVisit.Parameters.AddWithValue("@StudentID", studentId);
                int alreadyChecked = (int)checkVisit.ExecuteScalar();
                if (alreadyChecked > 0)
                {
                    MessageBox.Show("This student has already checked in at this booth.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                // Insert visit
                var insertVisit = new SqlCommand("INSERT INTO Visits (StudentID, BoothID, VisitTime) VALUES (@StudentID, @BoothID, @VisitTime)", conn);
                insertVisit.Parameters.AddWithValue("@StudentID", studentId);
                insertVisit.Parameters.AddWithValue("@BoothID", selectedBooth.Value);
                insertVisit.Parameters.AddWithValue("@VisitTime", DateTime.Now);
                insertVisit.ExecuteNonQuery();
                MessageBox.Show("Check-in successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}