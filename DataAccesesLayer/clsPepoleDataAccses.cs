using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    static public class clsPepoleDataAccses
    {


        static public bool FindPepole(int Id, ref string NationalNO, ref string FirstName, ref string SecondName, ref string LastName,
            ref Byte Gender, ref string Email, ref string PhoneNum,
            ref string Addres, ref int CountryKey, ref string ImagePath, ref DateTime BirthofDate)
        {
            bool result = false;
            string query = "SELECT * FROM People WHERE PersonID = @Id";
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
                        result = true;
                        NationalNO = reader["NationalNo"].ToString();
                        FirstName = reader["FirstName"].ToString();
                        SecondName = reader["SecondName"].ToString();
                        LastName = reader["LastName"].ToString();
                        Email = reader["Email"].ToString();
                        PhoneNum = reader["Phone"].ToString();
                        Addres = reader["Address"].ToString();
                        CountryKey = Convert.ToInt32(reader["NationalityCountryID"]);
                        ImagePath = reader["ImagePath"].ToString();
                        BirthofDate = Convert.ToDateTime(reader["DateOfBirth"]);
                        Gender = Convert.ToByte(reader["Gendor"]);

                    }

                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    result = false;

                }

            }
            return result;
        }
        static public bool FindByFirstAndLastName(ref int Id, string FirstNameAndLastName, ref string FirstName, ref string SecondName,  ref string LastName, ref string NationalNO,
            ref Byte Gender, ref string Email, ref string PhoneNum, ref string Addres,ref int CountryKey, ref string ImagePath, ref DateTime BirthofDate)
        {
            bool result = false;
            string query = "SELECT * FROM People WHERE FirstName+' '+LastName = @FirstNameAndLastName";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstNameAndLastName", FirstNameAndLastName);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            result = true;
                            Id = Convert.ToInt32(reader["PersonID"]);
                            FirstName = reader["FirstName"].ToString();
                            LastName = reader["LastName"].ToString();
                            SecondName = reader["SecondName"].ToString();
                            NationalNO = reader["NationalNo"].ToString();
                            Email = reader["Email"].ToString();
                            PhoneNum = reader["Phone"].ToString();
                            Addres = reader["Address"].ToString();
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
                    static public bool FindByNationalId(ref int Id, string NationalNO, ref string FirstName,ref string SecondName, ref string LastName,
            ref Byte Gender, ref string Email, ref string PhoneNum,
            ref string Addres, ref int CountryKey, ref string ImagePath, ref DateTime BirthofDate)
        {
            bool result = false;
            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNO);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            result = true;
                            Id = Convert.ToInt32(reader["PersonID"]);
                            FirstName = reader["FirstName"].ToString();
                            SecondName = reader["SecondName"].ToString();
                            LastName = reader["LastName"].ToString();
                            Email = reader["Email"].ToString();
                            PhoneNum = reader["Phone"].ToString();
                            Addres = reader["Address"].ToString();
                            CountryKey = Convert.ToInt32(reader["NationalityCountryID"]);
                            ImagePath = reader["ImagePath"].ToString();
                            BirthofDate = Convert.ToDateTime(reader["DateOfBirth"]);
                            Gender = Convert.ToByte(reader["Gendor"]);

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
        static public bool UpdatePepole(int Id, string NationalNO, string FirstName, string SecondName, string LastName,
          Byte Gender, string Email, string PhoneNum,
          string Addres, int CountryKey, string ImagePath, DateTime BirthofDate)
        {
            bool result = false;
            string query = "UPDATE People SET NationalNo = @NationalNo, FirstName = @FirstName, SecondName = @SecondName, LastName = @LastName, " +
                           "Gendor = @Gendor, Email = @Email, Phone = @Phone, Address = @Address, " +
                           "NationalityCountryID = @NationalityCountryID, ImagePath = @ImagePath, DateOfBirth = @BirthOfDate WHERE PersonID = @Id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@NationalNo", NationalNO);
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@SecondName", SecondName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("Gendor", Gender);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@Phone", PhoneNum);
                command.Parameters.AddWithValue("@Address", Addres);
                command.Parameters.AddWithValue("@NationalityCountryID", CountryKey);
                command.Parameters.AddWithValue("@ImagePath", (object)ImagePath ?? DBNull.Value);
                command.Parameters.AddWithValue("@BirthOfDate", BirthofDate);
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

        static public int AddPepole(string NationalNO, string FirstName, string SecondName, string LastName,
             Byte Gender, string Email, string PhoneNum,
             string Addres, int CountryKey, string ImagePath, DateTime BirthofDate)
        {
            int PeopleId = -1;
            string query = "INSERT INTO People (NationalNo, FirstName, SecondName, LastName, Gendor, Email, Phone, Address, NationalityCountryID, ImagePath, DateOfBirth) " +
                           "VALUES (@NationalNo, @FirstName, @SecondName, @LastName, @Gendor, @Email, @Phone, @Address, @NationalityCountryID, @ImagePath, @DateOfBirth); SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString)) {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNO);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@Gendor", Gender);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Phone", PhoneNum);    
                    command.Parameters.AddWithValue("@Address", Addres);
                    command.Parameters.AddWithValue("@NationalityCountryID", CountryKey);
                    command.Parameters.AddWithValue("@ImagePath", (object)ImagePath ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", BirthofDate);


                    connection.Open();
                    var id = command.ExecuteScalar();
                    if (id != null && int.TryParse(id.ToString(), out int insertedID))
                    {
                        PeopleId = insertedID;
                    }
                }
            }
            return PeopleId;
        }

        static public bool DeletePepole(int Id)
        {
            bool result = false;
            string query = "DELETE FROM People WHERE PersonID = @Id";
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
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    result = false;
                }
            }
            return result;
        }
        static public bool FindPeopleByDriverId(int DriverId, ref int Id, ref string NationalNO, ref string FirstName, ref string SecondName, ref string LastName,
            ref Byte Gender, ref string Email, ref string PhoneNum,
            ref string Addres, ref int CountryKey, ref string ImagePath, ref DateTime BirthofDate)
        {
            bool result = false;
            string query = "select P.* from People P join Drivers D on P.PersonID=D.PersonID where D.DriverID =@DriverId";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@DriverId", DriverId);
                        try
                        {
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                result = true;
                        Id= reader.GetInt32(0);
                                NationalNO = reader["NationalNo"].ToString();
                                FirstName = reader["FirstName"].ToString();
                        SecondName = reader["SecondName"].ToString();
                        LastName = reader["LastName"].ToString();
                                Email = reader["Email"].ToString();
                                PhoneNum = reader["Phone"].ToString();
                                Addres = reader["Address"].ToString();
                                CountryKey = Convert.ToInt32(reader["NationalityCountryID"]);
                                ImagePath = reader["ImagePath"].ToString();
                                BirthofDate = Convert.ToDateTime(reader["DateOfBirth"]);
                                Gender = Convert.ToByte(reader["Gendor"]);

                            }


                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                            result = false;

                        } }
            return result;

        }
        static public bool FindPeopleIDByLicenseId(int LicenseId, ref int Id)
        {
            bool result = false;
            string query = "select P.PersonID from People P join Drivers D on P.PersonID=D.PersonID \r\njoin Licenses L on L.DriverID=D.DriverID where L.LicenseID=@LicenseId";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@LicenseId", LicenseId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        result = true;
                        Id = reader.GetInt32(0);
                        

                    }


                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");

                    result = false;

                }
            }
            return result;

        }


        static public bool IsExist(int Id)
        {
            bool result = false;
            string query = "select 1 from People where People.PersonID=@id";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", Id);
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

        static public DataTable DataTable()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM People";
            using (SqlConnection connection = new SqlConnection(clsDatabaseLocation.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reder = command.ExecuteReader();
                dt.Load(reder);
            }
         catch (Exception ex){System.Diagnostics.Debug.WriteLine($"[DAL Error] {ex.Message}");
            
                dt = null;
            }
           
            }
            return dt;
        }
    }
}
