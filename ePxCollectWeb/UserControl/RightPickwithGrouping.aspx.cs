using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data;
using ePxCollectDataAccess;

namespace ePxCollectWeb.UserControl
{
    public partial class RightPickwithGrouping : System.Web.UI.Page
    {
        string strConns = GlobalValues.strConnString;
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["PatientID"] == null)
            {
                Response.Redirect("SearchPatient.aspx");
            }
            if (!IsPostBack)
            {
                string strColumn = Request.QueryString["tName"].ToString();
                string strTable = strColumn.Substring(0, strColumn.IndexOf("."));
                strColumn = strColumn.Substring(strColumn.IndexOf(".") + 1);
                this.Title = strColumn;
                bool ShowEditBox = (bool)GlobalValues.ExecuteScalar("Select distinct isnull(RightSinglePickFixed,'') RightSinglePickFixed  from PDFields where [Field Name] ='" + strColumn.Replace("_-", " ").Replace("[", "").Replace("]", "") + "'");
                if (ShowEditBox == null) { ShowEditBox = false; }


                //string strSQL = "SELECT top 1 FieldValues from PDFields where [Field Name] ='" + strColumn.Replace("[", "").Replace("]", "") + "'";

                //List<string> lstVals = new List<string>();
                //string strCSVs = string.Empty;
                //try
                //{
                //    strCSVs = (string)GlobalValues.ExecuteScalar(strSQL);
                //    if (strCSVs == null) { strCSVs = ""; }
                //}
                //catch (Exception)
                //{

                //    strCSVs = string.Empty;
                //}

                //string[] strItems;
                //strItems = strCSVs.Split(',');
                //foreach (string strValue in strItems)
                //{
                //    ListItem lstIJ = lstAvailable.Items.FindByText(strValue);

                //    if (lstIJ == null)
                //    {
                //        lstVals.Add(strValue);
                //        lstAvailable.Items.Add(strValue);
                //    }
                //}


                string strSQL = string.Empty;
                strSQL = "Select 'All' as DiagnosisGrouping union SELECT distinct [Diagnosis Grouping] as DiagnosisGrouping from Diagnosis   order by DiagnosisGrouping";
                DataSet ds = GlobalValues.ExecuteDataSet(strSQL);

                ddlGroup.DataSource = ds.Tables[0];
                ddlGroup.DataTextField = "DiagnosisGrouping";
                ddlGroup.DataValueField = "DiagnosisGrouping";
                ddlGroup.DataBind();
                ddlGroup.SelectedValue = "All";
                List<string> lstVals = new List<string>();
                if (ddlGroup.SelectedValue == "All")
                    strSQL = "SELECT  Diagnosis from Diagnosis order by Diagnosis";

                else
                    strSQL = "Select '' as Diagnosis union SELECT  Diagnosis from Diagnosis where [Diagnosis Grouping] ='" + ddlGroup.SelectedValue.Replace("[", "").Replace("]", "") + "'   order by Diagnosis";
                ds = GlobalValues.ExecuteDataSet(strSQL);
                for (int count = 0; count < ds.Tables[0].Rows.Count; count++)
                {
                    ListItem lstIJ = ddlSiteOfPrimary.Items.FindByText(ds.Tables[0].Rows[count]["Diagnosis"].ToString());
                    if (lstIJ == null)
                    {
                        //lstVals.Add(ds.Tables[0].Rows[count]["Diagnosis"].ToString());
                        //lstValues.Items.Add(ds.Tables[0].Rows[count]["Diagnosis"].ToString());
                        ddlSiteOfPrimary.Items.Add(new ListItem(ds.Tables[0].Rows[count]["Diagnosis"].ToString().Trim(), ds.Tables[0].Rows[count]["Diagnosis"].ToString().Trim()));
                    }
                }


                //List<string> lstVals = new List<string>();
                //string strCSVs = string.Empty;
                //try
                //{
                //    strCSVs = (string)GlobalValues.ExecuteScalar(strSQL);
                //    if (strCSVs == null) { strCSVs = ""; }
                //}
                //catch (Exception)
                //{

                //    strCSVs = string.Empty;
                //}

                //string[] strItems;
                //strItems = strCSVs.Split(',');
                //foreach (string strValue in strItems)
                //{
                //    ListItem lstIJ = lstValues.Items.FindByText(strValue);

                //    if (lstIJ == null)
                //    {
                //        lstVals.Add(strValue);
                //        lstValues.Items.Add(strValue);
                //    }
                //}
                strSQL = "SELECT DISTINCT isnull(" + strColumn + ",'') " + strColumn + " FROM " + strTable + " Order by " + strColumn;
                DataSet dsBind = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strSQL);
                foreach (DataRow dr in dsBind.Tables[0].Rows)
                {
                    string[] Vals = dr[0].ToString().Split(',');
                    for (int i = 0; i <= Vals.Length - 1; i++)
                    {
                        //if(lstVals.Find(item => item.Contains(Vals[i])) )
                        ListItem lstI = ddlSiteOfPrimary.Items.FindByText(Vals[i].Trim());

                        if (lstI == null)
                        {
                            ddlSiteOfPrimary.Items.Add(new ListItem(Vals[i], Vals[i]));
                        }
                    }
                }

