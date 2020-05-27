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
}
