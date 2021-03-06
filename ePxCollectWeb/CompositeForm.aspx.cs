﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ePxCollectDataAccess;
using System.Web.Services;

namespace ePxCollectWeb
{
    public partial class CompositeForm : System.Web.UI.Page
    {
        string strConns = GlobalValues.strConnString;
        DataSet dsPDFlds = new DataSet();
        static bool flagUrlReferrer = false;


        protected void Page_Init(object sender, EventArgs e)
        {

            try //Code modified on April 30,2015-Subhashini
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "FormChange", "getFormType();", true);

                string sqlStr = string.Empty;
                if (Request.QueryString["CN"] != null && Convert.ToString(Request.QueryString["CN"]) != "")
                {
                    string ColStr = Convert.ToString(Request.QueryString["CN"]);
                    this.Title = ColStr;
                    ColStr = ColStr.Replace("_-", " ");//Added for the code to work well-May 6,2015-Subhashini
                    sqlStr = "Select CompositeChildFieldsCSV from PDFields with (nolock) where [Field Name]='" + ColStr.Replace("_-", " ") + "'";
                    DataSet ds = new DataSet();
                    ds = SqlHelper.ExecuteDataset(strConns, CommandType.Text, sqlStr);
                    ColStr = Convert.ToString(ds.Tables[0].Rows[0][0]);
                    string pdSQL = sqlStr.Replace("Select ", "").Replace("],[", ",").Replace("[", "").Replace("]", "");
                    pdSQL = "Select * from PDFields with (nolock) where IsActive=1  and [Field Name] in ('"
                         + ColStr.Replace(",", "','").Trim()
                         + "') order by [Table Name],FieldOrder";
                    Session["SaveSQL"] = ColStr;
                    dsPDFlds = SqlHelper.ExecuteDataset(strConns, CommandType.Text, pdSQL);
                    sqlStr = "Select [" + ColStr.Replace(",", "],[") + "] " + GlobalValues.glbFromClause + " Where PatientDetails_0.PatientID='" + Convert.ToString(Session["PatientID"]).Trim() + "'";
                    ds = SqlHelper.ExecuteDataset(strConns, CommandType.Text, sqlStr);
                    LoadControls(ds);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadControls(DataSet dsForm)
        {
            try
            {
                string strFldVal;
                OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
                foreach (DataColumn dcValues in dsForm.Tables[0].Columns)
                {
                    strFldVal = string.Empty;
                    DataRow[] dr = dsPDFlds.Tables[0].Select("[Field Name]='" + dcValues.ColumnName.ToString() + "'");
                    if (dr.Length > 0)
                    {
                        DataRow drValues = dsForm.Tables[0].Rows[0];

                        Label lbCtl = new Label();
                        lbCtl.Text = dcValues.ColumnName.ToString() + "&nbsp;&nbsp;";
                        ViewState["LabelText"] = dcValues.ColumnName.ToString();
                        lbCtl.Width = 280;
                        lbCtl.CssClass = "LabelRight";

                        phProject.Controls.Add(lbCtl);
                        if (!(bool)dr[0]["Calculated Field"])
                        {
                            strFldVal = drValues[dcValues.ColumnName].ToString();
                            if ((bool)dr[0]["Patient Identity Field"])
                            {
                                try
                                {
                                    strFldVal = objEnc.Decrypt(strFldVal);
                                }
                                catch (Exception)
                                {

                                    //throw;
                                }

                            }
                            if (dcValues.ColumnName.Contains("Line Drug Group"))
                            {
                                TextBox txtCtl = new TextBox();
                                Button btnClick = new Button();
                                txtCtl.Text = strFldVal.ToString();
                                txtCtl.Width = 250;

                                //txtCtl.Width = strFldVal.ToString().Length <= 0 ? 250 : (strFldVal.ToString().Length * 8) + 50;
                                btnClick.Text = "...";
                                btnClick.ID = "btn" + dcValues.ColumnName;
                                btnClick.UseSubmitBehavior = false;
                                btnClick.CommandArgument = dr[0]["Table Name"].ToString() + "." + dr[0]["Field Name"].ToString();

                                txtCtl.ReadOnly = true;
                                btnClick.OnClientClick = "javascript:fnDrugFormC(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"" + dcValues.ColumnName.Replace(" ", "_-") + "_TX_Y\",\"[" + strFldVal.ToString() + "]\"); return false;";
                                //btnClick.Click += new EventHandler(ShowWindow);
                                //btnClick.Attributes.Add("Onclick", "ShowWindow()");

                                //txtCtl.Enabled = false;                                                               
                                txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "TX_Y";
                                txtCtl.Attributes.Add("onkeypress", "return handleKey()");
                                SetValidationsForUserInput(txtCtl, (dr[0]["DataType"]).ToString().ToUpper(), (dr[0]["Field Width"]).ToString().ToUpper());

                                txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                txtCtl.EnableViewState = true;
                                phProject.Controls.Add(txtCtl);
                                phProject.Controls.Add(btnClick);

                            }
                            else
                                if ((string)dr[0]["DataType"] == "DATE")
                                {
                                    TextBox dtP = new TextBox();

                                    dtP.ID = dcValues.ColumnName.Replace(" ", "_-") + "_DT_Y";
                                    dtP.Width = Unit.Pixel(198);
                                    dtP.Height = 30;//"images/Calendar.png"
                                    dtP.CssClass = "dynamictext";
                                    //dtP.LabelHeaderText = lbCtl.Text.Replace(":", "").Trim();
                                    if (strFldVal.ToString() != "")
                                    {
                                        dtP.Text = strFldVal.ToString();
                                    }
                                    else
                                    {
                                        dtP.Text = DateTime.Now.ToString();
                                    }
                                    phProject.Controls.Add(dtP);

                                    //SlimeeLibrary.DatePicker dtP = new SlimeeLibrary.DatePicker();
                                    //dtP.ID = dcValues.ColumnName.Replace(" ", "_-") + "_DT_Y";
                                    //dtP.Width = 150;
                                    //dtP.PaneWidth = 150;
                                    ////dtP.Height = 30;///"images/Calendar.png"
                                    //if (strFldVal.ToString() != "")
                                    //{
                                    //    dtP.SelectedDate = Convert.ToDateTime(strFldVal.ToString());
                                    //}
                                    //else
                                    //{
                                    //    dtP.SelectedDate = DateTime.Today;
                                    //}
                                    //phProject.Controls.Add(dtP);

                                    //TextBox txtCtl = new TextBox();
                                    //txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_DT_Y";
                                    //txtCtl.EnableViewState = true;
                                    //txtCtl.Attributes.Add("onkeypress", "return handleKey()");
                                    //ImageButton img = new ImageButton();
                                    //img.ID = dcValues.ColumnName.Replace(" ", "_-") + "_DT_IMG";
                                    //img.ImageUrl = "images/Calendar.png";
                                    //img.AlternateText = "Click to select a date";
                                    //AjaxControlToolkit.CalendarExtender dtP = new AjaxControlToolkit.CalendarExtender();
                                    //dtP.ID = dcValues.ColumnName.Replace(" ", "_-") + "_DT_AJ";
                                    //dtP.TargetControlID = dcValues.ColumnName.Replace(" ", "_-") + "_DT_Y";
                                    //dtP.PopupButtonID = dcValues.ColumnName.Replace(" ", "_-") + "_DT_IMG";//dcValues.ColumnName.Replace(" ", "_-") + "_DT_Y";
                                    //dtP.CssClass = "MyCalendar";
                                    //dtP.Format = "MMMM d, yyyy";
                                    //if (strFldVal.ToString() != "")
                                    //{
                                    //    DateTime dt = Convert.ToDateTime(strFldVal.ToString());
                                    //    dtP.SelectedDate = dt;//Convert.ToDateTime(strFldVal.ToString());
                                    //    txtCtl.Text = String.Format("{0:MMM d, yyyy}", dt); //strFldVal.ToString();
                                    //}
                                    //else
                                    //{
                                    //    dtP.SelectedDate = DateTime.Today;
                                    //}
                                    //phProject.Controls.Add(txtCtl);
                                    //phProject.Controls.Add(img);
                                    //phProject.Controls.Add(dtP);
                                }
                                else
                                    if ((bool)dr[0]["CompositeField"])
                                    {
                                        TextBox txtCtl = new TextBox();
                                        Button btnClick = new Button();
                                        txtCtl.Text = strFldVal.ToString();
                                        //txtCtl.Width = strFldVal.ToString().Length <= 0 ? 250 : (strFldVal.ToString().Length * 8) + 50;
                                        txtCtl.Width = 250;
                                        btnClick.Text = "...";
                                        btnClick.ID = "btn" + dcValues.ColumnName;
                                        btnClick.UseSubmitBehavior = false;
                                        btnClick.CommandArgument = dr[0]["Table Name"].ToString() + "." + dr[0]["Field Name"].ToString();
                                        btnClick.OnClientClick = "javascript:fnPopWindowC(\"" + "CompositeForm.aspx?CN=" + dcValues.ColumnName.Replace(" ", "_-") + "\",\"" + dcValues.ColumnName.ToString() + "\"); return false;";
                                        //txtCtl.Enabled = false;
                                        txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "TX_Y";
                                        txtCtl.Attributes.Add("onkeypress", "return handleKey()");
                                        txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                        txtCtl.EnableViewState = true;
                                        txtCtl.ReadOnly = true;
                                        SetValidationsForUserInput(txtCtl, (dr[0]["DataType"]).ToString().ToUpper(), (dr[0]["Field Width"]).ToString().ToUpper());
                                        phProject.Controls.Add(txtCtl);
                                        phProject.Controls.Add(btnClick);
                                    }
                                    else
                                        if ((bool)dr[0]["RightPick"] || (bool)dr[0]["SelectiveRightSinglePickField"])
                                        {
                                            TextBox txtCtl = new TextBox();
                                            Button btnClick = new Button();
                                            txtCtl.Text = strFldVal.ToString();
                                            txtCtl.Width = 250;
                                            txtCtl.ReadOnly = true;
                                            //txtCtl.Width = strFldVal.ToString().Length <= 0 ? 250 : (strFldVal.ToString().Length * 8) + 50;
                                            btnClick.Text = "...";
                                            btnClick.ID = "btn" + dcValues.ColumnName;
                                            btnClick.UseSubmitBehavior = false;
                                            btnClick.CommandArgument = dr[0]["Table Name"].ToString() + "." + dr[0]["Field Name"].ToString();
                                            if ((bool)dr[0]["RightPick"])
                                            {
                                                btnClick.OnClientClick = "javascript:fnPopupC(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"" + dcValues.ColumnName.Replace(" ", "_-") + "_TX_Y\"); return false;";
                                            }
                                            else
                                            {
                                                txtCtl.ReadOnly = true;
                                                btnClick.OnClientClick = "javascript:fnSingleSelectC(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"" + dcValues.ColumnName.Replace(" ", "_-") + "_TX_Y\",\"[" + strFldVal.ToString() + "]\"); return false;";
                                                //btnClick.Click += new EventHandler(ShowWindow);
                                                //btnClick.Attributes.Add("Onclick", "ShowWindow()");
                                            }
                                            //txtCtl.Enabled = false;
                                            txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_TX_Y";
                                            txtCtl.Attributes.Add("onkeypress", "return handleKey()");
                                            txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                            txtCtl.EnableViewState = true;
                                            phProject.Controls.Add(txtCtl);
                                            phProject.Controls.Add(btnClick);
                                        }
                                        else
                                            if ((bool)dr[0]["RightMultiPick"] || (bool)dr[0]["SelectiveRightMultiPickField"])
                                            {
                                                TextBox txtCtl = new TextBox();
                                                Button btnClick = new Button();
                                                txtCtl.Text = strFldVal.ToString();
                                                //txtCtl.Width = strFldVal.ToString().Length <= 0 ? 250 : (strFldVal.ToString().Length * 8) + 50;
                                                txtCtl.Width = 250;
                                                btnClick.Text = "...";
                                                btnClick.ID = "btn" + dcValues.ColumnName;
                                                btnClick.UseSubmitBehavior = false;
                                                btnClick.CommandArgument = dr[0]["Table Name"].ToString() + "." + dr[0]["Field Name"].ToString();
                                                if ((bool)dr[0]["RightMultiPick"])
                                                {
                                                    btnClick.OnClientClick = "javascript:fnPopupRightMultiPickOthers(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"" + dcValues.ColumnName.Replace(" ", "_-") + "_TX_Y\"); return false;";
                                                }
                                                else
                                                {
                                                    btnClick.OnClientClick = "javascript:fnMultiSelectC(\"" + dr[0]["Table Name"].ToString() + ".[" + dr[0]["Field Name"].ToString() + "]\",\"" + dcValues.ColumnName.Replace(" ", "_-") + "_TX_Y\",\"[" + (strFldVal.ToString()) + "]\"); return false;";
                                                }
                                                txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_TX_Y";
                                                txtCtl.Attributes.Add("onkeypress", "return handleKey()");
                                                txtCtl.Attributes.Add("readonly", "readonly");
                                                txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                                txtCtl.EnableViewState = true;

                                                phProject.Controls.Add(txtCtl);
                                                phProject.Controls.Add(btnClick);
                                            }
                                            else if ((bool)dr[0]["FixedDrop"])
                                            {
                                                DropDownList dpLst = new DropDownList();
                                                dpLst.Width = 250;
                                                //dpLst.Width = strFldVal.ToString().Length <= 0 ? 250 : (strFldVal.ToString().Length * 8) + 50;
                                                //dpLst.Enabled = false;
                                                dpLst.ID = dcValues.ColumnName.Replace(" ", "_-") + "_FD_Y";
                                                PopulateDropDownValues(dr[0]["FieldValues"].ToString(), dpLst);
                                                dpLst.SelectedValue = strFldVal.ToString();
                                                dpLst.ViewStateMode = ViewStateMode.Enabled;
                                                phProject.Controls.Add(dpLst);
                                            }
                                            else
                                            {
                                                TextBox txtCtl = new TextBox();
                                                txtCtl.Text = strFldVal.ToString();
                                                Int32 intWidth = strFldVal.ToString().Length;
                                                //txtCtl.Width = intWidth <= 0 ? 250 : intWidth > 50 ? 250 : (intWidth * 8) + 50;
                                                txtCtl.Width = 250;
                                                //txtCtl.Wrap = true;
                                                if (intWidth > 50) { txtCtl.TextMode = TextBoxMode.MultiLine; }
                                                // txtCtl.Enabled = false;
                                                if (dcValues.ColumnName == "PatientID") { txtCtl.ReadOnly = true; }
                                                txtCtl.ID = dcValues.ColumnName.Replace(" ", "_-") + "_TX_Y";
                                                txtCtl.ViewStateMode = ViewStateMode.Enabled;
                                                txtCtl.EnableViewState = true;
                                                SetValidationsForUserInput(txtCtl, (dr[0]["DataType"]).ToString().ToUpper(), (dr[0]["Field Width"]).ToString().ToUpper());

                                                phProject.Controls.Add(txtCtl);
                                            }

                            phProject.Controls.Add(new LiteralControl("<br />"));
                        }
                    }
                }
            }
            catch (Exception) { }

            // Panel1.Visible = true;
        }

        void PopulateDropDownValues(string strCSVs, DropDownList dl)
        {
            string[] strItems;
            strItems = strCSVs.Split(',');
            foreach (string strValue in strItems)
            {
                dl.Items.Add(strValue);
            }
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            flagUrlReferrer = false;
            string[,] strretval = SaveForm();
            string finalvalue = string.Empty;
            if (strretval[0, 0].ToString() == "False")
                finalvalue = "123123123";
            else
                finalvalue = strretval[0, 1].ToString();

            sendValueToJS(finalvalue);
        }

        public void sendValueToJS(string strVal)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "sendValuetoParent('" + strVal + "');", true);
        }

