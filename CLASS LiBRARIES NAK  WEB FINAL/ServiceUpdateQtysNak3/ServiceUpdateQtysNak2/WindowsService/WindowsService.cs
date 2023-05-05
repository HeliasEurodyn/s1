using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using System.IO;
using System.Data.SqlClient;
using System.Net;
using System.Data;
using System.Collections.Generic;

namespace WindowsService
{
    class WindowsService : ServiceBase
    {
        /// <summary>
        /// Public Constructor for WindowsService.
        /// - Put all of your Initialization code here.
        /// </summary>
        /// 
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

       public static String UPDATELOGFILE = "";
        //  public static String COMPANY;
        //  public static String OUTPUT_PATH = "";

        //  public static String FTPDOMAIN = "";
        //  public static String FTPUSER = "";
        // public static String FTPPASSWORD = "";
        public static String DaysBeforeStr = "";


        public WindowsService()
        {
            this.ServiceName = "NAKUpdateWebQty";
            this.EventLog.Source = "SyncProductsUnitech";
            this.EventLog.Log = "Application";
           // this.ServiceName = "SyncProductsUnitech";
          //  this.EventLog.Source = "SyncProductsUnitech";
            // These Flags set whether or not to handle that specific
            //  type of event. Set to true if you need it, false otherwise.
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;

            if (!EventLog.SourceExists("NAKUpdateWebQty"))
                EventLog.CreateEventSource("NAKUpdateWebQty", "Application");
        }

        /// <summary>
        /// The Main Thread: This is where your Service is Run.
        /// </summary>
        static void Main()
        {
            ServiceBase.Run(new WindowsService());
        }

        /// <summary>
        /// Dispose of objects that need it here.
        /// </summary>
        /// <param name="disposing">Whether or not disposing is going on.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// OnStart: Put startup code here
        ///  - Start threads, get inital data, etc.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

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

                line = reader.ReadLine();
                UPDATELOGFILE = line;

                
            }


            this.actions(null, null);
            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new ElapsedEventHandler(actions);
            myTimer.Interval = EXECTime;
            myTimer.Start();
        }

        public void LogMessageToFile(string msg)
        {

            if (UPDATELOGFILE != "USELOG") return;
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



        /*void uploadToFtp()
         {
             FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://www.office1.gr/comm/upload/products/products_prices.xls");
             request.Method = WebRequestMethods.Ftp.UploadFile;

             request.Credentials = new NetworkCredential("ftpuser1", "NXQayqY7mUlh");

             using (FileStream fs = File.OpenRead("pprices.xls"))
             {
                 byte[] buffer = new byte[fs.Length];
                 fs.Read(buffer, 0, buffer.Length);
                 fs.Close();
                 Stream requestStream = request.GetRequestStream();
                 requestStream.Write(buffer, 0, buffer.Length);
                 requestStream.Close();
                 requestStream.Flush();
             }
             FtpWebResponse response = (FtpWebResponse)request.GetResponse();

             response.Close();
         }*/

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

        private void actions(object sender, EventArgs e)
        {
            String connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            String sql = null;
            SqlDataReader dataReader;
           // CustomerHttp customerHttp = new CustomerHttp();
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
                " 	INNER JOIN WHOUSE W ON W.WHOUSE = CF.WHOUSE AND W.FAX = 'ONWEB'                                                                                      " +
                " 		AND W.COMPANY = CF.COMPANY  AND W.FAX = 'ONWEB'                                                                                                         " +
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


            int countProducts = 0;
            List<PopulateItem> items = new List<PopulateItem>();

            foreach (QTYProduct qtyProdct in qtyProdcts)
                {

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
                                 URL + "/rest/all/V1/softone/updatestocks/",
                                 items
                                 );
                        }
                        catch
                       (Exception ex)
                        {
                            LogMessageToFile(" [On updatestocks] " + ex.Message);
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
                                 URL + "/rest/all/V1/softone/updatestocks/",
                                 items
                                 );
                }
                catch
               (Exception ex)
                {
                    LogMessageToFile(" [On updatestocks] " + ex.Message);
                }

                countProducts = 0;
                items = new List<PopulateItem>();
            }



        }


        /// <summary>
        /// OnStop: Put your stop code here
        /// - Stop threads, set final data, etc.
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
        }

        /// <summary>
        /// OnPause: Put your pause code here
        /// - Pause working threads, etc.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>
        /// OnContinue: Put your continue code here
        /// - Un-pause working threads, etc.
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();
        }

        /// <summary>
        /// OnShutdown(): Called when the System is shutting down
        /// - Put code here when you need special handling
        ///   of code that deals with a system shutdown, such
        ///   as saving special data before shutdown.
        /// </summary>
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        /// <summary>
        /// OnCustomCommand(): If you need to send a command to your
        ///   service without the need for Remoting or Sockets, use
        ///   this method to do custom methods.
        /// </summary>
        /// <param name="command">Arbitrary Integer between 128 & 256</param>
        protected override void OnCustomCommand(int command)
        {
            //  A custom command can be sent to a service by using this method:
            //#  int command = 128; //Some Arbitrary number between 128 & 256
            //#  ServiceController sc = new ServiceController("NameOfService");
            //#  sc.ExecuteCommand(command);

            base.OnCustomCommand(command);
        }

        /// <summary>
        /// OnPowerEvent(): Useful for detecting power status changes,
        ///   such as going into Suspend mode or Low Battery for laptops.
        /// </summary>
        /// <param name="powerStatus">The Power Broadcase Status (BatteryLow, Suspend, etc.)</param>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        /// <summary>
        /// OnSessionChange(): To handle a change event from a Terminal Server session.
        ///   Useful if you need to determine when a user logs in remotely or logs off,
        ///   or when someone logs into the console.
        /// </summary>
        /// <param name="changeDescription"></param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
        }
    }
}
