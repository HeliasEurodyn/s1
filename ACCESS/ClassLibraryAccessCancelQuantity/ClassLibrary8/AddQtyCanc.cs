using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        public AddQtyCanc(XSupport xs, XModule xm)
        {
            InitializeComponent();
            xs_ = xs;
            xm_ = xm;
            
        }

        public void loadAutocompleteMtrls()
        {
            Dictionary<int, String> DICT = new Dictionary<int, String>();
            String sql =
               " SELECT MTRL,CODE,NAME FROM MTRL WHERE SODTYPE = 51 AND COMPANY = " + xs_.ConnectionInfo.CompanyId.ToString();

            var autoComplete = new AutoCompleteStringCollection();
            XTable DATASql = xs_.GetSQLDataSet(sql);

            for (int i = 0; i < DATASql.Count; i++)
            {
                autoComplete.Add(DATASql[i, "CODE"].ToString() + " " + DATASql[i, "NAME"].ToString());
                MTRLS.Add( (DATASql[i, "CODE"].ToString() + " " + DATASql[i, "NAME"].ToString()) , DATASql[i, "MTRL"].ToString() );
            }

            this.textBox1.AutoCompleteCustomSource = autoComplete;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("ΣΥΝΕΧΕΙΑ;", "ΣΥΝΕΧΕΙΑ;", MessageBoxButtons.YesNo);
            if (dialogResult != DialogResult.Yes) return;

            String colorCDIMLINESStr = "";
            String sizeCDIMLINESStr = "";

            int colorCDIMLINES = 0;
            int sizeCDIMLINES = 0;

            String MTRLStr = "";
            int MTRL = 0;
            Boolean rESULT = false;

            try
            {
                if (this.comboBox1.SelectedItem != null) 
                {
                    colorCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox1.SelectedItem).Key.ToString();
                    rESULT = int.TryParse(colorCDIMLINESStr, out colorCDIMLINES);

                }
            }
            catch (Exception ex){}

            try
            {
                if (this.comboBox2.SelectedItem != null)
                {
                    sizeCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox2.SelectedItem).Key.ToString();
                    rESULT =  int.TryParse(sizeCDIMLINESStr, out sizeCDIMLINES);
                }
            }
            catch (Exception ex) { }


            if (MTRLS.ContainsKey(this.textBox1.Text))
            {
                MTRLStr = MTRLS[this.textBox1.Text];
                rESULT = int.TryParse(MTRLStr, out MTRL);
            }
            else
            {
                MessageBox.Show("Δεν έχετε επιλέξει είδος");
                return;
            }
            


            if (colorCDIMLINESStr != "" && sizeCDIMLINESStr != "")
            {
              foreach (string findocStr in findocs_)
                {
                    int findoc = 0;
                    int.TryParse(findocStr, out findoc);
                    try
                    {
                        CancelQtysColorSize(findoc, MTRL, colorCDIMLINES, sizeCDIMLINES);
                    }
                    catch (Exception ex) { }
                }          

            }
            else if (sizeCDIMLINESStr != "")
            {
                foreach (string findocStr in findocs_)
                {
                    int findoc = 0;
                    int.TryParse(findocStr, out findoc);

                    try
                    {
                    CancelQtysColor(findoc, MTRL, colorCDIMLINES);
                    }catch (Exception ex) { }
                }
            }
            else if (colorCDIMLINESStr == "" && sizeCDIMLINESStr == "")
            {
                foreach (string findocStr in findocs_)
                {
                    int findoc = 0;
                    int.TryParse(findocStr, out findoc);

                    try
                    {
                    CancelQtys(findoc, MTRL);
                    } catch (Exception ex) { }
                }
            }


            MessageBox.Show("Η διαδικασία ολοκληρώθηκε.");

        }

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

        private void CancelQtysColorSize(int findoc,int mtrl, int colorCDIMLINES, int sizeCDIMLINES)
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

                       qty1NCov = qty1-(qty1canc+qty1cov);

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

        private void CancelQtysColor(int findoc,int mtrl, int colorCDIMLINES)
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
                        ITELINESTbl.Current["QTY1CANC"] = (double) NewQty1canc;
                        ITELINESTbl.Current.Post();

                        linesUpdated = true;
                    }
                }
            }

            if (linesUpdated) SALDOCModule.PostData();
        }




        private void textBox1_Leave(object sender, EventArgs e)
        {
            updateColor();
            updateSize();
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

        public void updateColor()
        {
            Dictionary<int, String> DICT = new Dictionary<int, String>();

            String query =
            " SELECT CL.CDIMLINES, CL.CODE  , CL.NAME                                           " +
            "  FROM MTRL M                                                                      " +
            "  INNER JOIN CDIMLINES CL ON CL.CDIM = M.CDIM1 AND CL.COMPANY = M.COMPANY          " +
            "  WHERE M.COMPANY =  " + xs_.ConnectionInfo.CompanyId.ToString() +
            "  AND M.CODE + ' ' +  M.NAME =   '" + this.textBox1.Text + "' ";

            XTable DATASql = xs_.GetSQLDataSet(query);

            if (DATASql.Count > 0)
            {
                for (int i = 0; i < DATASql.Count; i++)
                {
                    DICT.Add(
                        int.Parse(DATASql[i, "CDIMLINES"].ToString()),
                         DATASql[i, "CODE"].ToString() + " " + DATASql[i, "NAME"].ToString()

                         );
                }

                this.comboBox1.DataSource = new BindingSource(DICT, null);
                this.comboBox1.DisplayMember = "Value";
                this.comboBox1.ValueMember = "Key";

                if (DATASql.Count < 0) this.comboBox1.SelectedIndex = 0;
            }
        }


        public void updateSize()
        {
            Dictionary<int, String> DICT = new Dictionary<int, String>();

            String query =
            " SELECT CL.CDIMLINES, CL.CODE, CL.NAME                                             " +
            "  FROM MTRL M                                                                      " +
            "  INNER JOIN CDIMLINES CL ON CL.CDIM = M.CDIM2 AND CL.COMPANY = M.COMPANY          " +
            "  WHERE M.COMPANY =  " + xs_.ConnectionInfo.CompanyId.ToString() +
            "  AND M.CODE + ' ' +  M.NAME =   '" + this.textBox1.Text + "' ";

            XTable DATASql = xs_.GetSQLDataSet(query);

            
            if (DATASql.Count > 0)
            {
                for (int i = 0; i < DATASql.Count; i++)
                {
                    DICT.Add(
                        int.Parse(DATASql[i, "CDIMLINES"].ToString()),
                         DATASql[i, "CODE"].ToString() + " " + DATASql[i, "NAME"].ToString()

                         );
                }

                this.comboBox2.DataSource = new BindingSource(DICT, null);
                this.comboBox2.DisplayMember = "Value";
                this.comboBox2.ValueMember = "Key";

                if (DATASql.Count < 0) this.comboBox2.SelectedIndex = 0;
            }
        }



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

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("ΣΥΝΕΧΕΙΑ;", "ΣΥΝΕΧΕΙΑ;", MessageBoxButtons.YesNo);
            if (dialogResult != DialogResult.Yes) return;

            String colorCDIMLINESStr = "";
            String sizeCDIMLINESStr = "";

            int colorCDIMLINES = 0;
            int sizeCDIMLINES = 0;

            String MTRLStr = "";
            int MTRL = 0;
            Boolean rESULT = false;

            try
            {
                if (this.comboBox1.SelectedItem != null)
                {
                    colorCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox1.SelectedItem).Key.ToString();
                    rESULT = int.TryParse(colorCDIMLINESStr, out colorCDIMLINES);

                }
            }
            catch (Exception ex) { }

            try
            {
                if (this.comboBox2.SelectedItem != null)
                {
                    sizeCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox2.SelectedItem).Key.ToString();
                    rESULT = int.TryParse(sizeCDIMLINESStr, out sizeCDIMLINES);
                }
            }
            catch (Exception ex) { }


            if (MTRLS.ContainsKey(this.textBox1.Text))
            {
                MTRLStr = MTRLS[this.textBox1.Text];
                rESULT = int.TryParse(MTRLStr, out MTRL);
            }
            else
            {
                MessageBox.Show("Δεν έχετε επιλέξει είδος");
                return;
            }


            if (sizeCDIMLINESStr != "")
            {
                foreach (string findocStr in findocs_)
                {
                    int findoc = 0;
                    int.TryParse(findocStr, out findoc);

                    try
                    {
                        CancelQtysColor(findoc, MTRL, colorCDIMLINES);
                    }
                    catch (Exception ex) { }
                }
            }
            else
            {
                foreach (string findocStr in findocs_)
                {
                    int findoc = 0;
                    int.TryParse(findocStr, out findoc);

                    try
                    {
                        CancelQtys(findoc, MTRL);
                    }
                    catch (Exception ex) { }
                }
            }


            MessageBox.Show("Η διαδικασία ολοκληρώθηκε.");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("ΣΥΝΕΧΕΙΑ;", "ΣΥΝΕΧΕΙΑ;", MessageBoxButtons.YesNo);
            if (dialogResult != DialogResult.Yes) return;

            String colorCDIMLINESStr = "";
            String sizeCDIMLINESStr = "";

            int colorCDIMLINES = 0;
            int sizeCDIMLINES = 0;

            String MTRLStr = "";
            int MTRL = 0;
            Boolean rESULT = false;

            try
            {
                if (this.comboBox1.SelectedItem != null)
                {
                    colorCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox1.SelectedItem).Key.ToString();
                    rESULT = int.TryParse(colorCDIMLINESStr, out colorCDIMLINES);

                }
            }
            catch (Exception ex) { }

            try
            {
                if (this.comboBox2.SelectedItem != null)
                {
                    sizeCDIMLINESStr = ((KeyValuePair<int, String>)this.comboBox2.SelectedItem).Key.ToString();
                    rESULT = int.TryParse(sizeCDIMLINESStr, out sizeCDIMLINES);
                }
            }
            catch (Exception ex) { }


            if (MTRLS.ContainsKey(this.textBox1.Text))
            {
                MTRLStr = MTRLS[this.textBox1.Text];
                rESULT = int.TryParse(MTRLStr, out MTRL);
            }
            else
            {
                MessageBox.Show("Δεν έχετε επιλέξει είδος");
                return;
            }


         
            foreach (string findocStr in findocs_)
                {
                    int findoc = 0;
                    int.TryParse(findocStr, out findoc);

                    try
                    {
                        CancelQtys(findoc, MTRL);
                    }
                    catch (Exception ex) { }
                }
            


            MessageBox.Show("Η διαδικασία ολοκληρώθηκε.");

        }


    }

    
}
