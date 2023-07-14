using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;


namespace drivePackEd
{
    public partial class Form2 : Form
    {
        public Form1 parentRef = null;
        public cLogsNErrors statusLogsRef = null;
        public cDrivePackData drivePackRef = null;

        cComs commsObj = null;


        public Form2(){

            InitializeComponent();
            
            // get a list of serial port names and initialize the ComboBox with the names of available prots
            string[] strArr_ports = SerialPort.GetPortNames();
            if (strArr_ports.Length > 0){
                foreach (string str_portName in strArr_ports){
                    comboBox1.Items.Add(str_portName);
                }
                comboBox1.Text = comboBox1.Items[0].ToString();
            }

            // create the communications object
            commsObj = new cComs();

        }



        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentRef.sendRomForm.Dispose();
            parentRef.sendRomForm = null;
        }



        private void button1_Click(object sender, EventArgs e){
            ErrCode ec_ret_val = cErrCodes.ERR_NO_ERROR;
            string str_temp_file = "temp.drp";
            string str_aux = "";


            // informative message explaining  the actions that are going to be executed
            str_aux = "Saving current ROM to \"" + str_temp_file + "\\\" file ...";
            statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, Form1.COMMAND_SEND_FILE + str_aux, false);

            // first call the function that stores current drive pack conent to a temporary file in disk
            ec_ret_val = drivePackRef.saveDRPFile(str_temp_file);
            if (ec_ret_val.i_code < 0){

                // shows the file load error message to the user and in the logs
                str_aux = ec_ret_val.str_description + "Error saving current ROM in file \"" + str_temp_file + "\".";
                statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, Form1.COMMAND_SEND_FILE + str_aux, true);
            
            }else{

                // informative log message with the result of the operation
                str_aux = "Current ROM succesfully saved to file \"" + str_temp_file + "\\\".";
                statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, Form1.COMMAND_SEND_FILE + str_aux, false);

                // informative log message with operation to execute
                str_aux = "Sending \"" + str_temp_file + "\\\" file ...";
                statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, cErrCodes.ERR_NO_ERROR, Form1.COMMAND_SEND_FILE + str_aux, false);

                // send the specified file through the specified com port
                ec_ret_val = commsObj.send_file_1kXmodem(comboBox1.Text, str_temp_file,ref this.progressBar1, ref this.label2);

                if (ec_ret_val.i_code <0) { 

                    // shows the send file error message to the user and in the logs
                    str_aux = ec_ret_val.str_description + "Could not send the specified \""+ str_temp_file + "\" file.";
                    statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_ERROR, ec_ret_val, Form1.COMMAND_SEND_FILE + str_aux, true);

                }else{

                    // shows the file send error message to the user and in the logs
                    str_aux = "\"" + str_temp_file + "\" file succesfully sent.";
                    statusLogsRef.WriteMessage(-1, -1, cLogsNErrors.status_msg_type.MSG_INFO, ec_ret_val, Form1.COMMAND_SEND_FILE + str_aux, true);

                }//if

            }//if

            // after sending the file close the Send form 
            this.Close();

        }//button1_Click



        private void Form2_Load(object sender, EventArgs e){

        }//Form2_Load



        private void button2_Click(object sender, EventArgs e){

            // close the Send form 
            this.Close();

        }//button2_Click

    }//public partial class Form2 : Form

}
