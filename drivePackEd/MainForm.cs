﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using Be.Windows.Forms;
using System.Reflection;
using Microsoft.VisualBasic;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Runtime.Intrinsics.Arm;
using static drivePackEd.cThemesInfo;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Intrinsics.X86;

// Al hacer determinadas opreaciones de añadir, borrar, swap de instrucciones etc. se descentra todo
// Faltan muhchos mensjaes de log indicando las operaciones realizadas por el usuario
// Como afecta al tema activo (current theme) si borramos o añadimos un nuevo tema, o incluso si hemos borrado el que era el tema activo.
// Revisar toda la gestión de las DataGridViews
// En los UpdateControlsCodeM1(); UpdateControlsCodeM2(); etc se hace el Binding de los datagridviews y estos se llaman siempre que se actualizan y es incorrecto puesto que el binding solo hay que hacerlo cuando se selecciona otro tema.
// Los Idx de los temas comienzan en "0" mientras que en los cartuchos y en los propios teclados cominezan en el indice "1"
// Las rutinas de reindexado tras borrar o insertar un elemento se pueden implementar como un método de la propia lista de instrucciones
// Mira si hay que reorganizar las opciones del Tool strip Files para que quede todo más organizado.
// Borrar tanto la ROM como las listas de temas al hacer New
// Al guardar un tema en un fichero .COD se pierden los comentarios propios.
// Mantener seleccionado el ultimo puerto serie utilizado para transferir si es que sigue existiendo.
// Al actualizar los controles con la info de las Songs y Sheets se borran el texto de las entradas del ComboBox de sheets pero permanecen la lineas en blanco.
// Al cargar el fichero recibido este no se actualiza en el formulario.
// Si al recibir un fichero hacemos primero el Receive en el PC y luego el SEND en el ordenador el fichero no se envia.

// **********************************************************************************
// ****                          drivePACK Editor                                ****
// ****                         www.tolaemon.com/dpack                           ****
// ****                              Source code                                 ****
// ****                              20/12/2023                                  ****
// ****                            Jordi Bartolome                               ****
// ****                                                                          ****
// ****          IMPORTANT:                                                      ****
// ****          Using this code or any part of it means accepting all           ****
// ****          conditions exposed in: http://www.tolaemon.com/dpack            ****
// **********************************************************************************

namespace drivePackEd {

    public partial class MainForm : Form {

        const int ROWS_HEADER_WDITH = 20; // header widht, that is before column '0'

        // constants string used to access the song sheet columns

        // Themes list DataGridView columns
        public const string IDX_COLUMN_THEME_IDX = "Idx";// position of the theme in the list of themes
        public const string IDX_COLUMN_THEME_NAME = "Title";// title of the theme

        public const string IDX_COLUMN_THEME_IDX_TIT = "Idx";
        public const string IDX_COLUMN_THEME_NAME_TIT = "Title";

        // Melody 1 commands DataGridView columns
        public const string IDX_COLUMN_M1_IDX = "Idx";// Melody 1 note / command index                
        public const string IDX_COLUMN_M1 = "B0";   // Melody 1 Ocatve and Note
        public const string IDX_COLUMN_M1ON = "B1"; // Melody 1 ON duration        
        public const string IDX_COLUMN_M1OFF = "B2";// Melody 1 OFF duration
        public const string IDX_COLUMN_M1DESCR = "Descr"; // Explanation of the command bytes

        public const string IDX_COLUMN_M1_FORMAT = "000";// Formater string for the elements of the M1 Idx column

        public const string IDX_COLUMN_M1_IDX_TIT = "Idx";
        public const string IDX_COLUMN_M1_TIT = "B0";
        public const string IDX_COLUMN_M1ON_TIT = "B1";
        public const string IDX_COLUMN_M1OFF_TIT = "B2";
        public const string IDX_COLUMN_M1DESCR_TIT = "Description";

        // Melody 2 commands DataGridView columns
        public const string IDX_COLUMN_M2_IDX = "Idx";// Melody 1 note / command index
        public const string IDX_COLUMN_M2 = "B0";    // Melody 2: Ocatve and Note
        public const string IDX_COLUMN_M2ON = "B1";  // Melody 2 ON duration
        public const string IDX_COLUMN_M2OFF = "B2"; // Melody 2 OFF duration
        public const string IDX_COLUMN_M2DESCR = "Descr"; // Explanation of the command bytes

        public const string IDX_COLUMN_M2_FORMAT = "000";// Formater string for the elements of the M2 Idx column

        public const string IDX_COLUMN_M2_IDX_TIT = "Idx";
        public const string IDX_COLUMN_M2_TIT = "B0";
        public const string IDX_COLUMN_M2ON_TIT = "B1";
        public const string IDX_COLUMN_M2OFF_TIT = "B2";
        public const string IDX_COLUMN_M2DESCR_TIT = "Description";

        // Chord commands DataGridView columns
        public const string IDX_COLUMN_CH_IDX = "Idx";// CHord index
        public const string IDX_COLUMN_CH = "B0";       // CHord: chord
        public const string IDX_COLUMN_CHON = "B1";   //CHord ON duration 
        public const string IDX_COLUMN_CHDESCR = "Descr";// Explanation of the command bytes

        public const string IDX_COLUMN_CH_FORMAT = "000";// Formater string for the elements of the M2 Idx column

        public const string IDX_COLUMN_CH_IDX_TIT = "Idx";
        public const string IDX_COLUMN_CH_TIT = "B0";
        public const string IDX_COLUMN_CHON_TIT = "B1";
        public const string IDX_COLUMN_CHDESCR_TIT = "Description";

        public const string HEX_FONT = "Courier New";
        public const int HEX_SIZE = 10;
        public const string CODE_FONT = "Courier New";
        public const int CODE_SIZE = 9;

        public SendForm sendRomForm = null;
        public ReceiveForm receiveRomForm = null;

        cLogsNErrors statusNLogs;
        cDrivePack dpack_drivePack;
        cConfig configMgr = new cConfig();
        HexBox hexb_romEditor = null;

        bool bShowAboutOnLoad = false; // if true the About dialog box will be shown every time teh application starts

        /*******************************************************************************
        * @brief form class default constructor
        *******************************************************************************/
        public MainForm() {

            statusNLogs = new cLogsNErrors();
            dpack_drivePack = new cDrivePack(statusNLogs);

            InitializeComponent();
            InitControls();

            if (bShowAboutOnLoad) {
                showAboutDialog();
            }

        }//MainForm

