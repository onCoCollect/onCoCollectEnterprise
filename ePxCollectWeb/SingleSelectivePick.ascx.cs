using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ePxCollectWeb
{
    public partial class SingleSelectivePick : System.Web.UI.UserControl
    {
        string FldName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSql = string.Empty;
            string patientId = string.Empty;
            FldName = Convert.ToString(Request.QueryString["FN"]);
            if (FldName.Split('.').Length > 0) { FldName = FldName.Split('.')[1].ToString(); }
            FldName = FldName.Replace("[", "").Replace("]", "");
            patientId = Convert.ToString(Session["PatientID"]);
            if (!IsPostBack)
            {
                DataRow dr;
                strSql = "Select Diagnosis from Patientdetails_1 where Patient = '" + patientId + "'";
                DataSet ds = GlobalValues.ExecuteDataSet(strSql);
                dr = ds.Tables[0].NewRow();
                dr[0] = "";
                ds.Tables[0].Rows.InsertAt(dr, 0);
                dpByDiag.DataSource = ds.Tables[0];
                dpByDiag.DataTextField = "Diagnosis";
                dpByDiag.DataValueField = "Diagnosis";
                dpByDiag.DataBind();
                strSql = "Select StudyName from Studies where Active=1 and Instances like '%-" + GlobalValues.gInstanceID + "-%'";
                ds = GlobalValues.ExecuteDataSet(strSql);
                dr = ds.Tables[0].NewRow();
                dr[0] = "";
                ds.Tables[0].Rows.InsertAt(dr, 0);
                dpByStudy.DataSource = ds.Tables[0];
                dpByStudy.DataTextField = "StudyName";
                dpByStudy.DataValueField = "StudyName";
                dpByStudy.DataBind();
                dpAllValues.Items.Add("");
                dpAllValues.Items.Add("All Values");
                if (GlobalValues.gMenuDropVal.ToString().Length > 0)
                {
                    dpByStudy.SelectedItem.Text = GlobalValues.gMenuDropVal.ToString();
                    dpByStudy_SelectedIndexChanged(null, null);
                }
            }
        }

        protected void dpAllValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dpAllValues.SelectedItem.Text != "")
            {
                dpByDiag.SelectedItem.Text = "";
                dpByStudy.SelectedItem.Text = "";
                string SQL = "Select [FieldValues] from PDFields where [Field Name] ='" + FldName + "'";
                PopulateValues(SQL);
            }
        }

        protected void dpByDiag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dpByDiag.SelectedItem.Text != "")
            {
                dpAllValues.SelectedItem.Text = "";
                dpByStudy.SelectedItem.Text = "";
                string SQL = "Select FieldValue from FieldValues_ByDiagnosis where FieldName ='" + FldName + "' and DiagnosisName = '" + dpByDiag.SelectedItem.Text + "'";
                PopulateValues(SQL);
            }
        }

        protected void dpByStudy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dpByStudy.SelectedItem.Text != "")
            {
                dpAllValues.SelectedItem.Text = "";
                dpByDiag.SelectedItem.Text = "";
                string SQL = "Select FieldValue from FieldValues_ByStudy where FieldName='" + FldName + "' and StudyName = '" + dpByStudy.SelectedItem.Text + "'";
                PopulateValues(SQL);
            }

        }
        private void PopulateValues(string strSQL)
        {
            DataSet ds = GlobalValues.ExecuteDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lstValues.Items.Clear();
                string ColVal = dr[0].ToString();
                if (ColVal == "N/A") ColVal = "";
                string[] Cols = ColVal.Split(',');
                for (int I = 0; I < Cols.Length; I++)
                {

                    lstValues.Items.Add(Cols[I].ToString());
                }
            }
        }

        
    }
}