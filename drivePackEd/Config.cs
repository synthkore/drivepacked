using System;
using System.Reflection;
using System.Xml;

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

namespace drivePackEd
{

    // Solucionar:
    // Mirar porque tarda tanto en comenzar a transmitir el paquete cuando como mucho debería tardar el segundo que tarda en recibir la 'C' del receptor ?
    // Revisar organización en clases de las funciones de envio y recepcion por 1KXmodem
    // Mejoras:

    public class cConfig{
        public const int DEFAULT_FORM_WIDTH = 500;
        public const int DEFAULT_FORM_HEIGHT = 600;

        public const string SW_TITLE = "drivePackEd";
        public const string SW_DESCRIPTION = "drive Pack Editor";
        public const string SW_COMPANY = "©Tolaemon 2022";
        public const uint SW_MAX_TITLE_LENGTH = 40;

        // NOT persistent configuration parameters:

        // Program general configuration parameters
        public int m_i_screen_size_x = 200;
        public int m_i_screen_size_y = 200;
        public int m_i_screen_orig_x = 20;
        public int m_i_screen_orig_y = 20;
        public bool m_b_screen_maximized = false;

        public string m_str_logs_path = ""; // Path where the log files are stored
        public string m_str_cur_prj_file = ""; // Path and name of the current drivePACK project file
        public string m_str_cur_rom_file = ""; // Path and name of the currently open drive pack ROM file
        public string m_str_cur_cod_file = ""; // Path and name of the currently imported or exported drive pack COD theme file
        public string m_str_default_theme_file = "default_theme.cod"; // The file that contains the theme code used to initialize a new theme with default content
        public string m_str_last_used_COM = "";// the last COM port used in communications with remote drivePACK

        // Persistent configuration parameters:
        public string m_str_last_prj_file = ""; // Path and name of the last open and valid drivePACK project file        
        public string m_str_last_rom_file = ""; // Path and name of the last open and valid drive pack ROM file        
        public string m_str_last_cod_file = ""; // Path and name of the last imported or exported drive pack COD theme file
        public bool m_b_new_log_per_sesion = false; // Flag used to indicate whether the application should create a new log file each time it starts
        public string m_str_color_set = "STANDARD"; // String with the used color code


        /*******************************************************************************
        *  @brief Procedure that updates the form and other configuration parameters of the 
        *  application with the settings read from the configuration parameters of the config.xml.
        *  @return ErrCode with the error code, or cErrCodes.ERR_NO_ERROR if there is no error.
        *******************************************************************************/
        public ErrCode LoadConfigParameters(){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string  str_aux = "";

            try {

                m_i_screen_size_x = Settings.Default.iScreenSizeX;
                m_i_screen_size_y = Settings.Default.iScreenSizeY;
                m_i_screen_orig_x = Settings.Default.iScreenSizeX;
                m_i_screen_orig_y = Settings.Default.iScreenSizeY;

                m_b_screen_maximized = Settings.Default.bScreenMaximized;

                m_str_last_prj_file = Settings.Default.strLastPrjFile;
                m_str_last_rom_file = Settings.Default.strLastRomFile;
                m_str_last_cod_file = Settings.Default.strLastCodFile;
                m_str_color_set = Settings.Default.strColorSet;
                m_str_last_used_COM = Settings.Default.strLastCOMPort;
                m_b_new_log_per_sesion = Settings.Default.bNewLogPerSesion;

            } catch {

                ec_ret_val = cErrCodes.ERR_CONFIG_LOADING_FILE;

            }//try

            return ec_ret_val;

        }//LoadConfigParameters

        /*******************************************************************************
        * @brief Procedure that saves the application state in the config.xml file.
        * @return >=0 if parameters were succesfully stored, <0 with the error code.
        *******************************************************************************/
        public ErrCode SaveConfigParameters(){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;

            try{

                Settings.Default.iScreenSizeX = m_i_screen_size_x;
                Settings.Default.iScreenSizeY = m_i_screen_size_y;
                Settings.Default.iScreenSizeX = m_i_screen_orig_x;
                Settings.Default.iScreenSizeY = m_i_screen_orig_y;

                Settings.Default.bScreenMaximized = m_b_screen_maximized;

                Settings.Default.strLastPrjFile = m_str_last_prj_file;
                Settings.Default.strLastRomFile = m_str_last_rom_file;
                Settings.Default.strLastCodFile = m_str_last_cod_file;
                Settings.Default.strColorSet = m_str_color_set;
                Settings.Default.strLastCOMPort = m_str_last_used_COM;
                Settings.Default.bNewLogPerSesion = m_b_new_log_per_sesion;

                Settings.Default.Save();

            }catch{

                ec_ret_val = cErrCodes.ERR_CONFIG_SAVING_FILE;

            }//try

            return ec_ret_val;

        }//SaveConfigParameters


    }// class cConfig

}
