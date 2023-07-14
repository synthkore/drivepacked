using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace drivePackEd
{
    public class cLogsNErrors
    {
        public enum status_msg_type{
            MSG_EMPTY = 0,
            MSG_INFO,
            MSG_WARNING,
            MSG_TAG,
            MSG_ERROR
        }

        const string LOG_FILE_NAME = "drivePackEd.log";
        const long   MAX_FILE_SIZE   = 4194304;

        public Color BUSY_BACK_COLOR = Color.FromArgb(255, 153, 153);// color de las celdas NO editables que aun no han sido chequeadas

        System.Windows.Forms.ToolStrip MessagesStatusStrip = null;
        System.Windows.Forms.ToolStripStatusLabel MessagesStatusStripLabel = null;
        StreamWriter m_log_stream_writter = null; // reference to the log file
        TextBox      m_log_text_box;// reference to the textbox where the messages will be shown
        string       m_log_file_path = "";
        string       m_log_file_path_name = "";
        bool         m_app_is_busy  = false; // flag utilizado para marcar el estado de OCUPADO (procesando) NO_OCUPADO ( no procesando ) de la aplicacion
        protected readonly object lockObj = new object();

        // contadores utilizados para contar los diferentes tipos de mensajes que se activan, para estadisticas
        long m_msgs_ctr_error   = 0;
        long m_msgs_ctr_warning = 0;
        // maximo numero de caracteres permitido en la text box de salida
        long m_msgs_lenght      = 12000;



        /*******************************************************************************
        *  ClipTextLines
        *------------------------------------------------------------------------------
        *  Description
        *    Procedimiento que recibe una cadena de texto
        *  Parameters:
        *    text_to_clip: texto del que se quiere comprobar si supera o no la longitud máxima,
        *  y del cual, si la supera, se tomaran las ultimos lineas completas a partir de 
        *  "messages_max_buffer_lenght" caracteres.
        *  Return: 
        *    By reference:
        *      cliped_text: con la cadena clipada con las ultimas lineas compleas a partir de
        *  los ultimos "messages_max_buffer_lenght" caracteres.
        *    By value:
        *      true: si se ha tenido que recortar el texto
        *      false: si no ha sido necesario recortar el texto
        *******************************************************************************/
        private bool ClipTextLines( string text_to_clip, ref string cliped_text){
            bool b_ret_val = false;
            int i_aux = 0;
            int i_aux2 = 0;


            cliped_text = "";
            
            if (text_to_clip.Length > m_msgs_lenght){

                // find first \n starting from the last "messages_max_buffer_lenght" characters
                i_aux = (int)(text_to_clip.Length - m_msgs_lenght);
                i_aux2 = text_to_clip.IndexOf("\n", i_aux);
                cliped_text = text_to_clip.Substring(i_aux2);
                b_ret_val = true;

            }//if

            return b_ret_val;

        }//ClipTextLines



        /*******************************************************************************
        *  SetAppBusy
        *------------------------------------------------------------------------------
        *  Description
        *    Actualiza el estado del flag interno utilizado para marcar el estado de OCUPADO 
        *  (procesando) NO_OCUPADO ( no procesando ) de la aplicacion
        *  Parameters:
        *    app_is_busy: con el nuevo valor para el flag de estado de OCUPAD/NO_OCUPADO de la
        *  aplicacion.
        *  Return: 
        *    By reference:
        *    By value:
        *******************************************************************************/
        public void  SetAppBusy(bool app_is_busy) {

            m_app_is_busy = app_is_busy;
            if (MessagesStatusStrip != null) {

                if (app_is_busy) MessagesStatusStrip.BackColor = BUSY_BACK_COLOR; else MessagesStatusStrip.BackColor = SystemColors.Control;

            }

        }//SetAppBusy



        /*******************************************************************************
        *  IsAppBusy
        *------------------------------------------------------------------------------
        *  Description
        *    Devuelve el estado del flag interno utilizado para marcar el estado de  
        *  OCUPADO (procesando) NO_OCUPADO ( no procesando ) de la aplicacion
        *  Parameters:
        *  Return: 
        *    By reference:
        *    By value:
        *       TRUE: si la aplicacion esta ocupada procesando
        *       FALSE: si la aplicacion NO esta ocupada procesando
        *******************************************************************************/
        public bool IsAppBusy() {

            return m_app_is_busy;

        }//IsAppBusy



        /*******************************************************************************
        *  MessagesInit
        *------------------------------------------------------------------------------
        *  Description
        *    Procedimiento que inicializa la información necesaria para el funcionamiento
        *   del sistema de mensajes informativos, de aviso o error.
        *  Parameters:
        *    str_log_path: path donde debera buscar y abrir o crear el fichero de logs.
        *    b_multiple_files: true si hay que crear un nuevo fichero de logs cada vez que
        *  se abra el proyecto.
        *    tb_TextBox: textBox utilizado para sacar los logs.
        *    MessagesStatusStrip: pointing to the status strip control that contains the
        *  text label that shows the processing status. null if there is no any 
        *  MessagesStatusStripLabel to show status messages.
        *    MessagesStatusStripLabel: pointing to the text label of the strip control
        *    used to show the processing data. null if there is no any MessagesStatusStripLabel
        *  to show status messages.
        *  Return: 
        *    By reference:
        *    By value:
        *       ErrCode con el codigo del error o cErrCodes.ERR_NO_ERROR si no lo hay
        *******************************************************************************/
        public ErrCode MessagesInit(string str_log_path, bool b_multiple_files, TextBox tb_TextBox, System.Windows.Forms.ToolStrip TStr_MessagesStatusStrip, System.Windows.Forms.ToolStripStatusLabel TStrL_MessagesStatusStripLabel) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            bool b_folder_exists = false;
            bool b_file_exists = false;
            bool b_file_under_max_size = false;
            int i_file_index = 0;
            FileInfo finfo_aux = null;
            string str_aux = "";


            try { 

                // inicializa el status strip donde se ha de ir mostrando el ultimo mensaje de log.
                if ( (TStr_MessagesStatusStrip != null) && (TStrL_MessagesStatusStripLabel!=null ) ){

                    MessagesStatusStrip = TStr_MessagesStatusStrip;
                    MessagesStatusStripLabel = TStrL_MessagesStatusStripLabel;

                    // clean content
                    // MessagesStatusStripLabel.Text = "";
                    MessagesStatusStrip.Refresh();

                }else{

                    // no hay messages status strip disponible 
                    MessagesStatusStrip = null;
                    MessagesStatusStripLabel = null;

                }//if

                //  inicializa el text box para la salida de mensajes
                if (m_log_text_box == null) {
                    m_log_text_box = tb_TextBox;
                }

                // se inicializa el fichero de logs con el nombre recibido
                m_log_file_path = str_log_path + "Logs\\";
            
                // logs are stored in \\Logs folder, if it does not exist we create it
                b_folder_exists = System.IO.Directory.Exists(m_log_file_path);
                if (!b_folder_exists) System.IO.Directory.CreateDirectory(m_log_file_path);

                // antes de crear un fichero de logs nuevo se mira si ya existe uno al que 
                // poder seguir anadiendo, y si no existe entonces sí se crea. Para ello 
                // obtenemos todos los ficheros del path de logs y miramos si existe algun
                // con el nombre de un fichero de logs
                string[] fileEntries = System.IO.Directory.GetFiles(m_log_file_path);
                b_file_exists = false;
                if (fileEntries != null) {
                    
                    // mira si exsite fichero de logs en el directorio
                    i_file_index = 0;
                    while ( (i_file_index < fileEntries.Count()) && (b_file_exists==false) ){
                        str_aux = Path.GetFileName(fileEntries[i_file_index]);
                        if (str_aux.Contains(LOG_FILE_NAME)) {
                            b_file_exists = true;
                        } else {
                            i_file_index++;
                        }
                    }//while

                    // si existe mira si exsite supera o no el tamano maximo permitido
                    if (b_file_exists) {
                        finfo_aux = new System.IO.FileInfo(fileEntries[i_file_index]);
                        if (finfo_aux.Length < MAX_FILE_SIZE) {
                            b_file_under_max_size = true;
                        }
                    }//if

                }//if

                // si por configuracion en el XML se indica explicitamente ( NEW_LOG_P_SESION == FALSE ),
                // o si ya existe un fichero de log y este no supera el tamano permitido entonces se 
                // sigue anadiendo detras de este. Si en cambio no existe, o si sí existe pero supera 
                // el maximo permitido se crea un nuevo
                if ( ( b_file_exists && b_file_under_max_size ) && (!b_multiple_files) ) {
                    // append to existing file
                    m_log_file_path_name = m_log_file_path + Path.GetFileName(fileEntries[i_file_index]);
                    m_log_stream_writter = new StreamWriter(m_log_file_path_name,true); // falag "true" sirve para indicar que se va hacer append
                    m_log_stream_writter.Close();
                } else {
                    // create new file
                    m_log_file_path_name = m_log_file_path + GetStringCurrentYearDateTime() + " " + LOG_FILE_NAME;
                    m_log_stream_writter = new StreamWriter(m_log_file_path_name);
                    m_log_stream_writter.Close();
                }//if

            
            }catch {

                ec_ret_val = cErrCodes.ERR_LOG_CREATING;
            
            }
            
            return ec_ret_val;

        }//MessagesInit



        /*******************************************************************************
        *  GetStatusMsgTypeString
        *------------------------------------------------------------------------------
        *  Description
        *    Funcion que retorna la conversion a cadena de texto del codigo de mensaje 
        *  recibido como parámetro.
        *  Parameters:
        *    error_type: codigo del tipo de error del cual se quiere obtener su cadena 
        *  en texto.
        *  Return: 
        *    By reference:
        *    By value:
        *      cadena con la conversion a texto del codigo de tipo de error recibido
        *******************************************************************************/
        private string GetStatusMsgTypeString(status_msg_type error_type){
            string ret_string = "UNKONWN";


            switch (error_type){

                case status_msg_type.MSG_ERROR:
                    ret_string = "ERROR";
                    break;
                case status_msg_type.MSG_WARNING:
                    ret_string = "WARNING";
                    break;
                case status_msg_type.MSG_INFO:
                    ret_string = "INFO";
                    break;
                case status_msg_type.MSG_TAG:
                    ret_string = "TAG";
                    break;

                case status_msg_type.MSG_EMPTY:
                    ret_string = "";
                    break;

            }//switch

            return ret_string;

        }//GetStatusMsgTypeString



        /*******************************************************************************
        *  IncreseMessageTypeCounter
        *------------------------------------------------------------------------------
        *  Description
        *     Funcion que incrementa en 1 el contador del tipo de error recibido. Estos 
        *  contadores se usan para contabilizar los diferentes tipos de errores y poder
        *  sacar un report de la cantidad de errores tras procesar cada fichero.   
        *  Parameters:
        *    error_type: codigo del tipo de error del cual se quiere incrementar su contador
        *  Return: 
        *    By reference:
        *    By value:
        *******************************************************************************/
        private void IncreseMessageTypeCounter(status_msg_type error_type){


            switch (error_type){

                case status_msg_type.MSG_ERROR:
                    m_msgs_ctr_error ++;
                    break;

                case status_msg_type.MSG_WARNING:
                    m_msgs_ctr_warning ++;
                    break;

                case status_msg_type.MSG_INFO:
                    break;

                case status_msg_type.MSG_TAG:
                    break;

                case status_msg_type.MSG_EMPTY:
                    break;

            }//switch

        }//IncreseMessageTypeCounter



        /*******************************************************************************
        *  ResetMessageTypeCounters
        *------------------------------------------------------------------------------
        *  Description
        *     Pone a 0 los contadores de los diferentes tipos de mensajes.
        *  Return: 
        *    By reference:
        *    By value:
        *******************************************************************************/
        private void ResetMessageTypeCounters(){


            m_msgs_ctr_error = 0;
            m_msgs_ctr_warning = 0;

        }//ResetMessageTypeCounters



        /*******************************************************************************
        *  GetMessageTypeCounters
        *------------------------------------------------------------------------------
        *  Description
        *     Procedimiento que retorna el numero de errores de cada tipo que ha tenido
        *    lugar hasta el momento de la llamada.
        *  Return: 
        *    By reference:
        *     ctr_errors: número de errores contabilizados hasta el momemnto  
        *     ctr_warnings: numero de warnings contabilizados hasta el momento
        *    By value:
        *******************************************************************************/
        private void GetMessageTypeCounters(ref long ctr_errors, ref long ctr_warnings)
        {


            ctr_errors = m_msgs_ctr_error;
            ctr_warnings = m_msgs_ctr_warning;

        }//GetMessageTypeCounters



        /*******************************************************************************
        *  WriteMessage
        *------------------------------------------------------------------------------
        *  Description
        *    Escribe en la salidas el texto del error recibido
        *  Parameters:
        *   text_box_output: text box en el que se va a mostrar el mensaje de error. Si es null
        *  el mensaje no se muestra ni contabiliza.   
        *   file_line: linea del fichero en la que se ha producido el error.
        *   file_column: columna de la linea en la que se ha producido el error.   
        *   error_string: con el texto de error a guardar en el fichero de errores
        *  Return: 
        *    By reference:
        *    By value:
        *******************************************************************************/
        public void  WriteMessage(long file_line,long file_column,  status_msg_type error_type, ErrCode err_code, string error_text, bool pop_up)
        {
            string error_info = "";
            string error_type_string = "";
            string str_clipped_text = "";
            string error_code_string = "";
            string str_aux = "";
    
            // extended log option is selected
            DateTime CurrentTime = DateTime.Now;
            string str_date = ""; ;
            string str_time = "";


            // se prepara la cadena del mensaje con el timestamp, tipo de menaje, cadena etc.
            str_date = CurrentTime.Date.Year.ToString() + "\\" + CurrentTime.Date.Month.ToString("00") + "\\" + CurrentTime.Date.Day.ToString("00");
            str_time = CurrentTime.Hour.ToString("00") + ":" + CurrentTime.Minute.ToString("00") + ":" + CurrentTime.Second.ToString("00") + ":" + CurrentTime.Millisecond.ToString("000");

            IncreseMessageTypeCounter(error_type);
            error_type_string = GetStatusMsgTypeString(error_type) +": ";
            if (err_code.i_code < 0) {
                // el codigo de err_code solo se anade al mensaje si hay error (<0)
                error_code_string = "[" + err_code.i_code.ToString() + "]";
            }//if

            // si recibimos numero de linea lo metemos en la linea de error
            if (file_line!=-1){
                error_info=error_info + " [Li:"+ file_line.ToString()+"]";
            }

            // si recibimos numero de columna lo metemos en la linea de error
            if (file_column!=-1){
                error_info = error_info + " [Co:" + file_column.ToString() + "]";
            }

            lock (lockObj){

                // mostramos la informacion de estado en el status strip siempre que este esté inicializado
                if (MessagesStatusStripLabel != null) {

                    MessagesStatusStripLabel.Text = error_type_string + error_text + " " + error_info + error_code_string;
                    MessagesStatusStrip.Refresh();
                }

                if (m_log_text_box != null){

                    // update the output box wit the error messsage
                    m_log_text_box.Text = m_log_text_box.Text + str_date + " " + str_time + " " + error_type_string + error_text + " " + error_info + error_code_string + "\r\n";

                    // si la cantidad de texto en el control de salida supera lo maximo permitido, se capa a lo máximo permitido
                    if (ClipTextLines(m_log_text_box.Text, ref str_clipped_text) ){
                        m_log_text_box.Text = str_clipped_text;
                    }//if

                }//if (text_box_output !=null)     
           
                // si hay fichero de log habilitado se guarda el mensaje tambien en los logs
                if (m_log_file_path_name != "") {

                    try{
                        str_aux = str_date + " " + str_time + " " + error_type_string + error_text + " " + error_info + error_code_string + "\r\n";
                        // el fichero de log se abre y se cierra en cada esctirua
                        using (m_log_stream_writter = new StreamWriter(m_log_file_path_name, true)){
                       
                            m_log_stream_writter.Write(str_aux);
                            m_log_stream_writter.Close();
                        }

                    }catch{

                        MessageBox.Show(str_aux);

                    }//try

                }//if

                // si también hay que mostrar el error en un Pop Up se muestra
                str_aux = error_type_string + error_text + " " + error_info + error_code_string;
                if (pop_up){
                    MessageBox.Show(str_aux);
                }//if

            }//lock

        }//WriteMessage



        /*******************************************************************************
        *  WriteVariableError
        *------------------------------------------------------------------------------
        *  Description
        *    Escribe en la salidas el texto correspondiente al error detectado en una variable
        *  Parameters:
        *   text_box_output: text box en el que se va a mostrar el mensaje de error. Si es null
        *  el mensaje no se muestra ni contabiliza.   
        *   file_line: linea del fichero en la que se ha producido el error.
        *   file_column: columna de la linea en la que se ha producido el error. 
        *   index: indice de la variable en la que se ha producido el error.
        *   subidnex: subindice de la variable en la que se ha producido el error.
        *   variable_name: nombre de la variable en la que se ha producido el error.
        *   error_type: si es un ERROR o un WARNING
        *   error_string: con el texto de error a guardar en el fichero de errores
        *  Return: 
        *    By reference:
        *    By value:
        *******************************************************************************/
        public void WriteVariableError( long file_line,long file_column, string index, string subidnex, string variable_name, status_msg_type error_type, ErrCode err_code, string error_text, bool pop_up)
        {
            string error_info = "";
            string error_type_string = "";
            int i_aux = 0;
            string error_code_string = "";
            string str_aux = "";

            // extended log option is selected
            DateTime CurrentTime = DateTime.Now;
            string str_date = ""; ;
            string str_time = "";


            // se prepara la cadena del mensaje con el timestamp, tipo de menaje, cadena etc.
            str_date = CurrentTime.Date.Year.ToString() + "\\" + CurrentTime.Date.Month.ToString("00") + "\\" + CurrentTime.Date.Day.ToString("00");
            str_time = CurrentTime.Hour.ToString("00") + ":" + CurrentTime.Minute.ToString("00") + ":" + CurrentTime.Second.ToString("00") + ":" + CurrentTime.Millisecond.ToString("000");

            IncreseMessageTypeCounter(error_type);
            error_type_string = GetStatusMsgTypeString(error_type) + ": ";
            if (err_code.i_code < 0) {
                // el codigo de err_code solo se anade al mensaje si hay error (<0)
                error_code_string = "[" + err_code.i_code.ToString() + "]";
            }//if

            // si recibimos el indice lo lo metemos en la linea de error
            if (index!=""){
                error_info=error_info +"[Idx:"+index+"; "+subidnex+";]";
            }//if

            // si recibimos el nombre de la variable lo lo metemos en la linea de error
            if (index!=""){
                error_info=error_info +" [Var: "+variable_name+" ]";
            }//if

            // si recibimos numero de linea lo metemos en la linea de error
            if (file_line!=-1){
                error_info=error_info + " [Li:"+ file_line.ToString()+"]";
            }

            // si recibimos numero de columna lo metemos en la linea de error
            if (file_column!=-1){
                error_info = error_info + " [Co:" + file_column.ToString() + "]";
            }

            lock (lockObj){

                // muestra la informacion de estado en el status strip siempre que este esté inicializado
                if ((MessagesStatusStripLabel != null) && (MessagesStatusStrip != null)) {

                    MessagesStatusStripLabel.Text = error_type_string + error_text + " " + error_info;
                    MessagesStatusStrip.Refresh();

                }

                // muestra la informacion de estado en el textBox siempre que esté inicializado
                if (m_log_text_box != null){

                    // update the output box wit the error messsage
                    m_log_text_box.Text = m_log_text_box.Text + error_type_string + error_text + " " + error_info + "\r\n";

                    // si la cantidad de texto en el control de salida supera lo maximo permitido, se capa a lo máximo permitido
                    if (m_log_text_box.Text.Length>m_msgs_lenght){
                        i_aux = (int)(m_log_text_box.Text.Length - m_msgs_lenght);
                        m_log_text_box.Text = m_log_text_box.Text.Substring(i_aux, (int)m_msgs_lenght);
                    }

                }// if (text_box_output !=null)

                // si hay fichero de log habilitado se guarda el mensaje tambien en los logs
                if (m_log_file_path_name != "") {

                    try{
                        str_aux = str_date + " " + str_time + " " + error_type_string + error_text + " " + error_info + "\r\n";
                        // el fichero de log se abre y se cierra en cada esctirua
                        using (m_log_stream_writter = new StreamWriter(m_log_file_path_name, true)){
                       
                            m_log_stream_writter.Write(str_aux);
                            m_log_stream_writter.Close();
                        }

                    }catch{

                        MessageBox.Show(str_aux);

                    }//try

                }//if

                // si también hay que mostrar el error en un Pop Up se muestra
                str_aux = error_type_string + error_text + " " + error_info + error_code_string;
                if (pop_up){
                    MessageBox.Show(str_aux);
                }//if

            }//lock

        }//WriteVariableError



        /*******************************************************************************
        *  WriteAlarmError
        *------------------------------------------------------------------------------
        *  Description
        *    Escribe en la salida el texto correspondiente al error detectado en una alarma
        *  Parameters:
        *   text_box_output: text box en el que se va a mostrar el mensaje de error. Si es null
        *  el mensaje no se muestra ni contabiliza.   
        *   file_line: linea del fichero en la que se ha producido el error.
        *   file_column: columna de la linea en la que se ha producido el error.
        *   alarm_id: identificador de la alarma en la que ese ha producido el error
        *   alarm_bitset: bitset de la alarma en la que ese ha producido el error
        *   code: codigo de la alarma en la que se ha producido el error
        *   error_type: si es un ERROR o un WARNING
        *   error_string: con el texto de error a guardar en el fichero de errores
        *  Return: 
        *    By reference:
        *    By value:
        *******************************************************************************/
        public void WriteAlarmError(long file_line,long file_column,  long alarm_id, long alarm_bitset, string code, status_msg_type error_type, ErrCode err_code, string error_text, bool pop_up)
        {
            string error_info = "";
            string error_type_string = "";
            int i_aux = 0;
            string error_code_string = "";
            string str_aux = "";

            // extended log option is selected
            DateTime CurrentTime = DateTime.Now;
            string str_date = ""; ;
            string str_time = "";


            // se prepara la cadena del mensaje con el timestamp, tipo de menaje, cadena etc.
            str_date = CurrentTime.Date.Year.ToString() + "\\" + CurrentTime.Date.Month.ToString("00") + "\\" + CurrentTime.Date.Day.ToString("00");
            str_time = CurrentTime.Hour.ToString("00") + ":" + CurrentTime.Minute.ToString("00") + ":" + CurrentTime.Second.ToString("00") + ":" + CurrentTime.Millisecond.ToString("000");

            IncreseMessageTypeCounter(error_type);
            error_type_string = GetStatusMsgTypeString(error_type) + ": ";
            if (err_code.i_code < 0) {
                // el codigo de err_code solo se anade al mensaje si hay error (<0)
                error_code_string = "[" + err_code.i_code.ToString() + "]";
            }//if

            // si recibimos el identificador de la alarma lo mostramos en el mensaje de error
            if (alarm_id!=-1){
                error_info = error_info + "[Id: "+alarm_id.ToString() +" ]";
            }//if

            // si recibimos el nombre de la variable lo metemos en la linea de error
            if (alarm_bitset!=-1){
                error_info = error_info + " [Bitset:" + alarm_bitset.ToString() + "]";
            }//if

            // si recibimos numero de linea lo metemos en la linea de error
            if (code!=""){
                error_info = error_info + " [Code:" + code + "]";
            }

            // si recibimos numero de linea lo metemos en la linea de error
            if (file_line!=-1){
                error_info=error_info + " [Li:"+ file_line.ToString()+"]";
            }

            // si recibimos numero de columna lo metemos en la linea de error
            if (file_column!=-1){
                error_info = error_info + " [Co:" + file_column.ToString() + "]";
            }

            lock (lockObj){

                // muestra la informacion de estado en el status strip siempre que este esté inicializado
                if ((MessagesStatusStripLabel != null) && (MessagesStatusStrip != null)) {
                    MessagesStatusStripLabel.Text = error_type_string + error_text + " " + error_info;
                    MessagesStatusStrip.Refresh();
                }//if

                // muestra la informacion de estado en el textBox siempre que esté inicializado
                if (m_log_text_box != null){

                    // actualizamos el control de salida de texto con el mensaje de error
                    m_log_text_box.Text = m_log_text_box.Text + error_type_string + error_text + " " + error_info + "\r\n";

                    // si la cantidad de texto en el control de salida supera lo maximo permitido, se capa a lo máximo permitido
                    if (m_log_text_box.Text.Length>m_msgs_lenght){
                        i_aux = (int)(m_log_text_box.Text.Length - m_msgs_lenght);
                        m_log_text_box.Text = m_log_text_box.Text.Substring(i_aux, (int)m_msgs_lenght);
                    }
            
                }//if (text_box_output !=null)

                // si hay fichero de log habilitado se guarda el mensaje tambien en los logs
                if (m_log_file_path_name != "") {

                    try{
                        str_aux = str_date + " " + str_time + " " + error_type_string + error_text + " " + error_info + "\r\n";
                        // el fichero de log se abre y se cierra en cada esctirua
                        using (m_log_stream_writter = new StreamWriter(m_log_file_path_name, true)){
                       
                            m_log_stream_writter.Write(str_aux);
                            m_log_stream_writter.Close();
                        }

                    }catch{

                        MessageBox.Show(str_aux);

                    }//try

                }//if
            
                // si también hay que mostrar el error en un Pop Up se muestra
                str_aux = error_type_string + error_text + " " + error_info + error_code_string;
                if (pop_up){
                    MessageBox.Show(str_aux);
                }//if

            }//lock

        }//WriteAlarmError



        /*******************************************************************************
        *  GetStringCurrentYearDateTime
        *------------------------------------------------------------------------------
        *  Description
        *    Función que retorna una cadena con el año fecha y hora actuales en el formato:
        *      YYYYMMDD_hhmmss
        *  Parameters:
        *  Return: 
        *    By reference:
        *    By value:
        *       cadena con  el año la fecha y hora actuales en el formato: YYYYMMDD_hhmmss
        *******************************************************************************/
        public string GetStringCurrentYearDateTime()
        {
            string str_aux = "";
            string str_date = ""; ;
            string str_time = "";

            // extended log option is selected
            DateTime CurrentTime = DateTime.Now;

            // current date, time and messsage type is shown in timestamp
            str_date = CurrentTime.Date.Year.ToString() + "" + CurrentTime.Date.Month.ToString("00") + "" + CurrentTime.Date.Day.ToString("00");
            str_time = CurrentTime.Hour.ToString("00") + "" + CurrentTime.Minute.ToString("00") + "" + CurrentTime.Second.ToString("00");
            str_aux = str_date + "_" + str_time;

            return str_aux;

        }//GetStringCurrentYearDateTime


    }//public class cStatus

}
