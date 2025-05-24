using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fast_Connect_DB_Final_project
{
    public partial class AllReportsForm : Form
    {
        public AllReportsForm()
        {
            InitializeComponent();
        }

        // Student Participation Reports
        private void btnDepartmentRegistration_Click(object sender, EventArgs e)
        {
            Form2 departmentRegistrationReport = new Form2();
            departmentRegistrationReport.Show();
        }

        private void btnGpaDistribution_Click(object sender, EventArgs e)
        {
            Form3 gpaDistributionReport = new Form3();
            gpaDistributionReport.Show();
        }

        private void btnTopSkills_Click(object sender, EventArgs e)
        {
            Form4 topSkillsReport = new Form4();
            topSkillsReport.Show();
        }

        // Recruiter Activity Reports
        private void btnInterviewsPerCompany_Click(object sender, EventArgs e)
        {
            Form5 interviewsPerCompanyReport = new Form5();
            interviewsPerCompanyReport.Show();
        }

        private void btnOfferAcceptanceRatio_Click(object sender, EventArgs e)
        {
            Form6 offerAcceptanceRatioReport = new Form6();
            offerAcceptanceRatioReport.Show();
        }

        private void btnStudentRatings_Click(object sender, EventArgs e)
        {
            Form7 studentRatingsReport = new Form7();
            studentRatingsReport.Show();
        }

        // Placement Statistics Reports
        private void btnOverallPlacement_Click(object sender, EventArgs e)
        {
            Form8 overallPlacementReport = new Form8();
            overallPlacementReport.Show();
        }

        private void btnDepartmentPlacement_Click(object sender, EventArgs e)
        {
            Form9 departmentPlacementReport = new Form9();
            departmentPlacementReport.Show();
        }

        private void btnAverageSalary_Click(object sender, EventArgs e)
        {
            Form10 averageSalaryReport = new Form10();
            averageSalaryReport.Show();
        }

        // Event Performance Reports
        private void btnBoothTraffic_Click(object sender, EventArgs e)
        {
            Form11 boothTrafficReport = new Form11();
            boothTrafficReport.Show();
        }

        private void btnPeakHours_Click(object sender, EventArgs e)
        {
            Form12 peakHoursReport = new Form12();
            peakHoursReport.Show();
        }

        private void btnResourceUsage_Click(object sender, EventArgs e)
        {
            Form13 resourceUsageReport = new Form13();
            resourceUsageReport.Show();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
