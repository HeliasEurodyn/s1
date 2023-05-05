using Softone;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PiraeusBankImport
{
    public partial class PiraeusBankImportForm : Form
    {
        public XSupport XSupport= null;

        public PiraeusBankImportForm()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "(*.txt)|*.txt";
            // openFileDialog1.Filter = "(*.txt,*.asc)|*.txt;*.asc";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    ReadFile(openFileDialog1.FileName);
                }
                catch (Exception ex) { }
            }
            else
            {
                return;
            }
        }

        public void ReadFile(String path)
        {

            int lineCounter = 0;
            String line = "";

            using (StreamReader streamFile = new StreamReader(path, Encoding.Default))
            {
                
                while ((line = streamFile.ReadLine()) != null)
                {
                    lineCounter++;
                }
                streamFile.Close();

            }

            this.ItemDataGridView.Rows.Clear();
           
            using (StreamReader streamFile = new StreamReader(path, Encoding.Default))
            {
                float totalvalFloat = 0;
                int i = 0;
                while ((line = streamFile.ReadLine()) != null)
                {
                    if ((i > 0) && (i!= (lineCounter-1)) )
                    {
                        List<String> rowList = new List<string>();
                        String[] words = line.Split(',');

                        String valStr = line.Substring(76, 13);
                        float valFloat = 0;
                        float.TryParse(valStr, out valFloat);
                        totalvalFloat += valFloat;

                        rowList.Add(line.Substring(0, 1));  // Κωδικός Εγγραφής
                        rowList.Add(line.Substring(1, 5));  // Κωδικός Μεγάλου Πελάτη
                        rowList.Add(line.Substring(6, 4));  // Κωδικός Καταστήματος
                        rowList.Add(line.Substring(10, 13)); // Αριθμός Λογαριασμού
                        rowList.Add(line.Substring(23, 30).Replace(" ","")); // Αριθμός Συσχέτισης
                        rowList.Add(line.Substring(53, 20)); // Filler1
                        rowList.Add(line.Substring(73, 3));  // Νόμισμα
                        rowList.Add(line.Substring(76, 13)); // Ποσό
                        rowList.Add(line.Substring(89, 1));  // Κωδικός Χρεώσεως/Πιστώσης
                        //   rowList.Add(line.Substring(90, 20)); // Επώνυμο Δικαιούχου
                        //   rowList.Add(line.Substring(110, 6));  // Όνομα Διακιούχου
                        //   rowList.Add(line.Substring(116, 4));  // Πατρώνυμο Δικαιούχου

                        rowList.Add(line.Substring(90, 30)); // Επώνυμο Δικαιούχου
                        rowList.Add("");  // Όνομα Διακιούχου
                        rowList.Add("");  // Πατρώνυμο Δικαιούχου


                        rowList.Add(line.Substring(120, 9));  // ΑΦΜ
                        rowList.Add(line.Substring(129, 8));  // Ημ.νία Πίστωσης Δικαιούχου
                        rowList.Add(line.Substring(137, 1));  // Επιστροφή
                        rowList.Add(line.Substring(138, 27)); // IBAN
                       // rowList.Add(line.Substring(165, 5));  // Filler2
                        rowList.Add("");  // Filler2
                        rowList.Add("");  // Filler2

                        string[] row = rowList.ToArray();
                        this.ItemDataGridView.Rows.Add(row);
                    }
                    this.TotalAmountTextBox.Text = (totalvalFloat/100).ToString();
                    this.TotalRowsTextBox.Text = (i - 1).ToString();

                    i++;
                }
                streamFile.Close();

              

            }


         
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Καταχώρηση κινήσεων.", "Συνέχεια;", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            { 
            IterateItemDataGridViewAndInsertFindocs();
            MessageBox.Show("Η διαδικασία ολοκληρώθηκε.");
            }
        }

        public void IterateItemDataGridViewAndInsertFindocs()
        {
            //101 Ergoplanning
            String MoneyAccount = "";
            if(XSupport.ConnectionInfo.CompanyId == 101)
                MoneyAccount = Properties.ErgoSettings.Default["ErgoplanningAcc"].ToString();
            else
                MoneyAccount = Properties.ErgoSettings.Default["ErgopartnersAcc"].ToString();


            foreach (DataGridViewRow row in this.ItemDataGridView.Rows)
            {
                String Sql =
                         " SELECT                                                " +
                         " F.FINDOC,                                             " +
                         " F.TRDR,                                               " +
                         " TF.CHEQUE,                                            " +
                         " ISNULL(TF.PRJC,0) AS PRJC,                            " +
                         " (SELECT CODE FROM CHEQUE WHERE CHEQUE =TF.CHEQUE AND COMPANY = F.COMPANY ) AS CHEQUECODE, " +
                         " (SELECT TRDR FROM TRDR WHERE COMPANY = " + XSupport.ConnectionInfo.CompanyId.ToString() + " AND CODE = '"+ MoneyAccount + "') AS ACCOUNTTRDR" +

                         " FROM FINDOC F                                         " +
                         " INNER JOIN TRDFLINES TF ON F.FINDOC = TF.FINDOC       " +
                         " WHERE 1 = 1                                           " +
                         " AND F.COMPANY =                                    " + XSupport.ConnectionInfo.CompanyId.ToString() +
                         " AND F.SOSOURCE = 1381                                 " +
                         " AND F.SODTYPE = 13                                    " +
                         " AND F.SERIES = 4122                                   " +
                         " AND (SELECT CHEQUEBAL FROM CHEQUE WHERE CHEQUE =TF.CHEQUE AND COMPANY = F.COMPANY ) > 0  " +
                         " AND F.FINCODE = '" + row.Cells["K5"].Value.ToString() + "'  ";
             

                XTable results = XSupport.GetSQLDataSet(Sql);


                if (results.Count == 1)
                {
                    for (int i = 0; i < results.Count; i++)
                    {
                        int SourceFindoc = int.Parse(results[i, "FINDOC"].ToString());
                        int SourceTrdr = int.Parse(results[i, "TRDR"].ToString());
                        int SourceCHEQUE = int.Parse(results[i, "CHEQUE"].ToString());
                        double Lineval = Convert.ToDouble(row.Cells["K8"].Value.ToString());
                        String CHEQUECODE = results[i, "CHEQUECODE"].ToString();
                        int SourceACCOUNTTRDR = int.Parse(results[i, "ACCOUNTTRDR"].ToString());

                        int SourcePRJC = int.Parse(results[i, "PRJC"].ToString());
                        Lineval = Lineval / 100;


                        String TrnDateStr = row.Cells["K14"].Value.ToString();
                        //DateTime TrnDate = Convert.ToDateTime(TrnDateStr);
                        DateTime TrnDate = DateTime.Now;
                         TrnDate =  DateTime.ParseExact(TrnDateStr,
                                  "ddMMyyyy",
                                   CultureInfo.InvariantCulture);


                        InsCFNCUSDOCToS1(SourceFindoc, SourceTrdr, SourceCHEQUE, Lineval, CHEQUECODE, SourcePRJC, TrnDate);
                        InsBFNCUSDOCToS1(SourceFindoc, SourceTrdr, SourceCHEQUE, Lineval, CHEQUECODE, SourceACCOUNTTRDR, SourcePRJC, TrnDate);

                        row.DefaultCellStyle.BackColor = Color.LightBlue;
                        row.Cells["K17"].Value = "Καραχωρήθηκε"; 
                        row.Cells["K18"].Value = DateTime.Now.ToString("dd/MM/yy");
                    }
                 
                }
           
            }

        }


      //  public void InsertFindoc()
      //  {
      //  }

        public void InsCFNCUSDOCToS1(int SourceFindoc, int SourceTrdr, int SourceCHEQUE, double Lineval, String CHEQUECODE, int SourcePRJC, DateTime TrnDate)
        {
            
            XModule ModuleCFNCUSDOC = XSupport.CreateModule("CFNCUSDOC");
            XTable CFNCUSDOC = ModuleCFNCUSDOC.GetTable("CFNCUSDOC");
            XTable TRDFLINES = ModuleCFNCUSDOC.GetTable("CHEQUELINES");
            
           // int linenum = 1;


            try
                {

                ModuleCFNCUSDOC.InsertData();

                CFNCUSDOC.Current["SERIES"] = (int)4123;
                CFNCUSDOC.Current["TRNDATE"] = TrnDate; // DateTime.Now;
                CFNCUSDOC.Current["TRDR"] = (int)SourceTrdr;
                CFNCUSDOC.Current["CCCNUM01"] = (double)SourceFindoc;
                CFNCUSDOC.Current.Post();


                TRDFLINES.Current.Insert();

                //  TRDFLINES.Current["LINENUM"] = linenum;
                //  TRDFLINES.Current["SODTYPE"] = 51;


                TRDFLINES.Current["TPRMS"] = (int) 3316;
                //     TRDFLINES.Current["CHEQUE"] = (int) SourceCHEQUE;
                TRDFLINES.Current["CODE"] = CHEQUECODE;


                TRDFLINES.Current["LINEVAL"] = (double) Lineval;
                TRDFLINES.Current["SODTYPE"] = 51;

               if(SourcePRJC > 0) TRDFLINES.Current["PRJC"] = (int)SourcePRJC;
                
                TRDFLINES.Current.Post();

                ModuleCFNCUSDOC.PostData();
                }
                catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                //throw ex;
                }

        }

        public void InsBFNCUSDOCToS1(int SourceFindoc, int SourceTrdr, int SourceCHEQUE, double Lineval, String CHEQUECODE, int SourceACCOUNTTRDR, int SourcePRJC, DateTime TrnDate)
        {
            XModule ModuleBFNCUSDOC = XSupport.CreateModule("BFNCUSDOC");
            XTable BFNCUSDOC = ModuleBFNCUSDOC.GetTable("BFNCUSDOC");
            XTable TRDTLINES = ModuleBFNCUSDOC.GetTable("TRDTLINES");
            
            try
            {

 
                ModuleBFNCUSDOC.InsertData();

                BFNCUSDOC.Current["SERIES"] = (int)1002;
                BFNCUSDOC.Current["TRNDATE"] = TrnDate; // DateTime.Now;
                BFNCUSDOC.Current["TRDR"] = (int) SourceACCOUNTTRDR;// "38.00.00.0000";
                BFNCUSDOC.Current["CCCNUM01"] = (double)SourceFindoc;
                BFNCUSDOC.Current.Post();

                TRDTLINES.Current.Insert();

                TRDTLINES.Current["TRDR"] = (int)SourceTrdr;
                TRDTLINES.Current["LINEVAL"] = (double)Lineval;
                if (SourcePRJC > 0) TRDTLINES.Current["PRJC"] = (int)SourcePRJC;

                TRDTLINES.Current.Post();

                ModuleBFNCUSDOC.PostData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw ex;
            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                String Sql =

                   " SELECT F.FINDOC              " +
                   " FROM FINDOC F                " +
                   " WHERE F.COMPANY = " + XSupport.ConnectionInfo.CompanyId.ToString() +
                   "     AND F.SOSOURCE = 1381    " +
                   "     AND F.SODTYPE = 13       " +
                   "     AND F.FINCODE = '" + this.ItemDataGridView.Rows[ItemDataGridView.CurrentCell.RowIndex].Cells[4].Value.ToString() + "'       ";


                XTable results = XSupport.GetSQLDataSet(Sql);

                if (results.Count == 1)
                {
                    String FINDOC = results[0, "FINDOC"].ToString();
                    this.XSupport.ExecS1Command("BFNCUSDOC[LIST=Εμβάσματα απο Εντολές Χρέωσης,AUTOEXEC=1,FORCEFILTERS=FINDOCCCCNUM01L:" + FINDOC + "?FINDOCCCCNUM01H:" + FINDOC + "]", this);
                }
                else if (results.Count == 0)
                {
                    MessageBox.Show("Δεν υπάρχει παραστατικό.");
                }

            }
            catch (Exception ex) { }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {

                String Sql =
                  " SELECT F.FINDOC              " +
                  " FROM FINDOC F                " +
                  " WHERE F.COMPANY = " + XSupport.ConnectionInfo.CompanyId.ToString() +
                  "     AND F.SOSOURCE = 1381    " +
                  "     AND F.SODTYPE = 13       " +
                  "     AND F.FINCODE = '" + this.ItemDataGridView.Rows[ItemDataGridView.CurrentCell.RowIndex].Cells[4].Value.ToString() + "'       ";

                XTable results = XSupport.GetSQLDataSet(Sql);

                if (results.Count == 1)
                {
                    this.XSupport.ExecS1Command("CFNCUSDOC[LIST=Εντολές Χρέωσης,AUTOEXEC=1,FORCEFILTERS=FINDOCFINCODEL:" + this.ItemDataGridView.Rows[ItemDataGridView.CurrentCell.RowIndex].Cells[4].Value.ToString() + "*]", this);
                }
                else if (results.Count == 0)
                {
                    MessageBox.Show("Δεν υπάρχει παραστατικό.");
                }
            }
            catch (Exception ex) { }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

            try
            {
                String Sql =

                   " SELECT F.FINDOC              " +
                   " FROM FINDOC F                " +
                   " WHERE F.COMPANY = " + XSupport.ConnectionInfo.CompanyId.ToString() +
                   "     AND F.SOSOURCE = 1381    " +
                   "     AND F.SODTYPE = 13       " +
                   "     AND F.FINCODE = '" + this.ItemDataGridView.Rows[ItemDataGridView.CurrentCell.RowIndex].Cells[4].Value.ToString() + "'       ";


                XTable results = XSupport.GetSQLDataSet(Sql);

                if (results.Count == 1)
                {
                        String FINDOC = results[0, "FINDOC"].ToString();
                        this.XSupport.ExecS1Command("CFNCUSDOC[LIST=Εξόφληση Εντολής Χρέωσης,AUTOEXEC=1,FORCEFILTERS=FINDOCCCCNUM01L:" + FINDOC + "?FINDOCCCCNUM01H:" + FINDOC + "]", this);
                }
                else if (results.Count == 0)
                {
                    MessageBox.Show("Δεν υπάρχει παραστατικό.");
                }

            }
            catch (Exception ex) { }
        }
    }
}
