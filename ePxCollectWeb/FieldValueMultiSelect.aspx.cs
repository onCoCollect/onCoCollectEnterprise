using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Configuration;


namespace ePxCollectWeb
{
    public partial class FieldValueMultiSelect : System.Web.UI.Page
    {
        string FldName = string.Empty;
        string patientId = string.Empty;
        string fldVal = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            FldName = Convert.ToString(Request.QueryString["FN"]);
            if (FldName.Split('.').Length > 0) { FldName = FldName.Split('.')[1].ToString(); }
            FldName = FldName.Replace("[", "").Replace("]", "");
            fldVal = Convert.ToString((Request.QueryString["Val"])).Replace("*ampersand*", "&").Replace("*plus*", "+");
            fldVal = fldVal.Replace("[", "").Replace("]", "");
            if (Session["PatientID"] == null)
            {
                Response.Redirect("SearchPatient.aspx");
            }
            patientId = Convert.ToString(Session["PatientID"]);
            var SiteOFPrimary = GlobalValues.ExecuteScalar("select SiteOfPrimary from PatientDetails_1 where Patient='" + patientId + "' ");

            lblOrigVal.Text = fldVal.ToString();
            this.Title = FldName + ":" + patientId;
            if (!IsPostBack)
            {
                BindDropDowns();
                Session["PickedVal"] = string.Empty;
                if (Session["gMenuDropVal"] != null)
                {
                    if (Session["gMenuDropVal"].ToString().Length > 0)
                    {
                        dpByStudy.SelectedItem.Text = Session["gMenuDropVal"].ToString();// GlobalValues.gMenuDropVal.ToString();
                        ListItem lst = dpByStudy.Items.FindByValue(Session["gMenuDropVal"].ToString());//dpByStudy.Items.FindByValue(GlobalValues.gMenuDropVal.ToString());
                        if (lst != null)
                            dpByStudy.SelectedValue = Session["gMenuDropVal"].ToString();

                        else
                        {
                            if (SiteOFPrimary.ToString() != string.Empty)
                            {
                                dpByDiag.SelectedItem.Text = SiteOFPrimary.ToString();
                                lst = dpByDiag.Items.FindByValue(SiteOFPrimary.ToString());
                                if (lst != null)
                                    dpByDiag.SelectedValue = SiteOFPrimary.ToString();
                                dpByDiag_SelectedIndexChanged(null, null);
                            }

                        }
                        dpByStudy_SelectedIndexChanged(null, null);
                    }
                    else if (SiteOFPrimary.ToString() != string.Empty)
                    {
                        dpByDiag.SelectedItem.Text = SiteOFPrimary.ToString();
                        ListItem lst = dpByDiag.Items.FindByValue(SiteOFPrimary.ToString());
                        if (lst != null)
                            dpByDiag.SelectedValue = SiteOFPrimary.ToString();
                        dpByDiag_SelectedIndexChanged(null, null);
                    }
                }
                else
                    if (SiteOFPrimary.ToString() != string.Empty)
                    {
                        dpByDiag.SelectedItem.Text = SiteOFPrimary.ToString();
                        ListItem lst = dpByDiag.Items.FindByValue(SiteOFPrimary.ToString());
                        if (lst != null)
                            dpByDiag.SelectedValue = SiteOFPrimary.ToString();
                        dpByDiag_SelectedIndexChanged(null, null);
                    }
            }
            lblValuePicked.Text = Convert.ToString(Session["PickedVal"]);
        }

