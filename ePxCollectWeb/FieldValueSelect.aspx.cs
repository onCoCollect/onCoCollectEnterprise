using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ePxCollectWeb
{
    public partial class FieldValueSelect : System.Web.UI.Page
    {
        string FldName = string.Empty;
        string patientId = string.Empty;
        string fldVal = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            FldName = Convert.ToString(Request.QueryString["FN"]).Trim();
            if (FldName.Split('.').Length > 0) { FldName = FldName.Split('.')[1].ToString(); }
            FldName = FldName.Replace("[", "").Replace("]", "");
            fldVal = Convert.ToString(Request.QueryString["Val"]).Trim().Replace("*ampersand*", "&").Replace("*plus*", "+");
            fldVal = fldVal.Replace("[", "").Replace("]", "");
            if (Session["PatientID"] == null)
            {
                Response.Redirect("SearchPatient.aspx");
            }
            patientId = Convert.ToString(Session["PatientID"]);
            var SiteOFPrimary = GlobalValues.ExecuteScalar("select SiteOfPrimary from PatientDetails_1 where Patient='" + patientId + "' ");

            this.Title = FldName + ":" + patientId;
            if (!IsPostBack)
            {
                BindDropDowns();
                if (fldVal != string.Empty)
                {
                    dpAllValues.SelectedItem.Text = "All Values";
                    dpAllValues_SelectedIndexChanged(sender, e);
                    if (fldVal != null)
                    {
                        //ListItem lstV = lstValues.Items.FindByText(strVal);
                        foreach (ListItem lstIt in lstValues.Items)
                        {
                            if (lstIt.Text.Trim().ToUpper() == fldVal.ToUpper())
                            {
                                string ItemText = lstIt.Text;
                                ListItem itemsearch = lstValues.Items.FindByText(ItemText);
                                if (itemsearch != null)
                                {
                                    int x = lstValues.Items.IndexOf(itemsearch);
                                    lstValues.SelectedIndex = x;
                                    lstValues.Items[lstValues.Items.IndexOf(itemsearch)].Selected = true;
                                    lblValuePicked.Text = lstValues.Items[lstValues.Items.IndexOf(itemsearch)].Text;

                                }

                                lstIt.Selected = true;
                                break;
                            }
                        }

                    }
                }


                if (Session["gMenuDropVal"] != null)
                {

                    if (Session["gMenuDropVal"].ToString().Length > 0)
                    {
                        dpByStudy.SelectedItem.Text = Session["gMenuDropVal"].ToString();// GlobalValues.gMenuDropVal.ToString();
                        ListItem lst = dpByStudy.Items.FindByValue(Session["gMenuDropVal"].ToString()); //dpByStudy.Items.FindByValue(GlobalValues.gMenuDropVal.ToString());
                        if (lst != null)
                            dpByStudy.SelectedValue = Session["gMenuDropVal"].ToString();// GlobalValues.gMenuDropVal.ToString();

                        else
                        {
                            LOadSiteofPrimary(Convert.ToString(SiteOFPrimary));
                        }
                        dpByStudy_SelectedIndexChanged(null, null);
                    }
                    else
                        if (SiteOFPrimary.ToString() != string.Empty)
                        {
                            LOadSiteofPrimary(Convert.ToString(SiteOFPrimary));
                        }

                }
                else
                    if (SiteOFPrimary.ToString() != string.Empty)
                    {
                      

                        if (fldVal != string.Empty)
                        {
                            dpAllValues.SelectedItem.Text = "All Values";
                            dpAllValues_SelectedIndexChanged(sender, e);
                            if (fldVal != null)
                            {
                                //ListItem lstV = lstValues.Items.FindByText(strVal);
                                foreach (ListItem lstIt in lstValues.Items)
                                {
                                    if (lstIt.Text.Trim().ToUpper() == fldVal.ToUpper())
                                    {
                                        string ItemText = lstIt.Text;
                                        ListItem itemsearch = lstValues.Items.FindByText(ItemText);
                                        if (itemsearch != null)
                                        {
                                            int x = lstValues.Items.IndexOf(itemsearch);
                                            lstValues.SelectedIndex = x;
                                            lstValues.Items[lstValues.Items.IndexOf(itemsearch)].Selected = true;
                                            lblValuePicked.Text = lstValues.Items[lstValues.Items.IndexOf(itemsearch)].Text;

                                        }

                                        lstIt.Selected = true;
                                        break;
                                    }
                                }

                            }
                        }

                        dpByDiag.SelectedItem.Text = SiteOFPrimary.ToString();
                        ListItem lst = dpByDiag.Items.FindByValue(SiteOFPrimary.ToString());
                        if (lst != null)
                        {
                            dpByDiag.SelectedValue = SiteOFPrimary.ToString();
                            dpAllValues.SelectedItem.Text = "";
                        }
                        dpByDiag_SelectedIndexChanged(null, null);


                        dpByDiag.SelectedItem.Text = SiteOFPrimary.ToString();
                        lst = dpByDiag.Items.FindByValue(SiteOFPrimary.ToString());
                        if (lst != null)
                            dpByDiag.SelectedValue = SiteOFPrimary.ToString();
                    }



            }

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

        private void LOadSiteofPrimary(string SiteOFPrimary)
        {
            if (SiteOFPrimary.ToString() != string.Empty)
            {
                dpByDiag.SelectedItem.Text = SiteOFPrimary.ToString();
                ListItem lst = dpByDiag.Items.FindByValue(SiteOFPrimary.ToString());
                if (lst != null)
                    dpByDiag.SelectedValue = SiteOFPrimary.ToString();
                dpByDiag_SelectedIndexChanged(null, null);

                bool bval = false;
                if (fldVal != string.Empty)
                {
                    //dpAllValues.SelectedItem.Text = "All Values";
                    //dpAllValues_SelectedIndexChanged(sender, e);
                    if (fldVal != null)
                    {
                        //ListItem lstV = lstValues.Items.FindByText(strVal);
                        foreach (ListItem lstIt in lstValues.Items)
                        {
                            if (lstIt.Text.Trim().ToUpper() == fldVal.ToUpper())
                            {
                                string ItemText = lstIt.Text;
                                ListItem itemsearch = lstValues.Items.FindByText(ItemText);
                                if (itemsearch != null)
                                {
                                    bval = true;
                                    int x = lstValues.Items.IndexOf(itemsearch);
                                    lstValues.SelectedIndex = x;
                                    lstValues.Items[lstValues.Items.IndexOf(itemsearch)].Selected = true;
                                    lblValuePicked.Text = lstValues.Items[lstValues.Items.IndexOf(itemsearch)].Text;


                                }

                                lstIt.Selected = true;
                                break;
                            }
                        }

                    }
                }
                if (fldVal != null)
                {

                    if (fldVal != string.Empty && bval == false)
                    {
                        dpAllValues.SelectedItem.Text = "All Values";
                        dpAllValues_SelectedIndexChanged(dpAllValues, EventArgs.Empty);
                        //ListItem lstV = lstValues.Items.FindByText(strVal);
                        foreach (ListItem lstIt in lstValues.Items)
                        {
                            if (lstIt.Text.Trim().ToUpper() == fldVal.ToUpper())
                            {
                                string ItemText = lstIt.Text;
                                ListItem itemsearch = lstValues.Items.FindByText(ItemText);
                                if (itemsearch != null)
                                {
                                    bval = true;
                                    int x = lstValues.Items.IndexOf(itemsearch);
                                    lstValues.SelectedIndex = x;
                                    lstValues.Items[lstValues.Items.IndexOf(itemsearch)].Selected = true;
                                    lblValuePicked.Text = lstValues.Items[lstValues.Items.IndexOf(itemsearch)].Text;


                                }

                                lstIt.Selected = true;
                                break;
                            }
                        }

                    }
                }
                dpByDiag.SelectedItem.Text = SiteOFPrimary.ToString();
                lst = dpByDiag.Items.FindByValue(SiteOFPrimary.ToString());
                if (lst != null)
                    dpByDiag.SelectedValue = SiteOFPrimary.ToString();
            }
        }
        private void PopulateValues(string strSQL)
        {
            DataSet ds = GlobalValues.ExecuteDataSet(strSQL);
            lstValues.Items.Clear();
            int selIndex = -1;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                string ColVal = dr[0].ToString();
                if (ColVal == "N/A") ColVal = "";
                string[] Cols = ColVal.Split(',');
                for (int I = 0; I < Cols.Length; I++)
                {

                    lstValues.Items.Add(Cols[I].ToString());
                    //lstValues.Attributes.Add("title", Cols[I].ToString());
                    if (Cols[I].ToString() == fldVal) { selIndex = I; }
                }
                if (selIndex >= 0) { lstValues.SelectedIndex = selIndex; }
            }

            foreach (ListItem item in lstValues.Items)
                item.Attributes["title"] = item.Text;
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
            //strSql = "Select StudyName from Studies where Active=1 and Instances like '%-" + GlobalValues.gInstanceID + "-%' and '" + Session["Login"].ToString() + "' in ( select a.HosCode from fn_Splithospitalcode(replace(StudyPatientsList,'''','')) as a )";

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
    }
}