using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ePxCollectDataAccess;

namespace ePxCollectWeb
{
    public partial class LoginPage : System.Web.UI.Page
    {
        static string strConns;


        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet dsLogin = new DataSet();
            //.ConfigurationManager.ConnectionStrings["ePxConnString"].ConnectionString;
            Session.RemoveAll();
            strConns = GlobalValues.strConnString;
            //strConns = GlobalValues.strConnString;
            dsLogin = SqlHelper.ExecuteDataset(strConns, CommandType.Text, "Select * from Users");
            cboUserName.DataSource = dsLogin.Tables[0].DefaultView.ToTable();
            cboUserName.DataMember = "User_Name";
            cboUserName.DataValueField = "User_Name";
            cboUserName.DataBind();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string strSQL = "Select * From Users where User_Name like '" + cboUserName.Text +"'";
            OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
            DataSet dsUsers = SqlHelper.ExecuteDataset(strConns, System.Data.CommandType.Text, strSQL);
            string strPwd = dsUsers.Tables[0].Rows[0]["Password"].ToString();
            if (txtPassword.Text == objEnc.Decrypt(strPwd))
            {
                Session["UserName"] = cboUserName.SelectedItem.Text;
                Response.Redirect("SearchPatient.aspx");
            }
            else
            {
                Label3.Text = "Incorrect Password, please try again";
                Label3.Visible = true;
            }
        }
    }
}