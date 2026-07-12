using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    public static class clsUsersDataAccesses
    {
        public static bool FindUser(string username, string password,
           ref int userId, ref int personId, ref bool isActive,ref byte RoleId)
        {
            
        bool result = false;
            string   query = @"SELECT UserID, PersonID, IsActive, RoleID FROM Users  WHERE UserName = @UserName  AND Password = @Password";
            using(SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    conn.Open();
                    try
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                userId = reader.GetInt32(0);
                                personId = reader.GetInt32(1);
                                isActive = reader.GetBoolean(2);
                                RoleId = (byte)reader.GetInt32(3);
                                result = true;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}"); result = false;
                    }
            }
        return result;
        }
        

        }

        public static bool UpdateUser(int userId, string username, string password, bool isActive, byte roleId)
        {
            bool result = false;
            string query = @"UPDATE Users SET UserName = @UserName, Password = @Password, IsActive = @IsActive, RoleID = @RoleID     WHERE UserID = @UserID";
            using (SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@UserName", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);
                    cmd.Parameters.AddWithValue("@RoleID", roleId);
                    conn.Open();
                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        result = rowsAffected > 0;
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
        public static int AddUser(string username, string password, bool isActive, byte roleId)
        {
            int newUserId = -1;
            string query = @"INSERT INTO Users (UserName, Password, IsActive, RoleID) OUTPUT INSERTED.UserID VALUES (@UserName, @Password, @IsActive, @RoleID)";
            using (SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);
                    cmd.Parameters.AddWithValue("@RoleID", roleId);
                    conn.Open();
                    try
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedId))
                        {
                            newUserId = insertedId;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                        newUserId = -1;
                    }
                }
            }
            return newUserId;
        }

        public static bool DeleteUser(int userId)
        {
            bool result = false;
            string query = @"DELETE FROM Users WHERE UserID = @UserID";
            using (SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    conn.Open();
                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        result = rowsAffected > 0;
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
}}
