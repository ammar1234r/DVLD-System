using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    public static class clsLocalDrivingLicenseApplicationsDataAccesses
    {
        static SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString);
        static public bool FindLocalDrivingLicenseApp(int LocalDrivingLicenseApplicationID, ref int ApplicationID, ref int LicenseClassID)
        {
            bool Isfound = false;
            string query = "select * from LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID=@id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, conn);
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@id", LocalDrivingLicenseApplicationID);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Isfound = true;
                            ApplicationID = reader.GetInt32(1);
                            LicenseClassID = reader.GetInt32(2);
                        }
                    }
                    reader.Close();
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    Isfound = false;
                }
            }
            return Isfound;
        }

        static public int AddNewLocalDrivingLicenseApp(int ApplicationID, int LicenseClassID)
        {
            int newId = -1;
            string query = "insert into LocalDrivingLicenseApplications (ApplicationID, LicenseClassID) output INSERTED.LocalDrivingLicenseApplicationID values (@ApplicationID, @LicenseClassID)";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, conn);
            comm.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            comm.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                conn.Open();
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
        static public bool UpdateLocalDrivingLicenseApp(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            bool result = false;
            string query = "UPDATE LocalDrivingLicenseApplications SET ApplicationID = @ApplicationID, LicenseClassID = @LicenseClassID WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                conn.Open();
                int rowsAffected = command.ExecuteNonQuery();
                result = rowsAffected > 0;
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                result = false;
            }
            
            }
            return result;
        }

        public static bool DeleteLocalDrivingLicenseApp(int LocalDrivingLicenseApplicationID)
        {
            bool result = false;
            string query = "DELETE FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            try
            {
                conn.Open();
                int rowsAffected = command.ExecuteNonQuery();
                result = rowsAffected > 0;
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                result = false;
            }
            
            }
            return result;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM LocalDrivingLicenseApplications";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
            try
            {
                conn.Open();
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
    }
}