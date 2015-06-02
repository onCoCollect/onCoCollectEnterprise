using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data;
using System.IO;

using System.Threading;

namespace ePxCollectWeb
{

    public partial class AnalysisResult : System.Web.UI.Page
    {
        Thread thread;
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
        List<string> Values = new List<string>();
        #region private members
        String connectionString = GlobalValues.strConnString;
        private const string NoFilterSelected = "No Filters applied!";
        string strConns;
        string txtval = "";
        DataSet dsFilters = new DataSet();
        static DataSet dsValues = new DataSet();
        static DataTable dt1Progress = new DataTable();
        static DataTable dt2Progress = new DataTable();
        string AnalysisType = string.Empty;
        static DataSet ByDiagDataSet = new DataSet();
        static DataSet dsDataType = new DataSet();
        DataTable dtDataType = new DataTable();
        static string strDType = string.Empty;
        static string strTableName = string.Empty;
        static bool filterStatus = false;
        #endregion
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
            AnalysisType = Session["AnalysisType"] != null ? Session["AnalysisType"].ToString().Trim() : string.Empty;
            Session.Remove("Other");
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnYes);
            scriptManager.RegisterPostBackControl(this.ddlTemplates);
            scriptManager.RegisterPostBackControl(this.rdExportType);
            btnSaveSearch.Visible = ObjfeatureSet.isAnalysisByAllparams; //will be visible only if the user has permission to save queries

