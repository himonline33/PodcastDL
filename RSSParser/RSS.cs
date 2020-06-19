using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RSSParser {
    public class RSS {
        internal XmlNode xmlNode;

        internal string RSSSimpleElement(string selector) {
            string text = "";
            try {
                text = xmlNode.SelectSingleNode(selector).InnerText;
            }
            catch (Exception) {

                //throw;
            }
            return text;
        }

        internal string RSSFormatDate(string selector, bool noSpaces = false) {
            string text = "";
            try {
                text = xmlNode.SelectSingleNode(selector).InnerText;
                string[] dateArr = text.Split(' ');
                string[] timeArr = dateArr[4].Split(':');
                string time;
                //text = string.Format("{0} {1} {2} {3} {4}", dateArr[0].Trim(','), dateArr[1], dateArr[2], dateArr[3], time);
                if (noSpaces == true) {
                    time = string.Format("{0}{1}", timeArr[0], timeArr[1]);
                    text = string.Format("{0}-{1}-{2}-{3}-{4}", dateArr[0].Trim(','), dateArr[1], dateArr[2], dateArr[3], time);
                }
                else {
                    time = string.Format("{0}:{1}", timeArr[0], timeArr[1]);
                    text = string.Format("{0} {1} {2} {3} {4}", dateArr[0].Trim(','), dateArr[1], dateArr[2], dateArr[3], time);
                }


            }
            catch (Exception) {

                //throw;
            }
            return text;
        }
    }
}
