using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
namespace ePxCollectWeb
{
    public partial class ExportData : System.Web.UI.Page
    {
        string AnalysisType = string.Empty;
        String connectionString = GlobalValues.strConnString;
        public string Username { get { return Session["Login"].ToString(); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            AnalysisType = Session["AnalysisType"] != null ? Session["AnalysisType"].ToString().Trim() : string.Empty;
         //   Page.Header.Controls.Remove(new LiteralControl("<base target='_self' />"));
            if(!IsPostBack)
            {
                 
              //  Page.Header.Controls.Add(New LiteralControl("<base target='_self' />"))
                bindAttachTemplates();
                ddlTemplates.Enabled = false;
                if(AnalysisType =="ByDiag")
                {
                    
                        rdExportType.Items[1].Enabled = false;

                   
                }
            }
            
        }

        protected void ddlTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTemplates.SelectedIndex != 0)
            {
                lblExportMessage.Text = "";
                lblExportMessage.Visible = false;
              
            }
            else
            {

                lblExportMessage.Visible = true;
              
            }
        }

        public void bindAttachTemplates()
        {
            string connectionString = GlobalValues.strConnString;
            var sqlText = "select distinct(stat_TemplateName),mang_TemplateID from [tblStatTemplates] where  UserID='" + Username + "'";
            var templates = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sqlText);
            ddlTemplates.DataTextField = "stat_TemplateName";
            ddlTemplates.DataValueField = "mang_TemplateID";
            ddlTemplates.DataSource = templates;
            ddlTemplates.DataBind();
            ddlTemplates.Items.Insert(0, " ");
        }
        protected void rdExportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdExportType.SelectedIndex == 0)
            {

              
                ddlTemplates.Enabled = false;
                lblExportMessage.Visible = false;
                ddlTemplates.SelectedItem.Text = "";


            }
            else
            {

              
                ddlTemplates.Enabled = true;
                lblExportMessage.Visible = false;

            }
           
        }


        protected void btnYes_Click(object sender, EventArgs e)
        {

            if (rdExportType.SelectedValue == "WithGrouping")
            {
                if (ddlTemplates.SelectedIndex == 0)
                {
                    lblExportMessage.Visible = true;
                    lblExportMessage.Text = "Please select a Template to Export with Grouping.";
                   
                }
                else
                {
                    lblExportMessage.Visible = false;
                    lblExportMessage.Text = "";

                    ExportToExcel();
                   
                }

            }
            else if (rdExportType.SelectedValue == "export")
            {
                ExportToExcel();
            }

         }

        public void auditDetails(DataSet dsCopy)
        {
            string userID = Session["Login"].ToString();
            string totalRecords = "[" + Session["TotalRecords"].ToString() + "]";
            string discoveryTime = System.DateTime.Now.ToString();
            string auditParameters = string.Empty;
            string auditQuery = string.Empty;
            string auditPatients = String.Empty;
            string patientQuery = string.Empty;
            try
            {


                if (ViewState["auditQuery"] != null)
                {
                    auditQuery = ViewState["auditQuery"].ToString();

                    auditQuery = auditQuery.Replace("'", "''");
                }
                for (int index = 0; index < dsCopy.Tables[0].Columns.Count; index++)
                {
                    auditParameters += dsCopy.Tables[0].Columns[index].ToString() + ",";

                }

                auditParameters = auditParameters.Substring(0, auditParameters.Length - 1);
                if (ViewState["countPatient"] != null) { patientQuery = "select Patientdetails_0.[PatientID]  " + ViewState["countPatient"].ToString(); }
                var ds = GlobalValues.ExecuteDataSet(patientQuery);
                for (int pIndex = 0; pIndex < ds.Tables[0].Rows.Count; pIndex++)
                {
                    auditPatients += ds.Tables[0].Rows[pIndex][0].ToString() + ",";
                }
                auditPatients = auditPatients.Substring(0, auditPatients.Length - 1);

                string auditSqlQuery = "insert into AnalysisAudit values ('" + userID + "', '" + auditQuery + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "', '" + auditParameters + "' , '" + auditPatients + "','" + totalRecords + "')";
                SqlHelper.ExecuteNonQuery(connectionString, System.Data.CommandType.Text, auditSqlQuery);
            }
            catch
            {

            }

        }
        public void bindExcel(DataSet dsCopy)
        {

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=AnalysisResult.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            // grdREsults.RenderControl(htw);
            GridView grdA = new GridView();
            grdA.DataSource = dsCopy;
            grdA.DataBind();
            grdA.RenderBeginTag(htw);
            grdA.HeaderRow.RenderControl(htw);
            if(AnalysisType !="ByDiag")
            {
                auditDetails(dsCopy);
            }
        
            foreach (GridViewRow row in grdA.Rows)
            {
                row.RenderControl(htw);
            }
            grdA.FooterRow.RenderControl(htw);
            grdA.RenderEndTag(htw);
            Response.Write(sw);
            Page.Header.Controls.Remove(new LiteralControl("<base target='' />"));
       //    Page.Header.Controls.Add(New LiteralControl("<base target=""_self"" />"))
            Response.End();
          

        }
        private void ExportToExcel()
        {
            if (AnalysisType != "ByDiag")
            {

            int noOfRecordsFetch = GlobalValues.excelCount;

            string fromClause = GlobalValues.glbFromClause.ToString();

            string val = "select top " + noOfRecordsFetch + " Patientdetails_0.[PatientID], " +
                Session["baseString"].ToString() + fromClause + " where " + Session["LableMessage"].ToString();

            var dsOrig = GlobalValues.ExecuteDataSet(val);
            string selectedItem = ddlTemplates.SelectedItem.Text;
            lblExportMessage.Text = "";
            lblExportMessage.Visible = false;


            if (dsOrig.Tables[0].Rows.Count > 0)
            {
                if (rdExportType.SelectedIndex == 1)
                {
                    if (selectedItem == " ")
                    {
                        lblExportMessage.Visible = true;
                        lblExportMessage.Text = "Please select a Template for Export with Group";
                    }
                    else
                    {
                        lblExportMessage.Text = string.Empty;
                        lblExportMessage.Visible = false;
                        dsOrig = ApplyCustomCoding(dsOrig);
                        bindExcel(dsOrig);
                    }
                }
                else if (rdExportType.SelectedIndex == 0)
                {

                    bindExcel(dsOrig);


                }
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, typeof(string), "Window", "alert('Sorry, No Records for the selected Criteria');", true);
            }
            }
            else
            {
                if (Session["ByDiagDataSet"]!=null)
                {

                    bindExcel((DataSet)Session["ByDiagDataSet"]);
                }
            }

         
        }

        public DataSet attachTemplate(string sqlStr, DataSet ds)
        {

            DataSet dsCC = new DataSet();
            DataSet dsCopy = ds.Copy();
            if (Session["Login"] == null)
            { Response.Redirect("Login.aspx"); }


            //string selectedTemplate = ddlTemplates.SelectedItem.Text;

            ////string sqlStr = "Select * from CustomStatsFields with (nolock) wh ere Login='" + Session["Login"].ToString() + "'";
            //string sqlStr = "Select * from tblStatTemplates with (nolock) where mang_TemplateName = '" + selectedTemplate + "'  and  Login='" + Session["Login"].ToString() + "'";


            dsCC = GlobalValues.ExecuteDataSet(sqlStr);

            //  for(int count=0;count <dsCC.Tables)

            string strColName = string.Empty;
            DataTable dt = new DataTable();



            if (dsCC.Tables.Count > 0)
            {
                if (dsCC.Tables[0].Rows.Count > 0)
                {
                    foreach (DataColumn dc in dsCopy.Tables[0].Columns)
                    {
                        //   dsCC.Tables[0].DefaultView.RowFilter = "FldName='" + dc.ColumnName.ToString() + "'";
                        dsCC.Tables[0].DefaultView.RowFilter = "stat_FieldName='" + dc.ColumnName.ToString() + "'";

                        dt = dsCC.Tables[0].DefaultView.ToTable();


                        if (dt.Rows.Count > 0)
                        {
                            strColName = dc.ColumnName.ToString() + "_ STATS CODING";
                            DataColumn dcNew = new DataColumn(strColName);
                            try
                            {
                                ds.Tables[0].Columns.Add(strColName).SetOrdinal(ds.Tables[0].Columns[dc.ColumnName].Ordinal + 1);
                            }
                            catch (Exception)
                            {

                            }
                            /// Reading FieldValues 
                            /// 

                            List<string> TemplateValues = new List<string>();

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                // var statValues = string.Join(",", dt.Rows[i][3].ToString().Trim());
                                TemplateValues.Add(dt.Rows[i][3].ToString().Trim());
                            }

                            ////  foreach (DataRow drSource in ds.Tables[0].Rows)
                            //      for(int rowIndex =0;rowIndex<ds.Tables[0].Rows.Count;rowIndex++)
                            //  {
                            //      for (int index = 0; index < dt.Rows.Count; index++)
                            //      {

                            //          //  if (drSource[dc.ColumnName].ToString().Trim().Contains(TemplateValues[index].ToString().Trim()))
                            //          if (TemplateValues[index].ToString().Trim().Contains(ds.Tables[0].Rows[rowIndex][1].ToString().Trim()))
                            //         // if (TemplateValues[index].ToString().Trim().Contains(drSource[dc.ColumnName].ToString().Trim()))
                            //          {
                            //             // drSource[strColName] = dt.Rows[index][4].ToString();
                            //              ds.Tables[0].Rows[rowIndex][1] = dt.Rows[index][4].ToString();
                            //          }
                            //      }
                            //  }





                            foreach (DataRow drSource in ds.Tables[0].Rows)
                            {


                                foreach (DataRow dr in dt.Rows)
                                {
                                    //  string[] statValue = dr["stat_Value"].ToString().Split(',');
                                    // foreach (string stat in statValue)
                                    //  {
                                    if (dr["stat_Value"].ToString().Contains(drSource[dc.ColumnName].ToString()) && drSource[dc.ColumnName].ToString().Length > 0)
                                    //if (drSource[dc.ColumnName].ToString() == dr["stat_Value"].ToString())
                                    {
                                        //drSource[dc.ColumnName] = dr["FieldNewValue"].ToString();
                                        drSource[strColName] = dr["stat_GroupName"].ToString();
                                        break;

                                    }
                                    // }
                                }

                            }
                            //    }
                            //}
                        }
                        dsCC.Tables[0].DefaultView.RowFilter = "";
                    }
                }
            }
            return ds;

        }

        private DataSet ApplyCustomCoding(DataSet ds)
        {


            string sqlStr = string.Empty;
            string selectedTemplate = ddlTemplates.SelectedItem.Text;

            var dataSet = new DataSet();
            sqlStr = "Select * from tblStatTemplates with (nolock) where stat_TemplateName = '" +
                selectedTemplate.Trim().Replace("'", "''") + "'  and  UserID='" + Username + "' order by Stat_FieldName,Stat_value ";
            dataSet = attachTemplate(sqlStr, ds);
            return dataSet;





        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            rdExportType.SelectedIndex = 0;
            ddlTemplates.SelectedIndex = 0;

            ddlTemplates.Enabled = false;
            lblExportMessage.Text = "";
            
            
          
        }
    }
}