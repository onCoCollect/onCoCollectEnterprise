using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace ePxCollectWeb
{
    public partial class DeseaseScreen : System.Web.UI.Page
    {
        string FldName = string.Empty;
        string patientId = string.Empty;
        string fldVal = string.Empty;
        string strLine = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["fldVal"] = "";

            FldName = Convert.ToString(Request.QueryString["FN"]);
            if (FldName.Split('.').Length > 0) { FldName = FldName.Split('.')[1].ToString(); }
            FldName = FldName.Replace("[", "").Replace("]", "");
            fldVal = Convert.ToString(Request.QueryString["Val"]).Replace("*ampersand*", "&").Replace("*plus*", "+");
            fldVal = fldVal.Replace("[", "").Replace("]", "");

            lblValue.Text = fldVal;
            strLine = FldName.Substring(0, 8);
            if (Session["PatientID"] == null)
            {
                Response.Redirect("SearchPatient.aspx");
            }
            patientId = Convert.ToString(Session["PatientID"]);
            this.Title = FldName + ":" + patientId;
            if (!IsPostBack)
            {
                divMain.Visible = true;
                divPopup.Visible = false;
                BindGrid();
            }
        }

        protected void dpAllValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selVal = string.Empty;
            if (dpAllValues.SelectedItem.Text != "")
            {
                string SQL = string.Empty;
                selVal = dpAllValues.SelectedValue.ToString();
                if (dpAllValues.DataTextField == "SiteOfPrimary")
                {
                    SQL = "Select DrugGroups from DrugGroupsByDiagnosis with (nolock) where DiagnosisName ='" + dpAllValues.SelectedItem.Text + "'";
                }
                else
                {
                    SQL = "Select DrugGroups from DrugGroupsByStudy with (nolock) where Study ='" + dpAllValues.SelectedItem.Text + "'";
                }

                PopulateValues(SQL);
                // BindDropDowns();
                //dpByDiag.SelectedItem.Text = "";
                //dpByStudy.SelectedItem.Text = "";
                dpAllValues.SelectedValue = selVal;
                lstValues.Focus();
            }
        }

        //protected void dpByDiag_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string selVal = string.Empty;
        //    if (dpByDiag.SelectedItem.Text != "")
        //    {
        //        selVal = dpByDiag.SelectedValue.ToString();
        //        string SQL = "Select FieldValue from FieldValues_ByDiagnosis where FieldName ='" + FldName + "' and DiagnosisName = '" + dpByDiag.SelectedItem.Text + "'";
        //        PopulateValues(SQL);
        //        BindDropDowns();
        //        dpAllValues.SelectedItem.Text = "";
        //        dpByStudy.SelectedItem.Text = "";
        //        dpByDiag.SelectedValue = selVal;
        //        lstValues.Focus();
        //    }
        //}

        //protected void dpByStudy_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string selVal = string.Empty;
        //    if (dpByStudy.SelectedItem.Text != "")
        //    {
        //        selVal = dpByStudy.SelectedValue.ToString();
        //        string SQL = "Select FieldValue from FieldValues_ByStudy where FieldName='" + FldName + "' and StudyName = '" + dpByStudy.SelectedItem.Text + "'";
        //        PopulateValues(SQL);
        //        BindDropDowns();
        //        dpAllValues.SelectedItem.Text = "";
        //        dpByDiag.SelectedItem.Text = "";
        //        dpByStudy.SelectedValue = selVal;
        //        lstValues.Focus();
        //    }

        //}
        private void PopulateValues(string strSQL)
        {
            DataSet ds = GlobalValues.ExecuteDataSet(strSQL);
            lstValues.Items.Clear();
            int selIndex = -1;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                string ColVal = dr[0].ToString();
                if (ColVal == "N/A") ColVal = "";
                string[] Cols = ColVal.Split(',');
                for (int I = 0; I < Cols.Length; I++)
                {

                    lstValues.Items.Add(Cols[I].ToString());
                    if (Cols[I].ToString() == fldVal) { selIndex = I; }
                }
                if (selIndex >= 0) { lstValues.SelectedIndex = selIndex; }
            }
        }

        protected void btnAll_Click(object sender, EventArgs e)
        {
            string sqlStr = "Select distinct GroupName from Drugs with (nolock)";
            DataSet ds = GlobalValues.ExecuteDataSet(sqlStr);
            dpAllValues.Visible = false;
            lstValues.DataTextField = "GroupName";
            lstValues.DataValueField = "GroupName";
            lstValues.DataSource = ds;
            lstValues.DataBind();
            //UpdatePanel2.Update();
            // ModalPopupExtender1.Show();
            divMain.Visible = false;
            divPopup.Visible = true;
        }

        protected void btnDiag_Click(object sender, EventArgs e)
        {
            string sqlStr = "Select SiteOfPrimary from Patientdetails_1 with (nolock) where Patient ='" + patientId + "'";
            BindDropDown(sqlStr, "Diag");

        }

        protected void btnStudy_Click(object sender, EventArgs e)
        {
            // sqlStr = "Select StudyName  from [Studies] S inner join tblStudyUsers  SU on  s.StudyCode=SU.StudyCode where  instances like '%-" + GlobalValues.gInstanceID.ToString() + "-%'   and  '" + Session["Login"].ToString() + "'  in ( select a.HosCode from fn_Splithospitalcode(SU.Users) as a)    order by studyName";
            string sqlStr = "Select Study from DrugGroupsByStudy with (nolock) where DrugGroups <> '' And Study in (Select StudyName  from [Studies] S inner join tblStudyUsers  SU on  s.StudyCode=SU.StudyCode where  instances like '%-" + GlobalValues.gInstanceID.ToString() + "-%'   and  '" + Session["Login"].ToString() + "'  in ( select a.HosCode from fn_Splithospitalcode(SU.Users) as a))";
            BindDropDown(sqlStr, "Study");
        }
        private void BindDropDown(string SQL, string Type)
        {
            DataSet ds = GlobalValues.ExecuteDataSet(SQL);
            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.InsertAt(dr, 0);
            lstValues.Items.Clear();
            dpAllValues.Visible = true;
            dpAllValues.DataSource = ds;
            if (Type == "Diag")
            {
                dpAllValues.DataTextField = "SiteOfPrimary";
                dpAllValues.DataValueField = "SiteOfPrimary";
            }
            else
            {
                dpAllValues.DataTextField = "Study";
                dpAllValues.DataValueField = "Study";
            }
            dpAllValues.DataBind();
            // UpdatePanel2.Update();
            //ModalPopupExtender1.Show();
            divMain.Visible = false;
            divPopup.Visible = true;
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ViewState["lblvalue"] = lstValues.SelectedValue.ToString();
            fldVal = lstValues.SelectedValue.ToString();
            if (fldVal == "")
            {
                ModalPopupExtender1.Hide();
                ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Please select a Drug to proceed');", true);
                ModalPopupExtender1.Hide();

            }
            else
            {
                lblValue.Text = fldVal;
                string SqlStr = "Select * from PatientDrugsByLine with (nolock) where Patientid ='" + patientId + "'" + " and DrugLine = '" + strLine + "'";
                ds = GlobalValues.ExecuteDataSet(SqlStr);
                if (ds.Tables[0].Rows.Count <= 0)
                {

                    SqlStr = "Insert Into PatientDrugsByLine (PatientID,[DrugLine],[GroupName],[DrugName] ) Select '" + patientId + "', '" + strLine + "', [GroupName],[DrugName] from Drugs where GroupName ='" + fldVal + "'";
                    GlobalValues.ExecuteNonQuery(SqlStr);
                    UpdatePDTables();
                    BindGrid();
                }
                else
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["GroupName"] != fldVal)
                    {
                        lblConfirmText.Text = "Would you like to Delete the current Drug Group for " + strLine + " and replace it with " + fldVal + " ?";
                        ModalPopupExtender1.Show();
                    }
                    else
                    {
                        BindGrid();
                    }

                }
                //Update the PatientDetails Table with new value


            }

            //UpdatePanel1.Update();
            //ModalPopupExtender1.Hide();
            //divMain.Visible = true;
            //divPopup.Visible = false;
        }

        private void UpdatePDTables()
        {
            string SqlStr;
            string StrTableName;
            SqlStr = "Select [Table Name] from PDFields with (nolock) where [Field Name] ='" + strLine + " " + "Drug Group" + "'";
            DataSet dsP = GlobalValues.ExecuteDataSet(SqlStr);
            StrTableName = dsP.Tables[0].Rows[0][0].ToString();
            if (fldVal == "N/A") { fldVal = ""; }
            SqlStr = "Update " + StrTableName + " set [" + strLine + " " + "Drug Group] = '" + fldVal + "'  where Patient = '" + patientId + "'";
            GlobalValues.ExecuteNonQuery(SqlStr);
        }
        private void BindGrid()
        {

            string strSql = " Select PatientID, GroupName, DrugName, [Max Dose Per m2],[Min Dose Per m2],[Total Dose from All Cycles],[Number of Weeks],[Dose/M2/Week], [Units] From PatientDrugsByLine with (nolock) Where DrugLine = '" +
                strLine + "' and patientid ='" + patientId + "'";
            DataSet ds = GlobalValues.ExecuteDataSet(strSql);
            grdValues.DataSource = ds;
            grdValues.DataBind();
            ViewState["CurrentTable"] = ds.Tables[0].Copy();
            divMain.Visible = true;
            divPopup.Visible = false;

            sendValueToJS(fldVal);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            divMain.Visible = true;
            divPopup.Visible = false;
            BindGrid();
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            string strSql;
            fldVal = lstValues.SelectedValue.ToString();
            lblValue.Text = fldVal;
            strSql = "Delete from PatientDrugsByLine  where Patientid ='" + patientId + "'" + " and DrugLine = '" + strLine + "'";
            GlobalValues.ExecuteNonQuery(strSql);
            strSql = "Insert Into PatientDrugsByLine (PatientID,[DrugLine],[GroupName],[DrugName] ) Select '" + patientId + "', '" + strLine + "', [GroupName],[DrugName] from Drugs where GroupName ='" + fldVal + "'";
            GlobalValues.ExecuteNonQuery(strSql);
            ModalPopupExtender1.Hide();
            UpdatePDTables();
            BindGrid();
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
            UpdatePDTables();
            BindGrid();
        }

        protected void grdValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblValue.Text = grdValues.SelectedRow.Cells[2].Text.ToString();
        }

        protected void grdValues_SelectedIndexChanged(object sender, GridViewSelectEventArgs e)
        {
            lblValue.Text = grdValues.SelectedRow.Cells[2].Text.ToString();
        }

        protected void grdValues_RowEditing1(object sender, GridViewEditEventArgs e)
        {
            BindGrid();
        }


        //protected void grdValues_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        //{
        //    BindGrid();
        //}

        protected void grdValues_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdValues.EditIndex = e.NewEditIndex;
            BindGrid();

        }

        protected void grdValues_RowUpdated(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txt = new TextBox();
            string sqlStr = string.Empty;
            txt = (TextBox)grdValues.Rows[e.RowIndex].FindControl("txtMaxDose");
            sqlStr += "[Max Dose Per m2]='" + txt.Text.ToString() + "'";
            txt = (TextBox)grdValues.Rows[e.RowIndex].FindControl("txtMinDose");
            sqlStr += ", [Min Dose Per m2]='" + txt.Text.ToString() + "'";
            txt = (TextBox)grdValues.Rows[e.RowIndex].FindControl("txtTotDose");
            sqlStr += ", [Total Dose from All Cycles] ='" + txt.Text.ToString() + "'";
            txt = (TextBox)grdValues.Rows[e.RowIndex].FindControl("txtNoWk");
            sqlStr += ", [Number of Weeks] ='" + txt.Text.ToString() + "'";
            txt = (TextBox)grdValues.Rows[e.RowIndex].FindControl("DoseWk");
            sqlStr += ", [Dose/M2/Week] ='" + txt.Text.ToString() + "'";
            txt = (TextBox)grdValues.Rows[e.RowIndex].FindControl("txtUnits");
            sqlStr += ", [Units] ='" + txt.Text.ToString() + "'";
            Label lbl = new Label();
            lbl = (Label)grdValues.Rows[e.RowIndex].FindControl("lblDrug");
            string Drug = lbl.Text.ToString();//grdValues.Rows[e.RowIndex].Cells[1].Text.ToString();
            lbl = (Label)grdValues.Rows[e.RowIndex].FindControl("lblGroupName");
            string Group = lbl.Text.ToString();//grdValues.Rows[e.RowIndex].Cells[0].Text.ToString();
            sqlStr = "Update PatientDrugsByLine Set " + sqlStr + " where PatientID='" + patientId + "' and GroupName='" + Group + "' and DrugName ='" + Drug + "'";
            GlobalValues.ExecuteNonQuery(sqlStr);
            // BindGrid();
            grdValues.EditIndex = -1;
            BindGrid();
            updGrid.Update();
        }

        protected void grdValues_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdValues.EditIndex = -1;
            BindGrid();
            updGrid.Update();
        }

        protected void grdValues_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdValues_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            btnOk.Visible = true;
            btnCancel.Visible = true;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt.Rows.Count >= 1)
                {

                    string PatientId = string.Empty;
                    string sqlText = string.Empty;
                    PatientId = Convert.ToString(Session["PatientID"]);
                    if (PatientId == "") { Response.Redirect("SearchPatient.aspx"); }
                    Label lbl = new Label();
                    lbl = (Label)grdValues.Rows[e.RowIndex].FindControl("lblDrug");
                    string Drug = lbl.Text.ToString();
                    lbl = (Label)grdValues.Rows[e.RowIndex].FindControl("lblGroupName");
                    string Group = lbl.Text.ToString();
                    sqlText = "delete from PatientDrugsByLine  where PatientID='" + patientId + "' and GroupName='" + Group + "' and DrugName ='" + Drug + "'";
                    GlobalValues.ExecuteNonQuery(sqlText);
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["CurrentTable"] = dt;
                    grdValues.DataSource = dt;
                    grdValues.DataBind();
                    lblConfirmText.Text = "Durgs deleted successfully";


                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gr in grdValues.Rows)
            {
                TextBox txt = new TextBox();
                string sqlStr = string.Empty;
                txt = (TextBox)gr.FindControl("txtMaxDose");

                if (txt.Text.ToString().Trim() == string.Empty)
                    sqlStr += "[Max Dose Per m2]=null";
                else
                    sqlStr += "[Max Dose Per m2]='" + txt.Text.ToString() + "'";

                txt = (TextBox)gr.FindControl("txtMinDose");
                if (txt.Text.ToString().Trim() == string.Empty)
                    sqlStr += ", [Min Dose Per m2]=null";
                else
                    sqlStr += ", [Min Dose Per m2]='" + txt.Text.ToString() + "'";


                txt = (TextBox)gr.FindControl("txtTotDose");
                if (txt.Text.ToString().Trim() == string.Empty)
                    sqlStr += ", [Total Dose from All Cycles] =null";
                else
                    sqlStr += ", [Total Dose from All Cycles] ='" + txt.Text.ToString() + "'";

                txt = (TextBox)gr.FindControl("txtNoWk");
                if (txt.Text.ToString().Trim() == string.Empty)
                    sqlStr += ", [Number of Weeks] =null";
                else
                    sqlStr += ", [Number of Weeks] ='" + txt.Text.ToString() + "'";

                txt = (TextBox)gr.FindControl("DoseWk");
                if (txt.Text.ToString().Trim() == string.Empty)
                    sqlStr += ", [Dose/M2/Week] =null";
                else
                    sqlStr += ", [Dose/M2/Week] ='" + txt.Text.ToString() + "'";


                txt = (TextBox)gr.FindControl("txtUnits");
                if (txt.Text.ToString().Trim() == string.Empty)
                    sqlStr += ", [Units] =null";
                else
                    sqlStr += ", [Units] ='" + txt.Text.ToString() + "'";

                Label lbl = new Label();
                lbl = (Label)gr.FindControl("lblDrug");
                string Drug = lbl.Text.ToString();//grdValues.Rows[e.RowIndex].Cells[1].Text.ToString();
                lbl = (Label)gr.FindControl("lblGroupName");
                string Group = lbl.Text.ToString();//grdValues.Rows[e.RowIndex].Cells[0].Text.ToString();
                sqlStr = "Update PatientDrugsByLine Set " + sqlStr + " where PatientID='" + patientId + "' and GroupName='" + Group + "' and DrugName ='" + Drug + "'";
                GlobalValues.ExecuteNonQuery(sqlStr);
                // BindGrid();
                grdValues.EditIndex = -1;
                ViewState["lblvalue"] = Group;
            }
            if (ViewState["lblvalue"] != null)
            {
                fldVal = ViewState["lblvalue"].ToString();
                lblValue.Text = fldVal;
            }

            //string cScriptname = "ButtonClickScript";
            //Type cSType = this.GetType();
            //ClientScriptManager cs = Page.ClientScript;

            //// Check to see if the client script is already registered.
            //if (!cs.IsClientScriptBlockRegistered(cSType, cScriptname))
            //{
            //    StringBuilder cstext2 = new StringBuilder();
            //    cstext2.Append("<script type=\"text/javascript\"> function sendValuetoParent_dese(retVal) {");
            //    cstext2.Append("window.$(\"#onCoRightPickDiag\").dialog('close');} </");
            //    cstext2.Append("script>");
            //    cs.RegisterClientScriptBlock(cSType, cScriptname, cstext2.ToString(), false);
            //}

            BindGrid();
            updGrid.Update();

//            string close = @"<script type='text/javascript'> 
//                              window.returnValue ='" + fldVal;
//            string Close2 = @"';          
//                                window.close();
//                                </script>";
//            base.Response.Write(close + Close2);
            ctrlName.Value = fldVal;
            string close = "<script type='text/javascript'>window.parent.$('#onCoRightPickDiag').dialog('close'); return '" + fldVal + "'";
            string Close2 = "</script>";
            base.Response.Write(close + Close2);
            //Session["fldVal"] = fldVal;
            //sendValueToJS(Session["fldVal"].ToString());            
        }

        public void sendValueToJS(string strVal)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "Error_dese", "sendValuetoParent_dese('" + strVal + "');", true);
        }
    }
}