using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ClassLibrary81
{
    class XMLGroup_Header8
    {
        [XmlAttribute("xmlnsss")]
        public String xmlnsss = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

        [XmlElement("MsgId")]
        public String MessageIdentification = "";

        [XmlElement("CreDtTm")]
        public String CreationDateTime = "";

        [XmlElement("NbOfTxs")]
        public String NumberOfTrunsactions = "";

        [XmlElement("CtrlSum")]
        public String ControlSum = "";

        [XmlElement("InitgPty")]
        public InitiatingParty initiatingParty = new InitiatingParty();
    }



    public class InitiatingParty
    {
        [XmlElement("Id")]
        public Identification identification = new Identification();
    }

    public class Identification
    {
        [XmlElement("PrvtId")]
        public PrivateIdentification privateIdentification = new PrivateIdentification();

    }


    public class PrivateIdentification
    {
        [XmlElement("Othr")]
        public Other other = new Other();

    }

    public class Other
    {

        [XmlElement("Id")]
        public String Identification = "";


        [XmlElement("SchmeNm")]
        public SchemeName schemeName = new SchemeName();

    }

    public class SchemeName
    {

        [XmlElement("Prtry")]
        public String Proprietary = "SEPA";

    }




}
