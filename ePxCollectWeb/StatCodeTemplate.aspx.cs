using ePxCollectDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ePxCollectWeb
{
    public partial class StatCodeTemplate : System.Web.UI.Page
    {
        bool status = false;
        #region Connection String
        // Start : Connection String
        private string connectionString = GlobalValues.strConnString;

        #endregion

        #region Reading Username
        // Start : Reading Username from Session
        private string UserName
        {
            get
            {
                return Convert.ToString(Session["Login"]).Trim();
            }
        }
        // End : Reading Username from Session 
        #endregion

        #region Verifying Username
        // Start : Verifying Username
        private bool IsUserLoggedIn
        {
            get
            {
                return Session["Login"] != null && Convert.ToString(Session["Login"]) != "";
            }
        }
        // End : Verifying Username
        #endregion

        #region  Loading
        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["PreviousPage"] = Request.UrlReferrer;
            if (!IsUserLoggedIn)
            {
                Response.Redirect("login.Aspx");
            }

            if (!IsPostBack)
            {
                ddlTemplateNames.Enabled = false;
                bindCreatedTemplate();
                bindFields();
                bindValues();
                bindGroupNames();
                ddlSelectTemplate.Enabled = true;
                txtTemplateName.Focus();
                dpValues.Texts.SelectBoxCaption = "";
                ViewState["TemplageName"] = null;
            }

            ddlSelectTemplate.Enabled = true;

        }
        #endregion

        # region Binding Field Values
        public void bindValues()
        {
            try
            {

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

                if (!IsUserLoggedIn)
                    return;

                var rowValue = new List<string>();
                var recCount = "select count(*) from tblStatTemplates where  UserID='" + UserName + "'";
                var countRecord = GlobalValues.ExecuteDataSet(recCount);

                dpValues.Items.Clear();
                if (countRecord.Tables[0].Rows[0][0].Equals(0))
                {
                    var strSql = "Select isnull([Table Name],'') TableName from PDFields where [Field Name]='" + ddlField.SelectedItem.Text.ToString().Trim() + "'";
                    string TableName = (string)GlobalValues.ExecuteScalar(strSql);
                    if (TableName == null) { TableName = ""; }

                    var strSqlValues = "select distinct [" + fieldName + "] from  " + tableName + " order by [" + fieldName + "] ";

                    var ColNames = GlobalValues.ExecuteDataSet(strSqlValues);
                    var cols = ColNames.Tables[0].Rows;
                    for (int i = 0; i < ColNames.Tables[0].Rows.Count; i++)
                    {

                        if (cols[i][0].ToString() != "-" && cols[i][0].ToString() != "")
                        {
                            dpValues.Items.Add(cols[i][0].ToString().Trim());
                        }

                    }
                }
                else
                {
                    var selectValues = "select  [stat_TemplateName],stat_FieldName,stat_Value from [tblStatTemplates] where [stat_TemplateName] ='" + templateName.Trim() + "' and  UserID='" + UserName + "'";
                    var valuesSet = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, selectValues);
                    var valuesetRows = valuesSet.Tables[0].Rows;


                    dpValues.Items.Clear();
                    if (valuesetRows.Count == 0)
                    {
                        string strSql = "select distinct [" + fieldName + "] from  " + tableName + " order by [" + fieldName + "]";

                        var ColNames = GlobalValues.ExecuteDataSet(strSql);
                        var Cols = ColNames.Tables[0].Rows;
                        for (int iText = 0; iText < Cols.Count; iText++)
                        {
                            if ((Cols[iText][0].ToString().Trim() != "") && (Cols[iText][0].ToString().Trim() != "-"))
                            {
                                dpValues.Items.Add(Cols[iText][0].ToString().Trim());
                            }
                        }
                    }
                    else
                    {
                        dpValues.Items.Clear();
                        for (int rowCount = 0; rowCount < valuesetRows.Count; rowCount++)
                        {
                            rowValue.Add(valuesetRows[rowCount][2].ToString().Trim());
                        }

                        for (int index = 0; index < valuesetRows.Count; index++)
                        {
                            if (templateName.Trim() == valuesetRows[index][0].ToString().Trim())
                            {
                                if (strSelectedFieldName.Trim() == valuesetRows[index][1].ToString().Trim())
                                {
                                    dpValues.Items.Clear();
                                    string strSql = "select distinct [" + fieldName + "] from  " + tableName + " order by [" + fieldName + "]";

                                    var ColNames = GlobalValues.ExecuteDataSet(strSql);
                                    var Cols = ColNames.Tables[0].Rows;

                                    if (Cols.Count > 0)
                                    {
                                        var txtStrings = new List<string>();
                                        for (int txtIndex = 0; txtIndex < Cols.Count; txtIndex++)
                                        {
                                            if ((Cols[txtIndex][0].ToString()) != "" && (Cols[txtIndex][0].ToString() != "-"))
                                            {
                                                txtStrings.Add(Cols[txtIndex][0].ToString().Trim());
                                            }
                                        }

                                        string sqlQuery = " select stat_value from tblStatTemplates where stat_TemplateName='" + templateName.Trim() + "' and stat_FieldName='" + ddlField.SelectedItem.Text.ToString().Trim() + "' and  UserID='" + UserName + "'";
                                        var dsResult = GlobalValues.ExecuteDataSet(sqlQuery);
                                        var dbrows = dsResult.Tables[0].Rows;
                                        List<string> existedList = new List<string>();

                                        for (int lstIndex = 0; lstIndex < dbrows.Count; lstIndex++)
                                        {
                                            if (dbrows[lstIndex][0].ToString().Trim().Length > 0)
                                            {
                                                var dbvalues = dbrows[lstIndex][0].ToString().Trim().Split(',');

                                                for (int dbIndex = 0; dbIndex < dbvalues.Length; dbIndex++)
                                                {
                                                    if (dbvalues[dbIndex].Trim() != "")
                                                    {
                                                        existedList.Add(dbvalues[dbIndex].Trim());
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                existedList.Add(dsResult.Tables[0].Rows[lstIndex][0].ToString().Trim());
                                            }
                                        }
                                        dpValues.Items.Clear();
                                        for (int colIndex = 0; colIndex < Cols.Count; colIndex++)
                                        {
                                            if (!existedList.Contains(Cols[colIndex][0].ToString().Trim()))
                                            {
                                                if ((Cols[colIndex][0].ToString() != "") && (Cols[colIndex][0].ToString() != "-"))
                                                {
                                                    dpValues.Items.Add(Cols[colIndex][0].ToString().Trim());
                                                }

                                            }
                                        }

                                        if (ViewState["flag"] != null)
                                        {
                                            if (ViewState["flag"].ToString() == "Edit")
                                            {
                                               

                                                if (dpValues.Texts.SelectBoxCaption.Length > 1)
                                                {
                                                    dpValues.Items.Clear();
                                                    for (int iVal = 0; iVal < Cols.Count; iVal++)
                                                    {
                                                        if ((Cols[iVal].ToString() != "") && (Cols[iVal].ToString() != "-"))
                                                        {

                                                            dpValues.Items.Add(Cols[iVal][0].ToString().Trim());
                                                        }
                                                    }
                                                    for (int y = 0; y < txtStrings.Count; y++)
                                                    {
                                                        if (txtStrings.Contains(dpValues.Items[y].ToString().Trim()))
                                                        {

                                                            dpValues.Items[y].Selected = true;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (dpValues.Texts.SelectBoxCaption != "")
                                                    {
                                                        // chkList.Items.Add(txtValue.Text);
                                                        for (int colIndex = 0; colIndex < txtStrings.Count; colIndex++)
                                                        {
                                                            if (!existedList.Contains(txtStrings[colIndex][0].ToString().Trim()))
                                                            {
                                                                if ((txtStrings[colIndex][0].ToString() != "") && (txtStrings[colIndex][0].ToString() != "-"))
                                                                {
                                                                    dpValues.Items.Add(txtStrings[colIndex][0].ToString().Trim());
                                                                }

                                                            }
                                                        }
                                                        dpValues.Items.Add(dpValues.Texts.SelectBoxCaption);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strSql = "Select  isnull([Table Name],'') TableName from PDFields where [Field Name]='" + ddlField.SelectedItem.Text.ToString().Trim() + "'";
                                        string TableName = (string)GlobalValues.ExecuteScalar(strSql);
                                        if (TableName == null) { TableName = ""; }
                                        strSql = "select distinct [" + fieldName + "] from  " + tableName + " order by [" + fieldName + "]";
                                        dpValues.Items.Clear();
                                        var dsResult = GlobalValues.ExecuteDataSet(strSql);
                                        DataTable dt = dsResult.Tables[0];
                                        for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                                        {
                                            if ((dsResult.Tables[0].Rows[i][0].ToString() != "") && (dsResult.Tables[0].Rows[i][0].ToString() != "-"))
                                            {
                                                dpValues.Items.Add(dsResult.Tables[0].Rows[i][0].ToString());
                                            }
                                        }
                                    }
                                }
                                else if (strSelectedFieldName.Trim() != valuesetRows[index][1].ToString().Trim())
                                {

                                    dpValues.Items.Clear();

                                    string strSql = "select distinct [" + fieldName + "] from  " + tableName + " order by [" + fieldName + "]";

                                    var ColNames = GlobalValues.ExecuteDataSet(strSql);
                                    var Cols = ColNames.Tables[0].Rows;
                                    string sqlQuery = " select stat_value from tblStatTemplates where stat_TemplateName='" + templateName.Trim() + "' and stat_FieldName='" + ddlField.SelectedItem.Text.ToString().Trim() + "' and  UserID='" + UserName + "'";
                                    var dsResult = GlobalValues.ExecuteDataSet(sqlQuery);
                                    var dbrows = dsResult.Tables[0].Rows;
                                    List<string> existedList = new List<string>();

                                    for (int lstIndex = 0; lstIndex < dbrows.Count; lstIndex++)
                                    {
                                        if (dbrows[lstIndex][0].ToString().Trim().Length > 0)
                                        {
                                            var dbvalues = dbrows[lstIndex][0].ToString().Trim().Split(',');

                                            for (int dbIndex = 0; dbIndex < dbvalues.Length; dbIndex++)
                                            {
                                                if (dbvalues[dbIndex].Trim() != "")
                                                {
                                                    existedList.Add(dbvalues[dbIndex].Trim());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            existedList.Add(dsResult.Tables[0].Rows[lstIndex][0].ToString().Trim());
                                        }
                                    }

                                    if (ViewState["flag"] == null)
                                    {
                                        dpValues.Items.Clear();
                                        ///Required modification here
                                        for (int colIndex = 0; colIndex < Cols.Count; colIndex++)
                                        {
                                            if (!existedList.Contains(Cols[colIndex][0].ToString().Trim()))
                                            {
                                                if ((Cols[colIndex][0].ToString() != "") && (Cols[colIndex][0].ToString() != "-"))
                                                {
                                                    dpValues.Items.Add(Cols[colIndex][0].ToString().Trim());
                                                }

                                            }
                                        }
                                        //for (int lstIndex = 0; lstIndex < dbrows.Count; lstIndex++)
                                        //{
                                        //    if (dbrows[lstIndex][0].ToString().Trim().Length > 0)
                                        //    {
                                        //        for (int i = 0; i < Cols.Count; i++)
                                        //        {
                                        //            if ((Cols[i][0].ToString() != "") && (Cols[i][0].ToString() != "-"))
                                        //            {
                                        //                if (!existedList.Contains(Cols[i][0].ToString().Trim()))
                                        //                {
                                        //                    dpValues.Items.Add(Cols[i][0].ToString());
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}

                                    }
                                    else
                                    {

                                        string sqlQueryDB = " select stat_value from tblStatTemplates where stat_TemplateName='" + templateName.Trim() + "' and stat_FieldName='" + ddlField.SelectedItem.Text.ToString().Trim() + "' and  UserID='" + UserName + "'";
                                        var dsResultDB = GlobalValues.ExecuteDataSet(sqlQueryDB);
                                        List<string> existedListDB = new List<string>();

                                        for (int lstIndex = 0; lstIndex < dsResult.Tables[0].Rows.Count; lstIndex++)
                                        {
                                            if (dsResult.Tables[0].Rows[lstIndex][0].ToString().Trim().Length > 0)
                                            {
                                                string[] dbvalues = dsResult.Tables[0].Rows[lstIndex][0].ToString().Trim().Split(',');

                                                for (int dbIndex = 0; dbIndex < dbvalues.Length; dbIndex++)
                                                {
                                                    if (dbvalues[dbIndex].ToString().Trim() != "")
                                                    {
                                                        existedListDB.Add(dbvalues[dbIndex].ToString().Trim());
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                existedList.Add(dsResult.Tables[0].Rows[lstIndex][0].ToString().Trim());
                                            }
                                        }
                                        dpValues.Items.Clear();
                                        for (int colIndex = 0; colIndex < Cols.Count; colIndex++)
                                        {
                                            if (!existedList.Contains(Cols[colIndex].ToString().Trim()))
                                            {
                                                if ((Cols[colIndex][0].ToString().Trim() != "") && (Cols[colIndex][0].ToString().Trim() != "-"))
                                                {

                                                    dpValues.Items.Add(Cols[colIndex][0].ToString().Trim());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {

                                dpValues.Items.Clear();

                                dpValues.Texts.SelectBoxCaption = "";
                                string strSql = "select distinct [" + fieldName + "] from  " + tableName + " order by [" + fieldName + "]";

                                var ColsNames = GlobalValues.ExecuteDataSet(strSql);
                                var Cols = ColsNames.Tables[0].Rows;
                                if (Cols.Count > 0)
                                {
                                    for (int iText = 0; iText < Cols.Count; iText++)
                                    {
                                        if (!rowValue.Contains(Cols[iText].ToString()))
                                        {

                                            dpValues.Items.Add(Cols[iText][0].ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    strSql = "Select  isnull([Table Name],'') TableName from PDFields where [Field Name]='" + ddlField.SelectedItem.Text.ToString().Trim() + "'";
                                    string TableName = (string)GlobalValues.ExecuteScalar(strSql);
                                    if (TableName == null) { TableName = ""; }
                                    //  chkList.Items.Clear();
                                    dpValues.Items.Clear();
                                    strSql = "select distinct [" + fieldName + "] from  " + tableName + " order by [" + fieldName + "]";
                                    //"Select distinct [" + ddlField.SelectedItem.Text.ToString().Trim() + "] from " + TableName.ToString() + " where [" + ddlField.SelectedItem.Text.ToString().Trim() + "] is not null";
                                    //  DataSet dsResult = new DataSet();
                                    var dsResult = GlobalValues.ExecuteDataSet(strSql);
                                    DataTable dt = dsResult.Tables[0];
                                    for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                                    {
                                        if ((dsResult.Tables[0].Rows[i][0].ToString() != "") && (dsResult.Tables[0].Rows[i][0].ToString() != "-"))
                                        {
                                            //  chkList.Items.Add(dsResult.Tables[0].Rows[i][0].ToString());
                                            dpValues.Items.Add(dsResult.Tables[0].Rows[i][0].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                string[] values;

                if (ViewState["checkedValue"] != null)
                {
                    values = ViewState["checkedValue"].ToString().Split(',');
                    for (int count = 0; count < dpValues.Items.Count; count++)
                    {
                        for (int valCount = 0; valCount < values.Length; valCount++)
                        {
                            if (string.Equals(dpValues.Items[valCount].Value, values[valCount]))
                            {
                                dpValues.Items[valCount].Selected = true;
                            }
                        }
                    }
                }
                if (ViewState["txtValue"] != null)
                {
                    string[] txtVal = ViewState["txtValue"].ToString().Split(',');
                    for (int valCount = 0; valCount < dpValues.Items.Count; valCount++)
                    {
                        if (txtVal.Contains(dpValues.Items[valCount].Value))
                        {
                            dpValues.Items[valCount].Selected = true;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        # region Alert Messages
        private void RegisterAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string),
                "PopupWindow", "alert('" + msg + ".');", true);
        }
        #endregion

        #region Saving Manage Template
        protected void btnsave_Click(object sender, EventArgs e)
        {
            const string templateErr = "Please enter a Template name";
            const string descErr = "Please enter a Description";
            const string tempAlreadyexistErr = "Template name already Existed. Please use differnt Templates Name";
            const string rdListMessage = " Select a template name from list";
            var name = txtTemplateName.Text.Replace("'", "''");
            var description = txtDesc.Text.Replace("'", "''");
            int Index = rbList.SelectedIndex;
            switch (Index)
            {
                case 0:
                    if (txtTemplateName.Text.Trim() == "")
                    {
                        RegisterAlert(templateErr);
                    }
                    else if (txtDesc.Text.Trim() == "")
                    {
                        RegisterAlert(descErr);
                    }
                    else
                    {
                        // Verifying Template Name Exist or Not.
                        string sqlCount = "select count(*) from tblManageTemplate where mang_TemplateName = '" + name + "'  and userID='" + UserName + "' ";
                        int recCount = (int)GlobalValues.ExecuteScalar(sqlCount);

                        if (recCount > 0)
                        {
                            RegisterAlert(tempAlreadyexistErr);
                        }
                        else
                        {
                            try
                            {
                               

                                    // Inserting New Template Name.
                                    var insertStatement = "insert into tblManageTemplate values ( '" + name.Trim() + "','" + description.Trim() + "' ,'" + UserName + "')";
                                    var dataSet = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, insertStatement);
                                    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Template  created successfully.');", true);

                                    clearFields();
                                    var selectValue = "select mang_TemplateID,mang_TemplateName from tblManageTemplate where mang_TemplateName ='" + name + "'";
                                    var selectDataset = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, selectValue);

                                    ViewState["mangTemplateName"] = name.Trim();
                                    bindCreatedTemplate();                               

                            }
                            catch (Exception exp)
                            {
                            }
                        }
                    }


                    break;


                case 1:
                    if (ddlTemplateNames.SelectedItem.Text == "")
                    {


                        RegisterAlert(rdListMessage);


                    }
                    else if (txtTemplateName.Text.Trim() == "")
                    {
                        RegisterAlert(templateErr);
                    }
                    else if (txtDesc.Text.Trim() == "")
                    {
                        RegisterAlert(descErr);
                    }



                    else
                    {
                        // Updating Existed Template Name.

                        //string studycode = string.Empty;
                        //string templatename = txtTemplateName.Text.Trim();
                        //string templateID = ddlTemplateNames.SelectedValue.Trim();

                        //string sqlCount = "select count('" + templateID + "')  from tblManageTemplate where mang_TemplateName = '" + templatename + "'";
                        //var recCount = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sqlCount);
                        //if (Convert.ToInt32(recCount.Tables[0].Rows[0][0].ToString()) > 0)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert(' Template Name alread exist.');", true);
                        //}
                        //else
                        //{

                       
                            if (status != true)
                            {
                                var updateStatement = "update tblManageTemplate set mang_TemplateName ='" +
                                    txtTemplateName.Text.Trim() + "' ,mang_TemplateDesc ='" + txtDesc.Text.Trim() + "' where mang_TemplateID = '" +
                                    ddlTemplateNames.SelectedValue.Trim() + "'  and userID='" + UserName + "' ";
                                var updateSubTable = string.Format(@"Update [tblStatTemplates]
                                                          set stat_TemplateName ='{0}'
                                                          where mang_TemplateID = '{1}'  and userID='{2}'", txtTemplateName.Text.Trim(),
                                                                                                                     ddlTemplateNames.SelectedValue.Trim(),
                                                                                                                     UserName);

                                GlobalValues.ExecuteNonQuery(updateStatement);
                                GlobalValues.ExecuteNonQuery(updateSubTable);
                                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert(' Template updated  sucessfully.');", true);
                                bindTemplates();
                                btnsave.Text = "Save";
                                ddlTemplateNames.Enabled = true;
                                clearFields();
                                ViewState["mangTemplateName"] = null;

                                bindCreatedTemplate();
                                bindGroupNames();
                                grdStatDetails.Visible = false;
                            }
                        }

                        //}
                   

                    break;


            }
            Session["Edit"] = string.Empty;

        }
        #endregion
        protected void txtTemplateName_TextChanged(object sender, EventArgs e)
        {
            string studycode = string.Empty;
            string templatename = txtTemplateName.Text.Trim();
            string templateID = ddlTemplateNames.SelectedValue.Trim();

            string sqlCount = "select count('" + templatename + "')  from tblManageTemplate where mang_TemplateName = '" + templatename + "'";
            var recCount = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sqlCount);
            if (templatename != ddlTemplateNames.SelectedItem.Text.Trim())
            {
                if (Convert.ToInt32(recCount.Tables[0].Rows[0][0].ToString()) > 0)
                {
                    status = true;
                    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert(' Template Name alread exist.');", true);
                }
                else
                {
                    status = false;
                }
            }
           
        }
        #region Binding Manage Templatename
        public void bindTemplates()
        {
            ddlTemplateNames.Enabled = true;
            var connectionString = GlobalValues.strConnString;
            var selectedTemplateName = ddlTemplateNames.SelectedIndex;
            var templateNames = "select * from tblManageTemplate where UserID='" + Session["Login"].ToString() + "'";
            ddlTemplateNames.DataTextField = "mang_TemplateName";
            ddlTemplateNames.DataValueField = "mang_TemplateID";
            ddlTemplateNames.DataSource = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, templateNames);
            ddlTemplateNames.DataBind();
            ddlTemplateNames.Items.Insert(0, "");
        }
        #endregion

        #region Switching Radio button
        protected void rbList_SelectedIndexChanged(object sender, EventArgs e)
        {
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
        #endregion

        #region Manage Templatename Selection
        protected void ddlTemplateNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsUserLoggedIn) return;

            if (ddlTemplateNames.SelectedValue != "")
            {
                txtTemplateName.Enabled = true;
                txtDesc.Enabled = true;
                txtTemplateName.Text = "";
                txtDesc.Text = "";
                var sqlSelect = "select * from tblManageTemplate where mang_TemplateID = '" +
                    ddlTemplateNames.SelectedValue + "' and userID='" + UserName + "' ";
                var exeSql = SqlHelper.ExecuteReader(connectionString, System.Data.CommandType.Text, sqlSelect);
                if (exeSql.HasRows)
                {
                    if (exeSql.Read())
                    {
                        txtTemplateName.Text = exeSql["mang_TemplateName"].ToString();
                        txtDesc.Text = exeSql["mang_TemplateDesc"].ToString();
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
        #endregion

        #region Clearing Input Fields
        public void clearFields()
        {
            ddlTemplateNames.SelectedIndex = -1;
            txtTemplateName.Text = "";
            txtDesc.Text = "";
        }
        #endregion

        #region Selecting Manage Templatename
        public void bindCreatedTemplate()
        {
            // ddlSelectTemplate.Items.Clear();

            ddlSelectTemplate.Items.Insert(0, "");
            var sqlText = "select distinct(mang_TemplateName),mang_TemplateID from tblManageTemplate where UserID='" +
                Session["Login"].ToString() + "'";
            var templates = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sqlText);
            ddlSelectTemplate.DataTextField = "mang_TemplateName";
            ddlSelectTemplate.DataValueField = "mang_TemplateID";
            ddlSelectTemplate.DataSource = templates;
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
        #endregion

        #region Binding Field Names
        public void bindFields()
        {
            ddlField.Items.Clear();

            ddlField.Items.Insert(0, "");
            var sqlText = "select [Field Name],[FieldOrder],[Table Name] FROM [PDFields] " +
                       "where ( isnull([Patient Identity Field],0)=0) and [FieldOrder] not in ('" + 2147 + "','" + 2159 + "','" + 2 + "')  order by  [FieldOrder] ";
            //and statscoding='1'
            var FieldSet = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sqlText);
            var colsNames = FieldSet.Tables[0].Rows;
            if (ddlSelectTemplate.SelectedIndex == 0)
            {
                for (int colnameIndex = 0; colnameIndex < colsNames.Count; colnameIndex++)
                {
                    ddlField.Items.Add(colsNames[colnameIndex][0].ToString().Trim());
                }
            }
            else
            {


                var statQuery = "select distinct(stat_FieldName),stat_Value from tblStatTemplates where UserID = '" + UserName + "' and stat_TemplateName='" + ddlSelectTemplate.SelectedItem.Text.Trim() + "' ";

                var statValues = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, statQuery);

                var statCols = statValues.Tables[0].Rows;

                var templateQuery = "select distinct(stat_TemplateName) from tblStatTemplates where UserID = '" + UserName + "' ";

                var templateNameValues = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, templateQuery);

                var templateCols = templateNameValues.Tables[0].Rows;

                List<string> templateNameList = new List<string>();


                for (int tlistIndex = 0; tlistIndex < templateCols.Count; tlistIndex++)
                {
                    templateNameList.Add(templateCols[tlistIndex][0].ToString().Trim());
                }



                List<string> colsList = new List<string>();

                for (int colIndex = 0; colIndex < statCols.Count; colIndex++)
                {
                    colsList.Add(statCols[colIndex][0].ToString().Trim());
                }

                ///Checking Field Availability
                ///
                /// Start - Reading fields and Field values


                if (templateNameList.Contains(ddlSelectTemplate.SelectedItem.Text.ToString().Trim()))
                {


                    for (int index = 1; index < colsNames.Count; index++)
                    {
                        var colname = FieldSet.Tables[0].Rows[index][0].ToString().Trim();

                        if (colsList.Contains(colname))
                        {
                            string tablename = FieldSet.Tables[0].Rows[index][2].ToString().Trim();

                            var Query1 = "select distinct([" + colname + "]) from " + tablename + "  where [" + colname + "] is not null";


                            var count1 = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, Query1);

                            List<string> modifiedList = new List<string>();
                            var listCols = count1.Tables[0].Rows;
                            for (int listIndex = 0; listIndex < listCols.Count; listIndex++)
                            {
                                if ((listCols[listIndex][0].ToString().Trim() != "") && (listCols[listIndex][0].ToString().Trim() != "-"))
                                {
                                    modifiedList.Add(listCols[listIndex][0].ToString().Trim());
                                }
                            }

                            var Query2 = "select stat_Value from tblStatTemplates where stat_FieldName = '" + colname + "' and stat_TemplateName='" + ddlSelectTemplate.SelectedItem.Text.Trim() + "'  ";
                            var count2 = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, Query2);

                            List<string> fieldList = new List<string>();
                            for (int fIndex = 0; fIndex < count2.Tables[0].Rows.Count; fIndex++)
                            {
                                string[] countFields = count2.Tables[0].Rows[fIndex][0].ToString().Trim().Split(',');

                                for (int countIndex = 0; countIndex < countFields.Length; countIndex++)
                                {
                                    fieldList.Add(countFields[countIndex].ToString().Trim());
                                }
                            }

                            if (modifiedList.Count == fieldList.Count)
                            {
                                ddlField.Items.Remove(colname);
                            }
                            else
                            {
                                ddlField.Items.Add(colname);
                            }

                        }
                        else
                        {
                            ddlField.Items.Add(colname);
                        }
                    }

                }
                else
                {

                    for (int colnameIndex = 0; colnameIndex < colsNames.Count; colnameIndex++)
                    {
                        ddlField.Items.Add(colsNames[colnameIndex][0].ToString().Trim());
                    }

                }


            }

        }
        #endregion

        #region Field Selection
        protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpValues.Texts.SelectBoxCaption = "";
            txtGroupName.Text = "";
            Session["selectedString"] = ddlField.SelectedItem.ToString();
            bindValues();
        }
        #endregion

        #region Binding Fields
        protected void ddlSelectTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {

            bindFields();
        }
        #endregion

        #region Saving Stat Code
        protected void btnstatSave_Click(object sender, EventArgs e)
        {
            string templateName = string.Empty;
            ViewState["selected"] = ddlField.SelectedItem.Text;
            if (ddlSelectTemplate.SelectedItem.Text == "" ||
                ddlField.SelectedItem.Text == "" || dpValues.Texts.SelectBoxCaption == "" || txtGroupName.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Fields marked with asterisk (*) are required.');", true);
                ddlField.SelectedItem.Text = ViewState["selected"].ToString();
            }
            else
            {
                string templateID = ddlSelectTemplate.SelectedValue.ToString();
                templateName = ddlSelectTemplate.SelectedItem.Text.Trim();
                string strSelectedTemplateName = ddlSelectTemplate.SelectedItem.Text.Trim();
                string strSelectedField = ddlField.SelectedItem.Text.Trim();

                string strSelectedValue = dpValues.Texts.SelectBoxCaption;
                string groupName = txtGroupName.Text.Replace("'", "''");

                string sqlCount = "select count(*) from tblStatTemplates where stat_GroupName = '" + groupName.Trim() + "' and  UserID='" + UserName + "' and stat_TemplateName='" + templateName + "'";
                int recCount = (int)GlobalValues.ExecuteScalar(sqlCount);

                if (recCount > 0 && btnstatSave.Text != "Update")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Group Name already Existed. Please use differnt Group Name');", true);
                }
                else if (btnstatSave.Text == "Update")
                {

                    var updateStatement = "update tblStatTemplates set stat_FieldName ='" + ddlField.SelectedItem.Text.ToString().Trim() + "' ,stat_Value ='" + dpValues.Texts.SelectBoxCaption.Trim() + "',stat_GroupName ='" + txtGroupName.Text + "'   where stat_ID = '" + ViewState["statID"].ToString() + "' and  UserID='" + UserName + "'";

                    var dataSet = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, updateStatement);
                    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Stat code updated successfully.');", true);
                    btnstatSave.Text = "Save";
                    ddlSelectTemplate.SelectedItem.Text = "";
                    ViewState["TemplateName"] = strSelectedTemplateName.Trim();
                    dpValues.Texts.SelectBoxCaption = "";
                    txtGroupName.Text = "";
                    ddlField.SelectedItem.Text = "";
                    ViewState["selected"] = "";
                    ViewState["checkedValue"] = "";
                    ViewState["flag"] = "";
                    bindCreatedTemplate();
                    grdStatDetails.Visible = true;
                    bindCreatedTemplate();
                    bindFields();
                    dpValues.Items.Clear();


                }
                else
                {
                    try
                    {
                        string sqlGroupCount = "select count(*) from tblStatTemplates where stat_GroupName = '" + groupName.Trim() + "' and  UserID='" + UserName + "' and stat_TemplateName='" + templateName + "'";
                        int groupRecCount = (int)GlobalValues.ExecuteScalar(sqlGroupCount);

                        if (groupRecCount > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Group Name already Existed. Please use differnt Group Name');", true);
                        }
                        else
                        {
                            string insertStatCode = "insert into [tblStatTemplates] values ('" + strSelectedTemplateName.Trim() + "' ,'" + strSelectedField.Trim() + "','" + strSelectedValue.Trim() + "','" + groupName.Trim() + "'," + Convert.ToInt16(templateID) + ", '" + UserName.Trim() + "')";//,'" + templateName.Trim() + "')";
                            SqlHelper.ExecuteNonQuery(connectionString, System.Data.CommandType.Text, insertStatCode);
                            grdStatDetails.Visible = true;
                            ViewState["TemplateName"] = strSelectedTemplateName.Trim();
                            bindGroupNames();
                            ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Stat Code created successfully.');", true);
                            dpValues.Texts.SelectBoxCaption = "";
                            txtGroupName.Text = "";
                            ddlField.SelectedItem.Text = "";
                            ddlSelectTemplate.SelectedItem.Text = "";
                            bindCreatedTemplate();
                            ViewState["selected"] = "";
                            bindCreatedTemplate();
                            bindFields();

                        }
                    }
                    catch (Exception exp)
                    {
                    }
                }
            }

            bindFields();
            grdStatDetails.Visible = true;
            bindGrid();
            ddlField.SelectedItem.Text = ViewState["selected"].ToString();
        }
        #endregion

        #region Stat Group Selection
        protected void ddlStatGroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStatGroupName.SelectedItem.Text != "")
            {
                grdStatDetails.Visible = true;
                bindGrid();

            }
            else
            {
                grdStatDetails.Visible = false;
            }


        }
        #endregion

        #region Binding Grid
        public void bindGrid()
        {

            if (ddlStatGroupName.SelectedValue != "")
            {
                grdStatDetails.Visible = true;

                var selectStat = "select * from [tblStatTemplates] where stat_TemplateName ='" + ddlStatGroupName.SelectedItem.Text.Trim() + "'" + " and  UserID='" + UserName + "'";
                var statResult = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, selectStat);
                grdStatDetails.DataSource = statResult;
                grdStatDetails.DataBind();
            }
            else
            {
                grdStatDetails.Visible = false;
            }
        }
        #endregion

        #region Binding Group Names
        public void bindGroupNames()
        {

            ddlStatGroupName.Items.Clear();

            var sqlText = "select distinct(stat_TemplateName),mang_TemplateID from [tblStatTemplates] where UserID='" + UserName + "' ";
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
            else
            {
                ViewState["TemplateName"] = null;

            }
        }
        #endregion

        #region GridView Row Deleting
        protected void grdStatDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var gcells = grdStatDetails.Rows[e.RowIndex].Cells;

            if (!IsUserLoggedIn)
                return;

            try
            {

                string seletedStatID = grdStatDetails.Rows[e.RowIndex].Cells[0].Text.Trim();
                string deleteSql = "Delete from tblStatTemplates  where [stat_ID] ='" + seletedStatID + "'" + " and  UserID='" + UserName + "'";
                GlobalValues.ExecuteNonQuery(deleteSql);
                ScriptManager.RegisterStartupScript(this, typeof(string), "Delete", "alert('Stat Code Deleted Sucessfully');", true);
                bindGroupNames();
                if (btnstatSave.Text == "Update")
                {
                    btnstatSave.Text = "Save";
                }
                bindGrid();
                bindFields();
                dpValues.Texts.SelectBoxCaption = "";
                dpValues.Items.Clear();
                txtGroupName.Text = "";
                bindCreatedTemplate();
                bindValues();


            }
            catch (Exception exp)
            {
            }

        }
        #endregion

        #region GridView Row Selection
        protected void grdStatDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            var gcells = grdStatDetails.Rows[e.NewSelectedIndex].Cells;

            ViewState["statID"] = gcells[0].Text.Trim();
            ddlSelectTemplate.SelectedItem.Text = gcells[1].Text.Trim();
            ddlSelectTemplate.Enabled = false;

            ddlField.SelectedItem.Text = gcells[2].Text.Trim();

            dpValues.Texts.SelectBoxCaption = gcells[3].Text.Trim();

            if (IsHtmlEncoded(gcells[4].Text.Trim()))
            {
                txtGroupName.Text = HttpUtility.HtmlDecode(gcells[4].Text.Trim());
            }
            else
            {
                txtGroupName.Text = gcells[4].Text.Trim();
            }

            btnstatSave.Text = "Update";

            dpValues.Items.Add(dpValues.Texts.SelectBoxCaption);
            dpValues.Items.Clear();

            var strSqlValues = "select distinct [" + ddlField.SelectedItem.Text.Trim() + "]" + GlobalValues.glbFromClause + "where  [" + ddlField.SelectedItem.Text.Trim() + "] is not null";
            var ColNames = GlobalValues.ExecuteDataSet(strSqlValues);
            var cols = ColNames.Tables[0].Rows;
            string[] values = dpValues.Texts.SelectBoxCaption.ToString().Split(',');

            for (int lstIndex = 0; lstIndex < cols.Count; lstIndex++)
            {
                if ((cols[lstIndex][0].ToString() != "") && (cols[lstIndex][0].ToString() != "-"))
                {
                    dpValues.Items.Add(cols[lstIndex][0].ToString().Trim());
                }
            }
            var fieldName = ddlField.SelectedItem.Text;
            var fieldValuesQuery = "select stat_Value FROM [tblStatTemplates] where stat_FieldName ='" + fieldName + "' and stat_TemplateName='" + ddlSelectTemplate.SelectedItem.Text.Trim() + "'  and  UserID='" + UserName + "'";
            var fieldValues = GlobalValues.ExecuteDataSet(fieldValuesQuery);
            List<string> valuesList = new List<string>();
            var tableRows = fieldValues.Tables[0].Rows;
            for (int rowIndex = 0; rowIndex < tableRows.Count; rowIndex++)
            {
                string[] str = tableRows[rowIndex][0].ToString().Trim().Split(',');
                for (int strIndex = 0; strIndex < str.Length; strIndex++)
                {
                    valuesList.Add(str[strIndex].ToString().Trim());
                }
            }
            for (int j = 0; j < dpValues.Items.Count; j++)
            {
                if (valuesList.Contains(dpValues.Items[j].Text))
                {
                    if (dpValues.Texts.SelectBoxCaption.Contains(dpValues.Items[j].Text))
                    {
                        dpValues.Items[j].Selected = true;
                    }
                    else
                    {
                        dpValues.Items[j].Enabled = false;
                    }
                }
            }


            ViewState["checkValue"] = dpValues.Texts.SelectBoxCaption;// txtValue.Text;
            ViewState["flag"] = "Edit";
        }
        #endregion

        #region Encode / Decode Special Characters
        public bool IsHtmlEncoded(string text)
        {
            return (HttpUtility.HtmlDecode(text) != text);
        }
        #endregion

        #region Deleting Templatename
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!IsUserLoggedIn) return;

            if (ddlTemplateNames.SelectedItem.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, typeof(string),
                    "Delete", "alert('Please select a Template Name.');", true);
                return;
            }

            var selectTemplateNames = "Select stat_TemplateName from tblStatTemplates ";
            var TemaplateNamesList = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, selectTemplateNames);
            List<string> nameList = new List<string>();
            for (int i = 0; i < TemaplateNamesList.Tables[0].Rows.Count; i++)
            {
                nameList.Add(TemaplateNamesList.Tables[0].Rows[i][0].ToString().Trim());
            }
            if (nameList.Contains(ddlTemplateNames.SelectedItem.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "ShowConfirmation();", true);
              //  Page.ClientScript.RegisterStartupScript(this.GetType(), "showAl", "ShowConfirmation();", true);
              //  btnDelete.Attributes.Add("OnClick", "return ConfirmBox();");
              //  ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "Confirm('');", true);
            }
            else
            {



                var index = ddlTemplateNames.SelectedItem.Value;

                string deleteSql = "Delete from [tblManageTemplate]  where [mang_TemplateID] ='" +
                    index + "' and  UserID='" + UserName + "'";
                GlobalValues.ExecuteNonQuery(deleteSql);
                ScriptManager.RegisterStartupScript(this, typeof(string), "Delete", "alert('Template Name has been deleted.');", true);
                clearFields();
                bindTemplates();
                bindCreatedTemplate();
           }
        }
        protected void btnAlelrt_Click(object sender, EventArgs e)
        {
            //write your code which you wish to execute after the confirmation from the user....
            var index = ddlTemplateNames.SelectedItem.Value;

            string deleteSql = "Delete from [tblManageTemplate]  where [mang_TemplateID] ='" +
                index + "' and  UserID='" + UserName + "'";
            GlobalValues.ExecuteNonQuery(deleteSql);
            ScriptManager.RegisterStartupScript(this, typeof(string), "Delete", "alert('Template Name has been deleted.');", true);
            clearFields();
            bindTemplates();
            bindCreatedTemplate();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "showVal", "alert('Deleted Sucessfully');", true);
         //   Page.ClientScript.RegisterStartupScript(this.GetType(), "showVal", "alert('" + txtDetails.Text + "');", true);
        }
        #endregion

        #region Manage Template Fileds Reset.
        protected void btnReset_Click(object sender, EventArgs e)
        {
            clearFields();
            //bindCreatedTemplate();
            //bindTemplates();
            //bindFields();
            //bindValues();
            Session["Edit"] = string.Empty;
        }
        #endregion

        #region Clearing Statcode Fields
        public void clearStatCodeFields()
        {
            ddlSelectTemplate.SelectedItem.Text = "";
            ddlField.SelectedItem.Text = "";
            dpValues.Texts.SelectBoxCaption = "";
            txtGroupName.Text = "";
            bindCreatedTemplate();
            bindFields();
        }
        #endregion

        #region Reset Stat Fields
        protected void btnstatReset_Click(object sender, EventArgs e)
        {
            clearStatCodeFields();
            btnstatSave.Text = "Save";
            dpValues.Items.Clear();
        }
        #endregion

        #region Closing Page
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Session["Flag"] = "Stats";
            Response.Redirect("ProjectForm.aspx");
        }
        #endregion

        #region GriedView Page Index Changing
        protected void grdStatDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdStatDetails.PageIndex = e.NewPageIndex;
            bindGrid();
        }
        #endregion

        #region FieldValues Selction Process
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
                        li.Enabled =true;
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
        #endregion

        #region Values Selection
        protected void dpValues_SelectedIndexChanged(object sender, EventArgs e)
        {

            dpValues.Texts.SelectBoxCaption = getControlSelection()[0];

        }
        #endregion
    }
}