using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Softone;

namespace ClassLibrary8
{
        [WorksOn("PURDOC")]
    class CCCAddQtyCanc : TXCode
    {

            AddQtyCanc addQtyCanc;

        public override void Initialize()
        {

            addQtyCanc = new AddQtyCanc(XSupport, XModule);
            addQtyCanc.TopLevel = false;
            addQtyCanc.Visible = true;


            XModule.InsertControl(addQtyCanc, "*PAGE(Page3,Αυτόματη Εισαγωγή Παραστατικών)");

        }

    }
}