﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using ePxCollectDataAccess;
using System.Configuration;
using System.Threading;
using System.Web.Services;

namespace ePxCollectWeb
{
    public partial class SearchPatient : System.Web.UI.Page
    {
        string strConn;
        static DataSet dsSearch = new DataSet();
        string UserID = string.Empty;
        string strAuditConns = GlobalValues.strAuditConnString;
        Thread thread;
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
        public static string PatientID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FeatureSetPermission"] != null)
            {
                ObjfeatureSet = (FeatureSetPermission)Session["FeatureSetPermission"];
            }
            if (Session["ResetPassword"] != null)
            {
                Session["ResetPasswordMsg"] = "Please Change your password.";
                Response.Redirect("Changepassword.aspx");
            }
            txtOther.MaxLength = GlobalValues.intSearchLengthMax;
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }

            if (Session["Message"] != null)
            {
                if (Request.UrlReferrer != null)
                {
                    if (Request.UrlReferrer.ToString().IndexOf("SearchPatient.aspx") != -1)
                    {
                        Session["Message"] = null;
                    }
                }
                if (Session["Message"] != null)
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('" + Session["Message"] + "');", true);
                Session["Message"] = null;
            }
            strConn = GlobalValues.strConnString;//System.Configuration.ConfigurationManager.ConnectionStrings["OncoCollectEnterprise"].ConnectionString.Trim();
            txtFname.Visible = false;
            txtLName.Visible = false;
            lblFirstName.Visible = false;
            lblLastName.Visible = false;
            txtOther.Visible = true;
            txtOther.Focus();
            ViewState["OldPatient"] = Convert.ToString(Session["PatientID"]);

            //Session.Remove("PatientID");
            //Session.Remove("PatienDetails");

            if (!IsPostBack)
            {
                string[] PageSize = GlobalValues.strGridSize;
                for (int i = 0; i < PageSize.Length; i++)
                {
                    pager.Items.Add(new ListItem(PageSize[i], PageSize[i]));
                }

            }
            //if(!IsPostBack)
            //{
            //    cboSearchFor.Items.Insert(0, new ListItem(" Select Filter ", ""));
            //    btnPickPatient.Enabled = false;
            //    txtOther.Enabled = false;
            //}
            //if (cboSearchFor.Text == "" || cboSearchFor.Text =="Select Filter")
            //{
            //     btnPickPatient.Enabled = false;
            //     txtOther.Enabled = false;
            //}

            if (ObjfeatureSet.isRegistration == false)
            {
                //HyperLink1.Visible = false;
            }
            if (Convert.ToString(Session["Login"]) == "")
            {
                //    NavigationMenu.Enabled = false;
                Response.Redirect("login.Aspx");
            }
            UserID = Convert.ToString(Session["Login"]);
            divpager.Visible = false;
            // txtFname.Focus();


