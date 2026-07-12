using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesss_Logic_Layer
{
    public class clsLicnsesClasses
    {
        enum Mode
        {
            AddNew,
            Edit,
            View
        }
        Mode currentMode;
        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }

        public string ClassDescription { get; set; }

        public short MinmumAge { get; set; }

        public short DefaultValidityLength { get; set; }

        public int ClassFees { get; set; }
        public clsLicnsesClasses()
        {
            LicenseClassID = -1;
            ClassName = "";
            ClassDescription = "";
            MinmumAge = 0;
            DefaultValidityLength = 0;
            ClassFees = 0;
            currentMode = Mode.View;
        }
        public clsLicnsesClasses(string className, string classDescription, short minmumAge, short defaultValidityLength, int classFees)
        {
            LicenseClassID = -1;
            ClassName = className;
            ClassDescription = classDescription;
            MinmumAge = minmumAge;
            DefaultValidityLength = defaultValidityLength;
            ClassFees = classFees;
            currentMode = Mode.AddNew;
        }
       public  clsLicnsesClasses(int licenseClassID, string className, string classDescription, short minmumAge, short defaultValidityLength, int classFees)
        {
            LicenseClassID = licenseClassID;
            ClassName = className;
            ClassDescription = classDescription;
            MinmumAge = minmumAge;
            DefaultValidityLength = defaultValidityLength;
            ClassFees = classFees;
            currentMode = Mode.Edit;
        }
       

        static public clsLicnsesClasses Find(int id)
        {
            string className = "", classDescription = "";
            short minmumAge = 0, defaultValidityLength = 0;
            int classFees = 0;
            if (DataAccesesLayer.clsLicnsesClassesDataAcsses.FindLicenseClass(id, ref className, ref classDescription, ref minmumAge, ref defaultValidityLength, ref classFees))
            {
                return new clsLicnsesClasses(id, className, classDescription, minmumAge, defaultValidityLength, classFees);
            }
            else
            {
                return null;
            }
        }

        bool AddNewLicenseClass()
        {
            int newId = DataAccesesLayer.clsLicnsesClassesDataAcsses.AddLicenseClass(ClassName, ClassDescription, MinmumAge, DefaultValidityLength, ClassFees);
            if (newId > 0)
            {
                LicenseClassID = newId;
                return true;
            }
            else
            {
                return false;
            }
        }
        bool UpdateLicenseClass()
        {
            return DataAccesesLayer.clsLicnsesClassesDataAcsses.UpdateLicenseClass(LicenseClassID, ClassName, ClassDescription, MinmumAge, DefaultValidityLength, ClassFees);
        }
        public bool Save()
        {
            switch (currentMode)
            {
                case Mode.AddNew:
                    if (AddNewLicenseClass())
                    {
                        currentMode = Mode.Edit; // Change mode to Edit after successful addition
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Mode.Edit:
                    return UpdateLicenseClass();
                case Mode.View:
                    return true; // No action needed for view mode
                default:
                    return false;
            }
        }
        public static int ReturnDefaultValidityLength(int LicenseClassID)
        {
            return DataAccesesLayer.clsLicnsesClassesDataAcsses
                .ReturnDefaultValidityLength(LicenseClassID);
        }

        public static List<clsLicnsesClasses> GetAllLicenseClasses()
        {
            var dataTable = DataAccesesLayer.clsLicnsesClassesDataAcsses.GetAllLicnsesClass();
            List<clsLicnsesClasses> licenseClasses = new List<clsLicnsesClasses>();
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return licenseClasses; // Return empty list if no data
            }
            foreach (System.Data.DataRow row in dataTable.Rows)
            {

                licenseClasses.Add(new clsLicnsesClasses(
                    Convert.ToInt32(row["LicenseClassID"]),
                    row["ClassName"].ToString(),
                    row["ClassDescription"].ToString(),
                    Convert.ToInt16(row["MinimumAllowedAge"]),
                    Convert.ToInt16(row["DefaultValidityLength"]),
                    Convert.ToInt32(row["ClassFees"])
                )); 
            }
            return licenseClasses;
        }

        public static int FindClassIdByName(string className)
        {
            return DataAccesesLayer.clsLicnsesClassesDataAcsses.FindLicenseClassIdByName(className);
        }

    }
}
