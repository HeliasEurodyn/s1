using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Softone;

namespace PiraeusBankImport
{
    public partial class SaldocQuantitites : Form
    {
        public XSupport XSupport = null;
        List<MTRLINESCS> MTRLINESCSList = new List<MTRLINESCS>();

        public SaldocQuantitites()
        {
            InitializeComponent();
        }


        public void loadSeriesToCombobox()
        {
            Dictionary<int, String> DICT = new Dictionary<int, String>();

            String sql =
                " SELECT A.SERIES                    " +
                " ,A.CODE + '  ' + A.NAME AS DESCR   " +
                " FROM SERIES A                      " +
                " WHERE 1=1                          " +
                "     AND A.COMPANY =  " + XSupport.ConnectionInfo.CompanyId.ToString() + 
                "     AND A.SOSOURCE = 1351          " +
                "     AND A.ISACTIVE = 1             " +
                " ORDER BY A.SERIES                  " +
                "     ,A.COMPANY                     " +
                "     ,A.SOSOURCE                    ";


            XTable TRDRDATASql = XSupport.GetSQLDataSet(sql);
            for (int i = 0; i < TRDRDATASql.Count; i++)
            {
                DICT.Add(
                    int.Parse(TRDRDATASql[i, "SERIES"].ToString()),
                     TRDRDATASql[i, "SERIES"].ToString() + "  " + TRDRDATASql[i, "DESCR"].ToString());
            }

            this.SeriesComboBox.DataSource = new BindingSource(DICT, null);
            this.SeriesComboBox.DisplayMember = "Value";
            this.SeriesComboBox.ValueMember = "Key";
           
            if (TRDRDATASql.Count < 0) this.SeriesComboBox.SelectedValue = 9011;
        }



        private void toolStripButton1_Click(object sender, EventArgs e)   
        {
          
        }

        private void SaldocQuantitites_Load(object sender, EventArgs e)
        {
            loadSeriesToCombobox();
            this.SeriesComboBox.SelectedValue = 9011;
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-7);

        }


        private void CalcSumQtys()
        {

            int BALANCETotal = 0;
            int RESERVEDQTYTotal = 0;

            foreach (DataGridViewRow row2 in this.Item2DataGridView.Rows)
            {
                String BALANCEStr = row2.Cells["BALANCE"].Value.ToString();
                String RESERVEDQTYStr = row2.Cells["RESERVEDQTY"].Value.ToString();

                int BALANCEInt = 0;
                int RESERVEDQTYInt = 0;

                Boolean BALANCEParse = false;
                Boolean RESERVEDQTYParse = false;

                BALANCEParse = int.TryParse(BALANCEStr, out BALANCEInt);
                RESERVEDQTYParse = int.TryParse(RESERVEDQTYStr, out RESERVEDQTYInt);

                if (BALANCEParse && RESERVEDQTYParse)
                    {
                    BALANCETotal += BALANCEInt;
                    RESERVEDQTYTotal += RESERVEDQTYInt;
                    }
            }

            this.ITEMREMTextBox.Text = BALANCETotal.ToString();
            this.ItemResTextBox.Text = RESERVEDQTYTotal.ToString();



            // row.Cells[15].Value 
            foreach (DataGridViewRow row in this.ItemDataGridView.Rows)
            {
            
            }



        }



        private void CalcSumQtys2()
        {

            int BALANCETotal = 0;
            int RESERVEDQTYTotal = 0;

            foreach (DataGridViewRow row2 in this.ItemDataGridView.Rows)
            {
                String BALANCEStr = row2.Cells[13].Value.ToString();
                String RESERVEDQTYStr = row2.Cells[15].Value.ToString();

                int BALANCEInt = 0;
                int RESERVEDQTYInt = 0;

                Boolean BALANCEParse = false;
                Boolean RESERVEDQTYParse = false;

                BALANCEParse = int.TryParse(BALANCEStr, out BALANCEInt);
                RESERVEDQTYParse = int.TryParse(RESERVEDQTYStr, out RESERVEDQTYInt);

                if (BALANCEParse && RESERVEDQTYParse)
                {
                    BALANCETotal += BALANCEInt;
                    RESERVEDQTYTotal += RESERVEDQTYInt;
                }
            }

            this.ITEMREMTextBox2.Text = BALANCETotal.ToString();
            this.ItemResTextBox2.Text = RESERVEDQTYTotal.ToString();

        }


