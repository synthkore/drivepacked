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


// Al cargar el fichero recibido este no se actualiza en el formulario.
// Si al recibir un fichero hacemos primero el Receive en el PC y luego el SEND en el ordenador el fichero no se envia.

namespace drivePackEd
{
    public partial class Form1 : Form
    {
        // strings with the opperations to show in the logs
        public const string COMMAND_OPEN_FILE = "OPEN_FILE: ";
        public const string COMMAND_SAVE_FILE = "SAVE_FILE: ";
        public const string COMMAND_SEND_FILE = "SEND_FILE: ";
        public const string COMMAND_RECEIVE_FILE = "RECEIVE_FILE: ";

        public Form2 sendRomForm = null;
        public Form3 receiveRomForm = null;

        cDrivePackData dpack_drivePack = new cDrivePackData();
        cLogsNErrors StatusLogs = new cLogsNErrors();
        cConfig ConfigMgr = new cConfig();
        HexBox hexb_romEditor = null;
 


        private void Form1_FormClosing(object sender, FormClosingEventArgs e){

            // update internal configuration parameters according to the state of the controls
            UpdateConfigParametersWithAppState();

            // llamamos al metodo que realiza las tareas previes al cierre de la aplicacion
            e.Cancel = !CloseApplication();

        }//Form1_FormClosing



        public Form1()
        {
            InitializeComponent();
            InitControls();
        }//Form1



        private void button1_Click(object sender, EventArgs e){
            int i_aux;
            long l_num_stored_bytes;

            // apply to the memory buffer the changes done into the the hex editor 
            dpack_drivePack.dynbypr_memory_bytes.ApplyChanges();
            l_num_stored_bytes = dpack_drivePack.dynbypr_memory_bytes.Length;

            if (l_num_stored_bytes > 300) l_num_stored_bytes = 300;

            for (i_aux = 0; i_aux < l_num_stored_bytes; i_aux++){
                textBox1.Text = textBox1.Text + " 0x" + (dpack_drivePack.dynbypr_memory_bytes.Bytes[i_aux].ToString("X2"));
            }            

        }//button1_Click



        private void toolStripMenuItem5_Click(object sender, EventArgs e){

            // calling Application.Exit also calls FormClosing
            Application.Exit();

        }//toolStripMenuItem5_Click



        private void button2_Click(object sender, EventArgs e){

            textBox2.Text = "";

        }//button2_Click



        private void button3_Click(object sender, EventArgs e){

            textBox1.Text = "";
            textBox3.Text = "";

        }//button3_Click



