using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Softone;


namespace ClassLibraryNakWebSync
{
    public partial class WebSyncFormQtys : Form
    {

        XSupport XSupport = null;
        private Magento magento = new Magento();
        private Magento magento2 = new Magento();

        public WebSyncFormQtys()
        {
            InitializeComponent();

            this.UrlTextBox.Text = Properties.Settings1.Default["Url"].ToString();
            this.UserTextBox.Text = Properties.Settings1.Default["Username"].ToString();
            this.PasswordTextBox.Text = Properties.Settings1.Default["Password"].ToString();


            this.Url2TextBox.Text = Properties.Settings1.Default["Url2"].ToString();
            this.User2TextBox.Text = Properties.Settings1.Default["Username2"].ToString();
            this.Password2TextBox.Text = Properties.Settings1.Default["Password2"].ToString();


            if (Properties.Settings1.Default["Web1"].ToString() == "1") this.WebSite1CheckBox.Checked = true;
            else this.WebSite1CheckBox.Checked = false;

            if (Properties.Settings1.Default["Web2"].ToString() == "1") this.WebSite2CheckBox.Checked = true;
            else this.WebSite2CheckBox.Checked = false;


        }

        private void UpdateLatestQtysbutton_Click(object sender, EventArgs e)
        {
            RunUpdateQtys(2);
        }