            if (ObjfeatureSet.isSearch) { btnPickPatient.Visible = true; } else { btnPickPatient.Visible = false; }

        }

        protected void cboSearchFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string parentText = "Patient ID";
            string fileText = "File Number";
            txtOther.Visible = true;
            if ((string.Equals(cboSearchFor.Text, parentText)) || (string.Equals(cboSearchFor.Text, fileText)))
            {
                lblOthers.Text = cboSearchFor.Text.ToString();
                txtOther.Focus();
                txtOther.Text = string.Empty;
                btnPickPatient.Enabled = true;
                txtOther.Enabled = true;
            }
            else
            {
                btnPickPatient.Enabled = false;
                txtOther.Enabled = false;
                txtOther.Focus();
            }


            //if (cboSearchFor.Text == "All Patients")
            //{
            //    btnSearch_Click(null, null);
            //    txtOther.Visible = false;
            //    txtFname.Visible = false;
            //    txtLName.Visible = false;
            //    lblOthers.Visible = true;
            //    lblOthers.Text = cboSearchFor.Text.ToString();
            //    lblFirstName.Visible = false;
            //    lblLastName.Visible = false;

            //}
            //else if (cboSearchFor.Text == "Name")
            //{
            //    txtOther.Visible = false;
            //    txtFname.Visible = true;
            //    txtLName.Visible = true;
            //    lblOthers.Visible = false;
            //    lblFirstName.Visible = true;
            //    lblLastName.Visible = true;
            //    txtFname.Focus();
            //}
            //else
            //{
            //    txtOther.Visible = true;
            //    txtFname.Visible = false;
            //    txtLName.Visible = false;
            //    lblOthers.Visible = true;
            //    lblOthers.Text = cboSearchFor.Text.ToString();
            //    lblFirstName.Visible = false;
            //    lblLastName.Visible = false;
            //    txtOther.Focus();
            //}

            if (grdResult.Rows.Count > 0)
                divpager.Visible = true;
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //string strCancel;
            //strCancel = "";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string strWhere = string.Empty;
            string strSql = string.Empty;
            //Boolean ValidationErr = false;

            try
            {
                if ((txtFname.Text.ToString().Trim() + txtLName.Text.ToString().Trim() + txtOther.Text.Trim()).Length >= GlobalValues.intSearchLength)
                {
                    this.logImage.Visible = false;
                    switch (cboSearchFor.Text)
                    {
                        case "Name":
                            {

                                if (txtFname.Text.Trim() != "" && txtLName.Text.Trim() != "")
                                {
                                    strWhere = "PatientDetails_0.PatientName  like '%" + txtFname.Text.Trim() + "%' or PatientDetails_0.PatientName  like '%" + txtLName.Text.Trim() + "%'";

                                }
                                else if (txtFname.Text.Trim() != "")
                                {
                                    strWhere = "PatientDetails_0.PatientName  like '%" + txtFname.Text.Trim() + "%'";
                                }
                                else if (txtLName.Text.Trim() != "")
                                {
                                    strWhere = "PatientDetails_0.PatientName  like '%" + txtLName.Text.Trim() + "%'";
                                }
                                break;
                            }
                        case "File Number":
                            {
                                strWhere = "PatientDetails_0.HospitalFileNo like '%" + txtOther.Text.Trim() + "%'";
                                break;
                            }
                        case "Patient ID":
                            {
                                strWhere = "PatientDetails_0.PatientID Like  '%" + txtOther.Text.Trim() + "%'";
                                break;
                            }
                        case "City_Town":
                            {
                                strWhere = "PatientDetails_0.City_Town Like '%" + txtOther.Text.Trim() + "%'";

                                break;
                            }
                        case "State":
                            {
                                strWhere = "PatientDetails_0.State Like '%" + txtOther.Text.Trim() + "%'";
                                break;
                            }
                        case "Diagnosis":
                            {
                                strWhere = "PatientDetails_1.SiteOfPrimary Like '%" + txtOther.Text.Trim() + "%'";
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    if (strWhere == "")
                    {

                    }

                    //if (pnlOptions.Visible == false || OptSearch.SelectedValue == "2")
                    if (true)
                    {
                        ViewState["sqlWhere"] = strWhere;
                        strSql = "SELECT top 1000 PatientDetails_0.PatientID as 'Patient ID',PatientDetails_0.PatientName as 'Patient Name',PatientDetails_0.HospitalFileNo as 'File Number',PatientDetails_1.SiteOfPrimary as 'Site of Primary',PatientDetails_0.City_Town as 'City/Town',PatientDetails_0.State as 'State', datename(mm, PatientDetails_0.DateofRegistration)+ ' ' + cast(datepart(dd, PatientDetails_0.DateofRegistration) as char(2)) + ',' + cast(datepart(yyyy,PatientDetails_0.DateofRegistration) as char(4))   as 'Date of Registration' From PatientDetails_0 LEFT JOIN PatientDetails_1 ON PatientDetails_0.PatientID = PatientDetails_1.Patient ";
                        //Code remodified on March 09-2015,Subhashini
                        if (strWhere.Trim() != "") { strSql += " WHERE " + strWhere; }
                        strSql += " ORDER BY PatientDetails_0.SortID";

                        dsSearch = ePxCollectDataAccess.SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSql);

                    }
                    else
                    {
                        dsSearch = (DataSet)Session["ds" + Session.SessionID.ToString()];
                        //strWhere= strWhere.Replace("like", "=").Replace("PatientDetails_0.", "");
                        strWhere = strWhere.Replace("PatientDetails_0.", "");
                        strWhere = strWhere.Replace("PatientDetails_1.", "");
                        dsSearch.Tables[0].DefaultView.RowFilter = strWhere;
                        //grdResult.DataSource = dsSearch.Tables[0].DefaultView;
                        //grdResult.DataBind();
                    }
                    //lblErrorMsg.Visible = false;//Code modified on April 18,2015-Subhashini
                    lblErrorMsg.Text = "";
                    dsSearch.Tables[0].AcceptChanges();
                    grdResult.DataSource = dsSearch.Tables[0].DefaultView;
                    grdResult.DataBind();
                    if (dsSearch.Tables[0].Rows.Count == 0)//Code modified on March 06-2015,Subhashini
                    {
                        //grdResult.EmptyDataText = "No Records Found.";
                        lblErrorMsg.Visible = true;//Code modified on April 18,2015-Subhashini
                        lblErrorMsg.ForeColor = GlobalValues.FailureColor;
                        lblErrorMsg.Text = "No Records found.";
                    }
                    divpager.Visible = true;
                    Session["ds" + Session.SessionID.ToString()] = dsSearch;//.Tables[0].DefaultView;
                    UpdatePanel1.Update();
                    //txtOther.Text = string.Empty;
                    //if (dsSearch.Tables[0].Rows.Count > 0) { pnlOptions.Visible = true; }
                    //else { pnlOptions.Visible = false; }
                }

                else
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "alert('Please select atleast " + GlobalValues.intSearchLength + " character for your search value');", true);
                    //Code modified on March 06-2015,Subhashini
                    lblErrorMsg.Text = "Please enter File Number/Patient ID.";
                    lblErrorMsg.Visible = true;
                    grdResult.EmptyDataText = "";
                    grdResult.DataSource = null;
                    grdResult.DataBind();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void viewmoreDetails()
        {
            string value = string.Empty;
            bool loadPatient = true;
            value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[1].Text;
            string SQL = "Select Count(*) from PatientDetails_0 Where PatientID ='" + value + "' and Locked = 1";
            DataSet dsLock = SqlHelper.ExecuteDataset(strConn, CommandType.Text, "Select * from tblLock where PatientID='" + value + "'");
            //int isLocked = (int)SqlHelper.ExecuteScalar(strConn, CommandType.Text, SQL);
            if (dsLock.Tables[0].Rows.Count > 0)
            //if (isLocked > 0)
            {
                value = dsLock.Tables[0].Rows[0]["UserName"].ToString();
                if (value != UserID)
                {
                    loadPatient = false;
                }
            }
            if (loadPatient)
            {
                value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[1].Text;
                lblPatientID.Text = value;
                DataSet dspatient = GlobalValues.ExecuteDataSet("select PatientDetails_0.Address,PatientDetails_0.PhoneNumber,PatientDetails_0.EmailID,PatientDetails_0.Consultants,PatientDetails_9.[Relative Phone No],PatientDetails_9.[Mobile Number] from PatientDetails_0  inner join   PatientDetails_9 on PatientDetails_0.PatientID=PatientDetails_9.Patient where  PatientDetails_0.PatientID ='" + value + "' ");
                value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[2].Text;
                lblPatientName.ToolTip = value;
                if (value.Length > 20)
                    this.lblPatientName.Text = value.Substring(0, 20) + "...";
                else
                    this.lblPatientName.Text = value;
                lblPatientName.ToolTip = value;
                value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[3].Text;
                if (value.Length > 20)
                    this.lblFileNo.Text = value.Substring(0, 20) + "...";
                else
                    this.lblFileNo.Text = value;
                lblFileNo.ToolTip = value;
                value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[5].Text;
                this.lblCity.Text = value;
                OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
                //Patient Identity Fields
                if (dspatient.Tables[0].Rows.Count > 0)
                {
                    value = objEnc.Decrypt(dspatient.Tables[0].Rows[0]["Address"].ToString());
                    this.lblAddress.Text = value == "NULL" ? "" : value;

                    value = objEnc.Decrypt(dspatient.Tables[0].Rows[0]["PhoneNumber"].ToString());
                    this.lblPhoneNumber.Text = value == "NULL" ? "" : value;


                    value = objEnc.Decrypt(dspatient.Tables[0].Rows[0]["EmailId"].ToString());
                    this.lblmailid.Text = value == "NULL" ? "" : value;

                    if (value.Length > 20)
                        this.lblmailid.Text = lblmailid.Text.Trim().Substring(0, 20) + "...";
                    else
                        this.lblmailid.Text = lblmailid.Text;
                    lblmailid.ToolTip = value;
                    //value = objEnc.Decrypt(dspatient.Tables[0].Rows[0]["Consultants"].ToString());
                    //this.lblConsultants.Text = value;

                    value = objEnc.Decrypt(dspatient.Tables[0].Rows[0]["Mobile Number"].ToString());
                    this.lblmob.Text = value == "NULL" ? "" : value;

                    //value = objEnc.Decrypt(dspatient.Tables[0].Rows[0]["Relative Phone No"].ToString());
                    //this.lblRPNum.Text = value;

                }
                updConfirm.Update();
                ModalPopupExtender1.Show();
            }
            else
            {
                value = "The Patient Record is Locked by " + value + ". Please try later.";
                //value = "The Patient Record is Locked by another User. Please try later.";
                ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "alert('" + value + "');", true);
            }
        }


        protected void grdResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            divpager.Visible = true;
            string value = string.Empty;
            bool loadPatient = true;
            value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[1].Text;
            string PatientId = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[1].Text;

            string SQL = "Select Count(*) from PatientDetails_0 Where PatientID ='" + value + "' and Locked = 1";
            DataSet dsLock = SqlHelper.ExecuteDataset(strConn, CommandType.Text, "Select * from tblLock where PatientID='" + value + "'");
            //int isLocked = (int)SqlHelper.ExecuteScalar(strConn, CommandType.Text, SQL);
            if (dsLock.Tables[0].Rows.Count > 0)
            //if (isLocked > 0)
            {
                value = dsLock.Tables[0].Rows[0]["UserName"].ToString();
                if (value != UserID)
                {
                    loadPatient = false;
                }
            }
            if (loadPatient)
            {

                if (Convert.ToString(ViewState["OldPatient"]) == PatientId)
                {
                    updConfirm.Update();
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "confirm('Already you have selected the Patient  " + PatientId + " .Do you wish to conitnue?')return true;else return false;;", true);
                    lblMessgae.Text = "Already you have selected the Patient  " + PatientId + ". Do you wish to conitnue?";
                   
                    ModalPopupExtender2.Show();
                    //Move Focus to BtnYes While Showing Popup ,Aloow User While Pressing Enter Key Using Keyboard
                    ScriptManager.RegisterStartupScript(this,
                                                      this.GetType(),
                                                      "FocusScript",
                                                      "setTimeout(function(){$get('" + btnOkP.ClientID + "').focus();}, 100);",
                                                      true);
                }

                else
                {

                    value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[1].Text;
                    lblPatientID.Text = value;
                    value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[2].Text;
                    this.lblPatientName.Text = value;
                    value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[3].Text;
                    this.lblFileNo.Text = value;
                    value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[5].Text;
                    this.lblCity.Text = value;
                    viewmoreDetails();
                    updConfirm.Update();
                    ModalPopupExtender1.Show();
                    //Move Focus to BtnYes While Showing Popup ,Aloow User While Pressing Enter Key Using Keyboard
                    ScriptManager.RegisterStartupScript(this,
                                                       this.GetType(),
                                                       "FocusScript",
                                                       "setTimeout(function(){$get('" + btnYes.ClientID + "').focus();}, 100);",
                                                       true);
                    
                }
            }
            else
            {
                value = "The Patient Record is Locked by " + value + ". Please try later.";
                //value = "The Patient Record is Locked by another User. Please try later.";
                ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "alert('" + value + "');", true);
                

            }

        }





        protected void grdResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResult.PageIndex = e.NewPageIndex;
            dsSearch = (DataSet)Session["ds" + Session.SessionID.ToString()];
            grdResult.DataSource = dsSearch;
            grdResult.DataBind();
            divpager.Visible = true;
        }


        public void WorkThreadFunction()
        {
            try
            {
                GlobalValues.DBStatements(Convert.ToString(Session["PatientID"]));
            }
            catch (Exception ex)
            {
                // log errors
            }
            finally
            {
                thread.Abort();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            PatientID = "";
            divpager.Visible = true;
            Session["Message"] = null;
            GlobalValues.UnlockUser(Convert.ToString(Session["Login"]), Convert.ToString(Session["PatientID"]));
            thread = new Thread(new ThreadStart(WorkThreadFunction));
            thread.Start();


            if (Convert.ToString(Session["PatientID"]) != "")
            {
                GlobalValues.UpdateModifiedDate(Convert.ToString(Session["PatientID"]), Session["Login"].ToString());
                GlobalValues.WriteAuditRecord(Convert.ToString(Session["Login"]), Convert.ToString(Session["PatientID"]), "Record Cosed", GlobalValues.strEnterpriseDataBaseName);
            }
            Session["PatientID"] = lblPatientID.Text;
            PatientID = lblPatientID.Text;
            Session["PatienDetails"] = lblPatientName.Text + "/ ID:" + lblPatientID.Text + "/ File No:" + lblFileNo.Text + "/ City:" + lblCity.Text +
                "/ " + grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[4].Text;
            GlobalValues.gDisease = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[4].Text;
            string sqlStr = string.Empty;

            //SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, "Delete from tblLock where UserName ='" + UserID + "'");
            //sqlStr = "Insert into tblLock (PatientID, UserName, LockedDate) values ('" + lblPatientID.Text.ToString() + "','" + UserID + "',getdate())";
            //SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sqlStr);
            GlobalValues.lockUser(Convert.ToString(Session["Login"]), Convert.ToString(Session["PatientID"]));
            GlobalValues.WriteAuditRecord(Convert.ToString(Session["Login"]), Convert.ToString(Session["PatientID"]), "Record Opened", GlobalValues.strEnterpriseDataBaseName); //Audit Table
            thread = new Thread(new ThreadStart(WorkThreadFunction));
            thread.Start();
            //GlobalValues.UpdateStudyWithPatientID(lblPatientID.Text.ToString());
            //string GoURL = GlobalValues.gPostBackURL;
            string GoURL = Session["gPostBackURL"] != null ? Session["gPostBackURL"].ToString() : "";
            if (GoURL.ToString().IndexOf("LineTreatment") != -1)
            {
                var siteofPrimary = GlobalValues.ExecuteScalar("select [SiteOfPrimary] from PatientDetails_1 where Patient='" + Session["PatientId"].ToString() + "' ");
                if (siteofPrimary.ToString().Trim() == string.Empty || siteofPrimary.ToString().Trim() == "-")
                {
                    GoURL = "";
                }
            }

            if (GoURL == "") { GoURL = "ProjectForm.aspx"; }
            GlobalValues.SaveSessionLog(Session["PatientID"].ToString(), Session["Login"].ToString(), GlobalValues.SLogSearch);
            Response.Redirect(GoURL);

        }

        protected void btnPickPatient_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Register.aspx");
        }

        protected void grdResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            divpager.Visible = true;
        }

        protected void pager_SelectedIndexChanged(object sender, EventArgs e)
        {
            divpager.Visible = true;
            grdResult.PageSize = int.Parse(((DropDownList)sender).SelectedValue);
            grdResult.DataSource = dsSearch.Tables[0];
            grdResult.DataBind();
        }

        protected void btnOkP_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProjectForm.aspx");
        }

        [WebMethod(EnableSession = true)]
        public static string doCheckLoggedOut()
        {
            //code added by Thiru - 30 April, 2015
            OncoCollectLookup myLookup = new OncoCollectLookup();
            string str = "";
            string strLiveUpdate = "";
            string strResetPassword = "0";
            string strActiveUser = "0";
            if (HttpContext.Current.Session["Login"] != null)
            {
                str = myLookup.sqlLookup("loginusers", "count(userid)", "userid = '" + HttpContext.Current.Session["Login"] + "'");
                if (str.Trim() == "")
                    str = "0";

                strLiveUpdate = myLookup.sqlLookup("LiveUpdateMessage", "count(UserID)", "UserID = " + HttpContext.Current.Session["Login"] + "");
                if (strLiveUpdate.Trim() == "")
                    strLiveUpdate = "0";


                if (GlobalValues.BoolResetPassword(HttpContext.Current.Session["Login"].ToString()))
                {
                    strResetPassword = "1";
                }

                if (GlobalValues.BoolActiveUser(HttpContext.Current.Session["Login"].ToString()))
                {
                    strActiveUser = "1";
                }

                if (str == "0")
                {
                    GlobalValues.RemoveCurretLoginUsersDuringLogOut(HttpContext.Current.Session["Login"].ToString(), GlobalValues.gEnterpriseApplicationName, GlobalValues.gLHSessionTimeOut, "Forced Logout during Session TimeOut.");
                    GlobalValues.UnlockUser(HttpContext.Current.Session["Login"].ToString(), HttpContext.Current.Session["PatientID"].ToString());
                }
            }
           
            string text = str + ":" + strLiveUpdate + ":" + strResetPassword + ":" + strActiveUser;
            return text;
        }
    }
}