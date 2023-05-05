using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Files00700102
{

    public class OrgnlPmtInfAndRvsl
    {
         [XmlElement("RvslPmtInfId")]
        public String RvslPmtInfId = "";

         [XmlElement("OrgnlPmtInfId")]
        public String OrgnlPmtInfId = "";

         [XmlElement("PmtInfRvsl")]
        public String PmtInfRvsl = "";

         [XmlElement("TxInf")]
        public TxInf txInf = new TxInf();

    }

    public class TxInf
    {
         [XmlElement("RvslId")]
        public String RvslId = "";

         [XmlElement("OrgnlEndToEndId")]
        public String OrgnlEndToEndId = "";

         [XmlElement("OrgnlInstdAmt")]
        public String OrgnlInstdAmt = "";

         [XmlElement("RvsdInstdAmt")]
        public String RvsdInstdAmt = "";

         [XmlElement("ChrgBr")]
        public String ChrgBr = "SLEV";

         [XmlElement("RvslRsnInf")]
        public RvslRsnInf  rvslRsnInf = new RvslRsnInf();

         [XmlElement("OrgnlTxRef")]
        public OrgnlTxRef orgnlTxRef = new OrgnlTxRef();

    }

    public class OrgnlTxRef
    {
         [XmlElement("CdtrSchmeId")]
        public CdtrSchmeId cdtrSchmeId = new CdtrSchmeId();

         [XmlElement("PmtTpInf")]
        public PmtTpInf_ pmtTpInf = new PmtTpInf_();

         [XmlElement("MndtRltdInf")]
        public MndtRltdInf mndtRltdInf = new MndtRltdInf();
     
    }

    public class UltmtCdtr_
    {
        [XmlElement("Nm")]
        public String Nm = "";
    }

    public class CdtrAcct
    {
        [XmlElement("Id")]
        public Id___ id = new Id___();
    }


    public class Id___
    {
        [XmlElement("IBAN")]
        public String IBAN = "";     
    }

    public class Cdtr
    {
        [XmlElement("Nm")]
        public String Nm = "";

        [XmlElement("PstlAdr")]
        public PstlAdr__ pstlAdr = new PstlAdr__();
    }

    public class PstlAdr__
    {
        [XmlElement("Ctry")]
        public String Ctry = "";

         [XmlElement("AdrLine")]
        public String AdrLine = "";
    }

    
    public class DbtrAgt_
    {
        [XmlElement("FinInstnId")]
        public FinInstnId_ finInstnId = new FinInstnId_();
    }


    public class FinInstnId_
    {
        [XmlElement("BIC")]
        public String BIC = "";
    }


    


    public class DbtrAcct_
    {
         [XmlElement("Id")]
        public Id__ pstlAdr = new Id__();
    }

    public class Id__
    {
        [XmlElement("IBAN")]
        public String IBAN  = "";
    }


    public class Dbtr_
    {
        [XmlElement("Nm")]
        public String Nm = "";

        [XmlElement("PstlAdr")]
        public PstlAdr_ pstlAdr = new PstlAdr_();

        [XmlElement("CtryOfRes")]
        public String CtryOfRes = "";
    }


    public class PstlAdr_
    {
        [XmlElement("Ctry")]
        public String Ctry = "";

        [XmlElement("AdrLine")]
        public String AdrLine = "";
    }


    public class UltmtDbtr_
    {
         [XmlElement("Nm")]
        public String Nm = "";
    }

    public class RmtInf
    {
        [XmlElement("Ustrd")]
        public String Ustrd = "";
    }

    public class MndtRltdInf
    {
        [XmlElement("MndtId")]
        public String MndtId = "";

        [XmlElement("DtOfSgntr")]
        public String DtOfSgntr = "";

        [XmlElement("AmdmntInd")]
        public String AmdmntInd = "false";
    }


    public class PmtTpInf_
    {
        [XmlElement("SvcLvl")]
        public SvcLvl_ svcLvl = new SvcLvl_();

        [XmlElement("LclInstrm")]
        public LclInstrm_ lclInstrm = new LclInstrm_();

        [XmlElement("SeqTp")]
        public String SeqTp = "RCUR";
    }


    public class SvcLvl_
    {
        [XmlElement("Cd")]
        public String Cd = "";
    }

    public class LclInstrm_
    {
        [XmlElement("Cd")]
        public String Cd = "";
    }
    public class CdtrSchmeId
    {
        [XmlElement("Id")]
        public Id_ id = new Id_();
    }

    public class Id_
    {
        [XmlElement("PrvtId")]
        public PrvtId_ prvtId = new PrvtId_();
    }

    public class PrvtId_
    {
        [XmlElement("Othr")]
        public Othr othr = new Othr();
    }

    public class Othr
    {
        [XmlElement("Id")]
        public String Id = "";

        [XmlElement("SchmeNm")]
        public SchmeNm_ schmeNm = new SchmeNm_();
        
    }

    public class SchmeNm_
    {
        [XmlElement("Prtry")]
        public String Prtry = "";
    }

    

    public class Id
    {
        [XmlElement("Id")]
        public Id id = new Id();
    }

    public class RvslRsnInf
    {
        [XmlElement("Rsn")]
        public Rsn rsn = new Rsn();
    }

    public class Rsn
    {
        [XmlElement("Cd")]
        public String Cd = "MS02";
    }

}
