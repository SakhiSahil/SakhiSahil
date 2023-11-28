using Dental.Database;
using Guna.Charts.WinForms;
using Guna.UI2.WinForms;
using Intuit.Ipp.Data;
using LiveCharts.Wpf;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media;


namespace Dental.Forms
{
    public partial class DashboardForm : Form
    {

        // create MySQL Connection
        private MySqlConnection connection;
        private string connectionString = "Server=localhost;Database=DentalClinic;User ID=root;Password=;";



        public DashboardForm()
        {
            InitializeComponent();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        // Event handler for the form load event
        private void DashboardForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Establish a connection to the database
                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open(); // Open the database connection

                    // Update labels with counts from different queries
                    UpdateLabel(actPts, ExecuteScalarAsInt("SELECT COUNT(*) FROM Patients WHERE DATE(RegistrationDate) = CURDATE()"));
                    UpdateLabel(toPts, ExecuteScalarAsInt("SELECT COUNT(*) FROM dentists D LEFT JOIN Appointments A ON D.DentistID = A.DentistID WHERE A.AppointmentID IS NULL OR DATE(A.AppointmentDate) < CURDATE()"));
                    UpdateLabel(tAppoi, ExecuteScalarAsInt("SELECT COUNT(*) FROM Appointments WHERE DATE(AppointmentDate) = CURDATE() AND IsDone = 0"));

                    // Retrieve today's earnings and display them formatted as currency
                    object todaysEarningsObj = ExecuteScalar("SELECT SUM(Amount) FROM Billing WHERE DATE(BillingDate) = CURDATE()");
                    decimal todaysEarnings = todaysEarningsObj is decimal ? (decimal)todaysEarningsObj : 0.0m;
                    toDayEar.Text = todaysEarnings.ToString("C");

                    // Load the monthly income chart
                    LoadMonthlyIncomeChart();

                    // Load the gender chart
                    LoadGenderChart();
                }
            }
            catch (Exception ex)
            {
                // Display an error message in case of an exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Update the text of a label with the given value
        private void UpdateLabel(Label label, object value)
        {
            label.Text = $"{value}";
        }

        // Execute a SQL query and return the result as an integer
        private int ExecuteScalarAsInt(string query)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                return result is int ? (int)result : 0;
            }
        }

        // Execute a SQL query and return the result as an object
        private object ExecuteScalar(string query)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                return command.ExecuteScalar();
            }
        }

        // Load data for the monthly income chart
        private void LoadMonthlyIncomeChart()
        {
            string incomeChartSql = "SELECT MONTH(BillingDate) AS Month, COALESCE(SUM(Amount), 0) AS TotalIncome FROM Billing A GROUP BY MONTH(BillingDate)";
            using (MySqlCommand incomeChartCommand = new MySqlCommand(incomeChartSql, connection))
            using (MySqlDataReader incomeChartReader = incomeChartCommand.ExecuteReader())
            {
                gunaSplineAreaDataset1.DataPoints.Clear(); // Clear existing data points

                // Iterate over the results and add data points to the chart
                while (incomeChartReader.Read())
                {
                    if (int.TryParse(incomeChartReader["Month"]?.ToString(), out int month) &&
                        double.TryParse(incomeChartReader["TotalIncome"]?.ToString(), out double totalIncome))
                    {
                        gunaSplineAreaDataset1.DataPoints.Add(GetMonthName(month), totalIncome);
                    }
                }
            }
        }

        // Load data for the gender chart
        private void LoadGenderChart()
        {
            gunaBarDataset1.DataPoints.Clear(); // Clear existing data points
            gunaBarDataset2.DataPoints.Clear(); // Clear existing data points

            // Iterate over the last 6 days
            for (int i = 6; i >= 0; i--)
            {
                DateTime currentDate = DateTime.Now.Date.AddDays(-i);
                string dayAbbreviation = currentDate.ToString("ddd");

                // Construct the SQL query to fetch gender counts for the current day
                string genderCountSql = $"SELECT SUM(CASE WHEN Gender = 'Male' THEN 1 ELSE 0 END) AS MaleCount, " +
                                        $"SUM(CASE WHEN Gender = 'Female' THEN 1 ELSE 0 END) AS FemaleCount " +
                                        $"FROM Patients LEFT JOIN Appointments ON Patients.PatientID = Appointments.PatientID " +
                                        $"WHERE RegistrationDate = '{currentDate:yyyy-MM-dd}'";

                Console.WriteLine($"Executing SQL query: {genderCountSql}");

                // Execute the query and read the results
                using (MySqlCommand genderCountCommand = new MySqlCommand(genderCountSql, connection))
                using (MySqlDataReader genderCountReader = genderCountCommand.ExecuteReader())
                {
                    genderCountReader.Read(); // Read the single result row

                    // Parse the male and female counts
                    if (int.TryParse(genderCountReader["MaleCount"]?.ToString(), out int maleCount) &&
                        int.TryParse(genderCountReader["FemaleCount"]?.ToString(), out int femaleCount))
                    {
                        // Add data points to the gender chart
                        gunaBarDataset1.DataPoints.Add(new Guna.Charts.WinForms.LPoint(dayAbbreviation, maleCount));
                        gunaBarDataset2.DataPoints.Add(new Guna.Charts.WinForms.LPoint(dayAbbreviation, femaleCount));
                    }
                }
            }
        }

        // Get the month name for a given month number
        private string GetMonthName(int month)
        {
            return new DateTime(2023, month, 1).ToString("MMMM");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }

}
