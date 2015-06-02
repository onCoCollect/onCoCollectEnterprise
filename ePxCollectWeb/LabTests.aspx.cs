﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ePxCollectDataAccess;

namespace ePxCollectWeb
{
    public partial class LabTests : System.Web.UI.Page
    {
        string strConns = GlobalValues.strConnString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ResetPassword"] != null)
            {
                Session["ResetPasswordMsg"] = "Please Change your password.";
                Response.Redirect("Changepassword.aspx");
            }
            lblMsg.Text = "";//Code modified on April 18,2015-Subhashini
            btnSave.Visible = false;
            btnCancel.Visible = false;
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }

            string strQueryText = string.Empty;
            DataSet dsTest = new DataSet();
            if (Session["PatientID"] == null)
            {
                Session["Message"] = "Please pick a Patient.";
                Response.Redirect("SearchPatient.aspx");
            }
            string strPatientId = Convert.ToString(Session["PatientID"]);
            if (!IsPostBack)
            {
                BindTestGroup();
                BindDates();
                strQueryText = "Select TestName from TestIdGenerator";
                dsTest = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strQueryText);
                lstTests.DataSource = dsTest;
                lstTests.DataTextField = dsTest.Tables[0].Columns[0].ColumnName;
                lstTests.DataValueField = dsTest.Tables[0].Columns[0].ColumnName;
                lstTests.DataBind();
                //calDate.SelectedDate = DateTime.Now;
                //dtDate.Text = calDate.SelectedDate.ToString();
                //bindLabresults(dtDate.Text.ToString());
                /* Commented By Srinivas
                 * 
                dtDate.SelectedDate = DateTime.Now;
               
                bindLabresults(dtDate.SelectedDate.ToString("dd MMM yyyy"));
               // bindMeasureDetails();
                 * */

                //txtLabDate.Text = DateTime.Now.ToString();

