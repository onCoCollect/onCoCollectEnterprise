﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ePxCollectDataAccess;
using System.Data;


namespace ePxCollectWeb
{
    public partial class FrequencyPopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }

            if (!IsPostBack)
            {            

                if (Session["LableMessage"] != null)
                {
                    bindData();
                   // bindGrid();
                }

            }

       
        }
        public void bindData()
        {
             var filterValue =Page.Request.QueryString["filterText"].ToString();
            var lblMessage = Session["LableMessage"].ToString();
            freView.AutoGenerateColumns = false;

            ///Commented by Srinivas Dec 30,2014
          string val = GlobalValues.glbFromClause + " where";
                
              

           string str = val + " " + " " + lblMessage + " " + "group by" + " " + "[" + filterValue + "]";
               string strquery = "select" + " " + "[" + filterValue + "]" + "," + "count(" + "[" + filterValue + "]" + ") as Frequency  " + " " + str;            

            DataSet dscopy = GlobalValues.ExecuteDataSet(strquery);

            //START: Formatting all DateTime columns into Date
            DataTable newDataTable = dscopy.Tables[0].Clone();
            string[] valColumnName;
            string columnName = string.Empty;
            foreach (DataColumn dc in newDataTable.Columns)
            {
                if (dc.DataType == typeof(DateTime))
                {
                    dc.DataType = typeof(string);
                    columnName += dc.ColumnName + ",";
                }
            }
            if (columnName.Length > 0)
            {
                columnName = columnName.Substring(0, columnName.Length - 1);
                valColumnName = columnName.Split(',');
                foreach (DataRow dr in dscopy.Tables[0].Rows)
                {
                    newDataTable.ImportRow(dr);
                }
                foreach (DataRow row in newDataTable.Rows)
                {
                    foreach (string cName in valColumnName)
                    {
                        if (!string.IsNullOrEmpty(row[cName].ToString()))
                        {
                            DateTime dt = DateTime.Parse(row[cName].ToString());
                            row[cName] = dt.ToString("dd-MMM-yyyy");
                        }
                    }
                }
                newDataTable.AcceptChanges();
                dscopy = new DataSet();
                dscopy.Tables.Add(newDataTable);
            }
            //END: Formatting all DateTime columns into Date
            double allcolmnsum = 0;
            double eachpercentage = 0;
            //double rowpercentage = 0;
            double standardvalue = 100;
          
            for (int i = 0; i < dscopy.Tables[0].Rows.Count; i++)
            {
                allcolmnsum = allcolmnsum + Convert.ToDouble(dscopy.Tables[0].Rows[i][1].ToString());
            }

           // eachpercentage = standardvalue / allcolmnsum;
            eachpercentage = standardvalue / (allcolmnsum == 0 ? 1 : allcolmnsum);

            DataColumn percentagecolumn = new DataColumn("Percentage");
            percentagecolumn.DataType = System.Type.GetType("System.String");
            dscopy.Tables[0].Columns.Add(percentagecolumn);

            var rowCount = 0;
            var sumOfPercentage = 0.0;

            for (int i = 0; i < dscopy.Tables[0].Rows.Count; i++)
            {
                //dscopy.Tables[0].Rows[i][2] = Math.Round(Convert.ToInt64(dscopy.Tables[0].Rows[i][1]) * eachpercentage);

                //sumOfPercentage = sumOfPercentage + Convert.ToInt32(dscopy.Tables[0].Rows[i][2]);

                var prct = Math.Round(Convert.ToInt64(dscopy.Tables[0].Rows[i][1]) * eachpercentage);
                dscopy.Tables[0].Rows[i][2] = prct;
                sumOfPercentage += prct;
                rowCount = rowCount + Convert.ToInt32(dscopy.Tables[0].Rows[i][1]);
            }


            try
            {
                DataSet ds = new DataSet();
                ds = dscopy;
                DataTable dt = new DataTable();

                //gridview freview = new gridview();
                //freview.width = unit.pixel(600);
                //freview.autogeneratecolumns = false;
                // gridview freview = new gridview();

                //  catfield.headerstyle.horizontalalign = system.web.ui.webcontrols.horizontalalign.center;
                //   catfield.headerstyle.horizontalalign = system.web.ui.webcontrols.horizontalalign.left;



                DataRow newCustomersRow = ds.Tables[0].NewRow();

                newCustomersRow["Frequency"] = rowCount;
                newCustomersRow["Percentage"] = sumOfPercentage;

                ds.Tables[0].Rows.Add(newCustomersRow);
                //DataTable TablewithTotal = new DataTable("fdt");
                //DataRow dr = fdt.NewRow();
                ////  dr["Category"] ="Total";
                //dr[1] = "Total Frequency  :" + rowCount;
                //dr[2] = "Total Percentage :" + sumOfPercentage;
                //fdt.Rows.Add(dr);

                DataTable fdt = new DataTable();
               
                DataColumn Col = ds.Tables[0].Columns.Add("formattedColumn", System.Type.GetType("System.String"));
                Col.SetOrdinal(0);
                fdt = ds.Tables[0];

              


                for (int rCount = 0; rCount < fdt.Rows.Count;rCount++ )
                {
                    if(fdt.Rows[rCount][1].ToString() == "")
                    {
                        fdt.Rows[rCount][0] = rCount + 1 != fdt.Rows.Count ? "-" : "Total"; 
                         
                        // fdt.Rows[rCount][0] = "";
                        
                    }
                    else
                    {
                        fdt.Rows[rCount][0] = fdt.Rows[rCount][1].ToString();
                    }
                }

                Session["dataTable"] = fdt;


                BoundField catfield = new BoundField();
                catfield.HeaderText = ds.Tables[0].Columns[1].ColumnName;
                catfield.HeaderStyle.Width = 200;
                catfield.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                //  catfield.itemstyle.horizontalalign = system.web.ui.webcontrols.horizontalalign.right;
                catfield.ItemStyle.Width = 100;
                catfield.DataField = ds.Tables[0].Columns[0].ToString();
                freView.Columns.Add(catfield);

                BoundField recfield = new BoundField();
                recfield.HeaderText = "Frequency";
                recfield.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                
                recfield.HeaderStyle.Width = 300;
                recfield.ItemStyle.Width = 300;
                recfield.DataField = ds.Tables[0].Columns[2].ToString();
                freView.Columns.Add(recfield);

                BoundField percentfield = new BoundField();

                percentfield.HeaderText = "Percentage";
                percentfield.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
               
                percentfield.HeaderStyle.Width = 200;
                //percentfield.itemstyle.horizontalalign = system.web.ui.webcontrols.horizontalalign.right;
                percentfield.ItemStyle.Width = 100;
                percentfield.DataField = ds.Tables[0].Columns[3].ToString();
                freView.Columns.Add(percentfield);

                freView.PageSize = 21;

                freView.DataSource = fdt;
                
                freView.DataBind();
                //freView.Columns[1].Visible = false;
              //  freView.Columns[0].HeaderText = ds.Tables[0].Columns[1].ColumnName;// "DateofBirth";
                // pnlgridview.controls.add(freview);
            }
            catch (Exception exp)
            {

                Response.Write(exp.Message);
            }
        }

        protected void IndexChanging(object sender, GridViewPageEventArgs e)
        {
          freView .PageIndex  = e.NewPageIndex;
          bindGrid();

         // bindData();

        }
        void bindGrid()
        {
            DataTable dt = (DataTable)  Session["dataTable"] ;
            freView.DataSource = dt;
            freView.DataBind();
        }
    }
}