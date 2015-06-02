﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ePxCollectWeb
{
    public partial class SitesofMet : System.Web.UI.Page
    {
        string patientId;
        protected void Page_Load(object sender, EventArgs e)
        {
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
                Response.Redirect("SearchPatient.aspx");
            }
            patientId = Convert.ToString(Session["PatientID"]).Trim();
            string strSQL;
            if (!IsPostBack)
            {
                //GlobalValues.gPostBackURL = Request.Url.AbsoluteUri.ToString();
                Session["gPostBackURL"] = Request.Url.AbsoluteUri.ToString();
                LstLines.Items.Clear();
                LstSitesofMet.Items.Clear();

                LstLines.Items.Add(new ListItem("1st Line Sites of Metastasis", "1st Line Sites of Metastases"));
                LstLines.Items.Add(new ListItem("2nd Line Sites of Metastasis", "2nd Line Sites of Metastases"));
                LstLines.Items.Add(new ListItem("3rd Line Sites of Metastasis", "3rd Line Sites of Metastases"));
                LstLines.Items.Add(new ListItem("4th Line Sites of Metastasis", "4th Line Sites of Metastases"));
                LstLines.Items.Add(new ListItem("5th Line Sites of Metastasis", "5th Line Sites of Metastases"));
                strSQL = "Select SitesofMet from SitesofMet where SitesofMet <> ''";
                DataSet ds = GlobalValues.ExecuteDataSet(strSQL);
                LstSitesofMet.DataSource = ds.Tables[0];
                LstSitesofMet.DataTextField = "SitesofMet";
                LstSitesofMet.DataValueField = "SitesofMet";
                LstSitesofMet.DataBind();
            }
        }

        protected void LstLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMessage = string.Empty;
            lblErrorMsg.Text = "";
            //lblSave.Visible = false;
            string strsql = "Select [1st RecurrenceDate], [2nd RecurrenceDate],[3rd RecurrenceDate]  , [4th RecurrenceDate] from Recurrences where Patientid='" + patientId + "'";
            DataSet dsR = GlobalValues.ExecuteDataSet(strsql);
            DataRow dr;
            if (dsR.Tables[0].Rows.Count > 0)
            {
                dr = dsR.Tables[0].Rows[0];
                if (LstLines.SelectedItem.Text == "2nd Line Sites of Metastasis")
                {
                    if (Convert.ToString(dr["1st RecurrenceDate"]) == "") { strMessage = "Corresponding Recurrance date required."; }
                }
                else
                    if (LstLines.SelectedItem.Text == "3rd Line Sites of Metastasis")
                    {
                        if (Convert.ToString(dr["2nd RecurrenceDate"]) == "") { strMessage = "Corresponding Recurrance date required."; }
                        if (Convert.ToString(dr["1st RecurrenceDate"]) == "") { strMessage = "Corresponding Recurrance date required."; }

                    }
                    else
                        if (LstLines.SelectedItem.Text == "4th Line Sites of Metastasis")
                        {
                            if (Convert.ToString(dr["3rd RecurrenceDate"]) == "") { strMessage = "Corresponding Recurrance date required."; }
                            if (Convert.ToString(dr["2nd RecurrenceDate"]) == "") { strMessage = "Corresponding Recurrance date required."; }
                            if (Convert.ToString(dr["1st RecurrenceDate"]) == "") { strMessage = "Corresponding Recurrance date required."; }


                        }
                        else
                            if (LstLines.SelectedItem.Text == "5th Line Sites of Metastasis")
                            {
                                if (Convert.ToString(dr["4th RecurrenceDate"]) == "") { strMessage = "Corresponding Recurrance date required."; }
                                if (Convert.ToString(dr["3rd RecurrenceDate"]) == "") { strMessage = "Corresponding Recurrance date required."; }
                                if (Convert.ToString(dr["2nd RecurrenceDate"]) == "") { strMessage = "Corresponding Recurrance date required."; }
                                if (Convert.ToString(dr["1st RecurrenceDate"]) == "") { strMessage = "Corresponding Recurrance date required."; }
                            }

            }
            else
            {
                if (LstLines.SelectedItem.Text != "1st Line Sites of Metastasis")
                {
                    strMessage = "Corresponding Recurrance date required.";
                }
            }

            if (strMessage.Length > 0)
            {
                for (int i = 0; i <= LstSitesofMet.Items.Count - 1; i++)
                {
                    LstSitesofMet.Items[i].Selected = false;
                }
                LstSitesofMet.Enabled = false;
                btnSave.Enabled = false;
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = strMessage;
                lblErrorMsg.ForeColor = GlobalValues.FailureColor;
                // ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "alert('" + strMessage + "');", true);
            }
            else
            {
                LstSitesofMet.Enabled = true;
                btnSave.Enabled = true;
                strsql = "SELECT [" + LstLines.SelectedItem.Value.ToString() + "] from patientdetails_5 where Patient='" + patientId + "'";
                dsR = GlobalValues.ExecuteDataSet(strsql);
                string[] strVals;
                for (int i = 0; i <= LstSitesofMet.Items.Count - 1; i++)
                {
                    LstSitesofMet.Items[i].Selected = false;
                }
                if (dsR.Tables[0].Rows.Count > 0)
                {
                    //for (int i = 0; i <= LstSitesofMet.Items.Count - 1; i++)
                    //{
                    //    LstSitesofMet.Items[i].Selected = false;
                    //}
                    strVals = dsR.Tables[0].Rows[0][0].ToString().Split(',');
                    for (int j = 0; j <= strVals.Length - 1; j++)
                    {
                        for (int i = 0; i <= LstSitesofMet.Items.Count - 1; i++)
                        {
                            if (LstSitesofMet.Items[i].Text.ToString() == strVals[j].ToString())
                                LstSitesofMet.Items[i].Selected = true;
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strString = string.Empty;
            try
            {
                if (LstLines.SelectedItem != null)
                {

                    for (int i = 0; i <= LstSitesofMet.Items.Count - 1; i++)
                    {
                        if (LstSitesofMet.Items[i].Selected == true)
                        {
                            strString += "," + LstSitesofMet.Items[i].ToString();
                        }
                    }
                    if (strString.Length > 0)
                    {
                        strString = strString.Substring(1);
                        string strSql = "Update Patientdetails_5 set [" + LstLines.SelectedItem.Value.ToString() + "] ='" + strString + "'    where Patient='" + patientId + "'";
                        GlobalValues.ExecuteNonQuery(strSql);
                        lblErrorMsg.Text = "Changes Saved successfully.";
                        lblErrorMsg.ForeColor = GlobalValues.SucessColor;
                        GlobalValues.SaveSessionLog(Session["PatientID"].ToString(), Session["Login"].ToString(), GlobalValues.SLogSiteofMet);
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "alert('Saved changes successfully');", true);
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "SaveAlert(); return false", true);
                        //SaveAlert
                    }
                    else
                    {
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "Please select the Sites of Metastasis."; //Code modified on March 05-2015,Subhashini
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Please select the Sites of Metastasis.');", true);//Code modified on March 05-2015,Subhashini
                    }
                }
                else
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Please select a line of Metastasis.";//Code modified on March 05-2015,Subhashini
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Please select a line of Metastasis.');", true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProjectForm.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";//Code modified on March 05-2015,Subhashini
            //lblSave.Text = "";//Code modified on March 05-2015,Subhashini
            
            //LstLines.ClearSelection();
            //LstSitesofMet.Enabled = false;

            LstSitesofMet.ClearSelection();
        }
    }
}