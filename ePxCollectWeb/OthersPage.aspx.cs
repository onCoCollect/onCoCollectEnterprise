using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ePxCollectDataAccess;
using System.Collections;

namespace ePxCollectWeb
{
    public partial class OthersPage : System.Web.UI.Page
    {
        string strConns = GlobalValues.strConnString;
        protected void Page_Load(object sender, EventArgs e)
        {
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
                Response.Redirect("SearchPatient.aspx");
            }
            if (!IsPostBack)
            {
                string strSQL = string.Empty;
                DataSet dsCols = new DataSet();
                //strSQL = "SELECT PDFields.[Field Name] as FieldName, PDFields.[Table Name]+ '.['+ PDFields.[Field Name]+']' as ColValue From PDFields WHERE (((PDFields.ConfigurableInScreens)=1) AND ((PDFields.DATATYPE)<>'MEMO') AND ((PDFields.ISActive)=1)) OR (((PDFields.[CalCulated Field])=1) AND ((PDFields.DATATYPE)<>'MEMO') AND ((PDFields.ISActive)=1)) ORDER BY PDFields.FieldOrder";
                strSQL = "SELECT PDFields.[Field Name] as FieldName, PDFields.[Table Name]+ '.['+ PDFields.[Field Name]+']' as ColValue From PDFields WHERE (PDFields.ConfigurableInScreens=1  AND PDFields.ISActive=1)   and [Field Name]  not in  ('Consultants','HospitalCode')   Order by FieldOrder";

                dsCols = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strSQL);

                dsCols = GetUnUsedColumns(dsCols);

                lstStudy.DataSource = "";
                // lstStudy.DataBind();
                if (dsCols.Tables.Count > 0)
                {

                    if (dsCols.Tables[0].Rows.Count > 0)
                    {

                        lstStudy.DataSource = dsCols;
                        lstStudy.DataTextField = dsCols.Tables[0].Columns[0].ColumnName;
                        lstStudy.DataValueField = dsCols.Tables[0].Columns[1].ColumnName;
                        lstStudy.DataBind();
                        string strCols = string.Empty;
                        if (Session["SQLSTROTHER"] != null)
                        {
                            strCols = Session["SQLSTROTHER"].ToString().Replace("Select", "").Replace(" ", ""); ;
                            ArrayList values = StringToArrayList(strCols);
                            foreach (ListItem li in lstStudy.Items)
                            {
                                if (values.Contains(li.Value.Replace(" ", "")))
                                    li.Selected = true;
                                else
                                    li.Selected = false;
                            }

                        }
                    }
                }
            }

        }

        private DataSet GetUnUsedColumns(DataSet dsCols)
        {
            DataSet ds = new DataSet();
            DataTable dataTable = ds.Tables.Add();
            dataTable.Columns.Add("ColName");
            dsCols.Tables[0].Columns.Add("DispFlag");
            DataTable dtCols = dsCols.Tables[0];
            string SQLstr = "select distinct replace(replace(FieldListCSV,']',''),'[','') FieldListCSV from PDScreenMaster_Preset  " +
                        " union select distinct replace(replace(FieldListCSV,']',''),'[','') FieldListCSV from PDScreenMaster_Diagnosis  where DiseaseName = '" + GlobalValues.gDisease + "' ";
            //" union select distinct replace(replace(FieldListCSV,']',''),'[','') FieldListCSV from PDScreenMaster_Custom ";
            DataSet dsFilter = GlobalValues.ExecuteDataSet(SQLstr);

            foreach (DataRow dr in dsFilter.Tables[0].Rows)
            {
                string[] array = dr[0].ToString().Split(',');

                Array.ForEach(array, c => dataTable.Rows.Add()[0] = c);

            }
            foreach (DataRow drN in dtCols.Rows)
            {
                ds.Tables[0].DefaultView.RowFilter = "ColName='" + drN[0].ToString() + "'";
                if (ds.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                {
                    drN["DispFlag"] = "N";
                }
                else
                {
                    drN["DispFlag"] = "Y";
                }
            }
            dtCols.DefaultView.RowFilter = "DispFlag='Y'";
            dtCols = dtCols.DefaultView.ToTable();
            dsCols.Tables.RemoveAt(0);
            dsCols.Tables.Add(dtCols);
            return dsCols;
        }
        private ArrayList StringToArrayList(string value)
        {
            ArrayList _al = new ArrayList();
            string[] _s = value.Split(new char[] { ',' });

            foreach (string item in _s)
                _al.Add(item);

            return _al;
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProjectForm.aspx");
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            string strQuery = string.Empty;
            lblError.Visible = false;
            IEnumerable<string> CheckedItems = lstStudy.Items.Cast<ListItem>()
                                   .Where(i => i.Selected)
                                   .Select(i => i.Text);

            foreach (string str in CheckedItems)
                strQuery += "," + str.Trim();
            if (strQuery.Length > 0)
            {
                strQuery = strQuery.Substring(1);
                strQuery = "Select [" + strQuery.Replace(",", "],[") + "]";
                Session["SQLSTROTHER"] = strQuery;
                Session["UpdSQL"] = strQuery;
                Response.Redirect("ProjectForm.aspx?SN=OTHER&Type=Preset");
            }
            else
            {
                lblError.Text = "Please select the fields.";
                lblError.Visible = true;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lstStudy.ClearSelection();
            lblError.Text = "";
        }


    }
}