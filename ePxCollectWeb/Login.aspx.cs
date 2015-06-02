using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ePxCollectDataAccess;
using System.Collections;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Web.SessionState;
using System.Reflection;
using System.Web.Services;

namespace ePxCollectWeb
{
    public partial class Login : System.Web.UI.Page
    {

        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
        static string strConns = "";
        Thread thread;
        DataSet dsChangePassword = new DataSet();
        string userName = "";
        string OldSession = "";
        string ChangePasswordMessage = "";
        string ActiveUser = "";
        string MultipleUser = "";
        string ResetPasswordForcedLogout = "";
        string LiveUpdateForcedLogout = "";
        string UserId = "";
        string PatientId = "";

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


            strConns = GlobalValues.strConnString;
            //GlobalValues.gPostBackURL = "";
            Session["gPostBackURL"] = "";
            if (HttpContext.Current.Session != null)
            {

                if (Session["ActiveUser"] != null)
                {
                    ActiveUser = Session["ActiveUser"].ToString();
                }
                if (Session["ChangePassword"] != null)
                {
                    ChangePasswordMessage = Session["ChangePassword"].ToString();
                }

                if (Session["MutipleUserLogOut"] != null)
                {
                    MultipleUser = Session["MutipleUserLogOut"].ToString();
                    if (MultipleUser.Trim() == string.Empty)
                        MultipleUser = "Unknown User";
                }

                if (Session["ResetPasswordForcedLogOut"] != null)
                {
                    ResetPasswordForcedLogout = Session["ResetPasswordForcedLogOut"].ToString();
                }
                if (Session["LiveUpdate"] != null)
                {
                    LiveUpdateForcedLogout = Session["LiveUpdate"].ToString();
                }

                if (Session["Login"] != null)
                {
                    UserId = Session["Login"].ToString();
                }

                if (Session["PatientID"] != null)
                {
                    PatientId = Session["Login"].ToString();
                }
            }


            if (Request.UrlReferrer != null)
            {
                if (Request.UrlReferrer.ToString().IndexOf("Login.aspx") != -1)
                {
                    if (!IsPostBack)
                    {
                        //pnllogin.DefaultButton = submitLogin.UniqueID;
                        Session.RemoveAll();
                        Session["UserName"] = "";
                        Session["Login"] = "";
                        GlobalValues.gMenuType = "";
                        Session["gMenuType"] = "";
                        //txtPasword.Focus();                        
                        txtUsrName.Focus();
                    }
                    if (TextBox1.Text != "")
                    {
                        TextBox1.Focus();
                        //btnYes.Focus();
                    }
                    //if (Request["__EVENTTARGET"] == "btnYes")//Code Modified on April 18,2015-Subhashini
                    //{
                    //    if (pnlYESNO.Visible == true)
                    //        btnYes_Click(sender, e);
                    //}

                    //if (Request["__EVENTTARGET"] == "submitLogin")//Code Modified on April 18,2015-Subhashini
                    //{
                    //    btnLogin_Click(sender, e);
                    //}

                }
            }


            if (Convert.ToString(UserId) != "")
            {
                GlobalValues.UnlockUser(Convert.ToString(UserId), Convert.ToString(PatientId));
                thread = new Thread(new ThreadStart(WorkThreadFunction));
                thread.Start();
            }


            if (ActiveUser != string.Empty)
            {
                string Message = "Now you have been Logged out , as the System Administrator has deactive your account.";
                Session.Remove("ActiveUser");
                Session.Remove("PatientID");
                ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "alert('" + Message + "');", true);
            }


