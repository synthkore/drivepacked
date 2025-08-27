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
            chkBxNoGenChBeginEnd = new System.Windows.Forms.CheckBox();
            cmbBoxM1Chan = new System.Windows.Forms.ComboBox();
            cmbBoxM2Chan = new System.Windows.Forms.ComboBox();
            cmbBoxChordChan = new System.Windows.Forms.ComboBox();
            btnImport = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            lblMetaData = new System.Windows.Forms.Label();
            cmbBoxMetaData = new System.Windows.Forms.ComboBox();
            lblMIDISrcTrack = new System.Windows.Forms.Label();
            lblInstrument = new System.Windows.Forms.Label();
            cmbBoxM2Instr = new System.Windows.Forms.ComboBox();
            cmbBoxM1Instr = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            cmbBoxChordRythm = new System.Windows.Forms.ComboBox();
            lblRythm = new System.Windows.Forms.Label();
            lblKey = new System.Windows.Forms.Label();
            cmbBoxTime = new System.Windows.Forms.ComboBox();
            lblTime = new System.Windows.Forms.Label();
            nUpDwnKey = new System.Windows.Forms.NumericUpDown();
            nUpDwnDiscrimination = new System.Windows.Forms.NumericUpDown();
            lblDiscrimination = new System.Windows.Forms.Label();
            nUpDwnTempo = new System.Windows.Forms.NumericUpDown();
            lblTempo = new System.Windows.Forms.Label();
            warnTextBox = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            lblC3MIDI=new System.Windows.Forms.Label();
            cmbBoxC3MIDI =new System.Windows.Forms.ComboBox();
            lblArrow1= new System.Windows.Forms.Label();
            lblArrow2 = new System.Windows.Forms.Label();
            btnCheckMIDI = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)nUpDwnKey).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nUpDwnDiscrimination).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nUpDwnTempo).BeginInit();
            SuspendLayout();
            // 
            // lblM1Chan
            // 
            lblM1Chan.AutoSize = true;
            lblM1Chan.Location = new System.Drawing.Point(42, 60);
            lblM1Chan.Name = "lblM1Chan";
            lblM1Chan.Size = new System.Drawing.Size(139, 15);
            lblM1Chan.TabIndex = 0;
            lblM1Chan.Text = "Melody 1 channel (main)";
            // 
            // lblM2Chan
            // 
            lblM2Chan.AutoSize = true;
            lblM2Chan.Location = new System.Drawing.Point(18, 87);
            lblM2Chan.Name = "lblM2Chan";
            lblM2Chan.Size = new System.Drawing.Size(163, 15);
            lblM2Chan.TabIndex = 1;
            lblM2Chan.Text = "Melody 2 channel (obbligato)";
            // 
            // lblChordsChan
            // 
            lblChordsChan.AutoSize = true;
            lblChordsChan.Location = new System.Drawing.Point(91, 114);
            lblChordsChan.Name = "lblChordsChan";
            lblChordsChan.Size = new System.Drawing.Size(90, 15);
            lblChordsChan.TabIndex = 2;
            lblChordsChan.Text = "Chords channel";
            // 
            // chkBxNoGenChBeginEnd
            // 
            chkBxNoGenChBeginEnd.AutoSize = true;
            chkBxNoGenChBeginEnd.Checked = true;
            chkBxNoGenChBeginEnd.CheckState = System.Windows.Forms.CheckState.Checked;
            chkBxNoGenChBeginEnd.Location = new System.Drawing.Point(16, 309);
            chkBxNoGenChBeginEnd.Name = "chkBxNoGenChBeginEnd";
            chkBxNoGenChBeginEnd.Size = new System.Drawing.Size(284, 19);
            chkBxNoGenChBeginEnd.TabIndex = 3;
            chkBxNoGenChBeginEnd.Text = "Do not generate channels beginning and ending.";
            chkBxNoGenChBeginEnd.UseVisualStyleBackColor = true;
            chkBxNoGenChBeginEnd.CheckedChanged += chkBxNoGenChBeginEnd_CheckedChanged;
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
            btnImport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnImport.Location = new System.Drawing.Point(222, 452);
            btnImport.Name = "btnImport";
            btnImport.Size = new System.Drawing.Size(82, 22);
            btnImport.TabIndex = 9;
            btnImport.Text = "Import MIDI";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(315, 452);
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
            lblMetaData.Location = new System.Drawing.Point(124, 141);
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
            // lblInstrument
            // 
            lblInstrument.Location = new System.Drawing.Point(304, 33);
            lblInstrument.Name = "lblInstrument";
            lblInstrument.Size = new System.Drawing.Size(78, 21);
            lblInstrument.TabIndex = 16;
            lblInstrument.Text = "Instrument:";
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
            cmbBoxChordRythm.Location = new System.Drawing.Point(187, 165);
            cmbBoxChordRythm.Name = "cmbBoxChordRythm";
            cmbBoxChordRythm.Size = new System.Drawing.Size(95, 23);
            cmbBoxChordRythm.TabIndex = 20;
            // 
            // lblRythm
            // 
            lblRythm.AutoSize = true;
            lblRythm.Location = new System.Drawing.Point(139, 168);
            lblRythm.Name = "lblRythm";
            lblRythm.Size = new System.Drawing.Size(42, 15);
            lblRythm.TabIndex = 21;
            lblRythm.Text = "Rythm";
            // 
            // lblKey
            // 
            lblKey.AutoSize = true;
            lblKey.Location = new System.Drawing.Point(154, 222);
            lblKey.Name = "lblKey";
            lblKey.Size = new System.Drawing.Size(26, 15);
            lblKey.TabIndex = 23;
            lblKey.Text = "Key";
            // 
            // cmbBoxTime
            // 
            cmbBoxTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxTime.FormattingEnabled = true;
            cmbBoxTime.Location = new System.Drawing.Point(187, 192);
            cmbBoxTime.Name = "cmbBoxTime";
            cmbBoxTime.Size = new System.Drawing.Size(95, 23);
            cmbBoxTime.TabIndex = 22;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Location = new System.Drawing.Point(148, 194);
            lblTime.Name = "lblTime";
            lblTime.Size = new System.Drawing.Size(33, 15);
            lblTime.TabIndex = 24;
            lblTime.Text = "Time";
            // 
            // nUpDwnKey
            // 
            nUpDwnKey.Location = new System.Drawing.Point(187, 220);
            nUpDwnKey.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDwnKey.Name = "nUpDwnKey";
            nUpDwnKey.Size = new System.Drawing.Size(95, 23);
            nUpDwnKey.TabIndex = 25;
            nUpDwnKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nUpDwnDiscrimination
            // 
            nUpDwnDiscrimination.Location = new System.Drawing.Point(187, 274);
            nUpDwnDiscrimination.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
            nUpDwnDiscrimination.Name = "nUpDwnDiscrimination";
            nUpDwnDiscrimination.Size = new System.Drawing.Size(95, 23);
            nUpDwnDiscrimination.TabIndex = 27;
            nUpDwnDiscrimination.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblDiscrimination
            // 
            lblDiscrimination.AutoSize = true;
            lblDiscrimination.Location = new System.Drawing.Point(16, 276);
            lblDiscrimination.Name = "lblDiscrimination";
            lblDiscrimination.Size = new System.Drawing.Size(165, 15);
            lblDiscrimination.TabIndex = 26;
            lblDiscrimination.Text = "Discrimination (quarter notes)";
            // 
            // nUpDwnTempo
            // 
            nUpDwnTempo.Location = new System.Drawing.Point(187, 247);
            nUpDwnTempo.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDwnTempo.Name = "nUpDwnTempo";
            nUpDwnTempo.Size = new System.Drawing.Size(95, 23);
            nUpDwnTempo.TabIndex = 29;
            nUpDwnTempo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTempo
            // 
            lblTempo.AutoSize = true;
            lblTempo.Location = new System.Drawing.Point(139, 249);
            lblTempo.Name = "lblTempo";
            lblTempo.Size = new System.Drawing.Size(43, 15);
            lblTempo.TabIndex = 28;
            lblTempo.Text = "Tempo";
            // 
            // warnTextBox
            // 
            warnTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            warnTextBox.Location = new System.Drawing.Point(12, 358);
            warnTextBox.Multiline = true;
            warnTextBox.Name = "warnTextBox";
            warnTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            warnTextBox.Size = new System.Drawing.Size(385, 88);
            warnTextBox.TabIndex = 30;
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(12, 337);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(157, 21);
            label2.TabIndex = 31;
            label2.Text = "Selected MIDI file warnings:";
            // 
            // lblC3MIDI
            // 
            lblC3MIDI.Location = new System.Drawing.Point(301, 116);
            lblC3MIDI.Name = "lblC3MIDI";
            lblC3MIDI.Size = new System.Drawing.Size(96, 14);
            lblC3MIDI.TabIndex = 32;
            lblC3MIDI.Text = "C3 MIDI note:";
            // 
            // cmbBoxC3MIDI
            // 
            cmbBoxC3MIDI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbBoxC3MIDI.FormattingEnabled = true;
            cmbBoxC3MIDI.Location = new System.Drawing.Point(300, 138);
            cmbBoxC3MIDI.Name = "cmbBoxC3MIDI";
            cmbBoxC3MIDI.Size = new System.Drawing.Size(95, 23);
            cmbBoxC3MIDI.TabIndex = 33;
            // 
            // lblArrow1
            // 
            lblArrow1.Location = new System.Drawing.Point(285, 61);
            lblArrow1.Name = "lblArrow1";
            lblArrow1.Size = new System.Drawing.Size(13, 21);
            lblArrow1.TabIndex = 34;
            lblArrow1.Text = ">";
            // 
            // lblArrow2
            // 
            lblArrow2.Location = new System.Drawing.Point(285, 86);
            lblArrow2.Name = "lblArrow2";
            lblArrow2.Size = new System.Drawing.Size(13, 21);
            lblArrow2.TabIndex = 35;
            lblArrow2.Text = ">";
            // 
            // btnCheckMIDI
            // 
            btnCheckMIDI.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnCheckMIDI.Location = new System.Drawing.Point(12, 452);
            btnCheckMIDI.Name = "btnCheckMIDI";
            btnCheckMIDI.Size = new System.Drawing.Size(82, 22);
            btnCheckMIDI.TabIndex = 36;
            btnCheckMIDI.Text = "Check MIDI";
            btnCheckMIDI.UseVisualStyleBackColor = true;
            btnCheckMIDI.Click += btnCheckMIDI_Click;
            // 
            // MIDIimportForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(409, 486);
            Controls.Add(btnCheckMIDI);
            Controls.Add(lblArrow2);
            Controls.Add(lblArrow1);
            Controls.Add(cmbBoxC3MIDI);
            Controls.Add(lblC3MIDI);
            Controls.Add(label2);
            Controls.Add(warnTextBox);
            Controls.Add(nUpDwnTempo);
            Controls.Add(lblTempo);
            Controls.Add(nUpDwnDiscrimination);
            Controls.Add(lblDiscrimination);
            Controls.Add(nUpDwnKey);
            Controls.Add(lblTime);
            Controls.Add(lblKey);
            Controls.Add(cmbBoxTime);
            Controls.Add(lblRythm);
            Controls.Add(cmbBoxChordRythm);
            Controls.Add(label1);
            Controls.Add(cmbBoxM2Instr);
            Controls.Add(cmbBoxM1Instr);
            Controls.Add(lblInstrument);
            Controls.Add(lblMIDISrcTrack);
            Controls.Add(cmbBoxMetaData);
            Controls.Add(lblMetaData);
            Controls.Add(btnCancel);
            Controls.Add(btnImport);
            Controls.Add(cmbBoxChordChan);
            Controls.Add(cmbBoxM2Chan);
            Controls.Add(cmbBoxM1Chan);
            Controls.Add(chkBxNoGenChBeginEnd);
            Controls.Add(lblChordsChan);
            Controls.Add(lblM2Chan);
            Controls.Add(lblM1Chan);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            MinimumSize = new System.Drawing.Size(425, 525);
            Name = "MIDIimportForm";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "MIDI import options";
            FormClosing += MIDIimportForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)nUpDwnKey).EndInit();
            ((System.ComponentModel.ISupportInitialize)nUpDwnDiscrimination).EndInit();
            ((System.ComponentModel.ISupportInitialize)nUpDwnTempo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblM1Chan;
        private System.Windows.Forms.Label lblM2Chan;
        private System.Windows.Forms.Label lblChordsChan;
        private System.Windows.Forms.CheckBox chkBxNoGenChBeginEnd;
        private System.Windows.Forms.ComboBox cmbBoxM1Chan;
        private System.Windows.Forms.ComboBox cmbBoxM2Chan;
        private System.Windows.Forms.ComboBox cmbBoxChordChan;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblMetaData;
        private System.Windows.Forms.ComboBox cmbBoxMetaData;
        private System.Windows.Forms.Label lblMIDISrcTrack;
        private System.Windows.Forms.Label lblInstrument;
        private System.Windows.Forms.ComboBox cmbBoxM2Instr;
        private System.Windows.Forms.ComboBox cmbBoxM1Instr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBoxChordRythm;
        private System.Windows.Forms.Label lblRythm;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.ComboBox cmbBoxTime;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.NumericUpDown nUpDwnKey;
        private System.Windows.Forms.NumericUpDown nUpDwnDiscrimination;
        private System.Windows.Forms.Label lblDiscrimination;
        private System.Windows.Forms.NumericUpDown nUpDwnTempo;
        private System.Windows.Forms.Label lblTempo;
        private System.Windows.Forms.TextBox warnTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblC3MIDI;
        private System.Windows.Forms.ComboBox cmbBoxC3MIDI;
        private System.Windows.Forms.Label lblArrow1;
        private System.Windows.Forms.Label lblArrow2;
        private System.Windows.Forms.Button btnCheckMIDI;
    }
}