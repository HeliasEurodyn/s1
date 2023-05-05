using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PiraeusBankImport
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Properties.ErgoSettings.Default["ErgopartnersAcc"] = this.ErgopartnersAccTextBox.Text;
            Properties.ErgoSettings.Default["ErgoplanningAcc"] = this.ErgoplanningAccTextBox.Text;

            Properties.ErgoSettings.Default.Save();

            MessageBox.Show("Στοιχεία αποθηκεύτηκαν");
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.ErgopartnersAccTextBox.Text =  Properties.ErgoSettings.Default["ErgopartnersAcc"].ToString()  ;
            this.ErgoplanningAccTextBox.Text = Properties.ErgoSettings.Default["ErgoplanningAcc"].ToString()  ;
        }
    }
}
