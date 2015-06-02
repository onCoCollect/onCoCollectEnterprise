using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;
using Button = System.Web.UI.WebControls.Button;

namespace ePxCollectWeb
{
    public partial class Register : System.Web.UI.Page
    {
        private FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
        private string strConn = string.Empty;
        static string createHospitalCode = string.Empty;
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
            // int fileLength = System.Configuration.ConfigurationManager.AppSettings["fileNumber"].Length;
            string strSQL = string.Empty;
            DataSet dsDisease = new DataSet();
            if (ObjfeatureSet.isRegistration == false)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow",
                    "alert('Sorry, you don't have permission to this functionality.');", true);
                Response.Redirect("ProjectForm.aspx");
            }
            strConn = GlobalValues.strConnString;
            //System.Configuration.ConfigurationManager.ConnectionStrings["OncoCollectEnterprise"].ConnectionString;
            /* Srinivas Commented
            //dtRegDate.Width = 150;
            //dtRegDate.PaneWidth = 150; */
            if (!IsPostBack)
            {
                strSQL = "Select Distinct(Diagnosis) From Diagnosis";
                lblError.Text = "";
                //ePxCollectDataAccess.SqlHelper sqlOBJ ;
                dsDisease = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSQL);

                DataSet ds =
                    GlobalValues.ExecuteDataSet("select FieldValues from PDFields where [Field Name]='HospitalCode'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ddlHospitalCode.Items.Add(new ListItem("", ""));
                    string ColVal = dr[0].ToString();
                    string[] Cols = ColVal.Split(',');
                    for (int I = 0; I < Cols.Length; I++)
                    {
                        if (Cols[I].ToString().Trim() != string.Empty)
                            ddlHospitalCode.Items.Add(new ListItem(Cols[I].ToString().Trim(), Cols[I].ToString().Trim()));
                    }

                    var HospitalCode =
                        GlobalValues.ExecuteScalar("Select isnull(HospitalCode,'') from  HospitalUsers where UserID='" +
                                                   Session["Login"].ToString() + "'");
                    createHospitalCode = HospitalCode.ToString();
                    if (ddlHospitalCode.Items.FindByValue(HospitalCode.ToString()) != null)
                    {
                        ddlHospitalCode.SelectedValue = HospitalCode.ToString();
                    }


                }


                /*currently suspending Diagnosis Combo box as per customer requirement @  Nov 20,2014 
                cboDiagnosis.DataSource = dsDisease.Tables[0].DefaultView.ToTable();
                cboDiagnosis.DataMember = "Diagnosis";
                cboDiagnosis.DataValueField = "Diagnosis";
                cboDiagnosis.DataBind();
                 * */
                //dtRegDate.CalendarDate = DateTime.Today;
                /* srinivas Commented 
                 dtRegDate.SelectedDate=DateTime.Today;*/

                //dtRegDate.Text = calRegDt.SelectedDate.ToString();
                string strReadOnly = Convert.ToString(Request.QueryString["RO"]);
                if (strReadOnly == "True")
                {
                    pnlMain.Enabled = false;
                }
            }
            AvoidMultipleSubmit(btnSave, btnSave.Text);

        }

        public void AvoidMultipleSubmit(Button button, string text)
        {
            PostBackOptions optionsSubmit = new PostBackOptions(button);
            button.OnClientClick = "disableButtonOnClick(this, '" + text + "', 'button'); ";
            button.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmit);

        }

        //Code modified on May 27,2015-Subhashini 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string ValidationResult = string.Empty;
            try
            {
                bool bval1 = false;
                bool bval2 = false;
                bool bval3 = false;
                bool bval4 = false;
                bool bval5 = false;
                bool bval6 = false;
                bool bval7 = false;
                bool bval8 = false;

                string varPatientId = string.Empty;
                if (cboTitle.Text.Length <= 1)
                {
                    ValidationResult += " - Please Select Title. <br>";
                    bval1 = true;
                }
                if (ddlHospitalCode.Text.Length <= 1)
                {
                    ValidationResult += " - Please Select Hospital Code. <br>";
                    bval1 = true;
                }
                if ((TxtFirstName.Text.Trim()).Length <= 0)
                {
                    ValidationResult += " - Please Enter the Patient First Name. <br>";
                    bval2 = true;
                }
                if ((txtLastName.Text.Trim()).Length <= 0)
                {
                    ValidationResult += " - Please Enter the Patient Last Name. <br>";
                    bval3 = true;
                }
                if (txtFileNo.Text.Length <= 0)
                {
                    ValidationResult += " - Please Enter the File Number. <br>";
                    bval4 = true;
                }
                if ((txtRegDate.Text.Trim()) == "")
                {
                    ValidationResult += " - Please Enter the Registration Date. <br>";
                    bval5 = true;
                }


                if (ValidationResult.Length > 0)
                {
                    GlobalValues.FailureColor = lblError.ForeColor;
                    if (bval1 && bval2 && bval3 && bval4 && bval5)
                    {
                        lblError.Text = "Fields marked with asterisk (*) are required.";
                    }

                    else
                    {
                        lblError.Text = "Fields marked with asterisk (*) are required.";
                    }
                    return;


                }

                if (btnSave.Text == "Update")//Code modified on May 27,2015-Subhashini
                {
                    string Salutation = cboTitle.Text.ToString();
                    string FirstName = TxtFirstName.Text;
                    string PatientID = Session["PatientID"].ToString();
                    string HospitalFileNo = txtFileNo.Text.ToString();
                    string HospitalCode = ddlHospitalCode.Text.ToString();
                    string dateOfRegistration = txtRegDate.Text;
                    string LastName = txtLastName.Text;
                    string MiddleName = txtMidName.Text;
                    string PatientName = "";
                    if (MiddleName != "")
                    {
                        PatientName = FirstName.ToUpper() + " " + MiddleName.ToUpper() + " " + LastName.ToUpper() + " (" +
                                      Salutation + ")";
                    }
                    else
                    {
                        PatientName = FirstName.ToUpper() + " " + LastName + " (" + Salutation + ")";
                    }
                    string query = "update PatientDetails_0  set PatientName='" + PatientName + "', HospitalFileNo='" +
                                   HospitalFileNo + "',HospitalCode='" + HospitalCode + "', dateOfRegistration='" +
                                   dateOfRegistration +
                                   "'," + "FirstName='" + FirstName + "',MiddleName='" + MiddleName + "',LastName='" +
                                   LastName + "',Salutation='" +
                                   cboTitle.Text.ToString() + "' from PatientDetails_0 where PatientID='" + PatientID + "'";
                    int cnt = SqlHelper.ExecuteNonQuery(GlobalValues.strConnString, CommandType.Text, query);
                    if (cnt > 0)
                    {
                        lblError.Text = "Patient details updated successfully.";
                        lblError.ForeColor = GlobalValues.SucessColor;
                    }
                    GlobalValues.WriteAuditRecord(Convert.ToString(Session["Login"]),
                        Convert.ToString(Session["PatientID"]), "Updated Record", GlobalValues.strEnterpriseDataBaseName);
                }

                else if (btnSave.Text == "Save")//Code modified on May 27,2015-Subhashini
                {
                    String strSQL = string.Empty;
                    string varPatientName = TxtFirstName.Text.ToString().Trim().ToUpper();
                    if (txtMidName.Text.ToString().Length > 0)
                    {
                        varPatientName += " " + txtMidName.Text.ToString().Trim().ToUpper() + " ";
                        varPatientName += txtLastName.Text.ToString().Trim().ToUpper() + " (" + cboTitle.Text.ToString() +
                                          ")";
                    }
                    else
                    {
                        varPatientName += " " + txtLastName.Text.ToString().Trim().ToUpper() + " (" +
                                          cboTitle.Text.ToString() + ")";
                    }

                    DataSet dsPatientID = new DataSet();


                    var Length = GlobalValues.ExecuteScalar("Select len(InstanceID) from Instance");
                    int intlenth = Convert.ToInt32(Length) + 2;
                    strSQL = "Select isnull(max(convert(int,(SUBSTRING([patientId]," + intlenth +
                             ",10)))),0)+1 as PID, isnull(Max(sortID),0)+1 SortId, (Select max(InstanceID) from Instance) Instance from PatientDetails_0 ";

                    dsPatientID = GlobalValues.ExecuteDataSet(strSQL);
                    varPatientId = dsPatientID.Tables[0].Rows[0][2].ToString().Trim() + "-" +
                                   dsPatientID.Tables[0].Rows[0][0].ToString().Trim();

                    string varSortId = Convert.ToString(dsPatientID.Tables[0].Rows[0][0]);

                    string strRegDate = txtRegDate.Text;
                    string checkName = "Select count(*) from PatientDetails_0 where PatientName ='" +
                                       varPatientName.ToString() + "'";
                    int nameCount = (int)GlobalValues.ExecuteScalar(checkName);
                    if (nameCount == null)
                    {
                        nameCount = 0;
                    }
                    if (nameCount > 0)
                    {
                        lblError.Text = "The patient name you entered already exists.";

                        txtRegDate.Text = strRegDate;
                    }

                    string sqlstrCheck = "Select count(*) from PatientDetails_0 where HospitalFileNo ='" +
                                         txtFileNo.Text.ToString() + "'";
                    int RecCount = (int)GlobalValues.ExecuteScalar(sqlstrCheck);
                    if (RecCount == null)
                    {
                        RecCount = 0;
                    }
                    if (RecCount > 0)
                    {
                        txtRegDate.Text = strRegDate;

                        lblError.ForeColor = GlobalValues.FailureColor;
                        lblError.Text = "File Number already exists.";


                    }
                    if (RecCount <= 0 && nameCount <= 0)
                    {

                        strSQL =
                            "Insert into PatientDetails_0 (PatientName, PatientID, HospitalFileNo,HospitalCode, dateOfRegistration, Locked, SortId,FirstName,MiddleName,LastName,Salutation,CreatedBy,CreatedDate) values ('" +

                            varPatientName.ToString().Trim() + "','" + varPatientId.ToString().Trim() + "','" +
                            ((txtFileNo.Text == "")
                                ? "ONCO" + varPatientId.ToString().Trim()
                                : (txtFileNo.Text.ToString().Trim())) + "', '" +
                            ddlHospitalCode.SelectedItem.Text.ToString().Trim() + "'," +

                            (string.IsNullOrEmpty(txtRegDate.Text)
                                ? "NULL"
                                : ("'" + txtRegDate.Text.ToString().Trim() + "'")) + ",1," + varSortId + ",'" +
                            TxtFirstName.Text.ToString().Trim().ToUpper() + "','" +
                            txtMidName.Text.ToString().Trim().ToUpper() + "','" +
                            txtLastName.Text.ToString().Trim().ToUpper() + "','" +
                            cboTitle.SelectedValue.ToString().Trim() + "','" + Session["Login"].ToString().Trim() +
                            "',getdate())";

                        ePxCollectDataAccess.SqlHelper sqlCon = new SqlHelper();

                        SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
                        for (int intCount = 1; intCount <= 11; intCount++)
                        {
                            if (intCount == 1)
                            {
                                strSQL = "insert into  PatientDetails_" + intCount.ToString() +
                                         " (Patient, SiteOfPrimary) values ('" + varPatientId.ToString() + "','-')";
                            }
                            else
                            {
                                strSQL = "insert into  PatientDetails_" + intCount.ToString() + " (Patient) values ('" +
                                         varPatientId.ToString() + "')";
                            }
                            SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
                        }


                        lblError.Text = "Patient details registered successfully.";
                        lblError.ForeColor = GlobalValues.SucessColor;

                        GlobalValues.UnlockUser(Convert.ToString(Session["Login"]),
                            Convert.ToString(Session["PatientID"]));
                        Session["PatientID"] = varPatientId;
                        GlobalValues.lockUser(Convert.ToString(Session["Login"]), Convert.ToString(Session["PatientID"]));

                        string strSql =
                            "SELECT  PatientDetails_0.PatientID as 'Patient ID',PatientDetails_0.PatientName as 'Patient Name',PatientDetails_0.HospitalFileNo as 'File Number',PatientDetails_1.SiteOfPrimary as 'Site Of Primary',PatientDetails_0.City_Town as 'City/Town',PatientDetails_0.State as 'State', datename(mm, PatientDetails_0.DateOfRegistration)+ ' ' + cast(datepart(dd, PatientDetails_0.DateOfRegistration) as char(2)) + ',' + cast(datepart(yyyy,PatientDetails_0.DateOfRegistration) as char(4))   as 'Date Of Registration' ,PatientDetails_0.PhoneNumber as 'Phone Number'From PatientDetails_0 LEFT JOIN PatientDetails_1 ON PatientDetails_0.PatientID = PatientDetails_1.Patient where PatientDetails_0.PatientId='" +
                            varPatientId + "'  ";

                        DataSet dsSearch = ePxCollectDataAccess.SqlHelper.ExecuteDataset(strConn,
                            System.Data.CommandType.Text, strSql);
                        if (dsSearch.Tables.Count > 0)
                            if (dsSearch.Tables[0].Rows.Count > 0)
                                Session["PatienDetails"] =
                                    dsSearch.Tables[0].Rows[0]["Patient Name"].ToString().Trim() + "/ ID:" +
                                    dsSearch.Tables[0].Rows[0]["Patient ID"].ToString().Trim() + "/ File No:" +
                                    txtFileNo.Text.Trim() + "/ City:" +
                                    dsSearch.Tables[0].Rows[0]["City/Town"].ToString().Trim() + "/ " +
                                    dsSearch.Tables[0].Rows[0]["Site Of Primary"].ToString().Trim();


                        GlobalValues.WriteAuditRecord(Convert.ToString(Session["Login"]),
                            Convert.ToString(Session["PatientID"]), "New Record", GlobalValues.strEnterpriseDataBaseName);
                        GlobalValues.gDisease = "Diagnosis Suspended";

                        Response.Redirect("ProjectForm.aspx", false);
                        // clear();
                    }
                }
            }


            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void lnkClearDate_Click(object sender, EventArgs e)
        {
            txtRegDate.Text = "";
            //commented srinivasdtRegDate.SelectedDate = Convert.ToDateTime(null); 
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //Response.Redirect("~/Register.aspx");
            clear();
        }

        protected void btnClose_Click1(object sender, EventArgs e)
        {
            createHospitalCode=string.Empty;
            Response.Redirect("~/ProjectForm.aspx");
        }

        public void clear()
        {
            lblError.Text = string.Empty;
            TxtFirstName.Text = "";
            txtLastName.Text = "";
            txtMidName.Text = "";
            txtRegDate.Text = "";
            //txtFileNo.Text = "";
            cboTitle.SelectedIndex = 0;
            if (rdoCreate.Checked)
            {
                if (ddlHospitalCode.Items.FindByValue(createHospitalCode) != null)
                {
                    ddlHospitalCode.SelectedValue = createHospitalCode;
                }
            }
            else
            {
                ddlHospitalCode.SelectedIndex = 0;
            }
            cboTitle.Text = "";

        }

        //Code added on May 27,2015-Subhashini
        protected void rdoCreate_OnCheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdnButton = (RadioButton)sender;
            if (rdnButton.ID.ToString() == "rdoCreate")
            {
                btnSave.Text = "Save";
                cboTitle.SelectedIndex = 0;
                TxtFirstName.Text = "";
                txtMidName.Text = "";
                txtLastName.Text = "";
                ddlHospitalCode.Text = "";
                txtFileNo.Text = "";
                txtRegDate.Text = "";
                lblError.Text = "";
                txtFileNo.Enabled = true;
                ddlHospitalCode.Enabled = true;
                if (ddlHospitalCode.Items.FindByValue(createHospitalCode) != null)
                {
                    ddlHospitalCode.SelectedValue = createHospitalCode;
                }
            }
            else if (rdnButton.ID.ToString() == "rdoEdit")
            {
                btnSave.Text = "Update";
                string PatientID = "";
                txtFileNo.Enabled = false;
                ddlHospitalCode.Enabled = true;
                if (Session["PatientID"] != null && Session["PatientID"].ToString().Trim() != "")
                {
                    PatientID = Session["PatientID"].ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Please pick a patient.');", true);
                    Session["Message"] = "Please pick a Patient.";
                    Response.Redirect("SearchPatient.aspx");
                }
                var length = GlobalValues.ExecuteScalar("Select len(InstanceID) from Instance");
                int intlenth = Convert.ToInt32(length) + 2;
                string query = "select PatientName, PatientID, HospitalFileNo,HospitalCode, dateOfRegistration," +
                "FirstName,MiddleName,LastName,Salutation from PatientDetails_0 where PatientID='" + PatientID + "'";
                DataSet dsPatient = SqlHelper.ExecuteDataset(GlobalValues.strConnString, CommandType.Text, query);
                if (dsPatient != null && dsPatient.Tables.Count > 0)
                {
                    if (dsPatient.Tables[0].Rows.Count > 0)
                    {
                        TxtFirstName.Text = string.IsNullOrEmpty(dsPatient.Tables[0].Rows[0]["FirstName"].ToString()) ? "" : dsPatient.Tables[0].Rows[0]["FirstName"].ToString();
                        txtMidName.Text = string.IsNullOrEmpty(dsPatient.Tables[0].Rows[0]["MiddleName"].ToString()) ? "" : dsPatient.Tables[0].Rows[0]["MiddleName"].ToString();
                        txtLastName.Text = string.IsNullOrEmpty(dsPatient.Tables[0].Rows[0]["LastName"].ToString()) ? "" : dsPatient.Tables[0].Rows[0]["LastName"].ToString();
                        cboTitle.Text = string.IsNullOrEmpty(dsPatient.Tables[0].Rows[0]["Salutation"].ToString()) ? "" : dsPatient.Tables[0].Rows[0]["Salutation"].ToString();
                        string hospitalCode = string.IsNullOrEmpty(dsPatient.Tables[0].Rows[0]["HospitalCode"].ToString()) ? dsPatient.Tables[0].Rows[0]["HospitalFileNo"].ToString().Split('.')[0].ToString() : dsPatient.Tables[0].Rows[0]["HospitalCode"].ToString();
                        //System.Text.RegularExpressions.Regex newReg =new System.Text.RegularExpressions.Regex("[A-Z]+", RegexOptions.None);
                        string getHospitalCode = string.IsNullOrEmpty(hospitalCode) ? "" : GetHospitalCode(hospitalCode);
                        //if (newReg.IsMatch(hospitalCode))
                        //{
                        //     getHospitalCode = newReg.Match(hospitalCode).Value.ToString();
                        //}
                        ddlHospitalCode.Text = string.IsNullOrEmpty(getHospitalCode) ? "" : getHospitalCode.ToString();
                        txtFileNo.Text = string.IsNullOrEmpty(dsPatient.Tables[0].Rows[0]["HospitalFileNo"].ToString()) ? "" : dsPatient.Tables[0].Rows[0]["HospitalFileNo"].ToString().ToString();
                        txtRegDate.Text = string.IsNullOrEmpty(dsPatient.Tables[0].Rows[0]["dateOfRegistration"].ToString()) ? "" : Convert.ToDateTime(dsPatient.Tables[0].Rows[0]["dateOfRegistration"]).ToString("MMM dd, yyyy");
                    }
                }

            }

        }

        protected string GetHospitalCode(string hspCode)
        {
            string query = "select distinct HospitalCode from Hospitals";
            DataSet dsPatient = SqlHelper.ExecuteDataset(GlobalValues.strConnString, CommandType.Text, query);
            string HospitalCode = "";
            foreach (DataRow dr in dsPatient.Tables[0].Rows)
            {
                if (hspCode.ToString().Contains(dr["HospitalCode"].ToString()))
                {
                    HospitalCode = dr["HospitalCode"].ToString();
                    return HospitalCode;
                }

            }
            return HospitalCode;
        }
    }
}