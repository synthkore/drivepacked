
namespace drivePackEd
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            tabControl2 = new System.Windows.Forms.TabControl();
            tabPage4 = new System.Windows.Forms.TabPage();
            romTitleTextBox = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            clearInfoButton = new System.Windows.Forms.Button();
            romInfoTextBox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            tabPage3 = new System.Windows.Forms.TabPage();
            tabPage1 = new System.Windows.Forms.TabPage();
            totalSongsLabel = new System.Windows.Forms.Label();
            disassemblyButton = new System.Windows.Forms.Button();
            label5 = new System.Windows.Forms.Label();
            sequenceTitleTextBox = new System.Windows.Forms.TextBox();
            buildButton = new System.Windows.Forms.Button();
            swapChordEntriesButton = new System.Windows.Forms.Button();
            delChordEntryButton = new System.Windows.Forms.Button();
            addChordEntryButton = new System.Windows.Forms.Button();
            swaplM2EntriesButton = new System.Windows.Forms.Button();
            delM2EntryButton = new System.Windows.Forms.Button();
            addM2EntryButton = new System.Windows.Forms.Button();
            swapM1EntriesButton = new System.Windows.Forms.Button();
            delM1EntryButton = new System.Windows.Forms.Button();
            addM1EntryButton = new System.Windows.Forms.Button();
            label8 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            chordsDataGridView = new System.Windows.Forms.DataGridView();
            melody2DataGridView = new System.Windows.Forms.DataGridView();
            melody1DataGridView = new System.Windows.Forms.DataGridView();
            label4 = new System.Windows.Forms.Label();
            swapThemeButton = new System.Windows.Forms.Button();
            sequenceSelectComboBox = new System.Windows.Forms.ComboBox();
            addSequenceButton = new System.Windows.Forms.Button();
            delSequenceButton = new System.Windows.Forms.Button();
            tabPage2 = new System.Windows.Forms.TabPage();
            button2 = new System.Windows.Forms.Button();
            textBox2 = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            toolStripFile = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            openSongsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveSongsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveSongsAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            openROMStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveROMStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveROMAsStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            receiveStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sendStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            tabControl2.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chordsDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)melody2DataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)melody1DataGridView).BeginInit();
            tabPage2.SuspendLayout();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl2
            // 
            tabControl2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl2.Controls.Add(tabPage4);
            tabControl2.Controls.Add(tabPage3);
            tabControl2.Controls.Add(tabPage1);
            tabControl2.Controls.Add(tabPage2);
            tabControl2.Location = new System.Drawing.Point(11, 27);
            tabControl2.Name = "tabControl2";
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new System.Drawing.Size(1129, 673);
            tabControl2.TabIndex = 2;
            // 
            // tabPage4
            // 
            tabPage4.BackColor = System.Drawing.SystemColors.Control;
            tabPage4.Controls.Add(romTitleTextBox);
            tabPage4.Controls.Add(label3);
            tabPage4.Controls.Add(clearInfoButton);
            tabPage4.Controls.Add(romInfoTextBox);
            tabPage4.Controls.Add(label1);
            tabPage4.Location = new System.Drawing.Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new System.Windows.Forms.Padding(3);
            tabPage4.Size = new System.Drawing.Size(1121, 640);
            tabPage4.TabIndex = 1;
            tabPage4.Text = "ROM info";
            // 
            // romTitleTextBox
            // 
            romTitleTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            romTitleTextBox.Location = new System.Drawing.Point(11, 28);
            romTitleTextBox.Multiline = true;
            romTitleTextBox.Name = "romTitleTextBox";
            romTitleTextBox.Size = new System.Drawing.Size(1104, 31);
            romTitleTextBox.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(11, 5);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(41, 20);
            label3.TabIndex = 3;
            label3.Text = "Title:";
            // 
            // clearInfoButton
            // 
            clearInfoButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            clearInfoButton.Location = new System.Drawing.Point(11, 605);
            clearInfoButton.Name = "clearInfoButton";
            clearInfoButton.Size = new System.Drawing.Size(94, 29);
            clearInfoButton.TabIndex = 2;
            clearInfoButton.Text = "Clear";
            clearInfoButton.UseVisualStyleBackColor = true;
            clearInfoButton.Click += button3_Click;
            // 
            // romInfoTextBox
            // 
            romInfoTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            romInfoTextBox.Location = new System.Drawing.Point(11, 83);
            romInfoTextBox.Multiline = true;
            romInfoTextBox.Name = "romInfoTextBox";
            romInfoTextBox.Size = new System.Drawing.Size(1104, 516);
            romInfoTextBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 60);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(38, 20);
            label1.TabIndex = 0;
            label1.Text = "Info:";
            // 
            // tabPage3
            // 
            tabPage3.BackColor = System.Drawing.SystemColors.Control;
            tabPage3.Location = new System.Drawing.Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new System.Windows.Forms.Padding(3);
            tabPage3.Size = new System.Drawing.Size(1121, 640);
            tabPage3.TabIndex = 0;
            tabPage3.Text = "ROM";
            // 
            // tabPage1
            // 
            tabPage1.BackColor = System.Drawing.SystemColors.Control;
            tabPage1.Controls.Add(totalSongsLabel);
            tabPage1.Controls.Add(disassemblyButton);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(sequenceTitleTextBox);
            tabPage1.Controls.Add(buildButton);
            tabPage1.Controls.Add(swapChordEntriesButton);
            tabPage1.Controls.Add(delChordEntryButton);
            tabPage1.Controls.Add(addChordEntryButton);
            tabPage1.Controls.Add(swaplM2EntriesButton);
            tabPage1.Controls.Add(delM2EntryButton);
            tabPage1.Controls.Add(addM2EntryButton);
            tabPage1.Controls.Add(swapM1EntriesButton);
            tabPage1.Controls.Add(delM1EntryButton);
            tabPage1.Controls.Add(addM1EntryButton);
            tabPage1.Controls.Add(label8);
            tabPage1.Controls.Add(label7);
            tabPage1.Controls.Add(label6);
            tabPage1.Controls.Add(chordsDataGridView);
            tabPage1.Controls.Add(melody2DataGridView);
            tabPage1.Controls.Add(melody1DataGridView);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(swapThemeButton);
            tabPage1.Controls.Add(sequenceSelectComboBox);
            tabPage1.Controls.Add(addSequenceButton);
            tabPage1.Controls.Add(delSequenceButton);
            tabPage1.Location = new System.Drawing.Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new System.Drawing.Size(1121, 640);
            tabPage1.TabIndex = 2;
            tabPage1.Text = "Sequence";
            // 
            // totalSongsLabel
            // 
            totalSongsLabel.AutoSize = true;
            totalSongsLabel.Location = new System.Drawing.Point(130, 11);
            totalSongsLabel.Name = "totalSongsLabel";
            totalSongsLabel.Size = new System.Drawing.Size(57, 20);
            totalSongsLabel.TabIndex = 39;
            totalSongsLabel.Text = "Total: 0";
            // 
            // disassemblyButton
            // 
            disassemblyButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            disassemblyButton.Location = new System.Drawing.Point(927, 6);
            disassemblyButton.Name = "disassemblyButton";
            disassemblyButton.Size = new System.Drawing.Size(87, 29);
            disassemblyButton.TabIndex = 38;
            disassemblyButton.Text = "Dissy";
            disassemblyButton.UseVisualStyleBackColor = true;
            disassemblyButton.Click += decodeButton_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(8, 39);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(41, 20);
            label5.TabIndex = 37;
            label5.Text = "Title:";
            // 
            // sequenceTitleTextBox
            // 
            sequenceTitleTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            sequenceTitleTextBox.Location = new System.Drawing.Point(8, 61);
            sequenceTitleTextBox.Multiline = true;
            sequenceTitleTextBox.Name = "sequenceTitleTextBox";
            sequenceTitleTextBox.Size = new System.Drawing.Size(1099, 31);
            sequenceTitleTextBox.TabIndex = 36;
            // 
            // buildButton
            // 
            buildButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buildButton.Location = new System.Drawing.Point(1020, 6);
            buildButton.Name = "buildButton";
            buildButton.Size = new System.Drawing.Size(87, 29);
            buildButton.TabIndex = 35;
            buildButton.Text = "Build";
            buildButton.UseVisualStyleBackColor = true;
            buildButton.Click += buildButton_Click;
            // 
            // swapChordEntriesButton
            // 
            swapChordEntriesButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            swapChordEntriesButton.Location = new System.Drawing.Point(887, 608);
            swapChordEntriesButton.Name = "swapChordEntriesButton";
            swapChordEntriesButton.Size = new System.Drawing.Size(55, 29);
            swapChordEntriesButton.TabIndex = 34;
            swapChordEntriesButton.Text = "Swap";
            swapChordEntriesButton.UseVisualStyleBackColor = true;
            swapChordEntriesButton.Click += swapChordEntriesButton_Click;
            // 
            // delChordEntryButton
            // 
            delChordEntryButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            delChordEntryButton.Location = new System.Drawing.Point(826, 608);
            delChordEntryButton.Name = "delChordEntryButton";
            delChordEntryButton.Size = new System.Drawing.Size(55, 29);
            delChordEntryButton.TabIndex = 33;
            delChordEntryButton.Text = "Del";
            delChordEntryButton.UseVisualStyleBackColor = true;
            delChordEntryButton.Click += delChordEntryButton_Click;
            // 
            // addChordEntryButton
            // 
            addChordEntryButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            addChordEntryButton.Location = new System.Drawing.Point(765, 608);
            addChordEntryButton.Name = "addChordEntryButton";
            addChordEntryButton.Size = new System.Drawing.Size(55, 29);
            addChordEntryButton.TabIndex = 32;
            addChordEntryButton.Text = "Add";
            addChordEntryButton.UseVisualStyleBackColor = true;
            addChordEntryButton.Click += addChordEntryButton_Click;
            // 
            // swaplM2EntriesButton
            // 
            swaplM2EntriesButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            swaplM2EntriesButton.Location = new System.Drawing.Point(509, 608);
            swaplM2EntriesButton.Name = "swaplM2EntriesButton";
            swaplM2EntriesButton.Size = new System.Drawing.Size(55, 29);
            swaplM2EntriesButton.TabIndex = 31;
            swaplM2EntriesButton.Text = "Swap";
            swaplM2EntriesButton.UseVisualStyleBackColor = true;
            swaplM2EntriesButton.Click += swaplM2EntriesButton_Click;
            // 
            // delM2EntryButton
            // 
            delM2EntryButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            delM2EntryButton.Location = new System.Drawing.Point(448, 608);
            delM2EntryButton.Name = "delM2EntryButton";
            delM2EntryButton.Size = new System.Drawing.Size(55, 29);
            delM2EntryButton.TabIndex = 30;
            delM2EntryButton.Text = "Del";
            delM2EntryButton.UseVisualStyleBackColor = true;
            delM2EntryButton.Click += delM2EntryButton_Click;
            // 
            // addM2EntryButton
            // 
            addM2EntryButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            addM2EntryButton.Location = new System.Drawing.Point(386, 608);
            addM2EntryButton.Name = "addM2EntryButton";
            addM2EntryButton.Size = new System.Drawing.Size(55, 29);
            addM2EntryButton.TabIndex = 29;
            addM2EntryButton.Text = "Add";
            addM2EntryButton.UseVisualStyleBackColor = true;
            addM2EntryButton.Click += addM2EntryButton_Click;
            // 
            // swapM1EntriesButton
            // 
            swapM1EntriesButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            swapM1EntriesButton.Location = new System.Drawing.Point(130, 608);
            swapM1EntriesButton.Name = "swapM1EntriesButton";
            swapM1EntriesButton.Size = new System.Drawing.Size(55, 29);
            swapM1EntriesButton.TabIndex = 28;
            swapM1EntriesButton.Text = "Swap";
            swapM1EntriesButton.UseVisualStyleBackColor = true;
            swapM1EntriesButton.Click += swapM1EntriesButton_Click;
            // 
            // delM1EntryButton
            // 
            delM1EntryButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            delM1EntryButton.Location = new System.Drawing.Point(69, 608);
            delM1EntryButton.Name = "delM1EntryButton";
            delM1EntryButton.Size = new System.Drawing.Size(55, 29);
            delM1EntryButton.TabIndex = 27;
            delM1EntryButton.Text = "Del";
            delM1EntryButton.UseVisualStyleBackColor = true;
            delM1EntryButton.Click += delM1EntryButton_Click;
            // 
            // addM1EntryButton
            // 
            addM1EntryButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            addM1EntryButton.Location = new System.Drawing.Point(8, 608);
            addM1EntryButton.Name = "addM1EntryButton";
            addM1EntryButton.Size = new System.Drawing.Size(55, 29);
            addM1EntryButton.TabIndex = 26;
            addM1EntryButton.Text = "Add";
            addM1EntryButton.UseVisualStyleBackColor = true;
            addM1EntryButton.Click += addM1EntryButton_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(767, 109);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(58, 20);
            label8.TabIndex = 25;
            label8.Text = "Chords:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(386, 109);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(74, 20);
            label7.TabIndex = 24;
            label7.Text = "Melody 2:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(8, 109);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(74, 20);
            label6.TabIndex = 23;
            label6.Text = "Melody 1:";
            // 
            // chordsDataGridView
            // 
            chordsDataGridView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            chordsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            chordsDataGridView.Location = new System.Drawing.Point(765, 137);
            chordsDataGridView.Name = "chordsDataGridView";
            chordsDataGridView.RowHeadersWidth = 51;
            chordsDataGridView.RowTemplate.Height = 29;
            chordsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            chordsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            chordsDataGridView.Size = new System.Drawing.Size(343, 468);
            chordsDataGridView.TabIndex = 22;
            // 
            // melody2DataGridView
            // 
            melody2DataGridView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            melody2DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            melody2DataGridView.Location = new System.Drawing.Point(386, 137);
            melody2DataGridView.Name = "melody2DataGridView";
            melody2DataGridView.RowHeadersWidth = 51;
            melody2DataGridView.RowTemplate.Height = 29;
            melody2DataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            melody2DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            melody2DataGridView.Size = new System.Drawing.Size(371, 468);
            melody2DataGridView.TabIndex = 21;
            // 
            // melody1DataGridView
            // 
            melody1DataGridView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            melody1DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            melody1DataGridView.Location = new System.Drawing.Point(8, 137);
            melody1DataGridView.Name = "melody1DataGridView";
            melody1DataGridView.RowHeadersWidth = 51;
            melody1DataGridView.RowTemplate.Height = 29;
            melody1DataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            melody1DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            melody1DataGridView.Size = new System.Drawing.Size(371, 468);
            melody1DataGridView.TabIndex = 20;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(8, 11);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(46, 20);
            label4.TabIndex = 18;
            label4.Text = "Song:";
            // 
            // swapThemeButton
            // 
            swapThemeButton.Location = new System.Drawing.Point(391, 6);
            swapThemeButton.Name = "swapThemeButton";
            swapThemeButton.Size = new System.Drawing.Size(87, 29);
            swapThemeButton.TabIndex = 16;
            swapThemeButton.Text = "Swap";
            swapThemeButton.UseVisualStyleBackColor = true;
            // 
            // sequenceSelectComboBox
            // 
            sequenceSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            sequenceSelectComboBox.FormattingEnabled = true;
            sequenceSelectComboBox.Location = new System.Drawing.Point(58, 7);
            sequenceSelectComboBox.Name = "sequenceSelectComboBox";
            sequenceSelectComboBox.Size = new System.Drawing.Size(63, 28);
            sequenceSelectComboBox.TabIndex = 12;
            sequenceSelectComboBox.SelectionChangeCommitted += themeSelectComboBox_SelectionChangeCommitted;
            // 
            // addSequenceButton
            // 
            addSequenceButton.Location = new System.Drawing.Point(205, 6);
            addSequenceButton.Name = "addSequenceButton";
            addSequenceButton.Size = new System.Drawing.Size(87, 29);
            addSequenceButton.TabIndex = 10;
            addSequenceButton.Text = "Add";
            addSequenceButton.UseVisualStyleBackColor = true;
            addSequenceButton.Click += addThemeButton_Click;
            // 
            // delSequenceButton
            // 
            delSequenceButton.Location = new System.Drawing.Point(298, 6);
            delSequenceButton.Name = "delSequenceButton";
            delSequenceButton.Size = new System.Drawing.Size(87, 29);
            delSequenceButton.TabIndex = 9;
            delSequenceButton.Text = "Del";
            delSequenceButton.UseVisualStyleBackColor = true;
            delSequenceButton.Click += delSongButton_Click;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = System.Drawing.SystemColors.Control;
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(textBox2);
            tabPage2.Controls.Add(label2);
            tabPage2.Location = new System.Drawing.Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new System.Drawing.Size(1121, 640);
            tabPage2.TabIndex = 3;
            tabPage2.Text = "Log";
            // 
            // button2
            // 
            button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            button2.Location = new System.Drawing.Point(11, 533);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(94, 29);
            button2.TabIndex = 2;
            button2.Text = "Clear";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox2
            // 
            textBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox2.Location = new System.Drawing.Point(11, 28);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(857, 501);
            textBox2.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(11, 5);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(37, 20);
            label2.TabIndex = 0;
            label2.Text = "Log:";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new System.Drawing.Point(0, 706);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(1152, 26);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(151, 20);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripFile, toolStripMenuItem6, toolStripMenuItem7 });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            menuStrip1.Size = new System.Drawing.Size(1152, 30);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripFile
            // 
            toolStripFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1, openSongsToolStripMenuItem, saveSongsToolStripMenuItem, saveSongsAsToolStripMenuItem, toolStripSeparator3, openROMStripMenuItem, saveROMStripMenuItem, saveROMAsStripMenuItem, toolStripSeparator1, receiveStripMenuItem, sendStripMenuItem, toolStripSeparator2, toolStripMenuItem5 });
            toolStripFile.Name = "toolStripFile";
            toolStripFile.Size = new System.Drawing.Size(46, 24);
            toolStripFile.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(192, 26);
            toolStripMenuItem1.Text = "New";
            // 
            // openSongsToolStripMenuItem
            // 
            openSongsToolStripMenuItem.Name = "openSongsToolStripMenuItem";
            openSongsToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            openSongsToolStripMenuItem.Text = "Open songs";
            openSongsToolStripMenuItem.Click += openSongsToolStripMenuItem_Click;
            // 
            // saveSongsToolStripMenuItem
            // 
            saveSongsToolStripMenuItem.Name = "saveSongsToolStripMenuItem";
            saveSongsToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            saveSongsToolStripMenuItem.Text = "Save songs";
            saveSongsToolStripMenuItem.Click += saveSongsToolStripMenuItem_Click;
            // 
            // saveSongsAsToolStripMenuItem
            // 
            saveSongsAsToolStripMenuItem.Name = "saveSongsAsToolStripMenuItem";
            saveSongsAsToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            saveSongsAsToolStripMenuItem.Text = "Save songs as...";
            saveSongsAsToolStripMenuItem.Click += saveSongsAsToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(189, 6);
            // 
            // openROMStripMenuItem
            // 
            openROMStripMenuItem.Name = "openROMStripMenuItem";
            openROMStripMenuItem.Size = new System.Drawing.Size(192, 26);
            openROMStripMenuItem.Text = "Open ROM";
            openROMStripMenuItem.Click += openToolStripRomMenuItem_Click;
            // 
            // saveROMStripMenuItem
            // 
            saveROMStripMenuItem.Name = "saveROMStripMenuItem";
            saveROMStripMenuItem.Size = new System.Drawing.Size(192, 26);
            saveROMStripMenuItem.Text = "Save ROM";
            saveROMStripMenuItem.Click += saveRomToolStripMenuItem_Click;
            // 
            // saveROMAsStripMenuItem
            // 
            saveROMAsStripMenuItem.Name = "saveROMAsStripMenuItem";
            saveROMAsStripMenuItem.Size = new System.Drawing.Size(192, 26);
            saveROMAsStripMenuItem.Text = "Save ROM as...";
            saveROMAsStripMenuItem.Click += saveRomAsToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(189, 6);
            // 
            // receiveStripMenuItem
            // 
            receiveStripMenuItem.Name = "receiveStripMenuItem";
            receiveStripMenuItem.Size = new System.Drawing.Size(192, 26);
            receiveStripMenuItem.Text = "Receive";
            receiveStripMenuItem.Click += receiveToolStripMenuItem_Click;
            // 
            // sendStripMenuItem
            // 
            sendStripMenuItem.Name = "sendStripMenuItem";
            sendStripMenuItem.Size = new System.Drawing.Size(192, 26);
            sendStripMenuItem.Text = "Send";
            sendStripMenuItem.Click += sendToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(189, 6);
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new System.Drawing.Size(192, 26);
            toolStripMenuItem5.Text = "Exit";
            toolStripMenuItem5.Click += toolStripMenuItem5_Click;
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem });
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new System.Drawing.Size(55, 24);
            toolStripMenuItem6.Text = "Help";
            toolStripMenuItem6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new System.Drawing.Size(14, 24);
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1152, 732);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(tabControl2);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "drivePackEditor";
            FormClosing += Form1_FormClosing;
            tabControl2.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)chordsDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)melody2DataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)melody1DataGridView).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox romInfoTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button clearInfoButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openROMStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveROMStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveROMAsStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.TextBox romTitleTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem receiveStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button swapThemeButton;
        private System.Windows.Forms.ComboBox sequenceSelectComboBox;
        private System.Windows.Forms.Button addSequenceButton;
        private System.Windows.Forms.Button delSequenceButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView melody1DataGridView;
        private System.Windows.Forms.ToolStripMenuItem saveSongsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSongsAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem openSongsToolStripMenuItem;
        private System.Windows.Forms.DataGridView chordsDataGridView;
        private System.Windows.Forms.DataGridView melody2DataGridView;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button swapChordEntriesButton;
        private System.Windows.Forms.Button delChordEntryButton;
        private System.Windows.Forms.Button addChordEntryButton;
        private System.Windows.Forms.Button swaplM2EntriesButton;
        private System.Windows.Forms.Button delM2EntryButton;
        private System.Windows.Forms.Button addM2EntryButton;
        private System.Windows.Forms.Button swapM1EntriesButton;
        private System.Windows.Forms.Button delM1EntryButton;
        private System.Windows.Forms.Button addM1EntryButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox sequenceTitleTextBox;
        private System.Windows.Forms.Button disassemblyButton;
        private System.Windows.Forms.Button buildButton;
        private System.Windows.Forms.Label totalSongsLabel;
    }
}

