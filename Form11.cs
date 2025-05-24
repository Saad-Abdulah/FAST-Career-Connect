using Fast_Connect_DB_Final_project.FASTCareerConnectDataSetTableAdapters;
using Microsoft.Reporting.WinForms;
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
    public partial class Form11 : Form
    {
        public Form11()
        {
            InitializeComponent();
        }

        private void Form11_Load(object sender, EventArgs e)
        {
            var adapter = new BoothTrafficTableAdapter();
            var dt = new FASTCareerConnectDataSet.BoothTrafficDataTable();

            adapter.Fill(dt);

            reportViewer1.LocalReport.ReportPath = @"C:\Users\Saad Abdullah\Desktop\Semester 04\DB_L\project\Fast_Connect_DB_Final_project-master\Fast_Connect_DB_Final_project\Report15.rdlc";

            ReportDataSource rds = new ReportDataSource("BoothTraffic", (DataTable)dt);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.RefreshReport();
            
        }
    }
}
