﻿using System;
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
    public partial class TestGroup : System.Web.UI.Page
    {
        string strConn = string.Empty;
        DataTable dtTestGroup = new DataTable();
        static bool flagGridUpdate = false;

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
            strConn = GlobalValues.strConnString;//System.Configuration.ConfigurationManager.ConnectionStrings["OncoCollectEnterprise"].ConnectionString;

            if (!IsPostBack)
            {

                string strQueryText = "Select TestName from TestIdGenerator";
                DataSet dsTest = SqlHelper.ExecuteDataset(strConn, CommandType.Text, strQueryText);
                lstTests.DataSource = dsTest;
                lstTests.DataTextField = dsTest.Tables[0].Columns[0].ColumnName;
                lstTests.DataValueField = dsTest.Tables[0].Columns[0].ColumnName;
                lstTests.DataBind();
                BindTestGroup();
                btnSave.Text = "Save";
            }

            if (dtTestGroup.Columns.Count <= 0)
            {
                DataColumn dcName = new DataColumn("TestGroup");
                dcName.DataType = System.Type.GetType("System.String");

                DataColumn dcCSV = new DataColumn("TestListCSV");
                dcCSV.DataType = System.Type.GetType("System.String");

                dtTestGroup.Columns.Add(dcName);
                dtTestGroup.Columns.Add(dcCSV);
            }
            btnSave.Attributes.Add("onclick", "return ValidateTestGroup('" + txtGroupName.ClientID + "','" + lstTests.ClientID + "','" + lblError.ClientID + "')");
        }
        protected void btnSave_Click(object sender, EventArgs e)//Code modified on April 16-2015,Subhashini
        {
            int nameCount = 0;
            // int TestCount = 0;
            string TestName = "";
            string strNewVal = string.Empty;
            string userId = string.Empty;
            if (Session["Login"] != null)
            {
                userId = Session["Login"].ToString();
            }
            string currentDateTime = GetDateString();
            for (int I = 0; I < lstTests.Items.Count; I++)
            {
                if (lstTests.Items[I].Selected == true)
                {
                    strNewVal += "," + lstTests.Items[I].Text.ToString();
                }
            }
            string StringNewVal = strNewVal.TrimEnd(',').TrimStart(',');
            string checkName = string.Empty;
            int valCount = 0;
            bool DoesExists = false;
            string testGroupName = "";
            string[] NewVals = new string[100];
            string[] testnames = new string[100];
            bool stringsAreEqual = false;
            if (btnSave.Text == "Save" || btnSave.Text == "Update")
            {
                if (btnSave.Text == "Save" || ((btnSave.Text == "Update") && txtGroupName.Text.Trim().ToString() != ViewState["TestGroupName"].ToString()))
                {
                    checkName = "Select count(*) from TblTestGroups where TestGroup ='" + txtGroupName.Text.Trim().ToString() + "' and UserId='" + Session["Login"].ToString() + "' ";
                    nameCount = (int)GlobalValues.ExecuteScalar(checkName);

                }

                if (btnSave.Text == "Save")
                    checkName = "Select TestGroup,TestListCSV from TblTestGroups where UserId='" + Session["Login"].ToString() + "'";
                else
                    checkName = "Select TestGroup,TestListCSV from TblTestGroups where UserId='" + Session["Login"].ToString() + "' and TestGroup <> '" + ViewState["TestGroupName"].ToString() + "'   ";


                DataSet ds = GlobalValues.ExecuteDataSet(checkName);
                NewVals = StringNewVal.Split(',');

                //Im[plemented Premiya
                foreach (DataRow drTableRows in ds.Tables[0].Rows)
                {
                    testGroupName = drTableRows["TestGroup"].ToString();
                    TestName = drTableRows["TestListCSV"].ToString().TrimEnd(',').TrimStart(',');


                    //var set1 = new HashSet<string>(TestName.Split(',').Select(t => t.Trim()));
                    //bool setsEqual = set1.SetEquals(StringNewVal.Split(',').Select(t => t.Trim()));


                    var firstOrdered = TestName.Split(',')
                              .Select(t => t.Trim())
                              .OrderBy(t => t);
                    var secondOrdered = StringNewVal.Split(',')
                                                    .Select(t => t.Trim())
                                                    .OrderBy(t => t);
                    stringsAreEqual = firstOrdered.SequenceEqual(secondOrdered);
                    if (stringsAreEqual == true)
                        break;
                }

            }
            if (nameCount > 0)
            {
                //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Test Group name already Exists. Please enter differnt Test GroupName.');", true);
                lblError.ForeColor = GlobalValues.FailureColor;
                lblError.Text = "Test Group name already Exists. Please enter differnt Test GroupName.";
                txtGroupName.Text = "";
            }


            if (stringsAreEqual)
            {
                //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('The selected Test List already Exists in Test Name " + TestName + ". Please Select differnt Test from List.');", true);
                //txtGroupName.Text = "";
                lblError.ForeColor = GlobalValues.FailureColor;
                //lblError.Text = "The selected Test List already Exists in the Test Name '" + TestName + "'. Please Select different Test from the List.";
                lblError.Text = "The selected Test List already Exists in the Test Name '" + testGroupName + "'.";//Code modified on April 16-2015,Subhashini
            }
            if (nameCount <= 0 && stringsAreEqual == false)
            {
                if (btnSave.Text == "Save")
                {
                    string strSQL = "insert into  TblTestGroups (TestGroup, TestListCSV,UserID,CreatedDate,CreatedBy) values ('" + txtGroupName.Text.Trim() + "','" + strNewVal + "','" + userId + "','" + currentDateTime + "','" + userId + "')";
                    SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Test Group Created successfully.');", true);
                    lblError.ForeColor = GlobalValues.SucessColor;//Code modified on March 31-2015,Subhashini
                    lblError.Text = "Test Group Created successfully.";
                    grdTestGroup.PageIndex = 0;
                    BindTestGroup();
                }
                else
                {
                    string strSQLupdate = "update  TblTestGroups set TestGroup='" + txtGroupName.Text.Trim() + "' ,TestListCSV='" + strNewVal + "',ModifiedDate= '" + currentDateTime + "' ,LastModifiedBy = '" + userId + "'" +
                        " where TestGroup='" + ViewState["TestGroupName"].ToString() + "' and UserID='" + userId + "'";
                    SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQLupdate);
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Test Group updated successfully.');", true);
                    lblError.ForeColor = GlobalValues.SucessColor;//Code modified on March 31-2015,Subhashini
                    lblError.Text = "Test Group updated successfully.";
                    btnSave.Text = "Save";
                    grdTestGroup.PageIndex = 0;
                    BindTestGroupUpdate();
                }

                // clear();
                txtGroupName.Text = "";
                lstTests.ClearSelection();


            }
        }//Code modified on April 16-2015,Subhashini
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    int nameCount = 0;
        //    int TestCount = 0;
        //    string TestName = "";
        //    string strNewVal = string.Empty;
        //    for (int I = 0; I < lstTests.Items.Count; I++)
        //    {
        //        if (lstTests.Items[I].Selected == true)
        //        {
        //            strNewVal += "," + lstTests.Items[I].Text.ToString();
        //        }
        //    }
        //    if (btnSave.Text == "Save" || ((btnSave.Text == "Update") && txtGroupName.Text.Trim().ToString() != ViewState["TestGroupName"].ToString()))
        //    {
        //        string checkName = "Select count(*) from TblTestGroups where TestGroup ='" + txtGroupName.Text.Trim().ToString() + "'";
        //        nameCount = (int)GlobalValues.ExecuteScalar(checkName);


        //        checkName = "Select TestGroup ,isnull(Count(*),0) as TestCount from TblTestGroups where TestListCSV ='" + strNewVal + "'  group by TestGroup ";
        //        DataSet ds = GlobalValues.ExecuteDataSet(checkName);
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            TestCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TestCount"].ToString());
        //            TestName = (ds.Tables[0].Rows[0]["TestGroup"].ToString());
        //        }

        //    }
        //    if (nameCount > 0)
        //    {
        //        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Test Group name already Exists. Please enter differnt Test GroupName.');", true);
        //        lblError.ForeColor = GlobalValues.FailureColor;
        //        lblError.Text = "Test Group name already Exists. Please enter differnt Test GroupName.";
        //        txtGroupName.Text = "";
        //    }


        //    if (TestCount > 0)
        //    {
        //        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('The selected Test List already Exists in Test Name " + TestName + ". Please Select differnt Test from List.');", true);
        //        //txtGroupName.Text = "";
        //        lblError.ForeColor = GlobalValues.FailureColor;
        //        lblError.Text = "The selected Test List already Exists in the Test Name '" + TestName + "'. Please Select different Test from the List.";
        //    }
        //    if (nameCount <= 0 && TestCount <= 0)
        //    {


        //        if (btnSave.Text == "Save")
        //        {
        //            string strSQL = "insert into  TblTestGroups (TestGroup, TestListCSV,UserID) values ('" + txtGroupName.Text.Trim() + "','" + strNewVal + "','" + Session["Login"].ToString() + "')";
        //            SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
        //            //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Test Group Created successfully.');", true);
        //            lblError.ForeColor = GlobalValues.SucessColor;//Code modified on March 31-2015,Subhashini
        //            lblError.Text = "Test Group Created successfully.";
        //        }
        //        else
        //        {
        //            string strSQLupdate = "update  TblTestGroups set TestGroup='" + txtGroupName.Text.Trim() + "' ,TestListCSV='" + strNewVal + "' where TestGroup='" + ViewState["TestGroupName"].ToString() + "' and UserID='" + Session["Login"].ToString() + "' ";
        //            SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQLupdate);
        //            //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Test Group updated successfully.');", true);
        //            lblError.ForeColor = GlobalValues.SucessColor;//Code modified on March 31-2015,Subhashini
        //            lblError.Text = "Test Group updated successfully.";
        //            btnSave.Text = "Save";
        //        }
        //        BindTestGroup();
        //       // clear();
        //        txtGroupName.Text = "";
        //        lstTests.ClearSelection();

        //    }


        //}

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            txtGroupName.Text = "";
            lstTests.ClearSelection();
            Response.Redirect("~/TestGroup.aspx");
        }

        protected void btnClose_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/ProjectForm.aspx");
        }

        public void clear()
        {
            lblError.Text = string.Empty;
            txtGroupName.Text = "";
            lstTests.ClearSelection();

        }
        public static String GetDateString()
        {
            string dateTimeString = string.Empty;
            DateTime currentDateTime = DateTime.Now;
            string DateTimeFormat = "MM-dd-yyyy HH:mm:ss";
            dateTimeString = DateTime.Now.ToString(DateTimeFormat);
            return dateTimeString;
        }
        protected void grdTestGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTestGroup.PageIndex = e.NewPageIndex;
            if (flagGridUpdate == true)
            {
                BindTestGroupUpdate();
                flagGridUpdate = false;
            }
            else
            {
                BindTestGroup();
            }
            grdTestGroup.SelectedIndex = -1;

            //if (Session["dtTestGroup"] != null)
            //{
            //    DataTable dt = (DataTable)Session["dtTestGroup"];
            //    //grdAnalysisRes.DataSource = dsResult.Tables[0];
            //    //grdAnalysisRes.DataBind();
            //    //lblCaption.Text += " " + dsResult.Tables[0].Rows.Count.ToString();
            //    grdTestGroup.PageIndex = e.NewPageIndex;
            //    grdTestGroup.DataSource = dt;
            //    grdTestGroup.DataBind();

            //}


        }

        protected void grdTestGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //ImageButton lnkbtn = (ImageButton)e.CommandSource;
            //GridViewRow myRow = (GridViewRow)lnkbtn.Parent.Parent;
            //GridView myGrid = (GridView)sender;
            if (e.CommandName.ToString() == "EditItem")
            {
                lblError.Text = string.Empty;
                ImageButton lnkbtn = (ImageButton)e.CommandSource;
                GridViewRow myRow = (GridViewRow)lnkbtn.Parent.Parent;
                GridView myGrid = (GridView)sender;
                for (int i = 0; i <= lstTests.Items.Count - 1; i++)
                {
                    lstTests.Items[i].Selected = false;
                }
                btnSave.Text = "Update";
                txtGroupName.Text = ((Label)myRow.FindControl("lblTestGroupName")).Text.Trim();
                ViewState["TestGroupName"] = ((Label)myRow.FindControl("lblTestGroupName")).Text.Trim();
                string strCSV = ((Label)myRow.FindControl("lblTestListCSV")).Text.Trim();
                string[] strItems;
                strItems = strCSV.Split(',');
                foreach (string strValue in strItems)
                {
                    //ListItem lstIJ = lstTests.Items.FindByText(strValue);
                    //int x = lstTests.Items.IndexOf(lstIJ);
                    //lstTests.SelectedIndex = x;
                    //lstTests.Items[x].Selected = true;

                    for (int i = 0; i <= lstTests.Items.Count - 1; i++)
                    {
                        if (lstTests.Items[i].Text == strValue)
                            lstTests.Items[i].Selected = true;
                    }

                }
            }
            if (e.CommandName.ToString() == "DeleteItem")
            {
                ImageButton lnkbtn = (ImageButton)e.CommandSource;
                GridViewRow myRow = (GridViewRow)lnkbtn.Parent.Parent;
                GridView myGrid = (GridView)sender;
                txtGroupName.Text = "";
                for (int i = 0; i <= lstTests.Items.Count - 1; i++)
                {
                    lstTests.Items[i].Selected = false;
                }
                if (Session["dtTestGroup"] != null)
                    dtTestGroup = (DataTable)Session["dtTestGroup"];
                DataRow[] foundRows;
                foundRows = dtTestGroup.Select("TestGroup= '" + grdTestGroup.DataKeys[myRow.RowIndex].Values[0].ToString() + "' ");
                if (foundRows.Length > 0)
                {
                    string strSQL = "delete from TblTestGroups  where TestGroup= '" + grdTestGroup.DataKeys[myRow.RowIndex].Values[0].ToString() + "' and UserId='" + Session["Login"].ToString() + "'";
                    SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
                    foundRows[0].Delete();
                    dtTestGroup.AcceptChanges();
                }
                grdTestGroup.DataSource = dtTestGroup.Copy();
                grdTestGroup.DataBind();

                Session["dtTestGroup"] = dtTestGroup.Copy();
                if (foundRows.Length > 0)
                {
                    lblError.ForeColor = GlobalValues.SucessColor;//Code modified on April 20,2015-Subhashini
                    lblError.Text = "The selected Test Group deleted successfully.";
                }
            }
        }

        private void BindTestGroup()
        {
            string strQueryText = "Select TestGroup as 'TestGroup', Substring(TestListCSV,2,LEN(TestListCSV)) as TestListCSV from TblTestGroups where UserId='" + Session["Login"].ToString() + "' order by CreatedDate Desc";
            DataSet dsTest = SqlHelper.ExecuteDataset(strConn, CommandType.Text, strQueryText);
            if (dsTest != null && dsTest.Tables.Count > 0 && dsTest.Tables[0].Rows.Count > 0)
            {
                grdTestGroup.DataSource = dsTest;
                grdTestGroup.DataBind();
                dtTestGroup = dsTest.Tables[0].Copy();
                Session["dtTestGroup"] = dsTest.Tables[0].Copy();
                flagGridUpdate = false;
            }
            else
            {
                grdTestGroup.DataSource = null;
                grdTestGroup.DataBind();
            }
        }
        private void BindTestGroupUpdate()
        {
            grdTestGroup.DataSource = null;
            grdTestGroup.DataBind();
            string strQueryText = "Select TestGroup as 'TestGroup', Substring(TestListCSV,2,LEN(TestListCSV)) as TestListCSV,ModifiedDate from TblTestGroups where UserId='" + Session["Login"].ToString() + "' order by ModifiedDate DESC";
            DataSet dsTest = SqlHelper.ExecuteDataset(strConn, CommandType.Text, strQueryText);
            if (dsTest != null && dsTest.Tables.Count > 0 && dsTest.Tables[0].Rows.Count > 0)
            {
                //DataView dvTest = dsTest.Tables[0].DefaultView;
                //dvTest.Sort = "ModifiedDate DESC"; ;
                //grdTestGroup.DataSource = dvTest;
                grdTestGroup.DataSource = dsTest;
                grdTestGroup.DataBind();
                dtTestGroup = dsTest.Tables[0].Copy();
                Session["dtTestGroup"] = dsTest.Tables[0].Copy();
            }
            else
            {
                grdTestGroup.DataSource = null;
                grdTestGroup.DataBind();
            }
            flagGridUpdate = true;
        }
    }
}