﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ePxCollectWeb
{
    public partial class Status : System.Web.UI.Page
    {
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
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
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }
            if (Session["PatientID"] == null)
            {
                Session["Message"] = "Please pick a Patient.";
                Response.Redirect("SearchPatient.aspx");
            }
            string strPatientId = Convert.ToString(Session["PatientID"]);
            if (ObjfeatureSet.isStatus == false)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Sorry, you don't have permission to this functionality.');", true);
                Response.Redirect("ProjectForm.aspx");
            }
            if (!IsPostBack)
            {
                lblErrorMsg.Text = "";
                //calDOD.EndDate = DateTime.Today;
                string strsql = "select * from PatientDetails_0  where PatientId='" + strPatientId + "'";
                DataSet ds2 = GlobalValues.ExecuteDataSet(strsql);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    if (ds2.Tables[0].Rows[0]["DateOfBirth"].ToString() != string.Empty)
                    {
                        //calDOD.StartDate = Convert.ToDateTime(ds2.Tables[0].Rows[0]["DateOfBirth"]);
                    }
                }

                //calStatusDate.EndDate = DateTime.Today;
                cboCauseofDeath.Enabled = false;
                txtDOD.Enabled = false;


                //GlobalValues.gPostBackURL = Request.Url.AbsoluteUri.ToString();
                Session["gPostBackURL"] = Request.Url.AbsoluteUri.ToString();
                DataSet ds = GlobalValues.ExecuteDataSet("Select '' as status  union select status from patientstatus");
                cboStatus.DataSource = ds;
                cboStatus.DataTextField = "status";
                cboStatus.DataValueField = "status";
                cboStatus.DataBind();
                string strStatus = Convert.ToString(GlobalValues.ExecuteScalar("select status from patientdetails_1 where patient = '" + strPatientId + "'"));
                if (strStatus == null) { strStatus = ""; }
                if (strStatus != "")
                {
                    //cboStatus.SelectedItem.Text = strStatus;
                    cboStatus.SelectedValue = strStatus;
                }
                else
                    cboStatus.SelectedIndex = 0;
                if (strStatus == "Dead")
                {
                    cboCauseofDeath.Enabled = true;
                    // dtDOD.Enabled = true;
                    txtDOD.Enabled = true;

                    txtStatusDate.Text = "";
                    txtStatusDate.Enabled = false;

                }
                else
                {

                    txtDOD.Enabled = false;
                }



                DataSet ds1 = GlobalValues.ExecuteDataSet(" select '' as CauseOfDeath union select distinct (CauseOfDeath) from FixedDrops");
                cboCauseofDeath.DataSource = ds1;
                cboCauseofDeath.DataTextField = "CauseOfDeath";
                cboCauseofDeath.DataValueField = "CauseOfDeath";
                cboCauseofDeath.DataBind();
                string strcauseofdeath = Convert.ToString(GlobalValues.ExecuteScalar("select causeofdeath from patientdetails_1 where patient ='" + strPatientId + "'"));
                if (strcauseofdeath == null) { strcauseofdeath = ""; }
                if (strcauseofdeath != "")
                {
                    //cboCauseofDeath.SelectedItem.Text = strcauseofdeath;
                    cboCauseofDeath.SelectedValue = strcauseofdeath;
                }
                else
                {
                    cboCauseofDeath.SelectedIndex = 0;
                }

                string dtVals;
                try
                {
                    dtVals = Convert.ToString(GlobalValues.ExecuteScalar("select convert(Varchar(11),dateofdeath) dateofdeath from patientdetails_1 where patient ='" + strPatientId + "'"));
                    if (dtVals == null) { dtVals = ""; }
                    if (dtVals != "")
                    {
                        //dtDOD.CalendarDate = Convert.ToDateTime(dtVals);
                        txtDOD.Text = Convert.ToDateTime(dtVals).ToString("MMMM d, yyyy");
                        //calDOD.SelectedDate = Convert.ToDateTime(dtVals);
                    }
                    dtVals = Convert.ToString(GlobalValues.ExecuteScalar("select convert(Varchar(11),statusdate) statusdate from patientdetails_1 where patient ='" + strPatientId + "'"));
                    if (dtVals == null) { dtVals = ""; }
                    if (dtVals != "")
                    {
                        //dtStatusDate.CalendarDate = Convert.ToDateTime(dtVals);
                        txtStatusDate.Text = Convert.ToDateTime(dtVals).ToString("MMMM d, yyyy");
                        //calStatusDate.SelectedDate = Convert.ToDateTime(dtVals);
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProjectForm.aspx");
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchPatient.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";
            string lngG_PatId = Convert.ToString(Session["PatientID"]);
            string CboStatus = cboStatus.SelectedValue.ToString();
            string strstatusdate = txtStatusDate.Text.ToString();//dtStatusDate.CalendarDateString.ToString();
            string strDateofdeath = txtDOD.Text.ToString();//dtDOD.CalendarDateString.ToString();
            bool UpdateVals = true;
            string strMsg = string.Empty;
            if (cboStatus.SelectedItem.ToString() == "Dead")
            {

                if (txtDOD.Text == "")
                {
                    strMsg = "Pleae select the Death Date.";
                    UpdateVals = false;
                }
                else
                {

                    string txtboxText = txtDOD.Text;
                    if (txtboxText != null)
                        if (txtboxText != string.Empty)
                            strMsg += GlobalValues.DateValidation(Session["PatientID"].ToString(), "Status", GlobalValues.gDVDateOfDeath, Convert.ToDateTime(txtboxText), Convert.ToDateTime(txtboxText), 50);
                    if (strMsg != string.Empty)
                        UpdateVals = false;
                }
                if (cboCauseofDeath.SelectedValue == "")
                {
                    strMsg = "You have not entered the Cause of Death. Please select the Cause of Death from the drop-down.";
                    UpdateVals = false;
                }
            }


            if (cboStatus.SelectedValue == "")
            {
                strMsg = strMsg + "Please select a Status. ";
                UpdateVals = false;
            }
            var DateOfDiagnosis = GlobalValues.ExecuteScalar("select isnull(DateOfDiagnosis,'') from PatientDetails_1  where Patient='" + Session["PatientID"].ToString() + "' ");
            if (DateOfDiagnosis.ToString().Trim() == "")
            {
                strMsg = strMsg + "Please enter Date of Diagnosis. ";
                UpdateVals = false;
            }
            else if (Convert.ToDateTime(DateOfDiagnosis.ToString().Trim()).Year == 1900)
            {
                strMsg = strMsg + "Please enter Date of Diagnosis. ";
                UpdateVals = false;
            }


            if (UpdateVals)
            {
                // if (dtStatusDate.CalendarDateString.ToString() == "")
                if (txtStatusDate.Text.ToString() == "" && cboStatus.SelectedValue != "Dead")
                {
                    strMsg = "Please select the Status Date. ";
                    UpdateVals = false;
                }
                else
                {
                    //string txtboxText = txtStatusDate.Text;
                    //if (txtboxText != null)
                    //    if (txtboxText != string.Empty)
                    //        strMsg += GlobalValues.DateValidation(Session["PatientID"].ToString(), "Status", GlobalValues.gDVStatusDate, Convert.ToDateTime(txtboxText), Convert.ToDateTime(txtboxText), 50);
                    //if (strMsg != string.Empty)
                    //    UpdateVals = false;
                }
            }
            if (cboStatus.SelectedValue == "Dead")
            {
                if (txtDOD.Text.ToString() == "")
                {
                    if (txtStatusDate.Text.ToString() != "") { strDateofdeath = txtStatusDate.Text.ToString(); }
                }
                if (cboCauseofDeath.SelectedValue.ToString() == "")
                {
                    strMsg = " If the patient is dead, Cause of Death has to be mentioned.";
                    UpdateVals = false;
                }
                if ((strDateofdeath == ""))
                {
                    strMsg = " If the patient is dead Date of death has to be mentioned. If not available, please mention last Status Date.";
                    UpdateVals = false;
                }
            }

            try
            {


                if (UpdateVals)
                {
                    if (cboStatus.SelectedValue != "Dead")
                    {
                        string sqlStr = "UPDATE PatientDetails_1 SET PatientDetails_1.Status = '" + CboStatus + "',PatientDetails_1.StatusDate = '" + strstatusdate + "', PatientDetails_1.CauseOfDeath = '', PatientDetails_1.DateOfDeath = NULL  where patient ='" + lngG_PatId + "'";
                        GlobalValues.ExecuteNonQuery(sqlStr);
                        GlobalValues.SaveSessionLog(Session["PatientID"].ToString(), Session["Login"].ToString(), GlobalValues.SLogStatus);
                        lblErrorMsg.Text = strMsg.ToString();//Code modified on March 06-2015,Subhashini    
                        lblErrorMsg.ForeColor = GlobalValues.SucessColor;//Code modified on March 31-2015,Subhashini
                        lblErrorMsg.Text = "Status details updated successfully.";
                        // ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Status details updated successfully.');", true);//Code modified on March 31-2015,Subhashini
                    }
                    else
                    {
                        if ((strDateofdeath != "") && (DateTime.Parse(txtDOD.Text) > DateTime.Today))
                        {
                            lblErrorMsg.Text = "Death Date Should not Enter Greater than the Current Date.";//Code modified on March 06-2015,Subhashini
                            ScriptManager.RegisterStartupScript(this, typeof(string), "DOD", "alert('Death Date Should not Enter Greater than the Current Date.');", true);
                        }
                        else
                        {

                            string sqlStr = "UPDATE PatientDetails_1 SET PatientDetails_1.Status = '" + CboStatus + "',PatientDetails_1.StatusDate = null,PatientDetails_1.CauseOfDeath = '" + cboCauseofDeath.SelectedValue.ToString() + "', PatientDetails_1.DateOfDeath ='" + txtDOD.Text + "'  where patient ='" + lngG_PatId + "'";
                            GlobalValues.ExecuteNonQuery(sqlStr);
                            GlobalValues.SaveSessionLog(Session["PatientID"].ToString(), Session["Login"].ToString(), GlobalValues.SLogStatus);
                            lblErrorMsg.ForeColor = GlobalValues.SucessColor;//Code modified on March 31-2015,Subhashini
                            lblErrorMsg.Text = "Status details updated successfully.";
                            // ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Status details updated successfully.');", true);
                        }
                    }

                }
                else
                {
                    lblErrorMsg.ForeColor = GlobalValues.FailureColor;//Code modified on March 31-2015,Subhashini
                    lblErrorMsg.Text = strMsg.ToString();//Code modified on March 06-2015,Subhashini
                    // ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('" + strMsg + "');", true);
                }

            }
            catch (Exception ex)
            {

                if (ex.Message.ToUpper().Contains("DATE"))
                {
                    strMsg = "Status update failed. Please select a valid date.";
                    lblErrorMsg.Text = strMsg.ToString();//Code modified on March 06-2015,Subhashini
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('" + strMsg + "');", true);
                }
            }
        }



        protected void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";
            if (cboStatus.SelectedValue == "Dead")
            {
                cboCauseofDeath.Enabled = true;
                txtDOD.Enabled = true;

                txtStatusDate.Text = "";
                txtStatusDate.Enabled = false;

                cboCauseofDeath.SelectedIndex = 0;
                txtDOD.Text = "";


            }
            else
            {
                cboCauseofDeath.SelectedIndex = 0;
                //cboCauseofDeath.SelectedValue = "N/A";
                cboCauseofDeath.Enabled = false;
                txtDOD.Text = "";
                txtDOD.Enabled = false;
                txtStatusDate.Enabled = true;


                txtDOD.Text = "";
                if (cboStatus.SelectedValue == "")
                {
                    //calStatusDate.ClearTime = true;
                    //calDOD.ClearTime = true;
                }
            }
        }

        protected void cboCauseofDeath_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (Session["PatientID"] == null)
            {
                Response.Redirect("SearchPatient.aspx");
            }

            cboStatus.SelectedIndex = 0;
            txtStatusDate.Enabled = true;
            txtStatusDate.Text = "";
            cboCauseofDeath.ClearSelection();
            cboCauseofDeath.Enabled = false;
            lblErrorMsg.Text = "";//Code modified on March 06-2015,Subhashini
            txtDOD.Text = "";
            txtDOD.Enabled = false;
            //calStatusDate.ClearTime = true;
            //calDOD.ClearTime = true;
            //string strPatientId = Convert.ToString(Session["PatientID"]);
            //calDOD.EndDate = DateTime.Today;
            //calDOD.StartDate = DateTime.Today;
            //cboCauseofDeath.Enabled = false;
            //txtDOD.Enabled = false;
            //txtDOD.Text = string.Empty;

            //GlobalValues.gPostBackURL = Request.Url.AbsoluteUri.ToString();
            //DataSet ds = GlobalValues.ExecuteDataSet(" select '' as status union select status from patientstatus");
            //cboStatus.DataSource = ds;
            //cboStatus.DataTextField = "status";
            //cboStatus.DataValueField = "status";
            //cboStatus.DataBind();
            //string strStatus = Convert.ToString(GlobalValues.ExecuteScalar("select status from patientdetails_1 where patient = '" + strPatientId + "'"));
            //if (strStatus == null) { strStatus = ""; }
            //if (strStatus != "")
            //{
            //    //cboStatus.SelectedItem.Text = strStatus;
            //    cboStatus.SelectedValue = strStatus;
            //}
            //if (strStatus == "Dead")
            //{
            //    cboCauseofDeath.Enabled = true;
            //    // dtDOD.Enabled = true;
            //    txtDOD.Enabled = true;
            //    imgDOD.Visible = true;
            //    txtStatusDate.Text = "";
            //    txtStatusDate.Enabled = false;
            //    imgDate.Visible = false;
            //}
            //else
            //{
            //    imgDOD.Visible = false;
            //    txtDOD.Enabled = false;
            //}
            //DataSet ds1 = GlobalValues.ExecuteDataSet(" select '' as CauseOfDeath union select distinct (CauseOfDeath) from FixedDrops");

            //cboCauseofDeath.DataSource = ds1;
            //cboCauseofDeath.DataTextField = "CauseOfDeath";
            //cboCauseofDeath.DataValueField = "CauseOfDeath";
            //cboCauseofDeath.DataBind();
            //string strcauseofdeath = Convert.ToString(GlobalValues.ExecuteScalar("select causeofdeath from patientdetails_1 where patient ='" + strPatientId + "'"));
            //if (strcauseofdeath == null) { strcauseofdeath = ""; }
            //if (strcauseofdeath != "")
            //{
            //    //cboCauseofDeath.SelectedItem.Text = strcauseofdeath;
            //    cboCauseofDeath.SelectedValue = strcauseofdeath;
            //}
            //else
            //{
            //    cboCauseofDeath.SelectedValue = "N/A";
            //}

            //string dtVals;
            //try
            //{
            //    dtVals = Convert.ToString(GlobalValues.ExecuteScalar("select convert(Varchar(11),dateofdeath) dateofdeath from patientdetails_1 where patient ='" + strPatientId + "'"));
            //    if (dtVals == null) { dtVals = ""; }
            //    if (dtVals != "")
            //    {
            //        //dtDOD.CalendarDate = Convert.ToDateTime(dtVals);
            //        txtDOD.Text = dtVals;
            //        calDOD.SelectedDate = Convert.ToDateTime(dtVals);
            //    }
            //    dtVals = Convert.ToString(GlobalValues.ExecuteScalar("select convert(Varchar(11),statusdate) statusdate from patientdetails_1 where patient ='" + strPatientId + "'"));
            //    if (dtVals == null) { dtVals = ""; }
            //    if (dtVals != "")
            //    {
            //        //dtStatusDate.CalendarDate = Convert.ToDateTime(dtVals);
            //        txtStatusDate.Text = dtVals;
            //        calStatusDate.SelectedDate = Convert.ToDateTime(dtVals);
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

        }






    }
}