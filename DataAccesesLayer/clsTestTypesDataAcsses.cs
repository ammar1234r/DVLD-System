using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    public static class clsTestTypesDataAcsses
    {
       

        public static bool FindTestType(int id, ref string testTypeName, ref string description,ref int TestTypeFees)
        {
            bool result = false;
            string query = "SELECT * FROM TestTypes WHERE TestTypeID = @Id"; using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    testTypeName = reader["TestTypeTitle"].ToString();
                    description = reader["TestTypeDescription"].ToString();
                    TestTypeFees = Convert.ToInt32(reader["TestTypeFees"]);
                    result = true;
                }
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                result = false;
            }
            }
            return result;
        }

        public static bool UpdateTestType(int id, string testTypeName, string description, int TestTypeFees)
        {
            bool result = false;
            string query = "UPDATE TestTypes SET TestTypeTitle = @TestTypeTitle, TestTypeDescription = @TestTypeDescription, TestTypeFees = @TestTypeFees WHERE TestTypeID = @Id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@TestTypeTitle", testTypeName);
            command.Parameters.AddWithValue("@TestTypeDescription", description);
            command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);
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

        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM TestTypes";

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
                
                    // Handle exception (log it, rethrow it, etc.)
                }
            }
            return dt;
        }

        public  static int FindTestTypeByName(string name)
        {
            int id = 0;
            string query = "SELECT TestTypeID FROM TestTypes WHERE TestTypeTitle = @TestTypeTitle";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TestTypeTitle", name);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        id = Convert.ToInt32(reader["TestTypeID"]);

                    }
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    id = 0;
                }
            }
            return id;
        }
    }
}
