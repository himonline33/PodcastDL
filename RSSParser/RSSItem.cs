using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSParser {
    class RSSItem {

        // RSS 2.0 Specification    https://www.rssboard.org/rss-specification
        // Itunes elments included

        // Supported Item children

        // [Required]

        // Channel/Item/Title               title           The title of the item.
        // Channel/Item/Link                link            The URL of the item.
        // Channel/Item/Description         description     The item synopsis.


        // [Optinal]

        // Channel/Item/Author              author          Email address of the author of the item.

        // Channel/Item/Category            category        Includes the item in one or more categories.
        // Channel/Item/Category@Domain     domain          [Optional] string that identifies a categorization taxonomy.

        // Channel/Item/Comments            comments        URL of a page for comments relating to the item.

        // Channel/Item/Enclosure           enclosure /     Describes a media object that is attached to the item.
        // Channel/Item/Enclosure@URL       url             [Required]
        // Channel/Item/Enclosure@Length    length          [Required] (ignored by Itunes)          
        // Channel/Item/Enclosure@Type      type            [Required]


        // Channel/Item/Guid                guid            A string that uniquely identifies the item. (globally unique identifier)
        // Channel/Item/Guid@IsPermaLink    ispermalink     permalink to the item

        // Channel/Item/Publication Date    pubDate         Indicates when the item was published.

        // Channel/Item/Source              source          The RSS channel that the item came from.
        // Channel/Item/Source@URL          url             Required





    }
}
