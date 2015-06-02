using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ePxCollectWeb
{
    public partial class SavedQueriesView : System.Web.UI.Page
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
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }
            if (!IsPostBack)
            {
                BindQueryNames();
            }
        }
        private void BindQueryNames()
        {
            string userName = Convert.ToString(Session["Login"]);
            string sqlStr = "Select queryID,UserID, QueryName, QueryText, QueryDescription, DescriptionByUser from CustomQueries where UserID ='" + userName + "'";
            DataSet dsQueries = GlobalValues.ExecuteDataSet(sqlStr);
            DataRow dr = dsQueries.Tables[0].NewRow();
            dsQueries.Tables[0].Rows.InsertAt(dr, 0);
            dpQueryNames.DataSource = dsQueries;
            dpQueryNames.DataTextField = "QueryName";
            dpQueryNames.DataValueField = "queryID";
            dpQueryNames.DataBind();
            Session["DSQuery" + Session.SessionID.ToString()] = dsQueries;
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {

            if (dpQueryNames.Text != "")
            {
                string strQueryText = string.Empty;
                string strWhere = string.Empty;
                string strdynamicText = string.Empty; // Added by srinivas
                var queryid = dpQueryNames.SelectedValue;
                string sqlStr = "Select QueryText,QueryDescription,ParentForm,DynamicText from CustomQueries where queryID=" + dpQueryNames.SelectedValue.ToString();
                DataSet ds = GlobalValues.ExecuteDataSet(sqlStr);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        strQueryText = dr["QueryText"].ToString();
                        strWhere = dr["QueryDescription"].ToString();
                        //Added by srinivas
                        strdynamicText = dr["DynamicText"].ToString();
                        Session["AnalysisType"] = dr["ParentForm"].ToString();
                    }
                }
                 
                strQueryText = strQueryText.Replace("$$", "'");
                
                strWhere = strWhere.Replace("$$", "'");

                GlobalValues.QueryString = strQueryText.ToString();
                Session["Whr" + Session.SessionID.ToString()] = strWhere;

                Session["QueryText"] = strdynamicText;
                Session["QueryId"] = dpQueryNames.SelectedValue;
                Response.Redirect("AnalysisResult.aspx");


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please select a Query Name.');", true);
            }
        }

        protected void dpQueryNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dpQueryNames.Text != "")
            {
                //string sqlStr = "Select queryID, QueryText from CustomQueries where queryID=" + dpQueryNames.SelectedValue.ToString();
                DataSet dsF = (DataSet)Session["DSQuery" + Session.SessionID.ToString()];
                DataRow[] drF = dsF.Tables[0].Select("QueryID=" + dpQueryNames.SelectedValue.ToString());
                if (drF.Length > 0)
                {
                    string strFilter = drF[0]["DescriptionByUser"].ToString().Replace("$$", "'");
                    //if (strFilter != "1=1") 
                    { txtFilterText.Text = strFilter; }
                    //updMain.Update();
                }

            }
        }

        //Commented by Venkat on 2/Jul/2014. Added new code below for this event.
        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    ModalPopupExtender1.Show();
        //    updConfirm.Update();
        //    updMain.Update();
        //}

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (dpQueryNames.Text != "")
            {
                string sqlStr = "Delete from CustomQueries where queryID=" + dpQueryNames.SelectedValue.ToString();
                GlobalValues.ExecuteNonQuery(sqlStr);
                //ModalPopupExtender1.Show();
                //updConfirm.Update();
                txtFilterText.Text = "";
                BindQueryNames();
            }
            //if (dpQueryNames.Text != "")
            //{
                
            //    try
            //    {
            //        updMain.Update();
            //    }

            //    catch (Exception)
            //    {

            //    }
            //}
            //else
            //{

            //}
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            //string MyReferrer = Request.UrlReferrer.ToString();
            //string previousPageName = Request.Url.AbsoluteUri.ToString();
            //Response.Redirect(MyReferrer);
            Session["Flag"] = "SavedQueries";
            Response.Redirect("ProjectForm.aspx");
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            if (dpQueryNames.Text != "")
            {
                string sqlStr = "Delete from CustomQueries where queryID=" + dpQueryNames.SelectedValue.ToString();
                GlobalValues.ExecuteNonQuery(sqlStr);
                //ModalPopupExtender1.Show();
                //updConfirm.Update();
                txtFilterText.Text = "";
                BindQueryNames();
            }
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(string), "PopupWindow", "alert('Please Select Query Name from the List.');", true);
            //}
        }


    }
}