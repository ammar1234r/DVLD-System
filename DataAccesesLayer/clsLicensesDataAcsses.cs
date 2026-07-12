using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccesesLayer
{
    public static class clsLicensesDataAcsses
    {


        static public bool FindLicnse(int id, ref int ApplicationID, ref int DriverID, ref int LicnseClassId, ref DateTime IssueDate, ref DateTime ExpirtionDate, ref string Nots
            , ref int PaidFees, ref bool isActive, ref Byte IssueReason, ref int CreatedByUserID)
        {
            bool result = false;
            string query = "SELECT * FROM Licenses WHERE LicenseID = @Id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                        DriverID = Convert.ToInt32(reader["DriverID"]);
                        LicnseClassId = Convert.ToInt32(reader["LicenseClass"]);
                        IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                        ExpirtionDate = Convert.ToDateTime(reader["ExpirationDate"]);
                        if (reader["Notes"] == DBNull.Value)
                            Nots = string.Empty;
                        else
                            Nots = reader["Notes"].ToString();
                        PaidFees = Convert.ToInt32(reader["PaidFees"]);
                        isActive = Convert.ToBoolean(reader["isActive"]);
                        IssueReason = Convert.ToByte(reader["IssueReason"]);
                        CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                        result = true;

                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    result = false;
                }
            }
            return result;
        }

        static public bool FindLicenseByNationalId(string NationalNo, ref int id, ref int ApplicationID, ref int DriverID, ref int LicnseClassId, ref DateTime IssueDate, ref DateTime ExpirtionDate, ref string Nots
            , ref int PaidFees, ref bool isActive, ref Byte IssueReason, ref int CreatedByUserID)
        {
            bool result = false;
        
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_FindLicenseByNationalNo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["LicenseID"]);
                            ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                            DriverID = Convert.ToInt32(reader["DriverID"]);
                            LicnseClassId = Convert.ToInt32(reader["LicenseClass"]);
                            IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                            ExpirtionDate = Convert.ToDateTime(reader["ExpirationDate"]);
                            if (reader["Notes"] == DBNull.Value)
                                Nots = string.Empty;
                            else
                                Nots = reader["Notes"].ToString();
                            PaidFees = Convert.ToInt32(reader["PaidFees"]);
                            isActive = Convert.ToBoolean(reader["isActive"]);
                            IssueReason = Convert.ToByte(reader["IssueReason"]);
                            CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                        result = false;
                    }
                }
               
            }
            return result;
        }
      
     
        static public bool UpdateLicense(int id, int ApplicationID, int DriverID, int LicnseClassId, DateTime IssueDate, DateTime ExpirtionDate, string Nots
                , int PaidFees, bool isActive, Byte IssueReason, int CreatedByUserID)
            {
                bool result = false;

                string query = "UPDATE Licenses SET ApplicationID = @ApplicationID, DriverID = @DriverID, LicenseClass = @LicenseClass, IssueDate = @IssueDate, ExpirationDate = @ExpirationDate, Notes = @Notes, PaidFees = @PaidFees, isActive = @isActive, IssueReason = @IssueReason, CreatedByUserID = @CreatedByUserID WHERE LicenseID = @Id";

                using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", LicnseClassId);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirtionDate);
                    command.Parameters.AddWithValue("@Notes", Nots);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@isActive", isActive);
                    command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        result = rowsAffected > 0;
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                        result = false;
                    }
                }
                return result;
            }

            static public int AddNewLicnese(int ApplicationID, int DriverID, int LicnseClassId, DateTime IssueDate, DateTime ExpirtionDate, string Nots
                , int PaidFees, bool isActive, Byte IssueReason, int CreatedByUserID)
            {
                int Id = -1;
                string query = "INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, isActive, IssueReason, CreatedByUserID) " +
                               "VALUES (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @isActive, @IssueReason, @CreatedByUserID); " +
                               "SELECT SCOPE_IDENTITY();";
                using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", LicnseClassId);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirtionDate);
                    command.Parameters.AddWithValue("@Notes", Nots ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@isActive", isActive);
                    command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    try
                    {
                        connection.Open();
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            Id = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                        Id = -1;
                    }
                }
                return Id;


            }
        static public int GetActiveLicensesCount()
        {
            int count = 0;
            string query = "SELECT COUNT(*) FROM Licenses WHERE isActive = 1";
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
                    count = 0;
                }
            }
            return count;
        }

            static public bool DeleteLicense(int id)
            {
                bool result = false;

                using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
                {
                    string query = "DELETE FROM Licenses WHERE LicenseID = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        result = rowsAffected > 0;
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                        result = false;
                    }
                }
                return result;
            }
   

        static public bool ActiveLicense(int id)
            {
                bool result = false;

                string query = "UPDATE Licenses SET isActive = 1 WHERE LicenseID = @Id";
                using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        result = rowsAffected > 0?true:false;
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                        result = false;
                    }
                }
                return result;
            }
            static public bool DeActiveLicense(int id)
            {
                bool result = false;

                string query = "UPDATE Licenses SET isActive = 0 WHERE LicenseID = @Id";
                using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        result = rowsAffected > 0;
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                        result = false;
                    }
                }
                return result;
            }

            static public DataTable AllLicnses()
            {
                DataTable dt = new DataTable();

                string query = "SELECT * FROM Licenses";
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
                        reader.Close();

                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                        dt = null;
                    }
                }
                return dt;
            }

            static public bool IsPersonhaveLicnse(int PeopleId, int ClassId)
            {
                bool result = false;
                string query = "select 1 from Licenses join Drivers on Licenses.DriverID =Drivers.DriverID where Drivers.PersonID=@iD and  Licenses.LicenseClass=@ClassId";
                using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@iD", PeopleId);
                    command.Parameters.AddWithValue("@ClassId", ClassId);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            result = true;
                        }
                        reader.Close();
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                        result = false;
                    }
                }
                return result;
            }


        
        public static bool IsDriverHaveLicnse(int ApplicationID, int LicenseTypeId)
        {
            bool result = false;
            string query = "select 1 from Licenses where Licenses.ApplicationID=@Id and  Licenses.LicenseClass=@LicenseTypeId";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", ApplicationID);
                command.Parameters.AddWithValue("@LicenseTypeId", LicenseTypeId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        result = true;
                    }
                    reader.Close();
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    result = false;
                }

            }
            return result;
        }
        static public bool FindLicenseByLocalId(int AppId, ref int ID, ref int DriverID, ref int LicnseClassId, ref DateTime IssueDate, ref DateTime ExpirtionDate, ref string Nots
            , ref int PaidFees, ref bool isActive, ref Byte IssueReason, ref int CreatedByUserID)
        {
            bool result = false;
            string Query = "select * from Licenses where ApplicationID=@ID";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(Query, connection);
                comm.Parameters.AddWithValue("@ID", AppId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        ID = Convert.ToInt32(reader["LicenseID"]);
                        DriverID = Convert.ToInt32(reader["DriverID"]);
                        LicnseClassId = Convert.ToInt32(reader["LicenseClass"]);
                        IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                        ExpirtionDate = Convert.ToDateTime(reader["ExpirationDate"]);
                        if (reader["Notes"] == DBNull.Value)
                            Nots = string.Empty;
                        else
                            Nots = reader["Notes"].ToString();
                        PaidFees = Convert.ToInt32(reader["PaidFees"]);
                        isActive = Convert.ToBoolean(reader["isActive"]);
                        IssueReason = Convert.ToByte(reader["IssueReason"]);
                        CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                        result = true;

                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    result = false;
                }
            }
            return result;

        }
    } 
}
