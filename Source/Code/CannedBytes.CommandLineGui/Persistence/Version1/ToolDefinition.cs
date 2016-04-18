using System.Xml;
using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Persistence.Version1
{
    [XmlType("toolDefinition", Namespace = XmlNamespaces.GuiFileDocumentV1)]
    public class ToolDefinition
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("location")]
        public string Location { get; set; }

        [XmlAttribute("guiSchemaRef")]
        public string GuiSchemaRef { get; set; }

        [XmlElement("guiSchema")]
        public XmlElement GuiSchema { get; set; }
    }
}