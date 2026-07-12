using DataAccesesLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Businesss_Logic_Layer
{
    public class clsAppTypes
    {
        static public Dictionary<string, int > AppTypesList = new Dictionary<string, int > {    
    {  "New Local Driving License Service",15 },
    {  "Renew Driving License Service",5 },
    {  "Replacement for a Lost Driving License",10 },
    {  "Replacement for a Damaged Driving License",5 },
    {  "Release Detained Driving Licsense",15 },
    {  "New International License",50 },
    {  "Retake Test",5 }    
};
                    
        enum AppType
        {
            enAdd = 0,
            enUpdate = 1
        }
        AppType appType;
        public int Id { get; private set; }
        public string ApplicationTypeName { get;  set; }
        public int ApplicationFees { get;  set; }
        public clsAppTypes(string applicationTypeName, int applicationFees)
        {
            Id = -1;
            ApplicationTypeName = applicationTypeName;
            ApplicationFees = applicationFees;
            appType = AppType.enAdd;
        }

        public clsAppTypes()
        {
            Id = -1;
            ApplicationTypeName = "";
            ApplicationFees = 0;
            appType = AppType.enAdd;
        }
        clsAppTypes(int id, string applicationTypeName, int applicationFees)
        {
            Id = id;
            ApplicationTypeName = applicationTypeName;
            ApplicationFees = applicationFees;
            appType = AppType.enUpdate;
        }
        public static clsAppTypes Find(int Id)
        {
            string ApplicationTypeName = "";
            int ApplicationFees = 0;
            if (DataAccesesLayer.cls_ApplicationTypesDataAcsses.FindApplicationType(Id, ref ApplicationTypeName, ref ApplicationFees))
            {
                return new clsAppTypes(Id, ApplicationTypeName, ApplicationFees);
            }
            else
            {
                return null;
            }
        }

        bool AddNewApplicationType()
        {
            int newId = DataAccesesLayer.cls_ApplicationTypesDataAcsses.AddNewApplicationType(ApplicationTypeName, ApplicationFees);
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
        bool UpdateApplicationType()
        {
            if (DataAccesesLayer.cls_ApplicationTypesDataAcsses.UpdateApplicationType(Id, ApplicationTypeName, ApplicationFees))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Save() { switch(appType)
            {
                case AppType.enAdd:
                    if( AddNewApplicationType())
                    {
                        appType = AppType.enUpdate;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case AppType.enUpdate:
                    return UpdateApplicationType();
                default:
                    return false;
            }
        }

       public static DataTable GetAllApplicationTypes()
        {
            return cls_ApplicationTypesDataAcsses.GetAllAppTypes();
        }
    }
}
