﻿using System;
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
    public partial class PopupMultiSelect : System.Web.UI.Page
    {
        string strConns = GlobalValues.strConnString;
        protected void Page_Init(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "NewFormChange", "getNewFormType();", true);
            if (PopUpClicks.Value == "True")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

           if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "NewFormChange", "getNewFormType();", true);
                if (PopUpClicks.Value == "True")
                {
                    Response.Redirect("~/Login.aspx");
                    return;

                }
                else
                {
                    entryDiv.Visible = true;

                    if (Session["PatientID"] == null)
                    {

                        Response.Redirect("~/SearchPatient.aspx");
                    }
                    if (Request.QueryString["tName"] != null)
                    {
                        string strColumn = Request.QueryString["tName"].ToString();
                        string strTable = strColumn.Substring(0, strColumn.IndexOf("."));
                        strColumn = strColumn.Substring(strColumn.IndexOf(".") + 1);
                        this.Title = strColumn;
                        bool ShowEditBox = (bool)GlobalValues.ExecuteScalar("Select distinct isnull(RightSinglePickFixed,'') RightSinglePickFixed  from PDFields where [Field Name] ='" + strColumn.Replace("_-", " ").Replace("[", "").Replace("]", "") + "'");
                        if (ShowEditBox == null) { ShowEditBox = false; }
                        if (ShowEditBox)
                        {
                            Label1text.Visible = false;
                            txtValue.Visible = false;
                        }

                        //string strSQL = "SELECT DISTINCT isnull(" + strColumn + ",'') " + strColumn+" FROM " + strTable + " Order by " + strColumn;
                        //DataSet dsBind = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strSQL);
                        //lstValues.DataSource = dsBind;
                        //lstValues.DataTextField = strColumn.Replace("[", "").Replace("]", "");
                        //lstValues.DataBind();

                        string strSQL = "SELECT top 1 FieldValues from PDFields where [Field Name] ='" + strColumn.Replace("[", "").Replace("]", "") + "'";

                        List<string> lstVals = new List<string>();
                        string strCSVs = string.Empty;
                        try
                        {
                            strCSVs = (string)GlobalValues.ExecuteScalar(strSQL);
                            if (strCSVs == null) { strCSVs = ""; }
                        }
                        catch (Exception)
                        {

                            strCSVs = string.Empty;
                        }

                        string[] strItems;
                        strItems = strCSVs.Split(',');
                        foreach (string strValue in strItems)
                        {
                            ListItem lstIJ = lstValues.Items.FindByText(strValue);

                            if (lstIJ == null)
                            {
                                lstVals.Add(strValue);
                                lstValues.Items.Add(strValue);
                            }
                        }
                        strSQL = "SELECT DISTINCT isnull(" + strColumn + ",'') " + strColumn + " FROM " + strTable + " Order by " + strColumn;
                        DataSet dsBind = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strSQL);
                        foreach (DataRow dr in dsBind.Tables[0].Rows)
                        {
                            string[] Vals = dr[0].ToString().Split(',');
                            for (int i = 0; i <= Vals.Length - 1; i++)
                            {

                                //ListItem lstI = lstValues.Items.FindByText(Vals[i]);

                                //if (lstI == null)
                                //{
                                //    lstVals.Add(Vals[i]);
                                //    lstValues.Items.Add(Vals[i]);

                                //}


                                ListItem lstI = lstValues.Items.FindByText(dr[0].ToString());
                                if (lstI == null)
                                {
                                    lstVals.Add(dr[0].ToString());
                                    lstValues.Items.Add(dr[0].ToString());

                                }
                            }
                        }

                        bool boollstitem = false;
                        string strVal = Convert.ToString((Request.QueryString["Val"])).Replace("*ampersand*", "&").Replace("*plus*", "+");
                        if (strVal != null)
                        {
                            //ListItem lstV = lstValues.Items.FindByText(strVal);
                            foreach (ListItem lstIt in lstValues.Items)
                            {
                                if (lstIt.Text.Trim().ToUpper() == strVal.ToUpper())
                                {
                                    string ItemText = lstIt.Text;
                                    ListItem itemsearch = lstValues.Items.FindByText(ItemText);
                                    if (itemsearch != null)
                                    {
                                        int x = lstValues.Items.IndexOf(itemsearch);
                                        lstValues.SelectedIndex = x;
                                        lstValues.Items[lstValues.Items.IndexOf(itemsearch)].Selected = true;
                                        boollstitem = true;
                                        txtValue.Text = strVal.ToString();
                                        //lstValues.SelectedItem.Text = strVal.ToString();
                                    }

                                    lstIt.Selected = true;
                                    break;
                                }
                            }
                            if (boollstitem == false)
                            {
                                txtValue.Text = strVal.ToString();
                            }
                        }

                    }
                }
            }
        }

        protected void lstValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnSelectedValue.Value = lstValues.SelectedValue;
            //for (int i = 0; i < lstValues.Items.Count - 1; i++ )
            //{
            //    if (lstValues.Items[i].Selected == true)
            //    {
            //        txtValue.Text += lstValues.SelectedValue.ToString();
            //    }
            //}
        }




    }
}