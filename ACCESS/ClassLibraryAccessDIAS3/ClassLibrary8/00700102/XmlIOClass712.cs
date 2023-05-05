using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Files00800101;

namespace Files00700102
{
    [XmlRoot(ElementName = "fh:DIASMDDFH")]
    public class XmlIOClass712
    {

        [XmlElement("fh:DIASFileHdr")]
        public DIASFileHdr dIASFileHdr = new DIASFileHdr();

        [XmlElement("fh:CstmrPmtRvsl")]
        public CstmrPmtRvsl CstmrPmtRvsl = new CstmrPmtRvsl();

    }

    public class CstmrPmtRvsl
    {
        [XmlAttribute("xmlnsss")]
        public String xmlnsss = "urn:iso:std:iso:20022:tech:xsd:pain.007.001.02";

        [XmlElement("GrpHdr")]
        public XMLGroup_Header712 xMLGroup_Header = new XMLGroup_Header712();


        [XmlElement("OrgnlGrpInf")]
        public OrgnlGrpInf orgnlGrpInf = new OrgnlGrpInf();


       // [XmlElement("OrgnlPmtInfAndRvsl")]
       // public OrgnlPmtInfAndRvsl orgnlPmtInfAndRvsl = new OrgnlPmtInfAndRvsl();

        [XmlElement("OrgnlPmtInfAndRvsl")]
        public List<OrgnlPmtInfAndRvsl> orgnlPmtInfAndRvsls = new List<OrgnlPmtInfAndRvsl>();

    }




    public class DIASFileHdr
    {
        [XmlElement("fh:Sndr")]
        public String Sndr = "9999";

        [XmlElement("fh:Rcvr")]
        public String Rcvr = "ETHNGRAA";

        [XmlElement("fh:FileRef")]
        public String FileRef = "9999161010095135";

        [XmlElement("fh:SrvcID")]
        public String SrvcID = "ΣΑΧ";

        [XmlElement("fh:TstCode")]
        public String TstCode = "P";

        [XmlElement("fh:FType")]
        public String FType = "XDD";

        [XmlElement("fh:NumGrp")]
        public String NumGrp = "1";
    }



}