        private void AuthenticateWebServices()
        {
            try
            {

                String Username = Properties.Settings1.Default["Username"].ToString();
                String Password = Properties.Settings1.Default["Password"].ToString();

                String AccessToken = magento.Authenticate(
                      Properties.Settings1.Default["Url"].ToString() + "/rest/V1/integration/admin/token",
                     "{\"username\":\""+ Username + "\", \"password\":\""+ Password + "\"}",1);

           } 
            catch
           (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }




        private void AuthenticateWebServices2()
        {
            try
            {

                String Username = Properties.Settings1.Default["Username2"].ToString();
                String Password = Properties.Settings1.Default["Password2"].ToString();

                String AccessToken = magento2.Authenticate(
                  
                      Properties.Settings1.Default["Url2"].ToString() + "/rest/V1/integration/admin/token",
                     "{\"username\":\"" + Username + "\", \"password\":\"" + Password + "\"}",2);

            }
            catch
           (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }





        private void RunUpdatePrices()
        {

            String ExtraWhere = "";

            if (this.ItemCodeTextBox.Text == "")
            {
                ExtraWhere = " AND M.CCCINT01 = 1 ";
            }
            else
            {
                ExtraWhere =
                    " AND M.CODE LIKE '" + this.ItemCodeTextBox.Text.Replace("*", "%") + "'";
            }
              
          
            AuthenticateWebServices();
            AuthenticateWebServices2();
            String sql =

                " 	SELECT                                   " +
                " 	 M.MTRL AS itemId                        " +
                " 	,M.CODE AS itemCode                      " +
                " 	,M.PRICER AS retailPrice                 " +

                " 		,(M.PRICER - (ISNULL((SELECT TOP 1 A.FLD01                                                                  " +
                " 		     FROM PRCRDATA A                                                                                        " +
                " 		    WHERE A.COMPANY = M.COMPANY                                                                             " +
                " 		        AND A.SODTYPE = 13                                                                                  " +
                " 		        AND A.SOTYPE = 1                                                                                    " +
                " 		                  AND GETDATE() >= A.FROMDATE                                                               " +
                " 		                  AND GETDATE() <= A.FINALDATE                                                              " +
                " 		                  AND DIM1 = M.MTRL                                                                         " +
                " 		        AND A.PRCRULE IN (3000,4000,5000) ORDER BY A.FROMDATE DESC ),0))) AS specialPrice                   " +


                                                                                                            
                "  	,(SELECT TOP 1  convert(varchar, A.FROMDATE, 120)                                        "+
               	"    FROM PRCRDATA A                                                                         "+           
               	"   WHERE A.COMPANY = M.COMPANY                                                              "+           
               	"       AND A.SODTYPE = 13                                                                   "+           
               	"       AND A.SOTYPE = 1                                                                     "+           
               	"                 AND GETDATE() >= A.FROMDATE                                                "+           
               	"                 AND GETDATE() <= A.FINALDATE                                               "+           
               	"                 AND DIM1 = M.MTRL                                                          "+           
               	"       AND A.PRCRULE IN (3000,4000,5000) ORDER BY A.FROMDATE DESC ) AS fromDate             "+
                                                                                                            
                "  	,(SELECT TOP 1     convert(varchar, A.FINALDATE, 120)                                    " +
                "    FROM PRCRDATA A                                                                         " +
                "   WHERE A.COMPANY = M.COMPANY                                                              " +
                "       AND A.SODTYPE = 13                                                                   " +
                "       AND A.SOTYPE = 1                                                                     " +
                "                 AND GETDATE() >= A.FROMDATE                                                " +
                "                 AND GETDATE() <= A.FINALDATE                                               " +
                "                 AND DIM1 = M.MTRL                                                          " +
                "       AND A.PRCRULE IN (3000,4000,5000) ORDER BY A.FROMDATE DESC ) AS finalDate            " +

                " 		,FLOOR((" + this.GBPTextBox.Text + "*(m.PRICER - (ISNULL((SELECT TOP 1 A.FLD01                                                                  " +
                " 		     FROM PRCRDATA A                                                                                        " +
                " 		    WHERE A.COMPANY = M.COMPANY                                                                             " +
                " 		        AND A.SODTYPE = 13                                                                                  " +
                " 		        AND A.SOTYPE = 1                                                                                    " +
                " 		                  AND GETDATE() >= A.FROMDATE                                                               " +
                " 		                  AND GETDATE() <= A.FINALDATE                                                              " +
                " 		                  AND DIM1 = M.MTRL                                                                         " +
                " 		        AND A.PRCRULE IN (3000,4000,5000) ORDER BY A.FROMDATE DESC ),0))))) AS specialPriceUK                   " +

                " 		,FLOOR((m.PRICER*" + this.GBPTextBox.Text + "))  AS retailPriceUK                                                                        " +

                " 	FROM MTRL M                              " +
                " 	WHERE M.SODTYPE = 51         AND M.COMPANY = " + XSupport.ConnectionInfo.CompanyId.ToString()+
                ExtraWhere;


            XTable results = XSupport.GetSQLDataSet(sql);
            List<PopulateItem> itemsAll = new List<PopulateItem>();
          
            if (results.Count > 0)
            {
                formProgressBar.Minimum = 0;
                formProgressBar.Maximum = (results.Count * 2) + 1;
                formProgressBar.Value = 1;

            //    XModule SALDOCMODULE = XSupport.CreateModule("SALDOC");
            //    XTable SALDOCTBL = SALDOCMODULE.GetTable("SALDOC");
            //    XTable ITELINESTBL = SALDOCMODULE.GetTable("ITELINES");
            //    SALDOCMODULE.InsertData();
            //    SALDOCTBL.Current.Insert();
            //         SALDOCTBL.Current["SERIES"] = 7021;
            //    SALDOCTBL.Current.Post();

            //    ITELINESTBL.Current.Insert();

            //    SALDOCTBL.Current.Post();
                


                for (int i = 0; i < results.Count; i++)
                {
                    formProgressBar.Increment(1);
                    PopulateItem item = new PopulateItem();

                    item.itemId = results[i, "itemId"].ToString();
                    item.itemCode = results[i, "itemCode"].ToString();

                    item.retailPrice = results[i, "retailPrice"].ToString();
                    item.specialPrice = results[i, "specialPrice"].ToString();

                    item.retailPriceUK = results[i,  "retailPriceUK"].ToString();
                    item.specialPriceUK = results[i, "specialPriceUK"].ToString();

                    string[] FROMdATE = results[i, "fromDate"].ToString().Split(' ');
                    item.fromDate = FROMdATE[0];

                    string[] FINALdATE = results[i, "finalDate"].ToString().Split(' ');
                    item.finalDate = FINALdATE[0];


                    // SALDOCTBL.Current["X_CODE"] = item.itemCode;
                    // SALDOCTBL.Current["QTY1"] = (float) 1;

                  //  item.specialPrice = SALDOCTBL.Current["DISC1VAL"].ToString();
                    
                    itemsAll.Add(item);
                }

                int countProducts = 0;
                List<PopulateItem> items = new List<PopulateItem>();

                foreach (PopulateItem item in itemsAll)
                {
                    formProgressBar.Increment(1);
                    PopulateItem pitem = new PopulateItem();
                   // pitem.itemSubCode = item.itemSubCode;
                   // pitem.itemBalance = item.itemBalance;

                    pitem.itemId = item.itemId;
                    pitem.itemCode = item.itemCode;
                    pitem.retailPrice = item.retailPrice;
                    pitem.specialPrice = item.specialPrice;
                    pitem.fromDate = item.fromDate;
                    pitem.finalDate = item.finalDate;

                    items.Add(pitem);

                    if (countProducts >= 200)
                    {
                        try
                        {
                            String result = "";
                            result = magento.updateProductPrices(
                                 Properties.Settings1.Default["Url"].ToString() + "/rest/all/V1/softone/updateprices/",
                                 items,1
                                 );

                            result = magento2.updateProductPrices(
                                 Properties.Settings1.Default["Url2"].ToString() + "/rest/all/V1/softone/updateprices/",
                                 items,2
                                 );

                        }
                        catch
                        (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        countProducts = 0;
                        items = new List<PopulateItem>();
                    }

                    countProducts++;
                }

                if (items.Count > 0)
                {
                    try
                    {
                        String result = "";
                        result = magento.updateProductPrices(
                             Properties.Settings1.Default["Url"].ToString() + "/rest/all/V1/softone/updateprices/",
                             items,1
                             );

                        result = magento2.updateProductPrices(
                             Properties.Settings1.Default["Url2"].ToString() + "/rest/all/V1/softone/updateprices/",
                             items,2
                             );

                    }
                    catch
                   (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    countProducts = 0;
                    items = new List<PopulateItem>();
                }




            }

            formProgressBar.Value = 0;
            MessageBox.Show("Η διαδικασία ολοκληρώθηκε.");

        }







        private void RunUpdateQtys(int daysBefore)
        {

            String ExtraWhere = "";

            if (daysBefore > 0)
                ExtraWhere =
                    " AND itemId IN(SELECT ML.MTRL FROM MTRLINES ML INNER JOIN FINDOC F ON F.FINDOC = ML.FINDOC WHERE F.UPDDATE > DATEADD(day, -" + daysBefore.ToString() + ", GETDATE())) "; 
                   //    " 		AND m.MTRL IN (SELECT ML.MTRL FROM MTRLINES ML INNER JOIN FINDOC F ON F.FINDOC = ML.FINDOC WHERE F.TRNDATE >  DATEADD(day, -" + daysBefore.ToString() + ", GETDATE()) )  ";


            AuthenticateWebServices();
            AuthenticateWebServices2();

            String sql =

                " SELECT *                                                                                                                                                     " +
                " FROM (                                                                                                                                                       " +
                " 	SELECT                                                                                                                                                     " +
                " 	 M.MTRL AS itemId                                                                                                                                          " +
                " 	,m.code AS itemCode                                                                                                                                        " +
                " 	,MS.CODE AS itemSubCode                                                                                                                                    " +
                " 	,SUM(ISNULL(cf.OPNIMPQTY1, 0) + ISNULL(cf.IMPQTY1, 0) - ISNULL(cf.EXPQTY1, 0)) AS itemBalance                                                              " +
                " 	FROM MTRL m                                                                                                                                                " +
                " 	LEFT OUTER JOIN CDIMFINDATA cf ON cf.mtrl = m.mtrl                                                                                                         " +
                " 		AND cf.fiscprd = " + DateTime.Today.Year.ToString() +
                " 		AND CF.COMPANY = M.COMPANY                                                                                                                             " +
                " 		INNER JOIN MTRSUBSTITUTE MS ON                                                                                                                         " +
                " 			MS.COMPANY = CF.COMPANY                                                                                                                            " +
                " 			AND MS.MTRL = CF.MTRL                                                                                                                              " +
                " 			AND MS.MTRDIM1 = CF.CDIMLINES1                                                                                                                     " +
                " 			AND MS.MTRDIM2 = CF.CDIMLINES2                                                                                                                     " +
                " 			AND MS.MTRDIM3 = CF.CDIMLINES3                                                                                                                     " +
                " 		                                                                                                                                                       " +
                " 	LEFT OUTER JOIN WHOUSE W ON W.WHOUSE = CF.WHOUSE                                                                                                           " +
                " 		AND W.COMPANY = CF.COMPANY AND W.FAX = 'ONWEB'                                                                                                         " +
                " 	WHERE M.SODTYPE = 51                                                                                                                                       " +
                "    AND MS.ISACTIVE =1                                                                                                                             " +
                "    AND W.FAX = 'ONWEB'  "+
                "    AND M.CCCINT01 = 1 " +
                " 	GROUP BY m.mtrl                                                                                                                                            " +
                " 		,m.code                                                                                                                                                " +
                " 		,m.NAME                                                                                                                                                " +
                " 		,m.isactive                                                                                                                                            " +
                " 		,m.PRICER                                                                                                                                              " +
                " 		,m.CCCVARCHAR01                                                                                                                                        " +
                " 		,m.CCCINT01                                                                                                                                            " +
                "       ,MS.CODE                                                                                                                                               " +
                " 	) A                                                                                                                                                        " +
                " WHERE 1 = 1 " +// itemId IN (SELECT ML.MTRL FROM MTRLINES ML INNER JOIN FINDOC F ON F.FINDOC = ML.FINDOC WHERE F.UPDDATE >  DATEADD(day, -1, GETDATE()) )                ";
                ExtraWhere;

            /*

            "  SELECT * " +
                          "  FROM ( " +
                          "  	SELECT m.code AS itemCode " +
                          "  		,SUM(ISNULL(cf.OPNIMPQTY1, 0) + ISNULL(cf.IMPQTY1, 0) - ISNULL(cf.EXPQTY1, 0)) AS itemBalance " +
                          "  		,ISNULL(( " +
                          "  				SELECT CODE " +
                          "  				FROM ( " +
                          "  					SELECT ROW_NUMBER() OVER ( " +
                          "  							ORDER BY NAME " +
                          "  							) AS ROWNUMBER " +
                          "  						,CODE " +
                          "  					FROM MTRSUBSTITUTE ms " +
                          "  					WHERE ms.mtrl = m.mtrl " +
                          "  						AND ms.mtrdim1 = cl1.cdimlines " +
                          "  						AND ms.mtrdim2 = cl2.cdimlines " +
                          "  						AND ms.mtrdim3 = cl3.cdimlines " +
                          "  						AND ms.isactive = 1 " +
                          " 					) CODE1 " +
                          " 				WHERE ROWNUMBER = 1 " +
                          " 				), '') AS itemSubCode " +
                          " 		,ISNULL(( " +
                          " 				SELECT CODE " +
                          " 				FROM ( " +
                          " 					SELECT ROW_NUMBER() OVER ( " +
                          " 							ORDER BY NAME " +
                          " 							) AS ROWNUMBER " +
                          " 						,CODE " +
                          " 					FROM MTRSUBSTITUTE ms " +
                          " 					WHERE ms.mtrl = m.mtrl " +
                          " 						AND ms.mtrdim1 = cl1.cdimlines " +
                          " 						AND ms.mtrdim2 = cl2.cdimlines " +
                          " 						AND ms.mtrdim3 = cl3.cdimlines " +
                          " 						AND ms.isactive = 1 " +
                          " 					) CODE1 " +
                          " 				WHERE ROWNUMBER = 2 " +
                          " 				), '') AS itemSubCode1 " +
                          " 		,ISNULL(( " +
                          " 				SELECT CODE " +
                          " 				FROM ( " +
                          " 					SELECT ROW_NUMBER() OVER ( " +
                          " 							ORDER BY NAME " +
                          " 							) AS ROWNUMBER " +
                          " 						,CODE " +
                          " 					FROM MTRSUBSTITUTE ms " +
                          " 					WHERE ms.mtrl = m.mtrl " +
                          " 						AND ms.mtrdim1 = cl1.cdimlines " +
                          " 						AND ms.mtrdim2 = cl2.cdimlines " +
                          " 						AND ms.mtrdim3 = cl3.cdimlines " +
                          " 						AND ms.isactive = 1 " +
                          " 					) CODE1 " +
                          " 				WHERE ROWNUMBER = 3 " +
                          " 				), '') AS itemSubCode2 " +
                          " 		,ISNULL(( " +
                          " 				SELECT CODE " +
                          " 				FROM ( " +
                          " 					SELECT ROW_NUMBER() OVER ( " +
                          " 							ORDER BY NAME " +
                          " 							) AS ROWNUMBER " +
                          " 						,CODE " +
                          " 					FROM MTRSUBSTITUTE ms " +
                          " 					WHERE ms.mtrl = m.mtrl " +
                          " 						AND ms.mtrdim1 = cl1.cdimlines " +
                          " 						AND ms.mtrdim2 = cl2.cdimlines " +
                          " 						AND ms.mtrdim3 = cl3.cdimlines " +
                          " 						AND ms.isactive = 1 " +
                          " 					) CODE1 " +
                          " 				WHERE ROWNUMBER = 4 " +
                          " 				), '') AS itemSubCode3 " +
                          " 		,ISNULL(( " +
                          " 				SELECT CODE " +
                          " 				FROM ( " +
                          " 					SELECT ROW_NUMBER() OVER ( " +
                          " 							ORDER BY NAME " +
                          " 							) AS ROWNUMBER " +
                          " 						,CODE " +
                          " 					FROM MTRSUBSTITUTE ms " +
                          " 					WHERE ms.mtrl = m.mtrl " +
                          " 						AND ms.mtrdim1 = cl1.cdimlines " +
                          " 						AND ms.mtrdim2 = cl2.cdimlines " +
                          " 						AND ms.mtrdim3 = cl3.cdimlines " +
                          " 						AND ms.isactive = 1 " +
                          " 					) CODE1 " +
                          " 				WHERE ROWNUMBER = 5 " +
                          " 				), '') AS itemSubCode4 " +
                          " 	FROM MTRL m " +
                          " 	INNER JOIN CDIMFINDATA cf ON cf.mtrl = m.mtrl " +
                          " 		AND cf.fiscprd = " + DateTime.Today.Year.ToString() +
                          " 		AND CF.COMPANY = M.COMPANY " +
                          " 	LEFT JOIN cdimlines cl1 ON cl1.cdim = cf.cdim1 " +
                          " 		AND cl1.cdimlines = cf.cdimlines1 " +
                          " 		AND cl1.COMPANY = M.COMPANY " +
                          " 	LEFT JOIN cdimlines cl2 ON cl2.cdim = cf.cdim2 " +
                          " 		AND cl2.cdimlines = cf.cdimlines2 " +
                          " 		AND cl2.COMPANY = M.COMPANY " +
                          " 	LEFT JOIN cdimlines cl3 ON cl3.cdim = cf.cdim3 " +
                          " 		AND cl3.cdimlines = cf.cdimlines3 " +
                          " 		AND cl3.COMPANY = M.COMPANY " +
                          " 	LEFT JOIN MTRGROUP mr ON mr.MTRGROUP = m.MTRGROUP " +
                          " 		AND mr.COMPANY = M.COMPANY " +
                          " 	LEFT JOIN MTRMARK mm ON mm.MTRMARK = m.MTRMARK " +
                          " 		AND mm.COMPANY = M.COMPANY " +
                          " 	INNER JOIN WHOUSE W ON W.WHOUSE = CF.WHOUSE " +
                          " 		AND W.COMPANY = CF.COMPANY " +
                          " 	WHERE M.SODTYPE = 51 " +
                          " 		AND W.FAX = 'ONWEB' " +
                          " 		AND m.CDIMCATEG1 IS NOT NULL " +
                          " 		AND m.CDIMCATEG2 IS NOT NULL " +
                          " 		AND m.CDIMCATEG3 IS NOT NULL " +
                          ExtraWhere +
                          // " 		AND m.MTRL IN (SELECT ML.MTRL FROM MTRLINES ML INNER JOIN FINDOC F ON F.FINDOC = ML.FINDOC WHERE F.TRNDATE >  DATEADD(day, -1, GETDATE()) )  " +
                          " 	GROUP BY m.mtrl " +
                          " 		,m.code " +
                          " 		,cl1.NAME " +
                          " 		,cl2.NAME " +
                          " 		,cl3.NAME " +
                          " 		,m.NAME " +
                          " 		,m.isactive " +
                          " 		,mr.code " +
                          " 		,mr.NAME " +
                          " 		,mm.code " +
                          " 		,mm.NAME " +
                          " 		,m.PRICER " +
                          " 		,cl1.cdimlines " +
                          " 		,cl2.cdimlines " +
                          " 		,cl3.cdimlines " +
                          " 		,m.CCCVARCHAR01 " +
                          " 		,m.CCCINT01 " +
                          " 	) A " +
                          " WHERE 1 = 1 "; */


            XTable results = XSupport.GetSQLDataSet(sql);
            List<QTYProduct> qtyProdcts = new List<QTYProduct>();
         //   return;
            if (results.Count > 0)
            {
                formProgressBar.Minimum = 0;
                formProgressBar.Maximum = (results.Count*2) + 1;
                formProgressBar.Value = 1;


                for (int i = 0; i < results.Count; i++)
                {
                    formProgressBar.Increment(1);
                    QTYProduct qtyProdct = new QTYProduct();

                    qtyProdct.itemCode = results[i, "itemCode"].ToString();
                    qtyProdct.itemBalance = results[i, "itemBalance"].ToString();
                    qtyProdct.itemSubCode = results[i, "itemSubCode"].ToString();
                   // qtyProdct.itemSubCode1 = results[i, "itemSubCode1"].ToString();
                   // qtyProdct.itemSubCode2 = results[i, "itemSubCode2"].ToString();
                   // qtyProdct.itemSubCode3 = results[i, "itemSubCode3"].ToString();
                   // qtyProdct.itemSubCode4 = results[i, "itemSubCode4"].ToString();

                    qtyProdcts.Add(qtyProdct);
                }

                int countProducts = 0;
                List<PopulateItem> items = new List<PopulateItem>();

                foreach (QTYProduct qtyProdct in qtyProdcts)
                {
                    formProgressBar.Increment(1);
                    PopulateItem pitem = new PopulateItem();
                    pitem.itemSubCode = qtyProdct.itemSubCode;
                    pitem.itemBalance = qtyProdct.itemBalance;
                    items.Add(pitem);

                    if (countProducts >= 200)
                    {
                                   try
                                   {
                                    String result = "";
                                    result = magento.updateProductQtys(
                                         Properties.Settings1.Default["Url"].ToString() + "/rest/all/V1/softone/updatestocks/",
                                         items,1
                                         );

                                    result = magento2.updateProductQtys(
                                     Properties.Settings1.Default["Url2"].ToString() + "/rest/all/V1/softone/updatestocks/",
                                     items,2
                                     );


                                    }
                                    catch
                                   (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }

                        countProducts = 0;
                        items = new List<PopulateItem>();
                      }

                    countProducts++;
                    }

                if (items.Count > 0)
                {
                    try
                    {
                        String result = "";
                        result = magento.updateProductQtys(
                             Properties.Settings1.Default["Url"].ToString() + "/rest/all/V1/softone/updatestocks/",
                             items,1
                             );

                        result = magento2.updateProductQtys(
                            Properties.Settings1.Default["Url2"].ToString() + "/rest/all/V1/softone/updatestocks/",
                            items,2
                            );

                    }
                    catch
                   (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    countProducts = 0;
                    items = new List<PopulateItem>();
                }




            }

            formProgressBar.Value = 0;
            MessageBox.Show("Η διαδικασία ολοκληρώθηκε.");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RunUpdateQtys(0);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings1.Default["Url"] = this.UrlTextBox.Text;
            Properties.Settings1.Default["Username"] = this.UserTextBox.Text;
            Properties.Settings1.Default["Password"] = this.PasswordTextBox.Text;

            if (this.WebSite1CheckBox.Checked) Properties.Settings1.Default["Web1"] = "1";
            if (!this.WebSite1CheckBox.Checked) Properties.Settings1.Default["Web1"] = "0";


            if (this.WebSite1CheckBox.Checked) Magento.Web1 = true;
            if (!this.WebSite1CheckBox.Checked) Magento.Web1 = false;

            Properties.Settings1.Default.Save();

            MessageBox.Show("Στοιχεία αποθηκεύτηκαν");
        }

        private void UpdatePricesButton_Click(object sender, EventArgs e)
        {
            RunUpdatePrices();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Properties.Settings1.Default["Url2"] = this.Url2TextBox.Text;
            Properties.Settings1.Default["Username2"] = this.User2TextBox.Text;
            Properties.Settings1.Default["Password2"] = this.Password2TextBox.Text;

            if (this.WebSite2CheckBox.Checked) Properties.Settings1.Default["Web2"] = "1";
            if (!this.WebSite2CheckBox.Checked) Properties.Settings1.Default["Web2"] = "0";

            if (this.WebSite2CheckBox.Checked) Magento.Web2 = true;
            if (!this.WebSite2CheckBox.Checked) Magento.Web2 = false;

       
            Properties.Settings1.Default.Save();

            MessageBox.Show("Στοιχεία αποθηκεύτηκαν");

        }

        private void WebSyncFormQtys_Load(object sender, EventArgs e)
        {
            this.GBPTextBox.Text = GBPCURRENCY.GetCurrencyFromWeb().ToString().Replace(",", ".");
        }
    }
}

