using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Files00200101C
{
    public class XMLPaymentInformationC
    {

        [XmlElement("TxId")]
        public String TxId = "5166990000001";

        [XmlElement("PmtMtd")]
        public String PmtMtd = "DD";

        [XmlElement("SeqTp")]
        public String SeqTp = "RCUR";

        //[XmlElement("DtOfSgntr")]
        // public String DtOfSgntr = "2016-10-01";

        [XmlElement("MndtId")]
        public String MndtId = "076577899";

        [XmlElement("OrgnlMndtId")]
        public String OrgnlMndtId = "076577899";


       // [XmlElement("Rsn")]
       // public Rsn rsn = new Rsn();

    }





    public class UltmtCdtr
    {

        [XmlElement("Id")]
        public UltmtDbtrId Id = new UltmtDbtrId();

    }


    public class UltmtDbtr
    {
        // [XmlElement("Nm")]
        // public String Nm = "";

        [XmlElement("Id")]
        public UltmtDbtrId Id = new UltmtDbtrId();

    }


    public class UltmtDbtrId
    {

        [XmlElement("PrvtId")]
        public UltmtDbtrIdPrvtId PrvtId = new UltmtDbtrIdPrvtId();

    }

    public class UltmtDbtrIdPrvtId
    {

        [XmlElement("Othr")]
        public UltmtDbtrIdOthr othr = new UltmtDbtrIdOthr();
    }


    public class UltmtDbtrIdOthr
    {
        [XmlElement("Id")]
        public String Id = "";

        [XmlElement("SchmeNm")]
        public UltmtDbtrIdOthrSchmeNm SchmeNm = new UltmtDbtrIdOthrSchmeNm();

    }


    public class UltmtDbtrIdOthrSchmeNm
    {
        [XmlElement("Prtry")]
        public String Prtry = "";
    }


    // public class UltmtDbtrIdOthr
    // {
    //     [XmlElement("Id")]
    //     public String Id = "";
    // }


    public class DbtrAcct
    {
        [XmlElement("Id")]
        public DbtrAcctId dbtrAcctId = new DbtrAcctId();
    }

    public class DbtrAcctId
    {
        [XmlElement("IBAN")]
        public String IBAN = "";
    }


    public class Dbtr
    {
        [XmlElement("Nm")]
        public String Nm = "";


        // [XmlElement("PstlAdr")]
        //  public PstlAdr pstlAdr = new PstlAdr();

        [XmlElement("Id")]
        public DbtrId dbtrId = new DbtrId();

    }


    public class DbtrId
    {

        [XmlElement("PrvtId")]
        public PrvtId prvtId = new PrvtId();


        //   [XmlElement("SchmeNm")]
        //   public SchmeNm schmeNm = new SchmeNm();

        //   [XmlElement("AdrLine")]
        //   public String AdrLine = "";

    }

    //  public class PrvtId
    //  {
    ///      [XmlElement("Othr")]
    //      public PrvtIdOthr othr = new PrvtIdOthr();

    //      [XmlElement("SchmeNm")]
    //      public SchmeNm schmeNm = new SchmeNm();

    //  }


    public class PrvtId
    {
        [XmlElement("Othr")]
        public PrvtIdOthr othr = new PrvtIdOthr();

    }

    public class PrvtIdOthr
    {
        [XmlElement("Id")]
        public String Id = "";

        [XmlElement("SchmeNm")]
        public SchmeNm schmeNm = new SchmeNm();

    }



    public class SchmeNm
    {
        [XmlElement("Cd")]
        public String Cd = "NIDN";

    }

    public class PstlAdr
    {
        [XmlElement("Ctry")]
        public String Ctry = "";

        [XmlElement("AdrLine")]
        public String AdrLine = "";

    }




    public class Rsn
    {
        [XmlElement("Cd")]
        public String Cd = "MS03";
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

    //  public class FinInstnId
    //  {
    //      [XmlElement("BIC")]
    //      public String BIC = "";
    //  }

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
