using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ePxCollectWeb
{
    public partial class AnalysisResultFilter : System.Web.UI.Page
    {
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
            if (!IsPostBack)
            {
                List<string> lst = new List<string>();
                DataSet dsResult = (DataSet)Session["ds"];
                foreach (DataColumn dc in dsResult.Tables[0].Columns)
                {
                    lst.Add(dc.ColumnName);
                }
                dpColumns.DataSource = lst;
                dpColumns.DataBind();
            }
        }

        protected void dpColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dsResult = (DataSet)Session["ds"];
            DataTable dt = dsResult.Tables[0].DefaultView.ToTable(false, dpColumns.SelectedValue);
            grdCols.DataSource = dt;//dsResult.Tables[0].Columns[dpColumns.SelectedValue];
            grdCols.DataTextField= dt.Columns[0].ColumnName.ToString();
            grdCols.DataValueField = dt.Columns[0].ColumnName.ToString();
            grdCols.DataBind();
        }

        protected void grdCols_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtValue.Text = grdCols.Rows[grdCols.SelectedRow.RowIndex].Cells[1].Text;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            DataSet dsResult = (DataSet)Session["ds"];
            DataSet FilterSet = new DataSet();
            DataTable tbl = dsResult.Tables[0];
            tbl.DefaultView.RowFilter = dpColumns.SelectedValue.ToString() + "='" + txtValue.Text.ToString() + "'";
            tbl = tbl.DefaultView.ToTable();
            dsResult.Tables[0].Select();
            FilterSet.Tables.Add(tbl);
            FilterSet.Tables[0].TableName = "FilterTable";
            FilterSet.Tables.Add(dsResult.Tables[0].Copy());
            Session["ds"] = FilterSet;
            //Response.Redirect("AnalysisResult.aspx");
            ScriptManager.RegisterStartupScript(this, typeof(string), "", "window.close();", true);
        }

        

       

        protected void grdCols_SelectedIndexChanged2(object sender, EventArgs e)
        {
            txtValue.Text = grdCols.SelectedValue.ToString();
        }


    }
}