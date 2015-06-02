using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ePxCollectDataAccess;
using System.Data;

namespace ePxCollectWeb
{
    public partial class Backup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            //IF SQL Server Authentication then Connection String  
            //con.ConnectionString = @"Server=MyPC\SqlServer2k8;database=" + YourDBName + ";uid=sa;pwd=password;";  

            //IF Window Authentication then Connection String  
            string strConn = GlobalValues.strConnString;


            string backupDIR = Server.MapPath("~/BackUp");
            if (!System.IO.Directory.Exists(backupDIR))
            {
                System.IO.Directory.CreateDirectory(backupDIR);
            }
            try
            {
                string DBName = ConfigurationManager.AppSettings["Backup"];
                string strSQL = string.Empty;
                strSQL = "backup database " + DBName + " to disk='" + backupDIR + "\\" + DBName + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".bak'";
                GlobalValues.ExecuteNonQuery(strSQL);
                lblError.Text = "Backup database successfully";
            }
            catch (Exception ex)
            {
                lblError.Text = "Error Occured During DB backup process !<br>" + ex.ToString();
            }
        }
    }
}