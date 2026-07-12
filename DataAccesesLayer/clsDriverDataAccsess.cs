using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace DataAccesesLayer
{
    public static class clsDriverDataAccsess
    {


        public static int FindDriverIdByPersonId(int peopleId)
        {
            int driverId = -1;
            string query = "SELECT DriverID FROM Drivers WHERE PersonID = @PersonID";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@PersonID", peopleId);
                try
                {
                    connection.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int id))
                    {
                        driverId = id;
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}"); 

                    driverId = -1;
                }
            }
            return driverId;
        }
        public static bool FindDriverById(int id, ref int personId, ref int CreatedBy_ID, ref DateTime CreatedTime)
        {
            bool isFound = false;
            string query = "SELECT * FROM Drivers WHERE DriverID = @Id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        isFound = true;
                        personId = Convert.ToInt32(reader["PersonID"]);
                        CreatedBy_ID = Convert.ToInt32(reader["CreatedByUserID"]);
                        CreatedTime = Convert.ToDateTime(reader["CreatedDate"]);
                    }


                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    isFound = false;
                }
            }
            return isFound;
        }

        public static bool UserExists(int userId)
        {
            bool exists = false;
            string query = "SELECT COUNT(*) FROM People WHERE PersonID = @UserID";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserID", userId);
                try
                {
                    connection.Open();
                    int count = (int)cmd.ExecuteScalar();
                    exists = count > 0;
                } catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                    exists = false;
                }
            }
            return exists;
        }
        public static int AddDriver(int personId, int createdById, DateTime createdTime)
        {
            int DriverId = -1;
            string query = @" INSERT INTO Drivers(PersonID, CreatedByUserID, CreatedDate) OUTPUT INSERTED.DriverID VALUES(@PersonID, @CreatedByUserID, @CreatedDate)";
            using (SqlConnection Connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, Connection);
                if (UserExists(personId))
                {
                    cmd.Parameters.AddWithValue("@PersonID", personId);
                }
                else
                {

                    return -1; // PersonID does not exist
                }
                cmd.Parameters.AddWithValue("@CreatedByUserID", createdById);
                cmd.Parameters.AddWithValue("@CreatedDate", createdTime);
                try
                {
                    Connection.Open();
                    var Id = cmd.ExecuteScalar();
                    if (Id != null && int.TryParse(Id.ToString(), out int insertedID))
                    {
                        DriverId = insertedID;
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}"); 

                    {
                        DriverId = -1;
                    }
                }
                return DriverId;
            } }

        public static bool UpdateDriver(int id, int personId, int createdById, DateTime createdTime)
        {
            bool isUpdated = false;
            string query = "UPDATE Drivers SET PersonID = @PersonID, CreatedByUserID = @CreatedByUserID, CreatedDate = @CreatedDate WHERE DriverID = @Id";
            using (SqlConnection Connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, Connection);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@PersonID", personId);
                cmd.Parameters.AddWithValue("@CreatedByUserID", createdById);
                cmd.Parameters.AddWithValue("@CreatedDate", createdTime);
                try
                {
                    Connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    isUpdated = rowsAffected > 0;
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}"); 
                    {
                        isUpdated = false;
                    }
                }
                return isUpdated;
            } }

        public static bool DeleteDriver(int id)
        {
            bool isDeleted = false;
            string query = "DELETE FROM Drivers WHERE DriverID = @Id";
            using (SqlConnection Connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, Connection);
                cmd.Parameters.AddWithValue("@Id", id);
                try
                {
                    Connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    isDeleted = rowsAffected > 0;
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}"); 
                    isDeleted = false;
                }
            }
            return isDeleted;
        }

        public static DataTable GetAllDrivers()
        {
            DataTable driversTable = new DataTable();
            string query = "SELECT * FROM Drivers";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        driversTable.Load(reader);

                    }
                    reader.Close();
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");


                }
            }
            return driversTable;
        }
    }
}

