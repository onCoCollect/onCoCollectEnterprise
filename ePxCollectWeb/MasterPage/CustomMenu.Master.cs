using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data;
using System.Web.UI.HtmlControls;

namespace ePxCollectWeb.MasterPage
{
    public partial class CustomMenu : System.Web.UI.MasterPage
    {
        string strConn = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            strConn = GlobalValues.strConnString;

            //if (Convert.ToString(Session["Login"]) == "")
            //{
            //    //    NavigationMenu.Enabled = false;
            //    Response.Redirect("login.Aspx");
            //}
            //else
            //{
            //    NavigationMenu.Enabled = true;
            //}
            //PopulateMainMenu();

            //if (HttpContext.Current.Request.Url.AbsoluteUri.ToUpper().Contains("REGISTER.ASPX") == false)
            //{
            //    if (Convert.ToString(Session["PatientID"]) == "")
            //    {
            //        ClientScriptManager cs = Page.ClientScript;
            //        cs.RegisterClientScriptBlock(GetType(), "Pick Patient", "<script>alert('Please pick a patient to proceed');</script>", false);
            //        Response.Redirect("SearchPatient.aspx");
            //    }
            //}
            //lblPatientDetails.Text = Convert.ToString(Session["PatienDetails"]);
        }

        
        private void PopulateMainMenu()
        {
            try
            {
                DataSet dsMainMenu = new DataSet();
                if (Session["MenuType"] == "Study")
                {

                }
                else
                    if (Session["MenuType"] == "Diagnosis")
                    {

                    }
                    else
                        if (Session["MenuType"] == "Custom")
                        {

                        }
               
                //string strSql = "select M.MenuID, MenuDescription,TargetURL, M.ISACtive, [ReadOnly], Isnull(SubMenu,'') SubMenu , ParentID from tblMenuItems M inner join tblUserRights R on M.MenuID=R.MenuId and (m.IsActive=1 and R.IsActive=1) and UserGroupID=1";
                string strSql = "select M.MenuID, MenuDescription,TargetURL, M.ISACtive, [ReadOnly], Isnull(SubMenu,'') SubMenu , ParentID, DivName from tblMenuItems M inner join tblUserRights R on M.MenuID=R.MenuId and (m.IsActive=1 and R.IsActive=1) and  isnull(ParentID,'')='' and UserGroupID=1 order by M.MenuID";
                dsMainMenu = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSql);
                int iPos = 0;
                foreach (DataRow dr in dsMainMenu.Tables[0].Rows)
                {
                    LinkButton lBtn = new LinkButton();
                    string strURL = dr["TargetURL"].ToString();
                    strURL += (strURL.Contains("?") ? "&" + "RO=" + HttpUtility.UrlEncode(dr["ReadOnly"].ToString()) : "");
                    lBtn.OnClientClick = "javascript:window.location.href('" + strURL + "'); return false;";
                    //lBtn("OnClick","javascript:window.location.href('" + dr["TargetURL"].ToString() + "&ReadOnly=" +dr["ReadOnly"].ToString()+  "'); return false;'");
                    iPos = phMenus.Controls.Count;
                    if (dr["SubMenu"].ToString() == "False")
                    {
                        lBtn.CssClass = "MainLeftItem";
                        lBtn.ID = dr["MenuID"].ToString();
                        lBtn.Text = dr["MenuDescription"].ToString();
                        phMenus.Controls.Add(lBtn);
                    }
                    else
                    {
                        DataSet dsSubItems = new DataSet();
                        lBtn.CssClass = "MainLeftItem";
                        lBtn.OnClientClick = strURL.Replace("div", "ctl00_div") + "; return false;";
                        lBtn.ID = dr["MenuID"].ToString();
                        lBtn.Text = dr["MenuDescription"].ToString() + "   <img alt='>>' src='images/right.gif' />";
                        phMenus.Controls.Add(lBtn);
                        string strHtml = string.Empty;
                        HtmlGenericControl div1 = new HtmlGenericControl("DIV");
                        div1.ID = dr["DivName"].ToString(); //"div" + dr["MenuDescription"].ToString();
                        div1.Attributes.Add("style", "display:none");
                        if (div1.ID.Contains("Study"))
                        {
                        }
                        else
                            if (div1.ID.Contains("Custom"))
                            {

                            }
                            else
                                if (div1.ID.Contains("Custom"))
                                {

                                }
                                else
                                {
                                    strSql = "select M.MenuID, MenuDescription,TargetURL, M.ISACtive, [ReadOnly], Isnull(SubMenu,'') SubMenu , ParentID, DivName from tblMenuItems M inner join tblUserRights R on M.MenuID=R.MenuId and (m.IsActive=1 and R.IsActive=1)  and UserGroupID=1 and ParentID=" + dr["MenuID"].ToString() + " Order by M.MenuID";
                                }
                        dsSubItems = SqlHelper.ExecuteDataset(strConn, System.Data.CommandType.Text, strSql);
                        strHtml = "<ol> ";
                        foreach (DataRow drS in dsSubItems.Tables[0].Rows)
                        {
                            strURL = drS["TargetURL"].ToString();
                            strURL += (strURL.Contains("?") ? "&" + "RO=" + HttpUtility.UrlEncode(dr["ReadOnly"].ToString()) : "");
                            strHtml += "<li> <a href='" + strURL + "' class='IndentItem'> " + drS["MenuDescription"].ToString() + "</a></li>";

                        }
                        strHtml += "</ol>";
                        div1.InnerHtml = strHtml;
                        phMenus.Controls.Add(div1);
                    }


                    // phMenus.Controls.AddAt(iPos, lBtn);                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btn_click(object sender, ImageClickEventArgs e)
        {
            ImageButton btnMnu = (ImageButton)sender;
            Response.Redirect("ProjectForm.aspx?Tag=" + Convert.ToString(btnMnu.CommandArgument));

        }
    }
}