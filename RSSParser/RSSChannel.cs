using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace RSSParser {
    class RSSChannel {
        // RSS 2.0 Specification    https://www.rssboard.org/rss-specification
        // Itunes elments included

        // Supported Channel children


        // [Required]

        /// Channel/Title        title               
        /// Channel/Link         link
        /// Channel/Description  description


        // [Optinal]

        /// Channel/Image                image       parent
        /// Channel/Image/URL            url         Image URL
        /// Channel/Image/Title          title       Alternative for url
        /// Channel/Image/Link           link        link for source site
        /// Channel/Image/Width          width       Optional image width    def 88, max 144
        /// Channel/Image/height         height      Optional image height   def 31, max 400
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

        /// Channel/Language             language        language the channel is written in
        /// Channel/Copyright            copyright       Copyright notice for content in the channel.+
        /// Channel/Managing Editor      managingEditor  Email address for person responsible for editorial content.
        /// Channel/Web Master           webMaster       Email address for person responsible for technical issues relating to channel.

        /// Channel/Publication Date     pubDate         The publication date for the content in the channel.   
        /// Channel/Last Build Date      lastBuildDate   The last time the content of the channel changed.
        /// Channel/Catagory             catagory        categories that the channel belongs to.
        /// Channel/Generator            generator       A string indicating the program used to generate the channel.
        /// Channel/Docs                 docs            A URL that points to the documentation for the format used in the RSS file.
        // Channel/Rating               rating          The PICS rating for the channel.

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


        public string title { get; }
        public string link { get; }
        public string description { get; }
        public string language { get; }
        public string copyright { get; }
        public string managingEditor { get; }
        public string webMaster { get; }
        public string pubDate { get; }
        public string lastBuildDate { get; }
        public string catagory { get; }
        public string generator { get; }
        public string docs { get; }
        public string rating { get; }

        private struct Image {
            public string uri;
            public string title;
            public string link;
            public int width;
            public int height;
        }

        private XmlDocument xmlDoc;
        //private string rssUrl;

        //public RSSChannel() {
        //}

        public RSSChannel(string rssUrl) {

            if (ValidateUrl(rssUrl)) {


                xmlDoc = new XmlDocument();
                xmlDoc.Load(rssUrl);
                // Load the document and set the root element.  

                XmlNode documentRoot = xmlDoc.DocumentElement;

                title = SimpleElement(documentRoot,"channel//title");
                link = SimpleElement(documentRoot, "channel//link");
                description = SimpleElement(documentRoot, "channel//description");
                language = SimpleElement(documentRoot, "channel//language");
                copyright = SimpleElement(documentRoot, "channel//copyright");
                managingEditor = SimpleElement(documentRoot, "channel//managingEditor");
                webMaster = SimpleElement(documentRoot, "channel//webMaster");
                pubDate = SimpleElement(documentRoot, "channel//pubDate");
                lastBuildDate = SimpleElement(documentRoot, "channel//lastBuildDate");
                catagory = SimpleElement(documentRoot, "channel//catagory");
                generator = SimpleElement(documentRoot, "channel//generator");
                docs = SimpleElement(documentRoot, "channel//docs");
                rating = SimpleElement(documentRoot, "channel//rating");

                string s = string.Format("{0} {1} {2}", title, link, description);
                MessageBox.Show(s);


            }
        }

        private string SimpleElement(XmlNode node, string selector) {
            string text = "";
            try {
                text = node.SelectSingleNode(selector).InnerText;
            }
            catch (Exception) {

                //throw;
            }
            return text;
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
