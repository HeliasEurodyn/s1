using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Softone;

namespace Files00700102
{
    class ExportXmlFile7
    {

        public XModule curModule;
        public XSupport curSupport;

        public XTable CCCDIASFOLDERHEADERTable;
        public XTable CCCDIASFOLDERDETAILTable;
        public XTable CCCDIASFOLDERDATELOGTable;


        public void updateDIASFOLDERDETAIL(String Findoc, String REVERSALID)
        {
            for (int i = 0; i < CCCDIASFOLDERDETAILTable.Count; i++)
            {
                if (CCCDIASFOLDERDETAILTable[i, "FINDOC"].ToString() == Findoc)
                {
                    CCCDIASFOLDERDETAILTable[i, "REVERSALID"] = REVERSALID;

                }

            }
        }

        public void Export(
        String findocs_serialized,
        String BigCustomerText,
        int AINCFile,
        String DescrText,
        String IBANText,
        String PathText,

        String FTPDOMAIN,
        String FTPUSER,
        String FTPPASSWORD,
        String HeaderCODE,
        String ENDTOENDID,
        String PMNTINFID,
        String ORIGMSGID
        )
        {

            if (ENDTOENDID == "" || PMNTINFID == "")
            {
                MessageBox.Show("Δεν έχετε αποστήλει χρέωση για να κάνετε ακύρωση χρέωσης.");
                return;
            }

            Boolean JobDone = false;

            String AINCFileStr = AINCFile.ToString();
            if (AINCFileStr.Length == 1) AINCFileStr = "0" + AINCFileStr;

            try
            {

                String sql = @" 
                        SELECT A.COMPANY
	                        ,A.FINDOC
	                        ,A.SOSOURCE
	                        ,A.SOREDIR
	                        ,A.TRNDATE
	                        ,A.SERIES
	                        ,A.FPRMS
	                        ,A.FINCODE
	                        ,A.BRANCH
	                        ,A.SODTYPE
	                        ,A.TRDR
	                        ,C.CODE AS X_CODE
	                        ,C.NAME AS X_TNAME
	                        ,C.ISPROSP AS X_ISPROSP
	                        ,C.SOCURRENCY AS X_SOCURRENCY
                            ,C.AFM
	                        ,A.ISPRINT
	                        ,A.APPRV
	                        ,ISNULL(A.SUMAMNT, 0) AS SUMAMNT
                            ,(SELECT IBAN FROM TRDBANKACC WHERE CCCDIAS = 1 AND COMPANY = A.COMPANY  AND TRDR = A.TRDR)  AS IBAN
                            ,A.SUMAMNT  AS DUEPAY
                            , CONVERT(VARCHAR(10),DATEADD(day,2,GETDATE()),20) AS FINALDATE
                          
                            ,(SELECT TOP 1 CONVERT(VARCHAR(10),DATEOFSIGNATURE,20)
                            FROM CCCDIASMAND 
                            WHERE ISNULL(ACTIVE,0) = 1 
                            AND ISNULL(MANIRECEIVED,0) != 1 
                            AND ISNULL(DIASACTIVATED,0) = 1 
                            AND ISNULL(MANDID,'')  != ''
                            AND TRDR = A.TRDR ) AS SIGNATUREDATE

                            ,(SELECT TOP 1 CCCDIASMAND
                            FROM CCCDIASMAND 
                            WHERE ISNULL(ACTIVE,0) = 1 
                            AND ISNULL(MANIRECEIVED,0) != 1 
                            AND ISNULL(DIASACTIVATED,0) = 1 
                            AND ISNULL(MANDID,'')  != ''
                            AND TRDR = A.TRDR ) AS SIGNATUREID


                            ,(SELECT TOP 1 MANDID
                            FROM CCCDIASMAND 
                            WHERE ISNULL(ACTIVE,0) = 1 
                            AND ISNULL(MANIRECEIVED,0) != 1 
                            AND ISNULL(DIASACTIVATED,0) = 1 
                            AND ISNULL(MANDID,'')  != ''
                            AND TRDR = A.TRDR ) AS MANDID

                            ,(SELECT TOP 1 BB.BICCODE 
                            FROM BANKBRANCH BB
                            INNER JOIN TRDBANKACC  BAC ON BB.BANK = BAC.BANK
                            WHERE BAC.CCCDIAS = 1 AND BAC.COMPANY = A.COMPANY  AND BAC.TRDR = A.TRDR
                            AND BB.BICCODE != '') AS BIC

                            FROM 
	                        FINDOC A LEFT OUTER JOIN TRDR C ON A.TRDR = C.TRDR
                            WHERE 
	                        A.COMPANY = " + curSupport.ConnectionInfo.CompanyId.ToString() + @"
	                       
	                        AND A.SOREDIR = 0
	                        AND A.SODTYPE = 13
	                        AND A.FINDOC IN (" + findocs_serialized + @")

                            AND (SELECT COUNT(*)
                            FROM CCCDIASMAND
                            WHERE ISNULL(ACTIVE,0) = 1 
                            AND ISNULL(MANIRECEIVED,0) != 1 
                            AND ISNULL(DIASACTIVATED,0) = 1 
                            AND ISNULL(MANDID,'')  != ''
                            AND TRDR = A.TRDR ) = 1 

                            ORDER BY A.TRNDATE DESC
	                        ,A.FINDOC  ";



                XTable tbl = this.curSupport.GetSQLDataSet(sql);

                float TotalDUEPAY = 0;
                int totalTransuctions = 0;


                XmlIOClass712 xmlC = new XmlIOClass712();


                xmlC.dIASFileHdr.Sndr = BigCustomerText;
                xmlC.dIASFileHdr.Rcvr = "DIASGRA1";
                xmlC.dIASFileHdr.FileRef = "00" + DateTime.Now.ToString("yyyyMMddHHmmss");
                xmlC.dIASFileHdr.SrvcID = "DDD";
                xmlC.dIASFileHdr.TstCode = "T";
                xmlC.dIASFileHdr.FType = "XDD";
                xmlC.dIASFileHdr.NumGrp = "1";

                xmlC.CstmrPmtRvsl.xMLGroup_Header.MessageIdentification = DateTime.Now.ToString("yyyyMMddHHmmss") + "-1-" + HeaderCODE; // tbl[i, "FINDOC"].ToString() + "-3";
                xmlC.CstmrPmtRvsl.xMLGroup_Header.CreationDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff");
                xmlC.CstmrPmtRvsl.xMLGroup_Header.NumberOfTrunsactions = ""; 
                
                xmlC.CstmrPmtRvsl.xMLGroup_Header.initiatingParty.identification.privateIdentification.other.Identification = "GR80ZZZ00" + BigCustomerText;
                xmlC.CstmrPmtRvsl.xMLGroup_Header.initiatingParty.identification.privateIdentification.other.schemeName.Proprietary = "SEPA";


                xmlC.CstmrPmtRvsl.orgnlGrpInf.OrgnlMsgId = ORIGMSGID; //SOS
                xmlC.CstmrPmtRvsl.orgnlGrpInf.OrgnlMsgNmId = "pain.008.001.02";


                String DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

              
                for (int i = 0; i < tbl.Count; i++)
                {
                    OrgnlPmtInfAndRvsl orgnlPmtInfAndRvsl = new OrgnlPmtInfAndRvsl();

                    orgnlPmtInfAndRvsl.RvslPmtInfId = DateTimeStamp + tbl[i, "FINDOC"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-5";
                    orgnlPmtInfAndRvsl.OrgnlPmtInfId = PMNTINFID; //DateTime.Now.ToString("yyMMddHHmmss") + tbl[i, "FINDOC"].ToString();
                    orgnlPmtInfAndRvsl.PmtInfRvsl = "false";

                    orgnlPmtInfAndRvsl.txInf.RvslId = "99" + DateTime.Now.ToString("yyMMddHHmmss") + tbl[i, "FINDOC"].ToString();
                    orgnlPmtInfAndRvsl.txInf.OrgnlEndToEndId = ENDTOENDID;
                    orgnlPmtInfAndRvsl.txInf.OrgnlInstdAmt = float.Parse(tbl[i, "DUEPAY"].ToString()).ToString("0.00").Replace(",", ".");
                    orgnlPmtInfAndRvsl.txInf.RvsdInstdAmt = float.Parse(tbl[i, "DUEPAY"].ToString()).ToString("0.00").Replace(",", ".");


                    orgnlPmtInfAndRvsl.txInf.ChrgBr = "SLEV";
                    orgnlPmtInfAndRvsl.txInf.rvslRsnInf.rsn.Cd = "MS02";

                    orgnlPmtInfAndRvsl.txInf.orgnlTxRef.cdtrSchmeId.id.prvtId.othr.Id = "GR80ZZZ00" + BigCustomerText;
                    orgnlPmtInfAndRvsl.txInf.orgnlTxRef.cdtrSchmeId.id.prvtId.othr.schmeNm.Prtry = "SEPA";

                    orgnlPmtInfAndRvsl.txInf.orgnlTxRef.pmtTpInf.svcLvl.Cd = "SEPA";
                    orgnlPmtInfAndRvsl.txInf.orgnlTxRef.pmtTpInf.lclInstrm.Cd = "CORE";
                    orgnlPmtInfAndRvsl.txInf.orgnlTxRef.pmtTpInf.SeqTp = "RCUR";

                    orgnlPmtInfAndRvsl.txInf.orgnlTxRef.mndtRltdInf.MndtId = tbl[i, "MANDID"].ToString();
                    orgnlPmtInfAndRvsl.txInf.orgnlTxRef.mndtRltdInf.DtOfSgntr = tbl[i, "SIGNATUREDATE"].ToString();// DateTime.UtcNow.ToString("yyyy-MM-dd");
                    orgnlPmtInfAndRvsl.txInf.orgnlTxRef.mndtRltdInf.AmdmntInd = "false";

                    xmlC.CstmrPmtRvsl.orgnlPmtInfAndRvsls.Add(orgnlPmtInfAndRvsl);


                    this.updateDIASFOLDERDETAIL(
                                tbl[i, "FINDOC"].ToString(),
                                orgnlPmtInfAndRvsl.RvslPmtInfId
                                );


                    TotalDUEPAY += float.Parse(tbl[i, "DUEPAY"].ToString());
                    totalTransuctions++;
                }

                xmlC.CstmrPmtRvsl.xMLGroup_Header.NumberOfTrunsactions = totalTransuctions.ToString(); 

              //  xmlC.cstmrDrctDbtInitn.xMLGroup_Header.NumberOfTrunsactions = totalTransuctions.ToString();
              //  xmlC.cstmrDrctDbtInitn.xMLGroup_Header.ControlSum = TotalDUEPAY.ToString("0.00").Replace(",", ".");

                //Trailler Record
                if (tbl.Count > 0)
                {
                    JobDone = true;
                    XmlSerializer SerializerObj = new XmlSerializer(typeof(XmlIOClass712));
                    TextWriter WriteFileStream = new StreamWriter(PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI");

                    //new StreamWriter(PathText + @"\" + this.BigCustomerTextBox.Text + DateTime.Now.ToString("yyMMdd") + AINCFileTextBox.Text + ".XMI");
                    SerializerObj.Serialize(WriteFileStream, xmlC);
                    WriteFileStream.Close();


                    //    xmlns:fh="urn:DMDDfh:xsd:$DIASMDDFH">
                    File.WriteAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI"),
                          File.ReadAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI")).Replace("_x003A_", ":"));

                    File.WriteAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI"),
                      File.ReadAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI")).Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "xmlns:fh=\"urn:DMDDfh:xsd:$DIASMDDFH\""));

                    File.WriteAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI"),
                            File.ReadAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI")).Replace("xmlnsss", "xmlns"));

                    File.WriteAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI"),
                          File.ReadAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI")).Replace("<InstdAmt>", "<InstdAmt Ccy=\"EUR\">"));

                    
                    File.WriteAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI"),
                          File.ReadAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI")).Replace("<OrgnlInstdAmt>", "<OrgnlInstdAmt Ccy=\"EUR\">"));

