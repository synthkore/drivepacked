using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

// **********************************************************************************
// ****                          drivePACK Editor                                ****
// ****                      www.tolaemon.com/dpacked                            ****
// ****                            Source code                                   ****
// ****                            23/04/2023                                    ****
// ****                          Jordi Bartolome                                 ****
// ****                                                                          ****
// ****          IMPORTANT:                                                      ****
// ****          Using this code or any part of it means accepting all           ****
// ****          conditions exposed in: http://www.tolaemon.com/dpacked          ****
// **********************************************************************************

namespace drivePackEd {

    /*******************************************************************************
    *  @brief defines the auxiliar object that includes different helpfull methods
    *  or functions.
    *******************************************************************************/
    class AuxFuncs {

        /*******************************************************************************
        *  @brief custom modulo function to use insted of '%' C# operator. The modulo 
        *  operation returns the remainder or signed remainder of a division, after one 
        *  number is divided by another. It is implemented because the C# % modulo operator 
        *  does not properly treat the negative module parameters.
        *  @param[in] x dividend
        *  @param[in] y divisior
        *  @return the remainder of the division
        *******************************************************************************/
        static public int mod(int x, int m) {

            int r = x % m;
            return r < 0 ? r + m : r;

        }//mod

        /*******************************************************************************
        * @brief Receives an array with the bytes of an unsigned integer in the following order:
        *      byte[0]:bits0..7
        *      byte[1]:bits8..15
        *      byte[2]:bits16..23
        *      byte[3]:bits24..32
        *    And converts them to a 32 bit unsigned integer.
        * @param[in] by_to_convert with the array of bytes to convert to an unsigned int 32. It
        *  must have 4 bytes. Less bytes or more bytes are not allowed.
        * @param[out] ui32_value with the unsinged 32 that corresponds to the 4 recevied bytes.
        * @return  >=0 received bytes have been converted to a 32 bits unsigned integer. <0 if 
        * an error occurred 
        *******************************************************************************/
        static public int convert4BytesToUInt32LE(byte [] by_to_convert, ref UInt32 ui32_value){
            int i_ret_val = 0;

            ui32_value = 0;
            if (by_to_convert.Length != 4){

                // ERROR: received invalid number of bytes
                i_ret_val = -1;

            }else{

                ui32_value = (UInt32)( (by_to_convert[3] << 24) | (by_to_convert[2] << 16) | (by_to_convert[1] << 8) | by_to_convert[0] );

            }//if

            return i_ret_val;

        }//convert4BytesToUInt32LE

        /*******************************************************************************
        * @brief Receives a 32 bits unsigned integer and converts it to a 4 bytes array in
        * the following order:
        *      byte[0]:bits0..7
        *      byte[1]:bits8..15
        *      byte[2]:bits16..23
        *      byte[3]:bits24..32
        * @param[in] ui32_value with the unsigned int 32 value that must be converted to a 4 
        *  bytes array.
        * @param[out] by_converted a 4 bytes array with the uint32 separated in 4 bytes.
        * @return >=0 received uint 32 has been succesfully converted to a 4 bytes array.
        * <0 an error occurred 
        *******************************************************************************/
        static public int convertUInt32LETo4Bytes(UInt32 ui32_value, byte[] by_converted ){
            int i_ret_val = 0;


            if (by_converted.Length != 4){

                // ERROR: received invalid number of bytes
                i_ret_val = -1;

            }else{

                by_converted[0] = (byte)(ui32_value&0x000000FF);
                by_converted[1] = (byte)((ui32_value>>8) & 0x000000FF);
                by_converted[2] = (byte)((ui32_value >> 16) & 0x000000FF);
                by_converted[3] = (byte)((ui32_value >> 25) & 0x000000FF);

            }//if

            return i_ret_val;

        }//convertUInt32LETo4Bytes

        /*******************************************************************************
        * @brief Receives an array with the bytes of an unsigned integer in the following order:
        *      byte[0]:bits24..32
        *      byte[1]:bits16..23 
        *      byte[2]:bits8..15
        *      byte[3]: bits0..7
        *    And converts them to a 32 bit unsigned integer.
        * @param[in] by_to_convert with the array of bytes to convert to an unsigned int 32. It
        *  must have 4 bytes. Less bytes or more bytes are not allowed.
        * @param[out] ui32_value with the unsinged 32 that corresponds to the 4 recevied bytes.
        * @return  >=0 received bytes have been converted to a 32 bits unsigned integer. <0 if 
        * an error occurred 
        *******************************************************************************/
        static public int convert4BytesBEToUInt32(byte[] by_to_convert, ref UInt32 ui32_value) {
            int i_ret_val = 0;

            ui32_value = 0;
            if (by_to_convert.Length != 4) {

                // ERROR: received invalid number of bytes
                i_ret_val = -1;

            } else {

                ui32_value = (UInt32)((by_to_convert[0] << 24) | (by_to_convert[1] << 16) | (by_to_convert[2] << 8) | by_to_convert[3]);

            }//if

            return i_ret_val;

        }//convert4BytesBEToUInt32

