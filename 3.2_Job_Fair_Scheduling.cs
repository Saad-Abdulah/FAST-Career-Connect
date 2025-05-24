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
using System.Data.SqlTypes;
using System.Configuration;

namespace Fast_Connect_DB_Final_project
{
    public partial class TPO_Job_Fair_Management : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private int selectedJobFairID = -1;

        /// <summary>
        /// Adjusts UI controls for better visibility and usability
        /// </summary>
        private void AdjustUIControls()
        {
            // We now have proper labels and positioning in the Designer
            // This method is kept for any dynamic adjustments needed
            
            // Make sure the Add Event button has appropriate styling
            btnAdd_event.BackColor = Color.Green;
            btnAdd_event.ForeColor = Color.White;
            
            // Check if we need to make any adjustments based on screen resolution
            if (this.ClientSize.Width < 900)
            {
                // Adjust for smaller screens if needed
                lblInstructions.Font = new Font("Microsoft Sans Serif", 8);
            }
        }

        public TPO_Job_Fair_Management()
        {
            try
            {
                // Initialize the form from designer code
                InitializeComponent();
                
                // UI adjustments should be done AFTER InitializeComponent
                // These will be handled in the form load event instead
                
                // Set up error handlers for controls
                cmbCompany.Format += (s, e) => { 
                    try { e.Value = e.Value.ToString(); } 
                    catch { e.Value = "Invalid Value"; } 
                };
                
                cmbCoordinator.Format += (s, e) => { 
                    try { e.Value = e.Value.ToString(); } 
                    catch { e.Value = "Invalid Value"; } 
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing the Job Fair Management form: " + ex.Message, 
                    "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TPO_Job_Fair_Management_Load(object sender, EventArgs e)
        {
            try
            {
                // Apply UI adjustments first - do this after designer initialization
                AdjustUIControls();
                
                // Check database schema first
                if (!VerifyDatabaseSchema())
                {
                    MessageBox.Show("There are issues with the database schema. Some functionality may be limited.", 
                        "Database Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
                LoadJobFairEvents();
                LoadCompanies();
                LoadCoordinators();
                
                // Initially disable booth management panel until a job fair is selected
                panelBoothManagement.Enabled = false;
                lblSelectedJobFair.Text = "Select a job fair from the list";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing form: " + ex.Message, "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Checks if required tables exist in the database
        /// </summary>
        /// <returns>True if all required tables exist</returns>
        private bool VerifyDatabaseSchema()
        {
            bool result = true;
            List<string> requiredTables = new List<string> { "JobFairEvents", "Booths", "Companies", "Users" };
            List<string> missingTables = new List<string>();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    
                    foreach (string tableName in requiredTables)
                    {
                        SqlCommand cmd = new SqlCommand(
                            "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName", conn);
                        cmd.Parameters.AddWithValue("@TableName", tableName);
                        
                        int count = (int)cmd.ExecuteScalar();
                        if (count == 0)
                        {
                            missingTables.Add(tableName);
                            result = false;
                        }
                    }
                    
                    if (missingTables.Count > 0)
                    {
                        string missing = string.Join(", ", missingTables);
                        MessageBox.Show($"The following tables are missing from the database: {missing}\n\n" +
                            "The application expects these tables to exist. Please make sure the database is properly set up.",
                            "Database Schema Issue", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error verifying database schema: " + ex.Message, "Database Check Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result = false;
                }
            }
            
            return result;
        }

        #region JobFair Events
        private void LoadJobFairEvents()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    
                    // SQL query to get job fair events
                    SqlCommand cmd = new SqlCommand("SELECT JobFairID, EventDate, EventTime, Venue FROM JobFairEvents", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Clear existing data
                    dgvjobfairs.Rows.Clear();

                    // Display record count in status strip or for debugging
                    if (dt.Rows.Count == 0)
                    {
                        // No job fairs found - show a user-friendly message
                        MessageBox.Show("No job fair events found in the database. Add a new event to get started.", 
                            "No Events", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Populate the DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            // Format date and time properly for display
                            DateTime eventDate = Convert.ToDateTime(row["EventDate"]);
                            TimeSpan eventTime = TimeSpan.Parse(row["EventTime"].ToString());
                            
                            string formattedDateTime = $"{eventDate.ToShortDateString()} {new DateTime().Add(eventTime).ToString("h:mm tt")}";
                            
                            int rowIndex = dgvjobfairs.Rows.Add(
                                formattedDateTime,
                                row["Venue"].ToString()
                            );
                            
                            // Store JobFairID as a tag for later use
                            dgvjobfairs.Rows[rowIndex].Tag = row["JobFairID"];
                        }
                        catch (Exception ex)
                        {
                            // Log the error but continue processing other rows
                            Console.WriteLine($"Error processing row: {ex.Message}");
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    // Handle specific SQL exceptions
                    if (sqlEx.Number == 208) // Invalid object name error
                    {
                        MessageBox.Show("The JobFairEvents table doesn't exist in the database. Please ensure the database schema is correctly set up.", 
                            "Database Schema Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Database error loading job fair events: {sqlEx.Message}\nError Number: {sqlEx.Number}", 
                            "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading job fair events: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_event_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtVenue.Text))
            {
                MessageBox.Show("Please enter a venue for the job fair.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtVenue.Focus();
                return;
            }

            // Get date and time from picker
            DateTime selectedDate = dateTimePicker1.Value.Date;
            TimeSpan selectedTime = dateTimePicker1.Value.TimeOfDay;
            
            // Validate date is in future
            if (selectedDate < DateTime.Today)
            {
                MessageBox.Show("Please select a future date for the job fair.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker1.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Insert new job fair event
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO JobFairEvents (EventDate, EventTime, Venue) VALUES (@EventDate, @EventTime, @Venue)", conn);
                    
                    cmd.Parameters.Add("@EventDate", SqlDbType.Date).Value = selectedDate;
                    cmd.Parameters.Add("@EventTime", SqlDbType.Time).Value = selectedTime;
                    cmd.Parameters.Add("@Venue", SqlDbType.VarChar, 100).Value = txtVenue.Text.Trim();

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Job fair event added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadJobFairEvents();
                        txtVenue.Clear();
                        
                        // Reset date picker to current date/time
                        dateTimePicker1.Value = DateTime.Now;
                    }
                    else
                    {
                        MessageBox.Show("Failed to add job fair event. No rows affected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}\nError Number: {sqlEx.Number}", 
                    "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding job fair event: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_event_Click(object sender, EventArgs e)
        {
            if (dgvjobfairs.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a job fair event to remove.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int jobFairID = (int)dgvjobfairs.SelectedRows[0].Tag;
            
            // Check if there are any booths for this job fair
            bool hasBooths = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Booths WHERE JobFairID = @JobFairID", conn);
                    checkCmd.Parameters.AddWithValue("@JobFairID", jobFairID);
                    hasBooths = (int)checkCmd.ExecuteScalar() > 0;
                }
                catch (Exception)
                {
                    // Table might not exist yet, so no booths
                    hasBooths = false;
                }
            }

            if (hasBooths)
            {
                DialogResult result = MessageBox.Show(
                    "This job fair has booths assigned to it. Removing it will delete all associated booth information. Continue?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                    return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // First delete any associated booths
                    if (hasBooths)
                    {
                        SqlCommand deleteBoothsCmd = new SqlCommand("DELETE FROM Booths WHERE JobFairID = @JobFairID", conn);
                        deleteBoothsCmd.Parameters.AddWithValue("@JobFairID", jobFairID);
                        deleteBoothsCmd.ExecuteNonQuery();
                    }

                    // Then delete the job fair
                    SqlCommand cmd = new SqlCommand("DELETE FROM JobFairEvents WHERE JobFairID = @JobFairID", conn);
                    cmd.Parameters.AddWithValue("@JobFairID", jobFairID);
                    
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Job fair event removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadJobFairEvents();
                        
                        // Clear booths display and reset selected job fair
                        dgvbooths.Rows.Clear();
                        selectedJobFairID = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error removing job fair event: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Booths
        private void dgvjobfairs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvjobfairs.SelectedRows.Count > 0 && dgvjobfairs.SelectedRows[0].Tag != null)
            {
                try
                {
                    selectedJobFairID = (int)dgvjobfairs.SelectedRows[0].Tag;
                    LoadBooths(selectedJobFairID);
                    
                    // Enable the booth management controls
                    panelBoothManagement.Enabled = true;
                    
                    // Update the selected job fair label
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand("SELECT EventDate, Venue FROM JobFairEvents WHERE JobFairID = @JobFairID", conn);
                            cmd.Parameters.AddWithValue("@JobFairID", selectedJobFairID);
                            
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    DateTime eventDate = Convert.ToDateTime(reader["EventDate"]);
                                    string venue = reader["Venue"].ToString();
                                    lblSelectedJobFair.Text = $"Selected: {eventDate.ToShortDateString()} at {venue}";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error retrieving job fair details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (InvalidCastException castEx)
                {
                    // Handle case where Tag exists but is not an integer
                    MessageBox.Show("Error: Invalid job fair data format. Please try reloading the job fairs.", 
                        "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    selectedJobFairID = -1;
                    dgvbooths.Rows.Clear();
                    panelBoothManagement.Enabled = false;
                    lblSelectedJobFair.Text = "Select a job fair from the list";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error selecting job fair: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    selectedJobFairID = -1;
                    dgvbooths.Rows.Clear();
                    panelBoothManagement.Enabled = false;
                    lblSelectedJobFair.Text = "Select a job fair from the list";
                }
            }
            else
            {
                selectedJobFairID = -1;
                dgvbooths.Rows.Clear();
                
                // Disable the booth management controls
                panelBoothManagement.Enabled = false;
                lblSelectedJobFair.Text = "Select a job fair from the list";
            }
        }

        private void LoadBooths(int jobFairID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    
                    // First check if the job fair exists
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM JobFairEvents WHERE JobFairID = @JobFairID", conn);
                    checkCmd.Parameters.AddWithValue("@JobFairID", jobFairID);
                    int jobFairCount = (int)checkCmd.ExecuteScalar();
                    
                    if (jobFairCount == 0)
                    {
                        MessageBox.Show("The selected job fair no longer exists in the database.", 
                            "Data Consistency Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        selectedJobFairID = -1;
                        dgvbooths.Rows.Clear();
                        panelBoothManagement.Enabled = false;
                        lblSelectedJobFair.Text = "Select a job fair from the list";
                        return;
                    }
                    
                    // Use a safer query with LEFT JOINs to handle missing related data
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT b.BoothID, 
                               ISNULL(c.Name, 'Unknown Company') as CompanyName, 
                               b.Location, 
                               ISNULL(u.Name, 'Unknown User') as CoordinatorName 
                        FROM Booths b
                        LEFT JOIN Companies c ON b.CompanyID = c.CompanyID
                        LEFT JOIN Users u ON b.CoordinatorID = u.UserID
                        WHERE b.JobFairID = @JobFairID", conn);
                    
                    cmd.Parameters.AddWithValue("@JobFairID", jobFairID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Clear existing data
                    dgvbooths.Rows.Clear();

                    // Check if any booths were found
                    if (dt.Rows.Count == 0)
                    {
                        // No booths found for this job fair
                        lblSelectedJobFair.Text += " (No booths allocated yet)";
                    }
                    else
                    {
                        // Populate the DataGridView
                        foreach (DataRow row in dt.Rows)
                        {
                            try 
                            {
                                int rowIndex = dgvbooths.Rows.Add(
                                    row["CompanyName"].ToString(),
                                    row["Location"].ToString(),
                                    row["CoordinatorName"].ToString()
                                );
                                
                                // Store BoothID as a tag for later use
                                dgvbooths.Rows[rowIndex].Tag = row["BoothID"];
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error processing booth row: {ex.Message}");
                            }
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    // Handle specific SQL exceptions
                    if (sqlEx.Number == 208) // Invalid object name error
                    {
                        MessageBox.Show("The Booths table doesn't exist in the database. Please ensure the database schema is correctly set up.", 
                            "Database Schema Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (sqlEx.Number == 207) // Invalid column name
                    {
                        MessageBox.Show("There's a mismatch between the application and database column names. Please check the database schema.", 
                            "Schema Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Database error loading booths: {sqlEx.Message}\nError Number: {sqlEx.Number}", 
                            "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading booths: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadCompanies()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT CompanyID, Name FROM Companies", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    cmbCompany.DisplayMember = "Name";
                    cmbCompany.ValueMember = "CompanyID";
                    cmbCompany.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading companies: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void LoadCoordinators()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Get coordinators with the appropriate roles
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT UserID, Name FROM Users 
                        WHERE IsApproved = 1 AND (Role = 'TPO' OR Role = 'BoothCoordinator')", conn);
                    
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // If no coordinators found, show informational message
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No approved booth coordinators found in the system. Please add coordinators first.", 
                            "Coordinator Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Try to get any users to use as coordinators temporarily
                        cmd = new SqlCommand("SELECT UserID, Name FROM Users", conn);
                        dt = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }

                    cmbCoordinator.DisplayMember = "Name";
                    cmbCoordinator.ValueMember = "UserID";
                    cmbCoordinator.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading coordinators: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddBooth_Click(object sender, EventArgs e)
        {
            if (selectedJobFairID == -1)
            {
                MessageBox.Show("Please select a job fair first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please enter a location for the booth.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCompany.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a company.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCoordinator.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a coordinator.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get selected values
            int companyID, coordinatorID;
            try
            {
                companyID = Convert.ToInt32(cmbCompany.SelectedValue);
                coordinatorID = Convert.ToInt32(cmbCoordinator.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting selected values: {ex.Message}", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    
                    // Check if this company already has a booth in this job fair
                    SqlCommand checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Booths WHERE JobFairID = @JobFairID AND CompanyID = @CompanyID", conn);
                    checkCmd.Parameters.AddWithValue("@JobFairID", selectedJobFairID);
                    checkCmd.Parameters.AddWithValue("@CompanyID", companyID);
                    
                    int boothCount = (int)checkCmd.ExecuteScalar();
                    if (boothCount > 0)
                    {
                        MessageBox.Show("This company already has a booth in this job fair.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    // Add the booth
                    SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO Booths (JobFairID, CompanyID, Location, CoordinatorID) 
                        VALUES (@JobFairID, @CompanyID, @Location, @CoordinatorID)", conn);
                    
                    cmd.Parameters.AddWithValue("@JobFairID", selectedJobFairID);
                    cmd.Parameters.AddWithValue("@CompanyID", companyID);
                    cmd.Parameters.AddWithValue("@Location", txtLocation.Text.Trim());
                    cmd.Parameters.AddWithValue("@CoordinatorID", coordinatorID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Booth added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBooths(selectedJobFairID);
                        // Clear inputs
                        txtLocation.Clear();
                    }
                    else
                    {
                        MessageBox.Show("No rows were affected by the booth insertion.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (SqlException sqlEx)
                {
                    // Provide specific error messages based on SQL error
                    if (sqlEx.Number == 547) // Foreign key constraint violation
                    {
                        MessageBox.Show("One of the IDs provided does not exist in the database. Make sure the job fair, company, and coordinator are valid.",
                            "Foreign Key Constraint Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (sqlEx.Number == 2627) // Unique constraint violation
                    {
                        MessageBox.Show("A booth already exists for this company at this job fair.",
                            "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show($"Database error adding booth: {sqlEx.Message}\nError Number: {sqlEx.Number}",
                            "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding booth: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRemoveBooth_Click(object sender, EventArgs e)
        {
            if (dgvbooths.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a booth to remove.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int boothID = (int)dgvbooths.SelectedRows[0].Tag;
            
            DialogResult result = MessageBox.Show(
                "Are you sure you want to remove this booth?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM Booths WHERE BoothID = @BoothID", conn);
                        cmd.Parameters.AddWithValue("@BoothID", boothID);
                        
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Booth removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadBooths(selectedJobFairID);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error removing booth: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        #endregion

        private void Back_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dgvjobfairs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
