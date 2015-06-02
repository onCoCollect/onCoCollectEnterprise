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
    public partial class ChangePassword : System.Web.UI.Page
    {

        #region Declaration
        string strConns = GlobalValues.strConnString;
        static DataSet dsChangePassword = new DataSet();
        static string message = string.Empty;
        static OncoEncrypt.OncoEncrypt objEncryptDecrypt = new OncoEncrypt.OncoEncrypt();
        static string loginUserID = string.Empty;
        static string prevPage = string.Empty;
        static string password = string.Empty;
        static string FirstName = "", MiddleName = "", LastName = "", Email = "", Phone = "", Extension = "", HintQuestion = "", Answer = "", Passwords = "";
        #endregion

        #region Load Events

        protected void Page_Load(object sender, EventArgs e)
        {



            txtCurrentPassword.Attributes["value"] = txtCurrentPassword.Text;
            txtConfirmPassword.Attributes["value"] = txtConfirmPassword.Text;
            txtNewPassword.Attributes["value"] = txtNewPassword.Text;
            txtCurrentPassword.Attributes["type"] = "password";
            txtNewPassword.Attributes["type"] = "password";
            txtConfirmPassword.Attributes["type"] = "password";
            if (!Page.IsPostBack)
            {
                txtCurrentPassword.Focus();

                //prevPage = Request.UrlReferrer.ToString();
                if (Session["Login"] != null)
                {
                    loginUserID = Session["Login"].ToString().Trim();
                    BindPasswordHintQuestions();
                    GetUsersForChangePassword(loginUserID);
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }

            if (Session["ResetPasswordMsg"] != null)
            {
                lblmsg.Text = "Your password has been resetted by Administrator. Please change your Password.";
            }

        }

        #endregion

        #region Events

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string currentPassword = txtCurrentPassword.Text.Trim();
                string checkPassword = objEncryptDecrypt.Decrypt(password.Trim());
                if (String.Equals(currentPassword, checkPassword, StringComparison.Ordinal) == true)
                {
                    string message = getUpdatedMessage();
                    SqlHelper.ExecuteNonQuery(strConns, CommandType.Text, "update [HospitalUsers] set [Password] = '" + objEncryptDecrypt.Encrypt(txtNewPassword.Text.Trim()) + "', PasswordHintQ='" + ddlPasswordHintQuestion.SelectedItem.Text.ToString().Trim() + "' ,  PasswordHintA ='" + txtAnswer.Text.Trim() + "',[EmailID]='" + txtEmail.Text.Trim() + "',MobileNumber ='" + txtPhone.Text.Trim() + "', Extension = '" + txtExtension.Text.ToString() + "',ResetPassword=0     where [Password] ='" + objEncryptDecrypt.Encrypt(txtCurrentPassword.Text.Trim()) + "' and [UserID] = '" + loginUserID + "' ");
                    lblmsg.ForeColor = GlobalValues.SucessColor;
                    lblmsg.Text = "Password changed successfully.";
                    lblmsg.Focus();
                    ClearControls();
                    GlobalValues.UnlockUser(Convert.ToString(Session["Login"]), Convert.ToString(Session["PatientID"]));
                  
                    if (message == "")
                    {
                        Session["ChangePassword"] = "Password changed successfully. You will be redirected to login Page.";
                        GlobalValues.RemoveCurretLoginUsersDuringLogOut(Session["Login"].ToString(), GlobalValues.gEnterpriseApplicationName, GlobalValues.gLHchangePassowrd, "Log Out during Password Change.");

                    }
                    else
                    {
                        Session["ChangePassword"] = message + " You will be redirected to login Page.";
                        GlobalValues.RemoveCurretLoginUsersDuringLogOut(Session["Login"].ToString(), GlobalValues.gEnterpriseApplicationName, GlobalValues.gLHchangePassowrd, message);

                    }

                    Session.Remove("PatientID");
                    Session.Remove("Login");
                    Session.Remove("ResetPassword");
                    //Response.Redirect("login.aspx", false);
                    ScriptManager.RegisterStartupScript(this, GetType(), "checkRedirectPage", "checkRedirectPage('" + Session["ChangePassword"].ToString() + "');", true);
                }
                else
                {
                    lblmsg.ForeColor = GlobalValues.FailureColor;
                    lblmsg.Text = "Current password is incorrect.";
                    txtCurrentPassword.Attributes["value"] = string.Empty;
                    txtCurrentPassword.Text = string.Empty;
                    txtCurrentPassword.Focus();
                }

            }
            catch (Exception ex)
            {


            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearControls();
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtExtension.Text = string.Empty;
            ddlPasswordHintQuestion.SelectedIndex = -1;
            txtAnswer.Text = string.Empty;
        }

        #endregion

        #region Private Methods

        void ClearControls()
        {
            txtCurrentPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            //UpdatePanel1.Update();
            //txtFirstName.Text = string.Empty;
            //txtMiddleName.Text = string.Empty;
            //txtLastName.Text = string.Empty;
            // txtEmail.Text = string.Empty;
            //txtPhone.Text = string.Empty;
            //txtExtension.Text = string.Empty;
            //ddlPasswordHintQuestion.SelectedIndex = 0;
            //txtAnswer.Text = string.Empty;
            txtNewPassword.Attributes["value"] = string.Empty;
            txtConfirmPassword.Attributes["value"] = string.Empty;
            txtCurrentPassword.Attributes["value"] = string.Empty;
        }

        public void BindPasswordHintQuestions()
        {
            dsChangePassword = SqlHelper.ExecuteDataset(strConns, CommandType.Text, "Select '' as  PasswordHintQ  union select  PasswordHintQ from [PasswordHintQuestion]");
            if (dsChangePassword != null && dsChangePassword.Tables.Count > 0 && dsChangePassword.Tables[0].Rows.Count > 0)
            {
                //Populate hospital for search
                ddlPasswordHintQuestion.DataSource = dsChangePassword;
                ddlPasswordHintQuestion.DataTextField = "PasswordHintQ";
                ddlPasswordHintQuestion.DataValueField = "PasswordHintQ";
                ddlPasswordHintQuestion.DataBind();
                //ddlPasswordHintQuestion.Items.Insert(0, "");
            }
        }
        #endregion

        protected void GetUsersForChangePassword(string loginUserID)
        {
            string strsql = "select [FirstName],[MiddleName],[LastName],Active,	[EmailID],MobileNumber , Extension ,[Password],PasswordHintQ, PasswordHintA  from [HospitalUsers]   where [UserID] ='" + loginUserID + "'";
            dsChangePassword = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strsql);
            if (dsChangePassword != null && dsChangePassword.Tables.Count > 0 && dsChangePassword.Tables[0].Rows.Count > 0)
            {
                txtFirstName.Text = dsChangePassword.Tables[0].Rows[0]["FirstName"].ToString();
                FirstName = txtFirstName.Text.Trim();
                txtMiddleName.Text = dsChangePassword.Tables[0].Rows[0]["MiddleName"].ToString();
                MiddleName = txtMiddleName.Text.Trim();
                txtLastName.Text = dsChangePassword.Tables[0].Rows[0]["LastName"].ToString();
                LastName = txtLastName.Text.Trim();
                txtEmail.Text = dsChangePassword.Tables[0].Rows[0]["EmailID"].ToString();
                Email = txtEmail.Text.Trim();
                txtPhone.Text = dsChangePassword.Tables[0].Rows[0]["MobileNumber"].ToString();
                Phone = txtPhone.Text.Trim();
                txtExtension.Text = dsChangePassword.Tables[0].Rows[0]["Extension"].ToString();
                Extension = txtExtension.Text.Trim();
                password = dsChangePassword.Tables[0].Rows[0]["Password"].ToString();
                ddlPasswordHintQuestion.SelectedValue = dsChangePassword.Tables[0].Rows[0]["PasswordHintQ"].ToString();
                HintQuestion = ddlPasswordHintQuestion.SelectedValue.ToString();
                txtAnswer.Text = dsChangePassword.Tables[0].Rows[0]["PasswordHintA"].ToString();
                Answer = txtAnswer.Text.Trim();
                txtFirstName.Enabled = false;
                txtMiddleName.Enabled = false;
                txtLastName.Enabled = false;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProjectForm.aspx");
        }

        protected void btnChangePasswordHistory_Click(object sender, EventArgs e)
        {
            BindGridAudit();
            ModalPopupExtender2.Show();
        }

        public void BindGridAudit()
        {
            try
            {

                DataSet ds = GlobalValues.ExecuteDataSetAudit("select LogTime,Action,Comments=case when Comments='Log Out during Password Change.' then 'Password Changed.' else comments end  from Audit_LoginHistory where Action='" + GlobalValues.gLHchangePassowrd + "'  order by LogTime desc");
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GridLoginAudit.DataSource = ds;
                        GridLoginAudit.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
        }
        public string getUpdatedMessage()
        {
            int counts = 0;
            string PasswordMessage = "Password got updated successfully with changes in ";
            if (FirstName != txtFirstName.Text.Trim())
            {
                PasswordMessage += "FirstName,";
                counts = counts + 1;
            }
            if (MiddleName != txtMiddleName.Text.Trim())
            {
                PasswordMessage += "MiddleName,";
                counts = counts + 1;
            }
            if (LastName != txtLastName.Text.Trim())
            {
                PasswordMessage += "LastName,";
                counts = counts + 1;
            }
            if (Email != txtEmail.Text.Trim())
            {
                PasswordMessage += "Email,";
                counts = counts + 1;
            }
            if (Phone != txtPhone.Text.Trim())
            {
                PasswordMessage += "Phone,";
                counts = counts + 1;
            }
            if (Extension != txtExtension.Text.Trim())
            {
                PasswordMessage += "Phone Extension,";
                counts = counts + 1;
            }
            if (HintQuestion != ddlPasswordHintQuestion.Text)
            {
                PasswordMessage += "Hint Question,";
                counts = counts + 1;
            }
            if (Answer != txtAnswer.Text.Trim())
            {
                PasswordMessage += "Hint Answer,";
                counts = counts + 1;
            }
            if (counts > 1)
            {
                PasswordMessage = PasswordMessage.TrimEnd(',');
                int i = PasswordMessage.LastIndexOf(',');
                PasswordMessage = PasswordMessage.Substring(0, i) + " and " + PasswordMessage.Substring(i + 1) + ".";
            }
            else if (counts == 1)
            {
                PasswordMessage = PasswordMessage.TrimEnd(',') + ".";
            }
            else if (counts == 0)
            {
                PasswordMessage = "";
            }
            return PasswordMessage;
        }
    }
}