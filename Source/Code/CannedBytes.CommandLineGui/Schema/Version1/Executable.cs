using System.Xml.Serialization;

namespace CannedBytes.CommandLineGui.Schema.Version1
{
    /// <summary>
    /// The root element od a tool definition.
    /// </summary>
    [XmlTypeAttribute(Namespace = XmlNamespaces.CommandLineSchema)]
    public class Executable : ConfigurationObject
    {
        /// <summary>
        /// A logical name of the tool.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// The location of the tool.
        /// </summary>
        [XmlAttribute("location")]
        public string Location { get; set; }

        /// <summary>
        /// A single command line the displays the help screen.
        /// </summary>
        [XmlAttribute("helpCmd")]
        public string HelpCmd { get; set; }

        /// <summary>
        /// An url to an online documentation source.
        /// </summary>
        [XmlAttribute("helpUrl")]
        public string HelpUrl { get; set; }

        /// <summary>
        /// An array with all the argument definitions.
        /// </summary>
        [XmlArray("arguments")]
        [XmlArrayItem("argument")]
        public ArgumentList Arguments { get; set; }

        /// <summary>
        /// The root BindingGroup for the tool.
        /// </summary>
        [XmlElement("gui")]
        public BindingGroup Gui { get; set; }

        /// <summary>
        /// A definition of all process exit codes and their meaning.
        /// </summary>
        [XmlArray("exitCodes")]
        [XmlArrayItem("exitCode")]
        public ExitCodeList ExitCodes { get; set; }
    }
}