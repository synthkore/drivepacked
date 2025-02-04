using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static drivePackEd.ChordChannelCodeEntry;
using static drivePackEd.MChannelCodeEntry;

namespace drivePackEd {

    partial class dummy {/* this dummy class has been added only to overcome the issue with Visual Studio IDE that adds a new .resx file when the user clicks on any Form partial class file */ };

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
            byte by0 = 0;
            byte by1 = 0;
            byte by2 = 0;
            string strAux = "";

            // check if there is any theme selected to add the instructions to
            if (dpack_drivePack.themes.iCurrThemeIdx < 0) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }
            
            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // check that the maximum number of allowed instructions on the channel has not been reached yet
                if (dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_TOO_MUCH_INSTRUCTIONS;
                }

            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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
                GetM1ConfiguredCommand(ref melodyCodeEntryAux);
                melodyCodeEntryAux.Parse();
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

                dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                storeSelectedDGridViewRows();

                // store current application state into history stack to allow recovering it with Ctrl+Z
                historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                // informative message for the user 
                strAux = "Added an instruction at position " + iInstrIdx + " in the theme \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 1 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Could not add the instruction in the melody 1 channel.";
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
            byte by0 = 0;
            byte by1 = 0;
            byte by2 = 0;
            string strAux = "";

            // check if there is any theme selected to add the instructions to
            if (dpack_drivePack.themes.iCurrThemeIdx < 0) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // check that the maximum number of allowed instructions on the channel has not been reached yet
                if (dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_TOO_MUCH_INSTRUCTIONS;
                }

            }//if

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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
                GetM2ConfiguredCommand(ref melodyCodeEntryAux);
                melodyCodeEntryAux.Parse();
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

                dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                storeSelectedDGridViewRows();

                // store current application state into history stack to allow recovering it with Ctrl+Z
                historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                // informative message for the user 
                strAux = "Added an instruction at position " + iInstrIdx + " in the theme \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 2 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Could not add the instruction in the melody 2 channel.";
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
            byte by0 = 0;
            byte by1 = 0;
            string strAux = "";

            // check if there is any theme selected to add the instructions to
            if (dpack_drivePack.themes.iCurrThemeIdx < 0){
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // check that the maximum number of allowed instructions on the channel has not been reached yet
                if (dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_TOO_MUCH_INSTRUCTIONS;
                }

            }//if

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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
                GetChordConfiguredCommand(ref chordCodeEntryAux);
                chordCodeEntryAux.Parse();
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

                dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                storeSelectedDGridViewRows();

                // store current application state into history stack to allow recovering it with Ctrl+Z
                historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                // informative message for the user 
                strAux = "Added an instruction at position " + iInstrIdx + " in the theme \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" chords channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Could not add the instruction in the chords channel.";
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

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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

                // update the different dataGridView rows selection lists with the current dataGridView selected rows. This is done before
                // executing the modifications in order to restore that selected rows in case that the user Undoes the following modifications
                storeSelectedDGridViewRows();
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 


                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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
            int iThemeIdx = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

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
                        melodyCodeEntryAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By0;
                        melodyCodeEntryAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By1;
                        melodyCodeEntryAux.By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By2;
                        melodyCodeEntryAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].StrDescr;

                        // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By0;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By1;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By2;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].StrDescr;

                        // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By0 = melodyCodeEntryAux.By0;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By1 = melodyCodeEntryAux.By1;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By2 = melodyCodeEntryAux.By2;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].StrDescr = melodyCodeEntryAux.StrDescr;

                        iAux2--;

                    }//for (iAux=0;

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    themeM1DataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeM1DataGridView.Rows[idxSwapped].Selected = true;
                    }

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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
            int iThemeIdx = 0;


            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

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
                        melodyCodeEntryAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By0;
                        melodyCodeEntryAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By1;
                        melodyCodeEntryAux.By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By2;
                        melodyCodeEntryAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].StrDescr;

                        // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By0;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By1;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By2;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].StrDescr;

                        // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By0 = melodyCodeEntryAux.By0;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By1 = melodyCodeEntryAux.By1;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By2 = melodyCodeEntryAux.By2;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].StrDescr = melodyCodeEntryAux.StrDescr;

                        iAux2--;

                    }//for (iAux=0;

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    themeM2DataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeM2DataGridView.Rows[idxSwapped].Selected = true;
                    }

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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
            int iThemeIdx = 0;

            // check if there is any theme selected and if the Chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

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
                        instrAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By0;
                        instrAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By1;
                        instrAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].StrDescr;

                        // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By0;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By1;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].StrDescr;

                        // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By0 = instrAux.By0;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By1 = instrAux.By1;
                        dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].StrDescr = instrAux.StrDescr;

                        iAux2--;

                    }//for (iAux=0;

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    themeChordDataGridView.ClearSelection();
                    foreach (int idxSwapped in liISelectionIdx) {
                        themeChordDataGridView.Rows[idxSwapped].Selected = true;
                    }

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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
            int iThemeIdx = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

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
                            instrAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By0;
                            instrAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By1;
                            instrAux.By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By2;
                            instrAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].StrDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By2;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].StrDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By0 = instrAux.By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By1 = instrAux.By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By2 = instrAux.By2;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].StrDescr = instrAux.StrDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;


                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeM1DataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeM1DataGridView.Rows[idxInstruction - 1].Selected = true;
                        }

                        dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                        // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                        // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                        storeSelectedDGridViewRows();

                        // store current application state into history stack to allow recovering it with Ctrl+Z
                        historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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
            int iThemeIdx = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

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
                            instrAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By0;
                            instrAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By1;
                            instrAux.By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By2;
                            instrAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].StrDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By2;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].StrDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By0 = instrAux.By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By1 = instrAux.By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By2 = instrAux.By2;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].StrDescr = instrAux.StrDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeM2DataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeM2DataGridView.Rows[idxInstruction - 1].Selected = true;
                        }

                        dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                        // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                        // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                        storeSelectedDGridViewRows();

                        // store current application state into history stack to allow recovering it with Ctrl+Z
                        historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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
            int iThemeIdx = 0;

            // check if there is any theme selected and if the Chord channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

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
                            instrAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By0;
                            instrAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By1;
                            instrAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].StrDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].StrDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By0 = instrAux.By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By1 = instrAux.By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].StrDescr = instrAux.StrDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeChordDataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeChordDataGridView.Rows[idxInstruction - 1].Selected = true;
                        }

                        dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                        // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                        // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                        storeSelectedDGridViewRows();

                        // store current application state into history stack to allow recovering it with Ctrl+Z
                        historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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
            int iThemeIdx = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

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
                    if (iInstrIdx1 < (dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count - 1)) {

                        iInstrIdx1 = iInstrIdx1 + 1;

                        for (iAux = (int)(liISelectionIdx.Count - 1); iAux >= 0; iAux--) {

                            iInstrIdx2 = liISelectionIdx[iAux];

                            // swap the content of the rows less the Idx:
                            // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                            melodyCodeEntryAux = new MChannelCodeEntry();
                            melodyCodeEntryAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By0;
                            melodyCodeEntryAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By1;
                            melodyCodeEntryAux.By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By2;
                            melodyCodeEntryAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].StrDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By2;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx1].StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].StrDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By0 = melodyCodeEntryAux.By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By1 = melodyCodeEntryAux.By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].By2 = melodyCodeEntryAux.By2;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx2].StrDescr = melodyCodeEntryAux.StrDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // use the idx stored at the begining of the method to keep selected the rows that have been moved
                        themeM1DataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeM1DataGridView.Rows[idxInstruction + 1].Selected = true; ;
                        }

                        dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                        // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                        // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                        storeSelectedDGridViewRows();

                        // store current application state into history stack to allow recovering it with Ctrl+Z
                        historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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
            int iThemeIdx = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

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
                    if (iInstrIdx1 < (dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count - 1)) {

                        iInstrIdx1 = iInstrIdx1 + 1;

                        for (iAux = (int)(liISelectionIdx.Count - 1); iAux >= 0; iAux--) {

                            iInstrIdx2 = liISelectionIdx[iAux];

                            // swap the content of the rows less the Idx:
                            // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                            melodyCodeEntryAux = new MChannelCodeEntry();
                            melodyCodeEntryAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By0;
                            melodyCodeEntryAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By1;
                            melodyCodeEntryAux.By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By2;
                            melodyCodeEntryAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].StrDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By2;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx1].StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].StrDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By0 = melodyCodeEntryAux.By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By1 = melodyCodeEntryAux.By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].By2 = melodyCodeEntryAux.By2;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx2].StrDescr = melodyCodeEntryAux.StrDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // use the idx stored at the begining of the method to keep selected the rows that have been moved
                        themeM2DataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeM2DataGridView.Rows[idxInstruction + 1].Selected = true;
                        }

                        dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                        // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                        // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                        storeSelectedDGridViewRows();

                        // store current application state into history stack to allow recovering it with Ctrl+Z
                        historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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
            int iThemeIdx = 0;

            // check if there is any theme selected and if the Chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

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
                    if (iInstrIdx1 < (dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count - 1)) {

                        iInstrIdx1 = iInstrIdx1 + 1;

                        for (iAux = (int)(liISelectionIdx.Count - 1); iAux >= 0; iAux--) {

                            iInstrIdx2 = liISelectionIdx[iAux];

                            // swap the content of the rows less the Idx:
                            // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                            instrAux = new ChordChannelCodeEntry();
                            instrAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By0;
                            instrAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By1;
                            instrAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].StrDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx1].StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].StrDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By0 = instrAux.By0;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].By1 = instrAux.By1;
                            dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx2].StrDescr = instrAux.StrDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // use the idx stored at the begining of the method to keep selected the rows that have been moved
                        themeChordDataGridView.ClearSelection();
                        foreach (int idxInstruction in liISelectionIdx) {
                            themeChordDataGridView.Rows[idxInstruction + 1].Selected = true; ;
                        }

                        dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                        // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                        // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                        storeSelectedDGridViewRows();

                        // store current application state into history stack to allow recovering it with Ctrl+Z
                        historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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
            int iThemeIdx = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM1DataGridView.Rows.Count <= 0)) {
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

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // initialize and the temporary list of instructions                    
                    liCopyMelodyTemporaryInstr = new List<MChannelCodeEntry>();

                    // copy all the selected instructions in the temporary instructions list
                    for (iAux = 0; iAux < (int)liISelectionIdx.Count; iAux++) {

                        iInstrIdx = liISelectionIdx[iAux];
                        instrAux = new MChannelCodeEntry();
                        instrAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx].By0;
                        instrAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx].By1;
                        instrAux.By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx].By2;
                        instrAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr[iInstrIdx].StrDescr;
                        liCopyMelodyTemporaryInstr.Add(instrAux);

                    }//for

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

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
            int iThemeIdx = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM2DataGridView.Rows.Count <= 0)) {
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

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // initialize and the temporary list of instructions                    
                    liCopyMelodyTemporaryInstr = new List<MChannelCodeEntry>();

                    // copy all the selected instructions in the temporary instructions list
                    for (iAux = 0; iAux < (int)liISelectionIdx.Count; iAux++) {

                        iInstrIdx = liISelectionIdx[iAux];
                        instrAux = new MChannelCodeEntry();
                        instrAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx].By0;
                        instrAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx].By1;
                        instrAux.By2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx].By2;
                        instrAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr[iInstrIdx].StrDescr;
                        liCopyMelodyTemporaryInstr.Add(instrAux);

                    }//for

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

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
            ChordChannelCodeEntry instrAux = null;
            int iAux = 0;
            int iInstrIdx = 0;
            int iThemeIdx = 0;

            // check if there is any theme selected and if the Chord channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeChordDataGridView.Rows.Count <= 0)) {
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

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // initialize and the temporary list of instructions                    
                    liCopyChordTemporaryInstr = new List<ChordChannelCodeEntry>();

                    // copy all the selected instructions in the temporary instructions list
                    for (iAux = 0; iAux < (int)liISelectionIdx.Count; iAux++) {

                        iInstrIdx = liISelectionIdx[iAux];
                        instrAux = new ChordChannelCodeEntry();
                        instrAux.By0 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx].By0;
                        instrAux.By1 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx].By1;
                        instrAux.StrDescr = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr[iInstrIdx].StrDescr;
                        liCopyChordTemporaryInstr.Add(instrAux);

                    }//for

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

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

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // check that the maximum number of allowed instructions will not be reached after pasting the copied instructions
                iAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.Count + liCopyMelodyTemporaryInstr.Count();
                if (iAux >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_TOO_MUCH_INSTRUCTIONS;
                }

            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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
                foreach (MChannelCodeEntry instrAux in liCopyMelodyTemporaryInstr) {

                    melodyCodeEntryAux = new MChannelCodeEntry();
                    melodyCodeEntryAux.By0 = instrAux.By0;
                    melodyCodeEntryAux.By1 = instrAux.By1;
                    melodyCodeEntryAux.By2 = instrAux.By2;
                    melodyCodeEntryAux.StrDescr = instrAux.StrDescr;

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
                for (iAux = iInstrIdx; iAux < (iInstrIdx + liCopyMelodyTemporaryInstr.Count); iAux++) {
                    themeM1DataGridView.Rows[iAux].Selected = true;
                }

            }// if

            // show the corresponding message in the output logs
            if (ec_ret_val.i_code >= 0) {

                dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                storeSelectedDGridViewRows();

                // store current application state into history stack to allow recovering it with Ctrl+Z
                historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                // informative message for the user 
                strAux = "Pasted " + liCopyMelodyTemporaryInstr.Count() + " instructions at position " + iInstrIdx + " in  theme's \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 1 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Could not paste the instruction in the M1 channel.";
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

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // check that the maximum number of allowed instructions will not be reached after pasting the copied instructions
                iAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.Count + liCopyMelodyTemporaryInstr.Count();
                if (iAux >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_TOO_MUCH_INSTRUCTIONS;
                }

            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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
                foreach (MChannelCodeEntry instrAux in liCopyMelodyTemporaryInstr) {

                    melodyCodeEntryAux = new MChannelCodeEntry();
                    melodyCodeEntryAux.By0 = instrAux.By0;
                    melodyCodeEntryAux.By1 = instrAux.By1;
                    melodyCodeEntryAux.By2 = instrAux.By2;
                    melodyCodeEntryAux.StrDescr = instrAux.StrDescr;

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
                for (iAux = iInstrIdx; iAux < (iInstrIdx + liCopyMelodyTemporaryInstr.Count); iAux++) {
                    themeM2DataGridView.Rows[iAux].Selected = true;
                }

            }// if

            // show the corresponding message in the output logs
            if (ec_ret_val.i_code >= 0) {

                dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                storeSelectedDGridViewRows();

                // store current application state into history stack to allow recovering it with Ctrl+Z
                historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                // informative message for the user 
                strAux = "Pasted " + liCopyMelodyTemporaryInstr.Count() + " instructions at position " + iInstrIdx + " in  theme's \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" melody 2 channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Could not paste the instruction in the M2 channel.";
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

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // check that the maximum number of allowed instructions will not be reached after pasting the copied instructions
                iAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.Count + liCopyChordTemporaryInstr.Count();
                if (iAux >= Themes.MAX_INSTRUCTIONS_CHANNEL) {
                    ec_ret_val = cErrCodes.ERR_EDITION_TOO_MUCH_INSTRUCTIONS;
                }

            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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
                foreach (ChordChannelCodeEntry instrAux in liCopyChordTemporaryInstr) {

                    chordCodeEntryAux = new ChordChannelCodeEntry();
                    chordCodeEntryAux.By0 = instrAux.By0;
                    chordCodeEntryAux.By1 = instrAux.By1;
                    chordCodeEntryAux.StrDescr = instrAux.StrDescr;

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
                for (iAux = iInstrIdx; iAux < (iInstrIdx + liCopyChordTemporaryInstr.Count); iAux++) {
                    themeChordDataGridView.Rows[iAux].Selected = true;
                }

            }// if

            // show the corresponding message in the output logs
            if (ec_ret_val.i_code >= 0) {

                dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                storeSelectedDGridViewRows();

                // store current application state into history stack to allow recovering it with Ctrl+Z
                historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                // informative message for the user 
                strAux = "Pasted " + liCopyChordTemporaryInstr.Count() + " instructions at position " + iInstrIdx + " in  theme's \"" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title + "\" chords channel.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + strAux, false);

            } else {

                // informative message for the user 
                strAux = "Could not paste the instruction in the Chords channel.";
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

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

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

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if

            }// if

        }//btnBemolMChordEntry_Click
        
        /*******************************************************************************
        * @brief Delegate that processes the event when the user clicks on the button to 
        * apply the configured instruction over the selected rows in the M1 channel
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnEditM1Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            MChannelCodeEntry instrAux2 = null;
            int iThemeIdx = 0;

            // check if there is any theme selected and if the Melody 1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // first get the bytes of the current configured instruction
                instrAux = new MChannelCodeEntry();
                ec_ret_val = GetM1ConfiguredCommand(ref instrAux);

            }//if

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel M1 instruction with the specified Idx 
                        instrAux2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {

                            // set in the selected instructions the bytes of the current configured instruction in the instruction edition controls
                            instrAux2.By0 = instrAux.By0;
                            instrAux2.By1 = instrAux.By1;
                            instrAux2.By2 = instrAux.By2;
                            instrAux2.Parse();

                        }//if

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM1DataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeM1DataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if            

            }//if

        }//btnEditM1Entry_Click

        /*******************************************************************************
         * @brief Delegate that processes the event when the user clicks on the button to 
         * apply the configured instruction over the selected rows in the M2 channel
         * @param[in] sender reference to the object that raises the event
         * @param[in] e the information related to the event
         *******************************************************************************/
        private void btnEditM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            MChannelCodeEntry instrAux2 = null;
            int iThemeIdx = 0;

            // check if there is any theme selected and if the Melody 1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // first get the bytes of the current configured instruction
                instrAux = new MChannelCodeEntry();
                ec_ret_val = GetM2ConfiguredCommand(ref instrAux);

            }//if

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel M2 instruction with the specified Idx and decrease it 1/2 tone
                        instrAux2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {

                            instrAux2.By0 = instrAux.By0;
                            instrAux2.By1 = instrAux.By1;
                            instrAux2.By2 = instrAux.By2;
                            instrAux2.Parse();

                        }//if

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM2DataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeM2DataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if            

            }//if

        }//btnEditM2Entry_Click

        /*******************************************************************************
        * @brief Delegate that processes the event when the user clicks on the button to 
        * apply the configured instruction over the selected rows in the chord channel
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnEditChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ChordChannelCodeEntry instrAux = null;
            ChordChannelCodeEntry instrAux2 = null;
            int iThemeIdx = 0;

            // check if there is any theme selected and if the Chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // first get the bytes of the current configured instruction
                instrAux = new ChordChannelCodeEntry();
                ec_ret_val = GetChordConfiguredCommand(ref instrAux);

            }//if

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                // first check that there is at least 1 row selected
                if (liISelectionIdx.Count > 0) {

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel chord instruction with the specified Idx and decrease it 1/2 tone
                        instrAux2 = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux2 != null) {

                            instrAux2.By0 = instrAux.By0;
                            instrAux2.By1 = instrAux.By1;
                            instrAux2.Parse();

                        }//if

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeChordDataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeChordDataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if            

            }//if

        }//btnEditChordEntry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the Parse button to get all the information 
        * of the instructions in the Melody 1 channel.
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnParseM1Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                        // find each channel M1 instruction with the specified Idx and parse its bytes
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {
                            instrAux.Parse();
                        }

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM1DataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeM1DataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if

            }// if
        
        }//btnParseM1Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the Parse button to get all the information 
        * of the instructions in the Melody 2 channel.
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnParseM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            MChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                        // find each channel M2 instruction with the specified Idx and parse its bytes
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {
                            instrAux.Parse();
                        }

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM2DataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeM2DataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if

            }// if

        }//btnParseM2Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the Parse button to get all the information 
        * of the instructions in the Chords channel.
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnParseChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ChordChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;

            // check if there is any theme selected and if the Chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

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

                        // find each channel Chords instruction with the specified Idx and parse its bytes
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {
                            instrAux.Parse();
                        }//if

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeChordDataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeChordDataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if

            }// if
        
        }//btnParseChordEntry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the "Measure selected instructions lenght" 
        * button. This button allows to get the total Note and Rest duration of the 
        * selected notes in the M1 instructions dataGridView.
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnLengthM1Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            List<MChannelCodeEntry> liMChanCodeEntryAux = null;
            MChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iProcInstrCtr = 0;
            int iTotalRestDuration = 0;
            int iTotalNoteDuration = 0;
            int iTotalDuration = 0;
            string strAux = "";
            double dAux = 0;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the instructions selected in the dataGridView and then sort them
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // find each channel M1 instruction with the selected Idx values and store them in a temporary
                    // list in order to send it to the routine that calculates the Note and Rest duration values
                    liMChanCodeEntryAux = new List<MChannelCodeEntry>();
                    foreach (int iInstrIdx in liISelectionIdx) {

                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.First(p => p.Idx == iInstrIdx);
                        if (instrAux != null) {
                            liMChanCodeEntryAux.Add(instrAux);
                        }

                    }//foreach

                    // calculate the total Note and Rest duration of the notes in the list
                    ThemeCode.CalculateMelodyInstructionsDuration(liMChanCodeEntryAux, ref iProcInstrCtr, ref iTotalNoteDuration, ref iTotalRestDuration);

                    // show the message with the duration calculations over the selected instructions
                    iTotalDuration = iTotalNoteDuration + iTotalRestDuration;
                    strAux = "The calculated duration of M1 channel selected instructions is:\r\n";
                    strAux = strAux + " - Processed instructions:" + iProcInstrCtr.ToString() + "\r\n";
                    dAux = (((double)iTotalNoteDuration) / 24.0);
                    strAux = strAux + " - Total note duration:" + iTotalNoteDuration.ToString() + " (" + dAux.ToString("0.00") + " quarter notes )\r\n";
                    dAux = (((double)iTotalRestDuration) / 24.0);
                    strAux = strAux + " - Total rest duration:" + iTotalRestDuration.ToString() + " (" + dAux.ToString("0.00") + " quarter notes )\r\n";
                    dAux = (((double)iTotalDuration) / 24.0);
                    strAux = strAux + " - Total note and rest duration:" + iTotalDuration.ToString() + " (" + dAux.ToString("0.00") + " quarter notes )\r\n";
                    iAux = (iTotalDuration & 0x0000FF00) >> 8;
                    iAux2 = (iTotalDuration & 0x000000FF);
                    strAux = strAux + "  " + iTotalDuration.ToString() + " = 0x" + iTotalDuration.ToString("X4") + " U:" + iAux.ToString() + "=0x" + iAux.ToString("X2") + " L:" + iAux2.ToString() + "=0x" + iAux2.ToString("X2") + "\r\n\r\n";
                    strAux = strAux + " Repeat instructions are not conisdered!\r\n";
                    MessageBox.Show(strAux, "Calculated durations");

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM1DataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeM1DataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if

            }// if

        }//btnLengthM1Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the "Measure selected instructions lenght" 
        * button. This button allows to get the total Note and Rest duration of the 
        * selected notes in the M2 instructions dataGridView.
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnLengthM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            List<MChannelCodeEntry> liMChanCodeEntryAux = null;
            MChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iProcInstrCtr = 0;
            int iTotalRestDuration = 0;
            int iTotalNoteDuration = 0;
            int iTotalDuration = 0;
            string strAux = "";
            double dAux = 0;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the instructions selected in the dataGridView and then sort them
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // find each channel M2 instruction with the selected Idx values and store them in a temporary
                    // list in order to send it to the routine that calculates the Note and Rest duration values
                    liMChanCodeEntryAux = new List<MChannelCodeEntry>();
                    foreach (int iInstrIdx in liISelectionIdx) {
                                                
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.First(p => p.Idx == iInstrIdx);
                        if (instrAux!= null) {
                            liMChanCodeEntryAux.Add(instrAux);
                        }

                    }//foreach

                    // calculate the total Note and Rest duration of the notes in the list
                    ThemeCode.CalculateMelodyInstructionsDuration(liMChanCodeEntryAux, ref iProcInstrCtr, ref iTotalNoteDuration, ref iTotalRestDuration);

                    // show the message with the duration calculations over the selected instructions
                    iTotalDuration = iTotalNoteDuration + iTotalRestDuration;
                    strAux = "The calculated duration of M2 channel selected instructions is:\r\n";
                    strAux = strAux + " - Processed instructions:" + iProcInstrCtr.ToString() + "\r\n";
                    dAux = (((double)iTotalNoteDuration) / 24.0);
                    strAux = strAux + " - Total note duration:" + iTotalNoteDuration.ToString() + " (" + dAux.ToString("0.00") + " quarter notes )\r\n";
                    dAux = (((double)iTotalRestDuration) / 24.0);
                    strAux = strAux + " - Total rest duration:" + iTotalRestDuration.ToString() + " (" + dAux.ToString("0.00") + " quarter notes )\r\n";
                    dAux = (((double)iTotalDuration) / 24.0);
                    strAux = strAux + " - Total note and rest duration:" + iTotalDuration.ToString() + " (" + dAux.ToString("0.00") + " quarter notes )\r\n";
                    iAux = (iTotalDuration & 0x0000FF00) >> 8;
                    iAux2 = (iTotalDuration & 0x000000FF);
                    strAux = strAux + "  " + iTotalDuration.ToString() + " = 0x" + iTotalDuration.ToString("X4") + " U:" + iAux.ToString() + "=0x" + iAux.ToString("X2") + " L:" + iAux2.ToString() + "=0x" + iAux2.ToString("X2") + "\r\n\r\n";
                    strAux = strAux + " Repeat instructions are not conisdered!\r\n";
                    MessageBox.Show(strAux, "Calculated durations");

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM2DataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeM2DataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if

            }// if

        }//btnLengthM2Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the "Measure selected instructions lenght" 
        * button. This button allows to get the total Note and Rest duration of the 
        * selected notes in the M2 instructions dataGridView.
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnLengthChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            List<ChordChannelCodeEntry> liChordChanCodeEntryAux = null;
            ChordChannelCodeEntry instrAux = null;
            int iThemeIdx = 0;
            int iProcInstrCtr = 0;
            int iTotalRestDuration = 0;
            int iTotalChordDuration = 0;
            int iTotalDuration = 0;
            string strAux = "";
            double dAux = 0;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the instructions selected in the dataGridView and then sort them
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    // find each channel Chords instruction with the selected Idx values and store them in a temporary
                    // list in order to send it to the routine that calculates the Note and Rest duration values
                    liChordChanCodeEntryAux = new List<ChordChannelCodeEntry>();
                    foreach (int iInstrIdx in liISelectionIdx) {

                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.First(p => p.Idx == iInstrIdx);
                        if (instrAux != null) {
                            liChordChanCodeEntryAux.Add(instrAux);
                        }

                    }//foreach

                    // calculate the total Note and Rest duration of the notes in the list
                    ThemeCode.CalculateChordInstructionsDuration(liChordChanCodeEntryAux, ref iProcInstrCtr, ref iTotalChordDuration, ref iTotalRestDuration);

                    // show the message with the duration calculations over the selected instructions
                    iTotalDuration = iTotalChordDuration + iTotalRestDuration;
                    strAux = "The calculated duration of Chords channel selected instructions is:\r\n";
                    strAux = strAux + " - Processed instructions:" + iProcInstrCtr.ToString() + "\r\n";
                    dAux = (((double)iTotalChordDuration) / 24.0);
                    strAux = strAux + " - Total note duration: " + iTotalChordDuration.ToString() + " (" + dAux.ToString("0.00") + " quarter notes )\r\n";
                    dAux = (((double)iTotalRestDuration) / 24.0);
                    strAux = strAux + " - Total rest duration: " + iTotalRestDuration.ToString() + " (" + dAux.ToString("0.00") + " quarter notes )\r\n";
                    dAux = (((double)iTotalDuration) / 24.0);
                    strAux = strAux + " - Total note and rest duration: " + iTotalDuration.ToString() + " (" + dAux.ToString("0.00") + " quarter notes )\r\n";
                    iAux = (iTotalDuration & 0x0000FF00) >> 8;
                    iAux2 = (iTotalDuration & 0x000000FF);
                    strAux = strAux + "  " + iTotalDuration.ToString() + " = 0x" + iTotalDuration.ToString("X4") + " U:" + iAux.ToString() + "=0x" + iAux.ToString("X2") + " L:" + iAux2.ToString() + "=0x" + iAux2.ToString("X2") + "\r\n\r\n";
                    strAux = strAux + " Repeat instructions are not conisdered!\r\n";
                    MessageBox.Show(strAux, "Calculated durations");

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeChordDataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeChordDataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if

            }// if

        }//btnLengthChordEntry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the Melody 1 channel "Multiply selected 
        * instructions duration and rest time" button. This button allows to increase or 
        * decrease the duration and the rest time of the Melody 1 channel selected 
        * instructions by multiplying them by the corresponding factor.
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnMultM1Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            int iThemeIdx = 0;
            MChannelCodeEntry instrAux = null;
            MChannelCodeEntry.t_Instrument tInstrOutAux = MChannelCodeEntry.t_Instrument.PIANO;
            MChannelCodeEntry.t_On_Off tOnOffOutAux = MChannelCodeEntry.t_On_Off.ON;
            MChannelCodeEntry.t_Effect tEffectAux = MChannelCodeEntry.t_Effect.VIBRATO;
            MChannelCodeEntry.t_Notes tNoteAux = MChannelCodeEntry.t_Notes.C4;
            int iNoteDurOut = 0;
            int iRestDurOut = 0;
            string strAux = "";
            double dAux = 0;
            double dMultFactor = 0;
            byte by0 = 0;
            byte by1 = 0;
            byte by2 = 0;

            // check if there is any theme selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM1DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M1_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    dMultFactor = (double)nUpDwMultM1Entry.Value;

                    // process each row in the selection
                     foreach (int instrIdx in liISelectionIdx) {

                        // find each channel M1 instruction with the specified Idx and parse its bytes
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM1CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {

                            switch (instrAux.GetCmdType()) {

                                case MChannelCodeEntry.t_Command.TIMBRE_INSTRUMENT:
                                    // get the current note and rest duation instruction parameters, multiply them by the configured
                                    // factor and update the instructions parameters with the new calculated values
                                    instrAux.GetInstrumentCommandParams( ref tInstrOutAux, ref tOnOffOutAux, ref iRestDurOut);
                                    iRestDurOut = (int)(iRestDurOut * dMultFactor);
                                    instrAux.SetInstrumentCommandParams(tInstrOutAux, tOnOffOutAux, iRestDurOut);
                                    instrAux.Parse();
                                    break;

                                case MChannelCodeEntry.t_Command.EFFECT:
                                    instrAux.GetEffectCommandParams( ref tEffectAux, ref tOnOffOutAux, ref iRestDurOut);
                                    iRestDurOut = (int)(iRestDurOut * dMultFactor);
                                    instrAux.SetEffectCommandParams(tEffectAux,tOnOffOutAux,iRestDurOut);
                                    instrAux.Parse();
                                    break;

                                case MChannelCodeEntry.t_Command.REST_DURATION:
                                    instrAux.GetRestCommandParams(ref iRestDurOut);
                                    iRestDurOut = (int)(iRestDurOut * dMultFactor);
                                    instrAux.SetRestCommandParams(iRestDurOut);
                                    instrAux.Parse();
                                    break;

                                case MChannelCodeEntry.t_Command.NOTE:
                                    instrAux.GetNoteCommandParams(ref tNoteAux, ref iNoteDurOut, ref iRestDurOut);
                                    iNoteDurOut = (int)(iNoteDurOut * dMultFactor);
                                    iRestDurOut = (int)(iRestDurOut * dMultFactor);
                                    instrAux.SetNoteCommandParams(tNoteAux, iNoteDurOut, iRestDurOut);
                                    instrAux.Parse();
                                    break;

                                case MChannelCodeEntry.t_Command.DURATIONx2:
                                    break;

                            }//switch

                        }//if

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM1DataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeM1DataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if

            }// if

        }//btnMultM1Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the Melody 2 channel "Multiply selected 
        * instructions duration and rest time" button. This button allows to increase or 
        * decrease the duration and the rest time of the Melody 2 channel selected 
        * instructions by multiplying them by the corresponding factor.
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnMultM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            int iThemeIdx = 0;
            MChannelCodeEntry instrAux = null;
            MChannelCodeEntry.t_Instrument tInstrOutAux = MChannelCodeEntry.t_Instrument.PIANO;
            MChannelCodeEntry.t_On_Off tOnOffOutAux = MChannelCodeEntry.t_On_Off.ON;
            MChannelCodeEntry.t_Effect tEffectAux = MChannelCodeEntry.t_Effect.VIBRATO;
            MChannelCodeEntry.t_Notes tNoteAux = MChannelCodeEntry.t_Notes.C4;
            int iNoteDurOut = 0;
            int iRestDurOut = 0;
            string strAux = "";
            double dAux = 0;
            double dMultFactor = 0;
            byte by0 = 0;
            byte by1 = 0;
            byte by2 = 0;

            // check if there is any theme selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeM2DataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_M2_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    dMultFactor = (double)nUpDwMultM2Entry.Value;

                    // process each row in the selection
                     foreach (int instrIdx in liISelectionIdx) {

                        // find each channel M2 instruction with the specified Idx and parse its bytes
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liM2CodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {

                            switch (instrAux.GetCmdType()) {

                                case MChannelCodeEntry.t_Command.TIMBRE_INSTRUMENT:
                                    // get the current note and rest duation instruction parameters, multiply them by the configured
                                    // factor and update the instructions parameters with the new calculated values
                                    instrAux.GetInstrumentCommandParams(ref tInstrOutAux, ref tOnOffOutAux, ref iRestDurOut);
                                    iRestDurOut = (int)(iRestDurOut * dMultFactor);
                                    instrAux.SetInstrumentCommandParams(tInstrOutAux, tOnOffOutAux, iRestDurOut);
                                    instrAux.Parse();
                                    break;

                                case MChannelCodeEntry.t_Command.EFFECT:
                                    instrAux.GetEffectCommandParams(ref tEffectAux, ref tOnOffOutAux, ref iRestDurOut);
                                    iRestDurOut = (int)(iRestDurOut * dMultFactor);
                                    instrAux.SetEffectCommandParams(tEffectAux,tOnOffOutAux,iRestDurOut);
                                    instrAux.Parse();
                                    break;

                                case MChannelCodeEntry.t_Command.REST_DURATION:
                                    instrAux.GetRestCommandParams(ref iRestDurOut);
                                    iRestDurOut = (int)(iRestDurOut * dMultFactor);
                                    instrAux.SetRestCommandParams(iRestDurOut);
                                    instrAux.Parse();
                                    break;

                                case MChannelCodeEntry.t_Command.NOTE:
                                    instrAux.GetNoteCommandParams( ref tNoteAux, ref iNoteDurOut, ref iRestDurOut);
                                    iNoteDurOut = (int)(iNoteDurOut * dMultFactor);
                                    iRestDurOut = (int)(iRestDurOut * dMultFactor);
                                    instrAux.SetNoteCommandParams(tNoteAux, iNoteDurOut, iRestDurOut);
                                    instrAux.Parse();
                                    break;

                                case MChannelCodeEntry.t_Command.DURATIONx2:
                                    break;

                            }//switch

                        }//if

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeM2DataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeM2DataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if

            }// if

        }//btnMultM2Entry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the Chords channel "Multiply selected instructions
        * rest time" button. This button allows to increase or duration and decrease 
        * the duration and the rest time of the Chords channel selected instructions 
        * by multiplying them by the corresponding factor.
        *
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnMultChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            int iThemeIdx = 0;
            ChordChannelCodeEntry instrAux = null;
            ChordChannelCodeEntry.t_Notes chordNoteOutAux = ChordChannelCodeEntry.t_Notes.C;
            ChordChannelCodeEntry.t_ChordType chordTypeOutAux = ChordChannelCodeEntry.t_ChordType._MAJOR;
            int iRestDurOut = 0;
            int iChordDurOut = 0;
            double dMultFactor = 0;
            byte by0 = 0;
            byte by1 = 0;

            // check if there is any theme selected and if the Chord channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                // store application into history stack before executing modifications to allow recovering it with Ctrl+Z
                historyThemesState.updateLastRead(dpack_drivePack.themes);

                iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // take the Index of the selected instructions in the dataGridView 
                liISelectionIdx = new List<int>();
                foreach (DataGridViewRow rowAux in themeChordDataGridView.SelectedRows) {
                    liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_CH_IDX].Value));
                }
                liISelectionIdx.Sort();

                // get the index of all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liISelectionIdx.Count > 0) {

                    dMultFactor = (double)nUpDwMultChordEntry.Value;

                    // process each row in the selection
                    foreach (int instrIdx in liISelectionIdx) {

                        // find each channel Chord instruction with the specified Idx and parse its bytes
                        instrAux = dpack_drivePack.themes.liThemesCode[iThemeIdx].liChordCodeInstr.First(p => p.Idx == instrIdx);
                        if (instrAux != null) {

                            switch (instrAux.GetCmdType()) {

                                case ChordChannelCodeEntry.t_Command.CHORD:
                                    instrAux.GetChordCommandParams( ref chordNoteOutAux, ref chordTypeOutAux, ref iChordDurOut);
                                    iChordDurOut = (int)(iChordDurOut * dMultFactor);
                                    instrAux.SetChordCommandParams(chordNoteOutAux, chordTypeOutAux, iChordDurOut);
                                    instrAux.Parse();
                                    break;

                                case ChordChannelCodeEntry.t_Command.REST_DURATION:
                                    instrAux.GetRestCommandParams(ref iRestDurOut);
                                    iRestDurOut = (int)(iRestDurOut * dMultFactor);
                                    instrAux.SetRestCommandParams(iRestDurOut);
                                    instrAux.Parse();
                                    break;

                                case ChordChannelCodeEntry.t_Command.DURATIONx2:
                                    break;

                            }//switch

                        }//if

                    }//foreach

                    // use the idx stored at the begining of the method to keep selected the rows that have been updated
                    themeChordDataGridView.ClearSelection();
                    foreach (int idxInstruction in liISelectionIdx) {
                        themeChordDataGridView.Rows[idxInstruction].Selected = true;
                    }//foreach

                    dpack_drivePack.dataChanged = true;//set the flag that indicates that changes have been done to the ROM Pack cotent 

                    // update the different dataGridView rows selection lists with the current dataGridView selected rows after 
                    // havin executed the changes in case the user changes the current theme Idx or in case the user undoes last changes
                    storeSelectedDGridViewRows();

                    // store current application state into history stack to allow recovering it with Ctrl+Z
                    historyThemesState.pushAfterLastRead(dpack_drivePack.themes);

                }//if

            }// if

        }//btnMultChordEntry_Click

    }// public partial class MainForm

}//namespace drivepacked