        public string SaveCompositeFormonBtnSave()
        {
            flagUrlReferrer = false;
            string[,] strretval = SaveForm();
            string finalvalue = string.Empty;
            if (strretval[0, 0].ToString() == "False")
                finalvalue = "123123123";
            else
                finalvalue = strretval[0, 1].ToString();
            return finalvalue;
        }
        public string[,] SaveForm()
        {
            try
            {
                string strValue = string.Empty;
                string strTable = string.Empty;
                string strwhere = string.Empty;
                string strCol = string.Empty;
                string strColVal = string.Empty;
                string strUPD = Convert.ToString(Session["UpdSQL"]).Replace("Select", "");
                string[] arrFlds;
                string[] arrTbl;
                string[,] strval = new string[1, 2];
                double retval = 0;
                bool bval = false;
                OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
                //System.Collections.ArrayList TablesList = new System.Collections.ArrayList();
                string[] arrVals = new string[10];

                arrFlds = strUPD.Split(',');
                foreach (Control ctl in phProject.Controls)
                {
                    if (ctl.ID != null)
                    {

                        if (ctl.ID.ToString().EndsWith("_Y"))
                        {
                            strCol = ctl.ID.ToString().Substring(0, ctl.ID.Length - 5);
                            if (ctl.ID.ToString().EndsWith("_TX_Y"))
                            {
                                TextBox txtCtl = new TextBox();
                                txtCtl = (TextBox)ctl;
                                double value = 0;
                                if (double.TryParse(txtCtl.Text.ToString(), out value))
                                    retval += string.IsNullOrEmpty(txtCtl.Text) ? 0.0 : double.Parse(txtCtl.Text);
                                else
                                    retval += string.IsNullOrEmpty(txtCtl.Text) ? 0.0 : 0.0;
                                strColVal = "'" + txtCtl.Text.ToString() + "'";

                                if (!string.IsNullOrEmpty(txtCtl.Text))
                                {
                                    bval = true;
                                }
                            }
                            else if (ctl.ID.ToString().EndsWith("_RTX_Y"))
                            {
                                TextBox txtCtl = new TextBox();
                                txtCtl = (TextBox)ctl;
                                strColVal = "'" + txtCtl.Text.ToString() + "'";
                                strCol = ctl.ID.ToString().Substring(0, ctl.ID.Length - 6);
                                if (!string.IsNullOrEmpty(txtCtl.Text))
                                {
                                    bval = true;
                                }
                            }
                            else if (ctl.ID.ToString().EndsWith("_FD_Y"))
                            {
                                DropDownList dpLst = new DropDownList();
                                dpLst = (DropDownList)ctl;
                                strColVal = "'" + dpLst.SelectedItem.Text.ToString() + "'";
                            }
                            else
                                if (ctl.ID.ToString().EndsWith("_DT_Y"))
                                {
                                    TextBox dtP = new TextBox();
                                    dtP = (TextBox)ctl;
                                    strColVal = "'" + dtP.Text + "'";
                                    TextBox txtCtl = new TextBox();
                                    txtCtl = (TextBox)ctl;
                                    strColVal = "'" + txtCtl.Text.ToString() + "'";
                                    //SlimeeLibrary.DatePicker dtP = new SlimeeLibrary.DatePicker();
                                    //dtP = (SlimeeLibrary.DatePicker)ctl;
                                    //strColVal = "'" + dtP.SelectedDate.ToString("dd MMM yyyy") + "'";
                                }
                            //for (int i = 0; i < arrFlds.Length; i++)
                            //{
                            //        arrTbl = arrFlds[i].Split('.');
                            //      strTable = arrTbl[1].ToString().Replace("\n","");
                            //      if (strTable.Contains(strCol.Replace("_-", " ")))
                            //    {
                            //        arrVals[Convert.ToInt32(arrTbl[0].ToString().Substring(arrTbl[0].Length-1))]+=","+ strCol + "=" +strColVal;
                            //        break;
                            //    }
                            //}
                            DataRow[] drC = dsPDFlds.Tables[0].Select("[Field Name] ='" + strCol.Replace("_-", " ") + "'");

                            foreach (DataRow dr in drC)
                            //DataRow dr = drC.Rows[0];
                            {
                                strCol = "[" + strCol + "]";
                                if ((bool)dr["Patient Identity Field"])
                                {
                                    strColVal = "'" + objEnc.Encrypt(strColVal.Replace("'", "")) + "'";
                                }
                                if (strColVal.Trim() != string.Empty && strColVal.Trim() == "''")
                                { strColVal = "null"; }
                                arrVals[Convert.ToInt32(dr["Table Name"].ToString().Substring(dr["Table Name"].ToString().Length - 1))] += "," + strCol + "=" + strColVal;
                                //strValue += "," + strCol + strColVal;
                            }
                            strValue += "," + strCol + strColVal;

                        }

                    }
                }

                for (int j = 0; j < arrVals.Length; j++)
                {
                    strValue = arrVals[j];
                    if (strValue != null)
                    {
                        strValue = strValue.ToString().Substring(1).Replace("_-", " ");
                        strTable = "PatientDetails_" + j.ToString();
                        if (strTable == "PatientDetails_0")
                        {
                            strwhere = " Where PatientiD = '" + Convert.ToString(Session["PatientID"]).Trim() + "'";
                        }
                        else
                        {
                            strwhere = " Where Patient = '" + Convert.ToString(Session["PatientID"]).Trim() + "'";
                        }
                        strValue = "Update " + strTable + " Set " + strValue + strwhere;
                        Int32 updStat;
                        updStat = SqlHelper.ExecuteNonQuery(strConns, CommandType.Text, strValue);
                    }

                }
                string ColStr = Convert.ToString(Request.QueryString["CN"]);
                ColStr = ColStr.Replace("_-", " ");
                if (ColStr == "Total Cost of Primary Treatment(in INR)")
                    bval = true;
                else
                    bval = false;
                strval[0, 0] = bval.ToString();
                strval[0, 1] = retval.ToString();

                return strval;
                //Audit record required only for patient cahnge
                //GlobalValues.WriteAuditRecord(Convert.ToString( Session["Login"]), Convert.ToString(Session["PatientID"]), "Record Updated");
            }
            catch (Exception e)
            {
                int a = 10;

                throw;
            }
        }


