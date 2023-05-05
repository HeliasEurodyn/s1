using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Softone;

namespace ClassLibrary8
{
    public class ReadFtpFiles
    {
        public XSupport curSupport;

        public void updateFtpFileListToDb(string FTPAddress, string username, string password, XSupport Sup)
        {

            curSupport = Sup;
          //  List<string> files = new List<string>();

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };

                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(FTPAddress);
                req.EnableSsl = true;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.ListDirectory;
                req.Credentials = new NetworkCredential(username, password);


                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                while (!reader.EndOfStream)
                {
                    UpdateFtpFileInDatabase(reader.ReadLine());
                 //   files.Add(reader.ReadLine());
                }

            }
            catch (Exception ex)
            {
              //  MessageBox.Show(ex.Message);
            }

        }


        private void UpdateFtpFileInDatabase(String FILENAME)
        {

            String[] SPLITPARTS = FILENAME.Split(new char[] { '.' }, 2);

            String Sql =

              " IF NOT EXISTS (SELECT * FROM CCCDIASFTPFILESIN WHERE FILENAME='" + FILENAME + "' ) " +
              "     INSERT INTO CCCDIASFTPFILESIN(FILENAME,FILEEXTENSION,FILEREAD,READDATE) VALUES "+
              " ('"+FILENAME + "', "+
              " '" + SPLITPARTS[1] + "', " +
              " 0, " +
              " GETDATE() " +
              " ) ";

            this.curSupport.ExecuteSQL(Sql,null);
        }



        public void DownloadFiles(String FTPDOMAIN, String FTPUSER, String FTPPASSWORD, String LOCAL_PATH , String CustomerId)
        {
            String Sql =
           "  SELECT *                           " +
           "  FROM CCCDIASFTPFILESIN             " +
           "  WHERE FILENAME LIKE 'DDD" + CustomerId + "%'  " +
           "  AND ISNULL(FILEREAD,0) = 0         " +
           "  AND FILEEXTENSION = 'XMO'  ";

            XTable tbl = this.curSupport.GetSQLDataSet(Sql);

             for (int i = 0; i < tbl.Count; i++)
             {
                 Download(FTPDOMAIN + "/" + tbl[i, "FILENAME"].ToString(), FTPUSER, FTPPASSWORD, LOCAL_PATH + "\\" + tbl[i, "FILENAME"].ToString());
                 SearchFor002_001_01MandateActivations(LOCAL_PATH + "\\" + tbl[i, "FILENAME"].ToString());
                 SearchFor002_001_03Rejections(LOCAL_PATH + "\\" + tbl[i, "FILENAME"].ToString());
                 SearchFor054_001_03(LOCAL_PATH + "\\" + tbl[i, "FILENAME"].ToString());

                 String SqlUpdate = "UPDATE CCCDIASFTPFILESIN SET FILEREAD = 1 WHERE FILENAME = '" + tbl[i, "FILENAME"].ToString() + "'";
                 this.curSupport.ExecuteSQL(SqlUpdate, null);
             }

              Sql =
                   "  SELECT *                           " +
                   "  FROM CCCDIASFTPFILESIN             " +
                   "  WHERE FILENAME LIKE 'DDD" + CustomerId + "%'  " +
                   "  AND ISNULL(FILEREAD,0) = 0         " +
                   "  AND FILEEXTENSION = 'CRT'  ";

              tbl = this.curSupport.GetSQLDataSet(Sql);

             for (int i = 0; i < tbl.Count; i++)
             {
                 Download(FTPDOMAIN + "/" + tbl[i, "FILENAME"].ToString(), FTPUSER, FTPPASSWORD, LOCAL_PATH + "\\" + tbl[i, "FILENAME"].ToString());
                 updateCRT(LOCAL_PATH + "\\" + tbl[i, "FILENAME"].ToString());
                 
                 

                 String SqlUpdate = "UPDATE CCCDIASFTPFILESIN SET FILEREAD = 1 WHERE FILENAME = '" + tbl[i, "FILENAME"].ToString() + "'";
                 this.curSupport.ExecuteSQL(SqlUpdate, null);
             }



        }

        private void updateCRT(String FileName)
        {

            this.curSupport.ExecuteSQL(" TRUNCATE TABLE CCCDIASBANKS ", null);
            
            foreach (String BankLine in File.ReadLines(FileName, Encoding.GetEncoding("ISO-8859-7")))
            {

                //  String BankLine = "hehehe khohohohahaha";
                String BankLineTrimed = BankLine.TrimEnd();

                //  MessageBox.Show(BankLineTrimed.IndexOf(' ').ToString());

                if (BankLineTrimed.IndexOf(' ') > 0)
                {

                    String FirstPart = BankLineTrimed.Substring(0, BankLineTrimed.IndexOf(' '));
                    String SecondPart = BankLineTrimed.Substring(BankLineTrimed.IndexOf(' ') + 1);

                    FirstPart = FirstPart.Trim();
                    SecondPart = SecondPart.Trim();

                    if (FirstPart != "" && SecondPart != "")
                    {
                        try
                        {

                            String SqliNSERT = "INSERT INTO CCCDIASBANKS(BIC,NAME) VALUES ('" + FirstPart.Replace("'", "''") + "','" + SecondPart.Replace("'", "''") + "');";
                            this.curSupport.ExecuteSQL(SqliNSERT, null);

                        }
                        catch (Exception ex)
                        { }
                    }

                }

            }

        }


        public void SearchFor002_001_01MandateActivations(String path)
        {
            try
            {
                XDocument doci = XDocument.Load(path);
                var mijav =
                        doci.Descendants("{urn:DMDDfh:xsd:$DIASMDDFH}MndtInfrm").Descendants("{urn:DMDD:xsd:dias.002.001.01}MndtInf").Descendants("{urn:DMDD:xsd:dias.002.001.01}MndtId").ToList();

                foreach (String value in mijav)
                {
                    String SqlUpdate = " UPDATE CCCDIASMAND SET DIASACTIVATED = 1, MANDACTIVATEDBYDIASdt = GETDATE() " +
                            " WHERE MANDID =  '" + value + "' ";
                    this.curSupport.ExecuteSQL(SqlUpdate, null);

                }
            }
            catch (Exception ex)
            { }


        }

       

