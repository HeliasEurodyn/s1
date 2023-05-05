﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ClassLibrary8
{
    //[XmlElement("DIASMDDFH")]
    // [XmlRoot("DIASMDDFH")]
    [XmlRoot(ElementName = "fh:DIASMDDFH")]
    public class XmlIOClass
    {

        /*
        [XmlAttribute("xmlns")]
      public String xmlnsAttribure = "urn:DMDDxsd:$DIASMDDFH";
        */

       // private string xmlns_ = "urn:DMDDfh:xsd:$DIASMDDFH";
        //[XmlAttribute("xmlnsss")]
        //public String xmlnsss
        //{
        //    get { return xmlns_; }
        //   set { xmlns_ = value; }     
        //}


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
         public XMLGroup_Header GrpHdr = new XMLGroup_Header();
        //  List<XMLGroup_Header> xMLGroup_Headers = new List<XMLGroup_Header>();

        [XmlElement("MndtInf")]
        public List<XMLPaymentInformation> MndtInf = new List<XMLPaymentInformation>();
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
