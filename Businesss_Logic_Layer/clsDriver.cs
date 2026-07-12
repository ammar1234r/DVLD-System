using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesss_Logic_Layer
{
    public class clsDriver
    {
        enum Mode
        {
            enAdd = 0,
            enUpdate = 1
        }
        Mode mode;


        public int Id { get;private set; }
        public int PersonID { get; set; }

        public int CreatedByUserID { get;private set; }

        public int UpdatedByUserID { get; set; }

        public DateTime CreatedDate { get;private set; }

    public clsDriver(int personID, int createdByUserID)
        {
            Id = -1;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            CreatedDate = DateTime.Now;
            UpdatedByUserID = 1;
            mode = Mode.enAdd;
        }
        clsDriver(int id, int personID, int createdByUserID, DateTime createdDate, int updatedByUserID)
        {
            Id = id;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            CreatedDate = createdDate;
            UpdatedByUserID = updatedByUserID;
            mode = Mode.enUpdate;   
        }
        public clsDriver()
        {
            Id = -1;
            PersonID = -1;
            CreatedByUserID = 1;
            CreatedDate = DateTime.Now;
            UpdatedByUserID = -1;
            mode = Mode.enAdd;
        }
        public static clsDriver Find(int id)
        {
                       int personId = -1;
            int createdById = -1;
            DateTime createdTime = DateTime.MinValue;
            bool isFound = DataAccesesLayer.clsDriverDataAccsess.FindDriverById(id, ref personId, ref createdById, ref createdTime);
            if (isFound)
            {
                return new clsDriver(id, personId, createdById, createdTime, -1);
            }
            else
            {
                return null; // or throw an exception, or handle as appropriate
            }
        }
        bool Add()
        {
            bool isSuccess = false;
            int driverId = DataAccesesLayer.clsDriverDataAccsess.AddDriver(PersonID, CreatedByUserID, CreatedDate);
            if (driverId > 0)
            {
                Id = driverId;
                isSuccess = true;
            }
            return isSuccess;
        }

        public static int FindByPeopleID(int personId)
        {
            return DataAccesesLayer.clsDriverDataAccsess.FindDriverIdByPersonId(personId);
        }
        bool Update()
        {
            bool isSuccess = false;
            isSuccess = DataAccesesLayer.clsDriverDataAccsess.UpdateDriver(Id, PersonID, UpdatedByUserID, CreatedDate);
            return isSuccess;
        }
        public bool Save()
        {
       switch(mode)
            {
                case Mode.enAdd:
                    if(Add())
                    {
                       mode=Mode.enUpdate;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Mode.enUpdate:
                            return Update();
                        default:
                            return false;
                        }
        }

        public static bool Delete(int id)
        {
            return DataAccesesLayer.clsDriverDataAccsess.DeleteDriver(id);
        }
        public static DataTable GetAllDrivers()
        {
            return DataAccesesLayer.clsDriverDataAccsess.GetAllDrivers();
        }
      


    }
}
