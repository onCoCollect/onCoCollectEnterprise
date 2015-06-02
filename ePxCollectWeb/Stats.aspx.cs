using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ePxCollectDataAccess;
using System.Threading;
using hmlib.Web.UI.Controls;
using System.Web.Services;

namespace ePxCollectWeb
{
    public partial class Stats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["Login"] == null)
            //{
            //    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
            //    Guid.NewGuid().ToString("N"), "self.parent.location='login.aspx';", true);
            //}
            if (Convert.ToString(Session["Login"]) == "")
            {
                string script = "<script type=\"text/javascript\"> CloseDialog();return false; </script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "myscript", script);
                //Response.Write("<script language='javascript'> { self.close() }</script>");

                Response.Redirect("login.Aspx", false);
            }
            if (!IsPostBack)
            {
                string strSQL = GlobalValues.QueryString;
                string inCheckString = string.Empty;
                string strInClause = string.Empty;
                string[] strTabCol = strSQL.Split(',');
                for (int i = 0; i <= strTabCol.Length - 1; i++)
                {
                    
                    inCheckString += "'" + strTabCol[i].Trim() + "'" + ",";
                   
                }
                strInClause = inCheckString.Substring(0, inCheckString.Length - 1);
                strSQL = "Select [Field Name] as FieldName  from PDFields where [field Name] in ( " + strInClause + " ) and [dataType] in ('Int','Long', 'Double') and [Patient Identity Field]=0 and [FieldOrder] not in ('" + 2147 + "','" + 2159 + "','" + 2 + "') order by  [FieldOrder] ";
                DataSet ds = new DataSet();
                ds = GlobalValues.ExecuteDataSet(strSQL);
                DataRow dr = ds.Tables[0].NewRow();
                dr[0] = "";
                ds.Tables[0].Rows.InsertAt(dr, 0);
                dlstColumns.DataSource = ds;
                dlstColumns.DataTextField = "FieldName";
                dlstColumns.DataValueField = "FieldName";
                dlstColumns.DataBind();
            }
        }
        [WebMethod]
        public static object getProgress(string progressId)
        {
            return Progress.GetResponse(progressId);
        }
        protected void dlstColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            //Progress progress = ProgressBar1.Progress;

            //Thread thread = new Thread(() => Start(progress));
            //thread.Start();
            //dlstColumns.Enabled = false;
            mathCaluculations();
            //rdAnalysisType.Enabled = false;
         

        }

        protected void rdAnalysisType_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            //Progress progress = ProgressBar1.Progress;
            //Thread thread = new Thread(() => Start(progress));
            //thread.Start();
            mathCaluculations();
            //dlstColumns.Enabled = false;
            //rdAnalysisType.Enabled = false;
           
        }


        public void mathCaluculations()
        {
            
           
            var criteria = string.Empty;
            if (Session["LableMessage"] != null)
            {
                criteria = Session["LableMessage"].ToString();



                var column = dlstColumns.Text.ToString();
                if (column != "")
                {

                    var sqlQuery = "Select count(" + 1 + ")" +
                              GlobalValues.glbFromClause +
                              "where" + criteria + "";
                    var totalRecords = (int)GlobalValues.ExecuteScalar(sqlQuery);
                    var sqlIgnoredQuery = "Select count("+ 1 + ")" +
                             GlobalValues.glbFromClause +
                             "where" + criteria + " and [" + column + "] is null  ";

                    var ignoredRecords = (int)GlobalValues.ExecuteScalar(sqlIgnoredQuery);
                    column = dlstColumns.Text.ToString();
                    var SqlStr = string.Empty;
                    
                    SqlStr = "Select [" + column + "]" +
                               GlobalValues.glbFromClause +
                               "where" + criteria + " and " + " [" + column + "] >0  order by [" + column + "]";
                    //Fetch records
                    var ds = GlobalValues.ExecuteDataSet(SqlStr);

                    var lst = new List<double>();

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            lst.Add(double.Parse(row[column].ToString()));
                        }

                        if (rdAnalysisType.SelectedIndex == 0)
                        {
                            Decimal mean = Decimal.Parse(lst.Mean().ToString());
                            Decimal meanValue = Math.Round(mean, 2);
                            
                            lblMeanMed.Text = " Mean = " + Math.Round(meanValue, 2);

                        }
                        else
                        {
                            Decimal median = Decimal.Parse(lst.Median().ToString());
                            Decimal medianValue = Math.Round(median, 2);
                           
                            lblMeanMed.Text = " Median = " + Math.Round(medianValue, 2);

                        }
                        var rngstart = lst[0].ToString();
                        var rngEnd = lst[lst.Count - 1].ToString();
                        lblNoRecs.Text = "No. of Records Considered = " + ds.Tables[0].Rows.Count.ToString();
                        lblTotal.Text = totalRecords.ToString();
                        LblRange.Text = "Range = " + rngstart.ToString() + " - " + rngEnd.ToString();
                        lblRecsIgnored.Text = "Records Ignored (no values) = " + ignoredRecords;
                        return;
                    }

                }
            }
            lblTotal.Text = string.Empty;
            if (rdAnalysisType.SelectedIndex == 0)
            {
                lblMeanMed.Text = " Mean  = " + string.Empty;
            }
            else
            {
                lblMeanMed.Text = " Median  = " + string.Empty;
            }
            lblNoRecs.Text = string.Empty;
            lblRecsIgnored.Text = "Records Ignored (no values) = " + string.Empty;
            lblNoRecs.Text = "No. of Records Considered = " + string.Empty;
            LblRange.Text = "Range = " + string.Empty;
        }

        protected void ProgressBar1_Complete(object sender, EventArgs e)
        {
            var progressBar = (ProgressBar)sender;
        //    Label1.Text = "Completed";
           
            dlstColumns.Enabled = true;
            rdAnalysisType.Enabled = true;
        }
        private void Start(Progress progress)
        {
            double max = 50;
            for (int i = 0; i < 10; i++)
            {
                progress.SetProgress(i / 9.8d);
                Thread.Sleep(200);
                progress.AddMessageLine(i);
                //progress.SetProgress(i / max);
            }
            progress.SetProgress(1);
        }
      
    }
}
