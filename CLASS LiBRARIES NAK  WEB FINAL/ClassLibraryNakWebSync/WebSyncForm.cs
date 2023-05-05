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
    
    public partial class WebSyncForm : Form
    {

        XSupport XSupport = null;
        private Magento magento = new Magento();


        public WebSyncForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FillItemsDatagridView();

            if (this.ItemDataGridView.Rows.Count > 0)
            {
                this.UpdateProductsButton.Enabled = true;
            }
            else this.UpdateProductsButton.Enabled = false;


        }

        public void FillItemsDatagridView()
        {
            String SelectionDateStr = this.SelectionDateTimePicker.Value.Date.ToString("yyyyMMdd");
            String whereByDate = " AND M.UPDDATE > '"+ SelectionDateStr + "'";

            if(this.CodeFilterTextBox.Text != "") whereByDate = " AND M.CODE LIKE '" + this.CodeFilterTextBox.Text.Replace("*","%") + "'";


            String sql =
                " SELECT *                                                                                                            " +
                " FROM (                                                                                                              " +
                " 	SELECT m.mtrl AS itemId                                                                                         " +
                " 		,m.code AS itemCode                                                                                         " +
                " 		,m.NAME AS itemName                                                                                         " +
                " 		,m.isactive AS active                                                                                       " +
                " 		,mr.code AS groupCode                                                                                       " +
                " 		,mr.NAME AS groupName                                                                                       " +
                " 		,mm.code AS markCode                                                                                        " +
                " 		,mm.CCCNAMEWEB AS markName                                                                                  " +
                " 		,m.PRICER AS retailPrice                                                                                    " +
                " 		,cl1.NAME AS colorName                                                                                      " +
                " 		,cl2.NAME AS sizeName                                                                                       " +
                " 		,cl3.NAME AS seasonName                                                                                     " +
                " 		,(	SELECT SUM(ISNULL(cf.OPNIMPQTY1, 0) + ISNULL(cf.IMPQTY1, 0) - ISNULL(cf.EXPQTY1, 0))                    " +
                " 			FROM CDIMFINDATA cf                                                                                     " +
                " 			INNER JOIN WHOUSE W ON W.WHOUSE = CF.WHOUSE AND W.COMPANY = CF.COMPANY AND W.FAX= 'ONWEB'               " +
                " 			WHERE CF.MTRL = M.MTRL                                                                                  " +
                " 			AND CF.FISCPRD =  "+DateTime.Now.Year.ToString()+"                                                                                  " +
                " 			AND CF.COMPANY = M.COMPANY                                                                              " +
                " 			AND cf.cdim1 = cl1.cdim                                                                                 " +
                " 			AND cf.cdimlines1 = cl1.cdimlines                                                                       " +
                " 			AND cf.cdim2 = cl2.cdim                                                                                 " +
                " 			AND cf.cdimlines2 = cl2.cdimlines                                                                       " +
                " 			AND W.FAX = 'ONWEB') AS itemBalance                                                                     " +
                " 		,ISNULL((                                                                                                   " +
                " 				SELECT CODE                                                                                         " +
                " 				FROM (                                                                                              " +
                " 					SELECT ROW_NUMBER() OVER (                                                                      " +
                " 							ORDER BY NAME                                                                           " +
                " 							) AS ROWNUMBER                                                                          " +
                " 						,CODE                                                                                       " +
                " 					FROM MTRSUBSTITUTE ms                                                                           " +
                " 					WHERE ms.mtrl = m.mtrl                                                                          " +
                " 						AND ms.mtrdim1 = cl1.cdimlines                                                              " +
                " 						AND ms.mtrdim2 = cl2.cdimlines                                                              " +
                " 						AND ms.mtrdim3 = cl3.cdimlines                                                              " +
                " 						AND ms.isactive = 1                                                                         " +
                " 					) CODE1                                                                                         " +
                " 				WHERE ROWNUMBER = 1                                                                                 " +
                " 				), '') AS itemSubCode                                                                               " +
                " 		,ISNULL((                                                                                                   " +
                " 				SELECT CODE                                                                                         " +
                " 				FROM (                                                                                              " +
                " 					SELECT ROW_NUMBER() OVER (                                                                      " +
                " 							ORDER BY NAME                                                                           " +
                " 							) AS ROWNUMBER                                                                          " +
                " 						,CODE                                                                                       " +
                " 					FROM MTRSUBSTITUTE ms                                                                           " +
                " 					WHERE ms.mtrl = m.mtrl                                                                          " +
                " 						AND ms.mtrdim1 = cl1.cdimlines                                                              " +
                " 						AND ms.mtrdim2 = cl2.cdimlines                                                              " +
                " 						AND ms.mtrdim3 = cl3.cdimlines                                                              " +
                " 						AND ms.isactive = 1                                                                         " +
                " 					) CODE1                                                                                         " +
                " 				WHERE ROWNUMBER = 2                                                                                 " +
                " 				), '') AS itemSubCode1                                                                              " +
                " 		,ISNULL((                                                                                                   " +
                " 				SELECT CODE                                                                                         " +
                " 				FROM (                                                                                              " +
                " 					SELECT ROW_NUMBER() OVER (                                                                      " +
                " 							ORDER BY NAME                                                                           " +
                " 							) AS ROWNUMBER                                                                          " +
                " 						,CODE                                                                                       " +
                " 					FROM MTRSUBSTITUTE ms                                                                           " +
                " 					WHERE ms.mtrl = m.mtrl                                                                          " +
                " 						AND ms.mtrdim1 = cl1.cdimlines                                                              " +
                " 						AND ms.mtrdim2 = cl2.cdimlines                                                              " +
                " 						AND ms.mtrdim3 = cl3.cdimlines                                                              " +
                " 						AND ms.isactive = 1                                                                         " +
                " 					) CODE1                                                                                         " +
                " 				WHERE ROWNUMBER = 3                                                                                 " +
                " 				), '') AS itemSubCode2                                                                              " +
                " 		,ISNULL((                                                                                                   " +
                " 				SELECT CODE                                                                                         " +
                " 				FROM (                                                                                              " +
                " 					SELECT ROW_NUMBER() OVER (                                                                      " +
                " 							ORDER BY NAME                                                                           " +
                " 							) AS ROWNUMBER                                                                          " +
                " 						,CODE                                                                                       " +
                " 					FROM MTRSUBSTITUTE ms                                                                           " +
                " 					WHERE ms.mtrl = m.mtrl                                                                          " +
                " 						AND ms.mtrdim1 = cl1.cdimlines                                                              " +
                " 						AND ms.mtrdim2 = cl2.cdimlines                                                              " +
                " 						AND ms.mtrdim3 = cl3.cdimlines                                                              " +
                " 						AND ms.isactive = 1                                                                         " +
                " 					) CODE1                                                                                         " +
                " 				WHERE ROWNUMBER = 4                                                                                 " +
                " 				), '') AS itemSubCode3                                                                              " +
                " 		,ISNULL((                                                                                                   " +
                " 				SELECT CODE                                                                                         " +
                " 				FROM (                                                                                              " +
                " 					SELECT ROW_NUMBER() OVER (                                                                      " +
                " 							ORDER BY NAME                                                                           " +
                " 							) AS ROWNUMBER                                                                          " +
                " 						,CODE                                                                                       " +
                " 					FROM MTRSUBSTITUTE ms                                                                           " +
                " 					WHERE ms.mtrl = m.mtrl                                                                          " +
                " 						AND ms.mtrdim1 = cl1.cdimlines                                                              " +
                " 						AND ms.mtrdim2 = cl2.cdimlines                                                              " +
                " 						AND ms.mtrdim3 = cl3.cdimlines                                                              " +
                " 						AND ms.isactive = 1                                                                         " +
                " 					) CODE1                                                                                         " +
                " 				WHERE ROWNUMBER = 5                                                                                 " +
                " 				), '') AS itemSubCode4                                                                              " +
                " 		,ISNULL((                                                                                                   " +
                " 				SELECT CCCDIM1STR                                                                                   " +
                " 				FROM (                                                                                              " +
                " 					SELECT ROW_NUMBER() OVER (                                                                      " +
                " 							ORDER BY NAME                                                                           " +
                " 							) AS ROWNUMBER                                                                          " +
                " 						,CCCDIM1STR                                                                                 " +
                " 					FROM MTRSUBSTITUTE ms                                                                           " +
                " 					WHERE ms.mtrl = m.mtrl                                                                          " +
                " 						AND ms.mtrdim1 = cl1.cdimlines                                                              " +
                " 						AND ms.mtrdim2 = cl2.cdimlines                                                              " +
                " 						AND ms.mtrdim3 = cl3.cdimlines                                                              " +
                " 						AND ms.isactive = 1                                                                         " +
                " 					) CODE1                                                                                         " +
                " 				WHERE ROWNUMBER = 1                                                                                 " +
                " 				), '') AS colorName2                                                                                " +
                " 		,ISNULL((                                                                                                   " +
                " 				SELECT CCCVARCHARH03                                                                                " +
                " 				FROM (                                                                                              " +
                " 					SELECT ROW_NUMBER() OVER (                                                                      " +
                " 							ORDER BY NAME                                                                           " +
                " 							) AS ROWNUMBER                                                                          " +
                " 						,CCCVARCHARH03                                                                              " +
                " 					FROM MTRSUBSTITUTE ms                                                                           " +
                " 					WHERE ms.mtrl = m.mtrl                                                                          " +
                " 						AND ms.mtrdim1 = cl1.cdimlines                                                              " +
                " 						AND ms.mtrdim2 = cl2.cdimlines                                                              " +
                " 						AND ms.mtrdim3 = cl3.cdimlines                                                              " +
                " 						AND ms.isactive = 1                                                                         " +
                " 					) CODE1                                                                                         " +
                " 				WHERE ROWNUMBER = 1                                                                                 " +
                " 				), '') AS sizeWebEU                                                                                 " +
                " 		,ISNULL((                                                                                                   " +
                " 				SELECT CCCVARCHARH01                                                                                " +
                " 				FROM (                                                                                              " +
                " 					SELECT ROW_NUMBER() OVER (                                                                      " +
                " 							ORDER BY NAME                                                                           " +
                " 							) AS ROWNUMBER                                                                          " +
                " 						,CCCVARCHARH01                                                                              " +
                " 					FROM MTRSUBSTITUTE ms                                                                           " +
                " 					WHERE ms.mtrl = m.mtrl                                                                          " +
                " 						AND ms.mtrdim1 = cl1.cdimlines                                                              " +
                " 						AND ms.mtrdim2 = cl2.cdimlines                                                              " +
                " 						AND ms.mtrdim3 = cl3.cdimlines                                                              " +
                " 						AND ms.isactive = 1                                                                         " +
                " 					) CODE1                                                                                         " +
                " 				WHERE ROWNUMBER = 1                                                                                 " +
                " 				), '') AS sizeWebUK                                                                                 " +
                " 		,ISNULL((                                                                                                   " +
                " 				SELECT CCCVARCHARH02                                                                                " +
                " 				FROM (                                                                                              " +
                " 					SELECT ROW_NUMBER() OVER (                                                                      " +
                " 							ORDER BY NAME                                                                           " +
                " 							) AS ROWNUMBER                                                                          " +
                " 						,CCCVARCHARH02                                                                              " +
                " 					FROM MTRSUBSTITUTE ms                                                                           " +
                " 					WHERE ms.mtrl = m.mtrl                                                                          " +
                " 						AND ms.mtrdim1 = cl1.cdimlines                                                              " +
                " 						AND ms.mtrdim2 = cl2.cdimlines                                                              " +
                " 						AND ms.mtrdim3 = cl3.cdimlines                                                              " +
                " 						AND ms.isactive = 1                                                                         " +
                " 					) CODE1                                                                                         " +
                " 				WHERE ROWNUMBER = 1                                                                                 " +
                " 				), '') AS sizeWebUS                                                                                 " +
                " 		,ISNULL((                                                                                                   " +
                " 				SELECT CCCVARCHARH04                                                                                " +
                " 				FROM (                                                                                              " +
                " 					SELECT ROW_NUMBER() OVER (                                                                      " +
                " 							ORDER BY NAME                                                                           " +
                " 							) AS ROWNUMBER                                                                          " +
                " 						,CCCVARCHARH04                                                                              " +
                " 					FROM MTRSUBSTITUTE ms                                                                           " +
                " 					WHERE ms.mtrl = m.mtrl                                                                          " +
                " 						AND ms.mtrdim1 = cl1.cdimlines                                                              " +
                " 						AND ms.mtrdim2 = cl2.cdimlines                                                              " +
                " 						AND ms.mtrdim3 = cl3.cdimlines                                                              " +
                " 						AND ms.isactive = 1                                                                         " +
                " 					) CODE1                                                                                         " +
                " 				WHERE ROWNUMBER = 1                                                                                 " +
                " 				), '') AS sizeWebFR                                                                                 " +
                " 		,ISNULL((                                                                                                   " +
                " 				SELECT CCCVARCHARH05                                                                                " +
                " 				FROM (                                                                                              " +
                " 					SELECT ROW_NUMBER() OVER (                                                                      " +
                " 							ORDER BY NAME                                                                           " +
                " 							) AS ROWNUMBER                                                                          " +
                " 						,CCCVARCHARH05                                                                              " +
                " 					FROM MTRSUBSTITUTE ms                                                                           " +
                " 					WHERE ms.mtrl = m.mtrl                                                                          " +
                " 						AND ms.mtrdim1 = cl1.cdimlines                                                              " +
                " 						AND ms.mtrdim2 = cl2.cdimlines                                                              " +
                " 						AND ms.mtrdim3 = cl3.cdimlines                                                              " +
                " 						AND ms.isactive = 1                                                                         " +
                " 					) CODE1                                                                                         " +
                " 				WHERE ROWNUMBER = 1                                                                                 " +
                " 				), '') AS sizeWebJPN                                                                                " +
                " 		,m.CCCVARCHAR01 AS itemCode2                                                                                " +
                " 		,m.CCCINT01 AS OnWeb                                                                                        " +

                " 		,(m.PRICER - (ISNULL((SELECT TOP 1 A.FLD01                                                                  " +
                " 		     FROM PRCRDATA A                                                                                        " +
                " 		    WHERE A.COMPANY = M.COMPANY                                                                             " +
	            " 		        AND A.SODTYPE = 13                                                                                  " +
	            " 		        AND A.SOTYPE = 1                                                                                    " +
                " 		                  AND GETDATE() >= A.FROMDATE                                                               " +
                " 		                  AND GETDATE() <= A.FINALDATE                                                              " +
                " 		                  AND DIM1 = M.MTRL                                                                         " +
                " 		        AND A.PRCRULE IN (3000,4000,5000) ORDER BY A.FROMDATE DESC ),0))) AS specialPrice                   " +
                
                " 	FROM MTRL m                                                                                                     " +
                " 	                                                                                                                " +
                " 	INNER JOIN MTRSUBSTITUTE MS ON MS.MTRL = M.MTRL                                                                 " +
                " 	                                                                                                                " +
                "                                                                                                                   " +
                " 	LEFT JOIN cdimlines cl1 ON cl1.cdim =  M.CDIM1                                                                  " +
                " 		AND cl1.cdimlines = MS.MTRDIM1                                                                              " +
                " 		AND cl1.COMPANY = M.COMPANY                                                                                 " +
                " 	LEFT JOIN cdimlines cl2 ON cl2.cdim = M.CDIM2                                                                   " +
                " 		AND cl2.cdimlines = MS.MTRDIM2                                                                              " +
                " 		AND cl2.COMPANY = M.COMPANY                                                                                 " +
                " 	LEFT JOIN cdimlines cl3 ON cl3.cdim = M.CDIM3                                                                   " +
                " 		AND cl3.cdimlines = MS.MTRDIM3                                                                              " +
                " 		AND cl3.COMPANY = M.COMPANY                                                                                 " +
                " 	LEFT JOIN MTRGROUP mr ON mr.MTRGROUP = m.MTRGROUP                                                               " +
                " 		AND mr.COMPANY = M.COMPANY                                                                                  " +
                " 	LEFT JOIN MTRMARK mm ON mm.MTRMARK = m.MTRMARK                                                                  " +
                " 		AND mm.COMPANY = M.COMPANY                                                                                  " +
                " 	WHERE M.SODTYPE = 51                                                                                            " +
                " 		AND m.CDIMCATEG1 IS NOT NULL                                                                                " +
                " 		AND m.CDIMCATEG2 IS NOT NULL                                                                                " +
                " 		AND m.CDIMCATEG3 IS NOT NULL                                                                                " +
                " 		AND MS.ISACTIVE = 1                                                                                         " +

                whereByDate +
                " 	GROUP BY m.mtrl                                                                                                 " +
                " 		,m.code                                                                                                     " +
                " 		,cl1.NAME                                                                                                   " +
                " 		,cl2.NAME                                                                                                   " +
                " 		,cl3.NAME                                                                                                   " +
                " 		,m.NAME                                                                                                     " +
                " 		,m.isactive                                                                                                 " +
                " 		,mr.code                                                                                                    " +
                " 		,mr.NAME                                                                                                    " +
                " 		,mm.code                                                                                                    " +
                " 		,mm.CCCNAMEWEB                                                                                              " +
                " 		,m.PRICER                                                                                                   " +
                " 		,cl1.cdimlines                                                                                              " +
                " 		,cl2.cdimlines                                                                                              " +
                " 		,cl3.cdimlines                                                                                              " +
                " 		,m.CCCVARCHAR01                                                                                             " +
                " 		,m.CCCINT01                                                                                                 " +
                " 		,M.COMPANY                                                                                                  " +
                " 		,cl1.cdim                                                                                                   " +
                " 		,cl1.cdimlines                                                                                              " +
                " 		,cl2.cdim                                                                                                   " +
                " 		,cl2.cdimlines                                                                                              " +
                " 		,cl3.cdim                                                                                                   " +
                " 		,cl3.cdimlines                                                                                              " +
                " 	) A                                                                                                             " +
                " WHERE 1 = 1                                                                                                       " +
                " ORDER BY A.ITEMID                                                                                                 ";





/*
            sql=
            " SELECT * FROM ( " +
" SELECT m.mtrl AS itemId " +
" ,m.code AS itemCode " +
" ,m.NAME AS itemName " +
" ,m.isactive AS active " +
" ,mr.code AS groupCode " +
" ,mr.NAME AS groupName " +
" ,mm.code AS markCode " +
" ,mm.NAME AS markName " +
" ,m.PRICER AS retailPrice " +
" ,cl1.NAME AS colorName " +
" ,cl2.NAME AS sizeName " +
" ,cl3.NAME AS seasonName " +
" ,SUM(ISNULL(cf.OPNIMPQTY1, 0) + ISNULL(cf.IMPQTY1, 0) - ISNULL(cf.EXPQTY1, 0)) AS itemBalance " +
" ,ISNULL(( " +
" SELECT CODE " +
" FROM ( " +
" SELECT ROW_NUMBER() OVER ( " +
" ORDER BY NAME " +
" ) AS ROWNUMBER " +
" ,CODE " +
" FROM MTRSUBSTITUTE ms " +
" WHERE ms.mtrl=m.mtrl " +
" AND ms.mtrdim1=cl1.cdimlines " +
" AND ms.mtrdim2=cl2.cdimlines " +
" AND ms.mtrdim3=cl3.cdimlines " +
" AND ms.isactive=1 " +
" ) CODE1 " +
" WHERE ROWNUMBER=1 " +
" ), '') AS itemSubCode " +
" ,ISNULL(( " +
" SELECT CODE " +
" FROM ( " +
" SELECT ROW_NUMBER() OVER ( " +
" ORDER BY NAME " +
" ) AS ROWNUMBER " +
" ,CODE " +
" FROM MTRSUBSTITUTE ms " +
" WHERE ms.mtrl=m.mtrl " +
" AND ms.mtrdim1=cl1.cdimlines " +
" AND ms.mtrdim2=cl2.cdimlines " +
" AND ms.mtrdim3=cl3.cdimlines " +
" AND ms.isactive=1 " +
" ) CODE1 " +
" WHERE ROWNUMBER=2 " +
" ), '') AS itemSubCode1 " +
" ,ISNULL(( " +
" SELECT CODE " +
" FROM ( " +
" SELECT ROW_NUMBER() OVER ( " +
" ORDER BY NAME " +
" ) AS ROWNUMBER " +
" ,CODE " +
" FROM MTRSUBSTITUTE ms " +
" WHERE ms.mtrl=m.mtrl " +
" AND ms.mtrdim1=cl1.cdimlines " +
" AND ms.mtrdim2=cl2.cdimlines " +
" AND ms.mtrdim3=cl3.cdimlines " +
" AND ms.isactive=1 " +
" ) CODE1 " +
" WHERE ROWNUMBER=3 " +
" ), '') AS itemSubCode2 " +
" ,ISNULL(( " +
" SELECT CODE " +
" FROM ( " +
" SELECT ROW_NUMBER() OVER ( " +
" ORDER BY NAME " +
" ) AS ROWNUMBER " +
" ,CODE " +
" FROM MTRSUBSTITUTE ms " +
" WHERE ms.mtrl=m.mtrl " +
" AND ms.mtrdim1=cl1.cdimlines " +
" AND ms.mtrdim2=cl2.cdimlines " +
" AND ms.mtrdim3=cl3.cdimlines " +
" AND ms.isactive=1 " +
" ) CODE1 " +
" WHERE ROWNUMBER=4 " +
" ), '') AS itemSubCode3 " +
" ,ISNULL(( " +
" SELECT CODE " +
" FROM ( " +
" SELECT ROW_NUMBER() OVER ( " +
" ORDER BY NAME " +
" ) AS ROWNUMBER " +
" ,CODE " +
" FROM MTRSUBSTITUTE ms " +
" WHERE ms.mtrl=m.mtrl " +
" AND ms.mtrdim1=cl1.cdimlines " +
" AND ms.mtrdim2=cl2.cdimlines " +
" AND ms.mtrdim3=cl3.cdimlines " +
" AND ms.isactive=1 " +
" ) CODE1 " +
" WHERE ROWNUMBER=5 " +
" ), '') AS itemSubCode4 " +

" ,ISNULL(( " +
" SELECT CCCDIM1STR " +
" FROM ( " +
" SELECT ROW_NUMBER() OVER ( " +
" ORDER BY NAME " +
" ) AS ROWNUMBER " +
" ,CCCDIM1STR " +
" FROM MTRSUBSTITUTE ms " +
" WHERE ms.mtrl=m.mtrl " +
" AND ms.mtrdim1=cl1.cdimlines " +
" AND ms.mtrdim2=cl2.cdimlines " +
" AND ms.mtrdim3=cl3.cdimlines " +
" AND ms.isactive=1 " +
" ) CODE1 " +
" WHERE ROWNUMBER=1 " +
" ), '') AS colorName2 " +

" ,ISNULL(( " +
" SELECT CCCVARCHARH03 " +
" FROM ( " +
" SELECT ROW_NUMBER() OVER ( " +
" ORDER BY NAME " +
" ) AS ROWNUMBER " +
" ,CCCVARCHARH03 " +
" FROM MTRSUBSTITUTE ms " +
" WHERE ms.mtrl=m.mtrl " +
" AND ms.mtrdim1=cl1.cdimlines " +
" AND ms.mtrdim2=cl2.cdimlines " +
" AND ms.mtrdim3=cl3.cdimlines " +
" AND ms.isactive=1 " +
" ) CODE1 " +
" WHERE ROWNUMBER=1 " +
" ), '') AS sizeWebEU " +

" ,ISNULL(( " +
" SELECT CCCVARCHARH01 " +
" FROM ( " +
" SELECT ROW_NUMBER() OVER ( " +
" ORDER BY NAME " +
" ) AS ROWNUMBER " +
" ,CCCVARCHARH01 " +
" FROM MTRSUBSTITUTE ms " +
" WHERE ms.mtrl=m.mtrl " +
" AND ms.mtrdim1=cl1.cdimlines " +
" AND ms.mtrdim2=cl2.cdimlines " +
" AND ms.mtrdim3=cl3.cdimlines " +
" AND ms.isactive=1 " +
" ) CODE1 " +
" WHERE ROWNUMBER=1 " +
" ), '') AS sizeWebUK " +



" ,ISNULL(( " +
" SELECT CCCVARCHARH02 " +
" FROM ( " +
" SELECT ROW_NUMBER() OVER ( " +
" ORDER BY NAME " +
" ) AS ROWNUMBER " +
" ,CCCVARCHARH02 " +
" FROM MTRSUBSTITUTE ms " +
" WHERE ms.mtrl=m.mtrl " +
" AND ms.mtrdim1=cl1.cdimlines " +
" AND ms.mtrdim2=cl2.cdimlines " +
" AND ms.mtrdim3=cl3.cdimlines " +
" AND ms.isactive=1 " +
" ) CODE1 " +
" WHERE ROWNUMBER=1 " +
" ), '') AS sizeWebUS " +


" ,ISNULL(( " +
" SELECT CCCVARCHARH04 " +
" FROM ( " +
" SELECT ROW_NUMBER() OVER ( " +
" ORDER BY NAME " +
" ) AS ROWNUMBER " +
" ,CCCVARCHARH04 " +
" FROM MTRSUBSTITUTE ms " +
" WHERE ms.mtrl=m.mtrl " +
" AND ms.mtrdim1=cl1.cdimlines " +
" AND ms.mtrdim2=cl2.cdimlines " +
" AND ms.mtrdim3=cl3.cdimlines " +
" AND ms.isactive=1 " +
" ) CODE1 " +
" WHERE ROWNUMBER=1 " +
" ), '') AS sizeWebFR " +

" ,ISNULL(( " +
" SELECT CCCVARCHARH05 " +
" FROM ( " +
" SELECT ROW_NUMBER() OVER ( " +
" ORDER BY NAME " +
" ) AS ROWNUMBER " +
" ,CCCVARCHARH05 " +
" FROM MTRSUBSTITUTE ms " +
" WHERE ms.mtrl=m.mtrl " +
" AND ms.mtrdim1=cl1.cdimlines " +
" AND ms.mtrdim2=cl2.cdimlines " +
" AND ms.mtrdim3=cl3.cdimlines " +
" AND ms.isactive=1 " +
" ) CODE1 " +
" WHERE ROWNUMBER=1 " +
" ), '') AS sizeWebJPN " +

" ,m.CCCVARCHAR01 as itemCode2 " +
" ,m.CCCINT01 as OnWeb " +
" FROM MTRL m " +
" INNER JOIN CDIMFINDATA cf ON cf.mtrl=m.mtrl " +
" AND cf.fiscprd=2017 " +
" AND CF.COMPANY=M.COMPANY " +
" LEFT JOIN cdimlines cl1 ON cl1.cdim=cf.cdim1 " +
" AND cl1.cdimlines=cf.cdimlines1 " +
" AND cl1.COMPANY=M.COMPANY " +
" LEFT JOIN cdimlines cl2 ON cl2.cdim=cf.cdim2 " +
" AND cl2.cdimlines=cf.cdimlines2 " +
" AND cl2.COMPANY=M.COMPANY " +
" LEFT JOIN cdimlines cl3 ON cl3.cdim=cf.cdim3 " +
" AND cl3.cdimlines=cf.cdimlines3 " +
" AND cl3.COMPANY=M.COMPANY " +
" LEFT JOIN MTRGROUP mr ON mr.MTRGROUP=m.MTRGROUP " +
" AND mr.COMPANY=M.COMPANY " +
" LEFT JOIN MTRMARK mm ON mm.MTRMARK=m.MTRMARK " +
" AND mm.COMPANY=M.COMPANY " +
" INNER JOIN WHOUSE W ON W.WHOUSE=CF.WHOUSE AND W.COMPANY = CF.COMPANY " +
" WHERE M.SODTYPE=51 " +
" AND W.FAX='ONWEB' " +
" AND m.CDIMCATEG1 IS NOT NULL " +
" AND m.CDIMCATEG2 IS NOT NULL " +
" AND m.CDIMCATEG3 IS NOT NULL " +
whereByDate+
" GROUP BY m.mtrl, m.code,cl1.name,cl2.name,cl3.name, m.name,m.isactive,mr.code,mr.name,mm.code,mm.name,m.PRICER, cl1.cdimlines,cl2.cdimlines,cl3.cdimlines,m.CCCVARCHAR01,m.CCCINT01 " +
" ) A " +
" WHERE 1=1 " +

" ORDER BY A.ITEMID "; */



            XTable results = XSupport.GetSQLDataSet(sql);
            //    DataTable t = results.CreateDataTable(true);
            // XTable results = S1Init.myXSupport.GetSQLDataSet(sql);
            this.ItemDataGridView.Rows.Clear();
            //   this.ItemDataGridView.DataSource = t; 

            if (results.Count > 0)
            {
                //string[] row;


                for (int i = 0; i < results.Count; i++)
                {
                    //  WHNUM = 0 ;

                    List<String> rowList = new List<string>();

                    rowList.Add(results[i, "itemId"].ToString());
                    rowList.Add(results[i, "itemCode"].ToString());
                    rowList.Add(results[i, "itemName"].ToString());

                    rowList.Add(results[i, "active"].ToString());
                    rowList.Add(results[i, "groupCode"].ToString());
                    rowList.Add(results[i, "groupName"].ToString());
                    rowList.Add(results[i, "markCode"].ToString());

                    rowList.Add(results[i, "markName"].ToString());
                    rowList.Add(results[i, "retailPrice"].ToString());
                        rowList.Add(results[i, "specialPrice"].ToString());
                    rowList.Add(results[i, "colorName"].ToString());
                    rowList.Add(results[i, "sizeName"].ToString());



                    rowList.Add(results[i, "seasonName"].ToString());
                    rowList.Add(results[i, "itemBalance"].ToString());
                    rowList.Add(results[i, "itemSubCode"].ToString());
                    rowList.Add(results[i, "itemSubCode1"].ToString());
                    rowList.Add(results[i, "itemSubCode2"].ToString());


                    rowList.Add(results[i, "itemSubCode3"].ToString());
                    rowList.Add(results[i, "itemSubCode4"].ToString());
                    rowList.Add(results[i, "colorName2"].ToString());
                    rowList.Add(results[i, "sizeWebEU"].ToString());

                    rowList.Add(results[i, "sizeWebUK"].ToString());
                    rowList.Add(results[i, "sizeWebUS"].ToString());
                    rowList.Add(results[i, "sizeWebFR"].ToString());
                    rowList.Add(results[i, "sizeWebJPN"].ToString());

                    rowList.Add(results[i, "itemCode2"].ToString());
                    rowList.Add(results[i, "OnWeb"].ToString());
                

                    //   if (results[i, "itemSubCode"].ToString() != "") subCodes += results[i, "itemSubCode"].ToString();
                    //   if (results[i, "itemSubCode2"].ToString() != "") subCodes += "," + results[i, "itemSubCode2"].ToString();
                    //   if (results[i, "itemSubCode3"].ToString() != "") subCodes += "," + results[i, "itemSubCode3"].ToString();
                    //   if (results[i, "itemSubCode4"].ToString() != "") subCodes += "," + results[i, "itemSubCode4"].ToString();
                    //   if (results[i, "itemSubCode5"].ToString() != "") subCodes += "," + results[i, "itemSubCode4"].ToString();




                    // rowList.Add(results[i, "QTY1"].ToString());




                    /*
                    foreach (WHouse wHouse in WHouses)
                        {
                          String remQry = results[i, "WQTY" + WHNUM].ToString();
                          if (remQry == "0") remQry = "";
                          rowList.Add(remQry);            
                          WHNUM++; 
                        }*/

                    string[] row = rowList.ToArray();


                    this.ItemDataGridView.Rows.Add(row);

                }

            }

        }

        private void WebSyncForm_Load(object sender, EventArgs e)
        {
            // this.SelectionDateTimePicker.Value = DateTime.Now.;
            ItemDataGridView.Columns["colorName2"].DefaultCellStyle.BackColor = Color.LightGray;
            ItemDataGridView.Columns["sizeWebEU"].DefaultCellStyle.BackColor = Color.LightGray;
            ItemDataGridView.Columns["colorName"].DefaultCellStyle.BackColor = Color.LightGray;
            
        }


        private void AuthenticateWebServices()
        {
            String Username = Properties.Settings1.Default["Username"].ToString();
            String Password = Properties.Settings1.Default["Password"].ToString();

            try
            {
                String AccessToken = magento.Authenticate(
                   // "http://nak.optimus-prime.gr/rest/V1/integration/admin/token",
                      Properties.Settings1.Default["Url"].ToString() + "/rest/V1/integration/admin/token",
                     "{\"username\":\""+ Username + "\", \"password\":\""+ Password + "\"}");
                
            }
            catch
           (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void RunUpdateQtys(int daysBefore)
        {

            String ExtraWhere = "";

            if (daysBefore > 0)
                ExtraWhere =
                       " 		AND m.MTRL IN (SELECT ML.MTRL FROM MTRLINES ML INNER JOIN FINDOC F ON F.FINDOC = ML.FINDOC WHERE F.TRNDATE >  DATEADD(day, -" + daysBefore.ToString() + ", GETDATE()) )  ";


            AuthenticateWebServices();
            String sql = "  SELECT * " +
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
                          " 		AND cf.fiscprd = " + DateTime.Today.Year.ToString()+
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
                          " WHERE 1 = 1 ";


            XTable results = XSupport.GetSQLDataSet(sql);
            List<QTYProduct> qtyProdcts = new List<QTYProduct>();

            if (results.Count > 0)
            {



                for (int i = 0; i < results.Count; i++)
                {
                    QTYProduct qtyProdct = new QTYProduct();

                    qtyProdct.itemCode = results[i, "itemCode"].ToString();
                    qtyProdct.itemBalance = results[i, "itemBalance"].ToString();
                    qtyProdct.itemSubCode = results[i, "itemSubCode"].ToString();
                    qtyProdct.itemSubCode1 = results[i, "itemSubCode1"].ToString();
                    qtyProdct.itemSubCode2 = results[i, "itemSubCode2"].ToString();
                    qtyProdct.itemSubCode3 = results[i, "itemSubCode3"].ToString();
                    qtyProdct.itemSubCode4 = results[i, "itemSubCode4"].ToString();

                    qtyProdcts.Add(qtyProdct);

                }


                foreach (QTYProduct qtyProdct in qtyProdcts)
                {
                    try
                    {
                        String result = "";
                        if (qtyProdct.itemSubCode != "")
                            result = magento.UpdateProductQty(
                                // "http://nak.optimus-prime.gr/rest/V1/products/" + qtyProdct.itemSubCode + "/stockItems/1/",
                                Properties.Settings1.Default["Url"].ToString() + "/rest/V1/products/" + qtyProdct.itemSubCode + "/stockItems/1/",
                               //  Properties.Settings1.Default["Url"].ToString() + "/rest/V1/integration/admin/token",
                               qtyProdct.itemBalance
                               );

                        if (qtyProdct.itemSubCode1 != "")
                            result = magento.UpdateProductQty(
                              // "http://nak.optimus-prime.gr/rest/V1/products/" + qtyProdct.itemSubCode1 + "/stockItems/1/",
                               Properties.Settings1.Default["Url"].ToString() + "/rest/V1/products/" + qtyProdct.itemSubCode1 + "/stockItems/1/",
                               qtyProdct.itemBalance
                               );

                        if (qtyProdct.itemSubCode2 != "")
                            result = magento.UpdateProductQty(
                             //  "http://nak.optimus-prime.gr/rest/V1/products/" + qtyProdct.itemSubCode2 + "/stockItems/1/",
                                Properties.Settings1.Default["Url"].ToString() + "/rest/V1/products/" + qtyProdct.itemSubCode2 + "/stockItems/1/",
                               qtyProdct.itemBalance
                               );

                        if (qtyProdct.itemSubCode3 != "")
                            result = magento.UpdateProductQty(
                               // "http://nak.optimus-prime.gr/rest/V1/products/" + qtyProdct.itemSubCode3 + "/stockItems/1/",
                               Properties.Settings1.Default["Url"].ToString() + "/rest/V1/products/" + qtyProdct.itemSubCode3 + "/stockItems/1/",
                               qtyProdct.itemBalance
                               );

                        if (qtyProdct.itemSubCode4 != "")
                            result = magento.UpdateProductQty(
                           //    "http://nak.optimus-prime.gr/rest/V1/products/" + qtyProdct.itemSubCode4 + "/stockItems/1/",
                                Properties.Settings1.Default["Url"].ToString() + "/rest/V1/products/" + qtyProdct.itemSubCode4 + "/stockItems/1/",
                               qtyProdct.itemBalance
                               );

                    }
                    catch
                   (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }

            }
        }

        private void UpdateLatestQtysbutton_Click(object sender, EventArgs e)
        {
            RunUpdateQtys(2);
            /*

            AuthenticateWebServices();
            String sql = "  SELECT * " +
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
                          " 		AND cf.fiscprd = 2017 " +
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
                          " 		AND m.MTRL IN (SELECT ML.MTRL FROM MTRLINES ML INNER JOIN FINDOC F ON F.FINDOC = ML.FINDOC WHERE F.TRNDATE >  DATEADD(day, -1, GETDATE()) )  " +
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
                          " WHERE 1 = 1 ";


            XTable results = XSupport.GetSQLDataSet(sql);
            List<QTYProduct> qtyProdcts = new List<QTYProduct>();

            if (results.Count > 0)
            {



                for (int i = 0; i < results.Count; i++)
                {
                    QTYProduct qtyProdct = new QTYProduct();

                    qtyProdct.itemCode = results[i, "itemCode"].ToString();
                    qtyProdct.itemBalance = results[i, "itemBalance"].ToString();
                    qtyProdct.itemSubCode = results[i, "itemSubCode"].ToString();
                    qtyProdct.itemSubCode1 = results[i, "itemSubCode1"].ToString();
                    qtyProdct.itemSubCode2 = results[i, "itemSubCode2"].ToString();
                    qtyProdct.itemSubCode3 = results[i, "itemSubCode3"].ToString();
                    qtyProdct.itemSubCode4 = results[i, "itemSubCode4"].ToString();

                    qtyProdcts.Add(qtyProdct);

                }


                foreach (QTYProduct qtyProdct in qtyProdcts)
                {
                    try
                    {
                        String result = "";
                        if (qtyProdct.itemSubCode != "")
                            result = magento.UpdateProductQty(
                               "http://nak.optimus-prime.gr/rest/V1/products/" + qtyProdct.itemSubCode + "/stockItems/1/",
                               qtyProdct.itemBalance
                               );

                        if (qtyProdct.itemSubCode1 != "")
                            result = magento.UpdateProductQty(
                               "http://nak.optimus-prime.gr/rest/V1/products/" + qtyProdct.itemSubCode1 + "/stockItems/1/",
                               qtyProdct.itemBalance
                               );

                        if (qtyProdct.itemSubCode2 != "")
                            result = magento.UpdateProductQty(
                               "http://nak.optimus-prime.gr/rest/V1/products/" + qtyProdct.itemSubCode2 + "/stockItems/1/",
                               qtyProdct.itemBalance
                               );

                        if (qtyProdct.itemSubCode3 != "")
                            result = magento.UpdateProductQty(
                               "http://nak.optimus-prime.gr/rest/V1/products/" + qtyProdct.itemSubCode3 + "/stockItems/1/",
                               qtyProdct.itemBalance
                               );

                        if (qtyProdct.itemSubCode4 != "")
                            result = magento.UpdateProductQty(
                               "http://nak.optimus-prime.gr/rest/V1/products/" + qtyProdct.itemSubCode4 + "/stockItems/1/",
                               qtyProdct.itemBalance
                               );

                    }
                    catch
                   (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }

            } */
        }


        private void button2_Click(object sender, EventArgs e)
        {
            RunUpdateQtys(0);
        }
                   
        ResultMessageForm rmf = new ResultMessageForm();
        private void UpdateProductsButton_Click(object sender, EventArgs e)
        {
            AuthenticateWebServices();

            rmf = new ResultMessageForm();

            formProgressBar.Minimum = 0;
            formProgressBar.Maximum = (ItemDataGridView.Rows.Count) + 1;
            formProgressBar.Value = 1;

            try
            {
                String result = magento.truncate(
                 //    "http://local.nak2.gr/rest/all/V1/softone/truncate/"
                     Properties.Settings1.Default["Url"].ToString() + "/rest/all/V1/softone/truncate/"
                     );
            }
            catch
           (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            int i = 0;
            foreach (DataGridViewRow dr in ItemDataGridView.Rows)
            {
                formProgressBar.Increment(1);
                PopulateItem populateItem = new PopulateItem();


                populateItem.itemId = dr.Cells["itemId"].Value.ToString();
                populateItem.itemCode = dr.Cells["itemCode"].Value.ToString();
                populateItem.itemName = dr.Cells["itemName"].Value.ToString();
                populateItem.active = dr.Cells["active"].Value.ToString();
                populateItem.groupCode = dr.Cells["groupCode"].Value.ToString();
                populateItem.groupName = dr.Cells["groupName"].Value.ToString();
                populateItem.markCode = dr.Cells["markCode"].Value.ToString();
                populateItem.markName = dr.Cells["markName"].Value.ToString();
                populateItem.retailPrice = dr.Cells["retailPrice"].Value.ToString();
                populateItem.colorName = dr.Cells["colorName"].Value.ToString();
                populateItem.sizeName = dr.Cells["sizeName"].Value.ToString();
                populateItem.seasonName = dr.Cells["seasonName"].Value.ToString();
                populateItem.itemBalance = dr.Cells["itemBalance"].Value.ToString();
                populateItem.itemSubCode = dr.Cells["itemSubCode"].Value.ToString();
                populateItem.itemSubCode1 = dr.Cells["itemSubCode1"].Value.ToString();
                populateItem.itemSubCode2 = dr.Cells["itemSubCode2"].Value.ToString();
                populateItem.itemSubCode3 = dr.Cells["itemSubCode3"].Value.ToString();
                populateItem.itemSubCode4 = dr.Cells["itemSubCode4"].Value.ToString();
                populateItem.colorName2 = dr.Cells["colorName2"].Value.ToString();
                populateItem.sizeName2 = dr.Cells["sizeWebEU"].Value.ToString();
                populateItem.itemCode2 = dr.Cells["itemCode2"].Value.ToString();
                populateItem.specialPrice = dr.Cells["specialPrice"].Value.ToString();
                
                try
                {
                    populateItem.onWeb = dr.Cells["OnWeb"].Value.ToString();
                }
                catch (Exception)
                {
                    populateItem.onWeb = "0";
                }

                populateItem.sizeWebUK = dr.Cells["sizeWebUK"].Value.ToString();
                populateItem.sizeWebUS = dr.Cells["sizeWebUS"].Value.ToString();
                populateItem.sizeWebEU = dr.Cells["sizeWebEU"].Value.ToString();
                populateItem.sizeWebFR = dr.Cells["sizeWebFR"].Value.ToString();
                populateItem.sizeWebJPN = dr.Cells["sizeWebJPN"].Value.ToString();


                if (populateItem.colorName2 != "" && populateItem.sizeWebEU != "")
                {
                    try
                    {
                        String result = magento.populate(
                        //    "http://local.nak2.gr/rest/all/V1/softone/populate/",
                             Properties.Settings1.Default["Url"].ToString() + "/rest/all/V1/softone/populate/",
                            populateItem
                            );


                        if (result != "false")
                        {
                            rmf.MessageTextBox.AppendText(populateItem.itemCode + "  Result " + result + Environment.NewLine);
                            rmf.isActivated = true;
                        }
                    }
                    catch
                   (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }

            }



            try
            {
                String result = magento.importTriger(
                     //"http://local.nak2.gr/rest/all/V1/softone/importproducts/"
                      Properties.Settings1.Default["Url"].ToString() + "/rest/all/V1/softone/importproducts/"
                     );

                if (result != "false" && result != "" )
                {
                    rmf.MessageTextBox.AppendText( "Triger Result: " + result + Environment.NewLine);
                    rmf.isActivated = true;
                }

            }
            catch
           (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            formProgressBar.Value = 0;
            MessageBox.Show("Η ενημέρωση ολοληρώθηκε.");


        }

        private void ItemDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                String result = magento.importTriger(
                     //"http://local.nak2.gr/rest/all/V1/softone/importproducts/"
                       Properties.Settings1.Default["Url"].ToString() + "/rest/all/V1/softone/importproducts/"
                     );

             

            }
            catch
           (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            MessageBox.Show("OK");
        }

        private void ResultsButton_Click(object sender, EventArgs e)
        {
            if (rmf.isActivated) rmf.Visible = true;
        }
    }
}
