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
using Microsoft.VisualBasic;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Runtime.Intrinsics.Arm;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Intrinsics.X86;

// Al hacer Receive or Send, hacer automaticamente Decode o Build, o preguntar al usuario?
// Meter teclas rapidas para las funciones de Copy Paste, Move Up Down etc.
// Al salir, avisar de que hay cambios pendientes de guardar...
// Al hacer Add o Copy Paste de Temas hay que comprobar si no se supera el máximo permitido ( los temas ya los he mirado y ya se mira el maximo )
// Flata mostrar el texto los ec_ret_value en las operaciones de Añadir, Pegar, Elimniar etc.
// ¿ Se deberia independizar el Build del Save y el Decode del Load ? Un fichero DRP contiene toda la informacion del titulo de la ROM, los titulos de los temas, la información general y el binario del cartucho. El codigo fuente se obtiene del binario del cartucho. ¿ Al hacer Save hay que hacer Build antes ? ¿Al hacer Load hay que hacer Decode dentro del propio Load o se deberia independicar el decode del Load o el Buil del Save ?
// Preguntar si queremos hacer Build antes de Guardar ? Preguntar si queremos hacer decode tras cargar ?
// ¿Puede ser interesante editar el codigo binario en el editor hexa y guardarlo directamente ( sin hacer Build para que no se machaquen los cambios hechos )? Quizas se podria usar la opción bin para ello, y que utilidad luego si no se pueden cargar bins en el drivePACK ?
// Faltan muhchos mensjaes de log indicando las operaciones realizadas por el usuario, sobre todo en las operaciones sobre los temas y sobre las instrucciones
// Revisar toda la gestión de las DataGridViews
// En los UpdateControlsCodeM1(); UpdateControlsCodeM2(); etc se hace el Binding de los datagridviews y estos se llaman siempre que se actualizan y es incorrecto puesto que el binding solo hay que hacerlo cuando se selecciona otro tema.
// Los Idx de los temas comienzan en "0" mientras que en los cartuchos y en los propios teclados cominezan en el indice "1"
// Las rutinas de reindexado tras borrar o insertar un elemento se pueden implementar como un método de la propia lista de instrucciones
// Mira si hay que reorganizar las opciones del Tool strip Files para que quede todo más organizado.
// Borrar tanto la ROM como las listas de temas al hacer New
// Al guardar un tema en un fichero .COD se pierden los comentarios propios.
// Mantener seleccionado el ultimo puerto serie utilizado para transferir si es que sigue existiendo.
// Al actualizar los controles con la info de las themes y Sheets se borran el texto de las entradas del ComboBox de sheets pero permanecen la lineas en blanco.
// Al cargar el fichero recibido este no se actualiza en el formulario.
// Si al recibir un fichero hacemos primero el Receive en el PC y luego el SEND en el ordenador el fichero no se envia.
// Hecho:
// Al borrar instrucciones no se actualiza el contador de instrucciones.
// Como afecta al tema activo (current theme) si borramos o añadimos un nuevo tema, o incluso si hemos borrado el que era el tema activo.
// Al editar el titulo de un tema en la dataGridView de temas el cambio no se traslada al ComboBox de temas
// Al hacer determinadas opreaciones de añadir, borrar, swap de instrucciones etc. se descentra todo

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

        // constants string used to access the theme sheet columns

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

        public const string HEX_FONT = "Segoe UI";//"Courier New";
        public const int HEX_SIZE = 9;
        public const string CODE_FONT = "Segoe UI";//"Courier New";
        public const int CODE_SIZE = 9;
        public const string TITLES_FONT = "Segoe UI";
        public const int TITLES_SIZE = 9;

        public SendForm sendRomForm = null;
        public ReceiveForm receiveRomForm = null;

        cLogsNErrors statusNLogs;
        cDrivePack dpack_drivePack;
        cConfig configMgr = new cConfig();
        HexBox hexb_romEditor = null;

        private SizeF szFormScaleFactor = new SizeF(1.0f, 1.0f);// sotres the scale factor that windows applies over all the controls to fit them into the configured OS scale configuration ( 100%, 125%, 200% ... )

        List<MChannelCodeEntry> liCopyMelodyTemporaryInstr; // list of instructions selected by the user to be copied, used for the Copy & Paste instructions
        List<ChordChannelCodeEntry> liCopyChordTemporaryInstr; // list of instructions selected by the user to be copied, used for the Copy & Paste instructions
        List<ThemeCode> liCopyTemporaryThemes; // list of instructions selected by the user to be copied, used for the Copy & Paste instructions

        bool bShowAboutOnLoad = false; // if true the About dialog box will be shown every time teh application starts

        /*******************************************************************************
        * @brief form class default constructor
        *******************************************************************************/
        public MainForm() {
            Point pointAux;
            float flAux;

            InitializeComponent();

            statusNLogs = new cLogsNErrors();
            dpack_drivePack = new cDrivePack(statusNLogs);
            liCopyMelodyTemporaryInstr = new List<MChannelCodeEntry>();
            liCopyChordTemporaryInstr = new List<ChordChannelCodeEntry>();
            liCopyTemporaryThemes = new List<ThemeCode>();

            // get the Windows Screen Scale Factor
            pointAux = new Point(2, 2);// (float)Screen.Bounds.Left,)
            flAux = (float)DPIUtil.ScaleFactor(this, pointAux);
            szFormScaleFactor = new SizeF(flAux, flAux);

        }//MainForm

        /*******************************************************************************
        * @brief Delegate that processes the MainForm load event 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void MainForm_Load(object sender, EventArgs e) {

            InitControls();

            if (bShowAboutOnLoad) {
                showAboutDialog();
            }

        }//MainForm_Load

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
        * @brief Delegate for the click even in the clear theme information textbox button
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
            DialogResult dialogResult;
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

                            // clear all the ROM and themes information before loading the new theme
                            dpack_drivePack.themes.Clear();

                            // if file ends with ".drp" then call the function that opens the file in DRP format 
                            ec_ret_val = dpack_drivePack.loadDRPFile(str_aux);

                        } else if (str_aux2.EndsWith(".bin")) {

                            // clear all the ROM and themes information before loading the new theme
                            dpack_drivePack.themes.Clear();

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

                            // dialogResult = MessageBox.Show("Do yo want to decode the binary content to get each channel's source code?", "Decode current ROM themes?", MessageBoxButtons.YesNo);
                            // if (dialogResult == DialogResult.Yes) {

                            // call the method that extracts the themes from the ROM PACK binary content and translates  
                            // the bytes to the M1, M2 and Chord code channels instructions sequences
                            ec_ret_val = dpack_drivePack.decodeROMPACKtoSongThemes();

                            //}

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
        private void saveRomAsToolStripMenuItem_Click(object sender, EventArgs e) {
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
                saveRomAsToolStripMenuItem_Click(sender, e);

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
            ThemeCode theme = null;
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
        * @brief Delegate for the click on the export selected themes SONGS CODE AS tool 
        * strip menu option.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void exportThemesAsToolStripMenuItem_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            List<int> liISelectionIdx = null;
            bool b_format_ok = false;
            bool b_folder_exists = false;
            string str_path = "";
            string str_aux = "";
            string str_aux2 = "";

            // before operating, the state of the general configuration parameters of the application
            // are taken to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // take the Index of the selected themes in the dataGridView 
            liISelectionIdx = new List<int>();
            foreach (DataGridViewRow rowAux in themeTitlesDataGridView.SelectedRows) {
                liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_THEME_IDX].Value));
            }
            liISelectionIdx.Sort();

            //  check if that there are at least 1 theme row selected to be exported 
            if (liISelectionIdx.Count == 0) {

                ec_ret_val = cErrCodes.ERR_FILE_NOT_TEMES_SELECTED_TO_EXPORT;

            }// if (liISelectionIdx.Count > 0)

            if (ec_ret_val.i_code >= 0) {

                // antes de mostrar el dialogo donde establecer la ruta del proyecto, hay que localizar la ruta donde comenzar a
                // explorar, para ello mira si la ruta tomada como inicio de la busqueda tiene formato correcto
                b_format_ok = IsValidPath(configMgr.m_str_last_theme_file);
                if (b_format_ok == false) {

                    // si la ruta seleccionada no tiene formato correcto, entonces se pone en "C:"
                    saveFileDialog.InitialDirectory = "c:\\";

                } else {

                    str_path = Path.GetDirectoryName(configMgr.m_str_last_theme_file) + "\\";
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
                        str_aux = "Saving \"" + saveFileDialog.FileName + "\\\" themes file ...";
                        statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_SAVE_FILE + str_aux, false);

                        str_aux = saveFileDialog.FileName;

                        // call to the corresponding save file function deppending on the file extension
                        str_aux2 = str_aux.ToLower();
                        if (str_aux2.EndsWith(".cod")) {

                            //  call the function that stores the file in "code" format 
                            ec_ret_val = dpack_drivePack.exportSelectedThemesToCodeFile(str_aux, liISelectionIdx);

                        } else {

                            ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                        }//if                    

                    } catch (Exception ex) {

                        ec_ret_val = cErrCodes.ERR_FILE_EXPORTING_SELECTED_FILES;

                    }//try

                }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

                // update application state and controls content according to current application configuration
                statusNLogs.SetAppBusy(false);
                UpdateAppWithConfigParameters(true);

            }//if (ec_ret_val.i_code>=0)

            if (ec_ret_val.i_code < 0) {

                // shows the file load error message to the user and in the logs
                str_aux = ec_ret_val.str_description + " Error exporting selected themes.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_SAVE_FILE + str_aux, true);

            } else {

                // keep the current theme file name
                configMgr.m_str_cur_theme_file = saveFileDialog.FileName;
                configMgr.m_str_last_theme_file = configMgr.m_str_cur_theme_file;

                // show the message that informs that the file has been succesfully saved
                str_aux = "Selected themes succesfully exported to \"" + configMgr.m_str_cur_theme_file + "\" file.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_SAVE_FILE + str_aux, true);

            }//if

        }//exportThemesAsToolStripMenuItem_Click

        /*******************************************************************************
        * @brief  Delegate for the click on the import CODE tool strip menu option.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void importCodeToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            List<int> liISelectionIdx = null;
            int iThemeIdx = -1;
            int iAux = 0;
            bool b_format_ok = false;
            bool b_folder_exists = false;
            string str_path = "";
            string str_aux = "";
            string str_aux2 = "";
            int iNumImportedThemes = 0;

            // before operating, the more recent value of the general configuration parameters of the
            // application (controls... ) is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // before displaying the dialog to load the file, the starting path for the search must be located. To do
            // this, check if the starting path has the correct format.
            b_format_ok = IsValidPath(configMgr.m_str_last_theme_file);
            if (b_format_ok == false) {

                // if received path does not have the right format then set "C:"
                openFileDialog.InitialDirectory = "c:\\";

            } else {

                str_path = Path.GetDirectoryName(configMgr.m_str_last_theme_file) + "\\";
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
            if (openFileDialog.ShowDialog() != DialogResult.OK) {

                ec_ret_val = cErrCodes.ERR_FILE_CANCELLED;

            } else {

                // informative message of the action that is going to be executed
                str_aux = "Opening \"" + openFileDialog.FileName + "\\\" themes code file ...";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_OPEN_FILE + str_aux, false);

                // before operating, the more recent value of the general configuration parameters of the
                // application (controls... ) is taken in order to work with the latest parameters set by the user.
                UpdateConfigParametersWithAppState();
                statusNLogs.SetAppBusy(true);

                str_aux = openFileDialog.FileName;
                str_aux2 = str_aux.ToLower();
                if (str_aux2.EndsWith(".cod")) {

                    if (themeTitlesDataGridView.SelectedRows.Count == 0) {

                        // if the rom does not contain any theme or if there are no themes selected just add the new theme at the end
                        iThemeIdx = dpack_drivePack.themes.liThemesCode.Count();

                    } else {

                        // if there are themes selected get the lowest index of all selected rows and add the new theme after it

                        // take the Index of the slected themes in the dataGridView 
                        liISelectionIdx = new List<int>();
                        foreach (DataGridViewRow rowAux in themeTitlesDataGridView.SelectedRows) {
                            liISelectionIdx.Add(Convert.ToInt32(rowAux.Cells[IDX_COLUMN_THEME_IDX].Value));
                        }
                        liISelectionIdx.Sort();

                        iThemeIdx = liISelectionIdx[0] + 1;

                    }//if

                    // if file ends with ".cod" then call the function that opens the themes file in COD format 
                    ec_ret_val = dpack_drivePack.importCodeFile(str_aux2, iThemeIdx, ref iNumImportedThemes);

                } else {

                    ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                }//if

            }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

            if (ec_ret_val.i_code >= 0) {

                // set the current theme index pointing to the first of the copied themes and then
                // bind/update the form controls to the current theme index
                SetCurrentThemeIdx(iThemeIdx);
                UpdateInfoTabPageControls();
                UpdateCodeTabPageControls();

                // use the idx calculated at the begining to keep selected the pasted themes
                themeTitlesDataGridView.ClearSelection();
                for (iAux = iThemeIdx; iAux < (iThemeIdx + iNumImportedThemes); iAux++) {
                    themeTitlesDataGridView.Rows[iAux].Selected = true;
                }

                // keep the current file name
                configMgr.m_str_cur_theme_file = openFileDialog.FileName;
                configMgr.m_str_last_theme_file = openFileDialog.FileName;

                // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                hexb_romEditor.ByteProvider = dpack_drivePack.dynbyprMemoryBytes;
                hexb_romEditor.ByteProvider.ApplyChanges();

                // muestra el mensaje informativo indicando que se ha abierto el fichero indicado
                str_aux = "themes file \"" + openFileDialog.FileName + "\" succesfully loaded.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_OPEN_FILE + str_aux, true);

            } else {

                // shows the file load error message in to the user and in the logs
                str_aux = ec_ret_val.str_description + " Error opening \"" + str_aux + "\" themes file.";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_OPEN_FILE + str_aux, true);

            }

            // update application state and controls content according to current application configuration
            statusNLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//importCodeToolStripMenuItem_Click

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
                str_aux = "Buidling \"" + dpack_drivePack.themes.strROMTitle + "\\\" themes into ROMPACK ...";
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

                // update the channels structures of the current theme with the content in the
                // M1, M2 and chord DataGridViews before changing the selected theme
                // UpdateCodeChannelsWithDataGridView();

                // informative message of the action that is going to be executed
                str_aux = "Decoding \"" + dpack_drivePack.themes.strROMTitle + "\\\" ROM content ...";
                statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + str_aux, false);

                // call the method that extracts the themes from the ROM PACK content and translates the bytes 
                // to the M1, M2 and Chord code channels instructions sequences
                ec_ret_val = dpack_drivePack.decodeROMPACKtoSongThemes();

                if (ec_ret_val.i_code < 0) {

                    // shows the error information
                    str_aux = ec_ret_val.str_description + " Error decoding the ROM PACK  \"" + dpack_drivePack.themes.strROMTitle + "\" content.";
                    statusNLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, cErrCodes.COMMAND_DECODE_ROM + str_aux, true);

                } else {

                    // show the message to the user with the result of the ROM PACK content decode operation
                    str_aux = ec_ret_val.str_description + " ROM PACK \"" + dpack_drivePack.themes.strROMTitle + "\" content succesfully decoded.";
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
                strAux = "[" + dpack_drivePack.themes.iCurrThemeIdx.ToString() + "] \"" + dpack_drivePack.themes.liThemesCode[iAux].Title + "\"";

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
            StreamWriter sWriterTextFile = null;
            string str_aux = "";
            FolderBrowserDialog folderBrowserDialog1 = null;
            string strPathIn = "";
            string strPathOut = "";
            string strSummaryFileName = "";
            DialogResult result;
            int i_aux = 0;
            int iIdx = 0;

            MessageBox.Show("Set the INput directory with the ROM files to convert.");

            // Show the FolderBrowserDialo to let the user select the directory
            // that contains the files to process
            folderBrowserDialog1 = new FolderBrowserDialog();
            result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) {

                strPathIn = folderBrowserDialog1.SelectedPath;

                MessageBox.Show("Set the OUTput directory where the converted ROM files and the summary file will be stored.");

                // Show the FolderBrowserDialog to let the user select the directory
                // where the files resulting of processing the input files will be stored
                folderBrowserDialog1 = new FolderBrowserDialog();
                result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK) {

                    strPathOut = folderBrowserDialog1.SelectedPath;

                    // create the header for the summary file table
                    strSummaryFileName = "summary.csv";
                    if ((strSummaryFileName != "") && (sWriterTextFile = File.AppendText(strPathOut + "\\" + strSummaryFileName)) != null) {
                        str_aux = "Idx;ROM Title; Theme #;Theme name; ROM extra information";
                        sWriterTextFile.WriteLine(str_aux);
                    }
                    sWriterTextFile.Close();

                    // process all the files in the selected path
                    iIdx = 0;
                    processPath(ref iIdx, strPathIn, strPathOut, "summary.csv");

                    MessageBox.Show("Input ROM files have been processed.");

                }//if

            }//if

        }//butnRecurse_Click

        /*******************************************************************************
        * @brief  Delegate that processes the DoubleClick event in the Titles DataGridView cells
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void themeTitlesDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e) {

            if ((dpack_drivePack.themes.liThemesCode.Count > 0) && (e.RowIndex >= 0)) {

                dpack_drivePack.themes.iCurrThemeIdx = e.RowIndex;

                // as the current selected theme has changed the controls that show the theme 
                // information must be updtated to show the information of the current selected theme
                UpdateInfoTabPageControls();
                UpdateCodeTabPageControls();

                // set the clicked Row as the current selected row
                themeTitlesDataGridView.Rows[e.RowIndex].Selected = true;

            }//if

        }//themeTitlesDataGridView_CellContentDoubleClick

        /*******************************************************************************
        * @brief Delegate that processes the event when the user modifies the title of any 
        * of the themes in the data grid view.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void themeTitlesDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e) {

            // Chords channel DataGridView: bind the chords channel of the current selected theme to the chord DataGridView
            UpdateCodeTabPageControls();

        }//themeTitlesDataGridView_CellEndEdit

        /*******************************************************************************
        * @brief Delegate that processes the event when the user changes the instruction
        * in the M1 selection combo box.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void cmboBoxM1Instr_SelectedValueChanged(object sender, EventArgs e) {

            UpdateInstructionEditionControls();

        }//cmboBoxM1Instr_SelectedValueChanged

        /*******************************************************************************
        * @brief Delegate that processes the event when the user changes the instruction
        * in the M2 selection combo box.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void cmboBoxM2Instr_SelectedValueChanged(object sender, EventArgs e) {

            UpdateInstructionEditionControls();

        }//cmboBoxM2Instr_SelectedValueChanged

        /*******************************************************************************
        * @brief Delegate that processes the event when the user changes the instruction
        * in the Chords selection combo box.
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void cmboBoxChordInstr_SelectedValueChanged(object sender, EventArgs e) {

            UpdateInstructionEditionControls();

        }//cmboBoxChordInstr_SelectedValueChanged

        /*******************************************************************************
        * @brief delegate for the click on the reportIssue tool strip menu option
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void reportIssueToolStripMenuItem_Click(object sender, EventArgs e) {
            string str_aux = "";

            str_aux = "http://www.tolaemon.com/contacto.htm";

            try {
                System.Diagnostics.Process.Start("explorer", str_aux);
            } catch (System.ComponentModel.Win32Exception noBrowser) {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            } catch (System.Exception other) {
                MessageBox.Show(other.Message);
            }

        }//reportIssueToolStripMenuItem_Click

    }//class Form1 : Form

}// namespace drivePackEd
