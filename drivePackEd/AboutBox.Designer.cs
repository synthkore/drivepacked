using System.Windows.Forms;
using System.Drawing;

namespace drivePackEd {

    partial class AboutBox {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent() {
            tableLayoutPanel = new TableLayoutPanel();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            lblProductName = new Label();
            lblVersion = new Label();
            lblBuild = new Label();
            linkLProjectSite = new LinkLabel();
            panel2 = new Panel();
            linkLblSoruce = new LinkLabel();
            linkLblBeHex = new LinkLabel();
            linkLblLicense = new LinkLabel();
            lblLicense = new Label();
            textBoxDescription = new TextBox();
            linkLblAuthor = new LinkLabel();
            tableLayoutPanel.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(panel1, 0, 0);
            tableLayoutPanel.Controls.Add(lblProductName, 0, 1);
            tableLayoutPanel.Controls.Add(lblVersion, 0, 2);
            tableLayoutPanel.Controls.Add(lblBuild, 0, 3);
            tableLayoutPanel.Controls.Add(linkLProjectSite, 0, 5);
            tableLayoutPanel.Controls.Add(panel2, 0, 6);
            tableLayoutPanel.Controls.Add(textBoxDescription, 0, 7);
            tableLayoutPanel.Controls.Add(linkLblAuthor, 0, 4);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(10, 10);
            tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 8;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 285F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Size = new Size(544, 654);
            tableLayoutPanel.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(538, 279);
            panel1.TabIndex = 25;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = Properties.Resources.dpacklogo;
            pictureBox1.Location = new Point(4, 3);
            pictureBox1.MaximumSize = new Size(550, 265);
            pictureBox1.MinimumSize = new Size(459, 201);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(531, 265);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lblProductName
            // 
            lblProductName.Dock = DockStyle.Fill;
            lblProductName.Location = new Point(7, 285);
            lblProductName.Margin = new Padding(7, 0, 4, 0);
            lblProductName.MaximumSize = new Size(0, 20);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(533, 20);
            lblProductName.TabIndex = 19;
            lblProductName.Text = "Nombre de producto";
            lblProductName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVersion
            // 
            lblVersion.Dock = DockStyle.Fill;
            lblVersion.Location = new Point(7, 307);
            lblVersion.Margin = new Padding(7, 0, 4, 0);
            lblVersion.MaximumSize = new Size(0, 20);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(533, 20);
            lblVersion.TabIndex = 0;
            lblVersion.Text = "Versión";
            lblVersion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBuild
            // 
            lblBuild.Dock = DockStyle.Fill;
            lblBuild.Location = new Point(7, 331);
            lblBuild.Margin = new Padding(7, 2, 4, 0);
            lblBuild.MaximumSize = new Size(0, 20);
            lblBuild.Name = "lblBuild";
            lblBuild.Size = new Size(533, 20);
            lblBuild.TabIndex = 26;
            lblBuild.Text = "Build date";
            // 
            // linkLProjectSite
            // 
            linkLProjectSite.AutoSize = true;
            linkLProjectSite.Location = new Point(7, 373);
            linkLProjectSite.Margin = new Padding(7, 0, 4, 0);
            linkLProjectSite.Name = "linkLProjectSite";
            linkLProjectSite.Size = new Size(246, 15);
            linkLProjectSite.TabIndex = 1;
            linkLProjectSite.TabStop = true;
            linkLProjectSite.Text = "All project info at www.tolaemon.com/dpack";
            linkLProjectSite.LinkClicked += linkLblProjectSite_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(linkLblSoruce);
            panel2.Controls.Add(linkLblBeHex);
            panel2.Controls.Add(linkLblLicense);
            panel2.Controls.Add(lblLicense);
            panel2.Location = new Point(3, 397);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(485, 70);
            panel2.TabIndex = 27;
            // 
            // linkLblSoruce
            // 
            linkLblSoruce.AutoSize = true;
            linkLblSoruce.Location = new Point(4, 34);
            linkLblSoruce.Name = "linkLblSoruce";
            linkLblSoruce.Size = new Size(265, 15);
            linkLblSoruce.TabIndex = 3;
            linkLblSoruce.TabStop = true;
            linkLblSoruce.Text = "Source code is available at the GitHub repository.";
            linkLblSoruce.LinkClicked += linkLblSoruce_LinkClicked;
            // 
            // linkLblBeHex
            // 
            linkLblBeHex.AutoSize = true;
            linkLblBeHex.Location = new Point(4, 49);
            linkLblBeHex.Name = "linkLblBeHex";
            linkLblBeHex.Size = new Size(307, 15);
            linkLblBeHex.TabIndex = 2;
            linkLblBeHex.TabStop = true;
            linkLblBeHex.Text = "BeHex control by bernhardelbl is used in this application.";
            linkLblBeHex.Click += linkLblBeHex_Click;
            // 
            // linkLblLicense
            // 
            linkLblLicense.AutoSize = true;
            linkLblLicense.Location = new Point(4, 19);
            linkLblLicense.Name = "linkLblLicense";
            linkLblLicense.Size = new Size(291, 15);
            linkLblLicense.TabIndex = 1;
            linkLblLicense.TabStop = true;
            linkLblLicense.Text = "This software is distributed under by-nc-sa 4.0 license.";
            linkLblLicense.LinkClicked += linkLblLicense_LinkClicked;
            // 
            // lblLicense
            // 
            lblLicense.AutoSize = true;
            lblLicense.Location = new Point(4, 4);
            lblLicense.Name = "lblLicense";
            lblLicense.Size = new Size(46, 15);
            lblLicense.TabIndex = 0;
            lblLicense.Text = "License";
            // 
            // textBoxDescription
            // 
            textBoxDescription.Dock = DockStyle.Fill;
            textBoxDescription.Location = new Point(7, 473);
            textBoxDescription.Margin = new Padding(7, 3, 4, 3);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.ReadOnly = true;
            textBoxDescription.ScrollBars = ScrollBars.Both;
            textBoxDescription.Size = new Size(533, 178);
            textBoxDescription.TabIndex = 23;
            textBoxDescription.TabStop = false;
            textBoxDescription.Text = "Descripción";
            textBoxDescription.WordWrap = false;
            // 
            // linkLblAuthor
            // 
            linkLblAuthor.AutoSize = true;
            linkLblAuthor.Location = new Point(7, 351);
            linkLblAuthor.Margin = new Padding(7, 0, 4, 0);
            linkLblAuthor.Name = "linkLblAuthor";
            linkLblAuthor.Size = new Size(93, 15);
            linkLblAuthor.TabIndex = 2;
            linkLblAuthor.TabStop = true;
            linkLblAuthor.Text = "tolaemon - 2024";
            linkLblAuthor.Click += linkLblAuthor_Click;
            // 
            // AboutBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(564, 674);
            Controls.Add(tableLayoutPanel);
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MaximumSize = new Size(580, 985);
            MinimizeBox = false;
            MinimumSize = new Size(580, 535);
            Name = "AboutBox";
            Padding = new Padding(10);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About ...";
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel;
        private Label lblProductName;
        private Label lblVersion;
        private Panel panel1;
        private TextBox textBoxDescription;
        private Label lblBuild;
        private Panel panel2;
        private LinkLabel linkLblLicense;
        private Label lblLicense;
        private LinkLabel linkLblBeHex;
        private LinkLabel linkLblSoruce;
        private LinkLabel linkLblAuthor;
        private LinkLabel linkLProjectSite;
        internal PictureBox pictureBox1;
    }
}
