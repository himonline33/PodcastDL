using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Microsoft.SqlServer.Server;
using SortSøndagDL;
using System.Windows.Controls;
using System.Security.RightsManagement;

namespace RSSParser {
    public class RSSChannel : RSS{
        // RSS 2.0 Specification    https://www.rssboard.org/rss-specification
        // Itunes elments included

        // Supported Channel children

        // Channel/Image/itunes:author  

        // Channel/Cloud                cloud /      lightweight publish-subscribe protocol
        // Channel/Cloud@Domain         domain 
        // Channel/Cloud@Port           port
        // Channel/Cloud@Path           path
        // Channel/Cloud@RegistrationProcedure  registerProcedure
        // Channel/Cloud@Protocol       protocol

        // Channel/TTL                  ttl         how long a channel can be cached before refreshing from the source

        // Channel/Text Input           textInput       input for form submission
        // Channel/Text Input/Title     title           label of the Submit button
        // Channel/Text Input/Name      name            Name of input field
        // Channel/Text Input/Link      link            Link to pass form

        // Channel/Skip Hours           skipHours

        // Channel/SkipDays             skipDays
        // Channel/SkipDays/Monday
        // Channel/SkipDays/Tuesday
        // Channel/SkipDays/Wednesday
        // Channel/SkipDays/Thursday
        // Channel/SkipDays/Friday
        // Channel/SkipDays/Saturday 
        // Channel/SkipDays/Sunday


        // Channel/itunes:owner         itunes:owner     
        // Channel/itunes:owner/Email   email
        // Channel/itunes:owner/Name    name

        // Channel/itunes:new-feed-url  itunes:new-feed-url

        // Channel/itunes:image         itunes:image /
        // Channel/itunes:image@href

        // Channel/itunes:explicit      itunes:explicit
        // Channel/itunes:category      itunes:category

        // Channel/itunes:category      itunes:category /
        // Channel/itunes:category@text text

        // [Required]
        public string Title { get; }                // Channel/Title                title    
        public string Link { get; }                 // Channel/Link                 link
        public string Description { get; }          // Channel/Description          description
        // [Optinal]
        public string Language { get; }             // Channel/Language             language        language the channel is written in
        public string Copyright { get; }            // Channel/Copyright            copyright       Copyright notice for content in the channel.+
        public string ManagingEditor { get; }       // Channel/Managing Editor      managingEditor  Email address for person responsible for editorial content.
        public string WebMaster { get; }            // Channel/Web Master           webMaster       Email address for person responsible for technical issues relating to channel.
        public string PubDate { get; }              // Channel/Publication Date     pubDate         The publication date for the content in the channel. 
        public string LastBuildDate { get; }        // Channel/Last Build Date      lastBuildDate   The last time the content of the channel changed.
        public string Catagory { get; }             // Channel/Catagory             catagory        categories that the channel belongs to.
        public string Generator { get; }            // Channel/Generator            generator       A string indicating the program used to generate the channel.
        public string Docs { get; }                 // Channel/Docs                 docs            A URL that points to the documentation for the format used in the RSS file.
        public string Rating { get; }               // Channel/Rating               rating          The PICS rating for the channel.

        private struct Image {                      // Channel/Image                image       parent
            public string uri;                      // Channel/Image/URL            url         Image URL
            public string title;                    // Channel/Image/Title          title       Alternative for url
            public string link;                     // Channel/Image/Link           link        link for source site
            public int width;                       // Channel/Image/Width          width       Optional image width    def 88, max 144
            public int height;                      // Channel/Image/height         height      Optional image height   def 31, max 400
        }

        private readonly XmlDocument xmlDoc;
        private List<RSSItem> RSSItemList;

        XmlNodeList XmlNodes;

        public RSSChannel(string rssUrl) {

            if (ValidateUrl(rssUrl)) {
                

                xmlDoc = new XmlDocument();
                xmlDoc.Load(rssUrl);
                // Load the document and set the root element.  

                this.xmlNode = xmlDoc.DocumentElement;

                Title = RSSSimpleElement("channel//title");
                Link = RSSSimpleElement("channel//link");
                Description = RSSSimpleElement("channel//description");
                Language = RSSSimpleElement("channel//language");
                Copyright = RSSSimpleElement("channel//copyright");
                ManagingEditor = RSSSimpleElement("channel//managingEditor");
                WebMaster = RSSSimpleElement("channel//webMaster");
                PubDate = RSSFormatDate("channel//pubDate", false);
                LastBuildDate = RSSFormatDate("channel//lastBuildDate", false);
                Catagory = RSSSimpleElement("channel//catagory");
                Generator = RSSSimpleElement("channel//generator");
                Docs = RSSSimpleElement("channel//docs");
                Rating = RSSSimpleElement("channel//rating");

                XmlNodes = this.xmlNode.SelectNodes("channel//item");
                RSSItemList = new List<RSSItem>();

                 
            }
        }

        public List<RSSItem> GetItemRange(int start, int count) {
            RSSItemList.Clear();
            for (int i = start; i < count; i++) {
                RSSItemList.Add(new RSSItem(XmlNodes[(int)i]));
            }
            return RSSItemList;
        }

        private bool ValidateUrl(string rssUrl) {
            bool valid = true;

            if (Uri.IsWellFormedUriString(rssUrl, UriKind.Absolute)) {
                Uri rssUri = new Uri(rssUrl);
                string path = String.Format("{0}{1}{2}{3}", rssUri.Scheme, Uri.SchemeDelimiter, rssUri.Authority, rssUri.AbsolutePath);
                string extension = Path.GetExtension(path);
                if (!(extension.ToLower() == ".xml")) {
                    MessageBox.Show("File extension is not valid");
                    valid = false;
                }
            }
            else {
                valid = false;
                MessageBox.Show("URL is not valid try adding HTTP:// or HTTPS:// in front of URL");
            }
            return valid;
        }
    }
}
