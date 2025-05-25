namespace drivePackEd {
    partial class MIDIimportForm {
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
            lblM1Chan = new System.Windows.Forms.Label();
            lblM2Chan = new System.Windows.Forms.Label();
            lblChordsChan = new System.Windows.Forms.Label();
            chkBxGenChBeginEnd = new System.Windows.Forms.CheckBox();
            cmbBoxM1Chan = new System.Windows.Forms.ComboBox();
            cmbBoxM2Chan = new System.Windows.Forms.ComboBox();
            cmbBoxChordChan = new System.Windows.Forms.ComboBox();
            btnImport = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            lblMetaData = new System.Windows.Forms.Label();
            cmbBoxMetaData = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // lblM1Chan
            // 
            lblM1Chan.AutoSize = true;
            lblM1Chan.Location = new System.Drawing.Point(35, 49);
            lblM1Chan.Name = "lblM1Chan";
            lblM1Chan.Size = new System.Drawing.Size(145, 15);
            lblM1Chan.TabIndex = 0;
            lblM1Chan.Text = "Melody 1 channel ( main )";
            // 
            // lblM2Chan
            // 
            lblM2Chan.AutoSize = true;
            lblM2Chan.Location = new System.Drawing.Point(11, 76);
            lblM2Chan.Name = "lblM2Chan";
            lblM2Chan.Size = new System.Drawing.Size(169, 15);
            lblM2Chan.TabIndex = 1;
            lblM2Chan.Text = "Melody 2 channel ( obbligato )";
            // 
            // lblChordsChan
            // 
            lblChordsChan.AutoSize = true;
            lblChordsChan.Location = new System.Drawing.Point(90, 103);
            lblChordsChan.Name = "lblChordsChan";
            lblChordsChan.Size = new System.Drawing.Size(90, 15);
            lblChordsChan.TabIndex = 2;
            lblChordsChan.Text = "Chords channel";
            // 
            // chkBxGenChBeginEnd
            // 
            chkBxGenChBeginEnd.AutoSize = true;
            chkBxGenChBeginEnd.Checked = true;
            chkBxGenChBeginEnd.CheckState = System.Windows.Forms.CheckState.Checked;
            chkBxGenChBeginEnd.Location = new System.Drawing.Point(12, 158);
            chkBxGenChBeginEnd.Name = "chkBxGenChBeginEnd";
            chkBxGenChBeginEnd.Size = new System.Drawing.Size(241, 19);
            chkBxGenChBeginEnd.TabIndex = 3;
            chkBxGenChBeginEnd.Text = "Generate channel's instructions structure";
            chkBxGenChBeginEnd.UseVisualStyleBackColor = true;
            // 
            // cmbBoxM1Chan
            // 
            cmbBoxM1Chan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxM1Chan.FormattingEnabled = true;
            cmbBoxM1Chan.Location = new System.Drawing.Point(192, 46);
            cmbBoxM1Chan.Name = "cmbBoxM1Chan";
            cmbBoxM1Chan.Size = new System.Drawing.Size(95, 23);
            cmbBoxM1Chan.TabIndex = 4;
            // 
            // cmbBoxM2Chan
            // 
            cmbBoxM2Chan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxM2Chan.FormattingEnabled = true;
            cmbBoxM2Chan.Location = new System.Drawing.Point(192, 73);
            cmbBoxM2Chan.Name = "cmbBoxM2Chan";
            cmbBoxM2Chan.Size = new System.Drawing.Size(95, 23);
            cmbBoxM2Chan.TabIndex = 5;
            // 
            // cmbBoxChordChan
            // 
            cmbBoxChordChan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxChordChan.FormattingEnabled = true;
            cmbBoxChordChan.Location = new System.Drawing.Point(192, 100);
            cmbBoxChordChan.Name = "cmbBoxChordChan";
            cmbBoxChordChan.Size = new System.Drawing.Size(95, 23);
            cmbBoxChordChan.TabIndex = 6;
            // 
            // btnImport
            // 
            btnImport.Location = new System.Drawing.Point(117, 183);
            btnImport.Name = "btnImport";
            btnImport.Size = new System.Drawing.Size(82, 22);
            btnImport.TabIndex = 9;
            btnImport.Text = "Import MIDI";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(205, 183);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(82, 22);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblMetaData
            // 
            lblMetaData.AutoSize = true;
            lblMetaData.Location = new System.Drawing.Point(123, 130);
            lblMetaData.Name = "lblMetaData";
            lblMetaData.Size = new System.Drawing.Size(57, 15);
            lblMetaData.TabIndex = 11;
            lblMetaData.Text = "Metadata";
            // 
            // cmbBoxMetaData
            // 
            cmbBoxMetaData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxMetaData.FormattingEnabled = true;
            cmbBoxMetaData.Location = new System.Drawing.Point(192, 127);
            cmbBoxMetaData.Name = "cmbBoxMetaData";
            cmbBoxMetaData.Size = new System.Drawing.Size(95, 23);
            cmbBoxMetaData.TabIndex = 12;
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(7, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(302, 35);
            label1.TabIndex = 13;
            label1.Text = "Select the MIDI file track to use as source for each theme channel:";
            // 
            // MIDIimportForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(299, 216);
            Controls.Add(label1);
            Controls.Add(cmbBoxMetaData);
            Controls.Add(lblMetaData);
            Controls.Add(btnCancel);
            Controls.Add(btnImport);
            Controls.Add(cmbBoxChordChan);
            Controls.Add(cmbBoxM2Chan);
            Controls.Add(cmbBoxM1Chan);
            Controls.Add(chkBxGenChBeginEnd);
            Controls.Add(lblChordsChan);
            Controls.Add(lblM2Chan);
            Controls.Add(lblM1Chan);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "MIDIimportForm";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "MIDI import options";
            FormClosing += MIDIimportForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblM1Chan;
        private System.Windows.Forms.Label lblM2Chan;
        private System.Windows.Forms.Label lblChordsChan;
        private System.Windows.Forms.CheckBox chkBxGenChBeginEnd;
        private System.Windows.Forms.ComboBox cmbBoxM1Chan;
        private System.Windows.Forms.ComboBox cmbBoxM2Chan;
        private System.Windows.Forms.ComboBox cmbBoxChordChan;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblMetaData;
        private System.Windows.Forms.ComboBox cmbBoxMetaData;
        private System.Windows.Forms.Label label1;
    }
}