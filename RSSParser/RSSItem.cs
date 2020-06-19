
using System.Xml;

namespace RSSParser {
    public class RSSItem : RSS {

        // RSS 2.0 Specification    https://www.rssboard.org/rss-specification
        // Itunes elments included

        // Supported Item children

        // [Required]

        public string Title { get; }            // Channel/Item/Title               title           The title of the item.
        public string Link { get; }             // Channel/Item/Link                link            The URL of the item.
        public string Description { get; }      // Channel/Item/Description         description     The item synopsis.

        // [Optinal]

        public string Author { get; }           // Channel/Item/Author              author          Email address of the author of the item.
        public string Comments { get; }         // Channel/Item/Comments            comments        URL of a page for comments relating to the item.
        public string PubDate { get; }          // Channel/Item/Publication Date    pubDate         Indicates when the item was published.
        public Category category;
        public Enclosure enclosure;
        public GUID guid;
        public Source source;

        public struct Category {
            public string category { get; }     // Channel/Item/Category            category        Includes the item in one or more categories.
            public string Domain { get; }       // Channel/Item/Category@Domain     domain          [Optional] string that identifies a categorization taxonomy.            
            public Category(string category, string domain) { 
                this.category = category; 
                this.Domain = domain;
            }
        }

        public struct Enclosure {               // Channel/Item/Enclosure           enclosure /     Describes a media object that is attached to the item.
            public string URL { get; }          // Channel/Item/Enclosure@URL       url             [Required]
            public string Length { get; }       // Channel/Item/Enclosure@Length    length          [Required] (ignored by Itunes)          
            public string Type { get; }         // Channel/Item/Enclosure@Type      type            [Required]
            public Enclosure(string url, string length, string type) {
                this.URL = url;
                this.Length = length;
                this.Type = type;
            }
        }

        public struct GUID {
            public string guid { get; }         // Channel/Item/Guid                guid            A string that uniquely identifies the item. (globally unique identifier)
            public string IsPermaLink { get; }  // Channel/Item/Guid@IsPermaLink    ispermalink     permalink to the item
            public GUID(string guid, string isPermalink) {
                this.guid = guid;
                this.IsPermaLink = isPermalink;
            }
        }

        public struct Source {
            public string source { get; }       // Channel/Item/Source              source          The RSS channel that the item came from.
            public string Url { get; }          // Channel/Item/Source@URL          url             Required
            public Source(string source, string url) {
                this.source = source;
                this.Url = url;
            }
        }

        public string PubDateNoSpace;

        public RSSItem(XmlNode xmlNode) {
            this.xmlNode = xmlNode;
            Title = RSSSimpleElement("title");
            Link = RSSSimpleElement("link");
            Description= RSSSimpleElement("description");
            Author = RSSSimpleElement("author");
            Comments = RSSSimpleElement("comments");
            PubDate = RSSFormatDate("pubDate");
            PubDateNoSpace = RSSFormatDate("pubDate", true);
            Category category = new Category(
                RSSSimpleElement("category"),
                RSSSimpleElement("category/@domain")
            );
            enclosure = new Enclosure(
                RSSSimpleElement("enclosure/@url"),
                RSSSimpleElement("enclosure/@length"),
                RSSSimpleElement("enclosure/@type")
            );
            guid = new GUID(
                RSSSimpleElement("guid"),
                RSSSimpleElement("guid/@isPermaLink")
            );
            source = new Source(
                RSSSimpleElement("source"),
                RSSSimpleElement("source/@url")
            );
             
        }
    }
}