                //bindLabresults(txtLabDate.Text);
            }
        }
        private void BindTestGroup()
        {
            string strQueryText = "Select 'Select' as GroupName,'0' as ID union Select TestGroup,'1' as ID  from TblTestGroups  where UserId='" + Session["Login"].ToString() + "' order by ID , GroupName";
            DataSet dsTest = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strQueryText);
            cboTestGroup.DataSource = dsTest;
            cboTestGroup.DataTextField = dsTest.Tables[0].Columns[0].ColumnName;
            cboTestGroup.DataValueField = dsTest.Tables[0].Columns[0].ColumnName;
            cboTestGroup.DataBind();

        }

        protected void grdTests_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //var measures = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string,string>("Select","Select"),
            //    new KeyValuePair<string,string>("Mg/M2","Mg/M2"),
            //    new KeyValuePair<string,string>("MMOIS","MMOIS"),
            //    new KeyValuePair<string,string>("Mcg","Mcg"),
            //};
            string strConn = GlobalValues.strConnString;
            SqlConnection dbConnection = new SqlConnection(strConn);
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
            SqlCommand sqlcommand = new SqlCommand("select * from tblMeasure", dbConnection);
            sqlcommand.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlcommand);
            sqlAdapter.Fill(ds);


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the DropDownList in the Row
                var drpMeasures = (e.Row.FindControl("ddlMeasure") as DropDownList);
                drpMeasures.DataSource = ds;
                drpMeasures.DataTextField = "name";
                drpMeasures.DataValueField = "id";
                drpMeasures.DataBind();

                string msr = (e.Row.FindControl("lblMeasure") as Label).Text;

                //msr = string.IsNullOrWhiteSpace(msr) ? "Select" : msr;

                var item = drpMeasures.Items.FindByText(msr);
                if (item != null)
                    item.Selected = true;
                else
                    drpMeasures.SelectedIndex = -1;


            }
        }
        public void bindMeasureDetails()
        {
            // string connection = System.Configuration.ConfigurationManager.ConnectionStrings["ePxConnString"].ConnectionString;


        }

        private void BindDates()
        {
            string strQueryText = string.Empty;
            DataSet dsTest = new DataSet();
            string strPatientId = Convert.ToString(Session["PatientID"]);
            strQueryText = "Select distinct [Date of Investigation] from PatientTestsByLine where Patientid = '" + strPatientId + "'";
            dsTest = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strQueryText);
            DataRow dr = dsTest.Tables[0].NewRow();
            dr[0] = "";
            dsTest.Tables[0].Rows.InsertAt(dr, 0);
            cboDates.DataSource = dsTest.Tables[0].DefaultView;
            cboDates.DataTextField = "Date of Investigation";
            cboDates.DataValueField = "Date of Investigation";
            //cboDates.Text = "";
            cboDates.DataBind();
        }
        private void bindLabresults(string dateT)
        {
            //if (ViewState["CurrentTable"] == null)
            //{
            //    FirstGridViewRow();
            //}  
            if (dateT == null || dateT == string.Empty)
            {
                lblMsg.ForeColor = GlobalValues.FailureColor;
                lblMsg.Text = "Please select the date.";//Code modified on April 15,2015-Subhashini
                return;
            }
            FirstGridViewRow();

            //DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            string PatientId = Convert.ToString(Session["PatientID"]);
            if (PatientId == "") { Response.Redirect("SearchPatient.aspx"); }
            string sqlText = string.Empty;
            sqlText = " Select PatientID, [Date of Investigation] DOI, Investigation, [Observed Value] ObVal, Measure From PatientTestsByLine Where [Date of Investigation] = '" + dateT + "' and patientid ='" + PatientId + "'";
            DataSet dsTest = SqlHelper.ExecuteDataset(strConns, CommandType.Text, sqlText);
            //grdTests.DataSource = dsTest;
            //grdTests.DataBind();
            DataTable dtVal = dsTest.Tables[0];
            DataTable dtCurrentTable = new DataTable();
            dtCurrentTable.Columns.AddRange(new DataColumn[] { new DataColumn("DOI"), new DataColumn("Invest"), new DataColumn("OBSVal"), new DataColumn("Measure") });
            foreach (DataRow drVals in dtVal.Rows)
            {
                drCurrentRow = dtCurrentTable.NewRow();
                //drCurrentRow["RowNumber"] = i + 1;

                drCurrentRow["DOI"] = drVals["DOI"].ToString();
                drCurrentRow["Invest"] = drVals["Investigation"].ToString();
                drCurrentRow["OBSVal"] = drVals["ObVal"].ToString();
                drCurrentRow["Measure"] = drVals["Measure"].ToString();
                dtCurrentTable.Rows.Add(drCurrentRow);
            }
            ViewState["CurrentTable"] = dtCurrentTable;
            //grdTests.DataSource = null;

            grdTests.DataSource = dtCurrentTable;
            grdTests.DataBind();
            SetPreviousData();
            RefreshLabTestList();
        }
        protected void dtDate_TextChanged(object sender, EventArgs e)
        {
            cboDates.Text = "";
            cboDates.SelectedValue = "";
            //bindLabresults(dtDate.Text.ToString());
            lblMsg.Text = "";
            //Code modified on March 23-2015,Subhashini
            string strQueryText = "Select TestName from TestIdGenerator";
            DataSet dsTest = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strQueryText);
            lstTests.DataSource = dsTest;
            lstTests.DataTextField = dsTest.Tables[0].Columns[0].ColumnName;
            lstTests.DataValueField = dsTest.Tables[0].Columns[0].ColumnName;
            lstTests.DataBind();
            //   bindLabresults(dtDate.SelectedDate.ToString("dd MMM yyyy"));
            bindLabresults(txtLabDate.Text);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            string PatientId = Convert.ToString(Session["PatientID"]);
            if (PatientId == "") { Response.Redirect("SearchPatient.aspx"); }
            string dateT = string.Empty;
            if (cboDates.SelectedValue != "")
            {
                dateT = cboDates.SelectedValue;
            }
            else
            {
                //dateT = dtDate.Text.ToString();
                // Commented Srinvas  dateT = dtDate.SelectedDate.ToString("dd MMM yyyy");
                dateT = txtLabDate.Text;

            }
            SetRowData();
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                //if (Validation(dt)) // Commented By Premiya [Validation to Check atleas enter a value for single field]
                {
                    //Select PatientID, [Date of Investigation] DOI, Investigation, [Observed Value] ObVal, Measure From PatientTestsByLine Where [Date of Investigation] = '" + dateT + "' and patientid ='" + PatientId + "'"
                    foreach (DataRow dr in dt.Rows)
                    {
                        string SqlStr = "Update PatientTestsByLine set [Observed Value] = '" + dr["OBSVal"].ToString() + "', Measure='" + dr["Measure"].ToString() + "'" +
                            " Where PatientID='" + PatientId + "' and [Date of Investigation] ='" + dr["DOI"].ToString() + "' and Investigation ='" + dr["Invest"].ToString() + "'";
                        try
                        {
                            GlobalValues.ExecuteNonQuery(SqlStr);
                        }
                        catch (Exception)
                        {
                            lblMsg.ForeColor = GlobalValues.FailureColor;
                            lblMsg.Text = "Failed to save the test details. Please try again.";//Code modified on April 15,2015-Subhashini
                            // ScriptManager.RegisterStartupScript(this, typeof(string), "Failure", "alert('Failed to save the test details. Please try later.');", true);
                        }

                    }
                    GlobalValues.SaveSessionLog(Session["PatientID"].ToString(), Session["Login"].ToString(), GlobalValues.SLogLabTest);

                    bindLabresults(dateT);
                    lblMsg.ForeColor = GlobalValues.SucessColor;
                    lblMsg.Text = "Tests saved successfully.";
                }
                //else
                //{
                //    lblMsg.ForeColor = GlobalValues.FailureColor;
                //    lblMsg.Text = "Please enter Observed value for atleast one test.";
                //}
            }
            RefreshLabTestList();
        }

        private void RefreshLabTestList()
        {
            for (int gridCount = 0; gridCount < grdTests.Rows.Count; gridCount++)
            {
                Label lbl = (Label)grdTests.Rows[gridCount].FindControl("lblInvestigation");
                ListItem li = new ListItem(lbl.Text, lbl.Text);
                if (lstTests.Items.FindByText(li.Text) != null)
                {
                    lstTests.Items.Remove(li);
                }

            }
        }

        private bool Validation(DataTable dt)
        {
            bool bval = true;

            DataRow[] foundRows = dt.Select("OBSVal<>''");
            {
                if (foundRows.Length >= 1)
                    bval = true;
                else
                    bval = false;
            }
            return bval;
        }

        protected void grdTests_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            btnSave.Visible = true;
            btnCancel.Visible = true;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt.Rows.Count >= 1)
                {
                    string dateT = string.Empty;
                    string PatientId = string.Empty;
                    string sqlText = string.Empty;
                    PatientId = Convert.ToString(Session["PatientID"]);
                    if (PatientId == "") { Response.Redirect("SearchPatient.aspx"); }
                    if (cboDates.SelectedValue != "")
                    {
                        dateT = cboDates.SelectedValue;
                    }
                    else
                    {
                        //dateT = dtDate.Text.ToString();
                        dateT = txtLabDate.Text;// Commented srinivas following line dtDate.SelectedDate.ToString("dd MMM yyyy");
                    }
                    sqlText = "delete from PatientTestsByLine where PatientID = '" + PatientId + "' and [Date of Investigation]= '" + dateT +
                        "' and [Investigation]= '" + dt.Rows[rowIndex]["Invest"].ToString() + "'";

                    GlobalValues.ExecuteNonQuery(sqlText);
                    ListItem li = new ListItem(dt.Rows[rowIndex]["Invest"].ToString(), dt.Rows[rowIndex]["Invest"].ToString());
                    if (lstTests.Items.FindByText(li.Text) == null)
                    {
                        lstTests.Items.Add(li);
                    }
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["CurrentTable"] = dt;
                    grdTests.DataSource = dt;


                    grdTests.DataBind();
                    BindDates();
                    lblMsg.ForeColor = GlobalValues.SucessColor;//Code modified on April 13,2015-Subhashini
                    lblMsg.Text = "Test deleted successfully";
                    //for (int i = 0; i < grdTests.Rows.Count - 1; i++)
                    //{
                    //    grdTests.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    //}
                    SetPreviousData();
                }
            }

            if (grdTests.Rows.Count <= 0)
            {
                btnSave.Visible = false;
                btnCancel.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnCancel.Visible = true;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            lblMsg.Text = "";
            if (txtLabDate.Text == string.Empty)//&& cboDates.SelectedValue == "")
            {
                lblMsg.ForeColor = GlobalValues.FailureColor;
                lblMsg.Text = "Please select the date.";//Code modified on April 15,2015-Subhashini
                return;
                //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please select the Date.');", true);
            }
            else if (lstTests.SelectedValue == "")
            {
                lblMsg.ForeColor = GlobalValues.FailureColor;
                lblMsg.Text = "Please select test options.";//Code modified on April 15,2015-Subhashini
                return;
                //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please select test options.');", true);
            }
            else
            {
                string dateT = string.Empty;
                string PatientId = string.Empty;
                string sqlText = string.Empty;
                lblMsg.Text = "";
                //if (cboDates.SelectedValue != "")
                //{
                //    dateT = cboDates.SelectedValue;
                //}
                //else
                //{
                //    //dateT = dtDate.Text.ToString();
                //    dateT = txtLabDate.Text;// Commented by srinivas dtDate.SelectedDate.ToString("dd MMM yyyy");
                //}
                //     DateTime? DateToAdd=new DateTime();
                //DateToAdd =null;
                //if (cboDates.SelectedValue != "")
                //{
                //    DateToAdd = Convert.ToDateTime(cboDates.SelectedValue);
                //}
                //Code modified on March 23-2015,Subhashini
                PatientId = Convert.ToString(Session["PatientID"]);
                if (PatientId == "") { Response.Redirect("SearchPatient.aspx"); }

                IEnumerable<string> CheckedItems = lstTests.Items.Cast<ListItem>()
                                       .Where(i => i.Selected)
                                       .Select(i => i.Value);

                if (grdTests.Rows.Count > 0)
                {
                    for (int i = 0; i < grdTests.Rows.Count; i++)
                    {
                        GridViewRow gvRow = (GridViewRow)grdTests.Rows[i];
                        Label lbls = (Label)(gvRow.Cells[1].FindControl("lblInvestigation"));
                        string str = lbls.Text.ToString();
                        string SQlSTR = "Select count(*) from PatientTestsByLine where PatientId='" + PatientId + "' and [Date of Investigation] ='" +
                        Convert.ToDateTime(txtLabDate.Text.ToString()).ToString("MMMM d, yyyy") + "' and Investigation='" + str + "'";
                        int IsExist = (int)GlobalValues.ExecuteScalar(SQlSTR);
                        if (IsExist == null) { IsExist = 0; }
                        if (IsExist <= 0)
                        {
                            sqlText = "Insert into PatientTestsByLine (PatientID, [Date of Investigation],[Investigation])" +
                      " Values ('" + PatientId + "','" + Convert.ToDateTime(txtLabDate.Text.ToString()).ToString("MMMM d, yyyy") + "','" + str + "')";
                            SqlHelper.ExecuteNonQuery(strConns, CommandType.Text, sqlText);
                        }

                        Label lbl = (Label)(gvRow.Cells[1].FindControl("lblDOI"));
                        lbl.Text = Convert.ToDateTime(txtLabDate.Text).ToString("MMMM d, yyyy");
                        // grdTests.UpdateRow(i, true);
                    }
                }
                //Code modified on March 23-2015,Subhashini
                foreach (string str in CheckedItems)
                {
                    btnSave.Enabled = true;
                    // Insert into PatientTestsByLine (PatientID, [Date of Investigation],[Investigation]) Values ('6-8906','December 30, 2014','Acetone-Blood')
                    string SQlSTR = "Select count(*) from PatientTestsByLine where PatientId='" + PatientId + "' and [Date of Investigation] ='" +
                       Convert.ToDateTime(txtLabDate.Text.ToString()).ToString("MMMM d, yyyy") + "' and Investigation='" + str + "'";
                    int IsExist = (int)GlobalValues.ExecuteScalar(SQlSTR);
                    if (IsExist == null) { IsExist = 0; }
                    if (IsExist <= 0)
                    {
                        sqlText = "Insert into PatientTestsByLine (PatientID, [Date of Investigation],[Investigation])" +
                             " Values ('" + PatientId + "','" + Convert.ToDateTime(txtLabDate.Text.ToString()).ToString("MMMM d, yyyy") + "','" + str + "')";
                        SqlHelper.ExecuteNonQuery(strConns, CommandType.Text, sqlText);
                    }
                }


                bindLabresults(Convert.ToDateTime(txtLabDate.Text).ToString("MMMM d, yyyy"));

                foreach (ListItem it in lstTests.Items)
                {
                    it.Selected = false;
                }
                SetRowData();
                btnSave_Click(null, null);
                //bindLabresults(dateT);
                BindDates();
                lblMsg.Text = "";
                btnSave.Visible = true;
                btnCancel.Visible = true;
                RefreshLabTestList();
                cboTestGroup.Text = "Select";//Code modified on April 18,2015-Subhashini
            }

            if (grdTests.Rows.Count > 0)
            {
                btnSave.Visible = true;
                btnCancel.Visible = true;
            }
        }

        private void SetRowData()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    btnSave.Enabled = true;
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        Label lblDOI = (Label)grdTests.Rows[rowIndex].Cells[0].FindControl("lblDOI");
                        Label lblInvestigation = (Label)grdTests.Rows[rowIndex].Cells[1].FindControl("lblInvestigation");
                        TextBox lblObsValue = (TextBox)grdTests.Rows[rowIndex].Cells[2].FindControl("lblObsValue");
                        TextBox TextBoxAddress = (TextBox)grdTests.Rows[rowIndex].Cells[3].FindControl("txtMeasure");
                        DropDownList measureText = (DropDownList)grdTests.Rows[rowIndex].Cells[3].FindControl("ddlMeasure");
                        // drCurrentRow = dtCurrentTable.NewRow();
                        //drCurrentRow["RowNumber"] = i + 1;
                        dtCurrentTable.Rows[i - 1]["DOI"] = lblDOI.Text;
                        dtCurrentTable.Rows[i - 1]["Invest"] = lblInvestigation.Text;
                        dtCurrentTable.Rows[i - 1]["OBSVal"] = lblObsValue.Text;
                        dtCurrentTable.Rows[i - 1]["Measure"] = measureText.SelectedValue;
                        rowIndex++;
                    }

                    ViewState["CurrentTable"] = dtCurrentTable;
                    //grdTests.DataSource = dtCurrentTable;
                    //grdTests.DataBind();
                }
                else
                {
                    btnSave.Enabled = false;
                }
            }
            else
            {
                // Response.Write(" ");
                btnSave.Enabled = false;
            }
        }


        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    btnSave.Enabled = true;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Label lblDOI = (Label)grdTests.Rows[rowIndex].Cells[0].FindControl("lblDOI");
                        Label lblInvestigation = (Label)grdTests.Rows[rowIndex].Cells[1].FindControl("lblInvestigation");
                        TextBox lblObsValue = (TextBox)grdTests.Rows[rowIndex].Cells[2].FindControl("lblObsValue");
                        // TextBox TextBoxAddress = (TextBox)grdTests.Rows[rowIndex].Cells[3].FindControl("txtMeasure");
                        DropDownList measureText = (DropDownList)grdTests.Rows[rowIndex].Cells[3].FindControl("ddlMeasure");

                        // drCurrentRow["RowNumber"] = i + 1;

                        //grdTests.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                        lblDOI.Text = dt.Rows[i]["DOI"].ToString();
                        lblInvestigation.Text = dt.Rows[i]["Invest"].ToString();
                        lblObsValue.Text = dt.Rows[i]["OBSVal"].ToString();
                        measureText.Text = dt.Rows[i]["Measure"].ToString();
                        //  TextBoxAddress.Text = dt.Rows[i]["Measure"].ToString();
                        rowIndex++;
                    }
                }
                else
                {
                    btnSave.Enabled = false;
                }
            }
            else
            {
                btnSave.Enabled = false;
            }

        }

        //private void AddNewRow()
        //{
        //    int rowIndex = 0;
        //    if (ViewState["CurrentTable"] == null)
        //    {
        //        FirstGridViewRow();
        //    }
        //    if (ViewState["CurrentTable"] != null)
        //    {
        //        DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
        //        DataRow drCurrentRow = null;
        //        if (dtCurrentTable.Rows.Count > 0)
        //        {
        //            for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
        //            {
        //                Label lblDOI = (Label)grdTests.Rows[rowIndex].Cells[0].FindControl("lblDOI");
        //                Label lblInvestigation = (Label)grdTests.Rows[rowIndex].Cells[1].FindControl("lblInvestigation");
        //                TextBox lblObsValue = (TextBox)grdTests.Rows[rowIndex].Cells[2].FindControl("lblObsValue");
        //                TextBox TextBoxAddress = (TextBox)grdTests.Rows[rowIndex].Cells[3].FindControl("txtMeasure");
        //                drCurrentRow = dtCurrentTable.NewRow();
        //                //drCurrentRow["RowNumber"] = i + 1;

        //                dtCurrentTable.Rows[i - 1]["DOI"] = lblDOI.Text;
        //                dtCurrentTable.Rows[i - 1]["Invest"] = lblInvestigation.Text;
        //                dtCurrentTable.Rows[i - 1]["OBSVal"] = lblObsValue.Text;
        //                dtCurrentTable.Rows[i - 1]["Measure"] = TextBoxAddress.Text;
        //                rowIndex++;
        //            }
        //            dtCurrentTable.Rows.Add(drCurrentRow);
        //            ViewState["CurrentTable"] = dtCurrentTable;

        //            grdTests.DataSource = dtCurrentTable;
        //            grdTests.DataBind();

        //            TextBox txn = (TextBox)grdTests.Rows[rowIndex].Cells[1].FindControl("txtMeasure");
        //            txn.Focus();
        //            // txn.Focus;
        //        }
        //    }
        //    else
        //    {
        //        //Response.Write("ViewState is null");
        //    }
        //    SetPreviousData();
        //}

        private void FirstGridViewRow()
        {
            DataTable dt = new DataTable();
            //DataRow dr = null;

            dt.Columns.Add(new DataColumn("DOI", typeof(string)));
            dt.Columns.Add(new DataColumn("Invest", typeof(string)));
            dt.Columns.Add(new DataColumn("OBSVal", typeof(string)));
            dt.Columns.Add(new DataColumn("Measure", typeof(string)));
            //dr = dt.NewRow();
            ////dr["RowNumber"] = 1;
            //dr["DOI"] = string.Empty;
            //dr["Invest"] = string.Empty;
            //dr["OBSVal"] = string.Empty;
            //dr["Measure"] = string.Empty;
            ////dr["Col5"] = string.Empty;
            //dt.Rows.Add(dr);

            ViewState["CurrentTable"] = dt;
        }

        #region OldCodes
        protected void grdTests_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //string dt = string.Empty;
            //if (cboDates.SelectedValue != "")
            //{
            //    dt = cboDates.SelectedValue;
            //}
            //else
            //{
            //    dt = dtDate.Text.ToString();
            //}

            //grdTests.EditIndex = -1;
            //bindLabresults(dt);
            //updGrid.Update();
        }

        protected void grdTests_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //string dt = string.Empty;
            //if (cboDates.SelectedValue != "")
            //{
            //    dt = cboDates.SelectedValue;
            //}
            //else
            //{
            //    dt = dtDate.Text.ToString();
            //}

            //grdTests.EditIndex = e.NewEditIndex;
            //bindLabresults(dt);
            ////updGrid.Update();
        }

        protected void SaveChanges(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            lblMsg.Text = "";
            SetRowData();
            SetPreviousData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            grdTests.DataSource = null;
            grdTests.DataBind();
            Response.Redirect("ProjectForm.aspx");
        }

        protected void cboDates_SelectedIndexChanged1(object sender, EventArgs e)
        {
            btnSave.Visible = true;
            btnCancel.Visible = true;
            if (cboDates.Text != "")
            {
                string dateT = Convert.ToDateTime(cboDates.SelectedValue.ToString()).ToString("MMMM d, yyyy");
                txtLabDate.Text = cboDates.SelectedItem.Text.ToString();//Code modified on April 18,2015-Subhashini
                cboTestGroup.Text = "Select";
                bindLabresults(dateT);
                // dtDate.SelectedDate = Convert.ToDateTime(cboDates.SelectedItem.Text.ToString()); //commented srinivas

                //for (int i = 0; i <= lstTests.Items.Count - 1; i++)
                //{
                //    lstTests.Items[i].Selected = false;
                //}
            }

        }

        protected void cboTestGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboTestGroup.SelectedItem.Text != "Select")
            {
                getTestsForGroupname();//Code modified on April 18,2015-Subhashini
            }
            else
            {
                lstTests.ClearSelection();
            }


        }

        protected void getTestsForGroupname()//Code modified on April 18,2015-Subhashini
        {
            lstTests.ClearSelection();
            for (int i = 0; i <= lstTests.Items.Count - 1; i++)
            {
                lstTests.Items[i].Selected = false;
            }
            string strSQL = "Select TestListCSV from TblTestGroups where TestGroup='" + cboTestGroup.SelectedItem.Value + "'";
            List<string> lstVals = new List<string>();
            string strCSVs = (string)GlobalValues.ExecuteScalar(strSQL);
            string[] strItems;
            strItems = strCSVs.Split(',');
            foreach (string strValue in strItems)
            {
                if (strValue.Length > 0)
                {
                    for (int i = 0; i <= lstTests.Items.Count - 1; i++)
                    {
                        if (lstTests.Items[i].Text == strValue)
                            lstTests.Items[i].Selected = true;
                    }
                }
            }
        }

        #endregion
        //Code modified on March 23-2015,Subhashini
        protected void txtLabDate_TextChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            bool isFlag = false;//Code Modified on April 15(Evening),2015-Subhashini
            foreach (ListItem item in cboDates.Items)
            {

                if (txtLabDate.Text.ToString().Trim() == item.Text.ToString().Trim())
                {
                    cboDates.Text = item.Text;
                    bindLabresults(cboDates.Text.ToString());
                    isFlag = true;
                    break;
                }
            }
            if (cboTestGroup.SelectedItem.Text != "Select")//Code Modified on April 18,2015-Subhashini
            {
                isFlag = true;
                return;
            }


            if (lstTests.SelectedItem != null)
            {
                if (lstTests.SelectedItem.Selected == true)
                {
                    isFlag = true;
                    return;//Code Modified on April 20,2015-Subhashini
                }
            }
            if (isFlag == false)
            {
                cboDates.Text = "";
                grdTests.DataSource = null;
                grdTests.DataBind();
                string strQueryText = "Select TestName from TestIdGenerator";
                DataSet dsTest = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strQueryText);
                lstTests.DataSource = null;
                lstTests.DataBind();
                lstTests.DataSource = dsTest;
                lstTests.DataTextField = dsTest.Tables[0].Columns[0].ColumnName;
                lstTests.DataValueField = dsTest.Tables[0].Columns[0].ColumnName;
                lstTests.DataBind();
                BindTestGroup();//Code modified on April 15,2015-Subhashini
                for (int i = 0; i <= lstTests.Items.Count - 1; i++)
                {
                    lstTests.Items[i].Selected = false;
                }
                bindLabresults(txtLabDate.Text.ToString());//Code Modified on April 15(Evening),2015-Subhashini
            }
            //grdTests.DataSource = null;
            //grdTests.DataBind();//Code modified on April 15,2015-Subhashini
            //string strQueryText = "Select TestName from TestIdGenerator";
            //DataSet dsTest = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strQueryText);
            //lstTests.DataSource = null;
            //lstTests.DataBind();
            //lstTests.DataSource = dsTest;
            //lstTests.DataTextField = dsTest.Tables[0].Columns[0].ColumnName;
            //lstTests.DataValueField = dsTest.Tables[0].Columns[0].ColumnName;
            //lstTests.DataBind();
            //for (int i = 0; i <= lstTests.Items.Count - 1; i++)
            //{
            //    lstTests.Items[i].Selected = false;
            //}
            //cboDates.Text = txtLabDate.Text.ToString();
        }

        protected void lstTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
        }
    }
}
