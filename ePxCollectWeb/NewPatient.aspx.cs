using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data.SqlClient;
using System.Data;

namespace ePxCollectWeb
{
    public partial class NewPatient : System.Web.UI.Page
    {
        string strConn = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }

            string strSQL = string.Empty;
            DataSet dsDisease = new DataSet();
            strConn = GlobalValues.strConnString;//System.Configuration.ConfigurationManager.ConnectionStrings["OncoCollectEnterprise"].ConnectionString;
            if (!IsPostBack)
            {
                strSQL = "Select Distinct(Diagnosis) From Diagnosis";
                //ePxCollectDataAccess.SqlHelper sqlOBJ ;
                dsDisease = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSQL);
                cboDiagnosis.DataSource = dsDisease.Tables[0].DefaultView.ToTable();
                cboDiagnosis.DataMember = "Diagnosis";
                cboDiagnosis.DataValueField = "Diagnosis";
                cboDiagnosis.DataBind();
                dtRegDate.CalendarDate = DateTime.Today;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string ValidationResult = string.Empty;
            try
            {
                string varPatientId = string.Empty;
                if (cboTitle.Text.Length <= 0)
                {
                    ValidationResult += " - Title <br>";
                }
                if (cboDiagnosis.Text.Length <= 0)
                {
                    ValidationResult += " - Diagnosis <br>";
                }
                if (txtFileNo.Text.Length <= 0)
                {
                    ValidationResult += " - File Number <br>";
                }
                if ((TxtFirstName.Text + TxtFirstName.Text).Length <= 0)
                {
                    ValidationResult += " - Patient Name <br>";
                }
                if (!dtRegDate.IsValidDate)
                {
                    ValidationResult += " - Registration Date <br>";
                }

                if (ValidationResult.Length > 0)
                {
                    lblError.Text = "Please provide values for the following : <br>" + ValidationResult.ToString();
                }
                else
                {
                    String strSQL = string.Empty;
                    string varPatientName = TxtFirstName.Text.ToString();
                    if (txtMidName.Text.ToString().Length > 0) { varPatientName += txtMidName.Text.ToString() + ", "; }
                    varPatientName += ", " + txtLastName.Text.ToString() + "(" + cboTitle.Text.ToString() + ")";
                    ClientScript.RegisterStartupScript(GetType(), "hwa", "alert('Hello World');", true);

                    DataSet dsPatientID = new DataSet();
                    strSQL = "Select count(*)+1 as PID, Max(sortID)+1 SortId from PatientDetails_0";
                    dsPatientID = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSQL);
                    varPatientId = dsPatientID.Tables[0].Rows[0][0].ToString();
                    string varSortId = Convert.ToString(dsPatientID.Tables[0].Rows[0][1]);
                    strSQL = "Insert into  PatientDetails_0  (PatientName, PatientID, HospitalFileNo, dateOfRegistration, Locked, SortId) values ('" + varPatientName.ToString() + "','" + varPatientId.ToString() + "','" + ((txtFileNo.Text == "") ? "ONCO" + varPatientId.ToString() : (txtFileNo.Text)) + "', '" + dtRegDate.CalendarDateString.ToString() + "',1," +varSortId +")";
                    //ePxCollectDataAccess.SqlHelper sqlCon= new SqlHelper();
                    SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
                    for (int intCount = 1; intCount <= 11; intCount++)
                    {
                        if (intCount == 1)
                        {
                            strSQL = "insert into  PatientDetails_" + intCount.ToString() + " (Patient, Diagnosis) values ('" + varPatientId.ToString() + "','" + cboDiagnosis.SelectedItem.ToString() + "')";
                        }
                        else
                        {
                            strSQL = "insert into  PatientDetails_" + intCount.ToString() + " (Patient) values ('" + varPatientId.ToString() + "')";
                        }
                        SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
                    }



                }
            }
            catch (Exception ex) { }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

       
    }
}