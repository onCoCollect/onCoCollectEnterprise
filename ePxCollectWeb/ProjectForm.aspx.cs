﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using ePxCollectDataAccess;
using AjaxControlToolkit;
using System.Data.SqlClient;
using System.Web.Services;
using System.Globalization;

namespace ePxCollectWeb
{
    public partial class ProjectForm : System.Web.UI.Page
    {
        string SName = string.Empty;
        string strType = string.Empty;
        string strConns = GlobalValues.strConnString;
        DataSet dsPDFlds = new DataSet();
        DataSet dsCustomValidation = new DataSet();
        string FldName = string.Empty;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }
            //Session["PageURL"] = "false";
            string strSql = string.Empty;
            DataSet dsFormflds = new DataSet();
            string strPatientId = string.Empty;
            string strTag;

            // ModalPopupExtender1.Hide();
            strTag = Convert.ToString(Request.QueryString["Tag"]);
            string strReadOnly = Convert.ToString(Request.QueryString["RO"]);
            strType = Convert.ToString(Request.QueryString["Type"]);
            if (strType == null) { strType = ""; }
            //GlobalValues.gPostBackURL = Request.Url.AbsoluteUri.ToString(); //.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.ToString().LastIndexOf("/"));
            Session["gPostBackURL"] = Request.Url.AbsoluteUri.ToString();
            if (Session["PatientID"] == null && Session["Flag"] == null)
            {

                Session["Message"] = "Please pick a Patient.";//Request.UrlReferrer.ToString().IndexOf("Register.aspx") != -1
                if (Request.UrlReferrer.ToString().IndexOf("StatusReminder.aspx") != -1 || Request.UrlReferrer.ToString().IndexOf("ExportDataToStudy.aspx") != -1)
                {
                    Session["Message"] = null;
                }
                if (Session["MenuSelected"] != null)
                {
                    if (Convert.ToString(Session["MenuSelected"]) == "Data Analysis" || Convert.ToString(Session["MenuSelected"]) == "Configuration" || Convert.ToString(Session["MenuSelected"]) == "Home")
                    {
                        Session["Message"] = null;
                        Session["MenuSelected"] = null;
                    }
                    else
                        Response.Redirect("SearchPatient.aspx");
                }
                else
                    Response.Redirect("SearchPatient.aspx");

            }
            else if (Session["PatientID"] == null && Session["Flag"] != null)
            {
                if (Session["Flag"] != null)
                {

                    if (Session["Flag"].ToString() == "AnalysisResults")
                    {

                        Session["Message"] = null;

                        Response.Redirect("SearchPatient.aspx", false);

                        return;

                    }

                    else if (Session["Flag"].ToString() == "DrugAnalysis" || Session["Flag"].ToString() == "SavedQueries")
                    {

                        Session["Message"] = null;

                        Response.Redirect("SearchPatient.aspx", false);
                        return;

                    }
                    else if (Session["Flag"].ToString() == "Stats")
                    {
                        Session["Message"] = null;

                        Response.Redirect("SearchPatient.aspx", false);

                        return;

                    }

                }


            }
            else
            {
                strPatientId = Convert.ToString(Session["PatientID"]);
            }

            if (strReadOnly == "True")
                btnEdit.Visible = false;


            //if (IsPostBack)

            //if (Convert.ToString(Session["MenuChange"]) != "Yes" || (IsPostBack ==false) ||  Session["EditMode"] == "Edit")
            SName = Convert.ToString(Request.QueryString["SN"]);

            if (SName == null) { SName = ""; }

