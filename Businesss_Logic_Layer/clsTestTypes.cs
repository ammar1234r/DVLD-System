using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesss_Logic_Layer
{
    public class clsTestTypes
    {
        public int id { get;private set; }
        public string testTypeName { get; private set; }
        public string description { get; private set; }
        public int TestTypeFees { get; private set; }

        public clsTestTypes() { }
         clsTestTypes (int id, string testTypeName, string description, int TestTypeFees)
        {
            this.id = id;
            this.testTypeName = testTypeName;
            this.description = description;
            this.TestTypeFees = TestTypeFees;
        }

        public static clsTestTypes Find(int id)
        {
            string testTypeName = string.Empty;
            string description = string.Empty;
            int TestTypeFees = 0;
            if (DataAccesesLayer.clsTestTypesDataAcsses.FindTestType(id, ref testTypeName, ref description, ref TestTypeFees))
            {
                return new clsTestTypes(id, testTypeName, description, TestTypeFees);
            }
            else
            {
                return null;
            }
        }

        public static int FindTestIdByName(string name)
        {
            int id = DataAccesesLayer.clsTestTypesDataAcsses.FindTestTypeByName(name);
            return id>0? id : 0;
        }
        public DataTable GetAllTestTypes()
        {
            return DataAccesesLayer.clsTestTypesDataAcsses.GetAllTestTypes();
        }

        public bool Update(int id, string testTypeName, string description, int TestTypeFees)
        {
            return DataAccesesLayer.clsTestTypesDataAcsses.UpdateTestType(id, testTypeName, description, TestTypeFees);
        }

        public string ReturnTestTypeName(int id)
        {
            clsTestTypes testType = Find(id);
            if (testType.testTypeName == null)
            {
                return string.Empty;

            }
            else
            {
                return testType.testTypeName;
            }
        }

       
    }
}
