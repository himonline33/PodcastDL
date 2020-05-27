﻿using System;
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
        public List<Episode> eps;

        public MainWindow() {
            InitializeComponent();
            initDLDir();
            eps = new List<Episode>();
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

        private void _DL_RSS(object sender, RoutedEventArgs e) {
            DLSP.Children.Clear(); // clear stackpannel 
            epCtrl.Children.Clear();
            text.Text = ""; // clear textbox

            XmlNodeList nodeList = getRSS("channel//item");

            for (int i = 0; i < 10; i++) {
                Episode episode = new Episode(this, nodeList[i], i);
                eps.Add(episode);                
            }
        }

        private void initDLDir() {
            curDir = Directory.GetCurrentDirectory();
            //text.Text += curDir + "\n";
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

        private XmlNodeList getRSS(string selector) {
            // Load the document and set the root element.  
            XmlDocument doc = new XmlDocument();
            //doc.Load(@"https://www.dr.dk/mu/feed/sort-soendag.xml?format=podcast");
            doc.Load(@"https://www.dr.dk/mu/feed/p3-med-christian-og-maria.xml?format=podcast");
            XmlNode root = doc.DocumentElement;

            // Select all nodes where the book price is greater than 10.00.  
            return root.SelectNodes(selector);
        }

        private void _DL_Folder_Click(object sender, RoutedEventArgs e) {
            OpenFolder(DlPath);
        }
    }


}
