﻿using System;

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


    /*******************************************************************************
    *  @brief defines the object used to store the information of the errors or 
    *  evens that occur during application execution
    *******************************************************************************/
    public class ErrCode{
        public int i_code;
        public string str_description;

        public ErrCode(int code, string description){
            i_code = code;
            str_description = description;
        }//ErrCode

    }//ErrCode

    /*******************************************************************************
    *  @brief with the code number and description of the different errors and events 
    *  that can take place during application exectuion.
    *******************************************************************************/
    static class cErrCodes
    {
        // GENERAL                                                              
        public static readonly ErrCode ERR_NO_ERROR                               = new ErrCode(0, "Success.");
        public static readonly ErrCode ERR_OPENING_DLL                            = new ErrCode(-1, "Error trying to locate DLL.");
        public static readonly ErrCode ERR_INITIALIZATION                         = new ErrCode(-2, "Error initializing application and clearing data structures.");
        public static readonly ErrCode ERR_NO_THEME_SELECTED                      = new ErrCode(-3, "There is no theme selected.");
        public static readonly ErrCode ERR_OPERATION_CANCELLED                    = new ErrCode(-4, "The operation has been aborted.");

        // LOGGER ERRORS                                                        
        public static readonly ErrCode ERR_LOG_CREATING                           = new ErrCode(-100, "Error creating the log file.");

        // CONFIG ERRORS
        public static readonly ErrCode ERR_CONFIG_OPENING                         = new ErrCode(-200, "Error opening configuration file.");
        public static readonly ErrCode ERR_CONFIG_UPDATING_APP                    = new ErrCode(-201, "Error updating the state according to configuration parameters.");
        public static readonly ErrCode ERR_CONFIG_UPDATING_PARAMS                 = new ErrCode(-202, "Error updating configuration parameters with application state.");
        public static readonly ErrCode ERR_CONFIG_LOADING_FILE                    = new ErrCode(-203, "Error loading .xml config file.");
        public static readonly ErrCode ERR_CONFIG_SAVING_FILE                     = new ErrCode(-204, "Error saving .xml config file.");

        // FILE ERRORS
        public static readonly ErrCode ERR_FILE_OPENING                           = new ErrCode(-300, "Error opening drive pack file.");
        public static readonly ErrCode ERR_FILE_NOT_EXIST                         = new ErrCode(-301, "File does not exist.");
        public static readonly ErrCode ERR_FILE_INVALID_VERSION                   = new ErrCode(-302, "File invalid version.");
        public static readonly ErrCode ERR_FILE_INVALID_TYPE                      = new ErrCode(-303, "Invalid file type.");
        public static readonly ErrCode ERR_FILE_SPECIFIED                         = new ErrCode(-304, "Invalid specified file.");
        public static readonly ErrCode ERR_FILE_CREATING                          = new ErrCode(-305, "Error creating drive pack file.");
        public static readonly ErrCode ERR_FILE_CREATING_CANCELLED_BY_USER        = new ErrCode(-306, "File creation cancelled by user.");
        public static readonly ErrCode ERR_FILE_PARSING_ELEMENTS                  = new ErrCode(-307, "Some of the file parsed elements was wrong.");

        // SEND RECEIVE FILE ERRORS
        public static readonly ErrCode ERR_FILE_1KXMODEM_OPEN_TEMP_FILE           = new ErrCode(-400, "Error trying to open temporary drive pack file.");
        public static readonly ErrCode ERR_FILE_1KXMODEM_SENDING                  = new ErrCode(-401, "Error sending drive pack file through serial port.");
        public static readonly ErrCode ERR_FILE_1KXMODEM_SEND_TIMEOUT             = new ErrCode(-402, "Receiver has not started communications on the expected time.");
        public static readonly ErrCode ERR_FILE_1KXMODEM_RECEIVING                = new ErrCode(-403, "Error receiving drive pack file through serial port.");
        public static readonly ErrCode ERR_FILE_1KXMODEM_RECEIVE_TIMEOUT          = new ErrCode(-404, "Sender has not started communications on the expected time.");

        // DECODE ERRORS
        public static readonly ErrCode ERR_DECODING_EMPTY_ROM                     = new ErrCode(-500, "There is no ROM data to decode.");
        public static readonly ErrCode ERR_DECODING_INVALID_M1_MARK               = new ErrCode(-501, "The M1 start address mark is not valid.");
        public static readonly ErrCode ERR_DECODING_INVALID_M2_MARK               = new ErrCode(-502, "The M2 start address mark is not valid.");
        public static readonly ErrCode ERR_DECODING_INVALID_CHORD_MARK            = new ErrCode(-503, "The Chord start address mark is not valid.");
        public static readonly ErrCode ERR_DECODING_INVALID_M1_ADDRESS            = new ErrCode(-504, "The M1 channel start address seems to be out of range.");
        public static readonly ErrCode ERR_DECODING_INVALID_M2_ADDRESS            = new ErrCode(-505, "The M2 channel start address seems to be out of range.");
        public static readonly ErrCode ERR_DECODING_INVALID_CHORD_ADDRESS         = new ErrCode(-506, "The Chord channel start address seems to be out of range.");

        // EDITION ERRORS
        public static readonly ErrCode ERR_EDITION_IDX_OUT_OF_RANGE               = new ErrCode(-600, "The theme with the specified index does not exist.");


        // strings with the opperations to show in the logs
        public const string COMMAND_OPEN_FILE            = "OPEN_FILE: ";
        public const string COMMAND_SAVE_FILE            = "SAVE_FILE: ";
        public const string COMMAND_EDITION              = "EDITION: ";
        public const string COMMAND_SEND_FILE            = "SEND_FILE: ";
        public const string COMMAND_RECEIVE_FILE         = "RECEIVE_FILE: ";
        public const string COMMAND_BUILD_ROM            = "BUILD_ROM: ";
        public const string COMMAND_DECODE_ROM           = "DECODE_ROM: ";

    }
}
