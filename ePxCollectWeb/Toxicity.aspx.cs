using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ePxCollectWeb
{
    public partial class Toxicity : System.Web.UI.Page
    {
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
        string lngG_PatId = string.Empty;
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
            lngG_PatId = Convert.ToString(Session["PatientID"]);
            //GlobalValues.gPostBackURL = Request.Url.AbsoluteUri.ToString();
            Session["gPostBackURL"] = Request.Url.AbsoluteUri.ToString();
            if (ObjfeatureSet.isToxicity == false)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Sorry, you don't have permission to this functionality.');", true);
                Response.Redirect("ProjectForm.aspx");
            }
            if (!IsPostBack)
            {
                FillToxicity();
                lstLines.Items.Clear();
                lstLines.Items.Add("");
                lstLines.Items.Add("1st Line Worst Toxicity");
                lstLines.Items.Add("2nd Line Worst Toxicity");
                lstLines.Items.Add("3rd Line Worst Toxicity");
                lstLines.Items.Add("4th Line Worst Toxicity");
                lstLines.Items.Add("5th Line Worst Toxicity");


                lstLinesMoreToxicity.Items.Clear();
                lstLinesMoreToxicity.Items.Add("");
                lstLinesMoreToxicity.Items.Add("1st Line Worst Toxicity");
                lstLinesMoreToxicity.Items.Add("2nd Line Worst Toxicity");
                lstLinesMoreToxicity.Items.Add("3rd Line Worst Toxicity");
                lstLinesMoreToxicity.Items.Add("4th Line Worst Toxicity");
                lstLinesMoreToxicity.Items.Add("5th Line Worst Toxicity");




                ListBoxGradeMoreToxixcity.Items.Add(" ");
                ListBoxGradeMoreToxixcity.Items.Add(new ListItem("Grade 0", "0"));
                ListBoxGradeMoreToxixcity.Items.Add(new ListItem("Grade 1", "1"));
                ListBoxGradeMoreToxixcity.Items.Add(new ListItem("Grade 2", "2"));
                ListBoxGradeMoreToxixcity.Items.Add(new ListItem("Grade 3", "3"));
                ListBoxGradeMoreToxixcity.Items.Add(new ListItem("Grade 4", "4"));

                foreach (Control ctl in pnlMoreToxicity.Controls)
                {
                    if (ctl.ID != null)
                    {
                        if (ctl.ID.ToString().Contains("ListBox") && !ctl.ID.ToString().Contains("ListBoxGradeMoreToxixcity"))
                        {
                            ListBox lst = (ListBox)ctl;
                            lst.Items.Add(" ");
                            lst.Items.Add("Grade 0");
                            lst.Items.Add("Grade 1");
                            lst.Items.Add("Grade 2");
                            lst.Items.Add("Grade 3");
                            lst.Items.Add("Grade 4");
                        }
                    }
                }

                foreach (Control ctl in pnlControls.Controls)
                {
                    if (ctl.ID != null)
                    {
                        if (ctl.ID.ToString().Contains("ListBox"))
                        {
                            //if (ctl.ID == "ListBox1")
                            {
                                DropDownList lst = (DropDownList)ctl;
                                lst.Items.Add(" ");
                                lst.Items.Add("Grade 0");
                                lst.Items.Add("Grade 1");
                                lst.Items.Add("Grade 2");
                                lst.Items.Add("Grade 3");
                                lst.Items.Add("Grade 4");
                                //lst.Items.Add("Grade 5");
                            }
                            //else
                            //{
                            //    ListBox lst = (ListBox)ctl;
                            //    lst.Items.Add("Grade 0");
                            //    lst.Items.Add("Grade 1");
                            //    lst.Items.Add("Grade 2");
                            //    lst.Items.Add("Grade 3");
                            //    lst.Items.Add("Grade 4");
                            //    lst.Items.Add("Grade 5");
                            //}

                            //lst.Enabled = false;
                        }
                        else if (ctl.ID.ToString().Contains("TextBox"))
                        {
                            TextBox txt = (TextBox)ctl;
                            //txt.Enabled = false;
                            txt.ReadOnly = true;
                        }
                    }
                }
                pnlControls.Enabled = false;
            }
        }


        protected void ListBoxChanged(object sender, EventArgs e)
        {
            DropDownList lst = (DropDownList)sender;
            string lstId = lst.ID.ToString();
            string StrSufix = string.Empty;
            string StrPrefix = string.Empty;
            lstId = lstId.Replace("ListBox", ""); //lstId.Substring(lstId.Length - 1);
            TextBox tBox = (TextBox)pnlControls.FindControl("TextBox" + lstId);
            StrPrefix = lstLines.SelectedValue.ToString().Substring(0, 8);
            if (tBox != null)
            {
                StrSufix = getSufix(lstId);
                if (StrPrefix == string.Empty || StrSufix == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Not yet Ready for this');", true);
                }
                else
                {
                    string sql = string.Empty;
                    if (lst.SelectedValue.ToString().Trim() != "")
                    {
                        sql = "Update Patientdetails_2 set [" + StrPrefix + " " + StrSufix + "] = '" + (lst.SelectedIndex - 1).ToString() + "'  where patient ='" + lngG_PatId + "'";
                        GlobalValues.ExecuteNonQuery(sql);
                        sql = "SELECT Toxizity.ToxDetail From Toxizity Where Toxizity.ToxicityName = '" + StrSufix + "' And Toxizity.ToxGrade = '" + (lst.SelectedIndex - 1).ToString() + "'";
                        string strVal = Convert.ToString(GlobalValues.ExecuteScalar(sql));
                        if (strVal == null) { strVal = ""; }
                        tBox.Text = strVal;
                    }
                    else
                    {
                        sql = "Update Patientdetails_2 set [" + StrPrefix + " " + StrSufix + "] = null where patient ='" + lngG_PatId + "'";
                        GlobalValues.ExecuteNonQuery(sql);
                        tBox.Text = "";
                    }

                    GlobalValues.SaveSessionLog(Session["PatientID"].ToString(), Session["Login"].ToString(), GlobalValues.SLogToxicity);

                }

            }


            lst.Focus();
        }

        private string getSufix(string Index)
        {
            string StrSufix = string.Empty;
            switch ((Convert.ToInt32(Index) - 1).ToString())
            {
                case "0":
                    StrSufix = "Neutropenia"; break;
                case "1":
                    StrSufix = "Leukocytopenia"; break;
                case "2":
                    StrSufix = "Thrombocytopenia"; break;
                case "3":
                    StrSufix = "Anemia"; break;
                case "4":
                    StrSufix = "Febrile Neutropenia"; break;
                case "5":
                    StrSufix = "Nausea and Vomiting"; break;
                case "6":
                    StrSufix = "Diarrhea"; break;
                case "7":
                    StrSufix = "Oral Mucositis"; break;
                case "8":
                    StrSufix = "Skin Rash"; break;
                case "9":
                    StrSufix = "Hand Foot Syndrome"; break;
                case "10":
                    StrSufix = "Hypertension"; break;
                case "11":
                    StrSufix = "Peripheral Neuropathy"; break;
                case "12":
                    StrSufix = "Cardiac Lv Function"; break;
                case "13":
                    StrSufix = "Pulmonary Function"; break;
                case "14":
                    StrSufix = "Renal Function"; break;
                case "15":
                    StrSufix = "Proteinuria"; break;
            }
            return StrSufix;
        }
        protected void lstLines_SelectedIndexChanged(object sender, EventArgs e)
        {

            string sqlStr = string.Empty;
            bool EnablePnl = false;
            object retVal;
            string lstSel = string.Empty;
            if (lstLines.SelectedValue == "")
            {
                EnablePnl = false;
            }
            if (lstLines.SelectedValue == "1st Line Worst Toxicity")
            {
                EnablePnl = true;
            }
            else

                if (lstLines.SelectedValue == "2nd Line Worst Toxicity")
                {
                    sqlStr = " Select [1st RecurrenceDate]  from Recurrences where Patientid='" + lngG_PatId + "'";
                    retVal = GlobalValues.ExecuteScalar(sqlStr);
                    if (retVal == null) { retVal = ""; }
                    if (retVal.ToString() != "") { EnablePnl = true; } else { lstSel = "1st Line Worst Toxicity"; };
                }
                else
                    if (lstLines.SelectedValue == "3rd Line Worst Toxicity")
                    {
                        sqlStr = " Select  cast([1st RecurrenceDate]  as Datetime )  + cast( [2nd RecurrenceDate] as DateTime)  from Recurrences where Patientid='" + lngG_PatId + "'";
                        retVal = GlobalValues.ExecuteScalar(sqlStr);
                        if (retVal == null) { retVal = ""; }
                        if (retVal.ToString() != "") { EnablePnl = true; } else { lstSel = "2nd Line Worst Toxicity"; };
                    }
                    else
                        if (lstLines.SelectedValue == "4th Line Worst Toxicity")
                        {
                            sqlStr = " Select [1st RecurrenceDate]  + cast( [2nd RecurrenceDate] as DateTime)+cast( [3rd RecurrenceDate] as DateTime)  from Recurrences where Patientid='" + lngG_PatId + "'";
                            retVal = GlobalValues.ExecuteScalar(sqlStr);
                            if (retVal == null) { retVal = ""; }
                            if (retVal.ToString() != "") { EnablePnl = true; } else { lstSel = "3rd Line Worst Toxicity"; };
                        }
                        else
                            if (lstLines.SelectedValue == "5th Line Worst Toxicity")
                            {
                                sqlStr = " Select cast([1st RecurrenceDate]  as Datetime )  + cast( [2nd RecurrenceDate] as DateTime)+cast( [3rd RecurrenceDate] as DateTime) +cast( [4th RecurrenceDate] as DateTime)  from Recurrences where Patientid='" + lngG_PatId + "'";
                                retVal = GlobalValues.ExecuteScalar(sqlStr);
                                if (retVal == null) { retVal = ""; }
                                if (retVal.ToString() != "") { EnablePnl = true; } else { lstSel = "4th Line Worst Toxicity"; };
                            }

            if (EnablePnl)
            {
                try
                {

                    lblErrorMsg.Text = "";
                    pnlControls.Enabled = true;
                    String StrPrefix = lstLines.SelectedValue.ToString().Substring(0, 8);
                    for (int i = 1; i <= 16; i++)
                    {
                        string StrSufix = getSufix(i.ToString());
                        string strVal = (string)GlobalValues.ExecuteScalar("Select top 1 isnull([" + StrPrefix + " " + StrSufix + "],'') from Patientdetails_2 where Patient ='" + lngG_PatId + "'");
                        if (strVal != "")
                        {
                            try
                            {


                                DropDownList lst = (DropDownList)pnlControls.FindControl("ListBox" + i.ToString());
                                if (lst != null)
                                {
                                    //lst.Items[Convert.ToInt32(strVal)+1].Selected = true;
                                    lst.SelectedIndex = Convert.ToInt32(strVal) + 1;
                                    //ListBoxChanged(lst, null);
                                    TextBox tBox = (TextBox)pnlControls.FindControl("TextBox" + i.ToString());
                                    string sql = "SELECT Toxizity.ToxDetail From Toxizity Where Toxizity.ToxicityName = '" + StrSufix + "' And Toxizity.ToxGrade = '" + (lst.SelectedIndex - 1).ToString() + "'";
                                    tBox.Text = Convert.ToString(GlobalValues.ExecuteScalar(sql));
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                        else
                        {
                            TextBox tBox = (TextBox)pnlControls.FindControl("TextBox" + i.ToString());
                            tBox.Text = "";
                            DropDownList lst = (DropDownList)pnlControls.FindControl("ListBox" + i.ToString());
                            if (lst != null)
                            {
                                lst.SelectedIndex = 0;
                            }
                        }
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                pnlControls.Enabled = false;
                if (lstLines.SelectedValue == "")
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Please select a Line of Worst Toxicity.');", true);
                    lblErrorMsg.Text = "Please select a Line of Worst Toxicity.";
                    Clear();
                }
                else if (lstLines.SelectedValue != " ")
                {
                    lblErrorMsg.Text = "Previous Recurrence dates are required.";//Code modified on March 05-2015,Subhashini
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Previous Recurrence Dates are required');", true);
                    Clear();
                }
                //lstLines.SelectedValue = lstSel;
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchPatient.aspx");
        }
        private void Clear()
        {
            for (int i = 1; i <= 16; i++)
            {
                DropDownList lst = (DropDownList)pnlControls.FindControl("ListBox" + i.ToString());
                if (lst != null)
                {
                    lst.SelectedIndex = 0;
                }

                TextBox tBox = (TextBox)pnlControls.FindControl("TextBox" + i.ToString());
                if (tBox != null)
                    tBox.Text = "";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            ModalPopupExtender1.Show();
            if (lstLinesMoreToxicity.SelectedValue.ToString() != string.Empty && ddlToxicity.SelectedValue != string.Empty && ListBoxGradeMoreToxixcity.SelectedValue.ToString() != string.Empty)
            {
                lblError.Text = "";
                string StrPrefix = lstLinesMoreToxicity.SelectedValue.ToString().Substring(0, 8);
                string sql = string.Empty;
                string StrSufix = ddlToxicity.SelectedValue.ToString();
                try
                {
                    if (lstLinesMoreToxicity.SelectedValue.ToString().Trim() != "")
                    {
                        sql = "Update Patientdetails_2 set [" + StrPrefix + " " + StrSufix + "] = '" + ListBoxGradeMoreToxixcity.SelectedValue.ToString() + "'  where patient ='" + lngG_PatId + "'";
                        GlobalValues.ExecuteNonQuery(sql);
                    }
                    else
                    {
                        sql = "Update Patientdetails_2 set [" + StrPrefix + " " + StrSufix + "] = null where patient ='" + lngG_PatId + "'";
                        GlobalValues.ExecuteNonQuery(sql);
                    }
                }
                catch
                {
                    lblError.Text = "Column [" + StrPrefix + " " + StrSufix + "]  Not exists in the table PatientDetails_2";
                    return;
                }

                GlobalValues.SaveSessionLog(Session["PatientID"].ToString(), Session["Login"].ToString(), GlobalValues.SLogToxicity);
            }
            else
            {
                lblError.Text = "Fields marked with asterisk (*) are required.";
                return;
            }
        }

        protected void btnMoreToxixcity_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            ModalPopupExtender1.Show();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void lstLinesMoreToxicity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FillToxicity()
        {
            DataSet ds = GlobalValues.ExecuteDataSet(" Select '' as ToxicityName  union   select distinct ToxicityName from Toxizity  inner join PDfields   on Toxizity.CorrespondingPDField=RTRIM (LTRIM(SUBSTRING([Field Name],9,LEN([Field Name]))))  and PDfields.Toxizity=1 and Toxizity.CorrespondingPDField  is not null  where ToxicityName not in ('NEUTROPENIA','LEUKOCYTOPENIA','THROMBOCYTOPENIA','ANEMIA','FEBRILE NEUTROPENIA','NAUSEA AND VOMITING','DIARRHEA','ORAL MUCOSITIS','SKIN RASH','HYPERTENSION','RENAL FUNCTION','PROTEINURIA','CARDIAC LV FUNCTION','PULMONARY FUNCTION','HAND FOOT SYNDROME','PERIPHERAL NEUROPATHY')");
            if (ds.Tables.Count > 0)
            {
                ddlToxicity.DataTextField = "ToxicityName";
                ddlToxicity.DataValueField = "ToxicityName";
                ddlToxicity.DataSource = ds.Tables[0];
                ddlToxicity.DataBind();
            }
        }

        protected void ListBoxGradeMoreToxixcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            ModalPopupExtender1.Show();
            string StrSufix = ddlToxicity.SelectedValue.ToString();
            string sql = string.Empty;
            if (ListBoxGradeMoreToxixcity.SelectedValue.ToString().Trim() != "")
            {
                sql = "SELECT Toxizity.ToxDetail From Toxizity Where Toxizity.ToxicityName = '" + StrSufix + "' And Toxizity.ToxGrade = '" + (ListBoxGradeMoreToxixcity.SelectedValue.ToString()).ToString() + "'";
                string strVal = Convert.ToString(GlobalValues.ExecuteScalar(sql));
                if (strVal == null) { strVal = ""; }
                txtMoreToxicityValue.Text = strVal;
            }
            else
            {
                txtMoreToxicityValue.Text = "";
            }


        }
    }
}
