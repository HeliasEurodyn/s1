namespace ClassLibraryNakWebSync
{
    partial class WebSyncForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebSyncForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ResultsButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.CodeFilterTextBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.UpdateProductsButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.formProgressBar = new System.Windows.Forms.ProgressBar();
            this.ItemDataGridView = new System.Windows.Forms.DataGridView();
            this.GBPTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.itemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.active = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.markCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.markName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.retailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.retailPriceUK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.specialPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.specialPriceUK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finalDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seasonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemSubCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemSubCode1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemSubCode2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemSubCode3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemSubCode4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeWebEU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeWebUK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeWebUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeWebFR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeWebJPN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemCode2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OnWeb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ItemDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ResultsButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.CodeFilterTextBox);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.UpdateProductsButton);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.SelectionDateTimePicker);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 405);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(974, 103);
            this.panel1.TabIndex = 1;
            // 
            // ResultsButton
            // 
            this.ResultsButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ResultsButton.BackgroundImage")));
            this.ResultsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ResultsButton.Location = new System.Drawing.Point(285, 74);
            this.ResultsButton.Name = "ResultsButton";
            this.ResultsButton.Size = new System.Drawing.Size(23, 20);
            this.ResultsButton.TabIndex = 10;
            this.ResultsButton.UseVisualStyleBackColor = true;
            this.ResultsButton.Click += new System.EventHandler(this.ResultsButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Κωδικός Είδους";
            // 
            // CodeFilterTextBox
            // 
            this.CodeFilterTextBox.Location = new System.Drawing.Point(70, 74);
            this.CodeFilterTextBox.Name = "CodeFilterTextBox";
            this.CodeFilterTextBox.Size = new System.Drawing.Size(200, 20);
            this.CodeFilterTextBox.TabIndex = 8;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(334, 72);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // UpdateProductsButton
            // 
            this.UpdateProductsButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("UpdateProductsButton.BackgroundImage")));
            this.UpdateProductsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.UpdateProductsButton.Enabled = false;
            this.UpdateProductsButton.Location = new System.Drawing.Point(13, 54);
            this.UpdateProductsButton.Name = "UpdateProductsButton";
            this.UpdateProductsButton.Size = new System.Drawing.Size(46, 41);
            this.UpdateProductsButton.TabIndex = 6;
            this.UpdateProductsButton.UseVisualStyleBackColor = true;
            this.UpdateProductsButton.Click += new System.EventHandler(this.UpdateProductsButton_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(430, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(544, 103);
            this.panel3.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label5.Location = new System.Drawing.Point(160, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(370, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Δεν θα πρέπει να αλλάζουν στο S1 για είδη που υπάρχουν ήδη στo Web.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label6.Location = new System.Drawing.Point(160, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(340, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Τα πεδία με # είναι υποχρεωτικά (Περιγραφή Χρώμ 1, SizeWebEu).";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label4.Location = new System.Drawing.Point(160, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(362, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Τα είδη που τα έχουν κενά δεν μπορούν να ενημερωθουν στην σελίδα.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label3.Location = new System.Drawing.Point(160, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(222, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Προσοχή! Τα πεδία με * είναι υποχρεωτικά.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Είδη που ενημερώθηκαν απο ημερομηνία";
            // 
            // SelectionDateTimePicker
            // 
            this.SelectionDateTimePicker.Location = new System.Drawing.Point(70, 32);
            this.SelectionDateTimePicker.Name = "SelectionDateTimePicker";
            this.SelectionDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.SelectionDateTimePicker.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(13, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(46, 41);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.formProgressBar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 388);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(974, 17);
            this.panel2.TabIndex = 2;
            // 
            // formProgressBar
            // 
            this.formProgressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.formProgressBar.Location = new System.Drawing.Point(0, 0);
            this.formProgressBar.Name = "formProgressBar";
            this.formProgressBar.Size = new System.Drawing.Size(974, 17);
            this.formProgressBar.TabIndex = 0;
            // 
            // ItemDataGridView
            // 
            this.ItemDataGridView.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.ItemDataGridView.AllowUserToAddRows = false;
            this.ItemDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ItemDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemId,
            this.itemCode,
            this.itemName,
            this.active,
            this.groupCode,
            this.groupName,
            this.markCode,
            this.markName,
            this.retailPrice,
            this.retailPriceUK,
            this.specialPrice,
            this.specialPriceUK,
            this.fromDate,
            this.finalDate,
            this.colorName,
            this.sizeName,
            this.seasonName,
            this.itemBalance,
            this.itemSubCode,
            this.itemSubCode1,
            this.itemSubCode2,
            this.itemSubCode3,
            this.itemSubCode4,
            this.colorName2,
            this.sizeWebEU,
            this.sizeWebUK,
            this.sizeWebUS,
            this.sizeWebFR,
            this.sizeWebJPN,
            this.itemCode2,
            this.OnWeb});
            this.ItemDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemDataGridView.Location = new System.Drawing.Point(0, 0);
            this.ItemDataGridView.Name = "ItemDataGridView";
            this.ItemDataGridView.Size = new System.Drawing.Size(974, 388);
            this.ItemDataGridView.TabIndex = 39;
            this.ItemDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ItemDataGridView_CellContentClick);
            // 
            // GBPTextBox
            // 
            this.GBPTextBox.Location = new System.Drawing.Point(9, 18);
            this.GBPTextBox.Name = "GBPTextBox";
            this.GBPTextBox.ReadOnly = true;
            this.GBPTextBox.Size = new System.Drawing.Size(116, 20);
            this.GBPTextBox.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.GBPTextBox);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.groupBox1.Location = new System.Drawing.Point(5, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(139, 46);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EURO - GBP Currency";
            // 
            // itemId
            // 
            this.itemId.HeaderText = "ID Είδους";
            this.itemId.Name = "itemId";
            // 
            // itemCode
            // 
            this.itemCode.HeaderText = "Κωδικός";
            this.itemCode.Name = "itemCode";
            // 
            // itemName
            // 
            this.itemName.HeaderText = "Περιγραφή";
            this.itemName.Name = "itemName";
            // 
            // active
            // 
            this.active.HeaderText = "Ενεργό";
            this.active.Name = "active";
            // 
            // groupCode
            // 
            this.groupCode.HeaderText = "Κωδ. Group";
            this.groupCode.Name = "groupCode";
            // 
            // groupName
            // 
            this.groupName.HeaderText = "Περιγραφή Group";
            this.groupName.Name = "groupName";
            // 
            // markCode
            // 
            this.markCode.HeaderText = "Κωδ. Μαρκας";
            this.markCode.Name = "markCode";
            // 
            // markName
            // 
            this.markName.HeaderText = "Περιγραφή Μάρκας";
            this.markName.Name = "markName";
            // 
            // retailPrice
            // 
            this.retailPrice.HeaderText = "Τιμή";
            this.retailPrice.Name = "retailPrice";
            // 
            // retailPriceUK
            // 
            this.retailPriceUK.HeaderText = "Τιμή(UK)";
            this.retailPriceUK.Name = "retailPriceUK";
            // 
            // specialPrice
            // 
            this.specialPrice.HeaderText = "Τιμή Έκπτωσης";
            this.specialPrice.Name = "specialPrice";
            // 
            // specialPriceUK
            // 
            this.specialPriceUK.HeaderText = "Τιμή Έκπτωσης(UK)";
            this.specialPriceUK.Name = "specialPriceUK";
            // 
            // fromDate
            // 
            this.fromDate.HeaderText = "Έκπτωση απο";
            this.fromDate.Name = "fromDate";
            // 
            // finalDate
            // 
            this.finalDate.HeaderText = "Έκπτωση έως";
            this.finalDate.Name = "finalDate";
            // 
            // colorName
            // 
            this.colorName.HeaderText = " # Περιγραφή Χρώμ 1";
            this.colorName.Name = "colorName";
            // 
            // sizeName
            // 
            this.sizeName.HeaderText = "Περιγραφή Μεγ 1";
            this.sizeName.Name = "sizeName";
            // 
            // seasonName
            // 
            this.seasonName.HeaderText = "Season";
            this.seasonName.Name = "seasonName";
            // 
            // itemBalance
            // 
            this.itemBalance.HeaderText = "Υπόλοιπο";
            this.itemBalance.Name = "itemBalance";
            // 
            // itemSubCode
            // 
            this.itemSubCode.HeaderText = "Εναλλακτικός 1";
            this.itemSubCode.Name = "itemSubCode";
            // 
            // itemSubCode1
            // 
            this.itemSubCode1.HeaderText = "Εναλλακτικός 2";
            this.itemSubCode1.Name = "itemSubCode1";
            // 
            // itemSubCode2
            // 
            this.itemSubCode2.HeaderText = "Εναλλακτικός 3";
            this.itemSubCode2.Name = "itemSubCode2";
            // 
            // itemSubCode3
            // 
            this.itemSubCode3.HeaderText = "Εναλλακτικός 4";
            this.itemSubCode3.Name = "itemSubCode3";
            // 
            // itemSubCode4
            // 
            this.itemSubCode4.HeaderText = "Εναλλακτικός 5";
            this.itemSubCode4.Name = "itemSubCode4";
            // 
            // colorName2
            // 
            this.colorName2.HeaderText = "* Περιγραφή Χρώμ 2";
            this.colorName2.Name = "colorName2";
            // 
            // sizeWebEU
            // 
            this.sizeWebEU.HeaderText = "*# sizeWebEU";
            this.sizeWebEU.Name = "sizeWebEU";
            // 
            // sizeWebUK
            // 
            this.sizeWebUK.HeaderText = "sizeWebUK";
            this.sizeWebUK.Name = "sizeWebUK";
            // 
            // sizeWebUS
            // 
            this.sizeWebUS.HeaderText = "sizeWebUS";
            this.sizeWebUS.Name = "sizeWebUS";
            // 
            // sizeWebFR
            // 
            this.sizeWebFR.HeaderText = "sizeWebFR";
            this.sizeWebFR.Name = "sizeWebFR";
            // 
            // sizeWebJPN
            // 
            this.sizeWebJPN.HeaderText = "sizeWebJPN";
            this.sizeWebJPN.Name = "sizeWebJPN";
            // 
            // itemCode2
            // 
            this.itemCode2.HeaderText = "Μικρός Κωδικός";
            this.itemCode2.Name = "itemCode2";
            // 
            // OnWeb
            // 
            this.OnWeb.HeaderText = "Στο Web";
            this.OnWeb.Name = "OnWeb";
            // 
            // WebSyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 508);
            this.Controls.Add(this.ItemDataGridView);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "WebSyncForm";
            this.Text = "WebSyncForm";
            this.Load += new System.EventHandler(this.WebSyncForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ItemDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.DataGridView ItemDataGridView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker SelectionDateTimePicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button UpdateProductsButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ProgressBar formProgressBar;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CodeFilterTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ResultsButton;
        private System.Windows.Forms.TextBox GBPTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn active;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn markCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn markName;
        private System.Windows.Forms.DataGridViewTextBoxColumn retailPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn retailPriceUK;
        private System.Windows.Forms.DataGridViewTextBoxColumn specialPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn specialPriceUK;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn finalDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn seasonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemSubCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemSubCode1;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemSubCode2;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemSubCode3;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemSubCode4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colorName2;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeWebEU;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeWebUK;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeWebUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeWebFR;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeWebJPN;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemCode2;
        private System.Windows.Forms.DataGridViewTextBoxColumn OnWeb;
    }
}