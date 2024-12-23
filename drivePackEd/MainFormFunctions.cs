using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Linq;
using static drivePackEd.cDrivePack;
using System.ComponentModel;
using static drivePackEd.ChordChannelCodeEntry;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Security;
using static drivePackEd.MChannelCodeEntry;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

// **********************************************************************************
// ****                          drivePACK Editor                                ****
// ****                         www.tolaemon.com/dpack                           ****
// ****                              Source code                                 ****
// ****                              23/04/2024                                  ****
// ****                            Jordi Bartolome                               ****
// ****                                                                          ****
// ****          IMPORTANT:                                                      ****
// ****          Using this code or any part of it means accepting all           ****
// ****          conditions exposed in: http://www.tolaemon.com/dpack            ****
// **********************************************************************************
namespace drivePackEd{

    partial class dummy {/* this dummy class has been added only to overcome the issue with Visual Studio IDE that adds a new .resx file when the user clicks on any Form partial class file */ };

    public partial class MainForm : Form{

        /*******************************************************************************
        *  @brief Initialize the controls used to edit the different notes in the 
        *  Melody and Chord channels.
        *******************************************************************************/
        public void InitInstructionEditionControls() {
            int iCtrlYcoord = 98;
            int iLbYOffset = 3; // extra Y offset for th Y label controls
            int iCtrlXcoord = 6;
            int iCtrlXOffset = 0;
            int iCtrlXMargin = 5; // extra value added in the offset between the objects to leave a small margin between them
            // List for the Melody 1 commands fields ComboBoxes
            BindingList<string> liMelody1OnOff = new BindingList<string>();
            BindingList<string> liMelody1Instrument = new BindingList<string>();
            BindingList<string> liMelody1Notes = new BindingList<string>();
            BindingList<string> liMelody1Effect = new BindingList<string>();
            BindingList<string> liMelody1Repeat = new BindingList<string> ();
            BindingList<string> liMelody1Time = new BindingList<string>();
            // List for the Melody 2 commands fields ComboBoxes
            BindingList<string> liMelody2OnOff = new BindingList<string>();
            BindingList<string> liMelody2Instrument = new BindingList<string>();
            BindingList<string> liMelody2Notes = new BindingList<string>();
            BindingList<string> liMelody2Effect = new BindingList<string>();
            BindingList<string> liMelody2Repeat = new BindingList<string>();
            BindingList<string> liMelody2Time = new BindingList<string>();
            // List for the Chord commands fields ComboBoxes
            BindingList<string> liChordOnOff = new BindingList<string>();
            BindingList<string> liChordNotes = new BindingList<string>();
            BindingList<string> liChordTypes = new BindingList<string>();
            BindingList<string> liChordRythmMode = new BindingList<string>();
            BindingList<string> liChordRythmStyle = new BindingList<string>();
            BindingList<string> liChordRepeatMark = new BindingList<string>();
            // Fill de list of available Melody and Chords available Commands
            BindingList<string> liMelody1Cmds = new BindingList<string>();
            BindingList<string> liMelody2Cmds = new BindingList<string>();
            BindingList<string> liChordCmds = new BindingList<string>();

            // fill the lists for the Melody commands fields ComboBoxes
            foreach (MChannelCodeEntry.t_On_Off t_onOff in Enum.GetValues(typeof(MChannelCodeEntry.t_On_Off))) {
                liMelody1OnOff.Add(MChannelCodeEntry.tOnOffToString(t_onOff));
                liMelody2OnOff.Add(MChannelCodeEntry.tOnOffToString(t_onOff));
            }
            foreach (MChannelCodeEntry.t_Instrument t_instr in Enum.GetValues(typeof(MChannelCodeEntry.t_Instrument))) {
                liMelody1Instrument.Add(MChannelCodeEntry.tInstrumentToString(t_instr));
                liMelody2Instrument.Add(MChannelCodeEntry.tInstrumentToString(t_instr));
            }
            foreach (MChannelCodeEntry.t_Notes t_note in Enum.GetValues(typeof(MChannelCodeEntry.t_Notes))) {
                liMelody1Notes.Add(MChannelCodeEntry.tNotesToString(t_note));
                liMelody2Notes.Add(MChannelCodeEntry.tNotesToString(t_note));
            }
            foreach (MChannelCodeEntry.t_Effect t_effect in Enum.GetValues(typeof(MChannelCodeEntry.t_Effect))) {
                liMelody1Effect.Add(MChannelCodeEntry.tEffectToString(t_effect));
                liMelody2Effect.Add(MChannelCodeEntry.tEffectToString(t_effect));
            }
            foreach (MChannelCodeEntry.t_RepeatMark t_repeat in Enum.GetValues(typeof(MChannelCodeEntry.t_RepeatMark))) {
                liMelody1Repeat.Add(MChannelCodeEntry.tRepeatMarkToString(t_repeat));
                liMelody2Repeat.Add(MChannelCodeEntry.tRepeatMarkToString(t_repeat));
            }
            foreach (MChannelCodeEntry.t_Time t_time in Enum.GetValues(typeof(MChannelCodeEntry.t_Time))) {
                liMelody1Time.Add(MChannelCodeEntry.tTimeToString(t_time));
                liMelody2Time.Add(MChannelCodeEntry.tTimeToString(t_time));
            }
            // fill the lists for the Chords commands fields ComboBoxes
            foreach (ChordChannelCodeEntry.t_On_Off t_onOff in Enum.GetValues(typeof(ChordChannelCodeEntry.t_On_Off))) {
                liChordOnOff.Add(ChordChannelCodeEntry.tOnOffToString(t_onOff));
            }
            foreach (ChordChannelCodeEntry.t_Notes t_note in Enum.GetValues(typeof(ChordChannelCodeEntry.t_Notes))) {
                liChordNotes.Add(ChordChannelCodeEntry.tNotesToString(t_note));
            }
            foreach (ChordChannelCodeEntry.t_ChordType t_type in Enum.GetValues(typeof(ChordChannelCodeEntry.t_ChordType))) {
                liChordTypes.Add(ChordChannelCodeEntry.tChordTypeToString(t_type));
            }
            foreach (ChordChannelCodeEntry.t_RythmMode t_mode in Enum.GetValues(typeof(ChordChannelCodeEntry.t_RythmMode))) {
                liChordRythmMode.Add(ChordChannelCodeEntry.tRythmModeToString(t_mode));
            }
            foreach (ChordChannelCodeEntry.t_RythmStyle t_style in Enum.GetValues(typeof(ChordChannelCodeEntry.t_RythmStyle))) {
                liChordRythmStyle.Add(ChordChannelCodeEntry.tRythmStyleToString(t_style));
            }
            foreach (ChordChannelCodeEntry.t_RepeatMark t_RepeatMark in Enum.GetValues(typeof(ChordChannelCodeEntry.t_RepeatMark))) {
                liChordRepeatMark.Add(ChordChannelCodeEntry.tRepeatMarkToString(t_RepeatMark));
            }

            // #######################################################   controls for the NOTE SELECTION + DURATION + REST command ####### MELODY1 CHANNEL
            nUpDownM1NoteRest = new System.Windows.Forms.NumericUpDown();
            lblM1NoteRest = new System.Windows.Forms.Label(); 
            nUpDownM1NoteDur = new System.Windows.Forms.NumericUpDown();
            lblM1NoteDur = new System.Windows.Forms.Label();
            cmboBoxM1Note = new System.Windows.Forms.ComboBox();
            lblM1Note = new System.Windows.Forms.Label();

            iCtrlXOffset = 0;
            // 
            // labM1Note
            // 
            lblM1Note.AutoSize = true;
            lblM1Note.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord+ iLbYOffset);
            lblM1Note.Name = "labM1Note";
            lblM1Note.Size = new Size(34, 12);
            lblM1Note.TabStop = false;
            lblM1Note.Text = "Note:";
            lblM1Note.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1Note.Size.Width + iCtrlXMargin;
            // 
            // comboBoxM1Note
            // 
            cmboBoxM1Note.FormattingEnabled = true;
            cmboBoxM1Note.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM1Note.Name = "comboBoxM1Note";
            cmboBoxM1Note.Size = new Size(54, 18);
            cmboBoxM1Note.TabStop = false;
            cmboBoxM1Note.DataSource = liMelody1Notes;
            cmboBoxM1Note.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM1Note.Size.Width + iCtrlXMargin;
            // 
            // labM1Dur
            // 
            lblM1NoteDur.AutoSize = true;
            lblM1NoteDur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1NoteDur.Name = "labM1Dur";
            lblM1NoteDur.Size = new Size(23, 12);
            lblM1NoteDur.TabStop = false;
            lblM1NoteDur.Text = "Dur:";
            lblM1NoteDur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1NoteDur.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM1Dur
            // 
            nUpDownM1NoteDur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM1NoteDur.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDownM1NoteDur.Name = "nUpDownM1Dur";
            nUpDownM1NoteDur.Size = new Size(43, 18);
            nUpDownM1NoteDur.TabStop = false;
            nUpDownM1NoteDur.Maximum = 255;
            nUpDownM1NoteDur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM1NoteDur.Size.Width + iCtrlXMargin;
            // 
            // labM1Rest
            // 
            lblM1NoteRest.AutoSize = true;
            lblM1NoteRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1NoteRest.Name = "labM1Rest";
            lblM1NoteRest.Size = new Size(26, 12);
            lblM1NoteRest.TabStop = false;
            lblM1NoteRest.Text = "Rest:";
            lblM1NoteRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1NoteRest.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM1Rest
            // 
            nUpDownM1NoteRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM1NoteRest.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDownM1NoteRest.Name = "nUpDownM1Rest";
            nUpDownM1NoteRest.Size = new Size(43, 18);
            nUpDownM1NoteRest.TabStop = false;
            nUpDownM1NoteRest.Maximum = 255;
            nUpDownM1NoteRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM1NoteRest.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(nUpDownM1NoteRest, panel1, szFormScaleFactor);
            scaleAndAddToPanel(lblM1NoteRest, panel1, szFormScaleFactor);
            scaleAndAddToPanel(nUpDownM1NoteDur, panel1, szFormScaleFactor);
            scaleAndAddToPanel(lblM1NoteDur, panel1, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM1Note, panel1, szFormScaleFactor);
            scaleAndAddToPanel(lblM1Note, panel1, szFormScaleFactor);

            // #######################################################   controls for the TIMBRE + ON/OFF + REST command ####### MELODY1 CHANNEL
            nUpDownM1TimbreRest = new System.Windows.Forms.NumericUpDown();
            lblM1TimbreRest = new System.Windows.Forms.Label();
            cmboBoxM1TimbreOnOff = new System.Windows.Forms.ComboBox();
            lblM1Timbre = new System.Windows.Forms.Label();
            cmboBoxM1Timbre = new System.Windows.Forms.ComboBox();

            iCtrlXOffset = 0;
            // 
            // labM1Timbre
            // 
            lblM1Timbre.AutoSize = true;
            lblM1Timbre.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1Timbre.Name = "labM1Timbre";
            lblM1Timbre.Size = new Size(31, 12);
            lblM1Timbre.TabStop = false;
            lblM1Timbre.Text = "Instr:";
            lblM1Timbre.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1Timbre.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxM1Timbre
            // 
            cmboBoxM1Timbre.FormattingEnabled = true;
            cmboBoxM1Timbre.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM1Timbre.Name = "cmboBoxM1Timbre";
            cmboBoxM1Timbre.Size = new Size(108, 18);
            cmboBoxM1Timbre.TabStop = false;
            cmboBoxM1Timbre.DataSource = liMelody1Instrument;
            cmboBoxM1Timbre.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM1Timbre.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxM1TimbreOnOff
            // 
            cmboBoxM1TimbreOnOff.FormattingEnabled = true;
            cmboBoxM1TimbreOnOff.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM1TimbreOnOff.Name = "cmboBoxM1TimbreOnOff";
            cmboBoxM1TimbreOnOff.Size = new Size(50, 18);
            cmboBoxM1TimbreOnOff.TabStop = false;
            cmboBoxM1TimbreOnOff.DataSource = liMelody1OnOff;
            cmboBoxM1TimbreOnOff.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM1TimbreOnOff.Size.Width + iCtrlXMargin;
            // 
            // labM1TimbreRest
            // 
            lblM1TimbreRest.AutoSize = true;
            lblM1TimbreRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1TimbreRest.Name = "labM1TimbreRest";
            lblM1TimbreRest.Size =  new Size(32, 15);
            lblM1TimbreRest.TabStop = false;
            lblM1TimbreRest.Text = "Rest:";
            lblM1TimbreRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1TimbreRest.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM1TimbreRest
            // 
            nUpDownM1TimbreRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM1TimbreRest.Name = "nUpDownM1TimbreRest";
            nUpDownM1TimbreRest.Size = new Size(43, 18);
            nUpDownM1TimbreRest.TabStop = false;
            nUpDownM1TimbreRest.Maximum = 255;
            nUpDownM1TimbreRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM1TimbreRest.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(nUpDownM1TimbreRest, panel1, szFormScaleFactor);
            scaleAndAddToPanel(lblM1TimbreRest, panel1, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM1TimbreOnOff, panel1, szFormScaleFactor);
            scaleAndAddToPanel(lblM1Timbre, panel1, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM1Timbre, panel1, szFormScaleFactor);

            // #######################################################   controls for the EFFECT + ON/OFF + REST command ####### MELODY1 CHANNEL
            lblM1Effect = new System.Windows.Forms.Label();
            cmbBoxM1Effect = new System.Windows.Forms.ComboBox();
            cmbBoxM1EffectOnOff = new System.Windows.Forms.ComboBox();
            lblM1EffRest = new System.Windows.Forms.Label();
            nUpDownM1EffRest = new System.Windows.Forms.NumericUpDown();

            iCtrlXOffset = 0;
            // 
            // labM1Effect
            // 
            lblM1Effect.AutoSize = true;
            lblM1Effect.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1Effect.Name = "labM1Effect";
            lblM1Effect.Size = new Size(20, 16);
            lblM1Effect.TabStop = false;
            lblM1Effect.Text = "Eff:";
            lblM1Effect.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1Effect.Size.Width + iCtrlXMargin;
            // 
            // cmbBoxM1Effect
            // 
            cmbBoxM1Effect.FormattingEnabled = true;
            cmbBoxM1Effect.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmbBoxM1Effect.Name = "cmbBoxM1Effect";
            cmbBoxM1Effect.Size = new Size(112, 22);
            cmbBoxM1Effect.TabStop = false;
            cmbBoxM1Effect.DataSource = liMelody1Effect;
            cmbBoxM1Effect.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmbBoxM1Effect.Size.Width + iCtrlXMargin;
            // 
            // cmbBoxM1EffectOnOff
            // 
            cmbBoxM1EffectOnOff.FormattingEnabled = true;
            cmbBoxM1EffectOnOff.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmbBoxM1EffectOnOff.Name = "cmbBoxM1EffectOnOff";
            cmbBoxM1EffectOnOff.Size = new Size(50, 22);
            cmbBoxM1EffectOnOff.TabStop = false;
            cmbBoxM1EffectOnOff.DataSource = liMelody1OnOff;
            cmbBoxM1EffectOnOff.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmbBoxM1EffectOnOff.Size.Width + iCtrlXMargin;
            // 
            // labM1EffRest
            // 
            lblM1EffRest.AutoSize = true;
            lblM1EffRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1EffRest.Name = "labM1EffRest";
            lblM1EffRest.Size = new Size(32, 16);
            lblM1EffRest.TabStop = false;
            lblM1EffRest.Text = "Rest:";
            lblM1EffRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1EffRest.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM1EffRest
            // 
            nUpDownM1EffRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM1EffRest.Name = "nUpDownM1EffRest";
            nUpDownM1EffRest.Size = new Size(43, 22);
            nUpDownM1EffRest.TabStop = false;
            nUpDownM1EffRest.Maximum = 255;
            nUpDownM1EffRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM1EffRest.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(nUpDownM1EffRest, panel1, szFormScaleFactor);
            scaleAndAddToPanel(lblM1EffRest, panel1, szFormScaleFactor);
            scaleAndAddToPanel(cmbBoxM1EffectOnOff, panel1, szFormScaleFactor);
            scaleAndAddToPanel(cmbBoxM1Effect, panel1, szFormScaleFactor);
            scaleAndAddToPanel(lblM1Effect, panel1, szFormScaleFactor);

            // #######################################################   controls for the REST DURATION command ####### MELODY1 CHANNEL
            lblM1RestRest = new System.Windows.Forms.Label();
            nUpDownM1RestRest = new System.Windows.Forms.NumericUpDown();

            iCtrlXOffset = 0;
            // 
            // labM1RestRest
            // 
            lblM1RestRest.AutoSize = true;
            lblM1RestRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1RestRest.Name = "labM1RestRest";
            lblM1RestRest.Size = new Size(28, 16);
            lblM1RestRest.TabStop = false;
            lblM1RestRest.Text = "Rest:";
            lblM1RestRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1RestRest.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM1RestfRest
            // 
            nUpDownM1RestRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM1RestRest.Name = "nUpDownM1RestRest";
            nUpDownM1RestRest.Size = new Size(43, 22);
            nUpDownM1RestRest.TabStop = false;
            nUpDownM1RestRest.Maximum = 255;
            nUpDownM1RestRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM1RestRest.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblM1RestRest, panel1, szFormScaleFactor);
            scaleAndAddToPanel(nUpDownM1RestRest, panel1, szFormScaleFactor);

            // #######################################################   controls for the REPEAT command ####### MELODY1 CHANNEL
            lblM1Repeat = new System.Windows.Forms.Label();
            cmboBoxM1Repeat = new System.Windows.Forms.ComboBox();
		
