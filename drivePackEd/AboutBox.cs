using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace drivePackEd {

    /***********************************************************************************************
    * @brief implements the About Form that shows the general information and current 
    * version of the application.
    ******************************************************************************************/
    partial class AboutBox : Form {
        private string strRemoteNewsWebsite = "";
        private string strLocalNewsWebsite = "";
        private string strRemoteMainWebsite = "";


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

        public AboutBox(string strPrName, string strLicense, string strDescription) {

            InitializeComponent();

        }//AboutBox

        /***********************************************************************************************
        * @brief delegate that processes the form Load event
        * 
        * @param[in]  sender
        * @param[in]  e
        ***********************************************************************************************/
        private void AboutBox_Load(object sender, EventArgs e) {
            string strCompTime = Properties.Resources.BuildDate;
            string strCurrDir = "";

            this.Text = String.Format("About {0} ", AssemblyTitle);
            this.lblProductName.Text = AssemblyProduct;
            this.lblVersion.Text = String.Format("Version {0} ", AssemblyVersion);
            this.lblBuild.Text = "Build date " + strCompTime;

            // initialize the URL of the remote webpage to show in the AboutDialog box web browser
            strRemoteNewsWebsite = "http://www.tolaemon.com/dpackeded/news.php";
            strRemoteNewsWebsite = strRemoteNewsWebsite.ToLower();

            // initialize the URL of the website main page. It is only used to check if there is
            // a valid connection to the web server or not
            strRemoteMainWebsite = "http://www.tolaemon.com/dpackeded/index.htm";
            strRemoteMainWebsite = strRemoteMainWebsite.ToLower();

            // initialize the URL of the local webpage to show in the AboutDialog box web browser in
            // case that the remote webpage is not avialable.
            strCurrDir = Directory.GetCurrentDirectory();
            strLocalNewsWebsite = "file:///" + strCurrDir + "/local.htm";
            strLocalNewsWebsite = strLocalNewsWebsite.Replace("\\", "/");
            strLocalNewsWebsite = strLocalNewsWebsite.ToLower();

            // open the remote webpage or the local webpage deppending on if the remote website is available or not
            // to avoid that calling the CheckWebPageExists() function generates an entry in the remote .txt file
            if (AuxFuncs.CheckWebPageExists(strRemoteMainWebsite)) {
                aboutWebBrowser.Navigate(new Uri(strRemoteNewsWebsite));
            } else {
                aboutWebBrowser.Navigate(new Uri(strLocalNewsWebsite));
            }

        }//AboutBox_Load

        /***********************************************************************************************
        * @brief delegate that processes the click on any of the links in the website shown in the web
        * browser.
        * @param[in]  sender
        * @param[in]  e
        ***********************************************************************************************/
        private void aboutWebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e) {
            string strURLtoOpen = "";

            // intercept the click event, and if the received page does not correspond to 
            // the "strRemoteStartupWebsite" or to the "strLocalStartupWebsite", then 
            // cancel it and open it in the external system's configured web browser
            // ( Chrome, IExplorer... ). Only the news.htm or the local.htm can be open
            // in the About Dialog Box web browser.
            strURLtoOpen = e.Url.ToString().ToLower();
            if ((strURLtoOpen != strRemoteNewsWebsite) && (strURLtoOpen != strLocalNewsWebsite)) {

                //cancel the current event
                e.Cancel = true;

                //this opens the received URL in the user's default browser
                // strURLtoOpen = e.Url.ToString();
                try {
                    System.Diagnostics.Process.Start("explorer", strURLtoOpen);
                } catch (System.ComponentModel.Win32Exception noBrowser) {
                    if (noBrowser.ErrorCode == -2147467259)
                        MessageBox.Show(noBrowser.Message);
                } catch (System.Exception other) {
                    MessageBox.Show(other.Message);
                }

            }//if

        }//aboutWebBrowser_Navigating

        /***********************************************************************************************
        * @brief delegate that processes the click on the Accept button
        * browser.
        * @param[in]  sender
        * @param[in]  e
        ***********************************************************************************************/
        private void btnAccept_Click(object sender, EventArgs e) {

            this.Close();

        }//btnAccept_Click

        /***********************************************************************************************
        * @brief delegate that processes the click on the License link label
        * browser.
        * @param[in]  sender
        * @param[in]  e
        ***********************************************************************************************/
        private void lnkLblLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            string strTarget = "https://creativecommons.org/licenses/by-nc-sa/4.0/";

            // Specify that the link was visited.
            lnkLblLicense.LinkVisited = true;

            try {
                System.Diagnostics.Process.Start("explorer", strTarget);
            } catch (System.ComponentModel.Win32Exception noBrowser) {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            } catch (System.Exception other) {
                MessageBox.Show(other.Message);
            }

        }//lnkLblLicense_LinkClicked

    }//  partial class AboutBox : Form

}
