using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;

namespace drivePackEd
{

    /*******************************************************************************
    *  @brief defines the object used to manage the logs information
    *******************************************************************************/
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

        public Color BUSY_BACK_COLOR = Color.FromArgb(255, 153, 153);

        System.Windows.Forms.ToolStrip MessagesStatusStrip = null;
        System.Windows.Forms.ToolStripStatusLabel MessagesStatusStripLabel = null;
        StreamWriter m_log_stream_writter = null; // reference to the log file
        TextBox      m_log_text_box;// reference to the textbox where the messages will be shown
        string       m_log_file_path = "";
        string       m_log_file_path_name = "";
        bool         m_app_is_busy  = false; // flag used to keep the current application state BUSY ( app processing ) NOT_BUSY ( app not processing ) 
        protected readonly object lockObj = new object();

        // counters of the different types of messages shown to the user
        long m_msgs_ctr_error   = 0;
        long m_msgs_ctr_warning = 0;
        // maximum number of characters allowed in the output text box
        long m_msgs_lenght      = 12000;



        /*******************************************************************************
        *  @brief Procedure that receives a text string and clips it by taking the last
        *  characters of that string starting from the first \n contained in the last
        *  m_msgs_lenght characters. It takes the last complete lines that are present in the
        *  last m_msgs_lenght of the received text_to_clip. It received text_to_clip is not
        *  larger than m_msgs_lenght then it is not clipped.
        *  @param[in] text_to_clip text to check if it exceeds the maximum length, and from
        *  which, if exceeded, the last complete lines will be taken starting from received
        *  text_to_clip.Length - m_msgs_lenght.
        *  @param[out] cliped_text the string clipped with the last complete lines in the last
        *  m_msgs_lenght characeters in the received text_to_clip.
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
        *  @brief Updates the state of the internal flag used to mark the state of apllications 
        *  BUSY (processing) or NOT_BUSY (not processing) flag.
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
        *  @brief Returns the state of the internal flag used to mark the applications
        *  internal state  BUSY ( when it is processing) or NOT_BUSY (when it is not processing).
        *  @return true if the application is busy processing, false if the application
        *  is NOT busy processing.
        *******************************************************************************/
        public bool IsAppBusy() {

            return m_app_is_busy;

        }//IsAppBusy


        /*******************************************************************************
        *  @brief Initializes the information necessary to generate logs messages.
        *  @param[in] str_log_path path where it should look for and open or create the log file.
        *  @param[in] b_multiple_files true if a new log file should be created each time
        *  the project is opened.
        *  @param[in] tb_TextBox the TextBox used to print output the logs.
        *  @param[in] MessagesStatusStrip referemce to the status strip control that contains 
        *  the text label that shows the processing status. null if there is no
        *  MessagesStatusStripLabel to show status messages.
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

                // initializes the statusStrip control used to show the last message of the logs.
                if ( (TStr_MessagesStatusStrip != null) && (TStrL_MessagesStatusStripLabel!=null ) ){

                    MessagesStatusStrip = TStr_MessagesStatusStrip;
                    MessagesStatusStripLabel = TStrL_MessagesStatusStripLabel;

                    // clean content
                    // MessagesStatusStripLabel.Text = "";
                    MessagesStatusStrip.Refresh();

                }else{

                    // there i no satusStrip configured
                    MessagesStatusStrip = null;
                    MessagesStatusStripLabel = null;

                }//if

                // initializes the textBox control used to show the log text messages
                if (m_log_text_box == null) {
                    m_log_text_box = tb_TextBox;
                }

                // initializes the path wher the file logs will be stored
                m_log_file_path = str_log_path + "Logs\\";
            
                // logs are stored in \\Logs folder, if it does not exist we create it
                b_folder_exists = System.IO.Directory.Exists(m_log_file_path);
                if (!b_folder_exists) System.IO.Directory.CreateDirectory(m_log_file_path);

                // before creating a new logs file, we check if already exists one to 
                // continue adding new log messages on it, and if it does not exist a new
                // one is created. To check if already exists a log file we get the name
                // of all the files in the logs path and then check if any of them corresponds 
                // to a log file.
                string[] fileEntries = System.IO.Directory.GetFiles(m_log_file_path);
                b_file_exists = false;
                if (fileEntries != null) {
                    
                    // checks if exists any log file in the directory 
                    i_file_index = 0;
                    while ( (i_file_index < fileEntries.Count()) && (b_file_exists==false) ){
                        str_aux = Path.GetFileName(fileEntries[i_file_index]);
                        if (str_aux.Contains(LOG_FILE_NAME)) {
                            b_file_exists = true;
                        } else {
                            i_file_index++;
                        }
                    }//while

                    // if a log file already exists in the directory then checks that it does not
                    // exceed the maximum allowed log file length.
                    if (b_file_exists) {
                        finfo_aux = new System.IO.FileInfo(fileEntries[i_file_index]);
                        if (finfo_aux.Length < MAX_FILE_SIZE) {
                            b_file_under_max_size = true;
                        }
                    }//if

                }//if

                // if it is specified in the configuration file ( with NEW_LOG_P_SESION == FALSE ) and if
                // already exists a log file that does not exceed the maximum the last log file then
                // the log messages are append to the end of the last existing log file. If there is no 
                // a log file or if it exists but exceeds the maximum allowed value then a new one is created.
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
        *  @brief Function that returns the text representation of the received error
        *  code parameter.
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
        *  @brief Function that increments by 1 the counter of the received error type.
        *  These counters are used to count the different types of errors and are usefull 
        *  to generate reports of the differnt type of errors.
        *  @param[in] error_type code of the error type for which you want to increment 
        *  its counter.
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
        *  @brief Procedure that returns the number of errors of each type that have 
        *  occurred up to the moment of the call.
        *  @param[out] ctr_errors number of errors counted up to the moment,
        *  @param[out] ctr_warnings of warnings counted up to the moment.
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

            // prepare the log text string with the timestamp, erro type, error code, description ...
            str_date = CurrentTime.Date.Year.ToString() + "\\" + CurrentTime.Date.Month.ToString("00") + "\\" + CurrentTime.Date.Day.ToString("00");
            str_time = CurrentTime.Hour.ToString("00") + ":" + CurrentTime.Minute.ToString("00") + ":" + CurrentTime.Second.ToString("00") + ":" + CurrentTime.Millisecond.ToString("000");

            IncreseMessageTypeCounter(error_type);
            error_type_string = GetStatusMsgTypeString(error_type) +": ";
            // the error code is only added to the log message if it really corresponds to an error ( that is <0 )
            if (err_code.i_code < 0) {                
                error_code_string = "[" + err_code.i_code.ToString() + "]";
            }//if

            // if a line number is received then add it to the log message
            if (file_line!=-1){
                error_info=error_info + " [Li:"+ file_line.ToString()+"]";
            }

            // if a column number is received then add it to the log message
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

                    // if the length of the text added to the control exceeds the maximum allowed text
                    if (ClipTextLines(m_log_text_box.Text, ref str_clipped_text) ){
                        m_log_text_box.Text = str_clipped_text;
                    }//if

                }//if (text_box_output !=null)     
           
                // if there is a log file enabled then the log messages are added to the log file.
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

                // if the flag to show the message in a PopUp is set then show the message in a PopUp
                str_aux = error_type_string + error_text + " " + error_info + error_code_string;
                if (pop_up){
                    MessageBox.Show(str_aux);
                }//if

            }//lock

        }//WriteMessage


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
