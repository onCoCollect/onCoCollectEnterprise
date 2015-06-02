using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ePxCollectWeb
{
    public partial class Disclaimer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }
            this.Title = "... Disclaimer ...";
            string strDisc = Convert.ToString(GlobalValues.ExecuteScalar("Select isnull(Disclaimer,'') Disclaimer From Disclaimer"));
            if (strDisc == null) { strDisc = ""; }
            strDisc = strDisc.Replace("Onco Clinique", "Onco Clinique(TM)").Replace("OncoClinique", "Onco Clinique(TM)");
            TextBox1.Text = strDisc;
        }

        protected void Agree_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchPatient.aspx");
        }

        protected void DisAgree_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}