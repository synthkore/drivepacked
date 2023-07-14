using System;
using System.Collections.Generic;
using System.Text;

namespace drivePackEd
{
    class AuxFuncs{

        /*******************************************************************************
        *  convert4BytesToUInt32 
        *------------------------------------------------------------------------------
        *  Description
        *    Receives an array with the bytes of an unsigned integer in the following order:
        *      byte[0]:bits0..7
        *      byte[1]:bits8..15
        *      byte[2]:bits16..23
        *      byte[3]:bits24..32
        *    And converts them to a 32 bit unsigned integer.
        *  Parameters:
        *    by_to_convert: with the array of bytes to convert to an unsigned int 32. It
        *   must have 4 bytes. Less bytes or more bytes are not allowed.
        *  Return: 
        *    By reference:
        *    ui32_value: with the unsinged 32 that corresponds to the 4 recevied bytes.
        *  By value:
        *    >=0 received bytes have been converted to a 32 bits unsigned integer.
        *    <0 an error occurred 
        *******************************************************************************/
        static public int convert4BytesToUInt32(byte [] by_to_convert, ref UInt32 ui32_value){
            int i_ret_val = 0;


            if (by_to_convert.Length != 4){
                // ERROR: received invalid number of bytes
                i_ret_val = -1;
            }else{
                ui32_value = (UInt32)( (by_to_convert[3] << 24) | (by_to_convert[2] << 16) | (by_to_convert[1] << 8) | by_to_convert[0] );
            }//if

            return i_ret_val;

        }//convert4BytesToUInt32


        /*******************************************************************************
        *  convertUInt32To4Bytes 
        *------------------------------------------------------------------------------
        *  Description
        *    Receives a 32 bits unsigned integer and converts it to a 4 bytes array in
        *   the following order:
        *      byte[0]:bits0..7
        *      byte[1]:bits8..15
        *      byte[2]:bits16..23
        *      byte[3]:bits24..32
        *  Parameters:
        *     ui32_value: with the unsigned int 32 value that must be converted to a 4 
        *   bytes array.
        *  Return: 
        *    By reference:
        *     by_converted: a 4 elements byte array with the uint32 separed in 4 bytes.
        *  By value:
        *    >=0 received uint 32 has been succesfully converted to a 4 bytes array.
        *    <0 an error occurred 
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


    }//AuxFuncs

}
