using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace ePxCollectWeb
{
    public partial class PickPatient : System.Web.UI.Page
    {
        //string strConn;
        //DataSet dsSearch = new DataSet();
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    strConn = GlobalValues.strConnString;//System.Configuration.ConfigurationManager.ConnectionStrings["ePxConnString"].ConnectionString.Trim();

        //}

        //protected void cboSearchFor_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cboSearchFor.Text == "All Patients")
        //    {

        //    }
        //    else if (cboSearchFor.Text == "Name")
        //    {
        //        txtOther.Visible = false;
        //        txtFname.Visible = true;
        //        txtLName.Visible = true;
        //        lblOthers.Visible = false;
        //        lblFirstName.Visible = true;
        //        lblLastName.Visible = true;
        //        txtFname.Focus();
        //    }
        //    else
        //    {
        //        txtOther.Visible = true;
        //        txtFname.Visible = false;
        //        txtLName.Visible = false;
        //        lblOthers.Visible = true;
        //        lblOthers.Text = cboSearchFor.Text.ToString();
        //        lblFirstName.Visible = false;
        //        lblLastName.Visible = false;
        //        txtOther.Focus();
        //    }
        //}


        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    string strCancel;
        //    strCancel = "";
        //}

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    string strWhere = string.Empty;
        //    string strSql = string.Empty;

        //    try
        //    {
        //        switch (cboSearchFor.Text)
        //        {
        //            case "Name":
        //                {
        //                    if (txtFname.Text.Trim() != "" && txtLName.Text.Trim() != "")
        //                    {
        //                        strWhere = "PatientDetails_0.PatientName  like '%" + txtFname.Text.Trim() + "%' or PatientDetails_0.PatientName  like '%" + txtLName.Text.Trim() + "%'";

        //                    }
        //                    else if (txtFname.Text.Trim() != "")
        //                    {
        //                        strWhere = "PatientDetails_0.PatientName  like '%" + txtFname.Text.Trim() + "%'";
        //                    }
        //                    else if (txtLName.Text.Trim() != "")
        //                    {
        //                        strWhere = "PatientDetails_0.PatientName  like '%" + txtLName.Text.Trim() + "%'";
        //                    }
        //                    break;
        //                }
        //            case "File Number":
        //                {
        //                    strWhere = "PatientDetails_0.HospitalFileNo like '%" + txtOther.Text.Trim() + "%'";
        //                    break;
        //                }
        //            case "Patient ID":
        //                {
        //                    strWhere = "PatientDetails_0.PatientID Like  '%" + txtOther.Text.Trim() + "%'";
        //                    break;
        //                }
        //            default:
        //                {
        //                    break;
        //                }
        //        }

        //        if (pnlOptions.Visible == false || OptSearch.SelectedValue == "2")
        //        {
        //            strSql = "SELECT PatientDetails_0.PatientID,PatientDetails_0.PatientName,PatientDetails_0.HospitalFileNo,PatientDetails_1.Diagnosis,PatientDetails_0.City_Town,PatientDetails_0.State,PatientDetails_0.DateOfRegistration,PatientDetails_0.PhoneNumber From PatientDetails_0 LEFT JOIN PatientDetails_1 ON PatientDetails_0.PatientID = PatientDetails_1.Patient ";
        //            if (strWhere.Trim() != "") { strSql += " WHERE " + strWhere; }
        //            strSql += " ORDER BY PatientDetails_0.SortID";

        //            dsSearch = ePxCollectDataAccess.SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSql);

        //        }
        //        else
        //        {
        //            dsSearch = (DataSet)Session["ds" + Session.SessionID.ToString()];
        //            //strWhere= strWhere.Replace("like", "=").Replace("PatientDetails_0.", "");
        //            strWhere = strWhere.Replace("PatientDetails_0.", "");
        //            dsSearch.Tables[0].DefaultView.RowFilter = strWhere;
        //            //grdResult.DataSource = dsSearch.Tables[0].DefaultView;
        //            //grdResult.DataBind();
        //        }
                
        //        grdResult.DataSource = dsSearch.Tables[0].DefaultView;
        //        grdResult.DataBind();
        //        Session["ds" + Session.SessionID.ToString()] = dsSearch;//.Tables[0].DefaultView;
        //        if (dsSearch.Tables[0].Rows.Count > 0) { pnlOptions.Visible = true; }
        //        else { pnlOptions.Visible = false; }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //protected void grdResult_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string value = string.Empty;
            
        //    value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[1].Text;
        //    lblPatientID.Text = value;            
        //    value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[2].Text;
        //    this.lblPatientName.Text = value;
        //    value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[3].Text;
        //    this.lblFileNo.Text = value;
        //    value = grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[5].Text; 
        //    this.lblCity.Text = value;  
            
        //    ModalPopupExtender1.Show();
        //}

     

      

        //protected void grdResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdResult.PageIndex = e.NewPageIndex;
        //    dsSearch = (DataSet)Session["ds" + Session.SessionID.ToString()];
        //    grdResult.DataSource = dsSearch;
        //    grdResult.DataBind();
        //}

        //protected void btnOk_Click(object sender, EventArgs e)
        //{
        //    Session["PatientID"] = lblPatientID.Text;
        //    Session["PatienDetails"] = lblPatientName.Text + "/ ID:" + lblPatientID.Text + "/ File No:" + lblFileNo.Text + "/ City:" + lblCity.Text +
        //        "/ "  + grdResult.Rows[grdResult.SelectedRow.RowIndex].Cells[4].Text;
        //    Response.Redirect("ProjectForm.aspx");
        //}

        //protected void btnPickPatient_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("Register.aspx");
        //}
    }
}