        private void button1_Click(object sender, EventArgs e)
        {

            this.ItemDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
         //   this.ItemDataGridView.Columns[9].DefaultCellStyle.ForeColor = Color.Yellow;
           //     DataGridViewAutoSizeColumnMode.;

            String SeriesStr = ((KeyValuePair<int, String>)this.SeriesComboBox.SelectedItem).Key.ToString();

            String from = dateTimePicker1.Value.Date.ToString("yyyyMMdd");
            String to = dateTimePicker2.Value.Date.ToString("yyyyMMdd");

            String WHouseWhere = "";
            if (this.WHouseTextBox.Text.ToString() != "")
            {
                WHouseWhere = " AND A.WHOUSE IN (" + this.WHouseTextBox.Text.ToString() + ") ";
            }


            String BalanceSql =
               " ISNULL((SELECT                                                                                 " +
               "    SUM( ISNULL(A.OPNIMPQTY1,0) + ISNULL(A.IMPQTY1, 0) - ISNULL(A.EXPQTY1, 0)) AS BALANCE     " +
               "     FROM CDIMFINDATA A                                                                  " +
               "     WHERE 1=1                                                                           " +
               "     AND A.COMPANY = " + XSupport.ConnectionInfo.CompanyId.ToString() +
               "     AND A.FISCPRD = " + DateTime.Now.Year.ToString() + 
               WHouseWhere + 
             //  "     AND A.WHOUSE = 812                                                                  " +
             //  "     AND ISNULL(A.OPNIMPQTY1,0) + ISNULL(A.IMPQTY1, 0) - ISNULL(A.EXPQTY1, 0) > 0        " +
               "     AND A.MTRL  =  M.MTRL                                                                   " +
               "     AND ISNULL(A.CDIMLINES1,0) =  ISNULL(CL.CDIMLINES,0)                                " +
               "     AND ISNULL(A.CDIMLINES2,0) = ISNULL(CL2.CDIMLINES,0)  ),0) AS [ΥΠΟΛΟΙΠΟ] ,              ";

            String ItemWhere = "";
            if (this.ItemTextBox2.Text.ToString() != "")
            {
                ItemWhere = " AND M.CODE LIKE '" + this.ItemTextBox2.Text.ToString().Replace("*", "%") + "' ";
            }

            


  

          //  this.ItemDataGridView.Rows.Clear();

            String Sql =
             " SELECT "+
             " CS.ID,  "+
             " T.SOSCORE AS [Επίδοση], " +
             " T.TRDR AS [ID ΠΕΛΑΤΗ], " +
             " T.NAME AS [ΠΕΛΑΤΗΣ] , " +
             " F.FINDOC AS [ID ΠΑΡΑΣΤΑΤΙΚΟΥ],           " +
             " F.FINCODE AS [ΠΑΡΑΣΤΑΤΙΚΟ],           " +
             " M.MTRL AS [ID ΕΙΔΟΥΣ],    " +
             " M.CODE AS [ΕΙΔΟΣ],    " +
             " CL.CDIMLINES AS [ID ΧΡΩΜΑΤΟΣ]  ,    " +
             " CL.NAME AS [ΧΡΩΜΑ]  ,    " +
             " CL2.CDIMLINES AS [ID ΜΕΓΕΘΟΥΣ]  ,    " +
             " CL2.NAME AS [ΜΕΓΕΘΟΣ] ,     " +
             " CS.QTY1 AS [ΠΟΣ1] ,    " +
             " (CS.QTY1 -(CS.QTY1COV+CS.QTY1CAN)) AS [ΑΝ. ΠΟΣ.],  " +
             BalanceSql +
             " 0 AS [ΑΝΑΘΕΣΗ]  " +
             " FROM FINDOC F                                         " +
             " INNER JOIN MTRLINES ML ON ML.FINDOC = F.FINDOC        " +
             " INNER JOIN MTRL M ON M.MTRL = ML.MTRL                 " +
             " INNER JOIN TRDR T ON T.TRDR = F.TRDR                  " +
             " LEFT OUTER JOIN TRDEXTRA TE ON T.TRDR = TE.TRDR            " +
             " LEFT OUTER JOIN vColorSizeAnalysisRep CS on CS.FINDOC = F.FINDOC AND CS.MTRLINES = ML.MTRLINES                       " +
             " INNER JOIN CDIMLINES CL ON CL.CDIMLINES = CS.CDIMLINES1 AND CL.CDIM = M.CDIM1 AND CL.COMPANY = M.COMPANY      " +
             " INNER JOIN CDIMLINES CL2 ON CL2.CDIMLINES = CS.CDIMLINES2 AND CL2.CDIM = M.CDIM2 AND CL2.COMPANY = M.COMPANY  " +
             " WHERE 1 = 1                                                     " +
              ItemWhere +
             " AND F.SERIES IN (" + SeriesStr + ")                             " +
             " AND F.SODTYPE = 13                                              " +
             " AND F.TRNDATE >= '" + from + "'                                 " +
             " AND F.TRNDATE <= '" + to + "'                                   " +
             " AND F.COMPANY = " + XSupport.ConnectionInfo.CompanyId.ToString() +
             " ORDER BY T.SOSCORE ";
            
             List<String> rowList = new List<String>();
             XTable results = XSupport.GetSQLDataSet(Sql);
             this.ItemDataGridView.DataSource = results.CreateDataTable(true);
             this.ItemDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
          //   this.ItemDataGridView.Columns[9].DefaultCellStyle.ForeColor = Color.Yellow;
            /*
               if (results.Count > 0)
               {
                   for (int i = 0; i < results.Count; i++)
                   {
                       rowList.Add(results[i, "BOOL02"].ToString());
                       rowList.Add(results[i, "TRDRNAME"].ToString());
                       rowList.Add(results[i, "FINCODE"].ToString());
                       rowList.Add(results[i, "TRDR"].ToString());
                       rowList.Add(results[i, "MTRLCODE"].ToString());
                       rowList.Add(results[i, "COLOR"].ToString());
                       rowList.Add(results[i, "SIZE"].ToString());
                       rowList.Add(results[i, "CSQTY1NCOV"].ToString());
                       rowList.Add(results[i, "CSQTY1"].ToString());
                       rowList.Add("");
                       rowList.Add("");
                       rowList.Add("");
                       string[] row = rowList.ToArray();
                       this.ItemDataGridView.Rows.Add(row);
                   }

                

               } */

             CalcSumQtys2();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Item2DataGridView.Rows.Clear();
            ITEMREMTextBox.Text = "";
            float TotBalanceQty = 0;


            String CompanyId = XSupport.ConnectionInfo.CompanyId.ToString();
            String FiscPrd = XSupport.ConnectionInfo.YearId.ToString();

            String sqlWhere = "";
            if (this.ItemTextBox.Text.ToString() != "")
            {
                sqlWhere = " AND MM.CODE LIKE '" + this.ItemTextBox.Text.ToString().Replace("*", "%") + "' ";
            }

            String WHouseWhere = "";
            if (this.WHouseTextBox.Text.ToString() != "")
            {
                WHouseWhere = " AND A.WHOUSE IN (" + this.WHouseTextBox.Text.ToString() + ") ";
            }



            String Sql =
             " SELECT A.COMPANY                                                                                                   " +
             " ,A.MTRL                                                                                                            " +
             " ,A.FISCPRD                                                                                                         " +
             " ,A.CDIM1                                                                                                           " +
             " ,ISNULL(A.CDIMLINES1,0) AS CDIMLINES1                                                                              " +
             " ,A.CDIM2                                                                                                           " +
             " ,ISNULL(A.CDIMLINES2,0) AS CDIMLINES2                                                                              " +
             " ,A.CDIM3                                                                                                           " +
             " ,A.CDIMLINES3                                                                                                      " +
             " ,A.WHOUSE                                                                                                          " +
             " ,MM.CODE AS MTRLCODE                                                                                               " +
             " ,MM.NAME AS MTRLNAME                                                                                               " +
             " ,(SELECT NAME FROM WHOUSE WHERE WHOUSE = A.WHOUSE AND COMPANY = A.COMPANY) AS WHOUSENAME                           " +
             " ,(SELECT NAME FROM CDIMLINES WHERE CDIM = A.CDIM1 AND CDIMLINES = A.CDIMLINES1  AND COMPANY = " + CompanyId + " )  AS CDIM1NAME     " +
             " ,(SELECT NAME FROM CDIMLINES WHERE CDIM = A.CDIM2 AND CDIMLINES = A.CDIMLINES2  AND COMPANY = " + CompanyId + " )  AS CDIM2NAME     " +
             " ,(SELECT NAME FROM CDIMLINES WHERE CDIM = A.CDIM3 AND CDIMLINES = A.CDIMLINES3  AND COMPANY = " + CompanyId + " )  AS CDIM3NAME     " +
             " ,ISNULL(A.OPNIMPQTY1,0) + ISNULL(A.IMPQTY1, 0) - ISNULL(A.EXPQTY1, 0) AS BALANCE                                   " +
             " FROM CDIMFINDATA A INNER JOIN MTRL MM ON MM.MTRL = A.MTRL                                                          " +
             WHouseWhere + 
           //  " WHERE A.FISCPRD = " + FiscPrd + "   " +
           //  " AND A.WHOUSE = 200 " +
             " WHERE A.FISCPRD = " + DateTime.Now.Year.ToString() + 
           //  " AND A.WHOUSE = 812 " +
             " AND ISNULL(A.OPNIMPQTY1,0) + ISNULL(A.IMPQTY1, 0) - ISNULL(A.EXPQTY1, 0) > 0 " +
             sqlWhere;


            XTable results = XSupport.GetSQLDataSet(Sql);
            

            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    List<String> rowList = new List<String>();

                    rowList.Add(results[i, "MTRL"].ToString());

                    rowList.Add(results[i, "CDIM1"].ToString());
                    rowList.Add(results[i, "CDIMLINES1"].ToString());
                    rowList.Add(results[i, "CDIM2"].ToString());
                    rowList.Add(results[i, "CDIMLINES2"].ToString());
                    rowList.Add(results[i, "CDIM3"].ToString());
                    rowList.Add(results[i, "CDIMLINES3"].ToString());

                    rowList.Add(results[i, "WHOUSE"].ToString());
                    rowList.Add(results[i, "WHOUSENAME"].ToString());

                    rowList.Add(results[i, "MTRLCODE"].ToString());
                    rowList.Add(results[i, "MTRLNAME"].ToString());
                    rowList.Add(results[i, "CDIM1NAME"].ToString());
                    rowList.Add(results[i, "CDIM2NAME"].ToString());
                    rowList.Add(results[i, "CDIM3NAME"].ToString());
                    rowList.Add(results[i, "BALANCE"].ToString());
                    rowList.Add("0");
                    string[] row = rowList.ToArray();
                    this.Item2DataGridView.Rows.Add(row);

                    float CurQty = 0;
                    Boolean parsed = false;

                    parsed = float.TryParse( results[i, "BALANCE"].ToString(), out CurQty);
                    if (parsed) TotBalanceQty += CurQty;

                }



            }



            ITEMREMTextBox.Text = "";
            CalcSumQtys();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.SeriesComboBox.SelectedValue.ToString());
         //   this.SeriesComboBox.SelectedValue = (int) 9001;
          //  this.SeriesComboBox.SelectedValue = "9001";
        //    this.SeriesComboBox.SelectedValue = 9001;
           // this.SeriesComboBox.SelectedValue = 9001;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

            String SeriesStr = ((KeyValuePair<int, String>)this.SeriesComboBox.SelectedItem).Key.ToString();

            String from = dateTimePicker1.Value.Date.ToString("d/M/yyyy");
            String to = dateTimePicker2.Value.Date.ToString("d/M/yyyy");

            this.XSupport.ExecS1Command("SALDOC[LIST=Στατιστικά Εκτελεσιμότητας Με Αναθέσεις,AUTOEXEC=1,FORCEFILTERS=FINDOCTRNDATEL:" + from + "?FINDOCTRNDATEH:" + to + "?FINDOCSERIESL:" + SeriesStr + "]", this);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Επανυπολογισμός στατιστικών παραστατικών?", "Επιλογή", MessageBoxButtons.YesNoCancel);
            if (result != DialogResult.Yes)
            {
                return;
            }

            var StartDate = DateTime.Now;


            MTRLINESCSList.Clear();

            String SeriesStr = ((KeyValuePair<int, String>)this.SeriesComboBox.SelectedItem).Key.ToString();

            String from = dateTimePicker1.Value.Date.ToString("yyyyMMdd");
            String to = dateTimePicker2.Value.Date.ToString("yyyyMMdd");



            String Sql =
               " SELECT ML.* FROM FINDOC F                          " +
               " INNER JOIN MTRLINES ML ON ML.FINDOC = F.FINDOC     " +
               " WHERE 1 = 1                                        " +
               " AND F.SERIES IN (" + SeriesStr + ")                " + //9001
               " AND F.SODTYPE = 13                                 " +
               " AND F.COMPANY = " + XSupport.ConnectionInfo.CompanyId.ToString() +
               " AND F.TRNDATE >= '" + from + "'                                 " +
               " AND F.TRNDATE <= '" + to + "'                                   ";

            XTable results = XSupport.GetSQLDataSet(Sql);


            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {

                    string[] lines = Regex.Split(results[i, "SOANAL"].ToString(), ";");

                    if (lines.Count() == 4)
                    {
                        string[] ColorSizes = Regex.Split(lines[3], "~");

                        foreach (string colorSize in ColorSizes)
                        {
                            if (colorSize.ToString() != "")
                            {
                                string[] ColorSizeData = colorSize.Split('|');
                                MTRLINESCS MTRLINEScs = new MTRLINESCS();
                                // MessageBox.Show(ColorSizeData[0].ToString() + " " + ColorSizeData[1].ToString() + " " + ColorSizeData[2].ToString() + " " + ColorSizeData[3].ToString() + " " + ColorSizeData[4].ToString() + " " + ColorSizeData[6].ToString() + " ");
                                //   MTRLINEScs.LINENUM = results[i, "LINENUM"].ToString();
                                MTRLINEScs.MTRLINES = results[i, "MTRLINES"].ToString();
                                MTRLINEScs.FINDOC = results[i, "FINDOC"].ToString();
                                MTRLINEScs.MTRL = results[i, "MTRL"].ToString();

                                MTRLINEScs.CDIMLINES1 = ColorSizeData[0].ToString();
                                MTRLINEScs.CDIMLINES2 = ColorSizeData[1].ToString();
                                MTRLINEScs.CDIMLINES3 = ColorSizeData[2].ToString();
                                MTRLINEScs.QTY1 = ColorSizeData[6].ToString();
                                MTRLINEScs.QTY2 = ColorSizeData[5].ToString();
                                MTRLINEScs.QTY3 = ColorSizeData[8].ToString();

                                MTRLINESCSList.Add(MTRLINEScs);
                            }
                        }

                    }


                }

                String Sql3 = "TRUNCATE TABLE ColorSizeAnalysisSQ ";
                XSupport.ExecuteSQL(Sql3, null);

                foreach (MTRLINESCS MTRLINEScs in MTRLINESCSList)
                {
                    String QTY1 = MTRLINEScs.QTY1;
                    String QTY1COV = MTRLINEScs.QTY2;
                    String QTY1CAN = MTRLINEScs.QTY3;

                    if (QTY1 == "") QTY1 = "0";
                    if (QTY1COV == "") QTY1COV = "0";
                    if (QTY1CAN == "") QTY1CAN = "0";

                    String Sql2 =
                         " INSERT INTO ColorSizeAnalysisSQ  (MTRLINES,FINDOC,MTRL,CDIMLINES1,CDIMLINES2,CDIMLINES3,QTY1,QTY1COV,QTY1CAN) " +
                         " VALUES ( " +
                        " '"+ MTRLINEScs.MTRLINES + "', " +
                        " '" + MTRLINEScs.FINDOC + "', " +
                        " '" + MTRLINEScs.MTRL + "', " +
                        " '" + MTRLINEScs.CDIMLINES1 + "', " +
                        " '" + MTRLINEScs.CDIMLINES2 + "', " +
                        " '" + MTRLINEScs.CDIMLINES3 + "', " +
                        " '" + QTY1 + "', " +
                        " '" + QTY1COV + "', " +
                        " '" + QTY1CAN + "') ";


                    XSupport.ExecuteSQL(Sql2, null);

                }



            }


          
            var EndDate = DateTime.Now;
            var diff = EndDate.Subtract(StartDate);
            String executionTime = String.Format("{0}:{1}:{2}", diff.Hours, diff.Minutes, diff.Seconds);
            MessageBox.Show(executionTime);

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance =
                (int)(splitContainer1.ClientSize.Width * 0.0);
        }
       
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
          

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance =
                (int)(splitContainer1.ClientSize.Width * 0.70);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            //this.splitContainer1.SplitterIncrement = 10;  
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance =
             (int)(splitContainer1.ClientSize.Width * 0.50);
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {

            this.ItemDataGridView.Sort(this.ItemDataGridView.Columns[1], ListSortDirection.Descending);


            foreach (DataGridViewRow row in this.ItemDataGridView.Rows)
            {
               // row.Cells[11].Value = "1"; // Ποσότητα
               // row.Cells[12].Value = "1"; // Ανεκτέλεστη Ποσότητα
               // row.Cells[13].Value = "1"; // Νέα Ποσότητα
                int qtyNCov = 0;
                int qtyReserved = 0;
                bool result = false;
                bool result2 = false;



                String qtyNCovStr = row.Cells[13].Value.ToString();
                String qtyReservedStr = row.Cells[15].Value.ToString();
                if (qtyNCovStr == "") qtyNCovStr = "0";
                if (qtyReservedStr == "") qtyReservedStr = "0";

                result = int.TryParse(qtyNCovStr, out qtyNCov);
                result2 = int.TryParse(qtyReservedStr, out qtyReserved);

                int RequiredQty = qtyNCov;// -qtyReserved;

                if (result && result2 && RequiredQty > 0 )
                {
                    foreach (DataGridViewRow row2 in this.Item2DataGridView.Rows)
                        {

                            if (row2.Cells["RESERVEDQTY"].Value != row2.Cells["BALANCE"].Value.ToString() && row2.Cells["MTRL"].Value.ToString() == row.Cells[6].Value.ToString())
                        {

                                        String MTRLStr = row.Cells[6].Value.ToString();
                                        String MtrlMTRLStr = row2.Cells["MTRL"].Value.ToString();
                                        String CDIMLINES1Str = row.Cells[8].Value.ToString();
                                        String MtrlCDIMLINES1Str = row2.Cells["CDIMLINES1"].Value.ToString();
                                        String CDIMLINES2Str = row.Cells[10].Value.ToString();
                                        String MtrlCDIMLINES2Str = row2.Cells["CDIMLINES2"].Value.ToString();


                                        if (MTRLStr == MtrlMTRLStr &&
                                             CDIMLINES1Str == MtrlCDIMLINES1Str &&
                                             CDIMLINES2Str == MtrlCDIMLINES2Str &&
                                            RequiredQty > 0 
                                
                                            )
                                        {
                              

                                            int MtrlBalance = 0;
                                            int MtrlQtyReserved = 0;
                                            int MtrlRemainingQty = 0;
                                            bool result3 = false;
                                            bool result4 = false;

                                            result3 = int.TryParse(row2.Cells["BALANCE"].Value.ToString(), out MtrlBalance);
                                            result4 = int.TryParse(row2.Cells["RESERVEDQTY"].Value.ToString(), out MtrlQtyReserved);

                                            MtrlRemainingQty = MtrlBalance - MtrlQtyReserved;

                                            if (result3 && result4 && MtrlRemainingQty > 0)
                                            {

                                                int selectedQty = 0;
                                                if (MtrlRemainingQty <= RequiredQty) selectedQty = MtrlRemainingQty  ;
                                                else selectedQty = RequiredQty;
 
                                                qtyReserved += selectedQty;
                                                row.Cells[15].Value = qtyReserved.ToString();

                                                MtrlQtyReserved += selectedQty;
                                                row2.Cells["RESERVEDQTY"].Value = MtrlQtyReserved.ToString();

                                                RequiredQty = qtyNCov - qtyReserved;
                                            }


                                            //row.Cells[13].Value = "1";
                                        }
                            

                            }

                        }
                }

               // for (int i = 0; i < rw.Cells.Count; i++)
              //  {
               //     if ((rw.Cells[i] is DataGridViewCheckBoxCell) && ((bool)rw.Cells[i].Value))
               //         chkCount += 1;
               // }
            }

            CalcSumQtys();
            CalcSumQtys2();


        }


        Dictionary<string, int> MTRL_RESERVATIONS =
           new Dictionary<string, int>();


        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            MTRL_RESERVATIONS.Clear();


            foreach (DataGridViewRow row in this.ItemDataGridView.Rows)
            {
                // row.Cells[5].Value = "1"; // ID ΕΙΔΟΥΣ
                // row.Cells[7].Value = "1"; // ID ΧΡΩΜΑΤΟΣ
                // row.Cells[9].Value = "1"; // ID ΜΕΓΕΘΟΥΣ
                // row.Cells[11].Value = "1"; // Ποσότητα
                // row.Cells[12].Value = "1"; // Ανεκτέλεστη Ποσότητα
                // row.Cells[13].Value = "1"; // Υπόλοιπο σε ΑΧ 200
                // row.Cells[14].Value = "1"; // Νέα Ποσότητα

                String key =
                    row.Cells[5].Value + "~" +
                    row.Cells[7].Value + "~" +
                    row.Cells[9].Value + "~";


                int qtyNCov = 0;
                int qtyReserved = 0;
                int RemQty = 0;
                bool result = false;
                bool result2 = false;
                bool result3 = false;


                String qtyNCovStr = row.Cells[12].Value.ToString();
                String RemQtyStr = row.Cells[13].Value.ToString();
                String qtyReservedStr = row.Cells[14].Value.ToString();

                result = int.TryParse(qtyNCovStr, out qtyNCov);
                result2 = int.TryParse(RemQtyStr, out RemQty);
                result3 = int.TryParse(qtyReservedStr, out qtyReserved);

                int RequiredQty = qtyNCov;// -qtyReserved;
                int ReservedQty = 0;

                if (MTRL_RESERVATIONS.ContainsKey(key))
                {
                    ReservedQty = MTRL_RESERVATIONS[key];
                    RemQty -= ReservedQty;
                }

                int selectedQty = 0;
                if (result && result2 &&  result3 && RequiredQty > 0 && RemQty > 0)
                    {
                       
                        if (RemQty <= RequiredQty) selectedQty = RemQty;
                        else selectedQty = RequiredQty;

                        row.Cells[14].Value = selectedQty.ToString();

                    }

                if (MTRL_RESERVATIONS.ContainsKey(key))
                    {
                        MTRL_RESERVATIONS[key] += selectedQty ;
                    }
                else
                    {
                    MTRL_RESERVATIONS.Add(key,selectedQty);
                    }

            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Επανυπολογισμός στατιστικών παραστατικών?", "Επιλογή", MessageBoxButtons.YesNoCancel);
            if (result != DialogResult.Yes)
            {
                return;
            }

            var StartDate = DateTime.Now;


            MTRLINESCSList.Clear();

            String SeriesStr = ((KeyValuePair<int, String>)this.SeriesComboBox.SelectedItem).Key.ToString();

            String from = dateTimePicker1.Value.Date.ToString("yyyyMMdd");
            String to = dateTimePicker2.Value.Date.ToString("yyyyMMdd");



            String Sql =
               " SELECT ML.* FROM FINDOC F                          " +
               " INNER JOIN MTRLINES ML ON ML.FINDOC = F.FINDOC     " +
               " WHERE 1 = 1                                        " +
               " AND F.SERIES IN (" + SeriesStr + ")                " + //9001
               " AND F.SODTYPE = 13                                 " +
               " AND F.COMPANY = " + XSupport.ConnectionInfo.CompanyId.ToString() +
               " AND F.TRNDATE >= '" + from + "'                                 " +
               " AND F.TRNDATE <= '" + to + "'                                   ";

            XTable results = XSupport.GetSQLDataSet(Sql);


            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {

                    string[] lines = Regex.Split(results[i, "SOANAL"].ToString(), ";");

                    if (lines.Count() == 4)
                    {
                        string[] ColorSizes = Regex.Split(lines[3], "~");

                        foreach (string colorSize in ColorSizes)
                        {
                            if (colorSize.ToString() != "")
                            {
                                string[] ColorSizeData = colorSize.Split('|');
                                MTRLINESCS MTRLINEScs = new MTRLINESCS();
                                // MessageBox.Show(ColorSizeData[0].ToString() + " " + ColorSizeData[1].ToString() + " " + ColorSizeData[2].ToString() + " " + ColorSizeData[3].ToString() + " " + ColorSizeData[4].ToString() + " " + ColorSizeData[6].ToString() + " ");
                                //   MTRLINEScs.LINENUM = results[i, "LINENUM"].ToString();
                                MTRLINEScs.MTRLINES = results[i, "MTRLINES"].ToString();
                                MTRLINEScs.FINDOC = results[i, "FINDOC"].ToString();
                                MTRLINEScs.MTRL = results[i, "MTRL"].ToString();

                                MTRLINEScs.CDIMLINES1 = ColorSizeData[0].ToString();
                                MTRLINEScs.CDIMLINES2 = ColorSizeData[1].ToString();
                                MTRLINEScs.CDIMLINES3 = ColorSizeData[2].ToString();
                                MTRLINEScs.QTY1 = ColorSizeData[6].ToString();
                                MTRLINEScs.QTY2 = ColorSizeData[5].ToString();
                                MTRLINEScs.QTY3 = ColorSizeData[8].ToString();

                                MTRLINESCSList.Add(MTRLINEScs);
                            }
                        }

                    }


                }

                String Sql3 = "TRUNCATE TABLE ColorSizeAnalysisSQ ";
                XSupport.ExecuteSQL(Sql3, null);

                String Sql2 =
                         " INSERT INTO ColorSizeAnalysisSQ  (MTRLINES,FINDOC,MTRL,CDIMLINES1,CDIMLINES2,CDIMLINES3,QTY1,QTY1COV,QTY1CAN) VALUES ";
                       /*  " VALUES ( " +
                        " '" + MTRLINEScs.MTRLINES + "', " +
                        " '" + MTRLINEScs.FINDOC + "', " +
                        " '" + MTRLINEScs.MTRL + "', " +
                        " '" + MTRLINEScs.CDIMLINES1 + "', " +
                        " '" + MTRLINEScs.CDIMLINES2 + "', " +
                        " '" + MTRLINEScs.CDIMLINES3 + "', " +
                        " '" + QTY1 + "', " +
                        " '" + QTY1COV + "', " +
                        " '" + QTY1CAN + "') "; */


                Boolean firstTime = true;
                    
                foreach (MTRLINESCS MTRLINEScs in MTRLINESCSList)
                {
                    String QTY1 = MTRLINEScs.QTY1;
                    String QTY1COV = MTRLINEScs.QTY2;
                    String QTY1CAN = MTRLINEScs.QTY3;

                    if (QTY1 == "") QTY1 = "0";
                    if (QTY1COV == "") QTY1COV = "0";
                    if (QTY1CAN == "") QTY1CAN = "0";

                    
                    if (!firstTime) Sql2 += " ,";
                    Sql2 += "  ( " +
                   " '" + MTRLINEScs.MTRLINES + "', " +
                   " '" + MTRLINEScs.FINDOC + "', " +
                   " '" + MTRLINEScs.MTRL + "', " +
                   " '" + MTRLINEScs.CDIMLINES1 + "', " +
                   " '" + MTRLINEScs.CDIMLINES2 + "', " +
                   " '" + MTRLINEScs.CDIMLINES3 + "', " +
                   " '" + QTY1 + "', " +
                   " '" + QTY1COV + "', " +
                   " '" + QTY1CAN + "') ";

                    /*
                    String Sql2 =
                         " INSERT INTO ColorSizeAnalysisSQ  (MTRLINES,FINDOC,MTRL,CDIMLINES1,CDIMLINES2,CDIMLINES3,QTY1,QTY1COV,QTY1CAN) " +
                         " VALUES ( " +
                        " '" + MTRLINEScs.MTRLINES + "', " +
                        " '" + MTRLINEScs.FINDOC + "', " +
                        " '" + MTRLINEScs.MTRL + "', " +
                        " '" + MTRLINEScs.CDIMLINES1 + "', " +
                        " '" + MTRLINEScs.CDIMLINES2 + "', " +
                        " '" + MTRLINEScs.CDIMLINES3 + "', " +
                        " '" + QTY1 + "', " +
                        " '" + QTY1COV + "', " +
                        " '" + QTY1CAN + "') "; */


                    XSupport.ExecuteSQL(Sql2, null);
                    firstTime = false;
                }

                if (firstTime == false) XSupport.ExecuteSQL(Sql2, null);

            }



            var EndDate = DateTime.Now;
            var diff = EndDate.Subtract(StartDate);
            String executionTime = String.Format("{0}:{1}:{2}", diff.Hours, diff.Minutes, diff.Seconds);
            MessageBox.Show(executionTime);

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
           /* if (e.KeyCode == Keys.Enter)
            {
                DataView dv;
                DataTable dt = (DataTable)this.ItemDataGridView.DataSource;
                dv = new DataView(dt, "type = 'business' ", "type Desc", DataViewRowState.CurrentRows);
                ItemDataGridView.DataSource = dv;
            } */
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
          
            XSupport.ExecuteSQL("UPDATE ColorSizeAnalysisRep SET qty1res = 0", null);
            int counter = 0;
            String Sql = "";
            foreach (DataGridViewRow row in this.ItemDataGridView.Rows)
            {
                String qty1res = row.Cells[15].Value.ToString();
                String Id = row.Cells[0].Value.ToString();

                if (qty1res != "0")
                {
                    Sql += " UPDATE ColorSizeAnalysisRep SET qty1res = " + qty1res + " WHERE ID = " + Id + "; " ;
                    XSupport.ExecuteSQL(Sql, null);
                    counter++;
                }


                if (counter == 100)
                {
                    XSupport.ExecuteSQL(Sql, null);
                    counter = 0;
                    Sql = "";
                }
              
            }

            if (Sql != "")
            {
                XSupport.ExecuteSQL(Sql, null);
            }

        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
           XSupport.ExecuteSQL("UPDATE ColorSizeAnalysisRep SET qty1res = 0", null);
            int counter = 0;
            String Sql = "";
            foreach (DataGridViewRow row in this.ItemDataGridView.Rows)
            {
                String qty1res = row.Cells[15].Value.ToString();
                String Id = row.Cells[0].Value.ToString();

                if (qty1res != "0")
                {
                    Sql += " UPDATE ColorSizeAnalysisRep SET qty1res = " + qty1res + " WHERE ID = " + Id + "; ";
                    XSupport.ExecuteSQL(Sql, null);
                    counter++;
                }


                if (counter == 100)
                {
                    XSupport.ExecuteSQL(Sql, null);
                    counter = 0;
                    Sql = "";
                }
            }

            if (Sql != "")
            {
                XSupport.ExecuteSQL(Sql, null);
            }

        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            String SeriesStr = ((KeyValuePair<int, String>)this.SeriesComboBox.SelectedItem).Key.ToString();
            String from = dateTimePicker1.Value.Date.ToString("yyyyMMdd");
            String to = dateTimePicker2.Value.Date.ToString("yyyyMMdd");

            String Sql =

            "  declare @vFINDOC int                                                                                                                                                              " +
            "  declare crf1Rep cursor FOR                                                                                                                                                           " +
            "  SELECT F.FINDOC                                                                                                                                                                   " +
            "  FROM FINDOC F                                                                                                                                                                     " +
            "  WHERE F.FULLYTRANSF IN (0,2)                                                                                                                                                      " +
            "  AND F.TFPRMS=201                                                                                                                                                                  " +
            "  AND F.COMPANY=1                                                                                                                                                                   " +
            "  AND F.TRNDATE>='" + from + "'                                                                                                                                                     " +
            "  AND F.TRNDATE<='" + to + "'                                                                                                                                                       " +
            "  AND F.SERIES IN (" + SeriesStr + ")                                                                                                                                               " +
        //    "  --AND F.TRDR = CASE WHEN '{?RTRDR}' = '' OR '{?RTRDR}' = '0' THEN F.TRDR ELSE {?RTRDR} END                                                                                        " +
         //   "  --AND F.FINDOC IN (SELECT ML.FINDOC FROM MTRLINES ML WHERE ML.FINDOC = F.FINDOC AND ML.MTRL = CASE WHEN '{?RMTRL}' = '' OR '{?RMTRL}' = '0' THEN ML.MTRL ELSE {?RMTRL} END )      " +
            "  begin                                                                                                                                                                             " +
            "  open crf1Rep                                                                                                                                                                         " +
            "  set @vFINDOC=0                                                                                                                                                                    " +
            "  fetch next from crf1Rep into @vFINDOC                                                                                                                                                " +
            "  while (@@fetch_status=0)                                                                                                                                                          " +
            "  begin                                                                                                                                                                             " +
            "  EXEC sp_ColorSizeAnalysisByFindocRep @vFINDOC                                                                                                                                       " +
            "  fetch next from crf1Rep into @vFINDOC                                                                                                                                                " +
            "  end                                                                                                                                                                               " +
            "  end                                                                                                                                                                               " +
            "  close crf1Rep                                                                                                                                                                        " +
            "  deallocate crf1Rep                                                                                                                                                                   ";

            XSupport.ExecuteSQL(Sql, null);
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            //this.splitContainer1.SplitterIncrement = 10;  
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance =
                (int)(splitContainer1.ClientSize.Width * 0.30);
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_KeyDown_1(object sender, KeyEventArgs e)
        {

        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
             XSupport.ExecuteSQL("UPDATE ColorSizeAnalysisRep SET qty1res = 0", null);
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance =
                (int)(splitContainer1.ClientSize.Width * 1.0);
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            String SeriesStr = ((KeyValuePair<int, String>)this.SeriesComboBox.SelectedItem).Key.ToString();

            String from = dateTimePicker1.Value.Date.ToString("d/M/yyyy");
            String to = dateTimePicker2.Value.Date.ToString("d/M/yyyy");

            this.XSupport.ExecS1Command("SALDOC[LIST=Στατιστικά Εκτελεσιμότητας Με Αναθέσεις Β,AUTOEXEC=1,FORCEFILTERS=FINDOCTRNDATEL:" + from + "?FINDOCTRNDATEH:" + to + "?FINDOCSERIESL:" + SeriesStr + "]", this);
        }
    }
}
