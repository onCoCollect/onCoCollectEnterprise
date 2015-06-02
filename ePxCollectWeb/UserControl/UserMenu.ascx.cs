using ePxCollectDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ePxCollectWeb.UserControl
{
    public partial class UserMenu : System.Web.UI.UserControl
    {
        string strConns = GlobalValues.strConnString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserMenuBind();
            }
        }

        public void SetSelectedMenu()
        {
            if (Session["ActiveLink"] != null)
            {
                MenuItem mnuTemp = User_Menu.FindItem(Session["ActiveLink"].ToString());
                if (mnuTemp != null)
                {
                    mnuTemp.Selected = true;
                    mnuTemp.Selectable = true;
                }

            }
        }

        protected void UserMenuBind()
        {

            if (Session["Login"] == null)
                Response.Redirect("login.aspx");

            User_Menu.Items.Clear();
            // User Profile Menu
            MenuItem MIDataAnalysis = new MenuItem("Data Analysis", "DataAnalysis");
            MIDataAnalysis.ToolTip = "DataAnalysis";

            MenuItem MIUPStatusCheckList = new MenuItem("Status Check List", "StatusCheckList");
            MIUPStatusCheckList.ToolTip = "Status Check List";

            MenuItem MIUPPatientAnalysis = new MenuItem("Patient Analysis", "PatientAnalysis");
            MIUPPatientAnalysis.ToolTip = "Patient Analysis";

            MenuItem MIUPPatientAnalysisDiagnosis = new MenuItem("By Diagnosis", "ByDiagnosis");
            MIUPPatientAnalysisDiagnosis.ToolTip = "ByDiagnosis";
            MenuItem MIUPPatientAnalysisStudy = new MenuItem("By Study", "ByStudy");
            MIUPPatientAnalysisStudy.ToolTip = "ByStudy";
            MenuItem MIUPPatientAnalysisParameter = new MenuItem("By Parameter", "ByParameter");
            MIUPPatientAnalysisParameter.ToolTip = "ByParameter";



            User_Menu.Items.Clear();

            string pdSQL = "select * from FeatureSetUsers where UserID='" + Session["Login"].ToString() + "'";


            DataSet dset = SqlHelper.ExecuteDataset(strConns, CommandType.Text, pdSQL);
            if (dset.Tables.Count > 0 && dset.Tables[0].Rows.Count > 0)
            {
                /*Default With out Permission
                User_Menu.Items.Add(MIDataAnalysis);
               
                User_Menu.FindItem(MIDataAnalysis.Value).ChildItems.Add(MIUPStatusCheckList);
               

                User_Menu.FindItem(MIDataAnalysis.Value).ChildItems.Add(MIUPPatientAnalysisDiagnosis);
                User_Menu.FindItem(MIDataAnalysis.Value + "/" + MIUPPatientAnalysisDiagnosis.Value).ChildItems.Add(MIUPPatientAnalysisDiagnosis);
               
                */
                string strMenu = "Analysis";

                DataRow[] foundRows = dset.Tables[0].Select("FeatureSetName='" + strMenu + "' and Enabled=1");
                if (foundRows.Length > 0)
                    User_Menu.Items.Add(MIDataAnalysis);
                strMenu = "StatusCheckList";
                foundRows = dset.Tables[0].Select("FeatureSetName='" + strMenu + "'  and Enabled=1");
                if (foundRows.Length > 0)
                    User_Menu.FindItem(MIDataAnalysis.Value).ChildItems.Add(MIUPStatusCheckList);

            }

        }


        protected void AdminMenu_Onclick(object sender, MenuEventArgs e)
        {

            if (User_Menu.SelectedValue == "StatusCheckList")
            {
                Response.Redirect("StatusReminder.aspx");
            }




        }


    }
}