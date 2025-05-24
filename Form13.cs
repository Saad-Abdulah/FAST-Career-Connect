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
    public partial class Form13 : Form
    {
        public Form13()
        {
            InitializeComponent();
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            var adapter = new ResUsageTableAdapter();
            var dt = new FASTCareerConnectDataSet.ResUsageDataTable();

            adapter.Fill(dt);

            reportViewer1.LocalReport.ReportPath = @"C:\Users\Saad Abdullah\Desktop\Semester 04\DB_L\project\Fast_Connect_DB_Final_project-master\Fast_Connect_DB_Final_project\Report17.rdlc";

            ReportDataSource rds = new ReportDataSource("ResUsage", (DataTable)dt);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.RefreshReport();
            
        }
    }
}
