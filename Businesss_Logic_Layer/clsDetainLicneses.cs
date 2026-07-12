using DataAccesesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesss_Logic_Layer
{
    public  class clsDetainLicneses
    {
      enum Mode
        {
           enAdd = 0,
            enUpdate = 1
        }
        Mode mode;
        public int Id { get;  private set; }
        public int LicneseId { get;set; }

        public DateTime DetentionDate { get; set; }

        public int fees { get; set; }

        public int CreatedBy { get; set; }

        public bool IsReleased { get; set; }

        public DateTime ReleasDate { get; set; }

        public int ReleasedByUserId { get; set; }

        public int releaseAppLicationId { get; set; }

        public string Notes { get; set; }

        
       static public IReadOnlyDictionary<string,int> FineByReason { get; } =
    new Dictionary<string,int>      
    {
        { "Traffic Violation",   50  },
        { "DUI / Drunk Driving", 200 },
        { "Expired License",     30  },
        { "Court Order",         100 },
        { "Medical Reasons",     10   },
        { "Other",               5     }
    };
        public clsDetainLicneses()
        {
            Id = -1;
            LicneseId = -1;
            DetentionDate=DateTime.MinValue;
            fees = 0;
            CreatedBy = -1;
            IsReleased = false;
            ReleasDate = DateTime.MinValue;
            ReleasedByUserId = 1;

            releaseAppLicationId = -1;
            mode = Mode.enAdd;

        }
        public clsDetainLicneses(int LicenseID, DateTime DetentionDate, int FineFees, int CreatedByUserID, bool IsReleased, DateTime? ReleaseDate = null, int? ReleasedByUserID = null, int? ReleaseApplicationID = null)
        {
            this.LicneseId = LicenseID;
            this.DetentionDate = DetentionDate;
            this.fees = FineFees;
            this.CreatedBy = CreatedByUserID;
            this.IsReleased = IsReleased;
            this.ReleasDate = ReleasDate;
            this.ReleasedByUserId = ReleasedByUserId;
            this.releaseAppLicationId = releaseAppLicationId;
            mode = Mode.enAdd;
        }
         clsDetainLicneses(int id,int LicenseID, DateTime DetentionDate, int FineFees, int CreatedByUserID, bool IsReleased, DateTime? ReleaseDate = null, int? ReleasedByUserID = null, int? ReleaseApplicationID = null)
        {
            this.Id = id;
            this.LicneseId = LicenseID;
            this.DetentionDate = DetentionDate;
            this.fees = FineFees;
            this.CreatedBy = CreatedByUserID;
            this.IsReleased = IsReleased;
            this.ReleasDate = ReleasDate;
            this.ReleasedByUserId = ReleasedByUserId;
            this.releaseAppLicationId = releaseAppLicationId;
            mode = Mode.enUpdate;
        }
        public static clsDetainLicneses Find(int id)
        {
            int Id = -1, LicenseID = -1; DateTime DetentionDate = DateTime.MinValue;
            int FineFees = -1, CreatedByUserID = -1; bool IsReleased = false; DateTime ReleaseDate = DateTime.MinValue; 
            int ReleasedByUserID = -1; int ReleaseApplicationID = -1;
            
         
            bool isFound = DataAccesesLayer.clsDetainedLicensesDataAccesses.FindDetainedLicense(id, ref LicenseID, ref DetentionDate, ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID);  
            return isFound ? new clsDetainLicneses(id, LicenseID, DetentionDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID) : null;
        }
        public static clsDetainLicneses FindDetainByLicensesId(int licenseId)
        {
            int Id = -1; DateTime DetentionDate = DateTime.MinValue;
            int FineFees = -1, CreatedByUserID = -1; bool IsReleased = false; DateTime ReleaseDate = DateTime.MinValue;
            int ReleasedByUserID = -1; int ReleaseApplicationID = -1;
            bool isFound = DataAccesesLayer.clsDetainedLicensesDataAccesses.FindDetainedLicenseByLicenseID(licenseId, ref Id, ref DetentionDate, ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID);
            return isFound ? new clsDetainLicneses(Id, licenseId, DetentionDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID) : null;
        }
        bool Update()
        {
            bool isSuccess = false;
            isSuccess = DataAccesesLayer.clsDetainedLicensesDataAccesses.UpdateDetainedLicense(Id, LicneseId, DetentionDate, fees, CreatedBy, IsReleased, ReleasDate, ReleasedByUserId, releaseAppLicationId);
            return isSuccess;
        }
        bool Add()
        {
            int detainedLicenseId = DataAccesesLayer.clsDetainedLicensesDataAccesses.AddNewDetainedLicense(
        LicneseId, DetentionDate, fees, CreatedBy, IsReleased,
        ReleasDate == DateTime.MinValue ? (DateTime?)null : ReleasDate,           
        ReleasedByUserId == -1 ? (int?)null : ReleasedByUserId,                   
        releaseAppLicationId == -1 ? (int?)null : releaseAppLicationId);          

            if (detainedLicenseId > 0)
            {
                Id = detainedLicenseId;
                return true;
            }
            return false;
        }
        public bool Save()
        {
            bool isSuccess = false;
            switch (mode)
            {
                case Mode.enAdd:
                    isSuccess = Add();
                    if (isSuccess)
                    {
                        mode = Mode.enUpdate; // Switch to update mode after a successful add
                    }
                    break;
                case Mode.enUpdate:
                    isSuccess = Update();
                    break;
                default:
                    throw new InvalidOperationException("Invalid mode.");
            }
            return isSuccess;
        }
        public static bool Delete(int id)
        {
            return clsDetainedLicensesDataAccesses.DeleteDetainedLicense(id);

        }
        public bool Release()
        {
            if (IsReleased)
            {
                throw new InvalidOperationException("The license is already released.");
            }
            clsApplications RelesApp = new clsApplications();
            RelesApp.ApplicationTypeID = 5;
            RelesApp.PaidFees = 15;
            RelesApp.ApplicantPersonID = clsPerson.FindPeopleIDByLicenseId(LicneseId) ;
            RelesApp.ApplicationStatus = (byte)clsApplications.AppStatus.enCompleted;
            RelesApp.LastStatusDate = DateTime.Now;
            RelesApp.ApplicationDate = DateTime.Now;
            RelesApp.CreatedByUserID = CreatedBy;

            if(!RelesApp.Save())
            {
                throw new InvalidOperationException("Failed to create the release application.");
                return false;
            }
            releaseAppLicationId = RelesApp.Id;
            ReleasedByUserId = CreatedBy; // Assuming the same user is releasing the license
            IsReleased = true;
            ReleasDate = DateTime.Now;


            DataAccesesLayer.clsLicensesDataAcsses.ActiveLicense(LicneseId);

            return Save();
        }
        public static int GetDetainedCount()
     => DataAccesesLayer.clsDetainedLicensesDataAccesses.GetDetainedCount();

    }
}
