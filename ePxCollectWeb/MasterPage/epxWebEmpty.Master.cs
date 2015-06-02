using ePxCollectDataAccess;
using System;
using System.Data;

namespace ePxCollectWeb.MasterPage
{
    public partial class epxWebEmpty : System.Web.UI.MasterPage
    {
        string strConn = string.Empty;
        string strConns = GlobalValues.strConnString;
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
        protected void Page_Load(object sender, EventArgs e)
        {

            //User Rights and Permissions Loaded Here
            if (Session["FeatureSetPermission"] != null)
            {
                ObjfeatureSet = (FeatureSetPermission)Session["FeatureSetPermission"];
                Session["SessionId"] = ObjfeatureSet.IsstrSessionId;

            }

            //Check If the User Is Active User else Unlock Patient and do Forced Logout
            if (GlobalValues.BoolActiveUser(Session["Login"].ToString()))
            {
                GlobalValues.UnlockUser(Convert.ToString(Session["Login"]), Convert.ToString(Session["PatientID"]));
                GlobalValues.RemoveCurretLoginUsersDuringLogOut(Convert.ToString(Session["Login"]), GlobalValues.gEnterpriseApplicationName, GlobalValues.gLHSessionTimeOut, "Forced Logout during Session TimeOut.");

                Session["ResetPasswordForcedLogOut"] = null;
                Session["ResetPassword"] = null;
                Session["MutipleUserLogOut"] = null;
                Session["LiveUpdate"] = null;
                Session["ActiveUser"] = true;
                Response.Redirect("login.Aspx", true);
            }

            //Check If the User exists in the Login Users Table  else Unlock Patient and do Forced Logout
            if (!GlobalValues.FindCurrentUserExistsInLoginUsersTableBySession(Convert.ToString(Session["Login"]), Session["SessionId"].ToString()))
            {

                var strresult = GlobalValues.FindIPAddress(Convert.ToString(Session["Login"]), Session["SessionId"].ToString());
                GlobalValues.UnlockUser(Convert.ToString(Session["Login"]), Convert.ToString(Session["PatientID"]));
                Session["MutipleUserLogOut"] = Convert.ToString(strresult);
                //Check Live Update is in Progress  and do Forced Logout
                if (GlobalValues.BoolLiveUpdateProgress())
                {
                    Session["ResetPasswordForcedLogOut"] = null;
                    Session["ResetPassword"] = null;
                    Session["MutipleUserLogOut"] = null;
                    Session["LiveUpdate"] = true;
                }
                // Check whether the logged in User Password is Resetted and do Forced Logout
                else if (GlobalValues.BoolResetPassword(Convert.ToString(Session["Login"])))
                {
                    Session["ResetPasswordForcedLogOut"] = true;
                    Session["ResetPassword"] = null;
                    Session["MutipleUserLogOut"] = null;
                    Session["LiveUpdate"] = null;
                }
                Response.Redirect("login.Aspx");
            }
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }



            GlobalValues.UpdateLastActTimeCurrentUser(Session["Login"].ToString(), Session["SessionId"].ToString(), GlobalValues.gEnterpriseApplicationName);
            ul1.Visible = false;
            lblUser.Text = Convert.ToString(Session["UserName"]);

