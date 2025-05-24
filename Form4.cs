using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fast_Connect_DB_Final_project.FASTCareerConnectDataSetTableAdapters;
using Microsoft.Reporting.WinForms;

namespace Fast_Connect_DB_Final_project
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            var adapter = new TopSkillsTableAdapter();
            var dt = new FASTCareerConnectDataSet.TopSkillsDataTable();

            adapter.Fill(dt);

            reportViewer1.LocalReport.ReportPath = @"C:\Users\Saad Abdullah\Desktop\Semester 04\DB_L\project\Fast_Connect_DB_Final_project-master\Fast_Connect_DB_Final_project\Report8.rdlc";

            ReportDataSource rds = new ReportDataSource("TopSkills", (DataTable)dt);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.RefreshReport();
            //this.reportViewer1.RefreshReport();
            //this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void reportViewer1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
