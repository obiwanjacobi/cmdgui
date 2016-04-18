using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// A representation of a process exit code.
    /// </summary>
    [XmlType("exitCode", Namespace = XmlNamespaces.CommandLineSchema)]
    public class ExitCode
    {
        /// <summary>
        /// The actual value of the process exit code.
        /// </summary>
        [XmlAttribute("value")]
        public int Value { get; set; }

        /// <summary>
        /// A helpful description of the process exit code.
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }
    }
}