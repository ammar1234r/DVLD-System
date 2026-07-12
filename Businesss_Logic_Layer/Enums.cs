using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesss_Logic_Layer
{
    
         public enum enIssueReason : byte
    {
        FirstTime   = 1,
        Renewal     = 2,
        LostDamaged = 3
    }

    public enum enApplicationStatus : byte
    {
        Deleted    = 0,
        Cancelled  = 1,
        InProgress = 2,
        Completed  = 3
    }

    public enum enGender : byte
    {
        Male   = 0,
        Female = 1
    }
    public enum enUserRole : byte
    {
        Admin=1,
        Employee=2, Police=3

    }
}

    

