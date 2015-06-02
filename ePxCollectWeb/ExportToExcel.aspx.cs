using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace ePxCollectWeb
{
    public partial class ExportToExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                System.Threading.Thread.Sleep(6000);
                ExportData();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }

        private void ExportData()
        {
            DataTable dt = new DataTable();
            if (Session[Session.SessionID.ToString()] != null)
                dt = (DataTable)Session[Session.SessionID.ToString()];
            grdAnalysisRes.DataSource = dt;
            grdAnalysisRes.DataBind();
            string attachment = "attachment; filename=AnalysisResult.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grdAnalysisRes.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

       
    }
}