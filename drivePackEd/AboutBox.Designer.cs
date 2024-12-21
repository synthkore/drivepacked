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
            picPanel = new Panel();
            picBoxLogo = new PictureBox();
            lblProductName = new Label();
            lblVersion = new Label();
            lblBuild = new Label();
            aboutWebBrowser = new WebBrowser();
            btnAccept = new Button();
            tableLayoutPanel.SuspendLayout();
            picPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBoxLogo).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(picPanel, 0, 0);
            tableLayoutPanel.Controls.Add(lblProductName, 0, 1);
            tableLayoutPanel.Controls.Add(lblVersion, 0, 2);
            tableLayoutPanel.Controls.Add(lblBuild, 0, 3);
            tableLayoutPanel.Controls.Add(aboutWebBrowser, 0, 4);
            tableLayoutPanel.Controls.Add(btnAccept, 0, 5);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(10, 10);
            tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 6;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 285F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel.Size = new Size(544, 705);
            tableLayoutPanel.TabIndex = 0;
            // 
            // picPanel
            // 
            picPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picPanel.Controls.Add(picBoxLogo);
            picPanel.Location = new Point(3, 3);
            picPanel.Name = "picPanel";
            picPanel.Size = new Size(538, 279);
            picPanel.TabIndex = 25;
            // 
            // picBoxLogo
            // 
            picBoxLogo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picBoxLogo.Image = Properties.Resources.dpacklogo;
            picBoxLogo.Location = new Point(4, 3);
            picBoxLogo.MaximumSize = new Size(550, 265);
            picBoxLogo.MinimumSize = new Size(459, 201);
            picBoxLogo.Name = "picBoxLogo";
            picBoxLogo.Size = new Size(531, 265);
            picBoxLogo.SizeMode = PictureBoxSizeMode.CenterImage;
            picBoxLogo.TabIndex = 0;
            picBoxLogo.TabStop = false;
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
            // aboutWebBrowser
            // 
            aboutWebBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            aboutWebBrowser.Location = new Point(3, 354);
            aboutWebBrowser.Name = "aboutWebBrowser";
            aboutWebBrowser.Size = new Size(538, 314);
            aboutWebBrowser.TabIndex = 0;
            aboutWebBrowser.Navigating += aboutWebBrowser_Navigating;
            // 
            // btnAccept
            // 
            btnAccept.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAccept.Location = new Point(423, 674);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(118, 28);
            btnAccept.TabIndex = 27;
            btnAccept.Text = "Accept";
            btnAccept.UseVisualStyleBackColor = true;
            btnAccept.Click += btnAccept_Click;
            // 
            // AboutBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(564, 725);
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
            picPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picBoxLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel;
        private Label lblProductName;
        private Label lblVersion;
        private Panel picPanel;
        private Label lblBuild;
        internal PictureBox picBoxLogo;
        private WebBrowser aboutWebBrowser;
        private Button btnAccept;
    }
}
