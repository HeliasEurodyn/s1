using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ClassLibrary81
{
    class XMLPaymentInformation2
    {

        [XmlElement("PmtInfId")]
        public String PaymentInformationIdentification = "";

        [XmlElement("PmtMtd")]
        public String PaymentMethod = "";

        [XmlElement("PmtTpInf")]
        public PaymentTypeInformation paymentTypeInformation = new PaymentTypeInformation();

        [XmlElement("ReqdColltnDt")]
        public String RequestedCollationDate = "2016-10-11";

        [XmlElement("Cdtr")]
        public Creditor creditor = new Creditor();

        [XmlElement("CdtrAcct")]
        public CreditorAccount creditorAccount = new CreditorAccount();


        [XmlElement("CdtrAgt")]
        public CreditorAgent creditorAgent = new CreditorAgent();

        [XmlElement("CdtrSchmeId")]
        public CreditorSchemeIdentification creditorSchemeIdentification = new CreditorSchemeIdentification();

        [XmlElement("DrctDbtTxInf")]
        public DirectDebitTransuction directDebitTransuction = new DirectDebitTransuction();

    }


    
    public class DirectDebitTransuction
    {
        [XmlElement("PmtId")]
        public PaymentIdentification paymentIdentification = new PaymentIdentification();
         
        [XmlElement("InstdAmt")]
        public String InstdAmt = "";

        [XmlElement("DrctDbtTx")]
        public DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction();

        [XmlElement("DbtrAgt")]
        public DebtorAgent debtorAgent = new DebtorAgent();

        [XmlElement("Dbtr")]
        public Debtor debtor = new Debtor();

        [XmlElement("DbtrAcct")]
        public DebtorAccount DebtorAccount = new DebtorAccount();

        [XmlElement("RmtInf")]
        public RemittanceInformation remittanceInformation = new RemittanceInformation();

    }


    public class RemittanceInformation
    {
        [XmlElement("Ustrd")]
        public String Ustrd = "ΠΑΓΙΑ ΕΝΤΟΛΗ";

     //   [XmlElement("Ustrd")]
    //    public CreditorReference creditorReference = new CreditorReference();
    }

    public class CreditorReference
    {
        [XmlElement("Type")]
        public CreditorReferenceType creditorReferenceType = new CreditorReferenceType();
    }

    public class CreditorReferenceType
    {
        [XmlElement("Code_Or_Proprietary")]
        public CodeOrProprietary codeOrProprietary = new CodeOrProprietary();
    }

    public class CodeOrProprietary
    {
        [XmlElement("Code")]
        public String Code = "";
    }

    public class DebtorAccount
    {
        [XmlElement("Id")]
        public DebtorAccountIdentification debtorAccountIdentification = new DebtorAccountIdentification();
    }

    public class DebtorAccountIdentification
    {
        [XmlElement("IBAN")]
        public String IBAN = "";
    }

    public class Debtor
    {
        [XmlElement("Nm")]
        public String Name = "NOTPROVIDED";
    }


    public class DebtorAgent
    {
        [XmlElement("FinInstnId")]
        public FinInstnId finInstnId = new FinInstnId();
    }



    public class DirectDebitTransaction
    {
        [XmlElement("MndtRltdInf")]
        public MandateRelatedInformation mandateRelatedInformation = new MandateRelatedInformation();
    }

    public class MandateRelatedInformation
    {
        [XmlElement("MndtId")]
        public String MandateIdentification = "";

        [XmlElement("DtOfSgntr")]
        public String DateOfSignature = "";

        [XmlElement("AmdmntInd")]
        public String AmdmntInd = "false";
    }



    public class PaymentIdentification
    {
        //2.28
        [XmlElement("EndToEndId")]
        public String EndOfIdentification = "";
    }

    public class CreditorSchemeIdentification
    {
        //2.27
        [XmlElement("Id")]
        public CreditorSchemeIdentificationIdentification creditorSchemeIdentificationIdentification = new CreditorSchemeIdentificationIdentification();

    }

    //2.27
    public class CreditorSchemeIdentificationIdentification
    {
        //2.27
        [XmlElement("PrvtId")]
        public CreditorSchemeIdentificationIdentificationPrivateIdentification creditorSchemeIdentificationIdentificationPrivateIdentification = new CreditorSchemeIdentificationIdentificationPrivateIdentification();

    }
    public class CreditorSchemeIdentificationIdentificationPrivateIdentification
    {
        //2.27
        [XmlElement("Othr")]
        public CreditorSchemeIdentificationIdentificationPrivateIdentificationOther creditorSchemeIdentificationIdentificationPrivateIdentificationOther = new CreditorSchemeIdentificationIdentificationPrivateIdentificationOther();

    }

    public class CreditorSchemeIdentificationIdentificationPrivateIdentificationOther
    {
        //2.27
        [XmlElement("Id")]
        public String Identification = "";

       // [XmlElement("Sceme_Name")]
       // public CreditorSchemeIdentificationIdentificationPrivateIdentificationOtherScemeName creditorSchemeIdentificationIdentificationPrivateIdentificationOtherScemeName = new CreditorSchemeIdentificationIdentificationPrivateIdentificationOtherScemeName();

    }

    public class CreditorSchemeIdentificationIdentificationPrivateIdentificationOtherScemeName
    {
        //2.27
        [XmlElement("Proprietary")]
        public String Proprietary = "";
    }


    public class CreditorAgent
    {
        [XmlElement("FinInstnId")]
        public FinInstnId finInstnId = new FinInstnId();
    }

    public class FinInstnId
    {
        [XmlElement("BIC")]
        public String BIC = "";

    }

    public class CreditorAccount
    {
      //  [XmlElement("Nm")]
      //  public String Name = "";

        [XmlElement("Id")]
        public CreditorAccountIdentification identification = new CreditorAccountIdentification();

    }
    public class CreditorAccountIdentification
    {
        [XmlElement("IBAN")]
        public String IBAN = "";

    }


    public class Creditor
    {
        [XmlElement("Nm")]
        public String Name = "";

    }


    public class PaymentTypeInformation
    {
        [XmlElement("SvcLvl")]
        public ServiceLevel serviceLevel = new ServiceLevel();

        [XmlElement("LclInstrm")]
        public LocalInstrument localInstrument = new LocalInstrument();

        [XmlElement("SeqTp")]
        public String SequenceType = "FRST";
    }

    public class ServiceLevel
    {
        [XmlElement("Cd")]
        public String Code = "SEPA";
    }


    public class LocalInstrument
    {
        [XmlElement("Cd")]
        public String Code = "B2B";
    }

}






