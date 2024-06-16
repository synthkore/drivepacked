using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace drivePackEd {

    public partial class MainForm : Form {


        /*******************************************************************************
        * @brief delegate that manages the click on the add entry to M1 channel button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addM1EntryButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            int iThemeIdx = 0;
            int iInstrIdx = 0;
            int iAux2 = 0;
            MChannelCodeEntry melodyCodeEntryAux = null;
            string strAux = "";
            
            // check if there is any theme selected to add the instructions to
            if (dpack_drivePack.themes.iCurrThemeIdx < 0) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            
            if (ec_ret_val.i_code >= 0) {

                // check that the maximum number of allowed instructions on the channel has not been reached yet
                if (dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_NO_SPACE_FOR_INSTRUCTIONS;
                }

            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                if (liISelectionIdx.Count == 0) {

                    // if the channel does not contain any instruction or if there are no instrucions selected just add the instructions at the end
                    iInstrIdx = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count;

                } else {

                    // if there are instructions selected get the lowest index of all selected rows and add the new instruction after it
                    iInstrIdx = liISelectionIdx[0];
                    iInstrIdx = iInstrIdx + 1;//iInstrIdx+1 to insert after speficied element

                }//if

                melodyCodeEntryAux = new MChannelCodeEntry();
                dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Insert(iInstrIdx, melodyCodeEntryAux);

                // as we have added new elements in the list , update the index of all the instructions. As the index
                // have been ordered with .Sort the new index can be applied startig from the first index in the list
                for (iAux2 = iInstrIdx; iAux2 < dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count; iAux2++) {
                    dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iAux2].Idx = iAux2;
                }

                // update the number of Melody 1 channel instructions to the new value
                lblMel1Ch.Text = "Melody 1 ch.code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count.ToString("D3") + "):";

                // keep selected the added instruction
                themeM1DataGridView.ClearSelection();
                themeM1DataGridView.Rows[iInstrIdx].Selected = true;

            }// if

            // show the corresponding message in the output logs
            if (ec_ret_val.i_code >= 0) {

                // informative message for the user 
                strAux = "Added an instruction at position " + iInstrIdx + " in the theme \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 1 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Could not add the instruction in the  themes \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 1 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_EDITION + strAux, false);

            }//if

        }//addM1EntryButton_Click

        /*******************************************************************************
        * @brief delegate that manages the click on the add entry to M2 channel button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addM2EntryButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            int iThemeIdx = 0;
            int iInstrIdx = 0;
            int iAux2 = 0;
            MChannelCodeEntry melodyCodeEntryAux = null;
            string strAux = "";

            // check if there is any theme selected to add the instructions to
            if (dpack_drivePack.themes.iCurrThemeIdx < 0) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // check that the maximum number of allowed instructions on the channel has not been reached yet
                if (dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_NO_SPACE_FOR_INSTRUCTIONS;
                }

            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                if (liISelectionIdx.Count == 0) {

                    // if the channel does not contain any instruction or if there are no instrucions selected just add the instructions at the end
                    iInstrIdx = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count;

                } else {

                    // if there are instructions selected get the lowest index of all selected rows and add the new instruction after it
                    iInstrIdx = liISelectionIdx[0];
                    iInstrIdx = iInstrIdx + 1;//iInstrIdx+1 to insert after speficied element

                }//if

                melodyCodeEntryAux = new MChannelCodeEntry();
                dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Insert(iInstrIdx, melodyCodeEntryAux);

                // as we have added new elements in the list , update the index of all the instructions. As the index
                // have been ordered with .Sort the new index can be applied startig from the first index in the list
                for (iAux2 = iInstrIdx; iAux2 < dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count; iAux2++) {
                    dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iAux2].Idx = iAux2;
                }

                // update the number of Melody 2 channel instructions to the new value
                lblMel2Ch.Text = "Melody 2 ch.code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count.ToString("D3") + "):";

                // keep selected the added instruction
                themeM2DataGridView.ClearSelection();
                themeM2DataGridView.Rows[iInstrIdx].Selected = true;

            }// if

            // show the corresponding message in the output logs
            if (ec_ret_val.i_code >= 0) {

                // informative message for the user 
                strAux = "Added an instruction at position " + iInstrIdx + " in the theme \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 2 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Could not add the instruction in the  themes \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 2 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_EDITION + strAux, false);

            }//if

        }//addM2EntryButton_Click

        /*******************************************************************************
        * @brief delegate that manages the click on the button that adds a new to chord 
        * entry to channel button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addChordEntryButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            int iThemeIdx = 0;
            int iInstrIdx = 0;
            int iAux2 = 0;
            ChordChannelCodeEntry chordCodeEntryAux = null;
            string strAux = "";

            // check if there is any theme selected to add the instructions to
            if (dpack_drivePack.themes.iCurrThemeIdx < 0){
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // check that the maximum number of allowed instructions on the channel has not been reached yet
                if (dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_NO_SPACE_FOR_INSTRUCTIONS;
                }

            }//if

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                if (liISelectionIdx.Count == 0) {

                    // if the channel does not contain any instruction or if there are no instrucions selected just add the instructions at the end
                    iInstrIdx = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count;

                } else {

                    // if there are instructions selected get the lowest index of all selected rows and add the new instruction after it
                    iInstrIdx = liISelectionIdx[0];
                    iInstrIdx = iInstrIdx + 1;//iInstrIdx+1 to insert after speficied element

                }//if

                chordCodeEntryAux = new ChordChannelCodeEntry();
                dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Insert(iInstrIdx, chordCodeEntryAux);

                // as we have added new elements in the list , update the index of all the instructions. As the index
                // have been ordered with .Sort the new index can be applied startig from the first index in the list
                for (iAux2 = iInstrIdx; iAux2 < dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count; iAux2++) {
                    dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iAux2].Idx = iAux2;
                }

                // update the number of chords channel instructions to the new value
                lblChordCh.Text = "Chords ch. code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count.ToString("D3") + "):";

                // keep selected the added instruction
                themeChordDataGridView.ClearSelection();
                themeChordDataGridView.Rows[iInstrIdx].Selected = true;

            }// if

            // show the corresponding message in the output logs
            if (ec_ret_val.i_code >= 0) {

                // informative message for the user 
                strAux = "Added an instruction at position " + iInstrIdx + " in the theme \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" chords channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Could not add the instruction in the  themes \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" chords channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_EDITION + strAux, false);

            }//if

        }//addChordEntryButton_Click

        /*******************************************************************************
        * @brief delegate that manages the click on the button to delete selected entries
        * from M1 channel.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void delM1EntryButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iAux = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of and delte all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel M1 instruction with the specified Idx and remove it from the M1 instructions list
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Remove(instrAux);
                        }

                    }//foreach

                    // as we have deleted a elements in the list , update the index of all the instructions. As the index
                    // have been ordered with .Sort the new index can be applied startig from the first index in the list
                    for (iAux = 0; iAux < dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count; iAux++) {
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iAux].Idx = iAux;
                    }

                    // update the number of Melody 1 channel instructions to the new value
                    lblMel1Ch.Text = "Melody 1 ch.code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count.ToString("D3") + "):";

                    // no instruction selected after deleting selected instructions
                    themeM1DataGridView.ClearSelection();

                }//if

            }// if

        }//delM1EntryButton_Click

        /*******************************************************************************
        * @brief delegate that manages the click on the button to delete selected entries
        * from M2 channel.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void delM2EntryButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iAux = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of and delte all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel M2 instruction with the specified Idx and remove it from the M2 instructions list
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Remove(instrAux);
                        }

                    }//foreach

                    // as we have deleted a elements in the list , update the index of all the instructions. As the index
                    // have been ordered with .Sort the new index can be applied startig from the first index in the list
                    for (iAux = 0; iAux < dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count; iAux++) {
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iAux].Idx = iAux;
                    }

                    // update the number of Melody 2 channel instructions to the new value
                    lblMel2Ch.Text = "Melody 2 ch.code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count.ToString("D3") + "):";

                    // no instruction selected after deleting selected instructions
                    themeM2DataGridView.ClearSelection();

                }//if

            }// if

        }//delM2EntryButton_Click

        /*******************************************************************************
        * @brief delegate that manages the click on the button to delete selected entries 
        * from chords channel.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void delChordEntryButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ChordChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iAux = 0;

            // check if there is any theme selected and if the Chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of and delte all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel Chords instruction with the specified Idx and remove it from the Chords instructions list
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Remove(instrAux);
                        }

                    }//foreach

                    // as we have deleted a elements in the list , update the index of all the instructions. As the index
                    // have been ordered with .Sort the new index can be applied startig from the first index in the list
                    for (iAux = 0; iAux < dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count; iAux++) {
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iAux].Idx = iAux;
                    }

                    // update the number of chords channel instructions to the new value
                    lblChordCh.Text = "Chords ch. code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count.ToString("D3") + "):";

                    // no instruction selected after deleting selected instructions
                    themeChordDataGridView.ClearSelection();

                }//if

            }// if 

        }//delChordEntryButton_Click

        /*******************************************************************************
        * @brief delegate that manages the click on the button to swap the order of the
        * selected M1 code entries.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void swapM1EntriesButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                if (liISelectionIdx.Count > 1) {

                    iAux2 = liISelectionIdx.Count - 1;
                    for (iAux = 0; iAux < (int)(liISelectionIdx.Count / 2); iAux++) {

                        iInstrIdx1 = liISelectionIdx[iAux];
                        iInstrIdx2 = liISelectionIdx[iAux2];

                        // swap the content of the rows less the Idx:
                        // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                        melodyCodeEntryAux = new MChannelCodeEntry();
                        melodyCodeEntryAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By0;
                        melodyCodeEntryAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By1;
                        melodyCodeEntryAux.By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By2;
                        melodyCodeEntryAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].strDescr;

                        // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By0;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By1;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By2;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].strDescr;

                        // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By0 = melodyCodeEntryAux.By0;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By1 = melodyCodeEntryAux.By1;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By2 = melodyCodeEntryAux.By2;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].strDescr = melodyCodeEntryAux.strDescr;

                        iAux2--;

                    }//for (iAux=0;

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    themeM1DataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeM1DataGridView.Rows[idxSwapped].Selected = true;
                    }

                }//if

            }//if  

        }//swapM1EntriesButton_Click

        /*******************************************************************************
        * @brief delegate that manages the click on the button to swap the order of the
        * selected M2 code entries.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void swaplM2EntriesButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                if (liISelectionIdx.Count > 1) {

                    iAux2 = liISelectionIdx.Count - 1;
                    for (iAux = 0; iAux < (int)(liISelectionIdx.Count / 2); iAux++) {

                        iInstrIdx1 = liISelectionIdx[iAux];
                        iInstrIdx2 = liISelectionIdx[iAux2];

                        // swap the content of the rows less the Idx:
                        // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                        melodyCodeEntryAux = new MChannelCodeEntry();
                        melodyCodeEntryAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By0;
                        melodyCodeEntryAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By1;
                        melodyCodeEntryAux.By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By2;
                        melodyCodeEntryAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].strDescr;

                        // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By0;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By1;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By2;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].strDescr;

                        // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By0 = melodyCodeEntryAux.By0;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By1 = melodyCodeEntryAux.By1;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By2 = melodyCodeEntryAux.By2;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].strDescr = melodyCodeEntryAux.strDescr;

                        iAux2--;

                    }//for (iAux=0;

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    themeM2DataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeM2DataGridView.Rows[idxSwapped].Selected = true;
                    }

                }//if

            }//if  

        }//swaplM2EntriesButton_Click

        /*******************************************************************************
        * @brief delegate that manages the click on the button to swap the order of the
        * selected chord code entries.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void swapChordCodeEntriesButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ChordChannelCodeEntry instrAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the Chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                if (liISelectionIdx.Count > 1) {

                    iAux2 = liISelectionIdx.Count - 1;
                    for (iAux = 0; iAux < (int)(liISelectionIdx.Count / 2); iAux++) {

                        iInstrIdx1 = liISelectionIdx[iAux];
                        iInstrIdx2 = liISelectionIdx[iAux2];

                        // swap the content of the rows less the Idx:
                        // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                        instrAux = new ChordChannelCodeEntry();
                        instrAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0;
                        instrAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1;
                        instrAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr;

                        // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr;

                        // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0 = instrAux.By0;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1 = instrAux.By1;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr = instrAux.strDescr;

                        iAux2--;

                    }//for (iAux=0;

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    themeChordDataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeChordDataGridView.Rows[idxSwapped].Selected = true;
                    }

                }//if

            }//if 

        }//swapChordCodeEntriesButton_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Up one position all selected M1 
        * Cahnnel instructions 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnUpM1Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iAux = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = liISelectionIdx[0];
                    if (iInstrIdx1 > 0) {

                        iInstrIdx1 = iInstrIdx1 - 1;

                        for (iAux = 0; iAux < (int)liISelectionIdx.Count; iAux++) {

                            iInstrIdx2 = liISelectionIdx[iAux];

                            // swap the content of the rows less the Idx:
                            // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                            instrAux = new MChannelCodeEntry();
                            instrAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By0;
                            instrAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By1;
                            instrAux.By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By2;
                            instrAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].strDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By2;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].strDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By0 = instrAux.By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By1 = instrAux.By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By2 = instrAux.By2;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].strDescr = instrAux.strDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;


                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeM1DataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeM1DataGridView.Rows[idxInstruction - 1].Selected = true;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if  

        }//btnUpM1Entry_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Up one position all selected M1 
        * Cahnnel instructions 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnUpM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iAux = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = liISelectionIdx[0];
                    if (iInstrIdx1 > 0) {

                        iInstrIdx1 = iInstrIdx1 - 1;

                        for (iAux = 0; iAux < (int)liISelectionIdx.Count; iAux++) {

                            iInstrIdx2 = liISelectionIdx[iAux];

                            // swap the content of the rows less the Idx:
                            // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                            instrAux = new MChannelCodeEntry();
                            instrAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By0;
                            instrAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By1;
                            instrAux.By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By2;
                            instrAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].strDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By2;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].strDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By0 = instrAux.By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By1 = instrAux.By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By2 = instrAux.By2;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].strDescr = instrAux.strDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeM2DataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeM2DataGridView.Rows[idxInstruction - 1].Selected = true;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if  

        }//btnUpM2Entry_Click

        /*******************************************************************************
         * @brief  Delegate for the click on the move Up one position all selected M2 
         * Cahnnel instructions 
         * button
         * @param[in] sender reference to the object that raises the event
         * @param[in] e the information related to the event
         *******************************************************************************/
        private void btnUpChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ChordChannelCodeEntry instrAux = null;
            int iAux = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the Chord channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = liISelectionIdx[0];
                    if (iInstrIdx1 > 0) {

                        iInstrIdx1 = iInstrIdx1 - 1;

                        for (iAux = 0; iAux < (int)liISelectionIdx.Count; iAux++) {

                            iInstrIdx2 = liISelectionIdx[iAux];

                            // swap the content of the rows less the Idx:
                            // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                            instrAux = new ChordChannelCodeEntry();
                            instrAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0;
                            instrAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1;
                            instrAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0 = instrAux.By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1 = instrAux.By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr = instrAux.strDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeChordDataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeChordDataGridView.Rows[idxInstruction - 1].Selected = true;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if  

        }//btnUpChordEntry_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Down one position all selected M1 
        * Cahnnel instructions 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnDownM1Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = liISelectionIdx[liISelectionIdx.Count - 1];
                    if (iInstrIdx1 < (dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr.Count - 1)) {

                        iInstrIdx1 = iInstrIdx1 + 1;

                        for (iAux = (int)(liISelectionIdx.Count - 1); iAux >= 0; iAux--) {

                            iInstrIdx2 = liISelectionIdx[iAux];

                            // swap the content of the rows less the Idx:
                            // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                            melodyCodeEntryAux = new MChannelCodeEntry();
                            melodyCodeEntryAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By0;
                            melodyCodeEntryAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By1;
                            melodyCodeEntryAux.By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By2;
                            melodyCodeEntryAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].strDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By2;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].strDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By0 = melodyCodeEntryAux.By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By1 = melodyCodeEntryAux.By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].By2 = melodyCodeEntryAux.By2;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx2].strDescr = melodyCodeEntryAux.strDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // use the idx stored at the begining of the method to keep selected the rows that have been moved
                        themeM1DataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeM1DataGridView.Rows[idxInstruction + 1].Selected = true; ;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if  

        }//btnDownM1Entry_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Down one position all selected M2 
        * Cahnnel instructions 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnDownM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = liISelectionIdx[liISelectionIdx.Count - 1];
                    if (iInstrIdx1 < (dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr.Count - 1)) {

                        iInstrIdx1 = iInstrIdx1 + 1;

                        for (iAux = (int)(liISelectionIdx.Count - 1); iAux >= 0; iAux--) {

                            iInstrIdx2 = liISelectionIdx[iAux];

                            // swap the content of the rows less the Idx:
                            // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                            melodyCodeEntryAux = new MChannelCodeEntry();
                            melodyCodeEntryAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By0;
                            melodyCodeEntryAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By1;
                            melodyCodeEntryAux.By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By2;
                            melodyCodeEntryAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].strDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By2;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].strDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By0 = melodyCodeEntryAux.By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By1 = melodyCodeEntryAux.By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].By2 = melodyCodeEntryAux.By2;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx2].strDescr = melodyCodeEntryAux.strDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // use the idx stored at the begining of the method to keep selected the rows that have been moved
                        themeM2DataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeM2DataGridView.Rows[idxInstruction + 1].Selected = true;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if  

        }//btnDownM2Entry_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Down one position all selected Chords 
        * Channel instructions 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnDownChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ChordChannelCodeEntry instrAux = null;
            int iAux = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the Chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = liISelectionIdx[liISelectionIdx.Count - 1];
                    if (iInstrIdx1 < (dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr.Count - 1)) {

                        iInstrIdx1 = iInstrIdx1 + 1;

                        for (iAux = (int)(liISelectionIdx.Count - 1); iAux >= 0; iAux--) {

                            iInstrIdx2 = liISelectionIdx[iAux];

                            // swap the content of the rows less the Idx:
                            // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                            instrAux = new ChordChannelCodeEntry();
                            instrAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0;
                            instrAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1;
                            instrAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0 = instrAux.By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1 = instrAux.By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr = instrAux.strDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // use the idx stored at the begining of the method to keep selected the rows that have been moved
                        themeChordDataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeChordDataGridView.Rows[idxInstruction + 1].Selected = true; ;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if  

        }//btnDownChordEntry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the copy selected M1 instructions button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnCopyM1Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iAux = 0;
            int iInstrIdx = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // initialize and the temporary list of instructions                    
                    liCopyTemporaryInstr = new List<MChannelCodeEntry>();

                    // copy all the selected instructions in the temporary instructions list
                    for (iAux = 0; iAux < (int)liISelectionIdx.Count; iAux++) {

                        iInstrIdx = liISelectionIdx[iAux];
                        instrAux = new MChannelCodeEntry();
                        instrAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx].By0;
                        instrAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx].By1;
                        instrAux.By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx].By2;
                        instrAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iInstrIdx].strDescr;
                        liCopyTemporaryInstr.Add(instrAux);

                    }//for

                }//if

            }//if  

        }//btnCopyM1Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the copy selected M2 instructions button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnCopyM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iAux = 0;
            int iInstrIdx = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // initialize and the temporary list of instructions                    
                    liCopyTemporaryInstr = new List<MChannelCodeEntry>();

                    // copy all the selected instructions in the temporary instructions list
                    for (iAux = 0; iAux < (int)liISelectionIdx.Count; iAux++) {

                        iInstrIdx = liISelectionIdx[iAux];
                        instrAux = new MChannelCodeEntry();
                        instrAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx].By0;
                        instrAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx].By1;
                        instrAux.By2 = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx].By2;
                        instrAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iInstrIdx].strDescr;
                        liCopyTemporaryInstr.Add(instrAux);

                    }//for

                }//if

            }//if  

        }//btnCopyM2Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the copy selected Chord instructions button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnCopyChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iAux = 0;
            int iInstrIdx = 0;
            int iSongIdx = 0;

            // check if there is any theme selected and if the Chord channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // initialize and the temporary list of instructions                    
                    liCopyTemporaryInstr = new List<MChannelCodeEntry>();

                    // copy all the selected instructions in the temporary instructions list
                    for (iAux = 0; iAux < (int)liISelectionIdx.Count; iAux++) {

                        iInstrIdx = liISelectionIdx[iAux];
                        instrAux = new MChannelCodeEntry();
                        instrAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx].By0;
                        instrAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx].By1;
                        instrAux.By2 = "0x00";
                        instrAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx].strDescr;
                        liCopyTemporaryInstr.Add(instrAux);

                    }//for

                }//if

            }//if  

        }//btnCopyChordEntry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the M1 instructions Paste button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnPasteM1Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            int iThemeIdx = 0;
            int iInstrIdx = 0;
            int iAux = 0;
            MChannelCodeEntry melodyCodeEntryAux = null;
            string strAux = "";

            // check if there is any theme selected to add the instructions to
            if (dpack_drivePack.themes.iCurrThemeIdx < 0) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // check that the maximum number of allowed instructions will not be reached after pasting the copied instructions
                iAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count + liCopyTemporaryInstr.Count();
                if (iAux >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_NO_SPACE_FOR_INSTRUCTIONS;
                }

            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                if (liISelectionIdx.Count == 0) {

                    // if the channel does not contain any instruction or if there are no instrucions selected just paste the instructions at the end
                    iInstrIdx = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count;

                } else {

                    // if there are instructions selected get the lowest index of all selected rows and then paste the new instructions after it
                    iInstrIdx = liISelectionIdx[0];
                    iInstrIdx = iInstrIdx + 1;//iInstrIdx+1 to insert after speficied element

                }//if

                iAux = iInstrIdx;
                foreach (MChannelCodeEntry instrAux in liCopyTemporaryInstr) {

                    melodyCodeEntryAux = new MChannelCodeEntry();
                    melodyCodeEntryAux.By0 = instrAux.By0;
                    melodyCodeEntryAux.By1 = instrAux.By1;
                    melodyCodeEntryAux.By2 = instrAux.By2;
                    melodyCodeEntryAux.strDescr = instrAux.strDescr;

                    dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Insert(iAux, melodyCodeEntryAux);

                    iAux++;
                }

                // as we have added new elements in the list , update the index of all the instructions. As the index
                // have been ordered with .Sort the new index can be applied startig from the first index in the list
                for (iAux = iInstrIdx; iAux < dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count; iAux++) {
                    dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iAux].Idx = iAux;
                }

                // update the number of Melody 1 channel instructions to the new value
                lblMel1Ch.Text = "Melody 1 ch.code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count.ToString("D3") + "):";

                // use the idx stored at the begining of the method to keep selected the rows that have been moved
                themeM1DataGridView.ClearSelection();
                for (iAux = iInstrIdx; iAux < (iInstrIdx + liCopyTemporaryInstr.Count); iAux++) {
                    themeM1DataGridView.Rows[iAux].Selected = true;
                }

            }// if

            // show the corresponding message in the output logs
            if (ec_ret_val.i_code >= 0) {

                // informative message for the user 
                strAux = "Pasted " + liCopyTemporaryInstr.Count() + " instructions at position " + iInstrIdx + " in  theme's \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 1 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Error pasting " + liCopyTemporaryInstr.Count() + " instructions in theme's \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 1 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_EDITION + strAux, false);

            }//if

        }//btnPasteM1Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the M2 instructions Paste button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnPasteM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            int iThemeIdx = 0;
            int iInstrIdx = 0;
            int iAux = 0;
            MChannelCodeEntry melodyCodeEntryAux = null;
            string strAux = "";

            // check if there is any theme selected to add the instructions to
            if (dpack_drivePack.themes.iCurrThemeIdx < 0) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // check that the maximum number of allowed instructions will not be reached after pasting the copied instructions
                iAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count + liCopyTemporaryInstr.Count();
                if (iAux >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_NO_SPACE_FOR_INSTRUCTIONS;
                }

            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                if (liISelectionIdx.Count == 0) {

                    // if the channel does not contain any instruction or if there are no instrucions selected just paste the instructions at the end
                    iInstrIdx = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count;

                } else {

                    // if there are instructions selected get the lowest index of all selected rows and then paste the new instructions after it
                    iInstrIdx = liISelectionIdx[0];
                    iInstrIdx = iInstrIdx + 1;//iInstrIdx+1 to insert after speficied element

                }//if

                iAux = iInstrIdx;
                foreach (MChannelCodeEntry instrAux in liCopyTemporaryInstr) {

                    melodyCodeEntryAux = new MChannelCodeEntry();
                    melodyCodeEntryAux.By0 = instrAux.By0;
                    melodyCodeEntryAux.By1 = instrAux.By1;
                    melodyCodeEntryAux.By2 = instrAux.By2;
                    melodyCodeEntryAux.strDescr = instrAux.strDescr;

                    dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Insert(iAux, melodyCodeEntryAux);

                    iAux++;
                }

                // as we have added new elements in the list , update the index of all the instructions. As the index
                // have been ordered with .Sort the new index can be applied startig from the first index in the list
                for (iAux = iInstrIdx; iAux < dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count; iAux++) {
                    dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iAux].Idx = iAux;
                }

                // update the number of Melody 2 channel instructions to the new value
                lblMel2Ch.Text = "Melody 2 ch.code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count.ToString("D3") + "):";

                // use the idx stored at the begining of the method to keep selected the rows that have been moved
                themeM2DataGridView.ClearSelection();
                for (iAux = iInstrIdx; iAux < (iInstrIdx + liCopyTemporaryInstr.Count); iAux++) {
                    themeM2DataGridView.Rows[iAux].Selected = true;
                }

            }// if

            // show the corresponding message in the output logs
            if (ec_ret_val.i_code >= 0) {

                // informative message for the user 
                strAux = "Pasted " + liCopyTemporaryInstr.Count() + " instructions at position " + iInstrIdx + " in  theme's \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 2 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Error pasting " + liCopyTemporaryInstr.Count() + " instructions in theme's \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 2 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_EDITION + strAux, false);

            }//if

        }//btnPasteM2Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the Chord instructions Paste button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnPasteChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            int iThemeIdx = 0;
            int iInstrIdx = 0;
            int iAux = 0;
            ChordChannelCodeEntry chordCodeEntryAux = null;
            string strAux = "";

            // check if there is any theme selected to add the instructions to
            if (dpack_drivePack.themes.iCurrThemeIdx < 0) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // check that the maximum number of allowed instructions will not be reached after pasting the copied instructions
                iAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count + liCopyTemporaryInstr.Count();
                if (iAux >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_NO_SPACE_FOR_INSTRUCTIONS;
                }

            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                if (liISelectionIdx.Count == 0) {

                    // if the channel does not contain any instruction or if there are no instrucions selected just paste the instructions at the end
                    iInstrIdx = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count;

                } else {

                    // if there are instructions selected get the lowest index of all selected rows and then paste the new instructions after it
                    iInstrIdx = liISelectionIdx[0];
                    iInstrIdx = iInstrIdx + 1;//iInstrIdx+1 to insert after speficied element

                }//if

                iAux = iInstrIdx;
                foreach (MChannelCodeEntry instrAux in liCopyTemporaryInstr) {

                    chordCodeEntryAux = new ChordChannelCodeEntry();
                    chordCodeEntryAux.By0 = instrAux.By0;
                    chordCodeEntryAux.By1 = instrAux.By1;
                    chordCodeEntryAux.strDescr = instrAux.strDescr;

                    dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Insert(iAux, chordCodeEntryAux);

                    iAux++;
                }

                // as we have added new elements in the list , update the index of all the instructions. As the index
                // have been ordered with .Sort the new index can be applied startig from the first index in the list
                for (iAux = iInstrIdx; iAux < dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count; iAux++) {
                    dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iAux].Idx = iAux;
                }

                // update the number of chords channel instructions to the new value
                lblChordCh.Text = "Chords ch. code (" + dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count.ToString("D3") + "):";

                // use the idx stored at the begining of the method to keep selected the rows that have been moved
                themeChordDataGridView.ClearSelection();
                for (iAux = iInstrIdx; iAux < (iInstrIdx + liCopyTemporaryInstr.Count); iAux++) {
                    themeChordDataGridView.Rows[iAux].Selected = true;
                }

            }// if

            // show the corresponding message in the output logs
            if (ec_ret_val.i_code >= 0) {

                // informative message for the user 
                strAux = "Pasted " + liCopyTemporaryInstr.Count() + " instructions at position " + iInstrIdx + " in  theme's \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" chords channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Error pasting " + liCopyTemporaryInstr.Count() + " instructions in theme's \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" chords channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_EDITION + strAux, false);

            }//if

        }//btnPasteChordEntry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the increase 1 semitone the selected notes of
        * Melody 1 channel 
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnSustM1Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel M1 instruction with the specified Idx and increse it 1/2 tone
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {

                            // check if the instructions corresponds to a note and increase it 1/2 tone if afirmative

                            // NOTE COMMAND
                            // FIG. 10A-1
                            // 8421 
                            // ---- 
                            // ....  SC PITCH     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
                            // ....  OC           OC=4-bit octave code.    octave code for the melody line.
                            // ----  
                            // ....  L1 TONE      8-bit ON duration code
                            // ....  L2 DURATION
                            // ---- 
                            // ....  L1  REST     8-bit OFF duration code
                            // ....  L2  DURATION           
                            // ---- 
                            iAux = ( (instrAux.By0AsByte() & 0xf0) >> 4);
                            iAux2 = (instrAux.By0AsByte() & 0x0f);
                            if ((iAux >= 0x1) && (iAux <= 0xC) && (iAux2 >= 0x3) && (iAux2 <= 5)) {

                                switch (iAux) {

                                    case 0x1:// C
                                    case 0x2:// C#
                                    case 0x3:// D
                                    case 0x4:// D#
                                    case 0x5:// E
                                    case 0x6:// F
                                    case 0x7:// F#
                                    case 0x8:// G
                                    case 0x9:// G#
                                    case 0xA:// A
                                    case 0xB:// A#
                                        // increase the note half tone
                                        iAux = iAux + 1;
                                        break;

                                    case 0xC:// B                                        
                                        if (iAux2 < 5) {
                                            // start in the C of following octave
                                            iAux = 1;
                                            iAux2 = iAux2 + 1;
                                        } else {
                                            // if the highest octave has been reached then move back to the C in the lowest octave 
                                            iAux = 1;
                                            iAux2 = 3;
                                        }
                                        break;

                                    default:
                                        break;
                                }//if

                                // prepare the By 0 with the new note code and octave and update the instruction code
                                iAux = iAux << 4;
                                iAux = iAux | iAux2;
                                instrAux.By0 = iAux.ToString();
                                instrAux.Parse();

                            }//if

                        }

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM1DataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeM1DataGridView.Rows[idxSwapped].Selected = true;
                    }

                }//if

            }// if

        }//btnSustM1Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the increase 1 semitone the selected notes of
        * Melody 2 channel 
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnSustM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel M2 instruction with the specified Idx and increse it 1/2 tone
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {

                            // check if the instructions corresponds to a note and increase it 1/2 tone if afirmative

                            // NOTE COMMAND
                            // FIG. 10A-1
                            // 8421 
                            // ---- 
                            // ....  SC PITCH     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
                            // ....  OC           OC=4-bit octave code.    octave code for the melody line.
                            // ----  
                            // ....  L1 TONE      8-bit ON duration code
                            // ....  L2 DURATION
                            // ---- 
                            // ....  L1  REST     8-bit OFF duration code
                            // ....  L2  DURATION           
                            // ---- 
                            iAux = ((instrAux.By0AsByte() & 0xf0) >> 4);
                            iAux2 = (instrAux.By0AsByte() & 0x0f);
                            if ((iAux >= 0x1) && (iAux <= 0xC) && (iAux2 >= 0x3) && (iAux2 <= 5)) {

                                switch (iAux) {

                                    case 0x1:// C
                                    case 0x2:// C#
                                    case 0x3:// D
                                    case 0x4:// D#
                                    case 0x5:// E
                                    case 0x6:// F
                                    case 0x7:// F#
                                    case 0x8:// G
                                    case 0x9:// G#
                                    case 0xA:// A
                                    case 0xB:// A#
                                        // increase the note half tone
                                        iAux = iAux + 1;
                                        break;

                                    case 0xC:// B
                                        if (iAux2 < 5) {
                                            // start in the C of following octave
                                            iAux = 1;
                                            iAux2 = iAux2 + 1;
                                        } else {
                                            // if the highest octave has been reached then move back to the C in the lowest octave 
                                            iAux = 1;
                                            iAux2 = 3;
                                        }
                                        break;

                                    default:
                                        break;
                                }//if

                                // prepare the By 0 with the new note code and octave and update the instruction code
                                iAux = iAux << 4;
                                iAux = iAux | iAux2;
                                instrAux.By0 = iAux.ToString();
                                instrAux.Parse();

                            }//if

                        }

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM2DataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeM2DataGridView.Rows[idxSwapped].Selected = true;
                    }

                }//if

            }// if

        }//btnSustM2Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the increase 1 semitone the selected chords of
        * Chords channel 
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnSustChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ChordChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any theme selected and if the Chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel Chords instruction with the specified Idx and increse it 1/2 tone
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {

                            // check if the instructions corresponds to a note and increase it 1/2 tone if afirmative

                            // 8421 
                            // ---- 
                            // ....  SC ROOT     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
                            // ....  OC NAME     OC=4-bit octave code.    octave code for the melody line.
                            // ----  
                            // ....  L1 CHORD
                            // ....  L2 DURATION
                            // ---- 
                            iAux = ((instrAux.By0AsByte() & 0xf0) >> 4);
                            iAux2 = (instrAux.By0AsByte() & 0x0f);
                            if ((iAux >= 0x1) && (iAux <= 0xC)) {
                                switch (iAux) {

                                    case 0x1:// C
                                    case 0x2:// C#
                                    case 0x3:// D
                                    case 0x4:// D#
                                    case 0x5:// E
                                    case 0x6:// F
                                    case 0x7:// F#
                                    case 0x8:// G
                                    case 0x9:// G#
                                    case 0xA:// A
                                    case 0xB:// A#
                                        // increase the note half tone
                                        iAux = iAux + 1;
                                        break;

                                    case 0xC:// B
                                        // move back to C
                                         iAux = 1;
                                        break;

                                    default:
                                        break;
                                }//if

                                // prepare the By 0 with the new note code and octave and update the instruction code
                                iAux = iAux << 4;
                                iAux = iAux | iAux2;
                                instrAux.By0 = iAux.ToString();
                                instrAux.Parse();

                            }//if

                        }

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeChordDataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeChordDataGridView.Rows[idxSwapped].Selected = true;
                    }

                }//if

            }// if

        }//btnSustChordEntry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the decrease 1 semitone the selected notes of
        * Melody 1 channel 
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnBemolM1Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel M1 instruction with the specified Idx and decrease it 1/2 tone
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {

                            // check if the instructions corresponds to a note and decrease it 1/2 tone if afirmative

                            // NOTE COMMAND
                            // FIG. 10A-1
                            // 8421 
                            // ---- 
                            // ....  SC PITCH     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
                            // ....  OC           OC=4-bit octave code.    octave code for the melody line.
                            // ----  
                            // ....  L1 TONE      8-bit ON duration code
                            // ....  L2 DURATION
                            // ---- 
                            // ....  L1  REST     8-bit OFF duration code
                            // ....  L2  DURATION           
                            // ---- 
                            iAux = ((instrAux.By0AsByte() & 0xf0) >> 4);
                            iAux2 = (instrAux.By0AsByte() & 0x0f);
                            if ((iAux >= 0x1) && (iAux <= 0xC) && (iAux2 >= 0x3) && (iAux2 <= 5)) {

                                switch (iAux) {

                                    case 0x1:// C
                                        if (iAux2 > 3) {
                                            // start in the B of previous octave
                                            iAux = 0xC;
                                            iAux2 = iAux2 - 1;
                                        } else {
                                            // if the lowest octave has been reached then move back to the B of the highest octave 
                                            iAux = 0xC;
                                            iAux2 = 5;
                                        }
                                        break;

                                    case 0x2:// C#
                                    case 0x3:// D
                                    case 0x4:// D#
                                    case 0x5:// E
                                    case 0x6:// F
                                    case 0x7:// F#
                                    case 0x8:// G
                                    case 0x9:// G#
                                    case 0xA:// A
                                    case 0xB:// A#
                                    case 0xC:// B
                                        // decrease the note half tone
                                        iAux = iAux - 1;
                                        break;

                                    default:
                                        break;

                                }//if

                                // prepare the By 0 with the new note code and octave and update the instruction code
                                iAux = iAux << 4;
                                iAux = iAux | iAux2;
                                instrAux.By0 = iAux.ToString();
                                instrAux.Parse();

                            }//if

                        }

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM1DataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeM1DataGridView.Rows[idxSwapped].Selected = true;
                    }

                }//if

            }// if

        }//btnBemolM1Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the decrease 1 semitone the selected notes of
        * Melody 2 channel 
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnBemolM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel M2 instruction with the specified Idx and decrease it 1/2 tone
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {

                            // check if the instructions corresponds to a note and decrease it 1/2 tone if afirmative

                            // NOTE COMMAND
                            // FIG. 10A-1
                            // 8421 
                            // ---- 
                            // ....  SC PITCH     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
                            // ....  OC           OC=4-bit octave code.    octave code for the melody line.
                            // ----  
                            // ....  L1 TONE      8-bit ON duration code
                            // ....  L2 DURATION
                            // ---- 
                            // ....  L1  REST     8-bit OFF duration code
                            // ....  L2  DURATION           
                            // ---- 
                            iAux = ((instrAux.By0AsByte() & 0xf0) >> 4);
                            iAux2 = (instrAux.By0AsByte() & 0x0f);
                            if ((iAux >= 0x1) && (iAux <= 0xC) && (iAux2 >= 0x3) && (iAux2 <= 5)) {

                                switch (iAux) {

                                    case 0x1:// C
                                        if (iAux2 > 3) {
                                            // start in the B of previous octave
                                            iAux = 0xC;
                                            iAux2 = iAux2 - 1;
                                        } else {
                                            // if the lowest octave has been reached then move back to the B in the highest octave 
                                            iAux = 0xC;
                                            iAux2 = 5;
                                        }
                                        break;

                                    case 0x2:// C#
                                    case 0x3:// D
                                    case 0x4:// D#
                                    case 0x5:// E
                                    case 0x6:// F
                                    case 0x7:// F#
                                    case 0x8:// G
                                    case 0x9:// G#
                                    case 0xA:// A
                                    case 0xB:// A#
                                    case 0xC:// B
                                        // decrease the note half tone
                                        iAux = iAux - 1;
                                        break;

                                    default:
                                        break;

                                }//if

                                // prepare the By 0 with the new note code and octave and update the instruction code
                                iAux = iAux << 4;
                                iAux = iAux | iAux2;
                                instrAux.By0 = iAux.ToString();
                                instrAux.Parse();

                            }//if

                        }

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM2DataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeM2DataGridView.Rows[idxSwapped].Selected = true;
                    }

                }//if

            }// if

        }//btnBemolM2Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the decrease 1 semitone the selected chords of
        * Chords channel 
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnBemolMChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ChordChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any theme selected and if the Chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel Chords instruction with the specified Idx and decrease it 1/2 tone
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {

                            // check if the instructions corresponds to a note and decrease it 1/2 tone if afirmative

                            // 8421 
                            // ---- 
                            // ....  SC ROOT     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
                            // ....  OC NAME     OC=4-bit octave code.    octave code for the melody line.
                            // ----  
                            // ....  L1 CHORD
                            // ....  L2 DURATION
                            // ---- 
                            iAux = ((instrAux.By0AsByte() & 0xf0) >> 4);
                            iAux2 = (instrAux.By0AsByte() & 0x0f);
                            if ((iAux >= 0x1) && (iAux <= 0xC)) {
                                switch (iAux) {

                                    case 0x1:// C
                                        // move back to the 0xB
                                        iAux = 0xC;
                                        break;

                                    case 0x2:// C#
                                    case 0x3:// D
                                    case 0x4:// D#
                                    case 0x5:// E
                                    case 0x6:// F
                                    case 0x7:// F#
                                    case 0x8:// G
                                    case 0x9:// G#
                                    case 0xA:// A
                                    case 0xB:// A#
                                    case 0xC:// B
                                        // decrease the note half tone
                                        iAux = iAux - 1;
                                        break;

                                    default:
                                        break;

                                }//if

                                // prepare the By 0 with the new note code and octave and update the instruction code
                                iAux = iAux << 4;
                                iAux = iAux | iAux2;
                                instrAux.By0 = iAux.ToString();
                                instrAux.Parse();

                            }//if

                        }

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeChordDataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeChordDataGridView.Rows[idxSwapped].Selected = true;
                    }

                }//if

            }// if

        }//btnBemolMChordEntry_Click

    }// public partial class MainForm

}//namespace drivepacked
