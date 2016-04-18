using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Persistence.Version1
{
    [XmlType(Namespace = XmlNamespaces.GuiFileDocumentV1)]
    [XmlRoot("document", Namespace = XmlNamespaces.GuiFileDocumentV1, IsNullable = false)]
    public class GuiFileDocument
    {
        [XmlElement("toolDefinition")]
        public ToolDefinition ToolDefinition { get; set; }

        [XmlArray("arguments")]
        [XmlArrayItem("argument")]
        public ArgumentList Arguments { get; set; }
    }
}