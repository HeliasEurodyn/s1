using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Softone;

namespace ClassLibrary8
{
    [WorksOn("CCCVTAddQtyCanc")]
    class CCCAddToDiasFolder : TXCode
    {
        AddToDiasFolder addToDiasFolder;

        public override void Initialize()
        {
            addToDiasFolder = new AddToDiasFolder(XSupport, XModule);
            addToDiasFolder.TopLevel = false;
            addToDiasFolder.Visible = true;

            XModule.InsertControl(addToDiasFolder, "*PAGE(CustomPanel,Αποστολή σε φάκελο Δίας)");
        }

    }
}
