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
    public partial class AddToDiasFolder : Form
    {
        XSupport xs_;
        XModule xm_;
        string[] findocs_;
        string findocs_serialized;
        int findocs_number_ = 0;

        public AddToDiasFolder(XSupport xs, XModule xm)
        {
            InitializeComponent();
            xs_ = xs;
            xm_ = xm;
        }


        private void AddToDiasFolder_Load(object sender, EventArgs e)
        {
            findocs_serialized = CTools.CToolsNewSDK.CustomToolsNewSDK.GetSelRecValuesFromRightClick(xm_);
            findocs_ = findocs_serialized.Split(',');// CTools.CToolsNewSDK.CustomToolsNewSDK.GetSelRecValuesFromRightClick(xm_).Split(',');

            foreach (string word in findocs_)
            {
                findocs_number_++;  
            }

            loadDiasFoldersToCombobox();
            this.toolStrip1.Enabled = true;
            this.MsgLabel.Text = " Αριθμός επιλεγμένων παραστατικών: " + findocs_number_.ToString();

        }


        public void loadDiasFoldersToCombobox()
        {
            Dictionary<int, String> DICT = new Dictionary<int, String>();

            String sql =
               " SELECT A.CCCDIASFOLDERHEADER AS ID,  " +
               " (A.CODE+ '  ' + A.NAME) AS DESCR     " +
               " FROM CCCDIASFOLDERHEADER A           " +
               " ORDER BY A.CCCDIASFOLDERHEADER       ";

            XTable TRDRDATASql = xs_.GetSQLDataSet(sql);
            for (int i = 0; i < TRDRDATASql.Count; i++)
            {
                DICT.Add(
                    int.Parse(TRDRDATASql[i, "ID"].ToString()),
                     TRDRDATASql[i, "DESCR"].ToString());
            }

            this.SelComboBox.DataSource = new BindingSource(DICT, null);
            this.SelComboBox.DisplayMember = "Value";
            this.SelComboBox.ValueMember = "Key";

            if (TRDRDATASql.Count < 0) this.SelComboBox.SelectedIndex = 0;
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            String CCCDIASFOLDERHEADERStr = ((KeyValuePair<int, String>)this.SelComboBox.SelectedItem).Key.ToString();

            foreach (string FINDOCStr in findocs_)
                {

                String BICCODE = "";
                String IBAN = "";
                String TRDR = "";

                String SqlBank =
                          " SELECT TOP 1 BB.BICCODE , BAC.IBAN , " +
                          " (SELECT NAME FROM TRDR WHERE TRDR = (SELECT TRDR FROM FINDOC WHERE FINDOC =" + FINDOCStr + "  )) AS TRDRNAME, " +
                           " (SELECT TRDR FROM FINDOC WHERE FINDOC =" + FINDOCStr + "  ) AS TRDR " + 
                          "  FROM BANKBRANCH BB " +
                          "  INNER JOIN TRDBANKACC  BAC ON BB.BANK = BAC.BANK " +
                          "  WHERE BAC.CCCDIAS = 1 AND BAC.COMPANY = " +  xs_.ConnectionInfo.CompanyId.ToString() + 
                          "  AND BAC.TRDR = (SELECT TRDR FROM FINDOC WHERE FINDOC = " + FINDOCStr + " ) " +
                          "  AND BB.BICCODE != '' ";

                XTable tbl = xs_.GetSQLDataSet(SqlBank);

                for (int i = 0; i < tbl.Count; i++)
                    {
                        BICCODE = tbl[i, "BICCODE"].ToString();
                        IBAN = tbl[i, "IBAN"].ToString();
                        TRDR = tbl[i, "TRDR"].ToString();
                    }


                    String sql =
                     " BEGIN   "+
                         " IF NOT EXISTS (SELECT * FROM CCCDIASFOLDERDETAIL " +
                         "                 WHERE FINDOC = " + FINDOCStr + " ) " +
                         " BEGIN                                        "+
                                    " INSERT INTO CCCDIASFOLDERDETAIL (FINDOC,CCCDIASFOLDERHEADER,BIC,IBAN,TRDR) " +
                                    "   VALUES (" + FINDOCStr + "," + CCCDIASFOLDERHEADERStr + ",'" + BICCODE + "','" + IBAN + "','" + TRDR + "') "+
                         " END  " +
                     " END  ";

                      xs_.ExecuteSQL(sql);
                }

            MessageBox.Show("Η διαδικασία ολοκληρώθηκε.");
            this.toolStrip1.Enabled = false;
        }




    }
}
