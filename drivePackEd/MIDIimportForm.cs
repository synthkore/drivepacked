using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// **********************************************************************************
// ****                          drivePACK Editor                                ****
// ****                         www.tolaemon.com/dpack                           ****
// ****                              Source code                                 ****
// ****                              17/05/2025                                  ****
// ****                            Jordi Bartolome                               ****
// ****                                                                          ****
// ****          IMPORTANT:                                                      ****
// ****          Using this code or any part of it means accepting all           ****
// ****          conditions exposed in: http://www.tolaemon.com/dpack            ****
// **********************************************************************************

namespace drivePackEd {

    public partial class MIDIimportForm : Form {

        const string STR_NO_TRACK_SELECTED = "---";
        const string STR_TRACK = "track ";

        // attributes used to store the result in the selection ComboBoxes
        public int iM1ChanMIDITrack;
        public int iM2ChanMIDITrack;
        public int iChordsChanMIDITrack;
        public int iMetaDataMIDITrack;
        public bool bNoGenChanBeginEnd;// flag that indicates if the ROM pack channels beginning and ending must be generated or not
        public int iTempo;
        public int iRythmDiscrimination;// the duration of time discrimination at the beggining of the theme, or 0 if there is no rythm discrimination
        public MChannelCodeEntry.t_Instrument tInstrM1Instrument;
        public MChannelCodeEntry.t_Instrument tInstrM2Instrument;
        public ChordChannelCodeEntry.t_RythmStyle tChordsRythm;
        public MChannelCodeEntry.t_Time tTimeMark;
        public int iKey;

        public MIDIimportForm() {
            InitializeComponent();
        }

