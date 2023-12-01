using System;
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

// Al actualizar los controles con la infor de las Songs y Sheets se borran el texto de las entradas del ComboBox de sheets pero permanecen la lineas en blanco.
// Al cargar el fichero recibido este no se actualiza en el formulario.
// Si al recibir un fichero hacemos primero el Receive en el PC y luego el SEND en el ordenador el fichero no se envia.

namespace drivePackEd {

    public partial class MainForm : Form {
        // strings with the opperations to show in the logs
        public const string COMMAND_OPEN_FILE = "OPEN_FILE: ";
        public const string COMMAND_SAVE_FILE = "SAVE_FILE: ";
        public const string COMMAND_SEND_FILE = "SEND_FILE: ";
        public const string COMMAND_RECEIVE_FILE = "RECEIVE_FILE: ";

        const int ROWS_HEADER_WDITH = 20; // header widht, that is before column '0'

        // constants string used to access the song sheet columns
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

        public SendForm sendRomForm = null;
        public ReceiveForm receiveRomForm = null;

        cDrivePackData dpack_drivePack = new cDrivePackData();
        cLogsNErrors StatusLogs = new cLogsNErrors();
        cConfig ConfigMgr = new cConfig();
        HexBox hexb_romEditor = null;


        /*******************************************************************************
        *  @brief form class default constructor
        *******************************************************************************/
        public MainForm() {

            InitializeComponent();
            InitControls();

        }//MainForm

        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {

            // before operating, the more recent value of the general configuration parameters of the
            // application (controls... ) is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // llamamos al metodo que realiza las tareas previes al cierre de la aplicacion
            e.Cancel = !CloseApplication();

        }//Form1_FormClosing
                

        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void button1_Click(object sender, EventArgs e) {
            int i_aux;
            long l_num_stored_bytes;

            // apply to the memory buffer the changes done into the the hex editor 
            dpack_drivePack.dynbyprMemoryBytes.ApplyChanges();
            l_num_stored_bytes = dpack_drivePack.dynbyprMemoryBytes.Length;

            if (l_num_stored_bytes > 300) l_num_stored_bytes = 300;

            for (i_aux = 0; i_aux < l_num_stored_bytes; i_aux++) {
                romInfoTextBox.Text = romInfoTextBox.Text + " 0x" + (dpack_drivePack.dynbyprMemoryBytes.Bytes[i_aux].ToString("X2"));
            }

        }//button1_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void toolStripMenuItem5_Click(object sender, EventArgs e) {

            // calling Application.Exit also calls FormClosing
            Application.Exit();

        }//toolStripMenuItem5_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void button2_Click(object sender, EventArgs e) {