            if (Menu1.Items[0].ChildItems.Count > 0)
            {
                string pdSQL = "select * from FeatureSetUsers where UserID='" + Session["Login"].ToString() + "'";
                DataSet dset = SqlHelper.ExecuteDataset(strConns, CommandType.Text, pdSQL);
                if (dset.Tables.Count > 0 && dset.Tables[0].Rows.Count > 0)
                {
                    //Data Analysis
                    DataRow[] foundrow = dset.Tables[0].Select("FeatureSetName='Data Analysis' and Enabled=1");
                    if (foundrow.Length > 0)
                        Menu1.Items[0].Enabled = true;
                    else
                        Menu1.Items[0].Enabled = false;

                    //Patient Analysis
                    foundrow = dset.Tables[0].Select("FeatureSetName='Data Analysis' and Enabled=1");
                    if (foundrow.Length > 0)
                    {
                        Menu1.Items[0].ChildItems[0].Enabled = true;
                    }
                    else
                    {
                        Menu1.Items[0].ChildItems[0].Enabled = false;
                    }

                    //By Study
                    foundrow = dset.Tables[0].Select("FeatureSetName='Patient Analysis - By Study' and Enabled=1");
                    if (foundrow.Length > 0)
                    {
                        Menu1.Items[0].ChildItems[0].ChildItems[0].Enabled = true;
                    }
                    else
                    {
                        Menu1.Items[0].ChildItems[0].ChildItems[0].Enabled = false;
                    }


                    //By All Parametrs
                    foundrow = dset.Tables[0].Select("FeatureSetName='Patient Analysis - By all Parameters' and Enabled=1");
                    if (foundrow.Length > 0)
                    {
                        Menu1.Items[0].ChildItems[0].ChildItems[1].Enabled = true;
                    }
                    else
                    {
                        Menu1.Items[0].ChildItems[0].ChildItems[0].Enabled = false;
                    }

                    //Saved Query
                    foundrow = dset.Tables[0].Select("FeatureSetName='My Saved Queries'and Enabled=1");
                    if (foundrow.Length > 0)
                        Menu1.Items[0].ChildItems[1].Enabled = true;
                    else
                        Menu1.Items[0].ChildItems[1].Enabled = false;


                    //Status Check List
                    foundrow = dset.Tables[0].Select("FeatureSetName='Status Check-List' and Enabled=1");
                    if (foundrow.Length > 0)
                        Menu1.Items[0].ChildItems[2].Enabled = true;
                    else
                        Menu1.Items[0].ChildItems[2].Enabled = false;


                    //Drug Dosage
                    foundrow = dset.Tables[0].Select("FeatureSetName='Drug Dosage Analysis' and Enabled=1");
                    if (foundrow.Length > 0)
                        Menu1.Items[0].ChildItems[3].Enabled = true;
                    else
                        Menu1.Items[0].ChildItems[3].Enabled = false;
                }

                ObjfeatureSet.GetUserPermissions(Session["Login"].ToString(), ObjfeatureSet.IsstrSessionId);
                Session["FeatureSetPermission"] = ObjfeatureSet;

            }

            //version number update code goes here
            if (Session["coVersion"] == null)
            {
                Session["coVersion"] = GlobalValues.SystemVersionNumber();
                coVersion.InnerText = Session["coVersion"].ToString();
            }
            else
                coVersion.InnerText = Session["coVersion"].ToString();
        }
        protected void lnklogout_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Login"]) != "")
            {
                GlobalValues.UnlockUser(Convert.ToString(Session["Login"]), Convert.ToString(Session["PatientID"]));
            }
            GlobalValues.RemoveCurretLoginUsersDuringLogOut(Session["Login"].ToString(), GlobalValues.gEnterpriseApplicationName, GlobalValues.gLHLogOut, "User Logged Out");
            ViewState.Clear();
            Session.RemoveAll();
            Response.Redirect("Login.aspx");
        }


        protected void lnkLoginHistory_Click(object sender, EventArgs e)
        {
            if (Session["ResetPassword"] != null)
            {
                Session["ResetPasswordMsg"] = "Please Change your password.";
                Response.Redirect("Changepassword.aspx");
            }
            else
            {
                BindGridAudit();
                ModalPopupExtender2.Show();
            }
        }

        public void BindGridAudit()
        {
            try
            {

                DataSet ds = GlobalValues.ExecuteDataSetAudit("select TOP 5 * from Audit_LoginHistory where UserID='" + Session["Login"].ToString() + "'  order by LogTime desc");
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

        protected void lnkErrorReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("ErrorReport.aspx", false);
        }
    }
}