public void SearchFor002_001_03Rejections(String path)
        {
            try
            {

                XDocument doci = XDocument.Load(path);

                var CstmrPmtStsRpt =
                        doci.Descendants("{urn:DMDDfh:xsd:$DIASMDDFH}CstmrPmtStsRpt").ToList();
                foreach (var CstmrPmtStsRp in CstmrPmtStsRpt)
                {


                                var OrgnlMsgIdVar =
                                        CstmrPmtStsRp.Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}OrgnlGrpInfAndSts").Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}OrgnlMsgId").ToList();

                                String OrgnlMsgId = "";
                                if (OrgnlMsgIdVar.Count > 0) OrgnlMsgId = OrgnlMsgIdVar.ElementAt(0).Value.ToString();
                                else return;

                                // doci.Descendants("{urn:DMDDfh:xsd:$DIASMDDFH}CstmrPmtStsRpt").Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}OrgnlGrpInfAndSts").Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}OrgnlMsgId").ToList().ElementAt(0).Value.ToString();


                                var GrpStsVar = CstmrPmtStsRp.Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}OrgnlGrpInfAndSts").Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}GrpSts").ToList();
                                String GrpSts = "";
                                if (GrpStsVar.Count > 0) GrpSts = GrpStsVar.ElementAt(0).Value.ToString();


                                if (GrpSts == "RJCT")
                                {
     
                                  //  MessageBox.Show("Full Rejections from OrgnlGrpInfAndSts");
                                    var MsgIdVar = CstmrPmtStsRp.Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}GrpHdr").Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}MsgId").ToList();
                                    String MsgId = "";

                                    if (MsgIdVar.Count > 0) MsgId = MsgIdVar.ElementAt(0).Value.ToString();


                                    String SqlUpdate = " UPDATE CCCDIASFOLDERDETAIL SET REJECTIONSID = '" + MsgId + "' WHERE  MSGID008 =  '" + OrgnlMsgId + "'; "; 
                                    this.curSupport.ExecuteSQL(SqlUpdate, null);


                                
                                }

                                var OrgnlPmtInfAndSts =
                                         CstmrPmtStsRp.Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}OrgnlPmtInfAndSts").ToList();
                                //.Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}GrpSts").ToString();

                                foreach (var OrgnlPmtInfAndSt in OrgnlPmtInfAndSts)
                                {

                                    var OrgnlPmtInfIdVar = OrgnlPmtInfAndSt.Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}OrgnlPmtInfId").ToList();
                                    var StsIdVar = OrgnlPmtInfAndSt.Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}TxInfAndSts").Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}StsId").ToList();
                                    var OrgnlEndToEndIdVar = OrgnlPmtInfAndSt.Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}TxInfAndSts").Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}OrgnlEndToEndId").ToList();
                                    var TxStsVar = OrgnlPmtInfAndSt.Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}TxInfAndSts").Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}TxSts").ToList();

                                    var RegectionRsnVar = OrgnlPmtInfAndSt.Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}TxInfAndSts").Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}StsRsnInf").Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}Rsn").Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}Cd").ToList();



                                    String OrgnlPmtInfId = "";
                                    String StsId = "";
                                    String OrgnlEndToEndId = "";
                                    String TxSts = "";
                                    var RegectionRsn = "";

                                    if (OrgnlPmtInfIdVar.Count > 0) OrgnlPmtInfId = OrgnlPmtInfIdVar.ElementAt(0).Value.ToString();
                                    if (StsIdVar.Count > 0) StsId = StsIdVar.ElementAt(0).Value.ToString();
                                    if (OrgnlEndToEndIdVar.Count > 0) OrgnlEndToEndId = OrgnlEndToEndIdVar.ElementAt(0).Value.ToString();
                                    if (TxStsVar.Count > 0) TxSts = TxStsVar.ElementAt(0).Value.ToString();
                                    if (RegectionRsnVar.Count > 0) RegectionRsn = RegectionRsnVar.ElementAt(0).Value.ToString();

                                   // MessageBox.Show("   OrgnlMsgId: " + OrgnlMsgId +
                                       // "   OrgnlPmtInfId: " + OrgnlPmtInfId +
                                       // "   StsId: " + StsId +
                                       // "   OrgnlEndToEndId: " + OrgnlEndToEndId +
                                       // "   TxSts: " + TxSts
                                       // );

                                    if (TxSts == "RJCT")
                                    {
                                        String SqlUpdate = " UPDATE CCCDIASFOLDERDETAIL SET REJECTIONSID = '" + StsId + "' ,REJECTIONSRSN = '" + RegectionRsn + "' WHERE MSGID008 = '" + OrgnlMsgId + "' AND PMNTINFID = '" + OrgnlPmtInfId + "' ; ";
                                        this.curSupport.ExecuteSQL(SqlUpdate, null);

                                        if (RegectionRsn == "AC04" || RegectionRsn == "MD01")
                                            {
                                               // String SqlUpdate = " UPDATE CCCDIASFOLDERDETAIL SET REJECTIONSID = '" + StsId + "' ,REJECTIONSRSN = '" + RegectionRsn + "' WHERE MSGID008 = '" + OrgnlMsgId + "' AND PMNTINFID = '" + OrgnlPmtInfId + "' ; ";
                                               // this.curSupport.ExecuteSQL(SqlUpdate, null);
                                            }


                                    }



                                    //    String SqlUpdate = " UPDATE CCCDIASMAND SET DIASACTIVATED = 1, MANDACTIVATEDBYDIASdt = GETDATE() " +
                                    //            " WHERE MANDID =  '" + value + "' ";
                                    //    this.curSupport.ExecuteSQL(SqlUpdate, null);

                                }




                }





            }
            catch (Exception ex)
            { }


        }

        public void SearchFor054_001_03(String path)
        {
            try
            {

                XDocument doci = XDocument.Load(path);

                var BkToCstmrDbtCdtNtfctn =
                        doci.Descendants("{urn:DMDDfh:xsd:$DIASMDDFH}BkToCstmrDbtCdtNtfctn").ToList();
                foreach (var BkToCstmrDbtCdtNtfct in BkToCstmrDbtCdtNtfctn)
                {


                    var Ntrys = BkToCstmrDbtCdtNtfct.Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}Ntfctn").Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}Ntry").ToList();

                            foreach (var Ntry in Ntrys)
                            {



                                var TxDtls =
                                         Ntry.Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}NtryDtls").Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}TxDtls").ToList();
                                //.Descendants("{urn:iso:std:iso:20022:tech:xsd:pain.002.001.03}GrpSts").ToString();

                                foreach (var TxDtl in TxDtls)
                                {

                                    var EndToEndIdVar = TxDtl.Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}Refs").Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}EndToEndId").ToList();
                                    var TxIdVar = TxDtl.Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}Refs").Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}TxId").ToList();
                                    var MndtIdVar = TxDtl.Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}Refs").Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}MndtId").ToList();
                                    var ReasonVar = TxDtl.Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}RtrInf").Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}Rsn").Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}Cd").ToList();
                                   
                                    var CdtDbtIndVar = TxDtl.Descendants("{urn:iso:std:iso:20022:tech:xsd:camt.054.001.03}CdtDbtInd").ToList();



                                    String EndToEndId = "";
                                    String TxId = "";
                                    String MndtId = "";
                                    String CdtDbtInd = "";
                                    String Reason = "";
                                    
                                    //var RegectionRsn = "";

                                    if (EndToEndIdVar.Count > 0) EndToEndId = EndToEndIdVar.ElementAt(0).Value.ToString();
                                    if (TxIdVar.Count > 0) TxId = TxIdVar.ElementAt(0).Value.ToString();
                                    if (MndtIdVar.Count > 0) MndtId = MndtIdVar.ElementAt(0).Value.ToString();
                                    if (CdtDbtIndVar.Count > 0) CdtDbtInd = CdtDbtIndVar.ElementAt(0).Value.ToString();
                                    if (ReasonVar.Count > 0) Reason = ReasonVar.ElementAt(0).Value.ToString();
                                   // if (RegectionRsnVar.Count > 0) RegectionRsn = RegectionRsnVar.ElementAt(0).Value.ToString();

                                    // MessageBox.Show("   OrgnlMsgId: " + OrgnlMsgId +
                                    // "   OrgnlPmtInfId: " + OrgnlPmtInfId +
                                    // "   StsId: " + StsId +
                                    // "   OrgnlEndToEndId: " + OrgnlEndToEndId +
                                    // "   TxSts: " + TxSts
                                    // );

                                    if (CdtDbtInd == "DBIT")
                                    {
                                        String SqlUpdate = " UPDATE CCCDIASFOLDERDETAIL SET RETURNSOFCOLLID  = '" + TxId + "' ,RETURNSOFCOLLRSN = '" + Reason + "' WHERE ENDTOENDID = '" + EndToEndId + "' ; ";
                                        this.curSupport.ExecuteSQL(SqlUpdate, null);

                                        if (Reason == "AC04" || Reason == "MD01")
                                        {
                                            // String SqlUpdate = " UPDATE CCCDIASFOLDERDETAIL SET REJECTIONSID = '" + StsId + "' ,REJECTIONSRSN = '" + RegectionRsn + "' WHERE MSGID008 = '" + OrgnlMsgId + "' AND PMNTINFID = '" + OrgnlPmtInfId + "' ; ";
                                            // this.curSupport.ExecuteSQL(SqlUpdate, null);
                                        }


                                    }


                                    if (CdtDbtInd == "CRDT")
                                    {
                                        String SqlUpdate = " UPDATE CCCDIASFOLDERDETAIL SET SUCCESSFULCOLLECTIONSID  = '" + TxId + "' WHERE ENDTOENDID = '" + EndToEndId + "' ; ";
                                        this.curSupport.ExecuteSQL(SqlUpdate, null); 
                                    }





                                }


                            }


                }


            }
            catch (Exception ex)
            { }


        }


        private void Download(String FTPDOMAIN, String FTPUSER, String FTPPASSWORD, String LOCAL_PATH )
        {

            try
            {

                System.Net.ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };

                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(FTPDOMAIN);
                req.EnableSsl = true;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.DownloadFile;
                req.Credentials = new NetworkCredential(FTPUSER, FTPPASSWORD);

                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                Stream responseStream = response.GetResponseStream();
               // StreamReader reader = new StreamReader(responseStream);

               // MessageBox.Show(reader.ReadToEnd());

                using (var file = File.Create(LOCAL_PATH))
                {
                    responseStream.CopyTo(file);
                }

               // reader.Close();
                response.Close();

             //   byte[] fileData = File.ReadAllBytes(LOCAL_PATH);

                // rdr.Close();
              //  req.ContentLength = fileData.Length;

             //   Stream reqStream = req.GetRequestStream();
           //     reqStream.Write(fileData, 0, fileData.Length);

            //    reqStream.Close();

            }
            catch (Exception ex)
            {
            //    MessageBox.Show(ex.Message);
            }
        }









    }





}
