using System;
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
    public partial class consultants : System.Web.UI.Page
    {

        #region Declarations
        SqlParameter[] Param = new SqlParameter[4];
        DataSet dsConsultants = new DataSet();
        static string message = string.Empty;
        static string consultantUserName = string.Empty;
        static string patientID = string.Empty;
        static string userID = string.Empty;
        string strConns = GlobalValues.strConnString;
        DataTable dtConsultants = new DataTable();

        //string Strqry = "select Salutation + ' ' + FirstName + ' ' + MiddleName +' ' + LastName + ' (' + ISNULL(NULLIF(UniqueReference,'')+ '-','') + UserID + ')' Consultants, UserType,UserID from HospitalUsers where [UserType] ='Consultant' and UserID='" + e + "'";
        //var ConsultantName = GlobalValues.ExecuteScalar(Strqry);
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ResetPassword"] != null)
            {
                Session["ResetPasswordMsg"] = "Please Change your password.";
                Response.Redirect("Changepassword.aspx");
            }
            if (dtConsultants.Columns.Count <= 0)
            {
                dtConsultants.Columns.Add("Consultants", typeof(string));
                dtConsultants.Columns.Add("ConsultantName", typeof(string));
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
            if (!Page.IsPostBack)
            {

                patientID = Session["PatientID"].ToString();
                userID = Session["Login"].ToString();
                BindGrid(patientID, userID);
            }
        }

        #region Private Methods

        void BindGrid(string patientID, string userID)
        {
            try
            {

                Param[0] = new SqlParameter("@consultants", SqlDbType.VarChar);
                Param[0].Value = DBNull.Value;

                Param[1] = new SqlParameter("@patientid", SqlDbType.NVarChar);
                Param[1].Value = Convert.ToString(patientID);

                Param[2] = new SqlParameter("@userid", SqlDbType.NVarChar);
                Param[2].Value = Convert.ToString(userID);


                Param[3] = new SqlParameter("@dataoperation", SqlDbType.NVarChar);
                Param[3].Value = "Select";

                dsConsultants = SqlHelper.ExecuteProcedure(strConns, "sp_Consultants", Param);

                if (dsConsultants != null && dsConsultants.Tables.Count > 0 && dsConsultants.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsConsultants.Tables[0].Rows.Count; i++)
                    {
                        if (dsConsultants.Tables[0].Rows[i]["UserID"].ToString() == userID)
                        {
                            consultantUserName = dsConsultants.Tables[0].Rows[i]["UserID"].ToString();
                            if (dsConsultants.Tables[1].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dsConsultants.Tables[1].Rows[i]["Consultants"].ToString()))
                                {
                                    dtConsultants.Rows.Clear();
                                    string[] valConsultants = dsConsultants.Tables[1].Rows[i]["Consultants"].ToString().Split(',');
                                    foreach (string consultant in valConsultants)
                                    {
                                        DataRow dr = dtConsultants.NewRow();
                                        dr["Consultants"] = consultant.Trim();
                                        dr["ConsultantName"] = GetConsultantName(consultant).Trim();
                                        dtConsultants.Rows.Add(dr);
                                    }
                                    dtConsultants.AcceptChanges();
                                    GridConsultants.DataSource = dtConsultants;
                                    GridConsultants.DataBind();
                                    lblmsg.Text = string.Empty;
                                }
                            }
                            else
                            {
                                GridConsultants.DataSource = null;
                                GridConsultants.DataBind();
                                lblmsg.ForeColor = System.Drawing.Color.Red;//Code modified on March 20,2015
                                lblmsg.Text = "Consultants not found";
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
        }

        private string GetConsultantName(string consultant)
        {
            //string Strqry = "select isnull(Salutation + ' ' + FirstName + ' ' + MiddleName +' ' + LastName + ' (' + ISNULL(NULLIF(UniqueReference,'')+ '-','') + UserID + ')','') as  Consultants, UserType,UserID from HospitalUsers where [UserType] ='Consultant' and UserID='" + consultant + "'";
            string Strqry = " select Salutation + ' ' + FirstName + ' ' + MiddleName +' ' + LastName + (case when NULLIF(UniqueReference,'') is null " +
                            " then '' else ' - ' + UniqueReference + '' end) as Consultants," +
                             " UserType,UserID from HospitalUsers where [UserType] ='Consultant' and UserID='" + consultant + "'";
            var ConsultantName = GlobalValues.ExecuteScalar(Strqry);
            return Convert.ToString(ConsultantName);
        }

        void AddConsultant(string patientID, string userID, string consultant)
        {
            //bool flag = false;
            string consultants = string.Empty;
            dtConsultants.Rows.Clear();
            try
            {
                Param[0] = new SqlParameter("@consultants", SqlDbType.VarChar);
                Param[0].Value = DBNull.Value;

                Param[1] = new SqlParameter("@patientid", SqlDbType.NVarChar);
                Param[1].Value = Convert.ToString(patientID);

                Param[2] = new SqlParameter("@userid", SqlDbType.NVarChar);
                Param[2].Value = Convert.ToString(userID);


                Param[3] = new SqlParameter("@dataoperation", SqlDbType.NVarChar);
                Param[3].Value = "Select";

                dsConsultants = SqlHelper.ExecuteProcedure(strConns, "sp_Consultants", Param);

                if (dsConsultants != null && dsConsultants.Tables.Count > 0)
                {
                    if (dsConsultants.Tables[1].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsConsultants.Tables[1].Rows[0]["Consultants"].ToString()))
                        {
                            dtConsultants.Rows.Clear();
                            string[] valConsultants = dsConsultants.Tables[1].Rows[0]["Consultants"].ToString().Split(',');
                            foreach (string consultantName in valConsultants)
                            {
                                if (consultantName.Trim() != string.Empty)
                                {
                                    DataRow dr = dtConsultants.NewRow();
                                    dr["Consultants"] = consultantName;
                                    dr["ConsultantName"] = GetConsultantName(consultantName);
                                    dtConsultants.Rows.Add(dr);
                                    dtConsultants.AcceptChanges();
                                }
                                if (consultantName == consultant)
                                {
                                    lblmsg.ForeColor = System.Drawing.Color.Red;//Code modified on March 20,2015
                                    lblmsg.Text = "Already you are a consultant to this patient.";
                                    return;
                                }

                            }
                        }
                    }

                    DataRow dr1 = dtConsultants.NewRow();
                    dr1["Consultants"] = consultant;
                    dr1["ConsultantName"] = GetConsultantName(consultant);
                    dtConsultants.Rows.Add(dr1);
                    dtConsultants.AcceptChanges();
                    foreach (DataRow drConsultant in dtConsultants.Rows)
                    {
                        consultants += drConsultant["Consultants"].ToString() + ",";
                        drConsultant["ConsultantName"] = GetConsultantName(drConsultant["Consultants"].ToString());
                    }
                    consultants = consultants.Substring(0, consultants.Length - 1);

                    dtConsultants.AcceptChanges();
                    Param[0] = new SqlParameter("@consultants", SqlDbType.VarChar);
                    Param[0].Value = consultants.Trim().TrimStart(',').TrimEnd(',');


                    Param[1] = new SqlParameter("@patientid", SqlDbType.NVarChar);
                    Param[1].Value = Convert.ToString(patientID);

                    Param[2] = new SqlParameter("@userid", SqlDbType.NVarChar);
                    Param[2].Value = DBNull.Value;


                    Param[3] = new SqlParameter("@dataoperation", SqlDbType.NVarChar);
                    Param[3].Value = "AddRemove";

                    message = SqlHelper.ExecuteProcedureString(strConns, "sp_Consultants", Param);
                    if (!string.IsNullOrEmpty(message) && message.Equals("success"))
                    {
                        lblmsg.ForeColor = System.Drawing.Color.Green;//Code modified on March 20,2015
                        lblmsg.Text = "Consultant added successfully for this patient.";
                        lblmsg.Focus();
                    }
                    if (dtConsultants != null && dtConsultants.Rows.Count > 0)
                    {
                        GridConsultants.DataSource = dtConsultants;
                        GridConsultants.DataBind();
                    }

                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
        }

        void RemoveConsultant(string patientID, string userID, string consultant)
        {
            string consultants = string.Empty;
            try
            {
                Param[0] = new SqlParameter("@consultants", SqlDbType.VarChar);
                Param[0].Value = DBNull.Value;

                Param[1] = new SqlParameter("@patientid", SqlDbType.NVarChar);
                Param[1].Value = Convert.ToString(patientID);

                Param[2] = new SqlParameter("@userid", SqlDbType.NVarChar);
                Param[2].Value = Convert.ToString(userID);


                Param[3] = new SqlParameter("@dataoperation", SqlDbType.NVarChar);
                Param[3].Value = "Select";

                dsConsultants = SqlHelper.ExecuteProcedure(strConns, "sp_Consultants", Param);
                if (dsConsultants != null && dsConsultants.Tables.Count > 0 && dsConsultants.Tables[0].Rows.Count > 0)
                {

                    if (dsConsultants.Tables[1].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsConsultants.Tables[1].Rows[0]["Consultants"].ToString()))
                        {
                            dtConsultants.Rows.Clear();
                            string[] valConsultants = dsConsultants.Tables[1].Rows[0]["Consultants"].ToString().Split(',');
                            foreach (string consultantName in valConsultants)
                            {
                                if (consultantName.Trim() != string.Empty)
                                {
                                    DataRow dr = dtConsultants.NewRow();
                                    dr["Consultants"] = consultantName;
                                    dr["ConsultantName"] = GetConsultantName(consultantName);
                                    dtConsultants.Rows.Add(dr);
                                }
                            }
                            dtConsultants.AcceptChanges();
                            int max = dtConsultants.Rows.Count - 1;
                            for (int i = max; i >= 0; --i)
                            {
                                if (dtConsultants.Rows[i]["Consultants"].ToString() == Session["Login"].ToString())
                                //if (dtConsultants.Rows[i]["Consultants"].ToString() == consultant)
                                {
                                    dtConsultants.Rows[i].BeginEdit();
                                    dtConsultants.Rows[i].Delete();
                                }

                            }
                            dtConsultants.AcceptChanges();

                            if (dtConsultants.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtConsultants.Rows)
                                {
                                    if (dr["Consultants"].ToString() != string.Empty)
                                    {
                                        consultants = consultants + "," + dr["Consultants"].ToString();
                                    }
                                }

                            }

                            Param[0] = new SqlParameter("@consultants", SqlDbType.VarChar);
                            Param[0].Value = consultants.Trim().TrimStart(',').TrimEnd(',');

                            Param[1] = new SqlParameter("@patientid", SqlDbType.NVarChar);
                            Param[1].Value = Convert.ToString(patientID);

                            Param[2] = new SqlParameter("@userid", SqlDbType.NVarChar);
                            Param[2].Value = DBNull.Value;


                            Param[3] = new SqlParameter("@dataoperation", SqlDbType.NVarChar);
                            Param[3].Value = "AddRemove";

                            message = SqlHelper.ExecuteProcedureString(strConns, "sp_Consultants", Param);

                            if (!string.IsNullOrEmpty(message) && message.Equals("success"))
                            {
                                lblmsg.ForeColor = System.Drawing.Color.Green;//Code modified on March 20,2015
                                lblmsg.Text = "Consultant removed successfully for this patient.";
                                lblmsg.Focus();
                            }
                            GridConsultants.DataSource = dtConsultants;
                            GridConsultants.DataBind();


                        }
                        else
                        {
                            lblmsg.ForeColor = System.Drawing.Color.Red;//Code modified on March 20,2015
                            lblmsg.Text = "You are not Consultant to this patient.";
                            lblmsg.Focus();

                        }

                    }
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
        }
        #endregion

        //protected void btnGet_Click(object sender, EventArgs e)
        //{

        //    string patientID = string.Empty;
        //    string userID = string.Empty;
        //     userID = txtUserID.Text.Trim();
        //    patientID = txtPatientID.Text.Trim();
        //    BindGrid(patientID, userID);
        //}

        protected void btnAddMe_Click(object sender, EventArgs e)
        {
            //string patientID = string.Empty;
            //string userID = string.Empty;
            //userID = txtUserID.Text.Trim();
            //patientID = txtPatientID.Text.Trim();
            BindGrid(patientID, userID);
            AddConsultant(patientID, userID, consultantUserName.TrimStart(','));
        }

        protected void btnRemoveMe_Click(object sender, EventArgs e)
        {

            BindGrid(patientID, userID);
            RemoveConsultant(patientID, userID, consultantUserName);
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            lblmsg.Text = string.Empty;
            Response.Redirect("~/ProjectForm.aspx");
        }

    }
}