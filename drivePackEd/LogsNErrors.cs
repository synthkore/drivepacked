using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;

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
        *  @brief Procedure that receives a text string and clips its length.
        *  @param[in] text_to_clip text to check if it exceeds the maximum length, and from
        *  which, if exceeded, the last complete lines will be taken starting from
        *  "messages_max_buffer_length" characters.
        *  @param[out] cliped_text the string clipped with the last complete lines starting from
        *  the last "messages_max_buffer_length" characters.
        *  @return true if the text had to be truncated, false if truncation was not necessary.
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
        *  @brief Updates the state of the internal flag used to mark the state of BUSY
        *  (processing) or NOT_BUSY (not processing) of the application.
        *  @param[in] app_is_busy: The new value for the state flag of BUSY/NOT_BUSY of the
        *  application.
        *  @return None.
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
        *  @brief Returns the state of the internal flag used to mark the state of
        *  BUSY (processing) or NOT_BUSY (not processing) of the application.
        *  @return true if the application is busy processing, false if the application
        *  is NOT busy processing.
        *******************************************************************************/
        public bool IsAppBusy() {

            return m_app_is_busy;

        }//IsAppBusy


        /*******************************************************************************
        *  @brief Initializes the necessary information for to generate logs messages
        *  @param[in] str_log_path path where it should look for and open or create the log file.
        *  @param[in] b_multiple_files true if a new log file should be created each time
        *  the project is opened.
        *  @param[in] tb_TextBox: TextBox used to output the logs.
        *  @param[in] MessagesStatusStrip: Pointing to the status strip control that contains the
        *  text label that shows the processing status. null if there is no MessagesStatusStripLabel
        *  to show status messages.
        *  @param[in] MessagesStatusStripLabel: Pointing to the text label of the strip control
        *  used to show the processing data. null if there is no MessagesStatusStripLabel
        *  to show status messages.
        *  @return ErrCode with the error code or cErrCodes.ERR_NO_ERROR if there is no error.
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
        *  @brief Function that returns the text representation of the message code
        * received as a parameter.
        *  @param[in] error_type code of the error type for which we want to obtain its
        *  text representation.
        *  @return string with the text representation of the received error type code.
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
        *  @brief Function that increments the counter of the received error type by 1.
        *  These counters are used to count the different types of errors and to generate
        *  a report of the number of errors of each type.
        *  @param[in] error_type: Code of the error type for which you want to increment its counter.
        *  @return None.
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
        * @brief  Clears to 0 the counters used to count the different types of messages.
        * @return none
        *******************************************************************************/
        private void ResetMessageTypeCounters(){


            m_msgs_ctr_error = 0;
            m_msgs_ctr_warning = 0;

        }//ResetMessageTypeCounters


        /*******************************************************************************
        *  @brief Procedure that returns the number of errors of each type that have occurred
        *  up to the moment of the call.
        *  @param[out] ctr_errors number of errors counted up to the moment,
        *  @param[out] ctr_warningsnumber of warnings counted up to the moment.
        *  @return none.
        *******************************************************************************/
        private void GetMessageTypeCounters(ref long ctr_errors, ref long ctr_warnings){

            ctr_errors = m_msgs_ctr_error;
            ctr_warnings = m_msgs_ctr_warning;

        }//GetMessageTypeCounters


        /*******************************************************************************
        *  @brief Writes the received error text to the configured output.
        *  @param[in] text_box_output text box where the error message will be displayed.
        *  If it is null, the message is not shown or counted.
        *  @param[in] file_line line in the file where the error occurred.
        *  @param[in] file_column column of the line where the error occurred.
        *  @param[in] error_string text of the error to be saved in the error file.
        *  @return None.
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
        *  @brief Writes the text corresponding to the error detected in a variable to
        *  the output.
        *  @param[in] text_box_output Text box where the error message will be displayed.
        *  If it is null, the message is not shown or counted.
        *  @param[in] file_line line in the file where the error occurred.
        *  @param[in] file_column column of the line where the error occurred.
        *  @param[in] index Index of the variable where the error occurred.
        *  @param[in] subindex subindex of the variable where the error occurred.
        *  @param[in] variable_name name of the variable where the error occurred.
        *  @param[in] error_type whether it is an ERROR or a WARNING.
        *  @param[out] error_string wext of the error to be saved in the error file.
        *  @return None.
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
        *  @brief Writes the text corresponding to the error detected in an alarm to the
        *  configured output.
        *  @param[in] file_line Line in the file where the error occurred.
        *  @param[in] file_column Column of the line where the error occurred.
        *  @param[in] alarm_id Identifier of the alarm where the error occurred.
        *  @param[in] alarm_bitset Bitset of the alarm where the error occurred.
        *  @param[in] code Code of the alarm where the error occurred.
        *  @param[in] error_type Whether it is an ERROR or a WARNING.
        *  @param[in] err_code Text of the error to be saved in the error file.
        *  @param[in] error_text Text of the error to be saved in the error file.
        *  @param[in] pop_up if ture a pop up with the message will be created. If false no
        *  pop up will be generated and the Alarm Error will be only written in the configured
        *  output.
        *  @return None.
        *******************************************************************************/
        public void WriteAlarmError(long file_line,long file_column,  long alarm_id, long alarm_bitset, string code, status_msg_type error_type, ErrCode err_code, string error_text, bool pop_up){
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
        *  @brief Function that returns a string with the current year, date, and time in
        *  the format: YYYYMMDD_hhmmss.
        *  @return String with the current year, date, and time in the format: YYYYMMDD_hhmmss.
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
