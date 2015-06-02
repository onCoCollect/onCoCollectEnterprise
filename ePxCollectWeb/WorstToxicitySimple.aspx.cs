using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ePxCollectWeb
{
    public partial class WorstToxicitySimple : System.Web.UI.Page
    {


        string lngG_PatId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }
            if (Session["PatientID"] == null)
            {
                Response.Redirect("SearchPatient.aspx");
            }
            lngG_PatId = Convert.ToString(Session["PatientID"]);
            //GlobalValues.gPostBackURL = Request.Url.AbsoluteUri.ToString();
            Session["gPostBackURL"] = Request.Url.AbsoluteUri.ToString();
            if (!IsPostBack)
            {
                lstLines.Items.Clear();
                lstLines.Items.Add("1st Line Worst Toxicity");
                lstLines.Items.Add("2nd Line Worst Toxicity");
                lstLines.Items.Add("3rd Line Worst Toxicity");
                lstLines.Items.Add("4th Line Worst Toxicity");
                lstLines.Items.Add("5th Line Worst Toxicity");
                foreach (Control ctl in pnlControls.Controls)
                {
                    if (ctl.ID != null)
                    {
                        if (ctl.ID.ToString().Contains("ListBox"))
                        {
                            ListBox lst = (ListBox)ctl;
                            lst.Items.Add(" ");
                            lst.Items.Add("Grade 0");
                            lst.Items.Add("Grade 1");
                            lst.Items.Add("Grade 2");
                            lst.Items.Add("Grade 3");
                            lst.Items.Add("Grade 4");
                            lst.Items.Add("Grade 5");
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
            ListBox lst = (ListBox)sender;
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
                    if (lst.SelectedValue.ToString() != "")
                    {
                        sql = "Update Patientdetails_2 set [" + StrPrefix + " " + StrSufix + "] = '" + lst.SelectedIndex.ToString() + "'  where patient ='" + lngG_PatId + "'";
                        GlobalValues.ExecuteNonQuery(sql);
                        sql = "SELECT Toxizity.ToxDetail From Toxizity with (nolock) Where Toxizity.ToxicityName = '" + StrSufix + "' And Toxizity.ToxGrade = '" + lst.SelectedIndex.ToString() + "'";
                        string strValue= Convert.ToString(GlobalValues.ExecuteScalar(sql));
                        if (strValue == null) { strValue = ""; }
                        tBox.Text = strValue;
                    }
                    else
                    {
                        sql = "Update Patientdetails_2 set [" + StrPrefix + " " + StrSufix + "] = null    where patient ='" + lngG_PatId + "'";
                        GlobalValues.ExecuteNonQuery(sql);
                    }
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
                    if (retVal.ToString() != "") { EnablePnl = true; }
                }
                else
                    if (lstLines.SelectedValue == "3rd Line Worst Toxicity")
                    {
                        sqlStr = " Select  [1st RecurrenceDate]  + [2nd RecurrenceDate]  from Recurrences where Patientid='" + lngG_PatId + "'";
                        retVal = GlobalValues.ExecuteScalar(sqlStr);
                        if (retVal == null) { retVal = ""; }
                        if (retVal.ToString() != "") { EnablePnl = true; }
                    }
                    else
                        if (lstLines.SelectedValue == "4th Line Worst Toxicity")
                        {
                            sqlStr = " Select [1st RecurrenceDate]  + [2nd RecurrenceDate]+[3rd RecurrenceDate]  from Recurrences where Patientid='" + lngG_PatId + "'";
                            retVal = GlobalValues.ExecuteScalar(sqlStr);
                            if (retVal == null) { retVal = ""; }

                            if (retVal.ToString() != "") { EnablePnl = true; }
                        }
                        else
                            if (lstLines.SelectedValue == "5th Line Worst Toxicity")
                            {
                                sqlStr = " Select [1st RecurrenceDate]  + [2nd RecurrenceDate]+[3rd RecurrenceDate] +[4th RecurrenceDate]  from Recurrences where Patientid='" + lngG_PatId + "'";
                                retVal = GlobalValues.ExecuteScalar(sqlStr);
                                if (retVal == null) { retVal = ""; }
                                if (retVal.ToString() != "") { EnablePnl = true; }
                            }

            if (EnablePnl)
            {
                pnlControls.Enabled = true;
                String StrPrefix = lstLines.SelectedValue.ToString().Substring(0, 8);
                for (int i = 1; i <= 16; i++)
                {
                    string StrSufix = getSufix(i.ToString());
                    string strVal = (string)GlobalValues.ExecuteScalar("Select isnull([" + StrPrefix + " " + StrSufix + "],'') from Patientdetails_2 with (nolock) where Patient ='" + lngG_PatId + "'");
                    if (strVal == null) { strVal = ""; }
                    if (strVal != "")
                    {
                        ListBox lst = (ListBox)pnlControls.FindControl("ListBox" + i.ToString());
                        if (lst != null)
                        {
                            lst.Items[Convert.ToInt32(strVal)].Selected = true;
                            ListBoxChanged(lst, null);
                        }
                    }
                    else
                    {
                        TextBox tBox = (TextBox)pnlControls.FindControl("TextBox" + i.ToString());
                        tBox.Text = "";
                    }
                }

            }
            else
            {
                pnlControls.Enabled = false;
                ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Previous Recurrence Dates are required');", true);
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchPatient.aspx");
        }
    }
}