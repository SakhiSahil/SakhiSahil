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

namespace Dental.Forms
{
    public partial class DashboardForm : Form
    {

        private MySqlConnection connection;
        private string connectionString = "Server=localhost;Database=test;User=root;Password=;";

        public DashboardForm()
        {
            InitializeComponent();
        }

        private void LoadChartData()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT MONTH(date) AS Month, SUM(income - expenses) AS NetProfit " +
                                   "FROM income " +
                                   "GROUP BY MONTH(date)";

                    using (var cmd = new MySqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Clear existing series and axis labels
                        chart1.Series.Clear();
                        chart1.AxisX.Clear();

                        // Create a new LineSeries
                        var series = new LiveCharts.Wpf.LineSeries
                        {
                            Title = "Net Profit",
                            Values = new LiveCharts.ChartValues<double>()
                        };

                        // Add the series to the chart
                        chart1.Series.Add(series);

                        // Configure X-axis labels with month names
                        chart1.AxisX.Add(new LiveCharts.Wpf.Axis
                        {
                            Labels = Enum.GetNames(typeof(MonthEnum)).ToList(), // Assuming you have an enum for months
                            Separator = new LiveCharts.Wpf.Separator(),
                        });

                        while (reader.Read())
                        {
                            int month = reader.GetInt32(reader.GetOrdinal("Month"));
                            double netProfit = reader.GetDouble(reader.GetOrdinal("NetProfit"));

                            // Add data point to the series
                            series.Values.Add(netProfit);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void ShowRowCount()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) AS RowCount FROM income"; // replace with your actual table name

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            int rowCount = Convert.ToInt32(result);

                            // Set the value of the SolidGauge chart
                           // angularGauge1.Value = rowCount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void DashboardForm_Load(object sender, EventArgs e)
        {
            LoadChartData();
        }
    }
}
