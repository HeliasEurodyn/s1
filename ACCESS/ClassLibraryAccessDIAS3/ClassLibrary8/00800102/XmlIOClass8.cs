using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ClassLibrary81
{
    [XmlRoot(ElementName = "fh:DIASMDDFH")]
    class XmlIOClass8
    {
        private string xmlns_ = "urn:DMDDfh:xsd:$DIASMDDFH";
        [XmlAttribute("xmlnsss")]
        public String xmlnsss
        {
            get { return xmlns_; }
            set { xmlns_ = value; }
        }

        [XmlElement("fh:DIASFileHdr")]
        public DIASFileHdr dIASFileHdr = new DIASFileHdr();

        // [XmlElement("CstmrDrctDbtInitn")]
        //  public CstmrDrctDbtInitn cstmrDrctDbtInitn = new CstmrDrctDbtInitn();

        [XmlElement("fh:CstmrDrctDbtInitn")]
        public CstmrDrctDbtInitn cstmrDrctDbtInitn = new CstmrDrctDbtInitn();

    }


    public class CstmrDrctDbtInitn
    {
        [XmlElement("GrpHdr")]
        public XMLGroup_Header2 xMLGroup_Header = new XMLGroup_Header2();
        //  List<XMLGroup_Header> xMLGroup_Headers = new List<XMLGroup_Header>();

        [XmlElement("PmtInf")]
        public List<XMLPaymentInformation2> xMLPaymentInformations = new List<XMLPaymentInformation2>();
    }



    public class DIASFileHdr
    {
        [XmlElement("Sndr")]
        public String Sndr = "9999";

        [XmlElement("Rcvr")]
        public String Rcvr = "ETHNGRAA";

        [XmlElement("FileRef")]
        public String FileRef = "9999161010095135";

        [XmlElement("SrvcID")]
        public String SrvcID = "ΣΑΧ";

        [XmlElement("TstCode")]
        public String TstCode = "P";

        [XmlElement("FType")]
        public String FType = "XDD";

        [XmlElement("NumGrp")]
        public String NumGrp = "1";
    }


}
