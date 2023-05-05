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
    class ExportXmlFileREVO
    {
      public XModule curModule;
      public XSupport curSupport;
      
      public XTable CCCDIASFOLDERHEADERTable; //CCCDIASFOLDERHEADER
      public XTable CCCDIASFOLDERDETAILTable; //CCCDIASFOLDERDETAIL
      public XTable CCCDIASFOLDERDATELOGTable; //CCCDIASFOLDERDATELOG

      public void Export(
      String findocs_serialized,
      String BigCustomerText,
      int AINCFile,
      String DescrText,
      String IBANText,
      String PathText,

      String FTPDOMAIN,
      String FTPUSER,
      String FTPPASSWORD
      )
        {
            Boolean JobDone = false;

            String AINCFileStr = AINCFile.ToString();
            if (AINCFileStr.Length == 1) AINCFileStr = "0" + AINCFileStr; 

            try
            {
                String sql = @" 
                          SELECT 
                             C.TRDR
	                        ,C.CODE AS X_CODE
	                        ,C.NAME AS X_TNAME
	                        ,C.ISPROSP AS X_ISPROSP
	                        ,C.SOCURRENCY AS X_SOCURRENCY
                            ,C.AFM

                            ,(SELECT IBAN FROM TRDBANKACC WHERE CCCDIAS = 1 AND COMPANY = C.COMPANY  AND TRDR = C.TRDR)  AS IBAN

                            ,(SELECT TOP 1 BB.BICCODE 
                            FROM BANKBRANCH BB
                            INNER JOIN TRDBANKACC  BAC ON BB.BANK = BAC.BANK
                            WHERE BAC.CCCDIAS = 1 AND BAC.COMPANY = C.COMPANY  AND BAC.TRDR = C.TRDR
                            AND BB.BICCODE != '') AS BIC

                            ,(SELECT TOP 1 CONVERT(VARCHAR(10),DATEOFSIGNATURE,20)
                            FROM CCCDIASMAND 
                            WHERE ISNULL(ACTIVE,0) = 1 
                            AND ISNULL(MANIRECEIVED,0) != 1 
                            AND TRDR = C.TRDR ) AS SIGNATUREDATE

                            ,(SELECT TOP 1 CCCDIASMAND
                            FROM CCCDIASMAND 
                            WHERE ISNULL(ACTIVE,0) = 1 
                            AND ISNULL(MANIRECEIVED,0) = 1 
                            AND ISNULL(DIASACTIVATED,0) != 1 
                            AND TRDR = C.TRDR ) AS SIGNATUREID

                             ,(SELECT TOP 1 MANDID
                            FROM CCCDIASMAND 
                            WHERE ISNULL(ACTIVE,0) = 1 
                            AND ISNULL(MANIRECEIVED,0) = 1 
                            AND ISNULL(DIASACTIVATED,0) != 1 
                            AND TRDR = C.TRDR ) AS MANDID

                            ,C.ADDRESS
                            ,(SELECT INTERCODE FROM COUNTRY WHERE COUNTRY = C.COUNTRY) AS INTERCODE
                            FROM TRDR C 
							
                            WHERE 1=1
	                        AND C.TRDR IN (SELECT DISTINCT TRDR FROM FINDOC WHERE FINDOC IN(" + findocs_serialized + @"))

                            AND (SELECT COUNT(*)
                            FROM CCCDIASMAND
                            WHERE ISNULL(ACTIVE,0) = 1 
                            AND ISNULL(MANIRECEIVED,0) = 1 
                            AND ISNULL(DIASACTIVATED,0) != 1 
                            AND TRDR = C.TRDR ) = 1  ";



                XTable tbl = this.curSupport.GetSQLDataSet(sql);


                float TotalDUEPAY = 0;
                int totalTransuctions = 0;

                Files00200101B.XmlIOClassB xmlC = new Files00200101B.XmlIOClassB();

                xmlC.dIASFileHdr.Sndr = BigCustomerText;
                xmlC.dIASFileHdr.Rcvr = "DIASGRA1";
                xmlC.dIASFileHdr.FileRef = "00" + DateTime.Now.ToString("yyyyMMddHHmmss");
                xmlC.dIASFileHdr.SrvcID = "DDD";
                xmlC.dIASFileHdr.TstCode = "T";
                xmlC.dIASFileHdr.FType = "XDD";
                xmlC.dIASFileHdr.NumGrp = "1";


                String DateTimeStamp = DateTime.Now.ToString("yyMMddHHmmss");

                for (int i = 0; i < tbl.Count; i++)
                {



                    Files00200101B.MndtInfrm mndtInfrm = new Files00200101B.MndtInfrm();


                    mndtInfrm.GrpHdr.MessageIdentification = DateTimeStamp + "-" + tbl[i, "TRDR"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-2"; // BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFile;
                    mndtInfrm.GrpHdr.CreationDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff");

                    mndtInfrm.GrpHdr.clrSys.Prtry = "DIAS";
                    mndtInfrm.GrpHdr.initiatingParty.identification.privateIdentification.other.Identification = "GR80ZZZ00" + BigCustomerText;
                    mndtInfrm.GrpHdr.initiatingParty.identification.privateIdentification.other.schemeName.Proprietary = "SEPA";


                    //mndtInfrm.GrpHdr.dbtrAgt.finInstnId.BIC = tbl[i, "BIC"].ToString();

                    mndtInfrm.GrpHdr.TxCd = "REVO";
                    mndtInfrm.GrpHdr.pmtTpInf.svcLvl.Cd = "SEPA";
                    mndtInfrm.GrpHdr.pmtTpInf.lclInstrm.Cd = "B2B";


                    Files00200101B.XMLPaymentInformationB mndtInf = new Files00200101B.XMLPaymentInformationB();

                    mndtInf.TxId = DateTimeStamp + "-" + tbl[i, "TRDR"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-2"; //tbl[i, "FINCODE"].ToString();
                    mndtInf.PmtMtd = "DD";
                    mndtInf.SeqTp = "RCUR";

                   // String FINALDATE = tbl[i, "FINALDATE"].ToString();
                    // mndtInf.DtOfSgntr = DateTime.UtcNow.ToString("yyyy-MM-dd");  //FINALDATE;
                    mndtInf.MndtId = tbl[i, "MANDID"].ToString();
                    mndtInf.rsn.Cd = "MS03";
                 //   mndtInf.dbtr.Nm = tbl[i, "X_TNAME"].ToString();
                    //  MndtInf.dbtr.pstlAdr.Ctry = tbl[i, "INTERCODE"].ToString();
                    //  MndtInf.dbtr.pstlAdr.AdrLine = tbl[i, "ADDRESS"].ToString();

                 //   mndtInf.dbtr.dbtrId.prvtId.othr.Id = tbl[i, "AFM"].ToString();
                 //   mndtInf.dbtr.dbtrId.prvtId.othr.schmeNm.Cd = "TXID";

                 //   mndtInf.dbtrAcct.dbtrAcctId.IBAN = tbl[i, "IBAN"].ToString();


                    mndtInfrm.MndtInf.Add(mndtInf);
                    xmlC.mndtInfrm.Add(mndtInfrm);

                    String SqlUpdate = " UPDATE CCCDIASMAND SET ACTIVE = 0, REVOID = '" +
                         DateTimeStamp + "-" + tbl[i, "TRDR"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-2"
                         + "' WHERE CCCDIASMAND =  " + tbl[i, "SIGNATUREID"].ToString();

                    this.curSupport.ExecuteSQL(SqlUpdate, null);


                    totalTransuctions++;

                }



                xmlC.dIASFileHdr.NumGrp = totalTransuctions.ToString();


                //  xmlC.cstmrDrctDbtInitn.xMLGroup_Header.NumberOfTrunsactions = totalTransuctions.ToString();
                //   xmlC.cstmrDrctDbtInitn.xMLGroup_Header.ControlSum = TotalDUEPAY.ToString("0.00").Replace(",", ".");

                //Trailler Record
                if (tbl.Count > 0)
                {
                    JobDone = true;
                    XmlSerializer SerializerObj = new XmlSerializer(typeof(Files00200101B.XmlIOClassB));
                    TextWriter WriteFileStream = new StreamWriter(PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI");
                    SerializerObj.Serialize(WriteFileStream, xmlC);
                    WriteFileStream.Close();

                    File.WriteAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI"),
                        File.ReadAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI")).Replace("_x003A_", ":"));

                    File.WriteAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI"),
                      File.ReadAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI")).Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "xmlns:fh=\"urn:DMDDfh:xsd:$DIASMDDFH\""));

                    File.WriteAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI"),
                            File.ReadAllText((PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI")).Replace("xmlnsss", "xmlns"));


                    UploadFileToFtpUsingSSL.Upload(
                            FTPDOMAIN + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI",
                            FTPUSER,
                            FTPPASSWORD,
                            (PathText + @"\DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI")
                        );

                    String FileName = ("DDD" + BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFileStr + ".XMI");
                    insertDateTimeStamp(AINCFile, FileName);


                }
             

                if (JobDone) MessageBox.Show("Η διαδικάσία ολοκληρώθηκε.");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }



      public void insertDateTimeStamp(int AINCFile , String FileName)
        {
            CCCDIASFOLDERDATELOGTable.Current.Append();
                CCCDIASFOLDERDATELOGTable.Current["TRNDATE"] = DateTime.Now;
                CCCDIASFOLDERDATELOGTable.Current["USERID"] = this.curSupport.ConnectionInfo.UserId;
                CCCDIASFOLDERDATELOGTable.Current["FILENUMBER"] = AINCFile;
                CCCDIASFOLDERDATELOGTable.Current["FILENAME"] = FileName;
                CCCDIASFOLDERDATELOGTable.Current["MSGID"] = 3;
                CCCDIASFOLDERDATELOGTable.Current["MSGDESCR"] = "Αρχείο Ανακλήσεων";

            CCCDIASFOLDERDATELOGTable.Current.Post();
            curModule.PostData();
        }




       /*public void uploadFileToFtp(String FTPDOMAIN, String FTPUSER, String FTPPASSWORD, String OUTPUT_PATH )
        {
                if (FTPDOMAIN != " " && FTPDOMAIN != "" && FTPDOMAIN != "~")
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential(FTPUSER, FTPPASSWORD);
                        client.UploadFile(FTPDOMAIN, OUTPUT_PATH);
                    }
                }
        } */



    }










    
}
