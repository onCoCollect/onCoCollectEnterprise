using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace ePxCollectWeb
{
    public partial class DrugGroup : System.Web.UI.Page
    {
        string strConn = string.Empty;
        DataTable dtDrugGroup = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxControlToolkit.ModalPopupExtender ModalPopupExtender2 = (this.Master.FindControl("ModalPopupExtender2") as AjaxControlToolkit.ModalPopupExtender);
            ModalPopupExtender2.Hide();

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
                string strQueryText = "Select DrugName from DrugNames";
                DataSet dsTest = SqlHelper.ExecuteDataset(strConn, CommandType.Text, strQueryText);
                lstTests.DataSource = dsTest;
                lstTests.DataTextField = dsTest.Tables[0].Columns[0].ColumnName;
                lstTests.DataValueField = dsTest.Tables[0].Columns[0].ColumnName;
                lstTests.DataBind();
                BindDrugGroup();
                btnSave.Text = "Save";
            }

            if (dtDrugGroup.Columns.Count <= 0)
            {
                DataColumn dcCSV = new DataColumn("DrugGroup");
                dcCSV.DataType = System.Type.GetType("System.String");
                dtDrugGroup.Columns.Add(dcCSV);
            }
            btnSave.Attributes.Add("onclick", "return ValidateDrugGroup('" + lstTests.ClientID + "','" + lblError.ClientID + "')");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AjaxControlToolkit.ModalPopupExtender ModalPopupExtender2 = (this.Master.FindControl("ModalPopupExtender2") as AjaxControlToolkit.ModalPopupExtender);
            ModalPopupExtender2.Hide();
            string userId = string.Empty;
            if (Session["Login"] != null)
            {
                userId = Session["Login"].ToString();
            }
            string currentDateTime = GetDateString();
            int nameCount = 0;
            if (btnSave.Text == "Save")
            {
                string checkName = "Select count(*) from GroupName where GroupName ='" + txtDrugList.Text.Trim().ToString() + "'";
                nameCount = (int)GlobalValues.ExecuteScalar(checkName);
            }
            if (nameCount > 0)
            {
                lblError.ForeColor = GlobalValues.FailureColor;//Code modified on March 31,2015
                lblError.Text = "Selected Drug group already exists.";
                txtDrugList.Text = "";
            }
            if (nameCount <= 0)
            {
                string strNewVal = string.Empty;

                if (btnSave.Text == "Save")
                {
                    string strSQL = "insert into  GroupName (GroupName,AProtocolDrugGroup,CreatedDate,CreatedBy) values ('" + txtDrugList.Text.Trim() + "','0','" + currentDateTime + "','" + userId + "')";
                    SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);

                    for (int I = 0; I < lstTests.Items.Count; I++)
                    {
                        if (lstTests.Items[I].Selected == true)
                        {
                            strSQL = "insert into  Drugs (GroupName,DrugName) values ('" + txtDrugList.Text.Trim() + "','" + lstTests.Items[I].Text.ToString() + "')";
                            SqlHelper.ExecuteNonQuery(strConn, System.Data.CommandType.Text, strSQL);
                        }
                    }
                    lblError.ForeColor = GlobalValues.SucessColor;
                    lblError.Text = "Drug Group Created successfully.";
                    grdDrugGroup.PageIndex = 0;
                    BindDrugGroup();
                    clear();
                    //Move Focus to BtnYes While Showing Popup ,Aloow User While Pressing Enter Key Using Keyboard
                    //ScriptManager.RegisterStartupScript(this,
                    //                                   this.GetType(),
                    //                                   "FocusScript",
                    //                                   "setTimeout(function(){$get('" + btnSave.ClientID + "').focus();}, 100);",
                    //                                   true);
                }
            }


        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }



        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            txtDrugList.Text = "";
            lstTests.ClearSelection();
            lstTests.Enabled = false;
            Response.Redirect("~/DrugGroup.aspx");
        }

        protected void btnClose_Click1(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            Response.Redirect("~/ProjectForm.aspx");
        }

        public void clear()
        {

            txtDrugList.Text = "";
            lstTests.ClearSelection();
            lstTests.Enabled = true;
            btnSave.Enabled = true;
        }
        public static String GetDateString()
        {
            string dateTimeString = string.Empty;
            DateTime currentDateTime = DateTime.Now;
            string DateTimeFormat = "MM-dd-yyyy HH:mm:ss";
            dateTimeString = DateTime.Now.ToString(DateTimeFormat);
            return dateTimeString;
        }


        protected void grdDrugGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            lblError.Text = "";
            try
            {
                grdDrugGroup.PageIndex = e.NewPageIndex;
                BindDrugGroup();
                grdDrugGroup.SelectedIndex = -1;
            }
            catch (Exception ex)
            {

            }

            //grdTestGroup.PageIndex = e.NewPageIndex;
            //dsSearch = (DataSet)Session["ds" + Session.SessionID.ToString()];
            //grdTestGroup.DataSource = dsSearch;
            //grdTestGroup.DataBind();

        }

        protected void grdDrugGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewItem")
            {
                LinkButton lnkbtn = (LinkButton)e.CommandSource;
                GridViewRow myRow = (GridViewRow)lnkbtn.Parent.Parent;
                GridView myGrid = (GridView)sender;
                if (e.CommandName.ToString() == "ViewItem")
                {

                    txtDrugList.Text = ((Label)myRow.FindControl("lblDrugList")).Text.Trim();
                    ViewState["TestGroupName"] = ((Label)myRow.FindControl("lblDrugList")).Text.Trim();
                    string strQueryText = "Select DrugName from Drugs where GroupName='" + ViewState["TestGroupName"].ToString() + "' ";
                    DataSet dsTest = SqlHelper.ExecuteDataset(strConn, CommandType.Text, strQueryText);

                    lstTests.DataSource = dsTest;
                    lstTests.DataTextField = dsTest.Tables[0].Columns[0].ColumnName;
                    lstTests.DataValueField = dsTest.Tables[0].Columns[0].ColumnName;
                    lstTests.DataBind();

                    lblError.Text = "";

                    for (int k = 0; k < dsTest.Tables[0].Rows.Count; k++)
                    {
                        for (int i = 0; i <= lstTests.Items.Count - 1; i++)
                        {
                            if (lstTests.Items[i].Text == dsTest.Tables[0].Rows[k]["DrugName"].ToString())
                                lstTests.Items[i].Selected = true;
                        }
                    }

                    lstTests.Enabled = false;
                    btnSave.Enabled = false;
                }
            }

        }

        private void BindDrugGroup()
        {
            string strQueryText = "Select GroupName as 'GroupName' from GroupName order by CreatedDate desc";
            DataSet dsTest = SqlHelper.ExecuteDataset(strConn, CommandType.Text, strQueryText);
            grdDrugGroup.DataSource = dsTest;
            grdDrugGroup.DataBind();
            dtDrugGroup = dsTest.Tables[0].Copy();
            Session["dtDrugGroup"] = dsTest.Tables[0].Copy();
        }

        protected void lstTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList arr = new ArrayList();
            string SelectedItem = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$'); ;
            int index = int.Parse(checkedBox[checkedBox.Length - 1]);
            if (lstTests.Items[index].Selected)
            {
                SelectedItem = lstTests.Items[index].Text;
            }

            for (int count = 0; count < lstTests.Items.Count; count++)
            {
                if (lstTests.Items[count].Selected == true)
                {
                    arr.Add(lstTests.Items[count].Text);
                    arr.Sort();

                }
            }

            for (int count = 0; count < lstTests.Items.Count; count++)
            {
                if (arr.Count > 7 && lstTests.Items[count].Text == SelectedItem)
                {
                    lblError.Text = "Seleted Drug list should not exceeed 7.";
                    lstTests.Items[count].Selected = false;
                    int indexItem = arr.IndexOf(SelectedItem);
                    arr.RemoveAt(indexItem);
                    break;
                }
                else
                {
                    lblError.Text = "";
                }
            }

            arr.Sort();

            txtDrugList.Text = "";
            for (int k = 0; k < arr.Count; k++)
            {
                if (txtDrugList.Text == string.Empty)
                    txtDrugList.Text = arr[k].ToString();
                else
                    txtDrugList.Text += "+" + arr[k].ToString();
            }

            //if (txtDrugList.Text.ToString().Length > 250)
            //{
            //    lblError.Text = "Seleted Drug list  Length Should not exceed 250 Characters.";
            //    lstTests.Items[count].Selected = false;
            //}
            //else
            //    lblError.Text = "";

        }

    }
}