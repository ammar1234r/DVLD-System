using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    static public class clsTestsDataAccesses
    {
      
        static public bool FindTest(int Id, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool Isfound = false;
            string query = "select* from Tests where TestID=@id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
            comm.Parameters.AddWithValue("@id", Id);
            try
            {
                connection.Open();
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Isfound = true;
                        TestAppointmentID = reader.GetInt32(1);
                        TestResult = reader.GetBoolean(2);
                        Notes = reader.GetString(3);
                        CreatedByUserID = reader.GetInt32(4);
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

        static public int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int newId = 0;
            string query = "insert into Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID) output INSERTED.TestID values (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID)";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
            comm.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            comm.Parameters.AddWithValue("@TestResult", TestResult);
            comm.Parameters.AddWithValue("@Notes", Notes);
            comm.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                connection.Open();
                var Id = comm.ExecuteScalar();
                if(Id != null && int.TryParse(Id.ToString(), out int insertedID))
                {
                    newId = insertedID;
                }
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                newId = 0;
            }
            
            }
            return newId;
        }

        static public bool UpdateTest(int Id, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            bool IsUpdated = false;
           
            string query = "update Tests set TestAppointmentID=@TestAppointmentID, TestResult=@TestResult, Notes=@Notes, CreatedByUserID=@CreatedByUserID where TestID=@Id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
            comm.Parameters.AddWithValue("@Id", Id);
            comm.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            comm.Parameters.AddWithValue("@TestResult", TestResult);
            comm.Parameters.AddWithValue("@Notes", Notes);
            comm.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                connection.Open();
                int rowsAffected = comm.ExecuteNonQuery();
                IsUpdated = rowsAffected > 0;
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                IsUpdated = false;
            }
           
            }
            return IsUpdated;
        }

        static public bool DeleteTest(int Id)
        {
            bool IsDeleted = false;
            string query = "delete from Tests where TestID=@Id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
            comm.Parameters.AddWithValue("@Id", Id);
            try
            {
                connection.Open();
                int rowsAffected = comm.ExecuteNonQuery();
                IsDeleted = rowsAffected > 0;
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                IsDeleted = false;
            }
          
            }
            return IsDeleted;
        }

       static public bool DidHePasTheTest(int AppId,Byte TestType)
        {
            bool IsFound = false;
            string query = "select * from TestAppointments join Tests on TestAppointments.TestAppointmentID=Tests.TestAppointmentID where Tests.TestResult=1 and TestAppointments.TestTypeID=@TypeId and TestAppointments.LocalDrivingLicenseApplicationID=@AppId;";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
            comm.Parameters.AddWithValue("@AppId", AppId);
            comm.Parameters.AddWithValue("@TypeId", TestType);
            try
            {
                connection.Open();
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    IsFound = true;
                }
                reader.Close();
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                IsFound = false;
            }
          
            }return IsFound;
        }

        public static bool IsApplicantTakedTheTest(int AppId, Byte TestType) { 
        bool IsFound = false;
            string query = "select * from TestAppointments join Tests on TestAppointments.TestAppointmentID=Tests.TestAppointmentID where TestAppointments.TestTypeID=@TypeId and TestAppointments.LocalDrivingLicenseApplicationID=@AppId and Tests.TestResult=1;";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
            comm.Parameters.AddWithValue("@AppId", AppId);
            comm.Parameters.AddWithValue("@TypeId", TestType);
            try
            {
                connection.Open();
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    IsFound = true;
                }
                reader.Close();
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                IsFound = false;
            }
            }
            return IsFound;
        }
    }
}
