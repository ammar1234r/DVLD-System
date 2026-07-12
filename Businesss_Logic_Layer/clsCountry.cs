using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccesesLayer;
namespace Businesss_Logic_Layer
{
    public class clsCountry
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public clsCountry()
        {
        }
        public clsCountry(int countryId, string countryName, string countryCode)
        {
            CountryId = countryId;
            CountryName = countryName;
            CountryCode = countryCode;
        }
        public static Dictionary<int,string> Countrys()
        {
            Dictionary<int, string> countries = new Dictionary<int, string>();
            countries= clsCountriesDataAccesses.GetAllCountrys();
            return countries;
        }
        public static  string GetCountryName(int countryId)
        {
            string countryName = string.Empty;
            bool isFound = clsCountriesDataAccesses.FindCountry(countryId, ref countryName);
            if (isFound)
            {
                return countryName;
            }
            else
            {
                return "Unknown Country";
            }
        }

        public static int GetCountryKeyByName(string countryName)
        {
            int countryId = 0;
            bool isFound = clsCountriesDataAccesses.FindCountryByName(countryName, ref countryId);
            if (isFound)
            {
                return countryId;
            }
            else
            {
                return 0;
            }
        }
    }
}
