using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ClassLibrary8
{
    class  UploadFileToFtpUsingSSL
    {
        public static void Upload(String FTPDOMAIN, String FTPUSER, String FTPPASSWORD, String OUTPUT_PATH)
        {


            try
            {

                System.Net.ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };

                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(FTPDOMAIN);
                req.EnableSsl = true;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.Credentials = new NetworkCredential(FTPUSER, FTPPASSWORD);

                byte[] fileData = File.ReadAllBytes(OUTPUT_PATH);

                // rdr.Close();
                req.ContentLength = fileData.Length;

                Stream reqStream = req.GetRequestStream();
                reqStream.Write(fileData, 0, fileData.Length);

                reqStream.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }



    }
}
