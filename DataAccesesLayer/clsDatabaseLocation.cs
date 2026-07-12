using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesesLayer
{
    static public class clsDatabaseLocation
    {
        public static string connectionString =
    System.Configuration.ConfigurationManager
          .ConnectionStrings["DVLD"].ConnectionString;

    }

}

    