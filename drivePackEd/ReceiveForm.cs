﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;


namespace drivePackEd {

    public partial class ReceiveForm : Form {

        public MainForm parentRef = null;
        public cLogsNErrors statusLogsRef = null;
        public cDrivePackData drivePackRef = null;
        cComs commsObj = null;


        /*******************************************************************************
        * @brief form class default constructor
        *******************************************************************************/
        public ReceiveForm() {

            InitializeComponent();

            // get a list of serial port names and initialize the ComboBox with the names of available prots
            string[] strArr_ports = SerialPort.GetPortNames();
            if (strArr_ports.Length > 0) {
                foreach (string str_portName in strArr_ports) {
                    comboBox1.Items.Add(str_portName);
                }
                comboBox1.Text = comboBox1.Items[0].ToString();
            }

            // create the communications object
            commsObj = new cComs();

        }//ReceiveForm


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void ReceiveForm_Closing(object sender, FormClosingEventArgs e) {

            parentRef.receiveRomForm.Dispose();
            parentRef.receiveRomForm = null;

        }//ReceiveForm_Closing


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void CancelButton_Click(object sender, EventArgs e) {

            // close the Receive form 
            this.Close();

        }//CancelButton_Click


        /*******************************************************************************
        * @brief 
        * @param[in] sender reference to the object that raises the event
        * @param[in] e the information related to the event
        *******************************************************************************/
        private void ReceiveButton_Click(object sender, EventArgs e) {
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_temp_file = "temp.drp";
            string str_aux = "";


            // once clicked disable the button to avoid that the user clicks it again 
            receiveButton.Enabled = false;

            // before operating, the state of the general configuration parameters of the application
            // is taken to work with the latest parameters set by the user.
            parentRef.UpdateConfigParametersWithAppState();
            statusLogsRef.SetAppBusy(true);

            // informative message of the action that is being executed
            str_aux = "Receiving \"" + str_temp_file + "\\\" file ...";
            statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, MainForm.COMMAND_RECEIVE_FILE + str_aux, false);

            // first call the function that receives the  file from remote computer and saves it to a temporary file
            ec_ret_val = commsObj.receive_file_1kXmodem(comboBox1.Text, str_temp_file, ref this.progressBar1, ref this.label2);
            if (ec_ret_val.i_code < 0) {

                // shows the file receive and save to file error message to the user and in the logs
                str_aux = ec_ret_val.str_description + " Error receiving file \"" + str_temp_file + "\".";
                statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, MainForm.COMMAND_RECEIVE_FILE + str_aux, true);

            } else {

                // informative log message with the result of the operation
                str_aux = "\"" + str_temp_file + "\\\" file succesfully received and saved.";
                statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, MainForm.COMMAND_RECEIVE_FILE + str_aux, false);

                str_aux = "Loading file \"" + str_temp_file + "\\\" to current ROM ...";
                statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, MainForm.COMMAND_RECEIVE_FILE + str_aux, false);

                // load the received temporary file from disk to the program current ROM in memory
                ec_ret_val = drivePackRef.loadDRPFile(str_temp_file);
                if (ec_ret_val.i_code < 0) {

                    // shows the file load error message to the user and in the logs
                    str_aux = ec_ret_val.str_description + " Error loading file \"" + str_temp_file + "\" to current ROM.";
                    statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, MainForm.COMMAND_RECEIVE_FILE + str_aux, true);

                } else {

                    // shows the file send error message to the user and in the logs
                    str_aux = "\"" + str_temp_file + "\" file succesfully loaded to current ROM.";
                    statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, ec_ret_val, MainForm.COMMAND_RECEIVE_FILE + str_aux, true);

                }//if

            }//if

            // after receiving the file close the Receive form 
            this.Close();

            // update the main form with received content
            statusLogsRef.SetAppBusy(false);
            parentRef.RefreshHexEditor();

            // once the operation has finished reenbale the button
            receiveButton.Enabled = true;

        }//ReceiveButton_Click

    }
}
