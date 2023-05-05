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
    class ExportXmlFile
    {

      public XModule curModule;
      public XSupport curSupport;

      public XTable CCCDIASFOLDERHEADERTable; 
      public XTable CCCDIASFOLDERDETAILTable; 
      public XTable CCCDIASFOLDERDATELOGTable; 

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
                            AND ISNULL(DIASACTIVATED,0) != 1 
                            AND ISNULL(MANDID,'')  = ''
                            AND TRDR = C.TRDR ) AS SIGNATUREDATE

                            ,(SELECT TOP 1 CCCDIASMAND
                            FROM CCCDIASMAND 
                            WHERE ISNULL(ACTIVE,0) = 1 
                            AND ISNULL(MANIRECEIVED,0) != 1 
                            AND ISNULL(DIASACTIVATED,0) != 1 
                            AND ISNULL(MANDID,'')  = ''
                            AND TRDR = C.TRDR ) AS SIGNATUREID
                           
                            ,C.ADDRESS
                            ,(SELECT INTERCODE FROM COUNTRY WHERE COUNTRY = C.COUNTRY) AS INTERCODE
                            FROM TRDR C 
							
                            WHERE 1=1
	                        AND C.TRDR IN (SELECT DISTINCT TRDR FROM FINDOC WHERE FINDOC IN(" + findocs_serialized + @"))

                            AND (SELECT COUNT(*)
                            FROM CCCDIASMAND
                            WHERE ISNULL(ACTIVE,0) = 1 
                            AND ISNULL(MANIRECEIVED,0) != 1 
                            AND ISNULL(DIASACTIVATED,0) != 1 
                            AND ISNULL(MANDID,'')  = ''
                            AND TRDR = C.TRDR ) = 1  ";



                XTable tbl = this.curSupport.GetSQLDataSet(sql);


                float TotalDUEPAY = 0;
                int totalTransuctions = 0;

                XmlIOClass xmlC = new XmlIOClass();

                xmlC.dIASFileHdr.Sndr = BigCustomerText;
                xmlC.dIASFileHdr.Rcvr = "DIASGRA1";
                xmlC.dIASFileHdr.FileRef = "00" + DateTime.Now.ToString("yyyyMMddHHmmss");
                xmlC.dIASFileHdr.SrvcID = "DDD";
                xmlC.dIASFileHdr.TstCode = "T";
                xmlC.dIASFileHdr.FType = "XDD";
                xmlC.dIASFileHdr.NumGrp = "1";



                for (int i = 0; i < tbl.Count; i++)
                {

                    String DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                    MndtInfrm mndtInfrm = new MndtInfrm();


                    mndtInfrm.GrpHdr.MessageIdentification = DateTimeStamp + "-" + tbl[i, "TRDR"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-4"; // BigCustomerText + DateTime.Now.ToString("yyMMdd") + AINCFile;
                    mndtInfrm.GrpHdr.CreationDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff");

                    mndtInfrm.GrpHdr.clrSys.Prtry = "DIAS";
                    mndtInfrm.GrpHdr.initiatingParty.identification.privateIdentification.other.Identification = "GR80ZZZ00" + BigCustomerText;
                    mndtInfrm.GrpHdr.initiatingParty.identification.privateIdentification.other.schemeName.Proprietary = "SEPA";


                    mndtInfrm.GrpHdr.dbtrAgt.finInstnId.BIC = tbl[i, "BIC"].ToString();

                    mndtInfrm.GrpHdr.TxCd = "MAND";
                    mndtInfrm.GrpHdr.pmtTpInf.svcLvl.Cd = "SEPA";
                    mndtInfrm.GrpHdr.pmtTpInf.lclInstrm.Cd = "B2B";


                    XMLPaymentInformation mndtInf = new XMLPaymentInformation();

                    mndtInf.TxId = DateTimeStamp + "-" + tbl[i, "TRDR"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-4";  //tbl[i, "FINCODE"].ToString();
                    mndtInf.PmtMtd = "DD";
                    mndtInf.SeqTp = "RCUR";

                    String FINALDATE = tbl[i, "SIGNATUREDATE"].ToString();

                    mndtInf.DtOfSgntr = FINALDATE; // DateTime.UtcNow.ToString("yyyy-MM-dd");  //FINALDATE;
                    mndtInf.MndtId = DateTimeStamp + "-" + tbl[i, "TRDR"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-4"; 

                    mndtInf.dbtr.Nm = tbl[i, "X_TNAME"].ToString();
                    //  MndtInf.dbtr.pstlAdr.Ctry = tbl[i, "INTERCODE"].ToString();
                    //  MndtInf.dbtr.pstlAdr.AdrLine = tbl[i, "ADDRESS"].ToString();

                    mndtInf.dbtr.dbtrId.prvtId.othr.Id = tbl[i, "AFM"].ToString();
                    mndtInf.dbtr.dbtrId.prvtId.othr.schmeNm.Cd = "TXID";

                    mndtInf.dbtrAcct.dbtrAcctId.IBAN = tbl[i, "IBAN"].ToString();


                    mndtInfrm.MndtInf.Add(mndtInf);

                    xmlC.mndtInfrm.Add(mndtInfrm);


                    String SqlUpdate = " UPDATE CCCDIASMAND SET MANDID = '"+
                        DateTimeStamp + "-" + tbl[i, "TRDR"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-4"
                        +"' WHERE CCCDIASMAND =  " +tbl[i, "SIGNATUREID"].ToString();

                    this.curSupport.ExecuteSQL(SqlUpdate,null);

                    totalTransuctions++;

                }



                xmlC.dIASFileHdr.NumGrp = totalTransuctions.ToString();

                //Trailler Record
                if (tbl.Count > 0)
                {
                    JobDone = true;
                    XmlSerializer SerializerObj = new XmlSerializer(typeof(XmlIOClass));
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



      public void insertDateTimeStamp(int AINCFile, String FileName)
      {
          CCCDIASFOLDERDATELOGTable.Current.Insert();
          CCCDIASFOLDERDATELOGTable.Current["TRNDATE"] = DateTime.Now;
          CCCDIASFOLDERDATELOGTable.Current["USERID"] = this.curSupport.ConnectionInfo.UserId;
          CCCDIASFOLDERDATELOGTable.Current["FILENUMBER"] = AINCFile;
          CCCDIASFOLDERDATELOGTable.Current["FILENAME"] = FileName;
          CCCDIASFOLDERDATELOGTable.Current["MSGID"] = 1;
          CCCDIASFOLDERDATELOGTable.Current["MSGDESCR"] = "Αρχείο Ανάθεσης";

          CCCDIASFOLDERDATELOGTable.Current.Post();
          curModule.PostData();
      }



    }
}
