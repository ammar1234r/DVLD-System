using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    public static  class clsInternationalLicensesDataAccesses
    {
       

        public static bool FindInternationalLicenses(int InternationalLicenseID, ref int AppId,ref int LicnseId, ref int IssuedUsingLocalLicenseID, 
            ref DateTime IssueDate, ref DateTime ExpiryDate, ref int CreatedByUserID,ref bool IsActive)
        {
            bool isFound = false;
            string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID=@id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", InternationalLicenseID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            isFound = true;
                            AppId = Convert.ToInt32(reader["ApplicationID"]);
                            LicnseId = Convert.ToInt32(reader["LicenseID"]);
                            IsActive = Convert.ToBoolean(reader["IsActive"]);
                            IssuedUsingLocalLicenseID = Convert.ToInt32(reader["IssuedUsingLocalLicenseID"]);


                            IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                            ExpiryDate = Convert.ToDateTime(reader["ExpirationDate"]);
                            CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                        }
                    }
                    reader.Close();
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    isFound = false;
                }
            }
            return isFound;
        }

        public static int AddNewLicnse(  int AppId,  int LicnseId,  int IssuedUsingLocalLicenseID,
             DateTime IssueDate,  DateTime ExpiryDate,  int CreatedByUserID,  bool IsActive)
        {
            int newId = 0;
            string query = "INSERT INTO InternationalLicenses ( ApplicationID, LicenseID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, CreatedByUserID, IsActive) " +
                           "VALUES ( @ApplicationID, @LicenseID, @IssuedUsingLocalLicenseID, @IssueDate, @ExpirationDate, @CreatedByUserID, @IsActive); " +
                           "SELECT SCOPE_IDENTITY();";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ApplicationID", AppId);
                command.Parameters.AddWithValue("@LicenseID", LicnseId);
                command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpiryDate);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                try
                {
                    connection.Open();
                    var IdResult = command.ExecuteScalar();
                    if (IdResult != null && int.TryParse(IdResult.ToString(), out int insertedID))
                    {
                        newId = insertedID;
                    }
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    newId = -1; // or handle the error as needed
                }
            }
            return newId;
        }

        public static bool UpdateInternationalLicense(int InternationalLicenseID, int AppId, int LicnseId, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpiryDate, int CreatedByUserID, bool IsActive)
        {
            bool isUpdated = false;
            string query = "UPDATE InternationalLicenses SET ApplicationID=@ApplicationID, LicenseID=@LicenseID, " +
                           "IssuedUsingLocalLicenseID=@IssuedUsingLocalLicenseID, IssueDate=@IssueDate, " +
                           "ExpirationDate=@ExpirationDate, CreatedByUserID=@CreatedByUserID, IsActive=@IsActive " +
                           "WHERE InternationalLicenseID=@InternationalLicenseID";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                command.Parameters.AddWithValue("@ApplicationID", AppId);
                command.Parameters.AddWithValue("@LicenseID", LicnseId);
                command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpiryDate);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    isUpdated = rowsAffected > 0;
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    isUpdated = false; // or handle the error as needed
                }
            }
            return isUpdated;
        }
        public static bool DeleteInternationalLicense(int InternationalLicenseID)
        {
            bool isDeleted = false;
            string query = "DELETE FROM InternationalLicenses WHERE InternationalLicenseID=@InternationalLicenseID";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    isDeleted = rowsAffected > 0;
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    isDeleted = false; // or handle the error as needed
                }
            }
            return isDeleted;
        }
    }
}