            textBox2.Text = "";

        }//button2_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void button3_Click(object sender, EventArgs e) {

            romInfoTextBox.Text = "";
            romTitleTextBox.Text = "";

        }//button3_Click


        /*******************************************************************************
        * @brief 
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

                // antes de abrir el proyecto se cierra y reinician todas las estructuras etc.
                // if (dpack_drivePack != null) dpack_drivePack.clear();

                // before displaying the dialog to load the file, the starting path for the search must be located. To do
                // this, check if the starting path has the correct format.
                b_format_ok = IsValidPath(ConfigMgr.m_str_last_rom_file);
                if (b_format_ok == false) {

                    // if received path does not have the right format then set "C:"
                    openFileDialog.InitialDirectory = "c:\\";

                } else {

                    str_path = Path.GetDirectoryName(ConfigMgr.m_str_last_rom_file) + "\\";
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
                openFileDialog.Filter = "drive pack files (*.drp)|*.drp|raw binary file (*.bin)|*.bin|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK) {

                    try {

                        // informative message of the action that is going to be executed
                        str_aux = "Opening \"" + openFileDialog.FileName + "\\\" ROM file ...";
                        StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_OPEN_FILE + str_aux, false);

                        // before operating, the more recent value of the general configuration parameters of the
                        // application (controls... ) is taken in order to work with the latest parameters set by the user.
                        UpdateConfigParametersWithAppState();
                        StatusLogs.SetAppBusy(true);

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
                            StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, COMMAND_OPEN_FILE + str_aux, true);

                        } else {

                            // keep the current file name
                            ConfigMgr.m_str_cur_rom_file = openFileDialog.FileName;
                            ConfigMgr.m_str_last_rom_file = openFileDialog.FileName;

                            // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                            hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                            hexb_romEditor.ByteProvider.ApplyChanges();

                            // muestra el mensaje informativo indicando que se ha abierto el fichero indicado
                            str_aux = "ROM file \"" + openFileDialog.FileName + "\" succesfully loaded.";
                            StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_OPEN_FILE + str_aux, true);

                        }//if

                    } catch (Exception ex) {

                        MessageBox.Show("Error: could not open the specified ROM file");

                    }//try

                }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

            }// if (b_close_project)

            // update application state and controls content according to current application configuration
            StatusLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//openToolStripRomMenuItem_Click


        /*******************************************************************************
        * @brief 
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
            b_format_ok = IsValidPath(ConfigMgr.m_str_last_rom_file);
            if (b_format_ok == false) {

                // si la ruta seleccionada no tiene formato correcto, entonces se pone en "C:"
                saveFileDialog.InitialDirectory = "c:\\";

            } else {

                str_path = Path.GetDirectoryName(ConfigMgr.m_str_last_rom_file) + "\\";
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

                    // informative message explaining  the actions that are going to be executed
                    str_aux = "Saving \"" + saveFileDialog.FileName + "\\\" ROM file ...";
                    StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_SAVE_FILE + str_aux, false);

                    // before operating, the state of the general configuration parameters of the application
                    // is taken to work with the latest parameters set by the user.
                    UpdateConfigParametersWithAppState();
                    StatusLogs.SetAppBusy(true);

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
                        StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, COMMAND_SAVE_FILE + str_aux, true);

                    } else {

                        // keep the current file name
                        ConfigMgr.m_str_cur_rom_file = saveFileDialog.FileName;
                        ConfigMgr.m_str_last_rom_file = ConfigMgr.m_str_cur_rom_file;

                        // show the message that informs that the file has been succesfully saved
                        str_aux = "ROM file \"" + ConfigMgr.m_str_cur_rom_file + "\" succesfully saved.";
                        StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_SAVE_FILE + str_aux, true);

                    }//if

                } catch (Exception ex) {

                    MessageBox.Show("Error: could not save the specified ROM file.");

                }//try

            }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

            // update application state and controls content according to current application configuration
            StatusLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//saveRomAsToolStripMenuItem_Click


        private void saveRomToolStripMenuItem_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";
            string str_aux2 = "";

            if ((ConfigMgr.m_str_cur_rom_file == "") || (!File.Exists(ConfigMgr.m_str_cur_rom_file))) {

                // if the current file has not been yet saved or if the file does not exist then call to the "Save as..." function
                SaveRomAsToolStripMenuItem_Click(sender, e);

            } else {

                // informative message of the action is going to be executed
                str_aux = "Saving \"" + ConfigMgr.m_str_cur_rom_file + "\\\" ROM file ...";
                StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_SAVE_FILE + str_aux, false);

                // before operating, the state of the general configuration parameters of the application
                // is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();
                StatusLogs.SetAppBusy(true);

                // call to the corresponding save file function deppending on the file extension
                str_aux2 = ConfigMgr.m_str_cur_rom_file.ToLower();
                if (str_aux2.EndsWith(".drp")) {

                    // if file ends with ".drp" then call the function that stores the file in DRP format 
                    ec_ret_val = dpack_drivePack.saveDRPFile(ConfigMgr.m_str_cur_rom_file);

                } else if (str_aux2.EndsWith(".bin")) {

                    // if file ends with ".bin" then call the function that stores the file in BIN format 
                    ec_ret_val = dpack_drivePack.saveBINFile(ConfigMgr.m_str_cur_rom_file);

                } else {

                    ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                }//if

                if (ec_ret_val.i_code < 0) {

                    // shows the error message to the user and in the logs
                    str_aux = ec_ret_val.str_description + " Error saving \"" + ConfigMgr.m_str_cur_rom_file + "\" ROM file.";
                    StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, COMMAND_SAVE_FILE + str_aux, true);

                } else {

                    // keep the current file name
                    ConfigMgr.m_str_last_rom_file = ConfigMgr.m_str_cur_rom_file;

                    // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                    hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                    hexb_romEditor.ByteProvider.ApplyChanges();

                    // show the message that informs that the file has been succesfully saved
                    str_aux = "ROM file \"" + ConfigMgr.m_str_cur_rom_file + "\" succesfully saved.";
                    StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_SAVE_FILE + str_aux, true);

                }//if

                // update application state and controls content according to current application configuration
                StatusLogs.SetAppBusy(false);
                UpdateAppWithConfigParameters(true);

            }//if

        }//saveRomToolStripMenuItem_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            string str_aux = "";


            // set fmain orm title
            str_aux = cConfig.SW_TITLE + " - " + cConfig.SW_VERSION + "\r\nDrive pack files viewer and editor" + "\r\nJordi Bartolomé - Tolaemon 2022-07-27";
            MessageBox.Show(str_aux);

        }//aboutToolStripMenuItem_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void sendToolStripMenuItem_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;


            if (sendRomForm == null) {

                // before operating, the state of the general configuration parameters of the application
                // is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();

                // update the channels structures of the current song with the content in the
                // M1, M2 and chord DataGridViews before changing the selected song
                UpdateCodeChannelsWithDataGridView();

                StatusLogs.SetAppBusy(true);

                // show the send form
                sendRomForm = new SendForm();
                sendRomForm.parentRef = this;
                sendRomForm.statusLogsRef = StatusLogs;
                sendRomForm.drivePackRef = dpack_drivePack;
                sendRomForm.StartPosition = FormStartPosition.CenterScreen;
                sendRomForm.Show();

                // actualiza el estado de la aplicación con el estado actual de la configuracion general
                StatusLogs.SetAppBusy(false);
                UpdateAppWithConfigParameters(true);

            } else {
                MessageBox.Show("ERROR: window is already open");
            }//if

        }//toolStripMenuItem9_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void receiveToolStripMenuItem_Click(object sender, EventArgs e) {

            if (receiveRomForm == null) {
                receiveRomForm = new ReceiveForm();
                receiveRomForm.parentRef = this;
                receiveRomForm.statusLogsRef = StatusLogs;
                receiveRomForm.drivePackRef = dpack_drivePack;
                receiveRomForm.StartPosition = FormStartPosition.CenterScreen;
                receiveRomForm.Show();
            } else {
                MessageBox.Show("ERROR: window is already open");
            }//if

        }//receiveToolStripMenuItem_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addThemeButton_Click(object sender, EventArgs e) {

            // se actualizan las variables internas con lo establecido en la aplicación ( controles etc. )
            UpdateConfigParametersWithAppState();

            // update the channels structures of the current song with the content in the
            // M1, M2 and chord DataGridViews before changing the selected song
            UpdateCodeChannelsWithDataGridView();

            dpack_drivePack.allSeqs.InsertNewSequence(dpack_drivePack.allSeqs.iCurrSeqIdx + 1);

            UpdateControlsWithSongInfo();

        }//addThemeButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void themeSelectComboBox_SelectionChangeCommitted(object sender, EventArgs e) {
            ThemeCode song = null;
            int iAux = 0;

            // before operating, the state of the general configuration parameters of the application
            // is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // update the channels structures of the current song with the content in the
            // M1, M2 and chord DataGridViews before changing the selected song
            UpdateCodeChannelsWithDataGridView();

            // update the selected song index to the new selected index
            iAux = sequenceSelectComboBox.SelectedIndex;
            if ((iAux >= 0) && (iAux < dpack_drivePack.allSeqs.liSequences.Count())) {
                dpack_drivePack.allSeqs.iCurrSeqIdx = iAux;
            }

            // refresh all the song edition controls according to the new selected song
            UpdateControlsWithSongInfo();

        }//themeSelectComboBox_SelectionChangeCommitted


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addM1EntryButton_Click(object sender, EventArgs e) {
            int iIdx = -1;
            DataGridViewRow rowAux = null;
            int iAux = 0;

            // before operating, the state of the general configuration parameters of the application
            // is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // update the channels structures of the current song with the content in the
            // M1, M2 and chord DataGridViews before changing the selected song
            UpdateCodeChannelsWithDataGridView();

            // check if there is any song selected and that M1 channel dataGridView has not reached the maximum allowed number of melody instructions
            if ((dpack_drivePack.allSeqs.iCurrSeqIdx != -1) && (melody1DataGridView.Rows.Count < cDrivePackData.MAX_ROWS_PER_CHANNEL)) {

                // get the row index of the current selected cell/row of the dataGridView, and if 
                // there is no selected cell/row then take the last row index to insert it at the end
                if (melody1DataGridView.SelectedCells.Count > 0) {
                    iIdx = melody1DataGridView.SelectedCells[0].RowIndex;
                    iIdx++;// increase the index to insert the new element just after the selected row
                } else {
                    iIdx = melody1DataGridView.Rows.Count;
                }

                // insert the new row at the corresponding index
                rowAux = new DataGridViewRow();
                melody1DataGridView.Rows.Insert(iIdx, rowAux);
                rowAux = melody1DataGridView.Rows[iIdx];
                rowAux.Cells[0].Value = iIdx.ToString(IDX_COLUMN_M1_FORMAT);
                rowAux.Cells[1].Value = "00";
                rowAux.Cells[2].Value = "00";
                rowAux.Cells[3].Value = "00";
                rowAux.Cells[4].Value = " ";

                // as we have inserted a new row, update the index of all the melody instruction
                for (iAux = iIdx; iAux < melody1DataGridView.Rows.Count; iAux++) {
                    rowAux = melody1DataGridView.Rows[iAux];
                    rowAux.Cells[0].Value = iAux.ToString(IDX_COLUMN_M1_FORMAT);
                }

            }//if

        }//addM1EntryButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void delM1EntryButton_Click(object sender, EventArgs e) {
            int iIdx = -1;
            DataGridViewRow rowAux = null;
            int iAux = 0;

            // before operating, the state of the general configuration parameters of the application
            // is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // update the channels structures of the current song with the content in the
            // M1, M2 and chord DataGridViews before changing the selected song
            UpdateCodeChannelsWithDataGridView();

            // check if there is any song selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.allSeqs.iCurrSeqIdx != -1) && (melody1DataGridView.Rows.Count > 0)) {

                // remove the selected rows of the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (melody1DataGridView.SelectedRows.Count > 0) {
                    foreach (DataGridViewRow row in melody1DataGridView.SelectedRows) {
                        melody1DataGridView.Rows.RemoveAt(row.Index);
                    }
                }//if

                // as we have deleted a row, update the index of all the channel instructions
                for (iAux = 0; iAux < melody1DataGridView.Rows.Count; iAux++) {
                    rowAux = melody1DataGridView.Rows[iAux];
                    rowAux.Cells[0].Value = iAux.ToString(IDX_COLUMN_M1_FORMAT);
                }

            }// if

        }//delM1EntryButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void buildButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            DialogResult dialogResult;
            string str_aux = "";

            dialogResult = MessageBox.Show("Building current themes code into a single ROM will overwrite ROM editor current content. Do yo want to continue?", "Build current themes", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) {

                // before operating, the state of the general configuration parameters of the application
                // is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();

                // update the channels structures of the current song with the content in the
                // M1, M2 and chord DataGridViews before changing the selected song
                UpdateCodeChannelsWithDataGridView();

                // if file ends with ".bin" then call the function that opens the file in BIN format 
                ec_ret_val = dpack_drivePack.buildROMPACK();

                if (ec_ret_val.i_code < 0) {

                    // shows the file load error message in to the user and in the logs
                    str_aux = ec_ret_val.str_description + "Something failed while trying to build the ROMPACK content.";
                    StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, COMMAND_OPEN_FILE + str_aux, true);

                } else {

                    // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                    hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                    hexb_romEditor.ByteProvider.ApplyChanges();

                    // muestra el mensaje informativo indicando que se ha abierto el fichero indicado
                    str_aux = "ROMPACK has been succesfully built.";
                    StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_OPEN_FILE + str_aux, true);

                }//if

            }
            // else if (dialogResult == DialogResult.No) {
            //    //do something else
            //}

        }//buildButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void delSongButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            DialogResult dialogResult;
            string str_aux = "";

            dialogResult = MessageBox.Show("Current theme will be permanently deleted. Do yo want to continue?", "Delete theme", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) {

                // before operating, the state of the general configuration parameters of the application
                // is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();

                // update the channels structures of the current song with the content in the
                // M1, M2 and chord DataGridViews before deleteing the selected song
                UpdateCodeChannelsWithDataGridView();

                dpack_drivePack.allSeqs.DeleteSequence(dpack_drivePack.allSeqs.iCurrSeqIdx);

                UpdateControlsWithSongInfo();

            }
            // else if (dialogResult == DialogResult.No) {
            //    //do something else
            //}

        }//delSongButton_Click


        /*******************************************************************************
        * @brief 
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
            // is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // update the channels structures of the current song with the content in the
            // M1, M2 and chord DataGridViews before deleteing the selected song
            UpdateCodeChannelsWithDataGridView();

            // antes de mostrar el dialogo donde establecer la ruta del proyecto, hay que localizar la ruta donde comenzar a
            // explorar, para ello mira si la ruta tomada como inicio de la busqueda tiene formato correcto
            b_format_ok = IsValidPath(ConfigMgr.m_str_last_song_file);
            if (b_format_ok == false) {

                // si la ruta seleccionada no tiene formato correcto, entonces se pone en "C:"
                saveFileDialog.InitialDirectory = "c:\\";

            } else {

                str_path = Path.GetDirectoryName(ConfigMgr.m_str_last_song_file) + "\\";
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
            saveFileDialog.Filter = "sng songs file (*.sng)|*.sng|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {

                try {

                    // informative message explaining  the actions that are going to be executed
                    str_aux = "Saving \"" + saveFileDialog.FileName + "\\\" songs file ...";
                    StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_SAVE_FILE + str_aux, false);

                    // before operating, the state of the general configuration parameters of the application
                    // is taken to work with the latest parameters set by the user.
                    UpdateConfigParametersWithAppState();
                    StatusLogs.SetAppBusy(true);

                    str_aux = saveFileDialog.FileName;

                    // call to the corresponding save file function deppending on the file extension
                    str_aux2 = str_aux.ToLower();
                    if (str_aux2.EndsWith(".sng")) {

                        // if file ends with ".sng" then call the function that stores the file in sng format 
                        ec_ret_val = dpack_drivePack.allSeqs.saveSNGFile(str_aux);

                        //} else if (str_aux2.EndsWith(".txt")) {
                        //
                        //    // if file ends with ".txt" then call the function that stores the file in txt format 
                        //    ec_ret_val = dpack_drivePack.allSeqs.saveSNGFile(str_aux);
                        //
                    } else {

                        ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                    }//if                    

                    if (ec_ret_val.i_code < 0) {

                        // shows the file load error message to the user and in the logs
                        str_aux = ec_ret_val.str_description + " Error saving \"" + str_aux + "\" songs file.";
                        StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, COMMAND_SAVE_FILE + str_aux, true);

                    } else {

                        // keep the current song file name
                        ConfigMgr.m_str_cur_song_file = saveFileDialog.FileName;
                        ConfigMgr.m_str_last_song_file = ConfigMgr.m_str_cur_song_file;

                        // show the message that informs that the file has been succesfully saved
                        str_aux = "Songs file \"" + ConfigMgr.m_str_cur_song_file + "\" succesfully saved.";
                        StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_SAVE_FILE + str_aux, true);

                    }//if

                } catch (Exception ex) {

                    MessageBox.Show("Error: could not save the specified songs file.");

                }//try

            }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

            // update application state and controls content according to current application configuration
            StatusLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//saveSongsAsToolStripMenuItem_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void saveSongsToolStripMenuItem_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";
            string str_aux2 = "";

            if ((ConfigMgr.m_str_cur_song_file == "") || (!File.Exists(ConfigMgr.m_str_cur_song_file))) {

                // if the current file has not been yet saved or if the file does not exist then call to the "Save as..." function
                saveSongsAsToolStripMenuItem_Click(sender, e);

            } else {

                // update the channels structures of the current song with the content in the
                // M1, M2 and chord DataGridViews before deleteing the selected song
                UpdateCodeChannelsWithDataGridView();

                // informative message of the action is going to be executed
                str_aux = "Saving \"" + ConfigMgr.m_str_cur_song_file + "\\\" songs file ...";
                StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_SAVE_FILE + str_aux, false);

                // before operating, the more recent value of the general configuration parameters of the
                // application (controls... ) is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();
                StatusLogs.SetAppBusy(true);

                // call to the corresponding save file function deppending on the file extension
                str_aux = ConfigMgr.m_str_cur_song_file.ToLower();
                if (str_aux.EndsWith(".sng")) {

                    // if file ends with ".sng" then call the function that stores the file in sng format 
                    ec_ret_val = dpack_drivePack.allSeqs.saveSNGFile(str_aux);

                    //} else if (str_aux2.EndsWith(".txt")) {
                    //
                    //    // if file ends with ".txt" then call the function that stores the file in txt format 
                    //    ec_ret_val = dpack_drivePack.allSeqs.saveSNGFile(str_aux);
                    //
                } else {

                    ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                }//if      

                if (ec_ret_val.i_code < 0) {

                    // shows the error message to the user and in the logs
                    str_aux = ec_ret_val.str_description + " Error saving \"" + ConfigMgr.m_str_cur_song_file + "\"songs file.";
                    StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, COMMAND_SAVE_FILE + str_aux, true);

                } else {

                    // keep the current file name
                    ConfigMgr.m_str_last_song_file = ConfigMgr.m_str_cur_song_file;

                    // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                    hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                    hexb_romEditor.ByteProvider.ApplyChanges();

                    // show the message that informs that the file has been succesfully saved
                    str_aux = "Songs file \"" + ConfigMgr.m_str_cur_song_file + "\" succesfully saved.";
                    StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_SAVE_FILE + str_aux, true);

                }//if

                // update application state and controls content according to current application configuration
                StatusLogs.SetAppBusy(false);
                UpdateAppWithConfigParameters(true);

            }//if

        }//saveSongsToolStripMenuItem_Click


        /*******************************************************************************
        * @brief 
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
                b_format_ok = IsValidPath(ConfigMgr.m_str_last_song_file);
                if (b_format_ok == false) {

                    // if received path does not have the right format then set "C:"
                    openFileDialog.InitialDirectory = "c:\\";

                } else {

                    str_path = Path.GetDirectoryName(ConfigMgr.m_str_last_song_file) + "\\";
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
                openFileDialog.Filter = "Songs files (*.sng)|*.sng|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK) {

                    //try {

                    // informative message of the action that is going to be executed
                    str_aux = "Opening \"" + openFileDialog.FileName + "\\\" songs file ...";
                    StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_OPEN_FILE + str_aux, false);

                    // before operating, the more recent value of the general configuration parameters of the
                    // application (controls... ) is taken in order to work with the latest parameters set by the user.
                    UpdateConfigParametersWithAppState();
                    StatusLogs.SetAppBusy(true);

                    str_aux = openFileDialog.FileName;
                    str_aux2 = str_aux.ToLower();
                    if (str_aux2.EndsWith(".sng")) {

                        // if file ends with ".sng" then call the function that opens the songs file in SNG format 
                        ec_ret_val = dpack_drivePack.allSeqs.loadSNGFile(str_aux2);

                    } else {

                        ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                    }//if

                    if (ec_ret_val.i_code < 0) {

                        // shows the file load error message in to the user and in the logs
                        str_aux = ec_ret_val.str_description + " Error opening \"" + str_aux + "\" songs file.";
                        StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, COMMAND_OPEN_FILE + str_aux, true);

                    } else {

                        // keep the current file name
                        ConfigMgr.m_str_cur_song_file = openFileDialog.FileName;
                        ConfigMgr.m_str_last_song_file = openFileDialog.FileName;

                        // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                        hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                        hexb_romEditor.ByteProvider.ApplyChanges();

                        // muestra el mensaje informativo indicando que se ha abierto el fichero indicado
                        str_aux = "Songs file \"" + openFileDialog.FileName + "\" succesfully loaded.";
                        StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_OPEN_FILE + str_aux, true);

                    }//if

                    //} catch (Exception ex) {

                    //    MessageBox.Show("Error: could not open the specified songs file");

                    //}//try

                }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

            }// if (b_close_project)

            // update the content of the controls with the loaded file
            UpdateControlsWithSongInfo();

            // update application state and controls content according to current application configuration
            StatusLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//openSongsToolStripMenuItem_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addM2EntryButton_Click(object sender, EventArgs e) {
            int iIdx = -1;
            DataGridViewRow rowAux = null;
            int iAux = 0;

            // check if there is any song selected and that M2 channel dataGridView has not reached the maximum allowed number of melody instructions
            if ((dpack_drivePack.allSeqs.iCurrSeqIdx != -1) && (melody2DataGridView.Rows.Count < cDrivePackData.MAX_ROWS_PER_CHANNEL)) {

                // get the row index of the current selected cell/row of the dataGridView, and if 
                // there is no selected cell/row then take the last row index to insert it at the end
                if (melody2DataGridView.SelectedCells.Count > 0) {
                    iIdx = melody2DataGridView.SelectedCells[0].RowIndex;
                    iIdx++;// increase the index to insert the new element just after the selected row
                } else {
                    iIdx = melody2DataGridView.Rows.Count;
                }

                // insert the new row at the corresponding index
                rowAux = new DataGridViewRow();
                melody2DataGridView.Rows.Insert(iIdx, rowAux);
                rowAux = melody2DataGridView.Rows[iIdx];
                rowAux.Cells[0].Value = iIdx.ToString(IDX_COLUMN_M2_FORMAT);
                rowAux.Cells[1].Value = "00";
                rowAux.Cells[2].Value = "00";
                rowAux.Cells[3].Value = "00";
                rowAux.Cells[4].Value = " ";

                // as we have inserted a new row, update the index of all the melody instruction
                for (iAux = iIdx; iAux < melody2DataGridView.Rows.Count; iAux++) {
                    rowAux = melody2DataGridView.Rows[iAux];
                    rowAux.Cells[0].Value = iAux.ToString(IDX_COLUMN_M2_FORMAT);
                }

            }//if

        }//addM2EntryButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void addChordEntryButton_Click(object sender, EventArgs e) {
            int iIdx = -1;
            DataGridViewRow rowAux = null;
            int iAux = 0;

            // check if there is any song selected and that chords channel dataGridView has not reached the maximum allowed number of melody instructions
            if ((dpack_drivePack.allSeqs.iCurrSeqIdx != -1) && (chordsDataGridView.Rows.Count < cDrivePackData.MAX_ROWS_PER_CHANNEL)) {

                // get the row index of the current selected cell/row of the dataGridView, and if 
                // there is no selected cell/row then take the last row index to insert it at the end
                if (chordsDataGridView.SelectedCells.Count > 0) {
                    iIdx = chordsDataGridView.SelectedCells[0].RowIndex;
                    iIdx++;// increase the index to insert the new element just after the selected row
                } else {
                    iIdx = chordsDataGridView.Rows.Count;
                }

                // insert the new row at the corresponding index
                rowAux = new DataGridViewRow();
                chordsDataGridView.Rows.Insert(iIdx, rowAux);
                rowAux = chordsDataGridView.Rows[iIdx];
                rowAux.Cells[0].Value = iIdx.ToString(IDX_COLUMN_CH_FORMAT);
                rowAux.Cells[1].Value = "00";
                rowAux.Cells[2].Value = "00";
                rowAux.Cells[3].Value = " ";

                // as we have inserted a new row, update the index of all the melody instruction
                for (iAux = iIdx; iAux < chordsDataGridView.Rows.Count; iAux++) {
                    rowAux = chordsDataGridView.Rows[iAux];
                    rowAux.Cells[0].Value = iAux.ToString(IDX_COLUMN_CH_FORMAT);
                }

            }//if

        }//addChordEntryButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void delM2EntryButton_Click(object sender, EventArgs e) {
            int iIdx = -1;
            DataGridViewRow rowAux = null;
            int iAux = 0;

            // check if there is any song selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.allSeqs.iCurrSeqIdx != -1) && (melody2DataGridView.Rows.Count > 0)) {

                // remove the selected rows of the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (melody2DataGridView.SelectedRows.Count > 0) {
                    foreach (DataGridViewRow row in melody2DataGridView.SelectedRows) {
                        melody2DataGridView.Rows.RemoveAt(row.Index);
                    }
                }//if

                // as we have deleted a row, update the index of all the channel instructions
                for (iAux = 0; iAux < melody2DataGridView.Rows.Count; iAux++) {
                    rowAux = melody2DataGridView.Rows[iAux];
                    rowAux.Cells[0].Value = iAux.ToString(IDX_COLUMN_M2_FORMAT);
                }

            }// if

        }//delM2EntryButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void delChordEntryButton_Click(object sender, EventArgs e) {
            int iIdx = -1;
            DataGridViewRow rowAux = null;
            int iAux = 0;

            // check if there is any song selected and if the chords channel dataGridView has any melody instruction
            if ((dpack_drivePack.allSeqs.iCurrSeqIdx != -1) && (chordsDataGridView.Rows.Count > 0)) {

                // remove the selected rows of the datagridview ( dataGridView configured SelectionMode must be FullRowSelect! )
                if (chordsDataGridView.SelectedRows.Count > 0) {
                    foreach (DataGridViewRow row in chordsDataGridView.SelectedRows) {
                        chordsDataGridView.Rows.RemoveAt(row.Index);
                    }
                }//if

                // as we have deleted a row, update the index of all the channel instructions
                for (iAux = 0; iAux < chordsDataGridView.Rows.Count; iAux++) {
                    rowAux = chordsDataGridView.Rows[iAux];
                    rowAux.Cells[0].Value = iAux.ToString(IDX_COLUMN_CH_FORMAT);
                }

            }// if

        }//delChordEntryButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void decodeButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";

            // before operating, the more recent value of the general configuration parameters of the
            // application (controls... ) is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // update the channels structures of the current song with the content in the
            // M1, M2 and chord DataGridViews before changing the selected song
            UpdateCodeChannelsWithDataGridView();

            // if file ends with ".bin" then call the function that opens the file in BIN format 
            /*
            ec_ret_val = dpack_drivePack.buildROMPACK();

            if (ec_ret_val.i_code < 0) {

                // shows the file load error message in to the user and in the logs
                str_aux = ec_ret_val.str_description + "Something failed while trying to build the ROMPACK content.";
                StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, COMMAND_OPEN_FILE + str_aux, true);

            } else {

                // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                hexb_romEditor.ByteProvider.ApplyChanges();

                // muestra el mensaje informativo indicando que se ha abierto el fichero indicado
                str_aux = "ROMPACK has been succesfully built.";
                StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_OPEN_FILE + str_aux, true);

            }//if
            */

        }//decodeButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void swapM1EntriesButton_Click(object sender, EventArgs e) {
            string[] arrString;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any song selected and if the M1 channel dataGridView has any melody instruction
            if ((dpack_drivePack.allSeqs.iCurrSeqIdx != -1) && (melody1DataGridView.Rows.Count > 0)) {

                iAux2 = melody1DataGridView.SelectedRows.Count - 1;
                for (iAux = 0; iAux < (int)(melody1DataGridView.SelectedRows.Count / 2); iAux++) {

                    // swap the content of the rows less the Idx
                    arrString = new string[4];
                    arrString[0] = melody1DataGridView.SelectedRows[iAux2].Cells[1].Value.ToString();
                    arrString[1] = melody1DataGridView.SelectedRows[iAux2].Cells[2].Value.ToString();
                    arrString[2] = melody1DataGridView.SelectedRows[iAux2].Cells[3].Value.ToString();
                    arrString[3] = melody1DataGridView.SelectedRows[iAux2].Cells[4].Value.ToString();

                    melody1DataGridView.SelectedRows[iAux2].Cells[1].Value = melody1DataGridView.SelectedRows[iAux].Cells[1].Value;
                    melody1DataGridView.SelectedRows[iAux2].Cells[2].Value = melody1DataGridView.SelectedRows[iAux].Cells[2].Value;
                    melody1DataGridView.SelectedRows[iAux2].Cells[3].Value = melody1DataGridView.SelectedRows[iAux].Cells[3].Value;
                    melody1DataGridView.SelectedRows[iAux2].Cells[4].Value = melody1DataGridView.SelectedRows[iAux].Cells[4].Value;

                    melody1DataGridView.SelectedRows[iAux].Cells[1].Value = arrString[0];
                    melody1DataGridView.SelectedRows[iAux].Cells[2].Value = arrString[1];
                    melody1DataGridView.SelectedRows[iAux].Cells[3].Value = arrString[2];
                    melody1DataGridView.SelectedRows[iAux].Cells[4].Value = arrString[3];

                    iAux2--;

                }//for (iAux=0;

            }//if  

        }//swapM1EntriesButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void swaplM2EntriesButton_Click(object sender, EventArgs e) {
            string[] arrString;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any song selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.allSeqs.iCurrSeqIdx != -1) && (melody2DataGridView.Rows.Count > 0)) {

                iAux2 = melody2DataGridView.SelectedRows.Count - 1;
                for (iAux = 0; iAux < (int)(melody2DataGridView.SelectedRows.Count / 2); iAux++) {

                    // swap the content of the rows less the Idx
                    arrString = new string[4];
                    arrString[0] = melody2DataGridView.SelectedRows[iAux2].Cells[1].Value.ToString();
                    arrString[1] = melody2DataGridView.SelectedRows[iAux2].Cells[2].Value.ToString();
                    arrString[2] = melody2DataGridView.SelectedRows[iAux2].Cells[3].Value.ToString();
                    arrString[3] = melody2DataGridView.SelectedRows[iAux2].Cells[4].Value.ToString();

                    melody2DataGridView.SelectedRows[iAux2].Cells[1].Value = melody2DataGridView.SelectedRows[iAux].Cells[1].Value;
                    melody2DataGridView.SelectedRows[iAux2].Cells[2].Value = melody2DataGridView.SelectedRows[iAux].Cells[2].Value;
                    melody2DataGridView.SelectedRows[iAux2].Cells[3].Value = melody2DataGridView.SelectedRows[iAux].Cells[3].Value;
                    melody2DataGridView.SelectedRows[iAux2].Cells[4].Value = melody2DataGridView.SelectedRows[iAux].Cells[4].Value;

                    melody2DataGridView.SelectedRows[iAux].Cells[1].Value = arrString[0];
                    melody2DataGridView.SelectedRows[iAux].Cells[2].Value = arrString[1];
                    melody2DataGridView.SelectedRows[iAux].Cells[3].Value = arrString[2];
                    melody2DataGridView.SelectedRows[iAux].Cells[4].Value = arrString[3];

                    iAux2--;

                }//for (iAux=0;

            }//if  

        }//swaplM2EntriesButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void swapChordEntriesButton_Click(object sender, EventArgs e) {
            string[] arrString;
            int iAux = 0;
            int iAux2 = 0;

            // check if there is any song selected and if the M2 channel dataGridView has any melody instruction
            if ((dpack_drivePack.allSeqs.iCurrSeqIdx != -1) && (chordsDataGridView.Rows.Count > 0)) {

                iAux2 = chordsDataGridView.SelectedRows.Count - 1;
                for (iAux = 0; iAux < (int)(chordsDataGridView.SelectedRows.Count / 2); iAux++) {

                    // swap the content of the rows less the Idx
                    arrString = new string[4];
                    arrString[0] = chordsDataGridView.SelectedRows[iAux2].Cells[1].Value.ToString();
                    arrString[1] = chordsDataGridView.SelectedRows[iAux2].Cells[2].Value.ToString();
                    arrString[2] = chordsDataGridView.SelectedRows[iAux2].Cells[3].Value.ToString();

                    chordsDataGridView.SelectedRows[iAux2].Cells[1].Value = chordsDataGridView.SelectedRows[iAux].Cells[1].Value;
                    chordsDataGridView.SelectedRows[iAux2].Cells[2].Value = chordsDataGridView.SelectedRows[iAux].Cells[2].Value;
                    chordsDataGridView.SelectedRows[iAux2].Cells[3].Value = chordsDataGridView.SelectedRows[iAux].Cells[3].Value;

                    chordsDataGridView.SelectedRows[iAux].Cells[1].Value = arrString[0];
                    chordsDataGridView.SelectedRows[iAux].Cells[2].Value = arrString[1];
                    chordsDataGridView.SelectedRows[iAux].Cells[3].Value = arrString[2];

                    iAux2--;

                }//for (iAux=0;

            }//if

        }//swapChordEntriesButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void disassemblyButton_Click(object sender, EventArgs e) {
            DialogResult dialogResult;

            dialogResult = MessageBox.Show("Decoding ROM editor content will ovewrite current themes in the code editor. Continue?", "Decode ROM editor content", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) {


            }

        }//disassemblyButton_Click


    }//class Form1 : Form

}// namespace drivePackEd
