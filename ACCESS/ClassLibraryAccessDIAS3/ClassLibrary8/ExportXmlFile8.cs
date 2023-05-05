using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Softone;


namespace Files00800101
{
    class ExportXmlFile8
    {

        public XModule curModule;
        public XSupport curSupport;

        public XTable CCCDIASFOLDERHEADERTable; 
        public XTable CCCDIASFOLDERDETAILTable; 
        public XTable CCCDIASFOLDERDATELOGTable;

        public void updateDIASFOLDERDETAIL(String Findoc,String EndToEndId, String PmntInfId,String MessageId)
        {
            for (int i = 0; i < CCCDIASFOLDERDETAILTable.Count; i++)
            { 
             if(CCCDIASFOLDERDETAILTable[i,"FINDOC"].ToString() == Findoc)
                {
                CCCDIASFOLDERDETAILTable[i, "ENDTOENDID"] = EndToEndId;
                CCCDIASFOLDERDETAILTable[i, "PMNTINFID"] = PmntInfId;
                CCCDIASFOLDERDETAILTable[i, "MSGID008"] = MessageId;
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
        String HeaderCODE
        )
        {
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
                            , CONVERT(VARCHAR(10),A.TRNDATE,20) AS SIGNATUREDATE

                            ,(SELECT TOP 1 BB.BICCODE 
                            FROM BANKBRANCH BB
                            INNER JOIN TRDBANKACC  BAC ON BB.BANK = BAC.BANK
                            WHERE BAC.CCCDIAS = 1 AND BAC.COMPANY = A.COMPANY  AND BAC.TRDR = A.TRDR
                            AND BB.BICCODE != '') AS BIC


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


                XmlIOClass8 xmlC = new XmlIOClass8();


                xmlC.dIASFileHdr.Sndr = BigCustomerText;
                xmlC.dIASFileHdr.Rcvr = "DIASGRA1";
                xmlC.dIASFileHdr.FileRef = "00" + DateTime.Now.ToString("yyyyMMddHHmmss");
                xmlC.dIASFileHdr.SrvcID = "DDD";
                xmlC.dIASFileHdr.TstCode = "T";
                xmlC.dIASFileHdr.FType = "XDD";
                xmlC.dIASFileHdr.NumGrp = "1";
                String DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                xmlC.cstmrDrctDbtInitn.xMLGroup_Header.MessageIdentification = DateTimeStamp + "-" + HeaderCODE + "-3";
                    //DateTimeStamp + "-" + tbl[i, "TRDR"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-3";
                    
                 //   DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + HeaderCODE; // tbl[i, "FINDOC"].ToString() + "-3";
                xmlC.cstmrDrctDbtInitn.xMLGroup_Header.CreationDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff");

                xmlC.cstmrDrctDbtInitn.xMLGroup_Header.initiatingParty.identification.privateIdentification.other.Identification = "GR80ZZZ00" + BigCustomerText;
                xmlC.cstmrDrctDbtInitn.xMLGroup_Header.initiatingParty.identification.privateIdentification.other.schemeName.Proprietary = "SEPA";



                for (int i = 0; i < tbl.Count; i++)
                {
                    XMLPaymentInformation8 xMLPaymentInformation = new XMLPaymentInformation8();

                    xMLPaymentInformation.PaymentInformationIdentification = DateTimeStamp + tbl[i, "FINDOC"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString()+ "-3";  //tbl[i, "FINCODE"].ToString();
                    xMLPaymentInformation.PaymentMethod = "DD";

                    xMLPaymentInformation.paymentTypeInformation.serviceLevel.Code = "SEPA";
                    xMLPaymentInformation.paymentTypeInformation.localInstrument.Code = "B2B";
                   // if (i == 0) xMLPaymentInformation.paymentTypeInformation.SequenceType = "FRST";
                   // else 
                    xMLPaymentInformation.paymentTypeInformation.SequenceType = "RCUR";

                    String FINALDATE = tbl[i, "FINALDATE"].ToString();

                    xMLPaymentInformation.RequestedCollationDate = FINALDATE;//.Substring(0, 10);

                    xMLPaymentInformation.creditor.Name = DescrText;
                    xMLPaymentInformation.creditorAccount.identification.IBAN = IBANText;
                    xMLPaymentInformation.creditorAgent.finInstnId.BIC = "PIRBGRA0";
                    xMLPaymentInformation.creditorSchemeIdentification.creditorSchemeIdentificationIdentification.creditorSchemeIdentificationIdentificationPrivateIdentification.creditorSchemeIdentificationIdentificationPrivateIdentificationOther.Identification = "GR80ZZZ00" + BigCustomerText;
                    xMLPaymentInformation.creditorSchemeIdentification.creditorSchemeIdentificationIdentification.creditorSchemeIdentificationIdentificationPrivateIdentification.creditorSchemeIdentificationIdentificationPrivateIdentificationOther.Sceme_Name.Proprietary = "SEPA";
                    //CdtrSchmeId
                    xMLPaymentInformation.directDebitTransuction.paymentIdentification.EndOfIdentification =
                        // tbl[i, "TRDR"].ToString();
                       DateTimeStamp + tbl[i, "FINDOC"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-3";

                    xMLPaymentInformation.directDebitTransuction.InstdAmt = float.Parse(tbl[i, "DUEPAY"].ToString()).ToString("0.00").Replace(",", ".");

                    xMLPaymentInformation.directDebitTransuction.directDebitTransaction.mandateRelatedInformation.MandateIdentification = tbl[i, "MANDID"].ToString();  //"000000" + tbl[i, "AFM"].ToString();
                    xMLPaymentInformation.directDebitTransuction.directDebitTransaction.mandateRelatedInformation.DateOfSignature = tbl[i, "SIGNATUREDATE"].ToString();    
                    
                    DateTime.UtcNow.ToString("yyyy-MM-dd");  //FINALDATE; tbl[i, "SIGNATUREDATE"].ToString().Substring(0, 10);


                    xMLPaymentInformation.directDebitTransuction.directDebitTransaction.mandateRelatedInformation.AmdmntInd = "false";

                    xMLPaymentInformation.directDebitTransuction.debtorAgent.finInstnId.BIC = tbl[i, "BIC"].ToString();  //"ETHNGRAA"; /*BIC*/
                    xMLPaymentInformation.directDebitTransuction.debtor.Name = tbl[i, "X_TNAME"].ToString();
                    xMLPaymentInformation.directDebitTransuction.DebtorAccount.debtorAccountIdentification.IBAN = tbl[i, "IBAN"].ToString();
                  //  xMLPaymentInformation.directDebitTransuction.remittanceInformation.Ustrd = "ΠΑΓΙΑ ΕΝΤΟΛΗ";

                    xmlC.cstmrDrctDbtInitn.xMLPaymentInformations.Add(xMLPaymentInformation);

                    for (int k = 0; k < CCCDIASFOLDERDETAILTable.Count; k++)
                    {
                        CCCDIASFOLDERDETAILTable.Current.Edit(k);
                        if (CCCDIASFOLDERDETAILTable.Current["FINDOC"].ToString() == tbl[i, "FINDOC"].ToString())
                        {
                            CCCDIASFOLDERDETAILTable.Current["XMLMSGID"] = xMLPaymentInformation.PaymentInformationIdentification;
                        //    CCCDIASFOLDERDETAILTable.Current["DATEOFSIGNATURE"] = xMLPaymentInformation.PaymentInformationIdentification;
                            CCCDIASFOLDERDETAILTable.Current.Post();
                        }
                       
                    }


                    this.updateDIASFOLDERDETAIL(
                        tbl[i, "FINDOC"].ToString(),
                        DateTimeStamp + tbl[i, "FINDOC"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-3",
                        DateTimeStamp + tbl[i, "FINDOC"].ToString() + "-" + tbl[i, "SIGNATUREID"].ToString() + "-3",
                        xmlC.cstmrDrctDbtInitn.xMLGroup_Header.MessageIdentification
                        );

                    TotalDUEPAY += float.Parse(tbl[i, "DUEPAY"].ToString());
                    totalTransuctions++;
                }

            

                xmlC.cstmrDrctDbtInitn.xMLGroup_Header.NumberOfTrunsactions = totalTransuctions.ToString();
                xmlC.cstmrDrctDbtInitn.xMLGroup_Header.ControlSum = TotalDUEPAY.ToString("0.00").Replace(",", ".");

                //Trailler Record
                if (tbl.Count > 0)
                {
                    JobDone = true;
                    XmlSerializer SerializerObj = new XmlSerializer(typeof(XmlIOClass8));
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
            CCCDIASFOLDERDATELOGTable.Current["MSGID"] = 4;
            CCCDIASFOLDERDATELOGTable.Current["MSGDESCR"] = "Αρχείο Χρέωσης";

            CCCDIASFOLDERDATELOGTable.Current.Post();
            curModule.PostData();
        }



    }
}
