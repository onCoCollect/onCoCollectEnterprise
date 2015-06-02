using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ePxCollectWeb.MasterPage
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Login"]) == "")
            {
                //    NavigationMenu.Enabled = false;
                Response.Redirect("../login.Aspx");
            }
            lblUser.Text = Session["Login"].ToString();
            if (!IsPostBack)
            {
                if (GlobalValues.gUserType.ToUpper().Trim() == "SUPERADMIN")
                {
                    LoginAS.Visible = true;
                    string sqlStr = "Select [UserID] from HospitalUsers where userType='Admin'";
                    System.Data.DataSet dsLogins = GlobalValues.ExecuteDataSet(sqlStr);
                    System.Data.DataRow dr = dsLogins.Tables[0].NewRow();
                    dsLogins.Tables[0].Rows.InsertAt(dr, 0);
                    LoginAS.DataSource = dsLogins;
                    LoginAS.DataTextField = "FirstName";
                    LoginAS.DataValueField = "UserID";
                    LoginAS.DataBind();
                    if (Session["SuperAdmin"] != null)
                    {
                        string strLoginAs = Session["SuperAdmin"].ToString();
                        LoginAS.SelectedValue=strLoginAs;
                    }
                    lblImpersonate.Visible = true;
                }
                else
                {
                    Session.Remove("SuperAdmin");
                    lblImpersonate.Visible = false;
                    LoginAS.Visible = false;
                }
            }
        }

        protected void LoginAS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoginAS.SelectedValue != "")
            {
                Session["SuperAdmin"] = LoginAS.SelectedValue.ToString();                
                //Response.Redirect("SplashAdmin.aspx");
            }
            else
            {
                Session.Remove("SuperAdmin");
            }
            Response.Redirect(Request.Url.AbsoluteUri.ToString());
        }
    }
}