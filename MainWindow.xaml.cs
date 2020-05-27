using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Net;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Management.Instrumentation;
using System.Diagnostics;

namespace SortSøndagDL {
    // TODO: fix date missing month
    // TODO: if dl explore window open focus to this instesd of opening new // Difficulty HIGH
    // TODO: next/back arrows for older episodes
    // TODO: when downloading no filetype, when done add filetype. to signify that file was dl incomplete
    // TODO: Play button to replace dl button. if file in dir, opens file in mediaplayer
    // TODO: Linkinput field to other RSS sources / split into other dirs
    // TODO: HASH ep description to replace date as unique identifyer
    // TODO: Split classes into seperate files
    public class Episode {
        private XmlNode xmlNode;

        public string BtnName { get; }
        private string Title { get; }
        private string DownloadUrl { get; }
        private string PubDate { get; }
        public string DT_date { get; }
        private MainWindow mainWindow;
        ProgressBar pg;
        Button epBtn;
        Button DLBtn;

        public Episode(MainWindow mainWindow, XmlNode xmlNode, int id) {
            this.mainWindow = mainWindow;
            this.xmlNode = xmlNode;
            
            BtnName = "dlep" + id.ToString();
            Title = this.xmlNode.SelectSingleNode("title").InnerText;
            DownloadUrl = this.xmlNode.SelectSingleNode("enclosure/@url").InnerText;
            PubDate = this.xmlNode.SelectSingleNode("pubDate").InnerText;
            DT_date = DateTime.Parse(PubDate).ToString("dd-mm-yyy");
            mainWindow.text.Text += "btn created\n";

            addEpisodeBtn();
        }

        public void addEpisodeBtn() {
            epBtn = new Button {
                Content = DT_date,
                Name = BtnName
            };
            epBtn.Click += new RoutedEventHandler(DLBtn_Click);
            mainWindow.DLSP.Children.Add(epBtn);

            pg = new ProgressBar {
                Name = BtnName + "pgb"
            };
            pg.HorizontalAlignment = HorizontalAlignment.Stretch;
            pg.Height = 0;
            mainWindow.epCtrl.Children.Add(pg);

            DLBtn = new Button {
                Content = "Download Udsendelse",
                Name = "Downloadep"
            };
            DLBtn.Click += new RoutedEventHandler(dlLink);

            mainWindow.epCtrl.Children.Add(DLBtn);
        }

        public void DLBtn_Click(object sender, RoutedEventArgs e) {
            //string btnName = (sender as Button).Name;
            mainWindow.text.Text += (string.Format("You clicked on the {0}. button. \n", BtnName));
            mainWindow.text.Text = Title + "\n";
            mainWindow.text.Text += DownloadUrl + "\n";
        }

        private void dlLink(object sender, RoutedEventArgs e) {
            mainWindow.text.Text = "download " + DownloadUrl;
            using (WebClient wc = new WebClient()) {                
                string dlP = mainWindow.DlPath + @"\" + DT_date + ".mp3";
                mainWindow.text.Text = "Download Path " + dlP;

                pg.Height = 20;
                DLBtn.Height = 0;

                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(new System.Uri(DownloadUrl), dlP);
                wc.DownloadFileCompleted += wc_Dlpgcmpl;
            }
        }
        
        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            pg.Value = e.ProgressPercentage;
        }

        void wc_Dlpgcmpl(object sender, AsyncCompletedEventArgs e) {
            pg.Height = 0;
            DLBtn.Height = 20;
        }
    }


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
