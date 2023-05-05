using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Files00700102
{
   public class OrgnlGrpInf
    {
        [XmlElement("OrgnlMsgId")]
        public String OrgnlMsgId = "";

        [XmlElement("OrgnlMsgNmId")]
        public String OrgnlMsgNmId = "";
    }
}
