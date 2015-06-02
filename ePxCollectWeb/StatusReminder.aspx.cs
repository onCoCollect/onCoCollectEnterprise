using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

using ePxCollectDataAccess;

namespace ePxCollectWeb
{
    public partial class StatusReminder : System.Web.UI.Page
    {
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
        string strConns = GlobalValues.strConnString;
        DataSet dsSearch = new DataSet();
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

            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnSendToGrid);
            if (ObjfeatureSet.isStatusCheckList == false)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Sorry, you don't have permission to this functionality.');", true);
                Response.Redirect("ProjectForm.aspx");
            }

            //GlobalValues.gPostBackURL = Request.Url.AbsoluteUri.ToString();
            Session["gPostBackURL"] = Request.Url.AbsoluteUri.ToString();
            // scriptManager.RegisterPostBackControl(this.Label1);

            AvoidMultipleSubmit(btnPatientStatus, btnPatientStatus.Text);
            AvoidMultipleSubmit(BtnOK, BtnOK.Text);
        }

        public void AvoidMultipleSubmit(Button button, string text)
        {
            PostBackOptions optionsSubmit = new PostBackOptions(button);
            button.OnClientClick = "disableButtonOnClick(this, '" + text + "', 'button'); ";
            button.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmit);

        }


        public void AvoidMultipleSubmit(LinkButton button, string text)
        {
            PostBackOptions optionsSubmit = new PostBackOptions(button);
            button.OnClientClick = "disableButtonOnClick(this, '" + text + "', 'lbltext'); ";
            button.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmit);

        }
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            int outP;
            string strQueryText = string.Empty;
            //DataSet dsSearch = new DataSet();
            if (txtNoOfDays.Text == "" || !(Int32.TryParse(txtNoOfDays.Text.Trim(), out outP)))
            {
                lblError.Text = "Please Enter Numeric Value in No.of Days";
            }
            else
            {
                lblError.Text = "";
                //strQueryText = "SELECT PatientDetails_0.PatientName,PatientDetails_0.PatientID, PatientDetails_0.HospitalFileNo, PatientDetails_0.DateOfRegistration,PatientDetails_1.[Days since Followup], Year(DateOfDiagnosis) AS YearOfDiagnosis, PatientDetails_0.DateOfBirth, PatientDetails_0.Sex, PatientDetails_0.City_Town, PatientDetails_0.Country_State, PatientDetails_0.ConsultantsName, PatientDetails_0.[Referral Physician], PatientDetails_1.Disease, PatientDetails_1.DateOfDiagnosis, PatientDetails_1.Status, PatientDetails_1.StatusDate, PatientDetails_1.CauseOfDeath, PatientDetails_1.DateOfDeath, PatientDetails_0.EmailID, PatientDetails_1.[1st RecurrencePattern], PatientDetails_0.Address, PatientDetails_0.PhoneNumber FROM PatientDetails_0 LEFT JOIN PatientDetails_1 ON PatientDetails_0.PatientID = PatientDetails_1.Patient WHERE ((([StatusDate]+" + (txtNoOfDays.Text) + ") Is Null)) OR ((([StatusDate]+" + txtNoOfDays.Text + ")<Getdate()) AND ((PatientDetails_1.Status)<>'dead'))";
                strQueryText = "SELECT PatientDetails_0.PatientName,PatientDetails_0.PatientID,PatientDetails_0.HospitalFileNo,Convert(Varchar(12),PatientDetails_0.DateOfRegistration) DateOfRegistration,PatientDetails_1.[Days since Followup], case when Year(DateOfDiagnosis)='1900' then NULL else Year(DateOfDiagnosis) end AS YearOfDiagnosis, case when Year(DateOfBirth)='1900' then NULL else Convert(Varchar(12),PatientDetails_0.DateOfBirth) end as DateOfBirth, PatientDetails_0.[Age at Diagnosis], PatientDetails_0.Sex, PatientDetails_0.City_Town, PatientDetails_0.State, PatientDetails_0.Consultants, PatientDetails_0.[Referral Physician], PatientDetails_1.SiteOfPrimary,  case when Year(DateOfDiagnosis)='1900' then NULL else Convert(Varchar(12),PatientDetails_1.DateOfDiagnosis) end as DateofDiagnosis, PatientDetails_1.Status,  case when Year(PatientDetails_1.StatusDate)='1900' then NULL else Convert(Varchar(12),PatientDetails_1.StatusDate) end as StatusDate, PatientDetails_1.CauseOfDeath,  case when Year(PatientDetails_1.DateOfDeath)='1900' then NULL else Convert(Varchar(12),PatientDetails_1.DateOfDeath) end as  DateOfDeath,  PatientDetails_1.[1st RecurrencePattern] FROM PatientDetails_0 LEFT JOIN PatientDetails_1 ON PatientDetails_0.PatientID = PatientDetails_1.Patient  WHERE    ((PatientDetails_1.Status)<>'dead')    and  DATEDIFF(DAY,PatientDetails_1.StatusDate,getdate())<=" + txtNoOfDays.Text + "";
                dsSearch = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strQueryText);
                //grdREsults.PageIndex = 0;
                grdREsults.DataSource = dsSearch.Tables[0].DefaultView;
                grdREsults.DataBind();
                UpdatePanel1.Update();
                for (int i = 0; i < grdREsults.Columns.Count; i++)
                {
                    grdREsults.Columns[i].ItemStyle.Width = Unit.Pixel(150);
                }
                Session["DS"] = dsSearch;
                lblCount.Text = " No. Of Patients: " + dsSearch.Tables[0].Rows.Count.ToString();
                lblNote.Visible = true;
            }
        }

        protected void btnPatientStatus_Click(object sender, EventArgs e)
        {
            string strQueryText = string.Empty;
            // DataSet dsSearch = new DataSet();            
            //strQueryText = "SELECT PatientDetails_0.PatientName, PatientDetails_0.PatientID, PatientDetails_0.HospitalFileNo, PatientDetails_0.DateOfRegistration,PatientDetails_1.[Days since Followup], Year(DateOfDiagnosis) AS YearOfDiagnosis, PatientDetails_0.DateOfBirth, PatientDetails_0.Sex, PatientDetails_0.City_Town, PatientDetails_0.Country_State, PatientDetails_0.ConsultantsName, PatientDetails_0.[Referral Physician], PatientDetails_1.Disease, PatientDetails_1.DateOfDiagnosis, PatientDetails_1.Status, PatientDetails_1.StatusDate, PatientDetails_1.CauseOfDeath, PatientDetails_1.DateOfDeath, PatientDetails_0.EmailID, PatientDetails_1.[1st RecurrencePattern], PatientDetails_0.Address, PatientDetails_0.PhoneNumber FROM PatientDetails_0 LEFT JOIN PatientDetails_1 ON PatientDetails_0.PatientID = PatientDetails_1.Patient WHERE ([StatusDate] IS NULL) ";// (((Len([StatusDate] + 'a'))<6))";
            strQueryText = "SELECT PatientDetails_0.PatientName, PatientDetails_0.PatientID, PatientDetails_0.HospitalFileNo, Convert(Varchar(12),PatientDetails_0.DateOfRegistration) DateOfRegistration,PatientDetails_1.[Days since Followup], case when Year(DateOfDiagnosis)='1900' then NULL else Year(DateOfDiagnosis) end AS YearOfDiagnosis,case when Year(DateOfBirth)='1900' then NULL else Convert(Varchar(12),PatientDetails_0.DateOfBirth) end as DateOfBirth, PatientDetails_0.[Age at Diagnosis], PatientDetails_0.Sex, PatientDetails_0.City_Town, PatientDetails_0.State, PatientDetails_0.Consultants, PatientDetails_0.[Referral Physician], PatientDetails_1.SiteOfPrimary,case when Year(DateOfDiagnosis)='1900' then NULL else Convert(Varchar(12),PatientDetails_1.DateOfDiagnosis) end as DateofDiagnosis, PatientDetails_1.Status,  case when Year(PatientDetails_1.StatusDate)='1900' then NULL else Convert(Varchar(12),PatientDetails_1.StatusDate) end as StatusDate, PatientDetails_1.CauseOfDeath,case when Year(PatientDetails_1.DateOfDeath)='1900' then NULL else Convert(Varchar(12),PatientDetails_1.DateOfDeath) end as  DateOfDeath,  PatientDetails_1.[1st RecurrencePattern] FROM PatientDetails_0 LEFT JOIN PatientDetails_1 ON PatientDetails_0.PatientID = PatientDetails_1.Patient WHERE [StatusDate] is null "; //(Len(Isnull(Convert(Varchar,[StatusDate]),'') ) <=0)
            dsSearch = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strQueryText);
            //grdREsults.PageIndex = 0;
            grdREsults.DataSource = dsSearch;
            grdREsults.DataBind();
            for (int i = 0; i < grdREsults.Columns.Count; i++)
            {
                grdREsults.Columns[i].ItemStyle.Width = Unit.Pixel(150);
            }
            Session["DS"] = dsSearch;
            lblCount.Text = " No. Of Patients: " + dsSearch.Tables[0].Rows.Count.ToString();
            txtNoOfDays.Text = "";
            lblNote.Visible = false;
        }

        protected void grdREsults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dsSearch = (DataSet)Session["DS"];
            grdREsults.PageIndex = e.NewPageIndex;
            grdREsults.DataSource = dsSearch.Tables[0].DefaultView;
            grdREsults.DataBind();

            lblCount.Text = " No. Of Patients: " + dsSearch.Tables[0].Rows.Count.ToString();
            lblNote.Visible = true;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void btnSendToGrid_Click(object sender, EventArgs e)
        {
            int outP;
            if (txtNoOfDays.Text == "" || !(Int32.TryParse(txtNoOfDays.Text.Trim(), out outP)))
            {
                lblError.Text = "Please Enter Numeric Value in No.of Days";
            }
            else
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=Status.xls");
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                // grdREsults.RenderControl(htw);
                DataSet ds = new DataSet();
                ds = (DataSet)Session["DS"];
                GridView grd = new GridView();
                grd.DataSource = ds;
                grd.DataBind();
                grd.RenderBeginTag(htw);
                grd.HeaderRow.RenderControl(htw);
                foreach (GridViewRow row in grd.Rows)
                {
                    row.RenderControl(htw);
                }
                grd.FooterRow.RenderControl(htw);
                grd.RenderEndTag(htw);
                Response.Write(sw);
                Response.End();

                //string attachment = "attachment; filename=Emp.xls";
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", attachment);
                //Response.ContentType = "application/ms-excel";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter htw = new HtmlTextWriter(sw);
                //grdREsults.RenderControl(htw);
                //Response.Write(sw.ToString());
                //Response.End();
            }
        }
        public static void WriteDataTable(DataTable sourceTable, System.IO.TextWriter writer, bool includeHeaders)
        {
            if (includeHeaders)
            {
                List<string> headerValues = new List<string>();
                foreach (DataColumn column in sourceTable.Columns)
                {
                    headerValues.Add(QuoteValue(column.ColumnName));
                }

                writer.WriteLine(String.Join(",", headerValues.ToArray()));
            }

            string[] items = null;
            foreach (DataRow row in sourceTable.Rows)
            {
                items = row.ItemArray.Select(o => QuoteValue(o.ToString())).ToArray();
                writer.WriteLine(String.Join(",", items));
            }

            writer.Flush();
        }

        private static string QuoteValue(string value)
        {
            return String.Concat("\"", value.Replace("\"", "\"\""), "\"");
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            Session["Message"] = null;
            Response.Redirect("ProjectForm.aspx");
        }




    }
}