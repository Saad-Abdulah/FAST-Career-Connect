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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var adapter = new DepWiseCountTableAdapter();
            var dt = new FASTCareerConnectDataSet.DepWiseCountDataTable();

            adapter.Fill(dt);

            reportViewer1.LocalReport.ReportPath = @"C:\Users\Saad Abdullah\Desktop\Semester 04\DB_L\project\Fast_Connect_DB_Final_project-master\Fast_Connect_DB_Final_project\Report6.rdlc";

            ReportDataSource rds = new ReportDataSource("DepWiseCount", (DataTable)dt);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
