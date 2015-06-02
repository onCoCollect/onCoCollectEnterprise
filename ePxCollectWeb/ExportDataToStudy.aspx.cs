using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ePxCollectDataAccess;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Globalization;

namespace ePxCollectWeb
{
    public partial class ExportDataToStudy : System.Web.UI.Page
    {
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
        string instaneID = "";
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

            string strPatientId = Convert.ToString(Session["PatientID"]);
            if (ObjfeatureSet.isExporttoExcelStudy == false)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Sorry, you don't have permission to this functionality.');", true);
                Response.Redirect("ProjectForm.aspx");
            }
            var instanceQuery = "select top 1 instanceID from Instance";
            instaneID = GlobalValues.ExecuteScalar(instanceQuery).ToString();
            if (!IsPostBack)
            {
                ViewState["Count"] = "0";
                BindStudyDropDown(instaneID);
            }

            //AvoidMultipleSubmit(btnExportDatatoCloud, "Export");
            //pnlAuditSelect.Visible = true;
            //DataSet dsExportData = new DataSet();
            //string ExportQuery = "select UserID,CreatedDate,StudyCode,StudyPatientsList,StudyFieldsListCSV,StudyCriteria,CumpulsoryFieldsCriteria,CumpulsoryFields from Audit_SendDatatoStudy";
            //dsExportData = SqlHelper.ExecuteDataset(GlobalValues.strAuditConnString, CommandType.Text, ExportQuery);
            //GridExportAudit.DataSource = dsExportData;
            //GridExportAudit.DataBind();
        }

        private void BindStudyDropDown(string instaneID)
        {

            string Query = " select '' as StudyCode,'' as StudyName union select StudyCode,StudyName from Studies where  StudyCode in (select StudyCode from tblStudyUsers where localPI like '%" + Session["Login"].ToString() + "%' )  and  Instances like '%-" + instaneID + "-%'  AND  getdate() between StudyStartDate and  DATEADD(day, 1, studyEndDate)";
            DataSet dsStudies = GlobalValues.ExecuteDataSet(Query);
            ddlStutyList.DataSource = dsStudies;
            ddlStutyList.DataTextField = "StudyName";
            ddlStutyList.DataValueField = "StudyCode";
            ddlStutyList.DataBind();

        }

        protected void btnExportDatatoCloud_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlStutyList.SelectedValue != string.Empty)
                {
                    DataSet dsExportData = new DataSet();
                    string ExportQuery = "select UserID,CreatedDate,StudyCode,StudyPatientsList,StudyFieldsListCSV,StudyCriteria,CumpulsoryFieldsCriteria,CumpulsoryFields from Audit_SendDatatoStudy order by CreatedDate desc";
                    dsExportData = SqlHelper.ExecuteDataset(GlobalValues.strAuditConnString, CommandType.Text, ExportQuery);
                    GridExportAudit.DataSource = dsExportData;
                    GridExportAudit.DataBind();
                    if (!BoolImportJobStatus())
                        ConnectToCloudDatabaseStudyImport(ddlStutyList.SelectedValue, instaneID);
                    else
                    {
                        lblMessage.ForeColor = GlobalValues.FailureColor;
                        lblMessage.Text = "Patient Details are being imported by Admin. Please try again after some time. ";
                        return;
                    }
                }
                else
                {
                    lblMessage.ForeColor = GlobalValues.FailureColor;
                    lblMessage.Text = "Please Select the Study Name.";
                    return;
                }
            }
            catch (Exception)
            {
                lblMessage.ForeColor = GlobalValues.FailureColor;
                lblMessage.Text = "Unable to Establish Connection with the Cloud Server. Internet connection is slow or unavailable. Please check the network settings and try again.";
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);
                return;
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);
            }
        }

        //public void AvoidMultipleSubmit(Button button, string text)
        //{
        //    PostBackOptions optionsSubmit = new PostBackOptions(button);
        //    button.OnClientClick = "disableButtonOnClick(this, '" + text + "', 'button'); ";
        //    button.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmit);

        //}


        private bool BoolImportJobStatus()
        {
            bool bval = false;
            string Query = "select isnull(count(*),0) from Audit_ImportJob where ImportStatus=1";
            int CountValue = Convert.ToInt32(GlobalValues.ExecuteScalar(Query));
            if (CountValue > 0)
                bval = true;

            return bval;
        }
        private void ConnectToCloudDatabaseStudyImport(string StudyCode, string Instance)//Recurrences and PatientTestsByLine and PatientDrugsByLine not Implemented here TODO
        {
            OncoEncrypt.OncoEncrypt ObjEnc = new OncoEncrypt.OncoEncrypt();
            lblMessage.ForeColor = GlobalValues.FailureColor;
            string Query = "";
            string connectionstring = ObjEnc.SecureDecrypt(ConfigurationManager.ConnectionStrings["OncoCollectStudy"].ToString());

            var LiveUpdateVersion = SqlHelper.ExecuteScalar(GlobalValues.strLUConnString, CommandType.Text, "select top 1  isnull([Version Number],'0') as CloudVersionNumber from LiveUpdateInfo  order by [Live Update Published Date] desc,[Version Number] desc");
            var LocalLiveUpdateVersion = GlobalValues.ExecuteScalarAudit("select top 1  isnull([Version Number],'0') as CloudVersionNumber from Cloud_LiveUpdateInfo  order by [Live Update Published Date] desc,[Version Number] desc");
            if (Convert.ToString(LiveUpdateVersion) != string.Empty)
            {
                if (Convert.ToString(LocalLiveUpdateVersion) != Convert.ToString(LiveUpdateVersion))
                {
                    lblMessage.Text = "Please request the System Administrator to update the system before send data to Study.";
                    return;
                }
            }

            DataTable dtArray = new DataTable();
            dtArray.Columns.Add(new DataColumn("FieldName", typeof(string)));
            dtArray.Columns.Add(new DataColumn("TableName", typeof(string)));
            try
            {
                DataSet ds = new DataSet();

                Query = "select * from Studies where StudyCode='" + StudyCode + "' AND  getdate() between StudyStartDate and  DATEADD(day, 1, studyEndDate) ";//Study Code
                ds = GlobalValues.ExecuteDataSet(Query);


                Query = "select distinct[Table Name] as TableName from PDFields where [Table Name] like 'PatientDetails_%'";
                DataSet dsTable = GlobalValues.ExecuteDataSet(Query);



                for (int intstudy = 0; intstudy < ds.Tables[0].Rows.Count; intstudy++)
                {
                    string field = string.Empty;
                    using (SqlConnection cn = new SqlConnection(connectionstring))
                    {
                        cn.Open();
                        string StudyFieldsListCSV = ds.Tables[0].Rows[intstudy]["StudyFieldsListCSV"].ToString();
                        string StudyPatientsList = ds.Tables[0].Rows[intstudy]["StudyPatientsList"].ToString();
                        string StudyCriteria = ds.Tables[0].Rows[intstudy]["StudyCriteria"].ToString();
                        string CumpulsoryFieldsCriteria = ds.Tables[0].Rows[intstudy]["CumpulsoryFieldsCriteria"].ToString();
                        string CumpulsoryFields = ds.Tables[0].Rows[intstudy]["cumpulsoryFields"].ToString();

                        //Code added by Subhashini -April 7,2015

                        string UserID = Session["Login"].ToString();
                        string CreatedDate = DateTime.Now.Date.ToString();
                        string StudysCode = StudyCode;
                        // string StudyPatientsList = "";
                        // string StudyFieldsListCSV = "";
                        //string StudyCriteria = "";
                        // string CumpulsoryFieldsCriteria = "";
                        // string CumpulsoryFields = "";

                        string CumpulsoryFieldsQuey = "";
                        if (CumpulsoryFields != string.Empty)
                        {
                            string[] splitword = CumpulsoryFields.Split(',');
                            if (splitword.Length > 0)
                            {
                                for (int count = 0; count < splitword.Length; count++)
                                {
                                    if (splitword[count].ToString().Trim() != string.Empty)
                                        CumpulsoryFieldsQuey = CumpulsoryFieldsQuey + "and  [" + splitword[count].ToString().Trim() + "] is not null and  [" + splitword[count].ToString().Trim() + "] <> '' ";
                                }
                            }

                        }

                        string AllPatientField = "";
                        AllPatientField = StudyPatientsList;
                        string StrPatientId = "";
                        if (Convert.ToString(Session["PatientID"]) != string.Empty)
                            StrPatientId = "'" + Convert.ToString(Session["PatientID"]) + "'";
                        if (StudyPatientsList != StrPatientId && ViewState["Count"].ToString() == "0")
                        {
                            if (StudyPatientsList == string.Empty)
                                Query = "Delete from PatientDetails_0 where  StudyCode='" + StudyCode + "'";
                            else
                                Query = "Delete from PatientDetails_0 where PatientID in (" + StudyPatientsList + ") and StudyCode='" + StudyCode + "'";
                            SqlCommand cmd = new SqlCommand(Query, cn);
                            cmd.ExecuteNonQuery();
                        }

                        bool BoolValueCount = booleanInstance(StudyPatientsList, cn, Instance, StudyCriteria, CumpulsoryFieldsQuey, StudyCode);

                        if ((StudyPatientsList != string.Empty && StudyPatientsList != StrPatientId && ViewState["Count"].ToString() == "0") || (BoolValueCount && StudyPatientsList != StrPatientId)) ///&& StudyPatientsList != strPatientId
                        {

                            try
                            {
                                string AllStudyField = "'" + StudyFieldsListCSV + "'";
                                AllStudyField = StudyFieldsListCSV.Replace(",", "','");

                                if (StudyPatientsList == "" || BoolValueCount)
                                {
                                    string sqlStr = " SELECT STUFF((SELECT Distinct ',''' + RTRIM ( PatientDetails_0.PatientID) + ''''" + GlobalValues.glbFromClause + " ";

                                    if (StudyCriteria != string.Empty)
                                        sqlStr = sqlStr + "and " + StudyCriteria;
                                    if (CumpulsoryFieldsQuey != string.Empty)
                                        sqlStr = sqlStr + CumpulsoryFieldsQuey;

                                    sqlStr = sqlStr + "  FOR XML PATH('')) ,1,1,',') AS PatientID ";
                                    StudyPatientsList = GlobalValues.ExecuteScalar(sqlStr).ToString();
                                    StudyPatientsList = StudyPatientsList.TrimStart(',');
                                }
                                else
                                {
                                    string sqlStr = " SELECT STUFF((SELECT Distinct ',''' + RTRIM ( PatientDetails_0.PatientID) + ''''" + GlobalValues.glbFromClause + " ";
                                    if (StudyCriteria != string.Empty)
                                        sqlStr = sqlStr + "and " + StudyCriteria;
                                    if (CumpulsoryFieldsQuey != string.Empty)
                                        sqlStr = sqlStr + CumpulsoryFieldsQuey;
                                    sqlStr = sqlStr + " and  PatientDetails_0.PatientId in (" + StudyPatientsList + ")";
                                    sqlStr = sqlStr + "  FOR XML PATH('')) ,1,1,',') AS PatientID ";
                                    StudyPatientsList = GlobalValues.ExecuteScalar(sqlStr).ToString();
                                    StudyPatientsList = StudyPatientsList.TrimStart(',');
                                }

                                if (StudyPatientsList.Trim() != string.Empty)
                                {
                                    ViewState["Count"] = "1";
                                    ArrayList arrlistPatient = new ArrayList();
                                    string[] strItems;
                                    strItems = StudyPatientsList.Split(',');
                                    foreach (string strValue in strItems)
                                    {
                                        arrlistPatient.Add(strValue);
                                    }

                                    dtArray = FindTableNameFromPDFields(cn, AllStudyField, dtArray);

                                    //Transaction Starts Here
                                    //SqlTransaction sqltr = cn.BeginTransaction("Test");
                                    try
                                    {
                                        ArrayList arrquery = new ArrayList();
                                        string queryselect = string.Empty;
                                        string queryInsert = string.Empty;
                                        string queryValues = string.Empty;
                                        DataView view = new DataView(dtArray.Copy());
                                        DataTable distinctValues = view.ToTable(true, "TableName");

                                        for (int inttable = 0; inttable < dsTable.Tables[0].Rows.Count; inttable++)
                                        {
                                            DataRow[] foundRow = distinctValues.Select("TableName='" + dsTable.Tables[0].Rows[inttable]["TableName"] + "'");
                                            if (foundRow.Length <= 0)
                                            {
                                                DataRow dr = distinctValues.NewRow();
                                                dr["TableName"] = dsTable.Tables[0].Rows[inttable]["TableName"].ToString();
                                                distinctValues.Rows.Add(dr);
                                                distinctValues.AcceptChanges();

                                            }
                                        }
                                        ArrayList arr = new ArrayList();

                                        for (int i = 0; i < distinctValues.Rows.Count; i++)
                                        {
                                            arr.Add(distinctValues.Rows[i]["TableName"].ToString());
                                        }
                                        arr.Sort();
                                        foreach (string strpatient in arrlistPatient)
                                        {
                                            string PatientId = strpatient;
                                            for (int i = 0; i < arr.Count; i++)
                                            {
                                                string strTableName = arr[i].ToString();
                                                DataRow[] foundRow = dtArray.Select("TableName='" + strTableName + "'");
                                                if (foundRow.Length > 0)
                                                {
                                                    for (int j = 0; j < foundRow.Length; j++)
                                                    {

                                                        if (queryInsert == string.Empty && queryselect == string.Empty)
                                                        {
                                                            if (strTableName == "PatientDetails_0")
                                                            {
                                                                queryInsert = "insert into " + strTableName + "(StudyCode,PatientID,CreatedDate,CreatedBy,";
                                                                queryselect = "Select '" + StudyCode + "',PatientID,getdate(),'" + Session["Login"].ToString() + "',";
                                                            }
                                                            else
                                                            {
                                                                queryInsert = "insert into " + strTableName + "(StudyCode,Patient,";
                                                                queryselect = "Select '" + StudyCode + "', Patient,";
                                                            }
                                                        }
                                                        queryInsert = queryInsert + "[" + foundRow[j]["FieldName"] + "],";
                                                        queryselect = queryselect + foundRow[j]["TableName"] + ".[" + foundRow[j]["FieldName"] + "],";
                                                    }
                                                }
                                                else
                                                {
                                                    if (strTableName == "PatientDetails_0")
                                                    {
                                                        queryInsert = "insert into " + strTableName + "(StudyCode,PatientID,CreatedDate,CreatedBy,";
                                                        queryselect = "Select '" + StudyCode + "',PatientID,getdate(),'" + Session["Login"].ToString() + "',";
                                                    }
                                                    else
                                                    {
                                                        queryInsert = "insert into " + strTableName + "(StudyCode,Patient,";
                                                        queryselect = "Select '" + StudyCode + "', Patient,";
                                                    }
                                                }

                                                queryInsert = queryInsert.TrimEnd(',') + ")";
                                                if (strTableName == "PatientDetails_0")
                                                    queryselect = queryselect.TrimEnd(',') + "  from " + strTableName + "  where PatientID = " + PatientId + "";
                                                else
                                                    queryselect = queryselect.TrimEnd(',') + "  from " + strTableName + "  where Patient = " + PatientId + " ";
                                                queryValues = InsertValuesFromSelectQuery(queryselect);


                                                arrquery.Add(queryInsert + queryValues);
                                                SqlCommand cmd = new SqlCommand(queryInsert + queryValues, cn);
                                                cmd.ExecuteNonQuery();
                                                queryInsert = string.Empty;
                                                queryselect = string.Empty;
                                            }

                                        }

                                        Query = "update Studies set StudyPatientsList='' where StudyCode='" + StudyCode + "'";
                                        GlobalValues.ExecuteNonQuery(Query);
                                        GlobalValues.UpdateStudyWithPatientID(Convert.ToString(Session["PatientID"]));
                                        lblMessage.ForeColor = GlobalValues.SucessColor;
                                        lblMessage.Text = "Patient Data send to Cloud Sucessfully.";
                                        SqlParameter[] param = new SqlParameter[8];
                                        param[0] = GlobalValues.addParameter("@UserID", Session["Login"].ToString());
                                        param[1] = GlobalValues.addParameter("@CreatedDate", SqlDbType.DateTime, DateTime.Now.ToString());
                                        param[2] = GlobalValues.addParameter("@StudyCode", SqlDbType.Text, StudyCode);
                                        param[3] = GlobalValues.addParameter("@StudyPatientsList", SqlDbType.Text, StudyPatientsList);
                                        param[4] = GlobalValues.addParameter("@StudyFieldsListCSV", SqlDbType.Text, StudyFieldsListCSV);
                                        param[5] = GlobalValues.addParameter("@StudyCriteria", SqlDbType.Text, StudyCriteria);
                                        param[6] = GlobalValues.addParameter("@CumpulsoryFieldsCriteria", SqlDbType.Text, CumpulsoryFieldsCriteria);
                                        param[7] = GlobalValues.addParameter("@CumpulsoryFields", SqlDbType.Text, CumpulsoryFields);

                                        StudyCriteria = StudyCriteria.Replace("''", "'''");
                                        StudyCriteria = StudyCriteria.Replace("'", "''");

                                        StudyPatientsList = StudyPatientsList.Replace("''", "'''");
                                        StudyPatientsList = StudyPatientsList.Replace("'", "''");

                                        CumpulsoryFieldsCriteria = CumpulsoryFieldsCriteria.Replace("''", "'''");
                                        CumpulsoryFieldsCriteria = CumpulsoryFieldsCriteria.Replace("'", "''");

                                        String AuditQuery = "insert into Audit_SendDatatoStudy(UserID,CreatedDate,StudyCode,StudyPatientsList,StudyFieldsListCSV,StudyCriteria,CumpulsoryFieldsCriteria,CumpulsoryFields)";
                                        AuditQuery += "values('" + Session["Login"].ToString() + "',getdate(),'" + StudyCode + "','" + StudyPatientsList + "','" + StudyFieldsListCSV + "','" + StudyCriteria + "','" + CumpulsoryFieldsCriteria + "','" + CumpulsoryFields + "')";
                                        lblMessage.ForeColor = GlobalValues.SucessColor;
                                        int records = SqlHelper.ExecuteNonQuery(GlobalValues.strAuditConnString, CommandType.Text, AuditQuery, param);
                                        //lblMessage.Text += "Data Added to Audit Trial successfully";

                                        cn.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        //sqltr.Rollback();
                                        cn.Close();
                                        throw (ex);
                                    }
                                }
                                else
                                {
                                    lblMessage.ForeColor = GlobalValues.FailureColor;
                                    lblMessage.Text = "None of the Patient satisfying the selected study Criteria.";
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        else
                        {
                            lblMessage.ForeColor = GlobalValues.FailureColor;
                            lblMessage.Text = "Same patient already exported.";
                            return;

                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        private DataTable FindTableNameFromPDFields(SqlConnection cn, string AllStudyField, DataTable dt)
        {
            DataSet ds = new DataSet();
            string TableName = string.Empty;
            string query = "select t.name as TableName,col.name  as FieldName from sys.tables t inner join sys.columns  col on t.object_id=col.object_id  and   t.name like 'PatientDetails_%'  and  col.name  in ('" + AllStudyField + "')";
            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            dt = ds.Tables[0].Copy();
            return dt;
        }

        private bool booleanInstance(string StudyPatientsList, SqlConnection cn, string Instance, string StudyCriteria, string CumpulsoryFieldsQuery, string StudyCode)
        {
            string query = "select isnull(count(*),0) " + GlobalValues.glbFromClause + " where PatientDetails_0.PatientID like '" + Instance + "%' and PatientDetails_0.StudyCode='" + StudyCode + "' ";
            if (StudyCriteria != string.Empty)
                query = query + "and " + StudyCriteria;
            if (CumpulsoryFieldsQuery != string.Empty)
                query = query + CumpulsoryFieldsQuery;
            SqlCommand cmd = new SqlCommand(query, cn);
            var result = cmd.ExecuteScalar();
            bool bvalreturn = false;

            string PatientId = "";
            if (Convert.ToString(Session["PatientID"]) != string.Empty)
                PatientId = "'" + Convert.ToString(Session["PatientID"]) + "'";


            if (StudyPatientsList == string.Empty || StudyPatientsList == PatientId)
            {
                if (Convert.ToInt32(result) <= 0)
                {
                    bvalreturn = true;
                }
            }
            else
            {
                string[] split = StudyPatientsList.Split(',');
                int length = split.Length;
                if (Convert.ToInt32(result) != length)
                {
                    bvalreturn = true;
                    string Query = "Delete from PatientDetails_0  where StudyCode='" + StudyCode + "'";
                    cmd = new SqlCommand(Query, cn);
                    cmd.ExecuteNonQuery();
                }
            }

            return bvalreturn;

        }


        private string InsertValuesFromSelectQuery(string strquery)
        {
            string strValues = string.Empty;

            DataSet ds = GlobalValues.ExecuteDataSet(strquery);
            if (ds.Tables.Count > 0)
            {
                if (strValues == string.Empty)
                {
                    strValues = "  Values(";

                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {

                        string Result = "";
                        Result = ds.Tables[0].Rows[i][j].ToString().Replace("'", "''");
                        int hypencount = Result.Split('-').Length - 1;
                        if (hypencount == 2)
                        {
                            try
                            {
                                Result = Convert.ToDateTime(Result).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                            }
                        }

                        if (Result.Trim() != string.Empty)
                            strValues = strValues + "'" + Result + "',";
                        else
                            strValues = strValues + " NULL,";
                    }
                }
            }
            strValues = strValues.TrimEnd(',');
            return strValues + ")";

        }

        private void Clear()
        {
            lblMessage.Text = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProjectForm.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            Clear();
        }


        protected void btnAuditTrail_Click(object sender, EventArgs e)
        {
            pnlSelect.Visible = true;
            ModalPopupExtender2.Show();
            BindGridView();
        }

        private void BindGridView()
        {
            DataSet dsExportData = new DataSet();
            string ExportQuery = "select UserID,CreatedDate,StudyCode,StudyPatientsList,StudyFieldsListCSV,StudyCriteria,CumpulsoryFieldsCriteria,CumpulsoryFields from Audit_SendDatatoStudy order by CreatedDate desc";
            dsExportData = SqlHelper.ExecuteDataset(GlobalValues.strAuditConnString, CommandType.Text, ExportQuery);
            GridExportAudit.DataSource = dsExportData;
            GridExportAudit.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dsExportData = new DataSet();
                string ExportQuery = "select UserID,CreatedDate,StudyCode,StudyPatientsList,StudyFieldsListCSV,StudyCriteria,CumpulsoryFieldsCriteria,CumpulsoryFields from Audit_SendDatatoStudy order by CreatedDate desc";
                dsExportData = SqlHelper.ExecuteDataset(GlobalValues.strAuditConnString, CommandType.Text, ExportQuery);
                GridExportAudit.DataSource = dsExportData;
                GridExportAudit.DataBind();
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


        protected void GridExportAudit_PageIndexChanging(object sender, GridViewPageEventArgs e)//Code modified on April 18,2015-Subhashini
        {

            DataSet dsExportData = new DataSet();
            string ExportQuery = "select UserID,CreatedDate,StudyCode,StudyPatientsList,StudyFieldsListCSV,StudyCriteria,CumpulsoryFieldsCriteria,CumpulsoryFields from Audit_SendDatatoStudy order by CreatedDate desc";
            dsExportData = SqlHelper.ExecuteDataset(GlobalValues.strAuditConnString, CommandType.Text, ExportQuery);
            GridExportAudit.PageIndex = e.NewPageIndex;
            GridExportAudit.DataSource = dsExportData;
            GridExportAudit.DataBind();
            ModalPopupExtender2.Show();
        }
    }
}