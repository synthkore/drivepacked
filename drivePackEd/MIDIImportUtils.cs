using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Reflection.Metadata;

// **********************************************************************************
// ****                          drivePACK Editor                                ****
// ****                         www.tolaemon.com/dpack                           ****
// ****                              Source code                                 ****
// ****                              17/05/2025                                  ****
// ****                            Jordi Bartolome                               ****
// ****                                                                          ****
// ****          IMPORTANT:                                                      ****
// ****          Using this code or any part of it means accepting all           ****
// ****          conditions exposed in: http://www.tolaemon.com/dpack            ****
// **********************************************************************************

namespace drivePackEd {

    /*******************************************************************************
    *  @brief defines the object used to store the general information or main 
    *  characteristics of each TRACK inside the imported MIDI file and other information
    *   needed to generate the theme in the ROM cartridge
    *******************************************************************************/
    public class ImportMIDITrackInfo {

        // general information of each track of the imported MIDI file
        public string strTitle;// title or other text data of the track
        public bool bMetadataTrack;// some tracks contain metadata information ( name of the theme, author ... ), while others contain musical information
        public bool bMusicTrack;// some tracks only contain metadata information ( name of the theme, author ... ), while others contain musical information
        public bool bPolyphonic;// if there are notes overlaping ( different notes playing simultanously )
        public int iNumberNotes;// the total number of notes in the track
        public int iHighestNoteCode;// the highest note in the track
        public int iLowestNoteCode;// the lowest note in the track
        // some track meta data / text data
        public string strTxtEvent;
        public string strCopyrightNotice;
        public string strTrackName;
        public string strInstrName;
        public string strLyric;
        public string strMarker;
        public string strCuePoint;
        // other information needed to generate the ROM theme channel
        public double dNotesStartTime;// the time elapsed since the theme starts and the first note of the track is played.

        public ImportMIDITrackInfo() {

            strTitle = "";
            bMetadataTrack = false;
            bMusicTrack = false;
            bPolyphonic = false;
            iNumberNotes = 0;
            iHighestNoteCode = 53;// initialize the note code with the lowest valid value 53 = F3
            iLowestNoteCode = 84;// initialize the note code with the highest valid value 84 = C6
            dNotesStartTime = 0.0;

            strTxtEvent ="";
            strCopyrightNotice = "";
            strTrackName = "";
            strInstrName = "";
            strLyric = "";
            strMarker = "";
            strCuePoint = "";

        }//ImportMIDITrackInfo

    }//ImportMIDITrackInfo

    /*******************************************************************************
    *  @brief defines the object used to store the general information or main 
    *  characteristics inside the imported MIDI file and other information needed to
    *  generate the theme in the ROM cartridge
    *******************************************************************************/
    public class ImportMIDIFileInfo{

        // general  
        public uint uiNTracks;
        public uint uiFormat;
        public uint uDivision;
        public uint uiTicksQuarterNote;
        public uint uiTicksPerFrame;
        public uint uiDeltaTimeToCasioTicks;
        // other information needed to generate the ROM cartridge
        public int iROMM1ChanIdx;// the MIDI track number that will be assigned to ROM PACK theme channel 1 ( man melody )
        public int iROMM2ChanIdx;// the MIDI track number that will be assigned to ROM PACK theme channel 2 ( obligatto )
        public int iROMChordsChanIdx;// the MIDI track number that will be assigned to the chords channel
        public int iROMMetadaChanIdx;// the MIDI track number that will be used to get other theme metadata
        public MChannelCodeEntry.t_Instrument tInstrM1Instrument;// the instrument that must be used when importing M1 channel notes
        public MChannelCodeEntry.t_Instrument tInstrM2Instrument;// the instrument that must be used when importing M2 channel notes
        public ChordChannelCodeEntry.t_RythmStyle tChordsRythm;// the rythm to configure in the chords channel
        public MChannelCodeEntry.t_Time tTimeMark;// the time mark to set in the header of the M1 channel
        public int iKey;// the musical key code  to set in the header of the M1 channel
        public int iTempo;
        public int iRythmDiscrimination;// the duration of time discrimination at the beggining of the theme, or 0 if there is no rythm discrimination

        public bool bNoGenChanBeginEnd;// determines if the beggining and the ending instructions of each theme channel must be added when importing the MIDI tracks content to the corresponding channels.If true the beginning and ending will be added, if false the beginning and ending instructions will not be added.
        
        public List<ImportMIDITrackInfo> liTracks; // list with the general information of each specific MIDI track in the file

