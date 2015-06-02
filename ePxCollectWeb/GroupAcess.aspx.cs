using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
namespace ePxCollectWeb
{
    public partial class GroupAcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         //   Page.Header.Controls.Remove(new LiteralControl("<base target='_self' />"));
            if(!IsPostBack)
            { 
            bindPermission();
        }
        }
      
        public void bindPermission()
        {
            string userID = string.Empty;
            string hospitalCode = string.Empty;
            try
            {


                if (Session["Login"] != null)
                {
                    userID = Session["Login"].ToString();
                }
                string connectionString = GlobalValues.strConnString;
                string sql1 = "select HospitalCode from HospitalUsers where UserId = '" + userID + "'";
                var ds1 = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sql1);
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    hospitalCode = ds1.Tables[0].Rows[0]["HospitalCode"].ToString();
                }
                //string sql =" select [FeatureSetUsers].[FeatureSetName],[HospitalUsers].[UserID],[FeatureSetUsers].[Enabled] from [FeatureSetUsers] "
                //            +" inner join [HospitalUsers] on [HospitalUsers].[UserID] = [FeatureSetUsers].UserID"
                //            +" where [HospitalUsers].[UserID] = '"+100001+"' and [FeatureSetUsers].[Enabled]='"+1+"'";

                string sql = " select h.HospitalName,aag.AccessGroupName,aag.Scope,aag.Criteria,aag.[Description],agm.UserID " +
                             " from AnalysisAccessGroupMembers agm " +
                             " inner join AnalysisAccessGroups aag on agm.CriteriaID=aag.CriteriaID " +
                             " Cross apply dbo.fn_Splithospitalcode(aag.HospitalCSV) as fc " +
                             " Inner join [Hospitals] h " +
                             " on (fc.HosCode = h.HospitalName) " +
                             " where agm.UserID = '" + userID + "' and h.HospitalCode = '" + hospitalCode + "'";
                var ds = SqlHelper.ExecuteDataset(connectionString, System.Data.CommandType.Text, sql);



                grdAccess.DataSource = ds;
                grdAccess.DataBind();
              
            }
            catch
            {

            }
        }
        protected void grdAccess_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {


            grdAccess.PageIndex = e.NewPageIndex;
            bindPermission();
          
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
           
         //   Response.Redirect("SearchPatient.aspx");
        }
    }
}