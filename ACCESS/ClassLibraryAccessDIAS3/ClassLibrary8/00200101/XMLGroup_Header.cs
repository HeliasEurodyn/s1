﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ClassLibrary8
{
    // [XmlRoot("Group_Header")]
    public class XMLGroup_Header
    {
       // [XmlAttribute("xmlnsss")]
      //  public String xmlnsss = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";


        [XmlElement("MsgId")]
        public String MessageIdentification = "";

        [XmlElement("CreDtTm")]
        public String CreationDateTime = "";


        [XmlElement("ClrSys")]
        public ClrSys clrSys = new ClrSys();

        [XmlElement("InitgPty")]
        public InitiatingParty initiatingParty = new InitiatingParty();

        [XmlElement("DbtrAgt")]
        public DbtrAgt dbtrAgt = new DbtrAgt();


        [XmlElement("TxCd")]
        public String TxCd = "MAND";

        [XmlElement("PmtTpInf")]
        public PmtTpInf pmtTpInf = new PmtTpInf();

        // [XmlElement("Authorisation")]
        //  public String Authorisation = "";

      //  [XmlElement("NbOfTxs")]
      //  public String NumberOfTrunsactions = "";

      //  [XmlElement("CtrlSum")]
     //   public String ControlSum = "";

       

    }


       

       

    public class DbtrAgt
    {

             [XmlElement("FinInstnId")]
        public FinInstnId finInstnId = new FinInstnId();
    }

    public class PmtTpInf
    {
        [XmlElement("SvcLvl")]
        public SvcLvl svcLvl = new SvcLvl();

         [XmlElement("LclInstrm")]
        public LclInstrm lclInstrm = new LclInstrm();
    }

    public class LclInstrm
    {
        [XmlElement("Cd")]
        public String Cd = "CORE";
    }

    public class SvcLvl
    {
        [XmlElement("Cd")]
        public String Cd = "SEPA";
    }
    
    public class ClrSys
    {
        //  [XmlElement("Initiating_Party")]
        //  public String InitiatingPartyId = "";
        // [XmlElement("Name")]
        //  public String Name = "";

        // [XmlElement("Postal_Address")]
        // public String PostalAddress = "";

        [XmlElement("Prtry")]
        public String Prtry = "DIAS";
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

