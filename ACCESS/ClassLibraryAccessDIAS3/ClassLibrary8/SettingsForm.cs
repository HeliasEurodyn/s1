using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Softone;

namespace ClassLibrary8
{
    public partial class SettingsForm : Form
    {

        public XSupport XSupport;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Settings1.Default["CustomerId"]     = this.CustomerIdTextBox.Text;
            Settings1.Default["Description"]    = this.DescriptionTextBox.Text;
            Settings1.Default["IBAN"]           = this.IBANTextBox.Text;
            Settings1.Default["FtpUrl"]         = this.FtpUrlTextBox.Text;
            Settings1.Default["FtpUrlIn"]       = this.FtpUrlInTextBox.Text;
            Settings1.Default["FtpUser"]        = this.FtpUserTextBox.Text;
            Settings1.Default["FtpPassword"]    = this.FtpPasswordTextBox.Text;
            Settings1.Default["EXPORTPATH"]     = this.EXPORTPATHTextBox.Text;
            Settings1.Default["IMPORTPATH"]     = this.IMPORTPATHTextBox.Text;

            Settings1.Default["TIMEINTERVAL"] = this.TimeIntervalTextBox.Text;

            


            Settings1.Default.Save();
            MessageBox.Show("Στοιχεία αποθηκεύτηκαν");
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
           this.CustomerIdTextBox.Text = Settings1.Default["CustomerId"].ToString();
           this.DescriptionTextBox.Text = Settings1.Default["Description"].ToString();
           this.IBANTextBox.Text = Settings1.Default["IBAN"].ToString();
           this.FtpUrlTextBox.Text = Settings1.Default["FtpUrl"].ToString();
           this.FtpUrlInTextBox.Text = Settings1.Default["FtpUrlIn"].ToString();
           this.FtpUserTextBox.Text = Settings1.Default["FtpUser"].ToString();
           this.FtpPasswordTextBox.Text = Settings1.Default["FtpPassword"].ToString();
           this.EXPORTPATHTextBox.Text = Settings1.Default["EXPORTPATH"].ToString();
           this.IMPORTPATHTextBox.Text = Settings1.Default["IMPORTPATH"].ToString();

          this.TimeIntervalTextBox.Text = Settings1.Default["TIMEINTERVAL"].ToString();


        }

        private void button2_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog openFileDialog1 = new FolderBrowserDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.SelectedPath;
                try
                {
                    this.EXPORTPATHTextBox.Text = file;
                }
                catch (Exception)
                {
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFileDialog1 = new FolderBrowserDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.SelectedPath;
                try
                {
                    this.IMPORTPATHTextBox.Text = file;
                }
                catch (Exception)
                {
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReadFtpFiles rf = new ReadFtpFiles();
            rf.curSupport = this.XSupport;

            rf.updateFtpFileListToDb(
                               Settings1.Default["FtpUrlIn"].ToString(),
                               Settings1.Default["FtpUser"].ToString(),
                               Settings1.Default["FtpPassword"].ToString(),
                               this.XSupport);

          

            rf.DownloadFiles(
                               Settings1.Default["FtpUrlIn"].ToString(),
                               Settings1.Default["FtpUser"].ToString(),
                               Settings1.Default["FtpPassword"].ToString(),
                               Settings1.Default["IMPORTPATH"].ToString(),
                               Settings1.Default["CustomerId"].ToString()
                               );

            MessageBox.Show("Η διαδικασία ολοκληρώθηκε.");

        }
    }
}