        protected void dpAllValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selVal = string.Empty;
            if (dpAllValues.SelectedItem.Text != "")
            {
                selVal = dpAllValues.SelectedValue.ToString();
                string SQL = "Select [FieldValues] from PDFields where [Field Name] ='" + FldName + "'";
                PopulateValues(SQL);
                BindDropDowns();
                dpByDiag.SelectedItem.Text = "";
                dpByStudy.SelectedItem.Text = "";
                dpAllValues.SelectedValue = selVal;
                lstValues.Focus();
            }
        }

        protected void dpByDiag_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selVal = string.Empty;
            if (dpByDiag.SelectedItem.Text != "")
            {
                selVal = dpByDiag.SelectedValue.ToString();
                string SQL = "Select FieldValue from FieldValues_ByDiagnosis where FieldName ='" + FldName + "' and DiagnosisName = '" + dpByDiag.SelectedItem.Text + "'";
                PopulateValues(SQL);
                BindDropDowns();
                dpAllValues.SelectedItem.Text = "";
                dpByStudy.SelectedItem.Text = "";
                dpByDiag.SelectedValue = selVal;
                lstValues.Focus();
            }
        }

        protected void dpByStudy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selVal = string.Empty;
            if (dpByStudy.SelectedItem.Text != "")
            {
                selVal = dpByStudy.SelectedValue.ToString();
                string SQL = "Select FieldValue from FieldValues_ByStudy where FieldName='" + FldName + "' and StudyName = '" + dpByStudy.SelectedItem.Text + "'";
                PopulateValues(SQL);
                BindDropDowns();
                dpAllValues.SelectedItem.Text = "";
                dpByDiag.SelectedItem.Text = "";
                dpByStudy.SelectedValue = selVal;
                lstValues.Focus();
            }

        }
        private void PopulateValues(string strSQL)
        {
            DataSet ds = GlobalValues.ExecuteDataSet(strSQL);
            lstValues.Items.Clear();
            string[] strVal;
            if (lblValuePicked.Text.ToString().Length > 0)
            {
                strVal = lblValuePicked.Text.Split(',');
            }
            else
            {
                strVal = lblOrigVal.Text.Split(',');
            }
            //lblValuePicked.Text = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string ColVal = dr[0].ToString();
                ColVal = ColVal.TrimStart(',').Trim();
                if (ColVal == "N/A") ColVal = "";
                string[] Cols = ColVal.Split(',');
                string strNewVal = string.Empty;
                for (int I = 0; I < Cols.Length; I++)
                {
                    //if (Cols[I].ToString() != string.Empty)
                    {
                        lstValues.Items.Add(Cols[I].ToString());
                        foreach (string s in strVal)
                        {
                            if (s == Cols[I].ToString())
                            {
                                if (lstValues.Items[I].Text != string.Empty)
                                {
                                    lstValues.Items[I].Selected = true;
                                    strNewVal += "," + s;
                                }
                            }
                        }
                    }
                }
                if (lblValuePicked.Text.Length <= 0 && strNewVal.Length > 0)
                {
                    lblValuePicked.Text = strNewVal.Substring(1);
                    Session["PickedVal"] = lblValuePicked.Text.ToString();
                }
            }

        }

        private void BindDropDowns()
        {
            string strSql = string.Empty;

            DataRow dr;
            strSql = "Select siteofprimary from Patientdetails_1 where Patient = '" + patientId + "'";
            DataSet ds = GlobalValues.ExecuteDataSet(strSql);
            dr = ds.Tables[0].NewRow();
            dr[0] = "";
            ds.Tables[0].Rows.InsertAt(dr, 0);
            dpByDiag.DataSource = ds.Tables[0];
            dpByDiag.DataTextField = "siteofprimary";
            dpByDiag.DataValueField = "siteofprimary";
            dpByDiag.DataBind();
            //strSql = "Select StudyName from Studies where Active=1 and Instances like '%-" + GlobalValues.gInstanceID + "-%'  and '" + patientId + "' in ( select a.HosCode from fn_Splithospitalcode(replace(StudyPatientsList,'''','')) as a )";
            strSql = "Select StudyName  from [Studies] S inner join tblStudyUsers  SU on  s.StudyCode=SU.StudyCode " +
                       " where  Active=1 and instances  like '%-" + GlobalValues.gInstanceID.ToString() + "-%'  and  su.users like '%" + Session["Login"].ToString() + "%' order by studyName ";

            ds = GlobalValues.ExecuteDataSet(strSql);
            dr = ds.Tables[0].NewRow();
            dr[0] = "";
            ds.Tables[0].Rows.InsertAt(dr, 0);
            dpByStudy.DataSource = ds.Tables[0];
            dpByStudy.DataTextField = "StudyName";
            dpByStudy.DataValueField = "StudyName";
            dpByStudy.DataBind();
            dpAllValues.Items.Clear();
            dpAllValues.Items.Add("");
            dpAllValues.Items.Add("All Values");

        }

        protected void lstValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lbVals = string.Empty;
            lbVals = Convert.ToString(Session["PickedVal"]);
            foreach (ListItem li in lstValues.Items)
            {
                if (lbVals.ToString().Contains(li.Text.ToString()))
                {
                    if (li.Selected == false)
                    {
                        if (lbVals.Length > 0 && li.Text.ToString().Length > 0)
                        {
                            lbVals = lbVals.Replace(li.Text.ToString(), "");
                        }
                    }
                }
                else
                {
                    if (li.Selected == true)
                    {
                        lbVals += "," + li.Text.ToString();
                    }
                }

            }
            lbVals = lbVals.Replace(",,", ",");
            if (lbVals.Length > 0)
            {
                if (lbVals.Substring(0, 1) == ",") { lbVals = lbVals.Substring(1); }
                lblValuePicked.Text = lbVals;
            }
            else
            {
                lblValuePicked.Text = string.Empty;
            }
            Session["PickedVal"] = lblValuePicked.Text.ToString();
            updValues.Update();
        }
        //string FldName = string.Empty;
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    string strSql = string.Empty;
        //    string patientId = string.Empty;           
        //    FldName = Convert.ToString(Request.QueryString["FN"]);
        //    if (FldName.Split('.').Length > 0) { FldName = FldName.Split('.')[1].ToString(); }
        //    FldName = FldName.Replace("[", "").Replace("]", "");
        //    patientId = Convert.ToString(Session["PatientID"]);
        //    if (!IsPostBack)
        //    {
        //        DataRow dr;
        //        strSql = "Select Diagnosis from Patientdetails_1 where Patient = '" + patientId + "'";
        //        DataSet ds = GlobalValues.ExecuteDataSet(strSql);
        //        dr = ds.Tables[0].NewRow();
        //        dr[0] = "";
        //        ds.Tables[0].Rows.InsertAt(dr, 0);
        //        dpByDiag.DataSource = ds.Tables[0];
        //        dpByDiag.DataTextField = "Diagnosis";
        //        dpByDiag.DataValueField = "Diagnosis";
        //        dpByDiag.DataBind();
        //        strSql = "Select StudyName from Studies where Active=1 and Instances like '%-" + GlobalValues.gInstanceID + "-%'";
        //        ds = GlobalValues.ExecuteDataSet(strSql);
        //        dr = ds.Tables[0].NewRow();
        //        dr[0] = "";
        //        ds.Tables[0].Rows.InsertAt(dr, 0);
        //        dpByStudy.DataSource = ds.Tables[0];
        //        dpByStudy.DataTextField = "StudyName";
        //        dpByStudy.DataValueField = "StudyName";
        //        dpByStudy.DataBind();
        //        dpAllValues.Items.Add("");
        //        dpAllValues.Items.Add("All Values");
        //        if (GlobalValues.gMenuDropVal.ToString().Length > 0)
        //        {
        //            dpByStudy.SelectedItem.Text = GlobalValues.gMenuDropVal.ToString();
        //            dpByStudy_SelectedIndexChanged(null, null);
        //        }
        //    }
        //}

        //protected void dpAllValues_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (dpAllValues.SelectedItem.Text != "")
        //    {
        //        dpByDiag.SelectedItem.Text = "";
        //        dpByStudy.SelectedItem.Text = "";
        //        string SQL = "Select [FieldValues] from PDFields where [Field Name] ='" + FldName + "'";
        //        PopulateValues(SQL);
        //    }
        //}

        //protected void dpByDiag_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (dpByDiag.SelectedItem.Text != "")
        //    {
        //        dpAllValues.SelectedItem.Text = "";
        //        dpByStudy.SelectedItem.Text = "";
        //        string SQL = "Select FieldValue from FieldValues_ByDiagnosis where FieldName ='" + FldName + "' and DiagnosisName = '" + dpByDiag.SelectedItem.Text + "'";
        //        PopulateValues(SQL);
        //    }
        //}

        //protected void dpByStudy_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (dpByStudy.SelectedItem.Text != "")
        //    {
        //        dpAllValues.SelectedItem.Text = "";
        //        dpByDiag.SelectedItem.Text = "";
        //        string SQL = "Select FieldValue from FieldValues_ByStudy where FieldName='" + FldName + "' and StudyName = '" + dpByStudy.SelectedItem.Text + "'";
        //        PopulateValues(SQL);
        //    }

        //}

        //private void PopulateValues(string strSQL)
        //{
        //    DataSet ds = GlobalValues.ExecuteDataSet(strSQL);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        DataRow dr = ds.Tables[0].Rows[0];
        //        lstValues.Items.Clear();
        //        string ColVal = dr[0].ToString();
        //        if (ColVal == "N/A") ColVal = "";
        //        string[] Cols = ColVal.Split(',');
        //        for (int I = 0; I < Cols.Length; I++)
        //        {

        //            lstValues.Items.Add(Cols[I].ToString());
        //        }
        //    }
        //}
    }
}