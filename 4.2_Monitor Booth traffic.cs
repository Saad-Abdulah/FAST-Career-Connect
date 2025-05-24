using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Fast_Connect_DB_Final_project
{
    public partial class Monitor_Booth_traffic : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public override string ToString() => Text;
        }

        public Monitor_Booth_traffic()
        {
            InitializeComponent();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(comboBox1.SelectedItem is ComboBoxItem selectedBooth))
                return;

            // Clear DataGridView
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // Add columns (match your DataGridView headers)
            dataGridView1.Columns.Add("Student_Name", "Student_Name");
            dataGridView1.Columns.Add("Fast_ID", "Fast_ID");
            dataGridView1.Columns.Add("Visit_Date", "Visit_Date");

            int totalVisits = 0;
            Dictionary<int, int> hourCounts = new Dictionary<int, int>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Get all visits for this booth
                var cmd = new SqlCommand(
                    @"SELECT u.Name, s.StudentID, v.VisitTime
                      FROM Visits v
                      JOIN Students s ON v.StudentID = s.StudentID
                      JOIN Users u ON s.StudentID = u.UserID
                      WHERE v.BoothID = @BoothID
                      ORDER BY v.VisitTime", conn);
                cmd.Parameters.AddWithValue("@BoothID", selectedBooth.Value);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string studentName = reader[0].ToString();
                    string fastId = reader[1].ToString();
                    DateTime visitTime = (DateTime)reader[2];
                    dataGridView1.Rows.Add(studentName, fastId, visitTime);

                    // Count for peak hour
                    int hour = visitTime.Hour;
                    if (!hourCounts.ContainsKey(hour))
                        hourCounts[hour] = 0;
                    hourCounts[hour]++;
                    totalVisits++;
                }
                reader.Close();
            }

            // Show total visits
            label3.Text = $"Total_Visit: {totalVisits}";

            // Show peak hour
            if (hourCounts.Count > 0)
            {
                int peakHour = hourCounts.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                //label4.Text = $"Peak_Hour: {peakHour}:00 - {peakHour + 1}:00";
            }
            else
            {
                //label4.Text = "Peak_Hour: N/A";
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Monitor_Booth_traffic_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    @"SELECT b.BoothID, c.Name, b.Location
                      FROM Booths b
                      JOIN Companies c ON b.CompanyID = c.CompanyID
                      ORDER BY c.Name, b.Location", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(new ComboBoxItem
                    {
                        Text = $"{reader[1]} - {reader[2]}",
                        Value = reader[0]
                    });
                }
                reader.Close();
            }
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
        }

        private void lblstepIn_Click(object sender, EventArgs e)
        {

        }

        private void lblPeakHour_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

        }
    }
}