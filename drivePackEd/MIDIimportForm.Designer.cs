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
            lblMIDISrcTrack = new System.Windows.Forms.Label();
            chkBxDiscrimination = new System.Windows.Forms.CheckBox();
            chkBxGetTempo = new System.Windows.Forms.CheckBox();
            lblInstrument = new System.Windows.Forms.Label();
            cmbBoxM2Instr = new System.Windows.Forms.ComboBox();
            cmbBoxM1Instr = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            cmbBoxChordRythm = new System.Windows.Forms.ComboBox();
            SuspendLayout();
            // 
            // lblM1Chan
            // 
            lblM1Chan.AutoSize = true;
            lblM1Chan.Location = new System.Drawing.Point(30, 60);
            lblM1Chan.Name = "lblM1Chan";
            lblM1Chan.Size = new System.Drawing.Size(145, 15);
            lblM1Chan.TabIndex = 0;
            lblM1Chan.Text = "Melody 1 channel ( main )";
            // 
            // lblM2Chan
            // 
            lblM2Chan.AutoSize = true;
            lblM2Chan.Location = new System.Drawing.Point(6, 87);
            lblM2Chan.Name = "lblM2Chan";
            lblM2Chan.Size = new System.Drawing.Size(169, 15);
            lblM2Chan.TabIndex = 1;
            lblM2Chan.Text = "Melody 2 channel ( obbligato )";
            // 
            // lblChordsChan
            // 
            lblChordsChan.AutoSize = true;
            lblChordsChan.Location = new System.Drawing.Point(85, 114);
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
            chkBxGenChBeginEnd.Location = new System.Drawing.Point(7, 174);
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
            cmbBoxM1Chan.Location = new System.Drawing.Point(187, 57);
            cmbBoxM1Chan.Name = "cmbBoxM1Chan";
            cmbBoxM1Chan.Size = new System.Drawing.Size(95, 23);
            cmbBoxM1Chan.TabIndex = 4;
            // 
            // cmbBoxM2Chan
            // 
            cmbBoxM2Chan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxM2Chan.FormattingEnabled = true;
            cmbBoxM2Chan.Location = new System.Drawing.Point(187, 84);
            cmbBoxM2Chan.Name = "cmbBoxM2Chan";
            cmbBoxM2Chan.Size = new System.Drawing.Size(95, 23);
            cmbBoxM2Chan.TabIndex = 5;
            // 
            // cmbBoxChordChan
            // 
            cmbBoxChordChan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxChordChan.FormattingEnabled = true;
            cmbBoxChordChan.Location = new System.Drawing.Point(187, 111);
            cmbBoxChordChan.Name = "cmbBoxChordChan";
            cmbBoxChordChan.Size = new System.Drawing.Size(95, 23);
            cmbBoxChordChan.TabIndex = 6;
            // 
            // btnImport
            // 
            btnImport.Location = new System.Drawing.Point(211, 242);
            btnImport.Name = "btnImport";
            btnImport.Size = new System.Drawing.Size(82, 22);
            btnImport.TabIndex = 9;
            btnImport.Text = "Import MIDI";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(304, 242);
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
            lblMetaData.Location = new System.Drawing.Point(118, 141);
            lblMetaData.Name = "lblMetaData";
            lblMetaData.Size = new System.Drawing.Size(57, 15);
            lblMetaData.TabIndex = 11;
            lblMetaData.Text = "Metadata";
            // 
            // cmbBoxMetaData
            // 
            cmbBoxMetaData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxMetaData.FormattingEnabled = true;
            cmbBoxMetaData.Location = new System.Drawing.Point(187, 138);
            cmbBoxMetaData.Name = "cmbBoxMetaData";
            cmbBoxMetaData.Size = new System.Drawing.Size(95, 23);
            cmbBoxMetaData.TabIndex = 12;
            // 
            // lblMIDISrcTrack
            // 
            lblMIDISrcTrack.Location = new System.Drawing.Point(185, 33);
            lblMIDISrcTrack.Name = "lblMIDISrcTrack";
            lblMIDISrcTrack.Size = new System.Drawing.Size(108, 21);
            lblMIDISrcTrack.TabIndex = 13;
            lblMIDISrcTrack.Text = "MIDI track source:";
            // 
            // chkBxDiscrimination
            // 
            chkBxDiscrimination.AutoSize = true;
            chkBxDiscrimination.Checked = true;
            chkBxDiscrimination.CheckState = System.Windows.Forms.CheckState.Checked;
            chkBxDiscrimination.Location = new System.Drawing.Point(7, 218);
            chkBxDiscrimination.Name = "chkBxDiscrimination";
            chkBxDiscrimination.Size = new System.Drawing.Size(162, 19);
            chkBxDiscrimination.TabIndex = 14;
            chkBxDiscrimination.Text = "Add rythm discrimination";
            chkBxDiscrimination.UseVisualStyleBackColor = true;
            // 
            // chkBxGetTempo
            // 
            chkBxGetTempo.AutoSize = true;
            chkBxGetTempo.Checked = true;
            chkBxGetTempo.CheckState = System.Windows.Forms.CheckState.Checked;
            chkBxGetTempo.Location = new System.Drawing.Point(6, 196);
            chkBxGetTempo.Name = "chkBxGetTempo";
            chkBxGetTempo.Size = new System.Drawing.Size(179, 19);
            chkBxGetTempo.TabIndex = 15;
            chkBxGetTempo.Text = "Use file timming information";
            chkBxGetTempo.UseVisualStyleBackColor = true;
            // 
            // lblInstrument
            // 
            lblInstrument.Location = new System.Drawing.Point(291, 33);
            lblInstrument.Name = "lblInstrument";
            lblInstrument.Size = new System.Drawing.Size(108, 21);
            lblInstrument.TabIndex = 16;
            lblInstrument.Text = "Instrument/Rythm:";
            // 
            // cmbBoxM2Instr
            // 
            cmbBoxM2Instr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxM2Instr.FormattingEnabled = true;
            cmbBoxM2Instr.Location = new System.Drawing.Point(301, 84);
            cmbBoxM2Instr.Name = "cmbBoxM2Instr";
            cmbBoxM2Instr.Size = new System.Drawing.Size(95, 23);
            cmbBoxM2Instr.TabIndex = 18;
            // 
            // cmbBoxM1Instr
            // 
            cmbBoxM1Instr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxM1Instr.FormattingEnabled = true;
            cmbBoxM1Instr.Location = new System.Drawing.Point(301, 57);
            cmbBoxM1Instr.Name = "cmbBoxM1Instr";
            cmbBoxM1Instr.Size = new System.Drawing.Size(95, 23);
            cmbBoxM1Instr.TabIndex = 17;
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(157, 21);
            label1.TabIndex = 19;
            label1.Text = "Set MIDI import options:";
            // 
            // cmbBoxChordRythm
            // 
            cmbBoxChordRythm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxChordRythm.FormattingEnabled = true;
            cmbBoxChordRythm.Location = new System.Drawing.Point(301, 111);
            cmbBoxChordRythm.Name = "cmbBoxChordRythm";
            cmbBoxChordRythm.Size = new System.Drawing.Size(95, 23);
            cmbBoxChordRythm.TabIndex = 20;
            // 
            // MIDIimportForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(412, 274);
            Controls.Add(cmbBoxChordRythm);
            Controls.Add(label1);
            Controls.Add(cmbBoxM2Instr);
            Controls.Add(cmbBoxM1Instr);
            Controls.Add(lblInstrument);
            Controls.Add(chkBxGetTempo);
            Controls.Add(chkBxDiscrimination);
            Controls.Add(lblMIDISrcTrack);
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
        private System.Windows.Forms.Label lblMIDISrcTrack;
        private System.Windows.Forms.CheckBox chkBxDiscrimination;
        private System.Windows.Forms.CheckBox chkBxGetTempo;
        private System.Windows.Forms.Label lblInstrument;
        private System.Windows.Forms.ComboBox cmbBoxM2Instr;
        private System.Windows.Forms.ComboBox cmbBoxM1Instr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBoxChordRythm;
    }
}