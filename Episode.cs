using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Net;
using System.ComponentModel;

namespace SortSøndagDL {
    public class Episode {
        private XmlNode xmlNode;

        private string BtnName { get; }
        private string Title { get; }
        private string DownloadUrl { get; }
        private string PubDate { get; }
        private string DT_date { get; }
        private MainWindow mainWindow;
        ProgressBar pg;
        Button epBtn;
        Button DLBtn;

        public Episode(MainWindow mainWindow, XmlNode xmlNode, int id) {
            // Set Variables
            this.mainWindow = mainWindow;
            this.xmlNode = xmlNode;
            BtnName = "dlep" + id.ToString();
            Title = this.xmlNode.SelectSingleNode("title").InnerText;
            DownloadUrl = this.xmlNode.SelectSingleNode("enclosure/@url").InnerText;
            PubDate = this.xmlNode.SelectSingleNode("pubDate").InnerText;
            DT_date = DateTime.Parse(PubDate).ToString("dd-mm-yyyy");

            AddEpisodeButton();
        }

        private void AddEpisodeButton() {
            //Add episode button to window
            epBtn = new Button {
                Content = DT_date,
                Name = BtnName
            };
            epBtn.Click += new RoutedEventHandler(_ShowEpisodeInfo_Click);
            mainWindow.SP_eps.Children.Add(epBtn);

            // Add progess Bar to window, Initially hidden
            pg = new ProgressBar {
                Name = BtnName + "pgb"
            };
            pg.HorizontalAlignment = HorizontalAlignment.Stretch;
            pg.Height = 0;
            mainWindow.SP_epCtrls.Children.Add(pg);

            //Add Download Button to window
            DLBtn = new Button {
                Content = "Download Udsendelse",
                Name = "Downloadep"
            };
            DLBtn.Click += new RoutedEventHandler(DownloadLinkEventHandler);

            mainWindow.SP_epCtrls.Children.Add(DLBtn);
        }

        private void _ShowEpisodeInfo_Click(object sender, RoutedEventArgs e) {
            //mainWindow.text.Text += (string.Format("You clicked on the {0}. button. \n", BtnName));
            mainWindow.text.Text = Title + "\n";
            mainWindow.text.Text += DownloadUrl + "\n";
        }

        private void DownloadLinkEventHandler(object sender, RoutedEventArgs e) {
            mainWindow.text.Text = "download " + DownloadUrl;
            using (WebClient wc = new WebClient()) {
                string dlP = mainWindow.DlPath + @"\" + DT_date + ".mp3";
                mainWindow.text.Text = "Download Path " + dlP;

                // Switch download button with progressbar 
                pg.Height = 20;
                DLBtn.Height = 0;

                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                wc.DownloadFileAsync(new System.Uri(DownloadUrl), dlP);
                wc.DownloadFileCompleted += Wc_DownloadProgressCompleted;
            }
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            // update progressbar
            pg.Value = e.ProgressPercentage;
        }

        private void Wc_DownloadProgressCompleted(object sender, AsyncCompletedEventArgs e) {
            //Switch back Download button from progrressbar
            pg.Height = 0;
            DLBtn.Height = 20;
        }
    }
}
