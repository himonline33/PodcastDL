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
using RSSParser;

namespace PodcastDL {
    public class Episode {
        private RSSItem rssItem;

        private string BtnName { get; }
        private MainWindow mainWindow;
        ProgressBar pg;
        Button epBtn;
        Button DLBtn;

        public Episode(MainWindow mainWindow, RSSItem rssItem, int id) {
            // Set Variables
            this.mainWindow = mainWindow;
            this.rssItem = rssItem;
            BtnName = "dlep" + id.ToString();

            AddEpisodeButton();
        }

        private void AddEpisodeButton() {
            //Add episode button to window
            epBtn = new Button {
                Content = rssItem.PubDate,
                Name = BtnName
            };
            epBtn.Click += new RoutedEventHandler(_ShowEpisodeInfo_Click);
            mainWindow.SP_episodes.Children.Add(epBtn);

            // Add progess Bar to window, Initially hidden
            pg = new ProgressBar {
                Name = BtnName + "pgb"
            };
            pg.HorizontalAlignment = HorizontalAlignment.Stretch;
            pg.Height = 20;
            pg.Visibility = Visibility.Collapsed;
            mainWindow.SP_episodesCtrls.Children.Add(pg);

            //Add Download Button to window
            DLBtn = new Button {
                Content = "Download Udsendelse",
                Name = "Downloadep"
            };
            DLBtn.Click += new RoutedEventHandler(DownloadLinkEventHandler);

            mainWindow.SP_episodesCtrls.Children.Add(DLBtn);
        }

        private void _ShowEpisodeInfo_Click(object sender, RoutedEventArgs e) {
            mainWindow.TBLK_Content.Text = rssItem.Title + "\n";
            mainWindow.TBLK_Content.Text += rssItem.enclosure.URL + "\n";
        }

        private void DownloadLinkEventHandler(object sender, RoutedEventArgs e) {
            mainWindow.TBLK_Content.Text = "download " + rssItem.enclosure.URL;
            using (WebClient wc = new WebClient()) {
                string dlP = mainWindow.DlPath + @"\" + rssItem.PubDateNoSpace + ".mp3";
                mainWindow.TBLK_Content.Text = "Download Path " + dlP;

                // Switch download button with progressbar 
                pg.Visibility = Visibility.Visible;
                DLBtn.Visibility = Visibility.Collapsed;

                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                wc.DownloadFileAsync(new System.Uri(rssItem.enclosure.URL), dlP);
                wc.DownloadFileCompleted += Wc_DownloadProgressCompleted;
            }
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            // update progressbar
            pg.Value = e.ProgressPercentage;
        }

        private void Wc_DownloadProgressCompleted(object sender, AsyncCompletedEventArgs e) {
            //Switch back Download button from progrressbar
            pg.Visibility = Visibility.Collapsed;
            DLBtn.Visibility = Visibility.Visible;
        }
    }
}
