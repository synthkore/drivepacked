﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Linq;
using static drivePackEd.cDrivePack;
using System.ComponentModel;

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
        *  @brief Initialize the controls used to edit the different notes in the 
        *  Melody and Chord channels.
        *******************************************************************************/
        public void InitEditInstructionControls() {

            // #######################################################   controls for the NOTE SELECTION + DURATION + REST command
            nUpDownM1NoteRest = new System.Windows.Forms.NumericUpDown();
            labM1NoteRest = new System.Windows.Forms.Label(); 
            nUpDownM1NoteDur = new System.Windows.Forms.NumericUpDown();
            labM1NoteDur = new System.Windows.Forms.Label();
            comboBoxM1Note = new System.Windows.Forms.ComboBox();
            labM1Note = new System.Windows.Forms.Label();

            // 
            // nUpDownM1Rest
            // 
            nUpDownM1NoteRest.Location = new Point(247, 79);
            nUpDownM1NoteRest.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDownM1NoteRest.Name = "nUpDownM1Rest";
            nUpDownM1NoteRest.Size = new Size(54, 23);
            nUpDownM1NoteRest.TabIndex = 43;
            // 
            // labM1Rest
            // 
            labM1NoteRest.AutoSize = true;
            labM1NoteRest.Location = new Point(211, 83);
            labM1NoteRest.Name = "labM1Rest";
            labM1NoteRest.Size = new Size(32, 15);
            labM1NoteRest.TabIndex = 42;
            labM1NoteRest.Text = "Rest:";
            // 
            // nUpDownM1Dur
            // 
            nUpDownM1NoteDur.Location = new Point(153, 79);
            nUpDownM1NoteDur.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nUpDownM1NoteDur.Name = "nUpDownM1Dur";
            nUpDownM1NoteDur.Size = new Size(54, 23);
            nUpDownM1NoteDur.TabIndex = 41;
            // 
            // labM1Dur
            // 
            labM1NoteDur.AutoSize = true;
            labM1NoteDur.Location = new Point(118, 83);
            labM1NoteDur.Name = "labM1Dur";
            labM1NoteDur.Size = new Size(29, 15);
            labM1NoteDur.TabIndex = 40;
            labM1NoteDur.Text = "Dur:";
            // 
            // comboBoxM1Note
            // 
            comboBoxM1Note.FormattingEnabled = true;
            comboBoxM1Note.Location = new Point(45, 79);
            comboBoxM1Note.Name = "comboBoxM1Note";
            comboBoxM1Note.Size = new Size(67, 23);
            comboBoxM1Note.TabIndex = 39;
            // 
            // labM1Note
            // 
            labM1Note.AutoSize = true;
            labM1Note.Location = new Point(6, 83);
            labM1Note.Name = "labM1Note";
            labM1Note.Size = new Size(36, 15);
            labM1Note.TabIndex = 38;
            labM1Note.Text = "Note:";

            nUpDownM1TimbreRest = new System.Windows.Forms.NumericUpDown();
            labM1TimbreRest = new System.Windows.Forms.Label();
            cmboBoxM1TimbreOnOff = new System.Windows.Forms.ComboBox();
            labM1Timbre = new System.Windows.Forms.Label();
            cmboBoxM1Timbre = new System.Windows.Forms.ComboBox();

            panel1.Controls.Add(nUpDownM1NoteRest);
            panel1.Controls.Add(labM1NoteRest);
            panel1.Controls.Add(nUpDownM1NoteDur);
            panel1.Controls.Add(labM1NoteDur);
            panel1.Controls.Add(comboBoxM1Note);
            panel1.Controls.Add(labM1Note);
            panel1.Controls.Add(cmboBoxM1Instr);

            // #######################################################   controls for the TIMBRE + ON/OFF + REST command
            // 
            // nUpDownM1TimbreRest
            // 
            nUpDownM1TimbreRest.Location = new Point(288, 79);
            nUpDownM1TimbreRest.Name = "nUpDownM1TimbreRest";
            nUpDownM1TimbreRest.Size = new Size(68, 23);
            nUpDownM1TimbreRest.TabIndex = 42;
            // 
            // labM1TimbreRest
            // 
            labM1TimbreRest.AutoSize = true;
            labM1TimbreRest.Location = new Point(250, 83);
            labM1TimbreRest.Name = "labM1TimbreRest";
            labM1TimbreRest.Size = new Size(32, 15);
            labM1TimbreRest.TabIndex = 41;
            labM1TimbreRest.Text = "Rest:";
            // 
            // cmboBoxM1TimbreOnOff
            // 
            cmboBoxM1TimbreOnOff.FormattingEnabled = true;
            cmboBoxM1TimbreOnOff.Location = new Point(176, 79);
            cmboBoxM1TimbreOnOff.Name = "cmboBoxM1TimbreOnOff";
            cmboBoxM1TimbreOnOff.Size = new Size(67, 23);
            cmboBoxM1TimbreOnOff.TabIndex = 40;
            // 
            // labM1Timbre
            // 
            labM1Timbre.AutoSize = true;
            labM1Timbre.Location = new Point(5, 82);
            labM1Timbre.Name = "labM1Timbre";
            labM1Timbre.Size = new Size(33, 15);
            labM1Timbre.TabIndex = 39;
            labM1Timbre.Text = "Instr:";
            // 
            // cmboBoxM1Timbre
            // 
            cmboBoxM1Timbre.FormattingEnabled = true;
            cmboBoxM1Timbre.Location = new Point(44, 79);
            cmboBoxM1Timbre.Name = "cmboBoxM1Timbre";
            cmboBoxM1Timbre.Size = new Size(126, 23);
            cmboBoxM1Timbre.TabIndex = 38;

            panel1.Controls.Add(nUpDownM1TimbreRest);
            panel1.Controls.Add(labM1TimbreRest);
            panel1.Controls.Add(cmboBoxM1TimbreOnOff);
            panel1.Controls.Add(labM1Timbre);
            panel1.Controls.Add(cmboBoxM1Timbre);


        }//InitEditInstructionControls

        /*******************************************************************************
        *  @brief InitControls
        *******************************************************************************/
        public ErrCode InitControls() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            BindingList<t_ROMCommand> liCommands = new BindingList<t_ROMCommand>();
            string str_aux = "";

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

            // initialize the drive pack object with the default drive pack file 
            dpack_drivePack.Initialize(configMgr.m_str_default_rom_file);
            dpack_drivePack.dynbyprMemoryBytes.Changed += new System.EventHandler(this.BeHexEditorChanged);// this method will be called every time there is a change in the content of the Be Hex editor
            // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
            hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;

            // add the Be Hex editor to the corresponding tab page
            tabControlMain.TabPages[2].Controls.Add(hexb_romEditor);

            // initialize de content of the instruction editon combo boxes
            // get all the command codes in the enumerate and add them to the list for the comboBox
            foreach (t_ROMCommand tcommand in Enum.GetValues(typeof(t_ROMCommand))) {
                liCommands.Add(tcommand);
            }
            cmboBoxM1Instr.DataSource = liCommands;

            // create and initialize the controls used to edit the content of the melody and chords instructions
            InitEditInstructionControls();

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
        * @brief shows the About dialog with the application version general information.
        *******************************************************************************/
        private void showAboutDialog() {
            AboutBox aboutForm = null;
            string strName = "";
            string strLicense = "";
            string strDescription = "";

            strName = "drivePack Editor ";
            strLicense = "(c) Tolaemon 2024";
            strDescription = "";
            aboutForm = new AboutBox(strName, strLicense, strDescription);
            aboutForm.MinimizeBox = false;
            aboutForm.MaximizeBox = false;

            // set main form title
            // str_aux = cConfig.SW_TITLE + " - " + cConfig.SW_VERSION + "\r\nDrive pack files viewer and editor" + "\r\nJordi Bartolomé - Tolaemon 2024-12-28";

            aboutForm.StartPosition = FormStartPosition.CenterScreen;
            aboutForm.Show(this);

        }//showAboutDialog

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
        * @brief Update, enable or disable the corresponding instruction edition controls 
        * according to the current selected instrucion in the instruction selection 
        * combo box.
        *******************************************************************************/
        public void UpdateInstructionEditionControls() {

           
            // if the main Form has been created and is visible
            if (this.Visible) {

                // update the instruction edition controls according to the current selected instruction

                // show or hide NOTE ON command controls
                if (cmboBoxM1Instr.Text == t_ROMCommand.I04_NOTE.ToString()) {
                    // if instruction is NOTE the enable and show the controls that allow to
                    // modify and update NOTE command
                    labM1Note.Enabled = true;
                    labM1Note.Visible = true;
                    comboBoxM1Note.Enabled = true;
                    comboBoxM1Note.Visible = true;
                    labM1NoteDur.Enabled = true;
                    labM1NoteDur.Visible = true;
                    nUpDownM1NoteDur.Enabled = true;
                    nUpDownM1NoteDur.Visible = true;
                    labM1NoteRest.Enabled = true;
                    labM1NoteRest.Visible = true;
                    nUpDownM1NoteRest.Enabled = true;
                    nUpDownM1NoteRest.Visible = true;

                } else {
                    // if instruction is not NOTE then disable and hide the controls used to
                    // modify and update NOTE command
                    labM1Note.Enabled = false;
                    labM1Note.Visible = false;
                    comboBoxM1Note.Enabled = false;
                    comboBoxM1Note.Visible = false;
                    labM1NoteDur.Enabled = false;
                    labM1NoteDur.Visible = false;
                    nUpDownM1NoteDur.Enabled = false;
                    nUpDownM1NoteDur.Visible = false;
                    labM1NoteRest.Enabled = false;
                    labM1NoteRest.Visible = false;
                    nUpDownM1NoteRest.Enabled = false;
                    nUpDownM1NoteRest.Visible = false;
                }

                // show or hide TIMBRE command controls
                if (cmboBoxM1Instr.Text == t_ROMCommand.I01_TIMBRE_INSTRUMENT.ToString()) {
                    // if instruction is TIMBRE the enable and show the controls that allow to
                    // modify and update TIMBRE command
                    labM1Timbre.Enabled = true;
                    labM1Timbre.Visible = true;
                    cmboBoxM1Timbre.Enabled = true;
                    cmboBoxM1Timbre.Visible = true;
                    cmboBoxM1TimbreOnOff.Enabled = true;
                    cmboBoxM1TimbreOnOff.Visible = true;
                    labM1TimbreRest.Enabled = true;
                    labM1TimbreRest.Visible = true;
                    nUpDownM1TimbreRest.Enabled = true;
                    nUpDownM1TimbreRest.Visible = true;
                } else {
                    // if instruction is not TIMBRE then disable and hide the controls used to
                    // modify and update TIMBRE command
                    labM1Timbre.Enabled = false;
                    labM1Timbre.Visible = false;
                    cmboBoxM1Timbre.Enabled = false;
                    cmboBoxM1Timbre.Visible = false;
                    cmboBoxM1TimbreOnOff.Enabled = false;
                    cmboBoxM1TimbreOnOff.Visible = false;
                    labM1TimbreRest.Enabled = false;
                    labM1TimbreRest.Visible = false;
                    nUpDownM1TimbreRest.Enabled = false;
                    nUpDownM1TimbreRest.Visible = false;
                }

            }//if (this.Visible)

        }// UpdateInstructionEditionControls

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


            // get the form dimensions and coordinates
            configMgr.m_i_screen_size_y = this.Height;
            configMgr.m_i_screen_size_x = this.Width;
            configMgr.m_i_screen_orig_y = this.Top;
            configMgr.m_i_screen_orig_x = this.Left;

            configMgr.m_b_screen_maximized = (this.WindowState == System.Windows.Forms.FormWindowState.Maximized);

            // update object properties with controls state
            if (dpack_drivePack.themes.strROMTitle != romTitleTextBox.Text) {
                dpack_drivePack.dataChanged = true;
                dpack_drivePack.themes.strROMTitle = romTitleTextBox.Text;
            }
            if (dpack_drivePack.themes.strROMInfo != romInfoTextBox.Text) {
                dpack_drivePack.dataChanged = true;
                dpack_drivePack.themes.strROMInfo = romInfoTextBox.Text;
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
                strAux = i_aux.ToString() + " : " + dpack_drivePack.themes.liThemesCode[i_aux].Title;
                themeSelectComboBox.Items.Add(strAux);
            }

            // initialize the label that indicates the total of sequences in memory
            lblThemesList.Text = "Theme (" + dpack_drivePack.themes.liThemesCode.Count.ToString() + "):";

            // update the selected theme ComboBox
            if ((iThemeIdx < 0) || (dpack_drivePack.themes.liThemesCode.Count == 0)) {

                // there is no theme selected in the list of avialable themes
                themeSelectComboBox.SelectedIndex = -1;
                themeSelectComboBox.Text = "";

            } else {

                // if there is any theme selected in the list of available themes then highlight it in the combo box
                themeSelectComboBox.SelectedIndex = iThemeIdx;
                themeSelectComboBox.Text = iThemeIdx.ToString() + "  :" + dpack_drivePack.themes.liThemesCode[iThemeIdx].Title;

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
        public ErrCode UpdateInfoTabPageControls() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            DataGridViewTextBoxColumn textBoxColumnAux = null;

            // update the themes list dataGridView with the current list of themes

            // init melody1 DataGridView: clear the M1 dataGridView before filling it with the content of the list of M1 entries 
            themeTitlesDataGridView.DefaultCellStyle.Font = new Font(TITLES_FONT, TITLES_SIZE);
            themeTitlesDataGridView.DataSource = null;
            themeTitlesDataGridView.Columns.Clear();
            themeTitlesDataGridView.Rows.Clear();
            themeTitlesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

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

        }//UpdateInfoTabPageControls

        /*******************************************************************************
        * @brief Takes different information of the themes and their channels and writes
        * it into the corresponding controls. So it updtates the controls with the current 
        * themes information.
        * @return
        *   - ErrCode >= 0 if the operation could be executed.
        *   - ErrCode < 0 if it was not possible to execute the operation.
        *******************************************************************************/
        // public ErrCode UpdateControls() {
        //     ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
        //     int i_aux = 0;
        //
        //
        //
        //     // update the instructions channels dataGridViews view with the instructions of the current selected theme
        //     UpdateCodeTabPageControls();
        //
        //     return ec_ret_val;
        //
        // }//UpdateControls

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
                if (i_aux2 == -1) { i_aux2 = str_aux.Length - 1; }// if there is no '\r' then it means that is the end of the text block
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
                i_ret_val = processFile(ref iIdx, str_path_in + "\\" + str_file_name, str_paht_out + "\\" + str_file_name, str_paht_out + "\\" + str_path_summary);
                i_aux++;

            }//while

            return i_ret_val;

        }//processPath

    }//public partial class MainForm : Form

}//namespace drivePackEd