        /*******************************************************************************
          * @brief Receives an array with the bytes of an 16 bit unsigned integer in the 
          * following order:
          *      byte[0]:bits8..15 
          *      byte[1]:bits0..7
          *    And converts them to a 32 bit unsigned integer.
          * @param[in] by_to_convert with the array of bytes to convert to an unsigned int 16. It
          *  must have 2 bytes. Less bytes or more bytes are not allowed.
          * @param[out] ui16_value with the unsinged 16 that corresponds to the 2 recevied bytes.
          * @return  >=0 received bytes have been converted to a 16 bits unsigned integer. <0 if 
          * an error occurred 
          *******************************************************************************/
        static public int convert2BytesBEToUInt16(byte[] by_to_convert, ref UInt16 ui16_value) {
            int i_ret_val = 0;

            ui16_value = 0;
            if (by_to_convert.Length != 2) {

                // ERROR: received invalid number of bytes
                i_ret_val = -1;

            } else {

                ui16_value = (UInt16)((by_to_convert[0] << 8) | by_to_convert[1]);

            }//if

            return i_ret_val;

        }//convert2BytesBEToUInt16

        /*******************************************************************************  
        * @brief Receives an array of bytes with the bytes of an unsigned integer. The nibbles of 
        * each byte are reversed in the following order: lowest 4 bits of the value are in the higher
        * 4 bits and higher 4 bits are in the lowest 4 bits:
        *     byte[0]:bits 7..4 > Uint32:bits 00..03
        *     byte[0]:bits 0..3 > Uint32:bits 04..07
        *     byte[1]:bits 7..4 > Uint32:bits 08..11
        *     byte[1]:bits 0..3 > Uint32:bits 12..15
        *     byte[2]:bits 7..4 > Uint32:bits 16..19
        *     byte[2]:bits 0..3 > Uint32:bits 20..23
        *     byte[3]:bits 7..4 > Uint32:bits 24..27
        *     byte[3]:bits 0..3 > Uint32:bits 28..21
        *   Ex:
        *    byte[0]=0x87 byte[1]=0x65 byte[2]=0x43 byte[3]=0x21 corresponds to 0x12345678  
        *    
        * @param[in] by_arr_to_convert with the array of bytes to convert to an unsigned int 32. It
        * must have 4 bytes and considers that the nibbles on each byte are reversed. Less bytes or
        * more bytes are not allowed.
        * @param[out] ui32_value with the unsinged 32 that corresponds to the conversion to uint32 of
        * the 4 recevied bytes.
        * @return  >=0 received bytes have been converted to a 32 bits unsigned integer. <0 if 
        * an error occurred 
        *******************************************************************************/
        static public int convert4BytesReversedToUInt32LE(byte[] by_arr_to_convert, ref UInt32 ui32_value) {
            int i_ret_val = 0;

            ui32_value = 0;
            if (by_arr_to_convert.Length != 4) {

                // ERROR: received invalid number of bytes
                i_ret_val = -1;

            } else {

                ui32_value = (uint)( ((by_arr_to_convert[0] & 0xF0) >> 4) | ((by_arr_to_convert[0] & 0x0F) << 4) );
                ui32_value = ui32_value | (uint)( ((by_arr_to_convert[1] & 0xF0) << 4)  | ((by_arr_to_convert[1] & 0x0F) << 12));
                ui32_value = ui32_value | (uint)( ((by_arr_to_convert[2] & 0xF0) << 12) | ((by_arr_to_convert[2] & 0x0F) << 20));
                ui32_value = ui32_value | (uint)( ((by_arr_to_convert[3] & 0xF0) << 20) | ((by_arr_to_convert[3] & 0x0F) << 28));

            }//if

            return i_ret_val;

        }//convert4BytesReversedToUInt32LE

