using System;

// **********************************************************************************
// ****                          drivePACK Editor                                ****
// ****                      www.tolaemon.com/dpacked                            ****
// ****                              Source code                                 ****
// ****                              20/12/2023                                  ****
// ****                            Jordi Bartolome                               ****
// ****                                                                          ****
// ****          IMPORTANT:                                                      ****
// ****          Using this code or any part of it means accepting all           ****
// ****          conditions exposed in: http://www.tolaemon.com/dpacked          ****
// **********************************************************************************

namespace drivePackEd
{


    /*******************************************************************************
    *  @brief defines the object used to store the information of the errors or 
    *  evens that occur during application execution
    *******************************************************************************/
    public class ErrCode{
        public int i_code; // the number that uniquely identifies the error type
        public string str_description;
        public bool b_popup;// true to indicate that the result must be shown in pop up, false if the result has only to be saved in the logs

        public ErrCode(int code, string description){
            i_code = code;
            str_description = description;
            b_popup = false;
        }//ErrCode

        public ErrCode(int code, string description, bool popup) {
            i_code = code;
            str_description = description;
            b_popup = popup;
        }//ErrCode

    }//ErrCode

    /*******************************************************************************
    *  @brief with the code number and description of the different errors and events 
    *  that can take place during application exectuion.
    *******************************************************************************/
    static class cErrCodes
    {
               
        // GENERAL                                                              
        public static readonly ErrCode ERR_NO_ERROR                               = new ErrCode(0, "Success.",false);
        public static readonly ErrCode ERR_OPENING_DLL                            = new ErrCode(-1, "Error trying to locate DLL.", true);
        public static readonly ErrCode ERR_INITIALIZATION                         = new ErrCode(-2, "Error initializing application and clearing data structures.", true);
        public static readonly ErrCode ERR_NO_THEME_SELECTED                      = new ErrCode(-3, "There is no theme selected.", true);
        public static readonly ErrCode ERR_OPERATION_CANCELLED                    = new ErrCode(-4, "Operation cancelled by user.", false);

        // LOGGER ERRORS                                                        
        public static readonly ErrCode ERR_LOG_CREATING                           = new ErrCode(-100, "Error creating the log file.", true);

        // CONFIG ERRORS
        public static readonly ErrCode ERR_CONFIG_OPENING                         = new ErrCode(-200, "Error opening configuration file.", true);
        public static readonly ErrCode ERR_CONFIG_UPDATING_APP                    = new ErrCode(-201, "Error updating the state according to configuration parameters.", true);
        public static readonly ErrCode ERR_CONFIG_UPDATING_PARAMS                 = new ErrCode(-202, "Error updating configuration parameters with application state.", true);
        public static readonly ErrCode ERR_CONFIG_LOADING_FILE                    = new ErrCode(-203, "Error loading .xml config file.", true);
        public static readonly ErrCode ERR_CONFIG_SAVING_FILE                     = new ErrCode(-204, "Error saving .xml config file.", true);

        // FILE ERRORS
        public static readonly ErrCode ERR_FILE_OPENING                           = new ErrCode(-300, "Error opening drive pack file.", true);
        public static readonly ErrCode ERR_FILE_NOT_EXIST                         = new ErrCode(-301, "File does not exist.", true);
        public static readonly ErrCode ERR_FILE_INVALID_VERSION                   = new ErrCode(-302, "File invalid version.", true);
        public static readonly ErrCode ERR_FILE_INVALID_TYPE                      = new ErrCode(-303, "Invalid file type.", true);
        public static readonly ErrCode ERR_FILE_EXCEPTION_SAVING_PROJECT          = new ErrCode(-304, "An exception occurred while savint the project.", true);
        public static readonly ErrCode ERR_FILE_SPECIFIED                         = new ErrCode(-305, "Invalid specified file.", true);
        public static readonly ErrCode ERR_FILE_CREATING                          = new ErrCode(-306, "Error creating drive pack file.", true);
        public static readonly ErrCode ERR_FILE_CREATING_CANCELLED_BY_USER        = new ErrCode(-307, "File creation cancelled by user.", true);
        public static readonly ErrCode ERR_FILE_PARSING_ELEMENTS                  = new ErrCode(-308, "Some of the file parsed elements was wrong.", true);
        public static readonly ErrCode ERR_FILE_PARSING_ROM_INFO_BLOCK            = new ErrCode(-309, "Error while parsing the content of the ROM INFO metadata block.", true);
        public static readonly ErrCode ERR_FILE_NOT_TEMES_SELECTED_TO_EXPORT      = new ErrCode(-310, "No themes selected to export. Select at least one theme from the themes list to export.", true);
        public static readonly ErrCode ERR_FILE_EXPORTING_SELECTED_FILES          = new ErrCode(-311, "There was an error while exporting selected themes.", true);
        public static readonly ErrCode ERR_FILE_IMPORTING_AT_SPECIFIED_POSITION   = new ErrCode(-312, "The specified position in the themes list is out of range.", true);
        public static readonly ErrCode ERR_FILE_IMPORT_THEMES_NO_SPACE            = new ErrCode(-313, "It is not possible to load so many themes to the themes list.", true);
        public static readonly ErrCode ERR_FILE_IMPORT_PARSING_MIDI_INFO          = new ErrCode(-314, "Error while processing the information in the MIDI file.");
        public static readonly ErrCode ERR_FILE_MID_HAS_TOO_MANY_TRACKS           = new ErrCode(-315, "The imported MIDI file has too many tracks.", true);