        protected void SetValidationsForUserInput(TextBox txtCtl, string DataType, string MaxLength)
        {
            int maxLength = 255;
            try
            {
                maxLength = Convert.ToInt32(MaxLength);
            }
            catch { }

            if (DataType != "MEMO")
                txtCtl.MaxLength = maxLength;
            if (DataType == "MEMO")
                txtCtl.MaxLength = 1000;

            if (DataType == "LONG")
                txtCtl.Attributes.Add("onkeypress", "return ValidateTextBoxForDataTypeLongINT(event,'" + txtCtl.Text + "',this,'" + ViewState["LabelText"].ToString() + "');");
            if (DataType == "TEXT" || DataType == "MEMO")
            {
                if (txtCtl.ID == "ctl00_MainContent_Postal_-Code_TX_Y")
                {
                    txtCtl.Attributes.Add("onkeypress", "return ValidateTextBoxForDataTypeTextPostalCode(event,'" + txtCtl.Text + "',this,'" + ViewState["LabelText"].ToString() + "');");
                }
                else if (txtCtl.ID == "R/T_-Boost_-Dose_TX_Y")
                {
                    txtCtl.Attributes.Add("onkeypress", "return ValidateTextBoxForDataTypeTextAlphaNumericCommaSpaceHypen(event,'" + txtCtl.Text + "',this,'" + ViewState["LabelText"].ToString() + "');");
                    txtCtl.Attributes.Add("onkeyup", "return validateKeyupforRTBoostDose(this);");
                }
                else
                {
                    txtCtl.Attributes.Add("onkeypress", "return ValidateTextBoxForDataTypeTextAlphaNumericCommaSpaceHypen(event,'" + txtCtl.Text + "',this,'" + ViewState["LabelText"].ToString() + "');");
                    txtCtl.Attributes.Add("onkeyup", "return validateKeyup(this);");
                }

            }

            if (DataType == "SINGLE")
                txtCtl.Attributes.Add("onkeypress", "return ValidateTextBoxForDataTypeSingleIsNumericWithDot1(event,'" + txtCtl.Text + "',this,'" + ViewState["LabelText"].ToString() + "');");
            if (DataType == "EMAIL")
            {
                txtCtl.Attributes.Add("onkeypress", "return ValidateTextBoxForDataTypeEmail(event,'" + txtCtl.Text + "',this,'" + ViewState["LabelText"].ToString() + "');");
            }


        }

        //protected void btnClose_Click(object sender, EventArgs e)
        //{
        //    flagUrlReferrer = false;
        //    string script = "<script type=\"text/javascript\"> CloseWindow(); </script>";
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "myscript", script);
        //}


    }

}