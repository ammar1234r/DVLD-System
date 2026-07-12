using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccesesLayer;

namespace Businesss_Logic_Layer
{
    public static class clsDashboard
    {
        public static (int persons, int drivers, int licenses, int detained) GetStats()
        {
            return clsDashboardDataAccesses.GetDashboardStats();
        }
    }
}
