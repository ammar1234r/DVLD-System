using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    static public class clsTestAppointmentsDataAcsses
    {
       

        static public bool FindTestAppointment(int Id, ref int ApplicationID, ref int TestTypeID, ref DateTime AppointmentDate,
            ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID, ref int PaidFees)
        {
            bool result = false;
            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @Id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", Id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        ApplicationID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]);
                        PaidFees = Convert.ToInt32(reader["PaidFees"]);
                        TestTypeID = Convert.ToInt32(reader["TestTypeID"]);
                        AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                        CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                        IsLocked = Convert.ToBoolean(reader["IsLocked"]);
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID == -1 ? (object)DBNull.Value : RetakeTestApplicationID);

                        result = true;
                    }
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    result = false;
                }
                
            }
            return result;
        }
        static public bool UpdateTestAppointment(int Id, int ApplicationID, int TestTypeID, DateTime AppointmentDate,
            int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID, int PaidFees)
        {
            bool result = false;
            string query = "UPDATE TestAppointments SET LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID, TestTypeID = @TestTypeID, AppointmentDate = @AppointmentDate, CreatedByUserID = @CreatedByUserID, IsLocked = @IsLocked, RetakeTestApplicationID = @RetakeTestApplicationID, PaidFees = @PaidFees WHERE TestAppointmentID = @Id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@IsLocked", IsLocked);
                command.Parameters.AddWithValue("@RetakeTestApplicationID",RetakeTestApplicationID <= 0 ? (object)DBNull.Value : RetakeTestApplicationID);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    result = rowsAffected > 0;
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    result = false;
                }
            }
            return result;
        }
      
        static public int AddNewAppoinment(int ApplicationID, int TestTypeID, DateTime AppointmentDate,
            int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID, int PaidFees)
        {
            int Id = -1;
            string query = "INSERT INTO TestAppointments (LocalDrivingLicenseApplicationID, TestTypeID, AppointmentDate, CreatedByUserID, IsLocked, RetakeTestApplicationID, PaidFees) " +
                           "OUTPUT INSERTED.TestAppointmentID " +
                           "VALUES (@LocalDrivingLicenseApplicationID, @TestTypeID, @AppointmentDate, @CreatedByUserID, @IsLocked, @RetakeTestApplicationID, @PaidFees)";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);
            if (RetakeTestApplicationID > 0)
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            }
            else
            {
                command.Parameters.Add("@RetakeTestApplicationID", SqlDbType.Int).Value = DBNull.Value;
            }
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    Id = Convert.ToInt32(result);
                }
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                Id = -1;
            }
            }
            return Id;
        }

        static public DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM TestAppointments";
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

        static public int GetAllTestAppointmentsCount(int id, int TestTypeId)
        {
            int count = 0;
            string query = "SELECT COUNT(*) FROM TestAppointments join LocalDrivingLicenseApplications on TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID where LocalDrivingLicenseApplications.ApplicationID=@id and TestAppointments.TestTypeID=@TypeId and RetakeTestApplicationID is null \r\n";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@TypeId", TestTypeId);
            try
            {
                connection.Open();
                count = (int)command.ExecuteScalar();
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                count = -1;
            }
            
            }
            return count;
        }

        static public DataTable GetAllAppointmentBYType(int LocalId, int typeId)
        {
            DataTable dataTable = new DataTable();
          
            string query = "select TestAppointmentID,AppointmentDate,PaidFees,IsLocked from TestAppointments where TestAppointments.LocalDrivingLicenseApplicationID=@Id and TestTypeID=@TypeId";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
            comm.Parameters.AddWithValue("@Id", LocalId);
            comm.Parameters.AddWithValue("@TypeId", typeId);
            try
            {
                connection.Open();
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    dataTable.Load(reader);
                }

            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                dataTable = null;
            }
            

            }
            return dataTable;
        }

        public static bool IsLocalIdHaveAppoinment(int LocalID,int TestTypeId)
        {
            bool result = false;
            string query = "select 1 from TestAppointments join Tests on TestAppointments.TestAppointmentID=Tests.TestAppointmentID where Tests.TestResult=1 AND  LocalDrivingLicenseApplicationID=@Id and  TestAppointments.TestTypeID=@TypeId and AppointmentDate<GETDATE();";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
            comm.Parameters.AddWithValue("@Id", LocalID);
            comm.Parameters.AddWithValue("@TypeId", TestTypeId);
            try
            {
                connection.Open();
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    result = true;
                }
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                result = false;
            }
            }
            return result;
        }

        public static bool IsAppoinmetExpired(int AppoinmetId)
        {
            bool result = false;
            string query = "select 1 from TestAppointments where TestAppointmentID=@Id and AppointmentDate>GETDATE();";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
                comm.Parameters.AddWithValue("@Id", AppoinmetId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        result = true;
                    }
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    result = false;
                }
            }
            return result;
        }
        public static bool IsApplicantPassedInTest(int LocalID, int TestTypeId)
        {
            bool result = false;
            string query = "select 1 from TestAppointments join Tests on TestAppointments.TestAppointmentID=Tests.TestAppointmentID where TestAppointments.LocalDrivingLicenseApplicationID=@Id and  TestAppointments.TestTypeID=@TypeId and Tests.TestResult=1;";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
                comm.Parameters.AddWithValue("@Id", LocalID);
                comm.Parameters.AddWithValue("@TypeId", TestTypeId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        result = true;
                    }
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    result = false;
                }
            }
            return result;
        }
       
    }
}