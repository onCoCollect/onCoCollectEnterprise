﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using ePxCollectDataAccess;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ePxCollectWeb
{
    public static class GlobalValues
    {
        #region Global varibales
        private static SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        private static string strConnStringTxt = System.Configuration.ConfigurationManager.ConnectionStrings["OncoCollectEnterprise"].ConnectionString;
        private static string strAuditConnStringTxt = System.Configuration.ConfigurationManager.ConnectionStrings["OncoCollectAudit"].ConnectionString;
        private static string strLUConnStringTxt = System.Configuration.ConfigurationManager.ConnectionStrings["OncoCollectLiveUpdate"].ConnectionString;

        private static string strEnterpriseDBName = "";
        private static int strSearchLength = Convert.ToInt32(ConfigurationManager.AppSettings["Search"].ToString());
        private static int strSearchLengthMax = Convert.ToInt32(ConfigurationManager.AppSettings["SearchMax"].ToString());
        private static string[] stringGridSize;
        private static int RowCount = Convert.ToInt32(ConfigurationManager.AppSettings["RowCount"].ToString());
        public static string glbFromClause = " FROM (((((((((((PatientDetails_0 INNER JOIN PatientDetails_1 ON PatientDetails_0.PatientID = PatientDetails_1.Patient) INNER JOIN PatientDetails_2 ON PatientDetails_0.PatientID = PatientDetails_2.Patient) INNER JOIN PatientDetails_3 ON PatientDetails_0.PatientID = PatientDetails_3.Patient) INNER JOIN PatientDetails_4 ON PatientDetails_0.PatientID = PatientDetails_4.Patient) INNER JOIN PatientDetails_5 ON PatientDetails_0.PatientID = PatientDetails_5.Patient) INNER JOIN PatientDetails_6 ON PatientDetails_0.PatientID = PatientDetails_6.Patient) INNER JOIN PatientDetails_7 ON PatientDetails_0.PatientID = PatientDetails_7.Patient) INNER JOIN PatientDetails_8 ON PatientDetails_0.PatientID = PatientDetails_8.Patient) INNER JOIN PatientDetails_9 ON PatientDetails_0.PatientID = PatientDetails_9.Patient) LEFT JOIN Recurrences ON PatientDetails_0.PatientID = Recurrences.PatientID) INNER JOIN PatientDetails_10 ON PatientDetails_0.PatientID = PatientDetails_10.Patient) INNER JOIN PatientDetails_11 ON PatientDetails_0.PatientID = PatientDetails_11.Patient ";
        public static string glbFromClauseForDiag = " FROM (((((((((((((PatientDetails_0 INNER JOIN PatientDetails_1 ON PatientDetails_0.PatientID = PatientDetails_1.Patient) INNER JOIN PatientDetails_2 ON PatientDetails_0.PatientID = PatientDetails_2.Patient) INNER JOIN PatientDetails_3 ON PatientDetails_0.PatientID = PatientDetails_3.Patient) INNER JOIN PatientDetails_4 ON PatientDetails_0.PatientID = PatientDetails_4.Patient) INNER JOIN PatientDetails_5 ON PatientDetails_0.PatientID = PatientDetails_5.Patient) INNER JOIN PatientDetails_6 ON PatientDetails_0.PatientID = PatientDetails_6.Patient) INNER JOIN PatientDetails_7 ON PatientDetails_0.PatientID = PatientDetails_7.Patient) INNER JOIN PatientDetails_8 ON PatientDetails_0.PatientID = PatientDetails_8.Patient) INNER JOIN PatientDetails_9 ON PatientDetails_0.PatientID = PatientDetails_9.Patient) LEFT JOIN Recurrences ON PatientDetails_0.PatientID = Recurrences.PatientID) INNER JOIN PatientDetails_10 ON PatientDetails_0.PatientID = PatientDetails_10.Patient) INNER JOIN PatientDetails_11 ON PatientDetails_0.PatientID = PatientDetails_11.Patient) LEFT OUTER JOIN PatientDrugsByLine On PatientDetails_0.PatientID = PatientDrugsByLine.PatientID) ";

        public static string gInstanceID = Convert.ToString(ExecuteScalar("select top 1 instanceID from Instance"));//"6";
        public static string QueryString = string.Empty; //Need to changed Session
        public static string gDisease = string.Empty; //Need to changed Session
        public static string gMenuType = "ByDiagnosis";
        public static string gMenuDropVal = string.Empty;
        public static string gPostBackURL = string.Empty;
        public static string gAuditQuery = string.Empty;
        public static string gUserTypeCriteria = string.Empty;//Need to changed Session
        public static string gUserType = string.Empty;//Need to changed Session
        public static Boolean gAdminUser = false;

        public static string gVDateOfBirth = "Date Of Birth";
        public static string gDVAgeAtDiagosis = "Age at Diagosis";
        public static string gDVDateOfDiagnosis = "Date Of Diagnosis";
        public static string gDVDateOfSurgery = "Date Of Surgery";
        public static string gDVRTStartDate = "R/T Start Date";
        public static string gDVBrainMets = "Date Of Brain Metastasis";
        public static string gDVLiverMets = "Date Of Liver Metastasis";
        public static string gDVRTEndDate = "R/T End Date";
        public static string gDVStatusDate = "Status Date";
        public static string gDVDateOfBiopsy = "Date Of Biopsy";
        public static string gDVDateOfDeath = "Date Of Death";

        public static string gDVAgeatFirstChild = "Age at First Child";
        public static string gDVAgeatMenarche = "Age at Menarche";
        //
        public static string gDV1stLineStartDate = "1st Line Start Date";
        public static string gDV1stLineEndDate = "1st Line End Date";

        public static string gDV2ndLineStartDate = "2nd Line Start Date";
        public static string gDV2ndLineEndDate = "2nd Line End Date";

        public static string gDV3rdLineStartDate = "3rd Line Start Date";
        public static string gDV3rdLineEndDate = "3rd Line End Date";

        public static string gDV4thLineStartDate = "4th Line Start Date";
        public static string gDV4thLineEndDate = "4th Line End Date";

        public static string gDV5thLineStartDate = "5th Line Start Date";
        public static string gDV5thLineEndDate = "5th Line End Date";

        public static string gDVDateNotReachableOn = "Patient Not Reachable on";

        public static System.Drawing.Color SucessColor = System.Drawing.Color.Green;
        public static System.Drawing.Color FailureColor = System.Drawing.Color.Red;


        public static string gEnterpriseApplicationName = "OncoCollect Enterprise";
        public static string gAdminApplicationName = "OncoCollect Admin";

        public static string gLHLogIN = "LogIn";
        public static string gLHLogOut = "LogOut";
        public static string gLHWrongPassword = "Wrong Password";
        public static string gLHSessionTimeOut = "Session TimeOut";
        public static string gLHError = "Exception Error";
        public static string gLHchangePassowrd = "Change Password";

        public static string SLogSearch = "Search";
        public static string SLogLabTest = "Lab Test";
        public static string SLogRecurrence = "Recurrences";
        public static string SLogStatus = "Status";
        public static string SLogSiteofMet = "Sites Of Metastasis";
        public static string SLogToxicity = "Worst Toxicity";
        public static string SLogCPwd = "Change Password";
        public static string SLogFPwd = "Forgot Password";
        #endregion


        public static string pSL_IPAddress = GetIPAddress();
        public static string pSL_PublicIPAddress = GetPublicIPAddress();
        // Declare a Name property of type string:

        public static string strConnString
        {
            get
            {
                OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
                return objEnc.SecureDecrypt(strConnStringTxt);

            }
            set
            {
                strConnStringTxt = value;
            }
        }

        public static string strLUConnString
        {
            get
            {
                OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
                return objEnc.SecureDecrypt(strLUConnStringTxt);

            }
            set
            {
                strLUConnStringTxt = value;
            }
        }


        public static string strEnterpriseDataBaseName
        {
            get
            {
                OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
                string EnterpriseConnectionStr = objEnc.SecureDecrypt(strConnStringTxt);
                builder.ConnectionString = EnterpriseConnectionStr;
                return builder.InitialCatalog;

            }
            set
            {
                strEnterpriseDBName = value;
            }
        }
        public static int excelCount
        {
            get
            {

                return (RowCount);

            }
            set
            {
                RowCount = value;
            }
        }
        public static int intSearchLength
        {
            get
            {

                return (strSearchLength);

            }
            set
            {
                strSearchLength = value;
            }
        }

        public static int intSearchLengthMax
        {
            get
            {
                return strSearchLengthMax;

            }
            set
            {
                strSearchLengthMax = value;
            }
        }

        public static string strAuditConnString
        {
            get
            {
                OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
                return objEnc.SecureDecrypt(strAuditConnStringTxt);

            }
            set
            {
                strAuditConnStringTxt = value;
            }
        }

        public static string[] strGridSize
        {
            get
            {
                string GridSize = (ConfigurationManager.AppSettings["GridSize"].ToString());
                string s = GridSize;
                string[] values = s.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i].Trim();
                }
                return values;

            }
            set
            {
                stringGridSize = value;
            }
        }

        public static SqlParameter addParameter(string name, SqlDbType dbType, string value)
        {
            SqlParameter Param = new SqlParameter();
            Param.ParameterName = name;
            Param.SqlDbType = dbType;
            Param.Value = value;
            return Param;

        }
        public static SqlParameter addParameter(string name, string value)
        {
            SqlParameter Param = new SqlParameter();
            Param.ParameterName = name;
            Param.SqlDbType = SqlDbType.NVarChar;
            Param.Size = 50;
            Param.Value = value;
            return Param;

        }
        public static SqlParameter addParameter(string name, SqlDbType dbType, DateTime value)
        {
            SqlParameter Param = new SqlParameter();
            Param.SqlDbType = dbType;
            Param.Value = value.ToString();
            return Param;

        }

        public static string PrepareSelectFromCSV(string strFldList)
        {
            string strSql = string.Empty;
            string[] strFlds = strFldList.Replace("\r\n", "\n").Split('\n');
            for (int i = 0; i <= strFlds.Length - 1; i++)
            {
                if (i == 0 && strFlds[i].Trim().ToUpper().StartsWith("SELECT"))
                {
                    strFlds[i].ToUpper().Replace("SELECT", "");
                }
                //if (strFlds[i].Split(',').Length > 0)
                //{
                strSql += "," + strFlds[i].Split(',')[0].ToString();
                //}
            }
            return "SELECT " + strSql.Substring(1);
        }
        public static void UnlockUser(string UserName, string PatientID)
        {
            string strConn = GlobalValues.strConnString.ToString();
            string sqlStr = "Update  PatientDetails_0 set Locked=0 Where PatientID ='" + PatientID + "' ";
            ePxCollectDataAccess.SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, "Delete from tblLock where UserName ='" + UserName + "'");
            SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, sqlStr);

            //SqlParameter[] Param = new SqlParameter[1];
            //Param[0] = new SqlParameter("@PatientID", SqlDbType.VarChar);
            //Param[0].Value = Convert.ToString(PatientID);
            //SqlHelper.ExecuteProcedure(strConn, "StudyPatientsList_Update", Param);
            UpdateStudyWithPatientID(PatientID);
            UpdateModifiedDate(PatientID, UserName);

            //SqlParameter[] param = new SqlParameter[1];
            //param[0] = new SqlParameter("@PatientId", SqlDbType.NVarChar);
            //param[0].Value = Convert.ToString(PatientID);
            //SqlHelper.ExecuteProcedure(strConn, "sp_DBStatements", param);
        }

        public static void DBStatements(string PatientID)
        {


            string strConn = GlobalValues.strConnString.ToString();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@PatientId", SqlDbType.NVarChar);
            param[0].Value = Convert.ToString(Convert.ToString(PatientID));
            SqlHelper.ExecuteProcedure(strConn, "sp_DBStatements", param);
        }
        public static void lockUser(string UserName, string PatientID)
        {
            if (UserName != "")
            {
                string strConn = GlobalValues.strConnString.ToString();
                string sqlStr = "Update  PatientDetails_0 set Locked=1 Where PatientID ='" + PatientID + "' ";
                //ePxCollectDataAccess.SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, "Delete from tblLock where UserName ='" + UserName + "'");
                SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, sqlStr);
                sqlStr = "Insert into tblLock (PatientID, UserName, LockedDate) values ('" + PatientID + "','" + UserName + "',getdate())";
                SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, sqlStr);
                UpdateStudyWithPatientID(PatientID);
            }
        }
        public static string PrepareFromClause(string strInput)
        {
            Int32 intPos = 0;
            string TableList = string.Empty;
            string tableName = string.Empty;
            intPos = strInput.IndexOf("PatientDetails_");
            string strFirstTable = string.Empty;
            if (intPos > 0)
            {
                while (intPos > 0)
                {
                    if (TableList.Length <= 0)
                    {
                        TableList = strInput.Substring(intPos, 16);
                        strFirstTable = "PatientDetails_0";//TableList;
                    }
                    else
                    {
                        tableName = strInput.Substring(intPos, 16);
                        if (!TableList.Contains(tableName))
                        {
                            if (tableName == "PatientDetails_0")
                            {
                                TableList += " Inner Join PatientDetails_0 on PatientDetails_0.PatientId =" + strFirstTable.Trim() + ".Patient ";
                            }
                            else
                            {
                                TableList += " Inner Join " + tableName + " on PatientDetails_0.PatientId =" + tableName.Trim() + ".Patient ";
                            }
                        }
                    }
                    intPos = strInput.IndexOf("PatientDetails_", intPos + 16);
                }
            }
            else
            {

                //string inputStr = strInput.ToUpper().Replace("AND", "1=1").Replace("OR", "1=1").Replace("IS NOT NULL", "1=1").Replace("IS NULL", "1=1").Replace("<>", "1=1").Replace(">", "1=1").Replace("<", "1=1").Replace(">=", "1=1").Replace("<=", "1=1");
                string inputStr = strInput.ToUpper().Replace(" AND ", ";").Replace(" OR ", ";").Replace("IS NOT NULL", "=").Replace("IS NULL", "=").Replace("<>", "=").Replace(">", "=").Replace("<", "=").Replace(">=", "=").Replace("<=", "=");
                string[] Collect = inputStr.Split(';');
                string strCol = string.Empty;

                if (Collect.Length > 0)
                {
                    for (int i = 0; i < Collect.Length - 1; i++)
                    {
                        string[] strColEq = Collect[i].Split('=');
                        if (!strCol.Contains(strColEq[0].Replace("[", "").Replace("]", "").Trim()))
                        {
                            strCol = strCol + ",'" + strColEq[0].Replace("[", "").Replace("]", "").Trim() + "'";
                        }
                        //strCol = strCol + ",'" + Collect[i].Replace("[", "").Replace("]", "").Trim() + "'";
                    }
                    if (strCol.Length > 0)
                    {
                        strCol = strCol.Substring(1);

                        strCol = strCol.Replace("LEN(", "").Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "");
                        string sqlStr = "Select distinct [Table Name] tableName from PDFields where [Field Name] in (" + strCol + ")";

                        DataSet dsTabls = ExecuteDataSet(sqlStr);
                        foreach (DataRow dr in dsTabls.Tables[0].Rows)
                        {
                            if (TableList.Length <= 0)
                            {
                                TableList = dr["tableName"].ToString();
                            }
                            else
                            {
                                TableList += " Inner Join " + dr["tableName"].ToString() + " on PatientDetails_0.PatientId =" + dr["tableName"].ToString().Trim() + ".Patient";
                            }

                        }
                    }
                }
            }
            if (TableList.Length > 0) { TableList = " from " + TableList; }
            else { TableList = glbFromClause; }

            return TableList;
        }
        public static void UpdateStudyWithPatientID(string PatientID)
        {
            if (!string.IsNullOrEmpty(PatientID))
            {
                string StrStudyCriteria = string.Empty;
                string SQL = "Select * from Studies where ACTIVE=1 and [instances] like  '%-" + gInstanceID + "-%'";
                DataSet ds = SqlHelper.ExecuteDataset(strConnString, CommandType.Text, SQL);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string CumpulsoryFieldsQuey = "";
                    if (dr["CumpulsoryFields"].ToString() != string.Empty)
                    {
                        string[] splitword = dr["CumpulsoryFields"].ToString().Split(',');
                        if (splitword.Length > 0)
                        {
                            for (int count = 0; count < splitword.Length; count++)
                            {
                                if (splitword[count].ToString().Trim() != string.Empty)
                                    CumpulsoryFieldsQuey = CumpulsoryFieldsQuey + "and  [" + splitword[count].ToString().Trim() + "] is not null and  [" + splitword[count].ToString().Trim() + "] <> '' ";
                            }
                        }

                    }

                    StrStudyCriteria = dr["StudyCriteria"].ToString() + CumpulsoryFieldsQuey;
                    string fromClause = PrepareFromClause(StrStudyCriteria);
                    SQL = "Select Count(Patientdetails_0.PatientID)  " + glbFromClause + " WHERE (" + StrStudyCriteria + ") and Patientdetails_0.PatientID ='" + PatientID + "'";
                    //SQL = "Select Count(Patientdetails_0.PatientID)  " + fromClause + " WHERE (" + StrStudyCriteria + ") and Patientdetails_0.PatientID ='" + PatientID + "'";
                    int rCnt = (int)SqlHelper.ExecuteScalar(strConnString, CommandType.Text, SQL);
                    if (rCnt > 0)
                    {
                        string strPList = dr["StudyPatientsList"].ToString();
                        if (strPList.Trim().Length > 0)
                        {
                            strPList += ",'" + PatientID.ToString() + "'";
                        }
                        else
                        {
                            strPList = "'" + PatientID.ToString() + "'";
                        }
                        SQL = "Update Studies Set StudyPatientsList = '" + strPList.Replace("'", "''") + "' where StudyName = '" + dr["StudyName"] + "'";
                        SqlHelper.ExecuteNonQuery(strConnString, CommandType.Text, SQL);
                    }
                }
            }
        }

        public static void WriteAuditRecord(string UserName, string PatientID, string Action, string Database)
        {
            try
            {


                //Commented by srinivas
                SqlConnection BMSCon = new SqlConnection();
                SqlCommand cmdBMS = new SqlCommand();
                BMSCon.ConnectionString = strAuditConnString;
                BMSCon.Open();
                cmdBMS.Connection = BMSCon;
                cmdBMS.CommandType = CommandType.StoredProcedure;
                cmdBMS.Parameters.AddWithValue("@PatientID", PatientID);
                cmdBMS.Parameters.AddWithValue("@UserName", UserName);
                cmdBMS.Parameters.AddWithValue("@Action", Action);
                cmdBMS.Parameters.AddWithValue("@DatabaseName", Database);
                cmdBMS.CommandText = "Onco_CreateAudit";
                cmdBMS.ExecuteNonQuery();
                if (BMSCon.State == ConnectionState.Open) BMSCon.Close();
                BMSCon = null;
                cmdBMS = null;
            }
            catch (Exception)
            {

                // throw;
            }
        }
        public static void UpdateModifiedDate(string PatientID, string LogInUserName)
        {
            try
            {
                SqlParameter[] Param = new SqlParameter[2];
                Param[0] = new SqlParameter("@PatientID", SqlDbType.VarChar);
                Param[0].Value = Convert.ToString(PatientID);
                Param[1] = new SqlParameter("@ModifiedBy", SqlDbType.VarChar);
                Param[1].Value = Convert.ToString(LogInUserName.ToString());
                SqlHelper.ExecuteProcedure(strConnString, "sp_UpdateModifiedDate", Param);
            }
            catch { }
        }
        public static string DateValidation(string PatientID, string ScreenName, string Description, DateTime DateToValidate, DateTime StartDate, int Age)
        {
            string returnmsg = string.Empty;
            try
            {
                SqlConnection BMSCon = new SqlConnection();
                SqlCommand cmdBMS = new SqlCommand();
                BMSCon.ConnectionString = strConnString;
                BMSCon.Open();
                cmdBMS.Connection = BMSCon;
                cmdBMS.CommandType = CommandType.StoredProcedure;
                cmdBMS.Parameters.AddWithValue("@PatientId", PatientID);
                cmdBMS.Parameters.AddWithValue("@ScreenName", ScreenName);
                if (StartDate.Year == 0001)
                    cmdBMS.Parameters.AddWithValue("@StartDate", DBNull.Value);
                else
                    cmdBMS.Parameters.AddWithValue("@StartDate", StartDate);
                cmdBMS.Parameters.AddWithValue("@DateToVaidate", DateToValidate);
                cmdBMS.Parameters.AddWithValue("@Age", Age);
                cmdBMS.Parameters.AddWithValue("@Description", Description);
                cmdBMS.CommandText = "sp_DateValidation";
                returnmsg = cmdBMS.ExecuteScalar().ToString();
                if (BMSCon.State == ConnectionState.Open) BMSCon.Close();
                BMSCon = null;
                cmdBMS = null;
            }
            catch (Exception)
            {

                throw;
            }

            return returnmsg;
        }
        public static DataSet CustomFormValidation(string PatientID, string ScreenName, string Description,
             string ParentField, string ParentFieldValue, string ParentFieldValueToValidate, string ChildField, string ChildFieldValue)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection BMSCon = new SqlConnection();
                SqlCommand cmdBMS = new SqlCommand();
                BMSCon.ConnectionString = strConnString;
                BMSCon.Open();
                cmdBMS.Connection = BMSCon;
                cmdBMS.CommandType = CommandType.StoredProcedure;
                cmdBMS.Parameters.AddWithValue("@PatientId", PatientID);
                cmdBMS.Parameters.AddWithValue("@ScreenName", ScreenName);
                cmdBMS.Parameters.AddWithValue("@ParentField", ParentField);
                cmdBMS.Parameters.AddWithValue("@ParentFieldValue", ParentFieldValue);
                cmdBMS.Parameters.AddWithValue("@ParentFieldValueToValidate", ParentFieldValueToValidate);
                cmdBMS.Parameters.AddWithValue("@ChildField", ChildField);
                cmdBMS.Parameters.AddWithValue("@ChildFieldValue", ChildFieldValue);
                cmdBMS.Parameters.AddWithValue("@Description", Description);
                cmdBMS.CommandText = "sp_CustomFormValidation";
                SqlDataAdapter da = new SqlDataAdapter(cmdBMS);
                da.Fill(ds);
                if (BMSCon.State == ConnectionState.Open) BMSCon.Close();
                BMSCon = null;
                cmdBMS = null;
            }
            catch (Exception)
            {

                throw;
            }

            return ds;
        }


        public static DataSet DependentFieldValidation(string PatientID, string ScreenName, string FieldName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection BMSCon = new SqlConnection();
                SqlCommand cmdBMS = new SqlCommand();
                BMSCon.ConnectionString = strConnString;
                BMSCon.Open();
                cmdBMS.Connection = BMSCon;
                cmdBMS.CommandType = CommandType.StoredProcedure;
                cmdBMS.Parameters.AddWithValue("@PatientId", PatientID);
                cmdBMS.Parameters.AddWithValue("@ScreenName", ScreenName);
                cmdBMS.Parameters.AddWithValue("@FieldName", FieldName);


                cmdBMS.CommandText = "sp_CustomFormValidationByRelatedGroup";
                SqlDataAdapter da = new SqlDataAdapter(cmdBMS);
                da.Fill(ds);
                if (BMSCon.State == ConnectionState.Open) BMSCon.Close();
                BMSCon = null;
                cmdBMS = null;
            }
            catch (Exception)
            {

                throw;
            }

            return ds;
        }



        public static DataSet ExecuteDataSet(string strSQL)
        {
            DataSet ds = SqlHelper.ExecuteDataset(strConnString, CommandType.Text, strSQL);
            return ds;
        }

        public static DataSet ExecuteDataSetAudit(string strSQL)
        {
            DataSet ds = SqlHelper.ExecuteDataset(strAuditConnString, CommandType.Text, strSQL);
            return ds;
        }
        public static void ExecuteNonQuery(string strSQL)
        {
            SqlHelper.ExecuteNonQuery(strConnString, CommandType.Text, strSQL);

        }

        public static void ExecuteNonQueryAudit(string strSQL)
        {
            SqlHelper.ExecuteNonQuery(strAuditConnString, CommandType.Text, strSQL);

        }
        public static object ExecuteScalar(string strSQL)
        {
            object retVal = SqlHelper.ExecuteScalar(strConnString, CommandType.Text, strSQL);
            return retVal;
        }

        public static object ExecuteScalarAudit(string strSQL)
        {
            object retVal = SqlHelper.ExecuteScalar(strAuditConnString, CommandType.Text, strSQL);
            return retVal;
        }

        //public static DataSet ExcuteStoredProc(string UserName, string PatientID, string Action)
        //{
        //    try
        //    {
        //        SqlConnection BMSCon = new SqlConnection();
        //        SqlCommand cmdBMS = new SqlCommand();
        //        DataSet dsREsult = new DataSet();
        //        BMSCon.ConnectionString = strConnString;
        //        BMSCon.Open();
        //        cmdBMS.Connection = BMSCon;
        //        cmdBMS.CommandType = CommandType.StoredProcedure;
        //        cmdBMS.Parameters.AddWithValue("@PatientID", PatientID);
        //        cmdBMS.Parameters.AddWithValue("@UserName", UserName);
        //        cmdBMS.Parameters.AddWithValue("@Action", Action);
        //        cmdBMS.CommandText = "Onco_CreateAudit";
        //        cmdBMS.execute;
        //        if (BMSCon.State == ConnectionState.Open) BMSCon.Close();
        //        BMSCon = null;
        //        cmdBMS = null;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        #region UserPermission Settings

        /*   enum FeatureSet
        {
            Registration = 1,
            DataAnalysis = 2,
            PatientAnalysisByallParameters = 3,
            PatientAnalysisByStudy = 4,
            StatusCheckList = 5,
            MySavedQueries = 6,
            Configuration = 7,
            DataGrouping = 8,
            ChangePassword = 9,
            Study = 10,
            ExportDatatoStudyPool = 11,
            ByDiagnosisForms = 12,
            ByStudyForms = 13,
            CustomForms = 14,
            Toxicity = 15,
            Recurrences = 16,
            LabTests = 17,
            Status = 18,
            CustomScreen = 19,
            SitesofMetastasis = 20,
            BottomNav = 21,
            Search = 22,
            DiseaseScreens = 23
        };
        public static bool isRegistration = false;
        public static bool isAnalysis = false;
        public static bool isAnalysisByStudy = false;
        public static bool isAnalysisByAllparams = false;
        public static bool isPresetForms = false;
        public static bool isStudyForms = false;
        public static bool isChangePassword = false;
        public static bool isToxicity = false;
        public static bool isRecurrence = false;
        public static bool isInvestigations = false;
        public static bool isStudy = false;
        public static bool isSaveQuery = false;
        public static bool isStatusCheckList = false;
        public static bool isStatus = false;
        public static bool isCustomScreen = false;
        public static bool isSitesOfMetsis = false;
        public static bool isBottomNav = false;
        public static bool isDiseaseDetails = false;
        public static bool isSearch = false;




        public static void GetUserPermissions(string UserLogin)
        {
            isAnalysisByAllparams = false;
            isAnalysisByStudy = false;
            isStudyForms = false;
            isRegistration = false;
            isAnalysis = false;
            isPresetForms = false;
            isChangePassword = false;
            isToxicity = false;
            isRecurrence = false;
            isInvestigations = false;
            isStudy = false;
            isSaveQuery = false;
            isStatusCheckList = false;
            isStatus = false;
            isCustomScreen = false;
            isSitesOfMetsis = false;
            isBottomNav = false;
            isDiseaseDetails = false;
            isStudyForms = false;
            isSearch = false;

            string sqlStr = "select FU.*, fs.FeatureSetName from FeatureSetUsers FU inner join FeatureSet Fs on fu.FeatureSetName=fs.FeatureSetName inner join HospitalUsers HU on fu.UserID=hu.UserID where FU.Enabled=1 and HU.UserID  ='" + UserLogin + "'";
            DataSet dsUserPerms = ExecuteDataSet(sqlStr);
            if (dsUserPerms.Tables.Count > 0)
            {
                foreach (DataRow dr in dsUserPerms.Tables[0].Rows)
                {
                    if (dr["FeatureSetName"].ToString() == FeatureSet.Registration.ToString())
                    {
                        isRegistration = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.DataAnalysis.ToString())
                    {
                        isAnalysis = true;
                    }
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.PatientAnalysisByStudy.ToString())
                    {
                        isAnalysisByStudy = true;
                    }
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.PatientAnalysisByallParameters.ToString())
                    {
                        isAnalysisByAllparams = true;
                    }
                    else if (((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("(", "")).Replace(")", "") == FeatureSet.ByDiagnosisForms.ToString())
                    {
                        isPresetForms = true;
                        isBottomNav = true;
                    }
                    else if (((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("(", "")).Replace(")", "") == FeatureSet.ByStudyForms.ToString())
                    {
                        isStudyForms = true;
                    }///
                    else if (((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("(", "")).Replace(")", "") == FeatureSet.CustomForms.ToString())
                    {
                        isCustomScreen = true;
                    }///
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.ChangePassword.ToString())
                    {
                        isChangePassword = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.Toxicity.ToString())
                    {
                        isToxicity = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "").Trim() == FeatureSet.Recurrences.ToString().Trim())
                    {
                        isRecurrence = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.LabTests.ToString())
                    {
                        isInvestigations = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.Study.ToString())
                    {
                        isStudy = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.MySavedQueries.ToString())
                    {
                        isSaveQuery = true;
                    }
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.StatusCheckList.ToString())
                    {
                        isStatusCheckList = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.Status.ToString())
                    {
                        isStatus = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.CustomForms.ToString())
                    {
                        isCustomScreen = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.SitesofMetastasis.ToString())
                    {
                        isSitesOfMetsis = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.BottomNav.ToString())
                    {
                        isBottomNav = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.DiseaseScreens.ToString())
                    {
                        isDiseaseDetails = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.Search.ToString())
                    {
                        isSearch = true;
                    }
                }
            }
        }*/

        public static bool TestConnectionStringSQLSERVER(string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    return (conn.State == ConnectionState.Open);
                }
                catch
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// Call the asssembly dynamically and execute a method
        ///
        /// </summary>
        /// <param name="AssemblyName">Name of the Assembly to be loaded</param>
        /// <param name="className">Name of the class to be intantiated </param>
        /// <param name="methodName">Name of the method to be called</param>
        /// <param name="parameterForTheMethod">Parameters should be passed as object array</param>
        /// <returns>Returns as Generic object..</returns>

        public static object Process(string AssemblyName, string className, string methodName, object[] parameterForTheMethod)
        {
            object returnObject = null;
            MethodInfo mi = null;
            ConstructorInfo ci = null;
            object responder = null;
            Type type = null;
            System.Type[] objectTypes;
            int count = 0;
            try
            {
                //Load the assembly and get it's information
                type = System.Reflection.Assembly.LoadFrom(AssemblyName + ".dll").GetType(AssemblyName + "." + className);


                //Get the Passed parameter types to find the method type
                objectTypes = new System.Type[parameterForTheMethod.GetUpperBound(0) + 1];
                foreach (object objectParameter in parameterForTheMethod)
                {
                    if (objectParameter != null)
                        objectTypes[count] = objectParameter.GetType();
                    count++;
                }

                //Get the reference of the method
                mi = type.GetMethod(methodName, objectTypes);
                ci = type.GetConstructor(Type.EmptyTypes);
                responder = ci.Invoke(null);
                //Invoke the method
                returnObject = mi.Invoke(responder, parameterForTheMethod);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mi = null;
                ci = null;
                responder = null;
                type = null;
                objectTypes = null;
            }

            //Return the value as a generic object
            return returnObject;

        }

        #endregion

        public static void SaveSessionLog(string PatientID, string UserId, string Action)
        {
            try
            {

                string strProName = "sp_SessionLog";
                SqlConnection oConnection = new SqlConnection(strConnString);
                oConnection.Open();
                SqlCommand oCmdInsertAIUsers = new SqlCommand(strProName, oConnection);
                oCmdInsertAIUsers.CommandType = CommandType.StoredProcedure;
                oCmdInsertAIUsers.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeOut"].ToString());
                oCmdInsertAIUsers.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = pSL_IPAddress;
                oCmdInsertAIUsers.Parameters.Add("@RemoteIPAddress", SqlDbType.VarChar).Value = pSL_PublicIPAddress;
                oCmdInsertAIUsers.Parameters.Add("@PatientId", SqlDbType.VarChar).Value = PatientID;
                oCmdInsertAIUsers.Parameters.Add("@UserId", SqlDbType.VarChar).Value = UserId;
                oCmdInsertAIUsers.Parameters.Add("@Action", SqlDbType.VarChar).Value = Action;
                oCmdInsertAIUsers.ExecuteNonQuery();
                oConnection.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string GetIPAddress()
        {
            string strHostName = "";
            string localIP = "";
            try
            {
                strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                foreach (IPAddress ip in ipEntry.AddressList)
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        localIP = ip.ToString();
                    }
                }

            }
            catch
            {
                strHostName = "";

            }
            return localIP.ToString();
            //IPAddress[] addr = ipEntry.AddressList;
            //return addr[addr.Length - 1].ToString();
        }
        public static string GetPublicIPAddress()
        {
            string PublicIPAddress = "";
            try
            {
                PublicIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            catch
            {
                PublicIPAddress = "";
            }
            return PublicIPAddress;
        }




        public static void ErrorLog(string UserID, string ErrorTrace, string ErrorMsg, string BrowserDetails, string ErrorLog)
        {

            SqlParameter[] Param = new SqlParameter[8];
            Param[0] = new SqlParameter("@ErrorLogDate", SqlDbType.DateTime);
            Param[1] = new SqlParameter("@UserID", SqlDbType.NVarChar);
            Param[2] = new SqlParameter("@ErrorTrace", SqlDbType.NVarChar);
            Param[3] = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar);
            Param[4] = new SqlParameter("@BowserDetails", SqlDbType.NVarChar);
            Param[5] = new SqlParameter("@ErrorLog", SqlDbType.NVarChar);
            Param[6] = new SqlParameter("@IPAddress", SqlDbType.NVarChar);
            Param[7] = new SqlParameter("@RemoteIPAddress", SqlDbType.NVarChar);

            Param[0].Value = System.DateTime.Now;
            Param[1].Value = UserID;
            Param[2].Value = ErrorTrace;
            Param[3].Value = ErrorMsg;
            Param[4].Value = BrowserDetails;
            Param[5].Value = ErrorLog;
            Param[6].Value = pSL_IPAddress;
            Param[7].Value = pSL_PublicIPAddress;
            SqlHelper.ExecuteProcedure(GlobalValues.strConnString, "sp_ErrorLog", Param);
        }

        public static void InsertCurrentLogInUser(string UserID, string ApplicationName, string SessionId)
        {
            GlobalValues.ExecuteNonQuery("insert into  LoginUsers(LoginTime,UserID,SessionID,ApplicationName,IPAddress,RemoteIPAddress)  values(getdate(),'" + UserID + "','" + SessionId + "','" + ApplicationName + "','" + pSL_IPAddress + "','" + pSL_PublicIPAddress + "')");
            InsertLoginHistory(UserID, ApplicationName, GlobalValues.gLHLogIN, "User Logged In");
        }
        public static void ForceLogOutForExpiredUsers()
        {
            int ExpiredAmount = Convert.ToInt32(ConfigurationManager.AppSettings["SessionForcetoExpired"]);
            string strsql = "Select * from LoginUsers where DATEDIFF(MI,LoginTime,getdate()) >= '" + ExpiredAmount + "'";
            DataSet ds = GlobalValues.ExecuteDataSet(strsql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                GlobalValues.ExecuteNonQuery("delete from LoginUsers where DATEDIFF(MI,LoginTime,getdate()) >= '" + ExpiredAmount + "' and UserId='" + ds.Tables[0].Rows[i]["UserId"].ToString() + "' and ApplicationName='" + ds.Tables[0].Rows[i]["ApplicationName"].ToString() + "' ");
                InsertLoginHistory(ds.Tables[0].Rows[i]["UserId"].ToString(), ds.Tables[0].Rows[i]["ApplicationName"].ToString(), GlobalValues.gLHLogOut, "User Logged Out Due to Session TimeOut.");
            }

        }

        public static void RemoveCurretLoginUsersDuringLogOut(string UserId, string ApplicationName, string Action, string Comments)
        {
            GlobalValues.ExecuteNonQuery("delete from LoginUsers where UserId = '" + UserId + "' and ApplicationName='" + ApplicationName + "'");
            InsertLoginHistory(UserId, ApplicationName, Action, Comments);

        }

        public static bool FindCurrentUserExistsInLoginUsersTableBySession(string UserId, string SessionID)
        {
            //InsertLoginHistory(UserId, SessionID, "Testing", "Log Find Current User Already Exixts by session Id");
            bool bval = true;
            var Count = GlobalValues.ExecuteScalar("Select  isnull(Count(*),0) from LoginUsers where UserId = '" + UserId + "' and SessionId='" + SessionID + "'");
            int countval = Convert.ToInt32(Count);
            if (countval <= 0)
                bval = false;

            return bval;
        }

        public static bool BoolResetPassword(string UserId)
        {
            bool bval = false;
            var Count = GlobalValues.ExecuteScalar("Select  isnull(Count(*),0) from HospitalUsers where UserId = '" + UserId + "' and ResetPassword=1");
            int countval = Convert.ToInt32(Count);
            if (countval > 0)
                bval = true;

            return bval;
        }



        public static string FindIPAddress(string UserId, string SessionID)
        {
            var ipadd = GlobalValues.ExecuteScalar("Select IPAddress from LoginUsers where UserId = '" + UserId + "' ");
            string ip = Convert.ToString(ipadd);
            return ip;
        }



        public static string RemoveCurretLoginUsersDuringMutipleUserLogin(string UserId, string SessionID, string ApplicationName, string Action, string Comments)
        {
            var IPAddress = GlobalValues.ExecuteScalar("Select IPAddress from LoginUsers where UserId = '" + UserId + "'");
            var Count = GlobalValues.ExecuteScalar("delete from LoginUsers where UserId = '" + UserId + "' and SessionId <> '" + SessionID + "'");
            InsertLoginHistory(UserId, ApplicationName, Action, Comments);
            return Convert.ToString(IPAddress);
        }
        public static void UpdateLastActTimeCurrentUser(string UserID, string SessionID, string ApplicationName)
        {
            if (UserID != null && UserID != string.Empty)
            {
                string strsql = "update LoginUsers set LoginTime=getdate()  where UserId='" + UserID + "' and ApplicationName='" + ApplicationName + "'  ";
                GlobalValues.ExecuteNonQuery(strsql);
            }
        }


        public static DataSet ValidateSingleSignOnUser(string UserID, string SessionID, string ApplicationName)
        {
            string strsql = "select UserID,IpAddress from LoginUsers where UserId='" + UserID + "' and ApplicationName='" + ApplicationName + "' ";
            DataSet ds = GlobalValues.ExecuteDataSet(strsql);
            return ds;
        }

        public static void InsertLoginHistory(string UserID, string ApplicationName, string Action, string Comments)
        {
            GlobalValues.ExecuteNonQueryAudit("insert into  Audit_LoginHistory(LogTime,UserID,Action,Comments,ApplicationName,IPAddress,RemoteIPAddress)  values(getdate(),'" + UserID + "','" + Action + "','" + Comments + "','" + ApplicationName + "','" + pSL_IPAddress + "','" + pSL_PublicIPAddress + "')");

        }

        public static void ForgotPassword(string password, string UserId)
        {
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@password", SqlDbType.DateTime);
                Param[1] = new SqlParameter("@userid", SqlDbType.NVarChar);
                Param[2] = new SqlParameter("@DatabaseName", SqlDbType.NVarChar);


                Param[0].Value = password;
                Param[1].Value = UserId;

                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                builder.ConnectionString = strAuditConnString;
                string server = builder.DataSource;
                string database = builder.InitialCatalog;

                Param[2].Value = database;

                SqlHelper.ExecuteProcedure(GlobalValues.strConnString, "sp_ForgotPassword", Param);

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
            }
        }

        public static bool BoolLiveUpdateProgress()
        {
            bool bval = false;
            var Count = GlobalValues.ExecuteScalar("Select  isnull(Count(*),0) from [LiveUpdateConnection]  WITH(NOLOCK) where LiveUpdate =1");
            int countval = Convert.ToInt32(Count);
            if (countval > 0)
                bval = true;

            return bval;
        }

        public static bool BoolActiveUser(string UserId)
        {
            bool bval = false;
            var Count = GlobalValues.ExecuteScalar("Select  isnull(Count(*),0) from [HospitalUsers] where Active =0 and  UserId='" + UserId + "' ");
            int countval = Convert.ToInt32(Count);
            if (countval > 0)
                bval = true;

            return bval;
        }

        public static string SystemVersionNumber()
        {
            string VersionNumber = string.Empty;
            var sqlSelect = "select max([Version Number]) from Cloud_LiveupdateInfo";
            var exeSql = SqlHelper.ExecuteReader(strAuditConnString, System.Data.CommandType.Text, sqlSelect);
            if (exeSql.HasRows)
            {
                while (exeSql.Read())
                {
                    if (string.IsNullOrEmpty(exeSql[0].ToString()))
                        VersionNumber = "";
                    else
                        VersionNumber = "v" + exeSql[0].ToString();

                }
            }

            return VersionNumber;
        }


    }

    [Serializable]
    public class FeatureSetPermission
    {
        enum FeatureSet
        {
            Registration = 1,
            DataAnalysis = 2,
            PatientAnalysisByallParameters = 3,
            PatientAnalysisByStudy = 4,
            StatusCheckList = 5,
            MySavedQueries = 6,
            Configuration = 7,
            DataGrouping = 8,
            ChangePassword = 9,
            Study = 10,
            ExportDatatoStudyPool = 11,
            ByDiagnosisForms = 12,
            ByStudyForms = 13,
            CustomForms = 14,
            Toxicity = 15,
            Recurrences = 16,
            LabTests = 17,
            Status = 18,
            CustomScreen = 19,
            SitesofMetastasis = 20,
            BottomNav = 21,
            Search = 22,
            DiseaseScreens = 23,
            ExporttoExcel = 24,
            Stats = 25,
            SaveQuery = 26,
            Frequency = 27
        };
        public bool isRegistration = false;
        public bool isAnalysis = false;
        public bool isAnalysisByStudy = false;
        public bool isAnalysisByAllparams = false;
        public bool isPresetForms = false;
        public bool isStudyForms = false;
        public bool isChangePassword = false;
        public bool isToxicity = false;
        public bool isRecurrence = false;
        public bool isInvestigations = false;
        public bool isStudy = false;
        public bool isSaveQuery = false;
        public bool isStatusCheckList = false;
        public bool isStatus = false;
        public bool isCustomScreen = false;
        public bool isSitesOfMetsis = false;
        public bool isBottomNav = false;
        public bool isDiseaseDetails = false;
        public bool isSearch = false;
        public bool isExporttoExcelStudy = false;
        public bool isExporttoExcelAllParams = false;
        public bool isExporttoExcelDrugDosage = false;
        public bool isStatsStudy = false;
        public bool isStatsAllParams = false;
        public bool isSaveQueryStudy = false;
        public bool isSaveQueryAllParams = false;
        public bool isFrequencyStudy = false;
        public bool isFrequencyAllParams = false;
        public string IsstrSessionId = string.Empty;


        public void GetUserPermissions(string UserLogin, string SessionID)
        {
            isAnalysisByAllparams = false;
            isAnalysisByStudy = false;
            isStudyForms = false;
            isRegistration = false;
            isAnalysis = false;
            isPresetForms = false;
            isChangePassword = false;
            isToxicity = false;
            isRecurrence = false;
            isInvestigations = false;
            isStudy = false;
            isSaveQuery = false;
            isStatusCheckList = false;
            isStatus = false;
            isCustomScreen = false;
            isSitesOfMetsis = false;
            isBottomNav = false;
            isDiseaseDetails = false;
            isStudyForms = false;
            isSearch = false;
            isExporttoExcelStudy = false;
            isExporttoExcelAllParams = false;
            isExporttoExcelDrugDosage = false;
            isStatsStudy = false;
            isStatsAllParams = false;
            isSaveQueryStudy = false;
            isSaveQueryAllParams = false;
            isFrequencyStudy = false;
            isFrequencyAllParams = false;
            IsstrSessionId = SessionID.ToString();
            string sqlStr = "select FU.*, fs.FeatureSetName,fs.ParentFeatureIDCSV  as ParentID from FeatureSetUsers FU inner join FeatureSet Fs on fu.FeatureSetName=fs.FeatureSetName inner join HospitalUsers HU on fu.UserID=hu.UserID where FU.Enabled=1 and HU.UserID  ='" + UserLogin + "'";
            DataSet dsUserPerms = GlobalValues.ExecuteDataSet(sqlStr);
            if (dsUserPerms.Tables.Count > 0)
            {
                foreach (DataRow dr in dsUserPerms.Tables[0].Rows)
                {
                    if (dr["FeatureSetName"].ToString() == FeatureSet.Registration.ToString())
                    {
                        isRegistration = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.DataAnalysis.ToString())
                    {
                        isAnalysis = true;
                    }
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.PatientAnalysisByStudy.ToString())
                    {
                        isAnalysisByStudy = true;
                    }
                    //Export to Excel
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.ExporttoExcel.ToString() && (dr["ParentID"].ToString().Trim() == "2.1"))//2.1 All params
                    {
                        isExporttoExcelAllParams = true;
                    }
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.ExporttoExcel.ToString() && (dr["ParentID"].ToString().Trim() == "2.2"))//2.2 Study
                    {
                        isExporttoExcelStudy = true;
                    }
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.ExporttoExcel.ToString() && (dr["ParentID"].ToString().Trim() == "2.4"))
                    {
                        isExporttoExcelDrugDosage = true;
                    }
                    //Stats
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.Stats.ToString() && (dr["ParentID"].ToString().Trim() == "2.1"))
                    {
                        isStatsAllParams = true; isStatsStudy = true;
                    }
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.Stats.ToString() && (dr["ParentID"].ToString().Trim() == "2.2"))
                    {
                        isStatsStudy = true;
                    }

                        //Save Query

                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.SaveQuery.ToString() && (dr["ParentID"].ToString().Trim() == "2.1"))
                    {
                        isSaveQueryAllParams = true; isStatsStudy = true;
                    }
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.SaveQuery.ToString() && (dr["ParentID"].ToString().Trim() == "2.2"))
                    {
                        isSaveQueryStudy = true;
                    }
                    //Frequency
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.Frequency.ToString() && (dr["ParentID"].ToString().Trim() == "2.1"))
                    {
                        isFrequencyAllParams = true; isStatsStudy = true;
                    }
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.Frequency.ToString() && (dr["ParentID"].ToString().Trim() == "2.2"))
                    {
                        isFrequencyStudy = true;
                    }

                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.PatientAnalysisByallParameters.ToString())
                    {
                        isAnalysisByAllparams = true;
                    }
                    else if (((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("(", "")).Replace(")", "") == FeatureSet.ByDiagnosisForms.ToString())
                    {
                        isPresetForms = true;
                        isBottomNav = true;
                    }
                    else if (((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("(", "")).Replace(")", "") == FeatureSet.ByStudyForms.ToString())
                    {
                        isStudyForms = true;
                    }///
                    else if (((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("(", "")).Replace(")", "") == FeatureSet.CustomForms.ToString())
                    {
                        isCustomScreen = true;
                    }///
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.ChangePassword.ToString())
                    {
                        isChangePassword = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.Toxicity.ToString())
                    {
                        isToxicity = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "").Trim() == FeatureSet.Recurrences.ToString().Trim())
                    {
                        isRecurrence = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.LabTests.ToString())
                    {
                        isInvestigations = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.Study.ToString())
                    {
                        isStudy = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.MySavedQueries.ToString())
                    {
                        isSaveQuery = true;
                    }
                    else if ((dr["FeatureSetName"].ToString().Replace(" ", "")).Replace("-", "") == FeatureSet.StatusCheckList.ToString())
                    {
                        isStatusCheckList = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.Status.ToString())
                    {
                        isStatus = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.CustomForms.ToString())
                    {
                        isCustomScreen = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.SitesofMetastasis.ToString())
                    {
                        isSitesOfMetsis = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.BottomNav.ToString())
                    {
                        isBottomNav = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.DiseaseScreens.ToString())
                    {
                        isDiseaseDetails = true;
                    }
                    else if (dr["FeatureSetName"].ToString().Replace(" ", "") == FeatureSet.Search.ToString())
                    {
                        isSearch = true;
                    }
                }
            }
        }

    }
}