using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data;
using System.IO;

namespace ePxCollectWeb
{
    public partial class AnalysisResult : System.Web.UI.Page
    {
        string strConns;
        DataSet dsFilters = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExportExcel);
            btnSaveSearch.Visible = GlobalValues.isSaveQuery; //will be visible only if the user has permission to save queries
            if (!IsPostBack)
            {
                string strSQL = GlobalValues.QueryString;
                Session["ds"] = null;
                //ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                //scriptManager.RegisterPostBackControl(this.btnExportExcel);
                string strWhere = Convert.ToString(Session["Whr" + Session.SessionID.ToString()]); //Convert.ToString(Request.QueryString["Whr"]);
                if (strWhere.Length > 0) { strWhere = " where " + strWhere; }
                if (strSQL.Length > 0)
                {
                    #region ApplyAnalysisFilters

                    string strGetFilters = " select Criteria from  dbo.AnalysisAccessGroupMembers AGM " +
                                        " inner join AnalysisAccessGroups AG on AGM.CriteriaID=AG.CriteriaID and AGM.Login ='" + Convert.ToString(Session["Login"]).ToString() + "'";
                    DataSet dsAnalFilter = new DataSet();
                    dsAnalFilter = GlobalValues.ExecuteDataSet(strGetFilters);
                    string strGlobalFilters = string.Empty;
                    foreach (DataRow dr in dsAnalFilter.Tables[0].Rows)
                    {
                        if (strGlobalFilters.Length > 0) { strGlobalFilters += " Or "; }
                        strGlobalFilters += dr["Criteria"].ToString();
                    }

                    if (strWhere.Length > 0) { if (strGlobalFilters.Length > 0) { strWhere += " And (" + strGlobalFilters.ToString() + ")"; } }

                    #endregion


                    if (strSQL.IndexOf(GlobalValues.glbFromClause) <= 0)
                    {
                        strSQL += GlobalValues.glbFromClause + " " + strWhere;
                    }
                    strSQL = strSQL.Replace("$$", "'"); //added to handle the saved queries, where single quotes are replaced with $$.
                    strConns = GlobalValues.strConnString;
                    DataSet dsResult = new DataSet();

                    if (Session["ds"] == null)
                    {
                        dsResult = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strSQL);
                        Session["ds"] = dsResult;
                        txtValue.Text = "";
                    }
                    else
                    {
                        dsResult = (DataSet)Session["ds"];
                    }
                    //grdAnalysisRes.DataSource = dsResult.Tables[0];
                    //grdAnalysisRes.DataBind();
                    BindGrid(dsResult.Tables[0]);

                    List<string> lst = new List<string>();
                    lst.Add("Select");
                    //DataSet dsResult = (DataSet)Session["ds"];
                    foreach (DataColumn dc in dsResult.Tables[0].Columns)
                    {
                        lst.Add(dc.ColumnName);
                    }
                    dpColumns.DataSource = lst;
                    dpColumns.DataBind();
                    //lblCaption.Text = "Total No. Of Records: " + dsResult.Tables[0].Rows.Count.ToString();                   
                    CreateDS();
                }
            }

        }

        private void CreateDS()
        {
            DataTable dtFilter = new DataTable();
            DataColumn dcColumn = new DataColumn("DispValue");
            DataColumn dcOperator = new DataColumn("Operator");
            DataColumn dcValue = new DataColumn("FilterValue");
            dcColumn.DataType = System.Type.GetType("System.String");
            dcOperator.DataType = System.Type.GetType("System.String");
            dcValue.DataType = System.Type.GetType("System.String");
            dtFilter.Columns.Add(dcColumn);
            dtFilter.Columns.Add(dcOperator);
            dtFilter.Columns.Add(dcValue);
            dsFilters.Tables.Add(dtFilter);
            Session["dsFilter"] = dsFilters;
        }
        private void BindGrid(DataTable dt)
        {
            grdAnalysisRes.DataSource = dt;//dsResult.Tables[0];
            grdAnalysisRes.DataBind();
            lblCaption.Text = "Total No. Of Records: " + dt.Rows.Count.ToString();
            Session[Session.SessionID.ToString()] = dt;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["ds"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=AnalysisResult.xls");
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                // grdREsults.RenderControl(htw);
                GridView grdA = new GridView();
                grdA.DataSource = ds;
                grdA.DataBind();
                grdA.RenderBeginTag(htw);
                grdA.HeaderRow.RenderControl(htw);
                foreach (GridViewRow row in grdA.Rows)
                {
                    row.RenderControl(htw);
                }
                grdA.FooterRow.RenderControl(htw);
                grdA.RenderEndTag(htw);
                Response.Write(sw);
                Response.End();
            }

        }


        protected void grdAnalysisRes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Session["ds"] != null)
            {
                DataSet dsResult = (DataSet)Session["ds"];
                //grdAnalysisRes.DataSource = dsResult.Tables[0];
                //grdAnalysisRes.DataBind();
                //lblCaption.Text += " " + dsResult.Tables[0].Rows.Count.ToString();
                grdAnalysisRes.PageIndex = e.NewPageIndex;
                BindGrid(dsResult.Tables[0]);
                updREsult.Update();
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            DataSet dsResult = (DataSet)Session["ds"];
            DataSet FilterSet = new DataSet();
            //DataTable tbl = dsResult.Tables[0];
            DataTable tbl = dsResult.Tables[dsResult.Tables.Count - 1];
            string rowFilter = string.Empty;

            if (Session["dsFilter"] == null)
            {
                CreateDS();
            }
            else
            {
                dsFilters = (DataSet)Session["dsFilter"];
            }

            if (dpOperator.SelectedValue != "Contains") { txtValue.Text = dpValues.SelectedValue.ToString(); }
            if (txtValue.Text.Length > 0)
            {

                DataRow[] dr = dsFilters.Tables[0].Select("DispValue='" + dpColumns.SelectedValue.ToString() + "'");
                if (dr.Length > 0)
                {
                    foreach (DataRow drT in dsFilters.Tables[0].Rows)
                    {
                        if (drT["DispValue"].ToString() == dpColumns.SelectedValue.ToString())
                        {
                            drT["Operator"] += ";" + dpOperator.SelectedValue.ToString();
                            drT["FilterValue"] += ";" + txtValue.Text.ToString();
                        }
                    }
                }
                else
                {
                    DataRow drF = dsFilters.Tables[0].NewRow();
                    drF[0] = dpColumns.SelectedValue.ToString();
                    drF[1] = dpOperator.SelectedValue.ToString();
                    drF[2] = txtValue.Text.ToString();
                    dsFilters.Tables[0].Rows.Add(drF);
                }

                foreach (DataRow drFilter in dsFilters.Tables[0].Rows)
                {
                    string[] Ops;
                    string[] Vals;
                    Ops = drFilter["Operator"].ToString().Split(';');
                    Vals = drFilter["FilterValue"].ToString().Split(';');
                    string strFilter = string.Empty;
                    for (int i = 0; i <= Ops.Length - 1; i++)
                    {
                        if (strFilter.Length > 0) { strFilter += " OR "; }
                        if (Ops[i].ToString() == "Contains")
                        {
                            strFilter += "[" + drFilter["DispValue"].ToString() + "]" + " LIKE '%" + Vals[i].ToString() + "%'";
                        }
                        else
                        {
                            strFilter += "[" + drFilter["DispValue"].ToString().Replace("[", "").Replace("]", "") + "]" + Ops[i].ToString() + "'" + Vals[i].ToString() + "'";
                        }
                    }

                    if (strFilter.Length > 0)
                    {
                        rowFilter += strFilter;
                        tbl.DefaultView.RowFilter = strFilter;
                        tbl = tbl.DefaultView.ToTable();
                    }
                }
                if (rowFilter.Length > 0)
                {
                    FilterSet.Tables.Add(tbl);
                    FilterSet.Tables[0].TableName = "FilterTable";
                    FilterSet.Tables.Add(dsResult.Tables[dsResult.Tables.Count - 1].Copy());
                    Session["ds"] = FilterSet;
                    BindGrid(tbl);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Please select value for applying filter');", true);
            }
            //if (dpColumns.SelectedItem.Text != "Select")
            //{
            //    if (dpOperator.SelectedValue == "Contains")
            //    {
            //        //tbl.DefaultView.RowFilter = "["+dpColumns.SelectedValue.ToString()+"]" + " LIKE '%" + txtValue.Text.ToString() + "%'";// dpValues.SelectedValue.ToString() + "'";
            //        if (txtValue.Text.Length > 0)
            //        {
            //            rowFilter = "[" + dpColumns.SelectedValue.ToString() + "]" + " LIKE '%" + txtValue.Text.ToString() + "%'";// dpValues.SelectedValue.ToString() + "'";
            //        }
            //    }
            //    else
            //    {
            //        if (txtValue.Text.Length <= 0) { txtValue.Text = dpValues.SelectedValue.ToString(); }
            //        //tbl.DefaultView.RowFilter = "[" + dpColumns.SelectedValue.ToString() + "]"  +dpOperator.SelectedValue.ToString() + "'" + txtValue.Text.ToString() + "'";// dpValues.SelectedValue.ToString() + "'";
            //        if (txtValue.Text.Length > 0)
            //        {
            //            rowFilter = "[" + dpColumns.SelectedValue.ToString() + "]" + dpOperator.SelectedValue.ToString() + "'" + txtValue.Text.ToString() + "'";// dpValues.SelectedValue.ToString() + "'";
            //        }
            //    }
            //    if (rowFilter.Length > 0)
            //    {
            //        tbl.DefaultView.RowFilter = rowFilter;
            //        tbl = tbl.DefaultView.ToTable();
            //        FilterSet.Tables.Add(tbl);
            //        FilterSet.Tables[0].TableName = "FilterTable";
            //        FilterSet.Tables.Add(dsResult.Tables[dsResult.Tables.Count - 1].Copy());
            //        Session["ds"] = FilterSet;
            //        BindGrid(tbl);
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Please select value for applying filter');", true);
            //    }
            //}
        }
        protected void btnFilter_Click_old(object sender, EventArgs e)
        {

            DataSet dsResult = (DataSet)Session["ds"];
            DataSet FilterSet = new DataSet();
            DataTable tbl = dsResult.Tables[0];
            string rowFilter = string.Empty;
            if (Session["dsFilter"] == null)
            {
                CreateDS();
            }
            else
            {
                dsFilters = (DataSet)Session["dsFilter"];
            }

            if (dpColumns.SelectedItem.Text != "Select")
            {
                if (dpOperator.SelectedValue == "Contains")
                {
                    //tbl.DefaultView.RowFilter = "["+dpColumns.SelectedValue.ToString()+"]" + " LIKE '%" + txtValue.Text.ToString() + "%'";// dpValues.SelectedValue.ToString() + "'";
                    if (txtValue.Text.Length > 0)
                    {
                        rowFilter = "[" + dpColumns.SelectedValue.ToString() + "]" + " LIKE '%" + txtValue.Text.ToString() + "%'";// dpValues.SelectedValue.ToString() + "'";
                    }
                }
                else
                {
                    if (txtValue.Text.Length <= 0) { txtValue.Text = dpValues.SelectedValue.ToString(); }
                    //tbl.DefaultView.RowFilter = "[" + dpColumns.SelectedValue.ToString() + "]"  +dpOperator.SelectedValue.ToString() + "'" + txtValue.Text.ToString() + "'";// dpValues.SelectedValue.ToString() + "'";
                    if (txtValue.Text.Length > 0)
                    {
                        rowFilter = "[" + dpColumns.SelectedValue.ToString() + "]" + dpOperator.SelectedValue.ToString() + "'" + txtValue.Text.ToString() + "'";// dpValues.SelectedValue.ToString() + "'";
                    }
                }
                if (rowFilter.Length > 0)
                {
                    tbl.DefaultView.RowFilter = rowFilter;
                    tbl = tbl.DefaultView.ToTable();
                    FilterSet.Tables.Add(tbl);
                    FilterSet.Tables[0].TableName = "FilterTable";
                    FilterSet.Tables.Add(dsResult.Tables[dsResult.Tables.Count - 1].Copy());
                    Session["ds"] = FilterSet;
                    BindGrid(tbl);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Please select value for applying filter');", true);
                }
            }
        }

        protected void dpColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dpColumns.SelectedValue != "Select")
            {
                DataSet dsResult = (DataSet)Session["ds"];
                DataTable dt = dsResult.Tables[dsResult.Tables.Count - 1].DefaultView.ToTable(true, dpColumns.SelectedValue);
                dpValues.DataSource = dt;//dsResult.Tables[0].Columns[dpColumns.SelectedValue];
                dpValues.DataTextField = dt.Columns[0].ColumnName.ToString();
                dpValues.DataValueField = dt.Columns[0].ColumnName.ToString();
                dpValues.DataBind();
            }
            else
            {
                dpValues.DataSource = "";
                dpValues.DataBind();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            DataSet dsResult = (DataSet)Session["ds"];
            DataTable dt = dsResult.Tables[dsResult.Tables.Count - 1].Copy();
            DataSet dsN = new DataSet();
            dsN.Tables.Add(dt);
            Session["ds"] = dsN;
            txtValue.Text = "";
            CreateDS();
            dpValues.DataSource = "";
            dpValues.DataBind();
            dpColumns.SelectedValue = "Select";
            //grdAnalysisRes.DataSource = dt;
            //grdAnalysisRes.DataBind();
            BindGrid(dt);
        }

        protected void dpValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtValue.Text = dpValues.SelectedValue.ToString();
        }

        protected void dpOperator_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (dpOperator.SelectedValue == "Contains")
            {
                dpValues.Visible = false;
                txtValue.Visible = true;
                txtValue.Text = "";
            }
            else
            {
                dpValues.Visible = true;
                txtValue.Visible = false;
            }
            updREsult.Update();
        }

        protected void btnSaveSearch_Click(object sender, EventArgs e)
        {
            ////GlobalValues.QueryString + GlobalValues.glbFromClause + " " + Convert.ToString(Session["Whr" + Session.SessionID.ToString()]);
            //string strWhere = Convert.ToString(Session["Whr" + Session.SessionID.ToString()]);
            //if (strWhere == "1=1") { strWhere = ""; }
            //updConfirm.Update();
            //txtFilterText.Text = strWhere;
            //ModalPopupExtender1.Show();
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            //if (txtQueryName.Text.Length > 0)
            //{
            //    string queryText = string.Empty;
            //    string userName = Convert.ToString(Session["UserName"]);
            //    userName = userName.Substring(userName.LastIndexOf('(') + 1).Replace(")", "");
            //    queryText = GlobalValues.QueryString + GlobalValues.glbFromClause + " " + Convert.ToString(Session["Whr" + Session.SessionID.ToString()]);
            //    queryText = queryText.Replace("'", "$$");
            //    string sqlstrCheck = "Select count(*) from CustomQueries where Login='" + userName + "' and QueryName='" + txtQueryName.Text.ToString() + "'";
            //    int RecCount = (int)GlobalValues.ExecuteScalar(sqlstrCheck);
            //    if (RecCount > 0)
            //    {
            //        ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('You already have a query saved with this name. Please use a different name.');", true);
            //        ModalPopupExtender1.Show();
            //        updConfirm.Update();

            //    }
            //    else
            //    {
            //        string sqlstr = "Insert into CustomQueries ( Login, QueryName, QueryText, QueryDescription, ParentForm, DescriptionByUser, Filterable )"
            //            + " Values ('" + userName + "','" + txtQueryName.Text.ToString().Replace("'", "") +
            //            "', '" + queryText + "','" + txtFilterText.Text.ToString().Replace("'", "$$") + "','Analysis','" + txtFilterText.Text.ToString().Replace("'", "$$") + "',0)";
            //        try
            //        {
            //            GlobalValues.ExecuteNonQuery(sqlstr);
            //            ModalPopupExtender1.Hide();
            //            updConfirm.Update();
            //            updREsult.Update();
            //        }
            //        catch (Exception)
            //        {
            //            ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Error while saving your query.');", true);
            //            ModalPopupExtender1.Show();
            //        }
            //    }

            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please provide a name for the Query.');", true);
            //    ModalPopupExtender1.Show();
            //}
        }

        //protected void btnStats_Click(object sender, EventArgs e)
        //{
        //    string strPop = "window.open('Stats.aspx?Whr=" + Convert.ToString(Request.QueryString["Whr"]) + "', 'Statistrics', 'dialogWidth:550px; dialogHeight:400px; center:yes; scroll:no; status:no', true);";
        //   ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", strPop, true);
        //}


    }
}