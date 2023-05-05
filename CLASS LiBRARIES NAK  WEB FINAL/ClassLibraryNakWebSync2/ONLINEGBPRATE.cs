using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ClassLibraryNakWebSync
{
    class ONLINEGBPRATE
    {

        public static double GetCurrencyListFromWeb()
        {
            double currency = -1;

            using (XmlReader xmlr = XmlReader.Create(@"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml"))
            {
                xmlr.ReadToFollowing("Cube");
                while (xmlr.Read())
                {
                    if (xmlr.NodeType != XmlNodeType.Element) continue;
                    if (xmlr.GetAttribute("currency") != null)
                    {
                        if (xmlr.GetAttribute("currency").ToString() == "GBP")
                        {
                            currency = double.Parse(xmlr.GetAttribute("rate").ToString().ToString().Replace(".", ","));
                        }
                    }
                }
            }
            return currency;
        }

    }
}