        /*******************************************************************************
        * @brief Constructor with parameters
        * @param[in] midiFileData structure with the general information retrieved from
        * the MIDI file before imporing it.
        *******************************************************************************/
        public MIDIimportForm(ImportMIDIFileInfo midiFileData) {
            string strAux = "";
            int iAux = 0;
            bool bAssigned = false;

            InitializeComponent();

            // List to bind to the Melody 1 instrument ComboBox
            BindingList<string> liMelody1Instrument = new BindingList<string>();
            // List to bind to the Melody 2 instrument ComboBox
            BindingList<string> liMelody2Instrument = new BindingList<string>();
            // List to bind to the Rythms selection ComboBox
            BindingList<string> liChordRythmStyle = new BindingList<string>();
            // List to bind to the Time selection ComboBox
            BindingList<string> liTime = new BindingList<string>();
            // Lists to bind to the MIDI track selection ComboBox
            BindingList<string> liM1MIDITracks = new BindingList<string>();
            BindingList<string> liM2MIDITracks = new BindingList<string>();
            BindingList<string> liChordsMIDITracks = new BindingList<string>();
            BindingList<string> liMetadataMIDITracks = new BindingList<string>();


            // populate the lists that will be binded to the ComboBoxes to allow selected the default instrument
            foreach (MChannelCodeEntry.t_Instrument t_instr in Enum.GetValues(typeof(MChannelCodeEntry.t_Instrument))) {
                liMelody1Instrument.Add(MChannelCodeEntry.tInstrumentToString(t_instr));
                liMelody2Instrument.Add(MChannelCodeEntry.tInstrumentToString(t_instr));
            }

            // populate the list that will be binded to the ComboBoxes to allow the default rythm
            foreach (ChordChannelCodeEntry.t_RythmStyle t_style in Enum.GetValues(typeof(ChordChannelCodeEntry.t_RythmStyle))) {
                liChordRythmStyle.Add(ChordChannelCodeEntry.tRythmStyleToString(t_style));
            }

            // populate the list that will be binded to the ComboBox to allow to select the them Time mark
            foreach (MChannelCodeEntry.t_Time t_time in Enum.GetValues(typeof(MChannelCodeEntry.t_Time))) {
                liTime.Add(MChannelCodeEntry.tTimeToString(t_time));
            }

            // populate the lists that will be binded to the channels MIDI track selection combo boxes: add each number of
            // the MIDI music tracks found in the MIDI file into the channel source track selection combo Boxes            
            // Start by adding to the list the null-no_MIDI_track_selected element
            liM1MIDITracks.Add(STR_NO_TRACK_SELECTED);
            liM2MIDITracks.Add(STR_NO_TRACK_SELECTED);
            liChordsMIDITracks.Add(STR_NO_TRACK_SELECTED);
            liMetadataMIDITracks.Add(STR_NO_TRACK_SELECTED);
            iAux = 0;
            foreach (ImportMIDITrackInfo midiTrack in midiFileData.liTracks) {

                if (midiTrack.bMusicTrack) {
                    liM1MIDITracks.Add(STR_TRACK + iAux.ToString());
                    liM2MIDITracks.Add(STR_TRACK + iAux.ToString());
                    liChordsMIDITracks.Add(STR_TRACK + iAux.ToString());
                }
                iAux++;

            }//foreach

            // fill the metadata combo box: add the number of the MIDI metadata tracks found
            // in the MIDI file into the metadata source selection combo Box
            cmbBoxMetaData.Items.Clear();
            cmbBoxMetaData.Items.Add(STR_NO_TRACK_SELECTED);
            iAux = 0;
            foreach (ImportMIDITrackInfo midiTrack in midiFileData.liTracks) {
                if (midiTrack.bMetadataTrack) {
                    liMetadataMIDITracks.Add(STR_TRACK + iAux.ToString());
                }
                iAux++;
            }

            // bind the lists with the data to different controls
            cmbBoxM1Chan.DataSource = liM1MIDITracks;
            cmbBoxM2Chan.DataSource = liM2MIDITracks;
            cmbBoxChordChan.DataSource = liChordsMIDITracks;
            cmbBoxMetaData.DataSource = liMetadataMIDITracks;
            cmbBoxM1Instr.DataSource = liMelody1Instrument;
            cmbBoxM2Instr.DataSource = liMelody2Instrument;
            cmbBoxChordRythm.DataSource = liChordRythmStyle;
            cmbBoxTime.DataSource = liTime;

            // set the default value in the form controls
            cmbBoxTime.Text = MChannelCodeEntry.tTimeToString(MChannelCodeEntry.t_Time._4x4);
            nUpDwnKey.Value = 128;
            nUpDwnDiscrimination.Value = 4;
            nUpDwnTempo.Value = 100;
            chkBxNoGenChBeginEnd.Checked = false;

            // set the default selected music track for each combobox
            iAux = 0;
            // set the selected track for melody1 source track selection combobox
            bAssigned = false;
            cmbBoxM1Chan.Text = STR_NO_TRACK_SELECTED;
            while ((iAux < midiFileData.liTracks.Count()) && !bAssigned) {
                if (midiFileData.liTracks[iAux].bMusicTrack) {
                    cmbBoxM1Chan.Text = STR_TRACK + iAux.ToString();
                    bAssigned = true;
                }
                iAux++;
            }
            // set the selected track for melody2 source track selection combobox
            bAssigned = false;
            cmbBoxM2Chan.Text = STR_NO_TRACK_SELECTED;
            while ((iAux < midiFileData.liTracks.Count()) && !bAssigned) {
                if (midiFileData.liTracks[iAux].bMusicTrack) {
                    cmbBoxM2Chan.Text = STR_TRACK + iAux.ToString();
                    bAssigned = true;
                }
                iAux++;
            }
            // set the selected track for chord source track selection combobox
            bAssigned = false;
            cmbBoxChordChan.Text = STR_NO_TRACK_SELECTED;
            while ((iAux < midiFileData.liTracks.Count()) && !bAssigned) {
                if (midiFileData.liTracks[iAux].bMusicTrack) {
                    cmbBoxChordChan.Text = STR_TRACK + iAux.ToString();
                    bAssigned = true;
                }
                iAux++;
            }
            // set the selected track for the title (metada) source track selection combobox
            iAux = 0;
            bAssigned = false;
            cmbBoxMetaData.Text = STR_NO_TRACK_SELECTED;
            while ((iAux < midiFileData.liTracks.Count()) && !bAssigned) {
                if (midiFileData.liTracks[iAux].bMetadataTrack) {
                    cmbBoxMetaData.Text = STR_TRACK + iAux.ToString();
                    bAssigned = true;
                }
                iAux++;
            }

            // set default selected MIDI tracks as none
            iM1ChanMIDITrack = -1;
            iM2ChanMIDITrack = -1;
            iChordsChanMIDITrack = -1;
            iMetaDataMIDITrack = -1;

            // check and show the warnings detected when importing the track
            ShowWarnings(midiFileData);

            UpdateControls();

        }//MIDIimportForm

        /*******************************************************************************
        * @brief Shows in the corresponding form textbox the possible elements that may
        * cause problems when importing the specified MIDI file.
        * 
        * @param[in] midiFileData structure with the general information retrieved from
        * the MIDI file before imporing it.
        *******************************************************************************/
        public void ShowWarnings(ImportMIDIFileInfo midiFileData) {
            int iIDx = 0;
            string str_aux = "";
            bool b_warning_detected = false; 

            iIDx = 0;
            foreach (ImportMIDITrackInfo track in midiFileData.liTracks) {

                // check if the processed MIDI channel has note codes under 53. 53 is the F3 MIDI note code 
                if ((track.iLowestNoteCode < 53) || (track.iHighestNoteCode<53)) {
                    str_aux = str_aux + "WARNING: MIDI chan " + iIDx + " has notes under F3 and won't be properly imported.\r\n";
                    b_warning_detected = true;
                }
                // check if the processed MIDI channel has note codes over 84. 84 is the C6 MIDI note code 
                if ((track.iLowestNoteCode > 84) || (track.iHighestNoteCode > 84)) {
                    str_aux = str_aux + "WARNING: MIDI chan " + iIDx + " has notes over C6 and won't be properly imported.\r\n";
                    b_warning_detected = true;
                }
                // check if the processed MIDI channel is polyphonic ( has notes playing simultaneously). CASIO ROMPACK
                // only allow monophonic tracks
                if (track.bPolyphonic) {
                    str_aux = str_aux + "WARNING: MIDI chan " + iIDx + " has some notes overlaped and may not be propely imported.\r\n";
                    b_warning_detected = true;
                }

                iIDx++;

            }//foreach

            if (!b_warning_detected) {
                str_aux = str_aux + "No warnings detected in the specified MIDI file.";
            }

            warnTextBox.Text = str_aux;

        }//ShowWarnings