        private void toolStripMenuItem2_Click(object sender, EventArgs e){
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            bool b_format_ok = false;
            bool b_folder_exists = false;
            string str_path = "";
            string str_aux = "";
            string str_aux2 = "";
            bool b_close_project = false;


            // before operating, the state of the general configuration parameters of the application
            // is taken in order to work with the latest parameters set by the user.
            UpdateConfigParametersWithAppState();

            // llama a la funcion que muestra el aviso al usuario preguntando si desa o no continuar 
            // dependiendo de si hay modificaciones pendientes de guardarse en disco o no.
            b_close_project = ConfirmCloseProject("There are pending modifications to save and they will be lost. Continue anyway?");
            if (b_close_project) {

                // antes de abrir el proyecto se cierra y reinician todas las estructuras etc.
                // if (dpack_drivePack != null) dpack_drivePack.clear();

                // before displaying the dialog to load the file, the starting path for the search must be located. To do
                // this, check if the starting path has the correct format.
                b_format_ok = IsValidPath(ConfigMgr.m_str_last_file);
                if (b_format_ok == false) {

                    // if received path does not have the right format then set "C:"
                    openFileDialog.InitialDirectory = "c:\\";

                } else {

                    str_path = Path.GetDirectoryName(ConfigMgr.m_str_last_file) + "\\";
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

                    try
                    {

                        // informative message of the action that is going to be executed
                        str_aux = "Opening \"" + openFileDialog.FileName + "\\\" file ...";
                        StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_OPEN_FILE + str_aux, false);

                        // before operating, the state of the general configuration parameters of the application
                        // is taken to work with the latest parameters set by the user.
                        UpdateConfigParametersWithAppState();
                        StatusLogs.SetAppBusy(true);

                        str_aux = openFileDialog.FileName;
                        str_aux2 = str_aux.ToLower();
                        if (str_aux2.EndsWith(".drp")) {

                            // if file ends with ".drp" then call the function that opens the file in DRP format 
                            ec_ret_val = dpack_drivePack.loadDRPFile(str_aux);

                        }else if (str_aux2.EndsWith(".bin")) {

                            // if file ends with ".bin" then call the function that opens the file in BIN format 
                            ec_ret_val = dpack_drivePack.loadBINFile(str_aux);

                        }else{

                            ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                        }//if

                        if (ec_ret_val.i_code < 0) {

                            // shows the file load error message in to the user and in the logs
                            str_aux = ec_ret_val.str_description + " Error opening \"" + str_aux + "\" drive pack file.";
                            StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, COMMAND_OPEN_FILE + str_aux, true);

                        } else {

                            // keep the current file name
                            ConfigMgr.m_str_cur_file = openFileDialog.FileName;
                            ConfigMgr.m_str_last_file = openFileDialog.FileName;

                            // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                            hexb_romEditor.ByteProvider = dpack_drivePack.dynbypr_memory_bytes;
                            hexb_romEditor.ByteProvider.ApplyChanges();

                            // muestra el mensaje informativo indicando que se ha abierto el fichero indicado
                            str_aux = "File \"" + openFileDialog.FileName + "\" succesfully loaded.";
                            StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_OPEN_FILE + str_aux, true);

                        }//if

                    } catch (Exception ex) {

                        MessageBox.Show("Error: could not open the specified file");

                    }//try

                }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

            }// if (b_close_project)

            // update application state and controls content according to current application configuration
            StatusLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);

        }//toolStripMenuItem2_Click



        private void toolStripMenuItem4_Click(object sender, EventArgs e){
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            bool b_format_ok = false;
            bool b_folder_exists = false;
            string str_path = "";
            string str_aux = "";
            string str_aux2 = "";


            // se actualizan las variables internas con lo establecido en la aplicación ( controles etc. )
            UpdateConfigParametersWithAppState();

            // antes de mostrar el dialogo donde establecer la ruta del proyecto, hay que localizar la ruta donde comenzar a
            // explorar, para ello mira si la ruta tomada como inicio de la busqueda tiene formato correcto
            b_format_ok = IsValidPath(ConfigMgr.m_str_last_file);
            if (b_format_ok == false) {

                // si la ruta seleccionada no tiene formato correcto, entonces se pone en "C:"
                saveFileDialog.InitialDirectory = "c:\\";

            } else {

                str_path = Path.GetDirectoryName(ConfigMgr.m_str_last_file) + "\\";
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
                    str_aux = "Saving \"" + saveFileDialog.FileName + "\\\" file ...";
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

                    }else if (str_aux2.EndsWith(".bin")){

                        // if file ends with ".bin" then call the function that stores the file in BIN format 
                        ec_ret_val = dpack_drivePack.saveBINFile(str_aux);

                    }else{

                        ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                    }//if                    
                                        
                    if (ec_ret_val.i_code < 0) {

                        // shows the file load error message in to the user and in the logs
                        str_aux = ec_ret_val.str_description + " Error saving \"" + str_aux + "\" drive pack file.";
                        StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, COMMAND_SAVE_FILE + str_aux, true);

                    } else {

                        // keep the current file name
                        ConfigMgr.m_str_cur_file = saveFileDialog.FileName;
                        ConfigMgr.m_str_last_file = ConfigMgr.m_str_cur_file;

                        // show the message that informs that the file has been succesfully saved
                        str_aux = "File \"" + ConfigMgr.m_str_cur_file + "\" succesfully saved.";
                        StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_SAVE_FILE + str_aux, true);

                    }//if

                } catch (Exception ex) {

                    MessageBox.Show("Error: could not save the specified file.");

                }//try

            }//if (openFolderDialog.ShowDialog() == DialogResult.OK)

            // update application state and controls content according to current application configuration
            StatusLogs.SetAppBusy(false);
            UpdateAppWithConfigParameters(true);
        
        }//toolStripMenuItem4_Click


        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";
            string str_aux2 = "";

            if ( (ConfigMgr.m_str_cur_file == "") || (!File.Exists(ConfigMgr.m_str_cur_file)) ) {

                // if the current file has not been yet saved or if the file does not exist then call to the "Save as..." function
                toolStripMenuItem4_Click(sender, e);
            
            }else{

                // informative message of the action is going to be executed
                str_aux = "Saving \"" + ConfigMgr.m_str_cur_file + "\\\" file ...";
                StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_SAVE_FILE + str_aux, false);

                // before executing the command take the last state/value of parameters to operate with most recent values
                UpdateConfigParametersWithAppState();
                StatusLogs.SetAppBusy(true);

                // call to the corresponding save file function deppending on the file extension
                str_aux2 = ConfigMgr.m_str_cur_file.ToLower();
                if (str_aux2.EndsWith(".drp")) {

                    // if file ends with ".drp" then call the function that stores the file in DRP format 
                    ec_ret_val = dpack_drivePack.saveDRPFile(ConfigMgr.m_str_cur_file);

                }else if (str_aux2.EndsWith(".bin")){

                    // if file ends with ".bin" then call the function that stores the file in BIN format 
                    ec_ret_val = dpack_drivePack.saveBINFile(ConfigMgr.m_str_cur_file);

                }else{

                    ec_ret_val = cErrCodes.ERR_FILE_INVALID_TYPE;

                }//if

                if (ec_ret_val.i_code < 0){

                    // shows the error message to the user and in the logs
                    str_aux = ec_ret_val.str_description + " Error saving \"" + ConfigMgr.m_str_cur_file + "\" drive pack file.";
                    StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, COMMAND_SAVE_FILE + str_aux, true);

                }else{

                    // keep the current file name
                    ConfigMgr.m_str_last_file = ConfigMgr.m_str_cur_file;

                    // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
                    hexb_romEditor.ByteProvider = dpack_drivePack.dynbypr_memory_bytes;
                    hexb_romEditor.ByteProvider.ApplyChanges();

                    // show the message that informs that the file has been succesfully saved
                    str_aux = "File \"" + ConfigMgr.m_str_cur_file + "\" succesfully saved.";
                    StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, COMMAND_SAVE_FILE + str_aux, true);

                }//if

                // update application state and controls content according to current application configuration
                StatusLogs.SetAppBusy(false);
                UpdateAppWithConfigParameters(true);

            }//if

        }//toolStripMenuItem3_Click



        private void aboutToolStripMenuItem_Click(object sender, EventArgs e){
            string str_aux = "";


            // set fmain orm title
            str_aux = cConfig.SW_TITLE + " - " + cConfig.SW_VERSION + "\r\nDrive pack files viewer and editor" + "\r\nJordi Bartolomé - Tolaemon 2022-07-27";
            MessageBox.Show (str_aux);

        }//aboutToolStripMenuItem_Click



        private void toolStripMenuItem9_Click(object sender, EventArgs e){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";


            if (sendRomForm == null) { 

                // before executing the command take the last state/value of parameters to operate with most recent values
                UpdateConfigParametersWithAppState();
                StatusLogs.SetAppBusy(true);

                // show the send form
                sendRomForm = new Form2();
                sendRomForm.parentRef = this;
                sendRomForm.statusLogsRef = StatusLogs;
                sendRomForm.drivePackRef = dpack_drivePack;
                sendRomForm.StartPosition = FormStartPosition.CenterScreen;
                sendRomForm.Show();

                // actualiza el estado de la aplicación con el estado actual de la configuracion general
                StatusLogs.SetAppBusy(false);
                UpdateAppWithConfigParameters(true);

            }else{
                MessageBox.Show("ERROR: window is already open");
            }//if

        }//toolStripMenuItem9_Click



        private void toolStripMenuItem8_Click(object sender, EventArgs e){

           if (receiveRomForm == null) {
                receiveRomForm = new Form3();
                receiveRomForm.parentRef = this;
                receiveRomForm.statusLogsRef = StatusLogs;
                receiveRomForm.drivePackRef = dpack_drivePack;
                receiveRomForm.StartPosition = FormStartPosition.CenterScreen;
                receiveRomForm.Show();
            }else{
                MessageBox.Show("ERROR: window is already open");
            }//if
        
        }//toolStripMenuItem8_Click

    }//class Form1 : Form

}// namespace drivePackEd
