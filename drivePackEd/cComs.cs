using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace drivePackEd{

    public class cComs {
        const int CHAR_1KXMODEM_C = 67;
        const int CHAR_1KXMODEM_STX = 2;
        const int CHAR_1KXMODEM_ACK = 6;
        const int CHAR_1KXMODEM_NACK = 21;
        const int CHAR_1KXMODEM_EOT = 4;

        const int MAX_START_TX_TOUT = 15000;
        const int MAX_START_RX_TOUT = 15000;
        const int MAX_START_RX_ACK_TOUT = 5000;
        const int MAX_RX_PACKET_TOUT = 5000;
        const int MAX_SEND_C_TOUT = 2000;

        static SerialPort _serialPort;

        // attributes used as parameters for the Send and Receive threads
        private string str_comPort = "";
        private string str_file_name = "";
        public ErrCode errCodeThreadResult = cErrCodes.ERR_NO_ERROR;


        /*******************************************************************************
        *  Default constructor 
        *------------------------------------------------------------------------------
        *  Description
        *  Default constructor 
        *  Parameters:
        *  Return:
        *    By reference:
        *    By value:
        *******************************************************************************/
        public cComs(){

            _serialPort = new SerialPort();

        }//cComs



        /*******************************************************************************
        *  USART_1KXmodem_calcrc 
        *------------------------------------------------------------------------------
        *  Description
        *     Receives a byte buffer and calculates the CRC of the first i_count bytes
        *  in that buffer.
        *  Parameters:
        *     buffer: the buffer with the bytes to calculcate the CRC
        *     i_count: the number of bytes of the buffer on which the CRC must be calculated
        *  Return:
        *    By reference:
        *    By value:
        *    ec_ret_val
        * Note: *************************************************************************
        *   Based on .C function:
        *   int16_t USART_1KXmodem_calcrc(uint8_t *ptr, int count){
        *     int16_t crc;
        *     uint8_t i;
        * 
        *     crc = 0;
        *     while (--count >= 0){
        *
        *         crc = crc ^ (int) *ptr++ << 8;
        *         i = 8;
        *         do{
        *
        *           if (crc & 0x8000){
        *               crc = crc << 1 ^ 0x1021;
        *           }else{
        *              crc = crc << 1;
        *             }//if
        *
        *         }while(--i);
        *     }
        *     return (crc);
        *******************************************************************************/
        private int calculate_crc(byte[] buf, int count){
            int crc;
            uint i;
            int i_index = 0;

            crc = 0;
            while (--count >= 0){

                crc = crc ^ (int)buf[i_index++] << 8;
                i = 8;
                do{
                    if ((crc & 0x8000) != 0){
                        crc = crc << 1 ^ 0x1021;
                    }else{
                        crc = crc << 1;
                    }//if

                } while (--i > 0);

            }//while

            return (crc);

        }//calculate_crc



        /*******************************************************************************
        *  USART_1KXmodem_calcrc 
        *------------------------------------------------------------------------------
        *  Description
        *     Receives a byte buffer and calculates the CRC of the first i_count bytes
        *  in that buffer.
        *  Parameters:
        *     buffer: the buffer with that containst he bytes to calculcate the CRC
        *     start: the index of the byte of the buffer to start calculating the CRC
        *     end: the index of the last byte of the buffer to calculate the CRC
        *  Return:
        *    By reference:
        *    By value:
        *    ec_ret_val
        * Note: *************************************************************************
        *   Based on .C function:
        *   int16_t USART_1KXmodem_calcrc(uint8_t *ptr, int count){
        *     int16_t crc;
        *     uint8_t i;
        * 
        *     crc = 0;
        *     while (--count >= 0){
        *
        *         crc = crc ^ (int) *ptr++ << 8;
        *         i = 8;
        *         do{
        *
        *           if (crc & 0x8000){
        *               crc = crc << 1 ^ 0x1021;
        *           }else{
        *              crc = crc << 1;
        *             }//if
        *
        *         }while(--i);
        *     }
        *     return (crc);
        *******************************************************************************/
        private int calculate_crc_start_end(byte[] buf, int start, int end){
            int crc;
            uint i;
            int i_index = start;

            crc = 0;
            while (--end >= start){

                crc = crc ^ (int)buf[i_index++] << 8;
                i = 8;
                do{
                    if ((crc & 0x8000) != 0){
                        crc = crc << 1 ^ 0x1021;
                    }else{
                        crc = crc << 1;
                    }//if

                } while (--i > 0);

            }//while

            return (crc);

        }//calculate_crc_start_end



        /*******************************************************************************
        *  send_file_1kXmodem 
        *------------------------------------------------------------------------------
        *  Description
        *     Reads specfied file and sends it through a 1kXmodem connection through
        *  the specified serial port.
        *  Parameters:
        *     str_comPort: string wiht the serial port to use to send the file
        *     str_file_name: name (with path) of the file to send to the remote system.
        *     ctrl_sendProgressBar: reference to the control used to show the progress
        *   of file transmission.
        *     ctrl_sendLabel: reference to the label to show the the progress of the
        *   file transmission.
        *  Return:
        *    By reference:
        *    By value:
        *    ec_ret_val
        *******************************************************************************/
        public ErrCode send_file_1kXmodem(string str_comPort, string str_file_name, ref ProgressBar ctrl_sendProgressBar, ref Label ctrl_sendLabel){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream = null;
            BinaryReader file_binary_reader = null;
            Stopwatch stopWatch = new Stopwatch();
            byte[] serial_buffer = new byte[1029];//3 + 1024 + 2 bytes
            byte[] packet_data = new byte[1024];//1024
            byte[] file_buffer = new byte[1024];//1024
            int i_packet_no = 0;
            int i_crc = 0;
            int i_aux = 0;
            float fl_aux = 0.0f;
            bool b_end_of_file = false;


            // first check if the specified file name exists
            if (!File.Exists(str_file_name)){

                ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_OPEN_TEMP_FILE;

            }else{

                // open specified file
                file_stream = new FileStream(str_file_name, FileMode.Open);
                file_binary_reader = new BinaryReader(file_stream);
                if (file_binary_reader == null){

                    ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_OPEN_TEMP_FILE;

                }//if

            }//if

            if (ec_ret_val.i_code >= 0) {

                try{

                    // configure and open the serial port connection on the specified str_comPort
                    _serialPort.PortName = str_comPort;
                    _serialPort.BaudRate = 115200;
                    _serialPort.DataBits = 8;
                    _serialPort.StopBits = StopBits.One;
                    _serialPort.Parity = Parity.None;
                    _serialPort.Open();
                    _serialPort.DiscardInBuffer();

                    // sender keeps waiting to receive CHAR_1KXMODEM_C code from receiver. CHAR_1KXMODEM_C code 
                    // indicates that receiver is ready to start file transmission ( then sender will send the 
                    // firt 1Kxmodem packet )
                    stopWatch.Restart();
                    serial_buffer[0] = 0;
                    while ( (serial_buffer[0]!= CHAR_1KXMODEM_C) && (stopWatch.ElapsedMilliseconds<MAX_START_TX_TOUT)) {

                        // keep checking if there is any byte in the serial port and read it if affirmative
                        if (_serialPort.BytesToRead > 0){
                            _serialPort.Read(serial_buffer, 0, 1);
                        }

                        Application.DoEvents();

                    }//while
                    // check if 'C' char code has been received or if there was a timeout
                    if (serial_buffer[0] != CHAR_1KXMODEM_C){
                        ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_SEND_TIMEOUT;
                    }//if
                    
                    b_end_of_file = false;
                    i_packet_no = 1;
                    while ((ec_ret_val.i_code >= 0) && (!b_end_of_file)){

                        // check if the file has been completely sent
                        if (file_binary_reader.BaseStream.Position >= file_binary_reader.BaseStream.Length){
                            
                            // file has been completely sent so send the EOT char code and leave the transimission loope
                            serial_buffer[0] = CHAR_1KXMODEM_EOT;
                            _serialPort.Write(serial_buffer, 0, 1);
                            b_end_of_file = true;

                        }else{ 
                            
                            // there are still bytes in file to send 

                            // read next block of data bytes from file and store them in the the packet_data buffer
                            file_buffer = file_binary_reader.ReadBytes(1024);
                            Array.Fill(packet_data, (byte)0); // fill  packet_data buffer with '0's, so if read bytes are less than 1024 then other bytes will remain to '0'
                            for (i_aux = 0; i_aux<file_buffer.Length; i_aux++){
                                packet_data[i_aux] = file_buffer[i_aux];
                            }//for

                            // prepare the 1kXModem packet to send
                            // HEADER: set the header in the packet
                            serial_buffer[0] = 2;// STX = 2
                            serial_buffer[1] = (byte)i_packet_no; // packet number
                            serial_buffer[2] = (byte)(~serial_buffer[1]); // inverted packet number
                            // DATA: set read data in the packet
                            for (i_aux = 0; i_aux<1024; i_aux++){
                                serial_buffer[3+i_aux] = packet_data[i_aux];
                            }
                            // TAIL: calculate the crc of the data and set it at the end of the packet
                            i_crc = calculate_crc(packet_data, 1024);
                            serial_buffer[1027] = (byte)((i_crc & 0xFF00)>>8);
                            serial_buffer[1028] = (byte)(i_crc & 0x00FF);

                            // send the 1kXmodem packet through specified COM port                            
                            _serialPort.Write(serial_buffer, 0, 1029);

                            // once the packet has been sent keep waiting for the ACK from the receiver
                            stopWatch.Restart();
                            serial_buffer[0] = 0;
                            while ((serial_buffer[0] != CHAR_1KXMODEM_ACK) && (stopWatch.ElapsedMilliseconds < MAX_START_RX_ACK_TOUT)){
                                // check if there is any byte in the serial port and read it if affirmative
                                if (_serialPort.BytesToRead > 0) _serialPort.Read(serial_buffer, 0, 1);
                            }//while

                            // if no ACK has been received that means that previous loop was left due TIMEOUT condition
                            if (serial_buffer[0] != CHAR_1KXMODEM_ACK){                            
                                ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_SEND_TIMEOUT;
                            }

                            // set operation progress here ( from 0 to 33 packets )
                            fl_aux = (((float)i_packet_no) / 33) * 100;
                            i_aux = (int)fl_aux;
                            if (ctrl_sendProgressBar != null) ctrl_sendProgressBar.Value = (int)i_aux;
                            if (ctrl_sendLabel != null) ctrl_sendLabel.Text = "Send progress ( " + i_aux.ToString() + "%):";
                            Application.DoEvents();

                            // prepare some transfer variables for next packet
                            i_packet_no++;

                        }//if (file_binary_reader.BaseStream.Position >= file_binary_reader.BaseStream.Length){

                    }//while

                    _serialPort.Close();

                }catch (Exception ex) {

                    ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_SENDING;
                    ec_ret_val.str_description = ec_ret_val.str_description + ex.Message;

                }//try

             }//if

            // close serial port
            if (_serialPort.IsOpen) _serialPort.Close();

            // close transfered file
            if (file_binary_reader != null) file_stream.Close();

            return ec_ret_val;

        }//send_file_1kXmodem



        /*******************************************************************************
        *  receive_file_1kXmodem 
        *------------------------------------------------------------------------------
        *  Description
        *     Receives a file through a 1kXmodem connection on the specified serial port
        *  and saves it to disk with the specified name.
        *  Parameters:
        *     str_comPort: string with the serial port to use to receive the file
        *     str_file_name: name (with path) of the file to save to disk.
        *     ctrl_sendProgressBar: reference to the control used to show the progress
        *   of file reception.
        *     ctrl_sendLabel: reference to the label to show the the progress of the
        *   file reception.
        *  Return:
        *    By reference:
        *    By value:
        *    ec_ret_val
        *******************************************************************************/
        public ErrCode receive_file_1kXmodem(string str_comPort, string str_file_name, ref ProgressBar ctrl_sendProgressBar, ref Label ctrl_sendLabel){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            FileStream file_stream = null;
            BinaryWriter file_binary_writer = null;
            Stopwatch stopWatch1 = new Stopwatch();
            Stopwatch stopWatch2 = new Stopwatch();
            byte[] serial_buffer = new byte[1029];//3 + 1024 + 2 bytes
            byte[] file_buffer = new byte[1024];//1024
            int i_packet_no = 1;
            int i_read_pack_bytes = 0;
            byte b_crc1 = 0;
            byte b_crc2 = 0;
            int i_aux = 0;
            float fl_aux = 0.0f;

            try{

                // before creating the new file to store received data, check if it already exists and delete it if affirmative
                if (File.Exists(str_file_name)){ File.Delete(str_file_name); }
                // create the specified file where the received data will be saved to
                file_stream = new FileStream(str_file_name, FileMode.CreateNew);
                file_binary_writer = new BinaryWriter(file_stream);
                if (file_binary_writer == null){
                    ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_OPEN_TEMP_FILE;
                }//if

                if (ec_ret_val.i_code >= 0) {

                    // configure and open the serial port connection on the specified str_comPort
                    _serialPort.PortName = str_comPort;
                    _serialPort.BaudRate = 115200;
                    _serialPort.DataBits = 8;
                    _serialPort.StopBits = StopBits.One;
                    _serialPort.Parity = Parity.None;
                    _serialPort.Open();
                    _serialPort.DiscardInBuffer();

                    // the receiver keeps sending the byte 'C' to the sender at regular intervals until the sender
                    // responds with the first packet, or until the maximum configured wait time elapses. With 
                    // CHAR_1KXMODEM_C code the receiver indicates to sender that is ready to start file reception
                    // ( then sender will answer sending the first 1Kxmodem packet )
                    stopWatch1.Restart();
                    stopWatch2.Restart();
                    i_read_pack_bytes = 0;
                    serial_buffer[0] = 0;
                    while ( (serial_buffer[0] != CHAR_1KXMODEM_STX)&&(stopWatch1.ElapsedMilliseconds<MAX_START_RX_TOUT) ){

                        // keep sending CHAR_1KXMODEM_C char at MAX_SEND_C_TOUT time interval
                        if (stopWatch2.ElapsedMilliseconds > MAX_SEND_C_TOUT){
                            serial_buffer[0] = CHAR_1KXMODEM_C;
                            _serialPort.Write(serial_buffer, 0, 1);
                            stopWatch2.Restart();
                        }//if

                        // keep checking if sender has answered to the CHAR_1KXMODEM_C by sending the first data packet
                        if (_serialPort.BytesToRead > 0) {
                            _serialPort.Read(serial_buffer, 0, 1);
                            i_read_pack_bytes = 1;
                        }//if

                        Application.DoEvents();

                    }//while
                    // check if CHAR_1KXMODEM_STX char code has been received or if there was a timeout
                    if (serial_buffer[0] != CHAR_1KXMODEM_STX){
                         ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_RECEIVE_TIMEOUT;
                    }//if

                    // keep reading the bytes of the packets sent by the remote computer until the CHAR_1KXMODEM_EOT
                    // is received or any error occurs.
                    stopWatch1.Restart();
                    while ((ec_ret_val.i_code >= 0) && (serial_buffer[0]!= CHAR_1KXMODEM_EOT)){

                        if (_serialPort.BytesToRead != 0){

                            // read received bytes and store them in the serial buffer
                            i_aux = _serialPort.BytesToRead;
                            _serialPort.Read(serial_buffer, i_read_pack_bytes, i_aux);
                            i_read_pack_bytes = i_read_pack_bytes + i_aux;
           
                            if (i_read_pack_bytes>=1029){

                                // all the bytes of the packet have been received, so its time to do all checks and store it
                                i_aux = calculate_crc_start_end(serial_buffer, 3, 1027);       
                                b_crc1 = (byte)((i_aux & 0xFF00)>>8);
                                b_crc2 = (byte)(i_aux & 0x00FF);

                                // check the received packet
                                if (serial_buffer[0] != CHAR_1KXMODEM_STX){
                                    // first byte of the packet should be an CHAR_1KXMODEM_STX char
                                    ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_RECEIVING;
                                }else if (serial_buffer[1] != (byte)i_packet_no){
                                    // received packet number does not correspond to the expected packet number
                                    ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_RECEIVING;
                                }else if (serial_buffer[1] != (byte)(~serial_buffer[2]) ){
                                    // received inverted packet number does not match with NOT inverted received packet number
                                    ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_RECEIVING;
                                }else if ( (serial_buffer[1027]!=b_crc1) || (serial_buffer[1028]!=b_crc2) ){
                                    // calculated CRC does not match with received CRC
                                    ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_RECEIVING;
                                }else{

                                    // store received packet to file: 3 corresponds to the position in the serial buffer  
                                    // of the first data byte and 1024 is the size of the serial_buffer
                                    file_binary_writer.Write(serial_buffer, 3, 1024);

                                    // send the ACK to sender indicating that packet has bee succesfully received
                                    // and we are ready to receive next packet
                                    serial_buffer[0] = CHAR_1KXMODEM_ACK;
                                    _serialPort.Write(serial_buffer, 0, 1);

                                    // set operation progress here ( from 0 to 33 packets )
                                    fl_aux = (((float)i_packet_no) / 33) * 100;
                                    i_aux = (int)fl_aux;
                                    if (ctrl_sendProgressBar != null) ctrl_sendProgressBar.Value = (int)i_aux;
                                    if (ctrl_sendLabel != null) ctrl_sendLabel.Text = "Receive progress ( " + i_aux.ToString() + "%):";
                                    Application.DoEvents();

                                    // prepare some transfer variables for next packet
                                    i_packet_no++;
                                    i_read_pack_bytes = 0;
                                    stopWatch1.Restart();


                                }//if

                            }//if (serial_buffer[0] != CHAR_1KXMODEM_STX)

                            // check if the maximum allowed time be
                            if (stopWatch1.ElapsedMilliseconds > MAX_RX_PACKET_TOUT){
                                ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_RECEIVE_TIMEOUT;
                            }

                        }//if (_serialPort.BytesToRead != 0)

                    }//while

                }//if

            }catch (Exception ex) {
                       
                ec_ret_val = cErrCodes.ERR_FILE_1KXMODEM_SENDING;
                ec_ret_val.str_description = ec_ret_val.str_description + ex.Message;

            }//try

            // close serial port
            if  (_serialPort.IsOpen) _serialPort.Close();

            // close created file
            if (file_binary_writer != null) file_stream.Close();

            return ec_ret_val;

        }//receive_file_1kXmodem

    }//class ccoms

}//namespace drivePackEd