        // SEND RECEIVE FILE ERRORS
        public static readonly ErrCode ERR_FILE_1KXMODEM_OPEN_TEMP_FILE           = new ErrCode(-400, "Error trying to open temporary drive pack file.", true);
        public static readonly ErrCode ERR_FILE_1KXMODEM_SENDING                  = new ErrCode(-401, "Error sending drive pack file through serial port.", true);
        public static readonly ErrCode ERR_FILE_1KXMODEM_SEND_TIMEOUT             = new ErrCode(-402, "Receiver has not started communications on the expected time.", true);
        public static readonly ErrCode ERR_FILE_1KXMODEM_RECEIVING                = new ErrCode(-403, "Error receiving drive pack file through serial port.", true);
        public static readonly ErrCode ERR_FILE_1KXMODEM_RECEIVE_TIMEOUT          = new ErrCode(-404, "Sender has not started communications on the expected time.", true);

        // DECODE ERRORS
        public static readonly ErrCode ERR_DECODING_EMPTY_ROM                     = new ErrCode(-500, "There is no ROM data to decode.", true);
        public static readonly ErrCode ERR_DECODING_INVALID_M1_MARK               = new ErrCode(-501, "The M1 start address mark is not valid.", true);
        public static readonly ErrCode ERR_DECODING_INVALID_M2_MARK               = new ErrCode(-502, "The M2 start address mark is not valid.", true);
        public static readonly ErrCode ERR_DECODING_INVALID_CHORD_MARK            = new ErrCode(-503, "The Chord start address mark is not valid.", true);
        public static readonly ErrCode ERR_DECODING_INVALID_M1_ADDRESS            = new ErrCode(-504, "The M1 channel start address seems to be out of range.", true);
        public static readonly ErrCode ERR_DECODING_INVALID_M2_ADDRESS            = new ErrCode(-505, "The M2 channel start address seems to be out of range.", true);
        public static readonly ErrCode ERR_DECODING_INVALID_CHORD_ADDRESS         = new ErrCode(-506, "The Chord channel start address seems to be out of range.", true);
        public static readonly ErrCode ERR_DECODING_INVALID_INSTRUCTION           = new ErrCode(-507, "There is any error in the received instruction bytes.", true);
        public static readonly ErrCode ERR_DECODING_MIDI_NOTE_OUT_OF_RANGE        = new ErrCode(-508, "Received MIDI note is out of the keyboard range.", true);

        // EDITION ERRORS
        public static readonly ErrCode ERR_EDITION_IDX_OUT_OF_RANGE               = new ErrCode(-600, "The theme with the specified index does not exist.", true);
        public static readonly ErrCode ERR_EDITION_ADD_NEW_THEME                  = new ErrCode(-601, "There was an error when trying to add a new theme.", true);
        public static readonly ErrCode ERR_EDITION_DELETE_THEME                   = new ErrCode(-602, "There was an error when trying to delete a theme.", true);
        public static readonly ErrCode ERR_EDITION_PASTE_NEW_THEME                = new ErrCode(-603, "There was an error when trying to paste a new theme.", true);
        public static readonly ErrCode ERR_EDITION_NO_SPACE_FOR_THEMES            = new ErrCode(-605, "It is not possible to add more themes to the themes list.", true);
        public static readonly ErrCode ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM   = new ErrCode(-606, "Some of the parameters of the command to encode were wrong.", true);
        public static readonly ErrCode ERR_EDITION_TOO_MUCH_INSTRUCTIONS          = new ErrCode(-607, "It is not possible to add more instructions into the theme.", true);

        // strings with the opperations to show in the logs
        public const string COMMAND_OPEN_FILE            = "OPEN_FILE: ";
        public const string COMMAND_SAVE_FILE            = "SAVE_FILE: ";
        public const string COMMAND_NEW_FILE             = "NEW_FILE: ";
        public const string COMMAND_EDITION              = "EDITION: ";
        public const string COMMAND_SEND_FILE            = "SEND_FILE: ";
        public const string COMMAND_RECEIVE_FILE         = "RECEIVE_FILE: ";
        public const string COMMAND_BUILD_ROM            = "BUILD_ROM: ";
        public const string COMMAND_DECODE_ROM           = "DECODE_ROM: ";

    }
}
