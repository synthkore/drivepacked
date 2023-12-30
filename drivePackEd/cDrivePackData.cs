using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Be.Windows.Forms;
using System.Linq;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
    *  @brief defines the object with all the current themes information: that is the   
    *  object that contains a list with all the themes, and each ThemeCode contains 
    *  the code of the M1, M2 and chords channels.
    *******************************************************************************/
    public class Themes {

        public List<ThemeCode> liThemesCode = new List<ThemeCode>(); // list with all the themes, each theme conta
        public int iCurrThemeIdx;// current selected Theme index

        // SNG file headers
        const string STR_SNG_FILE_N_THEMES = ";n_themes:";
        const string STR_SNG_FILE_SEQ_N = ";seq_n:";
        const string STR_SNG_FILE_SEQ_TITLE = ";seq_title:";
        const string STR_SNG_FILE_N_M1_CHAN_ENTRIES = ";n_m1_chan_entries:";
        const string STR_SNG_FILE_M1_CHAN_ENTRIES = ";m1_chan_entries:";
        const string STR_SNG_FILE_N_M2_CHAN_ENTRIES = ";n_m2_han_entries:";
        const string STR_SNG_FILE_M2_CHAN_ENTRIES = ";m2_han_entries:";
        const string STR_SNG_FILE_N_CHORD_CHAN_ENTRIES = ";n_chord_chan_entries:";
        const string STR_SNG_FILE_CHORD_CHAN_ENTRIES = ";chord_chan_entries:";

        

        /*******************************************************************************
        * @brief Adds a new theme to the list of themes
        * @return the ErrCode with the result or error of the operation.
        *******************************************************************************/
        public ErrCode AddNewTheme() {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR;
            ThemeCode newSong = new ThemeCode();

            liThemesCode.Add(newSong);

            // set the current Theme index pointing to the added Theme
            iCurrThemeIdx = liThemesCode.Count - 1;

            return erCodeRetVal;

        }//AddNewTheme


        /*******************************************************************************
        * @brief Inserts a new Theme at the specified iIdx position of the Themes list
        * @param[in] iIdx with the position in the Themes list at which the new Theme will 
        * be inserted.
        * @return the ErrCode with the result or error of the operation.
        ********************************************************************************/
        public ErrCode InsertNewTheme(int iIdx) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR;
            ThemeCode newSong = null;

            // if the received iIdx is out of range, then add the new Theme at the end of the list
            if ((iIdx < 0) || (iIdx >= liThemesCode.Count)) iIdx = liThemesCode.Count;

            if (iIdx < 255) {
                newSong = new ThemeCode();

                // set the current song Theme pointing to the inserted song
                liThemesCode.Insert(iIdx, newSong);
                iCurrThemeIdx = iIdx;
            }

            return erCodeRetVal;

        }//InsertNewTheme


        /*******************************************************************************
        * @brief Deletes the song at the specified iIdx position in the Theme list
        * @param[in] iIdx with the position in the songs list of the song to delete.
        * @return the ErrCode with the result or error of the operation.
        ********************************************************************************/
        public ErrCode DeleteTheme(int iIdx) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR;
            ThemeCode newSong = null;

            // check if the received iIdx is in a valid range
            if ((iIdx >= 0) && (iIdx < liThemesCode.Count)) {

                liThemesCode.RemoveAt(iIdx);

                // if the index of the current selected Theme is over the index of the deleted
                // Theme, then move it back one position to keep it pointing to the same element
                // it was pointing to before deleting the specified Theme. 
                if (iIdx <= iCurrThemeIdx) iCurrThemeIdx--;
                if ((iCurrThemeIdx < 0) && (liThemesCode.Count > 0)) { iCurrThemeIdx = 0; }

            }

            return erCodeRetVal;

        }//DeleteTheme


        /*******************************************************************************
        * @brief To get the currently selected Theme of the list ot Themes
        * @return a reference to the currently selected Theme or null if there is no any 
        * selected theme.
        *******************************************************************************/
        public ThemeCode GetCurrTheme() {
            ThemeCode retSong = null;

            if ((iCurrThemeIdx != -1) && (liThemesCode.Count > 0)) {
                retSong = liThemesCode[iCurrThemeIdx];
            }

            return retSong;

        }//GetCurrTheme


        /*******************************************************************************
        *  @brief Default constructor
        *******************************************************************************/
        public Themes() {
            iCurrThemeIdx = -1;
        }


        /*******************************************************************************
        *  @brief Deletes all the themes in the object
        *  @return >0 with the number of deleted themes, or <=0 if the list of Themes
        *  could not be cleared.
        *******************************************************************************/
        public int deleteAllThemes() {
            int retVal = 0;


            if (liThemesCode.Count > 0) {
                retVal = liThemesCode.Count;
                liThemesCode.Clear();
            } else {
                retVal = -1;
            }
                   
            iCurrThemeIdx = -1;

            return retVal;

        }//deleteAllThemes


        /*******************************************************************************
        * @brief Saves into a file all the current themes information.
        * @param[in] str_save_file with the name of the file to save the songs programs 
        * in.
        * @return the ErrCode with the result or error of the operation, if ErrCode>0 
        * file has been succesfully saved, if <0 an error occurred
        *******************************************************************************/
        public ErrCode saveSNGFile(string str_save_file){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            StreamWriter file_text_writer;
            ASCIIEncoding ascii = new ASCIIEncoding();
            string str_line = "";
            int iSeqN = 0;

            file_text_writer = new StreamWriter(str_save_file);

            if (file_text_writer == null){
                ec_ret_val = cErrCodes.ERR_FILE_CREATING;
            }//if

            if (ec_ret_val.i_code >= 0) {

                // first the number of themes in the list of themes
                str_line = STR_SNG_FILE_N_THEMES;
                file_text_writer.Write(str_line+"\r\n");
                str_line = liThemesCode.Count.ToString();
                file_text_writer.Write(str_line + "\r\n");

                // save the information of each Theme 
                iSeqN = 0;
                foreach (ThemeCode seq in liThemesCode) {

                    // the index of the song
                    str_line = STR_SNG_FILE_SEQ_N;
                    file_text_writer.Write(str_line + "\r\n");
                    str_line = iSeqN.ToString();
                    file_text_writer.Write(str_line + "\r\n");

                    // the title of the song
                    str_line = STR_SNG_FILE_SEQ_TITLE;
                    file_text_writer.Write(str_line + "\r\n");
                    str_line = seq.strThemeTitle;
                    file_text_writer.Write(str_line + "\r\n");

                    // the number of M1 channel code entries
                    str_line = STR_SNG_FILE_N_M1_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    str_line = seq.liM1CodeInstr.Count.ToString();
                    file_text_writer.Write(str_line + "\r\n");

                    // all the current song M1 channel code entries
                    str_line = STR_SNG_FILE_M1_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    foreach (MChannelCodeEntry melChanEntry in seq.liM1CodeInstr) {
                        str_line = "";
                        str_line = str_line + "0x" + melChanEntry.by0.ToString("X2") + ",";
                        str_line = str_line + "0x" + melChanEntry.by1.ToString("X2") + ",";
                        str_line = str_line + "0x" + melChanEntry.by2.ToString("X2");
                        file_text_writer.Write(str_line + "\r\n");
                    }//foreach

                    // the number of M2 channel code entries
                    str_line = STR_SNG_FILE_N_M2_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    str_line = seq.liM2CodeInstr.Count.ToString();
                    file_text_writer.Write(str_line + "\r\n");

                    // all the current song M2 channel code entries
                    str_line = STR_SNG_FILE_M2_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    foreach (MChannelCodeEntry melChanEntry in seq.liM2CodeInstr) {
                        str_line = "";
                        str_line = str_line + "0x" + melChanEntry.by0.ToString("X2") + ",";
                        str_line = str_line + "0x" + melChanEntry.by1.ToString("X2") + ",";
                        str_line = str_line + "0x" + melChanEntry.by2.ToString("X2");
                        file_text_writer.Write(str_line + "\r\n");
                    }//foreach

                    // the number of chord channel code entries
                    str_line = STR_SNG_FILE_N_CHORD_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    str_line = seq.liChordCodeInstr.Count.ToString();
                    file_text_writer.Write(str_line + "\r\n");

                    // all the current song chords channel code entries
                    str_line = STR_SNG_FILE_CHORD_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    foreach (ChordChannelCodeEntry chordChanEntry in seq.liChordCodeInstr) {
                        str_line = "";
                        str_line = str_line + "0x" + chordChanEntry.by0.ToString("X2") + ",";
                        str_line = str_line + "0x" + chordChanEntry.by1.ToString("X2");// + ",";
                        // str_line = "0x" + melChanEntry.by2.ToString("X2");
                        file_text_writer.Write(str_line + "\r\n");
                    }//foreach

                    iSeqN++;

                }//foreach

            }//if

            file_text_writer.Close();

            return ec_ret_val;

        }//saveSNGFile


        /*******************************************************************************
        * @brief  Loads to the themes object the themes stored into the specified 
        * file.
        * @param[in] str_load_file with the name of the file to load the themes from
        * @return the ErrCode with the result or error of the operation, if ErrCode>0 
        * file has been succesfully loaded into the object, if <0 an error occurred
        *******************************************************************************/
        public ErrCode loadSNGFile(string str_load_file) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            StreamReader file_text_reader;
            ASCIIEncoding ascii = new ASCIIEncoding();
            ThemeCode themeAux = null;
            MChannelCodeEntry MCodeEntryAux = null;
            ChordChannelCodeEntry chordCodeEntryAux = null;
            string[] arrEntryElems = null;
            string strLine = "";
            bool bReadLineIsHeader = false;// flag to indicate if last read line corresponds to a file section header or to a regular file content line
            string strCurrSection = "";
            int iTotalThemes = 0;
            int iM1TotalChannelEntries = 0;
            int iM2TotalChannelEntries = 0;
            int iChordChannelEntries = 0;
            int iCurrThemeN = 0;

            if (!File.Exists(str_load_file)) {

                ec_ret_val = cErrCodes.ERR_FILE_NOT_EXIST;

            }else{

                file_text_reader = new StreamReader(str_load_file);

                if (file_text_reader == null) {
                    ec_ret_val = cErrCodes.ERR_FILE_CREATING;
                }//if

                if (ec_ret_val.i_code >= 0) {

                    // clear the content of the object before loading the new content 
                    liThemesCode.Clear();
                    iCurrThemeIdx = -1;

                    strCurrSection = "";
                    
                    while ((ec_ret_val.i_code>=0) && ((strLine = file_text_reader.ReadLine()) != null) ){

                        strLine = strLine.Trim();

                        bReadLineIsHeader = false;
                        
                        // check if the read line corresponds to a section header line and update the strCurrSection if affirmative
                        switch (strLine) {
                            case STR_SNG_FILE_N_THEMES:
                                strCurrSection = STR_SNG_FILE_N_THEMES;
                                bReadLineIsHeader = true;
                                break;
                            case STR_SNG_FILE_SEQ_N:
                                strCurrSection = STR_SNG_FILE_SEQ_N;
                                bReadLineIsHeader = true;
                                break;
                            case STR_SNG_FILE_SEQ_TITLE:
                                strCurrSection = STR_SNG_FILE_SEQ_TITLE;
                                bReadLineIsHeader = true;
                                break;
                            case STR_SNG_FILE_N_M1_CHAN_ENTRIES:
                                strCurrSection = STR_SNG_FILE_N_M1_CHAN_ENTRIES;
                                bReadLineIsHeader = true;
                                break;
                            case STR_SNG_FILE_M1_CHAN_ENTRIES:
                                strCurrSection = STR_SNG_FILE_M1_CHAN_ENTRIES;
                                bReadLineIsHeader = true;
                                break;
                            case STR_SNG_FILE_N_M2_CHAN_ENTRIES:
                                strCurrSection = STR_SNG_FILE_N_M2_CHAN_ENTRIES;
                                bReadLineIsHeader = true;
                                break;
                            case STR_SNG_FILE_M2_CHAN_ENTRIES:
                                strCurrSection = STR_SNG_FILE_M2_CHAN_ENTRIES;
                                bReadLineIsHeader = true;
                                break;
                            case STR_SNG_FILE_N_CHORD_CHAN_ENTRIES:
                                strCurrSection = STR_SNG_FILE_N_CHORD_CHAN_ENTRIES;
                                bReadLineIsHeader = true;
                                break;
                            case STR_SNG_FILE_CHORD_CHAN_ENTRIES:
                                strCurrSection = STR_SNG_FILE_CHORD_CHAN_ENTRIES;
                                bReadLineIsHeader = true;
                                break;
                        }//switch

                        // if the line read in that iteration does not correspond to a header section line then process 
                        // it as regular file line according to the kind of section that is being processed
                        if (bReadLineIsHeader == false) {

                            // process the read line as a regular file line in one or another way deppending on the current section
                            switch (strCurrSection) {

                                case STR_SNG_FILE_N_THEMES:
                                    iTotalThemes = Convert.ToInt32(strLine);
                                    break;

                                case STR_SNG_FILE_SEQ_N:
                                    iCurrThemeN = Convert.ToInt32(strLine);
                                    themeAux = new ThemeCode();
                                    liThemesCode.Add(themeAux);
                                    // set the last loaded song as current selected theme
                                    iCurrThemeIdx = iCurrThemeN;
                                    break;

                                case STR_SNG_FILE_SEQ_TITLE:
                                    liThemesCode[iCurrThemeN].strThemeTitle = strLine;
                                    break;

                                case STR_SNG_FILE_N_M1_CHAN_ENTRIES:
                                    iM1TotalChannelEntries = Convert.ToInt32(strLine);
                                    break;

                                case STR_SNG_FILE_M1_CHAN_ENTRIES:
                                    strLine = strLine.Replace("0x", "");
                                    arrEntryElems = strLine.Split(',');
                                    if (arrEntryElems.Count() == 3) {
                                        MCodeEntryAux = new MChannelCodeEntry();
                                        MCodeEntryAux.by0 = Convert.ToByte(arrEntryElems[0], 16);
                                        MCodeEntryAux.by1 = Convert.ToByte(arrEntryElems[1], 16);
                                        MCodeEntryAux.by2 = Convert.ToByte(arrEntryElems[2], 16);
                                        liThemesCode[iCurrThemeN].liM1CodeInstr.Add(MCodeEntryAux);
                                    } else {
                                        ec_ret_val = cErrCodes.ERR_FILE_PARSING_ELEMENTS;
                                    }
                                    break;

                                case STR_SNG_FILE_N_M2_CHAN_ENTRIES:
                                    iM2TotalChannelEntries = Convert.ToInt32(strLine);
                                    break;

                                case STR_SNG_FILE_M2_CHAN_ENTRIES:
                                    strLine = strLine.Replace("0x", "");
                                    arrEntryElems = strLine.Split(',');
                                    if (arrEntryElems.Count() == 3) {
                                        MCodeEntryAux = new MChannelCodeEntry();
                                        MCodeEntryAux.by0 = Convert.ToByte(arrEntryElems[0], 16);
                                        MCodeEntryAux.by1 = Convert.ToByte(arrEntryElems[1], 16);
                                        MCodeEntryAux.by2 = Convert.ToByte(arrEntryElems[2], 16);
                                        liThemesCode[iCurrThemeN].liM2CodeInstr.Add(MCodeEntryAux);
                                    } else {
                                        ec_ret_val = cErrCodes.ERR_FILE_PARSING_ELEMENTS;
                                    }
                                    break;

                                case STR_SNG_FILE_N_CHORD_CHAN_ENTRIES:
                                    iChordChannelEntries = Convert.ToInt32(strLine);
                                    break;

                                case STR_SNG_FILE_CHORD_CHAN_ENTRIES:
                                    strLine = strLine.Replace("0x", "");
                                    arrEntryElems = strLine.Split(',');
                                    if (arrEntryElems.Count() == 2) {
                                        chordCodeEntryAux = new ChordChannelCodeEntry();
                                        chordCodeEntryAux.by0 = Convert.ToByte(arrEntryElems[0],16);
                                        chordCodeEntryAux.by1 = Convert.ToByte(arrEntryElems[1], 16);
                                        // seqChordEntryAux.by2 = Convert.ToByte(arrEntryElems[2]);
                                        liThemesCode[iCurrThemeN].liChordCodeInstr.Add(chordCodeEntryAux);
                                    } else {
                                        ec_ret_val = cErrCodes.ERR_FILE_PARSING_ELEMENTS;
                                    }
                                    break;

                            }//switch

                        }//if (bReadLineIsHeader == false) 

                    }//while
                
                }//if

                file_text_reader.Close();

            }//if

            return ec_ret_val;

        }//loadSNGFile

    }// themes


    // Contains the 3 lists with the instructions for each theme's channel. A theme is composed of 3 lists of code, and 
    // each list of code implements the sequence of notes or chords on each channel.
    public class ThemeCode {

        public string strThemeTitle = "";
        public BindingList<MChannelCodeEntry> liM1CodeInstr; // list with all the code entries of the Melody 1 channel
        public BindingList<MChannelCodeEntry> liM2CodeInstr; // list with all the code entries of the Melody 2 channel
        public BindingList<ChordChannelCodeEntry> liChordCodeInstr; // list with all the  code entries of the Melody 2 channel
        public int iCurrM1InstrIdx;// current Melody 1 channel selected instruction index
        public int iCurrM2InstrIdx;// current Melody 2 channel selected instruction index
        public int iCurrChInstrIdx;// current chord channel selected instruction index

        /*******************************************************************************
        * @brief Default constructor
        *******************************************************************************/
        public ThemeCode() {
            MChannelCodeEntry mPrAux = null;

            strThemeTitle = "Song title here";

            liM1CodeInstr = new BindingList<MChannelCodeEntry>();
            liM2CodeInstr = new BindingList<MChannelCodeEntry>();
            liChordCodeInstr = new BindingList<ChordChannelCodeEntry>();
            iCurrM1InstrIdx = -1;
            iCurrM2InstrIdx = -1;
            iCurrChInstrIdx = -1;

        }//sequence


    }// sequence


    // groups all the fields of a Melody entry ( for Melody1 or Melody2 )
    public class MChannelCodeEntry {
        public byte by0;
        public byte by1;
        public byte by2;


        /*******************************************************************************
        * @brief Default constructor
        *******************************************************************************/
        public MChannelCodeEntry() {
            by0 = 0x00;
            by1 = 0x00;
            by2 = 0x00;
        }


        /*******************************************************************************
        * @brief Constructor with parameters
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        *******************************************************************************/
        public MChannelCodeEntry(byte _by0, byte _by1, byte _by2) {
            by0 = _by0;
            by1 = _by1;
            by2 = _by2;
        }

    }//sequenceMelodyChannelEntry


    // groups all the fields of a Chord entry
    public class ChordChannelCodeEntry {

        public byte by0;
        public byte by1;

        public ChordChannelCodeEntry() {
            by0 = 0x00;
            by1 = 0x00;
        }

        /*******************************************************************************
        * @brief Constructor with parameters
        * @param[in] _by0
        * @param[in] _by1
        *******************************************************************************/
        public ChordChannelCodeEntry(byte _by0, byte _by1) {
            by0 = _by0;
            by1 = _by1;
        }

    }//SequenceChordChannelEntry


    public class cDrivePackData{

        // constants
        public const int MAX_ROWS_PER_CHANNEL = 2048; //the maximum number of elements in a channel
        public const int FILE_METADATA_TITLE = 0x01;
        public const int FILE_METADATA_SONGS_INFO = 0x02;
        public const int FILE_METADATA_SONGS_ROM = 0x03;
        public const int ROM_MAX_SIZE = 0x8000;  // 4095 bloc * (16 nibbles/bloc) = 65520 nibbles / 2 = 32768 bytes = 0x8000 bytes

        // ROM PACK content offests
        const int I_OFFSET_NUM_PIECES                    = 6;
        const int I_OFFSET_BEGIN_VACANT_ADDRESS          = 8;
        const int I_OFFSET_THEMES_START_ADDRESS          = 11;

        const int I_OFFSET_M1_START_ADDRESS    =  0; // offset of the Melody start address respect the current theme start address, preceeded by 0x00 mark
        const int I_OFFSET_M2_START_ADDRESS    =  4; // offset of the Obbligatto start address respect the current theme start address, preceeded by 0x01 mark
        const int I_OFFSET_CHORD_START_ADDRESS =  8; // offset of the Chords start address respect the current theme start address, preceeded by 0x20 mark
        const int I_OFFSET_THEME_END_ADDRESS   = 12; // IMPORTANT: NOT DOCUMENTED IN CASIO PATENT: offset of the end address of the curren theme, preceeded by 0xFF mark

        const int I_WORK_DATA_SIZE                   = 6; // bytes 
        const int I_NUM_PIECES_SIZE                  = 2; // bytes
        const int I_VACANT_START_ADDRESS_SIZE        = 3; // bytes
        const int I_THEME_START_ADDRESS_SIZE         = 3; // bytes 
        const int I_M1_CHAN_START_ADDRESS_SIZE       = 3; // bytes 
        const int I_M2_CHAN_START_ADDRESS_SIZE       = 3; // bytes 
        const int I_CHORD_CHAN_START_ADDRESS_SIZE    = 3; // bytes 
        // IMPORTANT: it is not documented in CASIO patent but it seems that the PIECE HEADER has an extra group of 4 bytes at the end that correpsonds to the
        // address of the end of the current piece, that is where the following theme starts. So the PIECE HEADER information would be:
        // 0x00 + I_M1_CHAN_START_ADDRESS_SIZE + 0x01 + I_M2_CHAN_START_ADDRESS_SIZE + 0x20 + I_CHORD_CHAN_START_ADDRESS_SIZE + 0xFF + I_FOLLOW_THEME_START_ADDRESS_SIZE
        const int I_FOLLOW_THEME_START_ADDRESS_SIZE  = 3; // bytes 
        const int I_PIECE_HEADER_SIZE                = 1 + I_M1_CHAN_START_ADDRESS_SIZE + 1 + I_M2_CHAN_START_ADDRESS_SIZE + 1 + I_CHORD_CHAN_START_ADDRESS_SIZE + 1 + I_FOLLOW_THEME_START_ADDRESS_SIZE;
        const int I_MELODY_CODE_ENTRY_SIZE           = 3; // bytes 
        const int I_CHORDS_CODE_ENTRY_SIZE           = 2; // bytes 

        // attributes
        public string strTitle = "";
        public string strSongInfo = "";
        public DynamicByteProvider dynbyprMemoryBytes; // reference to the dynamic bytes provider
        private bool bDataChanged; // flag to indicate if in the dirvePackis there are changes pending to save         
        public Themes themes = null; // object with a list with all the songs information
        private cLogsNErrors statusNLogsRef = null;// a reference to the logs to allow the objects of this class write information into the logs.

        /******************************************************************************
        * @brief Default constructor.
        * @param[in] _statusNLogsRef reference to the Logs and Errors recording
        * object to allow the objects of that class write information into the logs.
        *******************************************************************************/
        public cDrivePackData(cLogsNErrors _statusNLogsRef) {

            dynbyprMemoryBytes = null;
            bDataChanged = false;
            strTitle = "";
            strSongInfo = "";
            themes = new Themes();
            statusNLogsRef = _statusNLogsRef;

        }//DrivePackData


        /*******************************************************************************
        * @brief Receives an array with the bytes of a MELODY channel instruction and
        * returns a description of the instruction in that bytes.
        * @param[in] arrByInstr array of bytes
        * @return the string with the explanation of the MELODY instruction encoded in 
        * the received bytes.
        *******************************************************************************/
        public static string describeMelodyInstructionBytes(byte[] arrByInstr) {
            string strRet = "";

            if (arrByInstr.Count() == 3) {

            }
            
            return strRet;

        }//describeMelodyInstructionBytes


        /*******************************************************************************
        * @brief Receives an array with the bytes of a CHORD channel instruction and
        * returns a description of the instruction in that bytes.
        * @param[in] arrByInstr array of bytes
        * @return the string with the explanation of the CHORD instruction encoded in 
        * the received bytes.
        *******************************************************************************/
        public static string describeChordInstructionBytes(byte[] arrByInstr) {
            string strRet = "";

            if (arrByInstr.Count() == 3) {

            }

            return strRet;

        }//describeChordInstructionBytes


        /*******************************************************************************
        * @brief Initialize the drivePackData object.
        * @param[in] str_default_file the name of the file used to initialize the content
        * of the drivePackData object.
        *******************************************************************************/
        public void Initialize(string str_default_file){
            byte[] by_memory_bytes = null;
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;

            // if str_default_file contains a file name and it exists initialize the object with the file content 
            if ((str_default_file != "") && (File.Exists(str_default_file))){

                ec_ret_val = loadDRPFile(str_default_file);

            }else{

                // create the DynamicByteProvider and initilized with the previously initialized byte array
                by_memory_bytes = new byte[ROM_MAX_SIZE];

                this.strTitle = "Enter RO-XXX Title of the cart here.";
                this.strSongInfo = "Enter the list of the cart songs here:\r\n[1]-Son 1 name\r\n[2]-Son 2 name\r\n[3]-Son 3 name\r\n[4]-Son 3 name\r\n";

                // re initialize the DynamicByteProvider with the bytes read from the file
                dynbyprMemoryBytes = new DynamicByteProvider(by_memory_bytes);

            }//if

        }//Initialize


        /*******************************************************************************
        * @brief Getter setter of the flag used to indicate if in the DynamicByteProvider 
        *  there are pending modifications to save.  
        * @param[in] setModified: true to specify that the DynamicByteProvider has pending to 
        *  store modifications. false to specify that the DynamicByteProvider has no pending
        *  modifications to store.
        *******************************************************************************/
        public bool dataChanged
{
            set
            { bDataChanged = value; }
            get
            { return bDataChanged; }

        }//dataChanged


        /*******************************************************************************
        * @brief Loads data from a file in ROMPACKv00 format and stores it into the  
        * drivePackData object. 
        * @param[in] file_stream  binary file stream
        * @param[in] file_binary_reader  binary stream reader
        * @param[in] ui_read_bytes number of byts read from the file 
        * @param[out] ui_read_bytes the number of bytes read from of the file
        * @return >=0 file has been succesfully loaded into the object, <0 an error 
        * occurred 
        *******************************************************************************/
        public ErrCode loadDRP_ROMPACKv00(ref FileStream file_stream, ref BinaryReader file_binary_reader, ref uint ui_read_bytes){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            System.UInt32 ui32_data_offset = 0;
            byte[] by_read = null;
            byte by_aux = 0x00;


            // read the 4 bytes corresponding to uint32 data_offset 
            by_read = file_binary_reader.ReadBytes(4);
            ui_read_bytes = ui_read_bytes + 4;
            AuxFuncs.convert4BytesToUInt32(by_read, ref ui32_data_offset);

            // use the data_offset to jump to the data block and start reading the bytes
            file_stream.Seek(ui32_data_offset, SeekOrigin.Begin);
            List<byte> byli_byte_list = new List<byte>();
            while ((ec_ret_val.i_code >= 0) && (file_stream.Position != file_stream.Length))
            {
                by_aux = file_binary_reader.ReadByte();
                byli_byte_list.Add(by_aux);
            }//while

            // store the bytes from the bytes list to a bytes array
            byte[] by_array;
            by_array = byli_byte_list.ToArray();

            // re initialize the DynamicByteProvider with the bytes read from the file
            dynbyprMemoryBytes = new DynamicByteProvider(by_array);

            // just after loading the file data in memory corresponds exactly to what is stored in disk
            dataChanged = false;

            // set default values in the fields not implemented in ROMPACKv00 file format
            this.strTitle = "ROMPACKv00 files do not have title meta-data block. Update it and save it as ROMPACKv01";
            this.strSongInfo = "ROMPACKv00 files do not have songs information meta-data block. Update it and save it as ROMPACKv01";

            return ec_ret_val;

        }//loadDRP_ROMPACKv00


        /*******************************************************************************
        * @brief  Loads data from a file in ROMPACKv01 format and stores it into the  
        * drivePackData object.
        * @param[in] file_stream binary file stream
        * @param[in] file_binary_reader  binary stream reader
        * @param[in] ui_read_bytes number of byts read from the file 
        * @param[out] ui_read_bytes the number of bytes read from of the file
        * @return >=0 file has been succesfully loaded into the object, <0 an error 
        * occurred 
        *******************************************************************************/
        public ErrCode loadDRP_ROMPACKv01(ref FileStream file_stream, ref BinaryReader file_binary_reader, ref uint ui_read_bytes){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            ASCIIEncoding ascii = new ASCIIEncoding();
            System.UInt32 ui32_metadata_size = 0;
            byte by_metadata_block_type = 0;
            byte[] by_read = null;


            // process all the METADA_BLOCKS in the file
            while (file_stream.Position < file_stream.Length)
            {

                // read the 1 bytes corresponding to the METADATA_BLOCK_TYPE
                by_metadata_block_type = file_binary_reader.ReadByte();
                ui_read_bytes = ui_read_bytes = ui_read_bytes + 1;

                switch (by_metadata_block_type)
                {

                    case FILE_METADATA_TITLE:

                        // read the 4 bytes corresponding to the current metada block size
                        by_read = file_binary_reader.ReadBytes(4);
                        ui_read_bytes = ui_read_bytes = ui_read_bytes + 4;
                        AuxFuncs.convert4BytesToUInt32(by_read, ref ui32_metadata_size);

                        // read the ui32_metadata_size bytes of the current metadata block
                        by_read = file_binary_reader.ReadBytes((int)ui32_metadata_size);

                        // convert the read array of bytes to a string
                        this.strTitle = ascii.GetString(by_read);
                        break;

                    case FILE_METADATA_SONGS_INFO:

                        // read the 4 bytes corresponding to the current metada block size
                        by_read = file_binary_reader.ReadBytes(4);
                        ui_read_bytes = ui_read_bytes = ui_read_bytes + 4;
                        AuxFuncs.convert4BytesToUInt32(by_read, ref ui32_metadata_size);

                        // read the ui32_metadata_size bytes of the current metadata block
                        by_read = file_binary_reader.ReadBytes((int)ui32_metadata_size);

                        // convert the read array of bytes to a string
                        this.strSongInfo = ascii.GetString(by_read);
                        break;

                    case FILE_METADATA_SONGS_ROM:

                        // read the 4 bytes corresponding to the current metada block size
                        by_read = file_binary_reader.ReadBytes(4);
                        ui_read_bytes = ui_read_bytes = ui_read_bytes + 4;
                        AuxFuncs.convert4BytesToUInt32(by_read, ref ui32_metadata_size);

                        // read the ui32_metadata_size bytes of the current metadata block
                        by_read = file_binary_reader.ReadBytes((int)ui32_metadata_size);

                        // re initialize the DynamicByteProvider with the array of bytes read from the file
                        dynbyprMemoryBytes = new DynamicByteProvider(by_read);
                        break;

                    default:

                        // NOT SUPPORTED METADA BLOCK
                        // read the 4 bytes corresponding to the current metada block size
                        by_read = file_binary_reader.ReadBytes(4);
                        ui_read_bytes = ui_read_bytes = ui_read_bytes + 4;
                        AuxFuncs.convert4BytesToUInt32(by_read, ref ui32_metadata_size);

                        // move the read file_stream to place it in the next metadata block
                        file_stream.Seek(ui32_metadata_size, SeekOrigin.Current);
                        break;

                }//switch

            }//while

            return ec_ret_val;

        }//loadDRP_ROMPACKv01


        /*******************************************************************************
        * @brief   Loads the specified melody drive pack data file in DRP format into the
        * drivePackData object.
        * @param[in] str_load_file  with the name of the dirve pack data file to load into
        * the object.
        * @return >=0 file has been succesfully loaded into the object, <0 an error 
        * occurred 
        *******************************************************************************/
        public ErrCode loadDRPFile(string str_load_file){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream = null;
            BinaryReader file_binary_reader;
            ASCIIEncoding ascii = new ASCIIEncoding();
            uint ui_read_bytes = 0;
            byte[] by_read = null;
            string str_aux = "";


            if (!File.Exists(str_load_file)){

                ec_ret_val = cErrCodes.ERR_FILE_NOT_EXIST;

            }else{

                file_stream = new FileStream(str_load_file, FileMode.Open);
                file_binary_reader = new BinaryReader(file_stream);

                if (file_binary_reader == null){
                    ec_ret_val = cErrCodes.ERR_FILE_OPENING;
                }

                // read and check file format verstion tag
                ui_read_bytes = 0;
                if (ec_ret_val.i_code >= 0)
                {

                    // get the file format and version tag
                    file_stream.Seek(0, SeekOrigin.Begin);
                    by_read = file_binary_reader.ReadBytes(11);
                    ui_read_bytes = ui_read_bytes + 11;// 10:chars + 1:\0
                    str_aux = str_aux + ascii.GetString(by_read);

                    // process the file format and version tag 
                    if (str_aux == "ROMPACKv00\0"){

                        ec_ret_val = loadDRP_ROMPACKv00(ref file_stream, ref file_binary_reader, ref ui_read_bytes);

                    }else if (str_aux == "ROMPACKv01\0"){

                        ec_ret_val = loadDRP_ROMPACKv01(ref file_stream, ref file_binary_reader, ref ui_read_bytes);

                    }else{

                        ec_ret_val = cErrCodes.ERR_FILE_INVALID_VERSION;

                    }//if

                }//if

                file_stream.Close();

            }//if

            return ec_ret_val;

        }//loadDRPFile


        /*******************************************************************************
        * @brief   Loads the specified melody drive pack data file in binary raw format 
        * into the drivePackData object.
        * @param[in] str_load_file  with the name of the dirve pack data file to load into the 
        * object
        * @return >=0 file has been succesfully loaded into the object, <0 an error 
        * occurred 
        *******************************************************************************/
        public ErrCode loadBINFile(string str_load_file){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream = null;
            BinaryReader file_binary_reader;
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] bytes_read = null;
            System.UInt32 ui32_file_size = 0;
            int i_aux = 0;
            byte by_read = 0;
            byte by_aux = 0;


            if (!File.Exists(str_load_file)){

                ec_ret_val = cErrCodes.ERR_FILE_NOT_EXIST;

            }else{

                file_stream = new FileStream(str_load_file, FileMode.Open);
                file_binary_reader = new BinaryReader(file_stream);

                if (file_binary_reader == null)
                {
                    ec_ret_val = cErrCodes.ERR_FILE_OPENING;
                }

                // get and check the size of the specified binary file
                if (ec_ret_val.i_code >= 0){

                    ui32_file_size = (System.UInt32)file_stream.Length;
                    if (ui32_file_size > ROM_MAX_SIZE)
                    {
                        ec_ret_val = cErrCodes.ERR_FILE_SPECIFIED;
                    }//if

                }//if

                // load specified binart file
                if (ec_ret_val.i_code >= 0){

                    this.strTitle = "Enter RO-XXX Title of the cart here.";
                    this.strSongInfo = "Enter the list of the cart songs here:\r\n[1]-Son 1 name\r\n[2]-Son 2 name\r\n[3]-Son 3 name\r\n[4]-Son 3 name\r\n";

                    // read all the bytes of the specified binary file and store them into the songs object in memory
                    bytes_read = file_binary_reader.ReadBytes((int)ui32_file_size);

                    // in .bin files the nibbles are stored in reverse order than in drivePACK memory and the DRP files, so they must be reversed
                    for (i_aux = 0; i_aux< bytes_read.Count(); i_aux++){
                        by_read = bytes_read[i_aux];
                        by_aux = (byte)((0x0F & by_read)<<4);
                        by_read = (byte)(0x0F & (bytes_read[i_aux] >> 4));
                        by_read = (byte)(by_read | by_aux);
                        bytes_read[i_aux] = by_read;
                    }

                    // re initialize the DynamicByteProvider with the array of bytes read from the file
                    dynbyprMemoryBytes = new DynamicByteProvider(bytes_read);

                }//if

                file_stream.Close();

            }//if

            return ec_ret_val;

        }//loadBINFile


        /*******************************************************************************
        * @brief Saves to the specified binary DRP file (DRP) the ROM data that is currently  
        * stored in the ROM object in memory. The difference betweeb DRP files and BIN
        * files is that DRP contain extra information like the rom name, song titles...
        * while BIN files only containt the raw content of the original ROM PACK cart.
        * @param[in] str_save_file with the name of the file to save the ROM content in
        * DRP file format.
        * @return >=0 file has been succesfully saved, <0 an error occurred  
        *******************************************************************************/
        public ErrCode saveDRPFile(string str_save_file){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream = null;
            BinaryWriter file_binary_writer;
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] by_aux = null;
            byte[] by_title = null;
            byte[] by_title_length = new byte[4];
            byte[] by_song_info = null;
            byte[] by_song_info_length = new byte[4];


            file_stream = new FileStream(str_save_file, FileMode.Create);
            file_binary_writer = new BinaryWriter(file_stream);

            if (file_binary_writer == null){
                ec_ret_val = cErrCodes.ERR_FILE_CREATING;
            }//if

            if (ec_ret_val.i_code >= 0){

                // save version information
                by_aux = Encoding.ASCII.GetBytes("ROMPACKv01\0");
                file_binary_writer.Write(by_aux);

                // calculate the data_offset according to the length of the title a song information metadata fields
                // 11 bytes of the ROM PACK version ( 10 for version + 1 for the '\0')
                // 01 byte type of the title METADATA field: title information
                // ROM PACK METADATA: 0x01 FILE_METADATA_TITLE
                // 04 bytes of the length of the ROM PACK METADATA field ( it does not include the type and length bytes, only the data bytes )
                // n bytes of the title ROM PACK METADATA field
                // ROM PACK METADATA: 0x02 FILE_METADATA_SONGS_INFO
                // 01 byte type of the title METADATA field: songs information
                // 04 bytes of the length of the ROM PACK METADATA field ( it does not include the type and length bytes, only the data bytes )
                // n bytes of the songs information ROM PACK METADATA field
                // ROM PACK METADATA: 0x03 FILE_METADATA_SONGS_ROM
                // 01 byte type of the title METADATA field: songs information
                // 04 bytes of the length of the ROM PACK METADATA field ( it does not include the type and length bytes, only the data bytes )
                // n bytes of the songs information ROM PACK METADATA field
                // ...

                // save the TITLE METADATA ( FILE_METADATA_TITLE field ):
                by_title = Encoding.ASCII.GetBytes(this.strTitle + '\0');
                // 1 byte - METADATA type - type = FILE_METADATA_TITLE
                file_binary_writer.Write((byte)FILE_METADATA_TITLE);
                // 4 byte - METADATA field length
                by_aux = new byte[4];// re initialize the auxiliary array to 4 bytes
                AuxFuncs.convertUInt32To4Bytes((uint)by_title.Length, by_aux);
                file_binary_writer.Write(by_aux);
                // n byte - METADATA field content
                file_binary_writer.Write(by_title);

                // save the SONGS_INFO METADATA ( FILE_METADATA_SONGS_INFO field ):
                by_song_info = Encoding.ASCII.GetBytes(this.strSongInfo + '\0');
                // 1 byte - METADATA type - type = FILE_METADATA_TITLE
                file_binary_writer.Write((byte)FILE_METADATA_SONGS_INFO);
                // 4 byte - METADATA field length
                by_aux = new byte[4];// re initialize the auxiliary array to 4 bytes
                AuxFuncs.convertUInt32To4Bytes((uint)by_song_info.Length, by_aux);
                file_binary_writer.Write(by_aux);
                // n byte - METADATA field content
                file_binary_writer.Write(by_song_info);

                // save the ROM METADATA ( FILE_METADATA_SONGS_ROM field ):
                // 1 byte - METADATA type - type = FILE_METADATA_SONGS_ROM
                file_binary_writer.Write((byte)FILE_METADATA_SONGS_ROM);
                // 4 byte - METADATA field length
                by_aux = new byte[4];// re initialize the auxiliary array to 4 bytes
                AuxFuncs.convertUInt32To4Bytes((uint)this.dynbyprMemoryBytes.Length, by_aux);
                file_binary_writer.Write(by_aux);
                // n byte - METADATA ROM DATA field content
                by_aux = this.dynbyprMemoryBytes.Bytes.ToArray();
                file_binary_writer.Write(by_aux);

            }//if

            file_stream.Close();

            return ec_ret_val;

        }//saveDRPFile


        /*******************************************************************************
        * @brief Saves to the specified binary raw file (BIN) the ROM data that is currently  
        * stored in the ROM object in memory. The difference betweeb DRP files and BIN
        * files is that DRP contain extra information like the rom name, song titles...
        * while BIN files only containt the raw content of the original ROM PACK cart.
        * @param[in] str_save_file with the name of the file to save the ROM content in
        * raw binary format.
        * @return >=0 file has been succesfully saved, <0 an error occurred  
        *******************************************************************************/
        public ErrCode saveBINFile(string str_save_file){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream = null;
            BinaryWriter file_binary_writer;
            byte[] bytes_read = null;
            int i_aux = 0;
            byte by_aux = 0;
            byte by_read = 0;


            file_stream = new FileStream(str_save_file, FileMode.Create);
            file_binary_writer = new BinaryWriter(file_stream);

            if (file_binary_writer == null){
                ec_ret_val = cErrCodes.ERR_FILE_CREATING;
            }//if

            if (ec_ret_val.i_code >= 0){

                // write drive pack data to binary file
                bytes_read = this.dynbyprMemoryBytes.Bytes.ToArray();

                // in .bin files the nibbles are stored in reverse order than in drivePACK memory and the DRP files, so they must be reversed
                for (i_aux = 0; i_aux < bytes_read.Count(); i_aux++)
                {
                    by_read = bytes_read[i_aux];
                    by_aux = (byte)((0x0F & by_read) << 4);
                    by_read = (byte)(0x0F & (bytes_read[i_aux] >> 4));
                    by_read = (byte)(by_read | by_aux);
                    bytes_read[i_aux] = by_read;
                }

                file_binary_writer.Write(bytes_read);

            }//if

            file_stream.Close();

            return ec_ret_val;

        }//saveBINFile


        /*******************************************************************************
        * @brief Processes the content of the themes code structures that contains the 
        * information   f different songs and their cahnnels (M1,M2, chords ...) and 
        * generates the corresponding ROM object content that can be run on the original 
        * keyboard.
        * @param[in] str_save_file with the name of the file to save the ROM content in
        * raw binary format.
        * @return  >=0 the ROMPACK content has been succesfull generated , <0 an error
        * occurred  
        *******************************************************************************/
        public ErrCode buildROMPACK(){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            UInt32 ui32Aux = 0x0000;
            byte byAux = 0x00;
            byte[] arrByROM = null;
            byte[] arrByROMFinal = null;
            byte[] arr4ByAux = new byte[4];
            int iArrIdx = 0;
            int iSongsAddrBaseIdx =0; // address where start the address of the different songs
            int iM1ChanStartAddr = 0; // processed song M1 channel start address
            int iM1ChanEndAddress = 0;
            int iM2ChanStartAddr = 0; // processed song M2 channel start address
            int iM2ChanEndAddress = 0;
            int iChordChanStartAddr = 0; // processed song Chord channel start address
            int iChordChanEndAddress = 0;
            int iSongEndAddr = 0; // address where the song ends and next song starts
            uint[] uiThemesStartAddresses = null;
            uint[] uiThemesEndAddresses = null;
            uint uiHeadVacantAreaAddress = 0;
            uint uiNumThemes = 0;
            int iAux = 0;
            int iThemeIdxAux = 0;
            uint uiStartAddressAux = 0;
            int uiTotalInstructions = 0;
            string strAux = "";
            MChannelCodeEntry melodyCodeEntryAux = null;
            ChordChannelCodeEntry chordCodeEntryAux = null;
            byte[] byArrWorkDataHeader = { 0xA5, 0x00, 0x00, 0xD0, 0x2F, 0x00 };
            byte[] byArrEndOfROMPACK = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x23, 0x83, 0x93, 0x06, 0x47, 0x83, 0xAB, 0x02, 0x63, 0x27, 0x4B, 0x27, 0x47, 0x93, 0x2B, 0x83, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00 };

            // create the ROM PACK byte array with the maximum allowed size 
            arrByROM = new byte[ROM_MAX_SIZE]; 

            // create the arrays used to store the start and end address of the themes and then calculate their values
            uiNumThemes = (uint) themes.liThemesCode.Count;
            uiThemesStartAddresses = new uint[uiNumThemes];
            uiThemesEndAddresses = new uint[uiNumThemes];

            // place an informative message for the user in the logs
            strAux = "Detected " + uiNumThemes + " themes to build into the ROM PACK.";
            statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);

            // calculate the corresponding start and end address for each Theme in the list of Themes
            iThemeIdxAux = 0;
            uiStartAddressAux = I_WORK_DATA_SIZE + I_NUM_PIECES_SIZE + I_VACANT_START_ADDRESS_SIZE + (uiNumThemes * I_THEME_START_ADDRESS_SIZE) + I_VACANT_START_ADDRESS_SIZE;
            while ( (iThemeIdxAux < uiNumThemes) && (ec_ret_val.i_code >= 0) ) {

                // store the start address of current processed theme.
                uiThemesStartAddresses[iThemeIdxAux] = uiStartAddressAux;
                
                // calculate START following theme start adress by adding the size of the current theme 
                uiStartAddressAux = uiStartAddressAux + I_PIECE_HEADER_SIZE;
                uiStartAddressAux = uiStartAddressAux + (uint)(themes.liThemesCode[iThemeIdxAux].liM1CodeInstr.Count * I_MELODY_CODE_ENTRY_SIZE);
                uiStartAddressAux = uiStartAddressAux + (uint)(themes.liThemesCode[iThemeIdxAux].liM2CodeInstr.Count * I_MELODY_CODE_ENTRY_SIZE);
                uiStartAddressAux = uiStartAddressAux + (uint)(themes.liThemesCode[iThemeIdxAux].liChordCodeInstr.Count * I_CHORDS_CODE_ENTRY_SIZE);
                
                // store the END address of current processed theme. It ends where the following theme begins.
                uiThemesEndAddresses[iThemeIdxAux] = uiStartAddressAux-1;

                // place an informative message for the user in the logs. The "+1" in (uiThemesEndAddresses[iThemeIdxAux] * 2)+1)  is because when the byte address is converted to nibble addresses the last nibble of the byte is the second nibble of the last byte, not the first nibble.
                strAux = "Theme " + iThemeIdxAux + " address range is 0x" + (uiThemesStartAddresses[iThemeIdxAux] * 2).ToString("X6") + " - 0x" + ((uiThemesEndAddresses[iThemeIdxAux] * 2)+1).ToString("X6") + ".";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);

                iThemeIdxAux++;

            }//while

            // calculate the head vacant address: it corresponds to the following address to the last address of the last theme.
            uiHeadVacantAreaAddress = uiStartAddressAux;// in the ROM, addresses are stored at nibble level so convert the byte addresses to nibble address 

            // place an informative message for the user in the logs
            strAux = "Head vacant area address at 0x" + (uiHeadVacantAreaAddress*2).ToString("X6") + ".";
            statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);

            // start builiding the themes into the ROM PACK content: start by the WORK data header:

            //-------------------
            //00  00:0101:0xA
            //    01:1010:0x5
            //01  02:0000:0x0
            //    03:0000:0x0
            //02  04:0000:0x0    WORK
            //    05:0000:0x0    DATA 
            //03  06:0000:0x0
            //    07:1101:0xD
            //04  08:1111:0xF
            //    09:1011:0xB
            //05  0A:0000:0x0
            //    0B:0000:0x0
            //-------------------
            for (iAux=0;iAux<byArrWorkDataHeader.Length; iAux++) {
                arrByROM[iArrIdx] = byArrWorkDataHeader[iAux];
                iArrIdx++;
            }

            //-------------------
            //06  0C:----:
            //    0D:----:     NUMBER OF PIECES
            //07  0E:----:    4 Nibbles (2bytes)
            //    0F:----:
            //-------------------
            ui32Aux = (UInt32)uiNumThemes;
            AuxFuncs.convertUInt32To4BytesReversed(ui32Aux, arr4ByAux);
            arrByROM[iArrIdx] = arr4ByAux[0]; iArrIdx++;// 6
            arrByROM[iArrIdx] = arr4ByAux[1]; iArrIdx++;// 7

            //-------------------
            //08 10:----: 
            //   11:----:
            //09 12:----:   HEAD ADDRESS OF VACANT ADDRESS 
            //   13:----:       6 Nibbles (3bytes)
            //0A 14:----:
            //   15:----:
            //-------------------
            ui32Aux = (UInt32)(uiHeadVacantAreaAddress*2);
            AuxFuncs.convertUInt32To4BytesReversed(ui32Aux, arr4ByAux);
            arrByROM[iArrIdx] = arr4ByAux[0]; iArrIdx++;
            arrByROM[iArrIdx] = arr4ByAux[1]; iArrIdx++;
            arrByROM[iArrIdx] = arr4ByAux[2]; iArrIdx++;

            // build the HEAD ADDRESS OF MUSICAL PIECE DATA

            //----------- 
            //?? ??:----:
            //   ??:----:
            //?? ??:----:     SONG n start address
            //   ??:----:   6 nibbles (3bytes) to specify 
            //?? ??:----:    the start of each of the N songs
            //   ??:----:
            for (iThemeIdxAux =0; iThemeIdxAux < uiNumThemes; iThemeIdxAux++) {
                ui32Aux = (UInt32)(uiThemesStartAddresses[iThemeIdxAux]*2);
                AuxFuncs.convertUInt32To4BytesReversed(ui32Aux, arr4ByAux);
                arrByROM[iArrIdx] = arr4ByAux[0]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[1]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[2]; iArrIdx++;
            }
            iSongsAddrBaseIdx = iArrIdx; // get the index of the songs addresses base index

            //-------------------
            //?? ??:----: 
            //   ??:----:
            //?? ??:----:   HEAD ADDRESS OF VACANT ADDRESS 
            //   ??:----:       6 Nibbles (3bytes)
            //?? ??:----:
            //   ??:----:
            //-------------------
            ui32Aux = (UInt32)(uiHeadVacantAreaAddress * 2);
            AuxFuncs.convertUInt32To4BytesReversed(ui32Aux, arr4ByAux);
            arrByROM[iArrIdx] = arr4ByAux[0]; iArrIdx++;
            arrByROM[iArrIdx] = arr4ByAux[1]; iArrIdx++;
            arrByROM[iArrIdx] = arr4ByAux[2]; iArrIdx++;
          
            // secuentially build and store the information of each Theme in the Themes structure
            iThemeIdxAux = 0;
            while ((iThemeIdxAux < uiNumThemes) && (ec_ret_val.i_code >= 0)) {

                // place an informative message for the user in the logs
                strAux = "Building and storing theme " + iThemeIdxAux + " content ...";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);

                // calculate the start address of the channels in th theme before storing them into the ROM PACK byte array
                iM1ChanStartAddr = iArrIdx + I_PIECE_HEADER_SIZE; // processed song M1 channel start address
                iM2ChanStartAddr = iM1ChanStartAddr + (themes.liThemesCode[iThemeIdxAux].liM1CodeInstr.Count* I_MELODY_CODE_ENTRY_SIZE); // processed song M2 channel start address
                iChordChanStartAddr = iM2ChanStartAddr + (themes.liThemesCode[iThemeIdxAux].liM2CodeInstr.Count * I_MELODY_CODE_ENTRY_SIZE); // processed song Chord channel start address
                iSongEndAddr = iChordChanStartAddr + (themes.liThemesCode[iThemeIdxAux].liChordCodeInstr.Count * I_CHORDS_CODE_ENTRY_SIZE); // address where the theme/song ends and next song starts

                // calculate the END address of each theme's channel by using the other read addresses
                iM1ChanEndAddress = iM2ChanStartAddr - 1;
                iM2ChanEndAddress = iChordChanStartAddr - 1;
                iChordChanEndAddress = iSongEndAddr - 1;

                // place an informative message for the user in the logs. The "+1" in (iXXXChanEnAddress * 2)+1)  is because when the byte address is converted to nibble addresses the last nibble of the byte is the second nibble of the last byte, not the first nibble.
                strAux = "Theme " + iThemeIdxAux + " M1 channel address range is 0x" + (iM1ChanStartAddr * 2).ToString("X6") + " - 0x" + ((iM1ChanEndAddress * 2)+1).ToString("X6") + ".";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);
                strAux = "Theme " + iThemeIdxAux + " M2 channel address range is 0x" + (iM2ChanStartAddr * 2).ToString("X6") + " - 0x" + ((iM2ChanEndAddress * 2)+1).ToString("X6") + ".";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);
                strAux = "Theme " + iThemeIdxAux + " Chords channel address range is 0x" + (iChordChanStartAddr * 2).ToString("X6") + " - 0x" + ((iChordChanEndAddress * 2)+1).ToString("X6") + ".";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);
                strAux = "Theme " + iThemeIdxAux + " Theme End Mark address is 0x" + (iSongEndAddr * 2).ToString("X6") + ".";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);

                // store in the ROM PACK byte array the start address of the channels in the theme 
                //-------------------
                //??  ??:0000:0x0
                //    ??:0000:0x0
                //??  ??:----:
                //    ??:----:
                //??  ??:----:   MELODY
                //    ??:----:   DATA HEAD
                //??  ??:----:   ADDRESS
                //    ??:----:
                //??  ??:0000:0x0
                //    ??:0001:0x1
                //??  ??:----:
                //    ??:----:   OBBLIGATO
                //??  ??:----:   DATA HEAD
                //    ??:----:   ADDRESS
                //??  ??:----:
                //    ??:----:
                //??  ??:0010:0x2
                //    ??:0000:0x0
                //??  ??:----:
                //    ??:----:   CHORD
                //??  ??:----:   DATA HEAD
                //    ??:----:   ADDRESS
                //??  ??:----:
                //    ??:----:
                //??  ??:1111:0xF
                //    ??:1111:0xF
                //??  ??:----:
                //    ??:----:   PIECE END /
                //??  ??:----:   FOLLOWING PIECE
                //    ??:----:   ADDRESS
                //??  ??:----:
                //    ??:----:
                //-------------------
                arrByROM[iArrIdx] = 0x00; iArrIdx++;
                ui32Aux = (UInt32)(iM1ChanStartAddr * 2);
                AuxFuncs.convertUInt32To4BytesReversed(ui32Aux, arr4ByAux);
                arrByROM[iArrIdx] = arr4ByAux[0]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[1]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[2]; iArrIdx++;

                arrByROM[iArrIdx] = 0x01; iArrIdx++;
                ui32Aux = (UInt32)(iM2ChanStartAddr * 2);
                AuxFuncs.convertUInt32To4BytesReversed(ui32Aux, arr4ByAux);
                arrByROM[iArrIdx] = arr4ByAux[0]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[1]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[2]; iArrIdx++;

                arrByROM[iArrIdx] = 0x20; iArrIdx++;
                ui32Aux = (UInt32)(iChordChanStartAddr * 2);
                AuxFuncs.convertUInt32To4BytesReversed(ui32Aux, arr4ByAux);
                arrByROM[iArrIdx] = arr4ByAux[0]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[1]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[2]; iArrIdx++;

                arrByROM[iArrIdx] = 0xFF; iArrIdx++;
                ui32Aux = (UInt32)(iSongEndAddr * 2);
                AuxFuncs.convertUInt32To4BytesReversed(ui32Aux, arr4ByAux);
                arrByROM[iArrIdx] = arr4ByAux[0]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[1]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[2]; iArrIdx++;

                // store in the ROM PACK byte array the Code instructions of current themes M1 channel
                uiTotalInstructions = themes.liThemesCode[iThemeIdxAux].liM1CodeInstr.Count;
                iAux = 0;
                while (iAux < uiTotalInstructions) {

                    melodyCodeEntryAux = themes.liThemesCode[iThemeIdxAux].liM1CodeInstr[iAux];
                    arrByROM[iArrIdx] = melodyCodeEntryAux.by0; iArrIdx++;
                    arrByROM[iArrIdx] = melodyCodeEntryAux.by1; iArrIdx++;
                    arrByROM[iArrIdx] = melodyCodeEntryAux.by2; iArrIdx++;

                    iAux++;

                }//while

                // place an informative message for the user in the logs
                strAux = "Theme " + iThemeIdxAux + " M1 channel added " + uiTotalInstructions + " commands (" + (uiTotalInstructions * I_MELODY_CODE_ENTRY_SIZE).ToString() + "bytes).";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);

                // store in the ROM PACK byte array the Code instructions of current themes M2 channel
                uiTotalInstructions = themes.liThemesCode[iThemeIdxAux].liM2CodeInstr.Count;
                iAux = 0;
                while (iAux < uiTotalInstructions) {

                    melodyCodeEntryAux = themes.liThemesCode[iThemeIdxAux].liM2CodeInstr[iAux];
                    arrByROM[iArrIdx] = melodyCodeEntryAux.by0; iArrIdx++;
                    arrByROM[iArrIdx] = melodyCodeEntryAux.by1; iArrIdx++;
                    arrByROM[iArrIdx] = melodyCodeEntryAux.by2; iArrIdx++;

                    iAux++;

                }//while

                // place an informative message for the user in the logs
                strAux = "Theme " + iThemeIdxAux + " M2 channel added " + uiTotalInstructions + " commands (" + (uiTotalInstructions * I_MELODY_CODE_ENTRY_SIZE).ToString() + "bytes).";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);

                // store in the ROM PACK byte array the Code instructions of current themes Chords channel
                uiTotalInstructions = themes.liThemesCode[iThemeIdxAux].liChordCodeInstr.Count;
                iAux = 0;
                while (iAux < uiTotalInstructions) {

                    chordCodeEntryAux = themes.liThemesCode[iThemeIdxAux].liChordCodeInstr[iAux];
                    arrByROM[iArrIdx] = chordCodeEntryAux.by0; iArrIdx++;
                    arrByROM[iArrIdx] = chordCodeEntryAux.by1; iArrIdx++;

                    iAux++;

                }//while

                // place an informative message for the user in the logs
                strAux = "Theme " + iThemeIdxAux + " Chords channel added " + uiTotalInstructions + " commands (" + (uiTotalInstructions * I_CHORDS_CODE_ENTRY_SIZE).ToString() + "bytes).";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);

                iThemeIdxAux++;

            }

            // in order that the ROM PACK cartridge end string finishes at a 0xFF multiple address: pad the ROM 
            // content with "0x00"s until an address multiple of 0x...E0 is reachedand then add the 32 bytes of 
            // the ROMPACK cartridge end string.
            while ((iArrIdx % 0xE0) != 0) {
                arrByROM[iArrIdx] = 0x00; iArrIdx++;
            }
            for (iAux = 0; iAux < byArrEndOfROMPACK.Length; iAux++) {
                arrByROM[iArrIdx] = byArrEndOfROMPACK[iAux];
                iArrIdx++;
            }

            // once all the ROM PACK binary content has been generated move it into an array
            // with the size of the generated ROM PACK binary
            arrByROMFinal = new byte[iArrIdx];
            Buffer.BlockCopy(arrByROM, 0, arrByROMFinal, 0, iArrIdx);

            // re initialize the DynamicByteProvider with the bytes resulting of building the themes in the ROM PACK
            dynbyprMemoryBytes = new DynamicByteProvider(arrByROMFinal);

            return ec_ret_val;

        }//buildROMPACK


        /*******************************************************************************
        * @brief  Procedure that processes all the bytes of the array that contains the ROM 
        * data by decoding and organizing all that bytes into the theme channles (M1,M2,chords..)
        * structures.
        * @return  >=0 the code of the notes and chords in the ROM PACK cartridge has been
        * succesfully changed.
        *******************************************************************************/
        public ErrCode decodeROMPACKtoSongThemes() {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            byte[] arrByROM = null;
            byte[] arr4ByAux = new byte[4];
            byte[] arr3ByAux = new byte[3];
            uint uiNumThemes = 0;
            uint uiBeginHeadAddrVacantArea = 0;
            uint uiEndHeadAddrVacantArea = 0;
            uint[] uiThemesStartAddresses = null;
            uint[] uiThemesEndAddresses = null;
            uint uiThemeStartAddress = 0;
            uint uiThemeEndAddress = 0;
            uint uiM1ChanStartAddress = 0;
            uint uiM1ChanEndAddress = 0;
            uint uiM2ChanStartAddress = 0;
            uint uiM2ChanEndAddress = 0;
            uint uiChordChanStartAddress = 0;
            uint uiChordChanEndAddress = 0;
            uint uiEndThemeMarkAddress = 0;
            uint uiAuxAddress = 0;
            uint uiThemeIdxAux = 0;
            ThemeCode themeCodeAux = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            ChordChannelCodeEntry chordCodeEntryAux = null;
            string strAux = "";
            uint uiInstrCtr = 0;

            if (dynbyprMemoryBytes.Length == 0) {
                ec_ret_val = cErrCodes.ERR_DECODING_EMPTY_ROM;
            }

            if (ec_ret_val.i_code >= 0) {

                // first of all delete all themes in the list of Themes to make space for the
                // themes in the ROM PACK to decode
                themes.deleteAllThemes();

                // get the content of the OM PACK dynamic bytes provider as an array of bytes
                arrByROM = this.dynbyprMemoryBytes.Bytes.ToArray();

                // start processing the content ROM PACK array of bytes:

                // get the value in the field with the number of pieces (themes) in the ROM ( 2 bytes = 4 nibbles )
                arr4ByAux[0] = arrByROM[I_OFFSET_NUM_PIECES];
                arr4ByAux[1] = arrByROM[I_OFFSET_NUM_PIECES+1];
                arr4ByAux[2] = 0;
                arr4ByAux[3] = 0;
                AuxFuncs.convert4BytesReversedToUInt32(arr4ByAux, ref uiNumThemes);

                // place an informative message for the user in the loDecoded ROM PACK contains 0xgs
                strAux = "Detected " + uiNumThemes + " themes in ROM PACK.";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                // create the array to store the start and end addresses of the themes
                uiThemesStartAddresses = new uint[uiNumThemes];
                uiThemesEndAddresses = new uint[uiNumThemes];

                // get the different addresses in the ROM PACK header:
                //  beggining head address of vacant address
                //  theme 0 address
                //  theme 1 address
                //  ....
                //  theme n-1 address
                //  end head address of vacant addres

                // get beggining head vacant address ( 3 bytes = 6 nibbles )
                arr4ByAux[0] = arrByROM[I_OFFSET_BEGIN_VACANT_ADDRESS + 0];
                arr4ByAux[1] = arrByROM[I_OFFSET_BEGIN_VACANT_ADDRESS + 1];
                arr4ByAux[2] = arrByROM[I_OFFSET_BEGIN_VACANT_ADDRESS + 2];
                arr4ByAux[3] = 0;
                AuxFuncs.convert4BytesReversedToUInt32(arr4ByAux, ref uiBeginHeadAddrVacantArea);
                uiBeginHeadAddrVacantArea = uiBeginHeadAddrVacantArea / 2;//divide by 2 to convert from nibble address to byte address

                // place an informative message for the user in the logs
                strAux = "Begin vacant address at 0x" + (uiBeginHeadAddrVacantArea*2).ToString("X6") + ".";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                // get the different themes start address in the ROM PACK header 
                uiThemeIdxAux = 0;
                while ( (uiThemeIdxAux < uiNumThemes) && (ec_ret_val.i_code>=0)) {

                    // get the start addres of th theme ( 3 bytes = 6 nibbles )
                    arr4ByAux[0] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * uiThemeIdxAux) +0];
                    arr4ByAux[1] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * uiThemeIdxAux) +1];
                    arr4ByAux[2] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * uiThemeIdxAux) +2];
                    arr4ByAux[3] = 0;
                    AuxFuncs.convert4BytesReversedToUInt32(arr4ByAux, ref uiThemesStartAddresses[uiThemeIdxAux]); 
                    uiThemesStartAddresses[uiThemeIdxAux] = uiThemesStartAddresses[uiThemeIdxAux] / 2;//divide by 2 to convert from nibble address to byte address

                    // place an informative message for the user in the logs
                    strAux = "Theme " + uiThemeIdxAux + " start address at 0x" + (uiThemesStartAddresses[uiThemeIdxAux] * 2).ToString("X6") + ".";
                    statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                    uiThemeIdxAux++;

                }//while

                // get end head vacant address ( 3 bytes = 6 nibbles )
                arr4ByAux[0] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * uiThemeIdxAux) + 0];
                arr4ByAux[1] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * uiThemeIdxAux) + 1];
                arr4ByAux[2] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * uiThemeIdxAux) + 2];
                arr4ByAux[3] = 0;
                AuxFuncs.convert4BytesReversedToUInt32(arr4ByAux, ref uiEndHeadAddrVacantArea);               
                uiEndHeadAddrVacantArea = uiEndHeadAddrVacantArea / 2;//divide by 2 to convert from nibble address to byte address

                // place an informative message for the user in the logs
                strAux = "End vacant address at 0x" + (uiEndHeadAddrVacantArea * 2).ToString("X6") + ".";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                // calculate the END address of each theme by using other read addresses
                uiThemeIdxAux = 0;
                while ((uiThemeIdxAux < uiNumThemes) && (ec_ret_val.i_code >= 0)) {

                    if (uiThemeIdxAux >= (uiNumThemes - 1)) {
                        // if the processed theme is the last one then the all the addresses of that 
                        // theme must be between the theme start address and the vacant area.
                        uiThemesEndAddresses[uiThemeIdxAux] = uiBeginHeadAddrVacantArea;
                    } else {
                        // if the processed theme is NOT the last one, then the addresses of that 
                        // theme must be between the theme start address and the start address of
                        // the following theme
                        uiThemesEndAddresses[uiThemeIdxAux] = uiThemesStartAddresses[uiThemeIdxAux + 1] - 1;
                    }

                    // place an informative message for the user in the logs. The "+1" in ((uiThemesEndAddresses[uiThemeIdxAux] * 2)+1) is because when the byte address is converted to nibble addresses the last nibble of the byte is the second nibble of the last byte, not the first nibble.
                    strAux = "Theme " + uiThemeIdxAux + " address range is 0x" + (uiThemesStartAddresses[uiThemeIdxAux] * 2).ToString("X6")+ " - 0x" + ((uiThemesEndAddresses[uiThemeIdxAux] * 2)+1).ToString("X6") + ".";
                    statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                    uiThemeIdxAux++;

                }//while

                // use the addresses previously read from the ROM PACK header to process each piece (theme) in the ROM PACK,
                // creating a new Theme object with its different channels and its code for each piece in the ROM 
                uiThemeIdxAux = 0;
                while ((uiThemeIdxAux < uiNumThemes) && (ec_ret_val.i_code >= 0)) {

                    // place an informative message for the user in the logs
                    strAux = "Decoding theme " + uiThemeIdxAux + " content ...";
                    statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                    // add the new theme in the Themes data structure
                    themes.AddNewTheme();

                    // get all the information of the current processed theme, 
                    uiThemeStartAddress = uiThemesStartAddresses[uiThemeIdxAux];
                    uiThemeEndAddress = uiThemesEndAddresses[uiThemeIdxAux];

                    // get current theme M1 channel ( Melody channel ) address
                    if (ec_ret_val.i_code >= 0) {

                        if (arrByROM[uiThemeStartAddress + I_OFFSET_M1_START_ADDRESS] != 0x00) {

                            ec_ret_val = cErrCodes.ERR_DECODING_INVALID_M1_MARK;

                        } else {

                            // get the start address of M1 channel
                            arr4ByAux[0] = arrByROM[uiThemeStartAddress + I_OFFSET_M1_START_ADDRESS + 1];
                            arr4ByAux[1] = arrByROM[uiThemeStartAddress + I_OFFSET_M1_START_ADDRESS + 2];
                            arr4ByAux[2] = arrByROM[uiThemeStartAddress + I_OFFSET_M1_START_ADDRESS + 3];
                            arr4ByAux[3] = 0;
                            AuxFuncs.convert4BytesReversedToUInt32(arr4ByAux, ref uiM1ChanStartAddress);

                            uiM1ChanStartAddress = uiM1ChanStartAddress / 2;// divide by 2 to convert from nibble address to byte address

                        }

                    }//if (ec_ret_val.i_code >= 0)

                    // get current theme M2 channel ( Obligato channel ) address
                    if (ec_ret_val.i_code >= 0) {

                        if (arrByROM[uiThemeStartAddress + I_OFFSET_M2_START_ADDRESS] != 0x01) {

                            ec_ret_val = cErrCodes.ERR_DECODING_INVALID_M2_MARK;

                        } else {

                            // get the start address of the M2 channel
                            arr4ByAux[0] = arrByROM[uiThemeStartAddress + I_OFFSET_M2_START_ADDRESS + 1];
                            arr4ByAux[1] = arrByROM[uiThemeStartAddress + I_OFFSET_M2_START_ADDRESS + 2];
                            arr4ByAux[2] = arrByROM[uiThemeStartAddress + I_OFFSET_M2_START_ADDRESS + 3];
                            arr4ByAux[3] = 0;
                            AuxFuncs.convert4BytesReversedToUInt32(arr4ByAux, ref uiM2ChanStartAddress);

                            uiM2ChanStartAddress = uiM2ChanStartAddress / 2;// divide by 2 to convert from nibble address to byte address

                        }

                    }//if (ec_ret_val.i_code >= 0)

                    // get current theme Chords channel ( Obligato channel ) address
                    if (ec_ret_val.i_code >= 0) {

                        if (arrByROM[uiThemeStartAddress + I_OFFSET_CHORD_START_ADDRESS] != 0x20) {

                            ec_ret_val = cErrCodes.ERR_DECODING_INVALID_CHORD_MARK;

                        } else {

                            // get the start address of the Chord channel
                            arr4ByAux[0] = arrByROM[uiThemeStartAddress + I_OFFSET_CHORD_START_ADDRESS + 1];
                            arr4ByAux[1] = arrByROM[uiThemeStartAddress + I_OFFSET_CHORD_START_ADDRESS + 2];
                            arr4ByAux[2] = arrByROM[uiThemeStartAddress + I_OFFSET_CHORD_START_ADDRESS + 3];
                            arr4ByAux[3] = 0;
                            AuxFuncs.convert4BytesReversedToUInt32(arr4ByAux, ref uiChordChanStartAddress);

                            uiChordChanStartAddress = uiChordChanStartAddress / 2;// divide by 2 to convert from nibble address to byte address

                        }

                    }//if (ec_ret_val.i_code >= 0)

                    // IMPORTANT: this is not documented in CASIO patent, but after the 4 bytes of the 0x00+MELODY DATA HEAD ADDRESS, 
                    // the 4 bytes of the 0x01+OBLIGATTO_DATA_HEAD_ADDRESS, and the 4 bytes of the 0x20+CHORD_DATA_HEAD_ADDRESS, there
                    // are other 4 bytes that correspond to the 0xFF+THEME_END_ADDRESS
                    if (ec_ret_val.i_code >= 0) {

                        if (arrByROM[uiThemeStartAddress + I_OFFSET_THEME_END_ADDRESS] == 0xFF) {

                            // get the start address of the Chord channel
                            arr4ByAux[0] = arrByROM[uiThemeStartAddress + I_OFFSET_THEME_END_ADDRESS + 1];
                            arr4ByAux[1] = arrByROM[uiThemeStartAddress + I_OFFSET_THEME_END_ADDRESS + 2];
                            arr4ByAux[2] = arrByROM[uiThemeStartAddress + I_OFFSET_THEME_END_ADDRESS + 3];
                            arr4ByAux[3] = 0;
                            AuxFuncs.convert4BytesReversedToUInt32(arr4ByAux, ref uiEndThemeMarkAddress);

                            uiEndThemeMarkAddress = uiEndThemeMarkAddress / 2;// divide by 2 to convert from nibble address to byte address

                        }

                    }//if (ec_ret_val.i_code >= 0)

                    // check that the obtained channel addresses are in a valid range 
                    if (ec_ret_val.i_code >= 0) {

                        // confirm that the M1 channel start address is between the address range of the current Theme
                        if ( (uiM1ChanStartAddress<uiThemeStartAddress) || (uiM1ChanStartAddress>uiThemeEndAddress) ) {
                            ec_ret_val = cErrCodes.ERR_DECODING_INVALID_M1_ADDRESS;
                        }
                        // confirm that the M2 channel start address is between the address range of the current Theme and is not lower than the M1 channel start address
                        if ( (uiM2ChanStartAddress<uiThemeStartAddress) || (uiM2ChanStartAddress<uiM1ChanStartAddress) || (uiM1ChanStartAddress > uiThemeEndAddress)) {
                            ec_ret_val = cErrCodes.ERR_DECODING_INVALID_M2_ADDRESS;
                        }
                        // confirm that the Chords channel start address is between the address range of the current Theme and is not lower than the M2 channel start address
                        if ( (uiChordChanStartAddress < uiThemeStartAddress) || (uiChordChanStartAddress < uiM2ChanStartAddress) || (uiChordChanStartAddress > uiThemeEndAddress)) {
                            ec_ret_val = cErrCodes.ERR_DECODING_INVALID_CHORD_ADDRESS;
                        }

                    }//if (ec_ret_val.i_code >= 0)

                    if (ec_ret_val.i_code >= 0) {

                        // calculate the END address of each theme's channel by using the other read addresses
                        uiM1ChanEndAddress = uiM2ChanStartAddress - 1;
                        uiM2ChanEndAddress = uiChordChanStartAddress - 1;
                        uiChordChanEndAddress = uiThemeEndAddress - 1;

                        // place an informative message for the user in the logs.The "+1" in ((uiXXChanEndAddress * 2)+1) is because when the byte address is converted to nibble addresses the last nibble of the byte is the second nibble of the last byte, not the first nibble.
                        strAux = "Theme " + uiThemeIdxAux + " M1 channel address range is 0x" + (uiM1ChanStartAddress * 2).ToString("X6") + " - 0x" + ((uiM1ChanEndAddress * 2)+1).ToString("X6") + ".";
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);
                        strAux = "Theme " + uiThemeIdxAux + " M2 channel address range is 0x" + (uiM2ChanStartAddress * 2).ToString("X6") + " - 0x" + ((uiM2ChanEndAddress * 2) + 1).ToString("X6") + ".";
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);
                        strAux = "Theme " + uiThemeIdxAux + " Chords channel address range is 0x" + (uiChordChanStartAddress * 2).ToString("X6") + " - 0x" + ((uiChordChanEndAddress * 2) + 1).ToString("X6") + ".";
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);
                        if (uiEndThemeMarkAddress != 0) {
                            strAux = "Theme " + uiThemeIdxAux + " Theme End Mark address is 0x" + (uiEndThemeMarkAddress * 2).ToString("X6") + ".";
                        } else{
                            strAux = "Theme " + uiThemeIdxAux + " has no Theme End Mark.";
                        }
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                        // store the code entries to their respective channels in the theme 
                        themeCodeAux = themes.GetCurrTheme();

                        // store the melody 1 code entries into the theme's M1 channel
                        uiAuxAddress = uiM1ChanStartAddress;
                        uiInstrCtr = 0;
                        while (uiAuxAddress<=uiM1ChanEndAddress) {

                            melodyCodeEntryAux = new MChannelCodeEntry(arrByROM[uiAuxAddress + 0], arrByROM[uiAuxAddress + 1], arrByROM[uiAuxAddress + 2]);

                            // add the code of the read M1 entry into the themes M1 (melody) channel instructions list
                            themeCodeAux.liM1CodeInstr.Add(melodyCodeEntryAux);

                            uiAuxAddress = uiAuxAddress + I_MELODY_CODE_ENTRY_SIZE;
                            uiInstrCtr++;
                        }

                        // place an informative message for the user in the logs
                        strAux = "Theme " + uiThemeIdxAux + " M1 channel added " + uiInstrCtr + " commands (" + (uiInstrCtr* I_MELODY_CODE_ENTRY_SIZE).ToString() + "bytes)."; 
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                        // store the melody 2 ( obligato ) code entries into the theme's M2 channel
                        uiAuxAddress = uiM2ChanStartAddress;
                        uiInstrCtr = 0;
                        while (uiAuxAddress <= uiM2ChanEndAddress) {

                            melodyCodeEntryAux = new MChannelCodeEntry(arrByROM[uiAuxAddress + 0], arrByROM[uiAuxAddress + 1], arrByROM[uiAuxAddress + 2]);

                            // add the code of the read M2 entry into the themes M2 (obligato) channel instructions list
                            themeCodeAux.liM2CodeInstr.Add(melodyCodeEntryAux);

                            uiAuxAddress = uiAuxAddress + I_MELODY_CODE_ENTRY_SIZE;
                            uiInstrCtr++;
                        }

                        // place an informative message for the user in the logs
                        strAux = "Theme " + uiThemeIdxAux + " M2 channel added " + uiInstrCtr + " commands (" + (uiInstrCtr * I_MELODY_CODE_ENTRY_SIZE).ToString() + "bytes).";
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                        // store the CHords code entries into the theme's Chords channel
                        uiAuxAddress = uiChordChanStartAddress;
                        uiInstrCtr = 0;
                        while (uiAuxAddress <= uiChordChanEndAddress) {

                            chordCodeEntryAux = new ChordChannelCodeEntry(arrByROM[uiAuxAddress + 0], arrByROM[uiAuxAddress + 1]);

                            // add the code of the read Chord entry into the themes Chords channel instructions list
                            themeCodeAux.liChordCodeInstr.Add(chordCodeEntryAux);

                            uiAuxAddress = uiAuxAddress + I_CHORDS_CODE_ENTRY_SIZE;
                            uiInstrCtr++;
                        }

                        // place an informative message for the user in the logs
                        strAux = "Theme " + uiThemeIdxAux + " Chords channel added " + uiInstrCtr + " commands (" + (uiInstrCtr * I_CHORDS_CODE_ENTRY_SIZE).ToString() + "bytes).";
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                    }//if (ec_ret_val.i_code >= 0)

                    // process followint theme
                    uiThemeIdxAux++;

                }//while ((uiThemeIdxAux < uiNumThemes) && (ec_ret_val.i_code >= 0)) 

            }//if (ec_ret_val.i_code >= 0)

            return ec_ret_val;

        }//decodeROMPACKtoSongThemes

    }//cDrivePackData

}//drivePackEd