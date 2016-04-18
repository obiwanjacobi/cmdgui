using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// The root of the command line gui tool configuration.
    /// </summary>
    [XmlType(Namespace = XmlNamespaces.CommandLineSchema)]
    [XmlRoot("executables", Namespace = XmlNamespaces.CommandLineSchema, IsNullable = false)]
    public class CommandLineGuiConfig
    {
        /// <summary>
        /// An indication of the file version.
        /// </summary>
        [XmlAttribute("fileVersion")]
        public string FileVersion { get; set; }

        /// <summary>
        /// A list of executables defined in the file.
        /// </summary>
        [XmlElement("executable")]
        public ExecutableList Executables { get; set; }
    }
}