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
    public partial class TPO_ManageUser : Form
    {
        // Define the connection string
        private string connectionString = DatabaseConfig.ConnectionString;

        public TPO_ManageUser()
        {
            InitializeComponent();
        }

        private void TPO_ManageUser_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Name, Email, Role, IsApproved FROM Users", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                
                // Clear existing columns and rows
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                // Set up the DataGridView columns
                dataGridView1.Columns.Add("Name", "Name");
                dataGridView1.Columns.Add("Email", "Email");
                dataGridView1.Columns.Add("Role", "Role");
                dataGridView1.Columns.Add("IsApproved", "Status");

                // Populate the DataGridView with data
                foreach (DataRow row in dt.Rows)
                {
                    int rowIndex = dataGridView1.Rows.Add(
                        row["Name"], 
                        row["Email"], 
                        row["Role"], 
                        Convert.ToInt32(row["IsApproved"]) == 1 ? "Approved" : "Not Approved"
                    );
                    
                    // Color the status cell based on approval status
                    if (Convert.ToInt32(row["IsApproved"]) == 1)
                    {
                        dataGridView1.Rows[rowIndex].Cells["IsApproved"].Style.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        dataGridView1.Rows[rowIndex].Cells["IsApproved"].Style.BackColor = Color.LightCoral;
                    }
                }
                
                // Auto-resize columns for better display
                dataGridView1.AutoResizeColumns();
            }
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string email = dataGridView1.SelectedRows[0].Cells["Email"].Value.ToString();
                UpdateUserStatus(email, 0); // 0 for deactivation
                LoadUsers(); // Refresh the DataGridView
            }
        }

        private void UpdateUserStatus(string email, int isActive)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Users SET IsApproved = @IsActive WHERE Email = @Email", conn);
                cmd.Parameters.AddWithValue("@IsActive", isActive);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string email = dataGridView1.SelectedRows[0].Cells["Email"].Value.ToString();
                UpdateUserStatus(email, 1); // 1 for approval
                LoadUsers(); // Refresh the DataGridView
            }
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string email = dataGridView1.SelectedRows[0].Cells["Email"].Value.ToString();
                UpdateUserStatus(email, 0); // 0 for rejection/deactivation
                LoadUsers(); // Refresh the DataGridView
            }
        }
    }
}
