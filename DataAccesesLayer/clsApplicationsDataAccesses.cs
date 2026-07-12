using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    static public class clsApplicationsDataAccesses
    {
        static SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString);

        static public bool FindApplication(int Id, ref int ApplicationTypeID, ref int ApplicantID, ref DateTime ApplicationDate,
            ref int CreatedByUserID, ref byte AppStatus, ref int PaidFees, ref DateTime LastStatusDate)
        {
            bool result = false;
            string query = "SELECT * FROM Applications WHERE ApplicationID = @Id";
            using (SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        ApplicationTypeID = Convert.ToInt32(reader["ApplicationTypeID"]);
                        ApplicantID = Convert.ToInt32(reader["ApplicantPersonID"]);
                        ApplicationDate = Convert.ToDateTime(reader["ApplicationDate"]);
                        CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                        AppStatus = Convert.ToByte(reader["ApplicationStatus"]);
                        PaidFees = Convert.ToInt32(reader["PaidFees"]);
                        LastStatusDate = Convert.ToDateTime(reader["LastStatusDate"]);
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}"); System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    result = false;
                }
            }
            return result;
        }
        static public int AddNewApplication(int ApplicationTypeID, int ApplicantID, DateTime ApplicationDate,
            int CreatedByUserID, byte AppStatus, int PaidFees, DateTime LastStatusDate)
        {
            int newId = -1;
            string query = "INSERT INTO Applications (ApplicationTypeID, ApplicantPersonID, ApplicationDate, CreatedByUserID, ApplicationStatus, PaidFees, LastStatusDate) " +
                           "OUTPUT INSERTED.ApplicationID VALUES (@ApplicationTypeID, @ApplicantPersonID, @ApplicationDate, @CreatedByUserID, @ApplicationStatus, @PaidFees, @LastStatusDate)";
            using (SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, conn);
                comm.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                comm.Parameters.AddWithValue("@ApplicantPersonID", ApplicantID);
                comm.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                comm.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                comm.Parameters.AddWithValue("@ApplicationStatus", AppStatus);
                comm.Parameters.AddWithValue("@PaidFees", PaidFees);
                comm.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                try
                {
                    conn.Open();
                    var Id = comm.ExecuteScalar();
                    if (Id != null && int.TryParse(Id.ToString(), out int insertedID))
                    {
                        newId = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}"); System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    newId = -1;
                }
            }
            return newId;
        }

        static public bool UpdateApplication(int Id, int ApplicationTypeID, int ApplicantID, DateTime ApplicationDate,
            int CreatedByUserID, byte AppStatus, int PaidFees, DateTime LastStatusDate)
        {
            bool result = false;
            string query = "UPDATE Applications SET ApplicationTypeID = @ApplicationTypeID, ApplicantPersonID = @ApplicantPersonID, " +
                           "ApplicationDate = @ApplicationDate, CreatedByUserID = @CreatedByUserID, ApplicationStatus = @ApplicationStatus, " +
                           "PaidFees = @PaidFees, LastStatusDate = @LastStatusDate WHERE ApplicationID = @Id";

            using (SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantID);
                command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@ApplicationStatus", AppStatus);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                try
                {
                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    result = rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}"); System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    result = false;
                }
            }
            return result;
        }

        static public bool DeleteApplication(int Id)
        {
            bool result = false;
            string query = "DELETE FROM Applications WHERE ApplicationID = @Id";
            using (SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);
                try
                {
                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    result = rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}"); System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    result = false;
                }
            }
            return result;
        }
        static public DataTable GetAllApp()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Applications";
            using (SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString))
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
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    dt = null;
                }
            } return dt;
        }

   
        public static DataTable GetAllLocalApp()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand("sp_GetAllLocalApplications", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) dt.Load(reader);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                    dt = null;
                }
            }
            return dt;
        }


        

        public static DataTable GetLocalAppByStatus(int status)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand("sp_GetLocalAppByStatus", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Status", status);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) dt.Load(reader);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                    dt = null;
                }
            }
            return dt;
        }
    }

}