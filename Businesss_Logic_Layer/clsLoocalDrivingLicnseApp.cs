using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesss_Logic_Layer
{
    public class clsLoocalDrivingLicnseApp
    {
        enum Mode
        {
            AddNew,
            Edit,
            View
        }
        Mode currentMode;
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        clsLoocalDrivingLicnseApp(int localDrivingLicenseApplicationID, int applicationID, int licenseClassID)
        {
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            ApplicationID = applicationID;
            LicenseClassID = licenseClassID;
            currentMode = Mode.Edit;
        }
        public clsLoocalDrivingLicnseApp()
        {
            LocalDrivingLicenseApplicationID = -1;
            ApplicationID = -1;
            LicenseClassID = -1;
            currentMode = Mode.AddNew;
        }
        static public clsLoocalDrivingLicnseApp FindApp(int LocalDrivingLicenseApplicationID)
        {
            int applicationID = -1;
            int licenseClassID = -1;
            if (DataAccesesLayer.clsLocalDrivingLicenseApplicationsDataAccesses.FindLocalDrivingLicenseApp(LocalDrivingLicenseApplicationID, ref applicationID, ref licenseClassID))
            {
                return new clsLoocalDrivingLicnseApp(LocalDrivingLicenseApplicationID, applicationID, licenseClassID);
            }
            else
            {
                return null;
            }
        }
        bool AddNewLocalDrivingLicenseApp()
        {
            if (DataAccesesLayer.clsLocalDrivingLicenseApplicationsDataAccesses.AddNewLocalDrivingLicenseApp(ApplicationID, LicenseClassID) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool EditLocalDrivingLicenseApp()
        {
            if (DataAccesesLayer.clsLocalDrivingLicenseApplicationsDataAccesses.UpdateLocalDrivingLicenseApp(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Save()
        {
            switch (currentMode)
            {
                case Mode.AddNew:
                    if (AddNewLocalDrivingLicenseApp())
                    {
                        currentMode = Mode.Edit; // Change mode to Edit after adding
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Mode.Edit:
                    return EditLocalDrivingLicenseApp();
                case Mode.View:
                    return true; // No action needed for view mode
                default:
                    throw new InvalidOperationException("Invalid mode");
            }
        }

        public bool Delete()
        {
            if (DataAccesesLayer.clsLocalDrivingLicenseApplicationsDataAccesses.DeleteLocalDrivingLicenseApp(LocalDrivingLicenseApplicationID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DataTable GetAllTables()
        {
            return DataAccesesLayer.clsLocalDrivingLicenseApplicationsDataAccesses.GetAllLocalDrivingLicenseApplications();
        }
    }
}