        /*******************************************************************************
        * @brief Receives a 32 bits unsigned integer and converts it to a 4 bytes array in
        * the reversed following order:
        *      Uint32:bits 00..03 > byte[0]:bits 7..4 
        *      Uint32:bits 04..07 > byte[0]:bits 0..3 
        *      Uint32:bits 08..11 > byte[1]:bits 7..4 
        *      Uint32:bits 12..15 > byte[1]:bits 0..3 
        *      Uint32:bits 16..19 > byte[2]:bits 7..4 
        *      Uint32:bits 20..23 > byte[2]:bits 0..3 
        *      Uint32:bits 24..27 > byte[3]:bits 7..4 
        *      Uint32:bits 28..21 > byte[3]:bits 0..3 
        *   Ex:
        *   0x12345678: byte[0]=0x87 byte[1]=0x65 byte[2]=0x43 byte[3]=0x21
        *   
        * @param[in] ui32_value: with the unsigned int 32 value that must be converted to a 4 
        * bytes array and then reversed.
        * @param[out] by_arr_converted a 4 elements byte array with the uint32 separed in 4 bytes.
        * @return >=0 received uint 32 has been succesfully converted to a 4 bytes array or
        * <0 an error occurred 
        *******************************************************************************/
        static public int convertUInt32To4BytesReversed(UInt32 ui32_value, byte[] by_arr_converted ){
            int i_ret_val = 0;

            if (by_arr_converted.Length != 4){

                // ERROR: received invalid number of bytes
                i_ret_val = -1;

            }else{

                // byte 0
                by_arr_converted[0] = (byte)((ui32_value & 0x0000000F)<<4);
                by_arr_converted[0] = (byte)( by_arr_converted[0] | ((ui32_value&0x000000F0)>>4) );
                // byte 1
                by_arr_converted[1] = (byte)((ui32_value & 0x00000F00)>>4);
                by_arr_converted[1] = (byte)(by_arr_converted[1] | ((ui32_value & 0x0000F000)>>12));
                // byte 2
                by_arr_converted[2] = (byte)((ui32_value & 0x000F0000) >> 12);
                by_arr_converted[2] = (byte)(by_arr_converted[2] | ((ui32_value & 0x00F00000)>> 20));
                // byte 3
                by_arr_converted[3] = (byte)((ui32_value & 0x0F000000) >>20);
                by_arr_converted[3] = (byte)(by_arr_converted[3] | ((ui32_value & 0xF0000000)>> 28));

            }//if

            return i_ret_val;

        }//convertUInt32To4BytesReversed

        /*******************************************************************************
        * @brief receives an uint with a group of 'n' bits containing a Complement 2 value 
        * and returns its value as signed integer.
        *   
        * @param[in] ui32BitsInC2 containing the 'n' bits in C2 to convert to Int. The bits
        * to convert in the received bitset must be placed from position 0 to position 
        * uiNumbits-1
        * @param[in] uiNumBits containing the bits in C2 to convert to Int
        * @return the value in the received byte
        * @note some tests examples:
        *  int iAux = 0;
        *  iAux = AuxFuncs.convertNBitsInC2ToInt32(0x000D3, 8); // 1101 0011 > -45
        *  iAux = AuxFuncs.convertNBitsInC2ToInt32(0x00098, 8); // 1001 1000 > -104
        *  iAux = AuxFuncs.convertNBitsInC2ToInt32(0x000F6, 8); // 1111 0110 > -10
        *  iAux = AuxFuncs.convertNBitsInC2ToInt32(0x0000B, 4); // 1011 > -5
        *  iAux = AuxFuncs.convertNBitsInC2ToInt32(0x00001, 3); // 0001 > 1
        *  iAux = AuxFuncs.convertNBitsInC2ToInt32(0x00008, 8); // 1000 > 8
        *******************************************************************************/
        static public Int32 convertNBitsInC2ToInt32(UInt32 ui32BitsInC2, uint uiNumBits) {
            Int32 i32_ret_val = 0;
            Int32 iMask = 0x00000000;
            Int32 iMask2 = 0x00000000;

            // first get the different masks used in the calculations
            // get the mask used to check of the bitset highest bit
            iMask = 0x00000001 << (int)(uiNumBits - 1);
            // get the mask used to extend the highest bit in the bitset
            for (int iAux = 0; iAux < (32 - uiNumBits); iAux++) {
                iMask2 = (iMask2 | iMask) << 1;
            }

            // extend the highest bit of the received bitset
            if ((ui32BitsInC2 & iMask) != 0) {
                // highest bits in the int must be 1
                i32_ret_val = (Int32)(iMask2 | ui32BitsInC2);
            } else {
                // highest bits in the int must be 0
                i32_ret_val = (Int32)((~iMask2) & ui32BitsInC2);
            }//if

            return i32_ret_val;

        }//convertNBitsInC2ToInt

