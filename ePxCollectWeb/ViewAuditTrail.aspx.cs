﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ePxCollectDataAccess;
using System.Data.SqlClient;


namespace ePxCollectWeb
{
    public partial class ViewAuditTrail : System.Web.UI.Page
    {
        #region Declarations
        DataSet dsViewAudit = new DataSet();
        string strConns = GlobalValues.strConnString;
        string strAuditConns = GlobalValues.strAuditConnString;
        static bool flagAudit = false;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ddlListOfFields.Style.SelectBoxWidth = 280;
            ddlListOfFields.Style.DropDownBoxBoxWidth=280;
            if (Session["ResetPassword"] != null)
            {
                Session["ResetPasswordMsg"] = "Please Change your password.";
                Response.Redirect("Changepassword.aspx");
            }
            if (Session["PatientID"] == null)
            {
                Session["Message"] = "Please pick a Patient.";
                Response.Redirect("SearchPatient.aspx");
            }
            if (!Page.IsPostBack)
            {
                //ddlListOfFields.Style.SelectBoxWidth = 250;/*Code reModified on March 09-2015*/
                BindFieldList();

            }


        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            if (flagAudit == false)
            {
                getAuditTrail();
                flagAudit = true;
            }
            else
            {
                flagAudit = false;
            }
        }
        protected void getAuditTrail()
        {
            try
            {
                int countListOfFields = 0;
                string fieldList = string.Empty;
                string GridColumnList = string.Empty;
                string patientID = string.Empty;
                patientID = Session["PatientID"].ToString();
                for (int i = 0; i <= ddlListOfFields.Items.Count - 1; i++)
                {
                    foreach (ListItem li in ddlListOfFields.Items)
                    {
                        if (li.Selected == true)
                            countListOfFields += 1;
                    }
                }
                if (countListOfFields == 0)
                {
                    lblmsg.ForeColor = GlobalValues.FailureColor;//Code modified on April 2,2015
                    lblmsg.Text = "Select List Of Fields";
                    return;
                }
                foreach (System.Web.UI.WebControls.ListItem item in ddlListOfFields.Items)
                {
                    if (item.Selected)
                    {
                        lblmsg.Text = "";
                        if (fieldList == string.Empty)
                            fieldList = "[" + item.Value.ToString().Trim() + "],";
                        else
                            fieldList += "[" + item.Value.ToString().Trim() + "],";

                        if (GridColumnList == string.Empty)
                            GridColumnList = item.Value.ToString().Trim() + ",";
                        else
                            GridColumnList += item.Value.ToString().Trim() + ",";
                    }
                }
                fieldList = fieldList.Substring(0, fieldList.Length - 1);
                GridColumnList = GridColumnList.Substring(0, GridColumnList.Length - 1);

                string[] valFieldList = fieldList.Split(',');

                string[] GridColumnFieldList = GridColumnList.Split(',');

                BoundField b = new BoundField();
                b.DataField = "PatientID";
                b.HeaderText = "Patient ID";

                GridViewAudit.Columns.Add(b);



                foreach (string field in GridColumnFieldList)
                {
                    BoundField b1 = new BoundField();
                    b1.DataField = field;
                    b1.HeaderText = field;
                    GridViewAudit.Columns.Add(b1);

                }
                BindColumns();
                BindGrid(patientID, fieldList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BindColumns()
        {
            BoundField b2 = new BoundField();
            b2.DataField = "CreatedBy";
            b2.HeaderText = "Created BY";
            GridViewAudit.Columns.Add(b2);

            BoundField b3 = new BoundField();
            b3.DataField = "CreatedDate";
            b3.HeaderText = "Created Date";
            GridViewAudit.Columns.Add(b3);

            BoundField b4 = new BoundField();
            b4.DataField = "LastModifiedBY";
            b4.HeaderText = "Modified BY";
            GridViewAudit.Columns.Add(b4);

            BoundField b5 = new BoundField();
            b5.DataField = "ModifiedDate";
            b5.HeaderText = "Modified Date";
            GridViewAudit.Columns.Add(b5);
        }

        #region Private Methods
        void BindFieldList()
        {
            dsViewAudit = GlobalValues.ExecuteDataSet("select distinct [Field Name] as FieldName,[FieldOrder] from PDFields where [Table Name]<>'Recurrences' and [Field Name] not in ('PatientID','Patient') order by  [FieldOrder]  ");
            if (dsViewAudit != null && dsViewAudit.Tables.Count > 0 && dsViewAudit.Tables[0].Rows.Count > 0)
            {
                ddlListOfFields.DataSource = dsViewAudit.Tables[0];
                ddlListOfFields.DataTextField = "FieldName";
                ddlListOfFields.DataBind();
            }
           
        }

        public void BindGrid(string patientID, string listOfFields)
        {
            try
            {
                SqlParameter[] arParms = new SqlParameter[2];

                arParms[0] = new SqlParameter("@PatientID", SqlDbType.NVarChar);
                arParms[0].Value = patientID;

                arParms[1] = new SqlParameter("@fieldname", SqlDbType.NVarChar);
                arParms[1].Value = listOfFields;

                dsViewAudit = SqlHelper.ExecuteProcedure(strAuditConns, "sp_Audit_PatientDetails", arParms);
                if (dsViewAudit != null && dsViewAudit.Tables.Count > 0 && dsViewAudit.Tables[0].Rows.Count > 0)
                {
                    //START: Formatting all DateTime columns into Date
                    DataTable newDataTable = dsViewAudit.Tables[0].Clone();
                    string[] valColumnName;
                    string columnName = string.Empty;
                    foreach (DataColumn dc in newDataTable.Columns)
                    {
                        if (dc.ColumnName != "CreatedDate" && dc.ColumnName != "ModifiedDate")
                        {
                            if (dc.DataType == typeof(DateTime))
                            {
                                dc.DataType = typeof(string);
                                columnName += dc.ColumnName + ",";
                            }
                        }
                    }
                    if (columnName.Length > 0)
                    {
                        columnName = columnName.Substring(0, columnName.Length - 1);
                        valColumnName = columnName.Split(',');
                        foreach (DataRow dr in dsViewAudit.Tables[0].Rows)
                        {
                            newDataTable.ImportRow(dr);
                        }
                        foreach (DataRow row in newDataTable.Rows)
                        {
                            foreach (string cName in valColumnName)
                            {
                                if (!string.IsNullOrEmpty(row[cName].ToString()))
                                {
                                    DateTime dt = DateTime.Parse(row[cName].ToString());
                                    row[cName] = dt.ToString("dd-MM-yyyy");
                                }
                            }
                        }
                        newDataTable.AcceptChanges();
                        dsViewAudit = new DataSet();
                        dsViewAudit.Tables.Add(newDataTable);
                    }
                    //END: Formatting all DateTime columns into Date
                    GridViewAudit.DataSource = dsViewAudit;
                    GridViewAudit.DataBind();

                    for (int i = 0; i < GridViewAudit.Columns.Count; i++)
                    {
                        GridViewAudit.Columns[i].ItemStyle.Width = Unit.Pixel(150);
                    }
                }
                else
                {
                    lblmsg.ForeColor = GlobalValues.FailureColor;//Code modified on April 2,2015
                    lblmsg.Text = "No records found.";
                }
            }
            catch (Exception ex)
            {
                //Log.WriteLog(ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        protected void GridViewAudit_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
    }
}