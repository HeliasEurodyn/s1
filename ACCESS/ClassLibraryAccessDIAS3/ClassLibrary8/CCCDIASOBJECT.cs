using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

using Softone;

namespace ClassLibrary8
{
    [WorksOn("CCCDIASOBJ")]
    class CCCDIASOBJECT : TXCode
    {
        XTable CCCDIASFOLDERHEADERTable; //CCCDIASFOLDERHEADER
        XTable CCCDIASFOLDERDETAILTable; //CCCDIASFOLDERDETAIL
        XTable CCCDIASFOLDERDATELOGTable; //CCCDIASFOLDERDATELOG
        public XModule curModule;
        public XSupport curSupport;
        public int numberOfDay = 0;



        public override void Initialize()
        {
            curModule = XModule;
            curSupport = XSupport;
            CCCDIASFOLDERHEADERTable = XModule.GetTable("CCCDIASFOLDERHEADER");
            CCCDIASFOLDERDETAILTable = XModule.GetTable("CCCDIASFOLDERDETAIL");
            CCCDIASFOLDERDATELOGTable = XModule.GetTable("CCCDIASFOLDERDATELOG");
        }



        public override void AfterLocate()
        {
            String SQL = " SELECT MAX(ISNULL(FILENUMBER,0)) AS FILENUMBER  FROM CCCDIASFOLDERDATELOG  WHERE CONVERT(nvarchar(10),TRNDATE,120) = CONVERT(nvarchar(10), GETDATE(),120) ";
                XTable tbl = this.curSupport.GetSQLDataSet(SQL);
                numberOfDay = 0;
               if(tbl.Count>0) int.TryParse(  tbl[0, "FILENUMBER"].ToString(), out numberOfDay); 
        }


        public Boolean CheckBicExistance(String BIC)
        {
            String sql = "SELECT COUNT(*) AS BICSNUM  FROM CCCDIASBANKS WHERE '" + BIC + "' LIKE '%' +RIGHT(BIC,LEN(BIC)-1)  + '%' AND BIC != ''  ";
            XTable tbl = this.curSupport.GetSQLDataSet(sql);
            Boolean exists = false;

            for (int i = 0; i < tbl.Count; i++)
                {

                 int BICSNUM = 0; 
                 Boolean PARSED = false;
                 PARSED = int.TryParse(tbl[i, "BICSNUM"].ToString(),out BICSNUM);

                 if (PARSED && BICSNUM > 0) exists = true;

                 }

            return exists;
        }

