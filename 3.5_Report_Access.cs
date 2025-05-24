using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fast_Connect_DB_Final_project
{
    public partial class Report_Access : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private DataTable reportData = null;
        private string currentReportTitle = "";

        public Report_Access()
        {
            InitializeComponent();
        }

        private void Report_Access_Load(object sender, EventArgs e)
        {
            LoadJobFairs();
            comboBoxReportType.SelectedIndex = 0;
            comboBoxExportFormat.SelectedIndex = 0;
            
            // Set up the DataGrid appearance
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            
            // Apply rounded corners to buttons
            ApplyRoundedCorners();
            
            // Add event handler for report type change
            comboBoxReportType.SelectedIndexChanged += new EventHandler(comboBoxReportType_SelectedIndexChanged);
            
            // Disable export button until a report is generated
            btnExportCSV.Enabled = false;
            
            // Auto-generate a report on load (after a short delay to ensure UI is ready)
            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Tick += (s, args) => {
                timer.Stop();
                GenerateCurrentReport();
            };
            timer.Start();
        }

        private void comboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Auto-generate the report when the report type changes
            GenerateCurrentReport();
        }

        private void GenerateCurrentReport()
        {
            if (comboBoxJobFair.SelectedValue == null)
            {
                // Don't try to generate if no job fair is selected
                return;
            }

            try
            {
                // Show a cursor wait indicator
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                // Call the generate report logic
                btnGenerateReport_Click(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error auto-generating report: " + ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Restore cursor
                Cursor = Cursors.Default;
            }
        }

        private void LoadJobFairs()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT JobFairID, 
                               CONVERT(VARCHAR, EventDate, 103) + ' - ' + Venue AS JobFairDisplay
                        FROM JobFairEvents
                        ORDER BY EventDate DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBoxJobFair.DataSource = dt;
                    comboBoxJobFair.DisplayMember = "JobFairDisplay";
                    comboBoxJobFair.ValueMember = "JobFairID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading job fairs: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (comboBoxJobFair.SelectedValue == null)
            {
                MessageBox.Show("Please select a job fair.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int jobFairId = Convert.ToInt32(comboBoxJobFair.SelectedValue);
            string reportType = comboBoxReportType.SelectedItem.ToString();

            try
            {
                switch (reportType)
                {
                    case "Booth Traffic":
                        GenerateBoothTrafficReport(jobFairId);
                        break;
                    case "Company Participation":
                        GenerateCompanyParticipationReport(jobFairId);
                        break;
                    case "Student Attendance":
                        GenerateStudentAttendanceReport(jobFairId);
                        break;
                    case "Interview Statistics":
                        GenerateInterviewStatisticsReport(jobFairId);
                        break;
                    case "Job Application Trends":
                        GenerateJobApplicationsReport(jobFairId);
                        break;
                    default:
                        MessageBox.Show("Please select a valid report type.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }

                // Enable export button after report is generated
                btnExportCSV.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateBoothTrafficReport(int jobFairId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        b.Location AS 'Booth Location', 
                        c.Name AS 'Company', 
                        COUNT(v.VisitID) AS 'Total Visits',
                        CONVERT(VARCHAR, MIN(v.VisitTime), 108) AS 'First Visit Time',
                        CONVERT(VARCHAR, MAX(v.VisitTime), 108) AS 'Last Visit Time',
                        CASE 
                            WHEN COUNT(v.VisitID) > 0 THEN 
                                (SELECT TOP 1 DATEPART(HOUR, VisitTime) 
                                 FROM Visits v2 JOIN Booths b2 ON v2.BoothID = b2.BoothID
                                 WHERE b2.JobFairID = @JobFairID AND b2.BoothID = b.BoothID
                                 GROUP BY DATEPART(HOUR, VisitTime)
                                 ORDER BY COUNT(*) DESC)
                            ELSE NULL
                        END AS 'Peak Hour'
                    FROM 
                        Booths b
                    LEFT JOIN 
                        Companies c ON b.CompanyID = c.CompanyID
                    LEFT JOIN 
                        Visits v ON b.BoothID = v.BoothID
                    WHERE 
                        b.JobFairID = @JobFairID
                    GROUP BY 
                        b.BoothID, b.Location, c.Name
                    ORDER BY 
                        COUNT(v.VisitID) DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@JobFairID", jobFairId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Create a display version of the data for the grid
                DataTable displayDt = dt.Copy();
                displayDt.Columns.Add("Peak Hour Display", typeof(string));

                // Convert peak hour to AM/PM format
                foreach (DataRow row in displayDt.Rows)
                {
                    if (row["Peak Hour"] != DBNull.Value)
                    {
                        int hour = Convert.ToInt32(row["Peak Hour"]);
                        string amPm = hour >= 12 ? "PM" : "AM";
                        hour = hour > 12 ? hour - 12 : (hour == 0 ? 12 : hour);
                        row["Peak Hour Display"] = hour.ToString() + " " + amPm;
                    }
                    else
                    {
                        row["Peak Hour Display"] = "N/A";
                    }
                }

                // Remove the numeric column from display and rename the string column
                displayDt.Columns.Remove("Peak Hour");
                displayDt.Columns["Peak Hour Display"].ColumnName = "Peak Hour";

                dataGridView1.DataSource = displayDt;
                reportData = displayDt;
                currentReportTitle = "Booth Traffic Report - " + comboBoxJobFair.Text;
            }
        }

        private void GenerateCompanyParticipationReport(int jobFairId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        c.Name AS 'Company', 
                        c.Sector AS 'Industry Sector',
                        b.Location AS 'Booth Location',
                        u.Name AS 'Coordinator',
                        COUNT(DISTINCT jp.JobPostingID) AS 'Jobs Posted',
                        COUNT(DISTINCT a.ApplicationID) AS 'Applications Received',
                        COUNT(DISTINCT i.InterviewID) AS 'Interviews Scheduled'
                    FROM 
                        Booths b
                    INNER JOIN 
                        Companies c ON b.CompanyID = c.CompanyID
                    INNER JOIN 
                        Users u ON b.CoordinatorID = u.UserID
                    LEFT JOIN 
                        JobPostings jp ON c.CompanyID = jp.CompanyID
                    LEFT JOIN 
                        Applications a ON jp.JobPostingID = a.JobPostingID
                    LEFT JOIN 
                        Interviews i ON a.ApplicationID = i.ApplicationID
                    WHERE 
                        b.JobFairID = @JobFairID
                    GROUP BY 
                        c.Name, c.Sector, b.Location, u.Name
                    ORDER BY 
                        c.Name";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@JobFairID", jobFairId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
                reportData = dt;
                currentReportTitle = "Company Participation Report - " + comboBoxJobFair.Text;
            }
        }

        private void GenerateStudentAttendanceReport(int jobFairId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        u.Name AS 'Student Name',
                        s.DegreeProgram AS 'Degree Program',
                        s.CurrentSemester AS 'Semester',
                        s.GPA AS 'GPA',
                        COUNT(DISTINCT v.BoothID) AS 'Booths Visited',
                        COUNT(DISTINCT a.ApplicationID) AS 'Applications Submitted',
                        MIN(CONVERT(VARCHAR, v.VisitTime, 103) + ' ' + CONVERT(VARCHAR, v.VisitTime, 108)) AS 'First Booth Visit',
                        MAX(CONVERT(VARCHAR, v.VisitTime, 103) + ' ' + CONVERT(VARCHAR, v.VisitTime, 108)) AS 'Last Booth Visit'
                    FROM 
                        Students s
                    INNER JOIN 
                        Users u ON s.StudentID = u.UserID
                    INNER JOIN 
                        Visits v ON s.StudentID = v.StudentID
                    INNER JOIN 
                        Booths b ON v.BoothID = b.BoothID
                    LEFT JOIN 
                        Applications a ON s.StudentID = a.StudentID AND 
                            a.ApplicationDate BETWEEN CAST(DATEADD(day, -1, (SELECT EventDate FROM JobFairEvents WHERE JobFairID = @JobFairID)) AS DATE)
                            AND CAST(DATEADD(day, 7, (SELECT EventDate FROM JobFairEvents WHERE JobFairID = @JobFairID)) AS DATE)
                    WHERE 
                        b.JobFairID = @JobFairID
                    GROUP BY 
                        u.Name, s.DegreeProgram, s.CurrentSemester, s.GPA
                    ORDER BY 
                        COUNT(DISTINCT v.BoothID) DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@JobFairID", jobFairId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
                reportData = dt;
                currentReportTitle = "Student Attendance Report - " + comboBoxJobFair.Text;
            }
        }

        private void GenerateInterviewStatisticsReport(int jobFairId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        c.Name AS 'Company',
                        jp.Title AS 'Job Position',
                        SUM(CASE WHEN i.Status = 'Scheduled' THEN 1 ELSE 0 END) AS 'Scheduled',
                        SUM(CASE WHEN i.Status = 'Completed' THEN 1 ELSE 0 END) AS 'Completed',
                        SUM(CASE WHEN i.Status = 'Cancelled' THEN 1 ELSE 0 END) AS 'Cancelled',
                        COUNT(i.InterviewID) AS 'Total Interviews',
                        CASE WHEN AVG(r.Rating) IS NULL THEN NULL
                             ELSE CAST(ROUND(AVG(r.Rating), 1) AS DECIMAL(3,1)) 
                        END AS 'Average Rating'
                    FROM 
                        JobPostings jp
                    INNER JOIN 
                        Companies c ON jp.CompanyID = c.CompanyID
                    INNER JOIN 
                        Applications a ON jp.JobPostingID = a.JobPostingID
                    INNER JOIN 
                        Interviews i ON a.ApplicationID = i.ApplicationID
                    INNER JOIN 
                        Booths b ON c.CompanyID = b.CompanyID
                    LEFT JOIN 
                        Recruiters rc ON i.RecruiterID = rc.RecruiterID
                    LEFT JOIN 
                        Reviews r ON rc.RecruiterID = r.RecruiterID
                    WHERE 
                        b.JobFairID = @JobFairID
                    GROUP BY 
                        c.Name, jp.Title
                    ORDER BY 
                        c.Name, jp.Title";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@JobFairID", jobFairId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Create a display version of the data for the grid
                DataTable displayDt = dt.Copy();
                displayDt.Columns.Add("Rating Display", typeof(string));

                // Format the average rating for display
                foreach (DataRow row in displayDt.Rows)
                {
                    if (row["Average Rating"] != DBNull.Value)
                    {
                        double rating = Convert.ToDouble(row["Average Rating"]);
                        row["Rating Display"] = rating.ToString("0.0");
                    }
                    else
                    {
                        row["Rating Display"] = "N/A";
                    }
                }

                // Remove the numeric column from display and rename the string column
                displayDt.Columns.Remove("Average Rating");
                displayDt.Columns["Rating Display"].ColumnName = "Average Rating";

                dataGridView1.DataSource = displayDt;
                reportData = displayDt;
                currentReportTitle = "Interview Statistics Report - " + comboBoxJobFair.Text;
            }
        }

        private void GenerateJobApplicationsReport(int jobFairId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        jp.Title AS 'Job Title',
                        c.Name AS 'Company',
                        jp.JobType AS 'Job Type',
                        jp.SalaryRange AS 'Salary Range',
                        COUNT(a.ApplicationID) AS 'Total Applications',
                        SUM(CASE WHEN a.Status = 'Applied' THEN 1 ELSE 0 END) AS 'Applied',
                        SUM(CASE WHEN a.Status = 'Shortlisted' THEN 1 ELSE 0 END) AS 'Shortlisted',
                        SUM(CASE WHEN a.Status = 'Interviewed' THEN 1 ELSE 0 END) AS 'Interviewed',
                        SUM(CASE WHEN a.Status = 'Accepted' THEN 1 ELSE 0 END) AS 'Accepted',
                        CAST(SUM(CASE WHEN a.Status = 'Accepted' THEN 1 ELSE 0 END) * 100.0 / 
                            CASE WHEN COUNT(a.ApplicationID) = 0 THEN 1 ELSE COUNT(a.ApplicationID) END 
                            AS DECIMAL(5,2)) AS 'Success Rate (%)'
                    FROM 
                        JobPostings jp
                    INNER JOIN 
                        Companies c ON jp.CompanyID = c.CompanyID
                    INNER JOIN 
                        Booths b ON c.CompanyID = b.CompanyID
                    LEFT JOIN 
                        Applications a ON jp.JobPostingID = a.JobPostingID AND 
                            a.ApplicationDate BETWEEN CAST((SELECT EventDate FROM JobFairEvents WHERE JobFairID = @JobFairID) AS DATE)
                            AND CAST(DATEADD(day, 14, (SELECT EventDate FROM JobFairEvents WHERE JobFairID = @JobFairID)) AS DATE)
                    WHERE 
                        b.JobFairID = @JobFairID
                    GROUP BY 
                        jp.Title, c.Name, jp.JobType, jp.SalaryRange
                    ORDER BY 
                        COUNT(a.ApplicationID) DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@JobFairID", jobFairId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
                reportData = dt;
                currentReportTitle = "Job Application Trends Report - " + comboBoxJobFair.Text;
            }
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            if (reportData == null || reportData.Rows.Count == 0)
            {
                MessageBox.Show("Please generate a report first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string format = comboBoxExportFormat.SelectedItem.ToString();
            
            try
            {
                switch (format)
                {
                    case "CSV":
                        ExportToCSV();
                        break;
                    case "Excel":
                        ExportToExcel();
                        break;
                    case "PDF":
                        ExportToPDF();
                        break;
                    case "Text":
                        ExportToText();
                        break;
                    default:
                        MessageBox.Show("Please select a valid export format.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting report: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV files (*.csv)|*.csv";
            saveDialog.Title = "Export to CSV";
            saveDialog.FileName = currentReportTitle.Replace(" ", "_") + ".csv";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder csv = new StringBuilder();
                
                // Add column headers
                string[] columnNames = new string[reportData.Columns.Count];
                for (int i = 0; i < reportData.Columns.Count; i++)
                {
                    columnNames[i] = reportData.Columns[i].ColumnName;
                }
                csv.AppendLine(string.Join(",", columnNames));
                
                // Add rows
                foreach (DataRow row in reportData.Rows)
                {
                    string[] fields = new string[reportData.Columns.Count];
                    for (int i = 0; i < reportData.Columns.Count; i++)
                    {
                        if (row[i] != null && row[i] != DBNull.Value)
                        {
                            string field = row[i].ToString();
                            // Escape if field contains comma or quotes
                            if (field.Contains(",") || field.Contains("\""))
                            {
                                field = "\"" + field.Replace("\"", "\"\"") + "\"";
                            }
                            fields[i] = field;
                        }
                        else
                        {
                            fields[i] = "";
                        }
                    }
                    csv.AppendLine(string.Join(",", fields));
                }
                
                File.WriteAllText(saveDialog.FileName, csv.ToString());
                MessageBox.Show("Report exported successfully to CSV.", "Export Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportToExcel()
        {
            // This is a simplified Excel export that creates a CSV but with Excel extension
            // In a real app, you would use a library like EPPlus or Microsoft.Office.Interop.Excel
            
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveDialog.Title = "Export to Excel";
            saveDialog.FileName = currentReportTitle.Replace(" ", "_") + ".xlsx";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                // Create CSV content (simplified Excel format)
                StringBuilder csv = new StringBuilder();
                
                // Add column headers
                string[] columnNames = new string[reportData.Columns.Count];
                for (int i = 0; i < reportData.Columns.Count; i++)
                {
                    columnNames[i] = reportData.Columns[i].ColumnName;
                }
                csv.AppendLine(string.Join("\t", columnNames));
                
                // Add rows
                foreach (DataRow row in reportData.Rows)
                {
                    string[] fields = new string[reportData.Columns.Count];
                    for (int i = 0; i < reportData.Columns.Count; i++)
                    {
                        if (row[i] != null && row[i] != DBNull.Value)
                        {
                            fields[i] = row[i].ToString();
                        }
                        else
                        {
                            fields[i] = "";
                        }
                    }
                    csv.AppendLine(string.Join("\t", fields));
                }
                
                File.WriteAllText(saveDialog.FileName, csv.ToString());
                MessageBox.Show("Report exported successfully to Excel format.", "Export Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportToPDF()
        {
            // In a real application, you would use a library like iTextSharp or PDFsharp
            // This is just a placeholder message
            
            MessageBox.Show("PDF export requires additional libraries. In a real application, this would export to PDF format.", 
                "PDF Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportToText()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text files (*.txt)|*.txt";
            saveDialog.Title = "Export to Text";
            saveDialog.FileName = currentReportTitle.Replace(" ", "_") + ".txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder textContent = new StringBuilder();
                
                // Add title
                textContent.AppendLine(currentReportTitle);
                textContent.AppendLine(new string('-', currentReportTitle.Length));
                textContent.AppendLine();
                
                // Determine column widths
                int[] columnWidths = new int[reportData.Columns.Count];
                for (int i = 0; i < reportData.Columns.Count; i++)
                {
                    columnWidths[i] = reportData.Columns[i].ColumnName.Length;
                    
                    foreach (DataRow row in reportData.Rows)
                    {
                        if (row[i] != null && row[i] != DBNull.Value)
                        {
                            int fieldLength = row[i].ToString().Length;
                            if (fieldLength > columnWidths[i])
                            {
                                columnWidths[i] = fieldLength;
                            }
                        }
                    }
                    
                    // Add some padding
                    columnWidths[i] += 2;
                }
                
                // Build header
                string headerLine = "";
                string separatorLine = "";
                for (int i = 0; i < reportData.Columns.Count; i++)
                {
                    headerLine += reportData.Columns[i].ColumnName.PadRight(columnWidths[i]);
                    separatorLine += new string('-', columnWidths[i]);
                }
                textContent.AppendLine(headerLine);
                textContent.AppendLine(separatorLine);
                
                // Add rows
                foreach (DataRow row in reportData.Rows)
                {
                    string rowLine = "";
                    for (int i = 0; i < reportData.Columns.Count; i++)
                    {
                        if (row[i] != null && row[i] != DBNull.Value)
                        {
                            rowLine += row[i].ToString().PadRight(columnWidths[i]);
                        }
                        else
                        {
                            rowLine += "".PadRight(columnWidths[i]);
                        }
                    }
                    textContent.AppendLine(rowLine);
                }
                
                // Add summary
                textContent.AppendLine();
                textContent.AppendLine($"Total records: {reportData.Rows.Count}");
                textContent.AppendLine($"Report generated: {DateTime.Now}");
                
                File.WriteAllText(saveDialog.FileName, textContent.ToString());
                MessageBox.Show("Report exported successfully to Text.", "Export Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        // Method to apply rounded corners to buttons
        private void ApplyRoundedCorners()
        {
            // A simple class to create rounded button corners
            System.Drawing.Drawing2D.GraphicsPath GetRoundPath(Rectangle rect, int radius)
            {
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                float curveSize = radius * 2F;
                
                path.StartFigure();
                path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
                path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
                path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
                path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
                path.CloseFigure();
                return path;
            }
            
            // Apply to each button
            void ApplyRadius(Button btn)
            {
                btn.Region = new Region(GetRoundPath(new Rectangle(0, 0, btn.Width, btn.Height), 10));
                btn.FlatAppearance.BorderSize = 0;
            }
            
            // Apply rounded corners to all buttons
            ApplyRadius(btnBack);
            ApplyRadius(btnExportCSV);
            ApplyRadius(btnGenerateReport);
        }
    }
}
