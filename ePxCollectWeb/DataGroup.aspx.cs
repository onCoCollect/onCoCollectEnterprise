using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data;
using System.Data.SqlClient;

namespace ePxCollectWeb
{
    public partial class DataGroup : System.Web.UI.Page
    {
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
        List<string> Values = new List<string>();
        ListItem livalues = new ListItem();
        bool status = false;
        static int populateTemplateID = 0;
        private string connectionString = GlobalValues.strConnString;
        static string statGroupName = string.Empty;
        static DataTable dtFieldValues = new DataTable();
        DataSet dsDataAccess = new DataSet();
        static bool fieldValues = false;
        private string UserID
        {
            get
            {
                return Convert.ToString(Session["Login"]).Trim();
            }
        }
        private bool IsUserLoggedIn
        {
            get
            {
                return Session["Login"] != null && Convert.ToString(Session["Login"]) != "";
            }
        }
        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);
        //    bindCreatedTemplate();
        //    BindFields();

        //    bindGroupNames();
        //}
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["FeatureSetPermission"] != null)
            {
                ObjfeatureSet = (FeatureSetPermission)Session["FeatureSetPermission"];
            }
            if (Session["ResetPassword"] != null)
            {
                Session["ResetPasswordMsg"] = "Please Change your password.";
                Response.Redirect("Changepassword.aspx");
            }
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }

            dpValues.Style.DropDownBoxBoxHeight = 150;
            dpValues.Style.DropDownBoxBoxWidth = 220;
            dpValues.Style2.SelectBoxWidth = 220;
            if (!Page.IsPostBack)
            {
                ddlTemplateNames.Enabled = false;
                //bindTemplates();
                bindCreatedTemplate();
                BindFields();

                bindGroupNames();
                ddlSelectTemplate.Enabled = true;
                txtTemplateName.Focus();
                dpValues.Texts.SelectBoxCaption = "";
                ViewState["TemplageName"] = null;

            }
            //ddlSelectTemplate.SelectedValue = Request.Form[ddlSelectTemplate.UniqueID];
        }
        #region Events

        protected void rbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblmsg.Text = string.Empty;
            if (Convert.ToInt16(rbList.SelectedValue) == 2)
            {
                btnsave.Text = "Update";
                bindTemplates();
                txtTemplateName.Text = "";
                txtDesc.Text = "";
                txtTemplateName.Enabled = false;
                ddlTemplateNames.Enabled = true;
                txtDesc.Enabled = false;
                Session["Edit"] = "true";
            }
            else
            {
                btnsave.Text = "Save";
                ddlTemplateNames.SelectedItem.Text = "";
                txtTemplateName.Enabled = true;
                ddlTemplateNames.Enabled = false;
                txtDesc.Enabled = true;
                txtTemplateName.Text = "";
                txtDesc.Text = "";
                txtTemplateName.Focus();
                Session["Edit"] = string.Empty;
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!IsUserLoggedIn) return;
            if (ddlTemplateNames.SelectedItem.Text != string.Empty)
            {
                int cntRecs = 0;
                var selectTemplateNames = "Select count(stat_TemplateName) from tblStatTemplates where stat_TemplateName = '" + ddlTemplateNames.SelectedItem.Text + "'";
                cntRecs = (int)GlobalValues.ExecuteScalar(selectTemplateNames);

                if (cntRecs > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "script", "ShowConfirmation();", true);
                }
                else
                {
                    DeleteTemplate();
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(Page, GetType(), "script", "alert('Please Select Template Name.')", true);
                lblmsg.ForeColor = GlobalValues.FailureColor;//Code Added on April 1,2015
                lblmsg.Text = "Please Select Template Name.";
            }

            if (Convert.ToInt16(rbList.SelectedValue) == 2)
                ddlTemplateNames.Enabled = true;
            else
                ddlTemplateNames.Enabled = false;

        }
        protected void btnAlelrt_Click(object sender, EventArgs e)
        {
            DeleteTemplate();
        }
        private void DeleteTemplate()
        {
            var index = ddlTemplateNames.SelectedItem.Value;
            string deleteGroup = " delete  from tblStatTemplates where mang_TemplateID = '" + index + "'";
            GlobalValues.ExecuteNonQuery(deleteGroup);
            string deleteSql = "Delete from [tblManageTemplate]  where [mang_TemplateID] ='" +
                index + "' and  UserID='" + UserID + "'";
            GlobalValues.ExecuteNonQuery(deleteSql);
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Delete", "alert('Template Name deleted successfully.');", true);
            lblmsg.ForeColor = GlobalValues.SucessColor;//Code Added on April 1,2015
            lblmsg.Text = "Template Name deleted successfully.";
           
            clearFields();
            bindTemplates();
            bindCreatedTemplate();
            bindGroupNames();
            if (ddlTemplateNames.Items.Count == 1)
            {
                txtTemplateName.Enabled = false;
                ddlTemplateNames.Enabled = true;
                txtDesc.Enabled = false;
            }
            ddlStatGroupName_SelectedIndexChanged(ddlStatGroupName, EventArgs.Empty);
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            string templateName = string.Empty;
            string description = string.Empty;

            templateName = txtTemplateName.Text.Trim();
            description = txtDesc.Text.Trim();
            try
            {
                if (btnsave.Text == "Update")
                {
                    var updateStatement = "update tblManageTemplate set mang_TemplateName ='" +
                               txtTemplateName.Text.Trim() + "' ,mang_TemplateDesc ='" + txtDesc.Text.Trim() + "' where mang_TemplateID = '" +
                               ddlTemplateNames.SelectedValue.Trim() + "'  and userID='" + UserID + "' ";
                    var updateSubTable = string.Format(@"Update [tblStatTemplates]
                                                          set stat_TemplateName ='{0}'
                                                          where mang_TemplateID = '{1}'  and userID='{2}'", txtTemplateName.Text.Trim(),
                                                                                                         ddlTemplateNames.SelectedValue.Trim(),
                                                                                                         UserID);

                    GlobalValues.ExecuteNonQuery(updateStatement);
                    GlobalValues.ExecuteNonQuery(updateSubTable);
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Template updated  sucessfully.');", true);
                    lblmsg.ForeColor = GlobalValues.SucessColor;//Code modified on April 1,2015
                    lblmsg.Text = "Template updated  sucessfully.";
                    btnsave.Text = "Save";
                    ddlTemplateNames.Enabled = true;
                    bindTemplates();
                    // ViewState["mangTemplateName"] = null;
                    //bindGroupNames();
                    //grdStatDetails.Visible = false;
                }
                else
                {

                    const string tempAlreadyexistErr = "Template name already Exists. Please use differnt Templates Name";
                    string sqlCount = "select count(*) from tblManageTemplate where mang_TemplateName = '" + templateName + "'  and userID='" + UserID + "' ";
                    int recCount = (int)GlobalValues.ExecuteScalar(sqlCount);

                    if (recCount > 0)
                    {
                        //RegisterAlert(tempAlreadyexistErr);
                        lblmsg.ForeColor = GlobalValues.FailureColor;//Code modified on April 1,2015
                        lblmsg.Text = tempAlreadyexistErr;
                    }
                    else
                    {
                        var insertStatement = "insert into tblManageTemplate values ( '" + templateName.Trim() + "','" + description.Trim() + "' ,'" + UserID + "')";
                        var dataSet = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, insertStatement);
                        lblmsg.ForeColor = GlobalValues.SucessColor;//Code modified on April 1,2015
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Template  created successfully.');", true);
                        lblmsg.Text = "Template  created successfully.";
                    }
                    bindTemplates();
                    ddlTemplateNames.Enabled = false;
                }
                ViewState["mangTemplateName"] = templateName.Trim();

                bindCreatedTemplate();
                bindGroupNames();
                clearFields();

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblmsg.Text = string.Empty;
            clearFields();
        }
        #endregion

        #region Private Methods
        private void BindFields()
        {
            try
            {
                var sqlText = "select [Field Name],[FieldOrder],[Table Name] FROM [PDFields] " +
                      "where ( isnull([Patient Identity Field],0)=0) and statscoding = 1 and [FieldOrder] not in ('" + 2147 + "','" + 2159 + "','" + 2 + "')  order by  [FieldOrder] ";
                //and statscoding='1'
                var FieldSet = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sqlText);

                ddlField.DataSource = FieldSet.Tables[0];
                ddlField.DataTextField = "Field Name";
                ddlField.DataValueField = "Field Name";
                //ddlTemplateNames.DataValueField = "mang_TemplateID";
                ddlField.DataBind();
                ddlField.Items.Insert(0, "");
                ddlField.Enabled = false;
                btnOKField.CssClass = "buttonDisable";
                btnOKField.Enabled = false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void BindValues()
        {
            try
            {
                if (!IsUserLoggedIn)
                    return;
                string pdQuery = "select [Table Name],[Field Name],[fieldOrder] from pdfields where [Field Name] ='" + ddlField.SelectedItem.Value + "'";
                var pdData = GlobalValues.ExecuteDataSet(pdQuery);
                var colsNames = pdData.Tables[0].Rows;
                var tableName = string.Empty;
                var fieldName = string.Empty;
                var fieldOrder = string.Empty;
                for (int i = 0; i < pdData.Tables[0].Rows.Count; i++)
                {
                    tableName = colsNames[i][0].ToString();
                    fieldName = colsNames[i][1].ToString();
                    fieldOrder = colsNames[i][2].ToString();
                }

                string templateName = ddlSelectTemplate.SelectedItem.Text;
                string strSelectedFieldName = ddlField.SelectedItem.Text;
                string strSql = "select distinct [" + fieldName + "] AS FieldValue from  " + tableName +
                " where [" + fieldName + "] is not null and [" + fieldName + "] <> '' order by [" + fieldName + "]";

                var ColNames = GlobalValues.ExecuteDataSet(strSql);

                //dtFieldValues = ColNames.Tables[0];
                dpValues.DataSource = ColNames.Tables[0];
                dpValues.DataTextField = "FieldValue";
                dpValues.DataBind();
                dpValues.Texts.SelectBoxCaption = "";
                //ddlField.Items.Insert(0, "");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void clearFields()
        {
            ddlTemplateNames.SelectedIndex = -1;
            txtTemplateName.Text = "";
            txtDesc.Text = "";
        }
        public void clearStatFields()
        {
            ddlSelectTemplate.SelectedIndex = -1;
            ddlField.SelectedIndex = -1;
            dpValues.Texts.SelectBoxCaption = "";
            //for (int i = 0; i <= dpValues.Items.Count - 1; i++)
            //{
            //    foreach (ListItem li in dpValues.Items)
            //    {
            //        li.Selected = false;
            //    }
            //}
            dpValues.Items.Clear();
            txtGroupName.Text = string.Empty;
            btnstatSave.Text = "Save";
            ddlField.Enabled = false;
            btnOKField.CssClass = "buttonDisable";
            btnOKField.Enabled = false;
        }
        private void RegisterAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string),
                "PopupWindow", "alert('" + msg + ".');", true);
        }
        private void bindTemplates()
        {
            ddlTemplateNames.Enabled = true;
            var connectionString = GlobalValues.strConnString;
            var selectedTemplateName = ddlTemplateNames.SelectedIndex;
            var templateNames = "select * from tblManageTemplate where UserID='" + Session["Login"].ToString() + "'";
            ddlTemplateNames.DataSource = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, templateNames);
            ddlTemplateNames.DataTextField = "mang_TemplateName";
            ddlTemplateNames.DataValueField = "mang_TemplateID";
            ddlTemplateNames.DataBind();
            ddlTemplateNames.Items.Insert(0, "");
        }
        public void bindCreatedTemplate()
        {
            // ddlSelectTemplate.Items.Clear();

            ddlSelectTemplate.Items.Insert(0, "");
            var sqlText = "select distinct(LTRIM(RTRIM(mang_TemplateName))) AS mang_TemplateName,mang_TemplateID from tblManageTemplate where UserID='" +
                Session["Login"].ToString() + "'";
            var templates = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sqlText);
            ddlSelectTemplate.DataSource = templates;
            ddlSelectTemplate.DataTextField = "mang_TemplateName";
            ddlSelectTemplate.DataValueField = "mang_TemplateID";
            ddlSelectTemplate.DataBind();
            ddlSelectTemplate.Items.Insert(0, "");
            if (ViewState["mangTemplateName"] != null)
            {
                for (int templateIndex = 0; templateIndex < ddlSelectTemplate.Items.Count; templateIndex++)
                {
                    if (ddlSelectTemplate.Items[templateIndex].Text.Trim() == ViewState["mangTemplateName"].ToString().Trim())
                    {
                        ddlSelectTemplate.SelectedIndex = templateIndex;

                    }
                }
            }

            ViewState["mangTemplateName"] = null;


        }

        private string[] getControlSelection()
        {
            string[] val = new string[2];

            try
            {
                int i = 0; int allIndex = -1; int selCnt = 0;
                foreach (ListItem li in dpValues.Items)
                {
                    i++;
                    if (li.Selected)
                    {
                        selCnt++;
                        if (li.Text.ToUpper().ToString() == "ALL")
                        {

                            allIndex = i - 1;
                        }
                    }
                    else
                    {
                        li.Selected = false;
                        li.Enabled = true;
                    }
                }
                //if (selCnt > 1 && allIndex != -1)
                //{
                //    dpValues.Items[allIndex].Selected = false;


                //}
                foreach (ListItem li in dpValues.Items)
                {
                    if (li.Selected)
                    {
                        val[0] += li.Value + ",";
                        val[1] += li.Text + ",";
                    }
                }

                val[0] = (!string.IsNullOrEmpty(val[0])) ? val[0].Substring(0, val[0].Length - 1) : string.Empty;
                val[1] = (!string.IsNullOrEmpty(val[1])) ? val[1].Substring(0, val[1].Length - 1) : string.Empty;
            }
            catch (Exception ex)
            {
            }

            return val;
        }

        public void bindGroupNames()
        {
            //ddlStatGroupName.Items.Clear();
            var sqlText = "select distinct(stat_TemplateName),mang_TemplateID from [tblStatTemplates] where UserID='" + UserID + "' ";
            var templates = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sqlText);
            ddlStatGroupName.DataTextField = "stat_TemplateName";
            ddlStatGroupName.DataValueField = "mang_TemplateID";
            ddlStatGroupName.DataSource = templates;
            ddlStatGroupName.DataBind();
            ddlStatGroupName.Items.Insert(0, "");
            if (ViewState["TemplateName"] != null)
            {
                for (int templateIndex = 0; templateIndex < ddlStatGroupName.Items.Count; templateIndex++)
                {
                    if (ddlStatGroupName.Items[templateIndex].Text.Trim() == ViewState["TemplateName"].ToString().Trim())
                    {
                        ddlStatGroupName.SelectedIndex = templateIndex;

                    }
                }
            }
            //else
            //{
            ViewState["TemplateName"] = null;

            //}
        }

        public void bindGrid()
        {

            //if (ddlStatGroupName.SelectedValue != "")
            //{
            //    GridDataGroup.Visible = true;

            var selectStat = "select stat_ID,stat_TemplateName,stat_FieldName,stat_Value,stat_GroupName,mang_TemplateID,UserID,pd.[Table Name] " +
                                " from [tblStatTemplates] stat inner join PDFields pd on stat.stat_FieldName = pd.[Field Name] " +
                                " where stat_TemplateName ='" + statGroupName + "'" + " and  UserID='" + UserID + "'";
            var statResult = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, selectStat);
            GridDataGroup.DataSource = statResult;
            GridDataGroup.DataBind();
            //}
            //else
            //{
            //    GridDataGroup.Visible = false;
            //}
        }
        #endregion

        protected void ddlTemplateNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsUserLoggedIn) return;
            lblmsg.Text = string.Empty;
            if (ddlTemplateNames.SelectedValue != "")
            {
                txtTemplateName.Enabled = true;
                txtDesc.Enabled = true;
                txtTemplateName.Text = "";
                txtDesc.Text = "";
                var sqlSelect = "select * from tblManageTemplate where mang_TemplateID = '" +
                    ddlTemplateNames.SelectedValue + "' and userID='" + UserID + "' ";
                var exeSql = SqlHelper.ExecuteReader(connectionString, System.Data.CommandType.Text, sqlSelect);
                if (exeSql.HasRows)
                {
                    if (exeSql.Read())
                    {
                        txtTemplateName.Text = exeSql["mang_TemplateName"].ToString().Trim();
                        txtDesc.Text = exeSql["mang_TemplateDesc"].ToString().Trim();
                    }
                }
            }
            else
            {
                txtTemplateName.Text = "";
                txtDesc.Text = "";
                txtTemplateName.Enabled = false;
                txtDesc.Enabled = false;
            }
            btnsave.Text = "Update";
        }

        //protected void txtTemplateName_TextChanged(object sender, EventArgs e)
        //{
        //    string studycode = string.Empty;
        //    string templatename = txtTemplateName.Text.Trim();
        //    string templateID = ddlTemplateNames.SelectedValue.Trim();

        //    string sqlCount = "select count('" + templatename + "')  from tblManageTemplate where mang_TemplateName = '" + templatename + "'";
        //    var recCount = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sqlCount);
        //    if (templatename != ddlTemplateNames.SelectedItem.Text.Trim())
        //    {
        //        if (Convert.ToInt32(recCount.Tables[0].Rows[0][0].ToString()) > 0)
        //        {
        //            status = true;
        //            ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert(' Template Name alread exist.');", true);
        //        }
        //        else
        //        {
        //            status = false;
        //        }
        //    }
        //}

        #region Stat Events
        protected void btnstatSave_Click(object sender, EventArgs e)
        {
            string strSelectedTemplateName = string.Empty;
            try
            {
                if (ddlSelectTemplate.SelectedItem.Text == "" ||
                        ddlField.SelectedItem.Text == "" || dpValues.Texts.SelectBoxCaption == "" || txtGroupName.Text == "")
                {
                    lblMsgGroup.ForeColor = GlobalValues.FailureColor;//Code modified on April 1,2015
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Fields marked with asterisk (*) are required.');", true);
                    lblMsgGroup.Text = "Fields marked with asterisk (*) are required.";
                    return;
                }
                else
                {
                    string templateName = string.Empty;
                    string templateID = ddlSelectTemplate.SelectedValue.ToString();
                    templateName = ddlSelectTemplate.SelectedItem.Text.Trim();
                    strSelectedTemplateName = ddlSelectTemplate.SelectedItem.Text.Trim();
                    string strSelectedField = ddlField.SelectedItem.Text.Trim();

                    string strSelectedValue = dpValues.Texts.SelectBoxCaption;
                    string groupName = txtGroupName.Text;

                    if (btnstatSave.Text == "Update")
                    {
                        if (ViewState["GroupName"] != null)
                        {
                            if (ViewState["GroupName"].ToString().ToUpper() != groupName.Trim().ToUpper())
                            {
                                string sqlCount = "select count(*) from tblStatTemplates where stat_GroupName = '" + groupName.Trim() + "' and  UserID='" + UserID + "' and stat_TemplateName='" + templateName + "'";
                                int recCount = (int)GlobalValues.ExecuteScalar(sqlCount);
                                if (recCount > 0)
                                {
                                    txtGroupName.Text = string.Empty;
                                    lblMsgGroup.ForeColor = GlobalValues.FailureColor;
                                    //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Group Name already Exists. Please use differnt Group Name');", true);
                                    lblMsgGroup.Text = "Group Name already Exists.";
                                }
                                else
                                {
                                    var updateStatement = "update tblStatTemplates set stat_FieldName ='" + ddlField.SelectedItem.Text.ToString().Trim() + "' ,stat_Value ='" + dpValues.Texts.SelectBoxCaption.Trim() + "',stat_GroupName ='" + txtGroupName.Text + "'   where stat_ID = '" + ViewState["statID"].ToString() + "' and  UserID='" + UserID + "'";
                                    var dataSet = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, updateStatement);
                                    //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Stat code updated successfully.');", true);
                                    lblMsgGroup.ForeColor = GlobalValues.SucessColor;//Code modified on April 1,2015
                                    lblMsgGroup.Text = "Stat code updated successfully.";
                                    btnstatSave.Text = "Save";
                                    clearStatFields();

                                }
                            }
                            else
                            {
                                var updateStatement = "update tblStatTemplates set stat_FieldName ='" + ddlField.SelectedItem.Text.ToString().Trim() + "' ,stat_Value ='" + dpValues.Texts.SelectBoxCaption.Trim() + "',stat_GroupName ='" + txtGroupName.Text + "'   where stat_ID = '" + ViewState["statID"].ToString() + "' and  UserID='" + UserID + "'";
                                var dataSet = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, updateStatement);
                                lblMsgGroup.ForeColor = GlobalValues.SucessColor;//Code modified on April 1,2015
                                lblMsgGroup.Text = "Stat code updated successfully.";
                                btnstatSave.Text = "Save";
                                clearStatFields();
                            }
                        }


                    }
                    else
                    {
                        string sqlCount = "select count(*) from tblStatTemplates where stat_GroupName = '" + groupName.Trim() + "' and  UserID='" + UserID + "' and stat_TemplateName='" + templateName + "'";
                        int recCount = (int)GlobalValues.ExecuteScalar(sqlCount);

                        if (recCount > 0)
                        {
                            txtGroupName.Text = string.Empty;
                            //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Group Name already Exists. Please use differnt Group Name');", true);
                            lblMsgGroup.ForeColor = GlobalValues.FailureColor;//Code Added on April 1,2015
                            lblMsgGroup.Text = "Group Name already Exists.";
                        }
                        else
                        {
                            string insertStatCode = "insert into [tblStatTemplates] values ('" + strSelectedTemplateName.Trim() + "' ,'" + strSelectedField.Trim() + "','" + strSelectedValue.Trim() + "','" + groupName.Trim() + "'," + Convert.ToInt16(templateID) + ", '" + UserID.Trim() + "')";//,'" + templateName.Trim() + "')";
                            SqlHelper.ExecuteNonQuery(connectionString, System.Data.CommandType.Text, insertStatCode);
                            // grdStatDetails.Visible = true;

                            // bindGroupNames();
                            //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Stat Code created successfully.');", true);
                            lblMsgGroup.ForeColor = GlobalValues.SucessColor;//Code Added on April 1,2015
                            lblMsgGroup.Text = "Stat Code created successfully.";
                            clearStatFields();

                        }
                    }
                }
                statGroupName = strSelectedTemplateName;
                ViewState["TemplateName"] = strSelectedTemplateName.Trim();
                bindGroupNames();
                bindGrid();

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnstatReset_Click(object sender, EventArgs e)
        {
            lblMsgGroup.Text = string.Empty;
            clearStatFields();
        }

        //protected void ddlSelectTemplate_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(ddlSelectTemplate.SelectedValue))
        //    {
        //        populateTemplateID = Convert.ToInt32(ddlSelectTemplate.SelectedValue);
        //    }
        //    else
        //    {
        //        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please select a Template Name.');", true);
        //        lblMsgGroup.ForeColor = GlobalValues.FailureColor;//Code Added on April 1,2015
        //        lblMsgGroup.Text = "Please select a Template Name.";
        //        ddlField.Focus();
        //        return;
        //    }
        //    BindPopulateField();
        //}
        private void BindPopulateField()
        {
            ddlField.Enabled = true;
            btnOKField.Enabled = true;
            btnOKField.CssClass = "button";
            DataSet ds = PopulateField(populateTemplateID);
            ddlField.DataSource = ds.Tables[0];
            ddlField.DataTextField = "Field Name";
            ddlField.DataValueField = "Field Name";
            ddlField.DataBind();
            ddlField.Items.Insert(0, "");
        }
        public DataSet PopulateField(int templateID)
        {
            try
            {
                object[] objParams = { templateID };
                dsDataAccess = ExecuteDataSet("sp_Populate_Fields", objParams);
                if (dsDataAccess != null && dsDataAccess.Tables.Count > 0 && dsDataAccess.Tables[0].Rows.Count > 0)
                {
                    return dsDataAccess;
                }
                return dsDataAccess;
            }
            catch (Exception ex)
            {
                // Log.WriteLog(ex.ToString());
                throw;
            }
        }
        public DataSet ExecuteDataSet(string StoredProcedure, object[] objParams)
        {
            string strConn = GlobalValues.strConnString.ToString();
            return dsDataAccess = SqlHelper.ExecuteDataset(strConn, StoredProcedure, objParams);
        }
        //protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string FieldName = string.Empty;
        //    FieldName = ddlField.SelectedItem.Text.Trim();
        //    if (!string.IsNullOrEmpty(FieldName))
        //    {
        //        PopulateFieldValue(populateTemplateID, FieldName);
        //    }
        //    //dpValues.Texts.SelectBoxCaption = "";
        //}

        protected void dpValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsgGroup.Text = string.Empty;
            if (fieldValues == false)
            {
                dpValues.Texts.SelectBoxCaption = getControlSelection()[1];
                fieldValues = true;
            }
            else
            {
                fieldValues = false;
            }
        }

        protected void ddlStatGroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            statGroupName = ddlStatGroupName.SelectedItem.Text.Trim();
            bindGrid();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Session["Flag"] = "Stats";
            Response.Redirect("ProjectForm.aspx");
        }
        #endregion

        #region Grid Events
        protected void imgDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton delete = (ImageButton)sender;
                string seletedStatID = delete.CommandArgument.ToString();
                string deleteSql = "Delete from tblStatTemplates  where [stat_ID] ='" + seletedStatID + "'" + " and  UserID='" + UserID + "'";
                GlobalValues.ExecuteNonQuery(deleteSql);
                //ScriptManager.RegisterStartupScript(this, typeof(string), "Delete", "alert('Stat Code Deleted Sucessfully');", true);
                lblmsg.ForeColor = GlobalValues.SucessColor;//Code modified on April 1,2015
                lblmsg.Text = "Stat Code Deleted Sucessfully";
                bindGroupNames();
                bindGrid();
                clearStatFields();
            }

            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
        #endregion

        protected void GridDataGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridDataGroup.PageIndex = e.NewPageIndex;
            bindGrid();
        }

        protected void GridDataGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridDataGroup.SelectedRow;
            string fieldName = string.Empty;

            ViewState["statID"] = ((Label)row.FindControl("LabelSTATID")).Text.Trim();
            txtGroupName.Text = ((Label)row.FindControl("LabelGroupName")).Text.Trim();
            ViewState["GroupName"] = txtGroupName.Text.Trim();
            populateTemplateID = Convert.ToInt32(((Label)row.FindControl("LabelMANGTemplateID")).Text.Trim());
            bindCreatedTemplate();
            BindFields();

            //string tableName = ((Label)row.FindControl("LabelTableName")).Text.Trim();
            string fieldValue = ((Label)row.FindControl("LabelFieldValue")).Text.Trim();
            if (ddlSelectTemplate.DataSource != null)
            {
                string template = ((Label)row.FindControl("LabelTemplateName")).Text.Trim();
                ddlSelectTemplate.SelectedIndex = -1;
                ddlSelectTemplate.Items.FindByText(template).Selected = true;
            }
            BindPopulateField();
            if (ddlField.DataSource != null)
            {
                fieldName = ((Label)row.FindControl("LabelFieldName")).Text.Trim();
                ddlField.SelectedIndex = -1;
                if (ddlField.Items.FindByText(fieldName) != null)
                    ddlField.Items.FindByText(fieldName).Selected = true;
                else
                {
                    ddlField.Items.Add(new ListItem(fieldName, fieldName));
                    ddlField.Items.FindByText(fieldName).Selected = true;
                }
            }

            int statID = Convert.ToInt32(ViewState["statID"].ToString());
            PopulateFieldValue(populateTemplateID, statID);
            dpValues.Texts.SelectBoxCaption = fieldValue;
            lblmsg.Text = string.Empty;
            lblMsgGroup.Text = string.Empty;
            btnstatSave.Text = "Update";
            ddlSelectTemplate.Focus();
        }

        public void PopulateFieldValue(int templateID, int statID)
        {
            try
            {
                object[] objParams = { templateID, DBNull.Value, statID };
                var FieldName = GlobalValues.ExecuteScalar("select  stat_FieldName from tblStatTemplates where mang_TemplateID = '" + templateID + "' and stat_ID = '" + statID + "'");
                var DataType = GlobalValues.ExecuteScalar("select Datatype from PDFields where [Field Name]='" + FieldName.ToString() + "'");
                dsDataAccess = ExecuteDataSet("sp_Populate_Fieldvalue", objParams);
                BindPopulateFieldValue("Edit", DataType.ToString());

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void PopulateFieldValue(int templateID, string FieldName)
        {
            try
            {
                object[] objParams = { templateID, FieldName, 0 };

                var DataType = GlobalValues.ExecuteScalar("select Datatype from PDFields where [Field Name]='" + FieldName + "'");
                dsDataAccess = ExecuteDataSet("sp_Populate_Fieldvalue", objParams);
                BindPopulateFieldValue("Create", DataType.ToString());
                dpValues.Texts.SelectBoxCaption = "";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void BindPopulateFieldValue(string dataManipulation, string DataType)
        {
            dpValues.Items.Clear();
            if (dsDataAccess != null && dsDataAccess.Tables.Count > 0 && dsDataAccess.Tables[0].Rows.Count > 0)
            {

                for (int valuesIndex = 0; valuesIndex < dsDataAccess.Tables[0].Rows.Count; valuesIndex++)
                {
                    if (DataType == "DATE")
                    {
                        dpValues.Items.Add(new ListItem(Convert.ToDateTime(dsDataAccess.Tables[0].Rows[valuesIndex][0].ToString().Trim()).ToString("dd-MMM-yyyy"), dsDataAccess.Tables[0].Rows[valuesIndex][1].ToString()));
                    }
                    else
                    {
                        dpValues.Items.Add(new ListItem((dsDataAccess.Tables[0].Rows[valuesIndex][0].ToString().Trim()), dsDataAccess.Tables[0].Rows[valuesIndex][1].ToString()));
                    }
                }
                //dpValues.DataSource = dsDataAccess.Tables[0];
                //dpValues.DataTextField = "Field_Value";
                //dpValues.DataValueField = "StatusFlg";
                //dpValues.DataBind();
                if (dataManipulation == "Edit")
                {
                    foreach (ListItem li in dpValues.Items)
                    {
                        if (li.Value.Trim() == "T")
                        {
                            li.Selected = true;
                        }
                        else if (li.Value.Trim() == "Y")
                        {
                            li.Selected = false;
                        }
                        else if (li.Value.Trim() == "D")
                        {
                            li.Enabled = false;
                        }
                    }
                }
                else if (dataManipulation == "Create")
                {
                    for (int i = 0; i < dsDataAccess.Tables[0].Rows.Count; i++)
                    {
                        if (dsDataAccess.Tables[0].Rows[i][1].ToString() == "D")
                        {
                            dsDataAccess.Tables[0].Rows[i].BeginEdit();
                            dsDataAccess.Tables[0].Rows[i].Delete();
                        }
                    }
                    dsDataAccess.Tables[0].AcceptChanges();
                    //dpValues.DataSource = dsDataAccess.Tables[0];
                    //dpValues.DataTextField = "Field_Value";
                    //dpValues.DataValueField = "StatusFlg";
                    //dpValues.DataBind();

                    dpValues.Items.Clear();
                    for (int valuesIndex = 0; valuesIndex < dsDataAccess.Tables[0].Rows.Count; valuesIndex++)
                    {
                        if (DataType == "DATE")
                        {
                            dpValues.Items.Add(new ListItem(Convert.ToDateTime(dsDataAccess.Tables[0].Rows[valuesIndex][0].ToString().Trim()).ToString("dd-MMM-yyyy"), dsDataAccess.Tables[0].Rows[valuesIndex][1].ToString()));
                        }
                        else
                        {
                            dpValues.Items.Add(new ListItem((dsDataAccess.Tables[0].Rows[valuesIndex][0].ToString().Trim()), dsDataAccess.Tables[0].Rows[valuesIndex][1].ToString()));
                        }
                    }

                    //foreach (ListItem li in dpValues.Items)
                    //{
                    //    if (li.Value.Trim() == "Y")
                    //    {
                    //        li.Selected = false;
                    //    }
                    //    else if (li.Value.Trim() == "D")
                    //    {
                    //        li.Selected = true;
                    //    }
                    //}
                }

            }
        }


        protected void btnOKTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsgGroup.Text = string.Empty;
                if (!string.IsNullOrEmpty(ddlSelectTemplate.SelectedValue))
                {
                    populateTemplateID = Convert.ToInt32(ddlSelectTemplate.SelectedValue);
                }
                else
                {
                    ddlField.Enabled = false;
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please select a Template Name.');", true);
                    lblMsgGroup.ForeColor = GlobalValues.FailureColor;//Code Added on April 1,2015
                    lblMsgGroup.Text = "Please select a Template Name.";
                    ddlSelectTemplate.Focus();
                    return;
                }
                BindPopulateField();
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);                
                throw;
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);
            }
        }

        protected void btnOKField_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsgGroup.Text = string.Empty;
                string FieldName = string.Empty;
                FieldName = ddlField.SelectedItem.Text.Trim();
                if (!string.IsNullOrEmpty(FieldName))
                {
                    PopulateFieldValue(populateTemplateID, FieldName);
                }
                else
                {
                    lblMsgGroup.ForeColor = GlobalValues.FailureColor;
                    lblMsgGroup.Text = "Select Feild.";
                    ddlField.Focus();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);
                throw;
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);
            }
        }

        protected void ddlSelectTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsgGroup.Text = string.Empty;
            if (string.IsNullOrEmpty(ddlSelectTemplate.SelectedValue))
            {
                ddlField.Items.Clear();
                ddlField.Enabled = false;
                btnOKField.CssClass = "buttonDisable";
                btnOKField.Enabled = false;
                dpValues.Texts.SelectBoxCaption = "";
                dpValues.Items.Clear();
                txtGroupName.Text = string.Empty;
                btnstatSave.Text = "Save";
            }
            
        }

        protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsgGroup.Text = string.Empty;
        }
    }
}