using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;


namespace ePxCollectWeb
{
    public static class AppSettingsGet
    {
        
      
 
        public static string secssionCounter
        {
            get { return ConfigurationManager.AppSettings["secssionCounter"].ToString(); }
        }

        public static string recordCouner
        {
            get { return ConfigurationManager.AppSettings["recordCouner"].ToString(); }
        }

   
        public static string fileMinLength
        {
            get { return ConfigurationManager.ConnectionStrings["fileMinLength"].ConnectionString; }
        }

        public static string patientMinLength
        {
            get { return ConfigurationManager.ConnectionStrings["patientMinLength"].ConnectionString; }
        }

    }
}