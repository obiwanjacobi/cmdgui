using System.Collections.Generic;
using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Persistence.Version1
{
    [XmlType("binding", Namespace = XmlNamespaces.GuiFileDocumentV1)]
    public class Argument
    {
        public Argument()
        {
            Values = new List<string>();
        }

        [XmlElement("value")]
        public List<string> Values { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}