            //New Code
            //if ((strTag != null && strTag != "") || (SName != "" && strType != ""))
            if ((SName != "" && strType != ""))
            {
                // lblTitle.Text = SName;
                if (SName == "OTHER")
                {
                    strSql = Session["SQLSTROTHER"].ToString();
                    string pdSQL = strSql.Replace("Select ", "").Replace("],[", ",").Replace("[", "").Replace("]", "");
                    pdSQL = "Select * from PDFields with (nolock) where IsActive=1  and ConfigurableInScreens=1 and [Field Name] in ('"
                          + pdSQL.Replace(",", "','").Trim()
                          + "') order by [Table Name],FieldOrder";

                    strSql = Session["SQLSTROTHER"].ToString().Replace("[PatientID]", "PatientDetails_0.[PatientID]") + GlobalValues.glbFromClause.ToString() + " Where PatientDetails_0.PatientID='" + strPatientId + "'";

                    dsPDFlds = SqlHelper.ExecuteDataset(strConns, CommandType.Text, pdSQL);
                }
                else
                {

                    if (strType.Contains("Study"))
                    {
                        if (Session["gMenuDropVal"] == null)
                            Session["gMenuDropVal"] = "";
                        strSql = "Select FieldListCSV  from PDScreenMaster_Study with (nolock)where StudyName='" + Session["gMenuDropVal"].ToString() + "' and ScreenName='" + SName + "'";
                        Session["StudyName"] = Session["gMenuDropVal"].ToString();// GlobalValues.gMenuDropVal.ToString();
                    }
                    else if (strType.Contains("SiteOfPrimary")) //(strType.Contains("Diagnosis"))
                    {
                        var SiteofPrimary = GlobalValues.ExecuteScalar("select Siteofprimary from PatientDetails_1 where Patient='" + Session["PatientID"].ToString() + "' ");
                        if (Convert.ToString(SiteofPrimary) != string.Empty && Convert.ToString(SiteofPrimary).Trim() != "-")
                            strSql = "Select FieldListCSV from PDScreenMaster_Diagnosis with (nolock)  where ScreenName='" + SName + "' and  DiseaseName= '" + Convert.ToString(SiteofPrimary) + "' ";
                        else
                        {
                            string LineTreatment = Convert.ToString(Request.QueryString["LineTreatment"]);
                            if (LineTreatment == "LineTreatment")
                            {
                                var siteofPrimary = GlobalValues.ExecuteScalar("select [SiteOfPrimary] from PatientDetails_1 where Patient='" + Session["PatientId"].ToString() + "' ");
                                if (siteofPrimary.ToString().Trim() == string.Empty || siteofPrimary.ToString().Trim() == "-")
                                {
                                    strSql = "";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "checkRedirectPage", "checkRedirectPage('Select a valid Site of Primary for the selected Patient.');", true);
                                    return;
                                }
                                else
                                    strSql = "Select FieldListCSV from PDScreenMaster_Preset with (nolock) where ScreenName='" + SName + "'"; //Buttonindex= " + strTag;

                            }
                            else
                                strSql = "Select FieldListCSV from PDScreenMaster_Preset with (nolock) where ScreenName='" + SName + "'"; //Buttonindex= " + strTag;

                        }

                    }
                    else if (strType.Contains("Custom"))
                    { strSql = "Select FieldListCSV from PDScreenMaster_Custom with (nolock)  where ScreenName='" + SName + "'"; }
                    else
                    {
                        string LineTreatment = Convert.ToString(Request.QueryString["LineTreatment"]);
                        if (LineTreatment == "LineTreatment")
                        {
                            string strSQL = "Select Patient, convert(Varchar(11),DateofDiagnosis) DateofDiagnosis , convert(Varchar(11),DateofDeath) DateofDeath,  convert(Varchar(11),[1st RecurrenceDate]) as Rec1, convert(Varchar(11),[2nd RecurrenceDate]) Rec2, convert(Varchar(11),[3rd RecurrenceDate]) Rec3, convert(Varchar(11),[4th RecurrenceDate]) Rec4  from Patientdetails_1 p with (nolock) left outer join Recurrences R with (nolock) on P.Patient=R.PatientId  where P.Patient='" + strPatientId + "'";
                            DataSet dsRec = GlobalValues.ExecuteDataSet(strSQL);
                            var siteofPrimary = GlobalValues.ExecuteScalar("select [SiteOfPrimary] from PatientDetails_1 where Patient='" + Session["PatientId"].ToString() + "' ");
                            if (siteofPrimary.ToString().Trim() == string.Empty || siteofPrimary.ToString().Trim() == "-")
                            {
                                strSql = "";
                                ScriptManager.RegisterStartupScript(this, GetType(), "checkRedirectPage", "checkRedirectPage('Select a valid Site of Primary for the selected Patient.');", true);
                                return;
                            }
                            else
                                strSql = "Select FieldListCSV from PDScreenMaster_Preset with (nolock) where ScreenName='" + SName + "'"; //Buttonindex= " + strTag;

                            if (dsRec.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = dsRec.Tables[0].Rows[0];
                                if (!string.IsNullOrEmpty(SName) && SName == "2nd Line Treatment")
                                {
                                    if (Convert.ToString(dr["Rec1"]) == "")
                                    {
                                        strSql = "";
                                        ScriptManager.RegisterStartupScript(this, GetType(), "checkRedirectPage", "checkRedirectPage('Select a valid 1st Recurrence Date.');", true);
                                        return;
                                    }
                                }
                                if (!string.IsNullOrEmpty(SName) && SName == "3rd Line Treatment")
                                {
                                    if (Convert.ToString(dr["Rec2"]) == "")
                                    {
                                        strSql = "";
                                        ScriptManager.RegisterStartupScript(this, GetType(), "checkRedirectPage", "checkRedirectPage('Select a valid 2nd Recurrence Date.');", true);
                                        return;
                                    }
                                }
                                if (!string.IsNullOrEmpty(SName) && SName == "4th Line Treatment")
                                {
                                    if (Convert.ToString(dr["Rec3"]) == "")
                                    {
                                        strSql = "";
                                        ScriptManager.RegisterStartupScript(this, GetType(), "checkRedirectPage", "checkRedirectPage('Select a valid 3rd Recurrence Date.');", true);
                                        return;
                                    }
                                }
                                if (!string.IsNullOrEmpty(SName) && SName == "5th Line Treatment")
                                {
                                    if (Convert.ToString(dr["Rec4"]) == "")
                                    {
                                        strSql = "";
                                        ScriptManager.RegisterStartupScript(this, GetType(), "checkRedirectPage", "checkRedirectPage('Select a valid 4th Recurrence Date.');", true);
                                        return;
                                    }
                                }
                            }


                        }
                        else
                            strSql = "Select FieldListCSV from PDScreenMaster_Preset with (nolock) where ScreenName='" + SName + "'"; //Buttonindex= " + strTag;
                    }
                    if (strSql != "")
                    {
                        dsFormflds = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strSql);
                    }
                    //strSql = GlobalValues.PrepareSelectFromCSV(dsFormflds.Tables[0].Rows[0][0].ToString());
                    if (dsFormflds.Tables.Count > 0)
                    {
                        if (dsFormflds.Tables[0].Rows.Count > 0)
                        {
                            dsFormflds.Tables[0].Rows[0][0] = dsFormflds.Tables[0].Rows[0][0].ToString().Replace(", ", ",");
                            dsFormflds.AcceptChanges();
                            strSql = dsFormflds.Tables[0].Rows[0][0].ToString().Replace("[", "").Replace("]", "");

                            string strPDSQL = "Select * from PDFields with (nolock) where IsActive=1  and [Field Name] in ('"
                                       + strSql.Replace(",", "','")
                                       + "') ";
                            if (strType.Contains("Custom"))
                                strPDSQL += "   order by [Table Name]";
                            else
                                strPDSQL += "   order by [Table Name],FieldOrder";

                            Session["UpdSQL"] = strSql;
                            strSql = "[" + strSql.Replace(",", "],[") + "]";
                            strSql = strSql + GlobalValues.glbFromClause.ToString() + " Where PatientDetails_0.PatientID='" + strPatientId + "'";
                            strSql = (strSql.Contains("Select ") ? "" : "Select ") + strSql;
                            //strSql = dsFormflds.Tables[0].Rows[0][0].ToString() + GlobalValues.glbFromClause.ToString() + " Where PatientDetails_0.PatientID='" + strPatientId + "'";
                            // Session["UpdSQL"] = dsFormflds.Tables[0].Rows[0][0].ToString(); 

                            dsPDFlds = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strPDSQL);
                        }
                        else
                        { strSql = ""; }
                    }

                    else
                    {
                        //strSql = "Select FieldListCSV from PDScreenMaster_Preset where 1=0";
                        strSql = "";
                    }
                }

                if (strSql != "")
                {
                    dsFormflds = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strSql);
                    Session["EmptyControls"] = null;
                    LoadControls(dsFormflds);
                }
                else
                {
                    phProject.Controls.Clear();
                    Panel1.Visible = false;
                    btnEdit.Visible = false;
                    //btnRegister.Visible = false;Code modified on March 26-2015,Subhashini
                    Panel2.Visible = false;
                    updPHControls.Update();
                    Session["EmptyControls"] = true;
                    string LineTreatment = Convert.ToString(Request.QueryString["LineTreatment"]);
                    if (LineTreatment == "LineTreatment")
                    {
                        Response.Redirect("ProjectForm.aspx", false);
                    }
                }
                if (IsPostBack)
                {

                    if (Convert.ToString(Session["EditMode"]) == "" || Convert.ToString(Session["EditMode"]) != "Edit")
                    {
                        EnableDisableControls(false);
                    }

                }

            }
            else
            {
                phProject.Controls.Clear();
                Panel1.Visible = false;
                btnEdit.Visible = false;
                //btnRegister.Visible = false;Code modified on March 26-2015,Subhashini
                Panel2.Visible = false;
                updPHControls.Update();
                Session["EmptyControls"] = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ResetPassword"] != null)
            {
                Session["ResetPasswordMsg"] = "Please Change your password.";
                Response.Redirect("Changepassword.aspx");
            }
            //TextBox test = (TextBox)phProject.FindControl("Rectum:_-Distance_-from_-Anal_-Verge-in_-cm_TX_Y");
            //string fdf = test.Text;
            string SName = Convert.ToString(Request.QueryString["SN"]);
            if (SName == "Demographics" || strType == "Custom")
            {
                btnSaveProjectForm.Attributes.Add("onclick", "return ValidateTextBoxForDataTypeEmailFormat();");
            }

            dsCustomValidation = GlobalValues.ExecuteDataSet("select isnull([Relation Parent Field] ,0) as  ParentField,[Field Name] as FieldName,* from PDFields  where [Relation Validated Group] >0   order by  [Relation Validated Group] asc,[Relation Parent Field] desc");


        }

        private void LoadControls(DataSet dsForm)
        {
            try
            {
                string strFldVal;
                OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
                foreach (DataColumn dcValues in dsForm.Tables[0].Columns)
                {
                    strFldVal = string.Empty;
                    DataRow[] dr = dsPDFlds.Tables[0].Select("[Field Name]='" + dcValues.ColumnName.ToString() + "'");
                    Label lbCtl = new Label();
                    if (dr.Length > 0)
                    {
                        DataRow drValues = dsForm.Tables[0].Rows[0];


                        string LabelName = dcValues.ColumnName.ToString().Replace("Of", "of");
                        lbCtl.Text = LabelName + "&nbsp;&nbsp;";

                        //Demographics
                        if (dcValues.ColumnName == "DateOfBirth")
                            lbCtl.Text = "Date of Birth&nbsp;&nbsp;";
                        if (dcValues.ColumnName == "City_Town")
                            lbCtl.Text = "City/Town&nbsp;&nbsp;";
                        if (dcValues.ColumnName == "PhoneNumber")
                            lbCtl.Text = "Phone Number&nbsp;&nbsp;";
                        if (dcValues.ColumnName == "EmailID")
                            lbCtl.Text = "Email ID&nbsp;&nbsp;";
                        if (dcValues.ColumnName == "ConsultantsName")
                            lbCtl.Text = "Consultants&nbsp;&nbsp;";
                        if (dcValues.ColumnName == "Height - in Cm")
                            lbCtl.Text = "Height (Cm)&nbsp;&nbsp;";
                        if (dcValues.ColumnName == "Weight in KG -at Diagnosis")
                            lbCtl.Text = "Weight (KG) at Diagnosis&nbsp;&nbsp;";
                        if (dcValues.ColumnName == "Weight in KG -at Diagnosis")
                            lbCtl.Text = "Weight (KG) at Diagnosis&nbsp;&nbsp;";
                        if (dcValues.ColumnName == "HospitalCode")
                            lbCtl.Text = "Hospital Code&nbsp;&nbsp;";

                        //Disease Details
                        if (dcValues.ColumnName == "SiteOfPrimary")
                            lbCtl.Text = "Site of Primary &nbsp;&nbsp;";
                        if (dcValues.ColumnName == "DateOfDiagnosis")
                            lbCtl.Text = "Date of Diagnosis &nbsp;&nbsp;";
                        if (dcValues.ColumnName == "PathalogicalStage")
                            lbCtl.Text = "Pathalogical Stage &nbsp;&nbsp;";
                        if (dcValues.ColumnName == "RadioTherapy")
                            lbCtl.Text = "Radio Therapy &nbsp;&nbsp;";
                        if (dcValues.ColumnName == "SystemicTherapy")
                            lbCtl.Text = "Systemic Therapy &nbsp;&nbsp;";
                        if (dcValues.ColumnName == "StatusPost1stRx")
                            lbCtl.Text = "Status Post 1st Rx &nbsp;&nbsp;";
                        if (dcValues.ColumnName == "SecondPrimary")
                            lbCtl.Text = "Second Primary &nbsp;&nbsp;";
                        if (dcValues.ColumnName == " 1st RecurrencePattern")
                            lbCtl.Text = " 1st Recurrence Pattern &nbsp;&nbsp;";
                        if (dcValues.ColumnName == "SecondPrimary_Details")
                            lbCtl.Text = "Second Primary_Details&nbsp;&nbsp;";
                        if (dcValues.ColumnName == "M_Metastases")
                            lbCtl.Text = "M_Metastasis&nbsp;&nbsp;";

                        //Surgery
                        if (dcValues.ColumnName == "TypeOfSurgery")
                            lbCtl.Text = "Type of Surgery &nbsp;&nbsp;";
                        if (dcValues.ColumnName == "DateOfSurgery")
                            lbCtl.Text = "Date of Surgery &nbsp;&nbsp;";
                        if (dcValues.ColumnName == "AncillaryDetail")
                            lbCtl.Text = "Ancillary Detail &nbsp;&nbsp;";
                        if (dcValues.ColumnName == "Rectum: Distance from Anal Verge-in cm")
                            lbCtl.Text = "Rectum Distance from Anal Verge-in cm &nbsp;&nbsp;";

                        lbCtl.Width = 310;
                        lbCtl.CssClass = "LabelRight";
                        ViewState["LabelText"] = dcValues.ColumnName.ToString();
                        phProject.Controls.Add(lbCtl);
                        if (!(bool)dr[0]["Calculated Field"])
                        {
                            strFldVal = drValues[dcValues.ColumnName].ToString();
                            if ((bool)dr[0]["Encrypt"])
                            {
                                try
                                {
                                    strFldVal = objEnc.Decrypt(strFldVal);
                                }
                                catch (Exception)
                                {


                                }

                            }



                            if (dcValues.ColumnName.Contains("Line Drug Group"))
                            {
                                TextBox txtCtl = new TextBox();
                                Button btnClick = new Button();

                                txtCtl.Text = (strFldVal.ToString() == "0" ? "" : strFldVal.ToString());
                                //  txtCtl.Width = 250;
                                txtCtl.CssClass = "dynamictext";// "inputBox";
                                //txtCtl.Width = strFldVal.ToString().Length <= 0 ? 250 : (strFldVal.ToString().Length * 8) + 50;
                                btnClick.Text = "...";
                                btnClick.ID = "btn" + dcValues.ColumnName;
                                btnClick.UseSubmitBehavior = false;
                                btnClick.CommandArgument = dr[0]["Table Name"].ToString() + "." + dr[0]["Field Name"].ToString();

                                txtCtl.ReadOnly = true;
                                btnClick.OnClientClick = "javascript:fnDrugForm(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"ctl00_MainContent_" + dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y\",\"[" + strFldVal.ToString() + "]\"); return false;";


                                //txtCtl.Enabled = false;                                                               
                                txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y";
                                // txtCtl.Attributes.Add("onkeypress", "return handleKey()");

                                SetValidationsForUserInput(txtCtl, (dr[0]["DataType"]).ToString().ToUpper(), (dr[0]["Field Width"]).ToString().ToUpper());

                                txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                txtCtl.EnableViewState = true;
                                phProject.Controls.Add(txtCtl);
                                phProject.Controls.Add(btnClick);

                            }
                            else
                                if ((string)dr[0]["DataType"] == "DATE")
                                {
                                    try
                                    {
                                        TextBox txtCtl = new TextBox();
                                        txtCtl.CssClass = "dynamictext";// "inputBox";
                                        txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_DT_Y";
                                        txtCtl.Attributes.Add("ReadOnly", "true");

                                        if (strFldVal.ToString() != "")
                                        {
                                            txtCtl.Text = Convert.ToDateTime(strFldVal).ToString("MMMM d, yyyy");
                                        }
                                        phProject.Controls.Add(txtCtl);


                                        //TextBox dtP = new TextBox();
                                        //dtP.ID = dcValues.ColumnName.Replace(" ", "_-") + "_DT_Y";
                                        //dtP.Width = Unit.Pixel(198);
                                        //dtP.Height = 30;//"images/Calendar.png"
                                        //dtP.TextCssClass = "dynamictext";
                                        //dtP.LabelHeaderText = lbCtl.Text.Replace(":", "").Trim();
                                        ////dtP.LabelHeaderVisibility = false;
                                        ////dtP.LabelHeaderID = "lblheaderText";
                                        //if (strFldVal.ToString() != "")
                                        //{
                                        //    if (strFldVal.ToString() == "1/1/1900 12:00:00 AM")
                                        //    {
                                        //        dtP.CalendarDateFormatted = "";
                                        //    }
                                        //    else if (strFldVal.ToString() == "01/01/1900 12:00:00 AM")
                                        //    {
                                        //        dtP.CalendarDateFormatted = "";
                                        //    }
                                        //    else if (Convert.ToDateTime(strFldVal.ToString()).Year == 1900)
                                        //    { dtP.CalendarDateFormatted = ""; }
                                        //    else
                                        //    {
                                        //        dtP.CalendarDate = Convert.ToDateTime(strFldVal.ToString());
                                        //        dtP.CalendarDateFormatted = Convert.ToDateTime(strFldVal).ToString("MMMM d, yyyy");
                                        //    }
                                        //}
                                        //phProject.Controls.Add(dtP);
                                    }
                                    catch (Exception e) { var message = e.Message; }

                                }
                                else
                                    if ((bool)dr[0]["CompositeField"])
                                    {

                                        TextBox txtCtl = new TextBox();
                                        Button btnClick = new Button();
                                        Session["UrlReferrer"] = "false";
                                        if (dcValues.ColumnName == "Total Cost of Primary Treatment(in INR)")
                                            txtCtl.Text = (strFldVal.ToString() == "0" ? "0" : strFldVal.ToString());
                                        else
                                            txtCtl.Text = (strFldVal.ToString() == "0" ? "" : strFldVal.ToString());

                                        //txtCtl.Width = strFldVal.ToString().Length <= 0 ? 250 : (strFldVal.ToString().Length * 8) + 50;
                                        //  txtCtl.Width = 250;
                                        txtCtl.CssClass = "dynamictext";// "inputBox";
                                        //  dynamictext
                                        btnClick.Text = "...";
                                        btnClick.ID = "btn" + dcValues.ColumnName;
                                        btnClick.UseSubmitBehavior = false;
                                        btnClick.CommandArgument = dr[0]["Table Name"].ToString() + "." + dr[0]["Field Name"].ToString();
                                        txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_TX_Y";
                                        btnClick.OnClientClick = "javascript:fnPopWindow(\"" + "CompositeForm.aspx?CN=" + dcValues.ColumnName.Replace(" ", "_-") + "\",\"" + dcValues.ColumnName.ToString() + "\",\"ctl00_MainContent_" + txtCtl.ID + "\");  return false;";
                                        //txtCtl.Enabled = false;
                                        // txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "RTX_Y";
                                        txtCtl.Attributes.Add("onkeypress", "return handleKey()");
                                        txtCtl.Attributes.Add("readonly", "readonly");

                                        SetValidationsForUserInput(txtCtl, (dr[0]["DataType"]).ToString().ToUpper(), (dr[0]["Field Width"]).ToString().ToUpper());

                                        txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                        txtCtl.EnableViewState = true;
                                        phProject.Controls.Add(txtCtl);
                                        phProject.Controls.Add(btnClick);
                                    }

                                    else
                                        if ((bool)dr[0]["RightPickwithGrouping"])
                                        {
                                            TextBox txtCtl = new TextBox();
                                            Button btnClick = new Button();
                                            txtCtl.Text = (strFldVal.ToString() == "0" ? "" : strFldVal.ToString());
                                            txtCtl.Width = 200;
                                            txtCtl.CssClass = "dynamictext";///"inputBox";
                                            //   txtCtl.CssClass = "inputBox";
                                            //txtCtl.Width = strFldVal.ToString().Length <= 0 ? 250 : (strFldVal.ToString().Length * 8) + 50;
                                            btnClick.Text = "...";
                                            btnClick.ID = "btn" + dcValues.ColumnName;
                                            btnClick.UseSubmitBehavior = false;
                                            btnClick.CommandArgument = dr[0]["Table Name"].ToString() + "." + dr[0]["Field Name"].ToString();

                                            if ((bool)dr[0]["RightPickwithGrouping"])
                                            {
                                                txtCtl.ReadOnly = true;
                                                btnClick.OnClientClick = "javascript:fnPopupRightPickwithGrouping(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"ctl00_MainContent_" + dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y\",\"[" + strFldVal.ToString() + "]\"); return false;";

                                            }
                                            //txtCtl.Enabled = false;
                                            txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y";
                                            //txtCtl.Attributes.Add("onkeypress", "return handleKey()");
                                            txtCtl.Attributes.Add("readonly", "readonly");

                                            SetValidationsForUserInput(txtCtl, (dr[0]["DataType"]).ToString().ToUpper(), (dr[0]["Field Width"]).ToString().ToUpper());

                                            txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                            txtCtl.EnableViewState = true;
                                            phProject.Controls.Add(txtCtl);
                                            phProject.Controls.Add(btnClick);
                                        }

                                        else
                                            if ((bool)dr[0]["SequenceOrderingScreen"])
                                            {
                                                TextBox txtCtl = new TextBox();
                                                Button btnClick = new Button();
                                                txtCtl.Text = (strFldVal.ToString() == "0" ? "" : strFldVal.ToString());
                                                txtCtl.Width = 200;
                                                txtCtl.CssClass = "dynamictext";///"inputBox";
                                                //   txtCtl.CssClass = "inputBox";
                                                //txtCtl.Width = strFldVal.ToString().Length <= 0 ? 250 : (strFldVal.ToString().Length * 8) + 50;
                                                btnClick.Text = "...";
                                                btnClick.ID = "btn" + dcValues.ColumnName;
                                                btnClick.UseSubmitBehavior = false;
                                                btnClick.CommandArgument = dr[0]["Table Name"].ToString() + "." + dr[0]["Field Name"].ToString();
                                                txtCtl.ReadOnly = true;
                                                btnClick.OnClientClick = "javascript:fnPopupSequenceOrderingScreen(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"ctl00_MainContent_" + dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y\",\"[" + strFldVal.ToString() + "]\"); return false;";


                                                //txtCtl.Enabled = false;
                                                txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y";
                                                //txtCtl.Attributes.Add("onkeypress", "return handleKey()");
                                                txtCtl.Attributes.Add("readonly", "readonly");

                                                SetValidationsForUserInput(txtCtl, (dr[0]["DataType"]).ToString().ToUpper(), (dr[0]["Field Width"]).ToString().ToUpper());

                                                txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                                txtCtl.EnableViewState = true;
                                                phProject.Controls.Add(txtCtl);
                                                phProject.Controls.Add(btnClick);
                                            }
                                            else
                                                if ((bool)dr[0]["RightPick"] || (bool)dr[0]["SelectiveRightSinglePickField"] || (bool)dr[0]["RightSinglePickFixed"])
                                                {
                                                    TextBox txtCtl = new TextBox();
                                                    Button btnClick = new Button();
                                                    txtCtl.Text = (strFldVal.ToString() == "0" ? "" : strFldVal.ToString());
                                                    txtCtl.Width = 200;
                                                    txtCtl.CssClass = "dynamictext";///"inputBox";
                                                    //   txtCtl.CssClass = "inputBox";
                                                    //txtCtl.Width = strFldVal.ToString().Length <= 0 ? 250 : (strFldVal.ToString().Length * 8) + 50;
                                                    btnClick.Text = "...";
                                                    btnClick.ID = "btn" + dcValues.ColumnName;
                                                    btnClick.UseSubmitBehavior = false;
                                                    btnClick.CommandArgument = dr[0]["Table Name"].ToString() + "." + dr[0]["Field Name"].ToString();
                                                    if ((bool)dr[0]["SelectiveRightSinglePickField"])
                                                    {
                                                        txtCtl.ReadOnly = true;
                                                        btnClick.OnClientClick = "javascript:fnSingleSelect(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"ctl00_MainContent_" + dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y\",\"[" + strFldVal.ToString() + "]\"); return false;";
                                                    }
                                                    else
                                                    {
                                                        btnClick.OnClientClick = "javascript:fnPopup(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"ctl00_MainContent_" + dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y\",\"[" + (strFldVal.ToString()) + "]\"); return false;"; //); return false;";
                                                    }

                                                    //txtCtl.Enabled = false;
                                                    txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y";
                                                    txtCtl.Attributes.Add("readonly", "readonly");

                                                    if ((bool)dr[0]["Relation Validated Field"] && (bool)dr[0]["Relation Parent Field"])
                                                    {
                                                        txtCtl.AutoPostBack = true;
                                                        txtCtl.TextChanged += new EventHandler(CustomValidation);
                                                        txtCtl.Attributes.Add("OnChange", "javascript:return DoPostBack(this)");
                                                    }
                                                    SetValidationsForUserInput(txtCtl, (dr[0]["DataType"]).ToString().ToUpper(), (dr[0]["Field Width"]).ToString().ToUpper());

                                                    txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                                    txtCtl.EnableViewState = true;
                                                    phProject.Controls.Add(txtCtl);
                                                    phProject.Controls.Add(btnClick);
                                                }
                                                else
                                                    if ((bool)dr[0]["RightMultiPick"] || (bool)dr[0]["SelectiveRightMultiPickField"])
                                                    {
                                                        TextBox txtCtl = new TextBox();
                                                        Button btnClick = new Button();
                                                        txtCtl.Text = (strFldVal.ToString() == "0" ? "" : strFldVal.ToString());
                                                        //txtCtl.Width = strFldVal.ToString().Length <= 0 ? 250 : (strFldVal.ToString().Length * 8) + 50;
                                                        txtCtl.Width = 200;
                                                        txtCtl.CssClass = "dynamictext"; //"inputBox";
                                                        btnClick.Text = "...";
                                                        btnClick.ID = "btn" + dcValues.ColumnName;
                                                        btnClick.UseSubmitBehavior = false;

                                                        btnClick.CommandArgument = dr[0]["Table Name"].ToString() + "." + dr[0]["Field Name"].ToString();
                                                        if ((bool)dr[0]["RightMultiPick"])
                                                        {
                                                            btnClick.OnClientClick = "javascript:fnPopupRightMultiPick(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"ctl00_MainContent_" + dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y\"); return false;";
                                                        }
                                                        else
                                                        {
                                                            btnClick.OnClientClick = "javascript:fnMultiSelect(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"ctl00_MainContent_" + dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y\",\"[" + (strFldVal.ToString()) + "]\"); return false;";
                                                        }
                                                        txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_RTX_Y";
                                                        txtCtl.Attributes.Add("readonly", "readonly");

                                                        SetValidationsForUserInput(txtCtl, (dr[0]["DataType"]).ToString().ToUpper(), (dr[0]["Field Width"]).ToString().ToUpper());

                                                        txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                                        txtCtl.EnableViewState = true;


                                                        phProject.Controls.Add(txtCtl);
                                                        phProject.Controls.Add(btnClick);
                                                    }
                                                    else if ((bool)dr[0]["FixedDrop"])
                                                    {
                                                        DropDownList dpLst = new DropDownList();
                                                        //  dpLst.Width = 250;
                                                        dpLst.CssClass = "dllCss";//"inputBox";
                                                        dpLst.ID = dcValues.ColumnName.Replace(" ", "_-") + "_FD_Y";
                                                        PopulateDropDownValues(dr[0]["FieldValues"].ToString(), dpLst);
                                                        dpLst.SelectedValue = strFldVal.ToString();
                                                        if ((bool)dr[0]["Relation Validated Field"] && (bool)dr[0]["Relation Parent Field"])
                                                        {
                                                            dpLst.AutoPostBack = true;//Code Modified by Subhashini on May-4,201
                                                            dpLst.SelectedIndexChanged += new EventHandler(CustomValidation);
                                                        }


                                                        dpLst.ViewStateMode = ViewStateMode.Enabled;

                                                        phProject.Controls.Add(dpLst);
                                                    }
                                                    else
                                                    {
                                                        TextBox txtCtl = new TextBox();
                                                        txtCtl.Text = (strFldVal.ToString() == "0" ? "" : strFldVal.ToString());
                                                        txtCtl.Text = (strFldVal.ToString() == "NULL" ? "" : strFldVal.ToString());
                                                        Int32 intWidth = strFldVal.ToString().Length;
                                                        //txtCtl.Width = intWidth <= 0 ? 250 : intWidth > 50 ? 250 : (intWidth * 8) + 50;
                                                        //   txtCtl.Width = 250;
                                                        txtCtl.CssClass = "dynamictext";// "inputBox";
                                                        //txtCtl.Wrap = true;
                                                        if (intWidth > 50) { txtCtl.TextMode = TextBoxMode.MultiLine; }
                                                        // txtCtl.Enabled = false;
                                                        if (dcValues.ColumnName == "PatientID") { txtCtl.ReadOnly = true; }
                                                        txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_TX_Y";
                                                        txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                                        txtCtl.EnableViewState = true;
                                                        SetValidationsForUserInput(txtCtl, (dr[0]["DataType"]).ToString().ToUpper(), (dr[0]["Field Width"]).ToString().ToUpper());
                                                        phProject.Controls.Add(txtCtl);
                                                    }



                            phProject.Controls.Add(new LiteralControl("<br />"));
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "oncoNotify('" + ex.Message + "', 'E');", true);

            }

            // pnlControls.Enabled = false;
            EnableDisableControls(false);
            //After All Controls Are Loadedd Sucessfully Do Custom Validation on Forms
            CustomValidation(null, null);
            if (phProject.Controls.Count > 0)
            {
                Panel1.Visible = true;
                btnEdit.Visible = true;
                // btnRegister.Visible = true;Code modified on March 26-2015,Subhashini
                //Panel2.Visible = true;
            }
        }

        #region "General Functions"
        void EnableDisableControls(Boolean blStatus)
        {
            //pnlControls.Enabled = blStatus;

            if (blStatus == true)
            {

            }
            foreach (Control ctl in phProject.Controls)
            {
                if (ctl.ID != null)
                {

                    if (ctl.ID.ToString().EndsWith("_Y"))
                    {
                        if (ctl.ID.ToString().EndsWith("_RTX_Y"))
                        {
                            TextBox txtCtl = new TextBox();
                            txtCtl = (TextBox)ctl;
                            txtCtl.ReadOnly = !blStatus;
                            // txtCtl.ReadOnly = true;
                        }
                        else if (ctl.ID.ToString().EndsWith("_TX_Y"))// || ctl.ID.ToString().EndsWith("_DT_Y"))
                        {
                            TextBox txtCtl = new TextBox();
                            txtCtl = (TextBox)ctl;
                            txtCtl.ReadOnly = !blStatus;
                            if (ctl.ID.ToString() == "HospitalCode_TX_Y")
                                txtCtl.ReadOnly = true;
                        }
                        else if (ctl.ID.ToString().EndsWith("_FD_Y"))
                        {
                            DropDownList dpLst = new DropDownList();
                            dpLst = (DropDownList)ctl;
                            dpLst.Enabled = blStatus;
                            //dpLst.Style.Add("color", "#123456");

                        }
                        else
                            if (ctl.ID.ToString().EndsWith("_DT_Y"))
                            {
                                TextBox txtCtl = new TextBox();
                                txtCtl = (TextBox)ctl;
                                if (blStatus == true)
                                {
                                    //txtCtl.ReadOnly = !blStatus;
                                    txtCtl.CssClass = "dynamictext onCoDatePik";
                                    txtCtl.Attributes.Add("ReadOnly", "true");
                                }
                                else
                                    txtCtl.CssClass = "dynamictext";
                                Session["status"] = !blStatus;

                                //TextBox dtP = new TextBox();
                                //dtP = (TextBox)ctl;
                                //dtP.Enabled = blStatus;

                            }

                            else
                                if (ctl.ID.ToString().EndsWith("AjaxCal"))
                                {
                                    CalendarExtender ajaxcal = new CalendarExtender();
                                    ajaxcal = (CalendarExtender)ctl;
                                    ajaxcal.Enabled = (bool)Session["status"];

                                }


                    }
                    else
                    {
                        if (ctl.ID.StartsWith("btn"))
                        {
                            Button bt = new Button();
                            bt = (Button)ctl;
                            bt.Visible = blStatus;
                        }
                        else//for calander image buttons
                        {
                            if (ctl.ID.ToString().EndsWith("DT_IMG"))
                            {
                                ImageButton img = new ImageButton();
                                img = (ImageButton)ctl;
                                img.Enabled = blStatus;
                            }
                        }
                    }
                }
            }
            try
            {
                if (phProject.Controls.Count > 0)
                {
                    Control ctlP = phProject.FindControl("PatientID");
                    TextBox ctlTXT;
                    if (ctlP != null)
                    {
                        ctlTXT = (TextBox)ctlP;
                        ctlTXT.Enabled = false;
                    }

                    Panel1.Visible = true;
                    btnEdit.Visible = true;
                    // btnRegister.Visible = true;Code modified on March 26-2015,Subhashini
                }
            }
            catch (Exception)
            {

                //throw;
            }


        }
        void PopulateDropDownValues(string strCSVs, DropDownList dl)
        {
            string[] strItems;
            strItems = strCSVs.Split(',');
            foreach (string strValue in strItems)
            {
                ListItem lst = dl.Items.FindByText(strValue.Trim());
                if (lst == null)
                {
                    dl.Items.Add(strValue.Trim());
                }
            }
        }
        #endregion

        public Boolean SaveForm()
        {
            string strValue = string.Empty;
            string strTable = string.Empty;
            string strwhere = string.Empty;
            string strCol = string.Empty;
            string strColVal = string.Empty;
            string strUPD = Convert.ToString(Session["UpdSQL"]).Replace("Select", "");
            string[] arrFlds;
            string[] arrTbl;
            OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
            //System.Collections.ArrayList TablesList = new System.Collections.ArrayList();
            string[] arrVals = new string[10];

            arrFlds = strUPD.Split(',');
            foreach (Control ctl in phProject.Controls)
            {
                if (ctl.ID != null)
                {

                    if (ctl.ID.ToString().EndsWith("_Y"))
                    {
                        strCol = ctl.ID.ToString().Substring(0, ctl.ID.Length - 5);
                        if (ctl.ID.ToString().EndsWith("_TX_Y"))
                        {
                            TextBox txtCtl = new TextBox();
                            txtCtl = (TextBox)ctl;
                            strColVal = "'" + txtCtl.Text.Trim().ToString() + "'";
                        }
                        else if (ctl.ID.ToString().EndsWith("_RTX_Y"))
                        {
                            TextBox txtCtl = new TextBox();
                            txtCtl = (TextBox)ctl;

                            strColVal = "'" + txtCtl.Text.Trim().ToString() + "'";
                            strCol = ctl.ID.ToString().Substring(0, ctl.ID.Length - 6);

                        }
                        else if (ctl.ID.ToString().EndsWith("RTX_Y"))
                        {
                            TextBox txtCtl = new TextBox();
                            txtCtl = (TextBox)ctl;

                            strColVal = "'" + txtCtl.Text.Trim().ToString() + "'";
                            strCol = ctl.ID.ToString().Substring(0, ctl.ID.Length - 6);

                        }
                        else if (ctl.ID.ToString().EndsWith("_FD_Y"))
                        {
                            DropDownList dpLst = new DropDownList();
                            dpLst = (DropDownList)ctl;
                            strColVal = "'" + dpLst.SelectedItem.Text.Trim().ToString() + "'";
                            //strColVal = dpLst.SelectedItem.Text.ToString();
                        }
                        else
                            if (ctl.ID.ToString().EndsWith("_DT_Y"))
                            {


                                //TextBox txtCtl = new TextBox();
                                //txtCtl = (TextBox)ctl;
                                //strColVal = "'" + txtCtl.Text + "'";
                                //if (strColVal != "")
                                //{
                                //    if (strColVal == "''")
                                //    {

                                //        strColVal = "''";
                                //    }
                                //    else
                                //        strColVal = "convert(datetime," + "'" + txtCtl.Text + "'," + "103)";
                                //}

                                //else
                                //{
                                //    // strColVal = "convert(datetime," + dtP + ",103)";
                                //    strColVal = "convert(datetime," + null + ",103)";
                                //}

                                //TextBox dtP = new TextBox();
                                //dtP = (TextBox)ctl;
                                //strColVal = "'" + dtP.ToString().Trim() + "'";

                                TextBox dtp = new TextBox();
                                dtp = (TextBox)ctl;
                                strColVal = "'" + dtp.Text + "'";


                                if (strColVal != "")
                                {
                                    if (strColVal == "''")
                                    {

                                        strColVal = "''";
                                    }
                                    else
                                        strColVal = "convert(datetime," + "'" + dtp.Text + "'," + "103)";
                                }

                                else
                                {
                                    // strColVal = "convert(datetime," + dtP + ",103)";
                                    strColVal = "convert(datetime," + null + ",103)";
                                }

                                if (strColVal.Trim() == string.Empty || strColVal.Trim() == "''")
                                    strColVal = "NULL";

                            }
                        //for (int i = 0; i < arrFlds.Length; i++)
                        //{
                        //        arrTbl = arrFlds[i].Split('.');
                        //      strTable = arrTbl[1].ToString().Replace("\n","");
                        //      if (strTable.Contains(strCol.Replace("_-", " ")))
                        //    {
                        //        arrVals[Convert.ToInt32(arrTbl[0].ToString().Substring(arrTbl[0].Length-1))]+=","+ strCol + "=" +strColVal;
                        //        break;
                        //    }
                        //}

                        DataRow[] drC = dsPDFlds.Tables[0].Select("[Field Name] ='" + strCol.Replace("_-", " ") + "'");
                        foreach (DataRow dr in drC)
                        {
                            //DataRow dr = drC.Rows[0];
                            strCol = "[" + strCol + "]";
                            if ((bool)dr["Encrypt"])
                            {
                                //if (dr["DataType"].ToString().ToUpper() != "LONG")
                                strColVal = "'" + objEnc.Encrypt(strColVal.Replace("'", "")) + "'";
                            }
                            if (strColVal.Trim() != string.Empty && strColVal.Trim() == "''")
                            { strColVal = "null"; }
                            arrVals[Convert.ToInt32(dr["Table Name"].ToString().Substring(dr["Table Name"].ToString().Length - 1))] += "," + strCol + "=" + strColVal;
                        }

                        strValue += "," + strCol + strColVal;
                    }

                }
            }
            try
            {


                for (int j = 0; j < arrVals.Length; j++)
                {
                    strValue = arrVals[j];
                    if (strValue != null)
                    {
                        strValue = strValue.ToString().Substring(1).Replace("_-", " ");
                        strTable = "PatientDetails_" + j.ToString();
                        if (strTable == "PatientDetails_0")
                        {
                            strwhere = "  Where PatientiD = '" + Convert.ToString(Session["PatientID"]).Trim() + "'";
                        }
                        else
                        {
                            strwhere = "  Where Patient = '" + Convert.ToString(Session["PatientID"]).Trim() + "'";
                        }
                        strValue = "Update " + strTable + " Set " + strValue + strwhere;
                        Int32 updStat;
                        updStat = SqlHelper.ExecuteNonQuery(strConns, CommandType.Text, strValue);
                    }

                }
                UpdatePatientDetailsSessionValueInMasterPage();
                GlobalValues.UpdateModifiedDate(Convert.ToString(Session["PatientID"]), Convert.ToString(Session["Login"]).ToString());
                //audit required only for Patient Change
                GlobalValues.WriteAuditRecord(Convert.ToString(Session["Login"]), Convert.ToString(Session["PatientID"]), "Record Updated", GlobalValues.strEnterpriseDataBaseName);
                return true;
            }
            catch (Exception ex)
            {

                //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Save operation failed, Please check the accuracy of data and try again.');", true);
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "oncoNotify('Save operation failed, Please check the accuracy of data and try again.', 'E');", true);
                return false;
            }
        }


        private void UpdatePatientDetailsSessionValueInMasterPage()
        {
            string strSql = "SELECT  PatientDetails_0.PatientID as 'Patient ID',PatientDetails_0.PatientName as 'Patient Name',PatientDetails_0.HospitalFileNo as 'File Number',PatientDetails_1.SiteOfPrimary as 'Site Of Primary',PatientDetails_0.City_Town as 'City/Town',PatientDetails_0.State as 'State', datename(mm, PatientDetails_0.DateOfRegistration)+ ' ' + cast(datepart(dd, PatientDetails_0.DateOfRegistration) as char(2)) + ',' + cast(datepart(yyyy,PatientDetails_0.DateOfRegistration) as char(4))   as 'Date Of Registration' ,PatientDetails_0.PhoneNumber as 'Phone Number'From PatientDetails_0 LEFT JOIN PatientDetails_1 ON PatientDetails_0.PatientID = PatientDetails_1.Patient where PatientDetails_0.PatientId='" + Session["PatientID"].ToString() + "'  ";
            DataSet dsSearch = GlobalValues.ExecuteDataSet(strSql);
            if (dsSearch.Tables.Count > 0)
                if (dsSearch.Tables[0].Rows.Count > 0)
                    Session["PatienDetails"] = dsSearch.Tables[0].Rows[0]["Patient Name"].ToString().Trim() + "/ ID:" + dsSearch.Tables[0].Rows[0]["Patient ID"].ToString().Trim() + "/ File No:" + dsSearch.Tables[0].Rows[0]["File Number"].ToString().Trim() + "/ City:" + dsSearch.Tables[0].Rows[0]["City/Town"].ToString().Trim() + "/ " + dsSearch.Tables[0].Rows[0]["Site Of Primary"].ToString().Trim();//Code modified on April 3,2015-Subhashini 

            SiteMaster siteMasterPage = this.Master as SiteMaster;
            if (siteMasterPage != null)
            {
                Label lblPatientDetails = siteMasterPage.FindControl("lblPatientDetails") as Label;
                lblPatientDetails.Text = Session["PatienDetails"].ToString();
            }
        }


        private void CustomValidation(object sender, EventArgs e)
        {
            if (sender != null)
                if (Convert.ToString(Session["EditMode"]) == "Edit")
                {
                    EnableDisableControls(true);
                    Panel1.Visible = false;
                }
                else
                {
                    Panel1.Visible = true;
                }


            foreach (Control ctl in phProject.Controls)
            {
                if (ctl.ID != null)
                {
                    string ColumnName = "";
                    ColumnName = ctl.ID;

                    if (ctl.ID.ToString().EndsWith("_RTX_Y"))
                        ColumnName = ColumnName.ToString().Remove(ColumnName.ToString().LastIndexOf("_RTX_Y"));

                    else if (ctl.ID.ToString().EndsWith("_FD_Y"))
                        ColumnName = ColumnName.ToString().Remove(ColumnName.ToString().LastIndexOf("_FD_Y"));


                    ColumnName = ColumnName.ToString().Replace("_-", " ");
                    if (dsCustomValidation.Tables.Count > 0)
                        if (dsCustomValidation.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] FoundValidationGroup = dsCustomValidation.Tables[0].Select("FieldName='" + ColumnName + "' and ParentField=1");
                            if (FoundValidationGroup.Length > 0)
                            {
                                DataSet dset = GlobalValues.DependentFieldValidation(Session["PatientID"].ToString(), SName, ColumnName);

                                if (dset.Tables[0].Rows.Count > 0)
                                {
                                    bool EnableDisable = false;
                                    DataRow[] FoundRowParent = dset.Tables[0].Select("ParentField=1");
                                    DataRow[] FoundRowChild = dset.Tables[0].Select("ParentField<>1");
                                    if (FoundRowParent.Length > 0)
                                    {
                                        string FieldName = FoundRowParent[0]["Field Name"].ToString().Replace(" ", "_-");
                                        string ValueCSV = FoundRowParent[0]["Relation Validated ValueCSV"].ToString();

                                        string[] stringValueArray = ValueCSV.Split(',');

                                        if (!(bool)FoundRowParent[0]["Calculated Field"])
                                            if ((string)FoundRowParent[0]["DataType"] == "DATE")
                                            {
                                            }
                                            else if ((bool)FoundRowParent[0]["RightPick"] || (bool)FoundRowParent[0]["SelectiveRightSinglePickField"] || (bool)FoundRowParent[0]["RightSinglePickFixed"])
                                            {
                                                TextBox txtbox = new TextBox();
                                                txtbox = (TextBox)phProject.FindControl(FieldName + "_RTX_Y");
                                                if (txtbox != null && Convert.ToString(Session["EditMode"]) != "Read")
                                                {
                                                    if (txtbox.Text.Trim() == string.Empty)
                                                    { EnableDisable = false; }
                                                    else
                                                    {
                                                        int pos = Array.IndexOf(stringValueArray, txtbox.Text.Trim());
                                                        if (pos > -1)
                                                        {
                                                            EnableDisable = false;
                                                        }
                                                        else
                                                        {
                                                            EnableDisable = true;
                                                        }
                                                    }

                                                }



                                            }
                                            else if ((bool)FoundRowParent[0]["FixedDrop"])
                                            {
                                                DropDownList dpLst = new DropDownList();
                                                dpLst = (DropDownList)phProject.FindControl(FieldName + "_FD_Y");
                                                if (dpLst != null && Convert.ToString(Session["EditMode"]) != "Read")
                                                {
                                                    if (dpLst.SelectedItem.Text == string.Empty)
                                                    { EnableDisable = false; }
                                                    else
                                                    {
                                                        int pos = Array.IndexOf(stringValueArray, dpLst.SelectedItem.Text.Trim());
                                                        if (pos > -1)
                                                            EnableDisable = false;
                                                        else
                                                            EnableDisable = true;

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                TextBox txtbox = new TextBox();
                                                txtbox = (TextBox)phProject.FindControl(FieldName + "_TX_Y");
                                                if (txtbox != null && Convert.ToString(Session["EditMode"]) != "Read")
                                                {
                                                    if (txtbox.Text.Trim() == string.Empty)
                                                    { EnableDisable = false; }
                                                    else
                                                    {
                                                        int pos = Array.IndexOf(stringValueArray, txtbox.Text.Trim());
                                                        if (pos > -1)
                                                        {
                                                            EnableDisable = false;
                                                        }
                                                        else
                                                        {
                                                            EnableDisable = true;
                                                        }
                                                    }
                                                }

                                            }
                                    }
                                    foreach (DataRow dr in FoundRowChild)
                                    {
                                        string FieldName = dr["Field Name"].ToString().Replace(" ", "_-");

                                        if (!(bool)dr["Calculated Field"])
                                            if ((string)dr["DataType"] == "DATE")
                                            {
                                            }
                                            else if ((bool)dr["RightPick"] || (bool)dr["SelectiveRightSinglePickField"] || (bool)dr["RightSinglePickFixed"])
                                            {
                                                TextBox txtbox = new TextBox();
                                                txtbox = (TextBox)phProject.FindControl(FieldName + "_RTX_Y");
                                                if (txtbox != null && Convert.ToString(Session["EditMode"]) != "Read")
                                                {
                                                    if (EnableDisable == false && txtbox.Text != string.Empty)
                                                    {
                                                        // ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Dependent Field Value Cleared Here!.');", true);
                                                        ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "oncoNotify('Dependent Field Value Cleared Here!', 'E');", true);
                                                    }
                                                    txtbox.Enabled = EnableDisable;
                                                    if (!EnableDisable)
                                                        txtbox.Text = "";
                                                }

                                                Button btnClick = new Button();
                                                btnClick = (Button)phProject.FindControl("btn" + dr["Field Name"].ToString());
                                                if (btnClick != null && Convert.ToString(Session["EditMode"]) != "Read")
                                                    btnClick.Enabled = EnableDisable;
                                            }
                                        if ((bool)dr["RightMultiPick"] || (bool)dr["SelectiveRightMultiPickField"])
                                        {

                                            Button btnClick = new Button();
                                            btnClick = (Button)phProject.FindControl("btn" + dr["Field Name"].ToString());
                                            if (btnClick != null && Convert.ToString(Session["EditMode"]) != "Read")
                                                btnClick.Enabled = EnableDisable;
                                            TextBox txtbox = new TextBox();
                                            txtbox = (TextBox)phProject.FindControl(FieldName + "_RTX_Y");
                                            if (txtbox != null && Convert.ToString(Session["EditMode"]) != "Read")
                                            {
                                                if (EnableDisable == false && txtbox.Text != string.Empty)
                                                {
                                                    //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Dependent Field Value Cleared Here!.');", true);
                                                    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "oncoNotify('Dependent Field Value Cleared Here!', 'E');", true);
                                                }
                                                txtbox.Enabled = EnableDisable;
                                                if (!EnableDisable)
                                                    txtbox.Text = "";
                                            }

                                        }
                                        else if ((bool)dr["FixedDrop"])
                                        {
                                            DropDownList dpLst = new DropDownList();
                                            dpLst = (DropDownList)phProject.FindControl(FieldName + "_FD_Y");
                                            if (dpLst != null && Convert.ToString(Session["EditMode"]) != "Read")
                                            {
                                                if (EnableDisable == false && dpLst.SelectedItem.Text != string.Empty)
                                                {
                                                    //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Dependent Field Value Cleared Here!.');", true);
                                                    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "oncoNotify('Dependent Field Value Cleared Here!', 'E');", true);
                                                }
                                                dpLst.Enabled = EnableDisable;
                                                if (!EnableDisable)
                                                    dpLst.SelectedIndex = 0;

                                            }
                                        }
                                        else
                                        {
                                            TextBox txtbox = new TextBox();
                                            txtbox = (TextBox)phProject.FindControl(FieldName + "_TX_Y");
                                            if (txtbox != null && Convert.ToString(Session["EditMode"]) != "Read")
                                            {
                                                if (EnableDisable == false && txtbox.Text != string.Empty)
                                                {
                                                    //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Dependent Field Value Cleared Here!.');", true);
                                                    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "oncoNotify('Dependent Field Value Cleared Here!', 'E');", true);
                                                }
                                                txtbox.Enabled = EnableDisable;
                                                if (!EnableDisable)
                                                    txtbox.Text = "";

                                            }

                                        }
                                    }
                                }

                            }

                        }
                }
            }

            //if (Convert.ToString(Session["EditMode"]) == "Edit")
            //{
            //    EnableDisableControls(true);
            //    Panel1.Visible = false;
            //}
            //else
            //{
            //    Panel1.Visible = true;
            //}


        }


        private string Validation()
        {

            //Custom Hard Code Validation for Form Fields via Stored Procedure

            #region "Date Validation"
            string ErrorMessage = string.Empty;
            if (SName == "Demographics" || strType == "Custom")
            {
                TextBox txtboxText = ((TextBox)phProject.FindControl("DateOfBirth_DT_Y"));
                TextBox txtAge = ((TextBox)phProject.FindControl("Age_-at_-Diagnosis_TX_Y"));
                int age = 0;
                string txtDateOfBirth = string.Empty;
                if (txtAge != null)
                {
                    age = txtAge.Text.Trim() == string.Empty ? 0 : Convert.ToInt32(txtAge.Text.Trim());
                }
                if (txtboxText != null)
                    if (txtboxText.Text != string.Empty)
                    {
                        txtDateOfBirth = txtboxText.Text;
                        txtDateOfBirth = txtDateOfBirth == string.Empty ? "1/1/1900" : txtDateOfBirth;
                        if (txtDateOfBirth != null)
                            if (txtDateOfBirth != string.Empty)
                                ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gVDateOfBirth, Convert.ToDateTime(txtDateOfBirth), Convert.ToDateTime(txtDateOfBirth), age);


                    }

                if (txtAge != null)
                    if (txtAge.Text.Trim() != string.Empty)
                    {
                        txtDateOfBirth = txtDateOfBirth == string.Empty ? "1/1/1900" : txtDateOfBirth;
                        ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVAgeAtDiagosis, Convert.ToDateTime(txtDateOfBirth), Convert.ToDateTime("1/1/2014"), Convert.ToInt32(txtAge.Text));
                    }



            }

            else if (SName == "OTHER" || strType == "Custom")
            {
                TextBox txt = ((TextBox)phProject.FindControl("Age_-at_-First_-Child_-in_-Yrs_TX_Y"));
                if (txt != null)
                {
                    string txtAgeFirst = ((TextBox)phProject.FindControl("Age_-at_-First_-Child_-in_-Yrs_TX_Y")).Text;
                    if (txtAgeFirst != null)
                        if (txtAgeFirst != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVAgeatFirstChild, Convert.ToDateTime("1/1/2014"), Convert.ToDateTime("1/1/2014"), Convert.ToInt32(txtAgeFirst));

                }
                txt = ((TextBox)phProject.FindControl("Age_-at_-First_-Child_-in_-Yrs_TX_Y"));
                if (txt != null)
                {
                    string txtAgeMenarch = ((TextBox)phProject.FindControl("Age_-at_-Menarche_-in_-Yrs_TX_Y")).Text;
                    int agemenarche = txtAgeMenarch == string.Empty ? 0 : Convert.ToInt32(txtAgeMenarch);
                    if (txtAgeMenarch != null)
                        if (txtAgeMenarch != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVAgeatMenarche, Convert.ToDateTime("1/1/2014"), Convert.ToDateTime("1/1/2014"), Convert.ToInt32(txtAgeMenarch));
                }

            }


            else if (SName == "Compulsory" || strType == "Custom")
            {
                if ((TextBox)phProject.FindControl("DateOfDiagnosis_DT_Y") != null)
                {
                    string txtDateOfDiag = ((TextBox)phProject.FindControl("DateOfDiagnosis_DT_Y")).Text;
                    if (txtDateOfDiag != null)
                        if (txtDateOfDiag != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVDateOfDiagnosis, Convert.ToDateTime(txtDateOfDiag), Convert.ToDateTime(txtDateOfDiag), 50);
                }

                if ((TextBox)phProject.FindControl("Age_-at_-Diagnosis_TX_Y") != null)
                {
                    string txtAge = ((TextBox)phProject.FindControl("Age_-at_-Diagnosis_TX_Y")).Text;
                    if (txtAge != null)
                        if (txtAge != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVAgeAtDiagosis, Convert.ToDateTime("1/1/2014"), Convert.ToDateTime("1/1/2014"), Convert.ToInt32(txtAge));
                }

            }
            else if (SName == "Disease Details" || SName == "Diagnosis Details")
            {
                string txtDateOfDiag = ((TextBox)phProject.FindControl("DateOfDiagnosis_DT_Y")).Text;
                if (txtDateOfDiag != null)
                    if (txtDateOfDiag != string.Empty)
                        ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVDateOfDiagnosis, Convert.ToDateTime(txtDateOfDiag), Convert.ToDateTime(txtDateOfDiag), 50);

            }

            else if (SName == "Surgery" || SName == "OTHER" || strType == "Custom")
            {
                string txtSurgery = "";
                if ((TextBox)phProject.FindControl("DateOfSurgery_DT_Y") != null)
                    txtSurgery = ((TextBox)phProject.FindControl("DateOfSurgery_DT_Y")).Text;
                    if (txtSurgery != string.Empty)
                        ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVDateOfSurgery, Convert.ToDateTime(txtSurgery), Convert.ToDateTime(txtSurgery), 50);

                string txtboxText = "";
                if ((TextBox)phProject.FindControl("Date_-of_-Brain_-Mets_DT_Y") != null)
                    txtboxText = ((TextBox)phProject.FindControl("Date_-of_-Brain_-Mets_DT_Y")).Text;
                    if (txtboxText != string.Empty)
                        ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVBrainMets, Convert.ToDateTime(txtboxText), Convert.ToDateTime(txtboxText), 50);

                string txtboxTextnotReachable = "";
                if ((TextBox)phProject.FindControl("Patient_-Not_-Reachable_-On_DT_Y") != null)
                    txtboxTextnotReachable = ((TextBox)phProject.FindControl("Patient_-Not_-Reachable_-On_DT_Y")).Text;
                    if (txtboxTextnotReachable != string.Empty)
                        ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVDateNotReachableOn, Convert.ToDateTime(txtboxTextnotReachable), Convert.ToDateTime(txtboxTextnotReachable), 50);


                string txtBiospy = "";
                if ((TextBox)phProject.FindControl("Date_-of_-Biopsy_DT_Y") != null)
                    txtBiospy = ((TextBox)phProject.FindControl("Date_-of_-Biopsy_DT_Y")).Text;
                    if (txtBiospy != string.Empty)
                        ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVDateOfBiopsy, Convert.ToDateTime(txtBiospy), Convert.ToDateTime(txtBiospy), 50);


            }

            else if (SName == "Radiation" || SName == "Radiotherapy" || strType == "Custom")
            {
                string txtboxText = "";
                if ((TextBox)phProject.FindControl("R/T_-Start_-Date_DT_Y") != null)
                {
                    txtboxText = ((TextBox)phProject.FindControl("R/T_-Start_-Date_DT_Y")).Text;
                    if (txtboxText != null)
                        if (txtboxText != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVRTStartDate, Convert.ToDateTime(txtboxText), Convert.ToDateTime(txtboxText), 50);
                }
                if ((TextBox)phProject.FindControl("R/T_-End_-Date_DT_Y") != null)
                {
                    string txtboxTextEnd = ((TextBox)phProject.FindControl("R/T_-End_-Date_DT_Y")).Text;
                    if (txtboxTextEnd != null)
                        if (txtboxTextEnd != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVRTEndDate, Convert.ToDateTime(txtboxTextEnd), txtboxText == string.Empty ? Convert.ToDateTime(null) : Convert.ToDateTime(txtboxText), 50);

                }
            }

            else if (SName == "Liver Metastases" || SName == "OTHER" || strType == "Custom")
            {
                string txtboxText = ((TextBox)phProject.FindControl("Date_-of_-Liver_-Surgery_DT_Y")).Text;
                if (txtboxText != null)
                    if (txtboxText != string.Empty)
                        ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDVLiverMets, Convert.ToDateTime(txtboxText), Convert.ToDateTime(txtboxText), 50);

            }

            else if (SName == "1st Line Treatment" || strType == "Custom")
            {
                string txtboxText = string.Empty;
                if ((TextBox)phProject.FindControl("1st_-Line_-Chemo_-Start_-Date_DT_Y") != null)
                {
                    txtboxText = ((TextBox)phProject.FindControl("1st_-Line_-Chemo_-Start_-Date_DT_Y")).Text;
                    if (txtboxText != null)
                        if (txtboxText != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDV1stLineStartDate, Convert.ToDateTime(txtboxText), Convert.ToDateTime(txtboxText), 50);
                }
                if ((TextBox)phProject.FindControl("1st_-Line_-Chemo_-End_-Date_DT_Y") != null)
                {
                    string txtboxTextEnd = ((TextBox)phProject.FindControl("1st_-Line_-Chemo_-End_-Date_DT_Y")).Text;
                    if (txtboxTextEnd != null)
                        if (txtboxTextEnd != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDV1stLineEndDate, Convert.ToDateTime(txtboxTextEnd), txtboxText == string.Empty ? Convert.ToDateTime(null) : Convert.ToDateTime(txtboxText), 50);
                }

            }

            else if (SName == "2nd Line Treatment" || strType == "Custom")
            {
                string txtboxText = string.Empty;
                if ((TextBox)phProject.FindControl("2nd_-Line_-Chemo_-Start_-Date_DT_Y") != null)
                {
                    txtboxText = ((TextBox)phProject.FindControl("2nd_-Line_-Chemo_-Start_-Date_DT_Y")).Text;
                    if (txtboxText != null)
                        if (txtboxText != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDV2ndLineStartDate, Convert.ToDateTime(txtboxText), Convert.ToDateTime(txtboxText), 50);
                }


                if ((TextBox)phProject.FindControl("2nd_-Line_-Chemo_-End_-Date_DT_Y") != null)
                {
                    string txtboxTextEnd = ((TextBox)phProject.FindControl("2nd_-Line_-Chemo_-End_-Date_DT_Y")).Text;
                    if (txtboxTextEnd != null)
                        if (txtboxTextEnd != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDV2ndLineEndDate, Convert.ToDateTime(txtboxTextEnd), txtboxText == string.Empty ? Convert.ToDateTime(null) : Convert.ToDateTime(txtboxText), 50);
                }

            }

            else if (SName == "3rd Line Treatment" || strType == "Custom")
            {
                string txtboxText = string.Empty;
                if ((TextBox)phProject.FindControl("3rd_-Line_-Chemo_-Start_-Date_DT_Y") != null)
                {
                    txtboxText = ((TextBox)phProject.FindControl("3rd_-Line_-Chemo_-Start_-Date_DT_Y")).Text;
                    if (txtboxText != null)
                        if (txtboxText != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDV3rdLineStartDate, Convert.ToDateTime(txtboxText), Convert.ToDateTime(txtboxText), 50);
                }
                if ((TextBox)phProject.FindControl("3rd_-Line_-Chemo_-End_-Date_DT_Y") != null)
                {
                    string txtboxTextEnd = ((TextBox)phProject.FindControl("3rd_-Line_-Chemo_-End_-Date_DT_Y")).Text;
                    if (txtboxTextEnd != null)
                        if (txtboxTextEnd != string.Empty || strType == "Custom")
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDV3rdLineEndDate, Convert.ToDateTime(txtboxTextEnd), txtboxText == string.Empty ? Convert.ToDateTime(null) : Convert.ToDateTime(txtboxText), 50);
                }

            }

            else if (SName == "4th Line Treatment" || strType == "Custom")
            {
                string txtboxText = string.Empty;
                if ((TextBox)phProject.FindControl("4th_-Line_-Chemo_-Start_-Date_DT_Y") != null)
                {
                    txtboxText = ((TextBox)phProject.FindControl("4th_-Line_-Chemo_-Start_-Date_DT_Y")).Text;
                    if (txtboxText != null)
                        if (txtboxText != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDV4thLineStartDate, Convert.ToDateTime(txtboxText), Convert.ToDateTime(txtboxText), 50);
                }
                if ((TextBox)phProject.FindControl("4th_-Line_-Chemo_-End_-Date_DT_Y") != null)
                {
                    string txtboxTextEnd = ((TextBox)phProject.FindControl("4th_-Line_-Chemo_-End_-Date_DT_Y")).Text;
                    if (txtboxTextEnd != null)
                        if (txtboxTextEnd != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDV4thLineEndDate, Convert.ToDateTime(txtboxTextEnd), txtboxText == string.Empty ? Convert.ToDateTime(null) : Convert.ToDateTime(txtboxText), 50);
                }

            }

            else if (SName == "5th Line Treatment" || strType == "Custom")
            {
                string txtboxText = string.Empty;
                if ((TextBox)phProject.FindControl("5th_-Line_-Chemo_-Start_-Date_DT_Y") != null)
                {
                    txtboxText = ((TextBox)phProject.FindControl("5th_-Line_-Chemo_-Start_-Date_DT_Y")).Text;
                    if (txtboxText != null)
                        if (txtboxText != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDV5thLineStartDate, Convert.ToDateTime(txtboxText), Convert.ToDateTime(txtboxText), 50);
                }
                if ((TextBox)phProject.FindControl("5th_-Line_-Chemo_-End_-Date_DT_Y") != null)
                {
                    string txtboxTextEnd = ((TextBox)phProject.FindControl("5th_-Line_-Chemo_-End_-Date_DT_Y")).Text;
                    if (txtboxTextEnd != null)
                        if (txtboxTextEnd != string.Empty)
                            ErrorMessage += GlobalValues.DateValidation(Session["PatientID"].ToString(), SName, GlobalValues.gDV5thLineEndDate, Convert.ToDateTime(txtboxTextEnd), txtboxText == string.Empty ? Convert.ToDateTime(null) : Convert.ToDateTime(txtboxText), 50);
                }

            }
            #endregion


            //Disease Details Validation



            return ErrorMessage;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            EnableDisableControls(true);
            Session["EditMode"] = "Edit";
            CustomValidation(null, null);
            //updPHControls.Update();
            btnSaveProjectForm.Enabled = true;
            Panel2.Visible = true;
            Panel1.Visible = false;

            if (Session["EmptyControls"] != null)
                Panel2.Visible = false;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = Validation();
            string errMsg = "";
            if (strMessage == string.Empty)
            {
                //Session["EditMode"] = "Edit";
                if (SaveForm())
                {
                    GlobalValues.SaveSessionLog(Session["PatientID"].ToString(), Session["Login"].ToString(), SName);
                    Panel2.Visible = false;
                    Panel1.Visible = true;
                    Session["EditMode"] = "Read";
                    EnableDisableControls(false);
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Patient Details Saved Sucessfully.'); window.location.reload();", true);
                    errMsg = "Patient Details Saved";
                    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "oncoNotify('" + errMsg + "', 'S');", true);
                    lblError.Text = "";
                }
                else
                {
                    EnableDisableControls(true);
                    Session["EditMode"] = "Edit";
                    //updPHControls.Update();
                    btnSaveProjectForm.Enabled = true;
                    Panel2.Visible = true;
                    Panel1.Visible = false;
                }
            }
            else
            {
                EnableDisableControls(true);
                //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('" + strMessage + "');", true);
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "oncoNotify('" + strMessage + "', 'E');", true);
                btnEdit.Visible = false;
                //btnRegister.Visible = false;Code modified on March 26-2015,Subhashini

            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            string strUR = Request.Url.ToString();
            Session["EditMode"] = "Read";
            strUR = strUR.Substring(strUR.IndexOf("Project"));
            Response.Redirect(strUR);
            // Server.TransferRequest(Request.Url.AbsolutePath, false);
            //EnableDisableControls(false);           
            //updPHControls.Update();
            btnSaveProjectForm.Enabled = false;
            Panel2.Visible = false;
            Panel1.Visible = true;
            // Session["MenuChange"] = "NA";
        }

        //protected void btnRegister_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("SearchPatient.aspx");
        //}Code modified on March 26-2015,Subhashini

        protected void SetValidationsForUserInput(TextBox txtCtl, string DataType, string MaxLength)
        {
            int maxLength = 255;
            try
            {
                maxLength = Convert.ToInt32(MaxLength);
            }
            catch { }
            if (txtCtl.ID == "Age_-at_-Diagnosis_TX_Y")
            {
                txtCtl.Text = txtCtl.Text.ToString() == "0" ? string.Empty : txtCtl.Text;
            }
            if (DataType == "LONG")
                txtCtl.Attributes.Add("onkeypress", "return ValidateTextBoxForDataTypeLongINT(event,'" + txtCtl.Text + "',this,'" + ViewState["LabelText"].ToString() + "');");
            if (DataType == "TEXT" || DataType == "MEMO")
            {
                if (txtCtl.ID == "Postal_-Code_TX_Y")
                {
                    txtCtl.Text = txtCtl.Text.ToString() == "0" ? string.Empty : txtCtl.Text;
                    txtCtl.Attributes.Add("onkeypress", "return ValidateTextBoxForDataTypeTextPostalCode(event,'" + txtCtl.Text + "',this,'" + ViewState["LabelText"].ToString() + "');");
                }
                else
                {
                    txtCtl.Attributes.Add("onkeypress", "return ValidateTextBoxForDataTypeTextAlphaNumericCommaSpaceHypen(event,'" + txtCtl.Text + "',this,'" + ViewState["LabelText"].ToString() + "');");
                    txtCtl.Attributes.Add("onkeyup", "return validateKeyup(this);");
                }

            }
            if (DataType == "SINGLE")
                txtCtl.Attributes.Add("onkeypress", "return ValidateTextBoxForDataTypeSingleIsNumericWithDot1(event,'" + txtCtl.Text + "',this,'" + ViewState["LabelText"].ToString() + "');");
            if (DataType == "EMAIL")
            {
                txtCtl.Attributes.Add("onkeypress", "return ValidateTextBoxForDataTypeEmail(event,'" + txtCtl.Text + "',this,'" + ViewState["LabelText"].ToString() + "');");
            }
            if (txtCtl.TextMode != TextBoxMode.MultiLine)
            {
                if (DataType != "MEMO")
                    txtCtl.MaxLength = maxLength;
                if (DataType == "MEMO")
                    txtCtl.MaxLength = 1000;
            }
            else
            {
                if (DataType == "MEMO")
                {
                    txtCtl.MaxLength = 1000;
                    maxLength = 1000;
                }
                txtCtl.Attributes.Add("onkeyDown", "return checkTextAreaMaxLength(this,event,'" + maxLength + "');");

            }
        }


        [WebMethod]
        public static void AbandonSession()
        {
            //string UserID = Convert.ToString(Session["Login"]);
            //string PatientID = Convert.ToString(Session["PatientID"]);
            //GlobalValues.UnlockUser(UserID, PatientID);
            HttpContext.Current.Session.Abandon();
        }
    }
}