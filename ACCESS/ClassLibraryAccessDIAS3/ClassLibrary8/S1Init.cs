using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Softone;
using System.Timers;

namespace ClassLibrary8
{
   
    class S1Init : TXCode
    {
        private static System.Timers.Timer myTimer;
        private static bool FirstTime = true;
        public static XSupport myXSupport;


        public override void Initialize()
        {
            base.Initialize();
            myXSupport = this.XSupport;

            int timerInterval = 0;
            Boolean ParseTimeInterval = false;
            ParseTimeInterval = int.TryParse(Settings1.Default["TIMEINTERVAL"].ToString(), out timerInterval);
             


            if (timerInterval > 0)
            {
                myTimer = new System.Timers.Timer(); // (settings.timerInterval * 60000);
                myTimer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);

                myTimer.Interval = 10 * 60000; //Πρώτη φορά εκτέλεση στα 10 Λεπτά // settings.timerInterval * 60000;
                myTimer.Enabled = true;
            }

        }


        private static void OnTimerElapsed(object source, EventArgs e)
        {

            try
            {
                ReadFtpFiles rf = new ReadFtpFiles();
                rf.curSupport = myXSupport;

                rf.updateFtpFileListToDb(
                                   Settings1.Default["FtpUrlIn"].ToString(),
                                   Settings1.Default["FtpUser"].ToString(),
                                   Settings1.Default["FtpPassword"].ToString(),
                                   myXSupport);

                rf.DownloadFiles(
                                   Settings1.Default["FtpUrlIn"].ToString(),
                                   Settings1.Default["FtpUser"].ToString(),
                                   Settings1.Default["FtpPassword"].ToString(),
                                   Settings1.Default["IMPORTPATH"].ToString(),
                                   Settings1.Default["CustomerId"].ToString()
                                   );
             
            }
            catch
            { }

            if (FirstTime)
            {
               
                int timerInterval = 0;
                Boolean ParseTimeInterval = false;
                ParseTimeInterval = int.TryParse(Settings1.Default["TIMEINTERVAL"].ToString(), out timerInterval);
                myTimer.Interval = timerInterval * 60000;
                FirstTime = false;
            }
        }



    }
}