        // default constructor
        public ImportMIDIFileInfo() {

            uiNTracks = 0;
            uiFormat = 0;
            uDivision = 0;
            uiTicksQuarterNote = 0;
            uiTicksPerFrame = 0;
            uiDeltaTimeToCasioTicks = 0;

            iROMM1ChanIdx = -1;
            iROMM2ChanIdx = -1;
            iROMChordsChanIdx = -1;
            iROMMetadaChanIdx = -1;

            tInstrM1Instrument = MChannelCodeEntry.t_Instrument.PIANO;// the instrument that must be used when importing M1 channel notes
            tInstrM2Instrument = MChannelCodeEntry.t_Instrument.PIANO;// the instrument that must be used when importing M2 channel notes
            tChordsRythm = ChordChannelCodeEntry.t_RythmStyle.DISCO;// the rythm to configure in the chords channel
            tTimeMark = MChannelCodeEntry.t_Time._4x4;// the time mark to set in the header of the M1 channel
            iKey = 128;// the key to set in the header of the M1 channel            
            iTempo = 100;
            iRythmDiscrimination = 4;// the duration of the ticks before start playing the rythm ( 4 ticks 1 per quarter )

            bNoGenChanBeginEnd = false;// determines if the beggining and the ending instructions of each theme channel must be added when importing the MIDI tracks content to the corresponding channels.If true the beginning and ending will be added, if false the beginning and ending instructions will not be added.

            liTracks = new List<ImportMIDITrackInfo>();

        }//ImportMIDIFileInfo

    }//ImportMIDIFileInfo


    class MIDIImportUtils {

        /*******************************************************************************
        * @brief reads the following Variable Length value from the received file stream.
        * 
        * @param[in] file_binary_reader  with the file stream from which the function must
        * read the Variable Length value
        * @param[out] ui32_num_read_bytes with the number of bytes occupied by the Variable 
        * Length value in the file.
        * @return >=0 with the Variable Length value read from the received file.
        *
        * These values are 1 to 4 bytes long, with the most significant bit of each byte 
        * serving as a flag.The most significant bit of the final byte is set to 0, and the 
        * most significant bit of every other byte is set to 1. The highest bit to 1 is removed 
        * after having read the bit. These are some examples of variable length quantities:
        * 
        * Value    < Variable-length quantity
        * 00000000 < 00
        * 00000040 < 40
        * 0000007F < 7F
        * 00000080 < 81 00
        * 00002000 < C0 00
        * 00003FFF < FF 7F
        * 00004000 < 81 80 00
        * 00100000 < C0 80 00
        * 001FFFFF < FF FF 7F
        * 00200000 < 81 80 80 00
        * 08000000 < C0 80 80 00
        * 0FFFFFFF < FF FF FF 7F
        *
        * To read a variable length value: 
        * do
        *   read_byte = read()
        *   final_value = (final_value<<7) | (0x7F & read_byte)
        * while (read_byte&0x80!=0)
        *******************************************************************************/
        public static UInt32 readMIDIVariableLength(BinaryReader file_binary_reader, ref UInt32 ui32_num_read_bytes) {
            UInt32 ui32RetVal = 0;
            byte[] by_read = null;

            ui32RetVal = 0;
            ui32_num_read_bytes = 0;
            if (file_binary_reader != null) {

                do {

                    by_read = file_binary_reader.ReadBytes(1);
                    ui32_num_read_bytes = ui32_num_read_bytes + 1;
                    ui32RetVal = (UInt16)((ui32RetVal << 7) | (0x7F & by_read[0])); // 0x7F& to clear the highest bit in case it is '1'

                } while ((by_read[0] & 0x80) != 0);

            }

            return ui32RetVal;

        }//readMIDIVariableLength

        /*******************************************************************************
        * @brief Method that reads the specified MIDI file and obtains its most relevant
        * information and cahracteristics. The retrieved information is stored into the 
        * corresponding fields of the received midiFInfo structure.
        * 
        * @param[in] strMidiFileName with the name of the MIDI file to load.
        * @param[out] midiFInfo structure where the information retrieved from the MIDI file
        * will be stored in.
        * 
        * @return >=0 file has been succesfully loaded into the object, <0 an error 
        * occurred 
        * 
        * @note recomended link to understand MIDI file structure: 
        * https://www.music.mcgill.ca/~ich/classes/mumt306/StandardMIDIfileformat.html
        * PDF document: Standard MIDI Files 1.0
        *******************************************************************************/
        public ErrCode getImportedMIDIFileInfo(string strMidiFileName, ref ImportMIDIFileInfo midiFInfo) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream_reader = null;
            BinaryReader file_binary_reader = null;
            ASCIIEncoding ascii = new ASCIIEncoding();
            uint ui_total_read_bytes = 0;
            uint ui_chunk_read_bytes = 0;
            byte[] by_read = null;
            byte by_last_run_status = 0x00;
            byte by_meta_event = 0x00;
            int iTrackCtr = 0;
            UInt32 ui32ChunkLength = 0;
            UInt16 ui16Format = 0;
            UInt16 ui16NumTracks = 0;
            UInt16 ui16Division = 0;
            UInt16 ui16TicksQuarterNote = 0;
            UInt16 ui16DeltaTimeToCasioTicks = 0;
            Int16 i16NegSMPTEFormat = 0;
            UInt16 ui16TicksPerFrame = 0;
            UInt32 ui32DeltaTime = 0;
            int iByMidiCmdCode = 0;
            int iByMidiCmdChan = 0;
            UInt32 uiMetaEventLength = 0;
            double dTrackTime = 0;
            // variables used to process the read notes and generate the corresponding CASIO ROM pack melody channel note instructions
            double dOnTrackTime = 0;
            double dOffTrackTime = 0;
            byte byCurrentNote = 0;
            UInt32 ui32Aux = 0;
            double dAux = 0;
            string str_aux = "";
            ImportMIDITrackInfo midiTrackInfoAux = null;
            // variables only for debugging
            bool b_Midi_dbg = false;// flag to indicate if MIDI debug information must be generated or not
            StreamWriter file_str_writer_dbg = null; // only for debuggin purposes
            string str_dbg_out = "";

