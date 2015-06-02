﻿using ePxCollectDataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ePxCollectWeb
{
    public partial class ErrorReport : System.Web.UI.Page
    {
        public static OncoEncrypt.OncoEncrypt objEncryptDecrypt = new OncoEncrypt.OncoEncrypt();
        string strConn = string.Empty;
        public static string reportID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }
            if (!Page.IsPostBack)
            {
                BindErrorReport();
                txtUserName.Text = Convert.ToString(Session["UserName"]);
            }
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnDownload);
            scriptManager.RegisterPostBackControl(this.grdErrorReport);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "Save")
            {
                HttpPostedFile file = (HttpPostedFile)(flImage.PostedFile);
                int iFileSize = file.ContentLength;
                if (iFileSize > 3145728)  // 1MB approx (1000000 actually less though) 1048576 = 1MB
                {
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Text = "File size should be less than 3MB.";
                    return;
                }
            }
            //Response.ContentType = "Application/xlsx";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=filename.xlsx");
            //Response.TransmitFile(Server.MapPath("~/Templates/filename.xlsx"));
            //Response.End(); 
            byte[] imgBin = null;
            SqlConnection conn = null;
            string connString = WebConfigurationManager.ConnectionStrings["OncoCollectEnterprise"].ConnectionString;
            connString = objEncryptDecrypt.SecureDecrypt(connString);
            DateTime reportDateTime = DateTime.Now;
            string fileName = string.Empty;
            try
            {
                try
                {
                    DateTime currentDateTime = GetDate();
                    if (flImage.Visible == true)
                    {
                        string[] allowExtentions = (ConfigurationManager.AppSettings["FileType"]).Split(',');
                        if (allowExtentions.Contains(Path.GetExtension(flImage.PostedFile.FileName)))
                        {
                            if (flImage.PostedFile != null && !string.IsNullOrEmpty(flImage.PostedFile.FileName) && Path.GetExtension(flImage.PostedFile.FileName) != "")
                            {

                                fileName = Path.GetFileName(flImage.PostedFile.FileName);
                                //create byte array with size corresponding to the currently selected file
                                imgBin = new byte[flImage.PostedFile.ContentLength];
                                //store the currently selected file in memory
                                HttpPostedFile img = flImage.PostedFile;
                                //store the image binary data of the selected file in the imgBin byte  array
                                img.InputStream.Read(imgBin, 0, (int)flImage.PostedFile.ContentLength);
                            }
                        }
                        else
                        {
                            lblError.ForeColor = System.Drawing.Color.Red;
                            lblError.Text = "Select only image files(.gif,.jpg,.jpeg,.bmp,.png,.ods,.odt,.xls,.xlsx,.doc,.txt).";
                            return;
                        }
                    }
                    //connect to the db
                    conn = new SqlConnection(connString);
                    if (btnSave.Text == "Save")
                    {
                        //sql command to send all of our img data to the db
                        SqlCommand cmd = new SqlCommand("INSERT INTO ErrorReport(AttachmentImage, ImageType, ImageSize,FileName,URL,UserName,Description,ReportedDate,CreatedDate,CreatedBy,Status) VALUES (@ImgBin, @ImgType, @ImgSize,@FileName,@URL,@UserName,@Description,@ReportedDate,@CreatedDate,@CreatedBy,@Status)", conn);
                        cmd.CommandType = CommandType.Text;

                        //add the image binary data to the sql command
                        cmd.Parameters.Add("@ImgBin", SqlDbType.Image, imgBin.Length).Value = imgBin;
                        //add the image type to the sql command
                        cmd.Parameters.AddWithValue("@ImgType", flImage.PostedFile.ContentType);
                        //add the image size to the sql command
                        cmd.Parameters.AddWithValue("@ImgSize", flImage.PostedFile.ContentLength);
                        cmd.Parameters.AddWithValue("@FileName", fileName);
                        cmd.Parameters.AddWithValue("@URL", txtURL.Text.Trim());
                        cmd.Parameters.AddWithValue("@UserName", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.ToString());
                        cmd.Parameters.AddWithValue("@ReportedDate", txtReportDate.Text.Trim());
                        cmd.Parameters.AddWithValue("@CreatedDate", currentDateTime);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["Login"].ToString());
                        cmd.Parameters.AddWithValue("@Status", txtStatus.Text);

                        using (conn)
                        {
                            //open the connection
                            conn.Open();
                            //send the sql query to store the data
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Error Report saved successfully.";
                    }
                    else
                    {
                        SqlCommand cmd = null;
                        if (flImage.Visible == true)
                        {
                            cmd = new SqlCommand("update ErrorReport set AttachmentImage=@ImgBin, ImageType=@ImgType, ImageSize=@ImgSize,FileName=@FileName,URL=@URL,UserName=@UserName,Description=@Description,ReportedDate=@ReportedDate,CreatedDate=@CreatedDate,CreatedBy=@CreatedBy,Status=@Status where ReportId = @ReportID", conn);
                            cmd.CommandType = CommandType.Text;

                            //add the image binary data to the sql command
                            cmd.Parameters.Add("@ImgBin", SqlDbType.Image, imgBin.Length).Value = imgBin;
                            //add the image type to the sql command
                            cmd.Parameters.AddWithValue("@ImgType", flImage.PostedFile.ContentType);
                            //add the image size to the sql command
                            cmd.Parameters.AddWithValue("@ImgSize", flImage.PostedFile.ContentLength);
                            cmd.Parameters.AddWithValue("@FileName", fileName);
                        }
                        else
                        {
                            cmd = new SqlCommand("update ErrorReport set URL=@URL,UserName=@UserName,Description=@Description,ReportedDate=@ReportedDate,CreatedDate=@CreatedDate,CreatedBy=@CreatedBy,Status=@Status where ReportId = @ReportID", conn);
                            cmd.CommandType = CommandType.Text;
                        }
                        if (!string.IsNullOrEmpty(txtReportDate.Text.Trim()))
                            reportDateTime = GetDate(txtReportDate.Text.Trim());
                        cmd.Parameters.AddWithValue("@URL", txtURL.Text.Trim());
                        cmd.Parameters.AddWithValue("@UserName", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.ToString());
                        cmd.Parameters.AddWithValue("@ReportedDate", reportDateTime);
                        cmd.Parameters.AddWithValue("@CreatedDate", currentDateTime);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["Login"].ToString());
                        cmd.Parameters.AddWithValue("@Status", txtStatus.Text);
                        cmd.Parameters.AddWithValue("@ReportID", reportID);

                        using (conn)
                        {
                            //open the connection
                            conn.Open();
                            //send the sql query to store the data
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Error Report updated successfully.";
                    }

                    Clear();
                    BindErrorReport();


                }
                catch (Exception ex)
                {
                    // lblError.Text = "Error: " + ex.Message;
                }
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            Clear();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchPatient.aspx", false);
        }
        void Clear()
        {
            txtURL.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtReportDate.Enabled = true;
            txtReportDate.Text = string.Empty;
            //txtStatus.Text = string.Empty;
            txtFixedDescription.Text = string.Empty;
            btnSave.Enabled = true;
            btnSave.CssClass = "button";
            btnSave.Text = "Save";
            btnDownload.Visible = false;
            lbtnClear.Visible = false;
            flImage.Visible = true;
            txtURL.ReadOnly = false;
            txtDescription.ReadOnly = false;
            txtFixedDescription.ReadOnly = false;
            txtStatus.ReadOnly = false;
            //string script = "<script type=\"text/javascript\"> ClearFileUploadControl(); </script>";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "myscript", script);
        }

        public static DateTime GetDate()
        {
            DateTime currentDateTime = DateTime.Now;
            string dateAndTime = currentDateTime.ToString("MMM d yyyy HH:mm:ss");
            currentDateTime = Convert.ToDateTime(dateAndTime);
            return currentDateTime;
        }
        public static DateTime GetDate(string reportDate)
        {
            DateTime reportDateTime = Convert.ToDateTime(reportDate);
            string dateAndTime = reportDateTime.ToString("MMM d yyyy HH:mm:ss");
            reportDateTime = Convert.ToDateTime(dateAndTime);
            return reportDateTime;
        }
        private void BindErrorReport()
        {
            strConn = GlobalValues.strConnString;
            string strQueryText = "SELECT * FROM ErrorReport";
            DataSet dsErrorReport = SqlHelper.ExecuteDataset(strConn, CommandType.Text, strQueryText);
            grdErrorReport.DataSource = dsErrorReport;
            grdErrorReport.DataBind();

        }

        protected void imgDownload_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton download = (ImageButton)sender;
                reportID = download.CommandArgument.ToString();
                DownloadAttachment();

            }
            catch (Exception ex)
            {
            }
            finally
            {

            }
        }
        protected void imgView_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                btnDownload.Visible = true;
                lbtnClear.Visible = false;
                lbtnClear.Text = "Clear";
                lblred.Visible = false;
                flImage.Visible = false;
                ImageButton view = (ImageButton)sender;
                reportID = view.CommandArgument.ToString();
                GridViewRow row = (GridViewRow)view.NamingContainer;
                txtURL.Text = ((Label)row.FindControl("LabelURL")).Text.Trim();
                txtDescription.Text = ((Label)row.FindControl("LabelDescription")).Text.Trim();
                txtFixedDescription.Text = ((Label)row.FindControl("LabelFixedDescription")).Text.Trim();
                if (!string.IsNullOrEmpty(((Label)row.FindControl("LabelStatus")).Text.Trim()))
                    txtStatus.Text = ((Label)row.FindControl("LabelStatus")).Text.Trim();
                if (!string.IsNullOrEmpty(txtStatus.Text) && txtStatus.Text == "Fixed")
                    lnkReopen.Visible = true;
                txtReportDate.Text = ((Label)row.FindControl("LabelReportDate")).Text.Trim();
                btnSave.Text = "Save";
                btnSave.CssClass = "buttonDisable";
                btnSave.Enabled = false;
                txtURL.ReadOnly = true;
                txtDescription.ReadOnly = true;
                txtFixedDescription.ReadOnly = true;
                txtStatus.ReadOnly = true;
                txtReportDate.Enabled = false;
            }

            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
        protected void imgEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                btnDownload.Visible = true;
                lbtnClear.Visible = true;
                lbtnClear.Text = "Clear";
                lblred.Visible = false;
                flImage.Visible = false;
                btnSave.Enabled = true;
                btnSave.CssClass = "button";
                ImageButton edit = (ImageButton)sender;
                reportID = edit.CommandArgument.ToString();
                GridViewRow row = (GridViewRow)edit.NamingContainer;
                txtURL.Text = ((Label)row.FindControl("LabelURL")).Text.Trim();
                txtDescription.Text = ((Label)row.FindControl("LabelDescription")).Text.Trim();
                txtFixedDescription.Text = ((Label)row.FindControl("LabelFixedDescription")).Text.Trim();
                if (!string.IsNullOrEmpty(((Label)row.FindControl("LabelStatus")).Text.Trim()))
                    txtStatus.Text = ((Label)row.FindControl("LabelStatus")).Text.Trim();
                if (!string.IsNullOrEmpty(txtStatus.Text) && txtStatus.Text == "Fixed")
                    lnkReopen.Visible = true;
                txtReportDate.Text = ((Label)row.FindControl("LabelReportDate")).Text.Trim();
                btnSave.Text = "Update";

            }

            catch (Exception ex)
            {
            }
            finally
            {

            }
        }
        protected void grdErrorReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                grdErrorReport.PageIndex = e.NewPageIndex;
                BindErrorReport();
                grdErrorReport.SelectedIndex = -1;
                grdErrorReport.Focus();
            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkReopen_Click(object sender, EventArgs e)
        {
            txtStatus.Text = "Reopen";
            btnSave.Text = "Save";
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DownloadAttachment();
        }



        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            if (lbtnClear.Text == "Cancel")
            {
                btnDownload.Visible = true;
                flImage.Visible = false;
                lbtnClear.Text = "Clear";
                lblred.Visible = false;
            }
            else
            {
                btnDownload.Visible = false;
                flImage.Visible = true;
                lbtnClear.Text = "Cancel";
                lblred.Visible = true;
            }
        }

        private void DownloadAttachment()
        {
            byte[] bytes = null;
            string fileName=string.Empty, contentType=string.Empty;
            string constr = WebConfigurationManager.ConnectionStrings["OncoCollectEnterprise"].ConnectionString;
            constr = objEncryptDecrypt.SecureDecrypt(constr);
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select FileName, AttachmentImage, ImageType,ImageSize from ErrorReport where ReportId=@Id";
                    cmd.Parameters.AddWithValue("@Id", reportID);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            sdr.Read();
                            bytes = (byte[])sdr["AttachmentImage"];
                            contentType = sdr["ImageType"].ToString();
                            fileName = sdr["FileName"].ToString();
                        }
                    }
                    con.Close();
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            //Response.AppendHeader("Content-Disposition", "inline;filename==" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
    }
}