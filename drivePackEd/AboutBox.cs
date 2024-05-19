using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace drivePackEd {


    /***********************************************************************************************
    * @brief implements the About Form that shows the general information and current 
    * version of the application.
    ***********************************************************************************************/
    partial class AboutBox : Form {
        string strCompTime = "";// Properties.Resources.BuildDate;
        string strGitInfo = "";//Properties.Resources.GitInfo;

        public AboutBox(string strPrName, string strLicense, string strDescription) {

            InitializeComponent();

            this.Text = String.Format("About {0} ", AssemblyTitle);
            this.lblProductName.Text = AssemblyProduct;
            this.lblVersion.Text = String.Format("Version {0} ", AssemblyVersion);
            this.lblBuild.Text = "Build date " + strCompTime;
            this.textBoxDescription.Text = strGitInfo.Replace("\n", "\r\n"); ; // + " strVersion:" + strVersion + " assemblyProduct:" + AssemblyProduct  + " AssemblyTitle:" + AssemblyTitle + " AssemblyVersion:" + AssemblyVersion +" Assembly description:" + AssemblyDescription;

        }

        #region Descriptores de acceso de atributos de ensamblado

        public string AssemblyTitle {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0) {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "") {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion {
            get {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        /***********************************************************************************************
        * @brief delegate that processes the click on the close button.
        * 
        * @param[in]  sender
        * @param[in]  e
        ***********************************************************************************************/
        private void okButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        /***********************************************************************************************
        * @brief delegate that processes the click on the project license link label
        * 
        * @param[in]  sender
        * @param[in]  e
        ***********************************************************************************************/
        private void linkLblLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {

            // Specify that the link was visited.
            linkLblLicense.LinkVisited = true;

            string target = "http://www.tolaemon.com/dpack/download.htm";
            try {
                System.Diagnostics.Process.Start(target);
            } catch (System.ComponentModel.Win32Exception noBrowser) {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            } catch (System.Exception other) {
                MessageBox.Show(other.Message);
            }

        }//linkLblLicense_LinkClicked

        /***********************************************************************************************
        * @brief delegate that processes the click on the source code link label
        * 
        * @param[in]  sender
        * @param[in]  e
        ***********************************************************************************************/
        private void linkLblSoruce_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {

            // Specify that the link was visited.
            linkLblLicense.LinkVisited = true;

            string target = "https://github.com/synthkore/drivepacked";
            try {
                System.Diagnostics.Process.Start(target);
            } catch (System.ComponentModel.Win32Exception noBrowser) {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            } catch (System.Exception other) {
                MessageBox.Show(other.Message);
            }

        }//linkLblSoruce_LinkClicked

        /***********************************************************************************************
        * @brief delegate that processes the click on the BeHex project and license link label
        * 
        * @param[in]  sender
        * @param[in]  e
        ***********************************************************************************************/
        private void linkLblBeHex_Click(object sender, EventArgs e) {


            // Specify that the link was visited.
            linkLblLicense.LinkVisited = true;

            string target = "https://hexbox.sourceforge.net/";
            try {
                System.Diagnostics.Process.Start(target);
            } catch (System.ComponentModel.Win32Exception noBrowser) {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            } catch (System.Exception other) {
                MessageBox.Show(other.Message);
            }

        }//linkLblBeHex_Click

    }//  partial class AboutBox : Form

}
