using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Files00700102
{
   public class XMLGroup_Header712
    {

        [XmlElement("MsgId")]
        public String MessageIdentification = "";

        [XmlElement("CreDtTm")]
        public String CreationDateTime = "";

        // [XmlElement("Authorisation")]
        //  public String Authorisation = "";

        [XmlElement("NbOfTxs")]
        public String NumberOfTrunsactions = "";

        [XmlElement("GrpRvsl")]
        public String GrpRvsl = "false";
        

      //  [XmlElement("CtrlSum")]
     //   public String ControlSum = "";

        [XmlElement("InitgPty")]
        public InitiatingParty initiatingParty = new InitiatingParty();

    }


    public class InitiatingParty
    {
        //  [XmlElement("Initiating_Party")]
        //  public String InitiatingPartyId = "";
        // [XmlElement("Name")]
        //  public String Name = "";

        // [XmlElement("Postal_Address")]
        // public String PostalAddress = "";

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
