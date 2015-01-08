
using System.Xml.Linq;

namespace Saxx.LingoHubSyncer
{
    public class Phrase
    {
        public Phrase() { }

        public Phrase(string value, string comment)
        {
            Value = value;
            Comment = comment;
        }


        public Phrase(XElement element)
        {
            var valueElement = element.Element("value");
            if (valueElement != null)
                Value = valueElement.Value;

            var commentElement = element.Element("comment");
            if (commentElement != null)
                Comment = commentElement.Value;
        }

        public string Value { get; set; }
        public string Comment { get; set; }
    }
}