        /*******************************************************************************
        * @brief receives a string with the hexadecimal representation of an integer and 
        * converts it to an integer.
        *   
        * @param[in] str_hex_value: text with the hex representation of an integer value.
        * @param[out] i_value the integer with the conversion of the received str_hex_value
        * @return >=0 the content of the received string has been succesfully converted to
        * int, <0 an error occurred while converting the received string to int.
        *******************************************************************************/
        // static public int convertHexStringToInt(string str_hex_value, ref int i_value) {
        //     int i_ret_val = 0;
        //
        //
        //     return i_ret_val;
        //
        // }//convertHexStringToInt

        /*******************************************************************************
        * @brief receives a string with the representation of an byte and 
        * converts it to a byte. The  function detects if the received representation 
        * corresponds to a decimal or hexadecimal representation.
        * an hexade
        *   
        * @param[in] str_hex_value: text with the hex representation of an byte value.
        * @param[out] by_value the byte with the conversion of the received str_hex_value
        * @return >=0 the content of the received string has been succesfully converted to
        * int, <0 an error occurred while converting the received string to int.
        *******************************************************************************/
        static public int convertStringToByte(string str_value, ref byte by_value) {
            int i_ret_val = 0;
            bool b_is_hex = false;

            by_value = 0;
            try {

                if (str_value == null) {

                    by_value = 0;
                
                } else { 

                    str_value = str_value.ToLower();

                    if ( str_value.Contains("0x") ) {

                        str_value = str_value.Replace("0x", "");
                        b_is_hex = true;

                    } else if ( str_value.Contains("h") ){

                        str_value =  str_value.Replace("h", "");
                        b_is_hex = true;

                    }//if

                    if (b_is_hex) {
                        by_value = Convert.ToByte(str_value, 16);
                    } else {
                        by_value = Convert.ToByte(str_value);
                    }//if
                
                }
            } catch{

                // something failed when converting the received string to a byte value
                by_value = 0;

            }

            return i_ret_val;

        }//convertStringToByte

        /*******************************************************************************
        * @brief This procedure takes a string containing the file name and path. If it 
        * exceeds the specified number of characters, it is truncated to the indicated size.
        * @param[in] str_path_file String containing the file (including the path) to be
        * truncated.
        * @param[in] ui_num_chars Maximum number of characters allowed for the path and 
        * file name.
        * @return The string truncated to the specified length.
        *******************************************************************************/
        static public string ReducePathAndFile(string str_path_file, uint ui_num_chars) {
            string str_aux = "";
            int i_length = 0;
            int i_length_diff = 0;

            // comprueba que la longitud de la cadena es mayor que num_chars i si es así la recorta
            i_length = str_path_file.Length;
            if (i_length <= ui_num_chars) {

                // no se recorta el path/nombre del fichero
                str_aux = str_path_file;

            } else {

                // se calcula el exceso de caracteres respecto a 
                i_length_diff = i_length - (int)ui_num_chars;

                // quita los caracteres del principio que sobran y se queda con los del final, luego se pone '...' al ppio
                str_aux = str_path_file.Substring(i_length_diff, (int)ui_num_chars);
                str_aux = "..." + str_aux;

            }//if

            return str_aux;

        }//ReducePathAndFile

        /*******************************************************************************
        * @brief Receives an 8 bit value and swaps the position of the 4 lowes bits by the
        * position of 4 highest bits. So lowest nibble becomes highest nibble and viceversa.
        * @param[in] byteToSwap the byte whose nibbles we want to swap
        * @return The byte with the swapped nibbles
        *******************************************************************************/
        static public byte SwapByteNibbles(byte byteToSwap) {
            byte byRetVal = 0;
            int iAux = 0;
            
            iAux = ((((int)byteToSwap) & 0x0F) << 4);
            iAux = iAux | ((((int)byteToSwap) & 0xF0) >> 4);

            byRetVal = (byte)iAux;

            return byRetVal;

        }//SwapByteNibbles

        /*******************************************************************************
        * @brief checks if the web page specified in the received URL does exist or does
        * not exist.
        * @param[in] strURL string with the URL of the page to check if exists or not 
        * exists.
        * @return true if exists the web page withe received URL, false if it does not
        * exist.
        *******************************************************************************/
        static public bool CheckWebPageExists(string strURL) {
            bool bPageExists = false;

            // this code checks if the received URL exists or does not exist
            try{

                var request = WebRequest.Create(strURL) as HttpWebRequest;
                request.Method = "HEAD";
                using (var response = (HttpWebResponse)request.GetResponse()) {
                    bPageExists = response.StatusCode == HttpStatusCode.OK;
                }//using
            }catch{
                bPageExists = false;
            }//try

            return bPageExists;

        }//CheckWebPageExists

    }//AuxFuncs
     
}//namespace drivePackEd
