using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Be.Windows.Forms;

namespace drivePackEd
{
    public class cDrivePackData
    {
        // constants
        public const int FILE_METADATA_TITLE      = 0x01;
        public const int FILE_METADATA_SONGS_INFO = 0x02;
        public const int FILE_METADATA_SONGS_ROM  = 0x03;

        public const int ROM_MAX_SIZE           = 0x8000;

        // attributes
        public string str_title = "";
        public string str_songInfo= "";
        public DynamicByteProvider dynbypr_memory_bytes; // reference to the dynamic bytes provider
        private bool b_data_changed; // flag to indicate if in the dirvePackis there are changes pending to save 



        /*******************************************************************************
        *  DrivePackData
        *------------------------------------------------------------------------------
        *  Description
        *    Default constructor.
        *  Parameters:
        *  Return: 
        *    By reference:
        *    By value:
        *******************************************************************************/
        public cDrivePackData(){

            dynbypr_memory_bytes = null;
            b_data_changed = false;
            str_title = "";
            str_songInfo = "";

        }//DrivePackData



        /*******************************************************************************
        *  Initialize
        *------------------------------------------------------------------------------
        *  Description
        *     Initialize the drivePackData object.
        *  Parameters:
        *    str_default_file
        *  Return: 
        *    By reference:
        *    By value:
        *******************************************************************************/
        public void Initialize(string str_default_file)
        {            
            byte[] by_memory_bytes = null;
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;

            // if str_default_file contains a file name and it exists initialize the object with the file content 
            if ( (str_default_file != "") && (File.Exists(str_default_file)) ){

                ec_ret_val = loadDRPFile(str_default_file);

            }else{

                // create the DynamicByteProvider and initilized with the previously initialized byte array
                by_memory_bytes = new byte[ROM_MAX_SIZE];

                this.str_title = "Enter RO-XXX Title of the cart here.";
                this.str_songInfo = "Enter the list of the cart songs here:\r\n[1]-Son 1 name\r\n[2]-Son 2 name\r\n[3]-Son 3 name\r\n[4]-Son 3 name\r\n";

                // re initialize the DynamicByteProvider with the bytes read from the file
                dynbypr_memory_bytes = new DynamicByteProvider(by_memory_bytes, true);

            }//if

        }//Initialize



        /*******************************************************************************
        *  dataChanged 
        *------------------------------------------------------------------------------
        *  Description
        *     getter setter of the flag used to indicate if in the DynamicByteProvider 
        *  there are pending modifications to save.   
        *  Parameters:
        *     setModified: true to specify that the DynamicByteProvider has pending to 
        *  store modifications. false to specify that the DynamicByteProvider has no pending
        *  modifications to store.
        *  Return: 
        *    By reference:
        *    By value:
        *******************************************************************************/
        public bool dataChanged
        {
            set
            { b_data_changed = value; }
            get
            { return b_data_changed; }

        }//dataChanged