        /*******************************************************************************
        * @brief delegate for the form closing event
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void mainForm_FormClosing(object sender, FormClosingEventArgs e) {

            // before operating, the more recent value of the general configuration parameters of the
            // application (controls... ) is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // llamamos al metodo que realiza las tareas previes al cierre de la aplicacion
            e.Cancel = !CloseApplication();

        }//mainForm_FormClosing

        /*******************************************************************************
        * @brief  Delegate for the click on the exit tool strip menu option
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void exitStripMenuItem_Click(object sender, EventArgs e) {

            // calling Application.Exit also calls FormClosing
            Application.Exit();

        }//exitStripMenuItem5_Click

        /*******************************************************************************
        * @brief Delegate for the click on the clear log button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void clearLogButton_Click(object sender, EventArgs e) {

            textBox2.Text = "";

        }//button2_Click

        /*******************************************************************************
        * @brief Delegate for the click even in the clear song information textbox button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void clearInfoButton_Click(object sender, EventArgs e) {

            romInfoTextBox.Text = "";
            romTitleTextBox.Text = "";

        }//button3_Click

        /*******************************************************************************
        * @brief Delegate for the click on the open ROM file tool strip menu option
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void openToolStripRomMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            bool b_format_ok = false;
            bool b_folder_exists = false;
            string str_path = "";
            string str_aux = "";
            string str_aux2 = "";
            bool b_close_project = false;


            // before operating, the more recent value of the general configuration parameters of the
            // application (controls... ) is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // llama a la funcion que muestra el aviso al usuario preguntando si desa o no continuar 
            // dependiendo de si hay modificaciones pendientes de guardarse en disco o no.
            b_close_project = ConfirmCloseProject("There are pending modifications to save and they will be lost. Continue anyway?");
            if (b_close_project) {

                // initialize all structures before loading a new project
                // if (dpack_drivePack != null) dpack_drivePack.clear();

                // before displaying the dialog to load the file, the starting path for the search must be located. To do
                // this, check if the starting path has the correct format.
                b_format_ok = IsValidPath(configMgr.m_str_last_rom_file);
                if (b_format_ok == false) {

                    // if received path does not have the right format then set "C:"
                    openFileDialog.InitialDirectory = "c:\\";

                } else {

                    str_path = Path.GetDirectoryName(configMgr.m_str_last_rom_file) + "\\";
                    b_folder_exists = Directory.Exists(str_path);

                    if (!b_folder_exists) {

                        // if received path returns an error or if does not exist, then set "C:"
                        openFileDialog.InitialDirectory = "c:\\";

                    } else {

                        // si la ruta tomada como por defecto existe, entonces pone el path del FolderDialog apuntando a esta 
                        // para inicar la busqueda en esta.
                        openFileDialog.InitialDirectory = Path.GetDirectoryName(str_path);
                    }

                }//if

                // configure extensions and show the file / folder selection dialog
                openFileDialog.Filter = "drive pack files (*.drp)|*.drp|raw binary file (*.bin)|*.bin|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK) {

                    try {

                        // before operating, the more recent value of the general configuration parameters of the
                        // application (controls... ) is taken in order to work with the latest parameters set by the user.
                        UpdateConfigParametersWithAppState();
                        statusNLogs.SetAppBusy(true);

                        // informative message of the action that is going to be executed
                        str_aux = "Opening \"" + openFileDialog.FileName + "\\\" ROM file ...";
                        statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_OPEN_FILE + str_aux, false);

                        str_aux = openFileDialog.FileName;
                        str_aux2 = str_aux.ToLower();
                        if (str_aux2.EndsWith(".drp")) {

                            // if file ends with ".drp" then call the function that opens the file in DRP format 
                            ec_ret_val = dpack_drivePack.loadDRPFile(str_aux);

                        } else if (str_aux2.EndsWith(".bin")) {

                            // if file ends with ".bin" then call the function that opens the file in BIN format 
                            ec_ret_val = dpack_drivePack.loadBINFile(str_aux);

                        } else {

                            ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                        }//if

                        if (ec_ret_val.i_code < 0) {

                            // shows the file load error message in to the user and in the logs
                            str_aux = ec_ret_val.str_description + " Error opening \"" + str_aux + "\" ROM file.";
                            statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_OPEN_FILE + str_aux, true);

                        } else {

                            // keep the current file name
                            configMgr.m_str_cur_rom_file = openFileDialog.FileName;
                            configMgr.m_str_last_rom_file = openFileDialog.FileName;

                            // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                            hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                            hexb_romEditor.ByteProvider.ApplyChanges();

                            // show the message to the user with the result of the open file operation
                            str_aux = "ROM file \"" + openFileDialog.FileName + "\" succesfully loaded.";
                            statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_OPEN_FILE + str_aux, true);

                        }//if

                    } catch (Exception ex) {

                        MessageBox.Show("Error: could not open the specified ROM file");

                    }//try

                }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

            }// if (b_close_project)

            // update the content of all the controls with the loaded file
            UpdateInfoTabPageControls();
            UpdateCodeTabPageControls();

            // update application state and controls content according to current application configuration
            statusNLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//openToolStripRomMenuItem_Click

        /*******************************************************************************
        * @brief Delegate for the click on the save ROM AS file tool strip menu option
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void SaveRomAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            bool b_format_ok = false;
            bool b_folder_exists = false;
            string str_path = "";
            string str_aux = "";
            string str_aux2 = "";


            // before operating, the state of the general configuration parameters of the application
            // is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // antes de mostrar el dialogo donde establecer la ruta del proyecto, hay que localizar la ruta donde comenzar a
            // explorar, para ello mira si la ruta tomada como inicio de la busqueda tiene formato correcto
            b_format_ok = IsValidPath(configMgr.m_str_last_rom_file);
            if (b_format_ok == false) {

                // si la ruta seleccionada no tiene formato correcto, entonces se pone en "C:"
                saveFileDialog.InitialDirectory = "c:\\";

            } else {

                str_path = Path.GetDirectoryName(configMgr.m_str_last_rom_file) + "\\";
                b_folder_exists = Directory.Exists(str_path);

                if (!b_folder_exists) {

                    // si la ruta indicada retorna error o no existe,  entonces se pone en "C:"
                    saveFileDialog.InitialDirectory = "c:\\";

                } else {

                    // si la ruta tomada como por defecto existe, entonces pone el path del FolderDialog apuntando a esta 
                    // para inicar la busqueda en esta.
                    saveFileDialog.InitialDirectory = Path.GetDirectoryName(str_path);
                }

            }//if

            // se termina de configurar el dialogo de seleccion de carpeta / proyecto y se nuestra
            saveFileDialog.Filter = "drive pack file (*.drp)|*.drp|raw binary file (*.bin)|*.bin|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {

                try {

                    // before operating, the state of the general configuration parameters of the application
                    // is taken to work with the latest parameters set by the user.
                    UpdateConfigParametersWithAppState();
                    statusNLogs.SetAppBusy(true);

                    // informative message explaining  the actions that are going to be executed
                    str_aux = "Saving \"" + saveFileDialog.FileName + "\\\" ROM file ...";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_SAVE_FILE + str_aux, false);

                    str_aux = saveFileDialog.FileName;

                    // call to the corresponding save file function deppending on the file extension
                    str_aux2 = str_aux.ToLower();
                    if (str_aux2.EndsWith(".drp")) {

                        // if file ends with ".drp" then call the function that stores the file in DRP format 
                        ec_ret_val = dpack_drivePack.saveDRPFile(str_aux);

                    } else if (str_aux2.EndsWith(".bin")) {

                        // if file ends with ".bin" then call the function that stores the file in BIN format 
                        ec_ret_val = dpack_drivePack.saveBINFile(str_aux);

                    } else {

                        ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                    }//if                    

                    if (ec_ret_val.i_code < 0) {

                        // shows the file load error message in to the user and in the logs
                        str_aux = ec_ret_val.str_description + " Error saving \"" + str_aux + "\" ROM file.";
                        statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_SAVE_FILE + str_aux, true);

                    } else {

                        // keep the current file name
                        configMgr.m_str_cur_rom_file = saveFileDialog.FileName;
                        configMgr.m_str_last_rom_file = configMgr.m_str_cur_rom_file;

                        // show the message that informs that the file has been succesfully saved
                        str_aux = "ROM file \"" + configMgr.m_str_cur_rom_file + "\" succesfully saved.";
                        statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_SAVE_FILE + str_aux, true);

                    }//if

                } catch (Exception ex) {

                    MessageBox.Show("Error: could not save the specified ROM file.");

                }//try

            }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

            // update application state and controls content according to current application configuration
            statusNLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//saveRomAsToolStripMenuItem_Click

        /*******************************************************************************
        * @brief Delegate for the click on the save current ROM file tool strip menu option
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void saveRomToolStripMenuItem_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";
            string str_aux2 = "";

            if ((configMgr.m_str_cur_rom_file == "") || (!File.Exists(configMgr.m_str_cur_rom_file))) {

                // if the current file has not been yet saved or if the file does not exist then call to the "Save as..." function
                SaveRomAsToolStripMenuItem_Click(sender, e);

            } else {

                // before operating, the state of the general configuration parameters of the application
                // is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();
                statusNLogs.SetAppBusy(true);

                // informative message of the action is going to be executed
                str_aux = "Saving \"" + configMgr.m_str_cur_rom_file + "\\\" ROM file ...";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_SAVE_FILE + str_aux, false);

                // call to the corresponding save file function deppending on the file extension
                str_aux2 = configMgr.m_str_cur_rom_file.ToLower();
                if (str_aux2.EndsWith(".drp")) {

                    // if file ends with ".drp" then call the function that stores the file in DRP format 
                    ec_ret_val = dpack_drivePack.saveDRPFile(configMgr.m_str_cur_rom_file);

                } else if (str_aux2.EndsWith(".bin")) {

                    // if file ends with ".bin" then call the function that stores the file in BIN format 
                    ec_ret_val = dpack_drivePack.saveBINFile(configMgr.m_str_cur_rom_file);

                } else {

                    ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                }//if

                if (ec_ret_val.i_code < 0) {

                    // shows the error message to the user and in the logs
                    str_aux = ec_ret_val.str_description + " Error saving \"" + configMgr.m_str_cur_rom_file + "\" ROM file.";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_SAVE_FILE + str_aux, true);

                } else {

                    // keep the current file name
                    configMgr.m_str_last_rom_file = configMgr.m_str_cur_rom_file;

                    // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                    hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                    hexb_romEditor.ByteProvider.ApplyChanges();

                    // show the message that informs that the file has been succesfully saved
                    str_aux = "ROM file \"" + configMgr.m_str_cur_rom_file + "\" succesfully saved.";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_SAVE_FILE + str_aux, true);

                }//if

                // update application state and controls content according to current application configuration
                statusNLogs.SetAppBusy(false);
                UpdateAppWithConfigParameters(true);

            }//if

        }//saveRomToolStripMenuItem_Click

        /*******************************************************************************
        * @brief delegate for the click on the About... tool strip menu option
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {

            showAboutDialog();

        }//aboutToolStripMenuItem_Click

        /*******************************************************************************
        * @brief delegate for the click on the Send (ROM to drivePACK) tool strip menu option
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void sendToolStripMenuItem_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;


            if (sendRomForm == null) {

                // before operating, the state of the general configuration parameters of the application
                // is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();

                statusNLogs.SetAppBusy(true);

                // show the send form
                sendRomForm = new SendForm();
                sendRomForm.parentRef = this;
                sendRomForm.statusLogsRef = statusNLogs;
                sendRomForm.drivePackRef = dpack_drivePack;
                sendRomForm.StartPosition = FormStartPosition.CenterScreen;
                sendRomForm.Show();

                // update application state and controls content according to current application configuration
                statusNLogs.SetAppBusy(false);
                UpdateAppWithConfigParameters(true);

            } else {
                MessageBox.Show("ERROR: window is already open");
            }//if

        }//toolStripMenuItem9_Click

        /*******************************************************************************
        * @brief delegate for the click on the Receive (ROM from drivePACK) tool strip menu
        * option.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void receiveToolStripMenuItem_Click(object sender, EventArgs e) {

            if (receiveRomForm == null) {

                receiveRomForm = new ReceiveForm();
                receiveRomForm.parentRef = this;
                receiveRomForm.statusLogsRef = statusNLogs;
                receiveRomForm.drivePackRef = dpack_drivePack;
                receiveRomForm.StartPosition = FormStartPosition.CenterScreen;
                receiveRomForm.Show();

            } else {
                MessageBox.Show("ERROR: window is already open");
            }//if

        }//receiveToolStripMenuItem_Click

        /*******************************************************************************
        * @brief delegate that manages the event that occurs when the user changes the
        * current Theme selection combo box.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void themeSelectComboBox_SelectionChangeCommitted(object sender, EventArgs e) {
            ThemeCode song = null;
            int iAux = 0;

            // before operating, the state of the general configuration parameters of the application
            // is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // update the selected Theme index to the new selected index
            iAux = themeSelectComboBox.SelectedIndex;
            if ((iAux >= 0) && (iAux < dpack_drivePack.themes.liThemesCode.Count())) {
                dpack_drivePack.themes.iCurrThemeIdx = iAux;
            }

            // update all the controls to match current structures content
            UpdateCodeTabPageControls();

        }//themeSelectComboBox_SelectionChangeCommitted

        /*******************************************************************************
        * @brief delegate that manages the click on the add entry to M1 channel button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addM1EntryButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<DataGridViewRow> liSelRows = null;
            int iSongIdx = 0;
            int iInstrIdx = 0;
            int iAux2 = 0;
            MChannelCodeEntry melodyCodeEntryAux = null;


            // check if there is any song selected and that M1 channel dataGridView has not reached the maximum allowed number of melody instructions
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM1DataGridView.Rows.Count >= cDrivePack.MAX_ROWS_PER_CHANNEL)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeM1DataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                if (liSelRows.Count == 0) {

                    // if the channel does not contain any instruction or if there are no instrucions selected just add the instructions at the end
                    iInstrIdx = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr.Count;

                } else {

                    // if there are instructions selected get the lowest index of all selected rows and add the new instruction after it
                    iInstrIdx = Convert.ToInt32(liSelRows[0].Cells[IDX_COLUMN_M1_IDX].Value);
                    iInstrIdx = iInstrIdx + 1;//iInstrIdx+1 to insert after speficied element

                }//if

                melodyCodeEntryAux = new MChannelCodeEntry();
                dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr.Insert(iInstrIdx, melodyCodeEntryAux);

                // as we have added new elements in the list , update the index of all the instructions. As the index
                // have been ordered with .Sort the new index can be applied startig from the first index in the list
                for (iAux2 = iInstrIdx; iAux2 < dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr.Count; iAux2++) {
                    dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iAux2].Idx = iAux2;
                }

                // refresh the datagridview to refelect last changes
                UpdateControlsCodeM1();

                // keep selected the added instruction
                themeM1DataGridView.ClearSelection();
                themeM1DataGridView.Rows[iInstrIdx].Selected = true;

            }// if

        }//addM1EntryButton_Click

        /*******************************************************************************
        * @brief delegate that manages the click on the add entry to M2 channel button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addM2EntryButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<DataGridViewRow> liSelRows = null;
            int iSongIdx = 0;
            int iInstrIdx = 0;
            int iAux2 = 0;
            MChannelCodeEntry melodyCodeEntryAux = null;

            // check if there is any song selected and that M2 channel dataGridView has not reached the maximum allowed number of melody instructions
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM2DataGridView.Rows.Count >= cDrivePack.MAX_ROWS_PER_CHANNEL)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeM2DataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                if (liSelRows.Count == 0) {

                    // if the channel does not contain any instruction or if there are no instrucions selected just add the instructions at the end
                    iInstrIdx = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr.Count;

                } else {

                    // if there are instructions selected get the lowest index of all selected rows and add the new instruction after it
                    iInstrIdx = Convert.ToInt32(liSelRows[0].Cells[IDX_COLUMN_M2_IDX].Value);
                    iInstrIdx = iInstrIdx + 1;//iInstrIdx+1 to insert after speficied element

                }//if

                // sort the list with the indexs to the delete from lowest to greatest
                melodyCodeEntryAux = new MChannelCodeEntry();
                dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr.Insert(iInstrIdx, melodyCodeEntryAux);

                // as we have added new elements in the list , update the index of all the instructions. As the index
                // have been ordered with .Sort the new index can be applied startig from the first index in the list
                for (iAux2 = iInstrIdx; iAux2 < dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr.Count; iAux2++) {
                    dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iAux2].Idx = iAux2;
                }

                // refresh the datagridview to refelect last changes
                UpdateControlsCodeM2();

                // keep selected the added instruction
                themeM2DataGridView.ClearSelection();
                themeM2DataGridView.Rows[iInstrIdx].Selected = true;

            }// if

        }//addM2EntryButton_Click

        /*******************************************************************************
        * @brief delegate that manages the click on the button that adds a new to chord 
        * entry to channel button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addChordEntryButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<DataGridViewRow> liSelRows = null;
            int iSongIdx = 0;
            int iInstrIdx = 0;
            int iAux2 = 0;
            ChordChannelCodeEntry chordCodeEntryAux = null;


            // check if there is any song selected and that Chord channel dataGridView has not reached the maximum allowed number of melody instructions
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeChordDataGridView.Rows.Count >= cDrivePack.MAX_ROWS_PER_CHANNEL)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeChordDataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                if (liSelRows.Count == 0) {

                    // if the channel does not contain any instruction or if there are no instrucions selected just add the instructions at the end
                    iInstrIdx = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr.Count;

                } else {

                    // if there are instructions selected get the lowest index of all selected rows and add the new instruction after it
                    iInstrIdx = Convert.ToInt32(liSelRows[0].Cells[IDX_COLUMN_CH_IDX].Value);
                    iInstrIdx = iInstrIdx + 1;//iInstrIdx+1 to insert after speficied element

                }//if

                chordCodeEntryAux = new ChordChannelCodeEntry();
                dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr.Insert(iInstrIdx, chordCodeEntryAux);

                // as we have added new elements in the list , update the index of all the instructions. As the index
                // have been ordered with .Sort the new index can be applied startig from the first index in the list
                for (iAux2 = iInstrIdx; iAux2 < dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr.Count; iAux2++) {
                    dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iAux2].Idx = iAux2;
                }

                // refresh the datagridview to refelect last changes
                UpdateControlsCodeChords();

                // keep selected the added instruction
                themeChordDataGridView.ClearSelection();
                themeChordDataGridView.Rows[iInstrIdx].Selected = true;

            }// if

        }//addChordEntryButton_Click

        /*******************************************************************************
        * @brief delegate that manages the click on the button to delete selected entries
        * from M1 channel.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void delM1EntryButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<DataGridViewRow> liSelRows = null;
            int iSongIdx = 0;
            int iIdxToDelete = 0;
            int iAux = 0;

            // check if there is any song selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeM1DataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                // get the index of and delte all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liSelRows.Count > 0) {

                    // process each row in the selection
                    foreach (DataGridViewRow rowInList in liSelRows) {

                        // take the value of the Idx field from the datagriview row
                        iIdxToDelete = Convert.ToInt32(rowInList.Cells[IDX_COLUMN_M1_IDX].Value);

                        // delete the item with the same Idx value in the list of isntructions
                        MChannelCodeEntry item = dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr.SingleOrDefault(x => x.Idx == iIdxToDelete);
                        if (item != null) dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr.Remove(item);

                    }//foreach

                    // as we have deleted a elements in the list , update the index of all the instructions. As the index
                    // have been ordered with .Sort the new index can be applied startig from the first index in the list
                    for (iAux = 0; iAux < dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr.Count; iAux++) {
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr[iAux].Idx = iAux;
                    }

                    // refresh the datagridview to refelect last changes
                    UpdateControlsCodeM1();

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
            List<DataGridViewRow> liSelRows = null;
            int iSongIdx = 0;
            int iIdxToDelete = 0;
            int iAux = 0;

            // check if there is any song selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeM2DataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                // get the index of and delte all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liSelRows.Count > 0) {

                    // process each row in the selection
                    foreach (DataGridViewRow rowInList in liSelRows) {

                        // take the value of the Idx field from the datagriview row
                        iIdxToDelete = Convert.ToInt32(rowInList.Cells[IDX_COLUMN_M2_IDX].Value);

                        // delete the item with the same Idx value in the list of isntructions
                        MChannelCodeEntry item = dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr.SingleOrDefault(x => x.Idx == iIdxToDelete);
                        if (item != null) dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr.Remove(item);

                    }//foreach

                    // as we have deleted a elements in the list , update the index of all the instructions. As the index
                    // have been ordered with .Sort the new index can be applied startig from the first index in the list
                    for (iAux = 0; iAux < dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr.Count; iAux++) {
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr[iAux].Idx = iAux;
                    }

                }//if

                // refresh the datagridview to refelect last changes
                UpdateControlsCodeM2();

                // no instruction selected after deleting selected instructions
                themeM2DataGridView.ClearSelection();

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
            List<DataGridViewRow> liSelRows = null;
            int iSongIdx = 0;
            int iIdxToDelete = 0;
            int iAux = 0;

            // check if there is any song selected and if the Chord channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeChordDataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                // get the index of and delte all the instructions selected in the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (liSelRows.Count > 0) {

                    // process each row in the selection
                    foreach (DataGridViewRow rowInList in liSelRows) {

                        // take the value of the Idx field from the datagriview row
                        iIdxToDelete = Convert.ToInt32(rowInList.Cells[IDX_COLUMN_CH_IDX].Value);

                        // delete the item with the same Idx value in the list of isntructions
                        ChordChannelCodeEntry item = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr.SingleOrDefault(x => x.Idx == iIdxToDelete);
                        if (item != null) dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr.Remove(item);

                    }//foreach

                    // as we have deleted a elements in the list , update the index of all the instructions. As the index
                    // have been ordered with .Sort the new index can be applied startig from the first index in the list
                    for (iAux = 0; iAux < dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr.Count; iAux++) {
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iAux].Idx = iAux;
                    }

                }//if

                // refresh the datagridview to refelect last changes
                UpdateControlsCodeChords();

                // no instruction selected after deleting selected instructions
                themeChordDataGridView.ClearSelection();

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
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any song selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeM1DataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                if (liSelRows.Count > 1) {

                    // keep the idx of all selected items to keep the rows selected after finishing the swap operation             
                    liISeletionIdx = new List<int>();
                    for (iAux = 0; iAux < liSelRows.Count(); iAux++) {
                        liISeletionIdx.Add(Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M1_IDX].Value));
                    }

                    iAux2 = liSelRows.Count - 1;
                    for (iAux = 0; iAux < (int)(liSelRows.Count / 2); iAux++) {

                        iInstrIdx1 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M1_IDX].Value);
                        iInstrIdx2 = Convert.ToInt32(liSelRows[iAux2].Cells[IDX_COLUMN_M1_IDX].Value);

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

                    // refresh the datagridview to refelect last changes
                    UpdateControlsCodeM1();

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    themeM1DataGridView.ClearSelection();
                    foreach (int idxSwapped in liISeletionIdx) {
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
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any song selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeM2DataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                if (liSelRows.Count > 1) {

                    // keep the idx of all selected items to keep the rows selected after finishing the swap operation             
                    liISeletionIdx = new List<int>();
                    for (iAux = 0; iAux < liSelRows.Count(); iAux++) {
                        liISeletionIdx.Add(Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M2_IDX].Value));
                    }

                    iAux2 = liSelRows.Count - 1;
                    for (iAux = 0; iAux < (int)(liSelRows.Count / 2); iAux++) {

                        iInstrIdx1 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M2_IDX].Value);
                        iInstrIdx2 = Convert.ToInt32(liSelRows[iAux2].Cells[IDX_COLUMN_M2_IDX].Value);

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

                    // refresh the datagridview to refelect last changes
                    UpdateControlsCodeM2();

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    themeM2DataGridView.ClearSelection();
                    foreach (int idxSwapped in liISeletionIdx) {
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
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            ChordChannelCodeEntry chordCodeEntryAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any song selected and if the chords channel dataGridView has any instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeChordDataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                if (liSelRows.Count > 1) {

                    // keep the idx of all selected items to keep the rows selected after finishing the swap operation             
                    liISeletionIdx = new List<int>();
                    for (iAux = 0; iAux < liSelRows.Count(); iAux++) {
                        liISeletionIdx.Add(Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_CH_IDX].Value));
                    }

                    iAux2 = liSelRows.Count - 1;
                    for (iAux = 0; iAux < (int)(liSelRows.Count / 2); iAux++) {

                        iInstrIdx1 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_CH_IDX].Value);
                        iInstrIdx2 = Convert.ToInt32(liSelRows[iAux2].Cells[IDX_COLUMN_CH_IDX].Value);

                        // swap the content of the rows less the Idx:
                        // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                        chordCodeEntryAux = new ChordChannelCodeEntry();
                        chordCodeEntryAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0;
                        chordCodeEntryAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1;
                        chordCodeEntryAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr;

                        // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr;

                        // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0 = chordCodeEntryAux.By0;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1 = chordCodeEntryAux.By1;
                        dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr = chordCodeEntryAux.strDescr;

                        iAux2--;

                    }//for (iAux=0;

                    // refresh the datagridview to refelect last changes
                    UpdateControlsCodeChords();

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    themeChordDataGridView.ClearSelection();
                    foreach (int idxSwapped in liISeletionIdx) {
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
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any song selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeM1DataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                // first check that there is at least 1 row selected
                if (liSelRows.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = Convert.ToInt32(liSelRows[0].Cells[IDX_COLUMN_M1_IDX].Value);
                    if (iInstrIdx1 > 0) {

                        iInstrIdx1 = iInstrIdx1 - 1;

                        // keep the idx of all selected items to keep the rows selected after finishing the swap operation             
                        liISeletionIdx = new List<int>();
                        iAux2 = iInstrIdx1;
                        for (iAux = 0; iAux < liSelRows.Count(); iAux++) {
                            liISeletionIdx.Add(iAux2);
                            iAux2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M1_IDX].Value);
                        }

                        for (iAux = 0; iAux < (int)liSelRows.Count; iAux++) {

                            iInstrIdx2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M1_IDX].Value);

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

                        // refresh the datagridview to refelect last changes
                        UpdateControlsCodeM1();

                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeM1DataGridView.ClearSelection();
                        foreach (int idxSwapped in liISeletionIdx) {
                            themeM1DataGridView.Rows[idxSwapped].Selected = true; ;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if  

        }//btnUpM1Entry_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Down one position all selected M1 
        * Cahnnel instructions 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnDownM1Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any song selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM1DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeM1DataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                // first check that there is at least 1 row selected
                if (liSelRows.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = Convert.ToInt32(liSelRows[liSelRows.Count - 1].Cells[IDX_COLUMN_M1_IDX].Value);
                    if (iInstrIdx1 < (dpack_drivePack.themes.liThemesCode[iSongIdx].liM1CodeInstr.Count - 1)) {

                        iInstrIdx1 = iInstrIdx1 + 1;

                        // keep the idx of all selected items to keep the rows selected after finishing the swap operation             
                        liISeletionIdx = new List<int>();
                        iAux2 = iInstrIdx1;
                        for (iAux = (liSelRows.Count() - 1); iAux >= 0; iAux--) {
                            liISeletionIdx.Add(iAux2);
                            iAux2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M1_IDX].Value);
                        }

                        for (iAux = (int)(liSelRows.Count - 1); iAux >= 0; iAux--) {

                            iInstrIdx2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M1_IDX].Value);

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

                        // refresh the datagridview to refelect last changes
                        UpdateControlsCodeM1();

                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeM1DataGridView.ClearSelection();
                        foreach (int idxSwapped in liISeletionIdx) {
                            themeM1DataGridView.Rows[idxSwapped].Selected = true; ;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if  

        }//btnDownM1Entry_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Up one position all selected M1 
        * Cahnnel instructions 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnUpM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any song selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeM2DataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                // first check that there is at least 1 row selected
                if (liSelRows.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = Convert.ToInt32(liSelRows[0].Cells[IDX_COLUMN_M2_IDX].Value);
                    if (iInstrIdx1 > 0) {

                        iInstrIdx1 = iInstrIdx1 - 1;

                        // keep the idx of all selected items to keep the rows selected after finishing the swap operation             
                        liISeletionIdx = new List<int>();
                        iAux2 = iInstrIdx1;
                        for (iAux = 0; iAux < liSelRows.Count(); iAux++) {
                            liISeletionIdx.Add(iAux2);
                            iAux2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M2_IDX].Value);
                        }

                        for (iAux = 0; iAux < (int)liSelRows.Count; iAux++) {

                            iInstrIdx2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M2_IDX].Value);

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

                        // refresh the datagridview to refelect last changes
                        UpdateControlsCodeM2();

                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeM2DataGridView.ClearSelection();
                        foreach (int idxSelected in liISeletionIdx) {
                            themeM2DataGridView.Rows[idxSelected].Selected = true; ;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if  

        }//btnUpM2Entry_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Down one position all selected M2 
        * Cahnnel instructions 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnDownM2Entry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any song selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeM2DataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeM2DataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                // first check that there is at least 1 row selected
                if (liSelRows.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = Convert.ToInt32(liSelRows[liSelRows.Count - 1].Cells[IDX_COLUMN_M2_IDX].Value);
                    if (iInstrIdx1 < (dpack_drivePack.themes.liThemesCode[iSongIdx].liM2CodeInstr.Count - 1)) {

                        iInstrIdx1 = iInstrIdx1 + 1;

                        // keep the idx of all selected items to keep the rows selected after finishing the swap operation             
                        liISeletionIdx = new List<int>();
                        iAux2 = iInstrIdx1;
                        for (iAux = (liSelRows.Count() - 1); iAux >= 0; iAux--) {
                            liISeletionIdx.Add(iAux2);
                            iAux2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M2_IDX].Value);
                        }

                        for (iAux = (int)(liSelRows.Count - 1); iAux >= 0; iAux--) {

                            iInstrIdx2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_M2_IDX].Value);

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

                        // refresh the datagridview to refelect last changes
                        UpdateControlsCodeM2();

                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeM2DataGridView.ClearSelection();
                        foreach (int idxSelected in liISeletionIdx) {
                            themeM2DataGridView.Rows[idxSelected].Selected = true; ;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if

        }//btnDownM2Entry_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Up one position all selected M2 
        * Cahnnel instructions 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnUpChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            ChordChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any song selected and if the Chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeChordDataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                // first check that there is at least 1 row selected
                if (liSelRows.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = Convert.ToInt32(liSelRows[0].Cells[IDX_COLUMN_CH_IDX].Value);
                    if (iInstrIdx1 > 0) {

                        iInstrIdx1 = iInstrIdx1 - 1;

                        // keep the idx of all selected items to keep the rows selected after finishing the swap operation             
                        liISeletionIdx = new List<int>();
                        iAux2 = iInstrIdx1;
                        for (iAux = 0; iAux < liSelRows.Count(); iAux++) {
                            liISeletionIdx.Add(iAux2);
                            iAux2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_CH_IDX].Value);
                        }

                        for (iAux = 0; iAux < (int)liSelRows.Count; iAux++) {

                            iInstrIdx2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_CH_IDX].Value);

                            // swap the content of the rows less the Idx:
                            // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                            melodyCodeEntryAux = new ChordChannelCodeEntry();
                            melodyCodeEntryAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0;
                            melodyCodeEntryAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1;
                            melodyCodeEntryAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0 = melodyCodeEntryAux.By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1 = melodyCodeEntryAux.By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr = melodyCodeEntryAux.strDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // refresh the datagridview to refelect last changes
                        UpdateControlsCodeChords();

                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeChordDataGridView.ClearSelection();
                        foreach (int idxSelected in liISeletionIdx) {
                            themeChordDataGridView.Rows[idxSelected].Selected = true; ;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if  

        }//btnUpChordEntry_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the move Down one position all selected Chords 
        * Channel instructions 
        * button
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void btnDownChordEntry_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int iInstrIdx1 = 0;
            int iInstrIdx2 = 0;
            int iSongIdx = 0;

            // check if there is any song selected and if the Chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) && (themeChordDataGridView.Rows.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeChordDataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                // first check that there is at least 1 row selected
                if (liSelRows.Count > 0) {

                    // check that there is at less 1 free space over the selected themes to move them up 1 position
                    iInstrIdx1 = Convert.ToInt32(liSelRows[liSelRows.Count - 1].Cells[IDX_COLUMN_CH_IDX].Value);
                    if (iInstrIdx1 < (dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr.Count - 1)) {

                        iInstrIdx1 = iInstrIdx1 + 1;

                        // keep the idx of all selected items to keep the rows selected after finishing the swap operation             
                        liISeletionIdx = new List<int>();
                        iAux2 = iInstrIdx1;
                        for (iAux = (liSelRows.Count() - 1); iAux >= 0; iAux--) {
                            liISeletionIdx.Add(iAux2);
                            iAux2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_CH_IDX].Value);
                        }

                        for (iAux = (int)(liSelRows.Count - 1); iAux >= 0; iAux--) {

                            iInstrIdx2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_CH_IDX].Value);

                            // swap the content of the rows less the Idx:
                            // keep a temporary copy of the Instruction at iInstrIdx1 ( the Idx is not copied )
                            melodyCodeEntryAux = new MChannelCodeEntry();
                            melodyCodeEntryAux.By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0;
                            melodyCodeEntryAux.By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1;
                            melodyCodeEntryAux.strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr;

                            // overwrite the Instruction at iInstrIdx1 with the instruction at iInstrIdx2 ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By0 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].By1 = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx1].strDescr = dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr;

                            // overwrite the Instruction at iInstrIdx2 with the temporary copy of the intruction ( the Idx is not copied )
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By0 = melodyCodeEntryAux.By0;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].By1 = melodyCodeEntryAux.By1;
                            dpack_drivePack.themes.liThemesCode[iSongIdx].liChordCodeInstr[iInstrIdx2].strDescr = melodyCodeEntryAux.strDescr;

                            iInstrIdx1 = iInstrIdx2;

                        }//for (iAux=0;

                        // refresh the datagridview to refelect last changes
                        UpdateControlsCodeChords();

                        // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                        themeChordDataGridView.ClearSelection();
                        foreach (int idxSelected in liISeletionIdx) {
                            themeChordDataGridView.Rows[idxSelected].Selected = true; ;
                        }

                    }//if (iInstrIdx1 > 0)

                }//if

            }//if


        }//btnDownChordEntry_Click

        /*******************************************************************************
        * @brief Delegate for the click on the save CURRENT SONGS CODE AS tool strip menu 
        * option.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void saveSongsAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            bool b_format_ok = false;
            bool b_folder_exists = false;
            string str_path = "";
            string str_aux = "";
            string str_aux2 = "";

            // before operating, the state of the general configuration parameters of the application
            // are taken to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // antes de mostrar el dialogo donde establecer la ruta del proyecto, hay que localizar la ruta donde comenzar a
            // explorar, para ello mira si la ruta tomada como inicio de la busqueda tiene formato correcto
            b_format_ok = IsValidPath(configMgr.m_str_last_song_file);
            if (b_format_ok == false) {

                // si la ruta seleccionada no tiene formato correcto, entonces se pone en "C:"
                saveFileDialog.InitialDirectory = "c:\\";

            } else {

                str_path = Path.GetDirectoryName(configMgr.m_str_last_song_file) + "\\";
                b_folder_exists = Directory.Exists(str_path);

                if (!b_folder_exists) {

                    // si la ruta indicada retorna error o no existe,  entonces se pone en "C:"
                    saveFileDialog.InitialDirectory = "c:\\";

                } else {

                    // si la ruta tomada como por defecto existe, entonces pone el path del FolderDialog apuntando a esta 
                    // para inicar la busqueda en esta.
                    saveFileDialog.InitialDirectory = Path.GetDirectoryName(str_path);
                }

            }//if

            // se termina de configurar el dialogo de seleccion de carpeta / proyecto y se nuestra
            saveFileDialog.Filter = "Themes code file (*.cod)|*.cod|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {

                try {

                    statusNLogs.SetAppBusy(true);

                    // informative message explaining  the actions that are going to be executed
                    str_aux = "Saving \"" + saveFileDialog.FileName + "\\\" songs file ...";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_SAVE_FILE + str_aux, false);

                    str_aux = saveFileDialog.FileName;

                    // call to the corresponding save file function deppending on the file extension
                    str_aux2 = str_aux.ToLower();
                    if (str_aux2.EndsWith(".cod")) {

                        // if file ends with ".cod" then call the function that stores the file in "code" format 
                        ec_ret_val = dpack_drivePack.themes.saveCodeFile(str_aux);

                    } else {

                        ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                    }//if                    

                    if (ec_ret_val.i_code < 0) {

                        // shows the file load error message to the user and in the logs
                        str_aux = ec_ret_val.str_description + " Error saving \"" + str_aux + "\" songs file.";
                        statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_SAVE_FILE + str_aux, true);

                    } else {

                        // keep the current song file name
                        configMgr.m_str_cur_song_file = saveFileDialog.FileName;
                        configMgr.m_str_last_song_file = configMgr.m_str_cur_song_file;

                        // show the message that informs that the file has been succesfully saved
                        str_aux = "Songs file \"" + configMgr.m_str_cur_song_file + "\" succesfully saved.";
                        statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_SAVE_FILE + str_aux, true);

                    }//if

                } catch (Exception ex) {

                    MessageBox.Show("Error: could not save the specified songs file.");

                }//try

            }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

            // update application state and controls content according to current application configuration
            statusNLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//saveSongsAsToolStripMenuItem_Click

        /*******************************************************************************
        * @brief Delegate for the click on the save CURRENT SONGS CODE tool strip menu 
        * option.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void saveSongsToolStripMenuItem_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";
            string str_aux2 = "";

            if ((configMgr.m_str_cur_song_file == "") || (!File.Exists(configMgr.m_str_cur_song_file))) {

                // if the current file has not been yet saved or if the file does not exist then call to the "Save as..." function
                saveSongsAsToolStripMenuItem_Click(sender, e);

            } else {

                // informative message of the action is going to be executed
                str_aux = "Saving \"" + configMgr.m_str_cur_song_file + "\\\" songs file ...";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_SAVE_FILE + str_aux, false);

                // before operating, the more recent value of the general configuration parameters of the
                // application (controls... ) is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();
                statusNLogs.SetAppBusy(true);

                // call to the corresponding save file function deppending on the file extension
                str_aux = configMgr.m_str_cur_song_file.ToLower();
                if (str_aux.EndsWith(".cod")) {

                    // if file ends with ".cod" then call the function that stores the file in "code" format 
                    ec_ret_val = dpack_drivePack.themes.saveCodeFile(str_aux);

                } else {

                    ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                }//if      

                if (ec_ret_val.i_code < 0) {

                    // shows the error message to the user and in the logs
                    str_aux = ec_ret_val.str_description + " Error saving \"" + configMgr.m_str_cur_song_file + "\"songs file.";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_SAVE_FILE + str_aux, true);

                } else {

                    // keep the current file name
                    configMgr.m_str_last_song_file = configMgr.m_str_cur_song_file;

                    // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                    hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                    hexb_romEditor.ByteProvider.ApplyChanges();

                    // show the message that informs that the file has been succesfully saved
                    str_aux = "Songs file \"" + configMgr.m_str_cur_song_file + "\" succesfully saved.";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_SAVE_FILE + str_aux, true);

                }//if

                // update application state and controls content according to current application configuration
                statusNLogs.SetAppBusy(false);
                UpdateAppWithConfigParameters(true);

            }//if

        }//saveSongsToolStripMenuItem_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the open SONGS CODE tool strip menu option.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void openSongsToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            bool b_format_ok = false;
            bool b_folder_exists = false;
            string str_path = "";
            string str_aux = "";
            string str_aux2 = "";
            bool b_close_project = false;


            // before operating, the more recent value of the general configuration parameters of the
            // application (controls... ) is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // llama a la funcion que muestra el aviso al usuario preguntando si desa o no continuar 
            // dependiendo de si hay modificaciones pendientes de guardarse en disco o no.
            b_close_project = ConfirmCloseProject("There are pending modifications to save and they will be lost. Continue anyway?");
            if (b_close_project) {

                // antes de abrir el proyecto se cierra y reinician todas las estructuras etc.
                // if (dpack_drivePack != null) dpack_drivePack.clear();

                // before displaying the dialog to load the file, the starting path for the search must be located. To do
                // this, check if the starting path has the correct format.
                b_format_ok = IsValidPath(configMgr.m_str_last_song_file);
                if (b_format_ok == false) {

                    // if received path does not have the right format then set "C:"
                    openFileDialog.InitialDirectory = "c:\\";

                } else {

                    str_path = Path.GetDirectoryName(configMgr.m_str_last_song_file) + "\\";
                    b_folder_exists = Directory.Exists(str_path);

                    if (!b_folder_exists) {

                        // if received path returns an error or if does not exist, then set "C:"
                        openFileDialog.InitialDirectory = "c:\\";

                    } else {

                        // si la ruta tomada como por defecto existe, entonces pone el path del FolderDialog apuntando a esta 
                        // para inicar la busqueda en esta.
                        openFileDialog.InitialDirectory = Path.GetDirectoryName(str_path);
                    }

                }//if

                // se termina de configurar el dialogo de seleccion de carpeta / proyecto y se nuestra
                openFileDialog.Filter = "Themes code files (*.cod)|*.cod|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK) {

                    //try {

                    // informative message of the action that is going to be executed
                    str_aux = "Opening \"" + openFileDialog.FileName + "\\\" songs file ...";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_OPEN_FILE + str_aux, false);

                    // before operating, the more recent value of the general configuration parameters of the
                    // application (controls... ) is taken in order to work with the latest parameters set by the user.
                    UpdateConfigParametersWithAppState();
                    statusNLogs.SetAppBusy(true);

                    str_aux = openFileDialog.FileName;
                    str_aux2 = str_aux.ToLower();
                    if (str_aux2.EndsWith(".cod")) {

                        // if file ends with ".cod" then call the function that opens the songs file in COD format 
                        ec_ret_val = dpack_drivePack.themes.loadCodeFile(str_aux2);

                    } else {

                        ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                    }//if

                    if (ec_ret_val.i_code < 0) {

                        // shows the file load error message in to the user and in the logs
                        str_aux = ec_ret_val.str_description + " Error opening \"" + str_aux + "\" songs file.";
                        statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_OPEN_FILE + str_aux, true);

                    } else {

                        // keep the current file name
                        configMgr.m_str_cur_song_file = openFileDialog.FileName;
                        configMgr.m_str_last_song_file = openFileDialog.FileName;

                        // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                        hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                        hexb_romEditor.ByteProvider.ApplyChanges();

                        // muestra el mensaje informativo indicando que se ha abierto el fichero indicado
                        str_aux = "Songs file \"" + openFileDialog.FileName + "\" succesfully loaded.";
                        statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_OPEN_FILE + str_aux, true);

                    }//if

                    //} catch (Exception ex) {

                    //    MessageBox.Show("Error: could not open the specified songs file");

                    //}//try

                }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

            }// if (b_close_project)

            // update the content of all the controls with the loaded file
            UpdateInfoTabPageControls();
            UpdateCodeTabPageControls();

            // update application state and controls content according to current application configuration
            statusNLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//openSongsToolStripMenuItem_Click


        /*******************************************************************************
         * @brief Delegate for the click envent on the button that builds current themes 
         * code into ROM.
         * @param[in] sender reference to the object that raises the event
         * @param[in] e the information related to the event
         *******************************************************************************/
        private void buildButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            DialogResult dialogResult;
            string str_aux = "";


