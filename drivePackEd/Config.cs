using System;
using System.Xml;


namespace drivePackEd
{

    // Solucionar:
    // Mirar porque tarda tanto en comenzar a transmitir el paquete cuando como mucho debería tardar el segundo que tarda en recibir la 'C' del receptor ?
    // Revisar organización en clases de las funciones de envio y recepcion por 1KXmodem
    // Mejoras:

    class cConfig{
        public const int DEFAULT_FORM_WIDTH = 500;
        public const int DEFAULT_FORM_HEIGHT = 600;

        public const string SW_TITLE = "drivePackEd";
        public const string SW_VERSION = "v00_00_b00";
        public const string SW_DESCRIPTION = "drive Pack Editor";
        public const string SW_COMPANY = "©Tolaemon 2022";
        public const uint SW_MAX_TITLE_LENGTH = 40;

        // XML with the application persistent parameters
        XmlDocument m_config_XML = new XmlDocument();

        // Program general configuration parameters
        public int m_i_screen_size_x = 200;
        public int m_i_screen_size_y = 200;
        public int m_i_screen_orig_x = 20;
        public int m_i_screen_orig_y = 20;
        public bool m_b_screen_maximized = false;

        public string m_str_logs_path = ""; // Path where the log files are stored

        public string m_str_cur_rom_file = ""; // Path and name of the currently open drive pack ROM file
        public string m_str_last_rom_file = ""; // Path and name of the last open and valid drive pack ROM file
        public string m_str_default_rom_file = ""; // The file to use as the default drive pack template

        public string m_str_cur_song_file = ""; // Path and name of the currently open drive pack song file
        public string m_str_last_song_file = ""; // Path and name of the last open and valid drive pack song file
        
        public bool m_b_new_log_per_sesion = false; // Flag used to indicate whether the application should create a new log file each time it starts

        public string m_str_color_set = "STANDARD"; // String with the color code used


        /*******************************************************************************
        *  @brief Procedure that updates the form and other configuration parameters of the 
        *  application with the settings read from the configuration parameters of the config.xml.
        *  @return ErrCode with the error code, or cErrCodes.ERR_NO_ERROR if there is no error.
        *******************************************************************************/
        public ErrCode LoadConfigParameters(){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string  str_aux = "";
            XmlNode variable;

            try {

                m_config_XML.Load("config.xml");

                // get the form dimensions
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/SCREEN_SIZEX";
                variable = m_config_XML.SelectSingleNode(str_aux);
                str_aux = variable.InnerText;
                m_i_screen_size_x = Convert.ToInt32(str_aux.Trim());

                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/SCREEN_SIZEY";
                variable = m_config_XML.SelectSingleNode(str_aux);
                str_aux = variable.InnerText;
                m_i_screen_size_y = Convert.ToInt32(str_aux.Trim());

                // get the screen coordinates orgigin
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/SCREEN_ORIGX";
                variable = m_config_XML.SelectSingleNode(str_aux);
                str_aux = variable.InnerText;
                m_i_screen_orig_x = Convert.ToInt32(str_aux.Trim());

                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/SCREEN_ORIGY";
                variable = m_config_XML.SelectSingleNode(str_aux);
                str_aux = variable.InnerText;
                m_i_screen_orig_y = Convert.ToInt32(str_aux.Trim());

                // the screen will be shown maximized
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/SCREEN_MAXIMIZED";
                variable = m_config_XML.SelectSingleNode(str_aux);
                str_aux = variable.InnerText;
                if (str_aux == "TRUE") {
                    m_b_screen_maximized = true;
                } else {
                    m_b_screen_maximized = false;
                }

                // the application must create a new log file every time it opens the same project
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/NEW_LOG_P_SESION";
                variable = m_config_XML.SelectSingleNode(str_aux);
                str_aux = variable.InnerText;
                if (str_aux == "TRUE") {
                    m_b_new_log_per_sesion = true;
                } else {
                    m_b_new_log_per_sesion = false;
                }

                // the folder where the logs files are stored
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/LOGS_PATH";
                variable = m_config_XML.SelectSingleNode(str_aux);
                m_str_logs_path = variable.InnerText;

                // the folder where the log files of session are stored
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/LAST_PROJ_PATH";
                variable = m_config_XML.SelectSingleNode(str_aux);
                m_str_last_rom_file = variable.InnerText;

                // the name of the current colorset
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/COLORSET";
                variable = m_config_XML.SelectSingleNode(str_aux);
                m_str_color_set = variable.InnerText;

            } catch {

                ec_ret_val = cErrCodes.ERR_CONFIG_LOADING_FILE;

            }

            return ec_ret_val;

        }//LoadConfigParameters

        /*******************************************************************************
        * @brief Procedure that saves the application state in the config.xml file.
        * @return >=0 if parameters were succesfully stored, <0 with the error code.
        *******************************************************************************/
        public ErrCode SaveConfigParameters(){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_aux = "";
            XmlNode variable;


            try{

                //(1) the xml declaration is recommended, but not mandatory
                // XmlDeclaration xmlDeclaration = m_config_XML.CreateXmlDeclaration("1.0", "UTF-8", null);
                // XmlElement root = m_config_XML.DocumentElement;
                // m_config_XML.InsertBefore(xmlDeclaration, root);

                // store the form dimensions
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/SCREEN_SIZEX";
                variable = m_config_XML.SelectSingleNode(str_aux);
                variable.InnerText = Convert.ToString(m_i_screen_size_x);

                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/SCREEN_SIZEY";
                variable = m_config_XML.SelectSingleNode(str_aux);
                variable.InnerText = Convert.ToString(m_i_screen_size_y);

                // store the form origin coordinates
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/SCREEN_ORIGX";
                variable = m_config_XML.SelectSingleNode(str_aux);
                variable.InnerText = Convert.ToString(m_i_screen_orig_x);

                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/SCREEN_ORIGY";
                variable = m_config_XML.SelectSingleNode(str_aux);
                variable.InnerText = Convert.ToString(m_i_screen_orig_y);

                // to show the screen maximized or not
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/SCREEN_MAXIMIZED";
                variable = m_config_XML.SelectSingleNode(str_aux);
                if (m_b_screen_maximized){
                    str_aux = "TRUE";
                }else{
                    str_aux = "FALSE";
                }
                variable.InnerText = str_aux;

                // the application must create a new log file every time it opens the same project
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/NEW_LOG_P_SESION";
                variable = m_config_XML.SelectSingleNode(str_aux);
                if (m_b_new_log_per_sesion) {
                    str_aux = "TRUE";
                } else {
                    str_aux = "FALSE";
                }
                variable.InnerText = str_aux;

                // the folder where the logs files are stored
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/LOGS_PATH";
                variable = m_config_XML.SelectSingleNode(str_aux);
                variable.InnerText = m_str_logs_path;

                // the folder where the log files of session are stored
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/LAST_PROJ_PATH";
                variable = m_config_XML.SelectSingleNode(str_aux);
                variable.InnerText = m_str_last_rom_file;

                // the name of the current colorset
                str_aux = "/DRIVEPACKED/DRIVEPACKED_CONFIG/COLORSET";
                variable = m_config_XML.SelectSingleNode(str_aux);
                variable.InnerText = m_str_color_set;

                // save to disk
                m_config_XML.Save("config.xml");

            }catch{

                ec_ret_val = cErrCodes.ERR_CONFIG_SAVING_FILE;

            }//try

            return ec_ret_val;

        }//SaveConfigParameters


    }// class cConfig

}
