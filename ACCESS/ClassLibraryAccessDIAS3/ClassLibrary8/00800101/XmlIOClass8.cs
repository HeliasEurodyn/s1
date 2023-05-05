using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Files00800101
{

   [XmlRoot(ElementName = "fh:DIASMDDFH")]
    public class XmlIOClass8
    {



        [XmlElement("fh:DIASFileHdr")]
        public DIASFileHdr dIASFileHdr = new DIASFileHdr();

        [XmlElement("fh:CstmrDrctDbtInitn")]
        public CstmrDrctDbtInitn cstmrDrctDbtInitn = new CstmrDrctDbtInitn();

     
    }


    public class CstmrDrctDbtInitn
    {

       // public String test= "123";
        [XmlAttribute("xmlnsss")]
        public String xmlnsss = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

        [XmlElement("GrpHdr")] 
        public XMLGroup_Header8 xMLGroup_Header = new XMLGroup_Header8();
        //  List<XMLGroup_Header> xMLGroup_Headers = new List<XMLGroup_Header>();

        [XmlElement("PmtInf")]
        public List<XMLPaymentInformation8> xMLPaymentInformations = new List<XMLPaymentInformation8>();
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
