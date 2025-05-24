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
    public partial class JobPost : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private int recruiterId; // Store the recruiter ID

        public JobPost(int recruiterId)
        {
            InitializeComponent();
            this.recruiterId = recruiterId; // Assign the recruiter ID
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell content click if needed
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnPostJob_Click(object sender, EventArgs e)
        {
            string title = txtJobTitle.Text;
            string description = txtJobDescription.Text;
            string jobType = cmbJobType.SelectedItem.ToString();
            string salaryRange = txtSalaryRange.Text;
            string location = txtLocation.Text;

            int companyId = GetCompanyIdForRecruiter(recruiterId); // Get the CompanyID for the recruiter

            if (companyId == 0)
            {
                MessageBox.Show("No company associated with this recruiter.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO JobPostings (CompanyID, Title, Description, JobType, SalaryRange, Location) " +
                                   "VALUES (@CompanyID, @Title, @Description, @JobType, @SalaryRange, @Location)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CompanyID", companyId);
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@JobType", jobType);
                        cmd.Parameters.AddWithValue("@SalaryRange", salaryRange);
                        cmd.Parameters.AddWithValue("@Location", location);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Job posted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int GetCompanyIdForRecruiter(int recruiterId)
        {
            int companyId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CompanyID FROM Recruiters WHERE RecruiterID = @RecruiterID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RecruiterID", recruiterId);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        companyId = Convert.ToInt32(result);
                    }
                }
            }

            return companyId;
        }

        private void JobPost_Load(object sender, EventArgs e)
        {
            // Handle form load if needed
        }

        private void JobPost_Load_1(object sender, EventArgs e)
        {

        }
    }
}
