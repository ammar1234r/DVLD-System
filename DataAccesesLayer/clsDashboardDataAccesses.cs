using DataAccesesLayer;
using System;

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class clsDashboardDataAccesses
{
    public static (int persons, int drivers, int licenses, int detained) GetDashboardStats()
    {
        using (SqlConnection conn = new SqlConnection(clsDatabaseLocation.connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_GetDashboardStats", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return (
                    Convert.ToInt32(reader["TotalPersons"]),
                    Convert.ToInt32(reader["TotalDrivers"]),
                    Convert.ToInt32(reader["ActiveLicenses"]),
                    Convert.ToInt32(reader["DetainedLicenses"])
                );
            }
            return (0, 0, 0, 0);
        }
    }
}