        public override object ExecCommand(int Cmd)
        {
            String findocsSerialized = "";
            bool FirstRound = true;
            DialogResult dialogResult;

            String findocSelected = CCCDIASFOLDERDETAILTable.Current["FINDOC"].ToString();

            if (Cmd >= 150002 && Cmd <= 150008 )
            {
              

                for (int i = 0; i < CCCDIASFOLDERDETAILTable.Count; i++)
                {
                    CCCDIASFOLDERDETAILTable.Current.Edit(i);
                    if (FirstRound) findocsSerialized += CCCDIASFOLDERDETAILTable.Current["FINDOC"].ToString();
                    else findocsSerialized += "," + CCCDIASFOLDERDETAILTable.Current["FINDOC"].ToString();
                    FirstRound = false;

                    if (
                       !CheckBicExistance(CCCDIASFOLDERDETAILTable.Current["BIC"].ToString())
                        )
                    {
                        MessageBox.Show("Το BIC " + CCCDIASFOLDERDETAILTable.Current["BIC"].ToString() + " δεν υπάρχει στο ΔΙΑΣ. Πρέπει να γίνει διόρθωση.");
                        return base.ExecCommand(Cmd);
                    }

                }

                if (findocsSerialized == "") return base.ExecCommand(Cmd);
            }

            switch (Cmd)
            {
                case 150002:


                    dialogResult = MessageBox.Show("Αποστολή αρχείου σε Τράπεζα;","Επιλογή", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        return base.ExecCommand(Cmd); 
                    }

                    numberOfDay++;

                    ExportXmlFile actions = new ExportXmlFile();

                    actions.curModule = this.curModule;
                    actions.curSupport = this.curSupport;

                    actions.CCCDIASFOLDERHEADERTable = CCCDIASFOLDERHEADERTable;
                    actions.CCCDIASFOLDERDETAILTable = CCCDIASFOLDERDETAILTable;
                    actions.CCCDIASFOLDERDATELOGTable = CCCDIASFOLDERDATELOGTable;

                    actions.Export(
                        findocsSerialized, 
                        Settings1.Default["CustomerId"].ToString(),
                        numberOfDay,
                        Settings1.Default["Description"].ToString(),
                        Settings1.Default["IBAN"].ToString(),
                        Settings1.Default["EXPORTPATH"].ToString(),

                        Settings1.Default["FtpUrl"].ToString(),
                        Settings1.Default["FtpUser"].ToString(),
                        Settings1.Default["FtpPassword"].ToString()
                        );

                    

                break;

                    //AMEN

                case 150003:


                dialogResult = MessageBox.Show("Αποστολή αρχείου σε Τράπεζα;", "Επιλογή", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return base.ExecCommand(Cmd);
                }
                numberOfDay++;
                ExportXmlFileAMEN fileAmen = new ExportXmlFileAMEN();

                fileAmen.curModule = this.curModule;
                fileAmen.curSupport = this.curSupport;
                fileAmen.CCCDIASFOLDERHEADERTable = CCCDIASFOLDERHEADERTable;
                fileAmen.CCCDIASFOLDERDETAILTable = CCCDIASFOLDERDETAILTable;
                fileAmen.CCCDIASFOLDERDATELOGTable = CCCDIASFOLDERDATELOGTable;


                fileAmen.Export(
                    findocsSerialized,
                    Settings1.Default["CustomerId"].ToString(),
                    numberOfDay,
                    Settings1.Default["Description"].ToString(),
                    Settings1.Default["IBAN"].ToString(),
                    Settings1.Default["EXPORTPATH"].ToString(),

                    Settings1.Default["FtpUrl"].ToString(),
                    Settings1.Default["FtpUser"].ToString(),
                    Settings1.Default["FtpPassword"].ToString()
                    );

                break;



                case 150004:

              

                numberOfDay++;


                dialogResult = MessageBox.Show("Αποστολή αρχείου σε Τράπεζα;", "Επιλογή", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return base.ExecCommand(Cmd);
                }


                ExportXmlFileREVO fileRevo = new ExportXmlFileREVO();

                fileRevo.curModule = this.curModule;
                fileRevo.curSupport = this.curSupport;
                fileRevo.CCCDIASFOLDERHEADERTable = CCCDIASFOLDERHEADERTable;
                fileRevo.CCCDIASFOLDERDETAILTable = CCCDIASFOLDERDETAILTable;
                fileRevo.CCCDIASFOLDERDATELOGTable = CCCDIASFOLDERDATELOGTable;


               fileRevo.Export(
                    findocSelected,
                    Settings1.Default["CustomerId"].ToString(),
                    numberOfDay,
                    Settings1.Default["Description"].ToString(),
                    Settings1.Default["IBAN"].ToString(),
                    Settings1.Default["EXPORTPATH"].ToString(),

                    Settings1.Default["FtpUrl"].ToString(),
                    Settings1.Default["FtpUser"].ToString(),
                    Settings1.Default["FtpPassword"].ToString()
                    );


                break;



                case 150005:

                dialogResult = MessageBox.Show("Αποστολή αρχείου σε Τράπεζα;", "Επιλογή", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return base.ExecCommand(Cmd);
                }

                numberOfDay++;
                Files00800101.ExportXmlFile8 actions8 = new Files00800101.ExportXmlFile8();

                actions8.curModule = this.curModule;
                actions8.curSupport = this.curSupport;

                actions8.CCCDIASFOLDERHEADERTable = CCCDIASFOLDERHEADERTable;
                actions8.CCCDIASFOLDERDETAILTable = CCCDIASFOLDERDETAILTable;
                actions8.CCCDIASFOLDERDATELOGTable = CCCDIASFOLDERDATELOGTable;

                actions8.Export(
                    findocsSerialized,
                    Settings1.Default["CustomerId"].ToString(),
                    numberOfDay,
                    Settings1.Default["Description"].ToString(),
                    Settings1.Default["IBAN"].ToString(),
                    Settings1.Default["EXPORTPATH"].ToString(),

                    Settings1.Default["FtpUrl"].ToString(),
                    Settings1.Default["FtpUser"].ToString(),
                    Settings1.Default["FtpPassword"].ToString(),
                    CCCDIASFOLDERHEADERTable.Current["CCCDIASFOLDERHEADER"].ToString()
                    );


                break;


                case 150006:


                dialogResult = MessageBox.Show("Αποστολή αρχείου σε Τράπεζα;", "Επιλογή", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return base.ExecCommand(Cmd);
                }

                numberOfDay++;
                Files00700102.ExportXmlFile7 actions7 = new Files00700102.ExportXmlFile7();

                actions7.curModule = this.curModule;
                actions7.curSupport = this.curSupport;

                actions7.CCCDIASFOLDERHEADERTable = CCCDIASFOLDERHEADERTable;
                actions7.CCCDIASFOLDERDETAILTable = CCCDIASFOLDERDETAILTable;
                actions7.CCCDIASFOLDERDATELOGTable = CCCDIASFOLDERDATELOGTable;
                          
                actions7.Export(
                    findocSelected,
                    Settings1.Default["CustomerId"].ToString(),
                    numberOfDay,
                    Settings1.Default["Description"].ToString(),
                    Settings1.Default["IBAN"].ToString(),
                    Settings1.Default["EXPORTPATH"].ToString(),

                    Settings1.Default["FtpUrl"].ToString(),
                    Settings1.Default["FtpUser"].ToString(),
                    Settings1.Default["FtpPassword"].ToString(),
                    CCCDIASFOLDERHEADERTable.Current["CCCDIASFOLDERHEADER"].ToString(),
                    CCCDIASFOLDERDETAILTable.Current["ENDTOENDID"].ToString(),
                    CCCDIASFOLDERDETAILTable.Current["PMNTINFID"].ToString(),
                    CCCDIASFOLDERDETAILTable.Current["MSGID008"].ToString()
                    );


                break;


                case 150007:


                ReadFtpFiles rf = new ReadFtpFiles();

                rf.updateFtpFileListToDb(
                                   Settings1.Default["FtpUrlIn"].ToString(),
                                   Settings1.Default["FtpUser"].ToString(),
                                   Settings1.Default["FtpPassword"].ToString(), 
                                   this.curSupport);

                break;


                case 150008:


                ReadFtpFiles rf2 = new ReadFtpFiles();
                rf2.curSupport = this.curSupport;

                rf2.DownloadFiles(
                                   Settings1.Default["FtpUrlIn"].ToString(),
                                   Settings1.Default["FtpUser"].ToString(),
                                   Settings1.Default["FtpPassword"].ToString(), 
                                   Settings1.Default["IMPORTPATH"].ToString(),
                                   Settings1.Default["CustomerId"].ToString()
                                   );

                break;




                this.curModule.PostData();

            }
            return base.ExecCommand(Cmd);
        }


    }


}
