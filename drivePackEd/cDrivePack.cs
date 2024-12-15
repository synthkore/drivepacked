using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Be.Windows.Forms;
using System.Linq;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using static drivePackEd.MChannelCodeEntry;
using static drivePackEd.ChordChannelCodeEntry;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;

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

namespace drivePackEd{

    /*******************************************************************************
    *  @brief defines the object with all the current themes information: that is the   
    *  object that contains a list with all the themes, and each ThemeCode contains 
    *  the code of the M1, M2 and chords channels.
    *******************************************************************************/
    public class Themes{

        // constants
        public const int MAX_THEMES_ROM = 32; //the maximum number of themes that can be stored in a ROM
        public const int MAX_INSTRUCTIONS_CHANNEL = 1024; //the maximum number of instructions that can be stored in a channel of a theme

        public BindingList<ThemeCode> liThemesCode = null; // list with all the themes

        public List<int> liSelectedThemesDGridviewRows;// keeps the index of all the selected theme rows in the Themes DatagidView
                                                   // 
        public int iCurrThemeIdx;// current selected Theme index

        public string strROMTitle = "";
        public string strROMInfo = "";

        /*******************************************************************************
        * @brief Creates a copy of the received themes object structure into the received
        * themes destination structure.
        * @param[out] destination with the copy of the object.
        *******************************************************************************/
        public static void CopyThemes(Themes source, ref Themes destination) {
            int i_aux = 0;

            // create and copy the destination themes structure
            destination = new Themes();
            foreach (ThemeCode themcode in source.liThemesCode) {

                destination.AddNew();
                i_aux = destination.liThemesCode.Count;
                ThemeCode.CopyTheme(themcode, (ThemeCode)destination.liThemesCode[i_aux-1] );
            
            }//foreach

            // create and copy the list used to store the Indexes of the Rows seleceted on the Themes dataGridView 
            destination.liSelectedThemesDGridviewRows = new List<int>();
            foreach (int iIdx in source.liSelectedThemesDGridviewRows) {
                destination.liSelectedThemesDGridviewRows.Add(iIdx);
            }
            
            // copy the index of the current Edited Theme and other Theme information
            destination.iCurrThemeIdx = source.iCurrThemeIdx;

            destination.strROMInfo = source.strROMInfo;
            destination.strROMTitle = source.strROMTitle;
            

        }//CloneThemesFrom

        /*******************************************************************************
        * @brief Adds a new theme to the list of themes
        * @return the ErrCode with the result or error of the operation.
        *******************************************************************************/
        public ErrCode AddNew() {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR;
            ThemeCode newSong = new ThemeCode();

            if (liThemesCode.Count < MAX_THEMES_ROM) {

                liThemesCode.Add(newSong);
                // set the current Theme index pointing to the added Theme
                iCurrThemeIdx = liThemesCode.Count - 1;

            } else {
                erCodeRetVal = cErrCodes.ERR_EDITION_ADD_NEW_THEME;
            }

            return erCodeRetVal;

        }//AddNew

        /*******************************************************************************
        * @brief Inserts a new Theme at the specified iIdx position of the Themes list
        * @param[in] iIdx with the position in the Themes list at which the new Theme will 
        * be inserted.
        * @return the ErrCode with the result or error of the operation.
        ********************************************************************************/
        public ErrCode AddNewAt(int iIdx) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR;
            ThemeCode newTheme = null;

            // if the received iIdx is out of range, then add the new Theme at the end of the list
            if ((iIdx < 0) || (iIdx >= liThemesCode.Count)) iIdx = liThemesCode.Count;

            if (iIdx < Themes.MAX_INSTRUCTIONS_CHANNEL) {

                // create a new theme, initialize it with default values and the add it into the themes list
                newTheme = new ThemeCode();
                newTheme.Idx = iIdx;
                newTheme.Title = "Enter theme title here.";
                liThemesCode.Insert(iIdx, newTheme);

                // set the current theme pointing to the inserted theme
                iCurrThemeIdx = iIdx;

            } else {
                erCodeRetVal = cErrCodes.ERR_EDITION_ADD_NEW_THEME;  
            }

            return erCodeRetVal;

        }//AddNewAt