                    File.WriteAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI"),
                          File.ReadAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI")).Replace("<RvsdInstdAmt>", "<RvsdInstdAmt Ccy=\"EUR\">"));

                    

                }


                ClassLibrary8.UploadFileToFtpUsingSSL.Upload(
                   FTPDOMAIN + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI",
                   FTPUSER,
                   FTPPASSWORD,
                   (PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI")
               );

                String FileName = ("DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI");
                insertDateTimeStamp(AINCFile, FileName);

                if (JobDone) MessageBox.Show("Η διαδικάσία ολοκληρώθηκε.");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }



        public void insertDateTimeStamp(int AINCFile, String FileName)
        {
            CCCDIASFOLDERDATELOGTable.Current.Append();

            CCCDIASFOLDERDATELOGTable.Current["TRNDATE"] = DateTime.Now;
            CCCDIASFOLDERDATELOGTable.Current["USERID"] = this.curSupport.ConnectionInfo.UserId;
            CCCDIASFOLDERDATELOGTable.Current["FILENUMBER"] = AINCFile;
            CCCDIASFOLDERDATELOGTable.Current["FILENAME"] = FileName;
            CCCDIASFOLDERDATELOGTable.Current["MSGID"] = 5;
            CCCDIASFOLDERDATELOGTable.Current["MSGDESCR"] = "Request for Reversal";

            CCCDIASFOLDERDATELOGTable.Current.Post();
            curModule.PostData();
        }




    }
}