            // first check if exists any valid theme to build
            if (dpack_drivePack.themes.liThemesCode.Count <= 0) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                dialogResult = MessageBox.Show("Building current themes code into a single ROM will overwrite ROM editor current content. Do yo want to continue?", "Build current themes", MessageBoxButtons.YesNo);
                if (dialogResult != DialogResult.Yes) {
                    ec_ret_val = cErrCodes.ERR_OPERATION_CANCELLED;
                }

            }

            if (ec_ret_val.i_code >= 0) {

                // before operating, the state of the general configuration parameters of the application
                // is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();
                statusNLogs.SetAppBusy(true);

                // informative message of the action that is going to be executed
                str_aux = "Buidling \"" + dpack_drivePack.themes.info.strROMTitle + "\\\" themes into ROMPACK ...";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + str_aux, false);

                // call the method that organizes all the themes M1 M2 and chords channels code
                // into a ROM PACK binary to allow playing them in a real keyboard.
                ec_ret_val = dpack_drivePack.buildROMPACK();

                if (ec_ret_val.i_code < 0) {

                    // shows the file load error message in to the user and in the logs
                    str_aux = ec_ret_val.str_description + "Something failed while trying to build the ROMPACK content.";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_BUILD_ROM + str_aux, true);

                } else {

                    // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                    hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                    hexb_romEditor.ByteProvider.ApplyChanges();

                    // muestra el mensaje informativo indicando que se ha abierto el fichero indicado
                    str_aux = "ROMPACK has been succesfully built.";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + str_aux, true);

                }//if

            }

            // update the content of all the controls with the loaded file
            UpdateInfoTabPageControls();
            UpdateCodeTabPageControls();

            // update application state and controls content according to current application configuration
            statusNLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//buildButton_Click

        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void decodeButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            DialogResult dialogResult;
            string str_aux = "";

            if (ec_ret_val.i_code >= 0) {

                dialogResult = MessageBox.Show("Decoding ROM editor content will ovewrite current themes in the code editor. Continue?", "Decode ROM editor content", MessageBoxButtons.YesNo);
                if (dialogResult != DialogResult.Yes) {
                    ec_ret_val = cErrCodes.ERR_OPERATION_CANCELLED;
                }

            }//if

            if (ec_ret_val.i_code >= 0) {

                // before operating, the state of the general configuration parameters of the application
                // is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();
                statusNLogs.SetAppBusy(true);

                // update the channels structures of the current song with the content in the
                // M1, M2 and chord DataGridViews before changing the selected theme
                // UpdateCodeChannelsWithDataGridView();

                // informative message of the action that is going to be executed
                str_aux = "Decoding \"" + dpack_drivePack.themes.info.strROMTitle + "\\\" ROM content ...";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + str_aux, false);

                // call the method that extracts the themes from the ROM PACK content and translates the bytes 
                // to the M1, M2 and Chord code channels instructions sequences
                ec_ret_val = dpack_drivePack.decodeROMPACKtoSongThemes();

                if (ec_ret_val.i_code < 0) {

                    // shows the error information
                    str_aux = ec_ret_val.str_description + " Error decoding the ROM PACK  \"" + dpack_drivePack.themes.info.strROMTitle + "\" content.";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_DECODE_ROM + str_aux, true);

                } else {

                    // show the message to the user with the result of the ROM PACK content decode operation
                    str_aux = ec_ret_val.str_description + " ROM PACK \"" + dpack_drivePack.themes.info.strROMTitle + "\" content succesfully decoded.";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + str_aux, true);

                }//if

            }// if (ec_ret_val.i_code >= 0) {

            // update the content of all the controls with the decoded information
            UpdateInfoTabPageControls();
            UpdateCodeTabPageControls();

            // update application state and controls content according to current application configuration
            statusNLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//decodeButton_Click

        /*******************************************************************************
        * @brief Manages the event when the user clicks to parse the Melody and Chord 
        * channels isntructions.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void parseThemeButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            MChannelCodeEntry melodyCodeEntryAux = null;
            ChordChannelCodeEntry chordCodeEntryAux = null;
            DialogResult dialogResult;
            int iAux = 0;
            int iCurrThemeIdx = 0;
            int iNumInstructions = 0;
            string strAux = "";


            // check if there is any theme selected to be deleted
            if ((dpack_drivePack.themes.iCurrThemeIdx < 0) || (dpack_drivePack.themes.liThemesCode.Count <= 0)) {
                ec_ret_val = cErrCodes.ERR_NO_THEME_SELECTED;
            }

            if (ec_ret_val.i_code >= 0) {

                iAux = dpack_drivePack.themes.iCurrThemeIdx;
                strAux = "[" + dpack_drivePack.themes.iCurrThemeIdx.ToString() + "] \"" + dpack_drivePack.themes.info.liTitles[iAux].Title + "\"";

                dialogResult = MessageBox.Show("The custom content of the description field in the instructions of current " + strAux + " theme will be lost. Continue?", "Update theme", MessageBoxButtons.YesNo);
                if (dialogResult != DialogResult.Yes) {
                    ec_ret_val = cErrCodes.ERR_OPERATION_CANCELLED;
                }

            }

            if (ec_ret_val.i_code >= 0) {

                // before operating, the state of the general configuration parameters of the application
                // is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();

                iCurrThemeIdx = dpack_drivePack.themes.iCurrThemeIdx;

                // parse all M1 channel entries
                iAux = 0;
                iNumInstructions = dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liM1CodeInstr.Count;
                while ((iAux < iNumInstructions) && (ec_ret_val.i_code >= 0)) {
                    melodyCodeEntryAux = dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liM1CodeInstr[iAux];
                    melodyCodeEntryAux.Parse();

                    iAux++;
                }

                // parse all M2 channel entries
                iAux = 0;
                iNumInstructions = dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liM2CodeInstr.Count;
                while ((iAux < iNumInstructions) && (ec_ret_val.i_code >= 0)) {
                    melodyCodeEntryAux = dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liM2CodeInstr[iAux];
                    melodyCodeEntryAux.Parse();

                    iAux++;
                }

                // parse all Chord channel entries
                iAux = 0;
                iNumInstructions = dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liChordCodeInstr.Count;
                while ((iAux < iNumInstructions) && (ec_ret_val.i_code >= 0)) {
                    chordCodeEntryAux = dpack_drivePack.themes.liThemesCode[iCurrThemeIdx].liChordCodeInstr[iAux];
                    chordCodeEntryAux.Parse();

                    iAux++;
                }

                // i_aux = dpack_drivePack.themes.iCurrThemeIdx;
                // str_aux = dpack_drivePack.themes.liThemesCode[i_aux].strThemeTitle;
                // dpack_drivePack.themes.DeleteTheme(dpack_drivePack.themes.iCurrThemeIdx);
                // 
                // // informative message for the user 
                // str_aux = "Deleted theme " + str_aux + " from position " + i_aux + " in the themes list.";
                // statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + str_aux, false);

                // update the content of the code tab page controls with the data in the loaded file
                UpdateCodeTabPageControls();

            }//if

        }//parseThemeButton_Click

        // JBR 2024-05-07 Revisar si hay que quitar este metodo y los controles asociados
        /*******************************************************************************
        * @brief Manages the event when the user clicks to recursively process all the files
        * in a folder
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void butnRecurse_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowserDialog1;
            string strPath = "";

            folderBrowserDialog1 = new FolderBrowserDialog();

            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                strPath = folderBrowserDialog1.SelectedPath;
                processPath(strPath);

            }

        }//butnRecurse_Click
        // FIN JBR 2024-05-07 Revisar si hay que quitar este metodo y el codigo asociado

        /*******************************************************************************
        * @brief  Delegate that processes the DoubleClick event in the Titles DataGridView
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void themeTitlesDataGridView_DoubleClick(object sender, EventArgs e) {
            int i_aux = 0;

            if ((dpack_drivePack.themes.info.liTitles.Count > 0) && (themeTitlesDataGridView.SelectedRows.Count > 0)) {

                // take the lowest index of the selected rows as the current selected theme index
                i_aux = themeTitlesDataGridView.SelectedRows[0].Index;
                foreach (DataGridViewRow row in themeTitlesDataGridView.SelectedRows) {
                    if (row.Index < i_aux) {
                        i_aux = row.Index;
                    }
                }
                dpack_drivePack.themes.iCurrThemeIdx = i_aux;

                // as the current selected theme has changed the controls must be updtated to
                // show the information of the current selected theme
                UpdateInfoTabPageControls();

                tabControlMain.SelectedTab = tabControlMain.TabPages[1];

            }//if

        }//themeTitlesDataGridView_DoubleClick

        /*******************************************************************************
        * @brief delegate for the click on the button that adds a new theme infromation 
        * intothe application structures.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addThemeButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<DataGridViewRow> liSelRows = null;
            string str_aux = "";
            int iThemeIdx = 0;


            if (themeTitlesDataGridView.SelectedRows.Count == 0) {

                // if the rom does not contain any theme or if there are no themes selected just add the new theme at the end
                iThemeIdx = dpack_drivePack.themes.info.liTitles.Count();

            } else {

                // if there are themes selected get the lowest index of all selected rows and add the new theme after it

                // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
                liSelRows = (from DataGridViewRow row in themeTitlesDataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

                iThemeIdx = Convert.ToInt32(liSelRows[0].Cells[IDX_COLUMN_THEME_IDX].Value);
                iThemeIdx = iThemeIdx + 1;


            }//if

            // add new theme in the themes structure just after the current selected theme
            ec_ret_val = dpack_drivePack.AddNewThemeAt(iThemeIdx);

            if (ec_ret_val.i_code >= 0) {

                // informative message for the user 
                str_aux = dpack_drivePack.themes.info.liTitles[iThemeIdx].Title;
                str_aux = "Added theme " + str_aux + " at position " + iThemeIdx + " in the themes list.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_EDITION + str_aux, false);

            } else {

                // informative message for the user 
                str_aux = "Error adding a new theme.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_EDITION + str_aux, false);

            }

            // update the content of all the info tab page controls with the info of the new theme
            UpdateInfoTabPageControls();

            // keep selected the adde theme
            themeTitlesDataGridView.ClearSelection();
            themeTitlesDataGridView.Rows[iThemeIdx].Selected = true;

        }//addThemeButton_Click

        /*******************************************************************************
        * @brief  Delegate for the click event on the button that removes from the structures
        * the inforation of the selected themes.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void delThemeButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<DataGridViewRow> liSelRows = null;
            int iIdxToDelete = 0;

            // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
            liSelRows = (from DataGridViewRow row in themeTitlesDataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

            // first check if that there are at least 1 rows selected to delete
            if (liSelRows.Count > 0) {

                // process each row in the selection
                foreach (DataGridViewRow row in liSelRows) {
                    iIdxToDelete = Convert.ToInt32(row.Cells[IDX_COLUMN_THEME_IDX].Value);
                    dpack_drivePack.DeleteAt(iIdxToDelete);
                }

                // update the content of all the info tab page controls with the info of the new theme
                UpdateInfoTabPageControls();

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
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            ThemeCode thmCodeAux = null;
            cThemeInfo themeTitleAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int themeIdx1 = 0;
            int themeIdx2 = 0;
            int iSongIdx = 0;

            iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

            // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
            liSelRows = (from DataGridViewRow row in themeTitlesDataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

            // first check if that there are at least 2 rows selected to swap
            if (liSelRows.Count > 1) {

                // keep the idx of all selected items to keep the rows selected after finishing the swap operation             
                liISeletionIdx = new List<int>();
                for (iAux = 0; iAux < liSelRows.Count(); iAux++) {
                    liISeletionIdx.Add(Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_THEME_IDX].Value));
                }

                // swap the selected rows
                iAux2 = liSelRows.Count - 1;
                for (iAux = 0; iAux < (int)(liSelRows.Count / 2); iAux++) {

                    themeIdx1 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_THEME_IDX].Value);
                    themeIdx2 = Convert.ToInt32(liSelRows[iAux2].Cells[IDX_COLUMN_THEME_IDX].Value);

                    // swap the content of the themes in the list
                    // keep a temporary copy of the theme at themeIdx1
                    ThemeCode.Clone(thmCodeAux, dpack_drivePack.themes.liThemesCode[themeIdx1]);

                    // overwrite the theme at themeIdx1 with the theme at themeIdx2
                    ThemeCode.Clone(dpack_drivePack.themes.liThemesCode[themeIdx1], dpack_drivePack.themes.liThemesCode[themeIdx2]);

                    // overwrite the theme at themeIdx1 with the theme at themeIdx2
                    ThemeCode.Clone(dpack_drivePack.themes.liThemesCode[themeIdx2], thmCodeAux);

                    // swap the information of the themes in the list
                    // keep a temporary copy of the theme info of theme at themeIdx1
                    themeTitleAux = new cThemeInfo();
                    themeTitleAux.Idx = dpack_drivePack.themes.info.liTitles[themeIdx1].Idx; // when swapping the index must not be copied!
                    themeTitleAux.Title = dpack_drivePack.themes.info.liTitles[themeIdx1].Title;

                    // overwrite the theme info of themeIdx1 with the theme info of themeIdx2 ( the index must not be copied! )
                    dpack_drivePack.themes.info.liTitles[themeIdx1].Title = dpack_drivePack.themes.info.liTitles[themeIdx2].Title;

                    // overwrite the theme info of themeIdx2 with the theme info of themeIdx1 ( the index must not be copied! )
                    dpack_drivePack.themes.info.liTitles[themeIdx2].Title = themeTitleAux.Title;

                    iAux2--;

                }//for (iAux=0;

                // update the content of all the info tab page controls with the info of the new theme
                UpdateInfoTabPageControls();

                // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                foreach (int idxSwapped in liISeletionIdx) {
                    themeTitlesDataGridView.Rows[idxSwapped].Selected = true; ;
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
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            ThemeCode thmCodeAux = null;
            cThemeInfo themeTitleAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int themeIdx1 = 0;
            int themeIdx2 = 0;
            int iSongIdx = 0;


            iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

            // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
            liSelRows = (from DataGridViewRow row in themeTitlesDataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

            // first check that there is at leats 1 row selected
            if (liSelRows.Count > 0) {

                // first check that there is at less 1 free space over the selected themes to move them up 1 position
                themeIdx1 = Convert.ToInt32(liSelRows[0].Cells[IDX_COLUMN_THEME_IDX].Value);
                if (themeIdx1 > 0) {

                    themeIdx1 = themeIdx1 - 1;

                    // keep the idx of all selected items to keep the rows selected after finishing the move up operation
                    liISeletionIdx = new List<int>();
                    iAux2 = themeIdx1;
                    for (iAux = 0; iAux < liSelRows.Count(); iAux++) {
                        liISeletionIdx.Add(iAux2);
                        iAux2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_THEME_IDX].Value);
                    }

                    for (iAux = 0; iAux < (int)liSelRows.Count; iAux++) {

                        themeIdx2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_THEME_IDX].Value);

                        // swap the content of the themes in the list
                        // keep a temporary copy of the theme at themeIdx1
                        ThemeCode.Clone(thmCodeAux, dpack_drivePack.themes.liThemesCode[themeIdx1]);

                        // overwrite the theme at themeIdx1 with the theme at themeIdx2
                        ThemeCode.Clone(dpack_drivePack.themes.liThemesCode[themeIdx1], dpack_drivePack.themes.liThemesCode[themeIdx2]);

                        // overwrite the theme at themeIdx1 with the theme at themeIdx2
                        ThemeCode.Clone(dpack_drivePack.themes.liThemesCode[themeIdx2], thmCodeAux);

                        // swap the information of the themes in the list
                        // keep a temporary copy of the theme info of theme at themeIdx1
                        themeTitleAux = new cThemeInfo();
                        themeTitleAux.Idx = dpack_drivePack.themes.info.liTitles[themeIdx1].Idx; // when swapping the index must not be copied!
                        themeTitleAux.Title = dpack_drivePack.themes.info.liTitles[themeIdx1].Title;

                        // overwrite the theme info of themeIdx1 with the theme info of themeIdx2 ( the index must not be copied! )
                        dpack_drivePack.themes.info.liTitles[themeIdx1].Title = dpack_drivePack.themes.info.liTitles[themeIdx2].Title;

                        // overwrite the theme info of themeIdx2 with the theme info of themeIdx1 ( the index must not be copied! )
                        dpack_drivePack.themes.info.liTitles[themeIdx2].Title = themeTitleAux.Title;

                        themeIdx1 = themeIdx2;

                    }//for (iAux=0;

                    // update the content of all the info tab page controls with the info of the new theme
                    UpdateInfoTabPageControls();

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    foreach (int idxSwapped in liISeletionIdx) {
                        themeTitlesDataGridView.Rows[idxSwapped].Selected = true; ;
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
            List<DataGridViewRow> liSelRows = null;
            List<int> liISeletionIdx = null;
            ThemeCode thmCodeAux = null;
            cThemeInfo themeTitleAux = null;
            int iAux = 0;
            int iAux2 = 0;
            int themeIdx1 = 0;
            int themeIdx2 = 0;
            int iSongIdx = 0;


            iSongIdx = dpack_drivePack.themes.iCurrThemeIdx;

            // TRICK: take the selected rows in the DataGridView in the currently display order ( not in the user selection order )
            liSelRows = (from DataGridViewRow row in themeTitlesDataGridView.SelectedRows where !row.IsNewRow orderby row.Index select row).ToList<DataGridViewRow>();

            // first check that there is at leats 1 row selected
            if (liSelRows.Count > 0) {

                // first check that there is at less 1 free space under the selected themes to move them down 1 position
                themeIdx1 = Convert.ToInt32(liSelRows[liSelRows.Count - 1].Cells[IDX_COLUMN_THEME_IDX].Value);
                if (themeIdx1 < (dpack_drivePack.themes.info.liTitles.Count - 1)) {

                    themeIdx1 = themeIdx1 + 1;

                    // keep the idx of all selected items to keep the rows selected after finishing the move down operation
                    liISeletionIdx = new List<int>();
                    iAux2 = themeIdx1;
                    for (iAux = liSelRows.Count() - 1; iAux >= 0; iAux--) {
                        liISeletionIdx.Add(iAux2);
                        iAux2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_THEME_IDX].Value);
                    }

                    for (iAux = (int)(liSelRows.Count - 1); iAux >= 0; iAux--) {

                        themeIdx2 = Convert.ToInt32(liSelRows[iAux].Cells[IDX_COLUMN_THEME_IDX].Value);

                        // swap the content of the themes in the list
                        // keep a temporary copy of the theme at themeIdx1
                        ThemeCode.Clone(thmCodeAux, dpack_drivePack.themes.liThemesCode[themeIdx1]);

                        // overwrite the theme at themeIdx1 with the theme at themeIdx2
                        ThemeCode.Clone(dpack_drivePack.themes.liThemesCode[themeIdx1], dpack_drivePack.themes.liThemesCode[themeIdx2]);

                        // overwrite the theme at themeIdx1 with the theme at themeIdx2
                        ThemeCode.Clone(dpack_drivePack.themes.liThemesCode[themeIdx2], thmCodeAux);

                        // swap the information of the themes in the list
                        // keep a temporary copy of the theme info of theme at themeIdx1
                        themeTitleAux = new cThemeInfo();
                        themeTitleAux.Idx = dpack_drivePack.themes.info.liTitles[themeIdx1].Idx; // when swapping the index must not be copied!
                        themeTitleAux.Title = dpack_drivePack.themes.info.liTitles[themeIdx1].Title;

                        // overwrite the theme info of themeIdx1 with the theme info of themeIdx2 ( the index must not be copied! )
                        dpack_drivePack.themes.info.liTitles[themeIdx1].Title = dpack_drivePack.themes.info.liTitles[themeIdx2].Title;

                        // overwrite the theme info of themeIdx2 with the theme info of themeIdx1 ( the index must not be copied! )
                        dpack_drivePack.themes.info.liTitles[themeIdx2].Title = themeTitleAux.Title;

                        themeIdx1 = themeIdx2;

                    }//for (iAux=0;

                    // update the content of all the info tab page controls with the info of the new theme
                    UpdateInfoTabPageControls();

                    // use the idx stored at the begining of the method to keep selected the rows that have been swaped
                    foreach (int idxSwapped in liISeletionIdx) {
                        themeTitlesDataGridView.Rows[idxSwapped].Selected = true;
                    }

                }//if

            }// if (themeTitlesDataGridView.SelectedRows.Count > 0)

        }//btDownTheme_Click

    }//class Form1 : Form

}// namespace drivePackEd
