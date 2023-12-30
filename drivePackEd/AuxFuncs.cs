using System;
using System.Collections.Generic;
using System.Text;

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

namespace drivePackEd {

    /*******************************************************************************
    *  @brief defines the auxiliar object that includes different helpfull methods
    *  or functions.
    *******************************************************************************/
    class AuxFuncs {

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
        static public int convert4BytesToUInt32(byte [] by_to_convert, ref UInt32 ui32_value){
            int i_ret_val = 0;

            ui32_value = 0;
            if (by_to_convert.Length != 4){
                // ERROR: received invalid number of bytes
                i_ret_val = -1;
            }else{
                ui32_value = (UInt32)( (by_to_convert[3] << 24) | (by_to_convert[2] << 16) | (by_to_convert[1] << 8) | by_to_convert[0] );
            }//if

            return i_ret_val;

        }//convert4BytesToUInt32


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
        static public int convertUInt32To4Bytes(UInt32 ui32_value, byte[] by_converted ){
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

        }//convertUInt32To4Bytes


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
        static public int convert4BytesReversedToUInt32(byte[] by_arr_to_convert, ref UInt32 ui32_value) {
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

        }//convert4BytesToUInt32


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


    }//AuxFuncs

}
