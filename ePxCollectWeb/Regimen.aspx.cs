using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data.SqlClient;
using System.Data;

namespace ePxCollectWeb
{
    public partial class Regimen : System.Web.UI.Page
    {
        string strConn = string.Empty;
        DataTable dtlist = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }
            strConn = GlobalValues.strConnString;//System.Configuration.ConfigurationManager.ConnectionStrings["OncoCollectEnterprise"].ConnectionString;

            if (!IsPostBack)
            {
                string strQueryText = "select Diagnosis from Diagnosis";
                DataSet dsTest = SqlHelper.ExecuteDataset(strConn, CommandType.Text, strQueryText);
                lstsiteofPrimary.DataSource = dsTest;
                lstsiteofPrimary.DataTextField = dsTest.Tables[0].Columns[0].ColumnName;
                lstsiteofPrimary.DataValueField = dsTest.Tables[0].Columns[0].ColumnName;
                lstsiteofPrimary.DataBind();


                strQueryText = "select Groupname from Groupname";
                dsTest = SqlHelper.ExecuteDataset(strConn, CommandType.Text, strQueryText);
                lstGroups.DataSource = dsTest;
                lstGroups.DataTextField = dsTest.Tables[0].Columns[0].ColumnName;
                lstGroups.DataValueField = dsTest.Tables[0].Columns[0].ColumnName;
                lstGroups.DataBind();
                BindGroup();

                lstGroups.Enabled = true;
                btnSave.Text = "Save";
            }


            btnSave.Attributes.Add("onclick", "return ValidateRegimen('" + lstsiteofPrimary.ClientID + "','" + lstGroups.ClientID + "','" + lblError.ClientID + "')");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int nameCount = 0;
            string checkName = "Select count(*) from DrugGroupsByDiagnosis where DiagnosisName ='" + lstsiteofPrimary.SelectedValue.ToString() + "'";
            nameCount = (int)GlobalValues.ExecuteScalar(checkName);

            string strNewVal = string.Empty;
            for (int I = 0; I < lstGroups.Items.Count; I++)
            {
                if (lstGroups.Items[I].Selected == true)
                {
                    strNewVal += "," + lstGroups.Items[I].Text.ToString();
                }
            }
            if (nameCount <= 0)
            {
                string strSQL = "insert into  DrugGroupsByDiagnosis (DiagnosisName, DrugGroups) values ('" + lstsiteofPrimary.SelectedValue.ToString() + "','" + strNewVal + "')";
                SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('DrugGroup for the selected Diagnosis Created successfully.');", true);
                BindGroup();
                clear();

            }
            else
            {

                string strSQLupdate = "update  DrugGroupsByDiagnosis set DrugGroups='" + strNewVal + "' where DiagnosisName='" + ViewState["DiagnosisName"].ToString() + "'";
                SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQLupdate);
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('DrugGroup for the selected Diagnosis updated successfully.');", true);
                BindGroup();
                clear();

            }


        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            lstsiteofPrimary.ClearSelection();
            lstGroups.ClearSelection();
            lstGroups.Enabled = true;
            Response.Redirect("~/Regimen.aspx");
        }

        protected void btnClose_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/ProjectForm.aspx");
        }

        public void clear()
        {
            lblError.Text = string.Empty;
            lblError.Text = string.Empty;
            lstsiteofPrimary.ClearSelection();
            lstGroups.ClearSelection();
            lstGroups.Enabled = true;

        }

        protected void grdTestGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            if (Session["dtlist"] != null)
            {
                DataTable dt = (DataTable)Session["dtlist"];

                //grdAnalysisRes.DataSource = dsResult.Tables[0];
                //grdAnalysisRes.DataBind();
                //lblCaption.Text += " " + dsResult.Tables[0].Rows.Count.ToString();

                grdList.PageIndex = e.NewPageIndex;
                grdList.DataSource = dt;
                grdList.DataBind();

            }

        }

        protected void grdList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LinkButton lnkbtn = (LinkButton)e.CommandSource;
            GridViewRow myRow = (GridViewRow)lnkbtn.Parent.Parent;
            GridView myGrid = (GridView)sender;
            string SOP = ((Label)myRow.FindControl("lblDiagnosisName")).Text.Trim();
            ViewState["DiagnosisName"] = ((Label)myRow.FindControl("lblDiagnosisName")).Text.Trim();
            string strCSV = ((Label)myRow.FindControl("lblTestListCSV")).Text.Trim();

            for (int i = 0; i <= lstsiteofPrimary.Items.Count - 1; i++)
            {
                if (lstsiteofPrimary.Items[i].Text == SOP)
                    lstsiteofPrimary.Items[i].Selected = true;
            }
            if (e.CommandName.ToString() == "EditItem")
            {

                string[] strItems;
                strItems = strCSV.Split(',');

                foreach (string strValue in strItems)
                {
                    for (int i = 0; i <= lstGroups.Items.Count - 1; i++)
                    {
                        if (lstGroups.Items[i].Text == strValue)
                            lstGroups.Items[i].Selected = true;
                    }
                }

            }

            if (e.CommandName.ToString() == "DeleteItem")
            {
                if (Session["dtlist"] != null)
                    dtlist = (DataTable)Session["dtlist"];
                DataRow[] foundRows;
                foundRows = dtlist.Select("DiagnosisName= '" + grdList.DataKeys[myRow.RowIndex].Values[0].ToString() + "' ");
                if (foundRows.Length > 0)
                {
                    string strSQL = "delete from DrugGroupsByDiagnosis  where DiagnosisName= '" + grdList.DataKeys[myRow.RowIndex].Values[0].ToString() + "'";
                    SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
                    foundRows[0].Delete();
                    dtlist.AcceptChanges();
                }
                grdList.DataSource = dtlist.Copy();
                grdList.DataBind();

                Session["dtlist"] = dtlist.Copy();
                if (foundRows.Length > 0)
                {
                    lblError.Text = "The selected Item deleted successfully.";
                }
            }

        }

        private void BindGroup()
        {
            string strQueryText = "Select DiagnosisName as 'DiagnosisName', DrugGroups from DrugGroupsByDiagnosis";
            DataSet dsTest = SqlHelper.ExecuteDataset(strConn, CommandType.Text, strQueryText);
            grdList.DataSource = dsTest;
            grdList.DataBind();
            dtlist = dsTest.Tables[0].Copy();
            Session["dtlist"] = dsTest.Tables[0].Copy();
        }

    }
}