using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesss_Logic_Layer
{
    public class clsApplications
    {

        enum AppType
        {
            enAdd = 0,
            enUpdate = 1
        }

      public  enum AppStatus
        {
            enCompleted = 3,
            enCancelled = 1,
            enInProgress = 2,
                enDeleting = 0
        }
        AppType appType;
      public  AppStatus AppTypeProperty { get;set; }

        public int Id { get; private set; }
        public int ApplicationTypeID { get;  set; }

        public int ApplicantPersonID { get;  set; }

        public DateTime ApplicationDate { get;  set; }

        public int CreatedByUserID { get;  set; }

        public byte ApplicationStatus { get;  set; }

        public int PaidFees { get;  set; }

        public DateTime LastStatusDate { get;  set; }

        byte AppStatusToByte(AppStatus status)
        {
            return (byte)status;
        }

        public clsApplications(int applicationTypeID, int applicantPersonID, DateTime applicationDate,
            int createdByUserID, byte isLocked, int paidFees, DateTime lastStatusDate)
        {
            Id = -1;
            ApplicationTypeID = applicationTypeID;
            ApplicantPersonID = applicantPersonID;
            ApplicationDate = applicationDate;
            CreatedByUserID = createdByUserID;
           ApplicationStatus = isLocked;

            PaidFees = paidFees;
            LastStatusDate = lastStatusDate;
            appType = AppType.enAdd;
        }

        clsApplications
            (int id, int applicationTypeID, int applicantPersonID, DateTime applicationDate,
            int createdByUserID, Byte isLocked, int paidFees, DateTime lastStatusDate)
        {
            Id = id;
            ApplicationTypeID = applicationTypeID;
            ApplicantPersonID = applicantPersonID;
            ApplicationDate = applicationDate;
            CreatedByUserID = createdByUserID;
           ApplicationStatus = isLocked;
            PaidFees = paidFees;
            LastStatusDate = lastStatusDate;
            appType = AppType.enUpdate;
        }
        public  clsApplications()
        {
            Id = -1;
            ApplicationTypeID = -1;
            ApplicantPersonID = -1;
            ApplicationDate = DateTime.MinValue;
            CreatedByUserID = -1;
            ApplicationStatus = 0;
            PaidFees = 0;
            LastStatusDate = DateTime.MinValue;
            appType = AppType.enAdd;
        }
        public bool Validate()
        {
            if (ApplicantPersonID <= 0)
                return false;

            if (ApplicationTypeID <= 0)
                return false;

            if (PaidFees < 0)
                return false;

            if (ApplicationDate > DateTime.Now)
                return false;

            return true;
        }
        public static clsApplications Find(int Id)
        {
            int ApplicationTypeID = 0, ApplicantPersonID = 0, CreatedByUserID = 0, PaidFees = 0;
            DateTime ApplicationDate = DateTime.MinValue, LastStatusDate = DateTime.MinValue;
            Byte IsLocked = 0;
            if (DataAccesesLayer.clsApplicationsDataAccesses.FindApplication(Id, ref ApplicationTypeID, ref ApplicantPersonID,
                ref ApplicationDate, ref CreatedByUserID, ref IsLocked, ref PaidFees, ref LastStatusDate))
            {
                
                return new clsApplications(Id, ApplicationTypeID, ApplicantPersonID, ApplicationDate,
                    CreatedByUserID, IsLocked, PaidFees, LastStatusDate);
            }
            else
            {
                return null;
            }
        }
        public static DataTable GetLocalAppByStatus(int status)
        {
            return DataAccesesLayer.clsApplicationsDataAccesses.GetLocalAppByStatus(status);
        }
        bool AddNewApplication()
        {
            ApplicationStatus = AppStatusToByte(AppTypeProperty);
            int newId = DataAccesesLayer.clsApplicationsDataAccesses.AddNewApplication(ApplicationTypeID, ApplicantPersonID,
                ApplicationDate, CreatedByUserID, ApplicationStatus, PaidFees, LastStatusDate);
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

        bool UpdateApplication()
        {
            ApplicationStatus = AppStatusToByte(AppTypeProperty);

            if (DataAccesesLayer.clsApplicationsDataAccesses.UpdateApplication(Id, ApplicationTypeID, ApplicantPersonID,
                ApplicationDate, CreatedByUserID, ApplicationStatus, PaidFees, LastStatusDate))
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
            if (!Validate())
            {
                return false;
            }
            switch (appType)
            {
                case AppType.enAdd:
                    if (AddNewApplication())
                    {
                        appType = AppType.enUpdate; // Change to update mode after adding
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case AppType.enUpdate:
                    return UpdateApplication();
                default:
                    throw new InvalidOperationException("Unknown application type");
            }
        }

        public static DataTable GetAllLocalApp()
        {
                       return DataAccesesLayer.clsApplicationsDataAccesses.GetAllLocalApp();
        }

       
    }
}
