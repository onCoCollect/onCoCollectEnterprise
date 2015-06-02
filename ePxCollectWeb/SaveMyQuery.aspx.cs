using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ePxCollectWeb
{
    public partial class SaveMyQuery : System.Web.UI.Page
    {
        FeatureSetPermission ObjfeatureSet = new FeatureSetPermission();
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
           
            if (!IsPostBack)
            {
                txtFilterText.Text = Convert.ToString(Session["Whr" + Session.SessionID.ToString()]);
                if (txtFilterText.Text == "1=1") { txtFilterText.Text = ""; }
                if (ObjfeatureSet.isAnalysisByAllparams == false)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "CloseDialog();", true);
                }
            }
        }
        private void SaveFilterToDB(string user,string queryName)
        {
            if (Session["dsFilter"] == null) return ;
            var   dsFilters = (DataSet)Session["dsFilter"];
            var qry = string.Format("select Queryid from [CustomQueries] where  UserId = '{0}' and Queryname = '{1}'", user, queryName);
            var ins = "INSERT INTO [UserSavedQueryFilters] ([QueryID],[DispValue],[Operator],[FilterValue],[Type])VALUES(" +
           "{0},'{1}','{2}','{3}','{4}')";

            var id = GlobalValues.ExecuteScalar(qry);
            foreach(DataRow row in dsFilters.Tables[0].Rows)
            {
                qry = string.Format(ins, id, row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString());
                GlobalValues.ExecuteNonQuery(qry);
            }

        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            if (txtQueryName.Text.Length > 0)
            {
                string queryText = string.Empty;
                string userName = Convert.ToString(Session["Login"]);
                string strWhere = Convert.ToString(Session["Whr" + Session.SessionID.ToString()]);
                if (strWhere.Length > 0) { strWhere = " where " + strWhere; }
                queryText = GlobalValues.QueryString;// +GlobalValues.glbFromClause + " " + strWhere;
                queryText = queryText.Replace("'", "''");
                string queryName = txtQueryName.Text.Replace("'", "''");
                strWhere = strWhere.Replace("'", "$$").Replace("where", ""); ;
                string sqlstrCheck = "Select count(*) from CustomQueries where UserID='" + userName + "' and QueryName='" + queryName.Trim() +"'";
                int RecCount = (int)GlobalValues.ExecuteScalar(sqlstrCheck);
                if (RecCount == null) { RecCount = 0; }
                if (RecCount > 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('You already have a query saved with this name. Please use a different name.');", true);

                    updConfirm.Update();

                }
                else
                {                    
                    string dyText = Convert.ToString(Session["flterText"]).Trim();
                    var qryname =txtQueryName.Text.ToString().Replace("'", "");
                    string result = dyText .Replace("'","''");
                    string AnalysisType = string.Empty;
                    if (Session["AnalysisType"] != null)
                    {
                        AnalysisType = Session["AnalysisType"].ToString();
                    }
                    string sqlstr = "Insert into CustomQueries ( UserID, QueryName, QueryText, QueryDescription, ParentForm, DescriptionByUser, Filterable,DynamicText )"
                        + " Values ('" + userName + "','" + qryname +
                        "', '" + queryText.Replace("'","''") + "','" + strWhere + "','"+ AnalysisType + "','" + txtFilterText.Text.ToString().Replace("'", "$$") + "',0, '" +result+ "' )";
                    try
                    {
                        GlobalValues.ExecuteNonQuery(sqlstr);
                        SaveFilterToDB(userName, qryname);
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "CloseDialog();", true);
                        ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "window.parent.$('#SaveQuerydiag').dialog('close');", true);
                    }
                    catch (Exception)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Error while saving your query.');", true);
                    }
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please provide a name for the Query.');", true);

            }
        }
    }
}