            if (LiveUpdateForcedLogout != string.Empty)
            {
                string Message = "Now you have been Logged out as the System is being upgraded by the System Administrator.";
                Session.Remove("LiveUpdate");
                Session.Remove("PatientID");
                ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "alert('" + Message + "');", true);
            }


            if (ChangePasswordMessage != string.Empty)
            {
                Session.Remove("ChangePassword");
            }
            if (MultipleUser != string.Empty)
            {

                if (MultipleUser != "Unknown User")
                {
                    string Meaasge = "You have been force Logged out, as the same user is logged in from IP :" + MultipleUser;
                    Session.Remove("MutipleUserLogOut");
                    Session.Remove("PatientID");
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "alert('" + Meaasge + "');", true);
                    MultipleUser = "";//Code added on April 3,2015-Subhashini
                }
                else
                {
                    string Meaasge = "You have been force Logged out, as the same user is trying to Log In";
                    Session.Remove("MutipleUserLogOut");
                    Session.Remove("PatientID");
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "alert('" + Meaasge + "');", true);
                    MultipleUser = "";//Code added on April 3,2015-Subhashini
                }
            }

            if (ResetPasswordForcedLogout != string.Empty)
            {
                string Meaasge = "You have been force logged out, because your password has been resetted by Administrator.";
                Session.Remove("ResetPasswordForcedLogOut");
                Session.Remove("PatientID");
                ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "alert('" + Meaasge + "');", true);
            }



        }


        protected void Page_LoadCompleted(object sender, EventArgs e)
        {



        }
        void regenerateId()
        {
            System.Web.SessionState.SessionIDManager manager = new System.Web.SessionState.SessionIDManager();
            string oldId = manager.GetSessionID(Context);
            string newId = manager.CreateSessionID(Context);
            bool isAdd = false, isRedir = false;
            manager.SaveSessionID(Context, newId, out isRedir, out isAdd);
            HttpApplication ctx = (HttpApplication)HttpContext.Current.ApplicationInstance;
            HttpModuleCollection mods = ctx.Modules;
            System.Web.SessionState.SessionStateModule ssm = (SessionStateModule)mods.Get("Session");
            System.Reflection.FieldInfo[] fields = ssm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            SessionStateStoreProviderBase store = null;
            System.Reflection.FieldInfo rqIdField = null, rqLockIdField = null, rqStateNotFoundField = null;
            foreach (System.Reflection.FieldInfo field in fields)
            {
                if (field.Name.Equals("_store")) store = (SessionStateStoreProviderBase)field.GetValue(ssm);
                if (field.Name.Equals("_rqId")) rqIdField = field;
                if (field.Name.Equals("_rqLockId")) rqLockIdField = field;
                if (field.Name.Equals("_rqSessionStateNotFound")) rqStateNotFoundField = field;
            }
            object lockId = rqLockIdField.GetValue(ssm);
            if ((lockId != null) && (oldId != null)) store.ReleaseItemExclusive(Context, oldId, lockId);
            rqStateNotFoundField.SetValue(ssm, true);
            rqIdField.SetValue(ssm, newId);
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
        #region "Cloud Connections"

        /// <summary>
        /// Live Update Involves 14 Tables send data from Cloud to Local and Create new columns in PD_0 to PD_11 Tables
        /// </summary>
        private void LiveUpdate()
        {
            ArrayList arrdelete = new ArrayList();
            //arrdelete.Add("Disclaimer");
            arrdelete.Add("Studies");
            //arrdelete.Add("DrugNames");
            //arrdelete.Add("DrugGroupsByStudy");
            //arrdelete.Add("Diagnosis");
            //arrdelete.Add("PDFields");
            //arrdelete.Add("FixedDrops");
            //arrdelete.Add("PDScreenMaster_Study");
            //arrdelete.Add("PDScreenMaster_Diagnosis");
            //arrdelete.Add("PDScreenMaster_Preset");
            //arrdelete.Add("DBStatements");
            //arrdelete.Add("FieldValues_ByStudy");
            //arrdelete.Add("FieldValues_ByDiagnosis");


            //arrdelete.Add("Recurrences");
            //arrdelete.Add("PatientTestsByline");
            //arrdelete.Add("PatientDrugsByLine");

            //string connectionstring = "Server=CBS;Database=OncoCollect_Master_DB_Test;User ID=sa;Password=CODEbase@123;";
            string connectionstring = "Server=tcp:v5tuxu7dh2.database.windows.net,1433;Database=OCollect_Master_DB;User ID=Venkat@v5tuxu7dh2;Password=Tummychair64#;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            using (SqlConnection cn = new SqlConnection(connectionstring))
            {
                try
                {
                    cn.Open();
                    for (int arrcount = 0; arrcount < arrdelete.Count; arrcount++)
                    {

                        if (arrdelete[arrcount].ToString() != "Recurrences" && arrdelete[arrcount].ToString() != "PatientTestsByline" && arrdelete[arrcount].ToString() != "PatientDrugsByLine")
                        {
                            string AllQuery = string.Empty;
                            DataSet ds = new DataSet();
                            string Query = "select * from " + arrdelete[arrcount] + "";
                            SqlCommand cmd = new SqlCommand(Query, cn);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(ds);

                            GlobalValues.ExecuteNonQuery("delete from " + arrdelete[arrcount] + "");
                            string query = "";
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string Result = "";
                                query = "insert into " + arrdelete[arrcount] + "(";

                                StringBuilder output = new StringBuilder();
                                for (int k = 0; k < ds.Tables[0].Columns.Count; k++)
                                {
                                    string ColumnName = ds.Tables[0].Columns[k].ColumnName.ToString();
                                    query = query + "[" + ColumnName + "],";
                                    Result = ds.Tables[0].Rows[i][k].ToString().Replace("'", "''");
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
                                    output.AppendFormat("'{0}',", Result);
                                }
                                query = (query).TrimEnd(',') + ")";
                                query = ((query + " Values(" + output).TrimEnd(',')) + ")";
                                AllQuery = AllQuery + "\n" + query;
                                if (arrdelete[arrcount].ToString() == "FixedDrops")
                                {
                                    GlobalValues.ExecuteNonQuery("set identity_insert  " + arrdelete[arrcount].ToString() + "  ON   " + query);
                                    GlobalValues.ExecuteNonQuery("set identity_insert " + arrdelete[arrcount].ToString() + " OFF");
                                }
                                else
                                    GlobalValues.ExecuteNonQuery(query);
                            }

                            if (arrdelete[arrcount].ToString() == "PDFields")
                                LoopThroughPDFieldsCompareTables();
                        }
                        else
                        {
                            DataSet dsserver = new DataSet();
                            string Query = "SELECT c1.COLUMN_NAME, 'ALTER  TABLE  '+c1.TABLE_NAME +' ADD  '+c1.COLUMN_NAME+' '  +  case  when c1.CHARACTER_MAXIMUM_LENGTH is not null then  c1.DATA_TYPE+'('+Convert(varchar,c1.CHARACTER_MAXIMUM_LENGTH)+')' else  c1.DATA_TYPE end  as 'AlterQuery' FROM INFORMATION_SCHEMA.COLUMNS c1  where c1.TABLE_NAME='" + arrdelete[arrcount] + "'";
                            SqlCommand cmd = new SqlCommand(Query, cn);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dsserver);

                            DataSet dslocal = GlobalValues.ExecuteDataSet("select * from " + arrdelete[arrcount] + "");
                            for (int i = 0; i < dsserver.Tables[0].Rows.Count; i++)
                            {
                                string columnName = dsserver.Tables[0].Rows[i]["COLUMN_NAME"].ToString();
                                DataColumnCollection columns = dslocal.Tables[0].Columns;
                                if (!columns.Contains(columnName))
                                {

                                    string alterQuery = dsserver.Tables[0].Rows[i]["AlterQuery"].ToString();
                                    GlobalValues.ExecuteNonQuery(alterQuery);
                                }
                            }
                        }
                    }

                }
                catch
                {
                    throw;

                }
            }


        }


        private void ConnectToCloudDatabaseCloudToLocalPDFields()
        {
            try
            {
                string field = string.Empty;
                string connectionstring = "Server=tcp:v5tuxu7dh2.database.windows.net,1433;Database=OCollect_Master_DB;User ID=Venkat@v5tuxu7dh2;Password=Tummychair64#;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
                using (SqlConnection cn = new SqlConnection(connectionstring))
                {
                    DataSet ds = new DataSet();
                    cn.Open();
                    string Query = "select [Table Name] as TableName,[Field Name] as FieldName ,* from PDFields order by  [Table Name] ,[Field Name] asc  ";

                    SqlCommand cmd = new SqlCommand(Query, cn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    int count = 0;
                    DataSet dspdfieldslocal = GlobalValues.ExecuteDataSet(Query);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        DataRow[] foundRow = dspdfieldslocal.Tables[0].Select("TableName='" + ds.Tables[0].Rows[i]["TableName"].ToString() + "' and FieldName='" + ds.Tables[0].Rows[i]["FieldName"].ToString() + "'");
                        if (foundRow.Length <= 0)
                        {
                            //foundRow = dspdfieldslocal.Tables[0].Select("FieldOrder='" + ds.Tables[0].Rows[i]["FieldOrder"].ToString() + "' ");
                            //if (foundRow.Length > 0)
                            {
                                count += 1;
                                string FieldName = ds.Tables[0].Rows[i]["FieldName"].ToString();
                                string TableNAme = ds.Tables[0].Rows[i]["TableName"].ToString();
                                string query = "insert into PDFields ";
                                query = query + InsertValuesFromSelectQuery("select * from PDFields where [Table Name]='" + ds.Tables[0].Rows[i]["TableName"].ToString() + "' and  [Field Name]='" + ds.Tables[0].Rows[i]["FieldName"].ToString() + "' ", connectionstring);
                                GlobalValues.ExecuteNonQuery(query);
                            }
                        }
                    }
                    cn.Close();
                    int total = count;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        /// <summary>
        ///Send Study Data from Local to Cloud 
        /// </summary>
        private void ConnectToCloudDatabaseStudyImport(string StudyCode, string Instance)//Recurrences and PatientTestsByLine and PatientDrugsByLine not Implemented here TODO
        {
            //Before Send Data data to study Live Update Has to be done(If there is a Gap) -  Please request the system adminbistrartor to updte the system before send dta to study
            //Need to Check Instance Having rights for Send data to study
            //Patient Array List

            //Need to Check Rights
            var instanceQuery = "select top 1 instanceID from Instance";
            var instaneID = GlobalValues.ExecuteScalar(instanceQuery);
            string Query = "select * from Studies where Instances like '%-" + instaneID + "-%'";
            DataSet dsStudies = GlobalValues.ExecuteDataSet(Query);



            string LiveUpdateVersion = "";
            string connectionstring = "Server=CBS;Database=OCollect_Study_Cloud_30032015;User ID=sa;Password=CODEbase@123;";
            var LocalLiveUpdateVersion = GlobalValues.ExecuteScalarAudit("select top 1  isnull([Current Version Number],'') as LocalVersionNumber from Audit_LiveUpdateLog  order by [Date of Live Update] desc");
            using (SqlConnection cn = new SqlConnection(connectionstring))
            {
                LiveUpdateVersion = SqlHelper.ExecuteScalar(cn, CommandType.Text, "select top 1  isnull([Current Version Number],'') as LocalVersionNumber from Audit_LiveUpdateLog  order by [Date of Live Update] desc").ToString();
            }
            if (LiveUpdateVersion != string.Empty)
            {
                if (LocalLiveUpdateVersion.ToString() != LiveUpdateVersion)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Value", "alert('Please request the System Administrator to update the system before send data to Study');", true);
                    return;
                }
            }



            DataTable dtArray = new DataTable();

            dtArray.Columns.Add(new DataColumn("FieldName", typeof(string)));
            dtArray.Columns.Add(new DataColumn("TableName", typeof(string)));
            try
            {
                DataSet ds = new DataSet();

                Query = "select * from Studies where StudyCode='" + StudyCode + "'";//Study Code
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

                        bool BoolValueCount = booleanInstance(StudyPatientsList, cn, Instance, StudyCriteria, CumpulsoryFieldsCriteria, StudyCode);
                        if (StudyPatientsList != string.Empty || BoolValueCount)
                        {

                            try
                            {
                                string AllStudyField = "'" + StudyFieldsListCSV + "'";
                                AllStudyField = StudyFieldsListCSV.Replace(",", "','");

                                string AllPatientField = "";
                                //AllPatientField = StudyPatientsList.Replace(",", "','");
                                AllPatientField = StudyPatientsList;

                                if (StudyPatientsList == "" || BoolValueCount)
                                {
                                    string sqlStr = " SELECT STUFF((SELECT Distinct ',''' + RTRIM ( PatientDetails_0.PatientID) + ''''" + GlobalValues.glbFromClause + " ";

                                    if (StudyCriteria != string.Empty)
                                        sqlStr = sqlStr + "and " + StudyCriteria;
                                    if (CumpulsoryFieldsCriteria != string.Empty)
                                        sqlStr = sqlStr + CumpulsoryFieldsCriteria;

                                    sqlStr = sqlStr + "  FOR XML PATH('')) ,1,1,',') AS PatientID ";
                                    StudyPatientsList = GlobalValues.ExecuteScalar(sqlStr).ToString();
                                    StudyPatientsList = StudyPatientsList.TrimStart(',');
                                }

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
                                    if (AllPatientField != string.Empty)
                                    {
                                        Query = "Delete from PatientDetails_0 where PatientID in (" + StudyPatientsList + ") and StudyCode='" + StudyCode + "'";
                                        SqlCommand cmd = new SqlCommand(Query, cn);
                                        cmd.ExecuteNonQuery();
                                    }
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
                                    foreach (string strpatient in arrlistPatient)
                                    {

                                        string PatientId = strpatient;
                                        for (int i = 0; i < distinctValues.Rows.Count; i++)
                                        {
                                            string strTableName = distinctValues.Rows[i]["TableName"].ToString();
                                            DataRow[] foundRow = dtArray.Select("TableName='" + strTableName + "'");
                                            if (foundRow.Length > 0)
                                            {
                                                for (int j = 0; j < foundRow.Length; j++)
                                                {

                                                    if (queryInsert == string.Empty && queryselect == string.Empty)
                                                    {
                                                        if (strTableName == "PatientDetails_0")
                                                        {
                                                            queryInsert = "insert into " + strTableName + "(StudyCode,PatientId,";
                                                            queryselect = "Select '" + StudyCode + "',PatientId,";
                                                        }
                                                        else
                                                        {
                                                            queryInsert = "insert into " + strTableName + "(StudyCode,Patient,";
                                                            queryselect = "Select '" + StudyCode + "',Patient,  ";
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
                                                    queryInsert = "insert into " + strTableName + "(StudyCode,PatientId,";
                                                    queryselect = "Select '" + StudyCode + "',PatientId,";
                                                }
                                                else
                                                {
                                                    queryInsert = "insert into " + strTableName + "(StudyCode,Patient,";
                                                    queryselect = "Select '" + StudyCode + "',Patient,";
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

                                    //sqltr.Commit();
                                    cn.Close();
                                }
                                catch (Exception ex)
                                {
                                    //sqltr.Rollback();
                                    cn.Close();
                                    throw (ex);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
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

        private bool booleanInstance(string StudyPatientsList, SqlConnection cn, string Instance, string StudyCriteria, string CumpulsoryFieldsCriteria, string StudyCode)
        {
            bool bvalreturn = false;
            if (StudyPatientsList == string.Empty)
            {
                string query = "select count(*) " + GlobalValues.glbFromClause + " where PatientID like '" + Instance + "%' and StudyCode='" + StudyCode + "' ";
                if (StudyCriteria != string.Empty)
                    query = query + "and " + StudyCriteria;
                if (CumpulsoryFieldsCriteria != string.Empty)
                    query = query + CumpulsoryFieldsCriteria;
                SqlCommand cmd = new SqlCommand(query, cn);
                var result = cmd.ExecuteScalar();
                if (Convert.ToInt32(result) <= 0)
                {
                    bvalreturn = true;
                }
            }

            return bvalreturn;

        }
        private void MethodForStudyFunctionality(string Studyname)
        {


            string strsql = "Select StudyPatientsList from Studies where studyname='" + Studyname + "'";
            List<string> lstVals = new List<string>();
            string strCSVs = string.Empty;
            try
            {
                strCSVs = (string)GlobalValues.ExecuteScalar(strsql);
                if (strCSVs == null) { strCSVs = ""; }
            }
            catch (Exception)
            {

                strCSVs = string.Empty;
            }
            ArrayList arrlistPatient = new ArrayList();
            string[] strItems;
            strItems = strCSVs.Split(',');
            foreach (string strValue in strItems)
            {
                arrlistPatient.Add(strValue);
            }

            //Table Field Arra list
            strsql = "SELECT  StudyFieldsListCSV from Studies where studyname='" + Studyname + "'";
            lstVals = new List<string>();
            strCSVs = string.Empty;
            try
            {
                strCSVs = (string)GlobalValues.ExecuteScalar(strsql);
                if (strCSVs == null) { strCSVs = ""; }
            }
            catch (Exception)
            {

                strCSVs = string.Empty;
            }
            ArrayList arrlistFields = new ArrayList();

            strItems = strCSVs.Split(',');
            foreach (string strValue in strItems)
            {
                arrlistFields.Add(strValue);

            }

            ArrayList arr = new ArrayList();
            //Select Query

            DataTable dtArray = new DataTable();

            dtArray.Columns.Add(new DataColumn("TableName", typeof(string)));
            dtArray.Columns.Add(new DataColumn("FieldName", typeof(string)));
            dtArray.Columns.Add(new DataColumn("DataType", typeof(string)));



            foreach (string strFields in arrlistFields)
            {

                string PDFields = strFields;
                string strSQL = "SELECT [table Name] as TableName,[Field Name] as FieldName, data_type as 'DataType'  from PDFields  inner join information_schema.columns  on  table_name = PDFields.[Table Name] and   Column_Name='" + PDFields.Replace("[", "").Replace("]", "") + "'   where [Field Name] ='" + PDFields.Replace("[", "").Replace("]", "") + "'";
                DataSet ds = GlobalValues.ExecuteDataSet(strSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dtArray.NewRow();
                    dr = ds.Tables[0].Rows[0];
                    dtArray.ImportRow(dr);
                }
                else
                {

                }

            }
            ArrayList arrquery = new ArrayList();
            string queryselect = string.Empty;
            string queryInsert = string.Empty;
            string queryValues = string.Empty;
            DataView view = new DataView(dtArray.Copy());
            DataTable distinctValues = view.ToTable(true, "TableName");
            foreach (string strpatient in arrlistPatient)
            {

                string PatientId = strpatient;
                for (int i = 0; i < distinctValues.Rows.Count; i++)
                {
                    string strTableName = distinctValues.Rows[i]["TableName"].ToString();
                    DataRow[] foundRow = dtArray.Select("TableName='" + strTableName + "'");
                    for (int j = 0; j < foundRow.Length; j++)
                    {

                        //insert into Audit_PatientDetails_0(PatientID,PatientName,Locked,SortID) 
                        //select PatientName,PatientName,Locked,SortID from PatientDetails_0  where PatientName='6-1'

                        if (queryInsert == string.Empty && queryselect == string.Empty)
                        {
                            if (strTableName == "PatientDetails_0")
                            {
                                queryInsert = "insert into " + strTableName + "(PatientId,PatientName,Locked,SortID,";
                                queryselect = "Select PatientId,PatientName,Locked,SortID ,";
                            }
                            else
                            {
                                queryInsert = "insert into " + strTableName + "(Patient,";
                                queryselect = "Select Patient,  ";
                            }
                        }
                        queryInsert = queryInsert + "[" + foundRow[j]["FieldName"] + "],";
                        queryselect = queryselect + foundRow[j]["TableName"] + ".[" + foundRow[j]["FieldName"] + "],";
                    }
                    queryInsert = queryInsert.TrimEnd(',') + ")";
                    if (strTableName == "PatientDetails_0")
                        queryselect = queryselect.TrimEnd(',') + "  from " + strTableName + "  where PatientID = '" + PatientId + "'";
                    else
                        queryselect = queryselect.TrimEnd(',') + "  from " + strTableName + "  where Patient = '" + PatientId + "'";
                    queryValues = InsertValuesFromSelectQuery(queryselect);
                    arrquery.Add(queryInsert + queryValues);
                    queryInsert = string.Empty;
                    queryselect = string.Empty;
                }




            }
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
                        strValues = strValues + "'" + Result + "',";
                    }
                }
            }
            strValues = strValues.TrimEnd(',');
            return strValues + ")";

        }

        private string InsertValuesFromSelectQuery(string strquery, string connectionstring)
        {
            using (SqlConnection cn = new SqlConnection(connectionstring))
            {
                DataSet ds = new DataSet();
                string strValues = string.Empty;
                cn.Open();

                SqlCommand cmd = new SqlCommand(strquery, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

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
                            strValues = strValues + "'" + ds.Tables[0].Rows[i][j].ToString().Replace("'", "''") + "',";
                        }
                    }
                }
                strValues = strValues.TrimEnd(',');
                return strValues + ")";
            }

        }

        /// <summary>
        /// Loop through Local PD Fields and create Missed fields is Table
        /// </summary>
        private void LoopThroughPDFieldsCompareTables()
        {

            try
            {
                ArrayList arr = new ArrayList();
                string Query = "select [Table Name] as TableName,[Field Name] as FieldName ,* from PDFields order by  [Table Name] ,[Field Name] asc  ";
                DataSet dspdfieldslocal = GlobalValues.ExecuteDataSet(Query);
                for (int i = 0; i < dspdfieldslocal.Tables[0].Rows.Count; i++)
                {
                    string TableName = dspdfieldslocal.Tables[0].Rows[i]["TableName"].ToString();
                    string ColumnName = dspdfieldslocal.Tables[0].Rows[i]["FieldName"].ToString();
                    //ViewState["Column"] = TableName + ColumnName;
                    Query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME =  '" + TableName + "' and COLUMN_NAME='" + ColumnName + "'  ORDER BY ORDINAL_POSITION";
                    //ViewState["Query"] = Query;
                    var exists = GlobalValues.ExecuteScalar(Query);
                    if (exists == null)
                    {
                        //Need to Changed to Column [SQL Server Field Type] 
                        string DataTypepd = dspdfieldslocal.Tables[0].Rows[i]["DataType"].ToString();
                        string DataType = " nvarchar(255)";
                        string FieldWidth = dspdfieldslocal.Tables[0].Rows[i]["Field Width"].ToString();
                        if (dspdfieldslocal.Tables[0].Rows[i]["DataType"].ToString().ToUpper() == "TEXT")
                            DataType = " nvarchar(" + FieldWidth + ")";
                        if (dspdfieldslocal.Tables[0].Rows[i]["DataType"].ToString().ToUpper() == "LONG")
                            DataType = " numeric(" + FieldWidth + ",0)";
                        if (dspdfieldslocal.Tables[0].Rows[i]["DataType"].ToString().ToUpper() == "SINGLE")
                            DataType = " numeric(18,2)";
                        if (dspdfieldslocal.Tables[0].Rows[i]["DataType"].ToString().ToUpper() == "DATE")
                            DataType = " Datetime";
                        string alterQuery = "alter table " + TableName + " add  [" + ColumnName + "]" + DataType;
                        GlobalValues.ExecuteNonQuery(alterQuery);
                        if (TableName.Contains("PatientDetails_"))
                        {
                            string AuditTableName = "Audit_" + TableName;
                            alterQuery = "alter table " + AuditTableName + " add  [" + ColumnName + "]" + DataType;
                            SqlHelper.ExecuteNonQuery(GlobalValues.strAuditConnString, CommandType.Text, alterQuery);
                        }

                        arr.Add(alterQuery);

                    }
                }
                arr.Add("End");
            }
            catch (Exception ex)
            {
                //string sdsdsd = ViewState["Column"].ToString();
                //string sdsdscerd = ViewState["Query"].ToString();
                throw;
            }
        }



        /// <summary>
        /// Update Cloud DB Statements for SQLServer Queries
        /// </summary>
        private void ConnectToCloudLocaltoCloudDBStatements()
        {
            try
            {
                string field = string.Empty;
                string connectionstring = "Server=tcp:v5tuxu7dh2.database.windows.net,1433;Database=OCollect_Master_DB;User ID=Venkat@v5tuxu7dh2;Password=Tummychair64#;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
                using (SqlConnection cn = new SqlConnection(connectionstring))
                {
                    int count = 0;
                    string Query = "select * from DBStatements order by IDCounter";
                    DataSet dsDBStatements = GlobalValues.ExecuteDataSet(Query);
                    cn.Open();
                    for (int i = 0; i < dsDBStatements.Tables[0].Rows.Count; i++)
                    {
                        string SQLDBStmt = dsDBStatements.Tables[0].Rows[i]["SQLDBStatements"].ToString().Replace("'", "''");
                        string IDCounter = dsDBStatements.Tables[0].Rows[i]["IDCounter"].ToString();
                        string UpdateQuery = "update DBStatements set SQLDBStatements='" + SQLDBStmt + "' where IDCounter='" + IDCounter + "' ";
                        ViewState["UpQuery"] = UpdateQuery + i;
                        SqlCommand cmd = new SqlCommand(UpdateQuery, cn);
                        cmd.ExecuteNonQuery();
                        count += 1;
                    }
                    cn.Close();
                    int total = count;
                }
            }
            catch (Exception ex)
            {
                string sfsd = ViewState["UpQuery"].ToString();
                throw ex;

            }

        }

        private void GetPatientTableFromCloud()
        {
            try
            {
                string field = string.Empty;
                string connectionstring = "Server=tcp:v5tuxu7dh2.database.windows.net,1433;Database=OCollect_Master_DB;User ID=Venkat@v5tuxu7dh2;Password=Tummychair64#;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
                using (SqlConnection cn = new SqlConnection(connectionstring))
                {
                    DataSet ds = new DataSet();
                    cn.Open();
                    string Query = "select [Table Name] as TableName,[Field Name] as FieldName ,* from PDFields order by  [Table Name] ,[Field Name] asc  ";

                    SqlCommand cmd = new SqlCommand(Query, cn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    int count = 0;
                    DataSet dspdfieldslocal = GlobalValues.ExecuteDataSet(Query);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                string sdsd = ex.Message;
            }
        }
        #endregion


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            GlobalValues.ForceLogOutForExpiredUsers();
            OncoCollectLookup myLookup = new OncoCollectLookup();
            string sesUserID = myLookup.sqlLookup("loginusers", "UserID", "SessionID='" + Session.SessionID + "'");
            string errMsg = "";
            if (sesUserID.Trim() != "")
            {
                Label3.Visible = true;
                if (txtUsrName.Value.Trim() == sesUserID.Trim())
                {
                    //Label3.Text = "You are already logged in. Working in multiple tabs are restricted";
                    errMsg = "You are already logged in. Working in multiple tabs are restricted.";
                    txtUsrName.Attributes.Remove("autofocus");
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "JsFunc", "showErrMsg('" + errMsg + "');", true);
                }
                else
                {
                    //Label3.Text = "Sorry, the application is already using by UserID: " + sesUserID + ". Multiple users login is restricted.";
                    errMsg = "Sorry, the application is already using by UserID: " + sesUserID + ". Multiple users login is restricted.";
                    txtUsrName.Attributes.Remove("autofocus");
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "JsFunc", "showErrMsg('" + errMsg + "');", true);
                }
                return;
            }

            string strSQL = "Select * From HospitalUsers HU inner join UserTypes Ut on UT.UserType=HU.UserType and HU.UserType not in ('Super Admin','Admin')  AND HU.Active=1 and  USERID like '" + txtUsrName.Value.Trim() + "'";
            Label3.Text = "";
            OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
            DataSet dsUsers = SqlHelper.ExecuteDataset(strConns, System.Data.CommandType.Text, strSQL);
            if (dsUsers.Tables[0].Rows.Count > 0)
            {
                string strPwd = dsUsers.Tables[0].Rows[0]["Password"].ToString();
                userName = dsUsers.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsUsers.Tables[0].Rows[0]["LastName"].ToString();
                GlobalValues.gUserTypeCriteria = dsUsers.Tables[0].Rows[0]["UserTypeCriteria"].ToString();
                //GlobalValues.gAdminUser = (bool)dsUsers.Tables[0].Rows[0]["AdminUser"];
                GlobalValues.gUserType = dsUsers.Tables[0].Rows[0]["UserType"].ToString();
                //if (GlobalValues.gAdminUser == null) 
                { GlobalValues.gAdminUser = false; }
                if (txtPasword.Value.Trim() == objEnc.Decrypt(strPwd))
                {
                    int SignCount = 0;
                    Session["LoginSessionId"] = Session.SessionID;
                    DataSet dsLogin = GlobalValues.ValidateSingleSignOnUser(txtUsrName.Value.Trim(), Session["LoginSessionId"].ToString(), GlobalValues.gEnterpriseApplicationName);
                    if (dsLogin.Tables.Count > 0)
                    {
                        SignCount = dsLogin.Tables[0].Rows.Count;
                    }
                    if (SignCount <= 0)
                    {
                        if (GlobalValues.BoolLiveUpdateProgress())
                        {
                            //Label3.Visible = true;
                            //Label3.Text = "You are unable to Login, because the System is being upgraded by the System Administrator.";
                            errMsg = "You are unable to Login, because the System is being upgraded by the System Administrator.";
                            txtUsrName.Attributes.Remove("autofocus");
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "JsFunc", "showErrMsg('" + errMsg + "');", true);
                        }
                        else
                            AllowUser(userName);
                        //this.Form.DefaultButton = btnYes.UniqueID;
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "ShowDisclaimerConfirm();", true);
                    }
                    else
                    {

                        var sessionID = GlobalValues.ExecuteScalar("Select isnull(Count(*),0) from LoginUsers  where SessionID='" + Session["LoginSessionId"].ToString() + "' ");
                        if (Convert.ToInt32(sessionID) > 0)
                        {
                            System.Web.SessionState.SessionIDManager manager = new System.Web.SessionState.SessionIDManager();
                            string newId = manager.CreateSessionID(Context);
                            Session["LoginSessionId"] = newId;

                        }
                        lblMultipleUser.Text = "You were already logged in from IP : " + dsLogin.Tables[0].Rows[0]["IPAddress"].ToString() + ". Do you Still prefer to Force Login now?";
                        Session["MutipleUserLogOut"] = GlobalValues.RemoveCurretLoginUsersDuringMutipleUserLogin(txtUsrName.Value.Trim(), Session["LoginSessionId"].ToString(), GlobalValues.gEnterpriseApplicationName, GlobalValues.gLHLogOut, "Force Logged out due to Multiple user Login");
                        if (GlobalValues.BoolLiveUpdateProgress())
                        {
                            //Label3.Visible = true;
                            //Label3.Text = "You are unable to Login, because the System is being upgraded by the System Administrator.";
                            errMsg = "You are unable to Login, because the System is being upgraded by the System Administrator.";
                            txtUsrName.Attributes.Remove("autofocus");
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "JsFunc", "showErrMsg('" + errMsg + "');", true);
                        }
                        else
                            AllowUser(userName);
                    }
                }
                else
                {
                    GlobalValues.InsertLoginHistory(txtUsrName.Value.Trim(), GlobalValues.gEnterpriseApplicationName, GlobalValues.gLHWrongPassword, "User Entered Wrong Password.");
                    //Label3.Text = "Please enter a valid User ID/Password.";
                    //Label3.Visible = true;
                    errMsg = "Please enter a valid User ID/Password.";
                    txtUsrName.Attributes.Remove("autofocus");
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "JsFunc", "showErrMsg('" + errMsg + "');", true);
                    //txtPasword.Focus();
                }

            }
            else
            {
                //Label3.Text = "Please enter a valid User ID/Password.";
                //Label3.Visible = true;
                //if (txtUsrName.Value.Trim() == string.Empty)
                //    txtUsrName.Focus();
                //else
                //    txtPasword.Focus();
                errMsg = "Please enter a valid User ID/Password.";
                txtUsrName.Attributes.Remove("autofocus");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "JsFunc", "showErrMsg('"+errMsg+"');", true);
            }
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            if (Session["Login"] != null)
            {

                GlobalValues.InsertCurrentLogInUser(Session["Login"].ToString(), GlobalValues.gEnterpriseApplicationName, Session["LoginSessionId"].ToString());
                GlobalValues.ForceLogOutForExpiredUsers();

                var ResetPassword = GlobalValues.ExecuteScalar("Select ResetPassword from HospitalUsers Where UserID='" + Session["Login"].ToString() + "'");

                if (Convert.ToString(ResetPassword) == "True")
                {
                    Session["ResetPassword"] = true;
                    Session["ResetPasswordMsg"] = "Please Change your password.";
                    Response.Redirect("Changepassword.aspx");
                }
                else
                    Response.Redirect("SearchPatient.aspx");

            }
            else
                Response.Redirect("Login.aspx");
        }

        [WebMethod (EnableSession=true)]
        public static string doRedirect()
        {
            string rtnPageName = "";
            if (HttpContext.Current.Session["Login"] != null)
            {

                GlobalValues.InsertCurrentLogInUser(HttpContext.Current.Session["Login"].ToString(), GlobalValues.gEnterpriseApplicationName, HttpContext.Current.Session["LoginSessionId"].ToString());
                GlobalValues.ForceLogOutForExpiredUsers();

                var ResetPassword = GlobalValues.ExecuteScalar("Select ResetPassword from HospitalUsers Where UserID='" + HttpContext.Current.Session["Login"].ToString() + "'");

                if (Convert.ToString(ResetPassword) == "True")
                {
                    HttpContext.Current.Session["ResetPassword"] = true;
                    HttpContext.Current.Session["ResetPasswordMsg"] = "Please Change your password.";
                    //HttpContext.Current.Response.Redirect("Changepassword.aspx");
                    rtnPageName = "Changepassword.aspx";
                }
                else
                    rtnPageName = "SearchPatient.aspx";
                //HttpContext.Current.Response.Redirect("SearchPatient.aspx");

            }
            else
                rtnPageName = "Login.aspx";
                //HttpContext.Current.Response.Redirect("Login.aspx");

            return rtnPageName;
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
            //txtPasword.Focus();
            //ClientScript.RegisterHiddenField("__EVENTTARGET", "btnNo");
        }

        protected void btnMutipleUser_Click(object sender, EventArgs e)
        {
            if (GlobalValues.FindCurrentUserExistsInLoginUsersTableBySession(txtUsrName.Value.Trim(), Session["LoginSessionId"].ToString()))
            {
            }
            Session["MutipleUserLogOut"] = GlobalValues.RemoveCurretLoginUsersDuringMutipleUserLogin(txtUsrName.Value.Trim(), Session["LoginSessionId"].ToString(), GlobalValues.gEnterpriseApplicationName, GlobalValues.gLHLogOut, "Force Logged out due to Multiple user Login");
            AllowUser(userName);
        }

        protected void btnMutipleUserCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx");
        }


        private void AllowUser(string userName)
        {
            Session["UserName"] = userName + " (" + txtUsrName.Value + ")";
            Session["Login"] = txtUsrName.Value;
            GlobalValues.ExecuteNonQuery("delete from LiveUpdateMessage where UserID ='" + Session["Login"].ToString() + "' ");
            ObjfeatureSet.GetUserPermissions(txtUsrName.Value, Session["LoginSessionId"].ToString());
            Session["FeatureSetPermission"] = ObjfeatureSet;
            //Response.Redirect("SearchPatient.aspx");
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "fnPopupDisclaimer();", true);
            string strDisc = Convert.ToString(GlobalValues.ExecuteScalar("Select isnull(Disclaimer,'') Disclaimer From Disclaimer"));
            if (strDisc == null) { strDisc = ""; }
            strDisc = strDisc.Replace("Onco Clinique", "Onco Clinique(TM)").Replace("OncoClinique", "Onco Clinique(TM)");
            strDisc = strDisc.Replace("\n\r", "<br><br>");
            //TextBox1.Text = strDisc;
            //ClientScript.RegisterHiddenField("__EVENTTARGET", "btnYes");//Code Modified on April 18,2015-Subhashini
            //TextBox1.Focus();
            //ModalPopupExtender1.Focus();
            //ModalPopupExtender1.Show();

            disclaimerDiag.InnerHtml = "<p style=\"text-align:justify; line-height:1;\">" + strDisc.Trim() + "</p><p style=\"text-align:center;\"><input class=\"button\" type=\"button\" id=\"discOK\" value=\"I Agree\" />&nbsp;&nbsp;<input class=\"button\" type=\"button\" id=\"discNO\" value=\"I Disagree\" /></p>";

            //call javascript function
            Page.ClientScript.RegisterStartupScript(this.GetType(), "JsFunc", "showDisclaimer();", true);
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "ShowDisclaimerConfirm();", true);

        }

        protected void lbtnForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ForgotPassword.aspx", false);
        }



    }
}