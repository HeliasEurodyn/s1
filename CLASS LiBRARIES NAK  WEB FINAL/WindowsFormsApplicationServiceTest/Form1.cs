using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplicationServiceTest
{
    public partial class Form1 : Form
    {


        private Magento magento = new Magento();

        public static String DOMAIN;
        public static String SERVER;
        public static String DBName;
        public static String DBUser;
        public static String DBPassword;
        public static int EXECTime;

        public static String URL;
        public static String USER;
        public static String PASSWORD;
        public static String DaysBeforeStr = "";


        public Form1()
        {
            InitializeComponent();
        }


        private void AuthenticateWebServices()
        {
            try
            {

                String Username = USER;
                String Password = PASSWORD;

                String AccessToken = magento.Authenticate(
                      URL + "/rest/V1/integration/admin/token",
                     "{\"username\":\"" + Username + "\", \"password\":\"" + Password + "\"}");

            }
            catch
           (Exception ex)
            {
                LogMessageToFile(ex.Message);
                //  MessageBox.Show(ex.Message);
            }

        }


        public void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(@"C:\Vitatron\LogFile.txt");
            try
            {
                string logLine = System.String.Format(
                    "{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }



        private void run()
        {
            String connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            String sql = null;
            SqlDataReader dataReader;
            DateTime dt = DateTime.Now;
            AuthenticateWebServices();

            connetionString =
          "server = tcp:" + SERVER + ";Database=" + DBName + ";" +
          "User ID =" + DBUser + ";Password=" + DBPassword + ";Trusted_Connection=False;" +
          "Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";


            String ExtraWhere = "";

            int daysBefore = int.Parse(DaysBeforeStr);

            if (daysBefore > 0)
                ExtraWhere =
                    " AND itemId IN(SELECT ML.MTRL FROM MTRLINES ML INNER JOIN FINDOC F ON F.FINDOC = ML.FINDOC WHERE F.UPDDATE > DATEADD(day, -" + daysBefore.ToString() + ", GETDATE())) ";

            sql = " SELECT *                                                                                                                                                     " +
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
                " 		AND W.COMPANY = CF.COMPANY " + // AND W.FAX = 'ONWEB'                                                                                                         " +
                " 	WHERE M.SODTYPE = 51                                                                                                                                       " +
                "               AND MS.ISACTIVE =1                                                                                                                             " +
                "  AND M.CCCINT01 = 1 " +
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



            connection = new SqlConnection(connetionString);
            List<QTYProduct> qtyProdcts = new List<QTYProduct>();

            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                dataReader = command.ExecuteReader();




                while (dataReader.Read())
                {
                    QTYProduct qtyProdct = new QTYProduct();

                    qtyProdct.itemCode = dataReader["itemCode"].ToString();
                    qtyProdct.itemBalance = dataReader["itemBalance"].ToString();
                    qtyProdct.itemSubCode = dataReader["itemSubCode"].ToString();

                    qtyProdcts.Add(qtyProdct);
                }

            }
            catch (Exception ex)
            {
                LogMessageToFile(ex.Message);
            }

            foreach (QTYProduct qtyProdct in qtyProdcts)
            {
                try
                {
                    String result = "";
                    if (qtyProdct.itemSubCode != "")
                        result = magento.UpdateProductQty(
                           URL + "/rest/V1/products/" + qtyProdct.itemSubCode + "/stockItems/1/",
                           qtyProdct.itemBalance
                           );
                }
                catch
               (Exception ex)
                {
                    LogMessageToFile(" [" + qtyProdct.itemSubCode + "] " + ex.Message);
                    // MessageBox.Show(ex.Message);
                }

            }




        }



        private void runNewMethod()
        {
           
            AuthenticateWebServices();
            List<PopulateItem> items = new List<PopulateItem>();
            PopulateItem item = new PopulateItem();
            item.itemSubCode = "112233445566KKK";
            item.itemBalance = "15";
            items.Add(item);

            item = new PopulateItem();
            item.itemSubCode = "112233445561KKK";
            item.itemBalance = "25";
            items.Add(item);

            item = new PopulateItem();
            item.itemSubCode = "112233441261KKK";
            item.itemBalance = "35";
            items.Add(item);

            try
                {
                    String result = "";
                   
                   result = magento.updateProductQtys(
                           URL + "/rest/all/V1/softone/updatestocks/",
                           items
                           );
                }
                catch
               (Exception ex)
                {
                    LogMessageToFile(" Error qtys " + ex.Message);
                    // MessageBox.Show(ex.Message);
                }


            

        }






        private void Form1_Load(object sender, EventArgs e)
        {
            string line;
            using (StreamReader reader = new StreamReader(@"C:\Vitatron\params.ini"))
            {
                line = reader.ReadLine();
                SERVER = line;

                line = reader.ReadLine();
                DBName = line;

                line = reader.ReadLine();
                DBUser = line;

                line = reader.ReadLine();
                DBPassword = line;

                line = reader.ReadLine();
                EXECTime = Int32.Parse(line);

                line = reader.ReadLine();
                URL = line;

                line = reader.ReadLine();
                USER = line;

                line = reader.ReadLine();
                PASSWORD = line;

                line = reader.ReadLine();
                DaysBeforeStr = line;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.run();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.runNewMethod();
        }
    }
}
