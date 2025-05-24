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
    public partial class TPO_Monitoring_Form : Form
    {
        // Database connection string
        private string connectionString = DatabaseConfig.ConnectionString;
        private int selectedJobFairID = -1;

        public TPO_Monitoring_Form()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void TPO_Monitoring_Form_Load(object sender, EventArgs e)
        {
            try
            {
                LoadJobFairs();
                // Initially disable the data grids until a job fair is selected
                dataGridView1.Enabled = false;
                dataGridView2.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadJobFairs()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "SELECT JobFairID, CONVERT(VARCHAR, EventDate, 101) + ' - ' + Venue AS EventDisplay " +
                        "FROM JobFairEvents ORDER BY EventDate DESC", conn);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbJobFairs.DataSource = dt;
                    cmbJobFairs.DisplayMember = "EventDisplay";
                    cmbJobFairs.ValueMember = "JobFairID";

                    if (dt.Rows.Count > 0)
                    {
                        cmbJobFairs.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("No job fairs found. Please create job fairs first.", 
                            "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading job fairs: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbJobFairs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbJobFairs.SelectedValue != null && cmbJobFairs.SelectedValue is int)
            {
                selectedJobFairID = (int)cmbJobFairs.SelectedValue;
                LoadJobStatistics(selectedJobFairID);
                LoadBoothStatistics(selectedJobFairID);
                
                // Enable data grids
                dataGridView1.Enabled = true;
                dataGridView2.Enabled = true;
            }
        }

        private void LoadJobStatistics(int jobFairID)
        {
            dataGridView1.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    
                    // Get job statistics for companies participating in the selected job fair
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT 
                            jp.Title AS JobTitle,
                            COUNT(DISTINCT a.ApplicationID) AS ApplicationCount,
                            COUNT(DISTINCT CASE WHEN a.Status = 'Interviewed' THEN a.ApplicationID END) AS InterviewCount,
                            COUNT(DISTINCT CASE WHEN a.Status = 'Accepted' THEN a.ApplicationID END) AS HireCount
                        FROM 
                            JobPostings jp
                        INNER JOIN 
                            Companies c ON jp.CompanyID = c.CompanyID
                        INNER JOIN 
                            Booths b ON c.CompanyID = b.CompanyID
                        LEFT JOIN 
                            Applications a ON jp.JobPostingID = a.JobPostingID
                        WHERE 
                            b.JobFairID = @JobFairID
                        GROUP BY 
                            jp.Title", conn);
                    
                    cmd.Parameters.AddWithValue("@JobFairID", jobFairID);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string jobTitle = reader["JobTitle"].ToString();
                            int applicationCount = Convert.ToInt32(reader["ApplicationCount"]);
                            int interviewCount = Convert.ToInt32(reader["InterviewCount"]);
                            int hireCount = Convert.ToInt32(reader["HireCount"]);
                            
                            dataGridView1.Rows.Add(jobTitle, applicationCount, interviewCount, hireCount);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading job statistics: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadBoothStatistics(int jobFairID)
        {
            dataGridView2.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    
                    // Get booth visit statistics
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT 
                            b.Location AS BoothLocation,
                            c.Name AS CompanyName,
                            COUNT(v.VisitID) AS TotalVisits
                        FROM 
                            Booths b
                        INNER JOIN 
                            Companies c ON b.CompanyID = c.CompanyID
                        LEFT JOIN 
                            Visits v ON b.BoothID = v.BoothID
                        WHERE 
                            b.JobFairID = @JobFairID
                        GROUP BY 
                            b.Location, c.Name", conn);
                    
                    cmd.Parameters.AddWithValue("@JobFairID", jobFairID);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string boothLocation = reader["BoothLocation"].ToString();
                            string companyName = reader["CompanyName"].ToString();
                            int totalVisits = Convert.ToInt32(reader["TotalVisits"]);
                            
                            dataGridView2.Rows.Add(boothLocation, companyName, totalVisits);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading booth statistics: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TPO_Monitoring_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void lblJobStatistics_Click(object sender, EventArgs e)
        {

        }
    }
}
