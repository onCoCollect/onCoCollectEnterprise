﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Services;

namespace ePxCollectWeb
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        static string done = "x";
        static object objLock = new object();
        string strConn = string.Empty;
        string strConns = GlobalValues.strConnString;
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();

        protected void Page_Init(object sender, EventArgs e)
        {
            // Code for enabling Menu Control to work in Google Chrome Browser
            // This is necessary because Safari and Chrome browsers don't display the Menu control correctly.
            // All webpages displaying an ASP.NET menu control must inherit this class.
            if (Request.ServerVariables["http_user_agent"].IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1)
                Page.ClientTarget = "uplevel";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UserAgent.IndexOf("AppleWebKit") > 0)
            {
                Request.Browser.Adapters.Clear();
            }

            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
            Response.Expires = 0;
            Response.CacheControl = "no-Cache";

            Response.Expires = 0;
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetAllowResponseInBrowserHistory(false);

            if (Session["FeatureSetPermission"] != null)
            {
                ObjfeatureSet = (FeatureSetPermission)Session["FeatureSetPermission"];
                Session["SessionId"] = ObjfeatureSet.IsstrSessionId;
            }
            ModalPopupExtender2.Hide();

            //if (Request.UrlReferrer == null)
            //{
            //    Response.Redirect("login.aspx");
            //}
            strConn = GlobalValues.strConnString;

            if (Convert.ToString(Session["Login"]) == "")
            {
                //    NavigationMenu.Enabled = false;
                Response.Redirect("login.Aspx");
            }
            if (GlobalValues.BoolActiveUser(Session["Login"].ToString()))
            {
                Session["ResetPasswordForcedLogOut"] = null;
                Session["ResetPassword"] = null;
                Session["MutipleUserLogOut"] = null;
                Session["LiveUpdate"] = null;
                Session["ActiveUser"] = true;
                Response.Redirect("login.Aspx", true);
            }


            if (!GlobalValues.FindCurrentUserExistsInLoginUsersTableBySession(Convert.ToString(Session["Login"]), Session["SessionId"].ToString()))
            {
                var strresult = GlobalValues.FindIPAddress(Convert.ToString(Session["Login"]), Session["SessionId"].ToString());
                GlobalValues.UnlockUser(Convert.ToString(Session["Login"]), Convert.ToString(Session["PatienDetails"]));
                Session["MutipleUserLogOut"] = Convert.ToString(strresult);
                if (GlobalValues.BoolLiveUpdateProgress())
                {
                    Session["ResetPasswordForcedLogOut"] = null;
                    Session["ResetPassword"] = null;
                    Session["MutipleUserLogOut"] = null;
                    Session["LiveUpdate"] = true;
                }
                else if (GlobalValues.BoolResetPassword(Convert.ToString(Session["Login"])))
                {
                    Session["ResetPasswordForcedLogOut"] = true;
                    Session["ResetPassword"] = null;
                    Session["MutipleUserLogOut"] = null;
                    Session["LiveUpdate"] = null;
                }

                Response.Redirect("login.Aspx",true);

            }
            if (GlobalValues.BoolResetPassword(Convert.ToString(Session["Login"])))
            {
                Session["ResetPassword"] = "ResetPassword";
            }

            lblUser.Text = Convert.ToString(Session["UserName"]);
            GlobalValues.UpdateLastActTimeCurrentUser(Session["Login"].ToString(), Session["SessionId"].ToString(), GlobalValues.gEnterpriseApplicationName);

            if (Session["PatienDetails"] != null)
            {
                lblPatientDetails.Text = "Patient Details: " + Convert.ToString(Session["PatienDetails"]);
            }
            else
            {
                lblPatientDetails.Text = "";
            }




            //Setting the buttons based on the userpermissions            
            BtnReg.Visible = ObjfeatureSet.isRegistration;
            toxicity.Visible = ObjfeatureSet.isToxicity;
            Recurrence.Visible = ObjfeatureSet.isRecurrence;
            Status.Visible = ObjfeatureSet.isStatus;
            investigation.Visible = ObjfeatureSet.isInvestigations;
            Metasis.Visible = ObjfeatureSet.isSitesOfMetsis;


            for (int i = 0; i <= rdMenuType.Items.Count - 1; i++)
            {
                if (rdMenuType.Items[i].Value == "ByDiagnosis" && ObjfeatureSet.isPresetForms == false) { rdMenuType.Items.Remove(rdMenuType.Items[i]); i = 0; }
                if (rdMenuType.Items[i].Value == "ByStudy" && ObjfeatureSet.isStudyForms == false) { rdMenuType.Items.Remove(rdMenuType.Items[i]); i = 0; }
                if (rdMenuType.Items[i].Value == "Custom" && ObjfeatureSet.isCustomScreen == false) { rdMenuType.Items.Remove(rdMenuType.Items[i]); i = 0; }
            }

            // commented by THiru on 12 May, 2015
            //if (Menu1.Items[1].ChildItems.Count > 0)
            //{
            //    if (ObjfeatureSet.isAnalysis) { Menu1.Items[1].Enabled = true; } else { Menu1.Items[1].Enabled = false; }
            //    if (ObjfeatureSet.isAnalysis) { Menu1.Items[1].ChildItems[0].Enabled = true; } else { Menu1.Items[1].ChildItems[0].Enabled = false; }
            //    if (ObjfeatureSet.isAnalysis)
            //    {
            //        if (ObjfeatureSet.isAnalysisByStudy) { Menu1.Items[1].ChildItems[0].ChildItems[0].Enabled = true; } else { Menu1.Items[1].ChildItems[0].ChildItems[0].Enabled = false; }
            //        if (ObjfeatureSet.isAnalysisByAllparams) { Menu1.Items[1].ChildItems[0].ChildItems[1].Enabled = true; } else { Menu1.Items[1].ChildItems[0].ChildItems[0].Enabled = false; }

            //        if (ObjfeatureSet.isAnalysisByAllparams) { Menu1.Items[1].ChildItems[1].Enabled = true; } else { Menu1.Items[1].ChildItems[1].Enabled = false; }
            //        if (ObjfeatureSet.isStatusCheckList) { Menu1.Items[1].ChildItems[2].Enabled = true; } else { Menu1.Items[1].ChildItems[2].Enabled = false; }
            //    }
            //}

            if (ObjfeatureSet.isAnalysis == false)
            {
                //Menu1.Items[1].Text = "";
                //Menu1.Items[1].ChildItems.Clear();
                //Menu1.Items[1].Selectable = false;
            }
            if (ObjfeatureSet.isBottomNav == false) { BottomNav.Visible = false; }
            if (Session["gMenuType"] == null)
                Session["gMenuType"] = "";
            if (Session["gMenuType"].ToString() == "")
            {
                //if (ObjfeatureSet.isCustomScreen) { GlobalValues.gMenuType = "Custom"; }
                //if (ObjfeatureSet.isStudyForms) { GlobalValues.gMenuType = "ByStudy"; }
                //if (ObjfeatureSet.isPresetForms) { GlobalValues.gMenuType = "ByDiagnosis"; }


                if (ObjfeatureSet.isCustomScreen) { Session["gMenuType"] = "Custom"; }
                if (ObjfeatureSet.isStudyForms) { Session["gMenuType"] = "ByStudy"; }
                if (ObjfeatureSet.isPresetForms) { Session["gMenuType"] = "ByDiagnosis"; }

            }
            if (ObjfeatureSet.isSearch)
            {
                lnkpickpatient.Enabled = true;
            }
            else
                lnkpickpatient.Enabled = false;

            //till here - buttons settings based on userpermissions
            if (!IsPostBack)
            {

                //if (Convert.ToString(Session["PatientID"]) != "")
                {
                    //rdMenuType.SelectedValue = "ByDiagnosis";
                    Session["MenuChange"] = "No";
                    rdMenuType.SelectedValue = Session["gMenuType"].ToString();//GlobalValues.gMenuType.ToString();
                    Session["rdMenuType"] = true;
                    rdMenuType_SelectedIndexChanged(null, null);
                    Session["rdMenuType"] = null;
                    if (dpMenuItems.Items.Count > 0)
                    {
                        dpMenuItems.SelectedValue = Session["gMenuDropVal"].ToString();// GlobalValues.gMenuDropVal.ToString();
                        dpMenuItems_SelectedIndexChanged(null, null);
                    }
                }
            }
            else
            {
                string SName = string.Empty;
                SName = Convert.ToString(Request.QueryString["SN"]);

                //dpMenuItems.SelectedValue = GlobalValues.gMenuDropVal.ToString();
                //dpMenuItems_SelectedIndexChanged(null, null);
                if (dpMenuItems.SelectedItem != null)
                {
                    //GlobalValues.gMenuDropVal = dpMenuItems.SelectedItem.Text.ToString();
                    Session["gMenuDropVal"] = dpMenuItems.SelectedItem.Text.ToString();
                }
                Session["rdMenuType"] = true;
                rdMenuType_SelectedIndexChanged(null, null);
                Session["rdMenuType"] = null;
                if (dpMenuItems.Items.Count > 0)
                {
                    dpMenuItems.SelectedValue = Session["gMenuDropVal"].ToString();// GlobalValues.gMenuDropVal.ToString();
                    dpMenuItems_SelectedIndexChanged(null, null);
                }

                Session["MenuChange"] = "Yes";
            }
            //ContentPlaceHolder content = this.Page.Master.FindControl("MainContent") as ContentPlaceHolder;

            AuthorizationOfPages();
            BlockUserToSaveTheChanges();
            if (ObjfeatureSet.isSearch) { } else { lblPatientDetails.Text = ""; }
            CheckSessionTimeout();


        }

        private void CheckSessionTimeout()
        {
            string msgSession = "Warning: Within next 1 minutes, if you do not do anything our system will redirect to the login page. Please save changed data.";
            //time to remind, 1 minutes before session ends
            int int_MilliSecondsTimeReminder = (this.Session.Timeout * 60000) - 90000;
            //time to redirect, 30000 milliseconds before session ends
            int int_MilliSecondsTimeOut = (this.Session.Timeout * 60000) - 10000;

            string str_Script = @"
var myTimeReminder, myTimeOut;
clearTimeout(myTimeReminder);
clearTimeout(myTimeOut); " +
            "var sessionTimeReminder = " +
            int_MilliSecondsTimeReminder.ToString() + "; " +
            "var sessionTimeout = " + int_MilliSecondsTimeOut.ToString() + ";" +
            "function doRedirect(){ window.location='login.aspx'; }" + @"
myTimeOut=setTimeout('doRedirect()', sessionTimeout); ";



            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
            "CheckSessionOut", str_Script, true);
        }



        /* private void PopulateMainMenu_New()
         {
             //try
             //{
             //    DataSet dsMainMenu = new DataSet();
             //    //string strSql = "select M.MenuID, MenuDescription,TargetURL, M.ISACtive, [ReadOnly], Isnull(SubMenu,'') SubMenu , ParentID from tblMenuItems M inner join tblUserRights R on M.MenuID=R.MenuId and (m.IsActive=1 and R.IsActive=1) and UserGroupID=1";
             //    string strSql = "select M.MenuID, MenuDescription,TargetURL, M.ISACtive, [ReadOnly], Isnull(SubMenu,'') SubMenu , ParentID from tblMenuItems M inner join tblUserRights R on M.MenuID=R.MenuId and (m.IsActive=1 and R.IsActive=1) and  isnull(ParentID,'')='' and UserGroupID=1";
             //    dsMainMenu = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSql);
             //    int iPos = 0;
             //    foreach (DataRow dr in dsMainMenu.Tables[0].Rows)
             //    {
             //        HtmlGenericControl div1 = new HtmlGenericControl("DIV");
             //        LinkButton lBtn = new LinkButton();
             //        string strURL = dr["TargetURL"].ToString();
             //        strURL += (strURL.Contains("?") ? "&" + "ReadOnly=" + dr["ReadOnly"].ToString() : "");
             //        div1.Attributes.Add("OnClick", strURL);
             //        //lBtn.OnClientClick = "javascript:window.location.href('" + strURL + "'); return false;";
             //        //lBtn("OnClick","javascript:window.location.href('" + dr["TargetURL"].ToString() + "&ReadOnly=" +dr["ReadOnly"].ToString()+  "'); return false;'");
             //        iPos = phMenus.Controls.Count;
             //        if (dr["SubMenu"].ToString() == "False" || dr["SubMenu"].ToString() != "")
             //        {
             //            //lBtn.CssClass = "MainLeftItem";
             //            div1.Attributes.Add("class", "MainLeftItem");
             //        }
             //        else
             //        {
             //            LinkButton t = new LinkButton();
             //            foreach (Control c in phMenus.Controls)
             //            {
             //                t = (LinkButton)c;
             //                if (t.ID == dr["ParentID"].ToString())
             //                {
             //                    iPos = phMenus.Controls.IndexOf(t);
             //                }
             //            }
             //            //lBtn.CssClass = "IndentItem";
             //            div1.Attributes.Add("class", "MainLeftItem");
             //        }
             //        div1.ID = dr["MenuID"].ToString();
             //        //lBtn.ID = dr["MenuID"].ToString();
             //        lBtn.Text = dr["MenuDescription"].ToString();
             //        phMenus.Controls.AddAt(iPos,div1);
             //    }
             //}
             //catch (Exception)
             //{

             //    throw;
             //}
             try
             {
                 DataSet dsMainMenu = new DataSet();
                 //string strSql = "select M.MenuID, MenuDescription,TargetURL, M.ISACtive, [ReadOnly], Isnull(SubMenu,'') SubMenu , ParentID from tblMenuItems M inner join tblUserRights R on M.MenuID=R.MenuId and (m.IsActive=1 and R.IsActive=1) and UserGroupID=1";
                 string strSql = "select M.MenuID, MenuDescription,TargetURL, M.ISACtive, [ReadOnly], Isnull(SubMenu,'') SubMenu , ParentID from tblMenuItems M inner join tblUserRights R on M.MenuID=R.MenuId and (m.IsActive=1 and R.IsActive=1) and  isnull(ParentID,'')='' and UserGroupID=1";
                 dsMainMenu = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSql);
                 int iPos = 0;
                 foreach (DataRow dr in dsMainMenu.Tables[0].Rows)
                 {
                     LinkButton lBtn = new LinkButton();
                     string strURL = dr["TargetURL"].ToString();
                     strURL += (strURL.Contains("?") ? "&" + "RO=" + dr["ReadOnly"].ToString() : "");
                     lBtn.OnClientClick = "javascript:window.location.href('" + strURL + "'); return false;";
                     //lBtn("OnClick","javascript:window.location.href('" + dr["TargetURL"].ToString() + "&ReadOnly=" +dr["ReadOnly"].ToString()+  "'); return false;'");
                     iPos = phMenus.Controls.Count;
                     if (dr["SubMenu"].ToString() == "False" || dr["SubMenu"].ToString() != "")
                     {
                         lBtn.CssClass = "MainLeftItem";
                     }
                     else
                     {
                         LinkButton t = new LinkButton();
                         foreach (Control c in phMenus.Controls)
                         {
                             t = (LinkButton)c;
                             if (t.ID == dr["ParentID"].ToString())
                             {
                                 iPos = phMenus.Controls.IndexOf(t);
                             }
                         }
                         lBtn.CssClass = "IndentItem";
                     }

                     lBtn.ID = dr["MenuID"].ToString();
                     lBtn.Text = dr["MenuDescription"].ToString();
                     phMenus.Controls.AddAt(iPos, lBtn);
                 }
             }
             catch (Exception)
             {

                 throw;
             }
         }

         private void PopulateMainMenu_withTblMenu()
         {
             try
             {
                 DataSet dsMainMenu = new DataSet();
                 //string strSql = "select M.MenuID, MenuDescription,TargetURL, M.ISACtive, [ReadOnly], Isnull(SubMenu,'') SubMenu , ParentID from tblMenuItems M inner join tblUserRights R on M.MenuID=R.MenuId and (m.IsActive=1 and R.IsActive=1) and UserGroupID=1";
                 string strSql = "select M.MenuID, MenuDescription,TargetURL, M.ISACtive, [ReadOnly], Isnull(SubMenu,'') SubMenu , ParentID, DivName from tblMenuItems M inner join tblUserRights R on M.MenuID=R.MenuId and (m.IsActive=1 and R.IsActive=1) and  isnull(ParentID,'')='' and UserGroupID=1 order by M.MenuID";
                 dsMainMenu = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSql);
                 int iPos = 0;
                 foreach (DataRow dr in dsMainMenu.Tables[0].Rows)
                 {
                     LinkButton lBtn = new LinkButton();
                     string strURL = dr["TargetURL"].ToString();
                     strURL += (strURL.Contains("?") ? "&" + "RO=" + HttpUtility.UrlEncode(dr["ReadOnly"].ToString()) : "");
                     lBtn.OnClientClick = "javascript:window.location.href='" + strURL + "'; return false;";
                     //lBtn("OnClick","javascript:window.location.href('" + dr["TargetURL"].ToString() + "&ReadOnly=" +dr["ReadOnly"].ToString()+  "'); return false;'");
                     iPos = phMenus.Controls.Count;
                     if (dr["SubMenu"].ToString() == "False")
                     {
                         lBtn.CssClass = "MainLeftItem";
                         lBtn.ID = dr["MenuID"].ToString();
                         lBtn.Text = dr["MenuDescription"].ToString();
                         phMenus.Controls.Add(lBtn);
                     }
                     else
                     {
                         DataSet dsSubItems = new DataSet();
                         if (dr["DivName"].ToString() != "divTreatment")
                         {
                             lBtn.CssClass = "MainLeftItem";
                             lBtn.OnClientClick = "javascript:window.location.href='ProjectForm.aspx?Type=" + dr["DivName"].ToString() + "'; return false;";
                             //strURL.Replace("div", "ctl00_div")+ "; return false;";
                             lBtn.ID = dr["MenuID"].ToString();
                             lBtn.Text = dr["MenuDescription"].ToString() + "   <img alt='>>' src='images/right.gif' />";
                             phMenus.Controls.Add(lBtn);
                         }
                         else
                         {
                             Label lblMenu = new Label();
                             lblMenu.Text = dr["MenuDescription"].ToString() + "   <img alt='>>' src='images/right.gif' />";
                             lblMenu.CssClass = "MainLeftItem";
                             phMenus.Controls.Add(lblMenu);

                             string strHtml = string.Empty;
                             HtmlGenericControl div1 = new HtmlGenericControl("DIV");
                             div1.ID = dr["DivName"].ToString(); //"div" + dr["MenuDescription"].ToString();
                             //div1.Attributes.Add("style", "display:none");
                             strSql = "select M.MenuID, MenuDescription,TargetURL, M.ISACtive, [ReadOnly], Isnull(SubMenu,'') SubMenu , ParentID, DivName from tblMenuItems M inner join tblUserRights R on M.MenuID=R.MenuId and (m.IsActive=1 and R.IsActive=1)  and UserGroupID=1 and ParentID=" + dr["MenuID"].ToString() + " Order by M.MenuID";

                             dsSubItems = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSql);
                             strHtml = "<ol> ";
                             foreach (DataRow drS in dsSubItems.Tables[0].Rows)
                             {
                                 strURL = drS["TargetURL"].ToString();
                                 strURL += (strURL.Contains("?") ? "&" + "RO=" + HttpUtility.UrlEncode(dr["ReadOnly"].ToString()) : "");
                                 strHtml += "<li> <a href='" + strURL + "' class='IndentItem'> " + drS["MenuDescription"].ToString() + "</a></li>";

                             }
                             strHtml += "</ol>";
                             div1.InnerHtml = strHtml;
                             phMenus.Controls.Add(div1);
                         }
                     }


                     // phMenus.Controls.AddAt(iPos, lBtn);                    
                 }
             }
             catch (Exception)
             {

                 throw;
             }
         }
         private void PopulateMainMenu(string strSql)
         {
             try
             {
                 DataSet dsMainMenu = new DataSet();
                 dsMainMenu = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSql);
                 int iPos = 0;
                 foreach (DataRow dr in dsMainMenu.Tables[0].Rows)
                 {
                     LinkButton lBtn = new LinkButton();
                     string strURL = dr["TargetURL"].ToString();
                     strURL += (strURL.Contains("?") ? "&" + "RO=" + HttpUtility.UrlEncode(dr["ReadOnly"].ToString()) : "");
                     lBtn.OnClientClick = "javascript:window.location.href='" + strURL + "'; return false;";
                     //lBtn("OnClick","javascript:window.location.href('" + dr["TargetURL"].ToString() + "&ReadOnly=" +dr["ReadOnly"].ToString()+  "'); return false;'");
                     iPos = phMenus.Controls.Count;
                     if (dr["SubMenu"].ToString() == "False")
                     {
                         lBtn.CssClass = "MainLeftItem";
                         lBtn.ID = dr["MenuID"].ToString();
                         lBtn.Text = dr["MenuDescription"].ToString();
                         phMenus.Controls.Add(lBtn);
                     }
                     else
                     {
                         DataSet dsSubItems = new DataSet();
                         if (dr["DivName"].ToString() != "divTreatment")
                         {
                             lBtn.CssClass = "MainLeftItem";
                             lBtn.OnClientClick = "javascript:window.location.href='ProjectForm.aspx?Type=" + dr["DivName"].ToString() + "'; return false;";
                             //strURL.Replace("div", "ctl00_div")+ "; return false;";
                             lBtn.ID = dr["MenuID"].ToString();
                             lBtn.Text = dr["MenuDescription"].ToString() + "   <img alt='>>' src='images/right.gif' />";
                             phMenus.Controls.Add(lBtn);
                         }
                         else
                         {
                             Label lblMenu = new Label();
                             lblMenu.Text = dr["MenuDescription"].ToString() + "   <img alt='>>' src='images/right.gif' />";
                             lblMenu.CssClass = "MainLeftItem";
                             phMenus.Controls.Add(lblMenu);

                             string strHtml = string.Empty;
                             HtmlGenericControl div1 = new HtmlGenericControl("DIV");
                             div1.ID = dr["DivName"].ToString(); //"div" + dr["MenuDescription"].ToString();
                             //div1.Attributes.Add("style", "display:none");
                             strSql = "select M.MenuID, MenuDescription,TargetURL, M.ISACtive, [ReadOnly], Isnull(SubMenu,'') SubMenu , ParentID, DivName from tblMenuItems M inner join tblUserRights R on M.MenuID=R.MenuId and (m.IsActive=1 and R.IsActive=1)  and UserGroupID=1 and ParentID=" + dr["MenuID"].ToString() + " Order by M.MenuID";

                             dsSubItems = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSql);
                             strHtml = "<ol> ";
                             foreach (DataRow drS in dsSubItems.Tables[0].Rows)
                             {
                                 strURL = drS["TargetURL"].ToString();
                                 strURL += (strURL.Contains("?") ? "&" + "RO=" + HttpUtility.UrlEncode(dr["ReadOnly"].ToString()) : "");
                                 strHtml += "<li> <a href='" + strURL + "' class='IndentItem'> " + drS["MenuDescription"].ToString() + "</a></li>";

                             }
                             strHtml += "</ol>";
                             div1.InnerHtml = strHtml;
                             phMenus.Controls.Add(div1);
                         }
                     }


                     // phMenus.Controls.AddAt(iPos, lBtn);                    
                 }
             }
             catch (Exception)
             {

                 throw;
             }
         }*/


        private void PopulateSubMenu(string strSql, bool CustomMenu)
        {
            DataSet dsSubMenu = new DataSet();
            strConn = GlobalValues.strConnString;
            phMenus.Controls.Clear();
            dsSubMenu = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSql);
            string GroupName = string.Empty;
            LinkButton lBtnOther = new LinkButton();
            lBtnOther.Text = "";
            lBtnOther.CssClass = "MainLeftItem";
            foreach (DataRow dr in dsSubMenu.Tables[0].Rows)
            {
                if (dr["screenName"].ToString().Contains("Line Treatment"))
                {

                }
                else
                {
                    //if (GroupName != dr["GroupName"].ToString())
                    //{
                    //Label lblMenu = new Label();
                    //lblMenu.Text = dr["GroupName"].ToString();
                    //lblMenu.CssClass = "MainLeftItem";
                    //phMenus.Controls.Add(lblMenu);
                    //}

                    LinkButton lBtn = new LinkButton();
                    string strURL = "ProjectForm.aspx?SN=" + dr["ScreenName"].ToString().Trim() + "&Type=" + dr["SortOrder"].ToString();
                    lBtn.OnClientClick = "javascript:window.location.href= '" + strURL + "'; return false;";
                    lBtn.CssClass = "MainLeftItem"; //"IndentItem";
                    lBtn.ToolTip = dr["ScreenName"].ToString();
                    lBtn.ID = dr["ScreenName"].ToString();
                    string strCaption = dr["ScreenName"].ToString();
                    if (strCaption.Length > 27)
                    {
                        strCaption = strCaption.Substring(0, 25) + "...";
                    }
                    lBtn.Text = strCaption;
                    if (lBtn.ToolTip == "Others")
                    {
                        lBtnOther = lBtn;
                    }
                    else
                    {
                        phMenus.Controls.Add(lBtn);
                    }
                    GroupName = dr["GroupName"].ToString();
                    //Others
                }
            }
            if (lBtnOther.Text != "")
            {
                lBtnOther.OnClientClick = "javascript:window.location.href='OthersPage.aspx'; return false;";
                phMenus.Controls.Add(lBtnOther);
            }
            //LinkButton lBtn1 = new LinkButton();
            //lBtn1.Text = "Back";
            //lBtn1.OnClientClick = "javascript:window.location.href='ProjectForm.aspx'; return false;";
            //lBtn1.CssClass = "MainLeftItem";
            //phMenus.Controls.Add(lBtn1);

        }
        protected void btn_click(object sender, ImageClickEventArgs e)
        {
            ImageButton btnMnu = (ImageButton)sender;
            Response.Redirect("ProjectForm.aspx?Tag=" + Convert.ToString(btnMnu.CommandArgument));

        }

        protected void rdMenuType_SelectedIndexChanged(object sender, EventArgs e)
        {

            string sqlStr = string.Empty;
            if (Session["gMenuType"].ToString() != rdMenuType.SelectedValue.ToString())
            {
                //GlobalValues.gMenuDropVal = "";
                Session["gMenuDropVal"] = "";
            }
            //GlobalValues.gMenuType = rdMenuType.SelectedValue.ToString();
            Session["gMenuType"] = rdMenuType.SelectedValue.ToString();
            btnOther.Visible = false;
            if (rdMenuType.SelectedValue == "ByDiagnosis")
            {
                bool customMenu = false;
                btnOther.Visible = true;
                object objRes = SqlHelper.ExecuteScalar(strConn, CommandType.Text, "select isnull(SpecialLeftnavScreensNeeded,0) SpecialLeftnavScreensNeeded from Diagnosis where Diagnosis='" + GlobalValues.gDisease + "'");
                customMenu = Convert.ToBoolean(objRes);
                if (customMenu == false)
                {
                    sqlStr = "select  '' as GroupName, ScreenName,  OrderOfScreens, 'Preset' SortOrder from PDScreenMaster_Preset union all "
                             + " select DiseaseName as GroupName, ScreenName,  OrderOfScreens, 'SiteOfPrimary' SortOrder from PDScreenMaster_Diagnosis where DiseaseName = '" + GlobalValues.gDisease + "' "
                             + " order by SortOrder desc, OrderOfScreens";
                }
                else
                {
                    sqlStr = "select DiseaseName as GroupName, ScreenName,  OrderOfScreens, 'SiteOfPrimary' SortOrder from PDScreenMaster_Diagnosis where DiseaseName = '" + GlobalValues.gDisease + "' "
                            + " order by SortOrder desc, OrderOfScreens";
                }
                //PopulateMainMenu(sqlStr);
                PopulateSubMenu(sqlStr, customMenu);
                //pnlMenus.Visible = false;
                divMenuButtons.Style.Add("display", "none");
            }
            else
                if (rdMenuType.SelectedValue.Trim() == "ByStudy")
                {
                    //sqlStr = "Select StudyName  from [Studies] where instances like '%-" + GlobalValues.gInstanceID.ToString() + "-%' order by studyName";

                    //sqlStr = "Select StudyName  from [Studies] S inner join tblStudyUsers  SU on  s.StudyCode=SU.StudyCode where  instances like '%-" + GlobalValues.gInstanceID.ToString() + "-%'   and  '" + Session["Login"].ToString() + "'  in ( select a.HosCode from fn_Splithospitalcode(SU.Users) as a)    order by studyName";
                    sqlStr = "Select StudyName  from [Studies] S inner join tblStudyUsers  SU on  s.StudyCode=SU.StudyCode " +
                        " where  instances  like '%-" + GlobalValues.gInstanceID.ToString() + "-%'  and  su.users like '%" + Session["Login"].ToString() + "%' order by studyName ";

                    DataSet ds = new DataSet();
                    ds = SqlHelper.ExecuteDataset(strConn, CommandType.Text, sqlStr);
                    DataRow dr = ds.Tables[0].NewRow();
                    dr[0] = "";
                    //dr[1] = "";                    
                    ds.Tables[0].Rows.InsertAt(dr, 0);
                    dpMenuItems.DataSource = ds.Tables[0].DefaultView;
                    dpMenuItems.DataValueField = "StudyName";
                    dpMenuItems.DataTextField = "StudyName";
                    dpMenuItems.DataBind();
                    dpMenuItems.SelectedValue = "";
                    // pnlMenus.Visible = true;
                    divMenuButtons.Style.Add("display", "");
                }
                else if (rdMenuType.SelectedValue.Trim() == "Custom")
                {
                    sqlStr = "Select ScreenName  from PDScreenMaster_Custom PDC   where '" + Session["Login"].ToString() + "'  in ( select LTRIM(RTRIM(a.HosCode)) as HosCode from fn_Splithospitalcode(PDC.Users) as a)  order by ScreenName";
                    DataSet ds = new DataSet();
                    ds = SqlHelper.ExecuteDataset(strConn, CommandType.Text, sqlStr);
                    DataRow dr = ds.Tables[0].NewRow();
                    dr[0] = "";
                    //dr[1] = "";
                    ds.Tables[0].Rows.InsertAt(dr, 0);
                    dpMenuItems.DataSource = ds.Tables[0].DefaultView;
                    dpMenuItems.DataValueField = "ScreenName";
                    dpMenuItems.DataTextField = "ScreenName";
                    dpMenuItems.DataBind();
                    dpMenuItems.SelectedValue = "";
                    //pnlMenus.Visible = true;
                    divMenuButtons.Style.Add("display", "");
                }
                else
                {

                }
            if (Session["rdMenuType"] == null)
            {
                //Response.Redirect("ProjectForm.aspx", false);
            }
        }



        protected void dpMenuItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqlStr = string.Empty;
            //GlobalValues.gMenuDropVal = dpMenuItems.SelectedItem.Text.ToString();
            Session["gMenuDropVal"] = dpMenuItems.SelectedItem.Text.ToString();
            if (dpMenuItems.SelectedItem.Text.ToString() != "")
            {
                if (rdMenuType.SelectedValue == "ByStudy")
                {
                    sqlStr = " select '' as GroupName, ScreenName,  OrderOfScreens, 'Study' SortOrder from PDScreenMaster_Study where StudyName like '" + dpMenuItems.SelectedItem.Text.ToString() + "%' "
                     + " order by  OrderOfScreens";
                }
                else
                {
                    string strWhere = dpMenuItems.SelectedItem.Text.ToString();
                    if (strWhere.Trim().Length <= 0) { strWhere = "1=0"; }
                    sqlStr = " select '' as GroupName, ScreenName,  OrderOfScreens, 'Custom' SortOrder from PDScreenMaster_Custom where ScreenName  like '" + strWhere + "%' "
                     + " order by  OrderOfScreens";
                }
            }
            else
            {
                sqlStr = "select * from PDScreenMaster_Study where 1=0";
            }
            PopulateSubMenu(sqlStr, true);
        }///819KiuND1$ 

        protected void btnLabTest_Click(object sender, EventArgs e)
        {
            Response.Redirect("LabTests.aspx?SN=Toxicity&Type=LabTest");
        }

        protected void btnMetasis_Click(object sender, EventArgs e)
        {
            Response.Redirect("SitesofMet.aspx?SN=Toxicity&Type=SiteofMEt");
        }

        protected void btnToxicity_Click(object sender, EventArgs e)
        {
            Response.Redirect("ToxiCity.aspx?SN=Toxicity&Type=WorstToxcity");
            //string opt = "dialogWidth:1000px; dialogHeight:400px; center:yes; scroll:no; status:no";

            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", " window.showModalDialog('ToxoCity.aspx', 'Toxicity','dialogWidth:1000px; dialogHeight:400px; center:yes; scroll:no; status:no');", true);
        }

        protected void btnStatus_Click(object sender, EventArgs e)
        {
            Response.Redirect("Status.aspx?SN=Toxicity&Type=Status");
        }

        protected void btnRecurrence_Click(object sender, EventArgs e)
        {
            Response.Redirect("Recurrence.aspx?SN=Toxicity&Type=Recurrence");
        }

        private void AuthorizationOfPages()
        {
            try
            {
                // code modified by Thiru on 12 May, 2015
                string onCoMenu_html = "";
                string pdSQL = "select * from FeatureSetUsers where UserID='" + Session["Login"].ToString() + "'";
                DataSet dset = SqlHelper.ExecuteDataset(strConns, CommandType.Text, pdSQL);
                if (dset.Tables.Count > 0 && dset.Tables[0].Rows.Count > 0)
                {
                    //Data Analysis
                    DataRow[] foundrow = dset.Tables[0].Select("FeatureSetName='Data Analysis' and Enabled=1");
                    if (foundrow.Length > 0)
                    {
                        string lvl1 = "<a href=\"#\">Data Analysis</a>";
                        string lvl2 = "";
                        //Patient Analysis
                        foundrow = dset.Tables[0].Select("FeatureSetName='Data Analysis' and Enabled=1");
                        if (foundrow.Length > 0)
                        {
                            lvl2 = "<a href=\"#\">Patient Analysis</a>";
                            string lvl3 = "";
                            //By Study
                            foundrow = dset.Tables[0].Select("FeatureSetName='Patient Analysis - By Study' and Enabled=1");
                            if (foundrow.Length > 0)
                            {
                                lvl3 += "<li><a href=\"DataAnalysis.aspx?AType=ByStudy\">By Study</a></li>";
                            }
                            else
                            {
                                lvl3 += "<li class=\"disabled\"><a class=\"disabled\" href=\"DataAnalysis.aspx?AType=ByStudy\">By Study</a></li>";
                            }

                            //By All Parametrs
                            foundrow = dset.Tables[0].Select("FeatureSetName='Patient Analysis - By all Parameters' and Enabled=1");
                            if (foundrow.Length > 0)
                            {
                                lvl3 += "<li><a href=\"DataAnalysis.aspx?AType=All\">By All Parameters</a></li>";
                            }
                            else
                            {
                                lvl3 += "<li class=\"disabled\"><a class=\"disabled\" href=\"DataAnalysis.aspx?AType=All\">By All Parameters</a></li>";
                            }
                            lvl3 = "<ul class=\"dropdown-menu\">" + lvl3 + "</ul>";
                            lvl2 = "<li>" + lvl2 + lvl3 + "</li>";
                        }
                        else
                        {
                            lvl2 += "<li class=\"disabled\"><a class=\"disabled\" href=\"#\">Patient Analysis</a></li>";
                        }

                        //Saved Query
                        foundrow = dset.Tables[0].Select("FeatureSetName='My Saved Queries' and Enabled=1");
                        if (foundrow.Length > 0)
                            lvl2 += "<li><a href=\"SavedQueriesView.aspx?SN=Toxicity&Type=SavedQry\">My Saved Queries</a></li>";
                        else
                            lvl2 += "<li class=\"disabled\"><a class=\"disabled\" href=\"SavedQueriesView.aspx?SN=Toxicity&Type=SavedQry\">My Saved Queries</a></li>";

                        //Status Check List
                        foundrow = dset.Tables[0].Select("FeatureSetName='Status Check-List' and Enabled=1");
                        if (foundrow.Length > 0)
                            lvl2 += "<li><a href=\"StatusReminder.aspx?SN=Toxicity&Type=Status\">Status Check List</a></li>";
                        else
                            lvl2 += "<li class=\"disabled\"><a class=\"disabled\" href=\"StatusReminder.aspx?SN=Toxicity&Type=Status\">Status Check List</a></li>";

                        //Drug Dosage
                        foundrow = dset.Tables[0].Select("FeatureSetName='Drug Dosage Analysis' and Enabled=1");
                        if (foundrow.Length > 0)
                            lvl2 += "<li><a href=\"DataAnalysis.aspx?AType=ByDiag\">Drug Dosage Analysis</a></li>";
                        else
                            lvl2 += "<li class=\"disabled\"><a class=\"disabled\" href=\"DataAnalysis.aspx?AType=ByDiag\">Drug Dosage Analysis</a></li>";

                        lvl2 = "<ul class=\"dropdown-menu\">" + lvl2 + "</ul>";
                        onCoMenu_html += "<li>" + lvl1 + lvl2 + "</li>";
                    }
                    else
                        onCoMenu_html += "<li class=\"disabled\"><a class=\"disabled\" href=\"#\">Data Analysis</a></li>";

                    //Configuration
                    foundrow = dset.Tables[0].Select("FeatureSetName='Configuration' and Enabled=1");
                    if (foundrow.Length > 0)
                    {
                        string lvl1 = "<a href=\"#\">Configuration</a>";
                        string lvl2 = "";
                        //Data Grouping
                        foundrow = dset.Tables[0].Select("FeatureSetName='Data Grouping' and Enabled=1");
                        if (foundrow.Length > 0)
                            lvl2 += "<li><a href=\"DataGroup.aspx\">Data Groups</a></li>";
                        else
                            lvl2 += "<li class=\"disabled\"><a class=\"disabled\" href=\"DataGroup.aspx\">Data Groups</a></li>";

                        foundrow = dset.Tables[0].Select("FeatureSetName='Drug Grouping' and Enabled=1");
                        if (foundrow.Length > 0)
                            lvl2 += "<li><a href=\"DrugGroup.aspx\">Drug Groups</a></li>";
                        else
                            lvl2 += "<li class=\"disabled\"><a class=\"disabled\" href=\"DrugGroup.aspx\">Drug Groups</a></li>";


                        foundrow = dset.Tables[0].Select("FeatureSetName='Test Grouping' and Enabled=1");
                        if (foundrow.Length > 0)
                            lvl2 += "<li><a href=\"TestGroup.aspx\">Test Groups</a></li>";
                        else
                            lvl2 += "<li class=\"disabled\"><a class=\"disabled\" href=\"TestGroup.aspx\">Test Groups</a></li>";

                        lvl2 = "<ul class=\"dropdown-menu\">" + lvl2 + "</ul>";
                        onCoMenu_html += "<li>" + lvl1 + lvl2 + "</li>";
                    }
                    else
                        onCoMenu_html += "<li class=\"disabled\"><a class=\"disabled\" href=\"#\">Configuration</a></li>";

                    //Change Password
                    foundrow = dset.Tables[0].Select("FeatureSetName='Change Password' and Enabled=1");
                    if (foundrow.Length > 0)
                        onCoMenu_html += "<li><a href=\"ChangePassword.aspx\">Change Password</a></li>";
                    else
                        onCoMenu_html += "<li class=\"disabled\"><a class=\"disabled\" href=\"ChangePassword.aspx\">Change Password</a></li>";


                    //Study
                    foundrow = dset.Tables[0].Select("FeatureSetName='Study' and Enabled=1");
                    if (foundrow.Length > 0)
                    {
                        string lvl1 = "<a href=\"#\">Study</a>";
                        string lvl2 = "";
                        foundrow = dset.Tables[0].Select("FeatureSetName='Export Data to Study Pool' and Enabled=1");
                        if (foundrow.Length > 0)
                        {
                            var LocalPI = GlobalValues.ExecuteScalar("SELECT  isnull(COUNT(*),0) FROM tblStudyUsers WHERE localPI LIKE  '%" + Session["Login"].ToString() + "%'");
                            if (Convert.ToInt32(LocalPI) > 0)
                            {
                                lvl2 = "<li><a href=\"ExportDataToStudy.aspx\">Export Data to Study Pool</a></li>";
                            }
                            else
                            {
                                lvl2 = "<li class=\"disabled\"><a class=\"disabled\" href=\"ExportDataToStudy.aspx\">Export Data to Study Pool</a></li>";
                            }

                            lvl2 = "<ul class=\"dropdown-menu\">" + lvl2 + "</ul>";
                            onCoMenu_html += "<li>" + lvl1 + lvl2 + "</li>";
                        }
                        else
                        {
                            onCoMenu_html += "<li class=\"disabled\"><a class=\"disabled\" href=\"#\">Study</a></li>";
                        }
                    }
                    else
                        onCoMenu_html += "<li class=\"disabled\"><a class=\"disabled\" href=\"#\">Study</a></li>";

                    //Audit Trail
                    foundrow = dset.Tables[0].Select("FeatureSetName='Audit Trail' and Enabled=1");
                    if (foundrow.Length > 0)
                        onCoMenu_html += "<li><a href=\"ViewAuditTrail.aspx\">Audit Trail</a></li>";
                    else
                        onCoMenu_html += "<li class=\"disabled\"><a class=\"disabled\" href=\"ViewAuditTrail.aspx\">Audit Trail</a></li>";

                    //Consultants
                    foundrow = dset.Tables[0].Select("FeatureSetName='Consultants' and Enabled=1");
                    if (foundrow.Length > 0)
                        onCoMenu_html += "<li><a href=\"consultants.aspx\">Consultants</a></li>";
                    else
                        onCoMenu_html += "<li class=\"disabled\"><a class=\"disabled\" href=\"consultants.aspx\">Consultants</a></li>";

                    onCoMenu.InnerHtml = onCoMenu_html;

                    ObjfeatureSet.GetUserPermissions(Session["Login"].ToString(), ObjfeatureSet.IsstrSessionId);
                    Session["FeatureSetPermission"] = ObjfeatureSet;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert(' Project Form" + ex.Message + ex.InnerException.ToString() + "');", true);
                GlobalValues.ErrorLog("", ex.StackTrace.ToString(), ex.Message.ToString(), "For Testing", ex.InnerException.ToString());
            }
        }

        private void BlockUserToSaveTheChanges()
        {
            Button btnSaveProjectForm = (Button)MainContent.FindControl("btnSaveProjectForm");
            if (btnSaveProjectForm != null)
                if (btnSaveProjectForm.Visible)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('test.');", true);

                }
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
        protected void lnkErrorReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("ErrorReport.aspx", false);
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

        protected void BtnReg_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }

    }

    public static class BrowserClose
    {
        [WebMethod]
        public static void AbandonSession()
        {
            HttpContext.Current.Session.Abandon();

        }
    }
}