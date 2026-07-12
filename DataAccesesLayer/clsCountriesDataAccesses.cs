using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    public static class clsCountriesDataAccesses
    {
      
        static public bool FindCountry(int countryId, ref string CountryName)
        {
            bool isFoubd = false;
            string query = "SELECT * FROM Countries WHERE CountryID=@id";
            using (SqlConnection Connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, Connection);
                command.Parameters.AddWithValue("@id", countryId);
                try
                {

                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        isFoubd = true;
                        CountryName = reader["CountryName"].ToString();
                    }
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    isFoubd = false;
                    CountryName = string.Empty;
                }
            }
            return isFoubd;
        }

        static public bool FindCountryByName(string countryName, ref int countryId)
        {
            bool isFoubd = false;
            string query = "SELECT * FROM Countries WHERE CountryName=@name";
            using (SqlConnection Connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, Connection);
                command.Parameters.AddWithValue("@name", countryName);
                try
                {
                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        isFoubd = true;
                        countryId = Convert.ToInt32(reader["CountryID"]);
                    }
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    isFoubd = false;
                    countryId = 0;
                }
            }
            return isFoubd;
        }
        public static Dictionary<int,string> GetAllCountrys() 
        {
           Dictionary<int, string> countries = new Dictionary<int, string>();
            string query = "SELECT * FROM Countries";
            using (SqlConnection Connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, Connection);
                try
                {
                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int countryId = Convert.ToInt32(reader["CountryID"]);
                        string countryName = reader["CountryName"].ToString();
                        countries.Add(countryId, countryName);
                    }
                }
             catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
                
                    countries.Clear();
                }
            }
            return countries;
        }
    }

   
}
