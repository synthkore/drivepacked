using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace drivePackEd
{

    public partial class Form1 : Form{

         /*******************************************************************************
         *  InitControls
         *------------------------------------------------------------------------------
         *  Description
         *  Parameters:
         *  Return: 
         *    By reference:
         *    By value:
         *******************************************************************************/
        public ErrCode InitControls(){
            string str_aux = "";
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;

            // loads the configuration parameters according to the last state of the application
            ec_ret_val = ConfigMgr.LoadConfigParameters();
            if (ec_ret_val.i_code < 0){

                StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, ec_ret_val.str_description, true);
                System.Windows.Forms.Application.Exit();

            }//if

            // crea o reabre el fichero de logs donde se guardaran los eventos e incidencias durante la ejeuction de la aplicación    
            StatusLogs.MessagesInit(ConfigMgr.m_str_logs_path, ConfigMgr.m_b_new_log_per_sesion, textBox2, statusStrip1, toolStripStatusLabel1);
            if (ec_ret_val.i_code < 0) {

                StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, ec_ret_val.str_description, true);
                System.Windows.Forms.Application.Exit();

            } else {

                str_aux = "Log file open/created in \"" + ConfigMgr.m_str_logs_path + "Logs\\\".";
                StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, str_aux, false);
                str_aux = "User \"" + System.Environment.UserName + "\" logged in \"" + System.Environment.MachineName + "\".";
                StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, str_aux, false);

            }//if

            // create and edit the properties of Be Hex editor
            hexb_romEditor = new HexBox();
            hexb_romEditor.Width = tabControl2.TabPages[0].Width;
            hexb_romEditor.Height = tabControl2.TabPages[0].Height - ( button1.Height + 8 );
            hexb_romEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)| System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right))));
            hexb_romEditor.BytesPerLine = 16;
            hexb_romEditor.UseFixedBytesPerLine = true;
            hexb_romEditor.LineInfoVisible = true;
            hexb_romEditor.ColumnInfoVisible = true;
            hexb_romEditor.VScrollBarVisible = true;
            hexb_romEditor.GroupSize = 4;
            hexb_romEditor.HexCasing = HexCasing.Lower;
            hexb_romEditor.StringViewVisible = true;            

            // initialize the drive pack object with the default drive pack file 
            dpack_drivePack.Initialize(ConfigMgr.m_str_default_file);           
            dpack_drivePack.dynbypr_memory_bytes.Changed += new System.EventHandler(this.BeHexEditorChanged);// this method will be called every time there is a change in the content of the Be Hex editor
            // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
            hexb_romEditor.ByteProvider = dpack_drivePack.dynbypr_memory_bytes;

            // add the Be Hex editor to the corresponding tab page
            tabControl2.TabPages[1].Controls.Add(hexb_romEditor);
            
            // set application controls state according to the configuration parameters values
            UpdateAppWithConfigParameters(true);

            return ec_ret_val;

        }//InitControls


        /*******************************************************************************
        *  ConfirmCloseProject
        *------------------------------------------------------------------------------
        *  Description
        *     Metodo que muestra el mensaje al usuario indicando si desea o no cerrar
        *  el proyecot actual. Se llama antes de salir, abrir o crear un nuevo proyecto
        *  siempre que haya ya un proyecto cargado en memoria.
        *  Parameters:
        *     str_message: mensaje a mostrar en al usuario.
        *  Return: 
        *    By reference:
        *    By value:
        *      true: si se confirma que hay que cerrar el proyecto.
        *      false: si no hay que cerrar el proyecto
        *******************************************************************************/
        private bool ConfirmCloseProject(string str_message){
            bool b_pending_modifications = true;
            bool b_close_project = true;


            if (dpack_drivePack.dataChanged == false){
                // no hay modificaciones pendientes de guardarse así que se puede cerrar todo directamente
                b_pending_modifications = false;
                b_close_project = true;
            }else{
                // hay modificaciones pendientes de guardar en disco
                b_pending_modifications = true;
            }//if    

            if (b_pending_modifications){

                // si hay un proyecto abierto se pregunta antes de salir
                DialogResult dialogResult = MessageBox.Show(str_message, "Close?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes){
                    b_close_project = true;
                }else if (dialogResult == DialogResult.No){
                    // NO hay que salir
                    b_close_project = false;
                }//if

            }else{

                // si no hay moficaciones pendientes de guardarse se sale directamente
                b_close_project = true;

            }//if

            return b_close_project;

        }//ConfirmCloseProject


        /*******************************************************************************
        *  ReducePathAndFile
        *------------------------------------------------------------------------------
        *  Description:
        *   Procedimiento que toma un cadena con el nombre del fichero y path y si este supera el 
        *  numero de caracteres indicados entonces la recorta para que tenga el tamaño indicado.
        *  Parameters:
        *    str_path_file:cadena con el fichero ( path incluido ) que se desea recortar.
        *    ui_num_chars: numero maximo permitido para el nombre del path + fichero
        *  Return: 
        *    By reference:
        *    By value:
        *      cadena recortada a la longitud indicada
        *******************************************************************************/
        public string ReducePathAndFile(string str_path_file, uint ui_num_chars){
            string str_aux = "";
            int i_length = 0;
            int i_length_diff = 0;

            // comprueba que la longitud de la cadena es mayor que num_chars i si es así la recorta
            i_length = str_path_file.Length;
            if (i_length <= ui_num_chars) {

                // no se recorta el path/nombre del fichero
                str_aux = str_path_file;

            } else {

                // se calcula el exceso de caracteres respecto a 
                i_length_diff = i_length - (int)ui_num_chars;

                // quita los caracteres del principio que sobran y se queda con los del final, luego se pone '...' al ppio
                str_aux = str_path_file.Substring(i_length_diff, (int) ui_num_chars);
                str_aux = "..." + str_aux;

            }//if

            return str_aux;

        }//ReducePathAndFile


        /*******************************************************************************
        *  AreCorrdinatesInScreenBounds
        *------------------------------------------------------------------------------
        *  Description:
        *    Procedimiento que comprueba que las coordenadas x,y recibidas corresponden
        * con unas coordenadas de pantalla validas ( es decir que no caen fuera de las 
        * dimensiones de la pantalla ).
        *  Parameters:
        *   x: coordenada x
        *   y: coordenada y
        *  Return: 
        *    By reference:
        *    By value:
        *      true si las coordenadas recibidas son validas
        *      false si las cooredenadas recibidas caen fuera de la pantalla
        *******************************************************************************/
        private bool AreCorrdinatesInScreenBounds(int x, int y){
            bool b_are_in_bound = false;
            int x1, x2;
            int y1, y2;


            // process each available screen and check if the received X,Y coordinates are in the area of any of the available screens
            foreach (var screen in Screen.AllScreens){

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
                if ((x > x1) && (x < x2) && (y > y1) && (y < y2)){
                    b_are_in_bound = true;
                }

            }//foreach

            return b_are_in_bound;

        }//AreCorrdinatesInScreenBounds


        /*******************************************************************************
        *  EnableDisableControls
        *------------------------------------------------------------------------------
        *  Description:
        *    Procedimiento que habilita o deshabilita los controles de la aplciacion para
        *  evitar que el usuario interactue con la aplicacion mientras esta esta realizando
        *  otras tareas o procesando datos.
        *  Parameters:
        *     b_enable: true si los controles deben habilitarse, y false en caso de que deban
        *   deshabilitarse
        *  Return: 
        *    By reference:
        *    By value:
        *******************************************************************************/
        public void EnableDisableControls(bool b_enable) {


            if (b_enable == false) {
/*
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
*/
            } else {

                // se habilitan los botones disponibles
/*
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
*/            
            }//if (b_enable == false)

        }//EnableDisableControls



        /*******************************************************************************
        *  RefreshHexEditor
        *------------------------------------------------------------------------------
        *  Description:
        *  
        *  Parameters:
        *  Return: 
        *    By reference:
        *    By value:
        *******************************************************************************/
        public void RefreshHexEditor(){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;

            // initialize the Be Hex editor Dynamic byte provider used to store the data in the Be Hex editor
            hexb_romEditor.ByteProvider = dpack_drivePack.dynbypr_memory_bytes;
            hexb_romEditor.ByteProvider.ApplyChanges();

        }//RefreshHexEditor



       /*******************************************************************************
        *  UpdateAppWithConfigParameters
        *------------------------------------------------------------------------------
        *  Description:
        *    Procedimiento que actualiza el formulario y otros parametros de configuración 
        *  de la aplicación con lo establecido en los parametros de configuracion del config.xml
        *  Parameters:
        *     b_update_enabled_disabled_state: true si la operacion requiere actualizar o no el estado 
        *  habilitado/deshabilitado de los controles
        *  Return: 
        *    By reference:
        *    By value:
        *      ErrCode con el codigo del error o cErrCodes.ERR_NO_ERROR si no lo hay
        *******************************************************************************/
        public ErrCode UpdateAppWithConfigParameters(bool b_update_enabled_disabled_state){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";


            if (AreCorrdinatesInScreenBounds(ConfigMgr.m_i_screen_orig_x, ConfigMgr.m_i_screen_orig_y)){

                // get the form dimensions and coordinates
                this.Height = ConfigMgr.m_i_screen_size_y;
                this.Width = ConfigMgr.m_i_screen_size_x;
                this.Top = ConfigMgr.m_i_screen_orig_y;
                this.Left = ConfigMgr.m_i_screen_orig_x;

            }else{

                // get the form dimensions and coordinates
                this.Height = cConfig.DEFAULT_FORM_HEIGHT;
                this.Width = cConfig.DEFAULT_FORM_WIDHT;
                this.Top = 25;
                this.Left = 25;

            }// if ( AreCorrdinatesInScreenBounds(i_screen_orig_x,i_screen_orig_y) 

            if (ConfigMgr.m_b_screen_maximized) { 
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            }else{
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            }

            // cambiamos el puntero del raton a reloj si la aplicacion esta ocupada procesando
            // y lo dejamso con el icono estandar si no esta procesando
            if (StatusLogs.IsAppBusy()) {
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
            if (ConfigMgr.m_str_cur_file != "") str_aux = str_aux + " - " + ReducePathAndFile(ConfigMgr.m_str_cur_file, cConfig.SW_MAX_TITLE_LENGTH);
            this.Text = str_aux;

            // actualiza el estado Enabled/Disabled de los controles
            if (b_update_enabled_disabled_state) {
/*
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
*/
            }// if (b_update_enabled_disabled_state) 

            // updates the corresponding text box with the last read valid title
            textBox3.Text = dpack_drivePack.str_title;

            // updates the corresponding text box with the last read valid song information
            textBox1.Text = dpack_drivePack.str_songInfo;

            return ec_ret_val;

        }//UpdateAppWithConfigParameters



        /*******************************************************************************
        *  UpdateConfigParametersWithAppState
        *------------------------------------------------------------------------------
        *  Description
        *    Procedimiento que actualiza los parametros de configuracion con
        *  los valores correspondientes al estado del formulario y con el estado de otros 
        *  parametros de configuración.
        *  Parameters:
        *    current_form: referencia al formulario principal de la aplicación para poder acceder
        *   el estado de sus variables internas.
        *  Return: 
        *    By reference:
        *    By value:
        *      >=0: 
        *      <0: 
        *******************************************************************************/
        public int UpdateConfigParametersWithAppState(){
            int i_ret_val = 0;


            // get the form dimensions and coordinates
            ConfigMgr.m_i_screen_size_y = this.Height;
            ConfigMgr.m_i_screen_size_x = this.Width;
            ConfigMgr.m_i_screen_orig_y = this.Top;
            ConfigMgr.m_i_screen_orig_x = this.Left;

            ConfigMgr.m_b_screen_maximized = (this.WindowState == System.Windows.Forms.FormWindowState.Maximized);

            // update object properties with controls state
            if (dpack_drivePack.str_title != textBox3.Text){
                dpack_drivePack.dataChanged = true;
                dpack_drivePack.str_title = textBox3.Text;
            }
            if (dpack_drivePack.str_songInfo != textBox1.Text){
                dpack_drivePack.dataChanged = true;
                dpack_drivePack.str_songInfo = textBox1.Text;
            }
           
            return i_ret_val;

        }//UpdateConfigParametersWithAppState


        /*******************************************************************************
         *  BeHexEditorChanged
         *------------------------------------------------------------------------------
         *  Description
         *  Parameters:
         *  Return: 
         *    By reference:
         *    By value:
         *******************************************************************************/
        private void BeHexEditorChanged(object sender, EventArgs e) {

            dpack_drivePack.dataChanged = true;

        }//BeHexEditorChanged


        /*******************************************************************************
        *  IsValidPath
        *------------------------------------------------------------------------------
        *  Description
        *    Funcion que indica si la cadena recibida se corresponde a una cadena de path
        *  con formato válido o si en cambio la cadena no es un path o no tiene un formato 
        *  valido.
        *  Parameters:
        *     path
        *    allowRelativePaths
        *  Return: 
        *    By reference:
        *    By value:
        *      true: si es un path con formato correcto
        *      false: si es un path con formato incorrecto
        *   NOTA: se obtuvo de
        *   https://stackoverflow.com/questions/6198392/check-whether-a-path-is-valid
        *******************************************************************************/
        public bool IsValidPath(string path, bool allowRelativePaths = false){
            bool isValid = true;

            try{
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths){
                    isValid = Path.IsPathRooted(path);
                }else{
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }//if

            }catch(Exception ex){
                isValid = false;
            }

            return isValid;

        }//IsValidPath


        /*******************************************************************************
        *  CloseApplication
        *------------------------------------------------------------------------------
        *  Description
        *    Executes all the steps that must be excuted when cosing the appliation: 
        *     - Set the closing event in the logs
        *     - Save last active configuration
        *  Parametros:
        *  Return: 
        *    By reference:
        *    By value:
        *      true: if user confirms that application must be closed
        *      false: if the user cancelled the close application operation
        *******************************************************************************/
        private bool CloseApplication(){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            bool b_close_application = false;


            // antes de cerrar la aplicacion se llama a la funcion que muestra el aviso al usuario 
            // preguntando si desa o no continuar dependiendo de si hay proyecto activo o no
            b_close_application = ConfirmCloseProject("There pending modifications to save. Exit anyway?");

            if (b_close_application){

                // guarda en el fichero de configuracion los parametros con el estado de la aplicacion
                ec_ret_val = ConfigMgr.SaveConfigParameters();
                if (ec_ret_val.i_code < 0){
                  StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, ec_ret_val, ec_ret_val.str_description, false);
                }

                // se informa de cierre del log
                StatusLogs.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, "Log file closed", false);
            }//b_close_application

            return b_close_application;

        }//CloseApplication

    }


}
