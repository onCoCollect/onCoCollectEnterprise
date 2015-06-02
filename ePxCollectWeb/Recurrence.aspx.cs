using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ePxCollectWeb
{
    public partial class Recurrence : System.Web.UI.Page
    {
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
        string gPatientId = string.Empty;
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

            if (Session["PatientID"] == null)
            {
                Session["Message"] = "Please pick a Patient.";
                Response.Redirect("SearchPatient.aspx");
            }
            else
            {
                gPatientId = Convert.ToString(Session["PatientID"]);
            }
            txtPatient.Text = gPatientId;
            if (ObjfeatureSet.isRecurrence == false)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Sorry, you don't have permission to this functionality.');", true);
                Response.Redirect("ProjectForm.aspx");
            }
            if (!IsPostBack)
            {
                //GlobalValues.gPostBackURL = Request.Url.AbsoluteUri.ToString();
                Session["gPostBackURL"] = Request.Url.AbsoluteUri.ToString();
                string strSQL = "Select Patient, convert(Varchar(11),DateofDiagnosis) DateofDiagnosis , convert(Varchar(11),DateofDeath) DateofDeath,  convert(Varchar(11),[1st RecurrenceDate]) as Rec1, convert(Varchar(11),[2nd RecurrenceDate]) Rec2, convert(Varchar(11),[3rd RecurrenceDate]) Rec3, convert(Varchar(11),[4th RecurrenceDate]) Rec4  from Patientdetails_1 p with (nolock) left outer join Recurrences R with (nolock) on P.Patient=R.PatientId  where P.Patient='" + gPatientId + "'";
                DataSet dsRec = GlobalValues.ExecuteDataSet(strSQL);
                if (dsRec.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsRec.Tables[0].Rows[0];

                    if (dr["DateofDeath"].ToString() != string.Empty)
                    {
                        txtDateofDeath.Text = Convert.ToDateTime(dr["DateofDeath"].ToString()).ToString("MMMM d, yyyy");
                        if (Convert.ToDateTime(dr["DateofDeath"].ToString()).Year == 1900)
                        { txtDateofDeath.Text = ""; }
                    }
                    else
                        txtDateofDeath.Text = dr["DateofDeath"].ToString();

                    if (dr["DateofDiagnosis"].ToString() != string.Empty)
                    {
                        txtDiagnosisDate.Text = Convert.ToDateTime(dr["DateofDiagnosis"].ToString()).ToString("MMMM d, yyyy");
                        if (Convert.ToDateTime(dr["DateofDiagnosis"].ToString()).Year == 1900)
                        { txtDiagnosisDate.Text = ""; }
                    }
                    else
                        txtDiagnosisDate.Text = dr["DateofDiagnosis"].ToString();





                    if (Convert.ToString(dr["Rec1"]) != "")
                    {
                        dtRec1.Text = Convert.ToDateTime(dr["Rec1"].ToString().Trim()).ToString("MMMM d, yyyy");
                    }
                    if (Convert.ToString(dr["Rec2"]) != "")
                    {
                        dtRec2.Text = Convert.ToDateTime(dr["Rec2"].ToString().Trim()).ToString("MMMM d, yyyy");
                    }
                    if (Convert.ToString(dr["Rec3"]) != "")
                    {
                        dtRec3.Text = Convert.ToDateTime(dr["Rec3"].ToString().Trim()).ToString("MMMM d, yyyy");
                    }
                    if (Convert.ToString(dr["Rec4"]) != "")
                    {
                        dtRec4.Text = Convert.ToDateTime(dr["Rec4"].ToString().Trim()).ToString("MMMM d, yyyy");
                    }
                }
                else
                {
                    Response.Redirect("ProjectForm.aspx", false);
                }
                dtRec1.Attributes.Add("onkeypress", "return handleKey()");
                dtRec2.Attributes.Add("onkeypress", "return handleKey()");
                dtRec3.Attributes.Add("onkeypress", "return handleKey()");
                dtRec4.Attributes.Add("onkeypress", "return handleKey()");

                dtRec1.Width = 150;
                dtRec2.Width = 150;
                dtRec3.Width = 150;
                dtRec4.Width = 150;
                //dtRec1.PaneWidth = 150;
                //dtRec2.PaneWidth = 150;
                //dtRec3.PaneWidth = 150;
                //dtRec4.PaneWidth = 150;
                //if (txtDateofDeath.Text.Length <= 0)
                //{
                //    calRec1.EndDate = DateTime.Now;
                //    calRec2.EndDate = DateTime.Now;
                //    calRec3.EndDate = DateTime.Now;
                //    calRec4.EndDate = DateTime.Now;
                //}
                //else
                //{
                //    calRec1.EndDate = Convert.ToDateTime(txtDateofDeath.Text.ToString());
                //    calRec2.EndDate = Convert.ToDateTime(txtDateofDeath.Text.ToString());
                //    calRec3.EndDate = Convert.ToDateTime(txtDateofDeath.Text.ToString());
                //    calRec4.EndDate = Convert.ToDateTime(txtDateofDeath.Text.ToString());
                //}
            }
            //setStartDates();
        }

        //private void setStartDates()
        //{
        //    if (txtDiagnosisDate.Text.Length > 0) { calRec1.StartDate = Convert.ToDateTime(txtDiagnosisDate.Text.ToString()); }
        //    if (dtRec1.Text.Length > 0) { ViewState[dtRec1.ID.ToString()] = dtRec1.Text.ToString(); calRec2.StartDate = Convert.ToDateTime(dtRec1.Text.ToString()); }
        //    if (dtRec2.Text.Length > 0) { calRec3.StartDate = Convert.ToDateTime(dtRec2.Text.ToString()); }
        //    if (dtRec3.Text.Length > 0) { calRec4.StartDate = Convert.ToDateTime(dtRec3.Text.ToString()); }
        //    updPHControls.Update();
        //}



        //protected void dtRec2_TextChanged(object sender, EventArgs e)
        //{
        //    dtRec2.ReadOnly = false;
        //    setStartDates();
        //    dtRec2.ReadOnly = true;
        //}

        //protected void dtRec3_TextChanged(object sender, EventArgs e)
        //{
        //    setStartDates();
        //}
        private string ValidateDates()
        {
            string strMsg = string.Empty;
            try
            {
                if (dtRec1.Text.ToString().Length <= 0 && dtRec2.Text.ToString().Length <= 0 && dtRec3.Text.ToString().Length <= 0 && dtRec4.Text.ToString().Length <= 0)
                {
                    strMsg += "Please enter the Recurrence Date.";
                }
                if (dtRec1.Text.ToString().Length > 0)
                {
                    if (txtDateofDeath.Text.Length > 0)
                        if (Convert.ToDateTime(dtRec1.Text) > Convert.ToDateTime(txtDateofDeath.Text))
                        {
                            strMsg += "Recurrence Date1 can't be greater than the DateofDeath.";// <br/>";
                        }

                    if (Convert.ToDateTime(dtRec1.Text) != Convert.ToDateTime(null))
                    {

                        //if (txtDiagnosisDate.Text == "")
                        //{
                        //    strMsg += "Diagnosis Date is missing, hence recurrence date cannot be saved.";// <br/>";
                        //}
                        //else
                        {
                            if (Convert.ToDateTime(dtRec1.Text.ToString()) <= Convert.ToDateTime(txtDiagnosisDate.Text.ToString()))
                            {
                                strMsg += "Recurrence Date1 can't be less than the Diagnosis Date (" + txtDiagnosisDate.Text.ToString() + ")";// .<br/>";
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(dtRec2.Text.ToString()) != Convert.ToDateTime(null) && dtRec2.Text.ToString().Length > 0)
                        {
                            strMsg += "Recurrence Date2 can't be greater than the Recurrence Date1.";//<br/>";
                        }

                    }
                }
                if (dtRec2.Text.ToString().Length > 0)
                {
                    if (Convert.ToDateTime(dtRec2.Text) != Convert.ToDateTime(null))
                    {
                        if (Convert.ToDateTime(dtRec2.Text.ToString()) <= Convert.ToDateTime(dtRec1.Text.ToString()))
                        {
                            strMsg += "Recurrence Date2 can't be less than the Recurrence Date1.";// <br/>";
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(dtRec3.Text.ToString()) != Convert.ToDateTime(null) && dtRec3.Text.ToString().Length > 0)
                        {
                            strMsg += "Recurrence Date3 can't be greater than the Recurrence Date2.";// <br/>";
                        }

                    }
                }
                if (dtRec3.Text.ToString().Length > 0)
                {
                    if (Convert.ToDateTime(dtRec3.Text) != Convert.ToDateTime(null))
                    {
                        if (Convert.ToDateTime(dtRec3.Text.ToString()) <= Convert.ToDateTime(dtRec2.Text.ToString()))
                        {
                            strMsg += "Recurrence Date3 can't be less than the Recurrence Date2.";// <br/>";
                            // ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Recurrence details saved successfully');", true);
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(dtRec4.Text) != Convert.ToDateTime(null) && dtRec4.Text.ToString().Length > 0)
                        {
                            strMsg += "Recurrence Date4 can't be greater than the Recurrence Date3.";// <br/>";
                        }

                    }
                }
                if (dtRec4.Text.ToString().Length > 0)
                {
                    if (Convert.ToDateTime(dtRec4.Text) != Convert.ToDateTime(null))
                    {
                        if (Convert.ToDateTime(dtRec4.Text.ToString()) <= Convert.ToDateTime(dtRec3.Text.ToString()))
                        {
                            strMsg += "Recurrence Date4 can't be less than the Recurrence Date3. ";//<br/>";
                        }
                    }
                }
                if (strMsg.Length <= 0)
                {
                    if (dtRec1.Text.ToString().Length > 0)
                        if (Convert.ToDateTime(dtRec1.Text) > DateTime.Now) { strMsg += "Recurrence Date1 can't be greater than today."; }
                    if (dtRec2.Text.ToString().Length > 0)
                        if (Convert.ToDateTime(dtRec2.Text) > DateTime.Now) { strMsg += "Recurrence Date2 can't be greater than today."; }
                    if (dtRec3.Text.ToString().Length > 0)
                        if (Convert.ToDateTime(dtRec3.Text) > DateTime.Now) { strMsg += "Recurrence Date3 can't be greater than today."; }
                    if (dtRec4.Text.ToString().Length > 0)
                        if (Convert.ToDateTime(dtRec4.Text) > DateTime.Now) { strMsg += "Recurrence Date4 can't be greater than today."; }
                }
                return strMsg;
            }
            catch (Exception ex)
            {

                strMsg += "Validation failed, please check all the dates and try saving again.";
                return strMsg;
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string sqlStr = string.Empty;
            lblError.Text = "";
            if (txtDiagnosisDate.Text.ToString().Length > 0)
            {
                string validDates = ValidateDates();
                if (validDates.Length <= 0)
                {
                    sqlStr = "select patientId from Recurrences with (nolock) where patientId='" + gPatientId + "'";
                    string Patient = Convert.ToString(GlobalValues.ExecuteScalar(sqlStr));
                    if (Patient != null)
                    {
                        if (Patient.Length <= 0)
                        {
                            sqlStr = "Insert into Recurrences (patientId) values ('" + gPatientId + "')";
                            GlobalValues.ExecuteNonQuery(sqlStr);
                        }
                    }
                    sqlStr = string.Empty;
                    if (dtRec1.Text.ToString() != "")
                    {
                        if (sqlStr.Length > 0) { sqlStr += ","; }
                        if (Convert.ToDateTime(dtRec1.Text) == Convert.ToDateTime(null))
                        {
                            sqlStr += " [1st RecurrenceDate]=null";
                        }
                        else
                        {
                            sqlStr += " [1st RecurrenceDate]='" + Convert.ToDateTime(dtRec1.Text).ToString("dd MMM yyyy") + "'";
                        }
                    }
                    if (dtRec2.Text.ToString() != "")
                    {
                        if (sqlStr.Length > 0) { sqlStr += ","; }
                        if (Convert.ToDateTime(dtRec2.Text) == Convert.ToDateTime(null))
                        {
                            sqlStr += " [2nd RecurrenceDate]=null";
                        }
                        else
                        {
                            sqlStr += " [2nd RecurrenceDate]='" + Convert.ToDateTime(dtRec2.Text).ToString("dd MMM yyyy") + "'";
                        }
                    }
                    if (dtRec3.Text.ToString() != "")
                    {
                        if (sqlStr.Length > 0) { sqlStr += ","; }
                        if (Convert.ToDateTime(dtRec3.Text) != Convert.ToDateTime(null))
                        {
                            sqlStr += " [3rd RecurrenceDate]='" + Convert.ToDateTime(dtRec3.Text).ToString("dd MMM yyyy") + "'";
                        }
                        else
                        {
                            sqlStr += " [3rd RecurrenceDate]=null";
                        }
                    }
                    if (dtRec4.Text.ToString() != "")
                    {
                        if (sqlStr.Length > 0) { sqlStr += ","; }
                        if (Convert.ToDateTime(dtRec4.Text) != Convert.ToDateTime(null))
                        {
                            sqlStr += " [4th RecurrenceDate]='" + Convert.ToDateTime(dtRec4.Text).ToString("dd MMM yyyy") + "'";
                        }
                        else
                        {
                            sqlStr += " [4th RecurrenceDate]=null";
                        }
                    }
                    if (sqlStr.Length > 0)
                    {
                        sqlStr = "Update Recurrences Set " + sqlStr + " where PatientID= '" + gPatientId + "'";
                        GlobalValues.ExecuteNonQuery(sqlStr);
                        GlobalValues.SaveSessionLog(Session["PatientID"].ToString(), Session["Login"].ToString(), GlobalValues.SLogRecurrence);
                       // ScriptManager.RegisterStartupScript(this, typeof(string), "Success", "alert('Recurrence details saved successfully.');", true);
                        lblError.ForeColor = GlobalValues.SucessColor;//Code modified on April 24,2015-Subhashini
                        lblError.Visible = true;
                        lblError.Text = "Recurrence details saved";
                        // Response.Redirect("ProjectForm.aspx");
                    }
                    else
                    {

                    }
                }
                else
                {
                    //lblError.Text = "Please enter the Recurrence Date.";
                    lblError.ForeColor = GlobalValues.FailureColor;//Code modified on April 24,2015-Subhashini
                    lblError.Text = validDates;
                    lblError.Visible = true;
                    updPHControls.Update();
                }
            }
            else
            {
                lblError.ForeColor = GlobalValues.FailureColor;//Code modified on April 24,2015-Subhashini
                lblError.Text = "Please enter the Diagnosis Date.";
                lblError.Visible = true;
                updPHControls.Update();
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProjectForm.aspx");
        }

        /*protected void lnkClear1_Click(object sender, EventArgs e)
        {
            dtRec1.Text = Convert.ToDateTime(null).ToString();
        }

        protected void lnkClear2_Click(object sender, EventArgs e)
        {
            dtRec2.SelectedDate = Convert.ToDateTime(null);
        }

        protected void lnkClear3_Click(object sender, EventArgs e)
        {
            dtRec3.SelectedDate = Convert.ToDateTime(null); ;
        }

        protected void lnkClear4_Click(object sender, EventArgs e)
        {
            dtRec4.SelectedDate = Convert.ToDateTime(null);
        }

        */
        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblError.Text = String.Empty;
            lblError.Visible = true;
            updPHControls.Update();
            dtRec1.Text = "";
            dtRec2.Text = "";
            dtRec3.Text = "";
            dtRec4.Text = "";


        }




    }
}