        /*******************************************************************************
        * @brief This procedure updates all the controls shown in the MIDI import form
        * control according to the state of the different variables state.
        *******************************************************************************/
        public void UpdateControls() {

            if (chkBxNoGenChBeginEnd.Checked) {

                cmbBoxChordRythm.Enabled = false;
                cmbBoxTime.Enabled = false;
                nUpDwnKey.Enabled = false;
                nUpDwnTempo.Enabled = false;
                nUpDwnDiscrimination.Enabled = false;
                cmbBoxM1Instr.Enabled = false;
                cmbBoxM2Instr.Enabled = false;

            } else {

                cmbBoxChordRythm.Enabled = true;
                cmbBoxTime.Enabled = true;
                nUpDwnKey.Enabled = true;
                nUpDwnTempo.Enabled = true;
                nUpDwnDiscrimination.Enabled = true;
                cmbBoxM1Instr.Enabled = true;
                cmbBoxM2Instr.Enabled = true;

            }//if

        }//UpdateControls

        /*******************************************************************************
        * @brief delegate that manges the click event on the send Cancel button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnCancel_Click(object sender, EventArgs e) {

            // close the form and return "Cancel"
            DialogResult = DialogResult.Cancel;
            this.Close();

        }//btnCancel_Click

        /*******************************************************************************
        * @brief delegate for the click on the button to import the selected MIDI file
        * according to the configured parameters.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnImport_Click(object sender, EventArgs e) {
            string str_aux = "";

            // get the MIDI track number assigned to the ROM Pack M1 channel
            str_aux = cmbBoxM1Chan.Text.Replace(STR_TRACK, "");
            if (!int.TryParse(str_aux, out iM1ChanMIDITrack)) {
                iM1ChanMIDITrack = -1;
            }

            // get the MIDI track number assigned to the ROM Pack M2 channel
            str_aux = cmbBoxM2Chan.Text.Replace(STR_TRACK, "");
            if (!int.TryParse(str_aux, out iM2ChanMIDITrack)) {
                iM2ChanMIDITrack = -1;
            }

            // get the MIDI track number assigned to the chords channel
            str_aux = cmbBoxChordChan.Text.Replace(STR_TRACK, "");
            if (!int.TryParse(str_aux, out iChordsChanMIDITrack)) {
                iChordsChanMIDITrack = -1;
            }

            // get the MIDI track to use to get metadata
            str_aux = cmbBoxMetaData.Text.Replace(STR_TRACK, "");
            if (!int.TryParse(str_aux, out iMetaDataMIDITrack)) {
                iMetaDataMIDITrack = -1;
            }

            // take the value set by the user in the form controls
            tInstrM1Instrument = MChannelCodeEntry.strToInstrument(cmbBoxM1Instr.Text);// get the instrument asigned by the user to Melody 1 channel
            tInstrM2Instrument = MChannelCodeEntry.strToInstrument(cmbBoxM2Instr.Text); // get the instrument asigned by the user to Melody 2 channel            
            tChordsRythm = ChordChannelCodeEntry.strToTRythmStyle(cmbBoxChordRythm.Text);// get the rythm style set by the user 
            tTimeMark = MChannelCodeEntry.strToTimetMark(cmbBoxTime.Text);
            iKey = (int)nUpDwnKey.Value;
            iTempo = (int)nUpDwnTempo.Value;
            iRythmDiscrimination = (int)nUpDwnDiscrimination.Value;
            bNoGenChanBeginEnd = chkBxNoGenChBeginEnd.Checked;

            // close the form and return "Ok"
            DialogResult = DialogResult.OK;
            this.Close();

        }//btnImport_Click

        /*******************************************************************************
        * @brief  delegate for the send form closing event
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void MIDIimportForm_FormClosing(object sender, FormClosingEventArgs e) {

            Dispose();

        }//MIDIimportForm_FormClosing

        /*******************************************************************************
        * @brief  delegate for the event trigered when the user checks or unchecks that
        * checkbox.
        * 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void chkBxNoGenChBeginEnd_CheckedChanged(object sender, EventArgs e) {

            UpdateControls();

        }//chkBxNoGenChBeginEnd_CheckedChanged

    }

}
