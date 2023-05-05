using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Files00200101B
{
    [XmlRoot(ElementName = "fh:DIASMDDFH")]
    public class XmlIOClassB
    {

            [XmlElement("fh:DIASFileHdr")]
            public DIASFileHdr dIASFileHdr = new DIASFileHdr();

            [XmlElement("fh:MndtInfrm")]
            public List<MndtInfrm> mndtInfrm = new List<MndtInfrm>();

        
        
    }

    public class MndtInfrm
    {

        private string xmlns_ = "urn:DMDD:xsd:dias.002.001.01";
        [XmlAttribute("xmlnsss")]
        public String xmlnsss
        {
            get { return xmlns_; }
            set { xmlns_ = value; }
        }


        [XmlElement("GrpHdr")]
        public XMLGroup_HeaderB GrpHdr = new XMLGroup_HeaderB();
        //  List<XMLGroup_Header> xMLGroup_Headers = new List<XMLGroup_Header>();

        [XmlElement("MndtInf")]
        public List<XMLPaymentInformationB> MndtInf = new List<XMLPaymentInformationB>();
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
