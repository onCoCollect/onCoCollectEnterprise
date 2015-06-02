using ePxCollectDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ePxCollectWeb
{
    public partial class ForgotPassword : System.Web.UI.Page
    {

        #region Declaration
        string strConns = GlobalValues.strConnString;
        static DataSet dsChangePassword = new DataSet();
        static string message = string.Empty;
        static OncoEncrypt.OncoEncrypt objEncryptDecrypt = new OncoEncrypt.OncoEncrypt();
        static string loginUserID = string.Empty;
        static string prevPage = string.Empty;
        static string password = string.Empty;

        #endregion

        #region Load Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.UrlReferrer.ToString().IndexOf("login.aspx") != -1)
                {
                    Response.Redirect("Login.aspx", false);
                }
                BindPasswordHintQuestions();

            }
        }

        #endregion

        #region Events

        public void BindPasswordHintQuestions()
        {
            dsChangePassword = SqlHelper.ExecuteDataset(strConns, CommandType.Text, "select *  from [PasswordHintQuestion]");
            if (dsChangePassword != null && dsChangePassword.Tables.Count > 0 && dsChangePassword.Tables[0].Rows.Count > 0)
            {
                //Populate hospital for search
                ddlPasswordHintQuestion.DataSource = dsChangePassword;
                ddlPasswordHintQuestion.DataTextField = "PasswordHintQ";
                ddlPasswordHintQuestion.DataValueField = "PasswordHintQ";
                ddlPasswordHintQuestion.DataBind();
                ddlPasswordHintQuestion.Items.Insert(0, "");
            }
        }

        protected string GetUsersForChangePassword(string loginUserID)
        {
            string strmsg = "";
            string strsql = " select PasswordHintQ,PasswordHintA,UserType from HospitalUsers  where [UserID] ='" + loginUserID + "'";
            dsChangePassword = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strsql);
            if (dsChangePassword != null && dsChangePassword.Tables.Count > 0 && dsChangePassword.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsChangePassword.Tables[0].Rows[0]["UserType"].ToString()) && (dsChangePassword.Tables[0].Rows[0]["UserType"].ToString() == "Admin" || dsChangePassword.Tables[0].Rows[0]["UserType"].ToString() == "Super Admin"))
                {
                    strmsg = "Invalid UserID.";
                    return strmsg;
                }
                if (ddlPasswordHintQuestion.SelectedItem.Text != dsChangePassword.Tables[0].Rows[0]["PasswordHintQ"].ToString())
                    strmsg = "Invalid User Credentials.";

                if (txtAnswer.Text.Trim() != dsChangePassword.Tables[0].Rows[0]["PasswordHintA"].ToString())
                    strmsg = "Invalid User Credentials.";
            }
            else
            {
                strmsg = "Invalid UserID.";
            }
            return strmsg;
        }

        protected void btnNewPassword_Click(object sender, EventArgs e)
        {
            if (divChangePassword.Visible == true)
            {
                if (txtConfirmPassword.Text.Trim() != string.Empty && txtNewPassword.Text.Trim() != string.Empty)
                {
                    if (txtConfirmPassword.Text.Trim().ToLower() != txtNewPassword.Text.Trim().ToLower())
                    {
                        txtUserId.Enabled = false;
                        ddlPasswordHintQuestion.Enabled = false;
                        txtAnswer.Enabled = false;
                        lblMessage.Text = "New password should match with confirm password.";
                        return;
                    }
                    //SqlHelper.ExecuteNonQuery(strConns, CommandType.Text, "update [HospitalUsers] set [Password] = '" + objEncryptDecrypt.Encrypt(txtNewPassword.Text.Trim()) + "' where [UserID] = '" + hdnUserId.Value + "' ");
                    GlobalValues.ForgotPassword(objEncryptDecrypt.Encrypt(txtNewPassword.Text.Trim()), hdnUserId.Value);

                    //lblmsg.Text = "Password changed successfully.";
                    //lblmsg.Focus();
                    ClearControls();
                    GlobalValues.SaveSessionLog("", hdnUserId.Value, GlobalValues.SLogFPwd);
                    ScriptManager.RegisterStartupScript(this, GetType(), "checkRedirectPage", "checkRedirectPage();", true);
                    //Response.Redirect("login.aspx");
                    //                string close = @"<script type='text/javascript'> 
                    //                             alert(Redirected to Login Page.) </script>";

                    //                base.Response.Write(close);
                }
                else
                { lblMessage.Text = "Fields marked with asterisk (*) are required."; }
            }
            else
            {
                if (txtUserId.Text.Trim() != string.Empty && ddlPasswordHintQuestion.SelectedItem.Text != string.Empty && txtAnswer.Text.Trim() != string.Empty)
                {
                    string strmessage = GetUsersForChangePassword(txtUserId.Text.Trim());
                    if (strmessage == "")
                    {
                        if (GlobalValues.BoolResetPassword(txtUserId.Text.Trim()))
                        {
                            lblMessage.Text = "Your password has been reseted by Administrator.Please contact your Administrator.";
                            //ModalPopupExtender2.Hide();
                            this.divChangePassword.Visible = false;
                        }
                        else
                        {
                            hdnUserId.Value = txtUserId.Text.Trim();
                            lblMessage.Text = "";
                            txtUserId.Enabled = false;
                            ddlPasswordHintQuestion.Enabled = false;
                            txtAnswer.Enabled = false;
                            btnNewPassword.Enabled = true;
                            btncancel.Enabled = true;
                            //Code added by Subhashini March 17,2015
                            this.divChangePassword.Visible = true;
                            divHeight.Attributes.Add("style", "height: 205px;");
                            // ModalPopupExtender2.Show();
                        }
                    }
                    else
                    {
                        lblMessage.Text = strmessage;
                    }
                }
                else
                { lblMessage.Text = "Fields marked with asterisk (*) are required."; }
            }
        }


        #endregion


        protected void btncancel_Click(object sender, EventArgs e)
        {
            if (divChangePassword.Visible == true)
            {
                divChangePassword.Visible = false;
                txtUserId.Enabled = true;
                ddlPasswordHintQuestion.Enabled = true;
                txtAnswer.Enabled = true;
                divHeight.Attributes.Add("style", "height: 270px;");
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        private void ClearControls()
        {
            txtConfirmPassword.Text = "";
            txtNewPassword.Text = "";
            ddlPasswordHintQuestion.SelectedIndex = 0;
            txtUserId.Text = "";
            txtAnswer.Text = "";
        }

        //protected void btnNo_Click(object sender, EventArgs e)
        //{
        //    txtUserId.Enabled = true;//Code added on March 17,2015
        //    txtConfirmPassword.Text = "";//Code added on March 17,2015
        //    txtNewPassword.Text = "";
        //    ddlPasswordHintQuestion.Enabled = true;
        //    txtAnswer.Enabled = true;
        //    btnNewPassword.Enabled = true;
        //    btncancel.Enabled = true;
        //    txtAnswer.Enabled = true;
        //    txtUserId.Focus();//Code added on March 17,2015
        //}
    }
}