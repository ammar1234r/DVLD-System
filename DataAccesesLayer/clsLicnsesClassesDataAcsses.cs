using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    public static class clsLicnsesClassesDataAcsses
    {
        
        static public bool FindLicenseClass(int id, ref string className, ref string ClassDescription, ref short MinmumAge, ref short DefaultValidityLength
            , ref int ClassFees)
        {
            bool result = false;
            string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @Id";
            using (SqlConnection Connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, Connection);
                command.Parameters.AddWithValue("@Id", id);
                try
                {
                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        className = reader["ClassName"].ToString();
                        ClassDescription = reader["ClassDescription"].ToString();
                        MinmumAge = Convert.ToInt16(reader["MinimumAllowedAge"]);
                        DefaultValidityLength = Convert.ToInt16(reader["DefaultValidityLength"]);
                        ClassFees = Convert.ToInt32(reader["ClassFees"]);

                        result = true;
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    result = false;
                }
            }
            return result;
        }
        public static int ReturnDefaultValidityLength(int LicenseClassID)
        {
            int result = -1;
            string query = "SELECT DefaultValidityLength FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                try
                {
                    connection.Open();
                    var res = command.ExecuteScalar();
                    if (res != null)
                        result = Convert.ToInt32(res);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                }
            }
            return result;
        }
        static public bool UpdateLicenseClass(int id, string className, string ClassDescription, short MinmumAge, short DefaultValidityLength
            , int ClassFees)
        {
            bool result = false;
            string query = "UPDATE LicenseClasses SET ClassName = @ClassName, ClassDescription = @ClassDescription, MinimumAllowedAge = @MinmumAge, DefaultValidityLength = @DefaultValidityLength, ClassFees = @ClassFees WHERE LicenseClassID = @Id";
            using (SqlConnection Connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, Connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@ClassName", className);
                command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                command.Parameters.AddWithValue("@MinmumAge", MinmumAge);
                command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                command.Parameters.AddWithValue("@ClassFees", ClassFees);
                try
                {
                    Connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    result = rowsAffected > 0;
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    result = false;
                }
            }
            return result;
        }

        static public int AddLicenseClass(string className, string ClassDescription, short MinmumAge, short DefaultValidityLength
            , int ClassFees)
        {
            int LicenseClassID = -1;
            string query = "INSERT INTO LicenseClasses (ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees) OUTPUT INSERTED.LicenseClassID VALUES (@ClassName, @ClassDescription, @MinmumAge, @DefaultValidityLength, @ClassFees)";
            using (SqlConnection Connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, Connection);
                command.Parameters.AddWithValue("@ClassName", className);
                command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                command.Parameters.AddWithValue("@MinmumAge", MinmumAge);
                command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                command.Parameters.AddWithValue("@ClassFees", ClassFees);
                try
                {
                    Connection.Open();
                    var Id = command.ExecuteScalar();
                    if (Id != null && int.TryParse(Id.ToString(), out int insertedID))
                    {
                        LicenseClassID = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                }
            }
                return LicenseClassID;
            
        }

            static public DataTable GetAllLicnsesClass()
            {
                DataTable dt = new DataTable();
                string query = "SELECT * FROM LicenseClasses";
                using (SqlConnection Connection = new SqlConnection(clsDatabaseLocation.connectionString))
                {
                    SqlCommand command = new SqlCommand(query, Connection);
                    try
                    {
                        Connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                        dt = new DataTable();
                    }
                }
                return dt;
            }

            static public int FindLicenseClassIdByName(string className)
            {
                int classId = -1;
                string query = "SELECT LicenseClassID FROM LicenseClasses WHERE ClassName = @ClassName";
                using (SqlConnection Connection = new SqlConnection(clsDatabaseLocation.connectionString))
                {
                    SqlCommand command = new SqlCommand(query, Connection);
                    command.Parameters.AddWithValue("@ClassName", className);
                    try
                    {
                        Connection.Open();
                        var result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            classId = id;
                        }
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                        classId = -1;
                    }
                }
                return classId;
            }
        }

    } 
