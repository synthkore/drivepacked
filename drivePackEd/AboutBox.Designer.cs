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
            tableLayoutPanel.Location = new Point(11, 13);
            tableLayoutPanel.Margin = new Padding(5, 4, 5, 4);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 8;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 300F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Size = new Size(560, 680);
            tableLayoutPanel.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(3, 4);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(554, 292);
            panel1.TabIndex = 25;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = Properties.Resources.dpacklogo;
            pictureBox1.Location = new Point(15, 12);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.MaximumSize = new Size(525, 268);
            pictureBox1.MinimumSize = new Size(525, 268);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(525, 268);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lblProductName
            // 
            lblProductName.Dock = DockStyle.Fill;
            lblProductName.Location = new Point(8, 300);
            lblProductName.Margin = new Padding(8, 0, 5, 0);
            lblProductName.MaximumSize = new Size(0, 27);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(547, 27);
            lblProductName.TabIndex = 19;
            lblProductName.Text = "Nombre de producto";
            lblProductName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVersion
            // 
            lblVersion.Dock = DockStyle.Fill;
            lblVersion.Location = new Point(8, 330);
            lblVersion.Margin = new Padding(8, 0, 5, 0);
            lblVersion.MaximumSize = new Size(0, 27);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(547, 27);
            lblVersion.TabIndex = 0;
            lblVersion.Text = "Versión";
            lblVersion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBuild
            // 
            lblBuild.Dock = DockStyle.Fill;
            lblBuild.Location = new Point(8, 363);
            lblBuild.Margin = new Padding(8, 3, 5, 0);
            lblBuild.MaximumSize = new Size(0, 27);
            lblBuild.Name = "lblBuild";
            lblBuild.Size = new Size(547, 27);
            lblBuild.TabIndex = 26;
            lblBuild.Text = "Build date";
            // 
            // linkLProjectSite
            // 
            linkLProjectSite.AutoSize = true;
            linkLProjectSite.Location = new Point(8, 420);
            linkLProjectSite.Margin = new Padding(8, 0, 5, 0);
            linkLProjectSite.Name = "linkLProjectSite";
            linkLProjectSite.Size = new Size(307, 20);
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
            panel2.Location = new Point(3, 453);
            panel2.Name = "panel2";
            panel2.Size = new Size(554, 94);
            panel2.TabIndex = 27;
            // 
            // linkLblSoruce
            // 
            linkLblSoruce.AutoSize = true;
            linkLblSoruce.Location = new Point(5, 45);
            linkLblSoruce.Name = "linkLblSoruce";
            linkLblSoruce.Size = new Size(336, 20);
            linkLblSoruce.TabIndex = 3;
            linkLblSoruce.TabStop = true;
            linkLblSoruce.Text = "Source code is available at the GitHub repository.";
            linkLblSoruce.LinkClicked += linkLblSoruce_LinkClicked;
            // 
            // linkLblBeHex
            // 
            linkLblBeHex.AutoSize = true;
            linkLblBeHex.Location = new Point(5, 65);
            linkLblBeHex.Name = "linkLblBeHex";
            linkLblBeHex.Size = new Size(386, 20);
            linkLblBeHex.TabIndex = 2;
            linkLblBeHex.TabStop = true;
            linkLblBeHex.Text = "BeHex control by bernhardelbl is used in this application.";
            linkLblBeHex.Click += linkLblBeHex_Click;
            // 
            // linkLblLicense
            // 
            linkLblLicense.AutoSize = true;
            linkLblLicense.Location = new Point(5, 25);
            linkLblLicense.Name = "linkLblLicense";
            linkLblLicense.Size = new Size(364, 20);
            linkLblLicense.TabIndex = 1;
            linkLblLicense.TabStop = true;
            linkLblLicense.Text = "This software is distributed under by-nc-sa 4.0 license.";
            linkLblLicense.LinkClicked += linkLblLicense_LinkClicked;
            // 
            // lblLicense
            // 
            lblLicense.AutoSize = true;
            lblLicense.Location = new Point(5, 5);
            lblLicense.Name = "lblLicense";
            lblLicense.Size = new Size(57, 20);
            lblLicense.TabIndex = 0;
            lblLicense.Text = "License";
            // 
            // textBoxDescription
            // 
            textBoxDescription.Dock = DockStyle.Fill;
            textBoxDescription.Location = new Point(8, 554);
            textBoxDescription.Margin = new Padding(8, 4, 5, 4);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.ReadOnly = true;
            textBoxDescription.ScrollBars = ScrollBars.Both;
            textBoxDescription.Size = new Size(547, 122);
            textBoxDescription.TabIndex = 23;
            textBoxDescription.TabStop = false;
            textBoxDescription.Text = "Descripción";
            textBoxDescription.WordWrap = false;
            // 
            // linkLblAuthor
            // 
            linkLblAuthor.AutoSize = true;
            linkLblAuthor.Location = new Point(8, 390);
            linkLblAuthor.Margin = new Padding(8, 0, 5, 0);
            linkLblAuthor.Name = "linkLblAuthor";
            linkLblAuthor.Size = new Size(119, 20);
            linkLblAuthor.TabIndex = 2;
            linkLblAuthor.TabStop = true;
            linkLblAuthor.Text = "tolaemon - 2024";
            linkLblAuthor.Click += linkLblAuthor_Click;
            // 
            // AboutBox
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(582, 706);
            Controls.Add(tableLayoutPanel);
            Margin = new Padding(5, 4, 5, 4);
            MaximizeBox = false;
            MaximumSize = new Size(600, 1300);
            MinimizeBox = false;
            MinimumSize = new Size(600, 700);
            Name = "AboutBox";
            Padding = new Padding(11, 13, 11, 13);
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
        public PictureBox pictureBox1;
        private Panel panel2;
        private LinkLabel linkLblLicense;
        private Label lblLicense;
        private LinkLabel linkLblBeHex;
        private LinkLabel linkLblSoruce;
        private LinkLabel linkLblAuthor;
        private LinkLabel linkLProjectSite;
    }
}
