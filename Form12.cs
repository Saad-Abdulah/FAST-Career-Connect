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
    public partial class Form12 : Form
    {
        public Form12()
        {
            InitializeComponent();
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            var adapter = new PeakHoursTableAdapter();
            var dt = new FASTCareerConnectDataSet.PeakHoursDataTable();

            adapter.Fill(dt);

            reportViewer1.LocalReport.ReportPath = @"C:\Users\Saad Abdullah\Desktop\Semester 04\DB_L\project\Fast_Connect_DB_Final_project-master\Fast_Connect_DB_Final_project\Report16.rdlc";

            ReportDataSource rds = new ReportDataSource("PeakHours", (DataTable)dt);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.RefreshReport();
            
        }
    }
}
