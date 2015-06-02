using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace ePxCollectWeb
{
    public partial class ViewAuditLog : System.Web.UI.Page
    {
        string patientId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }

            if (Session["PatientID"] != null)
            {
                patientId = (string)Session["PatientID"];
            }
            else
            {
                Response.Redirect("SearchPatient.aspx");
            }
            lblPID.Text = " Patient ID : " + patientId;
            if (!IsPostBack)
            {
                string sqlQuery = "select replace([Table Name],'PatientDetails','Audit_PatientDetails')+ '.['+ [Field Name] +']' as TableName, [Field Name] as FieldName from PDFields where [Table Name] like 'PatientDetails_%' and [Field Name] not in ('Patient','PatientID','PatientName','HospitalFileNo') order by FieldOrder";
                DataSet dsColumns = GlobalValues.ExecuteDataSet(sqlQuery);
                ddlColumns.DataSource = dsColumns;
                ddlColumns.DataTextField = "FieldName";
                ddlColumns.DataValueField = "TableName";
                ddlColumns.DataBind();
                sqlQuery = string.Empty;
                foreach (DataRow dr in dsColumns.Tables[0].Rows)
                {
                    sqlQuery += ","+ dr["TableName"].ToString();
                }
                //sqlQuery = sqlQuery.Substring(1);
                sqlQuery = "Select   	 HospitalFileNo, [UserName] [Updated By], CreatedDate [Updated On]  " + sqlQuery
                    + GlobalValues.glbFromClause.ToString().Replace("PatientDetails", "Audit_PatientDetails") + " inner join Audit_Header on Audit_PatientDetails_0.PatientID=Audit_Header.PatientID Where Audit_PatientDetails_0.PatientId  ='" + patientId + "'";

                //sqlQuery = "Select top 1000  	[UserName] [Last Updated By],	[HospitalFileNo] ,	[DateOfRegistration] ,	[DateOfBirth] ,	[AgeRange] ,	[Age at Diagnosis] ,	[Sex] ,	[Address] ,	[City_Town] ,	[State] ,	[PhoneNumber] ,	[EmailID] ,	[ConsultantsName] ,	[Referral Physician] ,	[A_Second_Oncology_Consultation] ,	[Height - in Cm] ,	[Locked] ,	[Primary Treatment Intent] ,	[SortID] ,	[Country] ,	[Diagnosis] ,	[DateOfDiagnosis] ,	[pT_TumourSize] ,	[pN_LymphNodes] ,	[M_Metastases] ,	[PathalogicalStage] ,	[Primary Tumor Size _Clinical] ,	[Regional Nodes Palpable] ,	[Fixity of Nodes] ,	[StatusPost1stRx] ,	[1st RecurrencePattern] ,	[Status] ,	[StatusDate] ,	[CauseOfDeath] ,	[DateOfDeath] ,	[SecondPrimary] ,	[SecondPrimary_Details] ,	[1st line Systemic Therapy] ,	[Investigations] ,	[Synchronous Primary] ,	[1st Chemo Tolerance] ,	[Days since Followup] ,	[Patient Not Reachable On] ,	[First Recurrence] ,	[Second Recurrence] ,	[Third Recurrence] ,	[Fourth Recurrence] ,	[1st Line Drug Group] ,	[1st Line Best Response] ,	[2nd Line Best Response] ,	[3rd Line Best Response] ,	[4th Line Best Response] ,	[5th Line Best Response] ,	[Performance Status at Presentation] ,	[Performance Status at 2nd Line] ,	[Performance Status at 3rd Line] ,	[Performance Status at 4th Line] ,	[Performance Status at 5th Line] ,	[2nd Line Drug Group] ,	[3rd Line Drug Group] ,	[4th Line Drug Group] ,	[5th Line Drug Group] ,	[YearofDiagnosis] ,	[2nd Chemo Tolerance] ,	[3rd Chemo Tolerance] ,	[4th Chemo Tolerance] ,	[5th Chemo Tolerance] ,	[PFS1] ,	[PFS2] ,	[PFS3] ,	[PFS4] ,	[SurfaceArea] ,	[PresentHistory] ,	[PastHistory] ,	[Social History] ,	[Occupational History] ,	[Previous Malignancies] ,	[OnExamination] ,	[OtherDetails] ,	[RadioTherapy] ,	[Surgery] ,	[SystemicTherapy] ,	[TypeOfBMT] ,	[Allergies] ,	[Weight Loss at Presentation] ,	[Tumor Related Symptoms] ,	[Prior Tumor Therapy] ,	[STA_OS] ,	[1st Line Neutropenia] ,	[2nd Line Neutropenia] ,	[3rd Line Neutropenia] ,	[4th Line Neutropenia] ,	[5th Line Neutropenia] ,	[1st Line Leukocytopenia] ,	[2nd Line Leukocytopenia] ,	[3rd Line Leukocytopenia] ,	[4th Line Leukocytopenia] ,	[5th Line Leukocytopenia] ,	[1st Line Thrombocytopenia] ,	[2nd Line Thrombocytopenia] ,	[3rd Line Thrombocytopenia] ,	[4th Line Thrombocytopenia] ,	[5th Line Thrombocytopenia] ,	[1st Line Anemia] ,	[2nd Line Anemia] ,	[3rd Line Anemia] ,	[4th Line Anemia] ,	[5th Line Anemia] ,	[1st Line Febrile Neutropenia] ,	[2nd Line Febrile Neutropenia] ,	[3rd Line Febrile Neutropenia] ,	[4th Line Febrile Neutropenia] ,	[5th Line Febrile Neutropenia] ,	[1st Line Nausea and Vomiting] ,	[2nd Line Nausea and Vomiting] ,	[3rd Line Nausea and Vomiting] ,	[4th Line Nausea and Vomiting] ,	[5th Line Nausea and Vomiting] ,	[1st Line diarrhea] ,	[2nd Line diarrhea] ,	[3rd Line diarrhea] ,	[4th Line diarrhea] ,	[5th Line diarrhea] ,	[1st Line Oral Mucositis] ,	[2nd Line Oral Mucositis] ,	[3rd Line Oral Mucositis] ,	[4th Line Oral Mucositis] ,	[5th Line Oral Mucositis] ,	[1st Line Skin Rash] ,	[2nd Line Skin Rash] ,	[3rd Line Skin Rash] ,	[4th Line Skin Rash] ,	[5th Line Skin Rash] ,	[1st Line Hand Foot Syndrome] ,	[2nd Line Hand Foot Syndrome] ,	[3rd Line Hand Foot Syndrome] ,	[4th Line Hand Foot Syndrome] ,	[5th Line Hand Foot Syndrome] ,	[1st Line Hypertension] ,	[2nd Line Hypertension] ,	[3rd Line Hypertension] ,	[4th Line Hypertension] ,	[5th Line Hypertension] ,	[1st Line Peripheral Neuropathy] ,	[2nd Line Peripheral Neuropathy] ,	[3rd Line Peripheral Neuropathy] ,	[4th Line Peripheral Neuropathy] ,	[5th Line Peripheral Neuropathy] ,	[1st Line Cardiac LV Function] ,	[2nd Line Cardiac LV Function] ,	[3rd Line Cardiac LV Function] ,	[4th Line Cardiac LV Function] ,	[5th Line Cardiac LV Function] ,	[1st Line Pulmonary Function] ,	[2nd Line Pulmonary Function] ,	[3rd Line Pulmonary Function] ,	[4th Line Pulmonary Function] ,	[5th Line Pulmonary Function] ,	[1st Line Renal Function] ,	[2nd Line Renal Function] ,	[3rd Line Renal Function] ,	[4th Line Renal Function] ,	[5th Line Renal Function] ,	[1st Line Proteinuria] ,	[2nd Line Proteinuria] ,	[3rd Line Proteinuria] ,	[4th Line Proteinuria] ,	[5th Line Proteinuria] ,	[1st Line Systemic Therapy Type] ,	[2nd Line Systemic Therapy Type] ,	[3rd Line Systemic Therapy Type] ,	[4th Line Systemic Therapy Type] ,	[5th Line Systemic Therapy Type] ,	[Radiation_Notes] ,	[RT Skin Toxicity] ,	[RT Mucosal Toxicity] ,	[RT Oesophagus Toxicity] ,	[RT Rectal Toxicity] ,	[RT Bladder Toxicity] ,	[RT GI Toxicity] ,	[RT Lung Toxicity] ,	[RT Other Toxicity] ,	[Weight in KG -at Diagnosis] ,	[Paraffin Blocks Available] ,	[Colon -Site of Primary] ,	[Rectum: Distance from Anal Verge-in cm] ,	[Distance from Anal Verge Measured By] ,	[Liver Metastasis at presentation] ,	[Extra hepatic disease at presentation] ,	[Liver Function Tests] ,	[Postal Code] ,	[Albumin at presentation] ,	[Surgical Technique] ,	[Colorectal Surgical Procedure] ,	[Liver Resection] ,	[Neoadjuvant Treatment for Rectum] ,	[Level of Anastomosis from Anal Verge -in cm] ,	[Anastomosis] ,	[Anastomotic Leak] ,	[Covering Stoma] ,	[Covering Stoma closed subsequently] ,	[Hartmanns procedure successfully reversed] ,	[CRM - in mm] ,	[pCR] ,	[Liver Metastasis Resectability] ,	[Site of Liver Metastasis] ,	[Largest Liver Lesion Size - in cm] ,	[From Dx to liver Mets - in Months] ,	[Liver Mets Local Therapy] ,	[Resection of liver Metastases] ,	[Timing of Radiotherapy] ,	[Concurrent Chemotherapy Drugs] ,	[Follow-up Colonoscopy] ,	[Type of detection of 1st Recurrence] ,	[Monthly Family Income] ,	[Total cost of Surgery] ,	[Total cost of 1st line systemic therapy] ,	[Out of pocket expenses for 1st line therapy] ,	[1st Recurrence detection method] ,	[RT Late Mucosal Toxicity] ,	[RT Late Skin Toxicity] ,	[2nd Surgery] ,	[2nd Surgery Details] ,	[AncillaryDetail] ,	[Blood Loss] ,	[Blood Replaced] ,	[Co-Morbidities] ,	[DateOfSurgery] ,	[LymphNode Dissection] ,	[NameOfSurgery] ,	[No Of Days Of Hospitalisation] ,	[Post OP Mortality] ,	[Surgery Reconstruction] ,	[Surgical Morbidity] ,	[Surgical Notes] ,	[TypeOfSurgery] ,	[Wound healing] ,	[Surgical Procedure] ,	[Object Of Radiotherapy] ,	[Type Of Radiotherapy] ,	[Machine] ,	[R/T Technique] ,	[Method Of Brachy] ,	[R/T Fractions/Week] ,	[R/T Dose/Fraction] ,	[R/T Treatment Breaks] ,	[Cause of R/T Treatment Breaks] ,	[Response of Primary to R/T] ,	[Response of Nodes to R/T] ,	[1st Line Sites of Metastases] ,	[2nd Line Sites of Metastases] ,	[3rd Line Sites of Metastases] ,	[4th Line Sites of Metastases] ,	[5th Line Sites of Metastases] ,	[1st Line Weight at Start of Line] ,	[2nd Line Weight at Start of Line] ,	[3rd Line Weight at Start of Line] ,	[4th Line Weight at Start of Line] ,	[5th Line Weight at Start of Line] ,	[R/T Fractions/Day] ,	[R/T Machine] ,	[R/T Total Dose] ,	[Biopsy] ,	[Guided Biopsy] ,	[Biopsy Representative] ,	[Histology] ,	[Primary Tumor Size _Pathological] ,	[Tumour Grade] ,	[Vascular Invasion] ,	[Lymphatic Invasion] ,	[Perineural Invasion] ,	[Margins] ,	[Lymphnodes] ,	[Number Of Nodes Removed] ,	[Number Of Positive Nodes] ,	[Extra Capsular Spread] ,	[Chromosomal Abberations] ,	[STA_PFS1] ,	[STA_PFS2] ,	[STA_PFS3] ,	[STA_PFS4] ,	[OS] ,	[1st Line Chemo End Date] ,	[1st Line Chemo Start Date] ,	[1st Line Hormone Therapy] ,	[1st Line No of Cycles of Chemo] ,	[1st Line Other Drugs] ,	[1st Line Support Therapy] ,	[2nd Line Chemo End Date] ,	[2nd Line Chemo Start Date] ,	[2nd Line Hormone Therapy] ,	[2nd Line No of Cycles of Chemo] ,	[2nd Line Other Drugs] ,	[2nd Line Support Therapy] ,	[3rd Line Chemo End Date] ,	[3rd Line Chemo Start Date] ,	[3rd Line Hormone Therapy] ,	[3rd Line No of Cycles of Chemo] ,	[3rd Line Other Drugs] ,	[3rd Line Support Therapy] ,	[4th Line Chemo End Date] ,	[4th Line Chemo Start Date] ,	[4th Line Hormone Therapy] ,	[4th Line No of Cycles of Chemo] ,	[4th Line Other Drugs] ,	[4th Line Support Therapy] ,	[5th Line Chemo End Date] ,	[5th Line Chemo Start Date] ,	[5th Line Hormone Therapy] ,	[5th Line No of Cycles of Chemo] ,	[5th Line Other Drugs] ,	[5th Line Support Therapy] ,	[Adjuvant Hormone Therapy] ,	[Cardiac Toxicity] ,	[CEA at Presentation] ,	[Clinical Stage] ,	[cN Lymph Nodes] ,	[Colon - 1st Recurrence Type] ,	[cT Tumor Size] ,	[Date of Liver Surgery] ,	[DEB-TACE No of Procedures] ,	[Family History of Cancer] ,	[Haemoglobin at Presentation] ,	[History of Cancer In] ,	[Kras] ,	[Late Cystitis] ,	[Late Fistula] ,	[Late Proctitis] ,	[Late Stricture] ,	[Late Toxicity] ,	[MSI] ,	[No of Liver Mets] ,	[Patient with Insurance] ,	[Primary Surgery] ,	[Prior Treatment Cost] ,	[Registered as Private Patient] ,	[Resection R0] ,	[RFA No of Procedures] ,	[Sexual Dysfunction] ,	[Staging Done By] ,	[Stoma Function] ,	[Stoma Type] ,	[TACE No of Procedures] ,	[TARE No of Procedures] ,	[Total Cost of Radiotherapy] ,	[Total Treatment Cost] ,	[TRG Grade] ,	[Type of Local Therapy for Liver Mets] ,	[Type of Neo-Adjuvant Rx for Rectum] ,	[Second Primary Diagnosis] ,	[ER] ,	[PR] ,	[Her2New FISH] ,	[Ki67 as Percentage] ,	[Breast IHC Profile] ,	[Her2New IHC] ,	[CA19-9 at Presentation] ,	[Mobile Number] ,	[R/T Start Date] ,	[R/T End Date] ,	[Time to Completion of R/T -in weeks] ,	[Type of Reconstruction Surgery] ,	[Histologic Differentiation] ,	[Aspirin Usage] ,	[Clinical Trial Patient] ,	[Colonic Stent] ,	[Date of Brain Mets] ,	[LDH at Presentation] ,	[Lymphocytic Infiltrate] ,	[Metformin Usage] ,	[MLH1] ,	[Patient Consented For Study] ,	[Patient Consented For Tissue Analysis] ,	[Platelets at Presentation] ,	[Primary Dx Done Locally] ,	[Statin Usage] ,	[Surgery in Local Institute] ,	[WBC at Presentation] ,	[GCSF used in 1st Line] ,	[GCSF used in 2nd Line] ,	[GCSF used in 3rd Line] ,	[GCSF used in 4th Line] ,	[GCSF used in 5th Line] ,	[Peg-GCSF used in 1st Line] ,	[Peg-GCSF used in 2nd Line] ,	[Peg-GCSF used in 3rd Line] ,	[Peg-GCSF used in 4th Line] ,	[Peg-GCSF used in 5th Line] ,	[PMS2] ,	[MSH6] ,	[MSH2] ,	[Family History of Cancer-Type] ,	[Family History of Same Cancer] ,	[Smoking] ,	[Breast Receptor Pofile] ,	[Total Cost of Primary Treatment] ,	[2nd Line Post Adjuvant Chemo] ,	[WHO Classification] ,	[Extra Nodal Primary Site] ,	[Extent of Disease] ,	[B Symptoms] ,	[Bulky Disease- >10cms] ,	[Largest mass size in cms] ,	[Large Mediastinal Mass] ,	[Bone Marrow Biopsy] ,	[Bone Marrow Involvement] ,	[Number of nodal sites involved] ,	[Number of Extra-nodal sites involved] ,	[Ann Arbor Stage] ,	[IPI -Age greater than 60 years] ,	[IPI -Stage of the disease -III or greater] ,	[IPI -Poor performance status] ,	[IPI -More than 1 extra nodal sites] ,	[IPI -Elevated levels of LDH] ,	[Total IPI score] ,	[IPI Risk Group] ,	[FLIPI -Age greater than 60 years] ,	[FLIPI -Stage of the disease -III or greater] ,	[FLIPI -More than 4 lymph node groups involved] ,	[FLIPI -Serum haemoglobin less than 12gm] ,	[FLIPI -Elevated levels of LDH] ,	[Total FLIPI score] ,	[FLIPI Risk Group] ,	[Hodgkins IPI- Age greater than 45 years] ,	[Hodgkins IPI- Stage IV disease] ,	[Hodgkins IPI- Haemoglobin Less than 10 5gm] ,	[Hodgkins IPI- Lymphocyte count less than (600 or 8 percent)] ,	[Hodgkins IPI- Male] ,	[Hodgkins IPI- Albumin less than 4gm] ,	[Hodgkins IPI- White blood count greater than 15000] ,	[Total Hodgkins IPI score] ,	[1st Line Rituximab] ,	[1st Line Rituximab -mg per m2] ,	[1st Line Rituximab -No of Cycles] ,	[1st Line Rituximab Maintenance] ,	[Rituximab Infusion reaction] ,	[Rituximab Infusion reaction Type] ,	[CNS Prophylaxis] ,	[Cranial RT] ,	[1st Line Interim Response Evaluation] ,	[1st Line Response evaluation - Clinical] ,	[1st Line Response evaluation - Ultrasound] ,	[1st Line Response evaluation - CT Scan] ,	[1st Line Response evaluation - MRI Scan] ,	[1st Line Response evaluation - PET CT Scan] ,	[1st Line - Chemo Dose reduction] ,	[1st Line - Cause of dose reduction] ,	[1st Line Chemo Dose Reduction Percentage] ,	[1st Line Delay in Cycles] ,	[1st Line Cause of delay in Cycles] ,	[1st Line Treatment Discontinued] ,	[1st Line Cause for Discontinuation] ,	[LCA] ,	[CD3] ,	[CD5] ,	[CD10] ,	[CD15] ,	[CD19] ,	[CD20] ,	[CD23] ,	[CD30] ,	[CD43] ,	[CD45] ,	[CD79a] ,	[Kappa] ,	[Lambda] ,	[bcl-2] ,	[FMC7] ,	[CyclinD1] ,	[Lymphoma IHC Profile] ,	[Beta2 Microglobulin at Presentation] ,	[Extra Nodal Sites] ,	[1st Line Rituximab Details] ,	[1st Line Response evaluation By] ,	[1st Line Treatment Alterations] ,	[Hypertension] ,	[Diabetes Mellitus] ,	[Ischaemic Heart Disease] ,	[Dyslipidiemia] ,	[Hypothyroid] ,	[COPD] ,	[Hepatitis B] ,	[Hepatitis C] ,	[Tuberculosis] ,	[CVA_Stroke] ,	[Venous Thromboembolism] ,	[Presenting Symptoms] ,	[Disease Detection] ,	[Age at First Child in Yrs] ,	[Age at Menarche in Yrs] ,	[Menstrual Status] ,	[Parity] ,	[Primary Side] ,	[Primary Symptom Duration in Months] ,	[Primary Tumour Size Clinical in Cm] ,	[R/T Boost] ,	[R/T Boost Details] ,	[R/T Boost Dose] ,	[R/T Boost Dose/Fraction] ,	[R/T Boost Fractions] ,	[R/T Boost Site] ,	[R/T Boost Timing] ,	[R/T Energy] ,	[R/T Site] ,	[R/T Total Dose Planned] ,	[R/T Total No of Fractions] ,	[Sentinel Lymph Node Bx] ,	[Skin Changes] ,	[Supraclavicular Lymph Node Involved] ,	[R/T Acute Toxicity] ,	[ICD Code] ,	[Post OP CEA Decreased] ,	[Reconstructive Surgical Procedure] ,	[BMT Timing] ,	[Pre BMT Chemo Sensitivity] ,	[Pre BMT Status] ,	[Relative Phone No] ,	[1st RecurrenceDate] ,	[2nd RecurrenceDate] ,	[3rd RecurrenceDate] ,	[4th RecurrenceDate]  ,	[CreatedDate]  "
                //   + GlobalValues.glbFromClause.ToString().Replace("PatientDetails", "Audit_PatientDetails") + " inner join Audit_Header on Audit_PatientDetails_0.PatientID=Audit_Header.PatientID Where Audit_PatientDetails_0.PatientId  ='" + patientId + "'";
                BindGrid(sqlQuery);
            }
        }

        private void BindGrid(string strSql)
        {
            DataSet dsAudit = new DataSet();
            dsAudit = GlobalValues.ExecuteDataSet(strSql);
            Session["dsAudit"] = dsAudit;
            grdAuditRes.DataSource = dsAudit;
            grdAuditRes.DataBind();
        }
        protected void grdAuditRes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Session["dsAudit"] != null)
            {
                DataSet dsAudit = (DataSet)Session["dsAudit"];
                //grdAnalysisRes.DataSource = dsResult.Tables[0];
                //grdAnalysisRes.DataBind();
                //lblCaption.Text += " " + dsResult.Tables[0].Rows.Count.ToString();
                grdAuditRes.PageIndex = e.NewPageIndex;
                grdAuditRes.DataSource = dsAudit;
                grdAuditRes.DataBind();

            }
        }

        protected void ddlColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = string.Empty;
            try
            {
                text = getControlSelection()[0];
                ddlColumns.Texts.SelectBoxCaption = getControlSelection()[1];
                string sqlQuery = "Select   HospitalFileNo,	[UserName] [Last Updated By], CreatedDate [Updated On],  " + text + " " 
                   + GlobalValues.glbFromClause.ToString().Replace("PatientDetails", "Audit_PatientDetails") + " inner join Audit_Header on Audit_PatientDetails_0.PatientID=Audit_Header.PatientID Where Audit_PatientDetails_0.PatientId  ='" + patientId + "'";
                BindGrid(sqlQuery);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Audit log refresh failed. Please try later.');", true);
            }
        }

        private string[] getControlSelection()
        {
            string[] val = new string[2];

            try
            {
                int i = 0; int allIndex = -1; int selCnt = 0;
                foreach (ListItem li in ddlColumns.Items)
                {
                    i++;
                    if (li.Selected)
                    {
                        selCnt++;
                        if (li.Text.ToUpper().ToString() == "ALL")
                            allIndex = i - 1;
                    }
                }
                if (selCnt > 1 && allIndex != -1)
                {
                    ddlColumns.Items[allIndex].Selected = false;
                }
                foreach (ListItem li in ddlColumns.Items)
                {
                    if (li.Selected)
                    {
                        val[0] += li.Value + ",";
                        val[1] += li.Text + ",";
                    }

                }

                val[0] = (!string.IsNullOrEmpty(val[0])) ? val[0].Substring(0, val[0].Length - 1) : string.Empty;
                val[1] = (!string.IsNullOrEmpty(val[1])) ? val[1].Substring(0, val[1].Length - 1) : string.Empty;
            }

            catch (Exception ex)
            {

            }

            return val;
        }
    }
}