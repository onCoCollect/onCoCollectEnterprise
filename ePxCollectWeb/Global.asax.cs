using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;

namespace ePxCollectWeb
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

        }

        void Application_End(object sender, EventArgs e)
        {

        }

        void Application_Error(object sender, EventArgs e)
        {
            string UserID = "Exception Before Initialization";
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session != null)
                    UserID = Convert.ToString(Session["Login"]);
            }
            try
            {
                Exception exception = Server.GetLastError();
                System.Web.HttpBrowserCapabilities browser = Request.Browser;
                string BrowserDetails = "Type = " + browser.Type + "Name = " + browser.Browser + "Version = " + browser.Version;
                GlobalValues.ErrorLog(UserID, exception.StackTrace, exception.Message + exception.InnerException.Message.ToString() + exception.InnerException.ToString(), BrowserDetails, exception.ToString());

            }
            catch (Exception ex)
            {
              
            }


        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started


        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

            if (Session["Login"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            string UserID = Convert.ToString(Session["Login"]);
            string PatientID = Convert.ToString(Session["PatientID"]);
            GlobalValues.UnlockUser(UserID, PatientID);
            GlobalValues.ForceLogOutForExpiredUsers();
            GlobalValues.RemoveCurretLoginUsersDuringLogOut(Convert.ToString(Session["Login"]), GlobalValues.gEnterpriseApplicationName, GlobalValues.gLHSessionTimeOut, "Forced Logout during Session TimeOut.");
            //Response.Redirect("Login.aspx");

        }

    }
}
