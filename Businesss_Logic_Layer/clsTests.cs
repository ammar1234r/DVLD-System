
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesss_Logic_Layer
{
    public class clsTests
    {
        enum Mode
        {
            AddNew,
            Update
        }
        public enum TestType
        {
            Vision,
            Theory,
            Road
        }
        private Mode CurrentMode = Mode.AddNew;
        public int TestID { get; private set; }
        public int TestAppointmentID { get;  set; }
        public bool TestResult { get;  set; }
        public string Notes { get;  set; }

        public int CreatedByUserID { get;  set; }
        public clsTests(int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            TestAppointmentID = testAppointmentID;
            TestResult = testResult;
            Notes = notes;
            CreatedByUserID = createdByUserID;
            CurrentMode = Mode.AddNew;

        }
        clsTests(int TestId, int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            TestID = TestId;
            TestAppointmentID = testAppointmentID;
            TestResult = testResult;
            Notes = notes;
            CreatedByUserID = createdByUserID;
            CurrentMode = Mode.Update;


        }
        public clsTests()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = "";
            CreatedByUserID = 1;
            CurrentMode = Mode.AddNew;
        }

        public static clsTests Find(int Id)
        {
            int TestAppointmentID = 0;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = 0;
            if (DataAccesesLayer.clsTestsDataAccesses.FindTest(Id, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                return new clsTests(Id, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }
            else
            {
                return null;
            }
        }

        bool AddNewTest()
        {
            int newId = DataAccesesLayer.clsTestsDataAccesses.AddNewTest(TestAppointmentID, TestResult, Notes, CreatedByUserID);
            if (newId > 0)
            {
                TestID = newId;
                return true;
            }
            else
            {
                return false;
            }
        }

        bool UpdateTest()
        {
            return DataAccesesLayer.clsTestsDataAccesses.UpdateTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
        }

        public bool Save()
        {
            switch (CurrentMode)
            {
                case Mode.AddNew:
                    if (AddNewTest())
                    {
                        CurrentMode = Mode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Mode.Update:
                    return UpdateTest();
                default:
                    throw new InvalidOperationException("Invalid mode");
            }
        }

        public static bool ISDriverTakethisTest(int AppointmentID, TestType type)
        {
            int testId = 0;
            switch (type)
            {
                case TestType.Vision:
                    testId = 1;
                    return DataAccesesLayer.clsTestsDataAccesses.DidHePasTheTest(AppointmentID,1);
                case TestType.Theory:
                    testId = 2;
                    return DataAccesesLayer.clsTestsDataAccesses.DidHePasTheTest(AppointmentID,2);
                case TestType.Road:
                    testId = 3;
                    return DataAccesesLayer.clsTestsDataAccesses.DidHePasTheTest(AppointmentID,3);
                default:
                    throw new InvalidOperationException("Invalid test type");
            }

        }
        static public bool IsApplicantTakedTheTest(int AppId, Byte TestTypeId)
        {
            return DataAccesesLayer.clsTestsDataAccesses.IsApplicantTakedTheTest(AppId, TestTypeId);
        }
    }
}