        /*******************************************************************************
        *  loadDRP_ROMPACKv00 
        *------------------------------------------------------------------------------
        *  Description
        *     Loads data from a file in ROMPACKv00 format and stores it into the  
        *  drivePackData object.
        *  Parameters:
        *     file_stream: binary file stream
        *     file_binary_reader: binary stream reader
        *     ui_read_bytes: number of byts read from the file 
        *  Return: 
        *    By reference:
        *     ui_read_bytes: the number of bytes read from of the file
        *    By value:
        *     >=0 file has been succesfully loaded into the object
        *     <0 an error occurred 
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
            while ( (ec_ret_val.i_code >= 0) && (file_stream.Position != file_stream.Length) ){
                by_aux = file_binary_reader.ReadByte();
                byli_byte_list.Add(by_aux);
            }//while

            // store the bytes from the bytes list to a bytes array
            byte[] by_array;
            by_array = byli_byte_list.ToArray();

            // re initialize the DynamicByteProvider with the bytes read from the file
            dynbypr_memory_bytes = new DynamicByteProvider(by_array, true);
            
            // just after loading the file data in memory corresponds exactly to what is stored in disk
            dataChanged = false;

            // set default values in the fields not implemented in ROMPACKv00 file format
            this.str_title = "ROMPACKv00 files do not have title meta-data block. Update it and save it as ROMPACKv01";
            this.str_songInfo = "ROMPACKv00 files do not have songs information meta-data block. Update it and save it as ROMPACKv01";

            return ec_ret_val;

        }//loadDRP_ROMPACKv00



        /*******************************************************************************
        *  loadDRP_ROMPACKv01 
        *------------------------------------------------------------------------------
        *  Description
        *     Loads data from a file in ROMPACKv01 format and stores it into the  
        *  drivePackData object.
        *  Parameters:
        *     file_stream: binary file stream
        *     file_binary_reader: binary stream reader
        *     ui_read_bytes: number of byts read from the file 
        *  Return: 
        *    By reference:
        *     ui_read_bytes: the number of bytes read from of the file
        *    By value:
        *     >=0 file has been succesfully loaded into the object
        *     <0 an error occurred 
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
                        this.str_title = ascii.GetString(by_read);
                        break;

                    case FILE_METADATA_SONGS_INFO:

                        // read the 4 bytes corresponding to the current metada block size
                        by_read = file_binary_reader.ReadBytes(4);
                        ui_read_bytes = ui_read_bytes = ui_read_bytes + 4;
                        AuxFuncs.convert4BytesToUInt32(by_read, ref ui32_metadata_size);

                        // read the ui32_metadata_size bytes of the current metadata block
                        by_read = file_binary_reader.ReadBytes((int)ui32_metadata_size);

                        // convert the read array of bytes to a string
                        this.str_songInfo = ascii.GetString(by_read);
                        break;

                    case FILE_METADATA_SONGS_ROM:

                        // read the 4 bytes corresponding to the current metada block size
                        by_read = file_binary_reader.ReadBytes(4);
                        ui_read_bytes = ui_read_bytes = ui_read_bytes + 4;
                        AuxFuncs.convert4BytesToUInt32(by_read, ref ui32_metadata_size);

                        // read the ui32_metadata_size bytes of the current metadata block
                        by_read = file_binary_reader.ReadBytes((int)ui32_metadata_size);

                        // re initialize the DynamicByteProvider with the array of bytes read from the file
                        dynbypr_memory_bytes = new DynamicByteProvider(by_read, true);
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
        *  loadDRPFile 
        *------------------------------------------------------------------------------
        *  Description
        *    Loads the specified melody drive pack data file in DRP format into the drivePackData 
        *  object.
        *  Parameters:
        *    str_load_file: with the name of the dirve pack data file to load into the 
        *  object
        *  Return: 
        *    By reference:
        *    By value:
        *    >=0 file has been succesfully loaded into the object
        *    <0 an error occurred 
        *******************************************************************************/
        public ErrCode loadDRPFile(string str_load_file){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream = null;
            BinaryReader file_binary_reader;
            ASCIIEncoding ascii = new ASCIIEncoding();
            uint ui_read_bytes = 0;
            byte[] by_read = null;
            string str_aux = "";


            if (!File.Exists(str_load_file)) {

                ec_ret_val = cErrCodes.ERR_FILE_NOT_EXIST;

            }else{

                file_stream = new FileStream(str_load_file, FileMode.Open);
                file_binary_reader = new BinaryReader(file_stream);

                if (file_binary_reader ==null){
                    ec_ret_val = cErrCodes.ERR_FILE_OPENING;
                }

                // read and check file format verstion tag
                ui_read_bytes = 0;
                if (ec_ret_val.i_code >= 0){

                    // get the file format and version tag
                    file_stream.Seek(0, SeekOrigin.Begin);
                    by_read = file_binary_reader.ReadBytes(11);
                    ui_read_bytes = ui_read_bytes + 11;// 10:chars + 1:\0
                    str_aux = str_aux + ascii.GetString(by_read);

                    // process the file format and version tag 
                    if (str_aux== "ROMPACKv00\0"){
                        
                        ec_ret_val= loadDRP_ROMPACKv00(ref file_stream, ref file_binary_reader, ref ui_read_bytes);

                    }else if (str_aux== "ROMPACKv01\0"){

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
        *  loadBINFile 
        *------------------------------------------------------------------------------
        *  Description
        *    Loads the specified melody drive pack data file in binary raw format into the 
        *  drivePackData object.
        *  Parameters:
        *    str_load_file: with the name of the dirve pack data file to load into the 
        *  object
        *  Return: 
        *    By reference:
        *    By value:
        *    >=0 file has been succesfully loaded into the object
        *    <0 an error occurred 
        *******************************************************************************/
        public ErrCode loadBINFile(string str_load_file){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream = null;
            BinaryReader file_binary_reader;
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] by_read = null;
            System.UInt32 ui32_file_size=0;


            if (!File.Exists(str_load_file)) {

                ec_ret_val = cErrCodes.ERR_FILE_NOT_EXIST;

            }else{

                file_stream = new FileStream(str_load_file, FileMode.Open);
                file_binary_reader = new BinaryReader(file_stream);

                if (file_binary_reader ==null){
                    ec_ret_val = cErrCodes.ERR_FILE_OPENING;
                }

                // get and check the size of the specified binary file
                if (ec_ret_val.i_code >= 0){

                    ui32_file_size = (System.UInt32)file_stream.Length;
                    if (ui32_file_size > ROM_MAX_SIZE){
                        ec_ret_val = cErrCodes.ERR_FILE_SPECIFIED;                        
                    }//if

                }//if

                // load specified binart file
                if (ec_ret_val.i_code >= 0) {

                    this.str_title = "Enter RO-XXX Title of the cart here.";
                    this.str_songInfo = "Enter the list of the cart songs here:\r\n[1]-Son 1 name\r\n[2]-Son 2 name\r\n[3]-Son 3 name\r\n[4]-Son 3 name\r\n";

                    // read all the bytes of the specified binary file and store them into the songs object in memory
                    by_read = file_binary_reader.ReadBytes((int)ui32_file_size);

                    // re initialize the DynamicByteProvider with the array of bytes read from the file
                    dynbypr_memory_bytes = new DynamicByteProvider(by_read, true);
               
                }//if

                file_stream.Close();

            }//if

            return ec_ret_val;

        }//loadBINFile



        /*******************************************************************************
        *  saveDRPFile 
        *------------------------------------------------------------------------------
        *  Description
        *    Saves to the specified drive pack file (DRP format) the melody data in the 
        *  current drivePACK object in memory.
        *  Parameters:
        *    str_save_file: with the name of the a file to load into the 
        *  object
        *  Return: 
        *    By reference:
        *    By value:
        *    >=0 file has been succesfully loaded into the object
        *    <0 an error occurred 
        *******************************************************************************/
        public ErrCode saveDRPFile(string str_save_file){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream = null;
            BinaryWriter file_binary_writer;
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] by_aux= null;
            byte[] by_title = null;
            byte[] by_title_length = new byte[4];
            byte[] by_song_info = null;
            byte[] by_song_info_length = new byte[4];


            file_stream = new FileStream(str_save_file, FileMode.Create);
            file_binary_writer = new BinaryWriter(file_stream);

            if (file_binary_writer == null){
                ec_ret_val = cErrCodes.ERR_FILE_CREATING;
            }//if

            if (ec_ret_val.i_code >= 0) {

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
                by_title = Encoding.ASCII.GetBytes(this.str_title + '\0');
                // 1 byte - METADATA type - type = FILE_METADATA_TITLE
                file_binary_writer.Write((byte)FILE_METADATA_TITLE);
                // 4 byte - METADATA field length
                by_aux = new byte[4];// re initialize the auxiliary array to 4 bytes
                AuxFuncs.convertUInt32To4Bytes((uint)by_title.Length, by_aux);
                file_binary_writer.Write(by_aux);
                // n byte - METADATA field content
                file_binary_writer.Write(by_title);

                // save the SONGS_INFO METADATA ( FILE_METADATA_SONGS_INFO field ):
                by_song_info = Encoding.ASCII.GetBytes(this.str_songInfo + '\0');
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
                AuxFuncs.convertUInt32To4Bytes((uint)this.dynbypr_memory_bytes.Length, by_aux);
                file_binary_writer.Write(by_aux);
                // n byte - METADATA ROM DATA field content
                by_aux = this.dynbypr_memory_bytes.Bytes.ToArray();
                file_binary_writer.Write(by_aux);

            }//if

            file_stream.Close();

            return ec_ret_val;

        }//saveDRPFile



        /*******************************************************************************
        *  saveBINFile 
        *------------------------------------------------------------------------------
        *  Description
        *    Saves to the specified binary file (BIN) the melody data in the current  
        *  drivePACK object in memory.
        *  Parameters:
        *    str_save_file: with the name of the binary file to save in current drive 
        *  pack object melody data,
        *  Return: 
        *    By reference:
        *    By value:
        *    >=0 file has been succesfully loaded into the object
        *    <0 an error occurred 
        *******************************************************************************/
        public ErrCode saveBINFile(string str_save_file){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream = null;
            BinaryWriter file_binary_writer;
            byte[] by_aux= null;


            file_stream = new FileStream(str_save_file, FileMode.Create);
            file_binary_writer = new BinaryWriter(file_stream);

            if (file_binary_writer == null){
                ec_ret_val = cErrCodes.ERR_FILE_CREATING;
            }//if

            if (ec_ret_val.i_code >= 0) {

                // write drive pack data to binary file
                by_aux = this.dynbypr_memory_bytes.Bytes.ToArray();
                file_binary_writer.Write(by_aux);

            }//if

            file_stream.Close();

            return ec_ret_val;

        }//saveBINFile

    }

}