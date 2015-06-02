using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ePxCollectDataAccess;
using System.Data;
using DataImportLib;
using Quartz;
using System.Net;
using System.Net.Mail;
using Quartz.Impl;



namespace ePxCollectWeb
{
    public partial class DataImport : IJob
    {
        string strConn = GlobalValues.strConnString;
        string strConnString = GlobalValues.strConnString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }



        public void Execute(IJobExecutionContext context)
        {
            using (var message = new MailMessage("premiya@codebase.bz", "premiya@codebase.bz"))
            {
                message.Subject = "Test";
                message.Body = "Test at " + DateTime.Now;
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("premiya@codebase.bz", "shivani08")
                })
                {
                    client.Send(message);
                }
            }
        }


        protected void btnGetConnection_Click(object sender, EventArgs e)
        {
            string strconnectionString = txtConnString.Text.ToString();
            bool bval = GlobalValues.TestConnectionStringSQLSERVER(strconnectionString);
            string Query = txtQuery.Text;

        }

        private void ImportPatientData()
        {

            string QueryString = txtQuery.Text.Trim();
            DataSet ds = SqlHelper.ExecuteDataset(txtConnString.Text.ToString(), CommandType.Text, QueryString);
            if (ds.Tables.Count > 0)
            {

                try
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string strSQL = "Select isnull(max(convert(int,(SUBSTRING([patientId],3,5)))),0)+1 as PID, isnull(Max(sortID),0)+1 SortId, (Select max(InstanceID) from Instance) Instance from PatientDetails_0 ";
                        //dsPatientID = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSQL);
                        DataSet dsPatientID = GlobalValues.ExecuteDataSet(strSQL);
                        var varPatientId = dsPatientID.Tables[0].Rows[0][2].ToString() + "-" + dsPatientID.Tables[0].Rows[0][0].ToString();
                        //string varSortId = Convert.ToString(dsPatientID.Tables[0].Rows[0][1]);
                        string varSortId = Convert.ToString(dsPatientID.Tables[0].Rows[0][0]);

                        string HopitalFileNumber = ds.Tables[0].Rows[i]["HospitalFileNo"].ToString();
                        //if (CheckFileNumberExists(HopitalFileNumber))
                        {
                            strSQL = "Insert into PatientDetails_0 (PatientName, PatientID, HospitalFileNo, dateOfRegistration, Locked, SortId) values ('" +
                            ds.Tables[0].Rows[i]["PatientName"].ToString() + "','" + varPatientId.ToString() + "','" + ds.Tables[0].Rows[i]["HospitalFileNo"].ToString() + "','" +
                            ds.Tables[0].Rows[i]["dateOfRegistration"].ToString() + "',1," + varSortId + ")";

                            ePxCollectDataAccess.SqlHelper sqlCon = new SqlHelper();

                            SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
                            for (int intCount = 1; intCount <= 11; intCount++)
                            {
                                if (intCount == 1)
                                {
                                    strSQL = "insert into  PatientDetails_" + intCount.ToString() + " (Patient, SiteOfPrimary) values ('" + varPatientId.ToString() + "','" + "" + "')";
                                }
                                else
                                {
                                    strSQL = "insert into  PatientDetails_" + intCount.ToString() + " (Patient) values ('" + varPatientId.ToString() + "')";
                                }
                                SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
                            }
                        }
                    }
                    // ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Patient details registered successfully.');", true);

                }
                catch
                { }
            }
        }

        public void ImportPatientData(string ConnectionString, string strConnStringTxt, string strQueryTxt)
        {
            //Source DataSet
            DataSet ds = SqlHelper.ExecuteDataset(strConnStringTxt, CommandType.Text, strQueryTxt);

            string strQuery = "select [Table Name] as TableName,[Patient Identity Field] as PatientIdentity,[Field Name] as FieldName,* from PDFields where [Table Name] like '%PatientDetails_%'  order by [Table Name] asc";
            DataSet dsPDFields = SqlHelper.ExecuteDataset(strConnString, CommandType.Text, strQuery);
            DataSet distinctValues = SqlHelper.ExecuteDataset(strConnString, CommandType.Text, "select distinct [Table Name] as TableName from PDFields where [Table Name] like '%PatientDetails_%'  order by [Table Name] asc");

            OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();

            if (ds.Tables.Count > 0)
            {
                string queryInsert = "insert into ";
                string queryvalues = "values( ";

                try
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var Length = GlobalValues.ExecuteScalar("Select len(InstanceID) from Instance");
                        int intlenth = Convert.ToInt32(Length) + 2;
                        string strSQL = "Select isnull(max(convert(int,(SUBSTRING([patientId],3,5)))),0)+1 as PID, isnull(Max(sortID),0)+1 SortId, (Select max(InstanceID) from Instance) Instance from PatientDetails_0 ";
                        //dsPatientID = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSQL);
                        DataSet dsPatientID = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strSQL);
                        var varPatientId = dsPatientID.Tables[0].Rows[0][2].ToString() + "-" + dsPatientID.Tables[0].Rows[0][0].ToString();
                        //string varSortId = Convert.ToString(dsPatientID.Tables[0].Rows[0][1]);
                        string varSortId = Convert.ToString(dsPatientID.Tables[0].Rows[0][0]);

                        string HopitalFileNumber = ds.Tables[0].Rows[i]["HospitalFileNo"].ToString();
                        string PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        string DateofReg = ds.Tables[0].Rows[i]["DateofRegistration"].ToString();
                        if (CheckFileNumberExists(HopitalFileNumber, ConnectionString))
                        {
                            if (CheckForMandatory(PatientName, DateofReg))
                            {
                                //Destination
                                for (int j = 0; j < distinctValues.Tables[0].Rows.Count; j++)
                                {
                                    queryInsert = queryInsert + " " + distinctValues.Tables[0].Rows[j][0].ToString() + " ( ";
                                    if (distinctValues.Tables[0].Rows[j][0].ToString() == "PatientDetails_0")
                                    {
                                        queryInsert = queryInsert + ("PatientId,Locked,SortId,");
                                        queryvalues = queryvalues + "'" + varPatientId + "','1','" + varSortId + "',";
                                    }
                                    else
                                    {
                                        queryInsert = queryInsert + ("Patient,");
                                        queryvalues = queryvalues + "'" + varPatientId + "',";
                                    }

                                    DataRow[] foundRow = dsPDFields.Tables[0].Select("TableName='" + distinctValues.Tables[0].Rows[j]["TableName"].ToString().Trim() + "'");
                                    for (int PDFields = 0; PDFields < foundRow.Length; PDFields++)
                                    {
                                        string ColumnName = foundRow[PDFields]["Field Name"].ToString();
                                        bool bval = ValidateColunmninSource(ColumnName, ds.Tables[0], i);
                                        if (bval)
                                        {
                                            if (ValidateIdentityColumn(dsPDFields.Tables[0], ColumnName))
                                            {
                                                queryInsert = queryInsert + "[" + ColumnName + "],";
                                                queryvalues = queryvalues + "'" + objEnc.Encrypt(ds.Tables[0].Rows[i][ColumnName].ToString().Replace("'", "")) + "',";
                                            }
                                            else
                                            {
                                                queryInsert = queryInsert + "[" + ColumnName + "],";
                                                queryvalues = queryvalues + "'" + ds.Tables[0].Rows[i][ColumnName].ToString() + "',";
                                            }
                                        }

                                    }
                                    queryInsert = queryInsert.TrimEnd(',') + ")";
                                    queryvalues = queryvalues.TrimEnd(',') + ")";
                                    SqlHelper.ExecuteNonQuery(ConnectionString, System.Data.CommandType.Text, queryInsert + queryvalues);
                                    queryInsert = "insert into ";
                                    queryvalues = "values( ";
                                }
                            }


                        }



                    }


                }
                catch
                { }

            }
        }

        private bool ValidateColunmninSource(string ColumnName, DataTable dt, int RowCount)
        {
            bool bval = false;
            if (dt.Columns.Contains(ColumnName))
            {
                if (string.IsNullOrEmpty(dt.Rows[RowCount][ColumnName].ToString()) == false)
                    bval = true;
            }
            return bval;
        }

        private bool ValidateIdentityColumn(DataTable dt, string ColumnName)
        {
            bool bval = false;

            DataRow[] dr = dt.Select("Encrypt=1 and FieldName='" + ColumnName + "' ");
            if (dr.Length > 0)
                bval = true;
            return bval;
        }

        private bool CheckFileNumberExists(string FileNumber, string ConnectionString)
        {
            bool bval = false;
            var CountVal = SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "select Count(*) from PatientDetails_0 where HospitalFileNo='" + FileNumber + "' ");
            if (Convert.ToInt32(CountVal) <= 0)
                bval = true;
            return bval;
        }

        private bool CheckForMandatory(string PatientName, string DateOfReg)
        {
            bool bval = false;
            if (string.IsNullOrEmpty(PatientName) == false)
                bval = true;

            if (string.IsNullOrEmpty(DateOfReg) == false)
                bval = true;

            return bval;
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {


            //Need to Check Mandatory Fields.//Name File Number Date Of Regestration

            DataImportLib.DataImportLib DataImport = new DataImportLib.DataImportLib();
            DataImport.strConnString = "Data Source=CODEBASE-12;Initial Catalog=Oncoserver;User ID=sa;Password=admin123";
            DataImport.StrQuery = "select top 5 PatientName,HospitalFileNo,DateOfRegistration,DateOfBirth,[Age at Diagnosis],DateOfDiagnosis,City_Town,Country,State from PatientDetails_0 inner join PatientDetails_1 on PatientDetails_0.PatientID=PatientDetails_1.Patient ";
            DataImport.ImportPatientData(strConn, DataImport.strConnString, DataImport.StrQuery);
            //ImportPatientData(strConn, DataImport.strConnString, DataImport.StrQuery);


        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }

    }


    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<DataImport>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInMinutes(1)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                  )
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}