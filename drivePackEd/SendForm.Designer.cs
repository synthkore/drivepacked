
namespace drivePackEd {
    partial class SendForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            sendButton = new System.Windows.Forms.Button();
            comboBox1 = new System.Windows.Forms.ComboBox();
            cancelButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // sendButton
            // 
            sendButton.Location = new System.Drawing.Point(8, 106);
            sendButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            sendButton.Name = "sendButton";
            sendButton.Size = new System.Drawing.Size(82, 22);
            sendButton.TabIndex = 0;
            sendButton.Text = "Send";
            sendButton.UseVisualStyleBackColor = true;
            sendButton.Click += SendButton_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(102, 80);
            comboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(83, 23);
            comboBox1.TabIndex = 1;
            // 
            // cancelButton
            // 
            cancelButton.Location = new System.Drawing.Point(102, 106);
            cancelButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(82, 22);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 82);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(63, 15);
            label1.TabIndex = 3;
            label1.Text = "COM port:";
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(8, 54);
            progressBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(176, 12);
            progressBar1.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(9, 35);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(102, 15);
            label2.TabIndex = 5;
            label2.Text = "Tx progress ( 0% ):";
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(8, 4);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(185, 32);
            label3.TabIndex = 6;
            label3.Text = "Set drivePack unit in Receive mode and press Send.";
            // 
            // Form2
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(194, 139);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(progressBar1);
            Controls.Add(label1);
            Controls.Add(cancelButton);
            Controls.Add(comboBox1);
            Controls.Add(sendButton);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "Form2";
            Text = "Send";
            FormClosing += SendForm_Closing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}