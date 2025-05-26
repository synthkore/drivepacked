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
        const string STR_TRACK = "Track ";

        // attributes used to store the result in the selection ComboBoxes
        public int iM1ChanMIDITrack;
        public int iM2ChanMIDITrack;
        public int iChordsChanMIDITrack;
        public int iMetaDataMIDITrack;
        public bool bGenChanBeginningEnd;// flag that indicates if the ROM pack channels beginning and ending must be generated or not

        public MIDIimportForm() {
            InitializeComponent();
        }

        public MIDIimportForm(ImportMIDIFileInfo midiFileData) {
            string strAux = "";
            int iAux = 0;
            bool bAssigned = false;

            InitializeComponent();

            // fill the channels combo boxes: add each number of the MIDI music tracks found 
            // in the MIDI file into the channel source track selection combo Boxes
            cmbBoxM1Chan.Items.Clear();
            cmbBoxM1Chan.Items.Add(STR_NO_TRACK_SELECTED);
            cmbBoxM2Chan.Items.Clear();
            cmbBoxM2Chan.Items.Add(STR_NO_TRACK_SELECTED);
            cmbBoxChordChan.Items.Clear();
            cmbBoxChordChan.Items.Add(STR_NO_TRACK_SELECTED);
            iAux = 0;
            foreach (ImportMIDITrackInfo midiTrack in midiFileData.liTracks) {

                if (midiTrack.bMusicTrack) {
                    cmbBoxM1Chan.Items.Add(STR_TRACK + iAux.ToString());
                    cmbBoxM2Chan.Items.Add(STR_TRACK + iAux.ToString());
                    cmbBoxChordChan.Items.Add(STR_TRACK + iAux.ToString());
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
                    cmbBoxMetaData.Items.Add(STR_TRACK + iAux.ToString());
                }
                iAux++;
            }

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

        }//MIDIimportForm

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

            bGenChanBeginningEnd = chkBxGenChBeginEnd.Checked;

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

    }

}
