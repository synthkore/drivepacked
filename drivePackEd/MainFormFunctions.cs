using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using System.IO;
using System.Drawing;

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

    public partial class MainForm : Form{

        /*******************************************************************************
        *  @brief InitControls
        *******************************************************************************/
        public ErrCode InitControls() {
            string str_aux = "";
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;

            // loads the configuration parameters according to the last state of the application
            ec_ret_val = configMgr.LoadConfigParameters();
            if (ec_ret_val.i_code < 0) {

                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, ec_ret_val.str_description, true);
                System.Windows.Forms.Application.Exit();

            }//if

            // creates or opens the logs file where are stored the events that happen during application execution 
            statusNLogs.MessagesInit(configMgr.m_str_logs_path, configMgr.m_b_new_log_per_sesion, textBox2, statusStrip1, toolStripStatusLabel1);
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
            hexb_romEditor.Location = new System.Drawing.Point(9, 28);
            hexb_romEditor.Width = tabControl2.TabPages[0].Width - 18;
            hexb_romEditor.Height = tabControl2.TabPages[0].Height - (delSequenceButton.Height + 11);
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

            // initialize the drive pack object with the default drive pack file 
            dpack_drivePack.Initialize(configMgr.m_str_default_rom_file);
            dpack_drivePack.dynbyprMemoryBytes.Changed += new System.EventHandler(this.BeHexEditorChanged);// this method will be called every time there is a change in the content of the Be Hex editor
            // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
            hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;

            // add the Be Hex editor to the corresponding tab page
            tabControl2.TabPages[1].Controls.Add(hexb_romEditor);

            // set application controls state according to the configuration parameters values
            UpdateAppWithConfigParameters(true);

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
            str_aux = cConfig.SW_TITLE + " - " + cConfig.SW_VERSION + " - " + cConfig.SW_DESCRIPTION;
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
            romTitleTextBox.Text = dpack_drivePack.strTitle;

            // updates the corresponding text box with the last read valid song information
            romInfoTextBox.Text = dpack_drivePack.strSongInfo;

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


            // get the form dimensions and coordinates
            configMgr.m_i_screen_size_y = this.Height;
            configMgr.m_i_screen_size_x = this.Width;
            configMgr.m_i_screen_orig_y = this.Top;
            configMgr.m_i_screen_orig_x = this.Left;

            configMgr.m_b_screen_maximized = (this.WindowState == System.Windows.Forms.FormWindowState.Maximized);

            // update object properties with controls state
            if (dpack_drivePack.strTitle != romTitleTextBox.Text) {
                dpack_drivePack.dataChanged = true;
                dpack_drivePack.strTitle = romTitleTextBox.Text;
            }
            if (dpack_drivePack.strSongInfo != romInfoTextBox.Text) {
                dpack_drivePack.dataChanged = true;
                dpack_drivePack.strSongInfo = romInfoTextBox.Text;
            }

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
            b_close_application = ConfirmCloseProject("There pending modifications to save. Exit anyway?");

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
        public ErrCode UpdateCodeChannelsWithDataGridView() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            int iSongIdx = 0;


            // check if there is any song selected
            if (dpack_drivePack.themes.iCurrThemeIdx != -1) {
                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                dpack_drivePack.themes.liThemesCode[iSongIdx].strThemeTitle = sequenceTitleTextBox.Text.Trim();

                // as the the content of the .liM1CodeInstr, .liM2CodeInstr and .liChordCodeInstr  is binded to the
                // DataGridViews that lists are automatically updated when the user modifies the content in the
                // DataGridViews and there is no need to manually move the content of the dataGridViews to the lists.

            }//if

            return ec_ret_val;

        }//UpdateSequenceChannelsWithDataGridView


        /*******************************************************************************
        * @brief This procedure binds the list of code/instructions of the different melody
        * /chord channels of the currently selected sequence (theme) to the corresponding
        * DataGridViews
        * @return
        *   - ErrCode >= 0 if the operation could be executed.
        *   - ErrCode < 0 if it was not possible to execute the operation.
        *******************************************************************************/
        public ErrCode UpdateDataGridViewsWithSequenceChannels() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            DataGridViewTextBoxColumn textBoxColumnAux = null;
            DataGridViewRow dataGridRowAux = null;
            byte[] arrByAux = null;
            int iThemeIdx = 0;
            int iAux = 0;

            // if there is any song selected then fill the M1, M2 and Chrod dataGridViews with the corresponding
            // selected theme M1, M2 or Chord channels content.
            iThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;
            if (iThemeIdx != -1) {

                sequenceTitleTextBox.Text = dpack_drivePack.themes.liThemesCode[iThemeIdx].strThemeTitle;

                // Melody 1 (main melody) DataGridView: bind the channel 1 instructions of the current selected song to the M1 DataGridView ##################

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

                // Melody 2 (obligatto) DataGridView: bind the channel 2 instructions of the current selected song to the M2 DataGridView ##################
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

                // Chords channel DataGridView: bind the chords channel of the current selected song to the chord DataGridView ##################

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

            }//if

            return ec_ret_val;

        }//UpdateDataGridViewsWithSequenceChannels


        /*******************************************************************************
        * @brief Takes different information of the themes and their channels and writes
        * it into the corresponding controls. So it updtates the controls with the themes
        * information.
        * @return
        *   - ErrCode >= 0 if the operation could be executed.
        *   - ErrCode < 0 if it was not possible to execute the operation.
        *******************************************************************************/
        public ErrCode UpdateControlsWithSongInfo() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            int i_aux = 0;


            // update the items in the songs combo box with the list of available songs
            sequenceSelectComboBox.Items.Clear();
            for (i_aux = 0; i_aux < dpack_drivePack.themes.liThemesCode.Count; i_aux++) {
                sequenceSelectComboBox.Items.Add(i_aux);
            }

            // initialize the label that indicates the total of sequences in memory
            totalSongsLabel.Text = "Total: " + dpack_drivePack.themes.liThemesCode.Count.ToString();

            // check if there is any song selected in the list of available songs
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (dpack_drivePack.themes.liThemesCode.Count == 0)) {

                // there is no song selected in the list of avialable songs
                sequenceSelectComboBox.SelectedIndex = -1;

            } else {

                // if there is any song selected in the list of available songs then highlight it in the combo box
                sequenceSelectComboBox.SelectedIndex = dpack_drivePack.themes.iCurrThemeIdx;

            }//if

            UpdateDataGridViewsWithSequenceChannels();

            return ec_ret_val;

        }//UpdateControlsWithSongInfo

    }//public partial class MainForm : Form

}//namespace drivePackEd
