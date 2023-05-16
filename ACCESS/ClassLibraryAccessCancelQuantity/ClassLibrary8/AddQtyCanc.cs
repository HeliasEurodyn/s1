using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Softone;

namespace ClassLibrary8
{

    public partial class AddQtyCanc : Form
    {

        XSupport xs_;
        XModule xm_;

        public Dictionary<String, String> MTRLS =
            new Dictionary<String, String>();

        string[] findocs_;
        string findocs_serialized;
        int findocs_number_ = 0;
        string[] files;

        public AddQtyCanc(XSupport xs, XModule xm)
        {
            InitializeComponent();
            xs_ = xs;
            xm_ = xm;
            readFilesFromFolder();
        }

        void readFilesFromFolder()
        {
            string folderPath = @"C:\Soft1\RPA"; // replace with the folder path you want to read

            files = Directory.GetFiles(folderPath);

            Dictionary<int, String> dictionary = new Dictionary<int, String>();  //cz


            int index = 0;
            foreach (string file in files)
            {
                dictionary.Add(index, Path.GetFileName(file));
                // Console.WriteLine(Path.GetFileName(file));
                index++;
            }
            this.comboBox3.DataSource = new BindingSource(dictionary, null);
            this.comboBox3.DisplayMember = "Value";
            this.comboBox3.ValueMember = "Key";


        }

