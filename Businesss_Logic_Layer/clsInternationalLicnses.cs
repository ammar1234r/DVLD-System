using DataAccesesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesss_Logic_Layer
{
    public class clsInternationalLicnses
    {
        enum Mode
        {
            enAdd = 0,
            enUpdate = 1
        }
        Mode currentMode;
        public int Id { get; private set; }

        public int ApplicationID { get; set; }
      
        public int LicenseClassId { get; set; }

        /*  int InternationalLicenseID, ref int AppId, ref int LicnseId, ref int IssuedUsingLocalLicenseID,
              ref DateTime IssueDate, ref DateTime ExpiryDate, ref int CreatedByUserID, ref bool IsActive*/
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsActive { get; set; }


        public bool IsInactive { get; set; }
        public clsInternationalLicnses(int applicationID, int licenseClassId, int issuedUsingLocalLicenseID,
            DateTime issueDate, DateTime expirationDate, int createdByUserID, bool isActive)
        {
            Id = -1;
            ApplicationID = applicationID;
           
            LicenseClassId = licenseClassId;
            IssuedUsingLocalLicenseID = issuedUsingLocalLicenseID;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            CreatedByUserID = createdByUserID;
            IsActive = isActive;
            currentMode = Mode.enAdd;
        }
        clsInternationalLicnses
            (int id, int applicationID,  int licenseClassId, int issuedUsingLocalLicenseID,
            DateTime issueDate, DateTime expirationDate, int createdByUserID, bool isActive)
        {
            Id = id;
            ApplicationID = applicationID;
            
            LicenseClassId = licenseClassId;
            IssuedUsingLocalLicenseID = issuedUsingLocalLicenseID;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            CreatedByUserID = createdByUserID;
            IsActive = isActive;
            currentMode = Mode.enUpdate;
        }
        public clsInternationalLicnses()
        {
            Id = -1;
            ApplicationID = 0;
          
            LicenseClassId = 0;
            IssuedUsingLocalLicenseID = 0;
            IssueDate = DateTime.MinValue;
            ExpirationDate = DateTime.MinValue;
            CreatedByUserID = 0;
            IsActive = false;
            currentMode = Mode.enAdd;
        }
        static public clsInternationalLicnses Find(int InternationalLicenseID)
        {
            int AppId = 0, LicnseId = 0, IssuedUsingLocalLicenseID = 0, CreatedByUserID = 0;
            DateTime IssueDate = DateTime.MinValue, ExpiryDate = DateTime.MinValue;
            bool IsActive = false;
            if (clsInternationalLicensesDataAccesses.FindInternationalLicenses(InternationalLicenseID, ref AppId, ref LicnseId, ref IssuedUsingLocalLicenseID,
                ref IssueDate, ref ExpiryDate, ref CreatedByUserID, ref IsActive))
            {
                return new clsInternationalLicnses(InternationalLicenseID, AppId, LicnseId, IssuedUsingLocalLicenseID,
                    IssueDate, ExpiryDate, CreatedByUserID, IsActive);
            }
            return null;
        }
        bool AddNewInternationalLicense()
        {
            int newId = clsInternationalLicensesDataAccesses.AddNewLicnse(ApplicationID, LicenseClassId, IssuedUsingLocalLicenseID,
                IssueDate, ExpirationDate, CreatedByUserID, IsActive);
            if (newId > 0)
            {
                Id = newId;
                return true;
            }
            return false;
        }
        bool UpdateInternationalLicense()
        {
            bool result = clsInternationalLicensesDataAccesses.UpdateInternationalLicense(Id, ApplicationID, LicenseClassId,
                IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, CreatedByUserID, IsActive);
            return result;
        }
        public bool Save()
        {
            if (currentMode == Mode.enAdd)
            {
                if (AddNewInternationalLicense())
                {
                    currentMode = Mode.enUpdate; // Change mode to Update after adding
                    return true;
                }
                return false;
            }
            else if (currentMode == Mode.enUpdate)
            {
                return UpdateInternationalLicense();
            }
            return false;

        }
    }
}