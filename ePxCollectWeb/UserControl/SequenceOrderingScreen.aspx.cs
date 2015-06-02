using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data;
using ePxCollectDataAccess;
using System.Collections;

namespace ePxCollectWeb.UserControl
{
    public partial class SequenceOrderingScreen : System.Web.UI.Page
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

                //string strSQL = string.Empty;
                //strSQL = " SELECT  [Diagnosis] as Diagnosis from Diagnosis ";

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
                    ListItem lstIJ = lstAvailable.Items.FindByText(strValue);

                    if (lstIJ == null)
                    {
                        lstVals.Add(strValue);
                        lstAvailable.Items.Add(strValue);
                    }
                }





                bool boollstitem = false;
                string strVal = Convert.ToString(Server.HtmlDecode(Request.QueryString["Val"]));
                lblValuePicked.Text = strVal;
                string ValueCSV = strVal.Replace("-->", ",");
                string[] SplitWord = ValueCSV.Split(',');
                if (strVal != null)
                {

                    if (SplitWord.Length > 0)
                    {
                        for (int k = 0; k < SplitWord.Length; k++)
                        {
                            string value = SplitWord[k];
                            ListItem itemsearch = lstAvailable.Items.FindByText(value);
                            if (itemsearch != null)
                            {
                                lstAvailable.Items.Remove(itemsearch);
                                lstAssigned.Items.Add(itemsearch);

                            }
                        }
                    }
                }

                lstAssigned.ClearSelection();

            }
        }

        protected void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            int j = lstAssigned.Items.Count - 1;
            for (int i = 0; i <= j; i++)
            {
                lstAvailable.Items.Add(lstAssigned.Items[0]);
                lstAssigned.Items.Remove(lstAssigned.Items[0]);
            }

            SortedList slSort = new SortedList();
            foreach (ListItem li in lstAvailable.Items)
            {
                slSort.Add(li.Text, li.Value);
            }

            lstAvailable.DataSource = slSort;
            lstAvailable.DataValueField = "value";
            lstAvailable.DataTextField = "key";
            lstAvailable.DataBind();


            for (int m = 0; m < lstAssigned.Items.Count; m++)
            {
                lstAssigned.Items[m].Selected = false;
            }
            for (int n = 0; n < lstAvailable.Items.Count; n++)
            {
                lstAvailable.Items[n].Selected = false;
            }
        }

        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            int j = lstAvailable.Items.Count - 1;
            for (int i = 0; i <= j; i++)
            {
                lstAssigned.Items.Add(lstAvailable.Items[0]);
                lstAvailable.Items.Remove(lstAvailable.Items[0]);
            }


            SortedList slSort = new SortedList();
            foreach (ListItem li in lstAssigned.Items)
            {
                slSort.Add(li.Text, li.Value);
            }
            lstAssigned.DataSource = slSort;
            lstAssigned.DataValueField = "value";
            lstAssigned.DataTextField = "key";
            lstAssigned.DataBind();
        }

        protected void btnUnSelect_Click(object sender, EventArgs e)
        {
            for (int nTemp = lstAssigned.Items.Count - 1; nTemp >= 0; nTemp--)
            {
                if (lstAssigned.Items[nTemp].Selected == true)
                {
                    lstAvailable.Items.Add(lstAssigned.Items[nTemp]);
                    ListItem liTemp = lstAssigned.Items[nTemp];
                    string result = string.Empty;
                    string Removedstring = lstAssigned.Items[nTemp].Text.ToString();
                    string Text = lblValuePicked.Text;
                    if (Text.Contains("-->" + Removedstring))
                    {
                        result = Text.Replace("-->" + Removedstring, "");
                    }
                    else if (Text.Contains(Removedstring + "-->"))
                    {
                        result = Text.Replace(Removedstring + "-->", "");
                    }
                    else
                    {
                        result = Text.Replace(Removedstring, "");
                    }

                    lstAssigned.Items.Remove(liTemp);
                    result.TrimEnd('-');
                    result.TrimEnd('>');
                    result.TrimStart('-');
                    result.TrimStart('>');

                    lblValuePicked.Text = result;
                }
            }

        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {


            for (int nTemp = lstAvailable.Items.Count - 1; nTemp >= 0; nTemp--)
            {
                if (lstAvailable.Items[nTemp].Selected == true)
                {
                    lstAssigned.Items.Add(lstAvailable.Items[nTemp]);
                    ListItem liTemp = lstAvailable.Items[nTemp];

                    if (lblValuePicked.Text == string.Empty)
                        lblValuePicked.Text = lstAvailable.Items[nTemp].Text.ToString();
                    else
                        lblValuePicked.Text = lblValuePicked.Text + "-->" + lstAvailable.Items[nTemp].Text.ToString();
                    lstAvailable.Items.Remove(liTemp);

                    lstAssigned.ClearSelection();
                    ListItem itemsearch = lstAssigned.Items.FindByText(liTemp.Text.ToString());
                    if (itemsearch != null)
                    {
                        int x = lstAssigned.Items.IndexOf(itemsearch);
                        lstAssigned.SelectedIndex = x;
                        lstAssigned.Items[lstAssigned.Items.IndexOf(itemsearch)].Selected = true;
                    }
                }
            }

        }


    }
}