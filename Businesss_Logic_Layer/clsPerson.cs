using DataAccesesLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesss_Logic_Layer
{
    public class clsPerson
    {
        enum AppType
        {
            enAdd = 0,
            enUpdate = 1
        }
        AppType appType;
        public int Id { get; private set; }
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string NationalId { get; set; }

        public char Gender { get; set; }
        public string Address { get; set; }

        public int CountryKey { get; set; }

        public string ImagePath { get; set; }

        public DateTime BirthOfDate { get; set; }

        public clsPerson(string firstName, string secondName, string lastName, string email, string phone, string nationalId,
            string ImagePath, char Gender, string address, int countryKey, DateTime birthOfDate)
        {
            Id = -1;
            FirstName = firstName;
            if(string.IsNullOrWhiteSpace(secondName))
            {
                SecondName = lastName;
            }
            else
            {
                SecondName = secondName;
            }
            LastName = lastName;
            Email = email;
            Phone = phone;
            NationalId = nationalId;
            this.ImagePath = ImagePath;
            this.Gender = Gender;
            Address = address;
            CountryKey = countryKey;
            BirthOfDate = birthOfDate;
            appType = AppType.enAdd;

        }

        clsPerson(int id, string firstName, string secondName, string lastName, string email, string phone, string nationalId,
            string ImagePath, char Gender, string address, int countryKey, DateTime birthOfDate)
        {
            Id = id;
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            NationalId = nationalId;
            this.ImagePath = ImagePath;
            this.Gender = Gender;
            Address = address;
            CountryKey = countryKey;
            BirthOfDate = birthOfDate;
            appType = AppType.enUpdate;
        }
        public clsPerson()
        {
            Id = -1;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            NationalId = string.Empty;
            ImagePath = string.Empty;
            Gender = ' ';

        }
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                return false;

            if (string.IsNullOrWhiteSpace(LastName))
                return false;

            if (string.IsNullOrWhiteSpace(NationalId))
                return false;

            if (BirthOfDate >= DateTime.Now)
                return false;

            if (string.IsNullOrWhiteSpace(Phone))
                return false;

            if (CountryKey <= 0)
                return false;

            return true;
        }
        public static clsPerson FindPeopleByFirstAndLastName(string FirstNameAndLastName)
        {
            int countryKey = 0; int ID = 0;string email = string.Empty, FirstName = string.Empty, SecondName = string.Empty, LastName = string.Empty,
                phone = string.Empty, nationalId = string.Empty, imagePath = string.Empty, address = string.Empty; DateTime birthOfDate = DateTime.MinValue;
            char Gender = 'F'; Byte GenderByte = 0;
            if (clsPepoleDataAccses.FindByFirstAndLastName(ref ID, FirstNameAndLastName,ref FirstName, ref SecondName,  ref LastName, ref nationalId, ref GenderByte, ref email, ref phone,
                ref address, ref countryKey, ref imagePath, ref birthOfDate))
            {
                if (GenderByte == 0)
                {
                    Gender = 'M';
                }
                return new clsPerson(ID, FirstName, SecondName, LastName, email, phone, nationalId, imagePath, Gender, address, countryKey, birthOfDate);
            }
            else
            {
                return null;
            }
        }
        public static clsPerson FindPeopleById(int ID)
        {
            int countryKey = 0; string firstName = string.Empty, secondName = string.Empty, lastName = string.Empty, email = string.Empty,
                phone = string.Empty, nationalId = string.Empty, imagePath = string.Empty, address = string.Empty; DateTime birthOfDate = DateTime.MinValue;
            char Gender = 'F'; Byte GenderByte = 0;
            if (clsPepoleDataAccses.FindPepole(ID, ref nationalId, ref firstName, ref secondName, ref lastName, ref GenderByte, ref email, ref phone,
                ref address, ref countryKey, ref imagePath, ref birthOfDate))
            {
                if (GenderByte == 0)
                {
                    Gender = 'M';
                }
                return new clsPerson(ID, firstName, secondName, lastName, email, phone, nationalId, imagePath, Gender, address, countryKey, birthOfDate);
            }
            else
            {
                return null;
            }
        }
        public static int FindPeopleIDByLicenseId(int LicenseId)
        {
            int id = 0;
            if (clsPepoleDataAccses.FindPeopleIDByLicenseId(LicenseId, ref id))
            {
                return id;
            }
            return -1;
        }
        public static clsPerson FindPeopleByDriverId(int DriverID)
        {
            int id=0,countryKey = 0; string firstName = string.Empty, secondName = string.Empty, lastName = string.Empty, email = string.Empty,
                phone = string.Empty, nationalId = string.Empty, imagePath = string.Empty, address = string.Empty; DateTime birthOfDate = DateTime.MinValue;
            char Gender = 'F'; Byte GenderByte = 0;
            if (clsPepoleDataAccses.FindPeopleByDriverId(DriverID, ref id, ref nationalId, ref firstName, ref secondName, ref lastName, ref GenderByte, ref email, ref phone,
                ref address, ref countryKey, ref imagePath, ref birthOfDate))
            {
                if (GenderByte == 0)
                {
                    Gender = 'M';
                }
                return new clsPerson(id, firstName, secondName, lastName, email, phone, nationalId, imagePath, Gender, address, countryKey, birthOfDate);
            }
            else
            {
                return null;
            }
        }

        public static clsPerson FindByNationalId(string NationalId)
        {
            int id = 0, countryKey = 0; string firstName = string.Empty, secondName = string.Empty, lastName = string.Empty, email = string.Empty,
                phone = string.Empty, imagePath = string.Empty, address = string.Empty; DateTime birthOfDate = DateTime.MinValue;
            char Gender = 'F'; Byte GenderByte = 0;
            if (clsPepoleDataAccses.FindByNationalId(ref id, NationalId, ref firstName, ref secondName, ref lastName, ref GenderByte, ref email, ref phone,
              ref address, ref countryKey, ref imagePath, ref birthOfDate))
            {
                if (GenderByte == 0)
                {
                    Gender = 'M';
                }
                return new clsPerson(id, firstName, secondName, lastName, email, phone, NationalId, imagePath, Gender, address, countryKey, birthOfDate);
            }
            else
            {
                return null;
            } }


            bool UpdatePerson()
        {
            Byte GenderByte;
            if (Gender == 'M')
            {
                GenderByte = 0;
            }
            else
                GenderByte = 1;
            if (clsPepoleDataAccses.UpdatePepole(Id, NationalId, FirstName, SecondName, LastName, GenderByte,
                Email, Phone, Address, CountryKey, ImagePath, BirthOfDate))
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        bool AddNewPerson()
        {
            Byte GenderByte;
            if (Gender == 'M')
            {
                GenderByte = 0;
            }
            else
                GenderByte = 1;
            int newId = clsPepoleDataAccses.AddPepole(NationalId, FirstName, SecondName, LastName, GenderByte, Email, Phone, Address, CountryKey, ImagePath, BirthOfDate);
            if (newId > 0)
            {
                Id = newId;
                return true;
            }
            else
            {
                return false;
            }
        }

        
        public bool SavePerson()
        {
            if (!Validate())
            {
                return false;
            }
            if (appType == AppType.enAdd)
            {

                if (AddNewPerson())
                {

                    appType = AppType.enUpdate;
                    return true;
                }
                else
                {
                    return false;
                }


            }

            else
            {
                return UpdatePerson();
            }
        }

        public static DataTable GetAllPeople()
        {
            return clsPepoleDataAccses.DataTable();
        }
        public static bool DeletePeople(int id)
        {
            return clsPepoleDataAccses.DeletePepole(id);
        }
    }
}