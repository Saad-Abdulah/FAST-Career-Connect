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
    public partial class Recruiter : Form
    {
        private int userId = 0;
        public Recruiter(int userId)
        {
            InitializeComponent();  
            this.userId = userId;   
        }

        private void Recruiter_Load(object sender, EventArgs e)
        {

        }

        private void btnjobpost_Click(object sender, EventArgs e)
        {
            int recruiterId = userId;
            JobPost jobPost = new JobPost(recruiterId);
            jobPost.Show();
        }

        private void btnapplication_Click(object sender, EventArgs e)
        {
            Application_Review_Screen applicationReviewScreen = new Application_Review_Screen(userId);
            applicationReviewScreen.Show();
        }

        private void btninterview_Click(object sender, EventArgs e)
        {
            _2 allocateInterviewSlots = new _2(userId);
            allocateInterviewSlots.Show();
        }

        private void btnhiring_Click(object sender, EventArgs e)
        {
            Hire hire = new Hire(userId);
            hire.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Log_in login = new Log_in();
            login.Show();
        }
    }
}