            try {

                // create the object used to return the MIDI file information
                midiFInfo = new ImportMIDIFileInfo();

                // check that the received midi file to read exists
                if (!File.Exists(strMidiFileName)) {
                    ec_ret_val = cErrCodes.ERR_FILE_NOT_EXIST;
                }

                if (ec_ret_val.i_code >= 0) {
                    
                    // create the debug ouput file with the MIDI file parsed content
                    // file_str_writer_dbg = new StreamWriter("debug_out.txt");

                    // open the input MIDI binary file
                    file_stream_reader = new FileStream(strMidiFileName, FileMode.Open);
                    file_binary_reader = new BinaryReader(file_stream_reader);

                    if (file_binary_reader == null) {
                        ec_ret_val = cErrCodes.ERR_FILE_OPENING;
                    }

                }// if (ec_ret_val >= 0) 

                if (ec_ret_val.i_code >= 0) {

                    // read the beginning of the MIDI file and check that it corresponds to the MIDI HEADER CHUNK start tag
                    // "mthd". The header chunk strcuture is: < Header Chunk > = MThd<length> < format >< ntrks >< division >

                    // str_dbg_out = "\r\n### " + strMidiFileName + ":";
                    // file_str_writer_dbg.Write(str_dbg_out);

                    // check that the MIDI file has the valid HEADER CHUNK "Mthd"

                    // read and check file format verstion tag
                    ui_total_read_bytes = 0;

                    // get the file format and version tag
                    file_stream_reader.Seek(0, SeekOrigin.Begin);
                    by_read = file_binary_reader.ReadBytes(4);
                    ui_total_read_bytes = ui_total_read_bytes + 4;
                    str_aux = ascii.GetString(by_read);

                    if (str_aux.ToLower() != "mthd") { // "MThd"
                        ec_ret_val = cErrCodes.ERR_FILE_IMPORT_PARSING_MIDI_INFO;
                    }

                }// if (ec_ret_val >= 0) 

                if (ec_ret_val.i_code >= 0) {

                    // The "mthd" tag has been found so process the MIDI file HEADER CHUNK content:
                    // <Header Chunk> = MThd<length><format><ntrks><division>

                    // read the 4 bytes corresponding to the <length> of the chunk
                    by_read = file_binary_reader.ReadBytes(4);
                    ui_total_read_bytes = ui_total_read_bytes + 4;
                    AuxUtils.convert4BytesBEToUInt32(by_read, ref ui32ChunkLength);

                    // str_dbg_out = "\r\nHeader Length: 0x" + ui32ChunkLength.ToString("X4");
                    // file_str_writer_dbg.Write(str_dbg_out);

                    // get the <format>
                    by_read = file_binary_reader.ReadBytes(2);
                    ui_total_read_bytes = ui_total_read_bytes + 2;
                    AuxUtils.convert2BytesBEToUInt16(by_read, ref ui16Format);
                    midiFInfo.uiFormat = ui16Format;

                    // str_dbg_out = "\r\nFormat: " + ui16Format.ToString();
                    // file_str_writer_dbg.Write(str_dbg_out);

                    // get the <ntrks>
                    by_read = file_binary_reader.ReadBytes(2);
                    ui_total_read_bytes = ui_total_read_bytes + 2;
                    AuxUtils.convert2BytesBEToUInt16(by_read, ref ui16NumTracks);
                    midiFInfo.uiNTracks = ui16NumTracks;

                    // str_dbg_out = "\r\nTracks: " + ui16Format.ToString();
                    // file_str_writer_dbg.Write(str_dbg_out);

                    // get the <division>
                    by_read = file_binary_reader.ReadBytes(2);
                    ui_total_read_bytes = ui_total_read_bytes + 2;
                    AuxUtils.convert2BytesBEToUInt16(by_read, ref ui16Division);
                    midiFInfo.uDivision = ui16Division;

                    // check bit 15 of <division>
                    if ((ui16Division & 0x8000) == 0) {
                        // bit 15 == 0 -> bits 14 - 0: ticks per quarter-note ( or also Ticks per Beat )
                        ui16TicksQuarterNote = (UInt16)(ui16Division & 0x7FFF);
                        // use the read ticks per quarter-note as a conversion factor
                        ui16DeltaTimeToCasioTicks = ui16TicksQuarterNote;
                        midiFInfo.uiTicksQuarterNote = ui16TicksQuarterNote;
                        midiFInfo.uiDeltaTimeToCasioTicks = ui16DeltaTimeToCasioTicks;
                    } else {
                        // bit 15 == 1 -> bits 14 - 8: negative SMPTE format : bit 7 - 0: ticks per frame
                        i16NegSMPTEFormat = (Int16)((ui16Division & 0xEF00) >> 8);
                        i16NegSMPTEFormat = (Int16)AuxUtils.convertNBitsInC2ToInt32((UInt32)i16NegSMPTEFormat, 7);
                        ui16TicksPerFrame = (UInt16)(ui16Division & 0x000F);
                        // use the read ticks ticksPerFrame as  a conversion factor
                        ui16DeltaTimeToCasioTicks = ui16TicksPerFrame;
                        midiFInfo.uiTicksPerFrame = ui16TicksPerFrame;
                        midiFInfo.uiDeltaTimeToCasioTicks = ui16DeltaTimeToCasioTicks;
                    }

                    // if ((ui16Format != 0) && (ui16NumTracks != 1)) {
                    //     ec_ret_val = -1;// the MID file format is not supported
                    // }

                }// if (ec_ret_val >= 0) 

                // keep reading the MIDI file until all the MIDI tracks have been processed
                while ((ec_ret_val.i_code >= 0) && (iTrackCtr < ui16NumTracks)) {

                    // process the content of the following TRACK CHUNK in the MIDI file

                    // str_dbg_out =  "\r\n### Track " + iTrackCtr.ToString("D2");
                    // file_str_writer_dbg.Write(str_dbg_out);

                    // create a new MIDI track info object and add it to the MIDI file info structure
                    midiTrackInfoAux = new ImportMIDITrackInfo();
                    midiFInfo.liTracks.Add(midiTrackInfoAux);

                    byCurrentNote = 0;
                    dTrackTime = 0;
                    dOnTrackTime = -1;
                    dOffTrackTime = -1;

                    // check the following TRACK CHUNK
                    by_read = file_binary_reader.ReadBytes(4);
                    ui_total_read_bytes = ui_total_read_bytes + 4;
                    str_aux = ascii.GetString(by_read);

                    if (str_aux.ToLower() != "mtrk") {// "MTrk"
                        ec_ret_val = cErrCodes.ERR_FILE_IMPORT_PARSING_MIDI_INFO;
                    }

                    if (ec_ret_val.i_code >= 0) {

                        // process the MIDI file TRACK CHUNK content: <Track Chunk> = MTrk<length><MTrk event>+

                        // read the 4 bytes corresponding to the <length> of the chunk
                        by_read = file_binary_reader.ReadBytes(4);
                        ui_total_read_bytes = ui_total_read_bytes + 4;
                        AuxUtils.convert4BytesBEToUInt32(by_read, ref ui32ChunkLength);

                        // str_dbg_out = "\r\nTrack Chunk Length: 0x" + ui32ChunkLength.ToString("X4");
                        // file_str_writer_dbg.Write(str_dbg_out);

                        ui_chunk_read_bytes = 0;

                    }// if (ec_ret_val >= 0) 

                    // keep reading all the <MTrk event>s (<MTrk event> = <delta-time><event>) of the current processed MIDI track
                    while ((ec_ret_val.i_code >= 0) && (ui_chunk_read_bytes < ui32ChunkLength)) {

                        // process all the <MTrk event> in the track

                        // get the <delta-time>: the delta-times of MIDI track events are stored as "Variable-length" values.
                        ui32DeltaTime = readMIDIVariableLength(file_binary_reader, ref ui32Aux);
                        ui_total_read_bytes = ui_total_read_bytes + ui32Aux;
                        ui_chunk_read_bytes = ui_chunk_read_bytes + ui32Aux;
                        dAux = ((double)ui32DeltaTime / (double)ui16DeltaTimeToCasioTicks);
                        dTrackTime = dTrackTime + dAux;

                        // get the event bytes.
                        // <event> = <MIDI event> | <sysex event> | <meta-event>

                        // start by the first byte to identify the type of the event
                        by_read = file_binary_reader.ReadBytes(1);
                        ui_total_read_bytes = ui_total_read_bytes + 1;
                        ui_chunk_read_bytes = ui_chunk_read_bytes + 1;

                        // check if read byte highest bit is '1' or not in order to confirm that the byte read after the Delta Time
                        // data corresponds to a MIDI RunningStatus message or to a standard MIDI message with its Status byte etc
                        // ( status bytes have its hihgest bit to 1 )
                        if ((by_read[0] & 0x80) == 0) {
                            // hihgest bit is not set to '1' so it is MIDI RunningStatus nessage: move back the read pointer to
                            // the previous byte and use the previous status byte as current Status Byte
                            file_stream_reader.Seek(-1, SeekOrigin.Current);
                            ui_total_read_bytes = ui_total_read_bytes - 1;
                            ui_chunk_read_bytes = ui_chunk_read_bytes - 1;

                            by_read[0] = by_last_run_status;
                        }

                        // get the 4 upper bits with the MIDI code and process the command
                        iByMidiCmdCode = (int)(by_read[0] & 0xF0);
                        switch (iByMidiCmdCode) {

                            case 0x80: // 0x8X = NOTE OFF ( Voice Messages )
                                // keep in memory current Status byte in case the following messages correspond to Running Status messages
                                by_last_run_status = by_read[0];

                                iByMidiCmdChan = (int)(by_read[0] & 0x0F);
                                // read the 2 following bytes of the MIDI NOTE OFF instruction
                                by_read = file_binary_reader.ReadBytes(2);
                                ui_total_read_bytes = ui_total_read_bytes + 2;
                                ui_chunk_read_bytes = ui_chunk_read_bytes + 2;

                                dOffTrackTime = dTrackTime;

                                // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                // str_dbg_out = str_dbg_out + " Note Off:" + midiNoteCodeToString(by_read[0]);
                                // str_dbg_out = str_dbg_out + " Ch:" + iByMidiCmdChan.ToString("D1") + " At:" + dAux.ToString("0.00");
                                // file_str_writer_dbg.Write(str_dbg_out);
                                break;

                            case 0x90: // 0x9X = NOTE ON ( Voice Messages )
                                // keep in memory current Status byte in case the following messages correspond to Running Status messages
                                by_last_run_status = by_read[0];

                                iByMidiCmdChan = (int)(by_read[0] & 0x0F);
                                // read 2 following NOTE ON MIDI instruction bytes
                                by_read = file_binary_reader.ReadBytes(2);
                                ui_total_read_bytes = ui_total_read_bytes + 2;
                                ui_chunk_read_bytes = ui_chunk_read_bytes + 2;

                                // check the velocity byte because a NOTE ON MIDI event with velocity byte == 0 is equivalent to a NOTE OFF event 
                                if (by_read[1] == 0) {
                                    // the processed NOTE ON MIDI event is in fact a NOTE OFF ( it has velocity byte == 0 )
                                    dOffTrackTime = dTrackTime;

                                    // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                    // str_dbg_out = str_dbg_out + " Note On>Off:" + midiNoteCodeToString(by_read[0]);
                                    // str_dbg_out = str_dbg_out + " Ch:" + iByMidiCmdChan.ToString("D1") + " At:" + dAux.ToString("0.00");
                                    // file_str_writer_dbg.Write(str_dbg_out);
                                } else {
                                    // the processed NOTE ON MIDI event is a real NOTE ON

                                    // as the current track contains note/music instructions set to true the flag that indicates that current track has musical data
                                    midiTrackInfoAux.bMusicTrack = true;

                                    // call the method that processes the MIDI NOTE ON event and stores the processed note into the  
                                    // corresponding theme channel with the corresponding note and rest duration:
                                    // ec_ret_val = store_MIDI_NOTE_ON(iThemeIdxToInsert, iThemeDestChanIdx, byCurrentNote, dTrackTime, dOnTrackTime, dOffTrackTime);

                                    // determine whether it's the first note of the song by checking if a previous note was already active
                                    if (byCurrentNote == 0) {
                                        // no previous note was being processed so the received note is the first note of the current track, and
                                        // will use it to get the time from the beginning of the theme at which the first not shall 
                                        // start. To grant that all the ROM themes start playing at the right time a rest/pause command shall be
                                        // placed at the beginning of the track before to start playing that note in the channel. This ensures
                                        // that the channel starts playing at the right time, otherwise they would start playing at t=0s
                                        midiTrackInfoAux.dNotesStartTime = dTrackTime;
                                    }

                                    // check notes overlaping: if the NOTE ON MIDI event has been received while a previous NOTE ON MIDI event   
                                    // was being processed  (dOnTrackTime != -1) but the NOTE OFF MIDI event of that previous note was not yet
                                    // received then it means that there is note overlaping
                                    if ( (dOnTrackTime != -1) && (dOffTrackTime == -1) ){
                                        // notes overlap: the MIDI NOTE ON event of the new following note has arrived before having 
                                        // received the NoteOff ofevent of the current processed note.
                                        midiTrackInfoAux.bPolyphonic = true;
                                    }

                                    // store the information of the received Note On event to start processing it
                                    byCurrentNote = by_read[0];
                                    dOnTrackTime = dTrackTime;
                                    dOffTrackTime = -1;

                                    // check if the current note code is higher or lower than the other read notes
                                    if (byCurrentNote > midiTrackInfoAux.iHighestNoteCode) {
                                        midiTrackInfoAux.iHighestNoteCode = byCurrentNote;
                                    } else if (byCurrentNote < midiTrackInfoAux.iLowestNoteCode) {
                                        midiTrackInfoAux.iLowestNoteCode = byCurrentNote;
                                    }

                                    // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                    // str_dbg_out = str_dbg_out + " Note On:" + midiNoteCodeToString(by_read[0]);
                                    // str_dbg_out = str_dbg_out + " Ch:" + iByMidiCmdChan.ToString("D1") + " At:" + dAux.ToString("0.00");
                                    // file_str_writer_dbg.Write(str_dbg_out);
                                }//if
                                break;

                            case 0xA0:// = 0xAX After touch
                                // keep in memory current Status byte in case the following messages correspond to Running Status messages
                                by_last_run_status = by_read[0];

                                iByMidiCmdChan = (int)(by_read[0] & 0x0F);
                                // read 2 following After touch bytes
                                by_read = file_binary_reader.ReadBytes(2);
                                ui_total_read_bytes = ui_total_read_bytes + 2;
                                ui_chunk_read_bytes = ui_chunk_read_bytes + 2;

                                // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                // str_dbg_out = str_dbg_out + " AfterTouch:";
                                // str_dbg_out = str_dbg_out + " Ch:" + iByMidiCmdChan.ToString("D1") + " At:" + dAux.ToString("0.00");
                                // file_str_writer_dbg.Write(str_dbg_out);
                                break;

                            case 0xB0: // 0xBX = Control change ( Voice Messages ) :
                                // keep in memory current Status byte in case the following messages correspond to Running Status messages
                                by_last_run_status = by_read[0];

                                iByMidiCmdChan = (int)(by_read[0] & 0x0F);
                                iByMidiCmdChan = (int)(by_read[0] & 0x0F);
                                // read 2 following Control change bytes
                                by_read = file_binary_reader.ReadBytes(2);
                                ui_total_read_bytes = ui_total_read_bytes + 2;
                                ui_chunk_read_bytes = ui_chunk_read_bytes + 2;

                                // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                // str_dbg_out = str_dbg_out + " CtrlChng:";
                                // str_dbg_out = str_dbg_out + " Ch:" + iByMidiCmdChan.ToString("D1") + " At:" + dAux.ToString("0.00");
                                // file_str_writer_dbg.Write(str_dbg_out);
                                break;

                            case 0xC0:// 0xCX = Patch/program change
                                // as it is a single byte MIDI command it is not considered in Running Status messages and previous
                                // MIDI command will be considered instead

                                iByMidiCmdChan = (int)(by_read[0] & 0x0F);
                                // read 1 following Patch/program change
                                by_read = file_binary_reader.ReadBytes(1);
                                ui_total_read_bytes = ui_total_read_bytes + 1;
                                ui_chunk_read_bytes = ui_chunk_read_bytes + 1;

                                // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                // str_dbg_out = str_dbg_out + " PrgrChg:";
                                // str_dbg_out = str_dbg_out + " Ch:" + iByMidiCmdChan.ToString("D1") + " At:" + dAux.ToString("0.00");
                                // file_str_writer_dbg.Write(str_dbg_out);
                                break;

                            case 0xD0:// 0xDX = Channel presure
                                // as it is a single byte MIDI command it is not considered in Running Status messages and previous
                                // MIDI command will be considered instead

                                iByMidiCmdChan = (int)(by_read[0] & 0x0F);
                                // read 1 following Channel presure
                                by_read = file_binary_reader.ReadBytes(1);
                                ui_total_read_bytes = ui_total_read_bytes + 1;
                                ui_chunk_read_bytes = ui_chunk_read_bytes + 1;

                                // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                // str_dbg_out = str_dbg_out + " ChPress:";
                                // str_dbg_out = str_dbg_out + " Ch:" + iByMidiCmdChan.ToString("D1") + " At:" + dAux.ToString("0.00");
                                // file_str_writer_dbg.Write(str_dbg_out);
                                break;

                            case 0xE0:// 0xEX = Pitch wheel
                                // keep in memory current Status byte in case the following messages correspond to Running Status messages
                                by_last_run_status = by_read[0];

                                iByMidiCmdChan = (int)(by_read[0] & 0x0F);
                                // read 2 following Pitch wheel bytes
                                by_read = file_binary_reader.ReadBytes(2);
                                ui_total_read_bytes = ui_total_read_bytes + 2;
                                ui_chunk_read_bytes = ui_chunk_read_bytes + 2;

                                // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                // str_dbg_out = str_dbg_out + " PitchWheel:";
                                // str_dbg_out = str_dbg_out + " Ch:" + iByMidiCmdChan.ToString("D1") + " At:" + dAux.ToString("0.00");
                                // file_str_writer_dbg.Write(str_dbg_out);
                                break;

                            case 0xF0: //Category Generic Messages: System Common adn System realtim

                                switch (by_read[0]) {

                                    // Category: System Common
                                    case 0xF0: // 0xF0 = SysEx - System Exclusive
                                        // Sysex events and Meta Events cancel any running status
                                        by_last_run_status = 0;

                                        // the SysEx messages are the only messages with not fixed length. Its conent 
                                        // finishes whenthe byte 0xF7 is found, so keep reading until finding the 0xF7

                                        // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + "0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                        // str_dbg_out = str_dbg_out + " SySex start at:" + ui_total_read_bytes.ToString("X4");
                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        do {
                                            by_read = file_binary_reader.ReadBytes(1);
                                            ui_total_read_bytes = ui_total_read_bytes + 1;
                                            ui_chunk_read_bytes = ui_chunk_read_bytes + 1;
                                        } while (by_read[0] != 0xF7);

                                        // str_dbg_out ="  end at:" + ui_total_read_bytes.ToString("X4");
                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        break;

                                    case 0xF1: // 0xF1 = MTC Quarter Frame Message
                                        // read the other byte of the command
                                        by_read = file_binary_reader.ReadBytes(1);
                                        ui_total_read_bytes = ui_total_read_bytes + 1;
                                        ui_chunk_read_bytes = ui_chunk_read_bytes + 1;

                                        // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                        // str_dbg_out = str_dbg_out + " MTC Quarter Frame Message" + " At:" + dAux.ToString("0.00");
                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        break;

                                    case 0xF2: // 0xF2 = Song position pointer
                                        // read the other byte of the command
                                        by_read = file_binary_reader.ReadBytes(1);
                                        ui_total_read_bytes = ui_total_read_bytes + 1;
                                        ui_chunk_read_bytes = ui_chunk_read_bytes + 1;

                                        // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + " 0x" + ui_chunk_read_bytes.ToString("X4") + by_read[0].ToString("X2") + " t:" + dTrackTime.ToString("000.000");
                                        // str_dbg_out = str_dbg_out + " Song position pointer" + " At:" + dAux.ToString("0.00");
                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        break;

                                    case 0xF3: // 0xF3 = Song Select
                                        // read the other byte of the command
                                        by_read = file_binary_reader.ReadBytes(1);
                                        ui_total_read_bytes = ui_total_read_bytes + 1;
                                        ui_chunk_read_bytes = ui_chunk_read_bytes + 1;

                                        // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + " 0x" + ui_chunk_read_bytes.ToString("X4") + by_read[0].ToString("X2") + " t:" + dTrackTime.ToString("000.000");
                                        // str_dbg_out = str_dbg_out + " Song Select" + " At:" + dAux.ToString("0.00");
                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        break;

                                    case 0xF6: // 0xF6 = Tune Request
                                        // this command is only the status byte and does not contain more data bytes

                                        // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                        // str_dbg_out = str_dbg_out + " Tune Request" + " At:" + dAux.ToString("0.00");
                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        break;

                                    // Category: System Realtime
                                    case 0xF8:// 0xF8 = MIDI Clock 
                                              // this command is only the status byte and does not contain more data bytes

                                        // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                        // str_dbg_out = str_dbg_out + " MIDI Clck" + " At:" + dAux.ToString("0.00");
                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        break;

                                    case 0xFA: // 0xFA = MIDI Start
                                        // this command is only the status byte and does not contain more data bytes

                                        // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                        // str_dbg_out = str_dbg_out + " MIDI Start" + " At:" + dAux.ToString("0.00");
                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        break;

                                    case 0xFB: // 0xFB = MIDI Continue
                                        // this command is only the status byte and does not contain more data bytes

                                        // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                        // str_dbg_out = str_dbg_out + " MIDI Continue" + " At:" + dAux.ToString("0.00");
                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        break;

                                    case 0xFC: // 0xFC = MIDI Stop
                                        // this command is only the status byte and does not contain more data bytes

                                        // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                        // str_dbg_out = str_dbg_out + " MIDI Stop" + " At:" + dAux.ToString("0.00");
                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        break;

                                    case 0xFE: // 0xFE = Active Sense
                                        // this command is only the status byte and does not contain more data bytes

                                        // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                        // str_dbg_out = str_dbg_out + " MIDI Active sense" + " At:" + dAux.ToString("0.00");
                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        break;

                                    case 0xFF: // 0xFF <meta-event> specifies non-MIDI information useful to this format or to sequencers, with this syntax:

                                        // Sysex events and Meta Events cancel any running status
                                        by_last_run_status = 0;

                                        // read the <meta-event> type byte
                                        by_read = file_binary_reader.ReadBytes(1);
                                        ui_total_read_bytes = ui_total_read_bytes + 1;
                                        ui_chunk_read_bytes = ui_chunk_read_bytes + 1;
                                        by_meta_event = by_read[0];

                                        // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000");
                                        // str_dbg_out = str_dbg_out + " MetaEv Ty:0x" + by_read[0].ToString("X2") + " At:" + dAux.ToString("0.00");

                                        // get the <meta-event> length ( is Variable Lenght quantity)
                                        uiMetaEventLength = readMIDIVariableLength(file_binary_reader, ref ui32Aux);
                                        ui_total_read_bytes = ui_total_read_bytes + ui32Aux;
                                        ui_chunk_read_bytes = ui_chunk_read_bytes + ui32Aux;

                                        // str_dbg_out = str_dbg_out + " Length:" + uiMetaEventLength.ToString("D2");

                                        // List of metaevent codes
                                        // FF 00 02 ssss Sequence Number
                                        // FF 01 len text Text Event
                                        // FF 02 len text Copyright Notice
                                        // FF 03 len text Sequence/Track Name
                                        // FF 04 len text Instrument Name
                                        // FF 05 len text Lyric
                                        // FF 06 len text Marker
                                        // FF 07 len text Cue Point
                                        // FF 20 01 cc MIDI Channel Prefix
                                        // FF 2F 00 End of Track
                                        // FF 51 03 tttttt Set Tempo
                                        // FF 54 05 hr mn se fr ff SMPTE Offset
                                        // FF 58 04 nn dd cc bb Time Signature
                                        // FF 59 02 sf mi Key Signature
                                        // FF 7F len data Sequencer-Specific Meta-Event

                                        // count down until al the bytes of the <meta-event> have been read
                                        while (uiMetaEventLength > 0) {
                                            by_read = file_binary_reader.ReadBytes(1);
                                            ui_total_read_bytes = ui_total_read_bytes + 1;
                                            ui_chunk_read_bytes = ui_chunk_read_bytes + 1;
                                            uiMetaEventLength--;

                                            // if the received character is an ASCII character add it to the corresponding string
                                            switch (by_meta_event) {
                                                case 0x01: // text Text Event
                                                     break;
                                                case 0x02: // text Copyright Notice
                                                    midiTrackInfoAux.strCopyrightNotice = midiTrackInfoAux.strCopyrightNotice + " " + System.Text.Encoding.ASCII.GetString(new byte[] { by_read[0] });
                                                    midiTrackInfoAux.bMetadataTrack = true; // set to true the flag that indicates that current track has musical data
                                                    break;
                                                case 0x03: // text Sequence/Track Name
                                                    midiTrackInfoAux.strTrackName = midiTrackInfoAux.strTrackName + " " + System.Text.Encoding.ASCII.GetString(new byte[] { by_read[0] });
                                                    midiTrackInfoAux.bMetadataTrack = true; // set to true the flag that indicates that current track has musical data
                                                    break;
                                                case 0x04: // text Instrument Name
                                                    midiTrackInfoAux.strInstrName = midiTrackInfoAux.strInstrName + " " + System.Text.Encoding.ASCII.GetString(new byte[] { by_read[0] });
                                                    midiTrackInfoAux.bMetadataTrack = true; // set to true the flag that indicates that current track has musical data
                                                    break;
                                               case 0x05: // text Lyric
                                                    midiTrackInfoAux.strLyric = midiTrackInfoAux.strLyric + " " + System.Text.Encoding.ASCII.GetString(new byte[] { by_read[0] });
                                                    midiTrackInfoAux.bMetadataTrack = true; // set to true the flag that indicates that current track has musical data
                                                    break;
                                                case 0x06: // text Marker
                                                    midiTrackInfoAux.strMarker = midiTrackInfoAux.strMarker + " " + System.Text.Encoding.ASCII.GetString(new byte[] { by_read[0] });
                                                    midiTrackInfoAux.bMetadataTrack = true; // set to true the flag that indicates that current track has musical data
                                                    break;
                                                case 0x07: // text Cue Point
                                                    midiTrackInfoAux.strCuePoint = midiTrackInfoAux.strCuePoint + " " + System.Text.Encoding.ASCII.GetString(new byte[] { by_read[0] });
                                                    midiTrackInfoAux.bMetadataTrack = true; // set to true the flag that indicates that current track has musical data
                                                    break;
                                                default:
                                                    // str_dbg_out = str_dbg_out + " 0x" + by_read[0].ToString("X2");
                                                    break;
                                                }

                                        }//while

                                        // file_str_writer_dbg.Write(str_dbg_out);
                                        break;

                                }//switch

                                break;

                            default:
                                // str_dbg_out = "\r\n0x" + ui_total_read_bytes.ToString("X4") + " 0x" + ui_chunk_read_bytes.ToString("X4") + " t:" + dTrackTime.ToString("000.000") + " Undefined: 0x" + by_read[0].ToString("X2") + " At:" + dAux.ToString("0.00");
                                // file_str_writer_dbg.Write(str_dbg_out);
                                break;

                        }//switch

                    }//while ((ec_ret_val >= 0) && (ui_chunk_read_bytes < ui32ChunkLength)) {

                    // if the TRACK CHUNK was completely read and processed but the last note was still pending to be added 
                    // to the them waiting for the following NOTE ON event to get the rest duation time, then save it
                    if ((byCurrentNote != 0) && (dOnTrackTime != 0.0f) && (dOffTrackTime != 0.0f)) {

                        // processes the last MIDI NOTE ON event and note and rest duration
                        // check if the current note code is higher or lower than the other read notes
                        if (byCurrentNote > midiTrackInfoAux.iHighestNoteCode) {
                            midiTrackInfoAux.iHighestNoteCode = byCurrentNote;
                        } else if (byCurrentNote < midiTrackInfoAux.iLowestNoteCode) {
                            midiTrackInfoAux.iLowestNoteCode = byCurrentNote;
                        }
                        // ec_ret_val = store_MIDI_NOTE_ON(iThemeIdxToInsert, iThemeDestChanIdx, byCurrentNote, dTrackTime, dOnTrackTime, dOffTrackTime);

                    }//if ((byCurrentNote != 0) &

                    // increase the MIDI track counter to the following track index
                    iTrackCtr++;

                }//while ((ec_ret_val.i_code >= 0)

            } catch {
           
               ec_ret_val = cErrCodes.ERR_FILE_IMPORT_PARSING_MIDI_INFO;
           
            }//trye

            // if (file_str_writer_dbg != null) {
            //     file_str_writer_dbg.Close();// close the output file
            // }

            if (file_stream_reader != null) {
                file_stream_reader.Close();// close the input file
            }

            return ec_ret_val;

        }//getImportedMIDIFileInfo

    }//MIDIUtils

}//drivePackEd