        public void loadAutocompleteMtrls()
        {
            //Dictionary<int, String> DICT = new Dictionary<int, String>();
            //String sql =
            //   " SELECT MTRL,CODE,NAME FROM MTRL WHERE SODTYPE = 51 AND COMPANY = " + xs_.ConnectionInfo.CompanyId.ToString();

            //var autoComplete = new AutoCompleteStringCollection();
            //XTable DATASql = xs_.GetSQLDataSet(sql);

            //for (int i = 0; i < DATASql.Count; i++)
            //{
            //    autoComplete.Add(DATASql[i, "CODE"].ToString() + " " + DATASql[i, "NAME"].ToString());
            //    MTRLS.Add((DATASql[i, "CODE"].ToString() + " " + DATASql[i, "NAME"].ToString()), DATASql[i, "MTRL"].ToString());
            //}

            //this.textBox1.AutoCompleteCustomSource = autoComplete;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    DialogResult dialogResult = MessageBox.Show("ΣΥΝΕΧΕΙΑ;", "ΣΥΝΕΧΕΙΑ;", MessageBoxButtons.YesNo);
        //    if (dialogResult != DialogResult.Yes) return;

        //    String colorCDIMLINESStr = "";
        //    String sizeCDIMLINESStr = "";

        //    int colorCDIMLINES = 0;
        //    int sizeCDIMLINES = 0;

        //    String MTRLStr = "";
        //    int MTRL = 0;
        //    Boolean rESULT = false;

        //    try
        //    {
        //        if (this.comboBox1.SelectedItem != null)
        //        {
        //            colorCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox1.SelectedItem).Key.ToString();
        //            rESULT = int.TryParse(colorCDIMLINESStr, out colorCDIMLINES);

        //        }
        //    }
        //    catch (Exception ex) { }

        //    try
        //    {
        //        if (this.comboBox2.SelectedItem != null)
        //        {
        //            sizeCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox2.SelectedItem).Key.ToString();
        //            rESULT = int.TryParse(sizeCDIMLINESStr, out sizeCDIMLINES);
        //        }
        //    }
        //    catch (Exception ex) { }


        //    if (MTRLS.ContainsKey(this.textBox1.Text))
        //    {
        //        MTRLStr = MTRLS[this.textBox1.Text];
        //        rESULT = int.TryParse(MTRLStr, out MTRL);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Δεν έχετε επιλέξει είδος");
        //        return;
        //    }



        //    if (colorCDIMLINESStr != "" && sizeCDIMLINESStr != "")
        //    {
        //        foreach (string findocStr in findocs_)
        //        {
        //            int findoc = 0;
        //            int.TryParse(findocStr, out findoc);
        //            try
        //            {
        //                CancelQtysColorSize(findoc, MTRL, colorCDIMLINES, sizeCDIMLINES);
        //            }
        //            catch (Exception ex) { }
        //        }

        //    }
        //    else if (sizeCDIMLINESStr != "")
        //    {
        //        foreach (string findocStr in findocs_)
        //        {
        //            int findoc = 0;
        //            int.TryParse(findocStr, out findoc);

        //            try
        //            {
        //                CancelQtysColor(findoc, MTRL, colorCDIMLINES);
        //            } catch (Exception ex) { }
        //        }
        //    }
        //    else if (colorCDIMLINESStr == "" && sizeCDIMLINESStr == "")
        //    {
        //        foreach (string findocStr in findocs_)
        //        {
        //            int findoc = 0;
        //            int.TryParse(findocStr, out findoc);

        //            try
        //            {
        //                CancelQtys(findoc, MTRL);
        //            } catch (Exception ex) { }
        //        }
        //    }


        //    MessageBox.Show("Η διαδικασία ολοκληρώθηκε.");

        //}

        public static void HideWarningsFromS1Mdl(XModule XModule, XSupport XSupport)
        {
            object otherModule = XSupport.GetStockObj("ModuleIntf", true);

            object[] myArray1;
            myArray1 = new object[3];
            myArray1[0] = XModule.Handle;
            myArray1[1] = "WARNINGS";    //Param Name
            myArray1[2] = "OFF";         //Param Value
            XSupport.CallPublished(otherModule, "SetParamValue", myArray1);

            object[] myArray2;
            myArray2 = new object[3];
            myArray2[0] = XModule.Handle;
            myArray2[1] = "NOMESSAGES";    //Param Name
            myArray2[2] = "1";         //Param Value
            XSupport.CallPublished(otherModule, "SetParamValue", myArray2);
        }

        private void CancelQtysColorSize(int findoc, int mtrl, int colorCDIMLINES, int sizeCDIMLINES)
        {
            XModule SALDOCModule = xs_.CreateModule("SALDOC");
            HideWarningsFromS1Mdl(SALDOCModule, xs_);
            //XTable SALDOCTbl = SALDOCModule.GetTable("SALDOC");

            SALDOCModule.LocateData(findoc);
            XTable ITELINESTbl = SALDOCModule.GetTable("ITELINES");
            XTable VQTYANALTbl = SALDOCModule.GetTable("VQTYANAL");

            Boolean linesUpdated = false;

            for (int i = 0; i < ITELINESTbl.Count; i++)
            {
                ITELINESTbl.Current.Edit(i);

                for (int j = 0; j < VQTYANALTbl.Count; j++)
                {
                    VQTYANALTbl.Current.Edit(j);

                    if (ITELINESTbl.Current["MTRLINES"].ToString() == VQTYANALTbl.Current["MTRLINES"].ToString()
                        && ITELINESTbl.Current["MTRL"].ToString() == mtrl.ToString()
                        && VQTYANALTbl.Current["CDIMLINES1"].ToString() == colorCDIMLINES.ToString()
                        && VQTYANALTbl.Current["CDIMLINES2"].ToString() == sizeCDIMLINES.ToString()
                        )
                    {
                        String qty1Str = "";
                        double qty1 = 0;

                        String qty1cancStr = "";
                        double qty1canc = 0;

                        String qty1covStr = "";
                        double qty1cov = 0;

                        double qty1NCov = 0;

                        Boolean ParseResult = false;

                        qty1Str = VQTYANALTbl.Current["QTY1"].ToString();
                        qty1cancStr = VQTYANALTbl.Current["QTY1CANC"].ToString();
                        qty1covStr = VQTYANALTbl.Current["QTY1COV"].ToString();

                        ParseResult = double.TryParse(qty1Str, out qty1);
                        ParseResult = double.TryParse(qty1cancStr, out qty1canc);
                        ParseResult = double.TryParse(qty1covStr, out qty1cov);

                        qty1NCov = qty1 - (qty1canc + qty1cov);

                        if (qty1NCov > 0)
                        {
                            double NewQty1canc = qty1NCov + qty1canc;
                            VQTYANALTbl.Current["QTY1CANC"] = NewQty1canc;
                            VQTYANALTbl.Current.Post();
                            SALDOCModule.EvalFormula("IteLineAnal.SumQAnal(" + ITELINESTbl.Current["MTRLINES"].ToString() + ",'QTY1CANC')");


                            ITELINESTbl.Current.Post();
                            linesUpdated = true;
                        }

                    }
                }
            }

            if (linesUpdated) SALDOCModule.PostData();

        }

        private void CancelQtysColor(int findoc, int mtrl, int colorCDIMLINES)
        {
            XModule SALDOCModule = xs_.CreateModule("SALDOC");
            HideWarningsFromS1Mdl(SALDOCModule, xs_);
            //XTable SALDOCTbl = SALDOCModule.GetTable("SALDOC");

            SALDOCModule.LocateData(findoc);
            XTable ITELINESTbl = SALDOCModule.GetTable("ITELINES");
            XTable VQTYANALTbl = SALDOCModule.GetTable("VQTYANAL");

            Boolean linesUpdated = false;

            for (int i = 0; i < ITELINESTbl.Count; i++)
            {
                ITELINESTbl.Current.Edit(i);

                for (int j = 0; j < VQTYANALTbl.Count; j++)
                {
                    VQTYANALTbl.Current.Edit(j);

                    if (ITELINESTbl.Current["MTRLINES"].ToString() == VQTYANALTbl.Current["MTRLINES"].ToString()
                        && ITELINESTbl.Current["MTRL"].ToString() == mtrl.ToString()
                        && VQTYANALTbl.Current["CDIMLINES1"].ToString() == colorCDIMLINES.ToString()
                        )
                    {
                        String qty1Str = "";
                        double qty1 = 0;

                        String qty1cancStr = "";
                        double qty1canc = 0;

                        String qty1covStr = "";
                        double qty1cov = 0;

                        double qty1NCov = 0;

                        Boolean ParseResult = false;

                        qty1Str = VQTYANALTbl.Current["QTY1"].ToString();
                        qty1cancStr = VQTYANALTbl.Current["QTY1CANC"].ToString();
                        qty1covStr = VQTYANALTbl.Current["QTY1COV"].ToString();

                        ParseResult = double.TryParse(qty1Str, out qty1);
                        ParseResult = double.TryParse(qty1cancStr, out qty1canc);
                        ParseResult = double.TryParse(qty1covStr, out qty1cov);

                        qty1NCov = qty1 - (qty1canc + qty1cov);

                        if (qty1NCov > 0)
                        {
                            double NewQty1canc = qty1NCov + qty1canc;
                            VQTYANALTbl.Current["QTY1CANC"] = NewQty1canc;
                            VQTYANALTbl.Current.Post();
                            SALDOCModule.EvalFormula("IteLineAnal.SumQAnal(" + ITELINESTbl.Current["MTRLINES"].ToString() + ",'QTY1CANC')");

                            ITELINESTbl.Current.Post();
                            linesUpdated = true;
                        }

                    }
                }
            }

            if (linesUpdated) SALDOCModule.PostData();
        }

        private void CancelQtys(int findoc, int mtrl)
        {
            XModule SALDOCModule = xs_.CreateModule("SALDOC");
            HideWarningsFromS1Mdl(SALDOCModule, xs_);

            SALDOCModule.LocateData(findoc);
            XTable ITELINESTbl = SALDOCModule.GetTable("ITELINES");

            Boolean linesUpdated = false;

            for (int i = 0; i < ITELINESTbl.Count; i++)
            {
                ITELINESTbl.Current.Edit(i);
                if (ITELINESTbl.Current["MTRL"].ToString() == mtrl.ToString())
                {
                    String qty1Str = "";
                    float qty1 = 0;

                    String qty1cancStr = "";
                    float qty1canc = 0;

                    String qty1covStr = "";
                    float qty1cov = 0;

                    float qty1NCov = 0;

                    Boolean ParseResult = false;

                    qty1Str = ITELINESTbl.Current["QTY1"].ToString();
                    qty1cancStr = ITELINESTbl.Current["QTY1CANC"].ToString();
                    qty1covStr = ITELINESTbl.Current["QTY1COV"].ToString();

                    ParseResult = float.TryParse(qty1Str, out qty1);
                    ParseResult = float.TryParse(qty1cancStr, out qty1canc);
                    ParseResult = float.TryParse(qty1covStr, out qty1cov);

                    qty1NCov = qty1 - (qty1canc + qty1cov);

                    if (qty1NCov > 0)
                    {
                        float NewQty1canc = qty1NCov + qty1canc;
                        ITELINESTbl.Current["QTY1CANC"] = (double)NewQty1canc;
                        ITELINESTbl.Current.Post();

                        linesUpdated = true;
                    }
                }
            }

            if (linesUpdated) SALDOCModule.PostData();
        }




        private void textBox1_Leave(object sender, EventArgs e)
        {
            //updateColor();
            //updateSize();
        }

        /* 
        private void updateColor()
        {
            String query =
            " SELECT CL.CDIMLINES, CL.CODE  , CL.NAME                                           " +
            "  FROM MTRL M                                                                      " +
            "  INNER JOIN CDIMLINES CL ON CL.CDIM = M.CDIM1 AND CL.COMPANY = M.COMPANY          " +
            "  WHERE M.COMPANY =  " + xs_.ConnectionInfo.CompanyId.ToString()+
            "  AND M.CODE + ' ' +  M.NAME =   '" + this.textBox1.Text + "' ";
                                                                                                
        } 
        */

        //public void updateColor()
        //{
        //    Dictionary<int, String> DICT = new Dictionary<int, String>();

        //    String query =
        //    " SELECT CL.CDIMLINES, CL.CODE  , CL.NAME                                           " +
        //    "  FROM MTRL M                                                                      " +
        //    "  INNER JOIN CDIMLINES CL ON CL.CDIM = M.CDIM1 AND CL.COMPANY = M.COMPANY          " +
        //    "  WHERE M.COMPANY =  " + xs_.ConnectionInfo.CompanyId.ToString() +
        //    "  AND M.CODE + ' ' +  M.NAME =   '" + this.textBox1.Text + "' ";

        //    XTable DATASql = xs_.GetSQLDataSet(query);

        //    if (DATASql.Count > 0)
        //    {
        //        for (int i = 0; i < DATASql.Count; i++)
        //        {
        //            DICT.Add(
        //                int.Parse(DATASql[i, "CDIMLINES"].ToString()),
        //                 DATASql[i, "CODE"].ToString() + " " + DATASql[i, "NAME"].ToString()

        //                 );
        //        }

        //        this.comboBox1.DataSource = new BindingSource(DICT, null);
        //        this.comboBox1.DisplayMember = "Value";
        //        this.comboBox1.ValueMember = "Key";

        //        if (DATASql.Count < 0) this.comboBox1.SelectedIndex = 0;
        //    }
        //}


        //public void updateSize()
        //{
        //    Dictionary<int, String> DICT = new Dictionary<int, String>();

        //    String query =
        //    " SELECT CL.CDIMLINES, CL.CODE, CL.NAME                                             " +
        //    "  FROM MTRL M                                                                      " +
        //    "  INNER JOIN CDIMLINES CL ON CL.CDIM = M.CDIM2 AND CL.COMPANY = M.COMPANY          " +
        //    "  WHERE M.COMPANY =  " + xs_.ConnectionInfo.CompanyId.ToString() +
        //    "  AND M.CODE + ' ' +  M.NAME =   '" + this.textBox1.Text + "' ";

        //    XTable DATASql = xs_.GetSQLDataSet(query);


        //    if (DATASql.Count > 0)
        //    {
        //        for (int i = 0; i < DATASql.Count; i++)
        //        {
        //            DICT.Add(
        //                int.Parse(DATASql[i, "CDIMLINES"].ToString()),
        //                 DATASql[i, "CODE"].ToString() + " " + DATASql[i, "NAME"].ToString()

        //                 );
        //        }

        //        this.comboBox2.DataSource = new BindingSource(DICT, null);
        //        this.comboBox2.DisplayMember = "Value";
        //        this.comboBox2.ValueMember = "Key";

        //        if (DATASql.Count < 0) this.comboBox2.SelectedIndex = 0;
        //    }
        //}



        public void updateFindocs()
        {
            String findocs_serialized = CTools.CToolsNewSDK.CustomToolsNewSDK.GetSelRecValuesFromRightClick(xm_);
            string[] findocs_ = findocs_serialized.Split(',');

            foreach (string findocStr in findocs_)
            {
                int findocInt = 0;
                Boolean result = int.TryParse(findocStr, out findocInt);


                XModule SALDOCModule = xs_.CreateModule("SALDOC");
                HideWarningsFromS1Module(SALDOCModule, xs_);
                XTable SALDOCTbl = SALDOCModule.GetTable("SALDOC");
                XTable ITELINESTbl = SALDOCModule.GetTable("ITELINES");

                SALDOCModule.LocateData(findocInt);


                for (int i = 0; i < ITELINESTbl.Count; i++)
                {

                }


            }
        }

        public static void HideWarningsFromS1Module(XModule XModule, XSupport XSupport)
        {
            object otherModule = XSupport.GetStockObj("ModuleIntf", true);

            object[] myArray1;
            myArray1 = new object[3];
            myArray1[0] = XModule.Handle;
            myArray1[1] = "WARNINGS";    //Param Name
            myArray1[2] = "OFF";         //Param Value
            XSupport.CallPublished(otherModule, "SetParamValue", myArray1);

            object[] myArray2;
            myArray2 = new object[3];
            myArray2[0] = XModule.Handle;
            myArray2[1] = "NOMESSAGES";    //Param Name
            myArray2[2] = "1";         //Param Value
            XSupport.CallPublished(otherModule, "SetParamValue", myArray2);
        }

        private void AddQtyCanc_Load(object sender, EventArgs e)
        {


            findocs_serialized = CTools.CToolsNewSDK.CustomToolsNewSDK.GetSelRecValuesFromRightClick(xm_);
            findocs_ = findocs_serialized.Split(',');// CTools.CToolsNewSDK.CustomToolsNewSDK.GetSelRecValuesFromRightClick(xm_).Split(',');

            foreach (string word in findocs_)
            {
                findocs_number_++;
            }

            loadAutocompleteMtrls();

        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    DialogResult dialogResult = MessageBox.Show("ΣΥΝΕΧΕΙΑ;", "ΣΥΝΕΧΕΙΑ;", MessageBoxButtons.YesNo);
        //    if (dialogResult != DialogResult.Yes) return;

        //    String colorCDIMLINESStr = "";
        //    String sizeCDIMLINESStr = "";

        //    int colorCDIMLINES = 0;
        //    int sizeCDIMLINES = 0;

        //    String MTRLStr = "";
        //    int MTRL = 0;
        //    Boolean rESULT = false;

        //    try
        //    {
        //        if (this.comboBox1.SelectedItem != null)
        //        {
        //            colorCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox1.SelectedItem).Key.ToString();
        //            rESULT = int.TryParse(colorCDIMLINESStr, out colorCDIMLINES);

        //        }
        //    }
        //    catch (Exception ex) { }

        //    try
        //    {
        //        if (this.comboBox2.SelectedItem != null)
        //        {
        //            sizeCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox2.SelectedItem).Key.ToString();
        //            rESULT = int.TryParse(sizeCDIMLINESStr, out sizeCDIMLINES);
        //        }
        //    }
        //    catch (Exception ex) { }


        //    if (MTRLS.ContainsKey(this.textBox1.Text))
        //    {
        //        MTRLStr = MTRLS[this.textBox1.Text];
        //        rESULT = int.TryParse(MTRLStr, out MTRL);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Δεν έχετε επιλέξει είδος");
        //        return;
        //    }


        //    if (sizeCDIMLINESStr != "")
        //    {
        //        foreach (string findocStr in findocs_)
        //        {
        //            int findoc = 0;
        //            int.TryParse(findocStr, out findoc);

        //            try
        //            {
        //                CancelQtysColor(findoc, MTRL, colorCDIMLINES);
        //            }
        //            catch (Exception ex) { }
        //        }
        //    }
        //    else
        //    {
        //        foreach (string findocStr in findocs_)
        //        {
        //            int findoc = 0;
        //            int.TryParse(findocStr, out findoc);

        //            try
        //            {
        //                CancelQtys(findoc, MTRL);
        //            }
        //            catch (Exception ex) { }
        //        }
        //    }


        //    MessageBox.Show("Η διαδικασία ολοκληρώθηκε.");

        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    DialogResult dialogResult = MessageBox.Show("ΣΥΝΕΧΕΙΑ;", "ΣΥΝΕΧΕΙΑ;", MessageBoxButtons.YesNo);
        //    if (dialogResult != DialogResult.Yes) return;

        //    String colorCDIMLINESStr = "";
        //    String sizeCDIMLINESStr = "";

        //    int colorCDIMLINES = 0;
        //    int sizeCDIMLINES = 0;

        //    String MTRLStr = "";
        //    int MTRL = 0;
        //    Boolean rESULT = false;

        //    try
        //    {
        //        if (this.comboBox1.SelectedItem != null)
        //        {
        //            colorCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox1.SelectedItem).Key.ToString();
        //            rESULT = int.TryParse(colorCDIMLINESStr, out colorCDIMLINES);

        //        }
        //    }
        //    catch (Exception ex) { }

        //    try
        //    {
        //        if (this.comboBox2.SelectedItem != null)
        //        {
        //            sizeCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox2.SelectedItem).Key.ToString();
        //            rESULT = int.TryParse(sizeCDIMLINESStr, out sizeCDIMLINES);
        //        }
        //    }
        //    catch (Exception ex) { }


        //    if (MTRLS.ContainsKey(this.textBox1.Text))
        //    {
        //        MTRLStr = MTRLS[this.textBox1.Text];
        //        rESULT = int.TryParse(MTRLStr, out MTRL);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Δεν έχετε επιλέξει είδος");
        //        return;
        //    }



        //    foreach (string findocStr in findocs_)
        //    {
        //        int findoc = 0;
        //        int.TryParse(findocStr, out findoc);

        //        try
        //        {
        //            CancelQtys(findoc, MTRL);
        //        }
        //        catch (Exception ex) { }
        //    }



        //    MessageBox.Show("Η διαδικασία ολοκληρώθηκε.");

        //}
        /*
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        */
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /*
        private void button4_Click(object sender, EventArgs e)
        {
        }
        */
        private void button1_Click_1(object sender, EventArgs e)
        {
            String sql =
                   @"SELECT 
                    A.TRDR,
                    A.VAT
                    FROM TRDR A 
                    WHERE A.COMPANY= "  + xs_.ConnectionInfo.CompanyId.ToString() + 
                    @" AND A.SODTYPE=13
                    AND A.ISACTIVE=1 
                    AND A.VAT = ''
                    ORDER BY A.CODE,A.TRDR";

            XTable DATASql = xs_.GetSQLDataSet(sql);

            for (int i = 0; i < DATASql.Count; i++)
            {
                Console.WriteLine(DATASql[i, "VAT"].ToString());
            }

        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {

            String file = "";
            if (this.comboBox3.SelectedItem != null)
            {
                int index = ((KeyValuePair<int, String>)this.comboBox3.SelectedItem).Key;
                file = this.files[index];
            }
            else {
                return;
            }

            this.dataGridView1.Rows.Clear();

            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            Microsoft.Office.Interop.Excel.Range range;

            int rCnt;

            int rw = 0;
            int cl = 0;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(file, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            rw = range.Rows.Count;
            cl = range.Columns.Count;


            for (rCnt = 2; rCnt <= rw; rCnt++)
            {
                try
                {
                   
     
                    List<String> rowList = new List<string>();

                    String field1 = (xlWorkSheet.Cells[rCnt, 1].Value == null? "" : xlWorkSheet.Cells[rCnt, 1].Value.ToString());
                    String field2 = (xlWorkSheet.Cells[rCnt, 2].Value == null ? "" : xlWorkSheet.Cells[rCnt, 2].Value.ToString());
                    String field3 = (xlWorkSheet.Cells[rCnt, 3].Value == null ? "" : xlWorkSheet.Cells[rCnt, 3].Value.ToString());
                    String field4 = (xlWorkSheet.Cells[rCnt, 4].Value == null ? "" : xlWorkSheet.Cells[rCnt, 4].Value.ToString());
                    String field5 = (xlWorkSheet.Cells[rCnt, 5].Value == null ? "" : xlWorkSheet.Cells[rCnt, 5].Value.ToString());
                    String field6 = (xlWorkSheet.Cells[rCnt, 6].Value == null ? "" : xlWorkSheet.Cells[rCnt, 6].Value.ToString());
                    String field7 = (xlWorkSheet.Cells[rCnt, 7].Value == null ? "" : xlWorkSheet.Cells[rCnt, 7].Value.ToString());
                    String field8 = (xlWorkSheet.Cells[rCnt, 8].Value == null ? "" : xlWorkSheet.Cells[rCnt, 8].Value.ToString());
                    String field9 = (xlWorkSheet.Cells[rCnt, 9].Value == null ? "" : xlWorkSheet.Cells[rCnt, 9].Value.ToString());
                    String field10 = (xlWorkSheet.Cells[rCnt, 10].Value == null ? "" : xlWorkSheet.Cells[rCnt, 10].Value.ToString());
                    String field11 = (xlWorkSheet.Cells[rCnt, 11].Value == null ? "" : xlWorkSheet.Cells[rCnt, 11].Value.ToString());
                    String field12 = (xlWorkSheet.Cells[rCnt, 12].Value == null ? "" : xlWorkSheet.Cells[rCnt, 12].Value.ToString());
                 
                    rowList.Add(field1);
                    rowList.Add(field2); 
                    rowList.Add(field3);
                    rowList.Add(field4);
                    rowList.Add(field5);
                    rowList.Add(field6);
                    rowList.Add(field7);
                    rowList.Add(field8);
                    rowList.Add(field9);
                    rowList.Add(field10);
                 
                    rowList.Add(field11);
                    rowList.Add(field12);

                    string[] row = rowList.ToArray();
                    
                     dataGridView1.Rows.Add(row);
                } 
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);


            // Stream myStream = null;
            /* OpenFileDialog openFileDialog1 = new OpenFileDialog();

             openFileDialog1.InitialDirectory = "c:\\";
             openFileDialog1.Filter = "(*.xls,*.xlsx)|*.xls;*.xlsx";
             openFileDialog1.FilterIndex = 2;
             openFileDialog1.RestoreDirectory = true;
             DialogResult result = openFileDialog1.ShowDialog();
             String file = "";

             if (result == DialogResult.OK)
             {
                 file = openFileDialog1.FileName;
                 // this.XlsPathTextBox.Text = file;
             }
             else
             {
                 return;
             }

             try
             {

                 DataSet ds = new DataSet();
                 string constring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + file + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"";
                 OleDbConnection con = new OleDbConnection(constring + "");

                 con.Open();
                 DataTable dt1 = con.GetSchema("Tables");
                 string firstSheet = dt1.Rows[0]["TABLE_NAME"].ToString();
                 string sqlquery = "Select * From [" + dt1.Rows[0]["TABLE_NAME"].ToString() + "]";

                 OleDbDataAdapter da = new OleDbDataAdapter(sqlquery, con);
                 da.Fill(ds);
                 DataTable dt = ds.Tables[0];
                 this.ResultsDataGridView.DataSource = dt;
                 con.Close();
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Πρόβλημα ανάγνωσης αρχείου.");
             } */
        }
    }
}
