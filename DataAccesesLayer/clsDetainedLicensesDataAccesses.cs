using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    public static class clsDetainedLicensesDataAccesses
    {
        

        static public bool FindDetainedLicense(int DetainID,ref int LicnseId, ref DateTime DetainDate, ref int fees,
            ref int CreatedBy,ref bool IsReleased,ref DateTime ReleaseDate ,ref int ReleasedByUserID,ref int ReleaseApplicationID)
        {
            bool isFound = false;
            string query = "SELECT * FROM DetainedLicenses WHERE DetainID=@id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", DetainID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            isFound = true;
                            LicnseId = Convert.ToInt32(reader["LicenseID"]);
                            DetainDate = Convert.ToDateTime(reader["DetainDate"]);
                            fees = Convert.ToInt32(reader["FineFees"]);
                            CreatedBy = Convert.ToInt32(reader["CreatedByUserID"]);
                            IsReleased = Convert.ToBoolean(reader["IsReleased"]);
                            if (reader["ReleaseDate"] != DBNull.Value)
                            {
                                ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]);
                            }
                            else
                            {
                                ReleaseDate = DateTime.MinValue; // or any default value you prefer
                            }
                            if (reader["ReleasedByUserID"] != DBNull.Value)
                            {
                                ReleasedByUserID = Convert.ToInt32(reader["ReleasedByUserID"]);
                            }
                            else
                            {
                                ReleasedByUserID = -1; // or any default value you prefer
                            }
                            if (reader["ReleaseApplicationID"] != DBNull.Value)
                            {
                                ReleaseApplicationID = Convert.ToInt32(reader["ReleaseApplicationID"]);
                            }
                            else
                            {
                                ReleaseApplicationID = -1; // or any default value you prefer
                            }
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
        static public bool FindDetainedLicenseByLicenseID(int LicenseID, ref int DetainID, ref DateTime DetainDate, ref int fees,
            ref int CreatedBy, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;
            string query = "SELECT * FROM DetainedLicenses WHERE LicenseID=@LicenseID and IsReleased=0";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            isFound = true;
                            DetainID = Convert.ToInt32(reader["DetainID"]);
                            DetainDate = Convert.ToDateTime(reader["DetainDate"]);
                            fees = Convert.ToInt32(reader["FineFees"]);
                            CreatedBy = Convert.ToInt32(reader["CreatedByUserID"]);
                            IsReleased = Convert.ToBoolean(reader["IsReleased"]);
                            if (reader["ReleaseDate"] != DBNull.Value)
                            {
                                ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]);
                            }
                            else
                            {
                                ReleaseDate = DateTime.MinValue; // or any default value you prefer
                            }
                            if (reader["ReleasedByUserID"] != DBNull.Value)
                            {
                                ReleasedByUserID = Convert.ToInt32(reader["ReleasedByUserID"]);
                            }
                            else
                            {
                                ReleasedByUserID = -1; // or any default value you prefer
                            }
                            if (reader["ReleaseApplicationID"] != DBNull.Value)
                            {
                                ReleaseApplicationID = Convert.ToInt32(reader["ReleaseApplicationID"]);
                            }
                            else
                            {
                                ReleaseApplicationID = -1; // or any default value you prefer
                            }
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    isFound = false;
                }
            }
            return isFound;
        }
        static public int AddNewDetainedLicense(int LicenseID, DateTime DetainDate, int FineFees, int CreatedByUserID, bool IsReleased, DateTime? ReleaseDate = null, int? ReleasedByUserID = null, int? ReleaseApplicationID = null)
        {
            int newId = -1;
            string query = "INSERT INTO DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID) " +
                           "OUTPUT INSERTED.DetainID VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased, @ReleaseDate, @ReleasedByUserID, @ReleaseApplicationID)";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
                comm.Parameters.AddWithValue("@LicenseID", LicenseID);
                comm.Parameters.AddWithValue("@DetainDate", DetainDate);
                comm.Parameters.AddWithValue("@FineFees", FineFees);
                comm.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                comm.Parameters.AddWithValue("@IsReleased", IsReleased);
                comm.Parameters.AddWithValue("@ReleaseDate", (object)ReleaseDate ?? DBNull.Value);
                comm.Parameters.AddWithValue("@ReleasedByUserID", (object)ReleasedByUserID ?? DBNull.Value);
                comm.Parameters.AddWithValue("@ReleaseApplicationID", (object)ReleaseApplicationID ?? DBNull.Value);
                try
                {
                    connection.Open();
                    var Id = comm.ExecuteScalar();
                    if (Id != null && int.TryParse(Id.ToString(), out int insertedID))
                    {
                        newId = insertedID;
                    }
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    newId = -1;
                }
            }
            return newId;

        }

        static public bool UpdateDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, int FineFees, int CreatedByUserID, bool IsReleased, DateTime? ReleaseDate = null, int? ReleasedByUserID = null, int? ReleaseApplicationID = null)
        {
            bool isUpdated = false;
            string query = "UPDATE DetainedLicenses SET LicenseID=@LicenseID, DetainDate=@DetainDate, FineFees=@FineFees, CreatedByUserID=@CreatedByUserID, IsReleased=@IsReleased, ReleaseDate=@ReleaseDate, ReleasedByUserID=@ReleasedByUserID, ReleaseApplicationID=@ReleaseApplicationID WHERE DetainID=@DetainID";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DetainID", DetainID);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                command.Parameters.AddWithValue("@DetainDate", DetainDate);
                command.Parameters.AddWithValue("@FineFees", FineFees);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@IsReleased", IsReleased);
                command.Parameters.AddWithValue("@ReleaseDate", (object)ReleaseDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@ReleasedByUserID", (object)ReleasedByUserID ?? DBNull.Value);
                command.Parameters.AddWithValue("@ReleaseApplicationID", (object)ReleaseApplicationID ?? DBNull.Value);
                try
                {
                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();
                    isUpdated = rowsAffected > 0;
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    isUpdated = false;
                }
            }
            return isUpdated;
        }
   
    static public bool DeleteDetainedLicense(int DetainID)
        {
            bool isDeleted = false;
            string query = "DELETE FROM DetainedLicenses WHERE DetainID=@DetainID";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DetainID", DetainID);
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    isDeleted = rowsAffected > 0;
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    isDeleted = false;
                }
            }
            return isDeleted;
        }
    
    static public bool ReleaseDetainedLicense(int DetainID, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            bool isReleased = false;
            string query = "UPDATE DetainedLicenses SET IsReleased=1, ReleaseDate=@ReleaseDate, ReleasedByUserID=@ReleasedByUserID, ReleaseApplicationID=@ReleaseApplicationID WHERE DetainID=@DetainID";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DetainID", DetainID);
                command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
                command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
                command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    isReleased = rowsAffected > 0;
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    isReleased = false;
                }
            }
            return isReleased;
        }
   
    static public DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM DetainedLicenses";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    dt = null;
                }
            }
            return dt;
        }
        static public int GetDetainedCount()
        {
            int count = 0;
            string query = "SELECT COUNT(*) FROM DetainedLicenses WHERE IsReleased=0";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    count = -1;
                }
            }
            return count;
        }
    }
}
