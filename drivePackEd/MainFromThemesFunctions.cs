using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace drivePackEd {

    public partial class MainForm : Form {

        /*******************************************************************************
        * @brief sets the theme at the received index position as the current active theme.
        * @param[in] iIDx with the position in the Themes list of the theme to set as
        * active. From 0 to nTemes-1.
        * @return the ErrCode with the result or error of the operation.
        ********************************************************************************/
        public ErrCode SetCurrentThemeIdx(int iIDx) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR;
            
            if ( iIDx < dpack_drivePack.themes.liThemesCode.Count() ){
                dpack_drivePack.themes.iCurrThemeIdx = iIDx;
            } else {
                erCodeRetVal = cErrCodes.ERR_EDITION_IDX_OUT_OF_RANGE;
            }

            return erCodeRetVal;

        }// setCurrent

        /*******************************************************************************
        * @brief delegate for the click on the button that adds a new theme infromation 
        * intothe application structures.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addThemeButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISeletionIdx = null;
            string str_aux = "";
            int iThemeIdx = 0;

            // check that the maximum number of allowed themes in a ROM will not be reached after adding the new theme
            if (dpack_drivePack.themes.liThemesCode.Count() >= Themes.MAX_THEMES_ROM) {

                ec_ret_val = cErrCodes.ERR_EDITION_NO_SPACE_FOR_THEMES;

            }

            if (ec_ret_val.i_code >= 0) {

                if (themeTitlesDataGridView.SelectedRows.Count == 0) {

                    // if the rom does not contain any theme or if there are no themes selected just add the new theme at the end
                    iThemeIdx = dpack_drivePack.themes.liThemesCode.Count();

                } else {

                    // if there are themes selected get the lowest index of all selected rows and add the new theme after it

                    // take the Index of the slected themes in the dataGridView 
                    liISeletionIdx = new List<int>();
                    foreach (DataGridViewRow rowAux in themeTitlesDataGridView.SelectedRows) {
                        liISeletionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_THEME_IDX].Value));
                    }
                    liISeletionIdx.Sort();

                    iThemeIdx = liISeletionIdx[0] + 1;

                }//if

            }//if

            if (ec_ret_val.i_code >= 0) {

                // add new theme in the themes structure just after the current selected theme
                ec_ret_val = dpack_drivePack.themes.AddNewAt(iThemeIdx);
            
            }

            if (ec_ret_val.i_code >= 0) {

                // update the Idx field of all themes to ensure that they match with their real position in the list
                dpack_drivePack.themes.regenerateIdxs();

                // set the current theme index pointing to the added new theme, then bind/update the form controls to the new current theme index
                SetCurrentThemeIdx(iThemeIdx);
                UpdateInfoTabPageControls();
                UpdateCodeTabPageControls();

                // keep selected the added theme
                themeTitlesDataGridView.ClearSelection();
                themeTitlesDataGridView.Rows[iThemeIdx].Selected = true;

                // informative message for the user 
                str_aux = dpack_drivePack.themes.liThemesCode[iThemeIdx].Title;
                str_aux = "Added theme " + str_aux + " at position " + iThemeIdx + " in the themes list.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + str_aux, false);

            } else {

                // informative message for the user 
                str_aux = "Error adding a new theme in the themes list.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_EDITION + str_aux, false);

            }

        }//addThemeButton_Click

        /*******************************************************************************
        * @brief  Delegate for the click event on the button that removes from the structures
        * the inforation of the selected themes.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void delThemeButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ThemeCode themeAux = null;

            // take the Index of the selected themes in the dataGridView 
            liISelectionIdx = new List<int>();
            foreach (DataGridViewRow rowAux in themeTitlesDataGridView.SelectedRows) {
                liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_THEME_IDX].Value));
            }
            liISelectionIdx.Sort();

            // first check if that there are at least 1 rows selected to delete
            if (liISelectionIdx.Count > 0) {

                // process each row in the selection
                foreach (int themeIdx in liISelectionIdx) {

                    // find each theme with the specified Idx and remove it from the themes list
                    themeAux = dpack_drivePack.themes.liThemesCode.First(p => p.Idx == themeIdx);
                    if (themeAux != null) {
                        dpack_drivePack.themes.liThemesCode.Remove(themeAux);
                    }

                }//foreach

                // update the Idx field of all themes to ensure that they match with their real position in the list
                dpack_drivePack.themes.regenerateIdxs();

                // set the current theme index pointing to the added new theme, then bind/update the form controls to the new current theme index
                SetCurrentThemeIdx(-1);
                UpdateInfoTabPageControls();
                UpdateCodeTabPageControls();

                // no theme selected after deleting selected themes
                themeTitlesDataGridView.ClearSelection();

            }//if (liSelRows.Count > 0) {

        }//delThemeButton_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the Swap themes button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void swapThemeButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISeletionIdx = null;
            ThemeCode thmCodeAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int themeIdx1 = 0;
            int themeIdx2 = 0;
            int iSongIdx = 0;

            iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

            // take the Index of the selected themes in the dataGridView 
            liISeletionIdx = new List<int>();
            foreach (DataGridViewRow rowAux in themeTitlesDataGridView.SelectedRows) {
                liISeletionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_THEME_IDX].Value));
            }
            liISeletionIdx.Sort();

            // first check if that there are at least 2 elements selected to be swapped 
            if (liISeletionIdx.Count > 1) {

                // swap the selected elements
                iAux2 = liISeletionIdx.Count - 1;
                for (iAux = 0; iAux < (int)(liISeletionIdx.Count / 2); iAux++) {

                    themeIdx1 = liISeletionIdx[iAux];
                    themeIdx2 = liISeletionIdx[iAux2];

                    // swap the content of the themes in the list
                    // keep a temporary copy of the theme at themeIdx1
                    thmCodeAux = new ThemeCode();
                    thmCodeAux.CloneFrom(dpack_drivePack.themes.liThemesCode[themeIdx1]);

                    // overwrite the theme at themeIdx1 with the theme at themeIdx2
                    dpack_drivePack.themes.liThemesCode[themeIdx1].CloneFrom(dpack_drivePack.themes.liThemesCode[themeIdx2]);
                    dpack_drivePack.themes.liThemesCode[themeIdx1].Idx = themeIdx1;

                    // overwrite the theme at themeIdx1 with the theme at themeIdx2
                    dpack_drivePack.themes.liThemesCode[themeIdx2].CloneFrom(thmCodeAux);
                    dpack_drivePack.themes.liThemesCode[themeIdx2].Idx = themeIdx2;

                    iAux2--;

                }//for (iAux=0;

                // set the current theme index pointing to the first of the swapped themes and then
                // bind/update the form controls to the current theme index
                SetCurrentThemeIdx(liISeletionIdx[0]);
                UpdateInfoTabPageControls();
                UpdateCodeTabPageControls();

                // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                themeTitlesDataGridView.ClearSelection();
                foreach (int themeIdx in liISeletionIdx) {
                    themeTitlesDataGridView.Rows[themeIdx].Selected = true;
                }

            }// if (liSelRows.Count > 0)

        }//swapThemeButton_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Up one position all selected themes 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnUpTheme_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ThemeCode thmCodeAux = null;
            int iAux = 0;
            int themeIdx1 = 0;
            int themeIdx2 = 0;
            int iSongIdx = 0;

            iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

            // take the Index of the selected themes in the dataGridView 
            liISelectionIdx = new List<int>();
            foreach (DataGridViewRow rowAux in themeTitlesDataGridView.SelectedRows) {
                liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_THEME_IDX].Value));
            }
            liISelectionIdx.Sort();

            // check that there is at least 1 row selected to move
            if (liISelectionIdx.Count > 0) {

                // check that there is at least 1 row over the selected themes rows to move them up 1 position
                themeIdx1 = liISelectionIdx[0];
                if (themeIdx1 > 0) {

                    themeIdx1 = themeIdx1 - 1;

                    for (iAux = 0; iAux < (int)liISelectionIdx.Count; iAux++) {

                        themeIdx2 = liISelectionIdx[iAux];

                        // swap the content of the themes in the list
                        // keep a temporary copy of the theme at themeIdx1
                        thmCodeAux = new ThemeCode();
                        thmCodeAux.CloneFrom(dpack_drivePack.themes.liThemesCode[themeIdx1]);

                        // overwrite the theme at themeIdx1 with the theme at themeIdx2
                        dpack_drivePack.themes.liThemesCode[themeIdx1].CloneFrom(dpack_drivePack.themes.liThemesCode[themeIdx2]);
                        dpack_drivePack.themes.liThemesCode[themeIdx1].Idx = themeIdx1;

                        // overwrite the theme at themeIdx1 with the theme at themeIdx2
                        dpack_drivePack.themes.liThemesCode[themeIdx2].CloneFrom(thmCodeAux);
                        dpack_drivePack.themes.liThemesCode[themeIdx2].Idx = themeIdx2;

                        themeIdx1 = themeIdx2;

                    }//for (iAux=0;

                    // set the current theme index pointing to the first of the moved themes and then
                    // bind/update the form controls to the current theme index
                    SetCurrentThemeIdx(liISelectionIdx[0]-1);
                    UpdateInfoTabPageControls();
                    UpdateCodeTabPageControls();

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    themeTitlesDataGridView.ClearSelection();
                    foreach (int themeIdx in liISelectionIdx) {
                        themeTitlesDataGridView.Rows[themeIdx - 1].Selected = true; ;
                    }

                }//if

            }// if (themeTitlesDataGridView.SelectedRows.Count > 0)

        }//btnUpTheme_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Down one position all selected themes 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btDownTheme_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ThemeCode thmCodeAux = null;
            int iAux = 0;
            int themeIdx1 = 0;
            int themeIdx2 = 0;
            int iSongIdx = 0;


            iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

            // take the Index of the selected themes in the dataGridView 
            liISelectionIdx = new List<int>();
            foreach (DataGridViewRow rowAux in themeTitlesDataGridView.SelectedRows) {
                liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_THEME_IDX].Value));
            }
            liISelectionIdx.Sort();

            //  check that there is at least 1 row selected to move
            if (liISelectionIdx.Count > 0) {

                // check that there is at less 1 row under the selected themes rows to move them down 1 position
                themeIdx1 = liISelectionIdx[liISelectionIdx.Count - 1];
                if (themeIdx1 < (dpack_drivePack.themes.liThemesCode.Count - 1)) {

                    themeIdx1 = themeIdx1 + 1;

                    for (iAux = (int)(liISelectionIdx.Count - 1); iAux >= 0; iAux--) {

                        themeIdx2 = liISelectionIdx[iAux];

                        // swap the content of the themes in the list
                        // keep a temporary copy of the theme at themeIdx1
                        thmCodeAux = new ThemeCode();
                        thmCodeAux.CloneFrom(dpack_drivePack.themes.liThemesCode[themeIdx1]);

                        // overwrite the theme at themeIdx1 with the theme at themeIdx2
                        dpack_drivePack.themes.liThemesCode[themeIdx1].CloneFrom(dpack_drivePack.themes.liThemesCode[themeIdx2]);
                        dpack_drivePack.themes.liThemesCode[themeIdx1].Idx = themeIdx1;

                        // overwrite the theme at themeIdx1 with the theme at themeIdx2
                        dpack_drivePack.themes.liThemesCode[themeIdx2].CloneFrom(thmCodeAux);
                        dpack_drivePack.themes.liThemesCode[themeIdx2].Idx = themeIdx2;

                        themeIdx1 = themeIdx2;

                    }//for (iAux=0;

                    // set the current theme index pointing to the first of the moved themes and then
                    // bind/update the form controls to the current theme index
                    SetCurrentThemeIdx(liISelectionIdx[0] + 1);
                    UpdateInfoTabPageControls();
                    UpdateCodeTabPageControls();

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    themeTitlesDataGridView.ClearSelection();
                    foreach (int themeIdx in liISelectionIdx) {
                        themeTitlesDataGridView.Rows[themeIdx + 1].Selected = true;
                    }

                }//if

            }// if (themeTitlesDataGridView.SelectedRows.Count > 0)

        }//btDownTheme_Click

        /*******************************************************************************
        * @brief Delegate for the click on the copy selected Themes button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btCopyTheme_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            ThemeCode themeCodeAux = null;
            int iAux = 0;
            int iThemeIdx = 0;

            // take the Index of the selected themes in the dataGridView 
            liISelectionIdx = new List<int>();
            foreach (DataGridViewRow rowAux in themeTitlesDataGridView.SelectedRows) {
                liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_THEME_IDX].Value));
            }
            liISelectionIdx.Sort();

            //  check if that there are at least 1 theme row selected to be coppied 
            if (liISelectionIdx.Count > 0) {

                // initialize the temporary list of themes
                liCopyTemporaryThemes = new List<ThemeCode>();

                // copy all the selected themes into the temporary themes list
                for (iAux = 0; iAux < (int)liISelectionIdx.Count; iAux++) {

                    // make a a copy of each selected theme 
                    themeCodeAux = new ThemeCode();
                    iThemeIdx = liISelectionIdx[iAux];
                    themeCodeAux.CloneFrom(dpack_drivePack.themes.liThemesCode[iThemeIdx]);

                    // store the copy into the temporary list of themes
                    liCopyTemporaryThemes.Add(themeCodeAux);

                }//for

                // update the content of all the info tab page controls with the info of the new theme
                UpdateInfoTabPageControls();

                // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                themeTitlesDataGridView.ClearSelection();
                foreach (int themeIdx in liISelectionIdx) {
                    themeTitlesDataGridView.Rows[themeIdx].Selected = true;
                }

            }// if (liISelectionIdx.Count > 0)

        }//btCopyTheme_Click

        /*******************************************************************************
        * @brief Delegate for the click on the paste previously coppied Themes button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btPasteTheme_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISeletionIdx = null;
            string str_aux = "";
            int iThemeIdx = 0;
            int iAux = 0;
            int iAux2 = 0;

            // check that the maximum number of allowed themes in a ROM will not be reached after adding the new theme
            iAux = dpack_drivePack.themes.liThemesCode.Count() + liCopyTemporaryThemes.Count();
            if (iAux >= Themes.MAX_THEMES_ROM) {

                ec_ret_val = cErrCodes.ERR_EDITION_NO_SPACE_FOR_THEMES;

            }

            if (ec_ret_val.i_code >= 0) {

                if (themeTitlesDataGridView.SelectedRows.Count == 0) {

                    // if the rom does not contain any theme or if there are no themes selected just add the new theme at the end
                    iThemeIdx = dpack_drivePack.themes.liThemesCode.Count();

                } else {

                    // if there are themes selected get the lowest index of all selected rows and add the new theme after it

                    // take the Index of the slected themes in the dataGridView 
                    liISeletionIdx = new List<int>();
                    foreach (DataGridViewRow rowAux in themeTitlesDataGridView.SelectedRows) {
                        liISeletionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_THEME_IDX].Value));
                    }
                    liISeletionIdx.Sort();

                    iThemeIdx = liISeletionIdx[0] + 1;

                }//if

                iAux = 0;
                while ( (iAux<liCopyTemporaryThemes.Count()) && (ec_ret_val.i_code>=0)) {

                    iAux2 = iThemeIdx + iAux;

                    ec_ret_val = dpack_drivePack.themes.AddNewAt(iAux2);
                    if (ec_ret_val.i_code >= 0) {
                        dpack_drivePack.themes.liThemesCode[iAux2].CloneFrom(liCopyTemporaryThemes[iAux]);
                    }

                    iAux++;

                }//while
            
            }//if

            if (ec_ret_val.i_code >= 0) {

                // update the Idx field of all themes to ensure that they match with their real position in the list
                dpack_drivePack.themes.regenerateIdxs();

                // set the current theme index pointing to the first of the copied themes and then
                // bind/update the form controls to the current theme index
                SetCurrentThemeIdx(iThemeIdx);
                UpdateInfoTabPageControls();
                UpdateCodeTabPageControls();

                // use the idx calculated at the begining to keep selected the pasted themes
                themeTitlesDataGridView.ClearSelection();
                for (iAux = iThemeIdx; iAux < (iThemeIdx + liCopyTemporaryThemes.Count); iAux++) {
                    themeTitlesDataGridView.Rows[iAux].Selected = true;
                }

                // informative message for the user 
                str_aux = dpack_drivePack.themes.liThemesCode[iThemeIdx].Title;
                str_aux = "Pasted " + liCopyTemporaryThemes.Count() + " themes at " + iThemeIdx + " in the themes list.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + str_aux, false);

            } else {

                // informative message for the user 
                str_aux = "Error pasting the " + liCopyTemporaryThemes.Count() + " themes in the themes list.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_EDITION + str_aux, false);

            }

        }//btPasteTheme_Click

    }//public partial class MainForm

}//namespace drivePackEd 
