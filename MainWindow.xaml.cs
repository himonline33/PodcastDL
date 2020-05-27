using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace SortSøndagDL {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public string curDir;
        public string DlPath;
        public List<Episode> eps = new List<Episode>();

        public MainWindow() {
            InitializeComponent();
            InitializeDLDirectory();
        }

        private void InitializeDLDirectory() {
            curDir = Directory.GetCurrentDirectory();
            DlPath = curDir + @"\Udsendelser";
            try {
                if (!Directory.Exists(DlPath)) {
                    Directory.CreateDirectory(DlPath);
                    Environment.CurrentDirectory = (DlPath);
                    if (Directory.GetCurrentDirectory() == DlPath) {
                        text.Text += "Path Created: " + DlPath;
                    }
                }
            }
            catch (Exception e) {
                text.Text = e.Message;
            }
        }

        private XmlNodeList GetRSS(string selector) {
            // Load the document and set the root element.  
            XmlDocument doc = new XmlDocument();
            //doc.Load(@"https://www.dr.dk/mu/feed/sort-soendag.xml?format=podcast");
            doc.Load(@"https://www.dr.dk/mu/feed/p3-med-christian-og-maria.xml?format=podcast");
            XmlNode root = doc.DocumentElement; 
            return root.SelectNodes(selector);
        }

        private void _Download_RSS(object sender, RoutedEventArgs e) {
            SP_eps.Children.Clear(); // clear stackpannel 
            SP_epCtrls.Children.Clear();
            text.Text = ""; // clear textbox

            XmlNodeList nodeList = GetRSS("channel//item");

            for (int i = 0; i < 10; i++) {
                Episode episode = new Episode(this, nodeList[i], i);
                eps.Add(episode);
            }
        }

        private void _Download_Folder_Click(object sender, RoutedEventArgs e) {
            OpenFolder(DlPath);
        }

        private void OpenFolder(string folderPath) {
            if (Directory.Exists(folderPath)) {
                ProcessStartInfo startInfo = new ProcessStartInfo {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
            }
            else MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
        }
    }


}
