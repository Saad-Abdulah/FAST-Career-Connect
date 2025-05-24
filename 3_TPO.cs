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
    public partial class TPODashboard : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private Button btnuser;
        private Button btnjobfair;
        private Button btnMyReports;
        private Button btnRubricReports;
        private Label label1;
        private Button btnLogout;
        private Button btnmonitoring;
        private string userEmail;

        // Constructor to accept email
        public TPODashboard(string email)
        {
            InitializeComponent();
            userEmail = email;
            VerifyTPOAccess();
        }

        // Default constructor for designer
        public TPODashboard() : this(null)
        {
        }

        private void VerifyTPOAccess()
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                MessageBox.Show("No user email provided.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Role FROM Users WHERE Email = @Email AND Role = 'TPO'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", userEmail);
                        object result = cmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("You are not authorized to access the TPO Dashboard.");
                            this.Hide();
                            Log_in loginForm = new Log_in();
                            loginForm.Show();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error verifying TPO access: " + ex.Message);
                }
            }
        }

        private void InitializeComponent()
        {
            this.btnuser = new System.Windows.Forms.Button();
            this.btnjobfair = new System.Windows.Forms.Button();
            this.btnMyReports = new System.Windows.Forms.Button();
            this.btnRubricReports = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnmonitoring = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnuser
            // 
            this.btnuser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnuser.Location = new System.Drawing.Point(249, 122);
            this.btnuser.Name = "btnuser";
            this.btnuser.Size = new System.Drawing.Size(233, 47);
            this.btnuser.TabIndex = 0;
            this.btnuser.Text = "User Approval";
            this.btnuser.UseVisualStyleBackColor = true;
            this.btnuser.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnjobfair
            // 
            this.btnjobfair.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnjobfair.Location = new System.Drawing.Point(249, 189);
            this.btnjobfair.Name = "btnjobfair";
            this.btnjobfair.Size = new System.Drawing.Size(233, 51);
            this.btnjobfair.TabIndex = 1;
            this.btnjobfair.Text = "JobFair Scheduling and     Booth Allocation";
            this.btnjobfair.UseVisualStyleBackColor = true;
            this.btnjobfair.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnMyReports
            // 
            this.btnMyReports.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMyReports.Location = new System.Drawing.Point(249, 380);
            this.btnMyReports.Name = "btnMyReports";
            this.btnMyReports.Size = new System.Drawing.Size(233, 43);
            this.btnMyReports.TabIndex = 3;
            this.btnMyReports.Text = "My Reports";
            this.btnMyReports.UseVisualStyleBackColor = true;
            this.btnMyReports.Click += new System.EventHandler(this.btnMyReports_Click);
            // 
            // btnRubricReports
            // 
            this.btnRubricReports.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRubricReports.Location = new System.Drawing.Point(249, 318);
            this.btnRubricReports.Name = "btnRubricReports";
            this.btnRubricReports.Size = new System.Drawing.Size(233, 43);
            this.btnRubricReports.TabIndex = 4;
            this.btnRubricReports.Text = "Rubric Reports";
            this.btnRubricReports.UseVisualStyleBackColor = true;
            this.btnRubricReports.Click += new System.EventHandler(this.btnRubricReports_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(242, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 29);
            this.label1.TabIndex = 4;
            this.label1.Text = "Welcome TPO";
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.IndianRed;
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.Location = new System.Drawing.Point(276, 470);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(167, 50);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Log out ";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnmonitoring
            // 
            this.btnmonitoring.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnmonitoring.Location = new System.Drawing.Point(249, 258);
            this.btnmonitoring.Name = "btnmonitoring";
            this.btnmonitoring.Size = new System.Drawing.Size(233, 43);
            this.btnmonitoring.TabIndex = 6;
            this.btnmonitoring.Text = "Monitoring";
            this.btnmonitoring.UseVisualStyleBackColor = true;
            this.btnmonitoring.Click += new System.EventHandler(this.btnmonitoring_Click);
            // 
            // TPODashboard
            // 
            this.ClientSize = new System.Drawing.Size(693, 550);
            this.Controls.Add(this.btnmonitoring);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRubricReports);
            this.Controls.Add(this.btnMyReports);
            this.Controls.Add(this.btnjobfair);
            this.Controls.Add(this.btnuser);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TPODashboard";
            this.Load += new System.EventHandler(this.TPODashboard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void TPODashboard_Load(object sender, EventArgs e)
        {
        }

        private void btnMyReports_Click(object sender, EventArgs e)
        {
            Report_Access reportAccess = new Report_Access();
            reportAccess.Show();
        }

        private void btnRubricReports_Click(object sender, EventArgs e)
        {
            AllReportsForm rubricReports = new AllReportsForm();
            rubricReports.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TPO_ManageUser tpo_manageuser = new TPO_ManageUser();
            tpo_manageuser.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TPO_Job_Fair_Management tpo_job_fair = new TPO_Job_Fair_Management();
            tpo_job_fair.Show();
        }

        private void btnmonitoring_Click(object sender, EventArgs e)
        {
            TPO_Monitoring_Form tpo_monitoring = new TPO_Monitoring_Form();
            tpo_monitoring.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Log_in log_in = new Log_in();
            log_in.Show();
        }
    }
}