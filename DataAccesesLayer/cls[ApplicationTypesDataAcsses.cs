using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    public static class cls_ApplicationTypesDataAcsses
    {
        
        static public bool FindApplicationType(int Id, ref string ApplicationTypeName, ref int ApplicationFees)
        {
            bool Isfound = false;
            string query = "select * from ApplicationTypes where ApplicationTypeID=@id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                connection.Open();
                SqlCommand comm = new SqlCommand(query, connection);
                comm.Parameters.AddWithValue("@id", Id);
                try
                {

                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Isfound = true;
                            ApplicationTypeName = reader.GetString(1);
                            ApplicationFees = reader.GetInt32(2);
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

        static public int AddNewApplicationType(string ApplicationTypeName, int ApplicationFees)
        {
            int newId = -1;
            string query = "insert into ApplicationTypes (ApplicationTypeTitle, ApplicationFees) output INSERTED.ApplicationTypeID values (@ApplicationTypeName, @ApplicationFees)";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand comm = new SqlCommand(query, connection);
                comm.Parameters.AddWithValue("@ApplicationTypeName", ApplicationTypeName);
                comm.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
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

        static public bool UpdateApplicationType(int Id, string ApplicationTypeName, int ApplicationFees)
        {
            bool result = false;
            string query = "UPDATE ApplicationTypes SET ApplicationTypeName = @ApplicationTypeName, ApplicationFees = @ApplicationFees WHERE ApplicationTypeID = @Id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@ApplicationTypeName", ApplicationTypeName);
                command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    result = rowsAffected > 0;
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                            System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    result = false;
                }
            }
            return result;
        }

        static public bool DeleteApplicationType(int Id)
        {
            bool result = false;
            string query = "DELETE FROM ApplicationTypes WHERE ApplicationTypeID = @Id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", Id);
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
        public static DataTable GetAllAppTypes()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM ApplicationTypes";
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
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    dt = null;
                }
            }
            return dt;
        }
        
    }
}