        /*******************************************************************************
        * @brief Deletes the theme at the specified iIdx position in the Theme list
        * @param[in] iIdx with the position in the themes list of the theme to delete. From
        * 0 to nTemes-1.
        * @return the ErrCode with the result or error of the operation.
        ********************************************************************************/
        public ErrCode DeleteAt(int iIdx) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR;


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

        }//DeleteAt

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

            liThemesCode = new BindingList<ThemeCode>(); // list with all the themes, each theme conta
            
            liSelectedThemesDGridviewRows = new List<int>();
            
            strROMInfo = "";
            strROMTitle = "";
            liThemesCode.Clear();
            iCurrThemeIdx = -1;

        }//Themes

        /*******************************************************************************
        *  @brief Clears all the ROM and themes information.
        *******************************************************************************/
        public void Clear() {
               
            strROMInfo = "";
            strROMTitle = "";
            liThemesCode.Clear();
            iCurrThemeIdx = -1;

        }//Clear

        /*******************************************************************************
        *  @brief Deletes the content of all the channels of all the themes in the themes
        *  object.
        *******************************************************************************/
        public void deleteAllThemesInstructions() {
            int iAux = 0;

            for (iAux=0;iAux<liThemesCode.Count;iAux++){

                liThemesCode[iAux].liM1CodeInstr.Clear();
                liThemesCode[iAux].liM2CodeInstr.Clear();
                liThemesCode[iAux].liChordCodeInstr.Clear();
                liThemesCode[iAux].iM1IEditednstrIdx = -1;
                liThemesCode[iAux].iM2EditedInstrIdx = -1;
                liThemesCode[iAux].iChEditedInstrIdx = -1;

            }//for

        }//deleteAllThemesInstructions

        /*******************************************************************************
        * @brief Deletes all the instructions in all the channels of the specified theme
        * but keeps other information like the Title.
        * @param[in] iThemeIdx the index of the theme whose instructions we want to delete.
        * @return >=0 if the instructions of the specified have been deleted, <0
        * if something failed when deleting the specified theme instructions.
        *******************************************************************************/
        public int deleteThemeInstructions(int iThemeIdx) {
            int retVal = 0;

            if ((iThemeIdx >= 0) || (iThemeIdx < liThemesCode.Count())) {

                liThemesCode[iThemeIdx].liM1CodeInstr.Clear();
                liThemesCode[iThemeIdx].liM2CodeInstr.Clear();
                liThemesCode[iThemeIdx].liChordCodeInstr.Clear();
                liThemesCode[iThemeIdx].iM1IEditednstrIdx = -1;
                liThemesCode[iThemeIdx].iM2EditedInstrIdx = -1;
                liThemesCode[iThemeIdx].iChEditedInstrIdx = -1;
            
            } else {
                retVal = -1;
            }//if

            return retVal;

        }//deleteThemeInstructions

        /*******************************************************************************
        * @brief  Updates the Idx field of all themes in the list in order they all have
        * consecutive values. This function is usefull to correct index values after 
        * having adder or deleted new themes in the list.
        *******************************************************************************/
        public void regenerateIdxs() {
            int iAux = 0;

            for (iAux = 0; iAux < liThemesCode.Count; iAux++) {

                liThemesCode[iAux].Idx = iAux;

            }// for

        }//regenerateIdxs

    }// class themes

    /*******************************************************************************
    * @brief with all the information related to a single Theme. It contains the 3 
    * lists  with the instructions for each theme's channel. A theme is composed of
    * 3 lists of code, and  each list of code implements the sequence of notes or 
    * chords on each channel.
    *******************************************************************************/
    public class ThemeCode{

        int iIdx;  // JBR 2024-05-03 El campo Idx es bastante absurdo y está puesto solo para realizar el binding con el datagridview, habría que mirar la forma de quitarlo
        public int Idx {
            get {
                return iIdx;
            }
            set {
                iIdx = value;
            }
        }//idx

        string strTitle;
        public string Title {
            get {
                return strTitle;
            }
            set {
                strTitle = value;
            }
        }//strTitle   

        public BindingList<MChannelCodeEntry> liM1CodeInstr; // list with all the code entries of the Melody 1 channel
        public BindingList<MChannelCodeEntry> liM2CodeInstr; // list with all the code entries of the Melody 2 channel
        public BindingList<ChordChannelCodeEntry> liChordCodeInstr; // list with all the  code entries of the Melody 2 channel
        public List<int> liSelectedM1DGridviewRows;// keeps the index of all the selected instruction rows in the M1 Channel datagidView 
        public List<int> liSelectedM2DGridviewRows;// keeps the index of all the selected instruction rows in the M2 Channel datagidView 
        public List<int> liSelectedChordDGridviewRows;// keeps the index of all the selected instruction rows in the Chords Channel datagidView 
        public int iM1IEditednstrIdx;// current edited Melody 1 channel edited instruction index
        public int iM2EditedInstrIdx;// current edited Melody 2 channel edited instruction index
        public int iChEditedInstrIdx;// current edited chord channel edited instruction index

        /*******************************************************************************
        * @brief Default constructor
        *******************************************************************************/
        public ThemeCode() {
            MChannelCodeEntry mPrAux = null;

            liM1CodeInstr = new BindingList<MChannelCodeEntry>();
            liM2CodeInstr = new BindingList<MChannelCodeEntry>();
            liChordCodeInstr = new BindingList<ChordChannelCodeEntry>();


            liSelectedM1DGridviewRows = new List<int>();
            liSelectedM2DGridviewRows = new List<int>();
            liSelectedChordDGridviewRows = new List<int>();

            iM1IEditednstrIdx = -1;
            iM2EditedInstrIdx = -1;
            iChEditedInstrIdx = -1;

        }//ThemeCode

        /*******************************************************************************
        * @brief Copies the source theme structure into the destination structure
        *
        * @param[in] themeSource
        *******************************************************************************/
        public static void CopyTheme(ThemeCode themeSource, ThemeCode themeDestination) {
            MChannelCodeEntry melodyCodeEntryAux = null;
            ChordChannelCodeEntry chordCodeEntryAux = null;

            if (themeSource != null) {

                // create thes lists to store the entries of the theme different channels
                themeDestination.liM1CodeInstr = new BindingList<MChannelCodeEntry>();
                themeDestination.liM2CodeInstr = new BindingList<MChannelCodeEntry>();
                themeDestination.liChordCodeInstr = new BindingList<ChordChannelCodeEntry>();

                // copy each M1 channel instructions from the source theme to the destination theme, as the 
                // contain objects they need to be "deep copied" with the foreach instruction.
                foreach (MChannelCodeEntry melodyCodeEntrySource in themeSource.liM1CodeInstr) {
                    
                    // create one instruction fore each instruction in the M1 instructions list 
                    melodyCodeEntryAux = new MChannelCodeEntry();
                    // initialize each created instruction with the same content of each instruction in the source theme
                    melodyCodeEntryAux.Idx = melodyCodeEntrySource.Idx;
                    melodyCodeEntryAux.By0 = melodyCodeEntrySource.By0;
                    melodyCodeEntryAux.By1 = melodyCodeEntrySource.By1;
                    melodyCodeEntryAux.By2 = melodyCodeEntrySource.By2;
                    melodyCodeEntryAux.strDescr = melodyCodeEntrySource.strDescr;
                    // add the created instruction into the M1 channel instructions lit
                    themeDestination.liM1CodeInstr.Add(melodyCodeEntryAux);

                }//foreach

                // copy each M2 channel instructions from the source theme to the destination theme, as the 
                // contain objects they need to be "deep copied" with the foreach instruction.
                foreach (MChannelCodeEntry melodyCodeEntrySource in themeSource.liM2CodeInstr) {

                    // create one instruction fore each instruction in the M2 instructions list 
                    melodyCodeEntryAux = new MChannelCodeEntry();
                    // initialize each created instruction with the same content of each instruction in the source theme
                    melodyCodeEntryAux.Idx = melodyCodeEntrySource.Idx;
                    melodyCodeEntryAux.By0 = melodyCodeEntrySource.By0;
                    melodyCodeEntryAux.By1 = melodyCodeEntrySource.By1;
                    melodyCodeEntryAux.By2 = melodyCodeEntrySource.By2;
                    melodyCodeEntryAux.strDescr = melodyCodeEntrySource.strDescr;
                    // add the created instruction into the M2 channel instructions lit
                    themeDestination.liM2CodeInstr.Add(melodyCodeEntryAux);

                }//foreach

                // copy each Chords channel instructions from the source theme to the destination theme, as the 
                // contain objects they need to be "deep copied" with the foreach instruction.
                foreach (ChordChannelCodeEntry chordCodeEntrySource in themeSource.liChordCodeInstr) {

                    // create one instruction fore each instruction in the Chords instructions list 
                    chordCodeEntryAux = new ChordChannelCodeEntry();
                    // initialize each created instruction with the same content of each instruction in the source theme
                    chordCodeEntryAux.Idx = chordCodeEntrySource.Idx;
                    chordCodeEntryAux.By0 = chordCodeEntrySource.By0;
                    chordCodeEntryAux.By1 = chordCodeEntrySource.By1;
                    chordCodeEntryAux.strDescr = chordCodeEntrySource.strDescr;
                    // add the created instruction into the Chords channel instructions lit
                    themeDestination.liChordCodeInstr.Add(chordCodeEntryAux);

                }//foreach

                // create and copy the list used to store the Indexes of the Rows seleceted on the M1 
                // channel instructions dataGridView 
                themeDestination.liSelectedM1DGridviewRows = new List<int>();
                foreach (int iIdx in themeSource.liSelectedM1DGridviewRows) {
                    themeDestination.liSelectedM1DGridviewRows.Add(iIdx);
                }

                // create and copy the list used to store the Indexes of the Rows seleceted on the M2 
                // channel instructions dataGridView 
                themeDestination.liSelectedM2DGridviewRows = new List<int>();
                foreach (int iIdx in themeSource.liSelectedM2DGridviewRows) {
                    themeDestination.liSelectedM2DGridviewRows.Add(iIdx);
                }

                // create and copy the list used to store the Indexes of the Rows seleceted on the Chords 
                // channel instructions dataGridView 
                themeDestination.liSelectedChordDGridviewRows = new List<int>();
                foreach (int iIdx in themeSource.liSelectedChordDGridviewRows) {
                    themeDestination.liSelectedChordDGridviewRows.Add(iIdx);
                }

                // copy the index of the current Edited instructions
                themeDestination.iM1IEditednstrIdx = themeSource.iM1IEditednstrIdx;
                themeDestination.iM2EditedInstrIdx = themeSource.iM2EditedInstrIdx;
                themeDestination.iChEditedInstrIdx = themeSource.iChEditedInstrIdx;

                themeDestination.Idx = themeSource.Idx;
                themeDestination.Title = themeSource.Title;

            }//if (themeSource != null) {

        }//CopyTheme

    }// class ThemeCode

    // groups all the fields of a Melody entry ( for Melody1 or Melody2 )
    public class MChannelCodeEntry {

        public enum t_Command {
            TIMBRE_INSTRUMENT,
            EFFECT,
            REST_DURATION,
            NOTE,
            REPEAT,
            TIE,
            KEY,
            TIME,
            BAR,
            END,
            DURATIONx2
        }

        public enum t_On_Off{
            ON,
            OFF
        }

        public enum t_Instrument {
            PIANO,
            HARPISCHORD,
            ORGAN,
            VIOLIN,
            FLUTE,
            CLARINET,
            TRUMPET,
            CELESTA
        }

        public enum t_Effect {
            SUST0, SUST1, SUST2, SUST3, SUST4, SUST5, SUST6, SUST7, VIBRATO, DELAY_VIBRATO
        }

        public enum t_Notes {
            C3, Csh3, D3, Dsh3, E3, F3, Fsh3, G3, Gsh3, A3, Ash3, B3,
            C4, Csh4, D4, Dsh4, E4, F4, Fsh4, G4, Gsh4, A4, Ash4, B4,
            C5, Csh5, D5, Dsh5, E5, F5, Fsh5, G5, Gsh5, A5, Ash5, B5,
            C6
        }

        public enum t_RepeatMark {
            START, END, X1, X2, X3, X4, X5, X6, X7, X8
        }

        public enum t_Time {
            _16x16, _2x2, _2x4, _3x2, _3x4, _3x8, _4x16, _4x4, _6x4, _6x8, _12x8
        }

        int idx;
        public int Idx {
            get {
                return idx;
            }
            set {
              idx = value;
            }
        }//idx

        byte by0;
        public string By0 {
            get {
                // value is internally stored as a byte but it is delivered as the hex string representation
                return "0x"+by0.ToString("x2");
            }
            set {
                // value is received as the hex string representation of the value it is stored as a byte
                if (AuxFuncs.convertStringToByte(value,ref by0) < 0) {
                    by0 = 0;
                }
            }           
        }//by0

        byte by1;
        public string By1 {
            get {
                // value is internally stored as a byte but it is delivered as the hex string representation
                return "0x" + by1.ToString("x2");
            }
            set {
                // value is received as the hex string representation of the value it is stored as a byte
                if (AuxFuncs.convertStringToByte(value, ref by1) < 0) {
                    by0 = 0;
                }
            }
        }

        byte by2;
        public string By2 {
            get {
                // value is internally stored as a byte but it is delivered as the hex string representation
                return "0x" + by2.ToString("x2");
            }
            set {
                // value is received as the hex string representation of the value it is stored as a byte
                if (AuxFuncs.convertStringToByte(value, ref by2) < 0) {
                    by0 = 0;
                }
            }
        }

        public string strDescr { get; set; }

        /*******************************************************************************
         * @brief Converts the received t_Command variable to a string equivalent
         * @param[in] t_Command the t_Command variable to convert to a string eqquivalent.
         * @return the string conversion of the received t_Command variable.
         *******************************************************************************/
        public static string tCommandToString(t_Command tCommandToConvert) {
            string str_aux = "";

            switch (tCommandToConvert) {
                case t_Command.TIMBRE_INSTRUMENT: str_aux = "instrument"; break;
                case t_Command.EFFECT: str_aux = "effect"; break;
                case t_Command.REST_DURATION: str_aux = "rest duration"; break;
                case t_Command.NOTE: str_aux = "note"; break;
                case t_Command.REPEAT: str_aux = "repeat"; break;
                case t_Command.TIE: str_aux = "tie"; break;
                case t_Command.KEY: str_aux = "key"; break;
                case t_Command.TIME: str_aux = "time"; break;
                case t_Command.BAR: str_aux = "bar"; break;
                case t_Command.END: str_aux = "end"; break;
                case t_Command.DURATIONx2: str_aux = "durationx2"; break;
            }//switch

            return str_aux;

        }//tCommandToString

        /*******************************************************************************
        * @brief Converts the received string to the equivalent t_Command variable.
        * @param[in] tCommandToConvert with the string to convert to a t_Command 
        * variable.
        * @return the t_Command resulting of converting the received string.
        *******************************************************************************/
        public static t_Command  strToCommand(string strTCommandToConvert) {
            t_Command tTCommandAux = new t_Command();

            strTCommandToConvert = strTCommandToConvert.Trim();
            strTCommandToConvert = strTCommandToConvert.ToLower();
            switch (strTCommandToConvert) {
                case "instrument": tTCommandAux = t_Command.TIMBRE_INSTRUMENT; break;
                case "effect":tTCommandAux = t_Command.EFFECT; break;
                case "rest duration":tTCommandAux = t_Command.REST_DURATION; break;
                case "note":tTCommandAux = t_Command.NOTE; break;
                case "repeat":tTCommandAux = t_Command.REPEAT; break;
                case "tie":tTCommandAux = t_Command.TIE; break;
                case "key":tTCommandAux = t_Command.KEY; break;
                case "time":tTCommandAux = t_Command.TIME; break;
                case "bar":tTCommandAux = t_Command.BAR; break;
                case "end": tTCommandAux = t_Command.END; break;
                case "durationx2": tTCommandAux = t_Command.DURATIONx2; break;
            }//switch

            return tTCommandAux;

        }//tOnOffToString

        /*******************************************************************************
         * @brief Converts the received t_On_Off variable to a string equivalent
         * @param[in] tOnOffToConvert the t_On_Off variable to convert to a string eqquivalent.
         * @return the string conversion of the received t_On_Off variable.
         *******************************************************************************/
        public static string tOnOffToString(t_On_Off tOnOffToConvert) {
            string str_aux = "";

            switch (tOnOffToConvert) {
                case t_On_Off.ON: str_aux = "on"; break;
                case t_On_Off.OFF: str_aux = "off"; break;
            }//switch

            return str_aux;

        }//tOnOffToString

        /*******************************************************************************
        * @brief Converts the received string to the equivalent t_On_Off variable.
        * @param[in] strOnOffToConvert with the string to convert to a t_On_Off 
        * variable.
        * @return the t_On_Off resulting of converting the received string.
        *******************************************************************************/
        public static t_On_Off strToTOnOff(string strOnOffToConvert) {
            t_On_Off tOnOffAux = new t_On_Off();

            strOnOffToConvert = strOnOffToConvert.Trim();
            strOnOffToConvert = strOnOffToConvert.ToLower();
            switch (strOnOffToConvert) {
                case "on": tOnOffAux = t_On_Off.ON; break;
                case "off": tOnOffAux = t_On_Off.OFF; break;
            }//switch

            return tOnOffAux;

        }//strToTOnOff

        /*******************************************************************************
        * @brief Converts the received t_Notes variable to a string equivalent
        * @param[in] tNoteToConvert the tNote variable to convert to a string.
        * @return the string conversion of the received t_Notes variable.
        *******************************************************************************/
        public static string tNotesToString(t_Notes tNoteToConvert) {
            string str_aux = "";

            switch (tNoteToConvert) {
                // Octave 3:
                case t_Notes.C3:   str_aux = "c3";  break;
                case t_Notes.Csh3: str_aux = "c#3"; break;
                case t_Notes.D3:   str_aux = "d3";  break;
                case t_Notes.Dsh3: str_aux = "d#3"; break;
                case t_Notes.E3:   str_aux = "e3";  break;
                case t_Notes.F3:   str_aux = "f3";  break;
                case t_Notes.Fsh3: str_aux = "f#3"; break;
                case t_Notes.G3:   str_aux = "g3";  break;
                case t_Notes.Gsh3: str_aux = "g#3"; break;
                case t_Notes.A3:   str_aux = "a3";  break;
                case t_Notes.Ash3: str_aux = "a#3"; break;
                case t_Notes.B3:   str_aux = "b3";  break;
                // Octave 4:
                case t_Notes.C4:   str_aux = "c4";  break;
                case t_Notes.Csh4: str_aux = "c#4"; break;
                case t_Notes.D4:   str_aux = "d4";  break;
                case t_Notes.Dsh4: str_aux = "d#4"; break;
                case t_Notes.E4:   str_aux = "e4";  break;
                case t_Notes.F4:   str_aux = "f4";  break;
                case t_Notes.Fsh4: str_aux = "f#4"; break;
                case t_Notes.G4:   str_aux = "g4";  break;
                case t_Notes.Gsh4: str_aux = "g#4"; break;
                case t_Notes.A4:   str_aux = "a4";  break;
                case t_Notes.Ash4: str_aux = "a#4"; break;
                case t_Notes.B4:   str_aux = "b4";  break;
                // Octave 5:
                case t_Notes.C5:   str_aux = "c5";  break;
                case t_Notes.Csh5: str_aux = "c#5"; break;
                case t_Notes.D5:   str_aux = "d5";  break;
                case t_Notes.Dsh5: str_aux = "d#5"; break;
                case t_Notes.E5:   str_aux = "e5";  break;
                case t_Notes.F5:   str_aux = "f5";  break;
                case t_Notes.Fsh5: str_aux = "f#5"; break;
                case t_Notes.G5:   str_aux = "g5";  break;
                case t_Notes.Gsh5: str_aux = "g#5"; break;
                case t_Notes.A5:   str_aux = "a5";  break;
                case t_Notes.Ash5: str_aux = "a#5"; break;
                case t_Notes.B5:   str_aux = "b5";  break;
                case t_Notes.C6:   str_aux = "c6"; break;
            }//switch

            return str_aux;

        }//tNotesToString

        /*******************************************************************************
        * @brief Converts the received string to the equivalent t_Note variable.
        * @param[in] strNoteToConvert with the string to convert to a tNote variable.
        * @return the t_Note resulting of converting the received string.
        *******************************************************************************/
        public static t_Notes strToTNote(string strNoteToConvert) {
            t_Notes tNoteAux = new t_Notes();

            strNoteToConvert = strNoteToConvert.Trim();
            strNoteToConvert = strNoteToConvert.ToLower();
            switch (strNoteToConvert) {

                // octave 3:
                case "c3":  tNoteAux = t_Notes.C3; break;
                case "c#3": tNoteAux = t_Notes.Csh3; break;
                case "d3":  tNoteAux = t_Notes.D3; break;
                case "d#3": tNoteAux = t_Notes.Dsh3; break;
                case "e3":  tNoteAux = t_Notes.E3; break;
                case "f3":  tNoteAux = t_Notes.F3; break;
                case "f#3": tNoteAux = t_Notes.Fsh3; break;
                case "g3":  tNoteAux = t_Notes.G3; break;
                case "g#3": tNoteAux = t_Notes.Gsh3; break;
                case "a3":  tNoteAux = t_Notes.A3; break;
                case "a#3": tNoteAux = t_Notes.Ash3; break;
                case "b3":  tNoteAux = t_Notes.B3; break;
                // octave 4:                
                case "c4":  tNoteAux = t_Notes.C4; break;
                case "c#4": tNoteAux = t_Notes.Csh4; break;
                case "d4":  tNoteAux = t_Notes.D4; break;
                case "d#4": tNoteAux = t_Notes.Dsh4; break;
                case "e4":  tNoteAux = t_Notes.E4; break;
                case "f4":  tNoteAux = t_Notes.F4; break;
                case "f#4": tNoteAux = t_Notes.Fsh4; break;
                case "g4":  tNoteAux = t_Notes.G4; break;
                case "g#4": tNoteAux = t_Notes.Gsh4; break;
                case "a4":  tNoteAux = t_Notes.A4; break;
                case "a#4": tNoteAux = t_Notes.Ash4; break;
                case "b4":  tNoteAux = t_Notes.B4; break;
                // octave 5:               
                case "c5":  tNoteAux = t_Notes.C5; break;
                case "c#5": tNoteAux = t_Notes.Csh5; break;
                case "d5":  tNoteAux = t_Notes.D5; break;
                case "d#5": tNoteAux = t_Notes.Dsh5; break;
                case "e5":  tNoteAux = t_Notes.E5; break;
                case "f5":  tNoteAux = t_Notes.F5; break;
                case "f#5": tNoteAux = t_Notes.Fsh5; break;
                case "g5":  tNoteAux = t_Notes.G5; break;
                case "g#5": tNoteAux = t_Notes.Gsh5; break;
                case "a5":  tNoteAux = t_Notes.A5; break;
                case "a#5": tNoteAux = t_Notes.Ash5; break;
                case "b5":  tNoteAux = t_Notes.B5; break;
                // octave 6:
                case "c6":  tNoteAux = t_Notes.C6; break;
            }//switch

            return tNoteAux;

        }//strToTNote

        /*******************************************************************************
         * @brief Converts the received t_Instrument variable to a string equivalent
         * @param[in] tInstrToConvert the t_Instrument variable to convert to the string
         * equivalent.
         * @return the string conversion of the received t_Instrument variable.
         *******************************************************************************/
        public static string tInstrumentToString(t_Instrument tInstrToConvert) {
            string str_aux = "";

            switch (tInstrToConvert) {
                case t_Instrument.PIANO: str_aux = "piano"; break;
                case t_Instrument.HARPISCHORD: str_aux = "harpischord"; break;
                case t_Instrument.ORGAN: str_aux = "organ"; break;
                case t_Instrument.VIOLIN: str_aux = "violin"; break;
                case t_Instrument.FLUTE: str_aux = "flute"; break;
                case t_Instrument.CLARINET: str_aux = "clarinet"; break;
                case t_Instrument.TRUMPET: str_aux = "trumpet"; break;
                case t_Instrument.CELESTA: str_aux = "celesta"; break;
            }//switch

            return str_aux;

        }//tInstrumentToString

        /*******************************************************************************
        * @brief Converts the received string to the equivalent t_Instrument variable.
        * @param[in] strInstrumentToConvert with the string to convert to a t_Instrument 
        * equivalent.
        * @return the t_Instrument resulting of converting the received string.
        *******************************************************************************/
        public static t_Instrument strToInstrument(string strInstrumentToConvert) {
            t_Instrument tInstrAux = new t_Instrument();

            strInstrumentToConvert = strInstrumentToConvert.Trim();
            strInstrumentToConvert = strInstrumentToConvert.ToLower();
            switch (strInstrumentToConvert) {
                case "piano": tInstrAux = t_Instrument.PIANO; break;
                case "harpischord": tInstrAux = t_Instrument.HARPISCHORD; break;
                case "organ": tInstrAux = t_Instrument.ORGAN; break;
                case "violin": tInstrAux = t_Instrument.VIOLIN; break;
                case "flute": tInstrAux = t_Instrument.FLUTE; break;
                case "clarinet": tInstrAux = t_Instrument.CLARINET; break;
                case "trumpet": tInstrAux = t_Instrument.TRUMPET; break;
                case "celesta": tInstrAux = t_Instrument.CELESTA; break;
            }//switch

            return tInstrAux;

        }//strToInstrument

        /*******************************************************************************
         * @brief Converts the received t_Effect variable to a string equivalent
         * @param[in] tEffectToConvert the tNote variable to convert to a string.
         * @return the string conversion of the received t_Effect variable.
         *******************************************************************************/
        public static string tEffectToString(t_Effect tEffectToConvert) {
            string str_aux = "";

            switch (tEffectToConvert) {               
                case t_Effect.SUST0: str_aux = "sust0"; break;
                case t_Effect.SUST1: str_aux = "sust1"; break;
                case t_Effect.SUST2: str_aux = "sust2"; break;
                case t_Effect.SUST3: str_aux = "sust3"; break;
                case t_Effect.SUST4: str_aux = "sust4"; break;
                case t_Effect.SUST5: str_aux = "sust5"; break;
                case t_Effect.SUST6: str_aux = "sust6"; break;
                case t_Effect.SUST7: str_aux = "sust7"; break;
                case t_Effect.VIBRATO: str_aux = "vibrato"; break;
                case t_Effect.DELAY_VIBRATO: str_aux = "delay vibrato"; break;
            }//switch

            return str_aux;

        }//t_EffectToString

        /*******************************************************************************
        * @brief Converts the received string to the equivalent t_Effect variable.
        * @param[in] strEffectToConvert with the string to convert to a t_Effect variable.
        * @return the t_Effect resulting of converting the received string.
        *******************************************************************************/
        public static t_Effect strToTEffect(string strEffectToConvert) {
            t_Effect tEffectAux = new t_Effect();

            strEffectToConvert = strEffectToConvert.Trim();
            strEffectToConvert = strEffectToConvert.ToLower();
            switch (strEffectToConvert) {
                case "sust0": tEffectAux = t_Effect.SUST0; break;
                case "sust1": tEffectAux = t_Effect.SUST1; break;
                case "sust2": tEffectAux = t_Effect.SUST2; break;
                case "sust3": tEffectAux = t_Effect.SUST3; break;
                case "sust4": tEffectAux = t_Effect.SUST4; break;
                case "sust5": tEffectAux = t_Effect.SUST5; break;
                case "sust6": tEffectAux = t_Effect.SUST6; break;
                case "sust7": tEffectAux = t_Effect.SUST7; break;
                case "vibrato": tEffectAux = t_Effect.VIBRATO; break;
                case "delay vibrato": tEffectAux = t_Effect.DELAY_VIBRATO; break;
            }//switch

            return tEffectAux;

        }//strToTEffect

        /*******************************************************************************
         * @brief Converts the received t_RepeatMark variable to a string equivalent
         * @param[in] tRMarkToConvert the t_RepeatMark variable to convert to a string.
         * @return the string conversion of the received t_RepeatMark variable.
         *******************************************************************************/
        public static string tRepeatMarkToString(t_RepeatMark tRMarkToConvert) {
            string str_aux = "";

            switch (tRMarkToConvert) {
                case t_RepeatMark.START: str_aux = "start"; break;
                case t_RepeatMark.END: str_aux = "end"; break;
                case t_RepeatMark.X1: str_aux = "x1"; break;
                case t_RepeatMark.X2: str_aux = "x2"; break;
                case t_RepeatMark.X3: str_aux = "x3"; break;
                case t_RepeatMark.X4: str_aux = "x4"; break;
                case t_RepeatMark.X5: str_aux = "x5"; break;
                case t_RepeatMark.X6: str_aux = "x6"; break;
                case t_RepeatMark.X7: str_aux = "x7"; break;
                case t_RepeatMark.X8: str_aux = "x8"; break;
            }//switch

            return str_aux;

        }//t_RepeatMarkToString

        /*******************************************************************************
        * @brief Converts the received string to the equivalent t_RepeatMark variable.
        * @param[in] strRMarkToConvert string with the string to convert to a t_RepeatMark 
        * variable.
        * @return the t_RepeatMark resulting of converting the received string.
        *******************************************************************************/
        public static t_RepeatMark strToTRepeatMark(string strRMarkToConvert) {
            t_RepeatMark tRepeatAux = new t_RepeatMark();

            strRMarkToConvert = strRMarkToConvert.Trim();
            strRMarkToConvert = strRMarkToConvert.ToLower();
            switch (strRMarkToConvert) {
                case "end": tRepeatAux = t_RepeatMark.END; break;
                case "x1": tRepeatAux = t_RepeatMark.X1; break;
                case "x2": tRepeatAux = t_RepeatMark.X2; break;
                case "x3": tRepeatAux = t_RepeatMark.X3; break;
                case "x4": tRepeatAux = t_RepeatMark.X4; break;
                case "x5": tRepeatAux = t_RepeatMark.X5; break;
                case "x6": tRepeatAux = t_RepeatMark.X6; break;
                case "x7": tRepeatAux = t_RepeatMark.X7; break;
                case "x8": tRepeatAux = t_RepeatMark.X8; break;
            }//switch

            return tRepeatAux;

        }//strToTRepeatMark

        /*******************************************************************************
         * @brief Converts the received t_Time variable to a string equivalent
         * @param[in] tTimeToConvert the t_Time variable to convert to a string.
         * @return the string conversion of the received t_Time variable.
         *******************************************************************************/
        public static string tTimeToString(t_Time tTimeToConvert) {
            string str_aux = "";

            switch (tTimeToConvert) {
                case t_Time._16x16: str_aux = "16x16"; break;
                case t_Time._2x2: str_aux = "2x2"; break;
                case t_Time._2x4: str_aux = "2x4"; break;
                case t_Time._3x2: str_aux = "3x2"; break;
                case t_Time._3x4: str_aux = "3x4"; break;
                case t_Time._3x8: str_aux = "3x8"; break;
                case t_Time._4x16: str_aux = "4x16"; break;
                case t_Time._4x4: str_aux = "4x4"; break;
                case t_Time._6x4: str_aux = "6x4"; break;
                case t_Time._6x8: str_aux = "6x8"; break;
                case t_Time._12x8: str_aux = "12x8"; break;
            }//switch

            return str_aux;

        }//tTimeToString

        /*******************************************************************************
         * @brief Converts the received string to the equivalent t_Time variable.
         * @param[in] strTimeToConvert string with the string to convert to a t_Time 
         * variable.
         * @return the t_Time resulting of converting the received string.
         *******************************************************************************/
        public static t_Time strToTimetMark(string strTimeToConvert) {
            t_Time tTimeAux = new t_Time();

            strTimeToConvert = strTimeToConvert.Trim();
            strTimeToConvert = strTimeToConvert.ToLower();
            switch (strTimeToConvert) {
                case "16x16": tTimeAux = t_Time._16x16; break;
                case "2x2": tTimeAux = t_Time._2x2; break;
                case "2x4": tTimeAux = t_Time._2x4; break;
                case "3x2": tTimeAux = t_Time._3x2; break;
                case "3x4": tTimeAux = t_Time._3x4; break;
                case "3x8": tTimeAux = t_Time._3x8; break;
                case "4x16": tTimeAux = t_Time._4x16; break;
                case "4x4": tTimeAux = t_Time._4x4; break;
                case "6x4": tTimeAux = t_Time._6x4; break;
                case "6x8": tTimeAux = t_Time._6x8; break;
                case "12x8": tTimeAux = t_Time._12x8; break;
            }//switch

            return tTimeAux;

        }//strToTimetMark

        /*******************************************************************************
        * @brief Default Melody Channel Instruction constructor
        *******************************************************************************/
        public MChannelCodeEntry() {

            idx = 0;
            by0 = 0x00;
            by1 = 0x00;
            by2 = 0x00;
            strDescr = "";

        }//MChannelCodeEntry

        /*******************************************************************************
        * @brief Melody Channel Instruction constructor with parameters
        * @param[in] _idx position of that instruction in the whole channel instructions list
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        *******************************************************************************/
        public MChannelCodeEntry(int _idx, byte _by0, byte _by1, byte _by2) {

            idx = _idx;
            by0 = _by0;
            by1 = _by1;
            by2 = _by2;
            strDescr = "";

        }//MChannelCodeEntry

        /*******************************************************************************
        * @brief Melody Channel Instruction constructor with parameters
        * @param[in] _idx position of that instruction in the whole channel instructions list
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        * @paran[in] _strDescr description of the created Melody Channel Instruction
        *******************************************************************************/
        public MChannelCodeEntry(int _idx, byte _by0, byte _by1, byte _by2, string _strDescr) {

            idx = _idx;
            by0 = _by0;
            by1 = _by1;
            by2 = _by2;
            strDescr = _strDescr;

        }//MChannelCodeEntry

        /*******************************************************************************
        * Returns the Command Type that is encoded in the MChannelCodeEntry instruction
        * bytes.
        * 
        * @return >=0 returns the Command type encoded in the instruction bytes
        *******************************************************************************/
        public t_Command GetCmdType() {
            MChannelCodeEntry.t_Command tCmdAux = t_Command.NOTE;
            byte byAux = 0;
            byte byAux2 = 0;

            // TIMBRE / INSTRUMENT COMMAND:
            // FIG. 10D-1
            // ON    OFF
            // 8421  8421
            // ----  ----
            // 0000  0000  TIMBRE   
            // 0110  0110  COMMAND
            // ----  ---- 
            // xxxx  xxxx  TIMBRE
            // 0xxx  1xxx  DATA
            // ----  ----
            // ....  ....  L1  REST
            // ....  ....  L2  DURATION
            // ----  ----
            if (by0 == 0x06) {
                tCmdAux = MChannelCodeEntry.t_Command.TIMBRE_INSTRUMENT;
            }//if

            // EFFECT COMMAND
            // ON    OFF
            // 8421  8421
            // ----  ----
            // 0000  0000  EFFECT   
            // 0101  0101  COMMAND
            // ----  ---- 
            // xxxx  xxxx  EFFECT
            // 0xxx  1xxx  DATA
            // ----  ----
            // ....  .... L1  REST
            // ....  .... L2  DURATION
            // ----  ----
            if (by0 == 0x05) {
                tCmdAux = MChannelCodeEntry.t_Command.EFFECT;
            }//if

            // REST DURATION COMMAND
            // FIG. 10B
            // 8421 
            // ---- 
            // 0000  REST DURATION   
            // 0001  COMMAND
            // ---- 
            // ....  REST DURATION  
            // ....  
            // ---- 
            // 0000 
            // 0000
            // ---- 
            if (by0 == 0x01) {
                tCmdAux = MChannelCodeEntry.t_Command.REST_DURATION;
            }//if

            // NOTE COMMAND
            // FIG. 10A-1
            // 8421 
            // ---- 
            // ....  SC PITCH     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
            // ....  OC           OC=4-bit octave code.    octave code for the melody line.
            // ----  
            // ....  L1 TONE      8-bit ON duration code
            // ....  L2 DURATION
            // ---- 
            // ....  L1  REST     8-bit OFF duration code
            // ....  L2  DURATION           
            // ---- 
            byAux = (byte)((by0 & 0xf0) >> 4);
            byAux2 = (byte)(by0 & 0x0f);
            if ((byAux >= 0x1) && (byAux <= 0xC) && (byAux2 >= 0x3) && (byAux2 <= 5)) {
                tCmdAux = MChannelCodeEntry.t_Command.NOTE;
            }//if

            // REPEAT COMMAND:
            // FIG. 10C-1
            // 8421 
            // ---- 
            // 1111 REPEAT 
            // xxxx COMMAND 0=Begining mark, 1=End mark, 8=Repeat x1, 9=Repeat x2, A=Repeat x3, B=Repeat x4, C=Repeat x5, D=Repeat x6, E=Repeat x7, F=Repeat x8
            // ----  
            // 0000  
            // 0000 
            // ---- 
            // 0000  
            // 0000  
            // ---- 
            byAux = (byte)((by0 & 0xf0) >> 4);
            if ((byAux == 0xf) && (by1 == 0x00) && (by2 == 0x00)) {

                tCmdAux = MChannelCodeEntry.t_Command.REPEAT;

            }//if

            // TIE COMMAND:
            // FIG. 10F-1
            // ON    OFF     
            // 8421  8421 
            // ----  ---- 
            // 0000  0000 TIE
            // 1010  1011 COMMAND
            // ----  ---- 
            // 0000  0000
            // 0000  0000 
            // ----  ---- 
            // 0000  0000  
            // 0000  0000 
            // ----  ---- 
            if (by0 == 0x0A)  {
                // TIE ON
                tCmdAux = MChannelCodeEntry.t_Command.TIE;
            } else if (by0 == 0x0B) {
                // TIE OFF
                tCmdAux = MChannelCodeEntry.t_Command.TIE;
            }

            // KEY SYMBOL COMMAND:
            // FIG. 10H
            // 8421 
            // ---- 
            // 1110 KEY SYMBOL
            // 0010 COMMAND
            // ---- 
            // xxxx L  KEY
            // xxxx U  SYMBOL
            // ---- 
            // 0000 NO CHORD  
            // 0000 
            // ---- 
            if (by0 == 0xE2) {
                tCmdAux = MChannelCodeEntry.t_Command.KEY;
            }

            // TIME SYMBOL COMMAND:
            // FIG. 10G
            // 8421 
            // ---- 
            // 1110 TIME SYMBOL
            // 0001 COMMAND
            // ---- 
            // 0000 L  TIME    check FIG 14
            // 0000 U  SYMBOL
            // ---- 
            // 0000 NO CHORD  
            // 0000 
            // ---- 
            if (by0 == 0xE1) {
                tCmdAux = MChannelCodeEntry.t_Command.TIME;
            }

            // BAR COMMAND:
            // FIG. 10I
            // 8421 
            // ---- 
            // 1110 BAR COMMAND
            // 0000
            // ---- 
            // 0000
            // 0000
            // ---- 
            // 0000
            // 0000 
            // ---- 
            if (by0 == 0xE0) {
                tCmdAux = MChannelCodeEntry.t_Command.BAR;
            }

            // END COMMAND:
            // FIG. 10J
            // 8421 
            // ---- 
            // 0000 END COMMAND
            // 1111
            // ---- 
            // 0000
            // 0000
            // ---- 
            // 0000
            // 0000 
            // ---- 
            if (by0 == 0x0F) {
                tCmdAux = MChannelCodeEntry.t_Command.END;
            }


            // DOUBLE DURATION COMMAND:
            // FIG. 10A-2
            // 8421 
            // ---- 
            // 0000
            // 0010
            // ---- 
            // xxxx u1  TONE DURATION
            // xxxx u2 (UPPTER BIT)
            // ---- 
            // xxxx  u1 REST DURATION
            // xxxx  u2 (UPPTER BIT)
            // ---- 
            if (by0 == 0x02) {
                tCmdAux = t_Command.DURATIONx2;
            }

            return tCmdAux;

        }//GetCmdType

        /*******************************************************************************
        * @brief Returns the value of the B0 field of the Melody Channel Instruction in 
        * byte format.
        *******************************************************************************/
        public byte By0AsByte() {

            return by0;

        }//By0AsByte

        /*******************************************************************************
        * @brief Returns the value of the B1 field of the Melody Channel Instruction in 
        * byte format.
        *******************************************************************************/
        public byte By1AsByte() {

            return by1;

        }//By0AsByte

        /*******************************************************************************
        * @brief Returns the value of the B1 field of the Melody Channel Instruction in 
        * byte format.
        *******************************************************************************/
        public byte By2AsByte() {

            return by2;

        }//By0AsByte

        /*******************************************************************************
        * @brief method that analyzes the bytes of the melody instruction and updates the
        * instruction description field with an explanation of the instruction in that bytes.
        * If the current description of the instruction is only a comment then the instruction
        * is not parsed and only the comment is kept. If the description contains a comment
        * after the instruction description then the comment is kept after the parsed description.
        * 
        * @return >=0 the bytes of the command have been succesfully parsed , <0 an  
        * error occurred while trying to parse the bytes of the command.
        *******************************************************************************/
        public ErrCode Parse() {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR;
            string strComentSubstring = "";
            byte byAux = 0x00;
            byte byAux2 = 0x00;
            int iCommentIdx = -1;

            // check the position of the comment symbol and take its position if exists
            if ( (strDescr!=null) && (strDescr.Length>=2)) {
                iCommentIdx = strDescr.Trim().IndexOf("//");
            }
                
            if (iCommentIdx == 0) {

                // when the comment symbol is in the first position then the description
                // of the instruction is not parsed and only the comment is shown
                erCodeRetVal = cErrCodes.ERR_NO_ERROR;

            } else {

                // if the comment is behind the instruction description then it will be kept
                // and then added at the end after parsing the instruction.
                if (iCommentIdx > 0) {
                    strComentSubstring = strDescr.Substring(iCommentIdx, strDescr.Length - iCommentIdx);
                }

                strDescr = "";

                // TIMBRE / INSTRUMENT COMMAND:
                // FIG. 10D-1
                // ON    OFF
                // 8421  8421
                // ----  ----
                // 0000  0000  TIMBRE   
                // 0110  0110  COMMAND
                // ----  ---- 
                // xxxx  xxxx  TIMBRE
                // 0xxx  1xxx  DATA
                // ----  ----
                // ....  ....  L1  REST
                // ....  ....  L2  DURATION
                // ----  ----
                if (by0 == 0x06) {

                    // TIMBRE DATA
                    if ((by1 & 0x08) != 0) {
                        strDescr = "timbre:off:";
                    } else {
                        strDescr = "timbre:on:";
                    }//if

                    byAux = (byte)(by1 & 0xF7);// force bit 3 to '0'
                    switch (byAux) {
                        case 0x00:
                            strDescr = strDescr + "piano";
                            break;
                        case 0x01:
                            strDescr = strDescr + "harpsichord";
                            break;
                        case 0x02:
                            strDescr = strDescr + "organ";
                            break;
                        case 0x03:
                            strDescr = strDescr + "violin";
                            break;
                        case 0x04:
                            strDescr = strDescr + "flute";
                            break;
                        case 0x05:
                            strDescr = strDescr + "clarinet";
                            break;
                        case 0x06:
                            strDescr = strDescr + "trumpet";
                            break;
                        case 0x07:
                            strDescr = strDescr + "celesta";
                            break;
                        default:
                            strDescr = strDescr + "¿" + byAux.ToString() + "?";
                            break;
                    }//switch

                    // REST DURATION
                    strDescr = strDescr + " rest:" + AuxFuncs.SwapByteNibbles(by2).ToString("D3");// as high and low nibbles are inverted in the value they must be swapped

                }//if

                // EFFECT COMMAND
                // ON    OFF
                // 8421  8421
                // ----  ----
                // 0000  0000  EFFECT   
                // 0101  0101  COMMAND
                // ----  ---- 
                // xxxx  xxxx  EFFECT
                // 0xxx  1xxx  DATA
                // ----  ----
                // ....  .... L1  REST
                // ....  .... L2  DURATION
                // ----  ----
                if (by0 == 0x05) {

                    // EFFECT DATA
                    if ((by1 & 0x08) != 0) {
                        strDescr = "effect off:";
                    } else {
                        strDescr = "effect on:";
                    }//if

                    byAux = (byte)(by1 & 0xF7);// force bit 3 to '0'
                    switch (byAux) {
                        case 0x00:
                        case 0x01:
                        case 0x02:
                        case 0x03:
                        case 0x04:
                        case 0x05:
                        case 0x06:
                        case 0x07:
                            strDescr = strDescr + "sustain:" + byAux.ToString("D2");
                            break;
                        case 0x10:
                            strDescr = strDescr + "vibrato";
                            break;
                        case 0x20:
                            strDescr = strDescr + "delay vibrato";
                            break;
                        default:
                            strDescr = strDescr + "¿" + byAux.ToString() + "?";
                            break;
                    }//switch

                    strDescr = strDescr + " rest:" + AuxFuncs.SwapByteNibbles(by2).ToString("D3");// as high and low nibbles are inverted in the value they must be swapped

                }//if

                // REST DURATION COMMAND
                // FIG. 10B
                // 8421 
                // ---- 
                // 0000  REST DURATION   
                // 0001  COMMAND
                // ---- 
                // ....  REST DURATION  
                // ....  
                // ---- 
                // 0000 
                // 0000
                // ---- 
                if (by0 == 0x01) {

                    strDescr = "rest duration:";
                    strDescr = strDescr + "" + AuxFuncs.SwapByteNibbles(by1).ToString("D3");// as high and low nibbles are inverted in the value they must be swapped

                }//if

                // NOTE COMMAND
                // FIG. 10A-1
                // 8421 
                // ---- 
                // ....  SC PITCH     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
                // ....  OC           OC=4-bit octave code.    octave code for the melody line.
                // ----  
                // ....  L1 TONE      8-bit ON duration code
                // ....  L2 DURATION
                // ---- 
                // ....  L1  REST     8-bit OFF duration code
                // ....  L2  DURATION           
                // ---- 
                byAux = (byte)((by0 & 0xf0) >> 4);
                byAux2 = (byte)(by0 & 0x0f);
                if ((byAux >= 0x1) && (byAux <= 0xC) && (byAux2 >= 0x3) && (byAux2 <= 6)) {

                    strDescr = "note:";
                    switch (byAux) {
                        case 0x1:
                            strDescr = strDescr + "c" + byAux2.ToString() + " ";
                            break;
                        case 0x2:
                            strDescr = strDescr + "c#" + byAux2.ToString();
                            break;
                        case 0x3:
                            strDescr = strDescr + "d" + byAux2.ToString() + " ";
                            break;
                        case 0x4:
                            strDescr = strDescr + "d#" + byAux2.ToString();
                            break;
                        case 0x5:
                            strDescr = strDescr + "e" + byAux2.ToString() + " ";
                            break;
                        case 0x6:
                            strDescr = strDescr + "f" + byAux2.ToString() + " ";
                            break;
                        case 0x7:
                            strDescr = strDescr + "f#" + byAux2.ToString();
                            break;
                        case 0x8:
                            strDescr = strDescr + "g" + byAux2.ToString() + " ";
                            break;
                        case 0x9:
                            strDescr = strDescr + "g#" + byAux2.ToString();
                            break;
                        case 0xA:
                            strDescr = strDescr + "a" + byAux2.ToString() + " ";
                            break;
                        case 0xB:
                            strDescr = strDescr + "a#" + byAux2.ToString();
                            break;
                        case 0xC:
                            strDescr = strDescr + "b" + byAux2.ToString() + " ";
                            break;
                        default:
                            strDescr = strDescr + "¿" + byAux.ToString() + "?";
                            break;
                    }//if

                    strDescr = strDescr + " dur:" + AuxFuncs.SwapByteNibbles(by1).ToString("D3");// as high and low nibbles are inverted in the value they must be swapped;
                    strDescr = strDescr + " rest:" + AuxFuncs.SwapByteNibbles(by2).ToString("D3");// as high and low nibbles are inverted in the value they must be swapped

                }//if

                // REPEAT COMMAND:
                // FIG. 10C-1
                // 8421 
                // ---- 
                // 1111 REPEAT 
                // xxxx COMMAND 0=Begining mark, 1=End mark, 8=Repeat x1, 9=Repeat x2, A=Repeat x3, B=Repeat x4, C=Repeat x5, D=Repeat x6, E=Repeat x7, F=Repeat x8
                // ----  
                // 0000  
                // 0000 
                // ---- 
                // 0000  
                // 0000  
                // ---- 
                byAux = (byte)((by0 & 0xf0) >> 4);
                if ((byAux == 0xf) && (by1 == 0x00) && (by2 == 0x00)) {

                    strDescr = "repeat:";
                    byAux2 = (byte)(by0 & 0x0f);
                    switch (byAux2) {
                        case 0x0:
                            strDescr = strDescr + "start mark";
                            break;
                        case 0x1:
                            strDescr = strDescr + "end mark";
                            break;
                        case 0x8:
                            strDescr = strDescr + "x1";
                            break;
                        case 0x9:
                            strDescr = strDescr + "x2";
                            break;
                        case 0xA:
                            strDescr = strDescr + "x3";
                            break;
                        case 0xB:
                            strDescr = strDescr + "x4";
                            break;
                        case 0xC:
                            strDescr = strDescr + "x5";
                            break;
                        case 0xD:
                            strDescr = strDescr + "x6";
                            break;
                        case 0xE:
                            strDescr = strDescr + "x7";
                            break;
                        case 0xF:
                            strDescr = strDescr + "x8";
                            break;
                        default:
                            strDescr = strDescr + "¿" + byAux2.ToString() + "?";
                            break;
                    }//switch

                }//if

                // TIE COMMAND:
                // FIG. 10F-1
                // ON    OFF     
                // 8421  8421 
                // ----  ---- 
                // 0000  0000 TIE
                // 1010  1011 COMMAND
                // ----  ---- 
                // 0000  0000
                // 0000  0000 
                // ----  ---- 
                // 0000  0000  
                // 0000  0000 
                // ----  ---- 
                if (by0 == 0x0A) {
                    strDescr = "tie:on";
                } else if (by0 == 0x0B) {
                    strDescr = "tie:off";
                }

                // KEY SYMBOL COMMAND:
                // FIG. 10H
                // 8421 
                // ---- 
                // 1110 KEY SYMBOL
                // 0010 COMMAND
                // ---- 
                // xxxx L  KEY
                // xxxx U  SYMBOL
                // ---- 
                // 0000 NO CHORD  
                // 0000 
                // ---- 
                if (by0 == 0xE2) {
                    strDescr = "key sym:" + by1.ToString("D3");
                }

                // TIME SYMBOL COMMAND:
                // FIG. 10G
                // 8421 
                // ---- 
                // 1110 TIME SYMBOL
                // 0001 COMMAND
                // ---- 
                // 0000 L  TIME   check FIG 14
                // 0000 U  SYMBOL
                // ---- 
                // 0000 NO CHORD  
                // 0000 
                // ---- 
                if (by0 == 0xE1) {
                    strDescr = "time sym:";

                    switch (by1) {
                        // encode the time symbol
                        case 0x00: 
                            strDescr = strDescr + "16x16"; 
                            break;
                        case 0x22: 
                            strDescr = strDescr + "2x2"; 
                            break;
                        case 0x24: 
                            strDescr = strDescr + "2x4"; 
                            break;
                        case 0x32: 
                            strDescr = strDescr + "3x2"; 
                            break;
                        case 0x34: 
                            strDescr = strDescr + "3x4"; 
                            break;
                        case 0x38: 
                            strDescr = strDescr + "3x8"; 
                            break;
                        case 0x40: 
                            strDescr = strDescr + "4x16"; 
                            break;
                        case 0x44: 
                            strDescr = strDescr + "4x4"; 
                            break;
                        case 0x64: 
                            strDescr = strDescr + "6x4"; 
                            break;
                        case 0x68: 
                            strDescr = strDescr + "6x8"; 
                            break;
                        case 0xC8: 
                            strDescr = strDescr + "12x8"; 
                            break;
                    }//switch

                }//if

                // BAR COMMAND:
                // FIG. 10I
                // 8421 
                // ---- 
                // 1110 BAR COMMAND
                // 0000
                // ---- 
                // 0000
                // 0000
                // ---- 
                // 0000
                // 0000 
                // ---- 
                if (by0 == 0xE0) {
                    strDescr = "bar";
                }

                // END COMMAND:
                // FIG. 10J
                // 8421 
                // ---- 
                // 0000 END COMMAND
                // 1111
                // ---- 
                // 0000
                // 0000
                // ---- 
                // 0000
                // 0000 
                // ---- 
                if (by0 == 0x0F) {
                    strDescr = "end";
                }

                // DOUBLE DURATION COMMAND:
                // FIG. 10A-2
                // 8421 
                // ---- 
                // 0000
                // 0010
                // ---- 
                // xxxx u1  TONE DURATION
                // xxxx u2 (UPPTER BIT)
                // ---- 
                // xxxx  u1 REST DURATION
                // xxxx  u2 (UPPTER BIT)
                // ---- 
                if (by0 == 0x02) {
                    strDescr = "2xduration";
                    strDescr = strDescr + " dur:" + AuxFuncs.SwapByteNibbles(by1).ToString("D3");// as high and low nibbles are inverted in the value they must be swapped
                    strDescr = strDescr + " rest:" + AuxFuncs.SwapByteNibbles(by2).ToString("D3");// as high and low nibbles are inverted in the value they must be swapped
                }

                // add the instruction comment in case there is something
                strDescr = strDescr + strComentSubstring;

            }// if (iCommentIdx == 0) 

            return erCodeRetVal;

        }//Parse

        /*******************************************************************************
        * @brief method that receives the parameters of a TIMBRE_INSTRUMENT command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] tInstrumentIn
        * @param[in] tOnOffIn
        * @param[in] iRest
        * @param[out] _by0
        * @param[out] _by1
        * @param[out] _by2
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetInstrumentCommandBytesFromParams(t_Instrument tInstrumentIn, t_On_Off tOnOffIn, int iRestIn, ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 =0 ;
            int iBy1 = 0;
            int iBy2 = 0;

            // TIMBRE / INSTRUMENT COMMAND:
            // FIG. 10D-1
            // ON    OFF
            // 8421  8421
            // ----  ----
            // 0000  0000  TIMBRE   
            // 0110  0110  COMMAND
            // ----  ---- 
            // xxxx  xxxx  TIMBRE
            // 0xxx  1xxx  DATA
            // ----  ----
            // ....  ....  L1  REST
            // ....  ....  L2  DURATION
            // ----  ----

            // set the TIMBRE / INSTRUMENT COMMAND
            iBy0 = 0x06; 
            
            // encode the t_On_Off
            switch (tOnOffIn) {    
                case t_On_Off.OFF:
                    iBy1 = iBy1 | 0x08; // set byte 1 bit 3: OFF
                    break;
                case t_On_Off.ON:
                default:
                    iBy1 = iBy1 & 0xF7; // clear byte 1 bit 3: : ON
                    break;
            }
            
            // encode the instrument
            switch (tInstrumentIn) {  //
                case t_Instrument.PIANO:
                    iBy1 = iBy1 | 0x00;
                    break;
                case t_Instrument.HARPISCHORD:
                    iBy1 = iBy1 | 0x01;
                    break;
                case t_Instrument.ORGAN:
                    iBy1 = iBy1 | 0x02;
                    break;
                case t_Instrument.VIOLIN:
                    iBy1 = iBy1 | 0x03;
                    break;
                case t_Instrument.FLUTE:
                    iBy1 = iBy1 | 0x04;
                    break;
                case t_Instrument.CLARINET:
                    iBy1 = iBy1 | 0x05;
                    break;
                case t_Instrument.TRUMPET:
                    iBy1 = iBy1 | 0x06;
                    break;
                case t_Instrument.CELESTA:
                    iBy1 = iBy1 | 0x07;
                    break;
                default:
                    break;
            }//switch
            
            // encode the rest duration: the nibbles of the value must be swapped
            iBy2 = (byte)AuxFuncs.SwapByteNibbles((byte)iRestIn);         

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);
            _by2 = Convert.ToByte(iBy2);

            return erCodeRetVal;

        }//GetInstrumentCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a TIMBRE_INSTRUMENT command an returns  
        * the command parameters that correspond to the TIMBRE_INSTRUMENT command encoded
        * in the received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        * @param[out] tInstrumentOut
        * @param[out] tOnOffOut
        * @param[out] iRestOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetInstrumentCommandParamsFromBytes(byte _by0, byte _by1, byte _by2, ref t_Instrument tInstrumentOut, ref t_On_Off tOnOffOut, ref int iRestOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iBy2 = Convert.ToInt32(_by2);
            int iByAux = 0;


            // TIMBRE / INSTRUMENT COMMAND:
            // FIG. 10D-1
            // ON    OFF
            // 8421  8421
            // ----  ----
            // 0000  0000  TIMBRE   
            // 0110  0110  COMMAND
            // ----  ---- 
            // xxxx  xxxx  TIMBRE
            // 0xxx  1xxx  DATA
            // ----  ----
            // ....  ....  L1  REST
            // ....  ....  L2  DURATION
            // ----  ----

            // check the TIMBRE / INSTRUMENT COMMAND
            if (iBy0 != 0x06) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }

            if (erCodeRetVal.i_code >= 0) {
                // decode the t_On_Off: check bit 3: 1:OFF 0:ON
                iByAux = iBy1 & 0x08;
                switch (iByAux) {
                    case 0x08:
                        tOnOffOut = t_On_Off.OFF;
                        break;
                    case 0x00:
                        tOnOffOut = t_On_Off.ON;
                        break;
                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch
            }//if

            if (erCodeRetVal.i_code >= 0) {
                // decode the instrument
                iByAux = iBy1 & 0x07;
                switch (iByAux) {
                    case 0x00:
                        tInstrumentOut = t_Instrument.PIANO;
                        break;
                    case 0x01:
                        tInstrumentOut = t_Instrument.HARPISCHORD;
                        break;
                    case 0x02:
                        tInstrumentOut = t_Instrument.ORGAN;
                        break;
                    case 0x03:
                        tInstrumentOut = t_Instrument.VIOLIN;
                        break;
                    case 0x04:
                        tInstrumentOut = t_Instrument.FLUTE;
                        break;
                    case 0x05:
                        tInstrumentOut = t_Instrument.CLARINET;
                        break;
                    case 0x06:
                        tInstrumentOut = t_Instrument.TRUMPET;
                        break;
                    case 0x07:
                        tInstrumentOut = t_Instrument.CELESTA;
                        break;

                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch
            }//if

            if (erCodeRetVal.i_code >= 0) {
                // decode the rest duration: the nibbles of the value are sotred swapped 
                iRestOut = (int)AuxFuncs.SwapByteNibbles((byte)iBy2);
            }//if

            return erCodeRetVal;

        }//GetInstrumentCommandParamsFromBytes

        /*******************************************************************************
        * @brief method that receives the parameters of a EFFECT command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] tEffectIn
        * @param[in] tOnOffIn
        * @param[in] iRest
        * @param[out] _by0
        * @param[out] _by1
        * @param[out] _by2
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetEffectCommandBytesFromParams(t_Effect tEffectIn, t_On_Off tOnOffIn, int iRestIn, ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iBy2 = 0;

            // EFFECT COMMAND
            // ON    OFF
            // 8421  8421
            // ----  ----
            // 0000  0000  EFFECT   
            // 0101  0101  COMMAND
            // ----  ---- 
            // xxxx  xxxx  EFFECT
            // 0xxx  1xxx  DATA
            // ----  ----
            // ....  .... L1  REST
            // ....  .... L2  DURATION
            // ----  ----


            // set the EFFECT COMMAND
            iBy0 = 0x05;

            // encode the t_On_Off
            switch (tOnOffIn) {
                case t_On_Off.OFF:
                    iBy1 = iBy1 | 0x08; // set byte 1 bit 3: OFF
                    break;
                case t_On_Off.ON:
                default:
                    iBy1 = iBy1 & 0xF7; // clear byte 1 bit 3: : ON
                    break;
            }//switch

            // encode the effect
            switch (tEffectIn) {
                case t_Effect.SUST0:
                    iBy1 = iBy1 | 0x00;
                    break;
                case t_Effect.SUST1:
                    iBy1 = iBy1 | 0x01;
                    break;
                case t_Effect.SUST2:
                    iBy1 = iBy1 | 0x02;
                    break;
                case t_Effect.SUST3:
                    iBy1 = iBy1 | 0x03;
                    break;
                case t_Effect.SUST4:
                    iBy1 = iBy1 | 0x04;
                    break;
                case t_Effect.SUST5:
                    iBy1 = iBy1 | 0x05;
                    break;
                case t_Effect.SUST6:
                    iBy1 = iBy1 | 0x06;
                    break;
                case t_Effect.SUST7:
                    iBy1 = iBy1 | 0x07;
                    break;
                case t_Effect.VIBRATO:
                    iBy1 = iBy1 | 0x10;
                    break;
                case t_Effect.DELAY_VIBRATO:
                    iBy1 = iBy1 | 0x20;
                    break;
                default:
                    break;
            }//switch

            // encode the rest duration: the nibbles of the value must be swapped
            iBy2 = (byte)AuxFuncs.SwapByteNibbles((byte)iRestIn);

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);
            _by2 = Convert.ToByte(iBy2);

            return erCodeRetVal;

        }//GetEffectCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a EFFECT command an returns  
        * the command parameters that correspond to the EFFECT command encoded
        * in the received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        * @param[out] tEffectOut
        * @param[out] tOnOffOut
        * @param[out] iRestOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetEffectCommandParamsFromBytes( byte _by0, byte _by1, byte _by2, ref t_Effect tEffectOut, ref t_On_Off tOnOffOut, ref int iRestOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iBy2 = Convert.ToInt32(_by2);
            int iByAux = 0;

            // FIG 10E-1
            // EFFECT COMMAND
            // ON    OFF
            // 8421  8421
            // ----  ----
            // 0000  0000  EFFECT   
            // 0101  0101  COMMAND
            // ----  ---- 
            // xxxx  xxxx  EFFECT
            // 0xxx  1xxx  DATA
            // ----  ----
            // ....  .... L1  REST
            // ....  .... L2  DURATION
            // ----  ----

            // check the EFFECT
            if (iBy0 != 0x05) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }//if

            if (erCodeRetVal.i_code >= 0) {
                // decode the t_On_Off: check bit 3: 1:OFF 0:ON
                iByAux = iBy1 & 0x08;
                switch (iByAux) {
                    case 0x08:
                        tOnOffOut = t_On_Off.OFF;
                        break;
                    case 0x00:
                        tOnOffOut = t_On_Off.ON;
                        break;
                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch
            }//if

            if (erCodeRetVal.i_code >= 0) {
                // decode the instrument
                // JBR 2024-9-9 As this is a bitset this should be changed to allow multiple simultaneous effects
                iByAux = iBy1 & 0x07;
                switch (iByAux) {
                    case 0x00:
                        tEffectOut = t_Effect.SUST0;
                        break;
                    case 0x01:
                        tEffectOut = t_Effect.SUST1;
                        break;
                    case 0x02:
                        tEffectOut = t_Effect.SUST2;
                        break;
                    case 0x03:
                        tEffectOut = t_Effect.SUST3;
                        break;
                    case 0x04:
                        tEffectOut = t_Effect.SUST4;
                        break;
                    case 0x05:
                        tEffectOut = t_Effect.SUST5;
                        break;
                    case 0x06:
                        tEffectOut = t_Effect.SUST6;
                        break;
                    case 0x07:
                        tEffectOut = t_Effect.SUST7;
                        break;
                    case 0x10:
                        tEffectOut = t_Effect.VIBRATO;
                        break;
                    case 0x20:
                        tEffectOut = t_Effect.DELAY_VIBRATO;
                        break;
                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch
            }//if

            if (erCodeRetVal.i_code >= 0) {
                // decode the rest duration: the nibbles of the value are sotred swapped 
                iRestOut = (int)AuxFuncs.SwapByteNibbles((byte)iBy2);
            }//iff

            return erCodeRetVal;

        }//GetEffectCommandParamsFromBytes

        /*******************************************************************************
        * @brief method that receives the parameters of a REST_DURATION command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] iRestIn
        * @param[out] _by0
        * @param[out] _by1
        * @param[out] _by2
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetRestCommandBytesFromParams( int iRestIn, ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iBy2 = 0;

            // REST DURATION COMMAND
            // FIG. 10B
            // 8421 
            // ---- 
            // 0000  REST DURATION   
            // 0001  COMMAND
            // ---- 
            // ....  REST DURATION  
            // ....  
            // ---- 
            // 0000 
            // 0000
            // ---- 

            // set the REST_DURATION COMMAND
            iBy0 = 0x01;

            // encode the rest duration: the nibbles of the value must be swapped
            iBy1 = (byte)AuxFuncs.SwapByteNibbles((byte)iRestIn);

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);
            _by2 = Convert.ToByte(iBy2);

            return erCodeRetVal;

        }//GetRestCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a REST_DURATION command an returns  
        * the command parameters that correspond to the REST_DURATION command encoded
        * in the received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        * @param[out] iRestIn
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetRestCommandParamsFromBytes( byte _by0,  byte _by1, byte _by2, ref int iRestOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iBy2 = Convert.ToInt32(_by2);

            // REST DURATION COMMAND
            // FIG. 10B
            // 8421 
            // ---- 
            // 0000  REST DURATION   
            // 0001  COMMAND
            // ---- 
            // ....  REST DURATION  
            // ....  
            // ---- 
            // 0000 
            // 0000
            // ---- 

            // check that it is the REST_DURATION COMMAND
            if (iBy0 != 0x01) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }//if

            if (erCodeRetVal.i_code >= 0) {
                // decode the rest duration: the nibbles of the value are sotred swapped 
                iRestOut = (int)AuxFuncs.SwapByteNibbles((byte)iBy1);
            }//if

            return erCodeRetVal;

        }//GetRestCommandParamsFromBytes

        /*******************************************************************************
        * @brief method that receives the parameters of a NOTE command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] tNoteIn
        * @param[in] iDurationIn
        * @param[in] iRestIn
        * @param[out] _by0
        * @param[out] _by1
        * @param[out] _by2
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetNoteCommandBytesFromParams(t_Notes tNoteIn, int iDurationIn, int iRestIn, ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iBy2 = 0;

            // NOTE COMMAND
            // FIG. 10A-1
            // 8421 
            // ---- 
            // ....  SC PITCH     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
            // ....  OC           OC=4-bit octave code.    octave code for the melody line.
            // ----  
            // ....  L1 TONE      8-bit ON duration code
            // ....  L2 DURATION
            // ---- 
            // ....  L1  REST     8-bit OFF duration code
            // ....  L2  DURATION           
            // ---- 

            // encode the note code
            switch (tNoteIn) {
                case t_Notes.C3:
                case t_Notes.C4:
                case t_Notes.C5:
                case t_Notes.C6:
                    iBy0 = iBy0 | 0x10;
                    break;
                case t_Notes.Csh3:
                case t_Notes.Csh4:
                case t_Notes.Csh5:
                    iBy0 = iBy0 | 0x20;
                    break;
                case t_Notes.D3:
                case t_Notes.D4:
                case t_Notes.D5:
                    iBy0 = iBy0 | 0x30;
                    break;
                case t_Notes.Dsh3:
                case t_Notes.Dsh4:
                case t_Notes.Dsh5:
                    iBy0 = iBy0 | 0x40;
                    break;
                case t_Notes.E3:
                case t_Notes.E4:
                case t_Notes.E5:
                    iBy0 = iBy0 | 0x50;
                    break;
                case t_Notes.F3:
                case t_Notes.F4:
                case t_Notes.F5:
                    iBy0 = iBy0 | 0x60;
                    break;
                case t_Notes.Fsh3:
                case t_Notes.Fsh4:
                case t_Notes.Fsh5:
                    iBy0 = iBy0 | 0x70;
                    break;
                case t_Notes.G3:
                case t_Notes.G4:
                case t_Notes.G5:
                    iBy0 = iBy0 | 0x80;
                    break;
                case t_Notes.Gsh3:
                case t_Notes.Gsh4:
                case t_Notes.Gsh5:
                    iBy0 = iBy0 | 0x90;
                    break;
                case t_Notes.A3:
                case t_Notes.A4:
                case t_Notes.A5:
                    iBy0 = iBy0 | 0xA0;
                    break;
                case t_Notes.Ash3:
                case t_Notes.Ash4:
                case t_Notes.Ash5:
                    iBy0 = iBy0 | 0xB0;
                    break;
                case t_Notes.B3:
                case t_Notes.B4:
                case t_Notes.B5:
                    iBy0 = iBy0 | 0xC0;
                    break;
                default:
                    break;
            }

            // encode the note octave
            switch (tNoteIn) {

                case t_Notes.C3:
                case t_Notes.Csh3:
                case t_Notes.D3:
                case t_Notes.Dsh3:
                case t_Notes.E3:
                case t_Notes.F3:
                case t_Notes.Fsh3:
                case t_Notes.G3:
                case t_Notes.Gsh3:
                case t_Notes.A3:
                case t_Notes.Ash3:
                case t_Notes.B3:
                    iBy0 = iBy0 | 3;
                    break;

                case t_Notes.C4:
                case t_Notes.Csh4:
                case t_Notes.D4:
                case t_Notes.Dsh4:
                case t_Notes.E4:
                case t_Notes.F4:
                case t_Notes.Fsh4:
                case t_Notes.G4:
                case t_Notes.Gsh4:
                case t_Notes.A4:
                case t_Notes.Ash4:
                case t_Notes.B4:
                    iBy0 = iBy0 | 4;
                    break;

                case t_Notes.C5:
                case t_Notes.Csh5:
                case t_Notes.D5:
                case t_Notes.Dsh5:
                case t_Notes.E5:
                case t_Notes.F5:
                case t_Notes.Fsh5:
                case t_Notes.G5:
                case t_Notes.Gsh5:
                case t_Notes.A5:
                case t_Notes.Ash5:
                case t_Notes.B5:
                    iBy0 = iBy0 | 5;
                    break;

                case t_Notes.C6:
                    iBy0 = iBy0 | 6;
                    break;

                default:
                    break;
            }//switch

            // encode the rest duration: the nibbles of the value must be swapped
            iBy1 = (byte)AuxFuncs.SwapByteNibbles((byte)iDurationIn);

            // encode the rest duration: the nibbles of the value must be swapped
            iBy2 = (byte)AuxFuncs.SwapByteNibbles((byte)iRestIn);

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);
            _by2 = Convert.ToByte(iBy2);

            return erCodeRetVal;

        }//GetNoteCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a NOTE command an returns the command 
        * parameters that correspond to the NOTE command encoded in the received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        * @param[out] tNoteOut
        * @param[out] iDurationOut
        * @param[out] iRestOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetNoteCommandParamsFromBytes(byte _by0, byte _by1, byte _by2, ref t_Notes tNoteOut, ref int iDurationOut, ref int iRestOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iBy2 = Convert.ToInt32(_by2);
            int iByAux = 0;
            int iByAux2 = 0;

            // NOTE COMMAND
            // FIG. 10A-1
            // 8421 
            // ---- 
            // ....  SC PITCH     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
            // ....  OC           OC=4-bit octave code.    octave code for the melody line.
            // ----  
            // ....  L1 TONE      8-bit ON duration code
            // ....  L2 DURATION
            // ---- 
            // ....  L1  REST     8-bit OFF duration code
            // ....  L2  DURATION           
            // ---- 

            // check the NOTE COMMAND
            if ( (iBy0 < 0x10) || (iBy0 > 0xCF) ) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }

            if (erCodeRetVal.i_code >= 0) {

                iByAux = 0x0F & _by0;
                iByAux2 = 0xF0 & _by0;
                // first get the octave of the note
                switch (iByAux) {

                    case 0x03:// 3th OCTAVE ##########################

                        // then get the note in the octave
                        switch (iByAux2) {
                            case 0x10:
                                tNoteOut = t_Notes.C3;
                                break;
                            case 0x20:
                                tNoteOut = t_Notes.Csh3;
                                break;
                            case 0x30:
                                tNoteOut = t_Notes.D3;
                                break;
                            case 0x40:
                                tNoteOut = t_Notes.Dsh3;
                                break;
                            case 0x50:
                                tNoteOut = t_Notes.E3;
                                break;
                            case 0x60:
                                tNoteOut = t_Notes.F3;
                                break;
                            case 0x70:
                                tNoteOut = t_Notes.Fsh3;
                                break;
                            case 0x80:
                                tNoteOut = t_Notes.G3;
                                break;
                            case 0x90:
                                tNoteOut = t_Notes.Gsh3;
                                break;
                            case 0xA0:
                                tNoteOut = t_Notes.A3;
                                break;
                            case 0xB0:
                                tNoteOut = t_Notes.Ash3;
                                break;
                            case 0xC0:
                                tNoteOut = t_Notes.B3;
                                break;
                            default:
                                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                                break;
                        }//switch
                        break;

                    case 0x04:// 4th OCTAVE ##########################

                        // then get the note in the octave
                        switch (iByAux2) {
                            case 0x10:
                                tNoteOut = t_Notes.C4;
                                break;
                            case 0x20:
                                tNoteOut = t_Notes.Csh4;
                                break;
                            case 0x30:
                                tNoteOut = t_Notes.D4;
                                break;
                            case 0x40:
                                tNoteOut = t_Notes.Dsh4;
                                break;
                            case 0x50:
                                tNoteOut = t_Notes.E4;
                                break;
                            case 0x60:
                                tNoteOut = t_Notes.F4;
                                break;
                            case 0x70:
                                tNoteOut = t_Notes.Fsh4;
                                break;
                            case 0x80:
                                tNoteOut = t_Notes.G4;
                                break;
                            case 0x90:
                                tNoteOut = t_Notes.Gsh4;
                                break;
                            case 0xA0:
                                tNoteOut = t_Notes.A4;
                                break;
                            case 0xB0:
                                tNoteOut = t_Notes.Ash4;
                                break;
                            case 0xC0:
                                tNoteOut = t_Notes.B4;
                                break;
                            default:
                                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                                break;
                        }//switch
                        break;

                    case 0x05:// 5th OCTAVE ##########################

                        // then get the note in the octave
                        switch (iByAux2) {
                            case 0x10:
                                tNoteOut = t_Notes.C5;
                                break;
                            case 0x20:
                                tNoteOut = t_Notes.Csh5;
                                break;
                            case 0x30:
                                tNoteOut = t_Notes.D5;
                                break;
                            case 0x40:
                                tNoteOut = t_Notes.Dsh5;
                                break;
                            case 0x50:
                                tNoteOut = t_Notes.E5;
                                break;
                            case 0x60:
                                tNoteOut = t_Notes.F5;
                                break;
                            case 0x70:
                                tNoteOut = t_Notes.Fsh5;
                                break;
                            case 0x80:
                                tNoteOut = t_Notes.G5;
                                break;
                            case 0x90:
                                tNoteOut = t_Notes.Gsh5;
                                break;
                            case 0xA0:
                                tNoteOut = t_Notes.A5;
                                break;
                            case 0xB0:
                                tNoteOut = t_Notes.Ash5;
                                break;
                            case 0xC0:
                                tNoteOut = t_Notes.B5;
                                break;
                            default:
                                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                                break;
                        }//switch
                        break;

                    case 0x06:// 6th OCTAVE ##########################

                        // then get the note in the octave
                        switch (iByAux2) {
                            case 0x10:
                                tNoteOut = t_Notes.C4;
                                break;
                            default:
                                break;
                        }//switch
                        break;

                    default:
                        break;
                
                }//switch (iByAux)

            }//if                

            if (erCodeRetVal.i_code >= 0) {
                // decode the note duration: the nibbles of the value are sotred swapped 
                iDurationOut = (int)AuxFuncs.SwapByteNibbles((byte)iBy1);
            }//if


            if (erCodeRetVal.i_code >= 0) {
                // decode the rest duration: the nibbles of the value are sotred swapped 
                iRestOut = (int)AuxFuncs.SwapByteNibbles((byte)iBy2);
            }//if

            return erCodeRetVal;

        }//GetNoteCommandParamsFromBytes

        /*******************************************************************************
        * @brief method that receives the parameters of a REPEAT command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] tRepeatIn
        * @param[out] _by0
        * @param[out] _by1
        * @param[out] _by2
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetRepeatCommandBytesFromParams(t_RepeatMark tRepeatIn, ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iBy2 = 0;

            // REPEAT COMMAND:
            // FIG. 10C-1
            // 8421 
            // ---- 
            // 1111 REPEAT 
            // xxxx COMMAND 0=Begining mark, 1=End mark, 8=Repeat x1, 9=Repeat x2, A=Repeat x3, B=Repeat x4, C=Repeat x5, D=Repeat x6, E=Repeat x7, F=Repeat x8
            // ----  
            // 0000  
            // 0000 
            // ---- 
            // 0000  
            // 0000  
            // ---- 

            // encode the REPEAT command code
            iBy0 = iBy0 | 0xF0;
            switch (tRepeatIn) {
                case t_RepeatMark.START:
                    iBy0 = iBy0 | 0x00;
                    break;
                case t_RepeatMark.END:
                    iBy0 = iBy0 | 0x01;
                    break;
                case t_RepeatMark.X1:
                    iBy0 = iBy0 | 0x08;
                    break;
                case t_RepeatMark.X2:
                    iBy0 = iBy0 | 0x09;
                    break;
                case t_RepeatMark.X3:
                    iBy0 = iBy0 | 0x0A;
                    break;
                case t_RepeatMark.X4:
                    iBy0 = iBy0 | 0x0B;
                    break;
                case t_RepeatMark.X5:
                    iBy0 = iBy0 | 0x0C;
                    break;
                case t_RepeatMark.X6:
                    iBy0 = iBy0 | 0x0D;
                    break;
                case t_RepeatMark.X7:
                    iBy0 = iBy0 | 0x0E;
                    break;
                case t_RepeatMark.X8:
                    iBy0 = iBy0 | 0x0F;
                    break;
                default:
                    erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                    break;
            }//switch

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);
            _by2 = Convert.ToByte(iBy2);

            return erCodeRetVal;

        }//GetRepeatCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a REPEAT command an returns the command 
        * parameters that correspond to the REPEAT command encoded in the received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        * @param[out] tRepeatOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetRepeatCommandParamsFromBytes( byte _by0, byte _by1, byte _by2, ref t_RepeatMark tRepeatOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iBy2 = Convert.ToInt32(_by2);
            int iByAux = 0;

            // REPEAT COMMAND:
            // FIG. 10C-1
            // 8421 
            // ---- 
            // 1111 REPEAT 
            // xxxx COMMAND 0=Begining mark, 1=End mark, 8=Repeat x1, 9=Repeat x2, A=Repeat x3, B=Repeat x4, C=Repeat x5, D=Repeat x6, E=Repeat x7, F=Repeat x8
            // ----  
            // 0000  
            // 0000 
            // ---- 
            // 0000  
            // 0000  
            // ---- 

            // check the REPEAT command
            if ((iBy0 < 0xF0)) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }

            if (erCodeRetVal.i_code >= 0) {
                // decode the REPEAT command code
                iByAux = 0x0F & _by0;
                switch (iByAux) {
                    case 0x00:
                        tRepeatOut = t_RepeatMark.START;
                        break;
                    case 0x01:
                        tRepeatOut = t_RepeatMark.END;
                        break;
                    case 0x08:
                        tRepeatOut = t_RepeatMark.X1;
                        break;
                    case 0x09:
                        tRepeatOut = t_RepeatMark.X2;
                        break;
                    case 0x0A:
                        tRepeatOut = t_RepeatMark.X3;
                        break;
                    case 0x0B:
                        tRepeatOut = t_RepeatMark.X4;
                        break;
                    case 0x0C:
                        tRepeatOut = t_RepeatMark.X5;
                        break;
                    case 0x0D:
                        tRepeatOut = t_RepeatMark.X6;
                        break;
                    case 0x0E:
                        tRepeatOut = t_RepeatMark.X7;
                        break;
                    case 0x0F:
                        tRepeatOut = t_RepeatMark.X8;
                        break;
                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch            
            }//if

            return erCodeRetVal;

        }//GetRepeatCommandParamsFromBytes

        /*******************************************************************************
        * @brief method that receives the parameters of a TIE command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] tRepeatIn
        * @param[out] _by0
        * @param[out] _by1
        * @param[out] _by2
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetTieCommandBytesFromParams(t_On_Off tOnOffIn, ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iBy2 = 0;

            // TIE COMMAND:
            // FIG. 10F-1
            // ON    OFF     
            // 8421  8421 
            // ----  ---- 
            // 0000  0000 TIE
            // 1010  1011 COMMAND
            // ----  ---- 
            // 0000  0000
            // 0000  0000 
            // ----  ---- 
            // 0000  0000  
            // 0000  0000 
            // ----  ---- 

            // encode the TIE_ON or TIE_OFF command
            switch (tOnOffIn) {
                case t_On_Off.OFF:
                    iBy0 = iBy0 | 0x0B;
                    break;
                case t_On_Off.ON:
                default:
                    iBy0 = iBy0 | 0x0A;
                    break;
            }//switch

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);
            _by2 = Convert.ToByte(iBy2);

            return erCodeRetVal;

        }//GetTieCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a TIE command an returns the command 
        * parameters that correspond to the TIE command encoded in the received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        * @param[out] tOnOffOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetTieCommandParamsFromBytes(byte _by0, byte _by1, byte _by2, ref t_On_Off tOnOffOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iBy2 = Convert.ToInt32(_by2);
            int iByAux = 0;

            // TIE COMMAND:
            // FIG. 10F-1
            // ON    OFF     
            // 8421  8421 
            // ----  ---- 
            // 0000  0000 TIE
            // 1010  1011 COMMAND
            // ----  ---- 
            // 0000  0000
            // 0000  0000 
            // ----  ---- 
            // 0000  0000  
            // 0000  0000 
            // ----  ---- 

            // encode the TIE_ON or TIE_OFF command
            if  ( (iBy0!=0x0A) && (iBy0 != 0x0B)) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }//if

            if (erCodeRetVal.i_code >= 0) {
                switch (iBy0) {
                    case 0x0B:
                        tOnOffOut = t_On_Off.OFF;
                        break;
                    case 0x0A:
                        tOnOffOut = t_On_Off.ON;
                        break;
                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch            
            }//if
            
            return erCodeRetVal;

        }//GetTieCommandParamsFromBytes

        /*******************************************************************************
        * @brief method that receives the parameters of a KEY command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] iKeySymbolIn
        * @param[out] _by0
        * @param[out] _by1
        * @param[out] _by2
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetKeyCommandBytesFromParams(int iKeySymbolIn, ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iBy2 = 0;

            // KEY SYMBOL COMMAND:
            // FIG. 10H
            // 8421 
            // ---- 
            // 1110 KEY SYMBOL
            // 0010 COMMAND
            // ---- 
            // xxxx L  KEY
            // xxxx U  SYMBOL
            // ---- 
            // 0000 NO CHORD  
            // 0000 
            // ---- 

            // encode the KEY command
            iBy0 = 0xE2;

            // encode the key symbol
            iBy1 = iKeySymbolIn;

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);
            _by2 = Convert.ToByte(iBy2);

            return erCodeRetVal;

        }//GetKeyCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a KEY command an returns the command 
        * parameters that correspond to the KEY command encoded in the received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        * @param[out] iKeySymbolOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetKeyCommandParamsFromBytes(byte _by0, byte _by1, byte _by2, ref int iKeySymbolOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iBy2 = Convert.ToInt32(_by2);
            int iByAux = 0;

            // KEY SYMBOL COMMAND:
            // FIG. 10H
            // 8421 
            // ---- 
            // 1110 KEY SYMBOL
            // 0010 COMMAND
            // ---- 
            // xxxx L  KEY
            // xxxx U  SYMBOL
            // ---- 
            // 0000 NO CHORD  
            // 0000 
            // ---- 

            // check the KEY command
            if (iBy0!=0xE2) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }

            if (erCodeRetVal.i_code >= 0) {
                // encode the key symbol
                iKeySymbolOut = iBy1;
            }//if

            return erCodeRetVal;

        }//GetKeyCommandParamsFromBytes

        /*******************************************************************************
        * @brief method that receives the parameters of a TIME command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] iTimeIn
        * @param[out] _by0
        * @param[out] _by1
        * @param[out] _by2
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetTimeCommandBytesFromParams(t_Time tTimeIn, ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iBy2 = 0;

            // TIME SYMBOL COMMAND:
            // FIG. 10G
            // 8421 
            // ---- 
            // 1110 TIME SYMBOL
            // 0001 COMMAND
            // ---- 
            // 0000 L  TIME    check FIG 14
            // 0000 U  SYMBOL
            // ---- 
            // 0000 NO CHORD  
            // 0000 
            // ---- 

            // encode the TIME command
            iBy0 = 0xE1;

            // encode the time symbol
            switch (tTimeIn) {
                case t_Time._16x16: iBy1 = 0x00; break;
                case t_Time._2x2:   iBy1 = 0x22; break;
                case t_Time._2x4:   iBy1 = 0x24; break;
                case t_Time._3x2:   iBy1 = 0x32; break;
                case t_Time._3x4:   iBy1 = 0x34; break;
                case t_Time._3x8:   iBy1 = 0x38; break;
                case t_Time._4x16:  iBy1 = 0x40; break;
                case t_Time._4x4:   iBy1 = 0x44; break;
                case t_Time._6x4:   iBy1 = 0x64; break;
                case t_Time._6x8:   iBy1 = 0x68; break;
                case t_Time._12x8:  iBy1 = 0xC8; break;                  
            }
            
            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);
            _by2 = Convert.ToByte(iBy2);

            return erCodeRetVal;

        }//GetTimeCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a TIME command an returns the command parameters  
        * that correspond to the TIME command encoded in the received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        * @param[out] tTimeOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetTimeCommandParamsFromBytes( byte _by0, byte _by1, byte _by2, ref t_Time tTimeOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iBy2 = Convert.ToInt32(_by2);

            // TIME SYMBOL COMMAND:
            // FIG. 10G
            // 8421 
            // ---- 
            // 1110 TIME SYMBOL
            // 0001 COMMAND
            // ---- 
            // 0000 L  TIME     check FIG 14
            // 0000 U  SYMBOL
            // ---- 
            // 0000 NO CHORD  
            // 0000 
            // ---- 

            // check the TIME command
            if (iBy0 != 0xE1) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }

            if (erCodeRetVal.i_code >= 0) {
                switch (iBy1) {
                    case 0x00: tTimeOut = t_Time._16x16; break;
                    case 0x22: tTimeOut = t_Time._2x2; break;
                    case 0x24: tTimeOut = t_Time._2x4; break;
                    case 0x32: tTimeOut = t_Time._3x2; break;
                    case 0x34: tTimeOut = t_Time._3x4; break;
                    case 0x38: tTimeOut = t_Time._3x8; break;
                    case 0x40: tTimeOut = t_Time._4x16; break;
                    case 0x44: tTimeOut = t_Time._4x4; break;
                    case 0x64: tTimeOut = t_Time._6x4; break;
                    case 0x68: tTimeOut = t_Time._6x8; break;
                    case 0xC8: tTimeOut = t_Time._12x8; break;
                }
            }

            return erCodeRetVal;

        }//GetTimeCommandParamsFromBytes

        /*******************************************************************************
           * @brief method that receives the parameters of a BAR command and 
           * returns the bytes that codify that command with the received parameters.
           * 
           * @param[out] _by0
           * @param[out] _by1
           * @param[out] _by2
           * 
           * @return >=0 the bytes of the command have been succesfully generated, <0 an  
           * error occurred while trying to obtain the bytes of the command.
           *******************************************************************************/
        public static ErrCode GetBarCommandBytesFromParams(ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iBy2 = 0;

            // BAR COMMAND:
            // FIG. 10I
            // 8421 
            // ---- 
            // 1110 BAR COMMAND
            // 0000
            // ---- 
            // 0000
            // 0000
            // ---- 
            // 0000
            // 0000 
            // ---- 

            // encode the BAR command
            iBy0 = 0xE0;

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);
            _by2 = Convert.ToByte(iBy2);

            return erCodeRetVal;

        }//GetBarCommandBytesFromParams

        /*******************************************************************************
        * @brief method that returns the bytes that codify the END command.
        * 
        * @param[out] _by0
        * @param[out] _by1
        * @param[out] _by2
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetEndCommandBytesFromParams(ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iBy2 = 0;

            // END COMMAND:
            // FIG. 10J
            // 8421 
            // ---- 
            // 0000 END COMMAND
            // 1111
            // ---- 
            // 0000
            // 0000
            // ---- 
            // 0000
            // 0000 
            // ---- 

            // encode the END command
            iBy0 = 0x0F;

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);
            _by2 = Convert.ToByte(iBy2);

            return erCodeRetVal;

        }//GetEndCommandBytesFromParams

        /*******************************************************************************
       * @brief method that receives the parameters of a DOUBLE_DURATION command and 
       * returns the bytes that codify that command with the received parameters.
       * 
       * @param[in] iDurIn
       * @param[in] iRestIn
       * @param[out] _by0
       * @param[out] _by1
       * @param[out] _by2
       * 
       * @return >=0 the bytes of the command have been succesfully generated, <0 an  
       * error occurred while trying to obtain the bytes of the command.
       *******************************************************************************/
        public static ErrCode Get2xDurationCommandBytesFromParams(int iDurIn, int iRestIn, ref byte _by0, ref byte _by1, ref byte _by2) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iBy2 = 0;

            // DOUBLE DURATION COMMAND:
            // FIG. 10A-2
            // 8421 
            // ---- 
            // 0000
            // 0010
            // ---- 
            // xxxx u1 TONE DURATION
            // xxxx u2 (UPPTER BIT)
            // ---- 
            // xxxx  u1 REST DURATION
            // xxxx  u2 (UPPTER BIT)
            // ---- 

            // set the DOUBLE_DURATION COMMAND
            iBy0 = 0x02;

            // encode the 2x note duration: the nibbles of the value must be swapped
            iBy1 = (byte)AuxFuncs.SwapByteNibbles((byte)iDurIn);

            // encode the 2x rest duration: the nibbles of the value must be swapped
            iBy2 = (byte)AuxFuncs.SwapByteNibbles((byte)iRestIn);

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);
            _by2 = Convert.ToByte(iBy2);

            return erCodeRetVal;

        }//Get2xDurationCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a DOUBLE_DURATION command an returns  
        * the command parameters that correspond to the DOUBLE_DURATION command encoded
        * in the received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[in] _by2
        * @param[out] iDurOut
        * @param[out] iRestOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode Get2xDurationCommandParamsFromBytes(byte _by0, byte _by1, byte _by2, ref int iDurOut, ref int iRestDurOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iBy2 = Convert.ToInt32(_by2);

            // DOUBLE DURATION COMMAND:
            // FIG. 10A-2
            // 8421 
            // ---- 
            // 0000
            // 0010
            // ---- 
            // xxxx u1 TONE DURATION
            // xxxx u2 (UPPTER BIT)
            // ---- 
            // xxxx  u1 REST DURATION
            // xxxx  u2 (UPPTER BIT)
            // ---- 

            // check that it is the REST_DURATION COMMAND
            if (iBy0 != 0x02) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }//if

            if (erCodeRetVal.i_code >= 0) {

                // decode the note 2x duration: the nibbles of the value are sotred swapped 
                iDurOut = (int)AuxFuncs.SwapByteNibbles((byte)iBy1);

                // decode the rest 2x duration: the nibbles of the value are sotred swapped 
                iRestDurOut = (int)AuxFuncs.SwapByteNibbles((byte)iBy2);

            }

            return erCodeRetVal;

        }//Get2xDurationCommandParamsFromBytes

    }//class MChannelCodeEntry

    // groups all the fields of a Chord entry
    public class ChordChannelCodeEntry {

        public enum t_Command {
            REST_DURATION,
            CHORD,
            REPEAT,
            RYTHM,
            TEMPO,
            COUNTER_RESET,
            END,
            DURATIONx2
        }

        public enum t_On_Off {
            ON,
            OFF
        }

        public enum t_Notes {
            C, Csh, D, Dsh, E, F, Fsh, G, Gsh, A, Ash, B
        }

        public enum t_ChordType {
            _MAJOR, _MINOR, _7TH, _m7, _M6, _6TH, _m7_6, _sus4, _dim, _aug, _m6, _7_5, _9th,_9, OFF, ON
        }

        public enum t_RythmMode {
            SET, FILL_IN, DISCRIMINATION
        }

        public enum t_RythmStyle {
            ROCK, DISCO, SWING_2_BEAT, SAMBA, BOSSA_NOVA, TANGO, SLOW_ROCK, WALTZ, ROCK_N_ROLL, x16_BEAT, SWING_4_BEAT, LATIN_ROCK, BEGUINE, MARCH, BALLAD, ROCK_WALTZ, LATING_SWING
        }

        public enum t_RepeatMark {
            START, END, X1, X2, X3, X4, X5, X6, X7, X8
        }

        int idx;
        public int Idx {
            get {
                return idx;
            }
            set {
                idx = value;
            }
        }//idx

        byte by0;
        public string By0 {
            get {
                // value is internally stored as a byte but it is delivered as the hex string representation
                return "0x" + by0.ToString("x2");
            }
            set {
                // value is received as the hex string representation of the value it is stored as a byte
                if (AuxFuncs.convertStringToByte(value, ref by0) < 0) {
                    by0 = 0;
                }
            }
        }//by0      

        byte by1;
        public string By1 {
            get {
                // value is internally stored as a byte but it is delivered as the hex string representation
                return "0x" + by1.ToString("x2");
            }
            set {
                // value is received as the hex string representation of the value it is stored as a byte
                if (AuxFuncs.convertStringToByte(value, ref by1) < 0) {
                    by0 = 0;
                }
            }
        }
        public string strDescr { get; set; }

        /*******************************************************************************
           * @brief Converts the received t_Command variable to a string equivalent
           * @param[in] t_Command the t_Command variable to convert to a string eqquivalent.
           * @return the string conversion of the received t_Command variable.
           *******************************************************************************/
        public static string tCommandToString(t_Command tCommandToConvert) {
            string str_aux = "";

            switch (tCommandToConvert) {
                case t_Command.REST_DURATION: str_aux = "rest duration"; break;
                case t_Command.CHORD: str_aux = "chord"; break;
                case t_Command.REPEAT: str_aux = "repeat"; break;
                case t_Command.RYTHM: str_aux = "rythm"; break;
                case t_Command.END: str_aux = "end"; break;
                case t_Command.TEMPO: str_aux = "tempo"; break;
                case t_Command.COUNTER_RESET: str_aux = "counter reset"; break;
                case t_Command.DURATIONx2: str_aux = "durationx2"; break;
            }

            return str_aux;

        }//tCommandToString

        /*******************************************************************************
        * @brief Converts the received string to the equivalent t_Command variable.
        * @param[in] tCommandToConvert with the string to convert to a t_Command 
        * variable.
        * @return the t_Command resulting of converting the received string.
        *******************************************************************************/
        public static t_Command strToCommand(string strTCommandToConvert) {
            t_Command tTCommandAux = new t_Command();

            strTCommandToConvert = strTCommandToConvert.Trim();
            strTCommandToConvert = strTCommandToConvert.ToLower();
            switch (strTCommandToConvert) {
                case "rest duration": tTCommandAux = t_Command.REST_DURATION; break;
                case "chord": tTCommandAux = t_Command.CHORD; break;
                case "repeat": tTCommandAux = t_Command.REPEAT; break;
                case "rythm": tTCommandAux = t_Command.RYTHM; break;
                case "end": tTCommandAux = t_Command.END; break;
                case "tempo": tTCommandAux = t_Command.TEMPO; break;
                case "counter reset": tTCommandAux = t_Command.COUNTER_RESET; break;
                case "durationx2": tTCommandAux = t_Command.DURATIONx2; break;
            }//switch

            return tTCommandAux;

        }//tOnOffToString

        /*******************************************************************************
         * @brief Converts the received t_On_Off variable to a string equivalent
         * @param[in] t_On_Off the tNote variable to convert to a string.
         * @return the string conversion of the received t_On_Off variable.
         *******************************************************************************/
        public static string tOnOffToString(t_On_Off tOnOffToConvert) {
            string str_aux = "";

            switch (tOnOffToConvert) {
                case t_On_Off.ON: str_aux = "on"; break;
                case t_On_Off.OFF: str_aux = "off"; break;
            }//switch

            return str_aux;

        }//tOnOffToString

        /*******************************************************************************
        * @brief Converts the received string to the equivalent t_On_Off variable.
        * @param[in] t_On_Off with the string to convert to a t_On_Off variable.
        * @return the t_On_Off resulting of converting the received string.
        *******************************************************************************/
        public static t_On_Off strToTOnOff(string strOnOffToConvert) {
            t_On_Off tOnOffAux = new t_On_Off();

            strOnOffToConvert = strOnOffToConvert.Trim();
            strOnOffToConvert = strOnOffToConvert.ToLower();
            switch (strOnOffToConvert) {
                case "on": tOnOffAux = t_On_Off.ON; break;
                case "off": tOnOffAux = t_On_Off.OFF; break;
            }//switch

            return tOnOffAux;

        }//strToTOnOff

        /*******************************************************************************
        * @brief Converts the received t_RythmMode variable to a string equivalent
        * @param[in] tRythmToConvert the t_RythmMode variable to convert to a string.
        * @return the string conversion of the received t_RythmMode variable.
        *******************************************************************************/
        public static string tRythmModeToString(t_RythmMode tRythmToConvert) {
            string str_aux = "";

            switch (tRythmToConvert) {

                case t_RythmMode.SET: str_aux = "set"; break;
                case t_RythmMode.FILL_IN: str_aux = "fill in"; break;
                case t_RythmMode.DISCRIMINATION: str_aux = "discrimination"; break;

            }//switch

            return str_aux;

        }//tRythmModeToString

        /*******************************************************************************
        * @brief Converts the received string to the equivalent t_RythmMode variable.
        * @param[in] strRythmModeToConvert with the string to convert to a t_RythmMode variable.
        * @return the t_RythmMode resulting of converting the received string.
        *******************************************************************************/
        public static t_RythmMode strToTRythmMode(string strRythmModeToConvert) {
            t_RythmMode tRythmModeAux = new t_RythmMode();

            strRythmModeToConvert = strRythmModeToConvert.Trim();
            strRythmModeToConvert = strRythmModeToConvert.ToLower();
            switch (strRythmModeToConvert) {

                case "set": tRythmModeAux = t_RythmMode.SET; break;
                case "fill in": tRythmModeAux = t_RythmMode.FILL_IN; break;
                case "discrimination": tRythmModeAux = t_RythmMode.DISCRIMINATION; break;

            }//switch

            return tRythmModeAux;

        }//strToTRythmMode

        /*******************************************************************************
        * @brief Converts the received t_RythmStyle variable to a string equivalent
        * @param[in] tRythmToConvert the t_RythmStyle variable to convert to a string.
        * @return the string conversion of the received t_RythmStyle variable.
        *******************************************************************************/
        public static string tRythmStyleToString(t_RythmStyle tRythmToConvert) {
            string str_aux = "";

            switch (tRythmToConvert) {

                case t_RythmStyle.ROCK: str_aux = "rock"; break;
                case t_RythmStyle.DISCO: str_aux = "disco"; break;
                case t_RythmStyle.SWING_2_BEAT: str_aux = "swing 2 beat"; break;
                case t_RythmStyle.SAMBA: str_aux = "samba"; break;
                case t_RythmStyle.BOSSA_NOVA: str_aux = "bossa nova"; break;
                case t_RythmStyle.TANGO: str_aux = "tango"; break;
                case t_RythmStyle.SLOW_ROCK: str_aux = "slow rock"; break;
                case t_RythmStyle.WALTZ: str_aux = "waltz"; break;
                case t_RythmStyle.ROCK_N_ROLL: str_aux = "rock n roll"; break;
                case t_RythmStyle.x16_BEAT: str_aux = "x16 beat"; break;
                case t_RythmStyle.SWING_4_BEAT: str_aux = "swing 4 beat"; break;
                case t_RythmStyle.LATIN_ROCK: str_aux = "latin rock"; break;
                case t_RythmStyle.BEGUINE: str_aux = "beguine"; break;
                case t_RythmStyle.MARCH: str_aux = "march"; break;
                case t_RythmStyle.BALLAD: str_aux = "ballad"; break;
                case t_RythmStyle.ROCK_WALTZ: str_aux = "rock waltz"; break;
                case t_RythmStyle.LATING_SWING: str_aux = "latin swing"; break;

            }//switch

            return str_aux;

        }//tRythmStyleToString

        /*******************************************************************************
        * @brief Converts the received string to the equivalent t_RythmStyle variable.
        * @param[in] strRythmStyleToConvert with the string to convert to a t_RythmStyle variable.
        * @return the t_RythmStyle resulting of converting the received string.
        *******************************************************************************/
        public static t_RythmStyle strToTRythmStyle(string strRythmStyleToConvert) {
            t_RythmStyle tRythmStyleAux = new t_RythmStyle();

            strRythmStyleToConvert = strRythmStyleToConvert.Trim();
            strRythmStyleToConvert = strRythmStyleToConvert.ToLower();
            switch (strRythmStyleToConvert) {

                case "rock": tRythmStyleAux = t_RythmStyle.ROCK; break;
                case "disco": tRythmStyleAux = t_RythmStyle.DISCO; break;
                case "swing 2 beat": tRythmStyleAux = t_RythmStyle.SWING_2_BEAT; break;
                case "samba": tRythmStyleAux = t_RythmStyle.SAMBA; break;
                case "bossa nova": tRythmStyleAux = t_RythmStyle.BOSSA_NOVA; break;
                case "tango": tRythmStyleAux = t_RythmStyle.TANGO; break;
                case "slow rock": tRythmStyleAux = t_RythmStyle.SLOW_ROCK; break;
                case "waltz": tRythmStyleAux = t_RythmStyle.WALTZ; break;
                case "rock n roll": tRythmStyleAux = t_RythmStyle.ROCK_N_ROLL; break;
                case "x16 beat": tRythmStyleAux = t_RythmStyle.x16_BEAT; break;
                case "swing 4 beat": tRythmStyleAux = t_RythmStyle.SWING_4_BEAT; break;
                case "latin rock": tRythmStyleAux = t_RythmStyle.LATIN_ROCK; break;
                case "beguine": tRythmStyleAux = t_RythmStyle.BEGUINE; break;
                case "march": tRythmStyleAux = t_RythmStyle.MARCH; break;
                case "ballad": tRythmStyleAux = t_RythmStyle.BALLAD; break;
                case "rock waltz": tRythmStyleAux = t_RythmStyle.ROCK_WALTZ; break;
                case "latin swing": tRythmStyleAux = t_RythmStyle.LATING_SWING; break;

            }//switch

            return tRythmStyleAux;

        }//strToTRythmStyle

        /*******************************************************************************
        * @brief Converts the received t_Notes variable to a string equivalent
        * @param[in] tNoteToConvert the tNote variable to convert to a string.
        * @return the string conversion of the received t_Notes variable.
        *******************************************************************************/
        public static string tNotesToString(t_Notes tNoteToConvert) {
            string str_aux = "";

            switch (tNoteToConvert) {

                case t_Notes.C: str_aux = "c"; break;
                case t_Notes.Csh: str_aux = "c#"; break;
                case t_Notes.D: str_aux = "d"; break;
                case t_Notes.Dsh: str_aux = "d#"; break;
                case t_Notes.E: str_aux = "e"; break;
                case t_Notes.F: str_aux = "f"; break;
                case t_Notes.Fsh: str_aux = "f#"; break;
                case t_Notes.G: str_aux = "g"; break;
                case t_Notes.Gsh: str_aux = "g#"; break;
                case t_Notes.A: str_aux = "a"; break;
                case t_Notes.Ash: str_aux = "a#"; break;
                case t_Notes.B: str_aux = "b"; break;

            }//switch

            return str_aux;

        }//tNotesToString

        /*******************************************************************************
         * @brief Converts the received string to the equivalent t_Note variable.
         * @param[in] tNoteToConvert with the string to convert to a tNote variable.
         * @return the t_Note resulting of converting the received string.
         *******************************************************************************/
        public static t_Notes strToTNote(string strNoteToConvert) {
            t_Notes tNoteAux = new t_Notes();

            strNoteToConvert = strNoteToConvert.Trim();
            strNoteToConvert = strNoteToConvert.ToLower();
            switch (strNoteToConvert) {

                case "c": tNoteAux = t_Notes.C; break;
                case "c#": tNoteAux = t_Notes.Csh; break;
                case "d": tNoteAux = t_Notes.D; break;
                case "d#": tNoteAux = t_Notes.Dsh; break;
                case "e": tNoteAux = t_Notes.E; break;
                case "f": tNoteAux = t_Notes.F; break;
                case "f#": tNoteAux = t_Notes.Fsh; break;
                case "g": tNoteAux = t_Notes.G; break;
                case "g#": tNoteAux = t_Notes.Gsh; break;
                case "a": tNoteAux = t_Notes.A; break;
                case "a#": tNoteAux = t_Notes.Ash; break;
                case "b": tNoteAux = t_Notes.B; break;

            }//switch

            return tNoteAux;

        }//strToTNote

        /*******************************************************************************
        * @brief Converts the received t_ChordType variable to a string equivalent
        * @param[in] t_ChordType the tNote variable to convert to a string.
        * @return the string conversion of the received t_ChordType variable.
        *******************************************************************************/
        public static string tChordTypeToString(t_ChordType tChordTypeToConvert) {
            string str_aux = "";

            switch (tChordTypeToConvert) {

                case t_ChordType._MAJOR: str_aux = "major"; break;
                case t_ChordType._MINOR: str_aux = "minor"; break;
                case t_ChordType._7TH: str_aux = "7th"; break;
                case t_ChordType._m7: str_aux = "m7"; break;
                case t_ChordType._M6: str_aux = "m6"; break;
                case t_ChordType._6TH: str_aux = "6th"; break;
                case t_ChordType._m7_6: str_aux = "m7_6"; break;
                case t_ChordType._sus4: str_aux = "sus4"; break;
                case t_ChordType._dim: str_aux = "dim"; break;
                case t_ChordType._aug: str_aux = "aug"; break;
                case t_ChordType._m6: str_aux = "m6"; break;
                case t_ChordType._7_5: str_aux = "7_5"; break;
                case t_ChordType._9th: str_aux = "9th"; break;
                case t_ChordType._9: str_aux = "9"; break;
                case t_ChordType.ON: str_aux = "on"; break;
                case t_ChordType.OFF: str_aux = "off"; break;

            }//switch

            return str_aux;

        }//tChordTypeToString

        /*******************************************************************************
         * @brief Converts the received string to the equivalent t_ChordType 
         * variable.
         * @param[in] t_ChordTypeToStringToConvert with the string to convert to a 
         * t_ChordType variable.
         * @return the t_ChordType resulting of converting the received string.
         *******************************************************************************/
        public static t_ChordType strToTChordType(string strNoteToConvert) {
            t_ChordType tChordTypeAux = new t_ChordType();

            strNoteToConvert = strNoteToConvert.Trim();
            strNoteToConvert = strNoteToConvert.ToLower();
            switch (strNoteToConvert) {

                case "major": tChordTypeAux = t_ChordType._MAJOR; break;
                case "minor": tChordTypeAux = t_ChordType._MINOR; break;
                case "7th": tChordTypeAux = t_ChordType._7TH; break;
                case "m7": tChordTypeAux = t_ChordType._m7; break;
                case "m6": tChordTypeAux = t_ChordType._M6; break;
                case "6th": tChordTypeAux = t_ChordType._6TH; break;
                case "m7_6": tChordTypeAux = t_ChordType._m7_6; break;
                case "sus4": tChordTypeAux = t_ChordType._sus4; break;
                case "dim": tChordTypeAux = t_ChordType._dim; break;
                case "aug": tChordTypeAux = t_ChordType._aug; break;
                // case "m6": tNoteAux = t_ChordType._m6; break;  JBR 2024-06-27 A ver como distinguimos de M6 y m6 pues al hacer toLower se convierten en lo mismo
                case "7_5": tChordTypeAux = t_ChordType._7_5; break;
                case "9th": tChordTypeAux = t_ChordType._9th; break;
                case "9": tChordTypeAux = t_ChordType._9; break;
                case "on": tChordTypeAux = t_ChordType.ON; break;
                case "off": tChordTypeAux = t_ChordType.OFF; break;

            }//switch

            return tChordTypeAux;

        }//strToTChordType

        /*******************************************************************************
         * @brief Converts the received t_RepeatMark variable to a string equivalent
         * @param[in] tRMarkToConvert the t_RepeatMark variable to convert to a string.
         * @return the string conversion of the received t_RepeatMark variable.
         *******************************************************************************/
        public static string tRepeatMarkToString(t_RepeatMark tRMarkToConvert) {
            string str_aux = "";

            switch (tRMarkToConvert) {
                case t_RepeatMark.START: str_aux = "start"; break;
                case t_RepeatMark.END: str_aux = "end"; break;
                case t_RepeatMark.X1: str_aux = "x1"; break;
                case t_RepeatMark.X2: str_aux = "x2"; break;
                case t_RepeatMark.X3: str_aux = "x3"; break;
                case t_RepeatMark.X4: str_aux = "x4"; break;
                case t_RepeatMark.X5: str_aux = "x5"; break;
                case t_RepeatMark.X6: str_aux = "x6"; break;
                case t_RepeatMark.X7: str_aux = "x7"; break;
                case t_RepeatMark.X8: str_aux = "x8"; break;
            }//switch

            return str_aux;

        }//t_RepeatMarkToString

        /*******************************************************************************
        * @brief Converts the received string to the equivalent t_RepeatMark variable.
        * @param[in] strRMarkToConvert string with the string to convert to a t_RepeatMark 
        * variable.
        * @return the t_RepeatMark resulting of converting the received string.
        *******************************************************************************/
        public static t_RepeatMark strToTRepeatMark(string strRMarkToConvert) {
            t_RepeatMark tRepeatAux = new t_RepeatMark();

            strRMarkToConvert = strRMarkToConvert.Trim();
            strRMarkToConvert = strRMarkToConvert.ToLower();
            switch (strRMarkToConvert) {
                case "end": tRepeatAux = t_RepeatMark.END; break;
                case "x1": tRepeatAux = t_RepeatMark.X1; break;
                case "x2": tRepeatAux = t_RepeatMark.X2; break;
                case "x3": tRepeatAux = t_RepeatMark.X3; break;
                case "x4": tRepeatAux = t_RepeatMark.X4; break;
                case "x5": tRepeatAux = t_RepeatMark.X5; break;
                case "x6": tRepeatAux = t_RepeatMark.X6; break;
                case "x7": tRepeatAux = t_RepeatMark.X7; break;
                case "x8": tRepeatAux = t_RepeatMark.X8; break;
            }//switch

            return tRepeatAux;

        }//strToTRepeatMark

        /*******************************************************************************
        * @brief Default Chord Channel Instruction constructor
        *******************************************************************************/
        public ChordChannelCodeEntry() {

            idx = 0;
            by0 = 0x00;
            by1 = 0x00;
            strDescr = "";

        }//ChordChannelCodeEntry

        /*******************************************************************************
        * @brief Chord Channel Instruction Constructor with parameters
        * @param[in] _idx position of that instruction in the whole channel instructions list 
        * @param[in] _by0
        * @param[in] _by1
        *******************************************************************************/
        public ChordChannelCodeEntry(int _idx, byte _by0, byte _by1) {
            
            idx = _idx;
            by0 = _by0;
            by1 = _by1;
            strDescr = "";

        }//ChordChannelCodeEntry

        /*******************************************************************************
        * @brief Chord Channel Instruction constructor with parameters
        * @param[in] _idx position of that instruction in the whole channel instructions list 
        * @param[in] _by0
        * @param[in] _by1
        * @paran[in] _strDescr description of the created Chord Channel Instruction
        *******************************************************************************/
        public ChordChannelCodeEntry(int _idx, byte _by0, byte _by1, byte _by2, string _strDescr) {

            idx = _idx;
            by0 = _by0;
            by1 = _by1;
            strDescr = _strDescr;

        }//ChordChannelCodeEntry

        /*******************************************************************************
         * Returns the Command Type that is encoded in the ChordChannelCodeEntry instruction
         * bytes.
         * 
         * @return >=0 returns the Command type encoded in the instruction bytes
         *******************************************************************************/
        public t_Command GetCmdType() {
            ChordChannelCodeEntry.t_Command tCmdAux = t_Command.CHORD;
            byte byAux = 0;
            byte byAux2 = 0;


            // REST DURATION COMMAND
            // FIG. 
            // 8421 
            // ---- 
            // 0000  REST DURATION   
            // 0001  COMMAND
            // ---- 
            // ....  REST DURATION  
            // ....  
            // ---- 
            if (by0 == 0x01) {
                tCmdAux = t_Command.REST_DURATION;
            }//if

            // NOTE COMMAND
            // FIG. 11A-1
            // 8421 
            // ---- 
            // ....  SC ROOT     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
            // ....  OC NAME     OC=4-bit octave code.    octave code for the melody line.
            // ----  
            // ....  L1 CHORD
            // ....  L2 DURATION
            // ---- 
            byAux = (byte)((by0 & 0xf0) >> 4);
            if ((byAux >= 0x1) && (byAux <= 0xC)) {
                tCmdAux = t_Command.CHORD;

            }//if

            // REPEAT COMMAND:
            // FIG. 11C-1
            // 8421 
            // ---- 
            // 1111 REPEAT 
            // xxxx COMMAND 0=Begining mark, 1=End mark, 8=Repeat x1, 9=Repeat x2, A=Repeat x3, B=Repeat x4, C=Repeat x5, D=Repeat x6, E=Repeat x7, F=Repeat x8
            // ----  
            // 0000  
            // 0000 
            // ---- 
            byAux = (byte)((by0 & 0xf0) >> 4);
            if ((byAux == 0xf) && (by1 == 0x00)) {
                tCmdAux = t_Command.REPEAT;
            }//if

            // RYTHM  COMMAND:
            // FIG. 11D-1
            // ON   OFF
            // 8421 8421 
            // ---- ---- 
            // 0000 0000 RIFIR-D COMMAND
            // xxxx xxxx  
            // ---- ----  
            // xxxx xxxx  RYTHM  0x00=rock, 0x01=disco, 0x02=swing 2 beat, 0x03=samba, 0x04=bossa nova, 0x05=tango, 0x06=slow rock, 0x07=waltz, 0x10=rock'n roll, 0x11=16 beat, 0x12=swing 4 beat, 0x13=latin rock, 0x14=beguine, 0x15=march, 0x16=ballad, 0x17=rock waltz, 0x22=latin swing
            // 0xxx 1xxx  DATA
            // ---- ---- 
            byAux = (byte)((by0 & 0xf0) >> 4);
            byAux2 = (byte)(by0 & 0x0f);
            if ((byAux == 0x0) && ((byAux2 == 0x5) || (byAux2 == 0x6) || (byAux2 == 0x8))) {
                tCmdAux = t_Command.RYTHM;
            }//if

            // TEMPO  COMMAND:
            // FIG. 11E-1
            // ON   OFF
            // 8421 8421 
            // ---- ---- 
            // 0000 0000 TEMPO COMMAND
            // 1100 1100
            // ---- ----   
            // xxxx xxxx SC  TEMPO DATA: check FIG 16
            // 0xxx 1xxx OC
            // ---- ---- 
            if (by0 == 0x0C) {
                tCmdAux = t_Command.TEMPO;
            }

            // COUNTER RESET:
            if (by0 == 0x09) { 
                tCmdAux = t_Command.COUNTER_RESET;
            }

            // END COMMAND:
            // FIG. 10J
            // 8421 
            // ---- 
            // 0000 END COMMAND
            // 1111
            // ---- 
            // 0000
            // 0000
            // ---- 
            if (by0 == 0x0F) {
                tCmdAux = t_Command.END;
            }

            // DOUBLE DURATION COMMAND:
            // FIG. 11A-2
            // 8421 
            // ---- 
            // 0000
            // 0010
            // ---- 
            // xxxx  Duration
            // xxxx
            // ---- 
            if (by0 == 0x02) {
                tCmdAux = t_Command.DURATIONx2;            
            }

            return tCmdAux;

        }//GetCmdType

        /*******************************************************************************
        * @brief Returns the value of the B0 field of the Chord Channel Instruction in 
        * byte format.
        *******************************************************************************/
        public byte By0AsByte() {

            return by0;

        }//By0AsByte

        /*******************************************************************************
        * @brief Returns the value of the B1 field of the Chord Channel Instruction in 
        * byte format.
        *******************************************************************************/
        public byte By1AsByte() {

            return by1;

        }//By0AsByte

        /*******************************************************************************
        * @brief method that analyzes the bytes of the chord instruction and updates the
        * instruction description field with an explanation of the instruction in that bytes.
        * @return >=0 file has been succesfully loaded into the object, <0 an error 
        * occurred 
        *******************************************************************************/
        public ErrCode Parse() {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR;
            string strComentSubstring = "";
            int iAux = 0;
            byte byAux = 0x00;
            byte byAux2 = 0x00;
            int iCommentIdx = -1;

            // check the position of the comment symbol and take its position if exists
            if ((strDescr != null) && (strDescr.Length >= 2)) {
                iCommentIdx = strDescr.Trim().IndexOf("//");
            }

            if (iCommentIdx == 0) {

                // when the comment symbol is in the first position then the description
                // of the instruction is not parsed and only the comment is shown
                erCodeRetVal = cErrCodes.ERR_NO_ERROR;

            } else {

                // if the comment is behind the instruction description then it will be kept
                // and then added at the end after parsing the instruction.
                if (iCommentIdx > 0) {
                    strComentSubstring = strDescr.Substring(iCommentIdx, strDescr.Length - iCommentIdx);
                }

                strDescr = "";

                // REST DURATION COMMAND
                // FIG. 
                // 8421 
                // ---- 
                // 0000  REST DURATION   
                // 0001  COMMAND
                // ---- 
                // ....  REST DURATION  
                // ....  
                // ---- 
                if (by0 == 0x01) {

                    strDescr = "rest duration:";
                    strDescr = strDescr + "" + AuxFuncs.SwapByteNibbles(by1).ToString("D3");// as high and low nibbles are inverted in the value they must be swapped

                }//if

                // NOTE COMMAND
                // FIG. 11A-1
                // 8421 
                // ---- 
                // ....  SC ROOT     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
                // ....  OC NAME     OC=4-bit octave code.    octave code for the melody line.
                // ----  
                // ....  L1 CHORD
                // ....  L2 DURATION
                // ----

                //           ROOT (SC)    CHORD NAME
                // 0 | 0000 |    --    |   MAJOR     |
                // 1 | 0001 |    C     |   MINOR     |
                // 2 | 0010 |    C#    |   7th       |
                // 3 | 0011 |    D     |   m7        |
                // 4 | 0100 |    D#    |   M6        |
                // 5 | 0101 |    E     |   6 TH      |
                // 6 | 0110 |    F     |   m7-5      |
                // 7 | 0111 |    F#    |   SUS-4     |
                // 8 | 1000 |    G     |   DIM       |
                // 9 | 1001 |    G#    |   AUG       |
                // A | 1010 |    A     |   m6        |
                // B | 1011 |    A#    |   7-5       |
                // C | 1100 |    B     |   9 TH      |
                // D | 1101 |    --    |    9        |
                // E | 1110 |    --    |  OFF CHORD  |
                // F | 1111 |    --    |  MON CHORD  |

                byAux = (byte)((by0 & 0xf0) >> 4);
                if ((byAux >= 0x1) && (byAux <= 0xC)) {

                    strDescr = "chord:";
                    switch (byAux) {
                        case 0x1:
                            strDescr = strDescr + "c";
                            break;
                        case 0x2:
                            strDescr = strDescr + "c#";
                            break;
                        case 0x3:
                            strDescr = strDescr + "d";
                            break;
                        case 0x4:
                            strDescr = strDescr + "d#";
                            break;
                        case 0x5:
                            strDescr = strDescr + "e";
                            break;
                        case 0x6:
                            strDescr = strDescr + "f";
                            break;
                        case 0x7:
                            strDescr = strDescr + "f#";
                            break;
                        case 0x8:
                            strDescr = strDescr + "g";
                            break;
                        case 0x9:
                            strDescr = strDescr + "g#";
                            break;
                        case 0xA:
                            strDescr = strDescr + "a";
                            break;
                        case 0xB:
                            strDescr = strDescr + "a#";
                            break;
                        case 0xC:
                            strDescr = strDescr + "b";
                            break;
                        default:
                            strDescr = strDescr + "¿" + byAux.ToString() + "?";
                            break;
                    }//if

                    byAux2 = (byte)(by0 & 0x0f);
                    strDescr = strDescr + " ";
                    switch (byAux2) {
                        case 0x0:
                            strDescr = strDescr + "major";
                            break;
                        case 0x1:
                            strDescr = strDescr + "minor";
                            break;
                        case 0x2:
                            strDescr = strDescr + "7th";
                            break;
                        case 0x3:
                            strDescr = strDescr + "m7";
                            break;
                        case 0x4:
                            strDescr = strDescr + "M6";
                            break;
                        case 0x5:
                            strDescr = strDescr + "6th";
                            break;
                        case 0x6:
                            strDescr = strDescr + "m7-6";
                            break;
                        case 0x7:
                            strDescr = strDescr + "sus4";
                            break;
                        case 0x8:
                            strDescr = strDescr + "dim";
                            break;
                        case 0x9:
                            strDescr = strDescr + "aug";
                            break;
                        case 0xA:
                            strDescr = strDescr + "m6";
                            break;
                        case 0xB:
                            strDescr = strDescr + "7-5";
                            break;
                        case 0xC:
                            strDescr = strDescr + "9th";
                            break;
                        case 0xD:
                            strDescr = strDescr + "9";
                            break;
                        case 0xE:
                            strDescr = strDescr + " off";
                            break;
                        case 0xF:
                            strDescr = strDescr + " on";
                            break;
                        default:
                            strDescr = strDescr + "¿" + byAux.ToString() + "?";
                            break;
                    }//if
                    strDescr = strDescr + " dur:" + AuxFuncs.SwapByteNibbles(by1).ToString("D3");// as high and low nibbles are inverted in the value they must be swapped

                }//if

                // REPEAT COMMAND:
                // FIG. 11C-1
                // 8421 
                // ---- 
                // 1111 REPEAT 
                // xxxx COMMAND 0=Begining mark, 1=End mark, 8=Repeat x1, 9=Repeat x2, A=Repeat x3, B=Repeat x4, C=Repeat x5, D=Repeat x6, E=Repeat x7, F=Repeat x8
                // ----  
                // 0000  
                // 0000 
                // ---- 
                byAux = (byte)((by0 & 0xf0) >> 4);
                if ((byAux == 0xf) && (by1 == 0x00)) {

                    strDescr = "repeat:";
                    byAux2 = (byte)(by0 & 0x0f);
                    switch (byAux2) {
                        case 0x0:
                            strDescr = strDescr + "start mark";
                            break;
                        case 0x1:
                            strDescr = strDescr + "end mark";
                            break;
                        case 0x8:
                            strDescr = strDescr + "x1";
                            break;
                        case 0x9:
                            strDescr = strDescr + "x2";
                            break;
                        case 0xA:
                            strDescr = strDescr + "x3";
                            break;
                        case 0xB:
                            strDescr = strDescr + "x4";
                            break;
                        case 0xC:
                            strDescr = strDescr + "x5";
                            break;
                        case 0xD:
                            strDescr = strDescr + "x6";
                            break;
                        case 0xE:
                            strDescr = strDescr + "x7";
                            break;
                        case 0xF:
                            strDescr = strDescr + "x8";
                            break;
                        default:
                            strDescr = strDescr + "¿" + byAux2.ToString() + "?";
                            break;
                    }//switch

                }//if

                // RYTHM  COMMAND:
                // FIG. 11D-1
                // ON   OFF
                // 8421 8421 
                // ---- ---- 
                // 0000 0000 RIFIR-D COMMAND
                // xxxx xxxx  
                // ---- ----  
                // xxxx xxxx  RYTHM  0x00=rock, 0x01=disco, 0x02=swing 2 beat, 0x03=samba, 0x04=bossa nova, 0x05=tango, 0x06=slow rock, 0x07=waltz, 0x10=rock'n roll, 0x11=16 beat, 0x12=swing 4 beat, 0x13=latin rock, 0x14=beguine, 0x15=march, 0x16=ballad, 0x17=rock waltz, 0x22=latin swing
                // 0xxx 1xxx  DATA
                // ---- ---- 
                byAux = (byte)((by0 & 0xf0) >> 4);
                byAux2 = (byte)(by0 & 0x0f);
                if ((byAux == 0x0) && ((byAux2 == 0x5) || (byAux2 == 0x6) || (byAux2 == 0x8))) {

                    strDescr = "rtyhm mode:";
                    byAux2 = (byte)(by0 & 0x0f);
                    switch (byAux2) {
                        case 0x5:
                            strDescr = strDescr + "set";
                            break;
                        case 0x6:
                            strDescr = strDescr + "fill-in";
                            break;
                        case 0x8:
                            strDescr = strDescr + "discrimination";
                            break;
                        case 0x9:

                        default:
                            strDescr = strDescr + "¿" + byAux2.ToString() + "?";
                            break;
                    }//switch

                    strDescr = strDescr + " style:";
                    byAux2 = (byte)(by1 & 0x08);
                    if (byAux2 == 0x08) {
                        strDescr = strDescr + "off:";
                    } else {
                        strDescr = strDescr + "on:";
                    }

                    byAux2 = (byte)(by1 & 0xF7);
                    switch (by1 & byAux2) {
                        case 0x00:
                            strDescr = strDescr + "rock";
                            break;
                        case 0x01:
                            strDescr = strDescr + "disco";
                            break;
                        case 0x02:
                            strDescr = strDescr + "swing 2 beat";
                            break;
                        case 0x03:
                            strDescr = strDescr + "samba";
                            break;
                        case 0x04:
                            strDescr = strDescr + "bossa nova";
                            break;
                        case 0x05:
                            strDescr = strDescr + "tango";
                            break;
                        case 0x06:
                            strDescr = strDescr + "slow rock";
                            break;
                        case 0x07:
                            strDescr = strDescr + "waltz";
                            break;
                        case 0x10:
                            strDescr = strDescr + "rock'n roll";
                            break;
                        case 0x11:
                            strDescr = strDescr + "16 beat";
                            break;
                        case 0x12:
                            strDescr = strDescr + "swing 4 beat";
                            break;
                        case 0x13:
                            strDescr = strDescr + "latin rock";
                            break;
                        case 0x14:
                            strDescr = strDescr + "beguine";
                            break;
                        case 0x15:
                            strDescr = strDescr + "march";
                            break;
                        case 0x16:
                            strDescr = strDescr + "ballad";
                            break;
                        case 0x17:
                            strDescr = strDescr + "rock waltz";
                            break;
                        case 0x22:
                            strDescr = strDescr + "latin swing";
                            break;
                        default:
                            strDescr = strDescr + "¿" + byAux2.ToString() + "?";
                            break;
                    }//switch

                }//if

                // TEMPO  COMMAND:
                // FIG. 11E-1
                // ON   OFF
                // 8421 8421 
                // ---- ---- 
                // 0000 0000 TEMPO COMMAND
                // 1100 1100
                // ---- ----   
                // xxxx xxxx SC  TEMPO DATA: check FIG 16
                // 0xxx 1xxx OC
                // ---- ---- 
                if (by0 == 0x0C) {

                    // tempo is encoded as: xxxx ayyy (  check FIG16 ):
                    // tempo = (yyyxxxx+1) * 3  ==> (tempo/3) - 1 = yyyxxx
                    strDescr = "tempo:";
                    // take the tempo on/off bit: 'a'
                    byAux = (byte)(by1 & 0x08);
                    if (byAux == 0x08) {
                        strDescr = strDescr + "off:";
                    } else {
                        strDescr = strDescr + "on:";
                    }
                    // take the tempo value 
                    // tempo = ('yyyxxxx'+1) * 3  ==> (tempo/3) - 1 = yyyxxx
                    iAux = ((by1 & 0x07) << 4);
                    iAux = iAux | ((by1 & 0xF0) >> 4);
                    iAux = (iAux + 1) * 3;

                    strDescr = strDescr + "" +(iAux).ToString("D3");

                }//if

                // COUNTER RESET:
                if (by0 == 0x09) {
                    strDescr = "counter reset";
                }

                // END COMMAND:
                // FIG. 10J
                // 8421 
                // ---- 
                // 0000 END COMMAND
                // 1111
                // ---- 
                // 0000
                // 0000
                // ---- 
                if (by0 == 0x0F) {
                    strDescr = "end";
                }

                // DOUBLE DURATION COMMAND:
                // FIG. 11A-2
                // 8421 
                // ---- 
                // 0000
                // 0010
                // ---- 
                // xxxx  Duration
                // xxxx
                // ---- 
                if (by0 == 0x02) {
                    strDescr = "2xduration";
                    strDescr = strDescr + " dur:" + AuxFuncs.SwapByteNibbles(by1).ToString("D3");// as high and low nibbles are inverted in the value they must be swapped
                }

                // add the instruction comment in case there is something
                strDescr = strDescr + strComentSubstring;

            }//   if (iCommentIdx == 0)

            return erCodeRetVal;

        }//Parse

        /*******************************************************************************
        * @brief method that receives the parameters of a REST_DURATION command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] iRestIn
        * @param[out] _by0
        * @param[out] _by1
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetRestCommandBytesFromParams(int iRestIn, ref byte _by0, ref byte _by1) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;

            // REST DURATION COMMAND
            // FIG. 
            // 8421 
            // ---- 
            // 0000  REST DURATION   
            // 0001  COMMAND
            // ---- 
            // ....  REST DURATION  
            // ....  
            // ---- 

            // set the REST_DURATION COMMAND
            iBy0 = 0x01;

            // encode the rest duration: the nibbles of the value must be swapped
            iBy1 = (byte)AuxFuncs.SwapByteNibbles((byte)iRestIn);

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);

            return erCodeRetVal;

        }//GetRestCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a REST_DURATION command an returns
        * the parameters that correspond to the REST_DURATION command in encoded in the
        * received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[out] iRestOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetRestCommandParamsFromBytes(byte by0, byte by1, ref int iRestOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(by0);
            int iBy1 = Convert.ToInt32(by1);


            // REST DURATION COMMAND
            // FIG. 
            // 8421 
            // ---- 
            // 0000  REST DURATION   
            // 0001  COMMAND
            // ---- 
            // ....  REST DURATION  
            // ....  
            // ---- 

            // check the REST_DURATION command
            if (iBy0 != 0x01) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }

            if (erCodeRetVal.i_code >= 0) {
                // decode the rest duration: the nibbles of the value are sotred swapped 
                iRestOut = (int)AuxFuncs.SwapByteNibbles((byte)iBy1);
            }

            return erCodeRetVal;

        }//GetRestCommandParamsFromBytes


        /*******************************************************************************
        * @brief method that receives the parameters of a CHORD command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] tNoteIn
        * @param[in] tChordTypeIn
        * @param[in] iRestIn
        * @param[out] _by0
        * @param[out] _by1
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetChordCommandBytesFromParams(t_Notes tNoteIn, t_ChordType tChordTypeIn, int iRestIn,ref byte _by0, ref byte _by1) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;

            // NOTE COMMAND
            // FIG. 11A-1
            // 8421 
            // ---- 
            // ....  SC ROOT     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
            // ....  OC NAME     OC=4-bit octave code.    octave code for the melody line.
            // ----  
            // ....  L1 CHORD
            // ....  L2 DURATION
            // ---- 

            //           ROOT (SC)    CHORD NAME
            // 0 | 0000 |    --    |   MAJOR     |
            // 1 | 0001 |    C     |   MINOR     |
            // 2 | 0010 |    C#    |   7th       |
            // 3 | 0011 |    D     |   m7        |
            // 4 | 0100 |    D#    |   M6        |
            // 5 | 0101 |    E     |   6 TH      |
            // 6 | 0110 |    F     |   m7-5      |
            // 7 | 0111 |    F#    |   SUS-4     |
            // 8 | 1000 |    G     |   DIM       |
            // 9 | 1001 |    G#    |   AUG       |
            // A | 1010 |    A     |   m6        |
            // B | 1011 |    A#    |   7-5       |
            // C | 1100 |    B     |   9 TH      |
            // D | 1101 |    --    |    9        |
            // E | 1110 |    --    |  OFF CHORD  |
            // F | 1111 |    --    |  MON CHORD  |

            // encode the CHORD NOTE code
            switch (tNoteIn) {
                case t_Notes.C:
                    iBy0 = iBy0 | 0x10;
                    break;
                case t_Notes.Csh:
                    iBy0 = iBy0 | 0x20;
                    break;
                case t_Notes.D:
                    iBy0 = iBy0 | 0x30;
                    break;
                case t_Notes.Dsh:
                    iBy0 = iBy0 | 0x40;
                    break;
                case t_Notes.E:
                    iBy0 = iBy0 | 0x50;
                    break;
                case t_Notes.F:
                    iBy0 = iBy0 | 0x60;
                    break;
                case t_Notes.Fsh:
                    iBy0 = iBy0 | 0x70;
                    break;
                case t_Notes.G:
                    iBy0 = iBy0 | 0x80;
                    break;
                case t_Notes.Gsh:
                    iBy0 = iBy0 | 0x90;
                    break;
                case t_Notes.A:
                    iBy0 = iBy0 | 0xA0;
                    break;
                case t_Notes.Ash:
                    iBy0 = iBy0 | 0xB0;
                    break;
                case t_Notes.B:
                    iBy0 = iBy0 | 0xC0;
                    break;
                default:
                    break;
            }//switch

            // encode the CHORD TYPE code
            switch (tChordTypeIn) {
                case t_ChordType._MAJOR:
                    iBy0 = iBy0 | 0x00;
                    break;
                case t_ChordType._MINOR:
                    iBy0 = iBy0 | 0x01;
                    break;
                case t_ChordType._7TH:
                    iBy0 = iBy0 | 0x02;
                    break;
                case t_ChordType._m7:
                    iBy0 = iBy0 | 0x03;
                    break;
                case t_ChordType._M6:
                    iBy0 = iBy0 | 0x04;
                    break;
                case t_ChordType._6TH:
                    iBy0 = iBy0 | 0x05;
                    break;
                case t_ChordType._m7_6:
                    iBy0 = iBy0 | 0x06;
                    break;
                case t_ChordType._sus4:
                    iBy0 = iBy0 | 0x07;
                    break;
                case t_ChordType._dim:
                    iBy0 = iBy0 | 0x08;
                    break;
                case t_ChordType._aug:
                    iBy0 = iBy0 | 0x09;
                    break;
                case t_ChordType._m6:
                    iBy0 = iBy0 | 0x0A;
                    break;
                case t_ChordType._7_5:
                    iBy0 = iBy0 | 0x0B;
                    break;
                case t_ChordType._9th:
                    iBy0 = iBy0 | 0x0C;
                    break;
                case t_ChordType._9:
                    iBy0 = iBy0 | 0x0D;
                    break;
                case t_ChordType.OFF:
                    iBy0 = iBy0 | 0x0E;
                    break;
                case t_ChordType.ON:
                    iBy0 = iBy0 | 0x0F;
                    break;
                default:
                    break;
            }//switch

            // encode the rest duration: the nibbles of the value must be swapped
            iBy1 = (byte)AuxFuncs.SwapByteNibbles((byte)iRestIn);

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);

            return erCodeRetVal;

        }//GetChordCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a CHORD command an returns the command 
        * parameters that correspond to the NOTE command encoded in the received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[out] tNoteOut
        * @param[out] tChordTypeOut
        * @param[out] iRestOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetChordCommandParamsFromBytes( byte _by0, byte _by1, ref t_Notes tNoteOut, ref t_ChordType tChordTypeOut, ref int iRestOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iByAux = 0;


            // NOTE COMMAND
            // FIG. 11A-1
            // 8421 
            // ---- 
            // ....  SC ROOT     SC=4-bit note code.      Notes F3 to B5 are used for the note code and 
            // ....  OC NAME     OC=4-bit octave code.    octave code for the melody line.
            // ----  
            // ....  L1 CHORD
            // ....  L2 DURATION
            // ---- 

            //           ROOT (SC)    CHORD NAME
            // 0 | 0000 |    --    |   MAJOR     |
            // 1 | 0001 |    C     |   MINOR     |
            // 2 | 0010 |    C#    |   7th       |
            // 3 | 0011 |    D     |   m7        |
            // 4 | 0100 |    D#    |   M6        |
            // 5 | 0101 |    E     |   6 TH      |
            // 6 | 0110 |    F     |   m7-5      |
            // 7 | 0111 |    F#    |   SUS-4     |
            // 8 | 1000 |    G     |   DIM       |
            // 9 | 1001 |    G#    |   AUG       |
            // A | 1010 |    A     |   m6        |
            // B | 1011 |    A#    |   7-5       |
            // C | 1100 |    B     |   9 TH      |
            // D | 1101 |    --    |    9        |
            // E | 1110 |    --    |  OFF CHORD  |
            // F | 1111 |    --    |  MON CHORD  |

            // check the CHORD NOTE command
            if ((iBy0 < 0x10) || (iBy0 > 0xCF)) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }

            if (erCodeRetVal.i_code >= 0) {
                // decode the CHORD NOTE code
                iByAux = 0xF0 & _by0;
                switch (iByAux ) {
                    case 0x10:
                        tNoteOut = t_Notes.C;
                        break;
                    case 0x20:
                        tNoteOut = t_Notes.Csh;
                        break;
                    case 0x30:
                        tNoteOut = t_Notes.D;
                        break;
                    case 0x40:
                        tNoteOut = t_Notes.Dsh;
                        break;
                    case 0x50:
                        tNoteOut = t_Notes.E;
                        break;
                    case 0x60:
                        tNoteOut = t_Notes.F;
                        break;
                    case 0x70:
                        tNoteOut = t_Notes.Fsh;
                        break;
                    case 0x80:
                        tNoteOut = t_Notes.G;
                        break;
                    case 0x90:
                        tNoteOut = t_Notes.Gsh;
                        break;
                    case 0xA0:
                        tNoteOut = t_Notes.A;
                        break;
                    case 0xB0:
                        tNoteOut = t_Notes.Ash;
                        iBy0 = iBy0 | 0xB0;
                        break;
                    case 0xC0:
                        tNoteOut = t_Notes.B;
                        break;
                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch

                // decode the CHORD TTOE code
                iByAux = 0x0F & _by0;
                switch (iByAux) {
                    case 0x00:
                        tChordTypeOut = t_ChordType._MAJOR;
                        break;
                    case 0x01:
                        tChordTypeOut = t_ChordType._MINOR;
                        break;
                    case 0x02:
                        tChordTypeOut = t_ChordType._7TH;
                        break;
                    case 0x03:
                        tChordTypeOut = t_ChordType._m7;
                        break;
                    case 0x04:
                        tChordTypeOut = t_ChordType._M6;
                        break;
                    case 0x05:
                        tChordTypeOut = t_ChordType._6TH;
                        break;
                    case 0x06:
                        tChordTypeOut = t_ChordType._m7_6;
                        break;
                    case 0x07:
                        tChordTypeOut = t_ChordType._sus4;
                        break;
                    case 0x08:
                        tChordTypeOut = t_ChordType._dim;
                        break;
                    case 0x09:
                        tChordTypeOut = t_ChordType._aug;
                        break;
                    case 0x0A:
                        tChordTypeOut = t_ChordType._m6;
                        break;
                    case 0x0B:
                        tChordTypeOut = t_ChordType._7_5;
                        break;
                    case 0x0C:
                        tChordTypeOut = t_ChordType._9th;
                        break;
                    case 0x0D:
                        tChordTypeOut = t_ChordType._9;
                        break;
                    case 0x0E:
                        tChordTypeOut = t_ChordType.OFF;
                        break;
                    case 0x0F:
                        tChordTypeOut = t_ChordType.ON;
                        break;
                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch
            
            }//if

            // decode the rest duration: the nibbles of the value are sotred swapped 
            if (erCodeRetVal.i_code >= 0) {                
                iRestOut = (int)AuxFuncs.SwapByteNibbles((byte)iBy1);
            }

            return erCodeRetVal;

        }//GetChordCommandParamsFromBytes

        /*******************************************************************************
        * @brief method that receives the parameters of a REPEAT command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] tRepeatIn
        * @param[out] _by0
        * @param[out] _by1
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetRepeatCommandBytesFromParams(t_RepeatMark tRepeatIn, ref byte _by0, ref byte _by1) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;

            // REPEAT COMMAND:
            // FIG. 11C-1
            // 8421 
            // ---- 
            // 1111 REPEAT 
            // xxxx COMMAND 0=Begining mark, 1=End mark, 8=Repeat x1, 9=Repeat x2, A=Repeat x3, B=Repeat x4, C=Repeat x5, D=Repeat x6, E=Repeat x7, F=Repeat x8
            // ----  
            // 0000  
            // 0000 
            // ---- 

            // encode the REPEAT command code
            iBy0 = iBy0 | 0xF0;
            switch (tRepeatIn) {
                case t_RepeatMark.START:
                    iBy0 = iBy0 | 0x00;
                    break;
                case t_RepeatMark.END:
                    iBy0 = iBy0 | 0x01;
                    break;
                case t_RepeatMark.X1:
                    iBy0 = iBy0 | 0x08;
                    break;
                case t_RepeatMark.X2:
                    iBy0 = iBy0 | 0x09;
                    break;
                case t_RepeatMark.X3:
                    iBy0 = iBy0 | 0x0A;
                    break;
                case t_RepeatMark.X4:
                    iBy0 = iBy0 | 0x0B;
                    break;
                case t_RepeatMark.X5:
                    iBy0 = iBy0 | 0x0C;
                    break;
                case t_RepeatMark.X6:
                    iBy0 = iBy0 | 0x0D;
                    break;
                case t_RepeatMark.X7:
                    iBy0 = iBy0 | 0x0E;
                    break;
                case t_RepeatMark.X8:
                    iBy0 = iBy0 | 0x0F;
                    break;
            }//switch

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);

            return erCodeRetVal;

        }//GetRepeatCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a REST_DURATION command an returns the 
        * command parameters that correspond to the REST_DURATION command in encoded in the 
        * received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[out] tRepeatOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetRepeatCommandParamsFromBytes( byte _by0, byte _by1, ref t_RepeatMark tRepeatOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iByAux = 0;


            // REPEAT COMMAND:
            // FIG. 11C-1
            // 8421 
            // ---- 
            // 1111 REPEAT 
            // xxxx COMMAND 0=Begining mark, 1=End mark, 8=Repeat x1, 9=Repeat x2, A=Repeat x3, B=Repeat x4, C=Repeat x5, D=Repeat x6, E=Repeat x7, F=Repeat x8
            // ----  
            // 0000  
            // 0000 
            // ---- 

            // check the REPEAT command
            if ((iBy0 < 0xF0)) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }

            if (erCodeRetVal.i_code >= 0) {
                // decode the REPEAT command code
                iByAux = 0x0F & _by0;
                switch (iByAux) {
                    case 0x00:
                        tRepeatOut = t_RepeatMark.START;
                        break;
                    case 0x01:
                        tRepeatOut = t_RepeatMark.END;
                        break;
                    case 0x08:
                        tRepeatOut = t_RepeatMark.X1;
                        break;
                    case 0x09:
                        tRepeatOut = t_RepeatMark.X2;
                        break;
                    case 0x0A:
                        tRepeatOut = t_RepeatMark.X3;
                        break;
                    case 0x0B:
                        tRepeatOut = t_RepeatMark.X4;
                        break;
                    case 0x0C:
                        tRepeatOut = t_RepeatMark.X5;
                        break;
                    case 0x0D:
                        tRepeatOut = t_RepeatMark.X6;
                        break;
                    case 0x0E:
                        tRepeatOut = t_RepeatMark.X7;
                        break;
                    case 0x0F:
                        tRepeatOut = t_RepeatMark.X8;
                        break;
                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch              
            }//if

            return erCodeRetVal;

        }//GetRepeatCommandParamsFromBytes

        /*******************************************************************************
        * @brief method that receives the parameters of a RYTHM command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] tRythmModeIn
        * @param[in] tRythmStyleIn
        * @param[out] _by0
        * @param[out] _by1
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetRythmCommandBytesFromParams(t_RythmMode tRythmModeIn, t_RythmStyle tRythmStyleIn, t_On_Off tOnOffIn, ref byte _by0, ref byte _by1) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;

            // RYTHM  COMMAND:
            // FIG. 11D-1
            // ON   OFF
            // 8421 8421 
            // ---- ---- 
            // 0000 0000 RIFIR-D COMMAND
            // xxxx xxxx  
            // ---- ----  
            // xxxx xxxx  RYTHM  0x00=rock, 0x01=disco, 0x02=swing 2 beat, 0x03=samba, 0x04=bossa nova, 0x05=tango, 0x06=slow rock, 0x07=waltz, 0x10=rock'n roll, 0x11=16 beat, 0x12=swing 4 beat, 0x13=latin rock, 0x14=beguine, 0x15=march, 0x16=ballad, 0x17=rock waltz, 0x22=latin swing
            // 0xxx 1xxx  DATA
            // ---- ---- 

            // encode the RYTHM command 
            switch (tRythmModeIn) {
                case t_RythmMode.SET:
                    iBy0 = iBy0 | 0x05;
                    break;
                case t_RythmMode.FILL_IN:
                    iBy0 = iBy0 | 0x06;
                    break;
                case t_RythmMode.DISCRIMINATION:
                    iBy0 = iBy0 | 0x08;
                    break;
                default:
                    break;
            }//switch

            // encode the RYTHM ON OFF bit
            if (tOnOffIn == t_On_Off.OFF) {
                iBy1 = iBy1 | 0x08;
            } else {
                iBy1 = iBy1 & 0xF7;
            }

            // encode the RYTHM style
            switch (tRythmStyleIn) {
                case t_RythmStyle.ROCK:
                    iBy1 = iBy1 | 0x00;
                    break;
                case t_RythmStyle.DISCO:
                    iBy1 = iBy1 | 0x01;
                    break;
                case t_RythmStyle.SWING_2_BEAT:
                    iBy1 = iBy1 | 0x02;
                    break;
                case t_RythmStyle.SAMBA:
                    iBy1 = iBy1 | 0x03;
                    break;
                case t_RythmStyle.BOSSA_NOVA:
                    iBy1 = iBy1 | 0x04;
                    break;
                case t_RythmStyle.TANGO:
                    iBy1 = iBy1 | 0x05;
                    break;
                case t_RythmStyle.SLOW_ROCK:
                    iBy1 = iBy1 | 0x06;
                    break;
                case t_RythmStyle.WALTZ:
                    iBy1 = iBy1 | 0x07;
                    break;
                case t_RythmStyle.ROCK_N_ROLL:
                    iBy1 = iBy1 | 0x10;
                    break;
                case t_RythmStyle.x16_BEAT:
                    iBy1 = iBy1 | 0x11;
                    break;
                case t_RythmStyle.SWING_4_BEAT:
                    iBy1 = iBy1 | 0x12;
                    break;
                case t_RythmStyle.LATIN_ROCK:
                    iBy1 = iBy1 | 0x13;
                    break;
                case t_RythmStyle.BEGUINE:
                    iBy1 = iBy1 | 0x14;
                    break;
                case t_RythmStyle.MARCH:
                    iBy1 = iBy1 | 0x15;
                    break;
                case t_RythmStyle.BALLAD:
                    iBy1 = iBy1 | 0x16;
                    break;
                case t_RythmStyle.ROCK_WALTZ:
                    iBy1 = iBy1 | 0x17;
                    break;
                case t_RythmStyle.LATING_SWING:
                    iBy1 = iBy1 | 0x22;
                    break;
                default:
                    break;
            }//switch

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);

            return erCodeRetVal;

        }//GetRythmCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a RYTHM command an returns the 
        * command parameters that correspond to the RYTHM command encoded in the 
        * received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[out] tRythmModeOut
        * @param[out] tRythmStyleOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetRythmCommandParamsFromBytes(byte _by0, byte _by1, ref t_RythmMode tRythmModeOut, ref t_RythmStyle tRythmStyleOut, ref t_On_Off tOnOffOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iByAux = 0;

            // RYTHM  COMMAND:
            // FIG. 11D-1
            // ON   OFF
            // 8421 8421 
            // ---- ---- 
            // 0000 0000 RIFIR-D COMMAND
            // xxxx xxxx  
            // ---- ----  
            // xxxx xxxx  RYTHM  0x00=rock, 0x01=disco, 0x02=swing 2 beat, 0x03=samba, 0x04=bossa nova, 0x05=tango, 0x06=slow rock, 0x07=waltz, 0x10=rock'n roll, 0x11=16 beat, 0x12=swing 4 beat, 0x13=latin rock, 0x14=beguine, 0x15=march, 0x16=ballad, 0x17=rock waltz, 0x22=latin swing
            // 0xxx 1xxx  DATA
            // ---- ---- 

            // check the RYTHM command
            if ((iBy0 != 0x05) && (iBy0 != 0x06) && (iBy0 != 0x08)) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }

            if (erCodeRetVal.i_code >= 0) {
                // encode the RYTHM command 
                iByAux = _by0;
                switch (iByAux) {
                    case 0x05:
                        tRythmModeOut = t_RythmMode.SET;
                        break;
                    case 0x06:
                        tRythmModeOut = t_RythmMode.FILL_IN;
                        break;
                    case 0x08:
                        tRythmModeOut = t_RythmMode.DISCRIMINATION;
                        break;
                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch
            }//if

            if (erCodeRetVal.i_code >= 0) {
                // encode the RYTHM style
                iByAux = _by1 & 0xF7 ;
                switch (iByAux) {
                    case 0x00:
                        tRythmStyleOut = t_RythmStyle.ROCK;
                        break;
                    case 0x01:
                        tRythmStyleOut = t_RythmStyle.DISCO;
                        break;
                    case 0x02:
                        tRythmStyleOut = t_RythmStyle.SWING_2_BEAT;
                        break;
                    case 0x03:
                        tRythmStyleOut = t_RythmStyle.SAMBA;
                        break;
                    case 0x04:
                        tRythmStyleOut = t_RythmStyle.BOSSA_NOVA;
                        break;
                    case 0x05:
                        tRythmStyleOut = t_RythmStyle.TANGO;
                        break;
                    case 0x06:
                        tRythmStyleOut = t_RythmStyle.SLOW_ROCK;
                        break;
                    case 0x07:
                        tRythmStyleOut = t_RythmStyle.WALTZ;
                        break;
                    case 0x10:
                        tRythmStyleOut = t_RythmStyle.ROCK_N_ROLL;
                        break;
                    case 0x11:
                        tRythmStyleOut = t_RythmStyle.x16_BEAT;
                        break;
                    case 0x12:
                        tRythmStyleOut = t_RythmStyle.SWING_4_BEAT;
                        break;
                    case 0x13:
                        tRythmStyleOut = t_RythmStyle.LATIN_ROCK;
                        break;
                    case 0x14:
                        tRythmStyleOut = t_RythmStyle.BEGUINE;
                        break;
                    case 0x15:
                        tRythmStyleOut = t_RythmStyle.MARCH;
                        break;
                    case 0x16:
                        tRythmStyleOut = t_RythmStyle.BALLAD;
                        break;
                    case 0x17:
                        tRythmStyleOut = t_RythmStyle.ROCK_WALTZ;
                        break;
                    case 0x22:
                        tRythmStyleOut = t_RythmStyle.LATING_SWING;
                        break;
                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch

                // decode the RYTHM style
                iByAux = _by1 & 0x08;
                switch (iByAux) {
                    case 0x08:
                        tOnOffOut = t_On_Off.OFF;
                        break;
                    case 0x00:
                        tOnOffOut = t_On_Off.ON;
                        break;
                    default:
                        erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
                        break;
                }//switch

            }//if

            return erCodeRetVal;

        }//GetRythmCommandParamsFromBytes

        /*******************************************************************************
        * @brief method that returns the bytes that codify the TEMPO command.
        * 
        * @param[in]  iTempoIn
        * @param[out] _by0
        * @param[out] _by1
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetTempoCommandBytesFromParams(t_On_Off tOnOffIn, int iTempoIn, ref byte _by0, ref byte _by1) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iAux = 0;

            // TEMPO  COMMAND:
            // FIG. 11E-1
            // ON   OFF
            // 8421 8421 
            // ---- ---- 
            // 0000 0000 TEMPO COMMAND
            // 1100 1100
            // ---- ----   
            // xxxx xxxx SC  TEMPO DATA: check FIG 16
            // 0xxx 1xxx OC
            // ---- ---- 

            // set the TEMPO COMMAND:
            iBy0 = 0x0C;

            // set the TEMPO DATA: value and on/off bit
            // tempo is encoded as: xxxx ayyy (  check FIG16 ):
            // tempo = ('yyyxxxx'+1) * 3  ==> (tempo/3) - 1 = yyyxxx
            iAux = (iTempoIn / 3) - 1;
            // set tempo value
            iBy1 = (iAux & 0x0F)<<4;
            iBy1 = iBy1 | (iAux & 0x70)>>4 ;
            // set tempo on/off bit: 'a'
            if (tOnOffIn == t_On_Off.OFF) {
                iBy1 = iBy1 | 0x08;
            } else {
                iBy1 = iBy1 & 0xF7;
            }

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);

            return erCodeRetVal;

        }//GetTempoCommandBytesFromParams

        /*******************************************************************************
          * @brief method that receives the bytes of a TEMPO command an returns the 
          * command parameters that correspond to the TEMPO command encoded in the 
          * received bytes.
          *  
          * @param[in] _by0
          * @param[in] _by1
          * @param[out] tOnOffOut
          * @param[out] iTempoOut
          * 
          * @return >=0 the parameters of the command have been succesfully generated, <0 an  
          * error occurred while trying to obtain the bytes of the command.
          *******************************************************************************/
        public static ErrCode GetTempoCommandParamsFromBytes(byte _by0, byte _by1, ref t_On_Off tOnOffOut, ref int iTempoOut) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);
            int iAux = 0;
            int iAux2 = 0;

            // TEMPO  COMMAND:
            // FIG. 11E-1
            // ON   OFF
            // 8421 8421 
            // ---- ---- 
            // 0000 0000 TEMPO COMMAND
            // 1100 1100
            // ---- ----   
            // xxxx xxxx SC  TEMPO DATA: check FIG 16
            // 0xxx 1xxx OC
            // ---- ---- 

            // check the TEMPO command
            if (iBy0 != 0x0c) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }

            // get the TEMPO DATA: value and on/off bit
            // tempo is encoded as: xxxx ayyy ( check FIG16 )
            if (erCodeRetVal.i_code >= 0) {

                // take the TEMPO value:
                // tempo = ('yyyxxxx'+1) * 3  ==> (tempo/3) - 1 = yyyxxx
                iAux = (iBy1 & 0xF0) >> 4;
                iAux2 = (iBy1 & 0x07) << 4;
                iTempoOut = ((iAux2 | iAux)+1)*3;

                // take the ON OFF bit 'a' state
                iAux = (iBy1 & 0x08);
                if (iAux != 0) {
                    // On/Off bit is '1' so tempo is OFF
                    tOnOffOut = t_On_Off.OFF;
                } else {
                    // On/Off bit is '0' so tempo is ON
                    tOnOffOut = t_On_Off.ON;
                }

            }//if

            return erCodeRetVal;

        }//GetTempoCommandParamsFromBytes

        /*******************************************************************************
        * @brief method that returns the bytes that codify the COUNTER RESET command.
        * 
        * @param[out] _by0
        * @param[out] _by1
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetCounterResetCommandBytesFromParams(ref byte _by0, ref byte _by1) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;

            // encode the COUNTER RESET command
            iBy0 = 0x09;

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);

            return erCodeRetVal;

        }//GetCounterResetCommandBytesFromParams

        /*******************************************************************************
        * @brief method that returns the bytes that codify the END command.
        * 
        * @param[out] _by0
        * @param[out] _by1
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode GetEndCommandBytesFromParams(ref byte _by0, ref byte _by1) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;
            int iBy2 = 0;

            // END COMMAND:
            // FIG. 10J
            // 8421 
            // ---- 
            // 0000 END COMMAND
            // 1111
            // ---- 
            // 0000
            // 0000
            // ---- 
            // 0000
            // 0000 
            // ---- 

            // encode the END command
            iBy0 = 0x0F;

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);

            return erCodeRetVal;

        }//GetEndCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the parameters of a DOUBLE_DURATION command and 
        * returns the bytes that codify that command with the received parameters.
        * 
        * @param[in] iToneIn
        * @param[out] _by0
        * @param[out] _by1
        * 
        * @return >=0 the bytes of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode Get2xDurationCommandBytesFromParams(int iToneIn, ref byte _by0, ref byte _by1) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = 0;
            int iBy1 = 0;

            // DOUBLE DURATION COMMAND:
            // FIG. 11A-2
            // 8421 
            // ---- 
            // 0000
            // 0010
            // ---- 
            // xxxx  Duration
            // xxxx
            // ---- 

            // set the DOUBLE_DURATION COMMAND
            iBy0 = 0x02;

            // encode the 2x tone duration: the nibbles of the value must be swapped
            iBy1 = (byte)AuxFuncs.SwapByteNibbles((byte)iToneIn);

            // get the value of the bytes to retur
            _by0 = Convert.ToByte(iBy0);
            _by1 = Convert.ToByte(iBy1);

            return erCodeRetVal;

        }//Get2xDurationCommandBytesFromParams

        /*******************************************************************************
        * @brief method that receives the bytes of a DOUBLE_DURATION command an returns  
        * the command parameters that correspond to the DOUBLE_DURATION command encoded
        * in the received bytes.
        *  
        * @param[in] _by0
        * @param[in] _by1
        * @param[out] iToneOut
        * 
        * @return >=0 the parameters of the command have been succesfully generated, <0 an  
        * error occurred while trying to obtain the bytes of the command.
        *******************************************************************************/
        public static ErrCode Get2xDurationCommandParamsFromBytes(byte _by0, byte _by1, ref int iToneDurOut ) {
            ErrCode erCodeRetVal = cErrCodes.ERR_NO_ERROR; // ERR_EDITION_ENCODING_COMMAND_WRONG_PARAM
            int iBy0 = Convert.ToInt32(_by0);
            int iBy1 = Convert.ToInt32(_by1);

            // DOUBLE DURATION COMMAND:
            // FIG. 11A-2
            // 8421 
            // ---- 
            // 0000
            // 0010
            // ---- 
            // xxxx  Duration
            // xxxx
            // ---- 

            // check that it is the DOUBLE DURATION COMMAND
            if (iBy0 != 0x02) {
                erCodeRetVal = cErrCodes.ERR_DECODING_INVALID_INSTRUCTION;
            }//if

            if (erCodeRetVal.i_code >= 0) {

                // decode the rest duration: the nibbles of the value are sotred swapped 
                iToneDurOut = (int)AuxFuncs.SwapByteNibbles((byte)iBy1);

            }

            return erCodeRetVal;

        }//Get2xDurationCommandParamsFromBytes

    }//class ChordChannelCodeEntry


    /*******************************************************************************
    *  @brief defines the object with all the data of the current loaded ROM PACK 
    *  cartridge: that is the ROM cartridge binayro content and all other extra 
    *  information in text format.
    *******************************************************************************/
    public class cDrivePack{

        // DRP file METADATA BLOCK IDs
        public const int FILE_METADATA_TITLE = 0x01;
        public const int FILE_METADATA_SONGS_INFO = 0x02;
        public const int FILE_METADATA_SONGS_ROM = 0x03;
        public const int ROM_MAX_SIZE = 0x8000;  // 4095 bloc * (16 nibbles/bloc) = 65520 nibbles / 2 = 32768 bytes = 0x8000 bytes

        // TAGs used inside the ROMs INFO metadata block of the DRP files
        const string TAG_ROM_INFO = "<rom_info>";
        const string TAG_ROM_INFO_END = "</rom_info>";
        const string TAG_ROM_TITLE = "<ro_ti>";
        const string TAG_ROM_TITLE_END = "</ro_ti>";
        const string TAG_ROM_TITLE_LIST = "<li_ti>";
        const string TAG_ROM_TITLE_LIST_END = "</li_ti>";
        const string TAG_THEME_TITLE = "<ti>";
        const string TAG_THEME_TITLE_END = "</ti>";
        const string TAG_INFO = "<info>";
        const string TAG_INFO_END = "</info>";

        // Code file headers
        const string STR_THEME_COMMENT_SYMBOL = "//";
        const string STR_THEME_SEPARATION_SYMBOL = ";";
        const string STR_THEME_FILE_N_THEMES = "//n_themes:";
        const string STR_THEME_FILE_SEQ_N = "//seq_n:";
        const string STR_THEME_FILE_SEQ_TITLE = "//seq_title:";
        const string STR_THEME_FILE_N_M1_CHAN_ENTRIES = "//n_m1_chan_entries:";
        const string STR_THEME_FILE_M1_CHAN_ENTRIES = "//m1_chan_entries:";
        const string STR_THEME_FILE_N_M2_CHAN_ENTRIES = "//n_m2_han_entries:";
        const string STR_THEME_FILE_M2_CHAN_ENTRIES = "//m2_han_entries:";
        const string STR_THEME_FILE_N_CHORD_CHAN_ENTRIES = "//n_chord_chan_entries:";
        const string STR_THEME_FILE_CHORD_CHAN_ENTRIES = "//chord_chan_entries:";

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
        public DynamicByteProvider dynbyprMemoryBytes; // reference to the dynamic bytes provider
        private bool bDataChanged; // flag to indicate if in the dirvePackis there are changes pending to save         
        public Themes themes = null; // object with a list with all the themes information
        private cLogsNErrors statusNLogsRef = null;// a reference to the logs to allow the objects of this class write information into the logs.

        /******************************************************************************
        * @brief Default constructor.
        * @param[in] _statusNLogsRef reference to the Logs and Errors recording
        * object to allow the objects of that class write information into the logs.
        *******************************************************************************/
        public cDrivePack(cLogsNErrors _statusNLogsRef) {

            dynbyprMemoryBytes = null;
            bDataChanged = false;
            themes = new Themes();
            statusNLogsRef = _statusNLogsRef;

        }//DrivePackData

        // /*******************************************************************************
        // * @brief Receives an array with the bytes of a MELODY channel instruction and
        // * returns a description of the instruction in that bytes.
        // * @param[in] arrByInstr array of bytes
        // * @return the string with the explanation of the MELODY instruction encoded in 
        // * the received bytes.
        // *******************************************************************************/
        // public static string describeMelodyInstructionBytes(byte[] arrByInstr) {
        //     string strRet = "";
        // 
        //     if (arrByInstr.Count() == 3) {
        // 
        //     }
        //     
        //     return strRet;
        // 
        // }//describeMelodyInstructionBytes

        // /*******************************************************************************
        // * @brief Receives an array with the bytes of a CHORD channel instruction and
        // * returns a description of the instruction in that bytes.
        // * @param[in] arrByInstr array of bytes
        // * @return the string with the explanation of the CHORD instruction encoded in 
        // * the received bytes.
        // *******************************************************************************/
        // public static string describeChordInstructionBytes(byte[] arrByInstr) {
        //     string strRet = "";
        // 
        //     if (arrByInstr.Count() == 3) {
        // 
        //     }
        // 
        //     return strRet;
        // 
        // }//describeChordInstructionBytes

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

                themes.iCurrThemeIdx = -1;

                themes.Clear();

                themes.strROMTitle = "RO - XXX - Enter the title of the ROM cartridge here.";
                themes.strROMInfo = "Enter the general information of the ROM cartridge here.";

                // create the DynamicByteProvider and initilized with the previously initialized byte array
                by_memory_bytes = new byte[ROM_MAX_SIZE];
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
        * @brief Remove undesired characters from received string in order it can be saved
        * to the file and then read in the drivePACk unit or in the drivePACK Editor without 
        * problems.
        * @param[in] strToClean the string from which the undesired characters must be removed.
        * @return a string with the received strToClean but without the undesired characters
        *******************************************************************************/
        public string cleanStringForFile(string strToClean) {
            string strAux = "";

            if (strToClean != null) { 
                strAux = strToClean.Replace(">"," ");
                strAux = strToClean.Replace(">", " ");
                strAux = strToClean.Replace(";", " ");
            }

            return strAux;

        }//cleanStringForFile
   

        /*******************************************************************************
        * @brief Saves into a Code file the specifed themes.
        * @param[in] str_save_file with the name of the file to save the themes code in.
        * @param[in] liIdxThemes list with the themes that must be stored in the code file.
        * @return the ErrCode with the result or error of the operation, if ErrCode>0 
        * file has been succesfully saved, if <0 an error occurred
        *******************************************************************************/
        public ErrCode exportSelectedThemesToCodeFile(string str_save_file, List<int> liThemesIDxs) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            StreamWriter file_text_writer;
            ASCIIEncoding ascii = new ASCIIEncoding();
            ThemeCode seq = null;
            string str_line = "";
            int iSeqN = 0;

            file_text_writer = new StreamWriter(str_save_file);

            if (file_text_writer == null) {
                ec_ret_val = cErrCodes.ERR_FILE_CREATING;
            }//if

            if (ec_ret_val.i_code >= 0) {

                // first the number of themes in the list of themes
                str_line = STR_THEME_FILE_N_THEMES;
                file_text_writer.Write(str_line + "\r\n");
                str_line = liThemesIDxs.Count.ToString();
                file_text_writer.Write(str_line + "\r\n");

                // save the information of each Theme 
                iSeqN = 0;
                foreach (int themeIdx in liThemesIDxs) {

                    seq = themes.liThemesCode[themeIdx];

                    // the index of the theme
                    str_line = STR_THEME_FILE_SEQ_N;
                    file_text_writer.Write(str_line + "\r\n");
                    str_line = iSeqN.ToString();
                    file_text_writer.Write(str_line + "\r\n");

                    // the title of the theme
                    str_line = STR_THEME_FILE_SEQ_TITLE;
                    file_text_writer.Write(str_line + "\r\n");
                    str_line = seq.Title;
                    file_text_writer.Write(str_line + "\r\n");

                    // the number of M1 channel code entries
                    str_line = STR_THEME_FILE_N_M1_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    str_line = seq.liM1CodeInstr.Count.ToString();
                    file_text_writer.Write(str_line + "\r\n");

                    // all the current theme M1 channel code entries
                    str_line = STR_THEME_FILE_M1_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    foreach (MChannelCodeEntry melChanEntry in seq.liM1CodeInstr) {
                        str_line = "";
                        str_line = str_line + melChanEntry.By0 + STR_THEME_SEPARATION_SYMBOL;
                        str_line = str_line + melChanEntry.By1 + STR_THEME_SEPARATION_SYMBOL;
                        str_line = str_line + melChanEntry.By2 + STR_THEME_SEPARATION_SYMBOL;
                        if (melChanEntry.strDescr != null) {
                            str_line = str_line + melChanEntry.strDescr.Replace(STR_THEME_SEPARATION_SYMBOL, " ");// in case comment has any STR_THEME_SEPARATION_SYMBOL remove it
                        }
                        file_text_writer.Write(str_line + "\r\n");
                    }//foreach

                    // the number of M2 channel code entries
                    str_line = STR_THEME_FILE_N_M2_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    str_line = seq.liM2CodeInstr.Count.ToString();
                    file_text_writer.Write(str_line + "\r\n");

                    // all the current theme M2 channel code entries
                    str_line = STR_THEME_FILE_M2_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    foreach (MChannelCodeEntry melChanEntry in seq.liM2CodeInstr) {
                        str_line = "";
                        str_line = str_line + melChanEntry.By0 + STR_THEME_SEPARATION_SYMBOL;
                        str_line = str_line + melChanEntry.By1 + STR_THEME_SEPARATION_SYMBOL;
                        str_line = str_line + melChanEntry.By2 + STR_THEME_SEPARATION_SYMBOL;
                        if (melChanEntry.strDescr != null) {
                            str_line = str_line + melChanEntry.strDescr.Replace(STR_THEME_SEPARATION_SYMBOL, " ");// in case comment has any STR_THEME_SEPARATION_SYMBOL remove it
                        }
                        file_text_writer.Write(str_line + "\r\n");
                    }//foreach

                    // the number of chord channel code entries
                    str_line = STR_THEME_FILE_N_CHORD_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    str_line = seq.liChordCodeInstr.Count.ToString();
                    file_text_writer.Write(str_line + "\r\n");

                    // all the current theme chords channel code entries
                    str_line = STR_THEME_FILE_CHORD_CHAN_ENTRIES;
                    file_text_writer.Write(str_line + "\r\n");
                    foreach (ChordChannelCodeEntry chordChanEntry in seq.liChordCodeInstr) {
                        str_line = "";
                        str_line = str_line + chordChanEntry.By0 + STR_THEME_SEPARATION_SYMBOL;
                        str_line = str_line + chordChanEntry.By1 + STR_THEME_SEPARATION_SYMBOL;
                        if (chordChanEntry.strDescr != null) {
                            str_line = str_line + chordChanEntry.strDescr.Replace(STR_THEME_SEPARATION_SYMBOL, " ");// in case comment has any STR_THEME_SEPARATION_SYMBOL remove it
                        }
                        file_text_writer.Write(str_line + "\r\n");
                    }//foreach

                    iSeqN++;

                }//foreach

            }//if

            file_text_writer.Close();

            return ec_ret_val;

        }//exportSelectedThemesToCodeFile

        /*******************************************************************************
        * @brief Loads the different themes in the file to the current list of themes in 
        * memory.
        * @param[in] str_load_file with the name of the file to load the themes from
        * @param[in] iIdxToInsert position in the themes list at which the imported 
        * themes insertion will start.
        * @param[out] iNumImportedThemes the number of themes that have been imported and loded to memory
        * @return the ErrCode with the result or error of the operation, if ErrCode>0 
        * file has been succesfully loaded into the object, if <0 an error occurred
        *******************************************************************************/
        public ErrCode importCodeFile(string str_load_file,int iIdxToInsert, ref int iNumImportedThemes) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            StreamReader file_text_reader = null;
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
            int iM1ChannelEntriesCtr = 0;
            int iM2TotalChannelEntries = 0;
            int iM2ChannelEntriesCtr = 0;
            int iTotalChordChannelEntries = 0;
            int iChordChannelEntriesCtr = 0;
            int iCurrThemeN = 0;


            iNumImportedThemes = 0;
            if (iIdxToInsert> themes.liThemesCode.Count()) {
                ec_ret_val = cErrCodes.ERR_FILE_IMPORTING_AT_SPECIFIED_POSITION;            
            }

            if (ec_ret_val.i_code >= 0) {

                if (!File.Exists(str_load_file)) {

                    ec_ret_val = cErrCodes.ERR_FILE_NOT_EXIST;

                } else {

                    file_text_reader = new StreamReader(str_load_file);

                    if (file_text_reader == null) {
                        ec_ret_val = cErrCodes.ERR_FILE_CREATING;
                    }//if
                }

            }//if

            if (ec_ret_val.i_code >= 0) {

                // start from the received theme index
                iCurrThemeN = iIdxToInsert;

                strCurrSection = "";

                while ((ec_ret_val.i_code >= 0) && ((strLine = file_text_reader.ReadLine()) != null)) {

                    strLine = strLine.Trim();

                    bReadLineIsHeader = false;

                    // check if the read line corresponds to a section header line and update the strCurrSection if affirmative
                    switch (strLine) {

                        case STR_THEME_FILE_N_THEMES:
                            strCurrSection = STR_THEME_FILE_N_THEMES;
                            bReadLineIsHeader = true;
                            break;

                        case STR_THEME_FILE_SEQ_N:
                            strCurrSection = STR_THEME_FILE_SEQ_N;
                            bReadLineIsHeader = true;
                            break;

                        case STR_THEME_FILE_SEQ_TITLE:
                            strCurrSection = STR_THEME_FILE_SEQ_TITLE;
                            bReadLineIsHeader = true;
                            break;

                        case STR_THEME_FILE_N_M1_CHAN_ENTRIES:
                            strCurrSection = STR_THEME_FILE_N_M1_CHAN_ENTRIES;
                            bReadLineIsHeader = true;
                            break;

                        case STR_THEME_FILE_M1_CHAN_ENTRIES:
                            strCurrSection = STR_THEME_FILE_M1_CHAN_ENTRIES;
                            bReadLineIsHeader = true;
                            break;

                        case STR_THEME_FILE_N_M2_CHAN_ENTRIES:
                            strCurrSection = STR_THEME_FILE_N_M2_CHAN_ENTRIES;
                            bReadLineIsHeader = true;
                            break;

                        case STR_THEME_FILE_M2_CHAN_ENTRIES:
                            strCurrSection = STR_THEME_FILE_M2_CHAN_ENTRIES;
                            bReadLineIsHeader = true;
                            break;

                        case STR_THEME_FILE_N_CHORD_CHAN_ENTRIES:
                            strCurrSection = STR_THEME_FILE_N_CHORD_CHAN_ENTRIES;
                            bReadLineIsHeader = true;
                            break;

                        case STR_THEME_FILE_CHORD_CHAN_ENTRIES:
                            strCurrSection = STR_THEME_FILE_CHORD_CHAN_ENTRIES;
                            bReadLineIsHeader = true;
                            break;


                    }//switch

                    // if the line read in that iteration does not correspond to a header section line then process 
                    // it as regular file line according to the kind of section that is being processed
                    if (bReadLineIsHeader == false) {

                        // process the read line as a regular file line in one or another way deppending on the current section
                        switch (strCurrSection) {

                            case STR_THEME_FILE_N_THEMES:
                                // check if there is space before loading the themes form the file
                                iTotalThemes = Convert.ToInt32(strLine);
                                if ((themes.liThemesCode.Count() + iTotalThemes) > Themes.MAX_THEMES_ROM) {
                                    ec_ret_val = cErrCodes.ERR_FILE_IMPORT_THEMES_NO_SPACE;
                                }
                                break;

                            case STR_THEME_FILE_SEQ_N:
                                // check if there is space before adding a new theme to the themes list 
                                if ((themes.liThemesCode.Count() >= Themes.MAX_THEMES_ROM)) {
                                    ec_ret_val = cErrCodes.ERR_FILE_IMPORT_THEMES_NO_SPACE;
                                } else {
                                    themeAux = new ThemeCode();
                                    themes.liThemesCode.Insert(iCurrThemeN, themeAux);
                                    iNumImportedThemes = iNumImportedThemes + 1;
                                    iCurrThemeN = iCurrThemeN + 1;
                                    themes.iCurrThemeIdx = iCurrThemeN;
                                    themeAux.Title = "";
                                    themeAux.Idx = iCurrThemeN;
                                    // themes.liThemesCode[iCurrThemeN].Title = "";
                                    // themes.liThemesCode[iCurrThemeN].Idx = iCurrThemeN;
                                }
                                break;

                            case STR_THEME_FILE_SEQ_TITLE:
                                themeAux.Title = strLine;
                                // themes.liThemesCode[iCurrThemeN].Title = strLine;
                                break;

                            case STR_THEME_FILE_N_M1_CHAN_ENTRIES:
                                iM1TotalChannelEntries = Convert.ToInt32(strLine);
                                iM1ChannelEntriesCtr = 0;// reset to 0 the counter used to set the M2 instructions index
                                break;

                            case STR_THEME_FILE_M1_CHAN_ENTRIES:
                                // strLine = strLine.Replace("0x", "");
                                arrEntryElems = strLine.Split(STR_THEME_SEPARATION_SYMBOL);
                                if (arrEntryElems.Count() == 4) {
                                    MCodeEntryAux = new MChannelCodeEntry();
                                    MCodeEntryAux.Idx = iM1ChannelEntriesCtr;
                                    iM1ChannelEntriesCtr++;
                                    MCodeEntryAux.By0 = arrEntryElems[0];
                                    MCodeEntryAux.By1 = arrEntryElems[1];
                                    MCodeEntryAux.By2 = arrEntryElems[2];
                                    MCodeEntryAux.strDescr = arrEntryElems[3]; ;
                                    themeAux.liM1CodeInstr.Add(MCodeEntryAux);
                                    //themes.liThemesCode[iCurrThemeN].liM1CodeInstr.Add(MCodeEntryAux);
                                } else {
                                    ec_ret_val = cErrCodes.ERR_FILE_PARSING_ELEMENTS;
                                }
                                break;

                            case STR_THEME_FILE_N_M2_CHAN_ENTRIES:
                                iM2TotalChannelEntries = Convert.ToInt32(strLine);
                                iM2ChannelEntriesCtr = 0;// reset to 0 the counter used to set the M2 instructions index
                                break;

                            case STR_THEME_FILE_M2_CHAN_ENTRIES:
                                // strLine = strLine.Replace("0x", "");
                                arrEntryElems = strLine.Split(STR_THEME_SEPARATION_SYMBOL);
                                if (arrEntryElems.Count() == 4) {
                                    MCodeEntryAux = new MChannelCodeEntry();
                                    MCodeEntryAux.Idx = iM2ChannelEntriesCtr;
                                    iM2ChannelEntriesCtr++;
                                    MCodeEntryAux.By0 = arrEntryElems[0];
                                    MCodeEntryAux.By1 = arrEntryElems[1];
                                    MCodeEntryAux.By2 = arrEntryElems[2];
                                    MCodeEntryAux.strDescr = arrEntryElems[3];
                                    themeAux.liM2CodeInstr.Add(MCodeEntryAux);
                                    //themes.liThemesCode[iCurrThemeN].liM2CodeInstr.Add(MCodeEntryAux);
                                } else {
                                    ec_ret_val = cErrCodes.ERR_FILE_PARSING_ELEMENTS;
                                }
                                break;

                            case STR_THEME_FILE_N_CHORD_CHAN_ENTRIES:
                                iTotalChordChannelEntries = Convert.ToInt32(strLine);
                                iChordChannelEntriesCtr = 0;// reset to 0 the counter used to set the Chords instructions index
                                break;

                            case STR_THEME_FILE_CHORD_CHAN_ENTRIES:
                                // strLine = strLine.Replace("0x", "");
                                arrEntryElems = strLine.Split(STR_THEME_SEPARATION_SYMBOL);
                                if (arrEntryElems.Count() == 3) {
                                    chordCodeEntryAux = new ChordChannelCodeEntry();
                                    chordCodeEntryAux.Idx = iChordChannelEntriesCtr;
                                    iChordChannelEntriesCtr++;
                                    chordCodeEntryAux.By0 = arrEntryElems[0];
                                    chordCodeEntryAux.By1 = arrEntryElems[1];
                                    chordCodeEntryAux.strDescr = arrEntryElems[2];
                                    themeAux.liChordCodeInstr.Add(chordCodeEntryAux);
                                    // themes.liThemesCode[iCurrThemeN].liChordCodeInstr.Add(chordCodeEntryAux);
                                } else {
                                    ec_ret_val = cErrCodes.ERR_FILE_PARSING_ELEMENTS;
                                }
                                break;

                        }//switch

                    }//if (bReadLineIsHeader == false) 

                }//while

                file_text_reader.Close();

            }//if (ec_ret_val.i_code >= 0)

            if (ec_ret_val.i_code >= 0) {

                // set the received theme index as the selected one
                themes.iCurrThemeIdx = iIdxToInsert;

                // as new themes have been added to the themes structure regenerate the Idxs of each theme
                themes.regenerateIdxs();

            }

            return ec_ret_val;

        }//importCodeFile

        /*******************************************************************************
        * @brief function that receives a string in text XML format with the information ot  
        * the ROM cartridge and parses it and updates the corresponding structures in memories
        * with the data in that structure.
        * 
        * @param[in] strInfoMetadataBlock with the text XML format that contains the ROM
        * inforamtion ( themes titles, ROM general info ... ) that must be parsed and then
        * updated to the corresponding objects in memory.
        * @param[out] strROMTitle with the title of the ROM read from the the Information
        * Metadata Block
        * @param[out] liTitles a list of strings with the titles of the different themes in 
        * the ROM.
        * @param[out] strROMInfo with other general information of the  ROM cartridge
        * 
        * @return >=0 file has been succesfully loaded into the object, <0 an error 
        * occurred 
        *******************************************************************************/
         public ErrCode parseInformationMetadataBlock(string strInfoMetadataBlock, ref string strROMTitle, ref List<string> liTitles, ref string  strROMInfo) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string[] arrStrThemeTitles = null;
            string str_aux = "";
            int i_aux1 = 0;
            int i_aux2 = 0;

            // check the ROM INFO tags ( ROM title, themes titles, general info ... ) to confirm that all data is inside the valid "<rom_info>" "</rom_info>" tags
            i_aux1 = strInfoMetadataBlock.IndexOf(TAG_ROM_INFO);
            i_aux2 = strInfoMetadataBlock.IndexOf(TAG_ROM_INFO_END);
            if ( (i_aux1<0) || (i_aux2<0) || (i_aux2<i_aux1)) {
                //error: the TAGS that mark the content inside the INFORMATION METADATA BLOCK are wrong
                ec_ret_val = cErrCodes.ERR_FILE_PARSING_ROM_INFO_BLOCK;
            }//if

            // get the ROM PACK TITLE
            if (ec_ret_val.i_code >= 0) {

                // clear the ROM PACK TITLE before initializing it
                strROMTitle = "";

                i_aux1 = strInfoMetadataBlock.IndexOf(TAG_ROM_TITLE);
                i_aux2 = strInfoMetadataBlock.IndexOf(TAG_ROM_TITLE_END);
                if ((i_aux1 >= 0) && (i_aux2 >= 0) && (i_aux2 > i_aux1)) {

                    i_aux1 = i_aux1 + TAG_ROM_TITLE.Length;
                    str_aux = strInfoMetadataBlock.Substring(i_aux1, i_aux2 - i_aux1);
                    str_aux = str_aux.Trim();
                    strROMTitle = str_aux;

                }//if

            }//if

            // get the LIST OF THEME TITLES
            if (ec_ret_val.i_code >= 0) {

                liTitles.Clear();

                i_aux1 = strInfoMetadataBlock.IndexOf(TAG_ROM_TITLE_LIST);
                i_aux2 = strInfoMetadataBlock.IndexOf(TAG_ROM_TITLE_LIST_END);
                if ( (i_aux1 >=0) && (i_aux2 >= 0) && (i_aux2 > i_aux1) ) {

                    // get the content between the tags of the list of themes, then split it into the different titles strings and process each one 
                    i_aux1 = i_aux1 + TAG_ROM_TITLE_LIST.Length;
                    str_aux = strInfoMetadataBlock.Substring(i_aux1, i_aux2 - i_aux1);
                    arrStrThemeTitles = str_aux.Split(TAG_THEME_TITLE);

                    // process each title in the split array
                    i_aux1 = 0;
                    foreach (string strThemeTitle in arrStrThemeTitles ) {

                        str_aux = strThemeTitle.Replace(TAG_THEME_TITLE_END, "");
                        str_aux = str_aux.Trim();
                        if (str_aux != "") {
                            
                            // iniatialize the information of the theme information at i_aux1 index position
                            liTitles.Add(str_aux);
                            
                            i_aux1++;

                        }//if

                    }//foreach
                    
                }//if

            }//if

            // get the ROM PACK GENERAL INFORMATION
            if (ec_ret_val.i_code >= 0) {

                // clear the ROM PACK TITLE before initializing it
                strROMInfo = "";

                i_aux1 = strInfoMetadataBlock.IndexOf(TAG_INFO);
                i_aux2 = strInfoMetadataBlock.IndexOf(TAG_INFO_END);
                if ((i_aux1 >= 0) && (i_aux2 >= 0) && (i_aux2 > i_aux1)) {

                    i_aux1 = i_aux1 + TAG_INFO.Length;
                    str_aux = strInfoMetadataBlock.Substring(i_aux1, i_aux2 - i_aux1);
                    strROMInfo = str_aux.Trim();

                }//if

            }//if

            return ec_ret_val;

        }//parseInformationMetadataBlock

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
            while (file_stream.Position < file_stream.Length){

                // read the 1 bytes corresponding to the METADATA_BLOCK_TYPE
                by_metadata_block_type = file_binary_reader.ReadByte();
                ui_read_bytes = ui_read_bytes = ui_read_bytes + 1;

                switch (by_metadata_block_type){

                    case FILE_METADATA_TITLE:

                        // read the 4 bytes corresponding to the current metada block size
                        by_read = file_binary_reader.ReadBytes(4);
                        ui_read_bytes = ui_read_bytes = ui_read_bytes + 4;
                        AuxFuncs.convert4BytesToUInt32(by_read, ref ui32_metadata_size);

                        // read the ui32_metadata_size bytes of the current metadata block
                        by_read = file_binary_reader.ReadBytes((int)ui32_metadata_size);

                        // convert the read array of bytes to a string
                        this.themes.strROMTitle = ascii.GetString(by_read);
                        this.themes.strROMTitle = this.themes.strROMTitle.Remove(this.themes.strROMTitle.Length - 1);//remove the '\0' at the end
                        break;

                    case FILE_METADATA_SONGS_INFO:

                        // read the 4 bytes corresponding to the current metada block size
                        by_read = file_binary_reader.ReadBytes(4);
                        ui_read_bytes = ui_read_bytes = ui_read_bytes + 4;
                        AuxFuncs.convert4BytesToUInt32(by_read, ref ui32_metadata_size);

                        // read the ui32_metadata_size bytes of the current metadata block
                        by_read = file_binary_reader.ReadBytes((int)ui32_metadata_size);

                        // convert the read array of bytes to a string
                        this.themes.strROMInfo = ascii.GetString(by_read);
                        this.themes.strROMInfo = this.themes.strROMInfo.Remove(this.themes.strROMInfo.Length - 1);//remove the '\0' at the end
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

            // JBR 2024-05-03 Deberia comprobarse si no ha habido algun error antes de ejecutar lo sisguientes pasos
            // y si ha habido algun error entonces se deberían resetear las estructuras.

            return ec_ret_val;

        }//loadDRP_ROMPACKv01

        /*******************************************************************************
        * @brief  Loads data from a file in ROMPACKv02 format and stores it into the  
        * drivePackData object.
        * @param[in] file_stream binary file stream
        * @param[in] file_binary_reader  binary stream reader
        * @param[in] ui_read_bytes number of byts read from the file 
        * @param[out] ui_read_bytes the number of bytes read from of the file
        * @return >=0 file has been succesfully loaded into the object, <0 an error 
        * occurred 
        *******************************************************************************/
        public ErrCode loadDRP_ROMPACKv02(ref FileStream file_stream, ref BinaryReader file_binary_reader, ref uint ui_read_bytes) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            ASCIIEncoding ascii = new ASCIIEncoding();
            List<string> liTitlesAux = new List<string>();
            string strROMTitleAux = "";
            string strROMInfoAux = "";
            System.UInt32 ui32_metadata_size = 0;
            byte by_metadata_block_type = 0;
            byte[] by_read = null;
            string str_aux = "";
            int iAux = 0;


            // JBR 2024-05-03 Dentro del while se deberia comprobar en todo momento que no ha habido ningun
            // error y si lo ha habido entonces abortar la lectura y resetar todas las estructuras 

            // process all the METADA_BLOCKS in the file
            while (file_stream.Position < file_stream.Length) {

                // read the 1 bytes corresponding to the METADATA_BLOCK_TYPE
                by_metadata_block_type = file_binary_reader.ReadByte();
                ui_read_bytes = ui_read_bytes = ui_read_bytes + 1;

                switch (by_metadata_block_type) {

                    // FILE_METADATA_TITLE metada block was removed in drpV2 version

                    case FILE_METADATA_SONGS_INFO:

                        // read the 4 bytes corresponding to the current metada block size
                        by_read = file_binary_reader.ReadBytes(4);
                        ui_read_bytes = ui_read_bytes = ui_read_bytes + 4;
                        AuxFuncs.convert4BytesToUInt32(by_read, ref ui32_metadata_size);

                        // read the ui32_metadata_size bytes of the current metadata block
                        by_read = file_binary_reader.ReadBytes((int)ui32_metadata_size);

                        // convert the read array of bytes to a string
                        str_aux = ascii.GetString(by_read);

                        // parse the content of the read string and load it into the corresponding variables
                        ec_ret_val = parseInformationMetadataBlock(str_aux, ref strROMTitleAux,ref liTitlesAux, ref strROMInfoAux);
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

            // JBR 2024-05-03 Deberia comprobarse si no ha habido algun error antes de ejecutar lo sisguientes pasos
            // y si ha habido algun error entonces se deberían resetear las estructuras.

            if (ec_ret_val.i_code >= 0) {

                // once the themes code has been loaded into the the internal structure it is time
                // to update the internal structure with the information read from the Information
                // Metada Block: that is the ROM title, the titles and index of the differnt themes
                // and the ROM general information.
                themes.strROMTitle = strROMTitleAux;
                themes.strROMInfo = strROMInfoAux;
                
                iAux = 0;
                while ( (ec_ret_val.i_code >= 0) && (iAux< liTitlesAux.Count)) {
                    ec_ret_val = themes.AddNewAt(iAux);
                    if (ec_ret_val.i_code >= 0) { 
                        themes.liThemesCode[iAux].Idx = iAux;
                        themes.liThemesCode[iAux].Title = liTitlesAux[iAux];
                        iAux++;
                    }
                }//while
            
            }//if (ec_ret_val.i_code >= 0

            return ec_ret_val;

        }//loadDRP_ROMPACKv02

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
                    if (str_aux == "ROMPACKv01\0"){

                        ec_ret_val = loadDRP_ROMPACKv01(ref file_stream, ref file_binary_reader, ref ui_read_bytes);

                    } else if (str_aux == "ROMPACKv02\0") {

                        ec_ret_val = loadDRP_ROMPACKv02(ref file_stream, ref file_binary_reader, ref ui_read_bytes);

                    } else{

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

                    themes.strROMTitle = "RO-XXX Enter RO-XXX cart title here.";
                    themes.strROMInfo = "Enter the RO-XXX general information here: year, author, producer, other observations...\r\n";
                    themes.liThemesCode.Clear();

                    // read all the bytes of the specified binary file and store them into the themes object in memory
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
        * files is that DRP contain extra information like the rom name, theme titles...
        * while BIN files only containt the raw content of the original ROM PACK cart.
        * @param[in] str_save_file with the name of the file to save the ROM content in
        * DRP file format.
        * @return >=0 file has been succesfully saved, <0 an error occurred  
        *******************************************************************************/
        public ErrCode saveDRPFile(string str_save_file){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream = null;
            BinaryWriter file_binary_writer = null;
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] by_aux = null;
            byte[] by_title_length = new byte[4];
            byte[] by_rom_info = null;
            byte[] by_rom_info_length = new byte[4];
            string str_aux = "";
            string str_content = "";


            // before saving the DRP, build the latest code of all the themes channels 
            str_aux = "Building all themes code before saving the file...";
            statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + str_aux, false);
            ec_ret_val = this.buildROMPACK();

            if (ec_ret_val.i_code >= 0) {

                file_stream = new FileStream(str_save_file, FileMode.Create);
                file_binary_writer = new BinaryWriter(file_stream);

                if (file_binary_writer == null) {
                    ec_ret_val = cErrCodes.ERR_FILE_CREATING;
                }//if

            }//if

            if (ec_ret_val.i_code >= 0){

                // save version information
                by_aux = Encoding.ASCII.GetBytes("ROMPACKv02\0");
                file_binary_writer.Write(by_aux);

                // calculate the data_offset according to the length of the title a theme information metadata fields
                // 11 bytes of the ROM PACK version ( 10 for version + 1 for the '\0')
                // ROM PACK METADATA: 0x02 FILE_METADATA_SONGS_INFO
                // 01 byte type of the title METADATA field: themes information
                // 04 bytes of the length of the ROM PACK METADATA field ( it does not include the type and length bytes, only the data bytes )
                // n bytes of the themes information ROM PACK METADATA field
                // ROM PACK METADATA: 0x03 FILE_METADATA_SONGS_ROM
                // 01 byte type of the title METADATA field: themes information
                // 04 bytes of the length of the ROM PACK METADATA field ( it does not include the type and length bytes, only the data bytes )
                // n bytes of the themes information ROM PACK METADATA field
                // ...

                // save the SONGS_INFO METADATA ( FILE_METADATA_SONGS_INFO field ):
                // prepare the string that will be written into the METADATA block with all the titles and ROM general info
                str_aux = TAG_ROM_INFO;
                // add the ROM Title (ROM name) to the SONGS_INFO METADATA block
                str_content = cleanStringForFile(this.themes.strROMTitle);
                str_aux = str_aux + TAG_ROM_TITLE + str_content + TAG_ROM_TITLE_END;
                // add the list of titles of the themes in the ROM to the SONGS_INFO METADATA block
                str_aux = str_aux + TAG_ROM_TITLE_LIST;
                foreach (ThemeCode themeAux in this.themes.liThemesCode) {
                    str_content = cleanStringForFile(themeAux.Title);
                    str_aux = str_aux + TAG_THEME_TITLE + str_content + TAG_THEME_TITLE_END;
                }
                str_aux = str_aux + TAG_ROM_TITLE_LIST_END;
                // add the general information of the ROM to the SONGS_INFO METADAT block
                str_content = cleanStringForFile(this.themes.strROMInfo);
                str_aux = str_aux + TAG_INFO + str_content + TAG_INFO_END;
                str_aux = str_aux + TAG_ROM_INFO_END;

                by_rom_info = Encoding.ASCII.GetBytes(str_aux + '\0');
                // 1 byte - METADATA type - type = FILE_METADATA_TITLE
                file_binary_writer.Write((byte)FILE_METADATA_SONGS_INFO);
                // 4 byte - METADATA field length
                by_aux = new byte[4];// re initialize the auxiliary array to 4 bytes
                AuxFuncs.convertUInt32To4Bytes((uint)by_rom_info.Length, by_aux);
                file_binary_writer.Write(by_aux);
                // n byte - METADATA field content
                file_binary_writer.Write(by_rom_info);

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
        * files is that DRP contain extra information like the rom name, theme titles...
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
        * information   f different themes and their cahnnels (M1,M2, chords ...) and 
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
            int ithemesAddrBaseIdx =0; // address where start the address of the different themes
            int iM1ChanStartAddr = 0; // processed theme M1 channel start address
            int iM1ChanEndAddress = 0;
            int iM2ChanStartAddr = 0; // processed theme M2 channel start address
            int iM2ChanEndAddress = 0;
            int iChordChanStartAddr = 0; // processed theme Chord channel start address
            int iChordChanEndAddress = 0;
            int iSongEndAddr = 0; // address where the theme ends and next theme starts
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
            byte[] byArrWorkDataHeader = { 0x5A, 0x00, 0x00, 0x0D, 0xF2, 0x00 };
            byte[] byArrEndOfROMPACK = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x32, 0x38, 0x39, 0x60, 0x74, 0x38, 0xBA, 0x20, 0x36, 0x72, 0xB4, 0x72, 0x74, 0x39, 0xB2, 0x38, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00 };

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
            //?? ??:----:    the start of each of the N themes
            //   ??:----:
            for (iThemeIdxAux =0; iThemeIdxAux < uiNumThemes; iThemeIdxAux++) {
                ui32Aux = (UInt32)(uiThemesStartAddresses[iThemeIdxAux]*2);
                AuxFuncs.convertUInt32To4BytesReversed(ui32Aux, arr4ByAux);
                arrByROM[iArrIdx] = arr4ByAux[0]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[1]; iArrIdx++;
                arrByROM[iArrIdx] = arr4ByAux[2]; iArrIdx++;
            }
            ithemesAddrBaseIdx = iArrIdx; // get the index of the themes addresses base index

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
                iM1ChanStartAddr = iArrIdx + I_PIECE_HEADER_SIZE; // processed theme M1 channel start address
                iM2ChanStartAddr = iM1ChanStartAddr + (themes.liThemesCode[iThemeIdxAux].liM1CodeInstr.Count* I_MELODY_CODE_ENTRY_SIZE); // processed theme M2 channel start address
                iChordChanStartAddr = iM2ChanStartAddr + (themes.liThemesCode[iThemeIdxAux].liM2CodeInstr.Count * I_MELODY_CODE_ENTRY_SIZE); // processed theme Chord channel start address
                iSongEndAddr = iChordChanStartAddr + (themes.liThemesCode[iThemeIdxAux].liChordCodeInstr.Count * I_CHORDS_CODE_ENTRY_SIZE); // address where the theme/theme ends and next theme starts

                // calculate the END address of each theme's channel by using the other read addresses
                iM1ChanEndAddress = iM2ChanStartAddr - 1;
                iM2ChanEndAddress = iChordChanStartAddr - 1;
                iChordChanEndAddress = iSongEndAddr;

                // place an informative message for the user in the logs. The "+1" in (iXXXChanEnAddress * 2)+1)  is because when the byte address is converted to nibble addresses the last nibble of the byte is the second nibble of the last byte, not the first nibble.
                iAux = iM1ChanEndAddress - iM1ChanStartAddr;
                strAux = "Theme " + iThemeIdxAux + " M1 channel address range is 0x" + (iM1ChanStartAddr * 2).ToString("X6") + " - 0x" + ((iM1ChanEndAddress * 2)+1).ToString("X6") + "(" + iAux.ToString() + "bytes).";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);
                iAux = iM2ChanEndAddress - iM2ChanStartAddr;
                strAux = "Theme " + iThemeIdxAux + " M2 channel address range is 0x" + (iM2ChanStartAddr * 2).ToString("X6") + " - 0x" + ((iM2ChanEndAddress * 2)+1).ToString("X6") + "(" + iAux.ToString() + "bytes).";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);
                iAux = iChordChanEndAddress - iChordChanStartAddr;
                strAux = "Theme " + iThemeIdxAux + " Chords channel address range is 0x" + (iChordChanStartAddr * 2).ToString("X6") + " - 0x" + ((iChordChanEndAddress * 2)+1).ToString("X6") + "(" + iAux.ToString() + "bytes).";
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
                    arrByROM[iArrIdx] = melodyCodeEntryAux.By0AsByte(); iArrIdx++;
                    arrByROM[iArrIdx] = melodyCodeEntryAux.By1AsByte(); iArrIdx++;
                    arrByROM[iArrIdx] = melodyCodeEntryAux.By2AsByte(); iArrIdx++;

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
                    arrByROM[iArrIdx] = melodyCodeEntryAux.By0AsByte(); iArrIdx++;
                    arrByROM[iArrIdx] = melodyCodeEntryAux.By1AsByte(); iArrIdx++;
                    arrByROM[iArrIdx] = melodyCodeEntryAux.By2AsByte(); iArrIdx++;

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
                    arrByROM[iArrIdx] = chordCodeEntryAux.By0AsByte(); iArrIdx++;
                    arrByROM[iArrIdx] = chordCodeEntryAux.By1AsByte(); iArrIdx++;

                    iAux++;

                }//while

                // place an informative message for the user in the logs
                strAux = "Theme " + iThemeIdxAux + " Chords channel added " + uiTotalInstructions + " commands (" + (uiTotalInstructions * I_CHORDS_CODE_ENTRY_SIZE).ToString() + "bytes).";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_BUILD_ROM + strAux, false);

                iThemeIdxAux++;

            }

            // in order that the ROM PACK cartridge end string finishes at a 0x..7FF or 0x..FFF multiple address: pad 
            // the ROM content with "0x00"s until an address multiple of 0x..7E0 or 0x..FE0 is reached and then add 
            // the 32 bytes of the ROMPACK cartridge end string, so the ROM pack content finishes at 0x..7FF or 0x..FFF
            while ( ((iArrIdx & 0xFFF) != 0x7E0)  && ((iArrIdx & 0xFFF) != 0xFE0) ) {
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
            int iThemeIdxAux = 0;
            ThemeCode themeCodeAux = null;
            MChannelCodeEntry melodyCodeEntryAux = null;
            ChordChannelCodeEntry chordCodeEntryAux = null;
            uint uiInstrCtr = 0;
            string strAux = "";
            int iAux = 0;
            

            if (dynbyprMemoryBytes.Length == 0) {
                ec_ret_val = cErrCodes.ERR_DECODING_EMPTY_ROM;
            }

            if (ec_ret_val.i_code >= 0) {

                // first of all delete all the channels instructions of each theme in the list of Themes 
                // themes.deleteAllThemesInstructions();

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

                // place an informative message for the user in the loDecoded ROM PACK contains 0xgs
                strAux = "Calculating the start address of each of the " + uiNumThemes + " themes...";
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
                iThemeIdxAux = 0;
                while ( (iThemeIdxAux < uiNumThemes) && (ec_ret_val.i_code>=0)) {

                    // get the start addres of th theme ( 3 bytes = 6 nibbles )
                    arr4ByAux[0] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * iThemeIdxAux) +0];
                    arr4ByAux[1] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * iThemeIdxAux) +1];
                    arr4ByAux[2] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * iThemeIdxAux) +2];
                    arr4ByAux[3] = 0;
                    AuxFuncs.convert4BytesReversedToUInt32(arr4ByAux, ref uiThemesStartAddresses[iThemeIdxAux]); 
                    uiThemesStartAddresses[iThemeIdxAux] = uiThemesStartAddresses[iThemeIdxAux] / 2;//divide by 2 to convert from nibble address to byte address

                    // place an informative message for the user in the logs
                    strAux = "Theme " + iThemeIdxAux + " start address at 0x" + (uiThemesStartAddresses[iThemeIdxAux] * 2).ToString("X6") + ".";
                    statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                    iThemeIdxAux++;

                }//while

                // get end head vacant address ( 3 bytes = 6 nibbles )
                arr4ByAux[0] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * iThemeIdxAux) + 0];
                arr4ByAux[1] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * iThemeIdxAux) + 1];
                arr4ByAux[2] = arrByROM[I_OFFSET_THEMES_START_ADDRESS + (I_THEME_START_ADDRESS_SIZE * iThemeIdxAux) + 2];
                arr4ByAux[3] = 0;
                AuxFuncs.convert4BytesReversedToUInt32(arr4ByAux, ref uiEndHeadAddrVacantArea);               
                uiEndHeadAddrVacantArea = uiEndHeadAddrVacantArea / 2;//divide by 2 to convert from nibble address to byte address

                // place an informative message for the user in the logs
                strAux = "End vacant address at 0x" + (uiEndHeadAddrVacantArea * 2).ToString("X6") + ".";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                // place an informative message for the user in the loDecoded ROM PACK contains 0xgs
                strAux = "Calculating the address range of each of the " + uiNumThemes + " themes...";
                statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                // calculate the END address of each theme by using other read addresses
                iThemeIdxAux = 0;
                while ((iThemeIdxAux < uiNumThemes) && (ec_ret_val.i_code >= 0)) {

                    if (iThemeIdxAux >= (uiNumThemes - 1)) {
                        // if the processed theme is the last one then the all the addresses of that 
                        // theme must be between the theme start address and just before the vacant area.
                        uiThemesEndAddresses[iThemeIdxAux] = uiBeginHeadAddrVacantArea-1;
                    } else {
                        // if the processed theme is NOT the last one, then the addresses of that 
                        // theme must be between the theme start address and the start address of
                        // the following theme
                        uiThemesEndAddresses[iThemeIdxAux] = uiThemesStartAddresses[iThemeIdxAux + 1] - 1;
                    }

                    // place an informative message for the user in the logs. The "+1" in ((uiThemesEndAddresses[uiThemeIdxAux] * 2)+1) is because when the byte address is converted to nibble addresses the last nibble of the byte is the second nibble of the last byte, not the first nibble.
                    strAux = "Theme " + iThemeIdxAux + " address range is 0x" + (uiThemesStartAddresses[iThemeIdxAux] * 2).ToString("X6")+ " - 0x" + ((uiThemesEndAddresses[iThemeIdxAux] * 2)+1).ToString("X6") + ".";
                    statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                    iThemeIdxAux++;

                }//while

                // use the addresses previously read from the ROM PACK header to process each piece (theme) in the ROM PACK,
                // creating a new Theme object with its different channels and its code for each piece in the ROM 
                iThemeIdxAux = 0;
                while ((iThemeIdxAux < uiNumThemes) && (ec_ret_val.i_code >= 0)) {

                    // place an informative message for the user in the logs
                    strAux = "Decoding theme " + iThemeIdxAux + " channels content ...";
                    statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                    // check that the theme at iThemeIdxAux already exists. If does not exist then create it and if exists keep
                    // all the Title but delete all the channels information to replace it with the content in the ROM
                    if (iThemeIdxAux<themes.liThemesCode.Count()){
                        // the theme already exists so keep its title information but clear all the instruction before adding the instructions in the binary contentn
                        themes.deleteThemeInstructions(iThemeIdxAux);
                    } else {
                        // the theme does not exist so add it 
                        ec_ret_val = themes.AddNewAt(iThemeIdxAux);
                    }

                    // get current theme M1 channel ( Melody channel ) address
                    if (ec_ret_val.i_code >= 0) {

                        // get all the information of the current processed theme, 
                        uiThemeStartAddress = uiThemesStartAddresses[iThemeIdxAux];
                        uiThemeEndAddress = uiThemesEndAddresses[iThemeIdxAux];

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

                            // place an informative message for the user in the logs.
                            strAux = "Theme " + iThemeIdxAux + " M1 channel start addres is 0x" + (uiM1ChanStartAddress * 2).ToString("X6") + ".";
                            statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

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

                            // place an informative message for the user in the logs.
                            strAux = "Theme " + iThemeIdxAux + " M2 channel start addres is 0x" + (uiM2ChanStartAddress * 2).ToString("X6") + ".";
                            statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

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

                            // place an informative message for the user in the logs.
                            strAux = "Theme " + iThemeIdxAux + " chords channel start addres is 0x" + (uiChordChanStartAddress * 2).ToString("X6") + ".";
                            statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);
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
                        
                        // confirm that the M2 channel start address is between the address range of the current Theme and is not lower than the M1 channel start address
                        }else if ( (uiM2ChanStartAddress<uiThemeStartAddress) || (uiM2ChanStartAddress<uiM1ChanStartAddress) || (uiM1ChanStartAddress > uiThemeEndAddress)) {
                            ec_ret_val = cErrCodes.ERR_DECODING_INVALID_M2_ADDRESS;
                        
                        // confirm that the Chords channel start address is between the address range of the current Theme and is not lower than the M2 channel start address
                        } else if ( (uiChordChanStartAddress < uiThemeStartAddress) || (uiChordChanStartAddress < uiM2ChanStartAddress) || (uiChordChanStartAddress > uiThemeEndAddress)) {
                            ec_ret_val = cErrCodes.ERR_DECODING_INVALID_CHORD_ADDRESS;
                        }

                    }//if (ec_ret_val.i_code >= 0)

                    if (ec_ret_val.i_code >= 0) {

                        // place an informative message for the user in the logs
                        strAux = "Loading theme " + iThemeIdxAux + " channels content to memory...";
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                        // calculate the END address of each theme's channel by using the other read addresses
                        uiM1ChanEndAddress = uiM2ChanStartAddress - 1;
                        uiM2ChanEndAddress = uiChordChanStartAddress - 1;
                        uiChordChanEndAddress = uiThemeEndAddress;

                        // place an informative message for the user in the logs.The "+1" in ((uiXXChanEndAddress * 2)+1) is because when the byte address is converted to nibble addresses the last nibble of the byte is the second nibble of the last byte, not the first nibble.                        
                        iAux = (int)((uiM1ChanEndAddress+1)*2 - uiM1ChanStartAddress*2) /2;
                        strAux = "Theme " + iThemeIdxAux + " M1 channel address range is 0x" + (uiM1ChanStartAddress * 2).ToString("X6") + " - 0x" + ((uiM1ChanEndAddress * 2)+1).ToString("X6") + " (" + iAux .ToString() + "bytes).";
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);
                        iAux = (int)((uiM2ChanEndAddress+1)*2 - uiM2ChanStartAddress*2) /2;
                        strAux = "Theme " + iThemeIdxAux + " M2 channel address range is 0x" + (uiM2ChanStartAddress * 2).ToString("X6") + " - 0x" + ((uiM2ChanEndAddress * 2) + 1).ToString("X6") + " (" + iAux.ToString() + "bytes).";
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);
                        iAux = (int)((uiChordChanEndAddress+1)*2 - uiChordChanStartAddress*2) /2;
                        strAux = "Theme " + iThemeIdxAux + " Chords channel address range is 0x" + (uiChordChanStartAddress * 2).ToString("X6") + " - 0x" + ((uiChordChanEndAddress * 2) + 1).ToString("X6") + " (" + iAux.ToString() + "bytes).";
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);
                        if (uiEndThemeMarkAddress != 0) {
                            strAux = "Theme " + iThemeIdxAux + " Theme End Mark address is 0x" + (uiEndThemeMarkAddress * 2).ToString("X6") + ".";
                        } else{
                            strAux = "Theme " + iThemeIdxAux + " has no Theme End Mark.";
                        }
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                        // get a shorter reference to the code entries of the processed theme in order to store on it the melody instructions 
                        themeCodeAux = themes.liThemesCode[iThemeIdxAux];

                        // store the melody 1 code entries into the theme's M1 channel
                        uiAuxAddress = uiM1ChanStartAddress;
                        uiInstrCtr = 0;
                        while (uiAuxAddress<=uiM1ChanEndAddress) {

                            melodyCodeEntryAux = new MChannelCodeEntry((int)uiInstrCtr, arrByROM[uiAuxAddress + 0], arrByROM[uiAuxAddress + 1], arrByROM[uiAuxAddress + 2]);
                            melodyCodeEntryAux.Parse();// update the description field of the instruction 

                            // add the code of the read M1 entry into the themes M1 (melody) channel instructions list
                            themeCodeAux.liM1CodeInstr.Add(melodyCodeEntryAux);

                            uiAuxAddress = uiAuxAddress + I_MELODY_CODE_ENTRY_SIZE;
                            uiInstrCtr++;
                        }

                        // place an informative message for the user in the logs
                        strAux = "Theme " + iThemeIdxAux + " M1 channel loaded " + uiInstrCtr + " commands (" + (uiInstrCtr* I_MELODY_CODE_ENTRY_SIZE).ToString() + "bytes)."; 
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                        // store the melody 2 ( obligato ) code entries into the theme's M2 channel
                        uiAuxAddress = uiM2ChanStartAddress;
                        uiInstrCtr = 0;
                        while (uiAuxAddress <= uiM2ChanEndAddress) {

                            melodyCodeEntryAux = new MChannelCodeEntry((int)uiInstrCtr, arrByROM[uiAuxAddress + 0], arrByROM[uiAuxAddress + 1], arrByROM[uiAuxAddress + 2]);
                            melodyCodeEntryAux.Parse();// update the description field of the instruction 

                            // add the code of the read M2 entry into the themes M2 (obligato) channel instructions list
                            themeCodeAux.liM2CodeInstr.Add(melodyCodeEntryAux);

                            uiAuxAddress = uiAuxAddress + I_MELODY_CODE_ENTRY_SIZE;
                            uiInstrCtr++;
                        }

                        // place an informative message for the user in the logs
                        strAux = "Theme " + iThemeIdxAux + " M2 channel loaded " + uiInstrCtr + " commands (" + (uiInstrCtr * I_MELODY_CODE_ENTRY_SIZE).ToString() + "bytes).";
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                        // store the CHords code entries into the theme's Chords channel
                        uiAuxAddress = uiChordChanStartAddress;
                        uiInstrCtr = 0;
                        while (uiAuxAddress <= uiChordChanEndAddress) {

                            chordCodeEntryAux = new ChordChannelCodeEntry((int)uiInstrCtr, arrByROM[uiAuxAddress + 0], arrByROM[uiAuxAddress + 1]);
                            chordCodeEntryAux.Parse();// update the description field of the instruction 

                            // add the code of the read Chord entry into the themes Chords channel instructions list
                            themeCodeAux.liChordCodeInstr.Add(chordCodeEntryAux);

                            uiAuxAddress = uiAuxAddress + I_CHORDS_CODE_ENTRY_SIZE;
                            uiInstrCtr++;
                        }

                        // place an informative message for the user in the logs
                        strAux = "Theme " + iThemeIdxAux + " Chords channel loaded " + uiInstrCtr + " commands (" + (uiInstrCtr * I_CHORDS_CODE_ENTRY_SIZE).ToString() + "bytes).";
                        statusNLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, cErrCodes.COMMAND_DECODE_ROM + strAux, false);

                    }//if (ec_ret_val.i_code >= 0)

                    // process followint theme
                    iThemeIdxAux++;

                }//while ((uiThemeIdxAux < uiNumThemes) && (ec_ret_val.i_code >= 0)) 
               
            }//if (ec_ret_val.i_code >= 0)

            return ec_ret_val;

        }//decodeROMPACKtoSongThemes

    }//class cDrivePack

    /*******************************************************************************
    *  @brief special stack based on a circular buffer. It is used to store and to 
    *  recover the states of the application with Ctr+Y, Ctrl*Z. Appart of  working
    *  as standard stack on which elements are pushed and popped (push and pop) to 
    *  and from the top of the stack (LIFO), it has other special features:
    *  - If the stack is full, the the oldest element in the stack is removed to make 
    *  place for the new one. So all elements are always placed in the stack and never
    *  returns "full stack error".
    *  - It implements two methods to read back (readBack) and to read forward (readForward) 
    *  the elements pushed into the stack without removing them.
    *  - It implements a mehtod to update the content of hte last read element.
    *  - Appart of the standard push method that adds the element to the top of the
    *  stack, it implements a method to push an element just after the last element read
    *  with the readBack or readForward command. Pushing a new element after the last 
    *  read element removes all the elements from that element to the top of the stack.
    *******************************************************************************/
    public class HistoryStack {
        const int MAX_ELEMENTS = 8;
        public Themes[] arrayThemesStates;
        int iOldestIdx;// index of the oldest element pushed into the circular buffer.
        int iFreeIdx;// when the array is not empty, the index of the following free position in the circular buffer. It should point to the element that follows the last element pushed in the stack circular buffer
        public int iCount;// number of elements pushed into the stack ciruclar buffer (from 0 to MAX_ELEMENTS)
        public int iCurrReadIdx;// index of the last element of the circular buffer read with the read back and read forward methods

        private cLogsNErrors statusNLogsRef = null;// a reference to the logs to allow the objects of this class write information into the logs.

        /*******************************************************************************
        *  @brief constructor with parameters
        *  @param[in] reference to logger object to allow writting information into the logs.
        *******************************************************************************/
        public HistoryStack(cLogsNErrors loggerRef) {

            arrayThemesStates = new Themes[MAX_ELEMENTS];
            iOldestIdx = 0;
            iFreeIdx = 0;
            iCount = 0;
            iCurrReadIdx = 0;

            statusNLogsRef = loggerRef;

        }//HistoryStack

        /***********************************************************************************************
        * @brief prints the activity history stack content in a string in a comprehensible format.
        * @return string with the parsed content of the history stack
        ***********************************************************************************************/
        public string printHistoryStack() {
            int iAux = 0;
            int stackIdx = 0;
            int themeIdx = 0;
            string strAux = "\r\n";

            stackIdx = 0;
            foreach (Themes themesAux in arrayThemesStates) {
                
                if (themesAux == null) {
                    strAux = strAux + "Stack Idx:" + stackIdx + " is null\r\n";
                } else {

                    strAux = strAux + "Stack Idx:" + stackIdx + "\r\n";

                    // print the idx of the themes selected in the themes datagridview
                    strAux = strAux + "    Themes sel Idxs:";
                    if (themesAux.liSelectedThemesDGridviewRows != null) {
                        foreach (int iIdxAux in themesAux.liSelectedThemesDGridviewRows) {
                            strAux = strAux + iIdxAux.ToString() + ", ";
                        }                   
                    }//if
                    strAux = strAux + "\r\n";
                    
                    themeIdx = 0;
                    foreach (ThemeCode iTheme in themesAux.liThemesCode) {
                        strAux = strAux + "    Theme:" + themeIdx + "\r\n";

                        // print the idx of the instructions selected in the theme's M1 channel instructions datagridview
                        strAux = strAux + "       M1 sel Idxs:";
                        if (iTheme.liSelectedM1DGridviewRows != null) {
                            foreach (int iIdxAux in iTheme.liSelectedM1DGridviewRows) {
                                strAux = strAux + iIdxAux.ToString() + ", ";
                            }
                        }//if
                        strAux = strAux + "\r\n";

                        // print the idx of the instructions selected in the theme's M2 channel instructions datagridview
                        strAux = strAux + "       M2 sel Idxs:";
                        if (iTheme.liSelectedM2DGridviewRows != null) {
                            foreach (int iIdxAux in iTheme.liSelectedM2DGridviewRows) {
                                strAux = strAux + iIdxAux.ToString() + ", ";
                            }
                        }//if
                        strAux = strAux + "\r\n";

                        // print the idx of the instructions selected in the theme's Chords channel instructions datagridview
                        strAux = strAux + "       Chord sel Idxs:";
                        if (iTheme.liSelectedChordDGridviewRows != null) {
                            foreach (int iIdxAux in iTheme.liSelectedChordDGridviewRows) {
                                strAux = strAux + iIdxAux.ToString() + ", ";
                            }
                        }//if
                        strAux = strAux + "\r\n";

                        themeIdx++;
                    
                    }//foreach                   

                }
                stackIdx++;

            }

            return strAux;

        }//printHistoryStack

        /*******************************************************************************
        *  @brief clears and reinitializes the content of the HistoryStack
        *******************************************************************************/
        public void Clear() {

            iOldestIdx = 0;
            iFreeIdx = 0;
            iCount = 0;
            iCurrReadIdx = 0;

        }//Clear

        /*******************************************************************************
        *  @brief custom modulo function to use insted of '%' C# operator. The modulo 
        *  operation returns the remainder or signed remainder of a division, after one 
        *  number is divided by another. It is implemented because the C# % modulo operator 
        *  does not properly treat the negative module parameters.
        *  @param[in] x dividend
        *  @param[in] y divisior
        *  @return the remainder of the division
        *******************************************************************************/
        int mod(int x, int m) {

            int r = x % m;
            return r < 0 ? r + m : r;

        }//mod

        /*******************************************************************************
        *  @brief receives an element and pushes it into the circular stack. If there is
        *  no space for a new themes struct it remoeves the oldest one to make place for
        *  the new one.
        *  @param[in] elementToPush element to push into the circular stack
        *******************************************************************************/
        public void push(Themes elementToPush) {

            // check if the data structure is full and remove the oldest element if afirmative
            if (iCount == MAX_ELEMENTS) {

                // array is full: remove oldest element to make place for the new one
                iOldestIdx = mod(iOldestIdx + 1, MAX_ELEMENTS);

            } else {

                iCount++;

            }//if

            // add the element to the circular buffer
            Themes.CopyThemes(elementToPush, ref arrayThemesStates[iFreeIdx]);

            iCurrReadIdx = iFreeIdx; // set the read cursor on the last pushed element
            iFreeIdx = mod(iFreeIdx + 1, MAX_ELEMENTS);

        }//push

        /*******************************************************************************
        *  @brief receives a themes struct object and pushes it over the last read element
        *  of the circular stack, and sets it as the last pushed element removing the previous
        *  element.
        *  @param[in] elementToPush element to push into the circular stack
        *******************************************************************************/
        public void pushAfterLastRead(Themes elementToPush) {

            Themes elementPopedAux = null; // used onnly to remove the items untill the one 
            int iLastReadIdx = 0;

            // remove all elements between the top of the stack ant the one
            // that follows the last read element
            iLastReadIdx = iCurrReadIdx;
            while ( (mod(iLastReadIdx + 1, MAX_ELEMENTS) != iFreeIdx)  && (iCount != 0) ){
                pop(ref elementPopedAux);
            }

            // push the received element . It will be pushed just after the last read element
            push(elementToPush);

        }//pushAfterLastRead

        /*******************************************************************************
        *  @brief overwrites the content of the last read element with the received content.
        *  @param[in] elementToPush element to wirte over the last read element.
        *******************************************************************************/
        public void updateLastRead(Themes elementToWrite) {

            Themes elementPopedAux = null; // used onnly to remove the items untill the one 
            int iLastReadIdx = 0;

            // remove all elements between the top of the stack ant the one
            // that follows the last read element
            iLastReadIdx = iCurrReadIdx;
            while ((mod(iLastReadIdx, MAX_ELEMENTS) != iFreeIdx) && (iCount != 0)) {
                pop(ref elementPopedAux);
            }

            // push the received element . It will be pushed just after the last read element
            push(elementToWrite);

        }//updateLastRead

        /*******************************************************************************
        *  @brief removes and returns the element at the top of the stack, that is the
        *  last element pushed into the stack.
        *  @param[out] elementPoped the last element pushed into the stack
        *  @return true if there were available elements to pop ( stack was not empty ).
        *******************************************************************************/
        public bool pop(ref Themes elementPoped) {
            bool bRetValue = false;
            int iPrevIdx = 0;

          
            if (iCount == 0) {

                // array is empty
                bRetValue = false;

            } else {

                // array is not empty
                iPrevIdx = mod(iFreeIdx - 1, MAX_ELEMENTS);
                Themes.CopyThemes(arrayThemesStates[iPrevIdx], ref elementPoped);
                arrayThemesStates[iPrevIdx] = null; // clear the poped element 
                iFreeIdx = iPrevIdx;
                iCurrReadIdx = mod(iFreeIdx - 1, MAX_ELEMENTS);
                iCount--;

                bRetValue = true;

            }//if

            return bRetValue;

        }//pop

        /*******************************************************************************
         *  @brief moves the read index to the element prior to the last read element
         *  and returns its value without removing it from the stack.
         *  @param[out] elementRead the value of the element prior to the last read 
         *  value.
         *  @return true if there were available elements to pop (the stack was not empty).
         *******************************************************************************/
        public bool readBack(ref Themes elementRead) {
            bool bRetValue = false;
  
            if (iCount == 0) {

                // array is empty
                bRetValue = false;

            } else if (iCurrReadIdx == iOldestIdx) {

                // all elements of the array have been read ( read cursor is just before the
                // oldest element, so do not move cursor )
                Themes.CopyThemes(arrayThemesStates[iCurrReadIdx], ref elementRead);
                bRetValue = true;

            } else {

                iCurrReadIdx = mod(iCurrReadIdx - 1, MAX_ELEMENTS);
                Themes.CopyThemes(arrayThemesStates[iCurrReadIdx], ref elementRead);
                bRetValue = true;
            }

            return bRetValue;

        }//readBack

        /*******************************************************************************
        *  @brief moves the read index to the element following the last read element
        *  and returns its value without removing it from the stack.
        *  @param[out] elementRead The value of the element prior to the last read 
        *  value.
        *  @return true if there were available elements to pop (the stack was not empty).
        *******************************************************************************/
        public bool readForward(ref Themes elementRead) {
            bool bRetValue = false;

            if (iCount == 0) {

                // array is empty
                bRetValue = false;

            } else if (iCurrReadIdx == mod(iFreeIdx - 1, MAX_ELEMENTS)) {

                // all elements of the array have been read ( read cursos is just over the last pushed element )
                Themes.CopyThemes(arrayThemesStates[iCurrReadIdx], ref elementRead);
                bRetValue = true;

            } else {

                iCurrReadIdx = mod(iCurrReadIdx + 1, MAX_ELEMENTS);
                Themes.CopyThemes(arrayThemesStates[iCurrReadIdx], ref elementRead);
                bRetValue = true;
            }

            return bRetValue;

        }//readForward

    }//public class HistoryStack

}//drivePackEd