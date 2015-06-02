using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data;


namespace ePxCollectWeb.UserControl
{
    public partial class StatRightPick : System.Web.UI.Page
    {
       
            string strConns = GlobalValues.strConnString;
        protected void Page_Load(object sender, EventArgs e)
        {

          

            //if (Session["PatientID"] == null)
            //{
            //    Response.Redirect("SearchPatient.aspx");
            //}
            if (!IsPostBack)
            {
                string strColumn = Session["selectedString"].ToString();// Request.QueryString["tName"].ToString();
               //string strTable = strColumn.Substring(0, strColumn.IndexOf(","));
               //strColumn = strColumn.Substring(strColumn.IndexOf(",") + 1);
               // this.Title = strColumn;

                string strSQL = "SELECT FieldValues from PDFields where [Field Name] ='" + strColumn  + "' ";


                List<string> lstVals = new List<string>();
                string strCSVs = (string)GlobalValues.ExecuteScalar(strSQL);
                string[] strItems;
                strItems = strCSVs.Split(',');
                foreach (string strValue in strItems)
                {
                    if (strValue.Length > 0)
                    {
                        ListItem lstIJ = lstValues.Items.FindByText(strValue);

                        if (lstIJ == null)
                        {
                            lstVals.Add(strValue);
                            lstValues.Items.Add(strValue);
                        }
                    }
                }
                //strSQL = "SELECT DISTINCT isnull(" + strColumn + ",'') " + strColumn + " FROM " + strTable;
                DataSet dsBind = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strSQL);
                foreach(DataRow dr in dsBind.Tables[0].Rows)
                {
                    string[] Vals=dr[0].ToString().Split(',');
                    for (int i=0; i<=Vals.Length-1; i++)
                    {
                        //if(lstVals.Find(item => item.Contains(Vals[i])) )
                        if (Vals[i].Length > 0)
                        {
                            ListItem lstI = lstValues.Items.FindByText(Vals[i]);

                            if (lstI == null)
                            {
                                lstVals.Add(Vals[i]);
                                lstValues.Items.Add(Vals[i]);

                            }
                        }
                    }
                }
                string strVal = Convert.ToString(Request.QueryString["Val"]);
                if (strVal != null)
                {
                    string[] strVals = strVal.Split(',');
                    for (int pos = 0; pos <= strVals.Length - 1; pos++)
                    {
                        ListItem lstV = lstValues.Items.FindByText(strVals[pos]);

                        if (lstV != null)
                        {
                           lstValues.Items[lstValues.Items.IndexOf(lstV)].Selected = true;

                        }
                    }
                }
                //lstValues.DataSource=lstVals;
                ////lstValues.DataSource = dsBind;
                //lstValues.DataTextField = strColumn.Replace("[", "").Replace("]", "");
                //lstValues.DataBind();
            }
        }
        }
    }
