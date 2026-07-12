using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Businesss_Logic_Layer
{
    public class clsLicenses
    {
            enum LicenseType
            {
                enAdd = 0,
                enUpdate = 1
            }
        LicenseType licenseType;
        public int Id { get; private set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClassId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public int PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public int CreatedByUserID { get; set; }
        public clsLicenses(int applicationID, int driverID, int licenseClassId, DateTime issueDate, DateTime expirationDate, string notes, int paidFees, bool isActive, Byte issueReason, int createdByUserID)
        {
            Id = -1;
            ApplicationID = applicationID;
            DriverID = driverID;
            LicenseClassId = licenseClassId;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = (enIssueReason)issueReason;
            CreatedByUserID = createdByUserID;
            licenseType = LicenseType.enAdd;
        }
        clsLicenses(int id, int applicationID, int driverID, int licenseClassId, DateTime issueDate, DateTime expirationDate, string notes, int paidFees, bool isActive, Byte issueReason, int createdByUserID)
        {
            Id = id;
            ApplicationID = applicationID;
            DriverID = driverID;
            LicenseClassId = licenseClassId;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = (enIssueReason)issueReason;
            CreatedByUserID = createdByUserID;
            licenseType = LicenseType.enUpdate;
        }
        public clsLicenses()
        {
            Id = -1;
            ApplicationID = 0;
            DriverID = 0;
            LicenseClassId = 0;
            IssueDate = DateTime.MinValue;
            ExpirationDate = DateTime.MinValue;
            Notes = string.Empty;
            PaidFees = 0;
            IsActive = false;
            IssueReason =0;
            CreatedByUserID = 0;
            licenseType = LicenseType.enAdd; // Default to Add mode
        }
        public bool Validate()
        {
            if (ApplicationID <= 0)
                return false;

            if (DriverID <= 0)
                return false;

            if (LicenseClassId <= 0)
                return false;

            if (ExpirationDate <= IssueDate)
                return false;

            if (PaidFees < 0)
                return false;

            return true;
        }
        static public clsLicenses FindLicenseByLicenseID(int id)
        {
            int applicationID = 0, driverID = 0, licenseClassId = 0, paidFees = 0, createdByUserID = 0;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            string notes = string.Empty;Byte issueReason = 0;
            bool isActive = false;
            if (DataAccesesLayer.clsLicensesDataAcsses.FindLicnse(id, ref applicationID, ref driverID, ref licenseClassId,
                ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID))
            {
                return new clsLicenses(id, applicationID, driverID, licenseClassId, issueDate, expirationDate,
                    notes, paidFees, isActive, issueReason, createdByUserID);
            }
            else
            {
                return null;
            }
        }
        static public  clsLicenses FindByNationalID(string nationalId)
        {
            int id = 0, applicationID = 0, driverID = 0, licenseClassId = 0, paidFees = 0, createdByUserID = 0;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            string notes = string.Empty; Byte issueReason = 0;
            bool isActive = false;
            if (DataAccesesLayer.clsLicensesDataAcsses.FindLicenseByNationalId(nationalId, ref id, ref applicationID, ref driverID, ref licenseClassId,
                ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID))
            {
                return new clsLicenses(id, applicationID, driverID, licenseClassId, issueDate, expirationDate,
                    notes, paidFees, isActive, issueReason, createdByUserID);
            }
            else
            {
                return null;
            }
        }
        bool UpdateLicense()
        {
            bool result = DataAccesesLayer.clsLicensesDataAcsses.UpdateLicense(Id, ApplicationID, DriverID, LicenseClassId,
                IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (Byte)IssueReason, CreatedByUserID);
            return result;
        }
        bool AddNewLicense()
        {
            int newId = DataAccesesLayer.clsLicensesDataAcsses.AddNewLicnese(ApplicationID, DriverID, LicenseClassId,
                IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (Byte)IssueReason, CreatedByUserID);
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

        public bool Save()
        {
            if (!Validate()) { 
                return false; }
            switch (licenseType)
            {
                case LicenseType.enAdd:
                    if (AddNewLicense())
                    {
                        licenseType = LicenseType.enUpdate; // Change to update mode after adding
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case LicenseType.enUpdate:
                    return UpdateLicense();
                default:
                    throw new InvalidOperationException("Unknown license type.");
            }
                
                }

        public static bool IsThePeopleHaveLicnse(int personId,int  ClassId)
        {
            return DataAccesesLayer.clsLicensesDataAcsses.IsPersonhaveLicnse(personId,ClassId);
        }
        public static bool IsDriverHaveLicense(int AppID,int LicenseTypeId)
        {
           return DataAccesesLayer.clsLicensesDataAcsses.IsDriverHaveLicnse(AppID, LicenseTypeId);
        }
        public static clsLicenses FindByLocalID(int AppID)
        {
            int id = 0;
            int driverID = 0, licenseClassId = 0, paidFees = 0, createdByUserID = 0;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            string notes = string.Empty; Byte issueReason = 0;
            bool isActive = false;
            if (DataAccesesLayer.clsLicensesDataAcsses.FindLicenseByLocalId(AppID, ref id, ref driverID, ref licenseClassId,
                ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID))
            {
                return new clsLicenses(id, AppID, driverID, licenseClassId, issueDate, expirationDate,
                    notes, paidFees, isActive, issueReason, createdByUserID);
            }
            else
            {
                return null;
            }
        }
        public static int GetActiveLicensesCount()
    => DataAccesesLayer.clsLicensesDataAcsses.GetActiveLicensesCount();
    }
}