                bool boollstitem = false;
                string strVal = Convert.ToString((Request.QueryString["Val"])).Replace("*ampersand*", "&").Replace("*plus*", "+"); ;
                if (strVal != null)
                {
                    //ListItem lstV = lstValues.Items.FindByText(strVal);
                    foreach (ListItem lstIt in ddlSiteOfPrimary.Items)
                    {
                        if (lstIt.Text.Trim().ToUpper() == strVal.ToUpper())
                        {
                            string ItemText = lstIt.Text;
                            ListItem itemsearch = ddlSiteOfPrimary.Items.FindByText(ItemText);
                            if (itemsearch != null)
                            {
                                int x = ddlSiteOfPrimary.Items.IndexOf(itemsearch);
                                ddlSiteOfPrimary.SelectedIndex = x;
                                ddlSiteOfPrimary.Items[ddlSiteOfPrimary.Items.IndexOf(itemsearch)].Selected = true;
                                boollstitem = true;
                                lblPreviouslypicked.Text = strVal.ToString();
                            }

                            lstIt.Selected = true;
                            break;
                        }
                    }

                }

            }
        }

        protected void lstValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnSelectedValue.Value = ddlSiteOfPrimary.SelectedValue;
            //for (int i = 0; i < lstValues.Items.Count - 1; i++ )
            //{
            //    if (lstValues.Items[i].Selected == true)
            //    {
            //        txtValue.Text += lstValues.SelectedValue.ToString();
            //    }
            //}
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSQL = string.Empty;
            if (ddlGroup.SelectedValue == "All")
                strSQL = "SELECT  Diagnosis from Diagnosis order by Diagnosis";

            else
                strSQL = "Select '-' as Diagnosis union  SELECT  Diagnosis from Diagnosis where [Diagnosis Grouping] ='" + ddlGroup.SelectedValue.Replace("[", "").Replace("]", "") + "'  order by Diagnosis ";
            DataSet ds = GlobalValues.ExecuteDataSet(strSQL);
            ddlSiteOfPrimary.Items.Clear();
            for (int count = 0; count < ds.Tables[0].Rows.Count; count++)
            {
                ListItem lstIJ = ddlSiteOfPrimary.Items.FindByText(ds.Tables[0].Rows[count]["Diagnosis"].ToString());
                if (lstIJ == null)
                {
                    //lstVals.Add(ds.Tables[0].Rows[count]["Diagnosis"].ToString());
                    ddlSiteOfPrimary.Items.Add(new ListItem(ds.Tables[0].Rows[count]["Diagnosis"].ToString().Trim(), ds.Tables[0].Rows[count]["Diagnosis"].ToString().Trim()));
                }
            }
        }



    }
}