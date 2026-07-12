using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Businesss_Logic_Layer.clsTests;
using static System.Net.Mime.MediaTypeNames;
using DataAccesesLayer;
namespace Businesss_Logic_Layer
{
    public class clsTestAppointments
    {
         enum  Mode
        {
            enAdd, enUpdate

        }
        Mode mode;
        public int Id { get; private set; }
        public int ApplicationID { get;  set; }
        public int TestTypeID { get;  set; }
        public DateTime AppointmentDate { get;  set; }
        public int CreatedByUserID { get;  set; }
        public bool IsLocked { get;  set; }
        public int RetakeTestApplicationID { get;  set; }
        public int PaidFees { get;  set; }

        public clsTestAppointments( int applicationID, int testTypeID, DateTime appointmentDate, 
            int createdByUserID, bool isLocked, int retakeTestApplicationID, int paidFees)
        {
            Id = -1;
            ApplicationID = applicationID;
            TestTypeID = testTypeID;
            AppointmentDate = appointmentDate;
            CreatedByUserID = createdByUserID;
            IsLocked = isLocked;
            RetakeTestApplicationID = retakeTestApplicationID;
            PaidFees = paidFees;
            mode = Mode.enAdd;
        }
        clsTestAppointments(int id, int applicationID, int testTypeID, DateTime appointmentDate, 
            int createdByUserID, bool isLocked, int retakeTestApplicationID, int paidFees)
        {
            Id = id;
            ApplicationID = applicationID;
            TestTypeID = testTypeID;
            AppointmentDate = appointmentDate;
            CreatedByUserID = createdByUserID;
            IsLocked = isLocked;
            RetakeTestApplicationID = retakeTestApplicationID;
            PaidFees = paidFees;
            mode = Mode.enUpdate;
        }

        public clsTestAppointments()
        {
            Id = -1;
            ApplicationID =-1;
            TestTypeID = 0;
            AppointmentDate = DateTime.MinValue;
            CreatedByUserID = 1;
            IsLocked = false;
            RetakeTestApplicationID = 0;
            PaidFees = 0;
            mode = Mode.enAdd;
        }
        public static clsTestAppointments Find(int id)
        {
            int applicationID = 0, testTypeID = 0, createdByUserID = 0, retakeTestApplicationID = 0, paidFees = 0;
            DateTime appointmentDate = DateTime.MinValue;
            bool isLocked = false;
            if (DataAccesesLayer.clsTestAppointmentsDataAcsses.FindTestAppointment(id, ref applicationID, ref testTypeID,
                ref appointmentDate, ref createdByUserID, ref isLocked, ref retakeTestApplicationID, ref paidFees))
            {
                return new clsTestAppointments(id, applicationID, testTypeID, appointmentDate,
                    createdByUserID, isLocked, retakeTestApplicationID, paidFees);
            }
            else
            {
                return null;
            }
        }



        bool Update()
        {
            return DataAccesesLayer.clsTestAppointmentsDataAcsses.UpdateTestAppointment(
                Id, ApplicationID, TestTypeID, AppointmentDate,
                CreatedByUserID, IsLocked,
                RetakeTestApplicationID <= 0 ? -1 : RetakeTestApplicationID,
                PaidFees);
        }
        bool Add()
        {
            int id = DataAccesesLayer.clsTestAppointmentsDataAcsses.AddNewAppoinment(
                ApplicationID, TestTypeID, AppointmentDate,
                CreatedByUserID, IsLocked, RetakeTestApplicationID, PaidFees);

            if (id > 0)
            {
                Id = id; 
                mode = Mode.enUpdate;
                return true;
            }
            return false;
        }

        public bool Save()
        {
            if (mode == Mode.enAdd)
            {
                if(Add()) {                     mode = Mode.enUpdate;
                    return true;
                }
                return false;
            }
            else if (mode == Mode.enUpdate)
            {
                return Update();
            }
            return false;
        }

        public DataTable GetAllTestAppointments()
        {
            return DataAccesesLayer.clsTestAppointmentsDataAcsses.GetAllTestAppointments();
        }
        public static int CountOfTestByClassName(int id,int ClassTypeId)
        {
            return DataAccesesLayer.clsTestAppointmentsDataAcsses.GetAllTestAppointmentsCount(id,ClassTypeId);
        }
        public static DataTable GetAllAppointment(int LocalId,int TypeId)
        {
            return clsTestAppointmentsDataAcsses.GetAllAppointmentBYType(LocalId, TypeId);
        }
        public static  bool IsLocalIdHaveAppoinment(int LocalId,int TestTypeId)
        {
            return clsTestAppointmentsDataAcsses.IsLocalIdHaveAppoinment(LocalId, TestTypeId);
        }
        public static bool ISAppoinmentLocked(int AppoinmentId)
        {
            return clsTestAppointmentsDataAcsses.IsAppoinmetExpired(AppoinmentId);
        }
        public static bool ISApplicantPassedTest(int LocalId,int TestTypeId)
        {
            return clsTestAppointmentsDataAcsses.IsApplicantPassedInTest(LocalId, TestTypeId);
        }
    }

}