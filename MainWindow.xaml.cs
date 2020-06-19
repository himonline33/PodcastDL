using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml;
using System.IO;
using System.Diagnostics;
using RSSParser;
using System.Windows.Controls;
using System.Windows.Media;

namespace PodcastDL {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public string curDir;
        public string DlPath;
        public List<Episode> episodes = new List<Episode>();
        public RSSChannel rssChannel;
        public double spHeight;

        public MainWindow() {
            InitializeComponent();
            InitializeDLDirectories();



            TBOX_LinkBar.Text = @"https://www.dr.dk/mu/feed/sort-soendag.xml?format=podcast";            

            if (!File.Exists(curDir + @"\RSSLinks.txt")) {
                var RSSlinks = File.Create(curDir + @"\RSSLinks.txt");
                RSSlinks.Close();
                using (TextWriter tw = new StreamWriter(curDir + @"\RSSLinks.txt", true)) {
                    tw.Write(TBOX_LinkBar.Text);
                    tw.Close();
                }
            }
            else {
                string[] links = File.ReadAllText(curDir + @"\RSSLinks.txt").Split('\n');
                for (int i = 0; i < links.Length; i++) {
                    TextBlock tb = new TextBlock();
                    tb.Text = links[i];
                    tb.Background = Brushes.Yellow;
                    SP_Links.Children.Add(tb);
                }
            }
        }

        private void _Expand_Links(object sender, RoutedEventArgs e) {
            BT_Expand_Links.Visibility = Visibility.Hidden;
            BT_Collaps_Links.Visibility = Visibility.Visible;
            SP_Links.Visibility = Visibility.Visible;
        }

        private void _Collaps_Links(object sender, RoutedEventArgs e) {
            BT_Expand_Links.Visibility = Visibility.Visible;
            BT_Collaps_Links.Visibility = Visibility.Collapsed;
            SP_Links.Visibility = Visibility.Collapsed;
        }


        private bool LinkExists(string link, string filePath) {
            string[] links = File.ReadAllText(filePath).Split('\n');
            bool exists = false;

            foreach (string RSSlink in links) {
                if (RSSlink == link) exists = true;
            }

            return exists;
        }

        private void InitializeDLDirectories(string channel = "") {
            curDir = Directory.GetCurrentDirectory();
            DlPath = curDir + @"\Udsendelser";
            if (! (channel == "")) {
                DlPath += @"\" + channel;
            }
            try {
                if (!Directory.Exists(DlPath)) {
                    Directory.CreateDirectory(DlPath);
                    Environment.CurrentDirectory = (DlPath);
                    if (Directory.GetCurrentDirectory() == DlPath) {
                        TBLK_Content.Text += "Path Created: " + DlPath;
                    }
                }
            }
            catch (Exception e) {
                TBLK_Content.Text = e.Message;
            }
        }

        private void _Download_RSS(object sender, RoutedEventArgs e) {
            SP_episodes.Children.Clear(); // clear stackpannel 
            SP_episodesCtrls.Children.Clear();

            if (!LinkExists(TBOX_LinkBar.Text, curDir + @"\RSSLinks.txt")) {
                using (TextWriter tw = new StreamWriter(curDir + @"\RSSLinks.txt", true)) {
                    tw.WriteLine(TBOX_LinkBar.Text);
                    tw.Close();
                }
            }

            rssChannel = new RSSChannel(TBOX_LinkBar.Text);
            InitializeDLDirectories(rssChannel.Title);

            TBLK_Content.Text = rssChannel.Title + "\n";
            TBLK_Content.Text += rssChannel.Link  + "\n";
            TBLK_Content.Text += rssChannel.Description  + "\n";
            TBLK_Content.Text += rssChannel.Language  + "\n";
            TBLK_Content.Text += rssChannel.Copyright  + "\n";
            TBLK_Content.Text += rssChannel.ManagingEditor  + "\n";
            TBLK_Content.Text += rssChannel.WebMaster  + "\n";
            TBLK_Content.Text += rssChannel.PubDate  + "\n";
            TBLK_Content.Text += rssChannel.LastBuildDate  + "\n";
            TBLK_Content.Text += rssChannel.Catagory  + "\n";
            TBLK_Content.Text += rssChannel.Generator  + "\n";
            TBLK_Content.Text += rssChannel.Docs  + "\n";
            TBLK_Content.Text += rssChannel.Rating  + "\n";

            List<RSSItem> rssItems = rssChannel.GetItemRange(0, 30);
            for (int i = 0; i < rssItems.Count; i++) {
                Episode episode = new Episode(this, rssItems[i], i);
                episodes.Add(episode);
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