            iCtrlXOffset = 0;		
            // 
            // lblM1Repeat
            // 
            lblM1Repeat.AutoSize = true;
            lblM1Repeat.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord+ iLbYOffset);
            lblM1Repeat.Name = "lblM1Repeat";
            lblM1Repeat.Size = new Size(44, 12);
            lblM1Repeat.TabStop = false;
            lblM1Repeat.Text = "Repeat:";
            lblM1Repeat.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1Repeat.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxM1Repeat
            // 
            cmboBoxM1Repeat.FormattingEnabled = true;
            cmboBoxM1Repeat.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM1Repeat.Name = "cmboBoxM1Repeat";
            cmboBoxM1Repeat.Size = new Size(68, 18);
            cmboBoxM1Repeat.TabStop = false;
            cmboBoxM1Repeat.DataSource = liMelody1Repeat;
            cmboBoxM1Repeat.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM1Repeat.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblM1Repeat, panel1, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM1Repeat, panel1, szFormScaleFactor);

            // #######################################################   controls for the TIE command ####### MELODY1 CHANNEL
            lblM1Tie = new System.Windows.Forms.Label();
            cmboBoxM1Tie = new System.Windows.Forms.ComboBox();

            iCtrlXOffset = 0;
            // 
            // lblM1Tie
            // 
            lblM1Tie.AutoSize = true;
            lblM1Tie.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1Tie.Name = "lblM1Tie";
            lblM1Tie.Size = new Size(22, 12);
            lblM1Tie.TabStop = false;
            lblM1Tie.Text = "Tie:";
            lblM1Tie.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1Tie.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxM1Tie
            // 
            cmboBoxM1Tie.FormattingEnabled = true;
            cmboBoxM1Tie.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM1Tie.Name = "cmboBoxM1Tie";
            cmboBoxM1Tie.Size = new Size(50, 18);
            cmboBoxM1Tie.TabStop = false;
            cmboBoxM1Tie.DataSource = liMelody1OnOff;
            cmboBoxM1Tie.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM1Tie.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblM1Tie, panel1, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM1Tie, panel1, szFormScaleFactor);

            // #######################################################   controls for the KEY command ####### MELODY1 CHANNEL
            lblM1Key = new System.Windows.Forms.Label();
            nUpDownM1Key = new System.Windows.Forms.NumericUpDown();

            iCtrlXOffset = 0;
            // 
            // lblM1Key
            // 
            lblM1Key.AutoSize = true;
            lblM1Key.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1Key.Name = "lblM1Key";
            lblM1Key.Size = new Size(30, 16);
            lblM1Key.TabStop = false;
            lblM1Key.Text = "Key:";
            lblM1Key.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1Key.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM1Key
            // 
            nUpDownM1Key.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM1Key.Name = "nUpDownM1Key";
            nUpDownM1Key.Size = new Size(56, 22);
            nUpDownM1Key.TabStop = false;
            nUpDownM1Key.Maximum = 255;
            nUpDownM1Key.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM1Key.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblM1Key, panel1, szFormScaleFactor);
            scaleAndAddToPanel(nUpDownM1Key, panel1, szFormScaleFactor);

            // ####################################################### controls for the TIME command ####### MELODY1 CHANNEL
            lblM1Time = new System.Windows.Forms.Label();
            cmboBoxM1Time = new System.Windows.Forms.ComboBox();

            iCtrlXOffset = 0;
            // 
            // lblM1Time
            // 
            lblM1Time.AutoSize = true;
            lblM1Time.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1Time.Name = "lblM1Time";
            lblM1Time.Size = new Size(32, 16);
            lblM1Time.TabStop = false;
            lblM1Time.Text = "Time:";
            lblM1Time.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1Time.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxM1Time
            // 
            cmboBoxM1Time.FormattingEnabled = true;
            cmboBoxM1Time.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM1Time.Name = "cmboBoxM1Time";
            cmboBoxM1Time.Size = new Size(60, 22);
            cmboBoxM1Time.TabStop = false;
            cmboBoxM1Time.DataSource = liMelody1Time;
            cmboBoxM1Time.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM1Time.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblM1Time, panel1, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM1Time, panel1, szFormScaleFactor);


            // ####################################################### controls for the DURATION X2 command ####### MELODY1 CHANNEL
            lblM1DurationX2Dur = new System.Windows.Forms.Label();
            nUpDownM1DurationX2Dur = new System.Windows.Forms.NumericUpDown();
            lblM1DurationX2Rest = new System.Windows.Forms.Label();
            nUpDownM1DurationX2Rest = new System.Windows.Forms.NumericUpDown();
            
            iCtrlXOffset = 0;
            // 
            // lblM1DurationX2Dur
            // 
            lblM1DurationX2Dur.AutoSize = true;
            lblM1DurationX2Dur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1DurationX2Dur.Name = "labM1Dur";
            lblM1DurationX2Dur.Size = new Size(26, 12);
            lblM1DurationX2Dur.TabStop = false;
            lblM1DurationX2Dur.Text = "Dur:";
            lblM1DurationX2Dur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1DurationX2Dur.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM1DurationX2Dur
            // 
            nUpDownM1DurationX2Dur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM1DurationX2Dur.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDownM1DurationX2Dur.Name = "nUpDownM1Dur";
            nUpDownM1DurationX2Dur.Size = new Size(43, 18);
            nUpDownM1DurationX2Dur.TabStop = false;
            nUpDownM1DurationX2Dur.Maximum = 255;
            nUpDownM1DurationX2Dur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM1DurationX2Dur.Size.Width + iCtrlXMargin;
            // 
            // lblM1DurationX2Rest
            // 
            lblM1DurationX2Rest.AutoSize = true;
            lblM1DurationX2Rest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM1DurationX2Rest.Name = "labM1Rest";
            lblM1DurationX2Rest.Size = new Size(26, 12);
            lblM1DurationX2Rest.TabStop = false;
            lblM1DurationX2Rest.Text = "Rest:";
            lblM1DurationX2Rest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM1DurationX2Rest.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM1DurationX2Rest
            // 
            nUpDownM1DurationX2Rest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM1DurationX2Rest.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDownM1DurationX2Rest.Name = "nUpDownM1Rest";
            nUpDownM1DurationX2Rest.Size = new Size(43, 18);
            nUpDownM1DurationX2Rest.TabStop = false;
            nUpDownM1DurationX2Rest.Maximum = 255;
            nUpDownM1DurationX2Rest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM1DurationX2Rest.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(nUpDownM1DurationX2Dur, panel1, szFormScaleFactor);
            scaleAndAddToPanel(lblM1DurationX2Dur, panel1, szFormScaleFactor);
            scaleAndAddToPanel(nUpDownM1DurationX2Rest, panel1, szFormScaleFactor);
            scaleAndAddToPanel(lblM1DurationX2Rest, panel1, szFormScaleFactor);

            // #######################################################   controls for the NOTE SELECTION + DURATION + REST command ####### MELODY2 CHANNEL
            nUpDownM2NoteRest = new System.Windows.Forms.NumericUpDown();
            lblM2NoteRest = new System.Windows.Forms.Label();
            nUpDownM2NoteDur = new System.Windows.Forms.NumericUpDown();
            lblM2NoteDur = new System.Windows.Forms.Label();
            cmboBoxM2Note = new System.Windows.Forms.ComboBox();
            lblM2Note = new System.Windows.Forms.Label();

            iCtrlXOffset = 0;
            // 
            // labM2Note
            // 
            lblM2Note.AutoSize = true;
            lblM2Note.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2Note.Name = "labM2Note";
            lblM2Note.Size = new Size(34, 12);
            lblM2Note.TabStop = false;
            lblM2Note.Text = "Note:";
            lblM2Note.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2Note.Size.Width + iCtrlXMargin;
            // 
            // comboBoxM2Note
            // 
            cmboBoxM2Note.FormattingEnabled = true;
            cmboBoxM2Note.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM2Note.Name = "comboBoxM2Note";
            cmboBoxM2Note.Size = new Size(54, 18);
            cmboBoxM2Note.TabStop = false;
            cmboBoxM2Note.DataSource = liMelody2Notes;
            cmboBoxM2Note.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM2Note.Size.Width + iCtrlXMargin;
            // 
            // labM2Dur
            // 
            lblM2NoteDur.AutoSize = true;
            lblM2NoteDur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2NoteDur.Name = "labM2Dur";
            lblM2NoteDur.Size = new Size(23, 12);
            lblM2NoteDur.TabStop = false;
            lblM2NoteDur.Text = "Dur:";
            lblM2NoteDur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2NoteDur.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM2Dur
            // 
            nUpDownM2NoteDur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM2NoteDur.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDownM2NoteDur.Name = "nUpDownM2Dur";
            nUpDownM2NoteDur.Size = new Size(43, 18);
            nUpDownM2NoteDur.TabStop = false;
            nUpDownM2NoteDur.Maximum = 255;
            nUpDownM2NoteDur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM2NoteDur.Size.Width + iCtrlXMargin;
            // 
            // labM2Rest
            // 
            lblM2NoteRest.AutoSize = true;
            lblM2NoteRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2NoteRest.Name = "labM2Rest";
            lblM2NoteRest.Size = new Size(26, 12);
            lblM2NoteRest.TabStop = false;
            lblM2NoteRest.Text = "Rest:";
            lblM2NoteRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2NoteRest.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM2Rest
            // 
            nUpDownM2NoteRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM2NoteRest.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDownM2NoteRest.Name = "nUpDownM2Rest";
            nUpDownM2NoteRest.Size = new Size(43, 18);
            nUpDownM2NoteRest.TabStop = false;
            nUpDownM2NoteRest.Maximum = 255;
            nUpDownM2NoteRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM2NoteRest.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(nUpDownM2NoteRest, panel2, szFormScaleFactor);
            scaleAndAddToPanel(lblM2NoteRest, panel2, szFormScaleFactor);
            scaleAndAddToPanel(nUpDownM2NoteDur, panel2, szFormScaleFactor);
            scaleAndAddToPanel(lblM2NoteDur, panel2, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM2Note, panel2, szFormScaleFactor);
            scaleAndAddToPanel(lblM2Note, panel2, szFormScaleFactor);

            // #######################################################   controls for the TIMBRE + ON/OFF + REST command ####### MELODY2 CHANNEL
            nUpDownM2TimbreRest = new System.Windows.Forms.NumericUpDown();
            lblM2TimbreRest = new System.Windows.Forms.Label();
            cmboBoxM2TimbreOnOff = new System.Windows.Forms.ComboBox();
            lblM2Timbre = new System.Windows.Forms.Label();
            cmboBoxM2Timbre = new System.Windows.Forms.ComboBox();

            iCtrlXOffset = 0;
            // 
            // labM2Timbre
            // 
            lblM2Timbre.AutoSize = true;
            lblM2Timbre.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2Timbre.Name = "labM2Timbre";
            lblM2Timbre.Size = new Size(31, 12);
            lblM2Timbre.TabStop = false;
            lblM2Timbre.Text = "Instr:";
            lblM2Timbre.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2Timbre.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxM2Timbre
            // 
            cmboBoxM2Timbre.FormattingEnabled = true;
            cmboBoxM2Timbre.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM2Timbre.Name = "cmboBoxM2Timbre";
            cmboBoxM2Timbre.Size = new Size(108, 18);
            cmboBoxM2Timbre.TabStop = false;
            cmboBoxM2Timbre.DataSource = liMelody1Instrument;
            cmboBoxM2Timbre.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM2Timbre.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxM2TimbreOnOff
            // 
            cmboBoxM2TimbreOnOff.FormattingEnabled = true;
            cmboBoxM2TimbreOnOff.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM2TimbreOnOff.Name = "cmboBoxM2TimbreOnOff";
            cmboBoxM2TimbreOnOff.Size = new Size(50, 18);
            cmboBoxM2TimbreOnOff.TabStop = false;
            cmboBoxM2TimbreOnOff.DataSource = liMelody2OnOff;
            cmboBoxM2TimbreOnOff.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM2TimbreOnOff.Size.Width + iCtrlXMargin;
            // 
            // labM2TimbreRest
            // 
            lblM2TimbreRest.AutoSize = true;
            lblM2TimbreRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2TimbreRest.Name = "labM2TimbreRest";
            lblM2TimbreRest.Size = new Size(32, 15);
            lblM2TimbreRest.TabStop = false;
            lblM2TimbreRest.Text = "Rest:";
            lblM2TimbreRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2TimbreRest.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM2TimbreRest
            // 
            nUpDownM2TimbreRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM2TimbreRest.Name = "nUpDownM2TimbreRest";
            nUpDownM2TimbreRest.Size = new Size(43, 18);
            nUpDownM2TimbreRest.TabStop = false;
            nUpDownM2TimbreRest.Maximum = 255;
            nUpDownM2TimbreRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM2TimbreRest.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(nUpDownM2TimbreRest, panel2, szFormScaleFactor);
            scaleAndAddToPanel(lblM2TimbreRest, panel2, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM2TimbreOnOff, panel2, szFormScaleFactor);
            scaleAndAddToPanel(lblM2Timbre, panel2, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM2Timbre, panel2, szFormScaleFactor);

            // #######################################################   controls for the EFFECT + ON/OFF + REST command ####### MELODY2 CHANNEL
            lblM2Effect = new System.Windows.Forms.Label();
            cmbBoxM2Effect = new System.Windows.Forms.ComboBox();
            cmbBoxM2EffectOnOff = new System.Windows.Forms.ComboBox();
            lblM2EffRest = new System.Windows.Forms.Label();
            nUpDownM2EffRest = new System.Windows.Forms.NumericUpDown();

            iCtrlXOffset = 0;
            // 
            // labM2Effect
            // 
            lblM2Effect.AutoSize = true;
            lblM2Effect.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2Effect.Name = "labM2Effect";
            lblM2Effect.Size = new Size(20, 16);
            lblM2Effect.TabStop = false;
            lblM2Effect.Text = "Eff:";
            lblM2Effect.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2Effect.Size.Width + iCtrlXMargin;
            // 
            // cmbBoxM2Effect
            // 
            cmbBoxM2Effect.FormattingEnabled = true;
            cmbBoxM2Effect.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmbBoxM2Effect.Name = "cmbBoxM2Effect";
            cmbBoxM2Effect.Size = new Size(112, 22);
            cmbBoxM2Effect.TabStop = false;
            cmbBoxM2Effect.DataSource = liMelody2Effect;
            cmbBoxM2Effect.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmbBoxM2Effect.Size.Width + iCtrlXMargin;
            // 
            // cmbBoxM2EffectOnOff
            // 
            cmbBoxM2EffectOnOff.FormattingEnabled = true;
            cmbBoxM2EffectOnOff.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmbBoxM2EffectOnOff.Name = "cmbBoxM2EffectOnOff";
            cmbBoxM2EffectOnOff.Size = new Size(50, 22);
            cmbBoxM2EffectOnOff.TabStop = false;
            cmbBoxM2EffectOnOff.DataSource = liMelody2OnOff;
            cmbBoxM2EffectOnOff.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmbBoxM2EffectOnOff.Size.Width + iCtrlXMargin;
            // 
            // labM2EffRest
            // 
            lblM2EffRest.AutoSize = true;
            lblM2EffRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2EffRest.Name = "labM2EffRest";
            lblM2EffRest.Size = new Size(32, 16);
            lblM2EffRest.TabStop = false;
            lblM2EffRest.Text = "Rest:";
            lblM2EffRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2EffRest.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM2EffRest
            // 
            nUpDownM2EffRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM2EffRest.Name = "nUpDownM2EffRest";
            nUpDownM2EffRest.Size = new Size(43, 22);
            nUpDownM2EffRest.TabStop = false;
            nUpDownM2EffRest.Maximum = 255;
            nUpDownM2EffRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM2EffRest.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(nUpDownM2EffRest, panel2, szFormScaleFactor);
            scaleAndAddToPanel(lblM2EffRest, panel2, szFormScaleFactor);
            scaleAndAddToPanel(cmbBoxM2EffectOnOff, panel2, szFormScaleFactor);
            scaleAndAddToPanel(cmbBoxM2Effect, panel2, szFormScaleFactor);
            scaleAndAddToPanel(lblM2Effect, panel2, szFormScaleFactor);

            // #######################################################   controls for the REST DURATION command ####### MELODY2 CHANNEL
            lblM2RestRest = new System.Windows.Forms.Label();
            nUpDownM2RestRest = new System.Windows.Forms.NumericUpDown();

            iCtrlXOffset = 0;
            // 
            // labM2RestRest
            // 
            lblM2RestRest.AutoSize = true;
            lblM2RestRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2RestRest.Name = "labM2RestRest";
            lblM2RestRest.Size = new Size(28, 16);
            lblM2RestRest.TabStop = false;
            lblM2RestRest.Text = "Rest:";
            lblM2RestRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2RestRest.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM2RestfRest
            // 
            nUpDownM2RestRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM2RestRest.Name = "nUpDownM2RestRest";
            nUpDownM2RestRest.Size = new Size(43, 22);
            nUpDownM2RestRest.TabStop = false;
            nUpDownM2RestRest.Maximum = 255;
            nUpDownM2RestRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM2RestRest.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblM2RestRest, panel2, szFormScaleFactor);
            scaleAndAddToPanel(nUpDownM2RestRest, panel2, szFormScaleFactor);

            // #######################################################   controls for the REPEAT command ####### MELODY2 CHANNEL
            lblM2Repeat = new System.Windows.Forms.Label();
            cmboBoxM2Repeat = new System.Windows.Forms.ComboBox();

            iCtrlXOffset = 0;
            // 
            // lblM2Repeat
            // 
            lblM2Repeat.AutoSize = true;
            lblM2Repeat.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2Repeat.Name = "lblM2Repeat";
            lblM2Repeat.Size = new Size(44, 12);
            lblM2Repeat.TabStop = false;
            lblM2Repeat.Text = "Repeat:";
            lblM2Repeat.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2Repeat.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxM2Repeat
            // 
            cmboBoxM2Repeat.FormattingEnabled = true;
            cmboBoxM2Repeat.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM2Repeat.Name = "cmboBoxM2Repeat";
            cmboBoxM2Repeat.Size = new Size(68, 18);
            cmboBoxM2Repeat.TabStop = false;
            cmboBoxM2Repeat.DataSource = liMelody2Repeat;
            cmboBoxM2Repeat.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM2Repeat.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblM2Repeat, panel2, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM2Repeat, panel2, szFormScaleFactor);

            // #######################################################   controls for the TIE command ####### MELODY2 CHANNEL
            lblM2Tie = new System.Windows.Forms.Label();
            cmboBoxM2Tie = new System.Windows.Forms.ComboBox();

            iCtrlXOffset = 0;
            // 
            // lblM2Tie
            // 
            lblM2Tie.AutoSize = true;
            lblM2Tie.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2Tie.Name = "lblM2Tie";
            lblM2Tie.Size = new Size(22, 12);
            lblM2Tie.TabStop = false;
            lblM2Tie.Text = "Tie:";
            lblM2Tie.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2Tie.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxM2Tie
            // 
            cmboBoxM2Tie.FormattingEnabled = true;
            cmboBoxM2Tie.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM2Tie.Name = "cmboBoxM2Tie";
            cmboBoxM2Tie.Size = new Size(50, 18);
            cmboBoxM2Tie.TabStop = false;
            cmboBoxM2Tie.DataSource = liMelody2OnOff;
            cmboBoxM2Tie.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM2Tie.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblM2Tie, panel2, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM2Tie, panel2, szFormScaleFactor);

            // #######################################################   controls for the KEY command ####### MELODY2 CHANNEL
            lblM2Key = new System.Windows.Forms.Label();
            nUpDownM2Key = new System.Windows.Forms.NumericUpDown();

            iCtrlXOffset = 0;
            // 
            // lblM2Key
            // 
            lblM2Key.AutoSize = true;
            lblM2Key.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2Key.Name = "lblM2Key";
            lblM2Key.Size = new Size(30, 16);
            lblM2Key.TabStop = false;
            lblM2Key.Text = "Key:";
            lblM2Key.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2Key.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM2Key
            // 
            nUpDownM2Key.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM2Key.Name = "nUpDownM2Key";
            nUpDownM2Key.Size = new Size(56, 22);
            nUpDownM2Key.TabStop = false;
            nUpDownM2Key.Maximum = 255;
            nUpDownM2Key.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM2Key.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblM2Key, panel2, szFormScaleFactor);
            scaleAndAddToPanel(nUpDownM2Key, panel2, szFormScaleFactor);

            // ####################################################### controls for the TIME command ####### MELODY2 CHANNEL
            lblM2Time = new System.Windows.Forms.Label();
            cmboBoxM2Time = new System.Windows.Forms.ComboBox();

            iCtrlXOffset = 0;
            // 
            // lblM2Time
            // 
            lblM2Time.AutoSize = true;
            lblM2Time.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2Time.Name = "lblM2Time";
            lblM2Time.Size = new Size(32, 16);
            lblM2Time.TabStop = false;
            lblM2Time.Text = "Time:";
            lblM2Time.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2Time.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxM2Time
            // 
            cmboBoxM2Time.FormattingEnabled = true;
            cmboBoxM2Time.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxM2Time.Name = "cmboBoxM2Time";
            cmboBoxM2Time.Size = new Size(60, 22);
            cmboBoxM2Time.TabStop = false;
            cmboBoxM2Time.DataSource = liMelody2Time;
            cmboBoxM2Time.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxM1Time.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblM2Time, panel2, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxM2Time, panel2, szFormScaleFactor);

            // ####################################################### controls for the DURATION X2 command ####### MELODY2 CHANNEL
            lblM2DurationX2Dur = new System.Windows.Forms.Label();
            nUpDownM2DurationX2Dur = new System.Windows.Forms.NumericUpDown();
            lblM2DurationX2Rest = new System.Windows.Forms.Label();
            nUpDownM2DurationX2Rest = new System.Windows.Forms.NumericUpDown();

            iCtrlXOffset = 0;
            // 
            // lblM2DurationX2Dur
            // 
            lblM2DurationX2Dur.AutoSize = true;
            lblM2DurationX2Dur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2DurationX2Dur.Name = "labM2Dur";
            lblM2DurationX2Dur.Size = new Size(26, 12);
            lblM2DurationX2Dur.TabStop = false;
            lblM2DurationX2Dur.Text = "Dur:";
            lblM2DurationX2Dur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2DurationX2Dur.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM2DurationX2Dur
            // 
            nUpDownM2DurationX2Dur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM2DurationX2Dur.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDownM2DurationX2Dur.Name = "nUpDownM2Dur";
            nUpDownM2DurationX2Dur.Size = new Size(43, 18);
            nUpDownM2DurationX2Dur.TabStop = false;
            nUpDownM2DurationX2Dur.Maximum = 255;
            nUpDownM2DurationX2Dur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM2DurationX2Dur.Size.Width + iCtrlXMargin;
            // 
            // lblM2DurationX2Rest
            // 
            lblM2DurationX2Rest.AutoSize = true;
            lblM2DurationX2Rest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblM2DurationX2Rest.Name = "labM2Rest";
            lblM2DurationX2Rest.Size = new Size(26, 12);
            lblM2DurationX2Rest.TabStop = false;
            lblM2DurationX2Rest.Text = "Rest:";
            lblM2DurationX2Rest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblM2DurationX2Rest.Size.Width + iCtrlXMargin;
            // 
            // nUpDownM2DurationX2Rest
            // 
            nUpDownM2DurationX2Rest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownM2DurationX2Rest.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDownM2DurationX2Rest.Name = "nUpDownM2Rest";
            nUpDownM2DurationX2Rest.Size = new Size(43, 18);
            nUpDownM2DurationX2Rest.TabStop = false;
            nUpDownM2DurationX2Rest.Maximum = 255;
            nUpDownM2DurationX2Rest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownM2DurationX2Rest.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(nUpDownM2DurationX2Dur, panel2, szFormScaleFactor);
            scaleAndAddToPanel(lblM2DurationX2Dur, panel2, szFormScaleFactor);
            scaleAndAddToPanel(nUpDownM2DurationX2Rest, panel2, szFormScaleFactor);
            scaleAndAddToPanel(lblM2DurationX2Rest, panel2, szFormScaleFactor);

            // #######################################################   controls for the REST DURATION command ####### CHORD CHANNEL
            lblChordRestRest = new System.Windows.Forms.Label();
            nUpDownChordRestRest = new System.Windows.Forms.NumericUpDown();

            iCtrlXOffset = 0;
            // 
            // labChordRestRest
            // 
            lblChordRestRest.AutoSize = true;
            lblChordRestRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblChordRestRest.Name = "labChordRestRest";
            lblChordRestRest.Size = new Size(28, 16);
            lblChordRestRest.TabStop = false;
            lblChordRestRest.Text = "Rest:";
             lblChordRestRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblChordRestRest.Size.Width + iCtrlXMargin;
            // 
            // nUpDownChordRestfRest
            // 
            nUpDownChordRestRest.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownChordRestRest.Name = "nUpDownChordRestRest";
            nUpDownChordRestRest.Size = new Size(43, 22);
            nUpDownChordRestRest.TabStop = false;
            nUpDownChordRestRest.Maximum = 255;
            nUpDownChordRestRest.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownChordRestRest.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblChordRestRest, panel3, szFormScaleFactor);
            scaleAndAddToPanel(nUpDownChordRestRest, panel3, szFormScaleFactor);

            // #######################################################   controls for the CHORD NOTE command ####### CHORD CHANNEL
            lblChordNote = new System.Windows.Forms.Label();
            cmboBoxChordNote = new System.Windows.Forms.ComboBox();
            lblChordNoteType = new System.Windows.Forms.Label();
            cmboBoxChordNoteType = new System.Windows.Forms.ComboBox();
            lblChordNoteDur = new System.Windows.Forms.Label();
            nUpDownChordNoteDur = new System.Windows.Forms.NumericUpDown();

            iCtrlXOffset = 0;
            // 
            // lblChordNote
            // 
            lblChordNote.AutoSize = true;
            lblChordNote.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblChordNote.Name = "lblChordNote";
            lblChordNote.Size = new Size(34, 12);
            lblChordNote.TabStop = false;
            lblChordNote.Text = "Note:";
            lblChordNote.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblChordNote.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxChordNote
            // 
            cmboBoxChordNote.FormattingEnabled = true;
            cmboBoxChordNote.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxChordNote.Name = "cmboBoxChordNote";
            cmboBoxChordNote.Size = new Size(54, 18);
            cmboBoxChordNote.TabStop = false;
            cmboBoxChordNote.DataSource = liChordNotes;
            cmboBoxChordNote.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxChordNote.Size.Width + iCtrlXMargin;

            // 
            // lblChordNoteType
            // 
            lblChordNoteType.AutoSize = true;
            lblChordNoteType.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblChordNoteType.Name = "lblChordNoteType";
            lblChordNoteType.Size = new Size(32, 12);
            lblChordNoteType.TabStop = false;
            lblChordNoteType.Text = "Type:";
            lblChordNoteType.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblChordNoteType.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxChordNoteType
            // 
            cmboBoxChordNoteType.FormattingEnabled = true;
            cmboBoxChordNoteType.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxChordNoteType.Name = "cmboBoxChordNoteType";
            cmboBoxChordNoteType.Size = new Size(80, 18);
            cmboBoxChordNoteType.TabStop = false;
            cmboBoxChordNoteType.DataSource = liChordTypes;
            cmboBoxChordNoteType.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxChordNoteType.Size.Width + iCtrlXMargin;
            // 
            // lblChordNoteDur
            // 
            lblChordNoteDur.AutoSize = true;
            lblChordNoteDur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblChordNoteDur.Name = "lblChordNoteDur";
            lblChordNoteDur.Size = new Size(26, 16);
            lblChordNoteDur.TabStop = false;
            lblChordNoteDur.Text = "Dur:";
            lblChordNoteDur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblChordNoteDur.Size.Width + iCtrlXMargin;
            // 
            // nUpDownChordRestfRest
            // 
            nUpDownChordNoteDur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownChordNoteDur.Name = "nUpDownChordNoteDur";
            nUpDownChordNoteDur.Size = new Size(43, 22);
            nUpDownChordNoteDur.TabStop = false;
            nUpDownChordNoteDur.Maximum = 255;
            nUpDownChordNoteDur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownChordNoteDur.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblChordNote, panel3, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxChordNote, panel3, szFormScaleFactor);
            scaleAndAddToPanel(lblChordNoteType, panel3, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxChordNoteType, panel3, szFormScaleFactor);
            scaleAndAddToPanel(lblChordNoteDur, panel3, szFormScaleFactor);
            scaleAndAddToPanel(nUpDownChordNoteDur, panel3, szFormScaleFactor);

            // #######################################################   controls for the REPEAT command ####### CHORD CHANNEL
            lblChordRepeat = new System.Windows.Forms.Label();
            cmboChordRepeat = new System.Windows.Forms.ComboBox();

            iCtrlXOffset = 0;
            // 
            // lblChordRepeat
            // 
            lblChordRepeat.AutoSize = true;
            lblChordRepeat.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblChordRepeat.Name = "lblChordRepeat";
            lblChordRepeat.Size = new Size(44, 12);
            lblChordRepeat.TabStop = false;
            lblChordRepeat.Text = "Repeat:";
            lblChordRepeat.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblChordRepeat.Size.Width + iCtrlXMargin;
            // 
            // cmboChordRepeat
            // 
            cmboChordRepeat.FormattingEnabled = true;
            cmboChordRepeat.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboChordRepeat.Name = "cmboChordRepeat";
            cmboChordRepeat.Size = new Size(68, 18);
            cmboChordRepeat.TabStop = false;
            cmboChordRepeat.DataSource = liChordRepeatMark;
            cmboChordRepeat.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboChordRepeat.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblChordRepeat, panel3, szFormScaleFactor);
            scaleAndAddToPanel(cmboChordRepeat, panel3, szFormScaleFactor);

            // #######################################################   controls for the RYTHM command ####### CHORD CHANNEL
            lblChordRythmMode = new System.Windows.Forms.Label();
            cmboBoxChordRythmMode = new System.Windows.Forms.ComboBox();
            lblChordRythmStyle = new System.Windows.Forms.Label();
            cmboBoxChorddRythmOnOff = new System.Windows.Forms.ComboBox();
            cmboBoxChorddRythmStyle = new System.Windows.Forms.ComboBox();

            iCtrlXOffset = 0;
            // 
            // lblChordRythmMode
            // 
            lblChordRythmMode.AutoSize = true;
            lblChordRythmMode.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblChordRythmMode.Name = "lblChordRythmMode";
            lblChordRythmMode.Size = new Size(36, 12);
            lblChordRythmMode.TabStop = false;
            lblChordRythmMode.Text = "Mode:";
            lblChordRythmMode.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblChordRythmMode.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxChordRythmMode
            // 
            cmboBoxChordRythmMode.FormattingEnabled = true;
            cmboBoxChordRythmMode.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxChordRythmMode.Name = "cmboBoxChordRythmMode";
            cmboBoxChordRythmMode.Size = new Size(104, 18);
            cmboBoxChordRythmMode.TabStop = false;
            cmboBoxChordRythmMode.DataSource = liChordRythmMode;
            cmboBoxChordRythmMode.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxChordRythmMode.Size.Width + iCtrlXMargin;
            // 
            // lblChordRythmStyle
            // 
            lblChordRythmStyle.AutoSize = true;
            lblChordRythmStyle.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblChordRythmStyle.Name = "lblChordRythmStyle";
            lblChordRythmStyle.Size = new Size(30, 12);
            lblChordRythmStyle.TabStop = false;
            lblChordRythmStyle.Text = "Style:";
            lblChordRythmStyle.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblChordRythmStyle.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxChorddRythmOnOff
            // 
            cmboBoxChorddRythmOnOff.FormattingEnabled = true;
            cmboBoxChorddRythmOnOff.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxChorddRythmOnOff.Name = "cmboBoxChorddRythmOnOff";
            cmboBoxChorddRythmOnOff.Size = new Size(50, 18);
            cmboBoxChorddRythmOnOff.TabStop = false;
            cmboBoxChorddRythmOnOff.DataSource = liMelody1OnOff;
            cmboBoxChorddRythmOnOff.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxChorddRythmOnOff.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxChorddRythmStyle
            // 
            cmboBoxChorddRythmStyle.FormattingEnabled = true;
            cmboBoxChorddRythmStyle.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxChorddRythmStyle.Name = "cmboBoxChorddRythmStyle";
            cmboBoxChorddRythmStyle.Size = new Size(100, 18);
            cmboBoxChorddRythmStyle.TabStop = false;
            cmboBoxChorddRythmStyle.DataSource = liChordRythmStyle;
            cmboBoxChorddRythmStyle.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxChorddRythmStyle.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblChordRythmMode, panel3, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxChordRythmMode, panel3, szFormScaleFactor);
            scaleAndAddToPanel(lblChordRythmStyle, panel3, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxChorddRythmStyle, panel3, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxChorddRythmOnOff, panel3, szFormScaleFactor);

            // #######################################################   controls for the TEMPO command ####### CHORD CHANNEL
            lblChordTempo = new System.Windows.Forms.Label();
            cmboBoxChordTempoOnOff = new System.Windows.Forms.ComboBox();
            nUpDownChordTempo = new System.Windows.Forms.NumericUpDown();

            iCtrlXOffset = 0;
            // 
            // lblChordTempo
            // 
            lblChordTempo.AutoSize = true;
            lblChordTempo.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblChordTempo.Name = "lblChordTempo";
            lblChordTempo.Size = new Size(44, 12);
            lblChordTempo.TabStop = false;
            lblChordTempo.Text = "Tempo:";
            lblChordTempo.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblChordTempo.Size.Width + iCtrlXMargin;
            // 
            // cmboBoxChorddTempoOnOff
            // 
            cmboBoxChordTempoOnOff.FormattingEnabled = true;
            cmboBoxChordTempoOnOff.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            cmboBoxChordTempoOnOff.Name = "cmboBoxChordTempoOnOff";
            cmboBoxChordTempoOnOff.Size = new Size(50, 18);
            cmboBoxChordTempoOnOff.TabStop = false;
            cmboBoxChordTempoOnOff.DataSource = liChordOnOff;
            cmboBoxChordTempoOnOff.Visible = false;
            iCtrlXOffset = iCtrlXOffset + cmboBoxChordTempoOnOff.Size.Width + iCtrlXMargin;
            // 
            // nUpDownTempo
            // 
            nUpDownChordTempo.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownChordTempo.Name = "nUpDownChordTempo";
            nUpDownChordTempo.Size = new Size(56, 22);
            nUpDownChordTempo.TabStop = false;
            nUpDownChordTempo.Maximum = 385;
            nUpDownChordTempo.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownChordTempo.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(lblChordTempo, panel3, szFormScaleFactor);
            scaleAndAddToPanel(cmboBoxChordTempoOnOff, panel3, szFormScaleFactor);
            scaleAndAddToPanel(nUpDownChordTempo, panel3, szFormScaleFactor);

            // ####################################################### controls for the DURATION X2 command ####### CHORD CHANNEL
            lblChordDurationX2Dur = new System.Windows.Forms.Label();
            nUpDownChordDurationX2Dur = new System.Windows.Forms.NumericUpDown();

            iCtrlXOffset = 0;
            // 
            // lblChordDurationX2Dur
            // 
            lblChordDurationX2Dur.AutoSize = true;
            lblChordDurationX2Dur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord + iLbYOffset);
            lblChordDurationX2Dur.Name = "labChordDur";
            lblChordDurationX2Dur.Size = new Size(26, 12);
            lblChordDurationX2Dur.TabStop = false;
            lblChordDurationX2Dur.Text = "Dur:";
            lblChordDurationX2Dur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + lblChordDurationX2Dur.Size.Width + iCtrlXMargin;
            // 
            // nUpDownChordDurationX2Dur
            // 
            nUpDownChordDurationX2Dur.Location = new Point(iCtrlXcoord + iCtrlXOffset, iCtrlYcoord);
            nUpDownChordDurationX2Dur.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDownChordDurationX2Dur.Name = "nUpDownChordDur";
            nUpDownChordDurationX2Dur.Size = new Size(43, 18);
            nUpDownChordDurationX2Dur.TabStop = false;
            nUpDownChordDurationX2Dur.Maximum = 255;
            nUpDownChordDurationX2Dur.Visible = false;
            iCtrlXOffset = iCtrlXOffset + nUpDownChordDurationX2Dur.Size.Width + iCtrlXMargin;

            scaleAndAddToPanel(nUpDownChordDurationX2Dur, panel3, szFormScaleFactor);
            scaleAndAddToPanel(lblChordDurationX2Dur, panel3, szFormScaleFactor);


            // initialize de content of the M1, M2 and Chords instruction editon combo boxes
            // get all the melody command codes in the enumerate and add them to the list for the comboBox
            foreach (MChannelCodeEntry.t_Command tcommand in Enum.GetValues(typeof(MChannelCodeEntry.t_Command))) {
                liMelody1Cmds.Add(MChannelCodeEntry.tCommandToString(tcommand));
                liMelody2Cmds.Add(MChannelCodeEntry.tCommandToString(tcommand));
            }
            cmboBoxM1Instr.DataSource = liMelody1Cmds;
            cmboBoxM2Instr.DataSource = liMelody2Cmds;
            // get all the melody command codes in the enumerate and add them to the list for the comboBox
            foreach (ChordChannelCodeEntry.t_Command tcommand in Enum.GetValues(typeof(ChordChannelCodeEntry.t_Command))) {
                liChordCmds.Add(ChordChannelCodeEntry.tCommandToString(tcommand));
            }
            cmboBoxChordInstr.DataSource = liChordCmds;

        }//InitEditInstructionControls

        /*******************************************************************************
        *  @brief InitControls
        *******************************************************************************/
        public ErrCode InitControls() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            MChannelCodeEntry melodyCodeEntryAux = null;
            ChordChannelCodeEntry chordCodeEntryAux = null;
            string str_aux = "";

            // loads the configuration parameters according to the last state of the application
            ec_ret_val = configMgr.LoadConfigParameters();
            if (ec_ret_val.i_code < 0) {

                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, ec_ret_val.str_description, true);
                System.Windows.Forms.Application.Exit();

            }//if

            // creates or opens the logs file where are stored the events that happen during application execution 
            statusNLogs.MessagesInit(configMgr.m_str_logs_path, configMgr.m_b_new_log_per_sesion, txBoxLogs, statusStrip1, toolStripStatusLabel1);
            if (ec_ret_val.i_code < 0) {

                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, ec_ret_val.str_description, true);
                System.Windows.Forms.Application.Exit();

            } else {

                str_aux = "Log file open/created in \"" + configMgr.m_str_logs_path + "Logs\\\".";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, str_aux, false);
                str_aux = "User \"" + System.Environment.UserName + "\" logged in \"" + System.Environment.MachineName + "\".";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, str_aux, false);

            }//if

            // create and edit the properties of Be Hex editor
            hexb_romEditor = new HexBox();
            hexb_romEditor.Location = new System.Drawing.Point(9, lblROMContent.Height + 15);
            hexb_romEditor.Width = tabControlMain.TabPages[2].Width - 22;
            hexb_romEditor.Height = tabControlMain.TabPages[2].Height - (lblROMContent.Height + 15);
            hexb_romEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right))));
            hexb_romEditor.BytesPerLine = 16;
            hexb_romEditor.UseFixedBytesPerLine = true;
            hexb_romEditor.LineInfoVisible = true;
            hexb_romEditor.ColumnInfoVisible = true;
            hexb_romEditor.VScrollBarVisible = true;
            hexb_romEditor.GroupSize = 4;
            hexb_romEditor.HexCasing = HexCasing.Lower;
            hexb_romEditor.StringViewVisible = true;
            hexb_romEditor.Font = new Font(HEX_FONT, HEX_SIZE);

            // clear all the themes and ROM information
            dpack_drivePack.Initialize(configMgr.m_str_default_rom_file);
            
            // clear the user activity history
            historyThemesState.Clear();

            // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor with the content decoded from the loaded file
            hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;

            // as the dynbyprMemoryBytes has been recalculated, then the event delegate must be linked again and will be called every time
            // there is a change in the content of the Be Hex editor
            dpack_drivePack.dynbyprMemoryBytes.Changed += new System.EventHandler(this.BeHexEditorChanged);
            hexb_romEditor.ByteProvider.ApplyChanges();

            // clear the current file name variable content
            configMgr.m_str_cur_rom_file = "";

            // add the Be Hex editor to the corresponding tab page
            tabControlMain.TabPages[2].Controls.Add(hexb_romEditor);

            // create and initialize the controls used to edit the content of the melody and chords new instructions
            InitInstructionEditionControls();

            // set to visible the instruciton edition controls that correspond to the current instruction
            melodyCodeEntryAux = new MChannelCodeEntry(-1,0x14,0x10,0x10);//Default instruction Note:C4 Dur:0x10 Rest:0x10
            chordCodeEntryAux = new ChordChannelCodeEntry();
            SetVisibleM1InstructionEditionControls(melodyCodeEntryAux);
            SetVisibleM2InstructionEditionControls(melodyCodeEntryAux);
            SetVisibleChordInstructionEditionControls(chordCodeEntryAux);

            // set application controls state according to the configuration parameters values
            UpdateAppWithConfigParameters(true);

            // store current application state into history stack to allow recovering it with Ctrl+Z
            storeSelectedDGridViewRows();
            historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

            return ec_ret_val;

        }//InitControls

        /*******************************************************************************
        *  @brief Method that shows a message to the user requesting confirmation to 
        *  close or keep current porject. This method is called before closing the application,
        *  or before opening or creating a new project when exists another project is already
        *  open or pendint to be saved to disk.
        *  @param[in] the text that is going to be shown to the user.
        *  @return true: si se confirma que hay que cerrar el proyecto.
        *          false: si no hay que cerrar el proyecto
        *******************************************************************************/
        private bool ConfirmCloseProject(string str_message) {
            bool b_pending_modifications = true;
            bool b_close_project = true;


            if (dpack_drivePack.dataChanged == false) {
                // no hay modificaciones pendientes de guardarse así que se puede cerrar todo directamente
                b_pending_modifications = false;
                b_close_project = true;
            } else {
                // hay modificaciones pendientes de guardar en disco
                b_pending_modifications = true;
            }//if    

            if (b_pending_modifications) {

                // si hay un proyecto abierto se pregunta antes de salir
                DialogResult dialogResult = MessageBox.Show(str_message, "Close?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes) {
                    b_close_project = true;
                } else if (dialogResult == DialogResult.No) {
                    // NO hay que salir
                    b_close_project = false;
                }//if

            } else {

                // si no hay moficaciones pendientes de guardarse se sale directamente
                b_close_project = true;

            }//if

            return b_close_project;

        }//ConfirmCloseProject

        /*******************************************************************************
         * @brief Check if coordinates are within the screen bounds. This procedure checks
         * whether the provided x and y coordinates are valid screen coordinates, meaning 
         * they do not fall outside the screen dimensions.
         * @param[in] x The x-coordinate.
         * @param[in] y The y-coordinate.
         * @return
         *   - true if the coordinates are valid.
         *   - false if the coordinates fall outside the screen.
         *******************************************************************************/
        private bool AreCorrdinatesInScreenBounds(int x, int y) {
            bool b_are_in_bound = false;
            int x1, x2;
            int y1, y2;


            // process each available screen and check if the received X,Y coordinates are in the area of any of the available screens
            foreach (var screen in Screen.AllScreens) {

                // Get the information of each available screen
                // str_aux = (" Device Name: " + screen.DeviceName);
                // str_aux = str_aux + (" Bounds: " + screen.Bounds.ToString());
                // str_aux = str_aux + (" Type: " + screen.GetType().ToString());
                // str_aux = str_aux + (" Working Area: " + screen.WorkingArea.ToString());
                // str_aux = str_aux + (" Primary Screen: " + screen.Primary.ToString());

                // Check if the received coordinates is in the bounds of any of the available screens
                x1 = screen.Bounds.X;
                x2 = screen.Bounds.X + screen.Bounds.Width;
                y1 = screen.Bounds.Y;
                y2 = screen.Bounds.Y + screen.Bounds.Height;
                if ((x > x1) && (x < x2) && (y > y1) && (y < y2)) {
                    b_are_in_bound = true;
                }

            }//foreach

            return b_are_in_bound;

        }//AreCorrdinatesInScreenBounds

        /*******************************************************************************
        * @brief shows the About dialog with the application version general information.
        * @param[in] parentLocation x y coordinates of the top left corner of the parent
        * form.
        * @param[in] parentSize width and height of the parent form
        *******************************************************************************/
        private void showAboutDialog(Point parentLocation, Size parentSize) {
            AboutBox aboutForm = null;
            string strName = "";
            string strLicense = "";
            string strDescription = "";
            Point formCoordinates;
            int iX, iY = 0;

            strName = "drivePack Editor ";
            strLicense = "(c) Tolaemon 2024";
            strDescription = "";
            aboutForm = new AboutBox(strName, strLicense, strDescription);
            aboutForm.MinimizeBox = false;
            aboutForm.MaximizeBox = false;

            // calculate center of the parent Form
            iX = parentLocation.X + (parentSize.Width / 2);
            iY = parentLocation.Y + (parentSize.Height / 2);
            // match the center of the parent Form with that Form
            iX = iX - (aboutForm.Width / 2);
            iY = iY - (aboutForm.Height / 2);
            formCoordinates = new Point(iX, iY);

            aboutForm.Show(this);

            aboutForm.Location = formCoordinates;

        }//showAboutDialog

        /*******************************************************************************
        * @brief dissables the delegates of some controls to to avoid triggering them when
        * updating their content programatically from other parts of the code. This method
        * is used in in conjuction with ReEnableDelegates().
        *******************************************************************************/
        public void DisableDelegates() {

            themeTitlesDataGridView.CellValueChanged -= themeTitlesDataGridView_CellValueChanged;
            themeSelectComboBox.Leave -= romTitleTextBox_Leave;

        }//ReEnableDelegates

        /*******************************************************************************
        * @brief enables again the delegates of some controls that where preivously disabled
        * with DisableDelegates() to to avoid triggering them when updating their content
        * programatically from other parts of the code.
        *******************************************************************************/
        public void ReEnableDelegates() {

            themeSelectComboBox.Leave += romTitleTextBox_Leave;
            themeTitlesDataGridView.CellValueChanged += themeTitlesDataGridView_CellValueChanged;

        }//ReEnableDelegates

        /*******************************************************************************
        * @brief Enable or Disable Application Controls. This procedure enables or disables
        * the application's controls to prevent user interaction with the application while
        * it's performing other tasks or processing data.
        * @param b_enable true to enable controls, false to disable them.
        *******************************************************************************/
        public void EnableDisableControls(bool b_enable) {

            if (b_enable == false) {

                /* JBR 2024-04-23 Revisar
                // si estaban habilitados y hay que deshabilitarlos se deshabilitan
                if (b_last_ctrls_enabled_state == true) {

                    // se deshabilitan los botones de la TabPage con la lista de TCUs
                    btnAddCon.Enabled = false;
                    btnDelCon.Enabled = false;
                    btnReadCons.Enabled = false;
                    btnWriteCons.Enabled = false;
                    btnReadCons.Enabled = false;
                    btnGetVarsCDU.Enabled = false;
                    btnGetVarsCFG.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    exportBinStripMenuItem1.Enabled = false;
                    toolClosepMenuItem.Enabled = false;

                    // se deshabilitan los botones de cada una de las TabPages de cada conexion
                    // Button button_aux;
                    foreach (cConnection ccon_aux in ProjectDataModel.m_list_connections) {
                        // si existe una tabpage por coenxion se deshabilita tambien cada uno sus controles
                        if (ccon_aux.m_con_tab_page_ref != null) {
                            foreach (Control control in ccon_aux.m_con_tab_page_ref.Controls) {
                                control.Enabled = false;
                            }//foreach
                        }//if
                    }//foreach

                    // si hay tabPage MultiCons creada entonces se deshabilitan todos los controles contenidos en este
                    TabPage tpage_aux = (TabPage) tabControlMain.Controls[CTRL_MULTI_CON_VARS_TAB_NAME];
                    if (tpage_aux !=null) {
                        foreach (Control control in tpage_aux.Controls) {
                            control.Enabled = false;
                        }//foreach
                    }

                }//if                
                b_last_ctrls_enabled_state = false;                
                JBR 2024-04-23 Revisar */

            } else {
                // se habilitan los botones disponibles

                /* JBR 2024-04-23 Revisar
                // si estaban deshabilitados y hay que habilitarlos se habilitan
                if (b_last_ctrls_enabled_state == false) {

                    // se habilitan los botones de la TabPage con la lista de TCUs
                    btnAddCon.Enabled = true;
                    btnDelCon.Enabled = true;
                    btnReadCons.Enabled = true;
                    btnWriteCons.Enabled = true;
                    btnGetVarsCDU.Enabled = true;
                    btnGetVarsCFG.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                    exportBinStripMenuItem1.Enabled = true;
                    toolClosepMenuItem.Enabled = true;

                    // se habilitan los botones de cada una de las TabPages de cada conexion
                    // Button button_aux;                   
                    foreach (cConnection ccon_aux in ProjectDataModel.m_list_connections) {
                        // si existe una tabpage por coenxion se habilita tambien cada uno sus controles
                        if (ccon_aux.m_con_tab_page_ref != null) {
                            foreach (Control control in ccon_aux.m_con_tab_page_ref.Controls) {
                                control.Enabled = true;
                            }//foreach
                        }//if
                    }//foreach

                    // si hay tabPage MultiCons creada entonces se habilitan de nuevo todos los controles contenidos en este
                    TabPage tpage_aux = (TabPage)tabControlMain.Controls[CTRL_MULTI_CON_VARS_TAB_NAME];
                    if (tpage_aux != null) {
                        foreach (Control control in tpage_aux.Controls) {
                            control.Enabled = true;
                        }//foreach
                    }

                }//if
                b_last_ctrls_enabled_state = true;
                FIN JBR 2024-04-23 Revisar */

            }//if (b_enable == false)

        }//EnableDisableControls

        /*******************************************************************************
        * @brief
        *******************************************************************************/
        public void RefreshHexEditor() {

            // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
            hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
            hexb_romEditor.ByteProvider.ApplyChanges();

        }//RefreshHexEditor

        /*******************************************************************************
        * @brief Update, enable or disable the corresponding M1 channel instruction edition  
        * controls according to the instrucion in the received instruction. It also uses 
        * the data  in the received instruction to retrieve the data used to initialize the 
        * corresponding instruction edition controls.
        * 
        * @param[in] chanCodeEntry melody channel instruction from which the command type 
        * and the values of some specific controls are obtained.
        *******************************************************************************/
        public void SetVisibleM1InstructionEditionControls(MChannelCodeEntry chanCodeEntry) {
            MChannelCodeEntry.t_Command chanCmdType = MChannelCodeEntry.t_Command.BAR;


            // if the main Form has been created and is visible
            if (this.Visible) {

                // get the type of command encoded in the instruction
                chanCmdType = chanCodeEntry.GetCmdType();

                // ################# update the M1 instruction edition controls according to the current selected instruction

                // show or hide M1 NOTE ON command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.NOTE) {
                    // if instruction is NOTE the enable and show the controls that allow to
                    // modify and update NOTE command
                    lblM1Note.Enabled = true;
                    lblM1Note.Visible = true;
                    cmboBoxM1Note.Enabled = true;
                    cmboBoxM1Note.Visible = true;
                    lblM1NoteDur.Enabled = true;
                    lblM1NoteDur.Visible = true;
                    nUpDownM1NoteDur.Enabled = true;
                    nUpDownM1NoteDur.Visible = true;
                    lblM1NoteRest.Enabled = true;
                    lblM1NoteRest.Visible = true;
                    nUpDownM1NoteRest.Enabled = true;
                    nUpDownM1NoteRest.Visible = true;

                    // get the values to show into the NOTE edition controls from the received command
                    MChannelCodeEntry.t_Notes tNoteParam = MChannelCodeEntry.t_Notes.C4;
                    int iDurationParam = 0;
                    int iRestParam = 0;
                    MChannelCodeEntry.GetNoteCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tNoteParam, ref iDurationParam, ref iRestParam);
                    cmboBoxM1Note.Text = MChannelCodeEntry.tNotesToString(tNoteParam);
                    nUpDownM1NoteDur.Value = iDurationParam;
                    nUpDownM1NoteRest.Value = iRestParam;

                } else {
                    // if instruction is not NOTE then disable and hide the controls used to
                    // modify and update NOTE command
                    lblM1Note.Enabled = false;
                    lblM1Note.Visible = false;
                    cmboBoxM1Note.Enabled = false;
                    cmboBoxM1Note.Visible = false;
                    lblM1NoteDur.Enabled = false;
                    lblM1NoteDur.Visible = false;
                    nUpDownM1NoteDur.Enabled = false;
                    nUpDownM1NoteDur.Visible = false;
                    lblM1NoteRest.Enabled = false;
                    lblM1NoteRest.Visible = false;
                    nUpDownM1NoteRest.Enabled = false;
                    nUpDownM1NoteRest.Visible = false;
                }

                // show or hide M1 TIMBRE command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.TIMBRE_INSTRUMENT) {
                    // if instruction is TIMBRE the enable and show the controls that allow to
                    // modify and update TIMBRE command
                    lblM1Timbre.Enabled = true;
                    lblM1Timbre.Visible = true;
                    cmboBoxM1Timbre.Enabled = true;
                    cmboBoxM1Timbre.Visible = true;
                    cmboBoxM1TimbreOnOff.Enabled = true;
                    cmboBoxM1TimbreOnOff.Visible = true;
                    lblM1TimbreRest.Enabled = true;
                    lblM1TimbreRest.Visible = true;
                    nUpDownM1TimbreRest.Enabled = true;
                    nUpDownM1TimbreRest.Visible = true;

                    // get the values to show into the NOTE edition controls from the received command
                    MChannelCodeEntry.t_Instrument tInstrumentParam = MChannelCodeEntry.t_Instrument.PIANO;
                    MChannelCodeEntry.t_On_Off tOnOffParam = MChannelCodeEntry.t_On_Off.ON;
                    int iRestParam = 0;
                    MChannelCodeEntry.GetInstrumentCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tInstrumentParam, ref tOnOffParam, ref iRestParam);
                    cmboBoxM1Timbre.Text = MChannelCodeEntry.tInstrumentToString(tInstrumentParam);
                    cmboBoxM1TimbreOnOff.Text = MChannelCodeEntry.tOnOffToString(tOnOffParam);
                    nUpDownM1TimbreRest.Value = iRestParam;

                } else {
                    // if instruction is not TIMBRE then disable and hide the controls used to
                    // modify and update TIMBRE command
                    lblM1Timbre.Enabled = false;
                    lblM1Timbre.Visible = false;
                    cmboBoxM1Timbre.Enabled = false;
                    cmboBoxM1Timbre.Visible = false;
                    cmboBoxM1TimbreOnOff.Enabled = false;
                    cmboBoxM1TimbreOnOff.Visible = false;
                    lblM1TimbreRest.Enabled = false;
                    lblM1TimbreRest.Visible = false;
                    nUpDownM1TimbreRest.Enabled = false;
                    nUpDownM1TimbreRest.Visible = false;
                }

                // show or hide M1 EFFECT command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.EFFECT) {
                    // if instruction is EFFECT the enable and show the controls that allow to
                    // modify and update EFFECT command
                    lblM1Effect.Enabled = true;
                    lblM1Effect.Visible = true;
                    cmbBoxM1Effect.Enabled = true;
                    cmbBoxM1Effect.Visible = true;
                    cmbBoxM1EffectOnOff.Enabled = true;
                    cmbBoxM1EffectOnOff.Visible = true;
                    lblM1EffRest.Enabled = true;
                    lblM1EffRest.Visible = true;
                    nUpDownM1EffRest.Enabled = true;
                    nUpDownM1EffRest.Visible = true;

                    // get the values to show into the EFFECT edition controls from the received command
                    MChannelCodeEntry.t_Effect tEffectParam = MChannelCodeEntry.t_Effect.SUST0;
                    MChannelCodeEntry.t_On_Off tOnOffParam = MChannelCodeEntry.t_On_Off.ON;
                    int iRestParam = 0;
                    MChannelCodeEntry.GetEffectCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tEffectParam, ref tOnOffParam, ref iRestParam);
                    cmbBoxM1Effect.Text = MChannelCodeEntry.tEffectToString(tEffectParam);
                    cmbBoxM1EffectOnOff.Text = MChannelCodeEntry.tOnOffToString(tOnOffParam);
                    nUpDownM1EffRest.Value = iRestParam;

                } else {
                    // if instruction is not EFFECT then disable and hide the controls used to
                    // modify and update EFFECT command
                    lblM1Effect.Enabled = false;
                    lblM1Effect.Visible = false;
                    cmbBoxM1Effect.Enabled = false;
                    cmbBoxM1Effect.Visible = false;
                    cmbBoxM1EffectOnOff.Enabled = false;
                    cmbBoxM1EffectOnOff.Visible = false;
                    lblM1EffRest.Enabled = false;
                    lblM1EffRest.Visible = false;
                    nUpDownM1EffRest.Enabled = false;
                    nUpDownM1EffRest.Visible = false;
                }

                // show or hide M1 REST command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.REST_DURATION) {
                    // if instruction is REST the enable and show the controls that allow to
                    // modify and update REST command
                    lblM1RestRest.Enabled = true;
                    lblM1RestRest.Visible = true;
                    nUpDownM1RestRest.Enabled = true;
                    nUpDownM1RestRest.Visible = true;

                    // get the values to show into the REST edition controls from the received command
                    int iRestParam=0;
                    MChannelCodeEntry.GetRestCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref iRestParam);
                    nUpDownM1RestRest.Value = iRestParam;

                } else {
                    // if instruction is not REST then disable and hide the controls used to
                    // modify and update REST command
                    lblM1RestRest.Enabled = false;
                    lblM1RestRest.Visible = false;
                    nUpDownM1RestRest.Enabled = false;
                    nUpDownM1RestRest.Visible = false;
                }

                // show or hide M1 REPEAT command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.REPEAT) {
                    // if instruction is REPEAT the enable and show the controls that allow to
                    // modify and update REPEAT command
                    lblM1Repeat.Enabled = true;
                    lblM1Repeat.Visible = true;
                    cmboBoxM1Repeat.Enabled = true;
                    cmboBoxM1Repeat.Visible = true;

                    // get the values to show into the REPEAT edition controls from the received command
                    MChannelCodeEntry.t_RepeatMark tRepeatParam = MChannelCodeEntry.t_RepeatMark.START;
                    MChannelCodeEntry.GetRepeatCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tRepeatParam);
                    cmboBoxM1Repeat.Text = MChannelCodeEntry.tRepeatMarkToString(tRepeatParam);

                } else {
                    // if instruction is not REPEAT then disable and hide the controls used to
                    // modify and update REPEAT command
                    lblM1Repeat.Enabled = false;
                    lblM1Repeat.Visible = false;
                    cmboBoxM1Repeat.Enabled = false;
                    cmboBoxM1Repeat.Visible = false;
                }

                // show or hide M1 TIE command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.TIE) {
                    // if instruction is TIE the enable and show the controls that allow to
                    // modify and update TIE command
                    lblM1Tie.Enabled = true;
                    lblM1Tie.Visible = true;
                    cmboBoxM1Tie.Enabled = true;
                    cmboBoxM1Tie.Visible = true;

                    // get the values to show into the TIE edition controls from the received command
                    MChannelCodeEntry.t_On_Off tOnOfftParam = MChannelCodeEntry.t_On_Off.ON;
                    MChannelCodeEntry.GetTieCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tOnOfftParam);
                    cmboBoxM1Tie.Text = MChannelCodeEntry.tOnOffToString(tOnOfftParam);

                } else {
                    // if instruction is not TIE then disable and hide the controls used to
                    // modify and update TIE command
                    lblM1Tie.Enabled = false;
                    lblM1Tie.Visible = false;
                    cmboBoxM1Tie.Enabled = false;
                    cmboBoxM1Tie.Visible = false;
                }

                // show or hide M1 TIME command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.TIME) {
                    // if instruction is TIME the enable and show the controls that allow to
                    // modify and update TIME command
                    lblM1Time.Enabled = true;
                    lblM1Time.Visible = true;
                    cmboBoxM1Time.Enabled = true;
                    cmboBoxM1Time.Visible = true;

                    // get the values to show into the TIME edition controls from the received command
                    MChannelCodeEntry.t_Time tTimeParam  = MChannelCodeEntry.t_Time._4x4;
                    MChannelCodeEntry.GetTimeCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tTimeParam);
                    cmboBoxM1Time.Text = MChannelCodeEntry.tTimeToString(tTimeParam);

                } else {
                    // if instruction is not TIME then disable and hide the controls used to
                    // modify and update TIME command
                    lblM1Time.Enabled = false;
                    lblM1Time.Visible = false;
                    cmboBoxM1Time.Enabled = false;
                    cmboBoxM1Time.Visible = false;
                }

                // show or hide M1 KEY command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.KEY) {
                    // if instruction is KEY the enable and show the controls that allow to
                    // modify and update KEY command
                    lblM1Key.Enabled = true;
                    lblM1Key.Visible = true;
                    nUpDownM1Key.Enabled = true;
                    nUpDownM1Key.Visible = true;

                    // get the values to show into the KEY edition controls from the received command
                    int iKeySymbolParam = 0;
                    MChannelCodeEntry.GetKeyCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref iKeySymbolParam);
                    nUpDownM1Key.Value = iKeySymbolParam;

                } else {
                    // if instruction is not KEY then disable and hide the controls used to
                    // modify and update KEY command
                    lblM1Key.Enabled = false;
                    lblM1Key.Visible = false;
                    nUpDownM1Key.Enabled = false;
                    nUpDownM1Key.Visible = false;
                }

                // show or hide M1 DURATIONx2 command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.DURATIONx2) {
                    lblM1DurationX2Dur.Enabled = true;
                    lblM1DurationX2Dur.Visible = true;
                    nUpDownM1DurationX2Dur.Enabled = true;
                    nUpDownM1DurationX2Dur.Visible = true;
                    lblM1DurationX2Rest.Enabled = true;
                    lblM1DurationX2Rest.Visible = true;
                    nUpDownM1DurationX2Rest.Visible = true;
                    nUpDownM1DurationX2Rest.Enabled = true;

                } else {
                    lblM1DurationX2Dur.Enabled = false;
                    lblM1DurationX2Dur.Visible = false;
                    nUpDownM1DurationX2Dur.Enabled = false;
                    nUpDownM1DurationX2Dur.Visible = false;
                    lblM1DurationX2Rest.Enabled = false;
                    lblM1DurationX2Rest.Visible = false;
                    nUpDownM1DurationX2Rest.Visible = false;
                    nUpDownM1DurationX2Rest.Enabled = false;
                }

            }//if (this.Visible)

        }// SetVisibleM1InstructionEditionControls

        /*******************************************************************************
        * @brief Update, enable or disable the corresponding M2 channel instruction edition  
        * controls according to the instrucion in the received instruction. It also uses 
        * the data  in the received instruction to retrieve the data used to initialize the 
        * corresponding instruction edition controls.
        * 
        * @param[in] chanCodeEntry melody channel instruction from which the command type 
        * and the values of some specific controls are obtained.
        *******************************************************************************/
        public void SetVisibleM2InstructionEditionControls(MChannelCodeEntry chanCodeEntry) {
            MChannelCodeEntry.t_Command chanCmdType = MChannelCodeEntry.t_Command.BAR;


            // if the main Form has been created and is visible
            if (this.Visible) {

                // get the type of command encoded in the instruction
                chanCmdType = chanCodeEntry.GetCmdType();

                // ################# update the M2 instruction edition controls according to the current selected instruction

                // show or hide M2 NOTE ON command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.NOTE) {
                    // if instruction is NOTE the enable and show the controls that allow to
                    // modify and update NOTE command
                    lblM2Note.Enabled = true;
                    lblM2Note.Visible = true;
                    cmboBoxM2Note.Enabled = true;
                    cmboBoxM2Note.Visible = true;
                    lblM2NoteDur.Enabled = true;
                    lblM2NoteDur.Visible = true;
                    nUpDownM2NoteDur.Enabled = true;
                    nUpDownM2NoteDur.Visible = true;
                    lblM2NoteRest.Enabled = true;
                    lblM2NoteRest.Visible = true;
                    nUpDownM2NoteRest.Enabled = true;
                    nUpDownM2NoteRest.Visible = true;

                    // get the values to show into the NOTE edition controls from the received command
                    MChannelCodeEntry.t_Notes tNoteParam = MChannelCodeEntry.t_Notes.C4;
                    int iDurationParam = 0;
                    int iRestParam = 0;
                    MChannelCodeEntry.GetNoteCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tNoteParam, ref iDurationParam, ref iRestParam);
                    cmboBoxM2Note.Text = MChannelCodeEntry.tNotesToString(tNoteParam);
                    nUpDownM2NoteDur.Value = iDurationParam;
                    nUpDownM2NoteRest.Value = iRestParam;

                } else {
                    // if instruction is not NOTE then disable and hide the controls used to
                    // modify and update NOTE command
                    lblM2Note.Enabled = false;
                    lblM2Note.Visible = false;
                    cmboBoxM2Note.Enabled = false;
                    cmboBoxM2Note.Visible = false;
                    lblM2NoteDur.Enabled = false;
                    lblM2NoteDur.Visible = false;
                    nUpDownM2NoteDur.Enabled = false;
                    nUpDownM2NoteDur.Visible = false;
                    lblM2NoteRest.Enabled = false;
                    lblM2NoteRest.Visible = false;
                    nUpDownM2NoteRest.Enabled = false;
                    nUpDownM2NoteRest.Visible = false;
                }

                // show or hide M2 TIMBRE command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.TIMBRE_INSTRUMENT) {
                    // if instruction is TIMBRE the enable and show the controls that allow to
                    // modify and update TIMBRE command
                    lblM2Timbre.Enabled = true;
                    lblM2Timbre.Visible = true;
                    cmboBoxM2Timbre.Enabled = true;
                    cmboBoxM2Timbre.Visible = true;
                    cmboBoxM2TimbreOnOff.Enabled = true;
                    cmboBoxM2TimbreOnOff.Visible = true;
                    lblM2TimbreRest.Enabled = true;
                    lblM2TimbreRest.Visible = true;
                    nUpDownM2TimbreRest.Enabled = true;
                    nUpDownM2TimbreRest.Visible = true;

                    // get the values to show into the NOTE edition controls from the received command
                    MChannelCodeEntry.t_Instrument tInstrumentParam = MChannelCodeEntry.t_Instrument.PIANO;
                    MChannelCodeEntry.t_On_Off tOnOffParam = MChannelCodeEntry.t_On_Off.ON;
                    int iRestParam = 0;
                    MChannelCodeEntry.GetInstrumentCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tInstrumentParam, ref tOnOffParam, ref iRestParam);
                    cmboBoxM2Timbre.Text = MChannelCodeEntry.tInstrumentToString(tInstrumentParam);
                    cmboBoxM2TimbreOnOff.Text = MChannelCodeEntry.tOnOffToString(tOnOffParam);
                    nUpDownM2TimbreRest.Value = iRestParam;

                } else {
                    // if instruction is not TIMBRE then disable and hide the controls used to
                    // modify and update TIMBRE command
                    lblM2Timbre.Enabled = false;
                    lblM2Timbre.Visible = false;
                    cmboBoxM2Timbre.Enabled = false;
                    cmboBoxM2Timbre.Visible = false;
                    cmboBoxM2TimbreOnOff.Enabled = false;
                    cmboBoxM2TimbreOnOff.Visible = false;
                    lblM2TimbreRest.Enabled = false;
                    lblM2TimbreRest.Visible = false;
                    nUpDownM2TimbreRest.Enabled = false;
                    nUpDownM2TimbreRest.Visible = false;
                }

                // show or hide M2 EFFECT command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.EFFECT) {
                    // if instruction is EFFECT the enable and show the controls that allow to
                    // modify and update EFFECT command
                    lblM2Effect.Enabled = true;
                    lblM2Effect.Visible = true;
                    cmbBoxM2Effect.Enabled = true;
                    cmbBoxM2Effect.Visible = true;
                    cmbBoxM2EffectOnOff.Enabled = true;
                    cmbBoxM2EffectOnOff.Visible = true;
                    lblM2EffRest.Enabled = true;
                    lblM2EffRest.Visible = true;
                    nUpDownM2EffRest.Enabled = true;
                    nUpDownM2EffRest.Visible = true;

                    // get the values to show into the EFFECT edition controls from the received command
                    MChannelCodeEntry.t_Effect tEffectParam = MChannelCodeEntry.t_Effect.SUST0;
                    MChannelCodeEntry.t_On_Off tOnOffParam = MChannelCodeEntry.t_On_Off.ON;
                    int iRestParam = 0;
                    MChannelCodeEntry.GetEffectCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tEffectParam, ref tOnOffParam, ref iRestParam);
                    cmbBoxM2Effect.Text = MChannelCodeEntry.tEffectToString(tEffectParam);
                    cmbBoxM2EffectOnOff.Text = MChannelCodeEntry.tOnOffToString(tOnOffParam);
                    nUpDownM2EffRest.Value = iRestParam;

                } else {
                    // if instruction is not EFFECT then disable and hide the controls used to
                    // modify and update EFFECT command
                    lblM2Effect.Enabled = false;
                    lblM2Effect.Visible = false;
                    cmbBoxM2Effect.Enabled = false;
                    cmbBoxM2Effect.Visible = false;
                    cmbBoxM2EffectOnOff.Enabled = false;
                    cmbBoxM2EffectOnOff.Visible = false;
                    lblM2EffRest.Enabled = false;
                    lblM2EffRest.Visible = false;
                    nUpDownM2EffRest.Enabled = false;
                    nUpDownM2EffRest.Visible = false;
                }

                // show or hide M2 REST command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.REST_DURATION) {
                    // if instruction is REST the enable and show the controls that allow to
                    // modify and update REST command
                    lblM2RestRest.Enabled = true;
                    lblM2RestRest.Visible = true;
                    nUpDownM2RestRest.Enabled = true;
                    nUpDownM2RestRest.Visible = true;

                    // get the values to show into the REST edition controls from the received command
                    int iRestParam = 0;
                    MChannelCodeEntry.GetRestCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref iRestParam);
                    nUpDownM2RestRest.Value = iRestParam;

                } else {
                    // if instruction is not REST then disable and hide the controls used to
                    // modify and update REST command
                    lblM2RestRest.Enabled = false;
                    lblM2RestRest.Visible = false;
                    nUpDownM2RestRest.Enabled = false;
                    nUpDownM2RestRest.Visible = false;
                }

                // show or hide M2 REPEAT command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.REPEAT) {
                    // if instruction is REPEAT the enable and show the controls that allow to
                    // modify and update REPEAT command
                    lblM2Repeat.Enabled = true;
                    lblM2Repeat.Visible = true;
                    cmboBoxM2Repeat.Enabled = true;
                    cmboBoxM2Repeat.Visible = true;

                    // get the values to show into the REPEAT edition controls from the received command
                    MChannelCodeEntry.t_RepeatMark tRepeatParam = MChannelCodeEntry.t_RepeatMark.START;
                    MChannelCodeEntry.GetRepeatCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tRepeatParam);
                    cmboBoxM2Repeat.Text = MChannelCodeEntry.tRepeatMarkToString(tRepeatParam); 

                } else {
                    // if instruction is not REPEAT then disable and hide the controls used to
                    // modify and update REPEAT command
                    lblM2Repeat.Enabled = false;
                    lblM2Repeat.Visible = false;
                    cmboBoxM2Repeat.Enabled = false;
                    cmboBoxM2Repeat.Visible = false;
                }

                // show or hide M2 TIE command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.TIE) {
                    // if instruction is TIE the enable and show the controls that allow to
                    // modify and update TIE command
                    lblM2Tie.Enabled = true;
                    lblM2Tie.Visible = true;
                    cmboBoxM2Tie.Enabled = true;
                    cmboBoxM2Tie.Visible = true;

                    // get the values to show into the TIE edition controls from the received command
                    MChannelCodeEntry.t_On_Off tOnOfftParam = MChannelCodeEntry.t_On_Off.ON;
                    MChannelCodeEntry.GetTieCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tOnOfftParam);
                    cmboBoxM2Tie.Text = MChannelCodeEntry.tOnOffToString(tOnOfftParam); 

                } else {
                    // if instruction is not TIE then disable and hide the controls used to
                    // modify and update TIE command
                    lblM2Tie.Enabled = false;
                    lblM2Tie.Visible = false;
                    cmboBoxM2Tie.Enabled = false;
                    cmboBoxM2Tie.Visible = false;
                }

                // show or hide M2 TIME command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.TIME) {
                    // if instruction is TIME the enable and show the controls that allow to
                    // modify and update TIME command
                    lblM2Time.Enabled = true;
                    lblM2Time.Visible = true;
                    cmboBoxM2Time.Enabled = true;
                    cmboBoxM2Time.Visible = true;

                    // get the values to show into the TIME edition controls from the received command
                    MChannelCodeEntry.t_Time tTimeParam = MChannelCodeEntry.t_Time._4x4;
                    MChannelCodeEntry.GetTimeCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref tTimeParam);
                    cmboBoxM2Time.Text = MChannelCodeEntry.tTimeToString(tTimeParam);


                } else {
                    // if instruction is not TIME then disable and hide the controls used to
                    // modify and update TIME command
                    lblM2Time.Enabled = false;
                    lblM2Time.Visible = false;
                    cmboBoxM2Time.Enabled = false;
                    cmboBoxM2Time.Visible = false;
                }

                // show or hide M2 KEY command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.KEY) {
                    // if instruction is KEY the enable and show the controls that allow to
                    // modify and update KEY command
                    lblM2Key.Enabled = true;
                    lblM2Key.Visible = true;
                    nUpDownM2Key.Enabled = true;
                    nUpDownM2Key.Visible = true;

                    // get the values to show into the KEY edition controls from the received command
                    int iKeySymbolParam = 0;
                    MChannelCodeEntry.GetKeyCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), chanCodeEntry.By2AsByte(), ref iKeySymbolParam);
                    nUpDownM2Key.Value = iKeySymbolParam;

                } else {
                    // if instruction is not KEY then disable and hide the controls used to
                    // modify and update KEY command
                    lblM2Key.Enabled = false;
                    lblM2Key.Visible = false;
                    nUpDownM2Key.Enabled = false;
                    nUpDownM2Key.Visible = false;
                }

                // show or hide M1 DURATIONx2 command controls
                if (chanCmdType == MChannelCodeEntry.t_Command.DURATIONx2) {
                    lblM2DurationX2Dur.Enabled = true;
                    lblM2DurationX2Dur.Visible = true;
                    nUpDownM2DurationX2Dur.Enabled = true;
                    nUpDownM2DurationX2Dur.Visible = true;
                    lblM2DurationX2Rest.Enabled = true;
                    lblM2DurationX2Rest.Visible = true;
                    nUpDownM2DurationX2Rest.Visible = true;
                    nUpDownM2DurationX2Rest.Enabled = true;

                } else {
                    lblM2DurationX2Dur.Enabled = false;
                    lblM2DurationX2Dur.Visible = false;
                    nUpDownM2DurationX2Dur.Enabled = false;
                    nUpDownM2DurationX2Dur.Visible = false;
                    lblM2DurationX2Rest.Enabled = false;
                    lblM2DurationX2Rest.Visible = false;
                    nUpDownM2DurationX2Rest.Visible = false;
                    nUpDownM2DurationX2Rest.Enabled = false;
                }



            }//if (this.Visible)

        }// SetVisibleM2InstructionEditionControls

        /*******************************************************************************
        * @brief Update, enable or disable the corresponding Chord channel instruction   
        * edition controls according to the instrucion in the received instruction. It 
        * also uses  the data  in the received instruction to retrieve the data used to 
        * initialize the corresponding instruction edition controls.
        * 
        * @param[in] chanCodeEntry chord channel instruction from which the command type 
        * and the values of some specific controls are obtained.
        *******************************************************************************/
        public void SetVisibleChordInstructionEditionControls(ChordChannelCodeEntry chanCodeEntry) {
            ChordChannelCodeEntry.t_Command chanCmdType = ChordChannelCodeEntry.t_Command.REPEAT;

            // if the main Form has been created and is visible
            if (this.Visible) {

                // get the type of command encoded in the instruction
                chanCmdType = chanCodeEntry.GetCmdType();

                // ################# update the Chord instruction edition controls according to the current selected instruction

                // show or hide Chord REST command controls
                if (chanCmdType == ChordChannelCodeEntry.t_Command.REST_DURATION) {
                    // if instruction is REST then enable and show the controls that allow to
                    // modify and update REST command
                    lblChordRestRest.Enabled = true;
                    lblChordRestRest.Visible = true;
                    nUpDownChordRestRest.Enabled = true;
                    nUpDownChordRestRest.Visible = true;

                    // get the values to show into the REST_DURATION edition controls from the received command
                    int iRestParam = 0;
                    ChordChannelCodeEntry.GetRestCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), ref iRestParam);
                    nUpDownChordRestRest.Value = iRestParam;

                } else {
                    // if instruction is not REST then disable and hide the controls used to
                    // modify and update REST command
                    lblChordRestRest.Enabled = false;
                    lblChordRestRest.Visible = false;
                    nUpDownChordRestRest.Enabled = false;
                    nUpDownChordRestRest.Visible = false;
                }

                // show or hide Chord NOTE command controls
                if (chanCmdType == ChordChannelCodeEntry.t_Command.CHORD) {
                    // if instruction is NOTE then enable and show the controls that allow to
                    // modify and update NOTE command
                    lblChordNote.Enabled = true;
                    lblChordNote.Visible = true;
                    cmboBoxChordNote.Enabled = true;
                    cmboBoxChordNote.Visible = true;
                    lblChordNoteType.Enabled = true;
                    lblChordNoteType.Visible = true;
                    cmboBoxChordNoteType.Enabled = true;
                    cmboBoxChordNoteType.Visible = true;
                    lblChordNoteDur.Enabled = true;
                    lblChordNoteDur.Visible = true;
                    nUpDownChordNoteDur.Enabled = true;
                    nUpDownChordNoteDur.Visible = true;

                    // get the values to show into the NOTE edition controls from the received command
                    ChordChannelCodeEntry.t_Notes tNoteParam = ChordChannelCodeEntry.t_Notes.C;
                    ChordChannelCodeEntry.t_ChordType tChordTypeParam = ChordChannelCodeEntry.t_ChordType._7TH;
                    int iRestParam = 0;
                    ChordChannelCodeEntry.GetChordCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), ref tNoteParam, ref tChordTypeParam,ref iRestParam);
                    cmboBoxChordNote.Text = ChordChannelCodeEntry.tNotesToString(tNoteParam);
                    cmboBoxChordNoteType.Text = ChordChannelCodeEntry.tChordTypeToString(tChordTypeParam);
                    nUpDownChordNoteDur.Value = iRestParam;

                } else {
                    // if instruction is not NOTE then disable and hide the controls used to
                    // modify and update NOTE command
                    lblChordNote.Enabled = false;
                    lblChordNote.Visible = false;
                    cmboBoxChordNote.Enabled = false;
                    cmboBoxChordNote.Visible = false;
                    lblChordNoteType.Enabled = false;
                    lblChordNoteType.Visible = false;
                    cmboBoxChordNoteType.Enabled = false;
                    cmboBoxChordNoteType.Visible = false;
                    lblChordNoteDur.Enabled = false;
                    lblChordNoteDur.Visible = false;
                    nUpDownChordNoteDur.Enabled = false;
                    nUpDownChordNoteDur.Visible = false;
                }

                // show or hide Chord REPEAT command controls
                if (chanCmdType == ChordChannelCodeEntry.t_Command.REPEAT) {
                    // if instruction is REPEAT then enable and show the controls that allow to
                    // modify and update REPEAT command
                    lblChordRepeat.Enabled = true;
                    lblChordRepeat.Visible = true;
                    cmboChordRepeat.Enabled = true;
                    cmboChordRepeat.Visible = true;

                    // get the values to show into the REPEAT edition controls from the received command
                    ChordChannelCodeEntry.t_RepeatMark tRepeatParam = ChordChannelCodeEntry.t_RepeatMark.START;
                    ChordChannelCodeEntry.GetRepeatCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), ref tRepeatParam);
                    cmboChordRepeat.Text = ChordChannelCodeEntry.tRepeatMarkToString(tRepeatParam);

                } else {
                    // if instruction is not REPEAT then disable and hide the controls used to
                    // modify and update REPEAT command
                    lblChordRepeat.Enabled = false;
                    lblChordRepeat.Visible = false;
                    cmboChordRepeat.Enabled = false;
                    cmboChordRepeat.Visible = false;
                }

                // show or hide Chord RYTHM command controls
                if (chanCmdType == ChordChannelCodeEntry.t_Command.RYTHM) {
                    // if instruction is RYTHM then enable and show the controls that allow to
                    // modify and update REPEAT command
                    lblChordRythmMode.Enabled = true;
                    lblChordRythmMode.Visible = true;
                    cmboBoxChordRythmMode.Enabled = true;
                    cmboBoxChordRythmMode.Visible = true;
                    lblChordRythmStyle.Enabled = true;
                    lblChordRythmStyle.Visible = true;
                    cmboBoxChorddRythmStyle.Enabled = true;
                    cmboBoxChorddRythmStyle.Visible = true;
                    cmboBoxChorddRythmOnOff.Enabled = true;
                    cmboBoxChorddRythmOnOff.Visible = true;

                    // get the values to show into the RYTHM edition controls from the received command
                    ChordChannelCodeEntry.t_RythmMode tRythmModeParam = ChordChannelCodeEntry.t_RythmMode.SET;
                    ChordChannelCodeEntry.t_RythmStyle tRythmStyleParam = ChordChannelCodeEntry.t_RythmStyle.ROCK;
                    ChordChannelCodeEntry.t_On_Off tOnOffParam = ChordChannelCodeEntry.t_On_Off.ON;
                    ChordChannelCodeEntry.GetRythmCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), ref tRythmModeParam, ref tRythmStyleParam, ref tOnOffParam);
                    cmboBoxChordRythmMode.Text = ChordChannelCodeEntry.tRythmModeToString(tRythmModeParam);
                    cmboBoxChorddRythmStyle.Text = ChordChannelCodeEntry.tRythmStyleToString(tRythmStyleParam);
                    cmboBoxChorddRythmOnOff.Text = ChordChannelCodeEntry.tOnOffToString(tOnOffParam);

                } else {
                    // if instruction is not RYTHM then disable and hide the controls used to
                    // modify and update RYTHM command
                    lblChordRythmMode.Enabled = false;
                    lblChordRythmMode.Visible = false;
                    cmboBoxChordRythmMode.Enabled = false;
                    cmboBoxChordRythmMode.Visible = false;
                    lblChordRythmStyle.Enabled = false;
                    lblChordRythmStyle.Visible = false;
                    cmboBoxChorddRythmStyle.Enabled = false;
                    cmboBoxChorddRythmStyle.Visible = false;
                    cmboBoxChorddRythmOnOff.Enabled = false;
                    cmboBoxChorddRythmOnOff.Visible = false;
                }

                // show or hide Chord TEMPO command controls
                if (chanCmdType == ChordChannelCodeEntry.t_Command.TEMPO) {
                    // if instruction is TEMPO then enable and show the controls that allow to
                    // modify and update TEMPO command
                    lblChordTempo.Enabled = true;
                    lblChordTempo.Visible = true;
                    cmboBoxChordTempoOnOff.Enabled = true;
                    cmboBoxChordTempoOnOff.Visible = true;
                    nUpDownChordTempo.Enabled = true;
                    nUpDownChordTempo.Visible = true;


                    // get the values to show into the TEMPO edition controls from the received command
                    int iTempoParam = 0;
                    ChordChannelCodeEntry.t_On_Off tOnOffParam = ChordChannelCodeEntry.t_On_Off.ON;
                    ChordChannelCodeEntry.GetTempoCommandParamsFromBytes(chanCodeEntry.By0AsByte(), chanCodeEntry.By1AsByte(), ref tOnOffParam, ref iTempoParam);
                    cmboBoxChorddRythmOnOff.Text = ChordChannelCodeEntry.tOnOffToString(tOnOffParam);
                    nUpDownChordTempo.Text = iTempoParam.ToString();
                    cmboBoxChordTempoOnOff.Text = ChordChannelCodeEntry.tOnOffToString(tOnOffParam); ;

                } else {
                    // if instruction is not TEMPO then disable and hide the controls used to
                    // modify and update TEMPO command
                    lblChordTempo.Enabled = false;
                    lblChordTempo.Visible = false;
                    cmboBoxChordTempoOnOff.Enabled = false;
                    cmboBoxChordTempoOnOff.Visible = false;
                    nUpDownChordTempo.Enabled = false;
                    nUpDownChordTempo.Visible = false;
                }

                // show or hide DOUBLE DURATION command controls
                if (chanCmdType == ChordChannelCodeEntry.t_Command.DURATIONx2) {
                    // if instruction is DOUBLE DURATION then enable and show the controls that allow to
                    // modify and update DOUBLE DURATION command
                    lblChordDurationX2Dur.Enabled = true;
                    lblChordDurationX2Dur.Visible = true;
                    nUpDownChordDurationX2Dur.Enabled = true;
                    nUpDownChordDurationX2Dur.Visible = true;

                } else {
                    // if instruction is not DURATIONx2 then disable and hide the controls used to
                    // modify and update DURATIONx2 command
                    lblChordDurationX2Dur.Enabled = false;
                    lblChordDurationX2Dur.Visible = false;
                    nUpDownChordDurationX2Dur.Enabled = false;
                    nUpDownChordDurationX2Dur.Visible = false;
                }


            }//if (this.Visible)

        }// SetVisibleChordInstructionEditionControls

        /*******************************************************************************
        * @brief This procedure updates the application forms and controls and other 
        * configuration parameters of the application based on the settings in the 
        * config.xml configuration parameters @param[in] b_update_enabled_disabled_state:
        * true if the operation requires updating the enabled/disabled state of controls 
        * or false if not.
        * @return 
        *     - ErrCode with the error code or cErrCodes.
        *     - ERR_NO_ERROR if no error occurs.
        *******************************************************************************/
        public ErrCode UpdateAppWithConfigParameters(bool b_update_enabled_disabled_state) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";


            if (AreCorrdinatesInScreenBounds(configMgr.m_i_screen_orig_x, configMgr.m_i_screen_orig_y)) {

                // get the form dimensions and coordinates
                this.Height = configMgr.m_i_screen_size_y;
                this.Width = configMgr.m_i_screen_size_x;
                this.Top = configMgr.m_i_screen_orig_y;
                this.Left = configMgr.m_i_screen_orig_x;

            } else {

                // get the form dimensions and coordinates
                this.Height = cConfig.DEFAULT_FORM_HEIGHT;
                this.Width = cConfig.DEFAULT_FORM_WIDTH;
                this.Top = 25;
                this.Left = 25;

            }// if ( AreCorrdinatesInScreenBounds(i_screen_orig_x,i_screen_orig_y) 

            if (configMgr.m_b_screen_maximized) {
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            } else {
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            }

            // cambiamos el puntero del raton a reloj si la aplicacion esta ocupada procesando
            // y lo dejamos con el icono estandar si no esta procesando
            if (statusNLogs.IsAppBusy()) {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;
            } else {
                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }//if

            // aupdates main form title

            // set fmain orm title            
            str_aux = cConfig.SW_TITLE + " - v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " - " + cConfig.SW_DESCRIPTION;
            //if (dpack_drivePack.str_title  != "") str_aux = str_aux + " - " + dpack_drivePack.str_title;
            if (configMgr.m_str_cur_rom_file != "") str_aux = str_aux + " - " + AuxFuncs.ReducePathAndFile(configMgr.m_str_cur_rom_file, cConfig.SW_MAX_TITLE_LENGTH);
            this.Text = str_aux;

            // actualiza el estado Enabled/Disabled de los controles
            if (b_update_enabled_disabled_state) {

                /* JBR 2024-04-23 Revisar
                    if (!b_is_project_loaded) {

                        // si no hay proyecto cargado los deshabilitamos
                        EnableDisableControls(false);

                    } else {

                        // actualiza el estado Enabled/Disabled de los controles
                        if (StatusLogs.IsAppBusy()) {
                            // si hay proyecto cargado y la aplicacion esta ocupada se deshabilitan
                            EnableDisableControls(false);
                        } else {
                            // si hay proyecto cargado y la aplicacion NO esta ocupada se habilitan
                            EnableDisableControls(true);
                        }//if (StatusLogs.IsAppBusy()) {

                    }// if (!b_is_project_loaded)
                FIN JBR 2024-04-23 Revisar */

            }// if (b_update_enabled_disabled_state) 

            // updates the corresponding text box with the last read valid title
            romTitleTextBox.Text = dpack_drivePack.themes.strROMTitle;

            // updates the corresponding text box with the last read valid theme information
            romInfoTextBox.Text = dpack_drivePack.themes.strROMInfo;

            return ec_ret_val;

        }//UpdateAppWithConfigParameters

        /*******************************************************************************
         * @brief This procedure updates the configuration file with values 
         * that correspond to the state of the form and other internal parameters.
         * @return
         *     >=0: Indicates a successful operation.
         *     <0: Indicates an error code.
         *******************************************************************************/
        public int UpdateConfigParametersWithAppState() {
            int i_ret_val = 0;
            List<int> liISelectionIdx = new List<int>();
            int i_aux = 0;
            int i_aux2 = 0;

            // store the form dimensions and coordinates
            configMgr.m_i_screen_size_y = this.Height;
            configMgr.m_i_screen_size_x = this.Width;
            configMgr.m_i_screen_orig_y = this.Top;
            configMgr.m_i_screen_orig_x = this.Left;

            configMgr.m_b_screen_maximized = (this.WindowState == System.Windows.Forms.FormWindowState.Maximized);

            // store the object properties with controls content
            if (dpack_drivePack.themes.strROMTitle != romTitleTextBox.Text) {
                dpack_drivePack.themes.strROMTitle = romTitleTextBox.Text;
            }
            if (dpack_drivePack.themes.strROMInfo != romInfoTextBox.Text) {
                dpack_drivePack.themes.strROMInfo = romInfoTextBox.Text;
            }

            // update the internal indexes used to keep the rows currently selected on each M1,M2 and chords
            // channel edition dataGridView. If there is more than one element selected in the dataGridView
            // we only keep the index of the first element in the selection
            i_aux = dpack_drivePack.themes.iCurrThemeIdx;
            if (i_aux != -1) {

                // store the index of the first Melody 1 channel selected instructions in the dataGridView 
                liISelectionIdx.Clear();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();
                if (liISelectionIdx.Count > 0) {
                    dpack_drivePack.themes.liThemesCode[i_aux].iM1IEditednstrIdx = liISelectionIdx[0];
                }

                // store the index of the first Melody 2 channel selected instructions in the dataGridView 
                liISelectionIdx.Clear();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();
                if (liISelectionIdx.Count > 0) {
                    dpack_drivePack.themes.liThemesCode[i_aux].iM2EditedInstrIdx = liISelectionIdx[0];
                }

                // store the index of the first Chords channel selected instructions in the dataGridView 
                liISelectionIdx.Clear();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();
                if (liISelectionIdx.Count > 0) {
                    dpack_drivePack.themes.liThemesCode[i_aux].iChEditedInstrIdx = liISelectionIdx[0];
                }

            }// if (i_aux != -1)

            return i_ret_val;

        }//UpdateConfigParametersWithAppState

        /*******************************************************************************
        * @brief Event triggered when the content of the Be Hex editor has been modified
        * @param[in] sender
        * @param[in] e
        *******************************************************************************/
        private void BeHexEditorChanged(object sender, EventArgs e) {

            dpack_drivePack.dataChanged = true;

        }//BeHexEditorChanged

        /*******************************************************************************
        * @brief Method that checks if the received string has a valid path format or if
        * does not have a valid path string format.
        * @param[in] path
        * @param[in] allowRelativePaths
        * @return
        *    - true: if received string has a valid path format.
        *    - false: if received string does not have a valid path format.
        * @note based on from:
        *   https://stackoverflow.com/questions/6198392/check-whether-a-path-is-valid
        *******************************************************************************/
        public bool IsValidPath(string path, bool allowRelativePaths = false) {
            bool isValid = true;

            try {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths) {
                    isValid = Path.IsPathRooted(path);
                } else {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }//if

            } catch (Exception ex) {
                isValid = false;
            }

            return isValid;

        }//IsValidPath

        /*******************************************************************************
        * @brief Executes all the steps that must be excuted when cosing the appliation: 
        *     - Set the closing event in the logs
        *     - Save last active configuration
        * @return   
        *      true if the user confirms that application must be closed
        *      false if the user cancelled the close application operation
        *******************************************************************************/
        private bool CloseApplication() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            bool b_close_application = false;


            // antes de cerrar la aplicacion se llama a la funcion que muestra el aviso al usuario 
            // preguntando si desa o no continuar dependiendo de si hay proyecto activo o no
            b_close_application = ConfirmCloseProject("Current project modifications will be lost. Exit anyway?");

            if (b_close_application) {

                // guarda en el fichero de configuracion los parametros con el estado de la aplicacion
                ec_ret_val = configMgr.SaveConfigParameters();
                if (ec_ret_val.i_code < 0) {
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, ec_ret_val, ec_ret_val.str_description, false);
                }

                // se informa de cierre del log
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, "Log file closed", false);
            }//b_close_application

            return b_close_application;

        }//CloseApplication

        /*******************************************************************************
        * @brief This procedure takes the information of the current selected theme in the
        * Code edition tabPage controls and then stores it back into the corresponding memory 
        * sturctures. 
        * @return
        *   - ErrCode >= 0 if the operation could be executed.
        *   - ErrCode < 0 if it was not possible to execute the operation.
        *******************************************************************************/
        // JBR 2024-05-02 Revisar si se puede eliminar este metodo
        // public ErrCode UpdateCodeChannelsWithDataGridView() {
        //     ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
        //     int iThemeIdx = 0;
        // 
        // 
        //     // check if there is any theme selected
        //     iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;
        //     if ( (iThemeIdx >= 0) && (iThemeIdx< dpack_drivePack.themes.info.liTitles.Count()) ){
        //         
        //         dpack_drivePack.themes.info.liTitles[iThemeIdx].Title = sequenceTitleTextBox.Text.Trim();
        // 
        //         // as the the content of the .liM1CodeInstr, .liM2CodeInstr and .liChordCodeInstr  is binded to the
        //         // DataGridViews that lists are automatically updated when the user modifies the content in the
        //         // DataGridViews and there is no need to manually move the content of the dataGridViews to the lists.
        // 
        //     }//if
        // 
        //     return ec_ret_val;
        // 
        // }//UpdateCodeChannelsWithDataGridView

        /*******************************************************************************
        * @brief This procedure binds the list of code/instructions of the Melody 1
        * channel of the currently selected theme to the corresponding themeM1DataGridView
        * @return
        *   - ErrCode >= 0 if the operation could be executed.
        *   - ErrCode < 0 if it was not possible to execute the operation.
        *******************************************************************************/
        public ErrCode UpdateControlsCodeM1() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            DataGridViewTextBoxColumn textBoxColumnAux = null;
            int iThemeIdx = 0;

            // if there is any theme selected then fill the M1, M2 and Chrod dataGridViews with the corresponding
            // selected theme M1, M2 or Chord channels content.
            iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;
            if (iThemeIdx < 0) {

                themeM1DataGridView.DataSource = null;
                themeM1DataGridView.Rows.Clear();

            } else {

                // Melody 1 (main melody) DataGridView: bind the channel 1 instructions of the current selected theme to the M1 DataGridView ##################

                lblMel1Ch.Text = "Melody 1 ch.code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count.ToString("D3") + "):";

                // init melody1 DataGridView: clear the M1 dataGridView before filling it with the content of the list of M1 entries 
                themeM1DataGridView.DefaultCellStyle.Font = new Font(CODE_FONT, CODE_SIZE);
                themeM1DataGridView.DataSource = null;
                themeM1DataGridView.Columns.Clear();
                themeM1DataGridView.Rows.Clear();

                // define the columns in the 
                // column 0
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_M1_IDX_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_M1_IDX;
                textBoxColumnAux.DataPropertyName = "idx";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = true;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeM1DataGridView.Columns.Add(textBoxColumnAux);
                // column 1
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_M1_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_M1;
                textBoxColumnAux.DataPropertyName = "by0";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = false;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeM1DataGridView.Columns.Add(textBoxColumnAux);
                // column 2
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_M1ON_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_M1ON;
                textBoxColumnAux.DataPropertyName = "by1";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = false;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeM1DataGridView.Columns.Add(textBoxColumnAux);
                // column 3
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_M1OFF_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_M1OFF;
                textBoxColumnAux.DataPropertyName = "by2";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = false;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeM1DataGridView.Columns.Add(textBoxColumnAux);
                // column 4
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_M1DESCR_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_M1DESCR;
                textBoxColumnAux.DataPropertyName = "strDescr";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = false;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeM1DataGridView.Columns.Add(textBoxColumnAux);

                // set themeM1DataGridView general style parameters
                themeM1DataGridView.RowHeadersVisible = false;
                themeM1DataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // vertical row autosize
                themeM1DataGridView.AllowUserToAddRows = false;// to avoid the empty row at the end of the DataGridView
                // set themeM1DataGridView Idx column style
                themeM1DataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeM1DataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                themeM1DataGridView.Columns[0].DefaultCellStyle.BackColor = SystemColors.Control;
                themeM1DataGridView.Columns[0].DefaultCellStyle.ForeColor = Color.Gray;
                themeM1DataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // set themeM1DataGridView B0 column style
                themeM1DataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeM1DataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                themeM1DataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // set themeM1DataGridView B1 column style
                themeM1DataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeM1DataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                themeM1DataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // set themeM1DataGridView B2 column style
                themeM1DataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeM1DataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                themeM1DataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // set themeM1DataGridViewdescription column style
                themeM1DataGridView.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeM1DataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                themeM1DataGridView.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // bind the list of M1 entries to the datagridview1
                themeM1DataGridView.DataSource = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr;
            
            }//if (iThemeIdx != -1)

            return ec_ret_val;

        }//UpdateControlsCodeM1

        /*******************************************************************************
        * @brief This procedure binds the list of code/instructions of the Melody 2
        * channel of the currently selected theme to the corresponding themeM2DataGridView
        * @return
        *   - ErrCode >= 0 if the operation could be executed.
        *   - ErrCode < 0 if it was not possible to execute the operation.
        *******************************************************************************/
        public ErrCode UpdateControlsCodeM2() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            DataGridViewTextBoxColumn textBoxColumnAux = null;
            int iThemeIdx = 0;

            // if there is any theme selected then fill the M1, M2 and Chrod dataGridViews with the corresponding
            // selected theme M1, M2 or Chord channels content.
            iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;
            if (iThemeIdx < 0) {

                themeM2DataGridView.DataSource = null;
                themeM2DataGridView.Rows.Clear();

            } else {

                // Melody 2 (obligatto) DataGridView: bind the channel 2 instructions of the current selected theme to the M2 DataGridView ##################

                lblMel2Ch.Text = "Melody 2 ch.code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count.ToString("D3") + "):";

                // init melody channel 2 DataGridView: clear the M2 dataGridView before filling it with the content of the list of M2 entries 
                themeM2DataGridView.DefaultCellStyle.Font = new Font(CODE_FONT, CODE_SIZE);
                themeM2DataGridView.DataSource = null;
                themeM2DataGridView.Columns.Clear();
                themeM2DataGridView.Rows.Clear();
                // column 0
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_M2_IDX_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_M2_IDX;
                textBoxColumnAux.DataPropertyName = "idx";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = true;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeM2DataGridView.Columns.Add(textBoxColumnAux);
                // column 1
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_M2_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_M2;
                textBoxColumnAux.DataPropertyName = "by0";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = false;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeM2DataGridView.Columns.Add(textBoxColumnAux);
                // column 2
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_M2ON_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_M2ON;
                textBoxColumnAux.DataPropertyName = "by1";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = false;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeM2DataGridView.Columns.Add(textBoxColumnAux);
                // column 3
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_M2OFF_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_M2OFF;
                textBoxColumnAux.DataPropertyName = "by2";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = false;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeM2DataGridView.Columns.Add(textBoxColumnAux);
                // column 4
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_M2DESCR_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_M2DESCR;
                textBoxColumnAux.DataPropertyName = "strDescr";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = false;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeM2DataGridView.Columns.Add(textBoxColumnAux);

                // set themeM2DataGridView general style parameters
                themeM2DataGridView.RowHeadersVisible = false;
                themeM2DataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // vertical row autosize
                themeM2DataGridView.AllowUserToAddRows = false;// to avoid the empty row at the end of the DataGridView
                                                               // set themeM2DataGridView Idx column style
                themeM2DataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeM2DataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                themeM2DataGridView.Columns[0].DefaultCellStyle.BackColor = SystemColors.Control;
                themeM2DataGridView.Columns[0].DefaultCellStyle.ForeColor = Color.Gray;
                themeM2DataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // set themeM2DataGridView B0 column style
                themeM2DataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeM2DataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                themeM2DataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // set themeM2DataGridView B1 column style
                themeM2DataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeM2DataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                themeM2DataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // set themeM2DataGridView B2 column style
                themeM2DataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeM2DataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                themeM2DataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // set themeM2DataGridView description column style
                themeM2DataGridView.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeM2DataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                themeM2DataGridView.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                themeM2DataGridView.DataSource = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr;

            }//if (iThemeIdx != -1)

            return ec_ret_val;

        }//UpdateControlsCodeM2

        /*******************************************************************************
        * @brief This procedure binds the list of code/instructions of the Chords
        * channel of the currently selected theme to the corresponding themeChordDataGridView
        * @return
        *   - ErrCode >= 0 if the operation could be executed.
        *   - ErrCode < 0 if it was not possible to execute the operation.
        *******************************************************************************/
        public ErrCode UpdateControlsCodeChords() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            DataGridViewTextBoxColumn textBoxColumnAux = null;
            int iThemeIdx = 0;

            // if there is any theme selected then fill the M1, M2 and Chrod dataGridViews with the corresponding
            // selected theme M1, M2 or Chord channels content.
            iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;
            if (iThemeIdx < 0) {
            
                themeChordDataGridView.DataSource = null;
                themeChordDataGridView.Rows.Clear();
            
            } else { 

                // Chords channel DataGridView: bind the chords channel of the current selected theme to the chord DataGridView ##################
                
                lblChordCh.Text = "Chords ch. code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count.ToString("D3") + "):";

                // init chords DataGridView: clear the chords dataGridView before filling it with the content of the list of chord entries 
                themeChordDataGridView.DefaultCellStyle.Font = new Font(CODE_FONT, CODE_SIZE);
                themeChordDataGridView.DataSource = null;
                themeChordDataGridView.Columns.Clear();
                themeChordDataGridView.Rows.Clear();
                // column 0
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_CH_IDX_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_CH_IDX;
                textBoxColumnAux.DataPropertyName = "idx";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = true;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeChordDataGridView.Columns.Add(textBoxColumnAux);
                // column 1
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_CH_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_CH;
                textBoxColumnAux.DataPropertyName = "by0";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = false;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeChordDataGridView.Columns.Add(textBoxColumnAux);
                // column 2
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_CHON_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_CHON;
                textBoxColumnAux.DataPropertyName = "by1";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = false;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeChordDataGridView.Columns.Add(textBoxColumnAux);
                // column 3
                textBoxColumnAux = new DataGridViewTextBoxColumn();
                textBoxColumnAux.HeaderText = IDX_COLUMN_CHDESCR_TIT;
                textBoxColumnAux.Name = IDX_COLUMN_CHDESCR;
                textBoxColumnAux.DataPropertyName = "strDescr";
                textBoxColumnAux.ValueType = typeof(string);
                textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
                textBoxColumnAux.ReadOnly = false;
                textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                themeChordDataGridView.Columns.Add(textBoxColumnAux);

                // set themeChordDataGridView general style parameters
                themeChordDataGridView.RowHeadersVisible = false;
                themeChordDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // vertical row autosize
                themeChordDataGridView.AllowUserToAddRows = false;// to avoid the empty row at the end of the DataGridView
                                                                  // set themeChordDataGridView Idx column style
                themeChordDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeChordDataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                themeChordDataGridView.Columns[0].DefaultCellStyle.BackColor = SystemColors.Control;
                themeChordDataGridView.Columns[0].DefaultCellStyle.ForeColor = Color.Gray;
                themeChordDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // set themeChordDataGridView B0 column style
                themeChordDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeChordDataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                themeChordDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // set themeChordDataGridView B1 column style
                themeChordDataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeChordDataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                themeChordDataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // set themeChordDataGridView description column style
                themeChordDataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                themeChordDataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                themeChordDataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                themeChordDataGridView.DataSource = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr;
            
            }//if (iThemeIdx != -1) {

            return ec_ret_val;

        }//UpdateControlsCodeChords

        /*******************************************************************************
        * @brief This procedure updates all the controls shown in the theme Code Tab page
        * according to the state of the internal variables. It also binds the different
        * M1, M2 and Chord instrucions dataGridView to the corresponding list of instructions.
        * @return
        *   - ErrCode >= 0 if the operation could be executed.
        *   - ErrCode < 0 if it was not possible to execute the operation.
        *******************************************************************************/
        public ErrCode UpdateCodeTabPageControls() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            int iThemeIdx = 0;
            string strAux = "";
            int i_aux = 0;

            // if there is any theme selected then bind the lists with the M1, M2 and Chords code to
            // the M1, M2 and Chrod dataGridViews
            iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

            // update the items in the themes combo box with the list of available themes
            themeSelectComboBox.Items.Clear();
            for (i_aux = 0; i_aux < dpack_drivePack.themes.liThemesCode.Count; i_aux++) {
                strAux = dpack_drivePack.themes.liThemesCode[i_aux].Title;
                themeSelectComboBox.Items.Add(strAux);
            }

            // initialize the label that indicates the current theme and the total of themes in memory
            if (dpack_drivePack.themes.liThemesCode.Count == 0) {
                lblIdx.Text = "Idx: --";
                lblThemesList.Text = "Themes (total 0):";
                
            } else {
                lblIdx.Text = "Idx: " + iThemeIdx.ToString();
                lblThemesList.Text = "Themes (total "+ dpack_drivePack.themes.liThemesCode.Count + "):";
            }

            // update the selected theme ComboBox
            if ((iThemeIdx < 0) || (dpack_drivePack.themes.liThemesCode.Count == 0)) {

                // there is no theme selected in the list of avialable themes
                themeSelectComboBox.SelectedIndex = -1;
                themeSelectComboBox.Text = "";                

            } else {

                // if there is any theme selected in the list of available themes then highlight it in the combo box
                themeSelectComboBox.SelectedIndex = iThemeIdx;
                themeSelectComboBox.Text =  dpack_drivePack.themes.liThemesCode[iThemeIdx].Title;               

            }//if

            // Melody 1 (main melody) DataGridView: bind the channel 1 instructions of the current selected theme to the M1 DataGridView
            UpdateControlsCodeM1();

            // Melody 2 (obligatto) DataGridView: bind the channel 2 instructions of the current selected theme to the M2 DataGridView
            UpdateControlsCodeM2();

            // Chords channel DataGridView: bind the chords channel of the current selected theme to the chord DataGridView
            UpdateControlsCodeChords();

            return ec_ret_val;

        }//UpdateCodeTabPageControls

        /*******************************************************************************
        * @brief This procedure updates all the controls shown in the theme Code Tab page
        * according to the state of the internal variables. It also binds the different
        * M1, M2 and Chord instrucions dataGridView to the corresponding list of instructions.
        * @return
        *   - ErrCode >= 0 if the operation could be executed.
        *   - ErrCode < 0 if it was not possible to execute the operation.
        *******************************************************************************/
        public ErrCode UpdateThemesTabPageControls() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            DataGridViewTextBoxColumn textBoxColumnAux = null;

            // update the themes list dataGridView with the current list of themes

            // init melody1 DataGridView: clear the M1 dataGridView before filling it with the content of the list of M1 entries 
            themeTitlesDataGridView.DefaultCellStyle.Font = new Font(TITLES_FONT, TITLES_SIZE);
            themeTitlesDataGridView.DataSource = null;
            themeTitlesDataGridView.Columns.Clear();
            themeTitlesDataGridView.Rows.Clear();
            themeTitlesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            romTitleTextBox.Text = dpack_drivePack.themes.strROMTitle;
            romInfoTextBox.Text = dpack_drivePack.themes.strROMInfo;

            // define the columns in the 
            // column 0
            textBoxColumnAux = new DataGridViewTextBoxColumn();
            textBoxColumnAux.HeaderText = IDX_COLUMN_THEME_IDX_TIT;
            textBoxColumnAux.Name = IDX_COLUMN_THEME_IDX;
            textBoxColumnAux.DataPropertyName = "Idx";
            textBoxColumnAux.ValueType = typeof(int);
            textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
            textBoxColumnAux.ReadOnly = true;
            textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            themeTitlesDataGridView.Columns.Add(textBoxColumnAux);
            // column 1
            textBoxColumnAux = new DataGridViewTextBoxColumn();
            textBoxColumnAux.HeaderText = IDX_COLUMN_THEME_NAME_TIT;
            textBoxColumnAux.Name = IDX_COLUMN_THEME_NAME;
            textBoxColumnAux.DataPropertyName = "Title";
            textBoxColumnAux.ValueType = typeof(string);
            textBoxColumnAux.SortMode = DataGridViewColumnSortMode.NotSortable;
            textBoxColumnAux.ReadOnly = false;
            textBoxColumnAux.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            themeTitlesDataGridView.Columns.Add(textBoxColumnAux);

            // set themeTitlesDataGridView general style parameters
            themeTitlesDataGridView.RowHeadersVisible = false;
            themeTitlesDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // vertical row autosize
            themeTitlesDataGridView.AllowUserToAddRows = false;// to avoid the empty row at the end of the DataGridView

            // set themeTitlesDataGridView Idx column style
            themeTitlesDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            themeTitlesDataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            themeTitlesDataGridView.Columns[0].DefaultCellStyle.BackColor = SystemColors.Control;
            themeTitlesDataGridView.Columns[0].DefaultCellStyle.ForeColor = Color.Gray;
            themeTitlesDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // set themeTitlesDataGridView Title column style
            themeTitlesDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            themeTitlesDataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            themeTitlesDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // bind the list of themes entries to the datagridview1
            themeTitlesDataGridView.DataSource = dpack_drivePack.themes.liThemesCode;
            
            themeTitlesDataGridView.ClearSelection();

            return ec_ret_val;

        }//UpdateThemesTabPageControls

        /*******************************************************************************
         * @brief Executes the corresponding actions over the received file
         * @param[in] iIdx of the next added theme in the summary file
         * @param[in] str_file_in_name name of the them file to process
         * @param[in] str_file_out_name name of the file with the output theme resulting
         * of processing the input file
         * @param[in] str_li_themes_file_name the name of the file used to store the
         * report with all the themes and ROM names of all processed themes.
         * @return <0 is some error occurres while processing the received file, >=0
         * if the received file could be properly processed.
         *******************************************************************************/
        private int processFile(ref int iIdx, string str_file_in_name, string str_file_out_name,string str_summary_file_name) {
            int i_ret_val = 0;
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";
            string str_line = "";
            string str_theme_title = "";
            string str_rom_gen_info = "";
            int i_themes_ctr = 0;
            StreamWriter sWriterTextFile = null;
            int i_aux = 0;
            int i_aux2 = 0;
            int i_aux3 = 0;
            int i_aux4 = 0;

            // show the message to the user with the result of processing the file
            str_aux = ec_ret_val.str_description + "Processed:" + str_file_in_name;
            statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, str_aux, false);

            // if file ends with ".drp" then call the function that opens the file in DRP format 
            ec_ret_val = dpack_drivePack.loadDRPFile(str_file_in_name);

            // start with an empty themes list structure, and will use the content in the read DRP file to populate the themes list structure
            dpack_drivePack.themes.liThemesCode.Clear();

            // search for all the "[x] theme title" entries in the DRP1 ROMInfo field and store them into the corresponding theme title
            str_aux = dpack_drivePack.themes.strROMInfo;
            i_themes_ctr = 0;
            i_aux = 0;
            while ( (i_aux<str_aux.Length) && (i_ret_val>=0) ) {

                // take a line from the DRP1 ROMInfo field
                i_aux2 = str_aux.IndexOf("\r", i_aux); // get the index of then end of the current line
                if (i_aux2 == -1) { i_aux2 = str_aux.Length; }// if there is no '\r' then it means that is the end of the text block
                str_line = str_aux.Substring(i_aux, i_aux2 - i_aux);
                str_line = str_line.Replace("\r", "");
                str_line = str_line.Replace("\n", "");
                str_line = str_line.Trim();
                i_aux = i_aux2+1; // set the i_aux cursor at the end of the current processed line

                // check if the line contains "[x]" and if affirmative it means that this line corresponds to a theme title
                i_aux3 = str_line.IndexOf("[");
                i_aux4 = str_line.IndexOf("]");
                if ( (i_aux3!=-1) && (i_aux4 != -1) && (i_aux3<i_aux4)) {

                    // the processed line corresponds to a Song title because it includes the "[x]" so store the theme title in the list of titles
                    str_theme_title = str_line.Substring(i_aux4 + 1, str_line.Length - (i_aux4 + 1));

                    // add a new theme in the list of themes and store the information of the information read from the DRP1 file
                    dpack_drivePack.themes.AddNew();
                    dpack_drivePack.themes.liThemesCode[i_themes_ctr].Idx = i_themes_ctr;
                    dpack_drivePack.themes.liThemesCode[i_themes_ctr].Title = str_theme_title;

                    i_themes_ctr++;

                } else {

                    // the processed line does not correspond to a theme title because it does not include the "[x]" so store it in the ROM general information field
                    str_rom_gen_info = str_rom_gen_info + str_line;

                }//if
                
            }//while

            this.dpack_drivePack.themes.strROMInfo = str_rom_gen_info;

            ec_ret_val = dpack_drivePack.decodeROMPACKtoSongThemes();

            // once processed saved it to disk
            ec_ret_val = dpack_drivePack.saveDRPFile(str_file_out_name);

            // generate the entries of that ROM themes in the report summary file
            if (str_summary_file_name!="") {

                if ( (sWriterTextFile = File.AppendText(str_summary_file_name)) != null) { 
                    i_aux = 0;
                    for (i_aux=0;i_aux< dpack_drivePack.themes.liThemesCode.Count(); i_aux++) {
                        str_aux = iIdx.ToString() + ";" + dpack_drivePack.themes.strROMTitle + ";" + i_aux.ToString() + ";" + dpack_drivePack.themes.liThemesCode[i_aux].Title + ";" + dpack_drivePack.themes.strROMInfo;
                        sWriterTextFile.WriteLine(str_aux);
                        iIdx++;
                    }//for
                }
                sWriterTextFile.Close();

            }//if

            return i_ret_val;

        }//processFile

        /*******************************************************************************
        * @brief Method that recursively process the files and folders inside the received
        * folder.
        * @param[in] iIdx of the next added theme in the summary file
        * @param[in] str_path_in the path that contains the files that must be processed 
        * @param[in] str_paht_out the parht where the result of porecessing the input files
        * will be stored.
        * @param[in] str_li_themes_file_name the name of the file used to store the
        * report with all the themes and ROM names of all processed themes.
        * @return <0 is some error occurres while processing the received folder, >=0
        * if the received folder could be properly processed.
        *******************************************************************************/
        private int processPath(ref int iIdx, string str_path_in, string str_paht_out, string str_path_summary) {
            int i_ret_val = 0;
            int i_count = 0;
            // string[] dirs_list = Directory.GetDirectories(str_path, "*.drp", SearchOption.TopDirectoryOnly);
            string[] files_list = Directory.GetFiles(str_path_in, "*.drp", SearchOption.AllDirectories);
            string str_file_name = "";
            int i_aux = 0;

            // first process all the files in the current folder
            i_count = files_list.Count();
            i_aux = 0;
            while ((i_ret_val >= 0) && (i_aux < i_count)) {

                str_file_name = Path.GetFileName(files_list[i_aux]);
                i_ret_val = processFile(ref iIdx, files_list[i_aux], str_paht_out + "\\" + str_file_name, str_paht_out + "\\" + str_path_summary);
                i_aux++;

            }//while

            return i_ret_val;

        }//processPath

        /*******************************************************************************
        * @brief This function takes the values in the instruction configuration controls
        * and passes them to the corresponding fucntion to convert that values into the
        * bytes that encode the selected instruction.
        * 
        * @param[out] _by0
        * @param[out] _by1
        * @param[out] _by2
        * 
        * @return 
        *     - ErrCode with the error code or cErrCodes.
        *     - ERR_NO_ERROR if no error occurs.
        *******************************************************************************/
        public ErrCode GetM1ConfiguredCommand(ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";

            _by0 = 0;
            _by1 = 0;
            _by2 = 0;

            str_aux = cmboBoxM1Instr.Text;

            if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.TIMBRE_INSTRUMENT)) {

                // decode TIMBRE_INSTRUMENT command
                ec_ret_val = MChannelCodeEntry.GetInstrumentCommandBytesFromParams(MChannelCodeEntry.strToInstrument(cmboBoxM1Timbre.Text), MChannelCodeEntry.strToTOnOff(cmboBoxM1TimbreOnOff.Text), Convert.ToInt32(nUpDownM1TimbreRest.Value), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.EFFECT)) {

                // decode EFFECT command
                ec_ret_val = MChannelCodeEntry.GetEffectCommandBytesFromParams(MChannelCodeEntry.strToTEffect(cmbBoxM1Effect.Text), MChannelCodeEntry.strToTOnOff(cmbBoxM1EffectOnOff.Text),Convert.ToInt32(nUpDownM1EffRest.Value), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.REST_DURATION)) {

                // decode REST_DURATION command
                ec_ret_val = MChannelCodeEntry.GetRestCommandBytesFromParams(Convert.ToInt32(nUpDownM1RestRest.Value), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.NOTE)) {

                // decode NOTE command
                ec_ret_val = MChannelCodeEntry.GetNoteCommandBytesFromParams(MChannelCodeEntry.strToTNote(cmboBoxM1Note.Text), Convert.ToInt16(nUpDownM1NoteDur.Value),Convert.ToInt32(nUpDownM1NoteRest.Value), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.REPEAT)) {

                // decode REPEAT command
                ec_ret_val = MChannelCodeEntry.GetRepeatCommandBytesFromParams(MChannelCodeEntry.strToTRepeatMark(cmboBoxM1Repeat.Text), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.TIE)) {

                // decode TIE command
                ec_ret_val = MChannelCodeEntry.GetTieCommandBytesFromParams(MChannelCodeEntry.strToTOnOff(cmboBoxM1Tie.Text), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.KEY)) {

                // decode KEY command
                ec_ret_val = MChannelCodeEntry.GetKeyCommandBytesFromParams(Convert.ToInt32(nUpDownM1Key.Value), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.TIME)) {

                // decode TIME command
                ec_ret_val = MChannelCodeEntry.GetTimeCommandBytesFromParams(MChannelCodeEntry.strToTimetMark(cmboBoxM1Time.Text), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.BAR)) {

                // decode BAR command
                ec_ret_val = MChannelCodeEntry.GetBarCommandBytesFromParams(ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.END)) {

                // decode END command
                ec_ret_val = MChannelCodeEntry.GetEndCommandBytesFromParams( ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.DURATIONx2)) {

                // decode DOUBLE DURATION command
                ec_ret_val = MChannelCodeEntry.Get2xDurationCommandBytesFromParams(Convert.ToInt32(nUpDownM1DurationX2Dur.Value), Convert.ToInt32(nUpDownM1DurationX2Rest.Value), ref _by0, ref _by1, ref _by2);

            }//if

            return ec_ret_val;

        }//GetM1ConfiguredCommand

        /*******************************************************************************
        * @brief This function takes the values in the instruction configuration controls
        * and passes them to the corresponding fucntion to convert that values into the
        * bytes that encode the selected instruction.
        * 
        * @param[out] _by0
        * @param[out] _by1
        * @param[out] _by2
        * 
        * @return 
        *     - ErrCode with the error code or cErrCodes.
        *     - ERR_NO_ERROR if no error occurs.
        *******************************************************************************/
        public ErrCode GetM2ConfiguredCommand(ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";

            _by0 = 0;
            _by1 = 0;
            _by2 = 0;

            str_aux = cmboBoxM2Instr.Text;

            if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.TIMBRE_INSTRUMENT)) {

                // decode TIMBRE_INSTRUMENT command
                ec_ret_val = MChannelCodeEntry.GetInstrumentCommandBytesFromParams(MChannelCodeEntry.strToInstrument(cmboBoxM2Timbre.Text), MChannelCodeEntry.strToTOnOff(cmboBoxM2TimbreOnOff.Text), Convert.ToInt32(nUpDownM2TimbreRest.Value), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.EFFECT)) {

                // decode EFFECT command
                ec_ret_val = MChannelCodeEntry.GetEffectCommandBytesFromParams(MChannelCodeEntry.strToTEffect(cmbBoxM2Effect.Text), MChannelCodeEntry.strToTOnOff(cmbBoxM2EffectOnOff.Text), Convert.ToInt32(nUpDownM2EffRest.Value), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.REST_DURATION)) {

                // decode REST_DURATION command
                ec_ret_val = MChannelCodeEntry.GetRestCommandBytesFromParams(Convert.ToInt32(nUpDownM2RestRest.Value), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.NOTE)) {

                // decode NOTE command
                ec_ret_val = MChannelCodeEntry.GetNoteCommandBytesFromParams(MChannelCodeEntry.strToTNote(cmboBoxM2Note.Text), Convert.ToInt16(nUpDownM2NoteDur.Value), Convert.ToInt32(nUpDownM2NoteRest.Value), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.REPEAT)) {

                // decode REPEAT command
                ec_ret_val = MChannelCodeEntry.GetRepeatCommandBytesFromParams(MChannelCodeEntry.strToTRepeatMark(cmboBoxM2Repeat.Text), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.TIE)) {

                // decode TIE command
                ec_ret_val = MChannelCodeEntry.GetTieCommandBytesFromParams(MChannelCodeEntry.strToTOnOff(cmboBoxM2Tie.Text), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.KEY)) {

                // decode KEY command
                ec_ret_val = MChannelCodeEntry.GetKeyCommandBytesFromParams(Convert.ToInt32(nUpDownM2Key.Value), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.TIME)) {

                // decode TIME command
                ec_ret_val = MChannelCodeEntry.GetTimeCommandBytesFromParams(MChannelCodeEntry.strToTimetMark(cmboBoxM2Time.Text), ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.BAR)) {

                // decode BAR command
                ec_ret_val = MChannelCodeEntry.GetBarCommandBytesFromParams( ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.END)) {

                // decode END command
                ec_ret_val = MChannelCodeEntry.GetEndCommandBytesFromParams(ref _by0, ref _by1, ref _by2);

            } else if (str_aux == MChannelCodeEntry.tCommandToString(MChannelCodeEntry.t_Command.DURATIONx2)) {

                // decode DOUBLE DURATION command
                ec_ret_val = MChannelCodeEntry.Get2xDurationCommandBytesFromParams(Convert.ToInt32(nUpDownM2DurationX2Dur.Value), Convert.ToInt32(nUpDownM2DurationX2Rest.Value), ref _by0, ref _by1, ref _by2);

            }//if

            return ec_ret_val;

        }//GetM2ConfiguredCommand

        /*******************************************************************************
        * @brief This function takes the values in the instruction configuration controls
        * and passes them to the corresponding fucntion to convert that values into the
        * bytes that encode the selected instruction.
        * 
        * @param[out] _by0
        * @param[out] _by1
        * 
        * @return 
        *     - ErrCode with the error code or cErrCodes.
        *     - ERR_NO_ERROR if no error occurs.
        *******************************************************************************/
        public ErrCode GetChordConfiguredCommand(ref byte _by0, ref byte _by1) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";
            
            _by0 = 0;
            _by1 = 0;

            str_aux = cmboBoxChordInstr.Text;

            if (str_aux == ChordChannelCodeEntry.tCommandToString(ChordChannelCodeEntry.t_Command.REST_DURATION)) {

                // decode REST_DURATION command
                ec_ret_val = ChordChannelCodeEntry.GetRestCommandBytesFromParams(Convert.ToInt32(nUpDownChordRestRest.Value), ref _by0, ref _by1);

            }else if (str_aux == ChordChannelCodeEntry.tCommandToString(ChordChannelCodeEntry.t_Command.CHORD)) {

                    // decode NOTE command
                    ec_ret_val = ChordChannelCodeEntry.GetChordCommandBytesFromParams(ChordChannelCodeEntry.strToTNote(cmboBoxChordNote.Text), ChordChannelCodeEntry.strToTChordType(cmboBoxChordNoteType.Text),Convert.ToInt32(nUpDownChordNoteDur.Value), ref _by0, ref _by1);

            } else if (str_aux == ChordChannelCodeEntry.tCommandToString(ChordChannelCodeEntry.t_Command.REPEAT)) {

                // decode REPEAT command
                ec_ret_val = ChordChannelCodeEntry.GetRepeatCommandBytesFromParams(ChordChannelCodeEntry.strToTRepeatMark(cmboChordRepeat.Text), ref _by0, ref _by1);

            } else if (str_aux == ChordChannelCodeEntry.tCommandToString(ChordChannelCodeEntry.t_Command.RYTHM)) {

                // decode RYTHM command
                ec_ret_val = ChordChannelCodeEntry.GetRythmCommandBytesFromParams(ChordChannelCodeEntry.strToTRythmMode(cmboBoxChordRythmMode.Text),ChordChannelCodeEntry.strToTRythmStyle(cmboBoxChorddRythmStyle.Text), ChordChannelCodeEntry.strToTOnOff(cmboBoxChorddRythmOnOff.Text), ref _by0, ref _by1);

            } else if (str_aux == ChordChannelCodeEntry.tCommandToString(ChordChannelCodeEntry.t_Command.TEMPO)) {

                // decode TEMPO command
                ec_ret_val = ChordChannelCodeEntry.GetTempoCommandBytesFromParams(ChordChannelCodeEntry.strToTOnOff(cmboBoxChordTempoOnOff.Text), Convert.ToInt32(nUpDownChordTempo.Value), ref _by0, ref _by1);

            } else if (str_aux == ChordChannelCodeEntry.tCommandToString(ChordChannelCodeEntry.t_Command.COUNTER_RESET)) {

                // decode COUNTER_RESET command
                ec_ret_val = ChordChannelCodeEntry.GetCounterResetCommandBytesFromParams(ref _by0, ref _by1);


            } else if (str_aux == ChordChannelCodeEntry.tCommandToString(ChordChannelCodeEntry.t_Command.END)) {

                // decode END command
                ec_ret_val = ChordChannelCodeEntry.GetEndCommandBytesFromParams(ref _by0, ref _by1);

            } else if (str_aux == ChordChannelCodeEntry.tCommandToString(ChordChannelCodeEntry.t_Command.DURATIONx2)) {

                // decode DOUBLE DURATION command
                ec_ret_val = ChordChannelCodeEntry.Get2xDurationCommandBytesFromParams(Convert.ToInt32(nUpDownChordDurationX2Dur.Value), ref _by0, ref _by1);

            }//if

            return ec_ret_val;

        }//GetChordConfiguredCommand

        /***********************************************************************************************
        * @brief receives a container control and scales all the controls contained on according to the
        * received scale factors.
        * @param[in] containerControl the container control whose controls must be reescaled
        * @param[in] szScale the factors to apply when scaling on X and Y
        ***********************************************************************************************/
        public void scaleControlsInContainer(Control containerControl, SizeF szScale) {

            foreach (Control childControl in containerControl.Controls) {

                childControl.Scale(szScale);

            }//foreach

        }//scaleControlsInContainer

        /***********************************************************************************************
        * @brief receives a control and a container control then scales the received control and inserts
        * it into the received container control
        * @param[in] controlToAdd the control to scale and to insert into the container control.
        * @param[in] containerControl the container control on which the received control will be inserted
        * @param[in] szScale the factors to apply when scaling the received control on X and Y
        ***********************************************************************************************/
        public void scaleAndAddToPanel(Control controlToAdd, Control containerControl, SizeF szScale) {

            if (containerControl.GetType() == typeof(System.Windows.Forms.Panel)) {

                controlToAdd.Scale(szScale);

                Panel panelContainerCtrl = (Panel)containerControl;                
                panelContainerCtrl.Controls.Add(controlToAdd);

            } else {

                throw new ArgumentException("The received container control is not supported.");

            }//containerControl.GetType()

        }//scaleAndAddToPanel

        /***********************************************************************************************
        * @brief updates the index lists used to store the currently selected rows in the themes dataGridView
        * and the M1, M2 and chords instructions DataGridViews. Do not confuse the indexes of the currently 
        * selected rows in the DataGridView with the indexes of the active theme and instructions.
        ***********************************************************************************************/
        public void storeSelectedDGridViewRows() {
            int iCurrThemeIdx = 0;

            // take the index of the themes selected in the Themes dataGridView 
            dpack_drivePack.themes.liSelectedThemesDGridviewRows.Clear();
            foreach (DataGridViewRow rowAux in themeTitlesDataGridView.SelectedRows) {
                dpack_drivePack.themes.liSelectedThemesDGridviewRows.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_THEME_IDX].Value));
            }
            // liISelectionIdx.Sort();

            // take the index of the instructions selected in the M1, M2 and Chord dataGridViews
            iCurrThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;
            if (iCurrThemeIdx >= 0) {

                // keep the index of the instructions selected in the current theme M1 channel instructions datagridview
                dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedM1DGridviewRows.Clear();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedM1DGridviewRows.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                // dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedM1DGridviewRows.Sort();

                // keep the index of the instructions selected in the current theme M2 channel instructions datagridview
                dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedM2DGridviewRows.Clear();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedM2DGridviewRows.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                // dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedM2DGridviewRows.Sort();

                // keep the index of the instructions selected in the current theme Chord channel instructions datagridview
                dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedChordDGridviewRows.Clear();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedChordDGridviewRows.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                // dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedChordDGridviewRows.Sort();

                // store the index of the instruction that is at the top of the M1, M2 and chords dataGridViews due to scroll
                if (themeM1DataGridView.FirstDisplayedScrollingRowIndex != null) {
                    dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollM1DGridViewRow = themeM1DataGridView.FirstDisplayedScrollingRowIndex;
                } else {
                    dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollM1DGridViewRow = -1;
                }
                if (themeM2DataGridView.FirstDisplayedScrollingRowIndex != null) {
                    dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollM2DGridViewRow = themeM2DataGridView.FirstDisplayedScrollingRowIndex;
                } else {
                    dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollM2DGridViewRow = -1;
                }
                if (themeChordDataGridView.FirstDisplayedScrollingRowIndex != null) {
                    dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollChordDGridViewRow = themeChordDataGridView.FirstDisplayedScrollingRowIndex;
                } else {
                    dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollChordDGridViewRow = -1;
                }

            }//if

        }//storeSelectedDGridViewRows

        /***********************************************************************************************
        * @brief sets the selected rows in the different dataGridView according to the indexes specified
        * in the different Themes, M1,M2 and Chord indexes instructions lists. Do not confuse the indexes 
        * of the currently selected rows in the DataGridView with the indexes of the active theme 
        * and instructions.
        ***********************************************************************************************/
        public void setSelectedDGridViewRows() {
            int iCurrThemeIdx = 0;

            // set the selection of the themes specified in the selected themes list
            themeTitlesDataGridView.ClearSelection();
            foreach (int iIdxAux in dpack_drivePack.themes.liSelectedThemesDGridviewRows) {
                themeTitlesDataGridView.Rows[iIdxAux].Selected = true;
            }

            // set the selection of the M1, M2 and Chord instructions specified in the current theme instructions selected list
            iCurrThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;
            if (iCurrThemeIdx >= 0) {

                // select the M1 rows specified in the current theme selected M1 selected rows list
                themeM1DataGridView.ClearSelection();
                foreach (int iIdxAux in dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedM1DGridviewRows) {
                    themeM1DataGridView.Rows[iIdxAux].Selected = true;
                }

                // select the M2 rows instructios specified in the current theme selected M2 rows list
                themeM2DataGridView.ClearSelection();
                foreach (int iIdxAux in dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedM2DGridviewRows) {
                    themeM2DataGridView.Rows[iIdxAux].Selected = true;
                }

                // select the Chord rows instructios specified in the current theme selected Chord rows list
                themeChordDataGridView.ClearSelection();
                foreach (int iIdxAux in dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liSelectedChordDGridviewRows) {
                    themeChordDataGridView.Rows[iIdxAux].Selected = true;
                }

                // set the index of the instruction that is at the top of the M1, M2 and chords dataGridViews due to scroll
                if (dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollM1DGridViewRow>=0) {
                    themeM1DataGridView.FirstDisplayedScrollingRowIndex = dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollM1DGridViewRow;
                }
                if (dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollM2DGridViewRow >= 0) {
                    themeM2DataGridView.FirstDisplayedScrollingRowIndex = dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollM2DGridViewRow ;
                }
                if (dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollChordDGridViewRow >= 0) {
                    dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].iFirstScrollChordDGridViewRow = themeChordDataGridView.FirstDisplayedScrollingRowIndex;
                }

            }//if

        }//setSelectedDGridViewRows
 
    }//public partial class MainForm : Form

    /***********************************************************************************************
    * @brief Defines the object used to retrieve information of the screen scale configuration. It
    * is necessary to adjust the size and the location of the controlls added dinamically. It has 
    * been obtained from: https://stackoverflow.com/questions/32607468/get-scale-of-screen
    ***********************************************************************************************/
    public static class DPIUtil {

        // Retrieves a handle to the display monitor that contains a specified point: https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-monitorfrompoint
        [DllImport("User32.dll")]
        internal static extern IntPtr MonitorFromPoint([In] Point pt, [In] uint dwFlags);

        // Queries the dots per inch (dpi) of a display:https://learn.microsoft.com/en-us/windows/win32/api/shellscalingapi/nf-shellscalingapi-getdpiformonitor"/>
        [DllImport("Shcore.dll")]
        private static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

        // The RtlGetVersion routine returns version information about the currently running operating system: https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/wdm/nf-wdm-rtlgetversion
        [SecurityCritical]
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int RtlGetVersion(ref OSVERSIONINFOEXW versionInfo);

        // Struct which contains operating system version information: "https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-osversioninfoexw"
        [StructLayout(LayoutKind.Sequential)]
        private struct OSVERSIONINFOEXW {
            internal int dwOSVersionInfoSize;
            internal int dwMajorVersion;// The major version number of the operating system.
            internal int dwMinorVersion;// The minor version number of the operating system.
            internal int dwBuildNumber;// The build number of the operating system.
            internal int dwPlatformId;// The operating system platform.

            // A null-terminated string, such as "Service Pack 3", that indicates the latest Service Pack installed on the system.
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string szCSDVersion;
            internal ushort wServicePackMajor; // The major version number of the latest Service Pack installed on the system. 
            internal ushort wServicePackMinor; // The minor version number of the latest Service Pack installed on the system.
            internal short wSuiteMask; // A bit mask that identifies the product suites available on the system. 
            internal byte wProductType; // Any additional information about the system.
            internal byte wReserved; // Reserved for future use.
        }

        /// DPI type: "https://learn.microsoft.com/en-us/windows/win32/api/shellscalingapi/ne-shellscalingapi-monitor_dpi_type"
        private enum DpiType {
            Effective = 0, // The effective DPI. This value should be used when determining the correct scale factor for scaling UI elements.       
            Angular = 1, // The angular DPI. This DPI ensures rendering at a compliant angular resolution on the screen.       
            Raw = 2, // The raw DPI. This value is the linear DPI of the screen as measured on the screen itself. Use this value when you want to read the pixel density and not the recommended scaling setting.
        }

        private const int MinOSVersionBuild = 14393; // Min OS version build that supports DPI per monitor
        private const int MinOSVersionMajor = 10; // Min OS version major build that support DPI per monitor
        private static bool _isSupportingDpiPerMonitor; // Flag, if OS supports DPI per monitor
        private static bool _isOSVersionChecked; // Flag, if OS version checked

        /*******************************************************************************
        * @brief Flag, if OS supports DPI per monitor
        *******************************************************************************/
        internal static bool IsSupportingDpiPerMonitor {
            get {
                if (_isOSVersionChecked) {
                    return _isSupportingDpiPerMonitor;
                }

                _isOSVersionChecked = true;
                var osVersionInfo = new OSVERSIONINFOEXW {
                    dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEXW))
                };

                if (RtlGetVersion(ref osVersionInfo) != 0) {
                    _isSupportingDpiPerMonitor = Environment.OSVersion.Version.Major >= MinOSVersionMajor && Environment.OSVersion.Version.Build >= MinOSVersionBuild;

                    return _isSupportingDpiPerMonitor;
                }

                _isSupportingDpiPerMonitor = osVersionInfo.dwMajorVersion >= MinOSVersionMajor && osVersionInfo.dwBuildNumber >= MinOSVersionBuild;

                return _isSupportingDpiPerMonitor;
            }

        }//IsSupportingDpiPerMonitor

        /*******************************************************************************
        * @brief Get scale factor for an each monitor
        * @param[in] control Any control for OS who doesn't support DPI per monitor 
        * @param[in] monitorPoint Monitor point (Screen.Bounds)
        * @return Scale factor
        *******************************************************************************/
        public static double ScaleFactor(Control control, Point monitorPoint) {
            var dpi = GetDpi(control, monitorPoint);

            return dpi / 96.0;

        }//ScaleFactor

        /*******************************************************************************
        * @brief Get DPI for a monitor
        * @param[in] control Any control for OS who doesn't support DPI per monitor 
        * @param[in] monitorPoint Monitor point (Screen.Bounds)
        * @return DPI
        *******************************************************************************/
        public static uint GetDpi(Control control, Point monitorPoint) {
            uint dpiX;

            if (IsSupportingDpiPerMonitor) {
                var monitorFromPoint = MonitorFromPoint(monitorPoint, 2);

                GetDpiForMonitor(monitorFromPoint, DpiType.Effective, out dpiX, out _);
            } else {
                // If using with System.Windows.Forms - can be used Control.DeviceDpi
                dpiX = control == null ? 96 : (uint)control.DeviceDpi;
            }

            return dpiX;

        }//GetDpi

    }//DPIUtil

}//namespace drivePackEd
