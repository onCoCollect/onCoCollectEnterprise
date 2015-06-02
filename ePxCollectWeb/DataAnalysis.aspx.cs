﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ePxCollectDataAccess;

using System.Data.SqlClient;

namespace ePxCollectWeb
{
    #region Data Analysis Processing.
    public partial class DataAnalysis : System.Web.UI.Page
    {
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
        string strConns = GlobalValues.strConnString;
        string AnalysisType = string.Empty;
        #region Loading
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
            string strQueryText = string.Empty;
            AnalysisType = Request.QueryString["AType"];
            Session["AnalysisType"] = AnalysisType;
            if (ObjfeatureSet.isAnalysis == false)
            {
                //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Sorry, you don't have permission to this functionality.');", true);
                lblError.Text = "Sorry, you don't have permission to this functionality.";
                Response.Redirect("ProjectForm.aspx");
            }

            if (!IsPostBack)
            {
                Session.Remove("Other");
                // BindRegimenDetails();
                ddlDrug.Enabled = false;
                ddlRegNames.Enabled = false;
                // BindDrugDetails();
                chkStudyAll.Enabled = false;
                chkLinesAll.Enabled = false;
                DataSet dsStudy = new DataSet();

                // Start : Binding Line Fields to Lines Dropdown
                ///
                CboLines.Items.Add(" ");
                CboLines.Items.Add("1st Line");
                CboLines.Items.Add("2nd Line");
                CboLines.Items.Add("3rd Line");
                CboLines.Items.Add("4th Line");
                CboLines.Items.Add("5th Line");
                ///
                // End : Binding Line Fields to Lines Dropdown

                // Start :  Verifying Analysis Type and Enable / Disale Controls.
                //
                switch (AnalysisType)
                {
                    case "ByDiag":
                        {

                            strQueryText = " Select distinct [Diagnosis] DiseaseName from Diagnosis with (nolock)" +
                                            " where Diagnosis is not null and Diagnosis <>'' "; //and LTRIM(RTRIM(Diagnosis)) <> '-' //Commented by premiya
                            dsStudy = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strQueryText);
                            cboStudyName.DataSource = dsStudy.Tables[0].DefaultView;
                            cboStudyName.DataTextField = "DiseaseName";
                            cboStudyName.DataValueField = "DiseaseName";
                            Session["AnalysisType"] = "ByDiag";
                            tblDiag.Visible = true;
                            ddlRegNames.Visible = true;
                            ddlDrug.Visible = true;
                            lblReg.Visible = true;
                            lblDrug.Visible = true;
                            break;


                        }
                    case "ByStudy":
                        {
                            btnOther.Visible = false;

                            strQueryText = "Select distinct [StudyName] StudyName from Studies with (nolock) " +
                                            " inner join tblStudyUsers on studies.StudyCode = tblStudyUsers.StudyCode " +
                                            " where tblStudyUsers.Users like '%" + Session["Login"].ToString() + "%' " +
                                            " AND Instances like '%-" + GlobalValues.gInstanceID.ToString() + "-%'";
                            dsStudy = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strQueryText);
                            cboStudyName.DataSource = dsStudy.Tables[0].DefaultView;
                            cboStudyName.DataTextField = "StudyName";
                            cboStudyName.DataValueField = "StudyName";
                            break;
                        }
                    case "All":
                        {
                            cboStudyName.Visible = false;
                            BindColumns();
                            break;
                        }
                }
                if (strQueryText != "")
                {
                    DataRow dr = dsStudy.Tables[0].NewRow();
                    dsStudy.Tables[0].Rows.InsertAt(dr, 0);
                    cboStudyName.DataBind();
                }

            }
            //
            // End : Verifying Analysis Type and Enable / Disale Controls.

        }

        #endregion

        #region Load Fieldnames based on Analysistype
        private void BindColumns()
        {
            string strSQL = string.Empty;
            bool bindArray = true;
            DataSet dsCols = new DataSet();
            string[] colsArr;
            chkStudyAll.Enabled = false;
            chkLinesAll.Enabled = false;

            switch (AnalysisType)
            {
                case "ByDiag":
                    {
                        if (string.IsNullOrEmpty(cboStudyName.SelectedItem.Text) || cboStudyName.SelectedItem.Text.Trim() == "-")
                        {
                            lstStudy.DataSource = "";
                            lstStudy.DataBind();
                            return;
                        }
                        //strSQL = "Select ScreenFieldsList from PDScreenMaster_Diagnosis where rtrim([DiseaseName]) ='" + cboStudyName.SelectedItem.Text + "'";                        
                        Boolean isSpecial;

                        var retval = GlobalValues.ExecuteScalar("Select SpecialLeftnavScreensNeeded from Diagnosis with (nolock) where Diagnosis = '" + cboStudyName.SelectedItem.Text + "'");
                        if (retval == null) { isSpecial = false; }
                        else { isSpecial = true; }

                        if (isSpecial)
                        {
                            strSQL = "SELECT + '['+PDFields.[Field Name] +']' FROM PDFields with (nolock) WHERE PDFields.[ISActive] = 1  and PDFields.[CalCulated Field] = 1 and isnull([Patient Identity Field],0)=0";
                            strSQL += " union Select '['+ replace([FieldListCSV],',',']'+','+'[')+']' [FieldListCSV] from PDScreenMaster_Diagnosis with (nolock) where rtrim([DiseaseName]) ='" + cboStudyName.SelectedItem.Text + "'";
                        }
                        else
                        {
                            //strSQL = "Select ScreenFieldsList from PDScreenMaster_Study  where len(ScreenFieldsList)>5 and  rtrim([StudyName]) ='" + cboStudyName.SelectedItem.Text + "'";
                            strSQL = "Select isnull([FieldListCSV],'') from PDScreenMaster_Diagnosis with (nolock) where rtrim([DiseaseName]) ='" + cboStudyName.SelectedItem.Text + "'";
                            string csvValue = (string)GlobalValues.ExecuteScalar(strSQL);
                            if (csvValue != null)
                            {
                                if (Convert.ToString(csvValue).Length > 0) { csvValue += ","; }
                            }
                            csvValue += GetCSVValuesFromDB("Select [FieldListCSV] from PDScreenMaster_Preset with (nolock)");
                            strSQL = "SELECT PDFields.[Table Name] + '.['+PDFields.[Field Name] +']' ScreenFieldsList  From PDFields with (nolock) WHERE ( isnull([Patient Identity Field],0)=0 and ((PDFields.[Field Name]) In ( ";
                            strSQL += "'" + csvValue.Replace(",", "','") + "'";
                            strSQL += " )) AND ((PDFields.ConfigurableInScreens)=1) AND ((PDFields.DATATYPE)<>'MEMO') AND ((PDFields.ISActive)=1)) OR (((PDFields.DATATYPE)<>'MEMO') AND ((PDFields.ISActive)=1) AND ((PDFields.[CalCulated Field])=1)) OR (((PDFields.[Field Name])='Status')) OR (((PDFields.[Field Name])='StatusDate')) OR (((PDFields.[Field Name])='CauseofDeath')) OR (((PDFields.[Field Name])='DateofDeath')) ORDER BY PDFields.FieldOrder";
                        }
                        bindArray = true;
                        break;
                    }
                case "ByStudy":
                    {
                        Session["StudySelect"] = cboStudyName.SelectedItem.Text;
                        strSQL = "Select [StudyFieldsListCSV] from Studies with (nolock) where rtrim([StudyName]) ='" + cboStudyName.SelectedItem.Text + "'";
                        string csvValue = (string)GlobalValues.ExecuteScalar(strSQL);
                        if (csvValue == null) { csvValue = ""; }
                        //strSQL = "Select ScreenFieldsList from PDScreenMaster_Study  where len(ScreenFieldsList)>5 and  rtrim([StudyName]) ='" + cboStudyName.SelectedItem.Text + "'";
                        strSQL = "SELECT PDFields.[Table Name] + '.['+PDFields.[Field Name] +']' ScreenFieldsList  From PDFields with (nolock) WHERE (  isnull([Patient Identity Field],0)=0 and ((PDFields.[Field Name]) In ( ";
                        strSQL += "'" + csvValue.Replace(",", "','") + "'";
                        strSQL += " )) AND ((PDFields.DataType)<>'Memo') AND ((PDFields.[Patient Identity Field])=0) AND ((PDFields.[Local Instance Field])=0) AND ((PDFields.ISActive)=1) AND ((PDFields.[1st Line Field])=0) AND ((PDFields.[2nd Line Field])=0) AND ((PDFields.[3rd Line Field])=0) AND ((PDFields.[4th Line Field])=0) AND ((PDFields.[5th Line Field])=0)) ORDER BY PDFields.FieldOrder";
                        bindArray = true;
                        break;
                    }
                case "All":
                    {
                        strSQL = "SELECT  PDFields.[Field Name] as FieldName,  '[' + PDFields.[Field Name]+']' as FieldValue From PDFields with (nolock) WHERE (( isnull([Patient Identity Field],0)=0 and ((PDFields.ConfigurableInScreens)=1) AND ((PDFields.DATATYPE)<>'MEMO') AND ((PDFields.ISActive)=1)) OR (((PDFields.DATATYPE)<>'MEMO') AND ((PDFields.ISActive)=1) AND ((PDFields.[CalCulated Field])=1)) OR (((PDFields.[Field Name])='Status')) OR (((PDFields.[Field Name])='StatusDate')) OR (((PDFields.[Field Name])='CauseofDeath')) OR (((PDFields.[Field Name])='DateofDeath'))) And [Field Name] not Like '1st Line%' ORDER BY PDFields.FieldOrder";
                        bindArray = false;
                        break;
                    }
            }
            dsCols = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strSQL);
            lstStudy.DataSource = "";
            lstStudy.DataBind();
            if (dsCols.Tables.Count <= 0) { bindArray = false; }
            else
            {
                if (dsCols.Tables[0].Rows.Count <= 0) { bindArray = false; chkStudyAll.Enabled = false; }
                else { chkStudyAll.Enabled = true; }
            }
            if (bindArray)
            {
                //colsArr = dsCols.Tables[0].Rows[0][0].ToString().Split(',');
                DataTable dtFinalCols = new DataTable();
                DataColumn dcUserName = new DataColumn("DispValue");
                DataColumn dcReportName = new DataColumn("DataValue");
                dcUserName.DataType = System.Type.GetType("System.String");
                dcReportName.DataType = System.Type.GetType("System.String");
                dtFinalCols.Columns.Add(dcUserName);
                dtFinalCols.Columns.Add(dcReportName);

                foreach (DataRow dr in dsCols.Tables[0].Rows)
                {
                    //colsArr = GlobalValues.PrepareSelectFromCSV(dsCols.Tables[0].Rows[0][0].ToString()).Replace("SELECT", "").Split(',');
                    colsArr = GlobalValues.PrepareSelectFromCSV(dr[0].ToString()).Replace("SELECT", "").Trim().Split(',');
                    DataTable dtCols = PrepareDS(colsArr);
                    dtFinalCols.Merge(dtCols);
                }
                lstStudy.DataSource = dtFinalCols;//colsArr;
                lstStudy.DataTextField = "DispValue";
                lstStudy.DataValueField = "DataValue";
                lstStudy.DataBind();
            }
            else
            {
                lstStudy.DataSource = dsCols;
                lstStudy.DataTextField = "FieldName";//dsCols.Tables[0].Columns[0].ColumnName;
                lstStudy.DataValueField = "FieldValue";//dsCols.Tables[0].Columns[1].ColumnName;
                lstStudy.DataBind();
            }

        }
        #endregion

        #region BindingRegimen Details
        public void BindRegimenDetails()
        {
            var selectRegimenDetails = "select  distinct(GroupName) FROM [PatientDrugsByLine]  inner join PatientDetails_1 on   PatientDetails_1.Patient = PatientDrugsByLine.PatientID where PatientDetails_1.SiteOfPrimary ='" + cboStudyName.SelectedItem.Text + "' ";

            var RegimenDetailsSet = SqlHelper.ExecuteDataset(strConns, CommandType.Text, selectRegimenDetails);
            if (RegimenDetailsSet != null && RegimenDetailsSet.Tables.Count > 0 && RegimenDetailsSet.Tables[0].Rows.Count > 0)
            {
                ddlRegNames.DataSource = RegimenDetailsSet;
                ddlRegNames.DataTextField = "GroupName";
                ddlRegNames.DataValueField = "GroupName";
                ddlRegNames.DataBind();
                ddlRegNames.Items.Insert(0, "");
                ddlRegNames.Enabled = true;
            }
            else
            {
                ddlRegNames.Items.Clear();
                ddlRegNames.Enabled = false;
                ddlDrug.Items.Clear();
                ddlDrug.Enabled = false;
            }

        }
        #endregion

        #region BindDrugDetails
        public void BindDrugDetails(string groupName)
        {
            var selectRegimenDetails = "select distinct(DrugName) FROM [PatientDrugsByLine] where GroupName = '" + groupName + "'";
            var RegimenDetailsSet = SqlHelper.ExecuteDataset(strConns, CommandType.Text, selectRegimenDetails);
            ddlDrug.DataTextField = "DrugName";
            ddlDrug.DataValueField = "DrugName";
            ddlDrug.DataSource = RegimenDetailsSet;
            ddlDrug.DataBind();
            ddlDrug.Items.Insert(0, "");
        }
        #endregion

        #region Read Fieldnames.
        private string GetCSVValuesFromDB(string strSQL)
        {
            string csvValue = string.Empty;
            try
            {
                DataSet ds = GlobalValues.ExecuteDataSet(strSQL);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    csvValue += "," + dr["FieldListCSV"].ToString();
                }
                return csvValue.Substring(1);
            }
            catch (Exception)
            {

                return "";
            }

        }
        #endregion

        #region Loading Fieldnames into DataSet.
        private DataTable PrepareDS(string[] ColsToBind)
        {
            DataTable table = new DataTable();

            DataColumn dcUserName = new DataColumn("DispValue");
            DataColumn dcReportName = new DataColumn("DataValue");
            dcUserName.DataType = System.Type.GetType("System.String");
            dcReportName.DataType = System.Type.GetType("System.String");
            table.Columns.Add(dcUserName);
            table.Columns.Add(dcReportName);


            for (int i = 0; i <= ColsToBind.Length - 1; i++)
            {
                DataRow dr = table.NewRow();
                if (ColsToBind[i].Split('.').Length > 1)
                {
                    dr["DispValue"] = ColsToBind[i].Split('.')[1].Replace("[", "").Replace("]", "");
                }
                else
                {
                    dr["DispValue"] = ColsToBind[i].Replace("[", "").Replace("]", "");
                }
                dr["DataValue"] = ColsToBind[i];
                table.Rows.Add(dr);
            }
            return table;
        }
        #endregion

        #region Loading Fieldnames based on dropdown list selection
        protected void cboStudyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindColumns();
            chkStudyAll.Checked = false;
            BindRegimenDetails();
        }
        #endregion

        #region Checking all fields
        protected void chkStudyAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int Cnt = 0; Cnt <= lstStudy.Items.Count - 1; Cnt++)
            {
                lstStudy.Items[Cnt].Selected = chkStudyAll.Checked;
            }
        }
        #endregion

        #region Line selection From Dropdown List
        protected void CboLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dsLines = new DataSet();
            string strSql = string.Empty;
            if (CboLines.SelectedItem.Text.Trim() != "")
            {
                strSql = "Select [Field Name] as fieldName, [Table Name]+'.['+ [Field Name]+']' as TableName from PDFields with (nolock) where ISActive = 1 and [" + CboLines.SelectedItem.Text.Trim() + " Field] = 1";
                dsLines = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strSql);
                lstLines.DataTextField = "fieldName";
                lstLines.DataValueField = "TableName";
                lstLines.DataSource = dsLines;
                if (dsLines.Tables[0].Rows.Count > 0)
                {
                    chkLinesAll.Enabled = true;
                }
            }
            else { lstLines.DataSource = ""; chkLinesAll.Enabled = false; }
            lstLines.DataBind();
        }
        #endregion

        #region Selecting all Lines from the Rightside List
        protected void chkLinesAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int Cnt = 0; Cnt <= lstLines.Items.Count - 1; Cnt++)
            {
                lstLines.Items[Cnt].Selected = chkLinesAll.Checked;
            }
        }
        #endregion

        #region sending seleted fields to Analysis Result
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {

                Session.Remove("Other");

                if (AnalysisType == "ByDiag")
                {
                    if (cboStudyName.SelectedItem.Text == "")
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please Select SiteofPrimary from the List.');", true);
                        lblError.Text = "Please Select Site of Primary from the List.";
                        return;
                    }
                    int countListOfDiagnosis = 0;

                    for (int i = 0; i <= lstStudy.Items.Count - 1; i++)
                    {
                        foreach (ListItem li in lstStudy.Items)
                        {
                            if (li.Selected == true)
                                countListOfDiagnosis += 1;
                        }
                    }

                    int CountOthersFiels = 0;
                    for (int i = 0; i <= chkOtherFields.Items.Count - 1; i++)
                    {
                        foreach (ListItem li in chkOtherFields.Items)
                        {
                            if (li.Selected == true)
                                CountOthersFiels += 1;
                        }
                    }
                    if (countListOfDiagnosis == 0 && CountOthersFiels == 0)
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please Select Site of Primary Fields from the List.');", true);
                        lblError.Text = "Please Select Site of Primary Fields from the List.";
                        return;
                    }
                    if (CboLines.SelectedItem.Text == " ")
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please Select Line from the List.');", true);
                        lblError.Text = "Please Select Line from the List.";
                        return;
                    }

                    int countListOfLine = 0;

                    for (int i = 0; i <= lstLines.Items.Count - 1; i++)
                    {
                        foreach (ListItem li in lstLines.Items)
                        {
                            if (li.Selected == true)
                                countListOfLine += 1;
                        }
                    }
                    if (countListOfLine == 0)
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please Select Line Fields from the List.');", true);
                        lblError.Text = "Please Select Line Fields from the List.";
                        return;
                    }
                    if (ddlRegNames.SelectedItem != null)
                    {
                        if (ddlRegNames.SelectedItem.Text == "")
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please select the Drug Group.');", true);//Code modified on April 13,2015-Subhashini
                            lblError.Text = "Please select the Drug Group.";
                            return;
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please select the Drug Group.');", true);//Code modified on April 13,2015-Subhashini
                        lblError.Text = "Please select the Drug Group.";
                        return;
                    }
                    if (ddlRegNames.SelectedItem != null)
                    {
                        if (ddlDrug.SelectedItem.Text == "")
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please Select Drug Name.');", true);
                            lblError.Text = "Please Select Drug Name.";
                            return;
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please Select Drug Groups.');", true);
                        lblError.Text = "Please Select Drug Groups.";
                        return;
                    }
                    Session["ByDigValues"] = ddlRegNames.SelectedItem.Text + "," + ddlDrug.SelectedItem.Text + "," + CboLines.SelectedItem.Text;
                }
                else
                {
                    int countListOfDiagnosis = 0;

                    for (int i = 0; i <= lstStudy.Items.Count - 1; i++)
                    {
                        foreach (ListItem li in lstStudy.Items)
                        {
                            if (li.Selected == true)
                                countListOfDiagnosis += 1;
                        }
                    }

                    int CountOthersFiels = 0;
                    for (int i = 0; i <= chkOtherFields.Items.Count - 1; i++)
                    {
                        foreach (ListItem li in chkOtherFields.Items)
                        {
                            if (li.Selected == true)
                                CountOthersFiels += 1;
                        }
                    }
                    if (countListOfDiagnosis == 0 && CountOthersFiels == 0)
                    {

                        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please select a field.');", true);
                        lblError.Text = "Please select a field.";
                        return;
                    }
                }
                //Session["ByDigValues"] = ddlRegNames.SelectedItem.Text + "," + ddlDrug.SelectedItem.Text +","+ CboLines.SelectedItem.Text;
                //Session["Other"] = null;
                string strQuery = string.Empty;
                string strQueryS = string.Empty;
                string strQueryO = string.Empty;
                string strQueryForSQL = string.Empty;
                //lblError.Visible = false;
                IEnumerable<string> CheckedItems = lstStudy.Items.Cast<ListItem>()
                                       .Where(i => i.Selected)
                                       .Select(i => i.Text);



                foreach (string str in CheckedItems)
                {
                    if (!str.Trim().Contains("PatientDetails_1.[SiteOfPrimary]"))
                    {
                        strQuery += "," + str;
                        strQueryForSQL += "[" + str + "],";
                        // strQuery += "[" + str + "]";
                    }
                }

                IEnumerable<string> CheckedItemsL = lstLines.Items.Cast<ListItem>()
                                      .Where(i => i.Selected)
                                      .Select(i => i.Text);

                foreach (string strS in CheckedItemsL)
                {
                    strQueryS += ", " + strS;
                    strQueryForSQL += "[" + strS.Trim() + "],";
                }
                if (strQueryS.Length > 0)
                {
                    strQuery += strQueryS.Replace("[[", "[").Replace("]]", "]");
                }

                IEnumerable<string> CheckedItemsO = chkOtherFields.Items.Cast<ListItem>()
                                      .Where(i => i.Selected)
                                      .Select(i => i.Text);

                foreach (string strO in CheckedItemsO)
                {
                    strQueryO += ", " + strO;
                    strQueryForSQL += "[" + strO.Trim() + "],";
                }
                if (strQueryO.Length > 0) { strQuery += strQueryO.Replace("[[", "[").Replace("]]", "]"); }

                string strWhere = string.Empty;
                if (AnalysisType == "ByStudy")
                {
                    strWhere = "1=1";
                }
                else if (AnalysisType == "ByDiag")
                {
                    strWhere = "[SiteOfPrimary] ='" + cboStudyName.Text.ToString() + "'";
                }

                if (strQuery.Length > 0)
                {
                    //strQuery = "Select PatientDetails_0.PatientID, PatientDetails_1.Diagnosis  " + strQuery + "  ";
                    // Session["SQLSTROTHER"] = strQuery;
                    //Session["UpdSQL"] = strQuery;
                    Session["part1"] = strQuery;
                    //      strQuery = " PatientID, SiteOfPrimary  " + strQuery + "  ";
                    GlobalValues.QueryString = strQuery.ToString();
                    //Commented by Premiya on 
                    //strQueryForSQL = strQueryForSQL.Substring(0, strQueryForSQL.Length - 1);
                    strQueryForSQL = strQueryForSQL.Trim().TrimEnd(',').TrimStart(',');
                    Session["baseString"] = strQueryForSQL;
                    Session["ds"] = null;
                    Session["Whr" + Session.SessionID.ToString()] = strWhere;
                    Session["AnalysisType"] = AnalysisType;
                    Response.Redirect("AnalysisResult.aspx?SN=Toxicity&Type=AnalRes",false);

                    //string url = "AnalysisResultSet.aspx";
                    //string s = "window.open('" + url + "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');";
                    //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
                else
                {
                    //lblError.Text = "Please select a field."; //"Please select the items for which you would like to capture information";
                    //lblError.Visible = true;
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);
                    //return;
                }
            }
            catch (Exception ex)
            {
                
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);
                throw ;
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "HideProgress();", true);
            }
        }
        #endregion

        #region Loading Other Fields
        protected void btnOther_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            string strSQL = string.Empty;
            DataSet dsCols = new DataSet();

            strSQL = "SELECT PDFields.[Field Name] as FieldName, PDFields.[Table Name]+ '.['+ PDFields.[Field Name]+']' as ColValue From PDFields with (nolock) WHERE (PDFields.ConfigurableInScreens=1  AND PDFields.ISActive=1 And [Field Name] not like '1st Line%') Order by FieldOrder";

            dsCols = SqlHelper.ExecuteDataset(strConns, CommandType.Text, strSQL);

            dsCols = GetUnUsedColumns(dsCols);

            lstStudy.DataSource = "";
            // lstStudy.DataBind();
            if (dsCols.Tables.Count > 0)
            {

                if (dsCols.Tables[0].Rows.Count > 0)
                {
                    chkOtherFields.DataSource = dsCols;


                    chkOtherFields.DataTextField = dsCols.Tables[0].Columns[0].ColumnName;
                    chkOtherFields.DataValueField = dsCols.Tables[0].Columns[1].ColumnName;
                    chkOtherFields.DataBind();
                    if (Session["Other"] != null)
                    {
                        for (int i = 0; i < chkOtherFields.Items.Count; i++)
                        {

                            if (Session["Other"].ToString().Contains(chkOtherFields.Items[i].Text.ToString()))
                            {
                                chkOtherFields.Items[i].Selected = true;

                            }
                        }
                    }
                }

            }
            updConfirm.Update();
            ModalPopupExtender1.Show();

        }
        #endregion

        #region Unused Columns
        private DataSet GetUnUsedColumns(DataSet dsCols)
        {
            DataSet ds = new DataSet();
            DataTable dataTable = ds.Tables.Add();
            dataTable.Columns.Add("ColName");
            dsCols.Tables[0].Columns.Add("DispFlag");
            DataTable dtCols = dsCols.Tables[0];
            string SQLstr = "select distinct replace(replace(FieldListCSV,']',''),'[','') FieldListCSV from PDScreenMaster_Study with (nolock)  " +
                        " union select distinct replace(replace(FieldListCSV,']',''),'[','') FieldListCSV from PDScreenMaster_Preset with (nolock) " +
                        " union select distinct replace(replace(FieldListCSV,']',''),'[','') FieldListCSV from PDScreenMaster_Diagnosis with (nolock)  where DiseaseName = '" + GlobalValues.gDisease + "' ";

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
        #endregion

        #region Other Field Selection
        protected void btnOk_Click(object sender, EventArgs e)
        {
            string selectedCheckItems = string.Empty;

            for (int i = 0; i < chkOtherFields.Items.Count; i++)
            {

                if (chkOtherFields.Items[i].Selected)
                {
                    selectedCheckItems += chkOtherFields.Items[i].Text + ",";
                }

            }
            Session["Other"] = selectedCheckItems;

            if (chkOtherFields.SelectedItem == null)
            {
                lblMessage.Visible = false;
                ModalPopupExtender1.Show();
                UpdatePanel1.Update();
                lblMessage.Visible = true;
                lblMessage.Text = "Please select a field.";
            }
            else
            {

                ModalPopupExtender1.Hide();
                UpdatePanel1.Update();
            }

        }
        #endregion

        #region Closing Data Analysis Page
        protected void btnClose_Click(object sender, EventArgs e)
        {
            //Response.Redirect("SearchPatient.aspx");
            // Session["CloseFlag"] = "DataAnalysis";
            Session["Flag"] = "DrugAnalysis";
            Response.Redirect("ProjectForm.aspx", false);

        }
        #endregion

        protected void ddlRegNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDrug.Enabled = true;
            BindDrugDetails(ddlRegNames.SelectedItem.Text);
        }

    }
    #endregion
}