            if (AnalysisType == "All")
            {
                FeatureSetPermission(btnExportExcel, ObjfeatureSet.isExporttoExcelAllParams);
                FeatureSetPermission(btnStats, ObjfeatureSet.isStatsAllParams);
                FeatureSetPermission(btnSaveSearch, ObjfeatureSet.isSaveQueryAllParams);
                FeatureSetPermission(btnFreqencyGene, ObjfeatureSet.isFrequencyAllParams);
            }
            else if (AnalysisType == "ByStudy")
            {
                FeatureSetPermission(btnExportExcel, ObjfeatureSet.isExporttoExcelStudy);
                FeatureSetPermission(btnStats, ObjfeatureSet.isStatsStudy);
                FeatureSetPermission(btnSaveSearch, ObjfeatureSet.isSaveQueryStudy);
                FeatureSetPermission(btnFreqencyGene, ObjfeatureSet.isFrequencyStudy);
            }
            else if (AnalysisType == "ByDiag")
            { FeatureSetPermission(btnExportExcel, ObjfeatureSet.isExporttoExcelDrugDosage); }
            if (!IsPostBack)
            {
                try
                {
                    //dpValues.Style.DropDownBoxBoxWidth = 250;
                    //dpValues.Style2.SelectBoxWidth = 250;
                    //btnClear.CssClass = "buttonDisable";
                    //btnClear.Enabled = false;
                    //Button2.CssClass = "buttonDisable";
                    //Button2.Enabled = false;
                    btnClear.CssClass = "btn disabled btn-primary";
                    Button2.CssClass = "btn disabled btn-primary";
                    bindAttachTemplates();
                    string stJoin = string.Empty;

                    Session.Remove("dsFilter");
                    if (GlobalValues.QueryString.Length > 0) BindColumns();
                    if (dpValues.SelectedItem == null)
                    {
                        Session["dsFilter"] = null;
                    }
                    string strSQL = "Select Datatype,[Field Name],[Table Name] from PDfields with (nolock) ";
                    dsDataType = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, strSQL);

                    if (Session["baseString"] != null)
                        ViewState["baseString"] = Session["baseString"].ToString();
                    if (AnalysisType == "ByDiag")
                    {
                        rdExportType.Items[1].Enabled = false;
                        btnStats.CssClass = "buttonDisable";
                        btnStats.Enabled = false;
                        btnSaveSearch.CssClass = "buttonDisable";
                        btnSaveSearch.Enabled = false;
                        btnPermissions.CssClass = "buttonDisable";
                        btnPermissions.Enabled = false;
                        btnFreqencyGene.CssClass = "buttonDisable";
                        btnFreqencyGene.Enabled = false;
                        lblFrequency.Enabled = false;
                        dpFrequency.Enabled = false;
                        lblFilter.Enabled = false;
                        dpColumns.Enabled = false;
                        dpOperator.Enabled = false;
                        dpValues.Visible = false;
                        string[] values = Session["ByDigValues"].ToString().Split(',');
                        string Regimen = values[0].ToString();
                        string drugName = values[1].ToString();
                        string drugLine = values[2].ToString();
                        string GlobalQuery = GlobalValues.glbFromClauseForDiag;
                        string strParams = " PatientDetails_0.PatientID, GroupName, [Max Dose Per m2], DrugName, " +
                                              " [Total Dose from All Cycles] ,[DrugLine], " + Session["baseString"].ToString();
                        List<string> TemplateValues = new List<string>();
                        string[] valParams = strParams.Split(',');
                        foreach (string paramValue in valParams)
                        {
                            if (!TemplateValues.Contains(paramValue))
                                TemplateValues.Add(paramValue);
                        }
                        strParams = string.Empty;
                        foreach (string paramValue in TemplateValues)
                        {
                            strParams += paramValue + ",";
                        }
                        strParams = strParams.Substring(0, strParams.Length - 1);
                        var DiagQuery = " select * from (select " + strParams +
                                                 GlobalQuery + " Where GroupName='" + Regimen + "' " +
                                                  " AND PatientDetails_0.PatientID Is Not Null " +
                                                  " AND DrugLine='" + drugLine + "'" +
                                                  " ) as a " +
                                                  " Pivot ( Min([Max Dose Per m2]) for DrugName in ([" + drugName + "])) as b ";

                        ByDiagDataSet = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, DiagQuery);
                        lblMessage.Text = DiagQuery;
                        ddlTemplates.Enabled = false;
                        Session["ByDiagDataSet"] = ByDiagDataSet;
                        lblCaption.Text = "Total No. of Records: ";
                        lblCaptionCount.Text = ByDiagDataSet.Tables[0].Rows.Count.ToString();
                        //if(Convert.ToInt32(lblCaptionCount.Text) > 10000)
                        //{
                        //    //ScriptManager.RegisterStartupScript(this, typeof(string), "Window", "alert('No. of Records should be less than 10000 to Export');", true);
                        //    btnExportExcel.CssClass = "buttonDisable";
                        //    btnExportExcel.Enabled = false;
                        //}
                    }
                    else
                    {
                        GetReseultCount("");
                    }
                }
                catch { }
            }
            if (lblMessage.Text != "")
            {
                if (Session["QueryText"] != null)
                {
                    btnClear.CssClass = "btn btn-primary";
                    btnClear.Enabled = true;
                    loadFilterFromDB();
                    Session["QueryText"] = null;
                }
            }
            else
            {
                lblMessage.Text = "";
            }
            Session["LableMessage"] = lblMessage.Text;
        }

        private void FeatureSetPermission(Button btn, bool boolvalue)
        {
            btn.Enabled = boolvalue;
            if (boolvalue == false)
                btn.CssClass = "buttonDisable";
        }
        #region Generating ByDiag Query
        public void GenerateByDiagQuery()
        {


        }
        #endregion
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Session["Flag"] = "AnalysisResults";
            Response.Redirect("ProjectForm.aspx");
        }
        private void loadFilterFromDB()
        {
            if (Session["QueryId"] == null) return;
            var id = int.Parse(Session["QueryId"].ToString());
            var qry = string.Format(@"SELECT 
                         [DispValue]
                          ,[Operator]
                          ,[FilterValue]
                          ,[Type]
                      FROM [UserSavedQueryFilters]
                      where [QueryID]= {0}", id);
            if (Session["dsFilter"] == null)
            {
                CreateDS();
            }
            else
            {
                dsFilters = (DataSet)Session["dsFilter"];
            }
            var rows = GlobalValues.ExecuteDataSet(qry).Tables[0].Rows;
            var tab = dsFilters.Tables[0];
            foreach (DataRow row in rows)
            {
                var drow = tab.NewRow();
                drow[0] = row[0];
                drow[1] = row[1];
                drow[2] = row[2];
                drow[3] = row[3];
                tab.Rows.Add(drow);
            }
            dsFilters.AcceptChanges();
            string getFilterValue = getFilter();

            if (getFilterValue.Length > 0)
                lblMessage.Text += " AND " + getFilterValue;

            var result = lblMessage.Text.Trim();

            if (result.StartsWith(NoFilterSelected, StringComparison.InvariantCultureIgnoreCase))
                result = result.Replace(NoFilterSelected, "").Trim();

            if (result.StartsWith("AND", StringComparison.InvariantCultureIgnoreCase))
                result = result.Replace("AND", "").Trim();

            if (string.IsNullOrWhiteSpace(lblMessage.Text))
                result = NoFilterSelected;
            lblMessage.Text = result;
            Session["QueryId"] = null;
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
        private void BindColumns()
        {
            string ColNames = GlobalValues.QueryString.Replace("Select", "");
            string[] cols = ColNames.Split(',');
            dpColumns.Items.Add("Select");
            for (int i = 0; i <= cols.Length - 1; i++)
            {
                if (cols[i].ToString().Trim() != "")
                {
                    dpFrequency.Items.Add(cols[i].ToString().Trim());
                    dpColumns.Items.Add(cols[i].ToString().Trim());
                }
            }
            dpFrequency.Items.Insert(0, new ListItem(""));

        }
        private void GetReseultCount(string strFilter)
        {
            DataSet dsApplyFilter = new DataSet();
            string strSQL = "Select Count(PatientDetails_0.PatientID) ";
            string query = "";
            var xlparam = "select [QueryText] from [CustomQueries]";
            var xlstring = GlobalValues.ExecuteDataSet(xlparam);
            Session["ds"] = null;
            string strWhere = Convert.ToString(Session["Whr" + Session.SessionID.ToString()]).Trim();
            if (strWhere == "1=1") { strWhere = ""; }
            if (strWhere.Length > 0) { strWhere = " where" + strWhere; }
            string AnalysisType = Session["AnalysisType"] != null ? Session["AnalysisType"].ToString().Trim() : string.Empty;
            string studyCriteria = string.Empty;
            string strGetFilters = string.Empty;
            string strGlobalFilters = string.Empty;
            DataSet dsAnalFilter = new DataSet();
            if (AnalysisType.Length > 0)
            {
                if (AnalysisType == "ByStudy")
                {
                    strGetFilters = "Select StudyCriteria from Studies with (nolock) where StudyName='" + Session["StudySelect"].ToString() + "'";
                    dsAnalFilter = GlobalValues.ExecuteDataSet(strGetFilters);
                    if (dsAnalFilter.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsAnalFilter.Tables[0].Rows)
                        {
                            if (strGlobalFilters.Length > 0) { strGlobalFilters += " OR "; }
                            strGlobalFilters += dr["StudyCriteria"].ToString();
                        }
                        if (GlobalValues.gUserType == "Consultant")
                        {
                            if (Session["Login"] != null)
                            {
                                if (strGlobalFilters.Length > 0) { strGlobalFilters += " AND "; }
                                strGlobalFilters += "(PatientDetails_0.Consultants like '%" + Session["Login"].ToString() + "%')";
                                GlobalValues.gInstanceID.ToString();
                            }
                        }
                    }
                }
                else
                {
                    strGetFilters = " select Criteria from dbo.AnalysisAccessGroups AG inner join AnalysisAccessGroupMembers AGM on AGM.CriteriaID = AG.CriteriaID where AGM.UserID='" + Convert.ToString(Session["Login"]).ToString() + "'";
                    dsAnalFilter = GlobalValues.ExecuteDataSet(strGetFilters);
                    if (dsAnalFilter.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsAnalFilter.Tables[0].Rows)
                        {
                            if (strGlobalFilters.Length > 0) { strGlobalFilters += " AND "; }
                            strGlobalFilters += dr["Criteria"].ToString();
                            ViewState["BaseCriteria"] = strGlobalFilters;
                        }
                    }
                    if (GlobalValues.gUserType == "Consultant")
                    {
                        if (Session["Login"] != null)
                        {
                            if (strGlobalFilters.Length > 0) { strGlobalFilters += " OR "; }
                            strGlobalFilters += "(PatientDetails_0.Consultants like '%" + Session["Login"].ToString() + "%')";
                            GlobalValues.gInstanceID.ToString();
                        }
                    }
                }
            }
            if (strSQL.Length > 0)
            {
                #region ApplyAnalysisFilters
                if (strWhere.Length > 0)
                {
                    if (strGlobalFilters.Length > 0)
                    {
                        strWhere += strGlobalFilters.ToString();
                    }
                }
                else
                {
                    if (strGlobalFilters.Length > 0) { strWhere += " Where (" + strGlobalFilters.ToString() + ")"; }
                }
                if (strFilter.Length > 0)
                {
                    if (strWhere.Length > 0)
                    {
                        string filterText = " AND (" + strFilter.ToString() + ")";
                        Session["flterText"] = filterText;
                        strWhere += filterText;
                    }
                    else
                    {
                        strWhere += " Where (" + strFilter.ToString() + ")";
                    }
                }
                if (strWhere.Length > 0)
                {
                    lblMessage.Text = strWhere.Replace("Where", "").Replace("where 1=1 AND", "").Replace("where", "").Replace("1=1", "").Replace(" OR ", " OR ").Replace(" AND ", " AND ");
                    Session["LableMessage"] = lblMessage.Text;
                }
                else
                {
                    lblMessage.Text = NoFilterSelected;
                    lblCaption.Text = "Total No. of Records: ";
                    lblCaptionCount.Text = "0";
                    return;
                }
                #endregion

                if (strWhere.Length > 0 && strWhere.Trim() != "where 1=1")
                {
                    if (strSQL.IndexOf(GlobalValues.glbFromClause) <= 0)
                    {
                        string strFrom = GlobalValues.PrepareFromClause(strWhere.ToUpper().Replace("WHERE", "").Replace("'", "")) + strWhere;
                        ViewState["countPatient"] = strFrom;
                        if (strFrom.Contains("PatientDetails_0"))
                        {
                            query = "select Count(Patientdetails_0.[PatientID]) FROM (((((((((((PatientDetails_0 INNER JOIN PatientDetails_1 ON PatientDetails_0.PatientID = PatientDetails_1.Patient)INNER JOIN PatientDetails_2 ON PatientDetails_0.PatientID = PatientDetails_2.Patient) INNER JOIN PatientDetails_3 ON PatientDetails_0.PatientID = PatientDetails_3.Patient) INNER JOIN PatientDetails_4 ON PatientDetails_0.PatientID = PatientDetails_4.Patient) INNER JOIN PatientDetails_5 ON PatientDetails_0.PatientID = PatientDetails_5.Patient) INNER JOIN PatientDetails_6 ON PatientDetails_0.PatientID = PatientDetails_6.Patient) INNER JOIN PatientDetails_7 ON PatientDetails_0.PatientID = PatientDetails_7.Patient) INNER JOIN PatientDetails_8 ON PatientDetails_0.PatientID = PatientDetails_8.Patient) INNER JOIN PatientDetails_9 ON PatientDetails_0.PatientID = PatientDetails_9.Patient) LEFT JOIN Recurrences ON PatientDetails_0.PatientID = Recurrences.PatientID) INNER JOIN PatientDetails_10 ON PatientDetails_0.PatientID = PatientDetails_10.Patient) INNER JOIN PatientDetails_11 ON PatientDetails_0.PatientID = PatientDetails_11.Patient " + strWhere;

                        }
                        else
                        {
                            string baseQuery = GlobalValues.glbFromClause.ToString();
                            query = "select Count(Patientdetails_0.[PatientID]) " + baseQuery + " where " + lblMessage.Text;
                            string sessionCount = AppSettingsGet.secssionCounter;
                        }
                    }
                }
                else
                {
                    strSQL += " From PatientDetails_0";
                }
                if (Session["QueryText"] != null)
                {
                    query = query.Replace("where 1=1", "where");

                    query += Session["QueryText"].ToString();
                }
                else
                {
                    query = query.Replace("where 1=1", "where");
                }
                query = query.Replace("$$", "'");
                ViewState["auditQuery"] = query;
                strSQL = strSQL.Replace("$$", "'"); //added to handle the saved queries, where single quotes are replaced with $$.
                strConns = GlobalValues.strConnString;
                int cntRecs = 0;
                if (query != string.Empty)
                {
                    dsApplyFilter = GlobalValues.ExecuteDataSet(query);
                    Session["ds"] = dsApplyFilter;
                    cntRecs = Convert.ToInt32(dsApplyFilter.Tables[0].Rows[0][0].ToString());
                }
                lblCaption.Text = "Total No. of Records: ";
                lblCaptionCount.Text = cntRecs.ToString();
                //if (cntRecs > 10000)
                //{
                //    btnExportExcel.CssClass = "buttonDisable";
                //    btnExportExcel.Enabled = false;
                //}
                Session["TotalRecords"] = lblCaption.Text + lblCaptionCount.Text;
            }
        }
        private void CreateDS()
        {
            DataTable dtFilter = new DataTable();
            DataColumn dcColumn = new DataColumn("DispValue");
            DataColumn dcOperator = new DataColumn("Operator");
            DataColumn dcValue = new DataColumn("FilterValue");
            DataColumn dcType = new DataColumn("Type");
            dcColumn.DataType = System.Type.GetType("System.String");
            dcOperator.DataType = System.Type.GetType("System.String");
            dcValue.DataType = System.Type.GetType("System.String");
            dcType.DataType = System.Type.GetType("System.String");
            dtFilter.Columns.Add(dcColumn);
            dtFilter.Columns.Add(dcOperator);
            dtFilter.Columns.Add(dcValue);
            dtFilter.Columns.Add(dcType);
            dsFilters.Tables.Add(dtFilter);
            Session["dsFilter"] = dsFilters;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblCaptionCount.Text.Trim()) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Window", "alert('Sorry, No Records for the selected Criteria');", true);
                return;
            }
            int noOfRecordsFetch = GlobalValues.excelCount;
            if (Convert.ToInt32(lblCaptionCount.Text) > noOfRecordsFetch)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Window", "alert('No. of Records should be less than " + noOfRecordsFetch + " to Export Excel.');", true);
                return;
            }
            pnlConfirm.Visible = true;
            ModalPopupExtender1.Show();
            rdExportType.SelectedIndex = 0;
            ddlTemplates.SelectedIndex = 0;
            if (rdExportType.SelectedValue == "WithGrouping")
            {
                ddlTemplates.Enabled = true;
            }
            else
            {
                ddlTemplates.Enabled = false;
            }
            lblExportMessage.Text = "";
            pnlConfirm.Visible = true;
        }


        public DataSet attachTemplate(string sqlStr, DataSet ds)
        {
            DataSet dsCC = new DataSet();
            DataSet dsCopy = ds.Copy();
            if (Session["Login"] == null)
            { Response.Redirect("Login.aspx"); }

            dsCC = GlobalValues.ExecuteDataSet(sqlStr);
            string strColName = string.Empty;
            DataTable dt = new DataTable();
            if (dsCC.Tables.Count > 0)
            {
                if (dsCC.Tables[0].Rows.Count > 0)
                {
                    foreach (DataColumn dc in dsCopy.Tables[0].Columns)
                    {
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
                                TemplateValues.Add(dt.Rows[i][3].ToString().Trim());
                            }
                            foreach (DataRow drSource in ds.Tables[0].Rows)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["stat_Value"].ToString().Contains(drSource[dc.ColumnName].ToString()) && drSource[dc.ColumnName].ToString().Length > 0)
                                    {
                                        drSource[strColName] = dr["stat_GroupName"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        dsCC.Tables[0].DefaultView.RowFilter = "";
                    }
                }
            }
            return ds;
        }

        public string Username { get { return Session["Login"].ToString(); } }
        private DataSet ApplyCustomCoding(DataSet ds)
        {


            string sqlStr = string.Empty;
            string selectedTemplate = ddlTemplates.SelectedItem.Text;

            var dataSet = new DataSet();
            sqlStr = "Select * from tblStatTemplates with (nolock) where stat_TemplateName = '" +
                selectedTemplate.Trim().Replace("'", "''") + "'  AND  UserID='" + Username + "' order by Stat_FieldName,Stat_value ";
            dataSet = attachTemplate(sqlStr, ds);
            return dataSet;
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (dpOperator.SelectedItem.Text.Trim() != "Between")
                {
                    ApplyFilter();
                }
                else if ((ddlValue1.SelectedItem != null) && (ddlValue2.SelectedItem != null))
                {
                    var firstListValue = ddlValue1.SelectedItem.Text;
                    var secondListValue = ddlValue2.SelectedItem.Text;
                    if (txtDataType.Text != "DATE")
                    {
                        if ((secondListValue != "") && (firstListValue != ""))
                        {
                            if (Convert.ToInt32(secondListValue) < Convert.ToInt32(firstListValue))
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Selected value must be greater than the Previous selected value.');", true);
                            }
                            else
                            {
                                ApplyFilter();
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please select value from the List.');", true);
                        }
                    }
                    else
                    {
                        if ((secondListValue != "") && (firstListValue != ""))
                        {
                            if (Convert.ToDateTime(secondListValue) < Convert.ToDateTime(firstListValue))
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Selected value must be greater than the Previous selected value.');", true);
                            }
                            else
                            {
                                ApplyFilter();
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please select value from the List.');", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('There is no values to Filter.');", true);
                }
                btnClear.Enabled = true;
                btnClear.CssClass = "btn btn-primary";
                Button2.CssClass = "btn disabled btn-primary";
                Button2.Enabled = false;
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);
                throw;
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);
            }
        }

        protected void dpColumns_SelectedIndexChanged(object sender, EventArgs e)
        {

            ViewState["DataType"] = null;
            //HideOrShowControls();
            BindOperators();
            if (dpValues.Visible)
            {
                BindValues();
                loadExitingValues();
            }

        }
        private void BindValues()
        {
            if (dpColumns.SelectedValue != "Select" && dpOperator.SelectedValue != "Select")
            {
                string strSql = "Select  isnull([Table Name],'') TableName from PDFields where [Field Name]='" + dpColumns.SelectedValue.ToString() + "'";
                string TableName = (string)GlobalValues.ExecuteScalar(strSql);
                if (TableName == null) { TableName = ""; }
                strSql = "Select distinct [" + dpColumns.SelectedValue.ToString().Trim() + "] as colName  from " + strTableName + " where [" + dpColumns.SelectedValue.ToString().Trim() + "] is not null order by colName asc";

                dsValues = GlobalValues.ExecuteDataSet(strSql);
                if (dpValues.Visible == true)
                {
                    if (ViewState["textType"] != null)
                    {
                       
                        for (int valuesIndex = 0; valuesIndex < dsValues.Tables[0].Rows.Count; valuesIndex++)
                        {
                            if ((dsValues.Tables[0].Rows[valuesIndex][0].ToString().Trim()) != "" && (dsValues.Tables[0].Rows[valuesIndex][0].ToString().Trim() != "-"))
                            {
                                if (ViewState["textType"].ToString() == "DATE")
                                {
                                    Values.Add(Convert.ToDateTime(dsValues.Tables[0].Rows[valuesIndex][0].ToString().Trim()).ToString("dd-MMM-yyyy"));
                                }
                                else
                                {
                                    Values.Add(dsValues.Tables[0].Rows[valuesIndex][0].ToString().Trim());
                                }
                            }
                        }
                       
                        dpValues.Texts.SelectBoxCaption = "Select";
                        dpValues.DataSource = Values;
                        dpValues.DataBind();
                        ViewState["ListValues"] = Values;
                    }
                }
                else
                {
                    if (ViewState["textType"] != null)
                    {
                        for (int valuesIndex = 0; valuesIndex < dsValues.Tables[0].Rows.Count; valuesIndex++)
                        {
                            if ((dsValues.Tables[0].Rows[valuesIndex][0].ToString().Trim()) != "" && (dsValues.Tables[0].Rows[valuesIndex][0].ToString().Trim() != "-"))
                            {
                                if (ViewState["textType"].ToString() == "DATE")
                                {
                                    Values.Add(Convert.ToDateTime(dsValues.Tables[0].Rows[valuesIndex][0].ToString().Trim()).ToString("dd-MMM-yyyy"));
                                }
                                else
                                {
                                    Values.Add(dsValues.Tables[0].Rows[valuesIndex][0].ToString().Trim());
                                }
                            }
                        }
                    }
                    ViewState["ListValues"] = Values;
                    //ddlValue1.DataSource = dsValues;
                    ddlValue1.DataSource = Values;
                    //ddlValue1.DataTextField = "colName";
                    ddlValue1.DataBind();
                    //ddlValue2.DataSource = dsValues;
                    ddlValue2.DataSource = Values;
                    //ddlValue2.DataTextField = "colName";
                    ddlValue2.DataBind();
                    ddlValue1.Items.Insert(0, "");
                    ddlValue2.Items.Insert(0, "");
                }
                loadExitingValues();
            }
            else
            {
                if (ViewState["textType"].ToString() == "DATE")
                    txtDataType.Text = "DATE";
                else
                    txtDataType.Text = "TEXT";
                dpValues.DataSource = "";
                dpValues.DataBind();
            }
        }

        protected void ddlValue1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ddl1Text = ddlValue1.SelectedItem.Text;
            ViewState["ddl1Text"] = ddl1Text;
            dt1Progress = dsValues.Tables[0].Copy();
            int max = dt1Progress.Rows.Count - 1;
            for (int i = max; i >= 0; --i)
            {
                if (dt1Progress.Rows[i][0].ToString() == ddl1Text)
                {
                    dt1Progress.Rows[i].BeginEdit();
                    dt1Progress.Rows[i].Delete();
                }
            }
            dt1Progress.AcceptChanges();
            if (ViewState["textType"] != null)
            {
                for (int valuesIndex = 0; valuesIndex < dt1Progress.Rows.Count; valuesIndex++)
                {
                    if ((dt1Progress.Rows[valuesIndex][0].ToString().Trim()) != "" && (dt1Progress.Rows[valuesIndex][0].ToString().Trim() != "-"))
                    {
                        if (ViewState["textType"].ToString() == "DATE")
                        {
                            if (Convert.ToDateTime(dt1Progress.Rows[valuesIndex][0].ToString().Trim()).ToString("dd-MMM-yyyy") != ddl1Text)
                                Values.Add(Convert.ToDateTime(dt1Progress.Rows[valuesIndex][0].ToString().Trim()).ToString("dd-MMM-yyyy"));
                        }
                        else
                        {
                            Values.Add(dt1Progress.Rows[valuesIndex][0].ToString().Trim());
                        }
                    }
                }
            }
            //ddlValue2.DataSource = dt1Progress;
            ddlValue2.DataSource = Values;
            //ddlValue2.DataTextField = "colName";
            ddlValue2.DataBind();
            ddlValue2.Items.Insert(0, "");
            ddlValue1.SelectedIndex = -1;
            ddlValue1.Items.FindByValue(ddl1Text).Selected = true;
            if (ViewState["ddl2Text"] != null)
            {
                ddlValue2.Items.FindByValue(ViewState["ddl2Text"].ToString()).Selected = true;
            }
        }
        protected void ddlValue2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ddl2Text = ddlValue2.SelectedItem.Text;
            ViewState["ddl2Text"] = ddl2Text;
            dt2Progress = dsValues.Tables[0].Copy();
            int max = dt2Progress.Rows.Count - 1;
            for (int i = max; i >= 0; --i)
            {
                if (dt2Progress.Rows[i][0].ToString() == ddl2Text)
                {
                    dt2Progress.Rows[i].BeginEdit();
                    dt2Progress.Rows[i].Delete();
                }
            }
            dt2Progress.AcceptChanges();
            //
            if (ViewState["textType"] != null)
            {
                for (int valuesIndex = 0; valuesIndex < dt2Progress.Rows.Count; valuesIndex++)
                {
                    if ((dt2Progress.Rows[valuesIndex][0].ToString().Trim()) != "" && (dt2Progress.Rows[valuesIndex][0].ToString().Trim() != "-"))
                    {
                        if (ViewState["textType"].ToString() == "DATE")
                        {
                            if (Convert.ToDateTime(dt2Progress.Rows[valuesIndex][0].ToString().Trim()).ToString("dd-MMM-yyyy") != ddl2Text)
                                Values.Add(Convert.ToDateTime(dt2Progress.Rows[valuesIndex][0].ToString().Trim()).ToString("dd-MMM-yyyy"));
                        }
                        else
                        {
                            Values.Add(dt2Progress.Rows[valuesIndex][0].ToString().Trim());
                        }
                    }
                }
            }
            //
            //ddlValue1.DataSource = dt2Progress;
            ddlValue1.DataSource = Values;
            //ddlValue1.DataTextField = "colName";
            ddlValue1.DataBind();
            ddlValue1.Items.Insert(0, "");
            ddlValue2.Items.FindByValue(ddl2Text).Selected = true;
            if (ViewState["ddl1Text"] != null)
            {
                ddlValue1.Items.FindByValue(ViewState["ddl1Text"].ToString()).Selected = true;
            }
            Button2.CssClass = "btn btn-primary";
            Button2.Enabled = true;
            btnClear.CssClass = "btn disabled btn-primary";
            btnClear.Enabled = false;
        }


        public void setTextValue()
        {
            DataSet dsCheckDate = new DataSet();
            string checkNames = "select [Field name] from PDFields";// where DataType ='DATE'";
            dsCheckDate = GlobalValues.ExecuteDataSet(checkNames);
            for (int i = 0; i < dsCheckDate.Tables[0].Rows.Count; i++)
            {
                if (dpColumns.SelectedItem.Text == dsCheckDate.Tables[0].Rows[i][0].ToString())
                {
                    txtval = "DATE";
                    break;
                }
                else
                {
                    txtval = "TEXT";
                }
            }
        }
        private void loadExitingValues()
        {
            if (Session["dsFilter"] == null)
            {
                CreateDS();
            }
            else
            {
                dsFilters = (DataSet)Session["dsFilter"];
            }

            if (dpValues.Visible)
            {
                foreach (ListItem li in dpValues.Items)
                {
                    li.Selected = false;
                }
            }
            DataRow[] dr = dsFilters.Tables[0].Select("DispValue='" + dpColumns.SelectedValue.ToString() + "' AND Operator='" + dpOperator.SelectedValue.ToString() + "'");
            if (dr.Length > 0)
            {
                foreach (DataRow drT in dsFilters.Tables[0].Rows)
                {
                    if (drT["DispValue"].ToString() == dpColumns.SelectedValue.ToString())
                    {
                        if (drT["Operator"].ToString() == "Contains")
                        {
                            txtValue.Text = drT["FilterValue"].ToString();
                        }
                        else if (drT["Operator"].ToString() == "Between")
                        {
                            string[] BetVals = drT["FilterValue"].ToString().Split(',');
                            if (BetVals.Length > 0) { txtValue.Text = BetVals[0].ToString(); }
                            if (BetVals.Length > 1) { txtValue2.Text = BetVals[1].ToString(); }
                        }
                        else
                        {
                            dpOperator.SelectedItem.Text = drT["Operator"].ToString();
                            string[] Vals = drT["FilterValue"].ToString().Split(',');
                            for (int i = 0; i <= Vals.Length - 1; i++)
                            {
                                foreach (ListItem li in dpValues.Items)
                                {
                                    if (li.Value == Vals[i])
                                    {
                                        li.Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                        //saji
                    }
                }
            }
        }
        private void ApplyFilter()
        {
            txtValue.Text = dpValues.Texts.SelectBoxCaption.ToString();
            if (Session["dsFilter"] == null)
            {
                CreateDS();
            }
            else
            {
                dsFilters = (DataSet)Session["dsFilter"];
            }
            if (dpColumns.SelectedItem.Text == "Select" || dpOperator.SelectedItem.Text == "Select")
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "show", "alert('Please select required Filter Values.');", true);
            }
            else
            {
                lblMessage.Text = "Applying filter(s). Please wait ...";
            }
            DataSet dsResult = (DataSet)Session["ds"];
            DataSet FilterSet = new DataSet();
            try
            {
                if (txtDataType.Text != "DATE")
                {
                    if (dpOperator.SelectedValue == "=" && dpOperator.SelectedValue == "<>")
                    {
                        txtValue.Text = dpValues.Texts.SelectBoxCaption.ToString();
                    }
                }
                else
                {
                    if (dtDate1.Text != "")
                    {
                        dtDate1.Text = txtValue.Text;
                    }
                    else
                    {
                        dtDate1.Text = txtValue.Text;
                    }
                }
                if (((txtValue.Text.Length >= 0 && txtValue.Text != "Select") && dpOperator.SelectedValue != "Select") || (dpOperator.SelectedValue == "Between"))
                {
                    DataRow[] dr = dsFilters.Tables[0].Select("DispValue='" + dpColumns.SelectedValue.ToString() + "' AND Operator='" + dpOperator.SelectedValue.ToString() + "'");
                    if (dr.Length > 0)
                    {
                        foreach (DataRow drT in dsFilters.Tables[0].Rows)
                        {
                            if (drT["DispValue"].ToString() == dpColumns.SelectedValue.ToString() && drT["Operator"].ToString() == dpOperator.SelectedValue.ToString())
                            {
                                string text = txtValue.Text;
                                if (dpOperator.SelectedValue == "Contains")
                                {
                                    drT["Operator"] = dpOperator.SelectedValue.ToString();
                                    if (txtDataType.Text == "DATE")
                                    {
                                        drT["FilterValue"] += "," + dtDate1.Text.ToString();
                                        dtDate1.Text = dtDate1.Text;
                                    }
                                    else
                                    {
                                        drT["FilterValue"] += "," + txtValue.Text.ToString();
                                    }
                                }
                                else if (dpOperator.SelectedValue == "Between")
                                {
                                    drT["Operator"] = dpOperator.SelectedValue.ToString();
                                    if (txtDataType.Text == "DATE")
                                    {
                                        var eDate = ddlValue2.SelectedItem.Text;
                                        var sDate = ddlValue1.SelectedItem.Text;
                                        string DateTimeFormat = "MM-dd-yyyy";
                                        DateTime dtFrom = Convert.ToDateTime(sDate);
                                        sDate = dtFrom.ToString(DateTimeFormat);
                                        DateTime dtTo = Convert.ToDateTime(eDate);
                                        eDate = dtTo.ToString(DateTimeFormat);
                                        drT["FilterValue"] = sDate + ":" + eDate;
                                        dtDate1.Text = dtDate1.Text;
                                        dtDate2.Text = dtDate2.Text;
                                    }
                                    else
                                    {
                                        drT["FilterValue"] = ddlValue1.SelectedItem.Text.Trim() + ":" + ddlValue2.SelectedItem.Text.Trim();
                                    }
                                }
                                else if (dpOperator.SelectedValue == "=" || dpOperator.SelectedValue == "<>") //if (dpOperator.SelectedValue == "=" && dpOperator.SelectedValue == "<>")
                                {
                                    drT["Operator"] = dpOperator.SelectedValue.ToString();
                                    drT["FilterValue"] = text;
                                }
                                else
                                {
                                    drT["Operator"] = dpOperator.SelectedValue.ToString();
                                    drT["FilterValue"] = txtValue.Text.ToString();
                                }
                                //saji
                            }
                        }
                    }
                    else
                    {
                        DataRow drF = dsFilters.Tables[0].NewRow();
                        drF[0] = dpColumns.SelectedValue.ToString();
                        drF[1] = dpOperator.SelectedValue.ToString();
                        drF[3] = ViewState["textType"].ToString();

                        if (dpOperator.SelectedValue == "Between")
                        {
                            if (txtDataType.Text == "DATE")
                            {
                                var eDate = ddlValue2.SelectedItem.Text;
                                var sDate = ddlValue1.SelectedItem.Text;
                                string DateTimeFormat = "MM-dd-yyyy";
                                DateTime dtFrom = Convert.ToDateTime(sDate);
                                sDate = dtFrom.ToString(DateTimeFormat);
                                DateTime dtTo = Convert.ToDateTime(eDate);
                                eDate = dtTo.ToString(DateTimeFormat);
                                drF[2] = sDate + ":" + eDate;
                            }
                            else
                            {
                                drF[2] = ddlValue1.SelectedItem.Text.Trim() + ":" + ddlValue2.SelectedItem.Text.Trim();
                            }
                        }
                        else
                        {
                            if (txtDataType.Text == "DATE")
                            {
                                drF[2] = dtDate1.Text.ToString();
                            }
                            else
                            {
                                drF[2] = txtValue.Text; dpValues.Text.ToString();
                            }
                        }
                        dsFilters.Tables[0].Rows.Add(drF);
                    }
                    var rowFilter = getFilter();
                    if (rowFilter.Length >= 0)
                    {
                        GetReseultCount(rowFilter);
                        lblFilterStatus.Text = "YES";
                        rowFilter = rowFilter.Length > 500 ? rowFilter.Substring(0, 500) + "..." : rowFilter;
                    }
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Data filter failed. Please check your values and try again.');", true);
            }
        }

        #region Clearing Filter Details
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Hide();
            string diagType = Session["AnalysisType"] != null ? Session["AnalysisType"].ToString().Trim() : string.Empty;

            if (diagType == "ByDiag")
            {

                lblMessage.Text = string.Empty;

                lblFilter.Text = string.Empty;

                return;

            }
            txtValue.Text = "";
            CreateDS();
            dpValues.DataSource = "";
            dpFrequency.SelectedIndex = 0;
            dpValues.DataBind();
            dpValues.Texts.SelectBoxCaption = "Select";
            dpColumns.SelectedValue = "Select";
            dpOperator.SelectedValue = "Select";
            dpFrequency.SelectedIndex = 0;
            lblFilter.Text = "";
            dtDate1.Text = "";
            dtDate2.Text = "";
            txtValue2.Text = "";
            HideOrShowControls();
            GetReseultCount("");
            btnClear.CssClass = "btn disabled btn-primary";
            btnClear.Enabled = false;

        }
        #endregion
        protected void dpValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filterValue = string.Empty;
            try
            {
                if (lblFilterStatus.Text == "NO")
                {

                    foreach (System.Web.UI.WebControls.ListItem item in dpValues.Items)
                    {
                        if (item.Selected)
                        {
                            filterValue += item.Value.Trim() + ",";
                        }
                    }
                    if (filterValue != string.Empty)
                    {
                        filterValue = filterValue.Substring(0, filterValue.Length - 1);
                        dpValues.Texts.SelectBoxCaption = filterValue;
                        txtValue.Text = dpValues.Texts.SelectBoxCaption.ToString();
                        dpValues.Visible = true;
                        dpValues.Style.SelectBoxCssClass = "ddlselec";
                        if (txtValue.Text.Length >= 0 && txtValue.Text != "Select")
                        {
                            lblFilterStatus.Text = "YES";
                        }
                    }
                    else
                    {
                        lblFilterStatus.Text = "NO";
                    }
                }
                else
                {
                    lblFilterStatus.Text = "NO";
                }
                if (!string.IsNullOrEmpty(txtValue.Text))
                {
                    Button2.CssClass = "btn btn-primary";
                    Button2.Enabled = true;
                }
                btnClear.CssClass = "btn disabled btn-primary";
                btnClear.Enabled = false;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                // ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);
            }
        }

        protected void dpOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dpColumns.SelectedItem.Text != "Select")
            {
                string strSQL = "Select [DataType] from [PDFields] where [Field Name] = '" + dpColumns.SelectedItem.Text + "'";
                var textType = GlobalValues.ExecuteDataSet(strSQL);

                if (dpOperator.SelectedItem.Text.Trim() == "Between")
                {
                    ddlValue1.Visible = true;
                    ddlValue2.Visible = true;
                    dpValues.Visible = false;
                }
                else
                {
                    ddlValue1.Visible = false;
                    ddlValue2.Visible = false;
                    dpValues.Visible = true;
                }
                HideOrShowControls();
                if (dpValues.Visible)
                {
                    foreach (ListItem li in dpValues.Items)
                    {
                        li.Selected = false;
                    }
                    if (dpValues.Items.Count == 0)
                    {
                        BindValues();
                    }
                    loadExitingValues();
                }
                else
                {
                    BindValues();
                }
            }

            else
            {
                dpOperator.ClearSelection();
                dpOperator.SelectedIndex = 0;
            }
        }

        protected void btnSaveSearch_Click(object sender, EventArgs e)
        {
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
        }
        private string getFilter()
        {
            string rowFilter = string.Empty;
            foreach (DataRow drFilter in dsFilters.Tables[0].Rows)
            {
                var Ops = drFilter["Operator"].ToString().Split(',');
                var Vals = drFilter["FilterValue"].ToString().Split(',');
                string strFilter = string.Empty;
                for (int i = 0; i <= Ops.Length - 1; i++)
                {
                    for (int j = 0; j <= Vals.Length - 1; j++)
                    {
                        if (strFilter.Length > 0)
                        {
                            if (Ops[i].ToString() == "<>")
                            {
                                strFilter += " AND ";
                            }
                            else
                            {
                                strFilter += " OR ";
                            }
                        }
                        if (Ops[i].ToString() == "Contains")
                        {
                            strFilter += "[" + drFilter["DispValue"].ToString() + "]" + " LIKE '%" + Vals[j].ToString() + "%'";
                        }
                        else
                            if (Ops[i].ToString() == "Between")
                            {
                                string strColVal = "[" + drFilter["DispValue"].ToString() + "]";
                                strFilter += strColVal + " >= '" + Vals[j].ToString().Replace(":", "' AND " + strColVal + " <= '") + "'";
                            }
                            else
                            {
                                if (drFilter["Type"].ToString() == "Text")
                                {
                                    if (Vals[j] != "")
                                    {
                                        strFilter += "[" + drFilter["DispValue"].ToString().Replace("[", "").Replace("]", "") + "]" + Ops[i].ToString() + "'" + Vals[j].ToString() + "'";
                                    }
                                    else
                                    {
                                        strFilter = string.Empty;
                                    }
                                }
                                if (drFilter["Type"].ToString() == "LONG")
                                {
                                    if (Vals[j] != "")
                                    {
                                        strFilter += "[" + drFilter["DispValue"].ToString().Replace("[", "").Replace("]", "") + "]" + Ops[i].ToString() + "'" + Convert.ToInt64(Vals[j]) + "'";
                                    }
                                    else
                                    {
                                        strFilter = string.Empty;
                                    }
                                }
                                if (drFilter["Type"].ToString() == "SINGLE")
                                {
                                    if (Vals[j] != "")
                                    {
                                        strFilter += "[" + drFilter["DispValue"].ToString().Replace("[", "").Replace("]", "") + "]" + Ops[i].ToString() + "'" + Convert.ToSingle(Vals[j]) + "'";
                                    }
                                    else
                                    {
                                        strFilter = string.Empty;
                                    }
                                }
                                if (drFilter["Type"].ToString() == "BOOLEAN")
                                {
                                    if (Vals[j] != "")
                                    {
                                        strFilter += "[" + drFilter["DispValue"].ToString().Replace("[", "").Replace("]", "") + "]" + Ops[i].ToString() + "'" + Convert.ToBoolean(Vals[j]) + "'";
                                    }
                                    else
                                    {
                                        strFilter = string.Empty;
                                    }
                                }

                                if (drFilter["Type"].ToString() == "MEMO")
                                {
                                    if (Vals[j] != "")
                                    {
                                        strFilter += "[" + drFilter["DispValue"].ToString().Replace("[", "").Replace("]", "") + "]" + Ops[i].ToString() + "'" + Vals[j].ToString() + "'";
                                    }
                                    else
                                    {
                                        strFilter = string.Empty;
                                    }
                                }
                                if (drFilter["Type"].ToString() == "DATE")
                                {
                                    if (Vals[j] != "")
                                    {
                                        string DateTimeFormat = "MM-dd-yyyy";
                                        DateTime dateAndTime = Convert.ToDateTime(Vals[j].ToString());
                                        string dt = dateAndTime.ToString(DateTimeFormat);
                                        strFilter += "[" + drFilter["DispValue"].ToString().Replace("[", "").Replace("]", "") + "]" + Ops[i].ToString() + "'" + dt + "'";
                                    }
                                    else
                                    {
                                        strFilter = string.Empty;
                                    }
                                }
                            }
                    }
                }
                if (strFilter.Length > 0)
                {
                    if (rowFilter.Length > 0) { rowFilter += " AND "; }
                    if (strFilter != "")
                    {
                        rowFilter = rowFilter.Replace(strFilter, "");
                        rowFilter += "(" + strFilter + ")";
                    }
                    else
                    {
                        strFilter = "";
                    }
                }
            }
            return rowFilter;
        }
        private string[] getControlSelection()
        {
            string[] val = new string[2];
            try
            {
                int i = 0; int allIndex = -1; int selCnt = 0;
                foreach (ListItem li in dpValues.Items)
                {
                    i++;
                    if (li.Selected)
                    {
                        selCnt++;
                        if (li.Text.ToUpper().ToString() == "ALL")
                            allIndex = i - 1;
                    }
                }
                if (selCnt > 1 && allIndex != -1)
                {
                    dpValues.Items[allIndex].Selected = false;
                }
                foreach (ListItem li in dpValues.Items)
                {
                    if (li.Selected)
                    {
                        val[0] += li.Value + ",";
                        val[1] += li.Text + ",";
                    }
                }
                val[0] = (!string.IsNullOrEmpty(val[0])) ? val[0].Substring(0, val[0].Length - 1) : string.Empty;
                val[1] = (!string.IsNullOrEmpty(val[1])) ? val[1].Substring(0, val[1].Length - 1) : string.Empty;
            }
            catch (Exception ex)
            {
            }
            return val;
        }
        private void HideOrShowControls()
        {
            if (txtDataType.Text == "DATE")
            {
                txtValue.Visible = false;
                txtValue2.Visible = false;
                dpValues.Visible = false;
                dtDate1.Visible = false;
                ddlValue1.Visible = false;
                ddlValue2.Visible = false;
                dpValues.Visible = true;
                if (dpOperator.SelectedValue == "Between")
                {
                    ddlValue1.Visible = true;
                    lblAnd.Visible = true;
                    ddlValue2.Visible = true;
                    dpValues.Visible = false;
                }
                else
                {
                    lblAnd.Visible = false;
                    dtDate2.Visible = false;
                    dpValues.Visible = true;
                }
            }
            else
            {
                dpValues.Texts.SelectBoxCaption = "Select";

                dpValues.Visible = true;
                dtDate2.Visible = false;
                dtDate1.Visible = false;
                if (dpOperator.SelectedValue == "Between")
                {

                    lblAnd.Visible = true;
                    ddlValue1.Visible = true;
                    ddlValue2.Visible = true;
                    dpValues.Visible = false;
                }
                else if (dpOperator.SelectedValue != "=" && dpOperator.SelectedValue != "<>")
                {
                    lblAnd.Visible = false;
                    txtValue.Visible = false;
                    txtValue2.Visible = false;
                    ddlValue1.Visible = false;
                    ddlValue2.Visible = false;
                    dpValues.Visible = true;
                }
                else
                {
                    lblAnd.Visible = false;
                    txtValue.Visible = false;
                    txtValue2.Visible = false;
                    ddlValue1.Visible = false;
                    ddlValue2.Visible = false;
                    dpValues.Visible = true;
                }
            }
        }
        private void BindOperators()
        {
            HideOrShowControls();
            dpOperator.Items.Clear();
            string sqlQuery = string.Empty;
            sqlQuery = " [Field Name] ='" + dpColumns.SelectedValue.ToString().Trim() + "'";
            dtDataType = dsDataType.Tables[0].Select(sqlQuery).CopyToDataTable();
            if (dtDataType != null && dtDataType.Rows.Count > 0)
            {
                strDType = dtDataType.Rows[0]["Datatype"].ToString();
                strTableName = dtDataType.Rows[0]["Table Name"].ToString();
            }
            ViewState["textType"] = strDType;
            string strOPType = string.Empty;
            if (strDType != null)
            {
                txtDataType.Text = strDType.ToUpper();
                switch (strDType.ToUpper())
                {
                    case "TEXT":
                    case "MEMO":
                        {
                            dpOperator.Items.Add("Select");
                            dpOperator.Items.Add("=");
                            dpOperator.Items.Add("<>");
                            dpOperator.Items.Add("Contains");
                            break;
                        }
                    case "LONG":
                    case "SINGLE":
                        {
                            dpOperator.Items.Add("Select");
                            dpOperator.Items.Add("=");
                            dpOperator.Items.Add("<>");
                            dpOperator.Items.Add(">");
                            dpOperator.Items.Add(">=");
                            dpOperator.Items.Add("<");
                            dpOperator.Items.Add("<=");
                            dpOperator.Items.Add("Between");
                            break;
                        }
                    case "DATE":
                        {
                            dpOperator.Items.Add("Select");
                            dpOperator.Items.Add("=");
                            dpOperator.Items.Add("<>");
                            dpOperator.Items.Add(">");
                            dpOperator.Items.Add(">=");
                            dpOperator.Items.Add("<");
                            dpOperator.Items.Add("<=");
                            dpOperator.Items.Add("Between");
                            break;
                        }
                    case "BOOLEAN":
                        {
                            dpOperator.Items.Add("Select");
                            dpOperator.Items.Add("=");
                            dpOperator.Items.Add("<>");
                            //dpOperator.Items.Add("Contains");
                            break;
                        }
                }
            }
        }
        protected void btnNo_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
            updConfirm.Update();
            rdExportType.SelectedIndex = 0;
            ddlTemplates.SelectedIndex = 0;

            ddlTemplates.Enabled = false;
            lblExportMessage.Text = "";

        }
        protected void btnYes_Click(object sender, EventArgs e)
        {
            if (rdExportType.SelectedValue == "WithGrouping")
            {
                if (ddlTemplates.SelectedIndex == 0)
                {
                    lblExportMessage.Visible = true;
                    lblExportMessage.Text = "Please select a Template to Export with Grouping.";
                    ddlTemplates.Enabled = true;
                    ModalPopupExtender1.Show();
                }
                else
                {
                    lblExportMessage.Visible = false;
                    lblExportMessage.Text = "";
                    ddlTemplates.Enabled = false;
                    ExportToExcel();
                    ModalPopupExtender1.Show();
                    UpdatePanel1.Update();
                }

            }
            else if (rdExportType.SelectedValue == "export")
            {
                ExportToExcel();
            }



        }
        private void ExportToExcel()
        {
            if (AnalysisType != "ByDiag")
            {
                int noOfRecordsFetch = GlobalValues.excelCount;
                string fromClause = GlobalValues.glbFromClause.ToString();
                string val = "select top " + noOfRecordsFetch + " Patientdetails_0.[PatientID], " +
                    ViewState["baseString"].ToString() + fromClause + " where " + lblMessage.Text;

                var dsOrig = GlobalValues.ExecuteDataSet(val);
                //START: Formatting all DateTime columns into Date
                DataTable newDataTable = dsOrig.Tables[0].Clone();
                string[] valColumnName;
                string columnName = string.Empty;
                foreach (DataColumn dc in newDataTable.Columns)
                {
                    if (dc.DataType == typeof(DateTime))
                    {
                        dc.DataType = typeof(string);
                        columnName += dc.ColumnName + ",";
                    }
                }
                if (columnName.Length > 0)
                {
                    columnName = columnName.Substring(0, columnName.Length - 1);
                    valColumnName = columnName.Split(',');
                    foreach (DataRow dr in dsOrig.Tables[0].Rows)
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
                                row[cName] = dt.ToString("dd-MMM-yyyy");
                            }
                        }
                    }
                    newDataTable.AcceptChanges();
                    dsOrig = new DataSet();
                    dsOrig.Tables.Add(newDataTable);
                }
                //END: Formatting all DateTime columns into Date
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
                    return;
                }
            }
            else
            {
                if (Session["ByDiagDataSet"] != null)
                {
                    DataSet ds = new DataSet();
                    ds = (DataSet)Session["ByDiagDataSet"];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        bindExcel((DataSet)Session["ByDiagDataSet"]);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Window", "alert('Sorry, No Records for the selected Criteria');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Window", "alert('Sorry, No Records for the selected Criteria');", true);
                }
            }
        }
        public void bindExcel(DataSet dsCopy)
        {
            /* string outputPath = "C:\\";

             //Microsoft.Office.Interop.Excel.ApplicationClass excelApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

             // Create a new Excel Workbook
             Microsoft.Office.Interop.Excel.Workbook excelWorkbook = new Microsoft.Office.Interop.Excel.Workbook();

             int sheetIndex = 0;

             // Copy each DataTable as a new Sheet
             foreach (System.Data.DataTable dt in dsCopy.Tables)
             {

                 // Create a new Sheet
                 Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelWorkbook.Sheets.Add(
                     excelWorkbook.Sheets.get_Item(++sheetIndex),
                     Type.Missing, 1, Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);

                 excelSheet.Name = dt.TableName;

                 // Copy the column names (cell-by-cell)
                 for (int col = 0; col < dt.Columns.Count; col++)
                 {
                     excelSheet.Cells[1, col + 1] = dt.Columns[col].ColumnName;
                 }

                 ((Microsoft.Office.Interop.Excel.Range)excelSheet.Rows[1, Type.Missing]).Font.Bold = true;


                 // Copy the values (cell-by-cell)
                 for (int col = 0; col < dt.Columns.Count; col++)
                 {
                     for (int row = 0; row < dt.Rows.Count; row++)
                     {
                         excelSheet.Cells[row + 2, col + 1] = dt.Rows[row].ItemArray[col];
                     }
                 }

             }

             // Save and Close the Workbook
             excelWorkbook.SaveAs(outputPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing,
                 Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                 Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
             excelWorkbook.Close(true, Type.Missing, Type.Missing);
             excelWorkbook = null;

             // Release the Application object
             //excelApp.Quit();
             //excelApp = null;


             */

            //if (dsCopy.Tables[0].Columns.Contains("PatientID"))
            //    dsCopy.Tables[0].Columns.Remove(dsCopy.Tables[0].Columns["PatientID"]);

            dsCopy.AcceptChanges();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=AnalysisResult.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            GridView grdA = new GridView();
            grdA.DataSource = dsCopy;
            grdA.DataBind();
            //grdA.AllowPaging = false;
            //grdA.PageSize = 10000;
            grdA.RenderBeginTag(htw);
            grdA.HeaderRow.RenderControl(htw);
            if (AnalysisType != "ByDiag")
            {
                //ViewState["dsAudit"] = dsCopy.Copy();
                //thread = new Thread(new ThreadStart(WorkThreadFunction));
                //thread.Start();
                auditDetails(dsCopy);
            }
            foreach (GridViewRow row in grdA.Rows)
            {
                row.RenderControl(htw);
            }
            grdA.FooterRow.RenderControl(htw);
            grdA.RenderEndTag(htw);
            Response.Write(sw);
            Response.End();
        }

        public void WorkThreadFunction()
        {
            try
            {
                DataSet dsCopy = (DataSet)ViewState["dsAudit"];
                auditDetails(dsCopy);
            }
            catch (Exception ex)
            {
                // log errors
            }
            finally
            {
                thread.Abort();
                ViewState["dsAudit"] = null;
            }
        }


        public void auditDetails(DataSet dsCopy)
        {
            string userID = Session["Login"].ToString();
            string totalRecords = "[" + lblCaption.Text + lblCaptionCount.Text + "]";
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

                /*if (ViewState["countPatient"] != null) { patientQuery = "select Patientdetails_0.[PatientID]  " + ViewState["countPatient"].ToString(); }
                var ds = GlobalValues.ExecuteDataSet(patientQuery);
                for (int pIndex = 0; pIndex < ds.Tables[0].Rows.Count; pIndex++)
                {
                    auditPatients += ds.Tables[0].Rows[pIndex][0].ToString() + ",";
                }
                auditPatients = auditPatients.Substring(0, auditPatients.Length - 1);   commented by  Premiya for Perfomance Tuning for Issue Id 14 in Stress */



                var strpatient = GlobalValues.ExecuteScalar("select (SELECT ','+PatientDetails_0.PatientID " + ViewState["countPatient"].ToString() + "     FOR XML PATH ('')  , TYPE).value('.', 'nvarchar(max)') ");
                auditPatients = strpatient.ToString();

                string auditSqlQuery = "insert into AnalysisAudit values ('" + userID + "', '" + auditQuery + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "', '" + auditParameters + "' , '" + auditPatients + "','" + totalRecords + "')";
                SqlHelper.ExecuteNonQuery(connectionString, System.Data.CommandType.Text, auditSqlQuery);
            }
            catch
            {

            }

        }
        protected void btnFreqencyGene_Click(object sender, EventArgs e)
        {
            string filterValue = dpFrequency.SelectedItem.ToString();

        }

        #region Binding Analysis Access Group Permissions based on UserID.
        public void bindPermission()
        {
            string userID = string.Empty;
            string hospitalCode = string.Empty;
            try
            {
                if (Session["Login"] != null)
                {
                    userID = Session["Login"].ToString();
                }
                string connectionString = GlobalValues.strConnString;
                string sql1 = "select HospitalCode from HospitalUsers where UserId = '" + userID + "'";
                var ds1 = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sql1);
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    hospitalCode = ds1.Tables[0].Rows[0]["HospitalCode"].ToString();
                }
                string sql = " select h.HospitalName,aag.AccessGroupName,aag.Scope,aag.Criteria,aag.[Description],agm.UserID " +
                             " from AnalysisAccessGroupMembers agm " +
                             " inner join AnalysisAccessGroups aag on agm.CriteriaID=aag.CriteriaID " +
                             " Cross apply dbo.fn_Splithospitalcode(aag.HospitalCSV) as fc " +
                             " Inner join [Hospitals] h " +
                             " on (fc.HosCode = h.HospitalName) " +
                             " where agm.UserID = '" + userID + "' AND h.HospitalCode = '" + hospitalCode + "'";
                var ds = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sql);
                grdAccess.DataSource = ds;
                grdAccess.DataBind();
                Panel1.Visible = true;
                ModalPopupExtender2.Show();
                UpdatePanel1.Update();
            }
            catch
            {

            }
        }
        #endregion
        protected void btnPermissions_Click(object sender, EventArgs e)
        {
            // bindPermission();
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
            pnlConfirm.Visible = true;
            ModalPopupExtender1.Show();
        }
        protected void Close_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Hide();
            lblExportMessage.Text = "";
            Panel1.Visible = false;
        }
        protected void grdAccess_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAccess.PageIndex = e.NewPageIndex;
            bindPermission();
            ModalPopupExtender2.Show();
            UpdatePanel1.Update();
        }
        protected void ddlTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTemplates.SelectedIndex != 0)
            {
                lblExportMessage.Text = "";
                lblExportMessage.Visible = false;
                ModalPopupExtender1.Show();
            }
            else
            {
                lblExportMessage.Visible = true;
                ModalPopupExtender1.Show();
            }
        }
    }

}


