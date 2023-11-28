using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;

namespace Dental.Database
{
    public class Remining
    {
        private MySqlConnection connection;
        private string connectionString = "Server=localhost;Database=DentalClinic;User ID=root;Password=;";

        public void reminingUser(out string patientFullName, out double remainingAmount)
        {
            patientFullName = "";
            remainingAmount = 0;

            connection = new MySqlConnection(connectionString);
            connection.Open();

            try
            {
                // Fetch patient information from the database
                string selectPatientSql = "SELECT CONCAT(FirstName, ' ', LastName) AS PatientName, RemainingAmount FROM Patients LIMIT 1";

                using (MySqlCommand selectPatientCommand = new MySqlCommand(selectPatientSql, connection))
                {
                    using (MySqlDataReader patientReader = selectPatientCommand.ExecuteReader())
                    {
                        if (patientReader.Read())
                        {
                            // Get patient information from the database
                            patientFullName = patientReader["PatientName"].ToString();
                            remainingAmount = Convert.ToDouble(patientReader["RemainingAmount"]);
                        }
                        else
                        {
                            // No patients found
                            patientFullName = string.Empty;
                            remainingAmount = 0.0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or display an error message
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        internal void PopulateDataGridView(Guna2DataGridView guna2DataGridView1)
        {
            throw new NotImplementedException();
        }
    }
}
